using System;
using Server;

namespace Server.Items
{
    public class OrcishBlacksmithingBook : BlueBook  // You can change BlueBook to any other base class you'd use for books.
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Blacksmithing", "Gruk the Smith",
                new BookPageInfo
                (
                    "In the heart of the",
                    "orcish stronghold, we",
                    "craft weapons and",
                    "armor unlike any",
                    "made by human hands.",
                    "This tome serves as",
                    "a guide for aspiring",
                    "orcish smiths."
                ),
                new BookPageInfo
                (
                    "Tools of the Trade:",
                    "1. Smelting Furnace",
                    "2. Anvil & Hammer",
                    "3. Tongs",
                    "4. Water Bucket",
                    "",
                    "Ensure all tools are",
                    "kept in top condition."
                ),
                new BookPageInfo
                (
                    "Metals Commonly Used:",
                    "1. Iron",
                    "2. Obsidian",
                    "3. Bone",
                    "",
                    "Each metal has its",
                    "own unique properties."
                ),
                new BookPageInfo
                (
                    "Techniques:",
                    "1. Folding",
                    "2. Tempering",
                    "3. Enchanting",
                    "",
                    "Mastery over these",
                    "is essential."
                ),
                new BookPageInfo
                (
                    "Always remember,",
                    "the weapon is an",
                    "extension of oneself.",
                    "It must be crafted",
                    "with care, precision,",
                    "and a strong will."
                ),
				new BookPageInfo
				(
					"Advanced Metals:",
					"1. Dragon's Blood",
					"2. Ebonite",
					"3. Runed Silver",
					"",
					"Handle these materials",
					"with extreme caution."
				),
				new BookPageInfo
				(
					"Common Mistakes:",
					"1. Overheating Metal",
					"2. Uneven Hammering",
					"3. Poor Quenching",
					"",
					"Avoid these errors at",
					"all costs."
				),
				new BookPageInfo
				(
					"Enchantments:",
					"1. Fire Infusion",
					"2. Ice Coating",
					"3. Lightning Edge",
					"",
					"Elemental enchantments",
					"require special rituals."
				),
				new BookPageInfo
				(
					"Tales of Gruk:",
					"Once, Gruk the Smith",
					"forged a blade so fine,",
					"it could cut through",
					"stone as if it were",
					"air. The secret, he",
					"claimed, lay not just",
					"in his skill, but also"
				),
				new BookPageInfo
				(
					"in the ancient rites",
					"and chants passed down",
					"from his ancestors.",
					"",
					"It is believed that",
					"this blade still exists,",
					"held in the deepest",
					"vault of the clan."
				),
				new BookPageInfo
				(
					"Crafts of Note:",
					"1. Gruumsh's Helm",
					"2. Axe of the Horde",
					"3. Boneplate Armor",
					"",
					"These artifacts are",
					"cherished heirlooms."
				),
				new BookPageInfo
				(
					"Closing Thoughts:",
					"May your hammer be",
					"steady, your anvil",
					"unyielding, and your",
					"furnace forever hot.",
					"Go forth and let your",
					"crafts scream the",
					"pride of the Orcs!"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishBlacksmithingBook() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Orcish Blacksmithing Book" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Orcish Blacksmithing Book" );
        }

        public OrcishBlacksmithingBook( Serial serial ) : base( serial )
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
