using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    [Flipable(0x14EF, 0x14F0)]
    public class TamingContract : Item
    {
        private int m_CreatureType;
        private int m_GoldReward;
        private int m_AmountToTame;
        private int m_AmountTamed;
        private SkillName m_PowerScrollSkill;
        private double m_PowerScrollValue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int CreatureType
        {
            get { return m_CreatureType; }
            set { m_CreatureType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GoldReward
        {
            get { return m_GoldReward; }
            set { m_GoldReward = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountToTame
        {
            get { return m_AmountToTame; }
            set { m_AmountToTame = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountTamed
        {
            get { return m_AmountTamed; }
            set { m_AmountTamed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName PowerScrollSkill
        {
            get { return m_PowerScrollSkill; }
            set { m_PowerScrollSkill = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double PowerScrollValue
        {
            get { return m_PowerScrollValue; }
            set { m_PowerScrollValue = value; }
        }

        [Constructable]
        public TamingContract() : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            CreatureType = Utility.Random(AnimalContractType.Get.Length);
            AmountToTame = Utility.Random(10) + 5;
            GoldReward = (AmountToTame * 100);
            Name = "Taming Contract: " + AmountToTame + " " + AnimalContractType.Get[CreatureType].Name;
            AmountTamed = 0;

            // Set random skill for powerscroll
            Array skills = Enum.GetValues(typeof(SkillName));
            PowerScrollSkill = (SkillName)skills.GetValue(Utility.Random(skills.Length));

            // Set random value for powerscroll between 51 and 90
            PowerScrollValue = Utility.RandomMinMax(51, 90);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new TamingContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This contract must be in your backpack to use it.
            }
        }

        public TamingContract(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write(m_CreatureType);
            writer.Write(m_GoldReward);
            writer.Write(m_AmountToTame);
            writer.Write(m_AmountTamed);
            writer.Write((int)m_PowerScrollSkill);
            writer.Write(m_PowerScrollValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_CreatureType = reader.ReadInt();
            m_GoldReward = reader.ReadInt();
            m_AmountToTame = reader.ReadInt();
            m_AmountTamed = reader.ReadInt();

            if (version >= 1)
            {
                m_PowerScrollSkill = (SkillName)reader.ReadInt();
                m_PowerScrollValue = reader.ReadDouble();
            }

            LootType = LootType.Blessed;
        }
    }
}