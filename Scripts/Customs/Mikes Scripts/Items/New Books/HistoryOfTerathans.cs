using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTerathans : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Terathans", "Archivist Zir",
                new BookPageInfo
                (
                    "The Terathans are a",
                    "race of spider-like",
                    "creatures that dwell",
                    "in dark caverns.",
                    "They have been known",
                    "to be aggressive",
                    "and territorial.",
                    "            -Zir"
                ),
                new BookPageInfo
                (
                    "Their history is one",
                    "of conquest and",
                    "expansion, often at",
                    "the expense of",
                    "other creatures.",
                    "They worship a deity",
                    "known as 'Zyrra', the",
                    "Spider Queen."
                ),
                new BookPageInfo
                (
                    "Terathans are",
                    "organized in a",
                    "hierarchical society,",
                    "with Queens being",
                    "the ultimate rulers.",
                    "Warriors and drones",
                    "serve under them,",
                    "ensuring the hive"
                ),
                new BookPageInfo
                (
                    "functions efficiently.",
                    "Although they often",
                    "keep to themselves,",
                    "Terathans are known",
                    "to raid nearby",
                    "villages for",
                    "resources and",
                    "sacrifices."
                ),
                new BookPageInfo
                (
                    "In the past, there",
                    "have been several",
                    "attempts to establish",
                    "peace with the",
                    "Terathans, but all",
                    "have failed due to",
                    "their aggressive",
                    "nature."
                ),
                new BookPageInfo
                (
                    "As adventurers, it",
                    "is crucial to be",
                    "aware of Terathans",
                    "if delving into",
                    "caverns or other",
                    "dark places. Their",
                    "venom can be deadly,",
                    "and their numbers"
                ),
                new BookPageInfo
                (
                    "overwhelming if",
                    "not careful. Yet,",
                    "their chitin and",
                    "silk are valuable",
                    "resources if one",
                    "manages to overcome",
                    "them."
                ),
				new BookPageInfo
				(
					"In recent times, some",
					"scholars have shown",
					"interest in studying",
					"Terathan culture. While",
					"dangerous, these",
					"expeditions have",
					"revealed fascinating",
					"aspects."
				),
				new BookPageInfo
				(
					"For instance, Terathans",
					"are skilled in crafting",
					"with their silk, creating",
					"intricate patterns in",
					"their webbing that seem",
					"to serve both practical",
					"and decorative purposes."
				),
				new BookPageInfo
				(
					"Additionally, it has",
					"been discovered that",
					"they have a unique",
					"method of communication",
					"involving clicks and",
					"posture changes, not",
					"unlike a complex dance."
				),
				new BookPageInfo
				(
					"The Terathans have",
					"also been observed",
					"using alchemical",
					"substances, though the",
					"details of their",
					"alchemical practices",
					"remain largely unknown."
				),
				new BookPageInfo
				(
					"Despite these civilized",
					"qualities, one must not",
					"forget their aggressive",
					"tendencies. Terathans",
					"view other races as",
					"either threats or",
					"resources, seldom as",
					"equals."
				),
				new BookPageInfo
				(
					"It is said that the",
					"Terathans once had a",
					"civil war, a schism",
					"between two prominent",
					"Queens. The conflict",
					"was devastating and led",
					"to their current",
					"isolationism."
				),
				new BookPageInfo
				(
					"The Terathan's venom",
					"has been studied for",
					"its alchemical properties.",
					"Though potent and deadly,",
					"when properly diluted, it",
					"can serve as a base for",
					"potent elixirs."
				),
				new BookPageInfo
				(
					"In summary, while the",
					"Terathans may be feared",
					"and often rightfully so,",
					"they are also a subject",
					"of great fascination.",
					"They are a complex race",
					"with much yet to be",
					"understood."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTerathans() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Terathans");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Terathans");
        }

        public HistoryOfTerathans(Serial serial) : base(serial)
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
