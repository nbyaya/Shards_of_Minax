using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSolarFlare : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for Solar Flare
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown duration
        private DateTime m_NextSolarFlare;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlSolarFlare(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSolarFlare() { }

        [Attachable]
        public XmlSolarFlare(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextSolarFlare);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextSolarFlare = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextSolarFlare)
            {
                PerformSolarFlare(attacker);
                m_NextSolarFlare = DateTime.UtcNow + m_Cooldown;
            }
        }

        public void PerformSolarFlare(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Solar Flare! *");
            Effects.SendLocationEffect(owner.Location, owner.Map, 0x37B9, 10, 16); // Light ray effect

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Alive)
                {
                    m.SendMessage("You are blinded by the burst of sunlight!");
                    Effects.SendLocationEffect(m.Location, m.Map, 0x376A, 10, 16); // Blinding effect
                    m.Damage(m_Damage, owner); // Damage
                    if (Utility.RandomDouble() < 0.25) // 25% chance to stun
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage("The sunlight stuns you!");
                    }
                }
            }
        }
    }
}
