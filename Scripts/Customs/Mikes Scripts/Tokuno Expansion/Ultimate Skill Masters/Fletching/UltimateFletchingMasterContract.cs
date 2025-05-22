using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateFletchingMasterContract : Item
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
        public UltimateFletchingMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Extract the player's Fletchinging skill level
            double playerSkillLevel = player.Skills[SkillName.Fletching].Value;

            FletchingCollectionType.PopulateFletchingCollection();

            // Filter items based on the player's skill level
            var availableItems = FletchingCollectionType.Items
                .Where(item => playerSkillLevel >= item.MinDifficulty && playerSkillLevel <= item.MaxDifficulty)
                .ToList();

			// If nothing matches (e.g. skill > MaxDifficulty), give them any challenge
			if ( availableItems.Count == 0 )
				availableItems = FletchingCollectionType.Items;

            // Select a random item from the filtered list
            FletchingCollectionType selectedItem = availableItems[Utility.Random(availableItems.Count)];

            // Set the contract details
            ItemType = selectedItem.ItemType; // Assign the item type
            AmountNeeded = Utility.RandomMinMax(10, 20);
            Name = $"Fletching Collection Contract: {AmountNeeded} {selectedItem.Name}(s)"; // Set the name
            AmountCollected = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateFletchingMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateFletchingMasterContract(Serial serial) : base(serial)
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
