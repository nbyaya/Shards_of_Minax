using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheHeadless : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Headless", "Scholar Arlin",
                new BookPageInfo
                (
                    "The Headless are",
                    "mysterious creatures",
                    "that roam the lands,",
                    "terrifying local",
                    "populations.",
                    "",
                    "",
                    "         -Scholar Arlin"
                ),
                new BookPageInfo
                (
                    "Origins of the",
                    "Headless can be",
                    "traced back to dark",
                    "magic and necromancy.",
                    "They were originally",
                    "humans cursed or",
                    "transformed."
                ),
                new BookPageInfo
                (
                    "Various tales claim",
                    "they seek to find",
                    "their lost heads,",
                    "while others believe",
                    "they simply exist to",
                    "terrorize."
                ),
                new BookPageInfo
                (
                    "Some scholars believe",
                    "that they are souls",
                    "cursed to roam the",
                    "earth for eternity,",
                    "forever lost and",
                    "incomplete."
                ),
                new BookPageInfo
                (
                    "Regardless of their",
                    "origins, one thing",
                    "is clear: these",
                    "creatures are to be",
                    "avoided at all costs."
                ),
				new BookPageInfo
				(
					"Early Tales",
					"One of the earliest",
					"accounts of a Headless",
					"dates back to the",
					"era of the great",
					"mage wars. A Headless",
					"was said to have",
					"guarded a crypt."
				),
				new BookPageInfo
				(
					"Modern Sightings",
					"In recent years, more",
					"and more sightings have",
					"been reported. Farmers",
					"complain of missing",
					"livestock, while",
					"travellers speak of",
					"unnerving encounters."
				),
				new BookPageInfo
				(
					"Methods of Attack",
					"Headless primarily use",
					"melee attacks, but",
					"some have been known",
					"to employ dark magics.",
					"They are swift and",
					"often silent."
				),
				new BookPageInfo
				(
					"Defensive Measures",
					"Magic barriers and",
					"physical traps have",
					"some effectiveness.",
					"However, they are",
					"persistent creatures",
					"and often return."
				),
				new BookPageInfo
				(
					"Headless and Other Creatures",
					"It's unknown how",
					"Headless interact with",
					"other creatures. Some",
					"suggest a mutual",
					"avoidance, while others",
					"claim they fight."
				),
				new BookPageInfo
				(
					"Closing Thoughts",
					"The mystery of the",
					"Headless continues to",
					"intrigue and terrify.",
					"As more is learned,",
					"may we find a way",
					"to coexist or even",
					"to cure them."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheHeadless() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Headless");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Headless");
        }

        public HistoryOfTheHeadless(Serial serial) : base(serial)
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
