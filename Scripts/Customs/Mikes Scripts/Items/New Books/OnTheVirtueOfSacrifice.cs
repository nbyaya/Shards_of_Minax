using System;
using Server;

namespace Server.Items
{
	public class OnTheVirtueOfSacrifice : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"On the Virtue of Sacrifice", "Sage Eryndor",
				new BookPageInfo
				(
					"In the realm of",
					"virtues, sacrifice",
					"stands as a pinnacle",
					"of moral fortitude.",
					"Many have wondered",
					"what sacrifice",
					"entails and how one",
					"can embody it."
				),
				new BookPageInfo
				(
					"The mantra of this",
					"virtue is CAH ZEN FOD.",
					"Each component",
					"represents the facets",
					"of sacrifice:",
					"Compassion, Altruism,",
					"Honor."
				),
				new BookPageInfo
				(
					"To truly embrace",
					"sacrifice, one must",
					"be willing to give",
					"without the expectation",
					"of receiving. Whether",
					"it is wealth, time, or",
					"even one's own safety,",
					"the true act lies"
				),
				new BookPageInfo
				(
					"in the giving.",
					"",
					"Sacrifice builds",
					"communities, defeats",
					"evil, and enriches the",
					"soul. It serves as the",
					"cornerstone of",
					"virtuous living."
				),
				new BookPageInfo
				(
					"To master this",
					"virtue, one must",
					"practice it daily,",
					"imbue it in every",
					"action and thought.",
					"The mantra CAH ZEN",
					"FOD will guide your",
					"spirit."
				),
				new BookPageInfo
				(
					"Embrace the",
					"virtue of sacrifice",
					"and be uplifted, for",
					"it is the path to",
					"enlightenment and",
					"ultimate peace."
				),
				new BookPageInfo
				(
					"Sacrifice and Compassion",
					"Compassion serves as",
					"the starting point for",
					"all acts of sacrifice.",
					"It is the seed from",
					"which selfless acts",
					"grow, urging us to",
					"help those in need."
				),
				new BookPageInfo
				(
					"Sacrifice and Altruism",
					"Altruism is the path",
					"that makes sacrifice",
					"possible. It asks us to",
					"put others before",
					"ourselves, fostering a",
					"sense of community",
					"and mutual support."
				),
				new BookPageInfo
				(
					"Sacrifice and Honor",
					"Honor ties sacrifice",
					"to the moral fabric of",
					"our society. It assures",
					"that our sacrifices",
					"are made for just",
					"causes and are",
					"recognized as such."
				),
				new BookPageInfo
				(
					"Meditation on CAH",
					"CAH invites us to",
					"reflect on compassion,",
					"allowing us to understand",
					"the emotional and",
					"physical suffering of",
					"others."
				),
				new BookPageInfo
				(
					"Meditation on ZEN",
					"ZEN focuses our minds",
					"on altruism, pushing us",
					"to act in a way that",
					"benefits the greater good",
					"over individual gain."
				),
				new BookPageInfo
				(
					"Meditation on FOD",
					"FOD resonates with",
					"honor, reminding us to",
					"make sacrifices that",
					"align with the ethical",
					"and moral values we",
					"hold dear."
				),
				new BookPageInfo
				(
					"The Practice of Mantra",
					"Repeating the mantra",
					"CAH ZEN FOD in your",
					"daily meditation will",
					"bring you closer to",
					"mastering the virtue of",
					"sacrifice."
				),
				new BookPageInfo
				(
					"Closing Thoughts",
					"Embrace the teachings",
					"of this tome and",
					"embark on a lifelong",
					"journey of sacrifice.",
					"May CAH ZEN FOD guide",
					"you always."
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public OnTheVirtueOfSacrifice() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "On the Virtue of Sacrifice" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "On the Virtue of Sacrifice" );
		}

		public OnTheVirtueOfSacrifice( Serial serial ) : base( serial )
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
