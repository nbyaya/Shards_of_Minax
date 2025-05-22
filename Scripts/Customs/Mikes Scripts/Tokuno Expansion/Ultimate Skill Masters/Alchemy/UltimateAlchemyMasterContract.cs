using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateAlchemyMasterContract : Item
    {
        private AlchemyCollectionType m_AlchemyItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_AlchemyItem?.ItemType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string ItemName { get { return m_AlchemyItem?.Name; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateAlchemyMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            Hue = 0x4B5;
            LootType = LootType.Blessed;

            // Determine the player's alchemy skill
            double skillLevel = player?.Skills[SkillName.Alchemy]?.Value ?? 0.0;

            AlchemyCollectionType.PopulateAlchemyCollection();

            // Filter items based on skill level
            var eligibleItems = AlchemyCollectionType.Items
                .Where(item => skillLevel >= item.MinDifficulty && skillLevel <= item.MaxDifficulty)
                .ToList();

			if (eligibleItems.Count == 0)
			{
				// Fallback to hardest recipes if nothing matches
				double maxDiff = AlchemyCollectionType.Items.Max(i => i.MaxDifficulty);
				eligibleItems = AlchemyCollectionType.Items
					.Where(i => i.MaxDifficulty == maxDiff)
					.ToList();
			}

			// Safe: guaranteed to have items now
			m_AlchemyItem = eligibleItems[Utility.Random(eligibleItems.Count)];
			AmountNeeded = Utility.RandomMinMax(10, 20);
			Name = $"Alchemy Collection Contract: {AmountNeeded} {m_AlchemyItem.Name}(s)";
			AmountCollected = 0;

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateAlchemyMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateAlchemyMasterContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_AlchemyItem?.Name ?? string.Empty);
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            string itemName = reader.ReadString();
            m_AlchemyItem = AlchemyCollectionType.Items.FirstOrDefault(item => item.Name == itemName);
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();
        }
    }
}
