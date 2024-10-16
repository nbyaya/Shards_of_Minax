using System;
using Server;

namespace Server.Items
{
    public class TheArtOfWarMagic : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Art of War Magic", "Mageus Maximus",
                new BookPageInfo
                (
                    "War Magic is an",
                    "art that combines",
                    "the elements with",
                    "strategy, creating",
                    "an unbeatable",
                    "force on the",
                    "battlefield.",
                    "         -Mageus Maximus"
                ),
                new BookPageInfo
                (
                    "1. Elemental",
                    "Dominance",
                    "Master the four",
                    "elements to gain",
                    "superiority over",
                    "your enemies.",
                    "Fire, Water, Earth,",
                    "and Air."
                ),
                new BookPageInfo
                (
                    "2. Timing and",
                    "Positioning",
                    "The key to effective",
                    "casting lies in the",
                    "timing and positioning",
                    "of spells during",
                    "combat.",
                    ""
                ),
                new BookPageInfo
                (
                    "3. Runes and",
                    "Sigils",
                    "These magical",
                    "inscriptions can",
                    "boost the effectiveness",
                    "of your spells and",
                    "provide strategic",
                    "advantages."
                ),
                new BookPageInfo
                (
                    "4. The Ethics of",
                    "War Magic",
                    "Wield your power",
                    "wisely, as magic",
                    "can be as much a",
                    "curse as it is a",
                    "blessing.",
                    ""
                ),
                new BookPageInfo
                (
                    "By mastering these",
                    "concepts, a War",
                    "Mage can be a",
                    "formidable asset",
                    "to any army,",
                    "changing the tide",
                    "of battle in",
                    "unforeseen ways."
                ),
				new BookPageInfo
				(
					"5. Spell",
					"Combinations",
					"Combining spells in",
					"quick succession can",
					"result in devastating",
					"effects. It's important",
					"to experiment and find",
					"synergies."
				),
				new BookPageInfo
				(
					"6. Defensive",
					"Strategies",
					"Using spells like",
					"shield and sanctuary",
					"at the right time can",
					"prevent fatal blows and",
					"turn the tide."
				),
				new BookPageInfo
				(
					"7. Resource",
					"Management",
					"Mana is your most",
					"valuable resource.",
					"Learn to manage it",
					"effectively to sustain",
					"your casting."
				),
				new BookPageInfo
				(
					"8. Dueling",
					"Against Mages",
					"Fighting another mage",
					"is a chess game.",
					"Anticipate their moves",
					"and counter effectively."
				),
				new BookPageInfo
				(
					"9. Elemental",
					"Resistances",
					"Adapt your spell",
					"choices according to",
					"enemy resistances.",
					"Always have a diverse",
					"set of spells."
				),
				new BookPageInfo
				(
					"10. War Magic",
					"and Morality",
					"Remember, magic is a",
					"tool. Its use in war",
					"brings ethical dilemmas",
					"that each mage must",
					"consider."
				),
				new BookPageInfo
				(
					"11. Mastery and",
					"Limitations",
					"Even master War Mages",
					"have limits. Know yours",
					"and aim to expand them.",
					"Practice makes perfect."
				),
				new BookPageInfo
				(
					"12. A Life-long",
					"Journey",
					"War magic is a path",
					"with no end. There's",
					"always something new",
					"to learn and master."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheArtOfWarMagic() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "The Art of War Magic" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "The Art of War Magic" );
        }

        public TheArtOfWarMagic( Serial serial ) : base( serial )
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
