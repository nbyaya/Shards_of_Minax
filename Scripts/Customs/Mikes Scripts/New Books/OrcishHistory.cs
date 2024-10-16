using System;
using Server;

namespace Server.Items
{
	public class OrcishHistory : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Orcish History", "Grom'Kar",
				new BookPageInfo
				(
					"This tome aims to",
					"unfold the rich",
					"history of the",
					"orcish clans that",
					"roam Britannia.",
					"",
					"          -Grom'Kar"
				),
				new BookPageInfo
				(
					"Origins of the Orcs",
					"",
					"The orcs are not",
					"native to Britannia.",
					"Legends speak of",
					"their arrival through",
					"rifts from a realm",
					"known as Orkran."
				),
				new BookPageInfo
				(
					"Clan Hierarchy",
					"",
					"Orcish society is",
					"divided into clans.",
					"Each clan has its",
					"own unique customs,",
					"traditions, and",
					"territories."
				),
				// Add more pages here...
				new BookPageInfo
				(
					"The Bloodaxe Clan",
					"",
					"Known for their",
					"ferocity in battle,",
					"the Bloodaxe Clan is",
					"often at the front",
					"lines of any conflict.",
					"They specialize in"
				),
				new BookPageInfo
				(
					"heavy axes and",
					"blood magic, which",
					"they use to inflict",
					"maximum damage.",
					"",
					"Chief: Grall Bloodaxe"
				),
				new BookPageInfo
				(
					"The Stonefoot Clan",
					"",
					"Master builders and",
					"blacksmiths, the",
					"Stonefoot Clan is",
					"responsible for most",
					"of the orcs' sturdy",
					"fortifications."
				),
				new BookPageInfo
				(
					"Their craftsmen are",
					"unparalleled, and",
					"their siege weapons",
					"are feared by many.",
					"",
					"Chief: Urog Stonefoot"
				),
				new BookPageInfo
				(
					"The Shadowfang Clan",
					"",
					"Experts in stealth",
					"and assassination,",
					"the Shadowfang Clan",
					"is often employed for",
					"reconnaissance and",
					"sabotage missions."
				),
				new BookPageInfo
				(
					"Shrouded in mystery,",
					"their true numbers",
					"and bases are",
					"unknown.",
					"",
					"Chief: Vara Shadowfang"
				),
				new BookPageInfo
				(
					"Orcish Spirituality",
					"",
					"Orcs are deeply",
					"spiritual, often",
					"worshipping the",
					"spirits of nature",
					"and their ancestors."
				),
				new BookPageInfo
				(
					"Shamans hold a high",
					"place in society,",
					"often advising the",
					"chiefs on important",
					"matters.",
					""
				),
				new BookPageInfo
				(
					"Conclusion",
					"",
					"While often",
					"misunderstood, the",
					"orcs have a rich",
					"history and culture",
					"that stretches back",
					"centuries."
				),
				new BookPageInfo
				(
					"It is this writer's",
					"hope that understanding",
					"may yet bring peace",
					"between our races.",
					"",
					"          -Grom'Kar"
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public OrcishHistory() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Orcish History" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "Orcish History" );
		}

		public OrcishHistory( Serial serial ) : base( serial )
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
