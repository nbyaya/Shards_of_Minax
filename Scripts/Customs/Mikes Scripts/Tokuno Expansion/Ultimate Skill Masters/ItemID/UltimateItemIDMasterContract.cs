using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class UltimateItemIDMasterContract : Item
    {
        private Type m_ItemType;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_ItemType; } set { m_ItemType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateItemIDMasterContract() : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Randomize the required item
            int itemIndex = ItemIDCollectionType.Random();
            ItemType = ItemIDCollectionType.Items[itemIndex].Type;
            AmountNeeded = Utility.Random(10) + 10; // a bit more demanding
            Name = $"ItemID Collection Contract: {AmountNeeded} {ItemIDCollectionType.Items[itemIndex].Name}(s)";
            AmountCollected = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateItemIDMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateItemIDMasterContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_ItemType.ToString());
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ItemType = ScriptCompiler.FindTypeByFullName(reader.ReadString());
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();
        }
    }
}
