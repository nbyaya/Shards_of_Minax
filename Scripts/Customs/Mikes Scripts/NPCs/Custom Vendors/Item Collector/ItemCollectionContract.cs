using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
    public class ItemCollectionContract : Item
    {
        private Type m_ItemType;
        private int m_AmountNeeded;
        private int m_AmountCollected;
        private SkillName m_PowerScrollSkill;
        private double m_PowerScrollValue;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType
        {
            get { return m_ItemType; }
            set { m_ItemType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded
        {
            get { return m_AmountNeeded; }
            set { m_AmountNeeded = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected
        {
            get { return m_AmountCollected; }
            set { m_AmountCollected = value; }
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
        public ItemCollectionContract() : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            int itemIndex = ItemCollectionType.Random();
            ItemType = ItemCollectionType.Items[itemIndex].Type;
            AmountNeeded = Utility.Random(10) + 5;
            Name = $"Collection Contract: {AmountNeeded} {ItemCollectionType.Items[itemIndex].Name}s";
            AmountCollected = 0;

            // Set random skill for Powerscroll
            Array skills = Enum.GetValues(typeof(SkillName));
            PowerScrollSkill = (SkillName)skills.GetValue(Utility.Random(skills.Length));

            // Set random value for Powerscroll between 51 and 90
            PowerScrollValue = Utility.RandomMinMax(51, 90);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new ItemCollectionContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This contract must be in your backpack to use it.
            }
        }

        public ItemCollectionContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_ItemType.ToString());
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
            writer.Write((int)m_PowerScrollSkill);
            writer.Write(m_PowerScrollValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ItemType = ScriptCompiler.FindTypeByFullName(reader.ReadString());
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();

            if (version >= 1)
            {
                m_PowerScrollSkill = (SkillName)reader.ReadInt();
                m_PowerScrollValue = reader.ReadDouble();
            }
        }
    }
}
