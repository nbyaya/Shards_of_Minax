using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateTailoringMasterContract : Item
    {
        private TailoringCollectionType m_TailoringItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_TailoringItem?.ItemType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateTailoringMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Determine the player's Tailoring skill
            double skillLevel = player?.Skills[SkillName.Tailoring]?.Value ?? 0.0;

            TailoringCollectionType.PopulateTailoringCollection();

            // Filter items based on skill level
            var eligibleItems = TailoringCollectionType.Items
                .Where(item => skillLevel >= item.MinSkill && skillLevel <= item.MaxSkill)
                .ToList();

            if (eligibleItems.Count > 0)
            {
                // Randomly select an item
                m_TailoringItem = eligibleItems[Utility.Random(eligibleItems.Count)];

                // Set amount needed and name
                AmountNeeded = Utility.RandomMinMax(10, 20);
                Name = $"Tailoring Collection Contract: {AmountNeeded} {m_TailoringItem.ItemType}(s)";
                AmountCollected = 0;
            }
            else
            {
                // Default fallback if no items match the skill level
                m_TailoringItem = null;
                Name = "Invalid Tailoring Contract";
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateTailoringMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateTailoringMasterContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            string itemName = reader.ReadString();
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();
        }
    }
}
