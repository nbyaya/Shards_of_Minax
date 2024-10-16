using System;
using Server;

namespace Server.Items
{
    public class SpeculationsOnIceAndFrost : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Speculations on Ice", "Eirwen the Sage",
                new BookPageInfo
                (
                    "This tome aims to",
                    "explore the mysteries",
                    "and behaviors of ice",
                    "and frost creatures.",
                    "",
                    "",
                    "",
                    "            -Eirwen"
                ),
                new BookPageInfo
                (
                    "The icy depths of",
                    "Sosaria hide many",
                    "creatures, both",
                    "enigmatic and",
                    "dangerous. It is",
                    "believed that these",
                    "entities are born",
                    "from the pure essence"
                ),
                new BookPageInfo
                (
                    "of frost and ice.",
                    "Unlike regular",
                    "beasts, their",
                    "constitution seems",
                    "to be a blend of",
                    "arcane magic and",
                    "natural phenomena.",
                    ""
                ),
                new BookPageInfo
                (
                    "Common types include",
                    "Frost Trolls, Ice",
                    "Elementals, and the",
                    "elusive Snow Leopards.",
                    "These creatures are",
                    "often more resilient",
                    "to physical damage,",
                    "requiring magical"
                ),
                new BookPageInfo
                (
                    "means for an effective",
                    "counter.",
                    "",
                    "It's crucial for",
                    "adventurers to equip",
                    "themselves properly",
                    "before venturing into",
                    "their domains."
                ),
                new BookPageInfo
                (
                    "It's speculated that",
                    "these creatures may",
                    "have a unique",
                    "hierarchy or social",
                    "system, as yet",
                    "undiscovered by",
                    "mankind.",
                    ""
                ),
				new BookPageInfo
				(
					"The common Frost Troll",
					"is often mistakenly",
					"judged as a simple",
					"brute. Recent findings",
					"suggest they may have",
					"rudimentary religious",
					"practices involving",
					"ice sculptures."
				),
				new BookPageInfo
				(
					"Ice Elementals, on the",
					"other hand, are magical",
					"constructs, often bound",
					"to the will of a",
					"powerful sorcerer.",
					"Their core of frost",
					"essence is their",
					"lifeblood."
				),
				new BookPageInfo
				(
					"Snow Leopards, a rare",
					"sight, are believed to",
					"be magical beings",
					"transformed by the",
					"intense cold. Sightings",
					"often precede snowfall,",
					"leading to myths about",
					"their control over weather."
				),
				new BookPageInfo
				(
					"Many scholars are",
					"intrigued by the",
					"mysterious Ice Serpents.",
					"These creatures are",
					"known to guard ancient",
					"artifacts and are",
					"often found near",
					"frozen lakes."
				),
				new BookPageInfo
				(
					"In local folklore,",
					"a creature called the",
					"\"Frost Maiden\" is often",
					"mentioned. It's said",
					"to appear as a woman",
					"made entirely of ice,",
					"leading travelers",
					"astray."
				),
				new BookPageInfo
				(
					"These speculations are",
					"the first step to",
					"understanding the true",
					"nature of these ice",
					"and frost creatures.",
					"Further research is",
					"imperative for the",
					"betterment of Sosaria."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SpeculationsOnIceAndFrost() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Speculations on Ice and Frost Creatures");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Speculations on Ice and Frost Creatures");
        }

        public SpeculationsOnIceAndFrost(Serial serial) : base(serial)
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
