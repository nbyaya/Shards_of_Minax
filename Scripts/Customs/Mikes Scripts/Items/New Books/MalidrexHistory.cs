using System;
using Server;

namespace Server.Items
{
	public class MalidrexHistory : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"The Secret History of Malidrex", "Auris",
				new BookPageInfo
				(
					"Malidrex, a name",
					"feared by many, is a",
					"witch of legendary",
					"power. However, her",
					"origin remains a",
					"mystery.",
					"",
					"       - Auris"
				),
				new BookPageInfo
				(
					"Born in a humble",
					"village, Malidrex",
					"showed an affinity",
					"for magic at a young",
					"age. Unlike the",
					"typical mage, her",
					"power drew from",
					"the abyss."
				),
				new BookPageInfo
				(
					"Shunned by her",
					"community, she",
					"embraced her gift",
					"and ventured deep",
					"into the woods.",
					"It's said she made",
					"a pact with a",
					"shadowy entity."
				),
				new BookPageInfo
				(
					"With newfound",
					"strength, Malidrex",
					"returned to the",
					"world. Her spells",
					"now warped reality",
					"itself, leaving no",
					"doubt about her",
					"unearthly alliance."
				),
				new BookPageInfo
				(
					"However, the pact",
					"came at a cost.",
					"The shadowy entity",
					"demanded sacrifices,",
					"and Malidrex complied.",
					"Hence, the lands",
					"grew darker as",
					"she gained power."
				),
				new BookPageInfo
				(
					"To this day, no one",
					"knows where she",
					"resides. Attempts to",
					"stop her have ended",
					"in doom. One thing",
					"is certain: her story",
					"is far from over.",
					"The end is unclear."
				)
			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public MalidrexHistory() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "The Secret History of Malidrex the Witch" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "The Secret History of Malidrex the Witch" );
		}

		public MalidrexHistory( Serial serial ) : base( serial )
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
