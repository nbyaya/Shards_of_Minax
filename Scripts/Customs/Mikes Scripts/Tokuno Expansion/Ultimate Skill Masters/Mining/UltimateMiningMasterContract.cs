using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using System.Linq;

namespace Server.Items
{
    public class UltimateMiningMasterContract : Item
    {
        private MiningCollectionType m_MiningItem;
        private int m_AmountNeeded;
        private int m_AmountCollected;

        [CommandProperty(AccessLevel.GameMaster)]
        public Type ItemType { get { return m_MiningItem?.ResourceType; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountNeeded { get { return m_AmountNeeded; } set { m_AmountNeeded = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountCollected { get { return m_AmountCollected; } set { m_AmountCollected = value; } }

        [Constructable]
        public UltimateMiningMasterContract(Mobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Determine the player's Mining skill
            double skillLevel = player?.Skills[SkillName.Mining]?.Value ?? 0.0;

			MiningCollectionType.PopulateMiningCollection();

			var eligibleItems = MiningCollectionType.Resources
				.Where(item => skillLevel >= item.MinSkill && skillLevel <= item.MaxSkill)
				.ToList();

			if (eligibleItems.Count == 0)
			{
				double highestSkill = MiningCollectionType.Resources.Max(i => i.MaxSkill);
				eligibleItems = MiningCollectionType.Resources
					.Where(i => i.MaxSkill == highestSkill)
					.ToList();
			}

			m_MiningItem = eligibleItems[Utility.Random(eligibleItems.Count)];
			AmountNeeded = Utility.RandomMinMax(10, 20);
			AmountCollected = 0;
			Name = $"Mining Collection Contract: {AmountNeeded} {m_MiningItem.ResourceType.Name}(s)";

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendGump(new UltimateMiningMasterContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This must be in your backpack to use it.
            }
        }

        public UltimateMiningMasterContract(Serial serial) : base(serial)
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
