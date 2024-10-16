using System;
using Server;

namespace Server.Items
{
    public class OgreLordsAndHierarchy : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Ogre Lords and the Hierarchy of Ogres", "A Wise Scholar",
                new BookPageInfo
                (
                    "This tome serves to",
                    "educate the reader on",
                    "the complex social",
                    "structure of Ogres",
                    "and their mighty",
                    "Ogre Lords.",
                    "",
                    "          -A Wise Scholar"
                ),
                new BookPageInfo
                (
                    "Ogre Lords are the",
                    "rulers in Ogre",
                    "societies. They are",
                    "often the strongest",
                    "and the most cunning",
                    "among their kin."
                ),
                new BookPageInfo
                (
                    "Ogre societies are",
                    "often organized into",
                    "clans. Each clan is",
                    "ruled by an Ogre Lord."
                ),
                new BookPageInfo
                (
                    "Underneath the Ogre",
                    "Lords are the common",
                    "Ogres, who serve as",
                    "hunters and warriors."
                ),
                new BookPageInfo
                (
                    "Despite their brutish",
                    "appearance, Ogres",
                    "have a set of laws",
                    "and rituals, often",
                    "enforced by the Ogre",
                    "Lord himself."
                ),
				new BookPageInfo
				(
					"Ogres value strength",
					"above all else, and",
					"challenges for",
					"leadership often",
					"involve trials of",
					"combat and cunning."
				),
				new BookPageInfo
				(
					"The hierarchy is not",
					"merely about brute",
					"strength. Ogre Lords",
					"often need to prove",
					"their strategic",
					"capabilities as well."
				),
				new BookPageInfo
				(
					"Ogres have different",
					"roles within the clan,",
					"including hunters,",
					"warriors, and",
					"shamans. Each plays",
					"a vital part in the",
					"clan's survival."
				),
				new BookPageInfo
				(
					"Shamans hold a",
					"special position, as",
					"they are the spiritual",
					"guides and healers",
					"of the clan. They",
					"often serve as advisors",
					"to the Ogre Lord."
				),
				new BookPageInfo
				(
					"Female Ogres, known",
					"as Ogre Matrons, are",
					"responsible for",
					"nurturing the young",
					"and ensuring the",
					"continuation of the",
					"clan."
				),
				new BookPageInfo
				(
					"Ogre clans can be",
					"either nomadic or",
					"settled. The decision",
					"often depends on the",
					"wisdom and preference",
					"of the reigning",
					"Ogre Lord."
				),
				new BookPageInfo
				(
					"Conflicts between",
					"Ogre clans are",
					"common but often",
					"resolved through",
					"ritualistic combat",
					"rather than",
					"all-out war."
				),
				new BookPageInfo
				(
					"To outsiders, Ogre",
					"society may seem",
					"brutal and chaotic,",
					"but a closer look",
					"reveals a complex",
					"system of governance",
					"and social roles."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgreLordsAndHierarchy() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Ogre Lords and the Hierarchy of Ogres" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Ogre Lords and the Hierarchy of Ogres" );
        }

        public OgreLordsAndHierarchy( Serial serial ) : base( serial )
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
