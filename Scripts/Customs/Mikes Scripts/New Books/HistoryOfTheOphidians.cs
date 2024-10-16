using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheOphidians : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Ophidians", "Author Unknown",
                new BookPageInfo
                (
                    "The Ophidians are a",
                    "race of serpentine",
                    "beings living deep",
                    "in the underground",
                    "lairs.",
                    "This book explores",
                    "their history.",
                    "",
                    "       -Unknown"
                ),
                new BookPageInfo
                (
                    "The Ophidians were",
                    "once a highly",
                    "civilized society.",
                    "Their ancient ruins",
                    "still remain as",
                    "testaments to their",
                    "greatness.",
                    "However, they were"
                ),
                new BookPageInfo
                (
                    "forced to retreat",
                    "into obscurity due to",
                    "external threats and",
                    "internal strife.",
                    "Today, they are",
                    "often seen as",
                    "aggressive and",
                    "dangerous."
                ),
                new BookPageInfo
                (
                    "Their society is",
                    "divided into various",
                    "castes such as",
                    "warriors, shamans,",
                    "and the ruling elite.",
                    "Their complex",
                    "rituals and",
                    "ceremonies are still"
                ),
                new BookPageInfo
                (
                    "a mystery to",
                    "outsiders.",
                    "Despite their",
                    "isolation, they are",
                    "known to trade",
                    "with trusted",
                    "outsiders.",
                    "The items they trade"
                ),
                new BookPageInfo
                (
                    "are highly valued",
                    "for their unique",
                    "magical properties.",
                    "But one must",
                    "tread carefully",
                    "when dealing with",
                    "the Ophidians."
                ),
				new BookPageInfo
				(
					"The Ophidians' rise",
					"and fall is deeply",
					"interlinked with their",
					"belief in the Great",
					"Serpent, a deity",
					"that they consider",
					"to be the creator of",
					"all things."
				),
				new BookPageInfo
				(
					"This religious",
					"devotion led to",
					"years of internal",
					"struggles between",
					"different sects.",
					"Each sect interpreted",
					"the deity's will",
					"in their own way."
				),
				new BookPageInfo
				(
					"The constant strife",
					"left them vulnerable",
					"to invasions from",
					"other races and",
					"creatures.",
					"Over time, they",
					"were forced to",
					"retreat underground."
				),
				new BookPageInfo
				(
					"Their knowledge in",
					"alchemy and",
					"enchantments are",
					"unmatched. Some",
					"theorize that their",
					"magical items hold",
					"the key to their",
					"lost history."
				),
				new BookPageInfo
				(
					"In recent times,",
					"adventurers and",
					"explorers have",
					"encountered Ophidians",
					"in lost ruins and",
					"dark caves. These",
					"encounters often",
					"end in conflict."
				),
				new BookPageInfo
				(
					"Yet, some brave",
					"souls have managed",
					"to establish a",
					"fragile peace,",
					"opening up avenues",
					"for trade and",
					"cultural exchange.",
					"These interactions"
				),
				new BookPageInfo
				(
					"have allowed us to",
					"gain insights into",
					"their complex",
					"society. Although",
					"much remains a",
					"mystery, it is clear",
					"that the Ophidians",
					"are a race with a"
				),
				new BookPageInfo
				(
					"rich and nuanced",
					"history, deserving",
					"of both respect and",
					"caution."
				)

            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public HistoryOfTheOphidians() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "History of the Ophidians" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "History of the Ophidians" );
        }

        public HistoryOfTheOphidians( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.WriteEncodedInt( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadEncodedInt();
        }
    }
}
