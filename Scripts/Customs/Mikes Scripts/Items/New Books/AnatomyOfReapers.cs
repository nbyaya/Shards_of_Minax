using System;
using Server;

namespace Server.Items
{
	public class AnatomyOfReapers : BlueBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Anatomy of Reapers", "Sage Eldric",
				new BookPageInfo
				(
					"This book is a study",
					"on the anatomy of the",
					"much-feared Reapers.",
					"Understanding their",
					"structure could be",
					"vital to combating",
					"them.",
					"           -Sage Eldric"
				),
				new BookPageInfo
				(
					"Reapers are complex",
					"creatures with a",
					"unique internal",
					"structure. Their",
					"bark-like skin serves",
					"not only as armor",
					"but also as a means",
					"to gather nutrients."
				),
				new BookPageInfo
				(
					"Unlike typical plants,",
					"Reapers have a circulatory",
					"system that allows",
					"them to mobilize",
					"quickly. This system",
					"also allows them to",
					"heal rapidly, making",
					"them difficult foes."
				),
				new BookPageInfo
				(
					"Interestingly, they",
					"possess a primitive",
					"brain-like organ that",
					"governs their actions.",
					"Understanding this",
					"organ may lead us to",
					"find a way to",
					"neutralize them."
				),
				new BookPageInfo
				(
					"Through meticulous",
					"study, we may uncover",
					"the vulnerabilities",
					"of these enigmatic",
					"creatures and perhaps",
					"devise a way to",
					"combat them more",
					"effectively."
				),
				new BookPageInfo
				(
					"The Reaper's 'heart' is",
					"a nexus of magical",
					"energies that seem to",
					"be the source of its",
					"life force. This",
					"concentration of mana",
					"allows them to perform",
					"magical abilities."
				),
				new BookPageInfo
				(
					"The limbs of a Reaper",
					"are not just tools for",
					"attack. They have",
					"root-like appendages",
					"that they can use to",
					"anchor themselves,",
					"drawing nutrients from",
					"the ground."
				),
				new BookPageInfo
				(
					"Their visual system",
					"is rudimentary but",
					"effective. Instead of",
					"eyes, they possess",
					"photosensitive cells",
					"that allow them to",
					"detect changes in",
					"light and shadow."
				),
				new BookPageInfo
				(
					"Vocalizations are",
					"nonexistent. They",
					"communicate through",
					"chemical and magical",
					"signals, making it",
					"difficult to intercept",
					"or understand their",
					"intentions."
				),
				new BookPageInfo
				(
					"The most fearsome",
					"aspect of Reapers is",
					"their adaptability.",
					"They seem to learn",
					"from encounters,",
					"adjusting their",
					"tactics as needed.",
					"Beware repeat fights."
				),
				new BookPageInfo
				(
					"Studies have shown",
					"that Reapers are",
					"affected by certain",
					"types of magic more",
					"than others. Spells",
					"that manipulate",
					"nature seem to be",
					"most effective."
				),
				new BookPageInfo
				(
					"In summary, Reapers",
					"are formidable foes",
					"with complex anatomy.",
					"Understanding their",
					"biology could be the",
					"key to eradicating",
					"this menacing species.",
					"Proceed with caution."
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public AnatomyOfReapers() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Anatomy of Reapers" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "Anatomy of Reapers" );
		}

		public AnatomyOfReapers( Serial serial ) : base( serial )
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
