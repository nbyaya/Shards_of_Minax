using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfSpiderSilk : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Magical Properties", "Arachna",
                new BookPageInfo
                (
                    "This book aims to",
                    "explore the mystical",
                    "properties of spider",
                    "silk, a material often",
                    "overlooked yet",
                    "imbued with great",
                    "magical potential.",
                    ""
                ),
                new BookPageInfo
                (
                    "Spider silk is more",
                    "than just the web",
                    "of common spiders.",
                    "It has been used in",
                    "various magical",
                    "artifacts, potions,",
                    "and even spells."
                ),
                new BookPageInfo
                (
                    "To harness the silk's",
                    "magic, one must first",
                    "know how to properly",
                    "extract it without",
                    "damaging its innate",
                    "magical properties.",
                    "",
                    "It is often best"
                ),
                new BookPageInfo
                (
                    "to gather silk from",
                    "magical spider",
                    "species. These rare",
                    "species can be",
                    "found in hidden",
                    "corners of the",
                    "realm, usually",
                    "guarded by various"
                ),
                new BookPageInfo
                (
                    "challenges and traps.",
                    "Once you have the",
                    "silk, the possibilities",
                    "are endless. From",
                    "enchanting armor",
                    "to brewing potent",
                    "elixirs, the silk's",
                    "potential is vast."
                ),
                new BookPageInfo
                (
                    "This book serves as",
                    "a guide for the",
                    "aspiring magicians",
                    "and alchemists who",
                    "seek to unlock the",
                    "hidden powers of",
                    "this incredible",
                    "material."
                ),
				new BookPageInfo
				(
					"Properties of the Silk",
					"------------------------",
					"1) Conductivity: The silk",
					"has the natural ability to",
					"conduct magical energies,",
					"making it ideal for use in",
					"runes and glyphs."
				),
				new BookPageInfo
				(
					"2) Durability: The magical",
					"silk is much stronger than",
					"ordinary silk, and can",
					"therefore be used in",
					"magical bindings and",
					"seals."
				),
				new BookPageInfo
				(
					"3) Flexibility: Its flexible",
					"nature makes it invaluable",
					"in the creation of magic",
					"items that require",
					"movement, such as animated",
					"golems."
				),
				new BookPageInfo
				(
					"4) Purity: The silk can be",
					"used to purify other",
					"materials, enhancing the",
					"potency of potions and",
					"elixirs."
				),
				new BookPageInfo
				(
					"5) Elemental Affinity: The",
					"silk exhibits a strong",
					"affinity to elemental",
					"forces, making it a key",
					"component in spells and",
					"rituals."
				),
				new BookPageInfo
				(
					"How to Harvest",
					"----------------",
					"Harvesting magical spider",
					"silk is a delicate process.",
					"It involves the use of a",
					"special enchanted tool."
				),
				new BookPageInfo
				(
					"The tool, often called a",
					"\"Silk Strainer\", is passed",
					"carefully along the web,",
					"collecting the silk without",
					"damaging its magical",
					"properties."
				),
				new BookPageInfo
				(
					"It is important to approach",
					"the spider with great",
					"caution, as these magical",
					"species can be very",
					"aggressive."
				),
				new BookPageInfo
				(
					"Usage in Spells",
					"----------------",
					"To use the silk in spells,",
					"it should be woven into a",
					"circle and placed as a",
					"conduit in the center of",
					"the ritual area."
				),
				new BookPageInfo
				(
					"The magical properties of",
					"the silk will amplify the",
					"effects of the spell,",
					"making it more potent or",
					"far-reaching."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MagicalPropertiesOfSpiderSilk() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Magical Properties of Spider Silk");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Magical Properties of Spider Silk");
        }

        public MagicalPropertiesOfSpiderSilk(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
