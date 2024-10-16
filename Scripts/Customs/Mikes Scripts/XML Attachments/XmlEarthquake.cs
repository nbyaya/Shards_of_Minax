using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlEarthquake : XmlAttachment
    {
        private int m_Damage = 30; // Damage dealt
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(5); // Duration of freeze
        private TimeSpan m_MinCooldown = TimeSpan.FromSeconds(30); // Minimum cooldown time
        private TimeSpan m_MaxCooldown = TimeSpan.FromSeconds(60); // Maximum cooldown time
        private DateTime m_NextEarthquake;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MinCooldown { get { return m_MinCooldown; } set { m_MinCooldown = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MaxCooldown { get { return m_MaxCooldown; } set { m_MaxCooldown = value; } }

        public XmlEarthquake(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlEarthquake() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_FreezeDuration);
            writer.Write(m_MinCooldown);
            writer.Write(m_MaxCooldown);
            writer.WriteDeltaTime(m_NextEarthquake);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_FreezeDuration = reader.ReadTimeSpan();
                m_MinCooldown = reader.ReadTimeSpan();
                m_MaxCooldown = reader.ReadTimeSpan();
                m_NextEarthquake = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Earthquake Attack: Deals {0} damage and freezes targets for {1} seconds.", m_Damage, m_FreezeDuration.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextEarthquake)
            {
                PerformEarthquake(attacker);
            }
        }

        private void PerformEarthquake(Mobile owner)
        {
            foreach (Mobile m in owner.GetMobilesInRange(6))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("An earthquake shakes the ground violently!");
                    m.Damage(m_Damage, owner); // Significant damage
                    m.Freeze(m_FreezeDuration); // Lose footing
                }
            }

            Effects.PlaySound(owner.Location, owner.Map, 0x54C); // Earthquake sound effect

            Random rand = new Random();
            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next((int)m_MinCooldown.TotalSeconds, (int)m_MaxCooldown.TotalSeconds)); // Random cooldown
        }
    }
}
