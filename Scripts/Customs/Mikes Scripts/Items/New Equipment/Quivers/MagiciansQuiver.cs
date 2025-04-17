using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class MagiciansQuiver : BaseQuiver
    {
        [Constructable]
        public MagiciansQuiver()
            : base()
        {
            Name = "Magician's Quiver";
            Hue = Utility.Random(1300, 1500); // Unique color to match a magical theme
            this.WeightReduction = 25; // Slight weight reduction to not impede casting
            Attributes.BonusInt = 8; // Increased Intelligence for Magery skill enhancement
            Attributes.RegenMana = 3; // Boosts mana regeneration to help with spellcasting
            SkillBonuses.SetValues(0, SkillName.Magery, 20.0); // Direct bonus to Magery skill
            SkillBonuses.SetValues(1, SkillName.Meditation, 10.0); // Bonus to Meditation for faster mana regeneration
            Resistances.Physical = 5; // Provides some basic physical resistance
            Resistances.Energy = 15; // Greater resistance to Energy damage, useful against some spellcasters
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach for shard-level customization

            // Optionally, add a lore tag to the quiver for roleplay purposes
            this.LootType = LootType.Blessed; // Keeps the item from being looted by others
        }

        public MagiciansQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Magician's Quiver (adjust in language file if needed)
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
