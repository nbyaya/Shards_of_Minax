using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateInscribeMasterContract : Item
    {
        private InscribeCollectionType m_InscribeItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_InscribeItem?.ScrollType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string ItemName { get { return m_InscribeItem?.Name; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateInscribeMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Determine the player's Inscribe skill
            double skillLevel = player?.Skills[SkillName.Inscribe]?.Value ?? 0.0;

            InscribeCollectionType.PopulateInscribeCollection();

            // Filter items based on skill level
            var eligibleItems = InscribeCollectionType.Items
                .Where(item => skillLevel >= item.MinSkill && skillLevel <= item.MaxSkill)
                .ToList();

			if (eligibleItems.Count == 0)
			{
				// Fallback: Give them the easiest item (or random item)
				var fallbackItem = InscribeCollectionType.Items
					.OrderBy(i => i.MinSkill)
					.FirstOrDefault();

				if (fallbackItem == null)
					throw new InvalidOperationException("No Inscribe items defined in InscribeCollectionType.");

				eligibleItems.Add(fallbackItem);
			}

			// Now select from eligible (or fallback) list
			m_InscribeItem = eligibleItems[Utility.Random(eligibleItems.Count)];
			AmountNeeded = Utility.RandomMinMax(10, 20);
			Name = $"Inscribe Collection Contract: {AmountNeeded} {m_InscribeItem.Name}(s)";
			AmountCollected = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateInscribeMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateInscribeMasterContract(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_InscribeItem?.Name ?? string.Empty);
            writer.Write(m_AmountNeeded);
            writer.Write(m_AmountCollected);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            string itemName = reader.ReadString();
            m_InscribeItem = InscribeCollectionType.Items.FirstOrDefault(item => item.Name == itemName);
            m_AmountNeeded = reader.ReadInt();
            m_AmountCollected = reader.ReadInt();
        }
    }
}
