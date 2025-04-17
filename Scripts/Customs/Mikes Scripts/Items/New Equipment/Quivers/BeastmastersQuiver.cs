using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class BeastmastersQuiver : BaseQuiver
    {
        [Constructable]
        public BeastmastersQuiver()
            : base()
        {
            Name = "Beastmaster's Quiver";
            Hue = Utility.Random(1300, 1400); // A nature-based hue for a Beastmaster theme
            this.WeightReduction = 50; // Enhanced weight reduction for tamers who are always on the move
            Attributes.BonusInt = 8; // Bonus to Intelligence, useful for tamers
            Attributes.BonusDex = 5; // Bonus to Dexterity for taming agility and speed
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0); // A direct bonus to Animal Taming skill
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0); // Adds bonus to Animal Lore for understanding animals better
            SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0); // Veterinary skill to heal your tamed animals
            Resistances.Physical = 10; // Minor physical resistance, as tamers might face danger from wild creatures
            Resistances.Poison = 8; // Added resistance to poison, as tamers often deal with poisonous creatures
            XmlAttach.AttachTo(this, new XmlLevelItem()); // For additional customization through your shard’s XmlSpawner system

            // Optionally, provide a visual representation (using LootType for Blessing)
            this.LootType = LootType.Blessed; // Keep it safe for the tamer to prevent accidental loss
        }

        public BeastmastersQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Beastmaster's Quiver
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
