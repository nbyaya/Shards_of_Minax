using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheTitans : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Titans", "Athena",
                new BookPageInfo
                (
                    "The Titans were ancient",
                    "deities predating even",
                    "the gods of Olympus.",
                    "In the beginning, the",
                    "universe was ruled by",
                    "Chaos, until the Titans",
                    "rose to power."
                ),
                new BookPageInfo
                (
                    "The leader of the Titans",
                    "was Kronos, who was",
                    "overthrown by his own",
                    "son Zeus. This led to",
                    "the era of the Olympian",
                    "gods who ruled the",
                    "heavens, the Earth, and",
                    "the Underworld."
                ),
                new BookPageInfo
                (
                    "Although defeated, the",
                    "Titans were not entirely",
                    "destroyed. Some myths",
                    "suggest that they",
                    "remain imprisoned in",
                    "Tartarus, plotting their",
                    "return."
                ),
                new BookPageInfo
                (
                    "Despite their brutal",
                    "reputation, the Titans",
                    "contributed much to the",
                    "world. They were the",
                    "primordial forces of",
                    "nature, and their",
                    "legacy remains."
                ),
				new BookPageInfo
				(
					"Among the Titans, many",
					"were key figures in",
					"the shaping of the",
					"universe. Hyperion, the",
					"father of sun, moon,",
					"and dawn, ruled the",
					"skies."
				),
				new BookPageInfo
				(
					"Then there was Oceanus,",
					"the Titan god of the",
					"sea, and his sister",
					"Tethys, who was the",
					"nurturer of the world's",
					"rivers and streams."
				),
				new BookPageInfo
				(
					"Prometheus, one of the",
					"more popular Titans,",
					"is famed for his wisdom",
					"and cunning. He is best",
					"known for stealing fire",
					"from the gods and",
					"giving it to mankind."
				),
				new BookPageInfo
				(
					"The punishment of",
					"Prometheus was a tale",
					"that symbolized the",
					"endless struggle between",
					"mortal and divine.",
					"Bound to a rock, his",
					"liver eaten daily by an",
					"eagle."
				),
				new BookPageInfo
				(
					"Atlas, another notable",
					"Titan, was condemned to",
					"hold up the sky on his",
					"shoulders as punishment",
					"for his role in the",
					"Titanomachy, the war",
					"between the Titans and",
					"Olympians."
				),
				new BookPageInfo
				(
					"The tale of the Titans",
					"serves as a somber",
					"reminder that even the",
					"most powerful beings can",
					"be overthrown. Yet,",
					"their legacy and their",
					"contributions to the",
					"cosmos remain."
				),
				new BookPageInfo
				(
					"Scholars often debate",
					"the fate of the Titans.",
					"Some say they are still",
					"imprisoned, others",
					"believe they have",
					"escaped, waiting for",
					"the right moment to",
					"return."
				),
				new BookPageInfo
				(
					"In either case, the",
					"Titans are an integral",
					"part of our world's",
					"mythology and their",
					"stories will be told",
					"for generations to come."
				)

		
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheTitans() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Titans");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Titans");
        }

        public HistoryOfTheTitans(Serial serial) : base(serial)
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
