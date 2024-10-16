using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheCyclopes : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Cyclopes's", "Unknown Author",
                new BookPageInfo
                (
                    "The Cyclopes are an",
                    "ancient race, known",
                    "for their singular eye",
                    "and immense strength.",
                    "",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Originating from the",
                    "Islands of the Great",
                    "Sea, they have long",
                    "been a topic of myth",
                    "and legend.",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Often mistaken as",
                    "mindless brutes, the",
                    "Cyclopes are a deeply",
                    "cultural and spiritual",
                    "people.",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "They are skilled in",
                    "smithing, and it is",
                    "said that they once",
                    "forged weapons for",
                    "the gods themselves.",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "The Cyclopes have",
                    "been in conflict",
                    "with humans, elves",
                    "and other races for",
                    "centuries.",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "This book aims to",
                    "unveil the truth",
                    "behind these",
                    "misunderstood",
                    "creatures.",
                    "",
                    "",
                    ""
                ),
				new BookPageInfo
				(
					"Early Mythology",
					"states that the",
					"Cyclopes were born",
					"to Gaia and Uranus,",
					"the Earth and Sky.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"They were initially",
					"imprisoned in Tartarus",
					"but were later freed",
					"and contributed to",
					"the Titanomachy.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"In most myths, they",
					"are credited with",
					"forging Zeus's",
					"thunderbolts, Poseidon's",
					"trident, and Hades'",
					"helm of invisibility.",
					"",
					""
				),
				new BookPageInfo
				(
					"Over time, Cyclopes",
					"culture evolved and",
					"moved away from",
					"mythology into",
					"recorded history.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Despite their fearsome",
					"appearance, Cyclopes",
					"have a code of ethics",
					"that values honor,",
					"strength, and wisdom.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Some Cyclopes became",
					"renowned architects",
					"and craftsmen, their",
					"work still admired",
					"today.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Many ancient ruins",
					"are attributed to",
					"Cyclopes architecture,",
					"often identified by",
					"massive stone blocks.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"The Cyclopes are not",
					"without enemies. Many",
					"races see them as",
					"threats to be",
					"eradicated.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Recent years have",
					"seen efforts to",
					"bridge the gap",
					"between Cyclopes",
					"and other races.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Despite the",
					"challenges, there is",
					"hope that the",
					"Cyclopes will find",
					"their rightful place",
					"in the world.",
					"",
					"",
					""
				)

            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public HistoryOfTheCyclopes() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "History of the Cyclopes's" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "History of the Cyclopes's" );
        }

        public HistoryOfTheCyclopes( Serial serial ) : base( serial )
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
