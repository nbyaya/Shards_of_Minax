using System;
using Server;

namespace Server.Items
{
    public class HistoryOfNecromancy : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Necromancy", "Mage Zargoth",
                new BookPageInfo
                (
                    "Necromancy, the",
                    "dark art of raising",
                    "and manipulating",
                    "the dead, has a",
                    "long and sordid",
                    "history that dates",
                    "back to ancient",
                    "times."
                ),
                new BookPageInfo
                (
                    "It was first",
                    "practiced by the",
                    "cult of Mortus, a",
                    "group of mages",
                    "fascinated by the",
                    "powers that lie",
                    "beyond death."
                ),
                // Add more pages as you wish
				new BookPageInfo
				(
					"In its earliest",
					"days, the art was",
					"crude and not well",
					"understood. Many",
					"early practitioners",
					"paid a heavy price",
					"for tampering with",
					"forces they did not"
				),
				new BookPageInfo
				(
					"comprehend, often",
					"falling victim to",
					"their own dark",
					"experiments. Yet,",
					"as time passed, the",
					"cult of Mortus",
					"refined their",
					"techniques."
				),
				new BookPageInfo
				(
					"They discovered",
					"how to control",
					"skeletal and",
					"zombie minions",
					"and how to use",
					"dark magic to",
					"drain life force",
					"from the living."
				),
				new BookPageInfo
				(
					"As the power of",
					"necromancers grew,",
					"so did society's",
					"fear and",
					"condemnation.",
					"Necromancy was",
					"outlawed in most",
					"kingdoms, and"
				),
				new BookPageInfo
				(
					"practitioners were",
					"often hunted like",
					"common criminals.",
					"Despite this, the",
					"art continued to",
					"flourish in secret",
					"covens and hidden",
					"schools."
				),
				new BookPageInfo
				(
					"In recent years,",
					"some scholars have",
					"argued that",
					"necromancy is",
					"simply another",
					"form of magic,",
					"neither good nor",
					"evil, but shaped"
				),
				new BookPageInfo
				(
					"by the intent of",
					"the user. These",
					"scholars advocate",
					"for the study of",
					"necromancy to be",
					"brought back into",
					"the mainstream,",
					"albeit with strict"
				),
				new BookPageInfo
				(
					"regulations and",
					"ethical guidelines.",
					"The debate",
					"continues to this",
					"day, and it remains",
					"to be seen how",
					"society will",
					"ultimately judge"
				),
				new BookPageInfo
				(
					"this ancient and",
					"mysterious art.",
					"But one thing is",
					"certain -",
					"necromancy will",
					"always hold a",
					"fascination for",
					"those who seek"
				),
				new BookPageInfo
				(
					"to unlock the",
					"secrets of life",
					"and death, despite",
					"the risks that",
					"come with tampering",
					"with the eternal",
					"cycle.",
					""
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfNecromancy() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Necromancy");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Necromancy");
        }

        public HistoryOfNecromancy(Serial serial) : base(serial)
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
