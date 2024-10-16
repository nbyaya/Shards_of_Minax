using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlBreezeHeal : XmlAttachment
    {
        private int m_HealAmountMin = 10; // Minimum heal amount
        private int m_HealAmountMax = 20; // Maximum heal amount
        private TimeSpan m_HealInterval = TimeSpan.FromSeconds(120); // Default heal interval
        private DateTime m_NextHealTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmountMin { get { return m_HealAmountMin; } set { m_HealAmountMin = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HealAmountMax { get { return m_HealAmountMax; } set { m_HealAmountMax = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan HealInterval { get { return m_HealInterval; } set { m_HealInterval = value; } }

        public XmlBreezeHeal(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlBreezeHeal() { }

        [Attachable]
        public XmlBreezeHeal(int healMin, int healMax, double interval)
        {
            HealAmountMin = healMin;
            HealAmountMax = healMax;
            HealInterval = TimeSpan.FromSeconds(interval);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealAmountMin);
            writer.Write(m_HealAmountMax);
            writer.Write(m_HealInterval);
            writer.WriteDeltaTime(m_NextHealTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_HealAmountMin = reader.ReadInt();
                m_HealAmountMax = reader.ReadInt();
                m_HealInterval = reader.ReadTimeSpan();
                m_NextHealTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Healing Breeze: Heals between {0} and {1} HP every {2} seconds.", m_HealAmountMin, m_HealAmountMax, m_HealInterval.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextHealTime && attacker.Alive)
            {
                CastBreezeHeal(attacker);
                m_NextHealTime = DateTime.UtcNow + m_HealInterval;
            }
        }

        private void CastBreezeHeal(Mobile owner)
        {
            if (owner == null || !owner.Alive)
                return;

            int healAmount = Utility.RandomMinMax(m_HealAmountMin, m_HealAmountMax);

            // Heal itself
            owner.Hits += healAmount;
            if (owner.Hits > owner.HitsMax)
                owner.Hits = owner.HitsMax;

            // Heal allies
            foreach (Mobile m in owner.GetMobilesInRange(10))
            {
                if (m is BaseCreature && ((BaseCreature)m).Team == ((BaseCreature)owner).Team && m.Alive)
                {
                    ((BaseCreature)m).Hits += healAmount;
                    if (((BaseCreature)m).Hits > ((BaseCreature)m).HitsMax)
                        ((BaseCreature)m).Hits = ((BaseCreature)m).HitsMax;

                    m.SendMessage("You feel a healing breeze from your ally.");
                }
            }
        }
    }
}
