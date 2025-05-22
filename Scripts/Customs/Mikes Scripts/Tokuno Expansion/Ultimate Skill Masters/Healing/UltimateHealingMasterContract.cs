using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateHealingMasterContract : Item
    {
        private HealingCollectionType m_HealingItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_HealingItem?.ItemType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string ItemName { get { return m_HealingItem?.Name; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateHealingMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Determine the player's Healing skill
            double skillLevel = player?.Skills[SkillName.Alchemy]?.Value ?? 0.0;

            HealingCollectionType.PopulateHealingCollection();

            // Filter items based on skill level
            var eligibleItems = HealingCollectionType.Items
                .Where(item => skillLevel >= item.MinDifficulty && skillLevel <= item.MaxDifficulty)
                .ToList();
			if (eligibleItems.Count == 0)
			{
				// Default to the most difficult items
				double maxDiff = HealingCollectionType.Items.Max(i => i.MaxDifficulty);
				eligibleItems = HealingCollectionType.Items.Where(i => i.MaxDifficulty == maxDiff).ToList();
			}

			// At this point, we are guaranteed a valid list
			m_HealingItem = eligibleItems[Utility.Random(eligibleItems.Count)];
			AmountNeeded = Utility.RandomMinMax(10, 20);
			Name = $"Healing Collection Contract: {AmountNeeded} {m_HealingItem.Name}(s)";
			AmountCollected = 0;

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateHealingMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateHealingMasterContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_HealingItem?.Name ?? string.Empty);
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            string itemName = reader.ReadString();
            m_HealingItem = HealingCollectionType.Items.FirstOrDefault(item => item.Name == itemName);
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();
        }
    }
}
