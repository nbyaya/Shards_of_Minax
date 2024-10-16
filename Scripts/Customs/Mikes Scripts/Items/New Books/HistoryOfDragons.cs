using System;
using Server;

namespace Server.Items
{
	public class HistoryOfDragons : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"History of the Dragons", "Erandur",
				new BookPageInfo
				(
					"In the beginning,",
					"dragons were revered",
					"as gods. Majestic",
					"and powerful, they",
					"ruled the skies.",
					"",
					"        -Erandur"
				),
				new BookPageInfo
				(
					"As time passed, man",
					"grew jealous of",
					"dragons and their",
					"power. Attempts were",
					"made to slay them",
					"and seize their",
					"treasures.",
					""
				),
				new BookPageInfo
				(
					"Despite their strength,",
					"dragons were hunted",
					"to near extinction.",
					"Those that remain",
					"today are reclusive,",
					"guarded, and far less",
					"trusting of humans."
				),
				new BookPageInfo
				(
					"Some dragons are known",
					"to still forge alliances",
					"with worthy humans.",
					"These rare individuals",
					"are known as Dragon",
					"Riders.",
					"",
					"End."
				),
				new BookPageInfo
				(
					"Dragons are classified",
					"into various types,",
					"each with unique",
					"abilities. The Fire",
					"Dragons, for example,",
					"breathe flames and",
					"reside in volcanic",
					"regions."
				),
				new BookPageInfo
				(
					"Ice Dragons, as their",
					"name suggests, dwell",
					"in frigid zones. They",
					"breathe frost, capable",
					"of freezing foes solid.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Earth Dragons are",
					"known for their",
					"ability to move",
					"through soil as if",
					"it were water. They",
					"are found in deep",
					"caves and forests."
				),
				new BookPageInfo
				(
					"Then there are the",
					"rare Storm Dragons.",
					"They control the",
					"elements of wind",
					"and lightning, and",
					"are often seen only",
					"during tempests."
				),
				new BookPageInfo
				(
					"Dragons and magic",
					"are deeply linked.",
					"Many dragons are",
					"capable of casting",
					"spells, and their",
					"scales are often",
					"used in magical",
					"rituals."
				),
				new BookPageInfo
				(
					"The relationship",
					"between dragons",
					"and humans has",
					"been complicated.",
					"While some see",
					"dragons as threats,",
					"others revere them."
				),
				new BookPageInfo
				(
					"Dragons have their",
					"own language, known",
					"as Draconic. It is a",
					"complex tongue, with",
					"intricate grammar",
					"and vocabulary.",
					"",
					""
				),
				new BookPageInfo
				(
					"Today, dragons are",
					"the stuff of legend.",
					"Yet, those who have",
					"actually encountered",
					"these magnificent",
					"creatures know that",
					"they are very real."
				),
				new BookPageInfo
				(
					"Dragons are deeply",
					"spiritual beings,",
					"with a strong sense",
					"of morality and",
					"justice, contrary to",
					"popular beliefs."
				),
				new BookPageInfo
				(
					"This book is but a",
					"short introduction",
					"to the vast and",
					"intriguing world",
					"of dragons. May it",
					"inspire you to learn",
					"more.",
					"       -The End-"
				)

			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public HistoryOfDragons() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "History of the Dragons" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "History of the Dragons" );
		}

		public HistoryOfDragons( Serial serial ) : base( serial )
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
