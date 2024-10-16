using System;
using Server;

namespace Server.Items
{
    public class HistoryFireElementals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Fire Elementals", "Pyraethus",
                new BookPageInfo
                (
                    "In the depths of the",
                    "volcanoes and fiery",
                    "chambers of the world",
                    "lies the home of the",
                    "Fire Elementals. These",
                    "beings are as ancient",
                    "as the world itself,",
                    "born from the flames."
                ),
                new BookPageInfo
                (
                    "They are masters of",
                    "heat and combustion.",
                    "Fire Elementals are",
                    "known for their",
                    "ability to manipulate",
                    "fire, causing both",
                    "destruction and",
                    "renewal."
                ),
                new BookPageInfo
                (
                    "Contrary to common",
                    "belief, these beings",
                    "are not inherently",
                    "evil. Rather, they",
                    "represent the raw",
                    "force of fire in its",
                    "purest form. Yet,",
                    "they can be very"
                ),
                new BookPageInfo
                (
                    "dangerous if not",
                    "approached with",
                    "respect and caution.",
                    "They've been known",
                    "to forge alliances",
                    "with powerful mages",
                    "and offer their fire",
                    "in service."
                ),
                new BookPageInfo
                (
                    "The ancient scrolls",
                    "mention a special",
                    "ritual to summon",
                    "them, though such",
                    "knowledge is often",
                    "considered dangerous.",
                    "It involves the use",
                    "of rare fire gems."
                ),
                new BookPageInfo
                (
                    "This book serves as",
                    "a brief introduction",
                    "to the fascinating",
                    "world of Fire",
                    "Elementals. Approach",
                    "with care, for fire",
                    "is as much a friend",
                    "as it is a foe."
                ),
				new BookPageInfo
				(
					"Origins",
					"--------",
					"The Fire Elementals",
					"are believed to have",
					"originated from the",
					"Elemental Plane of",
					"Fire, a realm of",
					"unending flames."
				),
				new BookPageInfo
				(
					"Characteristics",
					"---------------",
					"Typically, Fire",
					"Elementals are",
					"composed of molten",
					"rock, magma, and",
					"pure flames. Their",
					"form is ever-changing."
				),
				new BookPageInfo
				(
					"Abilities",
					"---------",
					"Their abilities range",
					"from spitting fireballs",
					"to incinerating vast",
					"areas. They can even",
					"absorb fire-based",
					"attacks, gaining",
					"strength from them."
				),
				new BookPageInfo
				(
					"Weaknesses",
					"----------",
					"While mighty, these",
					"beings are vulnerable",
					"to cold-based magics",
					"and spells that",
					"extinguish flames,",
					"such as Water spells."
				),
				new BookPageInfo
				(
					"Summoning",
					"----------",
					"Skilled mages have",
					"been known to summon",
					"Fire Elementals using",
					"a combination of",
					"Fire and Summoning",
					"magics."
				),
				new BookPageInfo
				(
					"In Culture",
					"-----------",
					"Fire Elementals have",
					"been depicted in",
					"legends and folktales",
					"as both heroes and",
					"villains. They are",
					"revered and feared."
				),
				new BookPageInfo
				(
					"Interaction",
					"-----------",
					"Approaching a Fire",
					"Elemental should be",
					"done with extreme",
					"caution. Offerings",
					"of flame gems may",
					"gain their favor."
				),
				new BookPageInfo
				(
					"In Closing",
					"----------",
					"This book serves as",
					"a comprehensive",
					"guide to Fire",
					"Elementals. May your",
					"path be ever lit by",
					"the flames of",
					"knowledge."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryFireElementals() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Fire Elementals");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Fire Elementals");
        }

        public HistoryFireElementals(Serial serial) : base(serial)
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
