using System;
using Server;

namespace Server.Items
{
    public class OgreTailoringBook : BlueBook // BlueBook is assumed to be a parent class for books in your shard
    {
        public static readonly BookContent Content = new BookContent
            (
                "Ogre Tailoring", "Grokka",
                new BookPageInfo
                (
                    "Ogre tailoring is an",
                    "art that combines",
                    "both brute force and",
                    "delicate craftsmanship.",
                    "",
                    "",
                    "",
                    "          -Grokka"
                ),
                new BookPageInfo
                (
                    "The ogre's crude,",
                    "yet effective, method",
                    "of crafting has been",
                    "passed down for",
                    "generations. It may",
                    "not look pretty, but",
                    "it's sturdy and lasts",
                    "a long time."
                ),
                new BookPageInfo
                (
                    "Materials used are",
                    "often unconventional.",
                    "Leaves, mud, and even",
                    "bones can be found",
                    "in ogre-made garments.",
                    "They believe in using",
                    "what nature provides,",
                    "no matter how odd."
                ),
                new BookPageInfo
                (
                    "Ogre tailors use their",
                    "hands more than tools.",
                    "They will often just",
                    "tear a piece of fabric",
                    "rather than cutting it.",
                    "They believe the",
                    "imperfections add",
                    "character."
                ),
				new BookPageInfo
				(
					"An essential aspect",
					"of ogre tailoring is",
					"the curing process.",
					"Freshly torn fabric is",
					"typically buried in",
					"mud for several days",
					"to toughen it.",
					"The smell is a bonus."
				),
				new BookPageInfo
				(
					"Contrary to popular",
					"belief, ogres do use",
					"some stitching. Their",
					"preferred method is",
					"using vines or animal",
					"tendons as thread.",
					"This makes the seams",
					"strong, yet flexible."
				),
				new BookPageInfo
				(
					"Coloration is also",
					"important in ogre",
					"craftsmanship. They",
					"usually stick to",
					"earth tones, often",
					"achieved by rolling",
					"the garment on grass",
					"or in fallen leaves."
				),
				new BookPageInfo
				(
					"Patterns and symbols",
					"are common in ogre",
					"tailoring, usually",
					"depicting the story",
					"of a famous hunt or",
					"battle. They are",
					"drawn with charcoal",
					"or berry juice."
				),
				new BookPageInfo
				(
					"Ogre tailoring is",
					"not just for clothing.",
					"They also craft",
					"satchels, pouches,",
					"and even simple tents",
					"using similar",
					"techniques.",
					"Function over form."
				),
				new BookPageInfo
				(
					"While ogre tailoring",
					"may not be elegant,",
					"it's a crucial part",
					"of their culture and",
					"survival. The next",
					"time you see an ogre",
					"outfit, you may find",
					"it's more than it seems."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgreTailoringBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ogre Tailoring");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ogre Tailoring");
        }

        public OgreTailoringBook(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
