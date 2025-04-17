using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class NinjasQuiver : BaseQuiver
    {
        [Constructable]
        public NinjasQuiver()
            : base()
        {
            Name = "Ninja's Quiver";
            Hue = 1102; // Dark hue for a more stealthy look
            this.WeightReduction = 50; // Increased weight reduction to enhance mobility
            Attributes.BonusDex = 8; // Boost Dexterity for faster actions
            Attributes.BonusInt = 5; // Boost Intelligence for Ninjutsu-related abilities
            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 20.0); // Adds a bonus to Ninjutsu skill
            SkillBonuses.SetValues(1, SkillName.Hiding, 15.0); // Adds a bonus to Hiding skill for stealth
            SkillBonuses.SetValues(2, SkillName.Stealth, 10.0); // Adds a bonus to Stealth skill
            Resistances.Physical = 8; // Moderate resistance to physical damage
            Resistances.Fire = 5; // Minor resistance to Fire for elemental protection
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach to the item for extra customization in the shard

            // Lore for the Ninja quiver (can be customized for your shard’s story)
            this.LootType = LootType.Blessed; // Keeps the item from being looted by others
        }

        public NinjasQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Ninja's Quiver (adjust in the language file)
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
