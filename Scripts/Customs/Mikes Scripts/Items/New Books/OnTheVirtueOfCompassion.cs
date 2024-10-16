using System;
using Server;

namespace Server.Items
{
	public class OnTheVirtueOfCompassion : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"On the Virtue of Compassion", "AuthorName",
				new BookPageInfo
				(
					"Compassion is a",
					"virtue that stands",
					"as a pillar in the",
					"moral foundation",
					"of Britannia. It is",
					"the act of",
					"understanding and",
					"helping others."
				),
				new BookPageInfo
				(
					"The mantra of this",
					"virtue is 'NIX RAY",
					"MUH', a sacred",
					"incantation used to",
					"focus the mind and",
					"heart on acts of",
					"kindness and",
					"empathy."
				),
				new BookPageInfo
				(
					"Practicing compassion",
					"enables individuals",
					"to form meaningful",
					"connections and",
					"ultimately build a",
					"better society. The",
					"importance of this",
					"virtue cannot be"
				),
				new BookPageInfo
				(
					"understated, for it",
					"is in compassion",
					"that we find our",
					"true selves, and",
					"realize the",
					"interconnectedness",
					"of all beings.",
					""
				),
				new BookPageInfo
				(
					"Compassion starts",
					"small, often as a",
					"simple act of",
					"kindness. Whether",
					"it be helping a",
					"stranger in need",
					"or lending an ear",
					"to a troubled friend"
				),
				new BookPageInfo
				(
					"these acts accumulate",
					"to form a character",
					"imbued with love",
					"and understanding.",
					"The more one",
					"practices compassion,",
					"the more natural it",
					"becomes."
				),
				new BookPageInfo
				(
					"Compassion also",
					"extends to all",
					"creatures, not just",
					"humans. Even the",
					"lowliest of animals",
					"can benefit from a",
					"kind hand and",
					"a gentle heart."
				),
				new BookPageInfo
				(
					"Yet, practicing",
					"compassion is not",
					"always easy. It is",
					"often tested in",
					"times of difficulty",
					"and conflict. It is",
					"in these moments",
					"that its strength"
				),
				new BookPageInfo
				(
					"is truly revealed.",
					"Facing adversity",
					"with compassion",
					"instead of anger",
					"or fear is a",
					"difficult but",
					"rewarding choice.",
					"By maintaining"
				),
				new BookPageInfo
				(
					"this virtue, one can",
					"find peace and",
					"even inspire others",
					"to follow in the",
					"same path.",
					"",
					"In conclusion, the",
					"mantra 'NIX RAY MUH'"
				),
				new BookPageInfo
				(
					"should not just be",
					"recited, but lived.",
					"Let the essence of",
					"the words fill your",
					"daily actions, and",
					"in doing so, make",
					"the realm of",
					"Britannia a better"
				),
				new BookPageInfo
				(
					"place for all.",
					"",
					"May compassion",
					"light your path and",
					"warm your soul.",
					"",
					"The end."
				)

			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public OnTheVirtueOfCompassion() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "On the Virtue of Compassion" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "On the Virtue of Compassion" );
		}

		public OnTheVirtueOfCompassion( Serial serial ) : base( serial )
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
