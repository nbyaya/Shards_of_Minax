using System;
using Server;

namespace Server.Items
{
    public class TheMagicalPropertiesOfGinseng : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Magical Properties of Ginseng", "A Herbalist",
                new BookPageInfo
                (
                    "Ginseng is a root",
                    "known for its many",
                    "magical properties.",
                    "This book will cover",
                    "the usage and",
                    "applications of this",
                    "amazing herb.",
                    ""
                ),
                new BookPageInfo
                (
                    "Firstly, Ginseng is",
                    "often used in",
                    "restorative potions.",
                    "It has a unique",
                    "ability to channel",
                    "mana into its",
                    "consumer."
                ),
                new BookPageInfo
                (
                    "Secondly, Ginseng",
                    "can be used as a",
                    "ward against evil",
                    "spirits. Sprinkle its",
                    "dust around a",
                    "perimeter to keep",
                    "unwanted entities at",
                    "bay."
                ),
                new BookPageInfo
                (
                    "Lastly, consuming",
                    "Ginseng regularly",
                    "can extend one's",
                    "life. This is why it",
                    "is so highly valued",
                    "in magical circles."
                ),
				new BookPageInfo
				(
					"Ginseng in Combat:",
					"In addition to its",
					"health benefits, the",
					"herb is used by",
					"warriors to treat",
					"wounds and enhance",
					"stamina during",
					"battle."
				),
				new BookPageInfo
				(
					"Ginseng in Magic:",
					"Mages value Ginseng",
					"for its ability to",
					"amplify magical",
					"spells. A small",
					"amount added to a",
					"mana potion can",
					"greatly boost its"
				),
				new BookPageInfo
				(
					"effectiveness. It is",
					"also commonly used",
					"in enchantments and",
					"magical item",
					"creation.",
					"",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Alchemy and Ginseng:",
					"When combined with",
					"other rare herbs,",
					"Ginseng serves as a",
					"potent component in",
					"alchemy, helping to",
					"create elixirs and"
				),
				new BookPageInfo
				(
					"potions that can",
					"cure ailments that",
					"are otherwise",
					"untreatable.",
					"",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Warnings:",
					"While Ginseng is",
					"mighty, it should be",
					"used cautiously.",
					"Overuse can lead to",
					"mana exhaustion and"
				),
				new BookPageInfo
				(
					"a temporary loss of",
					"magical abilities.",
					"Always consult with",
					"an experienced",
					"herbalist or mage",
					"before using it in",
					"large quantities."
				),
				new BookPageInfo
				(
					"In Conclusion:",
					"Ginseng is a truly",
					"remarkable herb with",
					"a wide array of",
					"magical properties.",
					"Its benefits are",
					"numerous, but"
				),
				new BookPageInfo
				(
					"caution must be",
					"exercised to use it",
					"wisely and",
					"effectively.",
					"",
					"",
					"          - A Herbalist"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheMagicalPropertiesOfGinseng() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "The Magical Properties of Ginseng" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "The Magical Properties of Ginseng" );
        }

        public TheMagicalPropertiesOfGinseng( Serial serial ) : base( serial )
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
