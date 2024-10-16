using System;
using Server;

namespace Server.Items
{
	public class OrcishBaking : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Orcish Baking", "Grukk",
				new BookPageInfo
				(
					"This manuscript",
					"chronicles the culinary",
					"arts of the Orcs.",
					"Contrary to popular",
					"belief, orcs have",
					"a rich gastronomic",
					"culture.",
					"          -Grukk"
				),
				new BookPageInfo
				(
					"Boar Stew:",
					"Take one boar, skin",
					"and clean it. Boil",
					"it in a large pot",
					"with wild herbs.",
					"Add troll fat for",
					"extra flavor.",
					"Cook for half a day."
				),
				new BookPageInfo
				(
					"Orcish Bread:",
					"Mix ground grains",
					"with water. Add",
					"small rocks for",
					"texture. Bake in",
					"hot coals until",
					"rock hard.",
					"Delicious!"
				),
				new BookPageInfo
				(
					"Grub Pie:",
					"Gather grubs from",
					"decayed logs. Mix",
					"with mud and place",
					"in a bark crust.",
					"Bake until the",
					"grubs are tender.",
					"A classic!"
				),
				new BookPageInfo
				(
					"Always remember,",
					"Orcish cuisine is",
					"best enjoyed with",
					"a cup of fermented",
					"mushroom brew.",
					"Eat like an orc,",
					"live like an orc!"
				),
				new BookPageInfo
				(
					"Bat Wing Soup:",
					"Take 5 bat wings",
					"and chop them",
					"into fine pieces.",
					"Boil in water",
					"with mushrooms",
					"and cave moss.",
					"Serve hot."
				),
				new BookPageInfo
				(
					"Warrior's Delight:",
					"Take the liver of",
					"a slain enemy",
					"and marinate it",
					"in beast's blood.",
					"Grill over open",
					"fire. A delicacy!",
					"Perfect for feasts."
				),
				new BookPageInfo
				(
					"Stone Bread:",
					"Crush pebbles and",
					"mix with ground",
					"grains. Add water",
					"and knead the",
					"dough. Bake till",
					"as hard as a rock.",
					"It'll last ages!"
				),
				new BookPageInfo
				(
					"Spider Leg Jerky:",
					"Remove legs from",
					"large cave spiders.",
					"Sprinkle with",
					"crushed herbs and",
					"salt. Dry over",
					"smoky fire. Great",
					"for long hunts."
				),
				new BookPageInfo
				(
					"Mushroom Wine:",
					"Ferment dark cave",
					"mushrooms in a vat",
					"of stagnant water.",
					"Let sit for a",
					"month. Sip slowly.",
					"Very potent."
				),
				new BookPageInfo
				(
					"Goblin Cheese:",
					"Take goblin milk",
					"and add a drop of",
					"ogre sweat. Let",
					"sit till mold forms.",
					"Cut off the mold",
					"and enjoy the rest!"
				),
				new BookPageInfo
				(
					"Serpent Stew:",
					"Skin a swamp",
					"serpent. Cut into",
					"chunks. Boil with",
					"swamp grass and",
					"mud. A slimy but",
					"tasty treat!"
				),
				new BookPageInfo
				(
					"Orcish Feasts:",
					"For special days,",
					"mix all these",
					"dishes together.",
					"Cook in a giant",
					"pot. Invite the",
					"whole clan. A meal",
					"to remember!"
				)

			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public OrcishBaking() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Orcish Baking" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "Orcish Baking" );
		}

		public OrcishBaking( Serial serial ) : base( serial )
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
