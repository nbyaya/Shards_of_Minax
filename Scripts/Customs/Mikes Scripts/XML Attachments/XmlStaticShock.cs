using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStaticShock : XmlAttachment
    {
        private int m_MinDamage = 5; // Minimum damage
        private int m_MaxDamage = 15; // Maximum damage
        private TimeSpan m_FreezeDuration = TimeSpan.FromSeconds(2); // Freeze duration
        private TimeSpan m_StunDuration = TimeSpan.FromSeconds(1); // Stun duration
        private double m_StunChance = 0.5; // Chance to stun

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinDamage { get { return m_MinDamage; } set { m_MinDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDamage { get { return m_MaxDamage; } set { m_MaxDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan FreezeDuration { get { return m_FreezeDuration; } set { m_FreezeDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan StunDuration { get { return m_StunDuration; } set { m_StunDuration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public double StunChance { get { return m_StunChance; } set { m_StunChance = value; } }

        public XmlStaticShock(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStaticShock() { }

        [Attachable]
        public XmlStaticShock(int minDamage, int maxDamage, double stunChance, TimeSpan freezeDuration, TimeSpan stunDuration)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            StunChance = stunChance;
            FreezeDuration = freezeDuration;
            StunDuration = stunDuration;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_MinDamage);
            writer.Write(m_MaxDamage);
            writer.Write(m_FreezeDuration);
            writer.Write(m_StunDuration);
            writer.Write(m_StunChance);
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
                m_StunDuration = reader.ReadTimeSpan();
                m_StunChance = reader.ReadDouble();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (attacker == null || defender == null || !defender.Alive)
                return;

            attacker.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Discharges a powerful burst of static electricity! *");
            Effects.SendLocationEffect(defender.Location, defender.Map, 0x376A, 10, 16); // Static effect

            int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
            defender.Damage(damage, attacker);

            defender.Freeze(m_FreezeDuration);

            if (Utility.RandomDouble() < m_StunChance)
            {
                defender.SendMessage("You are stunned by the electric shock!");
                defender.Freeze(m_StunDuration);
            }
        }
    }
}
