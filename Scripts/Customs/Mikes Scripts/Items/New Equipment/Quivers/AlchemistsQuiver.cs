using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class AlchemistsQuiver : BaseQuiver
    {
        [Constructable]
        public AlchemistsQuiver()
            : base()
        {
            Name = "Alchemist's Quiver";
            Hue = 1167; // Alchemist-themed color (green)
            this.WeightReduction = 20; // Light weight reduction for practical use
            Attributes.BonusInt = 7; // Bonus Intelligence to enhance alchemical prowess
            Attributes.BonusDex = 3; // Dexterity to help in quick mixing of potions
            SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0); // Boost Alchemy skill
            SkillBonuses.SetValues(1, SkillName.TasteID, 10.0); // Boost Crafting skill for item making
            Resistances.Physical = 8; // Physical resistance
            Resistances.Energy = 12; // Increased resistance to energy for magical protection
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach for further shard customization
            LootType = LootType.Blessed; // Keeps the item from being looted by others

            // Lore - The quiver is used by the master alchemists who specialize in potion-making and explosive mixtures.
        }

        public AlchemistsQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Alchemist's Quiver (you can adjust this number in the language file)
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
