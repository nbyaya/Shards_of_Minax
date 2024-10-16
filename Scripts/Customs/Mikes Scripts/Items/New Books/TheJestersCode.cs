using System;
using Server;

namespace Server.Items
{
	public class TheJestersCode : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"The Jester's Code", "Bardello",
				new BookPageInfo
				(
					"Laughter, they say,",
					"is the best medicine.",
					"However, for a jester,",
					"laughter is more than",
					"medicine. It is a",
					"way of life, an art",
					"and a code.",
					"          -Bardello"
				),
				new BookPageInfo
				(
					"The First Rule:",
					"Never let the",
					"audience see your",
					"sorrow. A jester",
					"wears a mask of joy",
					"to bring smiles,",
					"even when burdened",
					"with sadness."
				),
				new BookPageInfo
				(
					"The Second Rule:",
					"Versatility is key.",
					"A jester must sing,",
					"dance, and jest.",
					"He must adapt",
					"to the mood and",
					"always be prepared",
					"for the next act."
				),
				new BookPageInfo
				(
					"The Third Rule:",
					"Do not jest at the",
					"expense of others’",
					"misfortune. Humor",
					"should lift spirits,",
					"not suppress them.",
					"Be the light, not",
					"the shadow."
				),
				new BookPageInfo
				(
					"The Final Rule:",
					"Remember, the",
					"jester's code is",
					"not just for jesters.",
					"Anyone can adopt",
					"these principles",
					"to live a happier",
					"and fuller life."
				),
				new BookPageInfo
				(
					"Master these rules,",
					"and you shall be",
					"a jester beyond",
					"compare, enriching",
					"the lives of all",
					"those you encounter."
				),
				new BookPageInfo
				(
					"The Art of Jest:",
					"To truly master",
					"jesting, one must",
					"understand timing.",
					"Well-timed humor",
					"can turn foes into",
					"friends and lighten",
					"the darkest moods."
				),
				new BookPageInfo
				(
					"Instruments of Joy:",
					"A jester's tools are",
					"not just bells and",
					"whistles. They include",
					"the lute, the flute,",
					"and even the humble",
					"drum. Learn to use",
					"them all."
				),
				new BookPageInfo
				(
					"Audience:",
					"Know your audience.",
					"What brings a smile",
					"to a lord might not",
					"amuse a peasant.",
					"Be observant and",
					"tailor your acts."
				),
				new BookPageInfo
				(
					"A Jester's Duty:",
					"Never forget, the",
					"primary duty of a",
					"jester is to serve.",
					"Serve your audience,",
					"serve the truth, and",
					"above all, serve the",
					"art of jest."
				),
				new BookPageInfo
				(
					"The Heart of Jest:",
					"At the heart of all",
					"jest lies a kernel",
					"of truth. Exaggerate",
					"it, cloak it in humor,",
					"but never stray far",
					"from it."
				),
				new BookPageInfo
				(
					"Sign Off:",
					"So, there you have it.",
					"The code, the art,",
					"and the heart. May",
					"your life be full of",
					"laughter and light.",
					"          -Bardello"
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public TheJestersCode() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "The Jester's Code" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "The Jester's Code" );
		}

		public TheJestersCode( Serial serial ) : base( serial )
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
