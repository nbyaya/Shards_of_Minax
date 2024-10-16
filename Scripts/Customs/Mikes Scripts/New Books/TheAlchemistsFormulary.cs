using System;
using Server;

namespace Server.Items
{
    public class TheAlchemistsFormulary : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Alchemists Formulary", "Aurelius",
                new BookPageInfo
                (
                    "This tome serves as a",
                    "comprehensive guide",
                    "to the art of alchemy,",
                    "penned with great care",
                    "and attention to detail.",
                    "",
                    "",
                    "           -Aurelius"
                ),
                new BookPageInfo
                (
                    "Elixirs and potions",
                    "are staples in the",
                    "world of magic and",
                    "healing. It is crucial",
                    "to understand the",
                    "basics before diving",
                    "into complex brews."
                ),
                new BookPageInfo
                (
                    "The following pages",
                    "will outline commonly",
                    "used reagents, mixing",
                    "techniques, and some",
                    "simple yet effective",
                    "recipes for any",
                    "aspiring alchemist."
                ),
				new BookPageInfo
				(
					"Chapter 1: Reagents",
					"The foundation of any",
					"alchemy formula lies",
					"in its reagents.",
					"Common reagents",
					"include Ginseng,",
					"Black Pearl, and",
					"Blood Moss."
				),
				new BookPageInfo
				(
					"Chapter 2: Tools",
					"Essential tools such",
					"as Mortar & Pestle",
					"are needed to grind",
					"reagents into usable",
					"forms. Other tools",
					"include mixing",
					"spoons and vials."
				),
				new BookPageInfo
				(
					"Chapter 3: Basic",
					"Elixirs",
					"Basic elixirs are easy",
					"to craft but effective.",
					"For example, a simple",
					"healing elixir can be",
					"made from Ginseng",
					"and Garlic."
				),
				new BookPageInfo
				(
					"Chapter 4: Advanced",
					"Potions",
					"Advanced potions",
					"offer potent effects",
					"but require rare",
					"reagents and precise",
					"techniques. Often,",
					"these are best left"
				),
				new BookPageInfo
				(
					"to seasoned",
					"alchemists.",
					"",
					"Chapter 5: Cautions",
					"While the art of",
					"alchemy offers many",
					"benefits, it's not",
					"without risk."
				),
				new BookPageInfo
				(
					"Incorrect ratios of",
					"reagents could result",
					"in volatile mixtures,",
					"and the misuse of",
					"potions can have",
					"unforeseen",
					"consequences."
				),
				new BookPageInfo
				(
					"Chapter 6: Ethics",
					"The power to create",
					"is also the power to",
					"destroy. It is",
					"imperative to practice",
					"alchemy responsibly.",
					"Remember, with great",
					"power comes great"
				),
				new BookPageInfo
				(
					"responsibility.",
					"",
					"",
					"The End"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheAlchemistsFormulary() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "The Alchemist's Formulary" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "The Alchemist's Formulary" );
        }

        public TheAlchemistsFormulary( Serial serial ) : base( serial )
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
