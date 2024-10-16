using System;
using Server;

namespace Server.Items
{
	public class MagicalPropertiesOfNightshade : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"The Magical Properties of Nightshade", "Morgana",
				new BookPageInfo
				(
					"Nightshade, a mysterious",
					"and often misunderstood",
					"herb, has long been",
					"associated with the art",
					"of magic and alchemy.",
					"",
					"",
					"         -Morgana"
				),
				new BookPageInfo
				(
					"This book aims to shed",
					"light on the enigmatic",
					"qualities of Nightshade",
					"and its role in various",
					"magical practices.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"Firstly, Nightshade is",
					"a crucial ingredient",
					"in various potions and",
					"elixirs. It's potent",
					"properties can be used",
					"to enhance many spells",
					"and rituals."
				),
				new BookPageInfo
				(
					"However, one must be",
					"cautious when handling",
					"Nightshade. Incorrect",
					"usage can lead to",
					"unintended effects,",
					"ranging from minor",
					"ailments to serious",
					"conditions."
				),
				new BookPageInfo
				(
					"In the hands of a",
					"skilled mage or alchemist,",
					"Nightshade can unlock",
					"powerful abilities and",
					"provide unparalleled",
					"advantages in battle.",
					"",
					""
				),
				new BookPageInfo
				(
					"To sum it up, Nightshade",
					"is a versatile and potent",
					"herb. Its correct usage",
					"can be a game-changer",
					"in the realms of",
					"magic and alchemy."
				),
				new BookPageInfo
				(
					"The Magical Origins",
					"of Nightshade:",
					"",
					"Nightshade is believed",
					"to have originated in",
					"the mysterious Shadow",
					"Realm. Its magical",
					"properties were",
					"discovered by ancient",
					"sorcerers."
				),
				new BookPageInfo
				(
					"Alchemy and Nightshade:",
					"",
					"Alchemists find this",
					"herb especially useful",
					"in brewing mana and",
					"health restoration",
					"potions. Its essence",
					"can also neutralize",
					"poisons."
				),
				new BookPageInfo
				(
					"Elemental Magic:",
					"",
					"Nightshade has an",
					"affinity with elemental",
					"forces. It can amplify",
					"spells related to",
					"fire, water, earth,",
					"and air."
				),
				new BookPageInfo
				(
					"Dark Arts:",
					"",
					"Practitioners of the",
					"dark arts also use",
					"Nightshade, albeit",
					"for nefarious purposes.",
					"It's a key ingredient",
					"in curses and hexes."
				),
				new BookPageInfo
				(
					"The Dangers:",
					"",
					"Overuse or misuse of",
					"Nightshade can lead to",
					"magical corruption.",
					"Always use it",
					"responsibly and under",
					"expert guidance."
				),
				new BookPageInfo
				(
					"Harvesting Nightshade:",
					"",
					"This herb is commonly",
					"found in dark, damp",
					"places. Harvesting it",
					"requires a skilled hand",
					"and a knowledge of",
					"herbal lore."
				),
				new BookPageInfo
				(
					"In Closing:",
					"",
					"Understanding and",
					"respecting Nightshade's",
					"properties can bring",
					"immeasurable benefits.",
					"Ignore its power at",
					"your own peril."
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public MagicalPropertiesOfNightshade() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "The Magical Properties of Nightshade" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "The Magical Properties of Nightshade" );
		}

		public MagicalPropertiesOfNightshade( Serial serial ) : base( serial )
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
