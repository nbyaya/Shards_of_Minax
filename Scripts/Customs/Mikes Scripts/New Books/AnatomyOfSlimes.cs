using System;
using Server;

namespace Server.Items
{
	public class AnatomyOfSlimes : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Anatomy of Slimes", "Dr. Slimeologist",
				new BookPageInfo
				(
					"The slimes of the",
					"world are mysterious",
					"creatures. Unlike",
					"common animals, their",
					"biology is rather",
					"enigmatic.",
					"",
					"         -Dr. Slimeologist"
				),
				new BookPageInfo
				(
					"A slime’s outer layer",
					"acts both as skin and",
					"muscle, allowing",
					"movement and",
					"absorption of food.",
					"Their cores serve as",
					"a sort of 'brain',",
					"directing their actions."
				),
				new BookPageInfo
				(
					"Color differences in",
					"slimes usually signify",
					"different abilities or",
					"traits. For example,",
					"red slimes are often",
					"more aggressive, while",
					"blue slimes are known",
					"for their tranquil nature."
				),
				new BookPageInfo
				(
					"Slimes reproduce via",
					"binary fission. When",
					"a slime grows large",
					"enough, it splits into",
					"two new entities,",
					"each carrying on the",
					"traits of the parent.",
					""
				),
				new BookPageInfo
				(
					"The diet of a slime",
					"consists mainly of",
					"organic material, but",
					"they can also consume",
					"minerals to augment",
					"their structures.",
					"",
					""
				),
				new BookPageInfo
				(
					"Understanding slimes",
					"can offer insights",
					"into magical biology",
					"and potentially reveal",
					"new alchemical",
					"recipes or crafting",
					"materials.",
					""
				),
				new BookPageInfo
				(
					"The slime's core also",
					"contains an array of",
					"crystal-like structures",
					"that seem to store",
					"energy. This energy is",
					"possibly magical in",
					"origin and is thought",
					"to regulate the slime's"
				),
				new BookPageInfo
				(
					"metabolism. Without this",
					"energy, a slime cannot",
					"sustain itself and will",
					"decompose into inert",
					"matter.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"The physical properties",
					"of slimes make them",
					"extremely versatile.",
					"They can move through",
					"tight spaces, and",
					"their semi-fluid bodies",
					"allow them to adapt",
					"to various shapes."
				),
				new BookPageInfo
				(
					"A slime's sense of",
					"direction seems to be",
					"guided by an innate",
					"magnetic field, rather",
					"than through any",
					"physical organs for",
					"sight or hearing.",
					""
				),
				new BookPageInfo
				(
					"When it comes to",
					"defense, slimes have",
					"the ability to dissolve",
					"organic material with",
					"enzymes in their body.",
					"This aids in both",
					"self-defense and",
					"nutrition."
				),
				new BookPageInfo
				(
					"Despite their simple",
					"nature, slimes are",
					"fascinating subjects",
					"of magical study.",
					"Many mages have spent",
					"years trying to",
					"understand the magical",
					"essence contained in"
				),
				new BookPageInfo
				(
					"slime cores. These",
					"studies have led to",
					"advancements in the",
					"field of alchemy,",
					"including the creation",
					"of new potions and",
					"elixirs.",
					""
				),
				new BookPageInfo
				(
					"To adventurers, a",
					"slime may seem like an",
					"easy foe, but underestimating",
					"these creatures has led",
					"to the downfall of many.",
					"Their resilience and",
					"unique abilities make",
					"them a force to be"
				),
				new BookPageInfo
				(
					"reckoned with.",
					"",
					"",
					"",
					"         -Dr. Slimeologist",
					"         Last Updated",
					"         Spring, 1423"
				)

			);

		public override BookContent DefaultContent { get { return Content; } }

		[Constructable]
		public AnatomyOfSlimes() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Anatomy of Slimes" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "Anatomy of Slimes" );
		}

		public AnatomyOfSlimes( Serial serial ) : base( serial )
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
