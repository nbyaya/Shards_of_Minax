using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlWhirlwindFury : XmlAttachment
    {
        private int m_MinDamage = 15; // Minimum damage
        private int m_MaxDamage = 25; // Maximum damage
        private int m_Range = 5; // Range of the attack
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_NextWhirlwindFury;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlWhirlwindFury(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlWhirlwindFury() { }

        [Attachable]
        public XmlWhirlwindFury(int minDamage, int maxDamage, int range, double refractory)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Range = range;
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_Range);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextWhirlwindFury);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_MinDamage = reader.ReadInt();
                m_MaxDamage = reader.ReadInt();
                m_Range = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_NextWhirlwindFury = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Whirlwind Fury: Deals {0}-{1} damage to players within {2} tiles every {3} seconds.", 
                m_MinDamage, m_MaxDamage, m_Range, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextWhirlwindFury)
            {
                PerformWhirlwindFury(attacker);
                m_NextWhirlwindFury = DateTime.UtcNow + m_Refractory;
            }
        }

        private void PerformWhirlwindFury(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(m_Range))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("You are caught in the whirlwind fury!");
                    m.Damage(Utility.RandomMinMax(m_MinDamage, m_MaxDamage), owner);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Confusion effect
                }
            }

            Effects.PlaySound(owner.Location, owner.Map, 0x307); // Whirlwind sound
            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A whirlwind surrounds you, causing havoc! *");
        }
    }
}
