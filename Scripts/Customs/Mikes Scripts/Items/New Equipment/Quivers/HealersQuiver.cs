using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class HealersQuiver : BaseQuiver
    {
        [Constructable]
        public HealersQuiver()
            : base()
        {
            Name = "Healer's Quiver";
            Hue = Utility.Random(2200, 2250); // Random hue for variation, something soft like green or blue
            this.WeightReduction = 30; // Weight reduction for practical use during healing
            Attributes.BonusInt = 5; // Bonus to Intelligence for faster healing
            Attributes.BonusHits = 10; // Increases the player's health
            SkillBonuses.SetValues(0, SkillName.Healing, 20.0); // Boosts Healing skill by 20 points
            SkillBonuses.SetValues(1, SkillName.Parry, 10.0); // Increases Parry for some defensive support
            Resistances.Physical = 8; // Provides some physical resistance for the healer's defense
            Resistances.Cold = 5; // Minor resistance to cold (commonly used in healing-related spells)
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the item for further shard-level customization

            // Optionally, a lore tag to describe the item and roleplay with it
            this.LootType = LootType.Blessed; // Keeps the item safe from being looted
        }

        public HealersQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Healer's Quiver (you can adjust this label number)
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
