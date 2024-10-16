using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheGargoyles : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Gargoyles", "Historian Kael",
                new BookPageInfo
                (
                    "This tome aims to",
                    "provide a concise",
                    "history of the",
                    "Gargoyle race.",
                    "Beginning from their",
                    "creation to their",
                    "role in modern-day",
                    "Britannia."
                ),
                new BookPageInfo
                (
                    "Gargoyles have",
                    "existed long before",
                    "most recorded",
                    "history. Originating",
                    "from the lands of",
                    "Ter Mur, they have",
                    "been both enemies",
                    "and allies."
                ),
                new BookPageInfo
                (
                    "Their civilization",
                    "was governed by",
                    "the Three",
                    "Principles: Control,",
                    "Passion, and",
                    "Diligence. This",
                    "ideology formed",
                    "their society."
                ),
                new BookPageInfo
                (
                    "Gargoyles are known",
                    "for their mastery in",
                    "mysticism and",
                    "alchemy, although",
                    "they are equally",
                    "adept in melee",
                    "combat.",
                    ""
                ),
                new BookPageInfo
                (
                    "The relationship",
                    "with humans has",
                    "been tumultuous,",
                    "but in times of",
                    "dire need, alliances",
                    "have been formed",
                    "to fend off greater",
                    "evils."
                ),
                new BookPageInfo
                (
                    "The contributions",
                    "of Gargoyles are",
                    "immense, from the",
                    "Void Plague",
                    "Eradication to the",
                    "Battle of",
                    "Windshade.",
                    ""
                ),
                new BookPageInfo
                (
                    "It is our hope that",
                    "by understanding",
                    "our neighbors, we",
                    "can build a",
                    "symbiotic",
                    "relationship for",
                    "generations to",
                    "come."
                ),
				new BookPageInfo
				(
					"The First Era:",
					"The Gargoyle",
					"Inception",
					"",
					"The Gargoyles were",
					"first created by the",
					"Primordials as beings",
					"of earth and magic."
				),
				new BookPageInfo
				(
					"It is said their",
					"bodies were chiseled",
					"from stone and then",
					"imbued with the",
					"essence of magics",
					"unknown to humans."
				),
				new BookPageInfo
				(
					"The Second Era:",
					"The Great Split",
					"",
					"A schism occurred",
					"between the Gargoyles",
					"over the interpretation",
					"of the Three Principles."
				),
				new BookPageInfo
				(
					"This split led to the",
					"formation of two",
					"distinct sects, each",
					"believing their",
					"understanding was the",
					"true path."
				),
				new BookPageInfo
				(
					"The Third Era:",
					"The Age of",
					"Enlightenment",
					"",
					"The Gargoyles made",
					"significant advancements",
					"in magic and technology."
				),
				new BookPageInfo
				(
					"The creation of the",
					"Mystic Portal allowed",
					"them to travel between",
					"worlds and gather",
					"knowledge."
				),
				new BookPageInfo
				(
					"The Fourth Era:",
					"Alliance and War",
					"",
					"The Gargoyles found",
					"themselves in alliances",
					"and conflicts with",
					"other races."
				),
				new BookPageInfo
				(
					"Despite their wisdom,",
					"war was sometimes",
					"inevitable, leading to",
					"losses on both sides."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheGargoyles() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Gargoyles");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Gargoyles");
        }

        public HistoryOfTheGargoyles(Serial serial) : base(serial)
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
