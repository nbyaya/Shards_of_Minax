using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class LockpickersQuiver : BaseQuiver
    {
        [Constructable]
        public LockpickersQuiver()
            : base()
        {
            Name = "Lockpicker's Quiver";
            Hue = Utility.Random(2500, 2250); // Random hue for uniqueness
            this.WeightReduction = 25; // A moderate weight reduction
            Attributes.BonusDex = 5; // Dexterity boost for quicker movements
            Attributes.BonusInt = 5; // Intelligence boost to aid in lockpicking and stealth
            SkillBonuses.SetValues(0, SkillName.Lockpicking, 20.0); // Adds a bonus to Lockpicking skill
            SkillBonuses.SetValues(1, SkillName.Hiding, 10.0); // Boosts Hiding skill, useful for thieves
            SkillBonuses.SetValues(2, SkillName.Snooping, 10.0); // Enhances Snooping skill for gathering info
            Resistances.Physical = 5; // Small physical resistance for protection
            XmlAttach.AttachTo(this, new XmlLevelItem()); // Allows for additional customization through XML

            // Additional flair, such as a special lore message for roleplay
            this.LootType = LootType.Blessed; // Keeps the item safe from being looted
        }

        public LockpickersQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Lockpicker's Quiver (adjust in the language file as needed)
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
