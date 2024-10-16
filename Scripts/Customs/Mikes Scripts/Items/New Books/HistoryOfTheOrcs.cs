using System;
using Server;

namespace Server.Items
{
	public class HistoryOfTheOrcs : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"History of the Orcs", "Grumlok",
				new BookPageInfo
				(
					"Orcs, often seen as",
					"savage beasts, have",
					"a history as complex",
					"and storied as any",
					"race in Britannia.",
					"This book aims to",
					"shed light on these",
					"misunderstood beings."
				),
				new BookPageInfo
				(
					"Origins of Orcs are",
					"shrouded in mystery.",
					"Some believe they",
					"were once elves,",
					"corrupted by dark",
					"magic. Others argue",
					"they were created",
					"by the gods for war."
				),
				new BookPageInfo
				(
					"The tribes have",
					"different customs,",
					"but all respect",
					"strength and honor.",
					"Despite their",
					"reputation, many",
					"tribes are not",
					"inherently evil."
				),
				new BookPageInfo
				(
					"Orcs have fought",
					"in many wars, often",
					"as mercenaries or",
					"conscripts. They are",
					"feared warriors,",
					"adept at surviving",
					"in harsh conditions",
					"and adverse odds."
				),
				new BookPageInfo
				(
					"Although misunderstood",
					"and often maligned,",
					"the orcs have made",
					"significant",
					"contributions to",
					"alchemy and",
					"weapon forging.",
					"Their shamans are"
				),
				new BookPageInfo
				(
					"also quite skilled",
					"in elemental magic.",
					"They have a rich",
					"oral history, passed",
					"down through",
					"generations, which",
					"deserves the",
					"attention of scholars."
				),
				new BookPageInfo
				(
					"Orcish Society is",
					"often organized into",
					"clans and tribes.",
					"Each tribe is led by",
					"a Warchief, the most",
					"skilled and wise",
					"warrior among them.",
					"Leadership is"
				),
				new BookPageInfo
				(
					"often determined by",
					"combat prowess, but",
					"wisdom and cunning",
					"are equally valued.",
					"Warchiefs consult",
					"shamans for spiritual",
					"guidance and omens",
					"before making decisions."
				),
				new BookPageInfo
				(
					"Orcish religion",
					"varies from tribe",
					"to tribe but usually",
					"focuses on ancestral",
					"worship and elemental",
					"forces. Shamans",
					"perform rituals to",
					"please the spirits"
				),
				new BookPageInfo
				(
					"and seek their",
					"guidance. It's a",
					"common misconception",
					"that Orcs are devoid",
					"of spirituality or",
					"complex beliefs.",
					"Their gods are",
					"feared, but also"
				),
				new BookPageInfo
				(
					"deeply respected.",
					"Sacrifices, often",
					"of food or weapons,",
					"are made to seek",
					"favor or guidance.",
					"Failure to honor",
					"the gods is seen",
					"as an affront"
				),
				new BookPageInfo
				(
					"not just to the",
					"divine but also",
					"to the tribe.",
					"Such individuals",
					"may be exiled or",
					"even executed,",
					"depending on the",
					"severity of the"
				),
				new BookPageInfo
				(
					"transgression.",
					"Orcish architecture",
					"is practical and",
					"sturdy. Their",
					"settlements, often",
					"found in mountainous",
					"or forested areas,",
					"are built to"
				),
				new BookPageInfo
				(
					"withstand sieges",
					"and attacks. Walls",
					"are made from stone",
					"or thick wood, and",
					"entrances are",
					"narrow to make",
					"invasion difficult.",
					"The interiors"
				),
				new BookPageInfo
				(
					"are adorned with",
					"trophies of war",
					"and hunt, each",
					"item telling a",
					"story and adding",
					"to the clan's",
					"collective memory.",
					"It's a way for"
				),
				new BookPageInfo
				(
					"Orcs to connect",
					"with their history",
					"and instill pride",
					"and unity among",
					"their members.",
					"Despite their",
					"rough exterior,",
					"Orcs have a"
				),
				new BookPageInfo
				(
					"rich tradition of",
					"storytelling and",
					"oral history.",
					"Elders are the",
					"keepers of lore,",
					"passing down tales",
					"of heroism and",
					"lessons learned."
				),
				new BookPageInfo
				(
					"It's a deeply",
					"engrained part",
					"of their culture,",
					"and each story",
					"serves as a moral",
					"or practical lesson",
					"for younger",
					"generations."
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public HistoryOfTheOrcs() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "History of the Orcs" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "History of the Orcs" );
		}

		public HistoryOfTheOrcs( Serial serial ) : base( serial )
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
