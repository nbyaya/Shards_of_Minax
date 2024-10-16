using System;
using Server;

namespace Server.Items
{
    public class AnatomyOfSpectralUndead : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Anatomy of Spectral Undead", "Necro Scholar",
                new BookPageInfo
                (
                    "This book serves as",
                    "a guide to understand",
                    "the spectral undead,",
                    "the ethereal entities",
                    "that roam our world.",
                    "",
                    "",
                    "         -Necro Scholar"
                ),
                new BookPageInfo
                (
                    "The spectral undead",
                    "are not bound by",
                    "physical constraints.",
                    "Their ethereal nature",
                    "makes them resilient",
                    "to common weaponry."
                ),
				new BookPageInfo
				(
					"Chapter 1: Origin",
					"The spectral undead",
					"originate from a",
					"variety of sources,",
					"ranging from powerful",
					"necromantic rituals",
					"to sudden, traumatic",
					"deaths."
				),
				new BookPageInfo
				(
					"Chapter 2: Forms",
					"They can take on",
					"different forms, such",
					"as spectral knights,",
					"phantom mages, or",
					"ghostly animals.",
					"Their appearance",
					"often relates to their",
					"past lives."
				),
				new BookPageInfo
				(
					"Chapter 3: Weaknesses",
					"Despite their",
					"ethereal nature, they",
					"are susceptible to",
					"certain magics and",
					"blessed weaponry. It's",
					"imperative to be well-",
					"prepared."
				),
				new BookPageInfo
				(
					"Chapter 4: Behavior",
					"Spectral undead are",
					"often bound to a",
					"location or object.",
					"Their behaviors vary",
					"from passive to",
					"aggressively territorial."
				),
				new BookPageInfo
				(
					"Chapter 5: Engaging",
					"To engage a spectral",
					"undead, a blend of",
					"physical and magical",
					"attacks is often",
					"most effective.",
					"Resistance to magic",
					"varies."
				),
				new BookPageInfo
				(
					"Chapter 6: Capture",
					"Capturing a spectral",
					"entity is a dangerous",
					"task, usually requiring",
					"advanced magical circles",
					"and binding spells to",
					"contain them."
				),
				new BookPageInfo
				(
					"Final Thoughts",
					"Understanding the",
					"spectral undead is key",
					"to coexisting with",
					"these entities or",
					"defeating them. This",
					"knowledge is crucial",
					"for any adventurer."
				)

            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public AnatomyOfSpectralUndead() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Anatomy of Spectral Undead" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Anatomy of Spectral Undead" );
        }

        public AnatomyOfSpectralUndead( Serial serial ) : base( serial )
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
