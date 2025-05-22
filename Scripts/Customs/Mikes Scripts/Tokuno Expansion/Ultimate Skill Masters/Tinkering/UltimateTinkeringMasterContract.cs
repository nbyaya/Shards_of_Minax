using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateTinkeringMasterContract : Item
    {
        private TinkeringCollectionType m_TinkeringItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_TinkeringItem?.ItemType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateTinkeringMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Determine the player's Tinkering skill
            double skillLevel = player?.Skills[SkillName.Tinkering]?.Value ?? 0.0;

            TinkeringCollectionType.PopulateTinkeringCollection();

            // Filter items based on skill level
            var eligibleItems = TinkeringCollectionType.Items
                .Where(item => skillLevel >= item.MinSkill && skillLevel <= item.MaxSkill)
                .ToList();

			if (eligibleItems.Count == 0)
			{
				// Fall back to highest-difficulty items if none matched
				double maxSkill = TinkeringCollectionType.Items.Max(i => i.MaxSkill);
				eligibleItems = TinkeringCollectionType.Items
					.Where(i => i.MaxSkill == maxSkill)
					.ToList();
			}

			// Now always valid
			m_TinkeringItem = eligibleItems[Utility.Random(eligibleItems.Count)];
			AmountNeeded = Utility.RandomMinMax(10, 20);
			Name = $"Tinkering Collection Contract: {AmountNeeded} {m_TinkeringItem.ItemType.Name}(s)";
			AmountCollected = 0;

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateTinkeringMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateTinkeringMasterContract(Serial serial) : base(serial)
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
