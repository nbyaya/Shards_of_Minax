using System;
using Server;

namespace Server.Items
{
    public class SpeculationsOnWisps : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Speculations on Wisps", "Eldric",
            new BookPageInfo
            (
                "This tome seeks to",
                "explore the enigmatic",
                "nature of Wisps, the",
                "luminous entities",
                "often found in dark",
                "forests and ancient",
                "ruins.",
                "",
                "          -Eldric"
            ),
            new BookPageInfo
            (
                "Wisps have puzzled",
                "scholars for centuries.",
                "Are they sentient?",
                "What is the source",
                "of their luminosity?",
                "This book aims to",
                "discuss such topics."
            ),
            new BookPageInfo
            (
                "Some argue that",
                "Wisps are fragments",
                "of pure mana, while",
                "others see them as",
                "messengers from the",
                "gods. Nonetheless, no",
                "one can deny their",
                "mysterious allure."
            ),
            new BookPageInfo
            (
                "There are accounts",
                "of adventurers",
                "following Wisps to",
                "hidden treasures, but",
                "also to their doom.",
                "So, one must ask,",
                "are they friends or",
                "foes?"
            ),
			new BookPageInfo
			(
				"Wisps and Magic:",
				"Many mages have",
				"attempted to study",
				"Wisps in hopes of",
				"understanding their",
				"magical properties.",
				"Few have succeeded."
			),
			new BookPageInfo
			(
				"One account tells of",
				"a mage who tried to",
				"capture a Wisp. The",
				"Wisp reacted by",
				"unleashing a burst",
				"of energy, destroying",
				"the mage's laboratory."
			),
			new BookPageInfo
			(
				"Wisps and the Gods:",
				"Certain religious",
				"texts mention Wisps",
				"as messengers or",
				"servants of gods.",
				"Their luminescence",
				"is said to be a divine"
			),
			new BookPageInfo
			(
				"trait, an ethereal",
				"glow bestowed upon",
				"them to guide lost",
				"souls or to lead the",
				"worthy to realms of",
				"enlightenment."
			),
			new BookPageInfo
			(
				"Wisps and the Spirit:",
				"There is also a",
				"theory that Wisps are",
				"spirits of the dead,",
				"bound to the world,",
				"unable to move on."
			),
			new BookPageInfo
			(
				"These theories",
				"sometimes overlap,",
				"suggesting Wisps are",
				"spirits employed by",
				"the gods, a thought",
				"that adds more to",
				"their enigmatic role."
			),
			new BookPageInfo
			(
				"In Conclusion:",
				"The Wisp remains an",
				"enigma. Whether it is",
				"a magical entity, a",
				"divine servant, or",
				"a restless spirit,"
			),
			new BookPageInfo
			(
				"is yet to be",
				"determined. What is",
				"certain is that their",
				"mystery continues to",
				"captivate all who",
				"encounter them."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SpeculationsOnWisps() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Speculations on Wisps" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Speculations on Wisps" );
        }

        public SpeculationsOnWisps( Serial serial ) : base( serial )
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
