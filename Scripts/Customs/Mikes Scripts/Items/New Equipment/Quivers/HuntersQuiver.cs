using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class HuntersQuiver : BaseQuiver
    {
        [Constructable]
        public HuntersQuiver()
            : base()
        {
            Name = "Hunter's Quiver";
            Hue = Utility.Random(2500, 2250); // Random hue for variation
            this.WeightReduction = 40; // Increased weight reduction for a specialized quiver
            Attributes.BonusDex = 5; // Adds Dexterity for archers
            Attributes.BonusStr = 3; // Adds Strength for more damage output
            SkillBonuses.SetValues(0, SkillName.Archery, 15.0); // Adds skill in Archery
            SkillBonuses.SetValues(1, SkillName.Swords, 10.0); // Adds skill for versatility (if using swords as well)
            Resistances.Physical = 12; // Provides some physical resistance
            Resistances.Energy = 5; // Minor resistance to energy attacks
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the item for extra customization in the shard


        }

        public HuntersQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032658; // Custom label for the Hunter's Quiver (you could adjust this number in the language file)
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
