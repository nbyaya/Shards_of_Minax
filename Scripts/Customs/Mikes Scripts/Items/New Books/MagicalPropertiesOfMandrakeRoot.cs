using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfMandrakeRoot : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Magical Properties", "Elenor Sage",
                new BookPageInfo
                (
                    "Mandrake Root has been",
                    "a cornerstone in the",
                    "realm of magical herbs",
                    "for centuries. Known",
                    "for its mystical",
                    "properties, it is a key",
                    "ingredient in various",
                    "potions and spells."
                ),
                new BookPageInfo
                (
                    "Its roots have a",
                    "peculiar shape that",
                    "resembles the human",
                    "form. This has led",
                    "many to believe that",
                    "the herb possesses",
                    "life-enhancing powers.",
                    "It is a common"
                ),
                new BookPageInfo
                (
                    "ingredient in potions",
                    "of greater healing and",
                    "invisibility, but its",
                    "uses are vast and",
                    "varied, ranging from",
                    "love spells to",
                    "necromantic rituals.",
                    "Handle with care."
                ),
                new BookPageInfo
                (
                    "To harvest, it is",
                    "essential to pull it",
                    "from the ground under",
                    "the full moon to",
                    "capture its full",
                    "potency. Use a silver",
                    "dagger for the best",
                    "results."
                ),
                new BookPageInfo
                (
                    "Store in a dark,",
                    "cool place and use",
                    "within a moon's",
                    "cycle for best",
                    "results."
                ),
				new BookPageInfo
				(
					"Mandrake Root in",
					"Alchemy:",
					"Mandrake is often",
					"used in the crafting",
					"of powerful elixirs.",
					"Its essence can",
					"intensify the effects",
					"of other ingredients."
				),
				new BookPageInfo
				(
					"Mandrake in",
					"Rituals:",
					"Often used to",
					"enhance spells of",
					"protection and luck.",
					"Its mystical aura",
					"serves to amplify",
					"the spell's energy."
				),
				new BookPageInfo
				(
					"Medical Uses:",
					"While primarily a",
					"magical component,",
					"it also has some",
					"beneficial medical",
					"uses, like treating",
					"insomnia and pain."
				),
				new BookPageInfo
				(
					"Warnings:",
					"Consuming Mandrake",
					"in large quantities",
					"can lead to severe",
					"side effects,",
					"including dizziness,",
					"and even death."
				),
				new BookPageInfo
				(
					"Legal Status:",
					"Due to its powerful",
					"properties, Mandrake",
					"is often regulated.",
					"Unauthorized sale or",
					"use may result in",
					"criminal charges."
				),
				new BookPageInfo
				(
					"Cultivation:",
					"Growing Mandrake",
					"requires a dark,",
					"moist soil and should",
					"be harvested only",
					"during a full moon",
					"to ensure potency."
				),
				new BookPageInfo
				(
					"Conclusion:",
					"With proper handling",
					"and respect for its",
					"potent abilities,",
					"Mandrake Root can be",
					"a beneficial asset in",
					"varied applications."
				)

            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public MagicalPropertiesOfMandrakeRoot() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "The Magical Properties of Mandrake Root" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "The Magical Properties of Mandrake Root" );
        }

        public MagicalPropertiesOfMandrakeRoot( Serial serial ) : base( serial )
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
