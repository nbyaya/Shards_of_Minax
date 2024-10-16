using System;
using Server;

namespace Server.Items
{
    public class TheNeedForOrderInBritannia : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Need for Order", "Lord British",
                new BookPageInfo
                (
                    "The lands of",
                    "Britannia have long",
                    "been plagued by",
                    "chaos and unrest.",
                    "It is the solemn duty",
                    "of each citizen to",
                    "strive for order,",
                    "justice, and unity."
                ),
                new BookPageInfo
                (
                    "We must stand",
                    "together against the",
                    "forces that threaten",
                    "to tear us asunder.",
                    "Order is not merely",
                    "the absence of chaos,",
                    "but the presence of",
                    "harmony."
                ),
                new BookPageInfo
                (
                    "The Principles of",
                    "Truth, Love, and",
                    "Courage guide us",
                    "toward a future of",
                    "prosperity and",
                    "peace. Let us not",
                    "deviate from this",
                    "path."
                ),
                new BookPageInfo
                (
                    "Only through",
                    "structured governance",
                    "and a unified populace",
                    "can Britannia hope to",
                    "stave off the evils",
                    "that lurk in the",
                    "shadows.",
                    "Stay vigilant."
                ),
				new BookPageInfo
				(
					"To neglect the",
					"Principles is to invite",
					"discord into our lives.",
					"Britannia can ill-afford",
					"such negligence,",
					"especially when dark",
					"forces conspire to",
					"bring us low."
				),
				new BookPageInfo
				(
					"The Virtues arise",
					"from the Principles,",
					"guiding us in our",
					"daily interactions and",
					"choices. Adherence to",
					"the Virtues ensures a",
					"stable society,",
					"resilient against chaos."
				),
				new BookPageInfo
				(
					"Honor in dealings,",
					"Valor in combat, and",
					"Compassion in all",
					"things. These virtues",
					"are the bedrock upon",
					"which Britannia must",
					"stand, now more than",
					"ever."
				),
				new BookPageInfo
				(
					"I, Lord British,",
					"entreat each and",
					"every citizen to rise",
					"to this occasion.",
					"Embrace the values",
					"that make us great,",
					"and reject the siren",
					"call of anarchy."
				),
				new BookPageInfo
				(
					"Remember, order is",
					"not imposed; it is",
					"built through the",
					"collective efforts of",
					"the people. Let this",
					"book serve as a",
					"reminder and guide",
					"to all."
				),
				new BookPageInfo
				(
					"Strive always for",
					"unity, in thought,",
					"word, and deed.",
					"Only when we stand",
					"together can we",
					"hope to face the",
					"challenges that lie",
					"ahead."
				),
				new BookPageInfo
				(
					"And so, let the",
					"Principles guide you,",
					"let the Virtues be",
					"your shield, and let",
					"order be the goal for",
					"which we strive,",
					"individually and",
					"together."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheNeedForOrderInBritannia() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "The Need for Order in Britannia" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "The Need for Order in Britannia" );
        }

        public TheNeedForOrderInBritannia( Serial serial ) : base( serial )
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
