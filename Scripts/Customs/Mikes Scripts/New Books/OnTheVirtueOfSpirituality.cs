using System;
using Server;

namespace Server.Items
{
	public class OnTheVirtueOfSpirituality : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"On the Virtue of Spirituality", "A Scribe",
				new BookPageInfo
				(
					"Spirituality is the",
					"virtue that lifts us",
					"above mere existence,",
					"guiding us to seek",
					"meaning in the world",
					"around us.",
					"",
					"- A Scribe"
				),
				new BookPageInfo
				(
					"The mantra for the",
					"virtue of Spirituality",
					"is 'OM JIX LOR'.",
					"Chanting this mantra",
					"allows the soul to",
					"connect deeply with",
					"the ethereal forces",
					"of the universe."
				),
				new BookPageInfo
				(
					"To embody this",
					"virtue is to seek",
					"wisdom, understand",
					"the interconnected-",
					"ness of all things,",
					"and to acknowledge",
					"the divine spark in",
					"each being."
				),
				new BookPageInfo
				(
					"May your path be",
					"illuminated by the",
					"light of Spiritual",
					"understanding, and",
					"may your soul find",
					"peace."
				),
				new BookPageInfo
				(
					"In times of turmoil,",
					"it is spirituality that",
					"offers solace. It",
					"creates a sanctuary",
					"within the soul,",
					"shielding us from the",
					"chaos of the outer",
					"world."
				),
				new BookPageInfo
				(
					"Through spirituality,",
					"we develop empathy,",
					"enabling us to view",
					"the struggles of",
					"others not as distant",
					"sorrows, but as",
					"shared human",
					"experiences."
				),
				new BookPageInfo
				(
					"Spirituality is not",
					"merely about faith in",
					"the divine, but also",
					"faith in humanity and",
					"in oneself. It teaches",
					"us that every action",
					"ripples through the",
					"cosmos."
				),
				new BookPageInfo
				(
					"When we chant 'OM",
					"JIX LOR', we are not",
					"just reciting words,",
					"but invoking an",
					"ancient energy. Each",
					"syllable vibrates,",
					"aligning us with the",
					"spiritual plane."
				),
				new BookPageInfo
				(
					"Whether in deep",
					"meditation or in",
					"simple acts of",
					"kindness, spirituality",
					"nourishes the soul.",
					"It serves as a beacon,",
					"guiding us through",
					"dark times."
				),
				new BookPageInfo
				(
					"Many have questioned",
					"the tangible benefits",
					"of spirituality, yet",
					"its impact is",
					"immeasurable. It",
					"transforms not just",
					"the individual, but",
					"society at large."
				),
				new BookPageInfo
				(
					"So let us embrace",
					"spirituality in our",
					"daily lives. Let us",
					"chant 'OM JIX LOR'",
					"and reach for a",
					"higher state of",
					"consciousness.",
					"Amen."
				)

			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public OnTheVirtueOfSpirituality() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "On the Virtue of Spirituality" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "On the Virtue of Spirituality" );
		}

		public OnTheVirtueOfSpirituality( Serial serial ) : base( serial )
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
