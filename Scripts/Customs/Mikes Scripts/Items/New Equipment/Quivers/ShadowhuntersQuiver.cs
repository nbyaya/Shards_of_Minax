using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    [FlipableAttribute(0x2FB7, 0x3171)]
    public class ShadowhuntersQuiver : BaseQuiver
    {
        [Constructable]
        public ShadowhuntersQuiver()
            : base()
        {
            Name = "Shadowhunter's Quiver";
            Hue = Utility.Random(1200, 1250); // A darker hue to match the stealth theme
            this.WeightReduction = 30; // Light weight to help with agility
            Attributes.BonusDex = 7; // Dexterity boost for faster movement and more precise actions
            Attributes.BonusInt = 5; // Intelligence boost for enhanced stealth and perception
            SkillBonuses.SetValues(0, SkillName.Hiding, 20.0); // Increased Hiding skill for better stealth
            SkillBonuses.SetValues(1, SkillName.Stealth, 15.0); // Boosts Stealth skill to complement Hiding
            Resistances.Physical = 8; // Minor physical resistance to enhance survivability
            Resistances.Fire = 5; // Low fire resistance for utility in combat situations

            // Optional: Attach an extra XmlLevelItem for more shard-specific customization
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Add a lore tag that highlights its connection to stealthy hunters
            this.LootType = LootType.Blessed; // Keeps it from being looted by others for roleplay or PvP
        }

        public ShadowhuntersQuiver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1032659; // Custom label for the Shadowhunter's Quiver (adjust this in the language file)
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
