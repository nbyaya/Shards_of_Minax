using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class FishermansQuiver : BaseQuiver
    {
        [Constructable]
        public FishermansQuiver()
            : base()
        {
            Name = "Fisherman's Quiver";
            Hue = 1152; // Custom Hue for fishing theme (light blue/greenish)
            this.WeightReduction = 30; // A moderate weight reduction for long fishing trips
            Attributes.BonusInt = 4; // Bonus Intelligence for better fishing skills (somewhat intellectual activity)
            Attributes.RegenStam = 3; // Regenerates stamina to help with long hours of fishing
            SkillBonuses.SetValues(0, SkillName.Fishing, 20.0); // Boost to Fishing skill
            SkillBonuses.SetValues(1, SkillName.Cooking, 10.0); // Bonus to Cooking (for when catching and preparing fish)
            Resistances.Physical = 5; // Minor Physical resistance for durability during fishing
            Resistances.Cold = 10; // Minor Cold resistance for cold weather fishing
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the item for extra customization

            // This item will be blessed, preventing it from being lost or stolen.
            this.LootType = LootType.Blessed;
        }

        public FishermansQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Fisherman's Quiver
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
