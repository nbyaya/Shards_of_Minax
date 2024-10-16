using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheRatmen : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Ratmen", "Scribus Rattus",
                new BookPageInfo
                (
                    "This tome aims to",
                    "uncover the complex",
                    "history of the Ratmen,",
                    "from their humble",
                    "beginnings to their",
                    "society today.",
                    "",
                    "        -Scribus Rattus"
                ),
                new BookPageInfo
                (
                    "Once considered mere",
                    "vermin, the Ratmen",
                    "have evolved into a",
                    "race with its own",
                    "culture and",
                    "technology. They are",
                    "now a force to be",
                    "reckoned with."
                ),
                new BookPageInfo
                (
                    "Many believe that",
                    "the Ratmen were",
                    "once common rats,",
                    "mutated by magic or",
                    "alchemy. Others say",
                    "they have a more",
                    "mysterious origin.",
                    "What is clear is"
                ),
                new BookPageInfo
                (
                    "their resilience and",
                    "ability to adapt have",
                    "allowed them to",
                    "thrive in environments",
                    "from sewers to",
                    "abandoned ruins.",
                    "They are master",
                    "scavengers and"
                ),
                new BookPageInfo
                (
                    "resourceful engineers.",
                    "Ratmen communities",
                    "are usually governed",
                    "by the strongest or",
                    "the most cunning,",
                    "and they have even",
                    "formed alliances",
                    "with other races."
                ),
				new BookPageInfo
				(
					"Their written language",
					"is a series of complex",
					"symbols that differ",
					"greatly from our own.",
					"Few have been able to",
					"decipher the scripts",
					"that are found in their",
					"dwellings."
				),
				new BookPageInfo
				(
					"Despite their menacing",
					"appearance and often",
					"violent interactions",
					"with other species,",
					"Ratmen are known to",
					"have a strict code of",
					"ethics within their own",
					"communities."
				),
				new BookPageInfo
				(
					"Family bonds are",
					"particularly strong",
					"among Ratmen, and",
					"elder members are",
					"usually revered.",
					"Disrespect towards",
					"an elder is one of",
					"their greatest taboos."
				),
				new BookPageInfo
				(
					"Ratmen warriors train",
					"from a young age and",
					"are quite skilled with",
					"rudimentary weapons.",
					"Some even display",
					"talents in the magical",
					"arts, although this is",
					"less common."
				),
				new BookPageInfo
				(
					"Food is a communal",
					"affair for Ratmen.",
					"They gather resources",
					"and share among the",
					"entire colony. Stealing",
					"food within the colony",
					"is grounds for exile or",
					"worse."
				),
				new BookPageInfo
				(
					"Their religion is not",
					"well understood, but",
					"there are shrines",
					"dedicated to some sort",
					"of rat deity within",
					"their larger colonies.",
					"Offerings of food and",
					"shiny objects are common."
				),
				new BookPageInfo
				(
					"It is not recommended",
					"to engage in hostilities",
					"with Ratmen without",
					"proper preparation.",
					"They often set traps",
					"and ambush their",
					"enemies when provoked."
				),
				new BookPageInfo
				(
					"Though often considered",
					"nuisances or pests by",
					"most civilizations, the",
					"Ratmen have managed",
					"to carve out an",
					"existence that is as",
					"complex as it is",
					"misunderstood."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheRatmen() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "History of the Ratmen" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "History of the Ratmen" );
        }

        public HistoryOfTheRatmen( Serial serial ) : base( serial )
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
