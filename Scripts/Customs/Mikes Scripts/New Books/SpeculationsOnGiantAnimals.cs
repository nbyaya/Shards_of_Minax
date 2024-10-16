using System;
using Server;

namespace Server.Items
{
    public class SpeculationsOnGiantAnimals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Speculations on Giant Animals", "Eldric the Wise",
                new BookPageInfo
                (
                    "This manuscript aims",
                    "to explore the various",
                    "theories and",
                    "speculations about",
                    "the existence of",
                    "giant animals in our",
                    "world.",
                    "        -Eldric"
                ),
                new BookPageInfo
                (
                    "The first account of",
                    "a giant creature",
                    "dates back to the age",
                    "of dragons. It was",
                    "said that these",
                    "dragons were so",
                    "large they could",
                    "carry off cattle."
                ),
                new BookPageInfo
                (
                    "In the mountains,",
                    "rumors persist of",
                    "giant spiders. Their",
                    "webs are said to be",
                    "so strong that even",
                    "horses can be",
                    "ensnared, left to",
                    "become the spider's"
                ),
                new BookPageInfo
                (
                    "next meal. Travelers",
                    "are advised to avoid",
                    "known spider",
                    "territories, as they",
                    "are exceedingly",
                    "dangerous and",
                    "unpredictable.",
                    ""
                ),
                new BookPageInfo
                (
                    "Giant rats in the",
                    "sewers are another",
                    "topic of frequent",
                    "discussion. Though",
                    "most citizens",
                    "dismiss it as a myth,",
                    "several adventurers",
                    "swear to have seen"
                ),
                new BookPageInfo
                (
                    "rats as large as",
                    "dogs lurking in the",
                    "depths below.",
                    "Whatever the truth,",
                    "it remains a subject",
                    "of both dread and",
                    "curiosity."
                ),
				new BookPageInfo
				(
					"The ocean hides",
					"even more secrets.",
					"Old sailor tales",
					"speak of krakens and",
					"leviathans, giant",
					"beasts that could",
					"engulf entire ships.",
					"These monsters are"
				),
				new BookPageInfo
				(
					"believed to be",
					"responsible for",
					"many lost voyages.",
					"Though evidence is",
					"scarce, the ocean",
					"depths remain largely",
					"unexplored.",
					""
				),
				new BookPageInfo
				(
					"In the skies, giant",
					"eagles and griffins",
					"are said to roam.",
					"According to legend,",
					"they dwell atop the",
					"highest peaks, away",
					"from human reach,",
					"guarding vast"
				),
				new BookPageInfo
				(
					"treasures. Those",
					"brave enough to",
					"ascend these",
					"mountains often",
					"return empty-handed,",
					"if they return at all.",
					"",
					""
				),
				new BookPageInfo
				(
					"Some even speculate",
					"that the giants of",
					"old were not just",
					"larger humans but",
					"may have had some",
					"connection to these",
					"giant beasts.",
					"Could it be that"
				),
				new BookPageInfo
				(
					"giants rode these",
					"immense creatures?",
					"While that remains",
					"pure speculation,",
					"the idea is",
					"intriguing to say",
					"the least.",
					""
				),
				new BookPageInfo
				(
					"Conclusively, while",
					"many of these",
					"theories are not",
					"supported by",
					"tangible evidence,",
					"they persist in the",
					"common lore and",
					"deserve investigation."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SpeculationsOnGiantAnimals() : base( false )
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Speculations on Giant Animals");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Speculations on Giant Animals");
        }

        public SpeculationsOnGiantAnimals(Serial serial) : base(serial)
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
