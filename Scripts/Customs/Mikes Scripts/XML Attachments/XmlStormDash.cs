using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStormDash : XmlAttachment
    {
        private int m_MinDamage = 20; // Minimum damage
        private int m_MaxDamage = 30; // Maximum damage
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(1); // Duration of the freeze effect
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(20); // Time between activations
        private DateTime m_NextDashTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlStormDash(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStormDash() { }

        [Attachable]
        public XmlStormDash(int minDamage, int maxDamage, double freezeDuration, double refractory)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            FreezeDuration = TimeSpan.FromSeconds(freezeDuration);
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_FreezeDuration);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextDashTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_FreezeDuration = reader.ReadTimeSpan();
                m_Refractory = reader.ReadTimeSpan();
                m_NextDashTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Storm Dash: Deals {0} to {1} damage and stuns for {2} seconds every {3} seconds.", m_MinDamage, m_MaxDamage, m_FreezeDuration.TotalSeconds, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextDashTime)
            {
                StormDash(attacker, defender);
                m_NextDashTime = DateTime.UtcNow + m_Refractory;
            }
        }

        private void StormDash(Mobile attacker, Mobile target)
        {
            if (target != null && target.Alive)
            {
                attacker.MoveToWorld(target.Location, target.Map);
                Effects.PlaySound(attacker.Location, attacker.Map, 0x1F2); // Dash sound
                target.Damage(Utility.RandomMinMax(m_MinDamage, m_MaxDamage), attacker);
                target.Freeze(m_FreezeDuration); // Stun effect

                attacker.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The creature dashes through you with immense force! *");
            }
        }
    }
}
