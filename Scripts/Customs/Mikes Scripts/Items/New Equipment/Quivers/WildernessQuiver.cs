using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class WildernessQuiver : BaseQuiver
    {
        [Constructable]
        public WildernessQuiver()
            : base()
        {
            Name = "Wilderness Quiver";
            Hue = Utility.Random(2250, 2300); // Earthy tones, suitable for the wilderness theme
            this.WeightReduction = 35; // Moderate weight reduction for ease of carrying camping gear
            Attributes.BonusDex = 4; // Dexterity helps with agility and movement, key for camping and setting up camp quickly
            Attributes.BonusInt = 5; // Intelligence for increased survival awareness and better resource management
            SkillBonuses.SetValues(0, SkillName.Camping, 25.0); // Significant bonus to Camping skill
            SkillBonuses.SetValues(1, SkillName.Hiding, 10.0); // Bonus to Hiding for stealth in the wilderness
            Resistances.Physical = 8; // Basic physical resistance to protect the wearer from harm while outdoors
            Resistances.Fire = 5; // Slight fire resistance, useful when working around campfires
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Optionally attach custom XML data for your shard’s needs

            // You can also set it to Blessed if you want to ensure it’s safe from being looted
            this.LootType = LootType.Blessed; // Makes sure this item stays with the character
        }

        public WildernessQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Wilderness Quiver (you can adjust the number in the language file)
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
