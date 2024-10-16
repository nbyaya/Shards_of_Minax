using System;
using Server;

namespace Server.Items
{
    public class OnJestersBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On Jesters", "Anonymous",
                new BookPageInfo
                (
                    "In the court of kings",
                    "and queens, jesters",
                    "have been revered as",
                    "much as they have",
                    "been scorned.",
                    "",
                    "",
                    "          -Anonymous"
                ),
                new BookPageInfo
                (
                    "Their jokes, sometimes",
                    "biting and insightful,",
                    "reveal the folly of",
                    "man and the paradox",
                    "of human existence.",
                    "Yet, their status",
                    "often renders them",
                    "disposable."
                ),
                new BookPageInfo
                (
                    "But what many fail",
                    "to realize is the",
                    "deep wisdom that",
                    "often hides behind",
                    "their painted smiles",
                    "and exaggerated",
                    "expressions. In this",
                    "book, we explore"
                ),
                new BookPageInfo
                (
                    "the art, the craft,",
                    "and the intricate",
                    "dances of word and",
                    "wit that define the",
                    "world of jesters.",
                    "From the tools of",
                    "their trade to the",
                    "unspoken rules that"
                ),
                new BookPageInfo
                (
                    "govern their",
                    "performances, we",
                    "take a closer look",
                    "at these enigmatic",
                    "figures of humor",
                    "and satire."
                ),
                new BookPageInfo
                (
                    "Whether in the halls",
                    "of great palaces, or",
                    "the streets of",
                    "bustling towns, jesters",
                    "are more than mere",
                    "entertainers. They",
                    "are the mirror in",
                    "which society"
                ),
                new BookPageInfo
                (
                    "sees its true self,",
                    "if only for a",
                    "fleeting moment.",
                    "So the next time",
                    "you laugh at a jester's",
                    "japes, remember the",
                    "wisdom that fuels",
                    "the folly."
                ),
				new BookPageInfo
				(
					"The jester's attire",
					"is not merely for",
					"show. Each piece of",
					"clothing, each bell",
					"on the cap, has a",
					"purpose. Together,",
					"they form an armor",
					"of ridicule."
				),
				new BookPageInfo
				(
					"A common tool among",
					"jesters is the 'bauble'",
					"a small scepter-like",
					"item often used in",
					"their routines. While",
					"it may appear trivial",
					"to the untrained eye,",
					"it is a symbol of"
				),
				new BookPageInfo
				(
					"the jester's trade,",
					"and sometimes, their",
					"only friend in a court",
					"full of intrigue and",
					"duplicity.",
					"",
					"",
					"Jester's Guild"
				),
				new BookPageInfo
				(
					"Contrary to popular",
					"belief, many jesters",
					"were part of guilds.",
					"These organizations",
					"trained young",
					"aspiring jesters in",
					"the art of humor, wit,",
					"and even survival."
				),
				new BookPageInfo
				(
					"Being a jester was",
					"often a dangerous",
					"job, especially when",
					"the humor revealed",
					"too much truth for",
					"powerful people. The",
					"guild provided a",
					"network of safety."
				),
				new BookPageInfo
				(
					"Through a series of",
					"hand gestures, secret",
					"signs, and coded",
					"language, jesters",
					"could communicate",
					"with each other even",
					"in the most hostile",
					"of environments."
				),
				new BookPageInfo
				(
					"In summary, jesters",
					"are more than mere",
					"clowns or entertainers.",
					"They are artists of",
					"the intangible, keepers",
					"of human folly, and",
					"sometimes, the",
					"wisest among us."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnJestersBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On Jesters");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On Jesters");
        }

        public OnJestersBook(Serial serial) : base(serial)
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
