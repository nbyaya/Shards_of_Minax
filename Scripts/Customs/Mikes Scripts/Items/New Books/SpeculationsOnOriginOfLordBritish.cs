using System;
using Server;

namespace Server.Items
{
	public class SpeculationsOnOriginOfLordBritish : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Speculations on the Origin", "Unknown Sage",
				new BookPageInfo
				(
					"The enigma surrounding",
					"Lord British has always",
					"captivated the minds of",
					"Britannia's citizens.",
					"Who is this immortal",
					"ruler, and from whence",
					"did he come?",
					"     -Unknown Sage"
				),
				new BookPageInfo
				(
					"Some believe that Lord",
					"British is not from",
					"this realm at all. A",
					"theory suggests that",
					"he comes from a place",
					"beyond the stars, a",
					"realm where immortality",
					"is but a norm."
				),
				new BookPageInfo
				(
					"Others think that he",
					"is a manifestation of",
					"the Virtues themselves,",
					"brought into existence",
					"to guide us towards",
					"the path of enlightenment",
					"and righteousness.",
					""
				),
				new BookPageInfo
				(
					"A darker theory whispers",
					"that Lord British might",
					"be a mage of immense",
					"power who found the",
					"secret of eternal life.",
					"Could his immortality",
					"be a result of a pact",
					"with unknown forces?"
				),
				new BookPageInfo
				(
					"Regardless of the truth,",
					"Lord British stands as",
					"a beacon of hope and",
					"guidance for us all.",
					"Until the day comes",
					"when the mystery is",
					"solved, we can but",
					"speculate."
				),
				new BookPageInfo
				(
					"Another prevailing theory",
					"states that Lord British",
					"might be a time traveler,",
					"unfettered by the",
					"limitations of past and",
					"future. This would explain",
					"his advanced knowledge",
					"and insight."
				),
				new BookPageInfo
				(
					"There's even talk of Lord",
					"British being a collective",
					"illusion, a personification",
					"of the people's need for",
					"a strong and just leader.",
					"Could he be a myth,",
					"perpetuated by society",
					"itself?"
				),
				new BookPageInfo
				(
					"A whimsical belief held by",
					"some is that Lord British",
					"is simply an adventurer",
					"who happened upon an",
					"artifact granting him",
					"immortality and power.",
					"Perhaps he is as lost as",
					"we are."
				),
				new BookPageInfo
				(
					"One controversial opinion",
					"suggests that Lord British",
					"is not singular but a",
					"title passed down in",
					"secrecy, from one ruler",
					"to the next. Each 'Lord'",
					"British could be selected",
					"for his virtues."
				),
				new BookPageInfo
				(
					"The priesthood of the",
					"Virtues often debate",
					"this subject. They wonder",
					"if Lord British is a divine",
					"entity, testing humanity's",
					"worthiness to attain",
					"ascension and join",
					"higher realms."
				),
				new BookPageInfo
				(
					"Some naturalists propose",
					"that Lord British may be",
					"an evolved form of human,",
					"a next step in natural",
					"selection. They claim",
					"his immortality is just",
					"advanced biology.",
					""
				),
				new BookPageInfo
				(
					"Of course, there are",
					"those who dismiss all",
					"these speculations,",
					"choosing to believe that",
					"the story of Lord British",
					"is exactly as it appears,",
					"without hidden meaning",
					"or mystery."
				),
				new BookPageInfo
				(
					"First documented,",
					"10.18.2023",
					"by a curious soul"
				)
			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public SpeculationsOnOriginOfLordBritish() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Speculations on the Origin of Lord British" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "Speculations on the Origin of Lord British" );
		}

		public SpeculationsOnOriginOfLordBritish( Serial serial ) : base( serial )
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
