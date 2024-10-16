using System;
using Server;

namespace Server.Items
{
    public class HistoryOfLizardmen : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Lizardmen", "Scribus",
                new BookPageInfo
                (
                    "Lizardmen have roamed",
                    "the lands for countless",
                    "generations. Their",
                    "history is as vast",
                    "as it is fascinating.",
                    "",
                    "",
                    "          -Scribus"
                ),
                new BookPageInfo
                (
                    "Originating from the",
                    "swamps, they are often",
                    "mistaken as mere",
                    "monsters. This book",
                    "aims to provide a",
                    "comprehensive history",
                    "of this misunderstood",
                    "race."
                ),
                new BookPageInfo
                (
                    "The first recorded",
                    "encounter of lizardmen",
                    "was documented by the",
                    "ancient scholar Aroth,",
                    "who ventured deep into",
                    "the swamps and lived",
                    "to tell the tale."
                ),
                new BookPageInfo
                (
                    "Lizardmen society is",
                    "matriarchal, led by the",
                    "most powerful female",
                    "warrior. They value",
                    "strength and cunning,",
                    "traits necessary for",
                    "survival in the harsh",
                    "swamp environment."
                ),
                new BookPageInfo
                (
                    "Many adventurers have",
                    "tried to uncover the",
                    "secrets of the lizardmen,",
                    "but few have succeeded.",
                    "Their hidden villages",
                    "are well-guarded and",
                    "protected by traps."
                ),
                new BookPageInfo
                (
                    "In recent times, some",
                    "lizardmen have begun",
                    "to venture out of the",
                    "swamps, driven by a",
                    "need to explore or",
                    "escape increasing",
                    "threats to their habitat."
                ),
				new BookPageInfo
				(
					"The Religion of the",
					"Lizardmen revolves",
					"around the worship of",
					"dragons, their mythical",
					"ancestors. Rituals and",
					"sacrifices are performed",
					"to earn their blessings."
				),
				new BookPageInfo
				(
					"They are also known",
					"for their craftsmanship",
					"in weaponry. Although",
					"they mostly rely on",
					"natural materials like",
					"bones and stones, their",
					"weapons are deadly."
				),
				new BookPageInfo
				(
					"Lizardmen are generally",
					"xenophobic but have",
					"been known to form",
					"alliances. Several times",
					"in history, they've",
					"allied with humans",
					"against common threats."
				),
				new BookPageInfo
				(
					"Lizardmen have their",
					"own form of written",
					"language, comprised",
					"of intricate symbols",
					"and patterns. This",
					"script is a closely",
					"guarded secret."
				),
				new BookPageInfo
				(
					"Warfare is common",
					"among different tribes.",
					"Conflicts often arise",
					"over territory, resources",
					"or religious disputes.",
					"The victors often absorb",
					"the losing tribe."
				),
				new BookPageInfo
				(
					"Lizardmen have been",
					"subject to many",
					"misunderstandings and",
					"myths. Contrary to",
					"popular belief, they are",
					"not simply mindless",
					"beasts but a society"
				),
				new BookPageInfo
				(
					"with its own sets of",
					"laws, traditions, and",
					"culture. They have a",
					"deep respect for nature",
					"and live in harmony",
					"with their swampy",
					"environments."
				),
				new BookPageInfo
				(
					"In recent years,",
					"encroachment by human",
					"settlements has put",
					"pressure on lizardmen",
					"territories, leading to",
					"more frequent conflicts",
					"between the two races."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfLizardmen() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Lizardmen");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Lizardmen");
        }

        public HistoryOfLizardmen(Serial serial) : base(serial)
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
