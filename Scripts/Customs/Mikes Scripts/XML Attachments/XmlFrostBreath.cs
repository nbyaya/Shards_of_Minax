using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFrostBreath : XmlAttachment
    {
        private int m_MinDamage = 10; // Minimum damage
        private int m_MaxDamage = 15; // Maximum damage
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(2); // Duration of freeze effect
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(20); // Time between activations
        private DateTime m_NextFrostBreath;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlFrostBreath(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFrostBreath() { }

        [Attachable]
        public XmlFrostBreath(int minDamage, int maxDamage, double freezeDuration, double refractory)
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
            writer.WriteDeltaTime(m_NextFrostBreath);
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
                m_NextFrostBreath = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextFrostBreath)
            {
                PerformFrostBreath(attacker, defender);
                m_NextFrostBreath = DateTime.UtcNow + Refractory;
            }
        }

        public void PerformFrostBreath(Mobile attacker, Mobile target)
        {
            if (target == null || !target.Alive)
                return;

            // Apply cold damage and slow down
            int damage = Utility.RandomMinMax(MinDamage, MaxDamage);
            target.Damage(damage, attacker);
            target.SendMessage("You are chilled by the breath of an unknown creature!");
            target.Freeze(FreezeDuration);
        }
    }
}
