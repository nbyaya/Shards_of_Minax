using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTrolls : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Trolls", "Aragorn",
                new BookPageInfo
                (
                    "Trolls have roamed",
                    "the lands of Sosaria",
                    "for countless",
                    "generations. Often",
                    "misunderstood, this",
                    "book aims to shed",
                    "light on their",
                    "history."
                ),
                new BookPageInfo
                (
                    "Origins of Trolls",
                    "are shrouded in",
                    "myth. It's believed",
                    "that they were once",
                    "ordinary creatures,",
                    "transformed by dark",
                    "magic."
                ),
                new BookPageInfo
                (
                    "Trolls are notorious",
                    "for their aggression",
                    "and territorial",
                    "nature. They have",
                    "been known to",
                    "attack travelers",
                    "without provocation."
                ),
                new BookPageInfo
                (
                    "Not all trolls are",
                    "evil, however. Some",
                    "tribes have been",
                    "known to trade and",
                    "even form alliances",
                    "with humans and",
                    "other races."
                ),
                new BookPageInfo
                (
                    "Troll skin is tough,",
                    "providing them",
                    "natural armor.",
                    "Their strength is",
                    "legendary, allowing",
                    "them to wield large",
                    "clubs and stones."
                ),
                new BookPageInfo
                (
                    "To this day, trolls",
                    "remain a subject of",
                    "both fear and",
                    "fascination. As we",
                    "strive to understand",
                    "them better, may we",
                    "also learn to coexist."
                ),
				new BookPageInfo
				(
					"Trolls are often",
					"found in damp,",
					"dark habitats. Caves",
					"and thick forests are",
					"their most common",
					"dwellings. These",
					"locations offer",
					"shelter and ambush"
				),
				new BookPageInfo
				(
					"points for hunting.",
					"Their diet mainly",
					"consists of meat, but",
					"they aren't averse to",
					"consuming plants.",
					"Trolls are also",
					"known scavengers,",
					"picking clean the"
				),
				new BookPageInfo
				(
					"remains of battles.",
					"Trolls have their",
					"own form of social",
					"structure. Larger,",
					"more powerful trolls",
					"usually assume the",
					"role of a chieftain,",
					"commanding respect"
				),
				new BookPageInfo
				(
					"from their kin.",
					"Chieftains are",
					"often challenged",
					"by younger, more",
					"ambitious trolls. The",
					"outcome of these",
					"duels can lead to",
					"significant changes"
				),
				new BookPageInfo
				(
					"in the tribe's",
					"dynamics.",
					"",
					"One of the most",
					"intriguing aspects of",
					"troll culture is their",
					"relationship with",
					"magic. Despite their"
				),
				new BookPageInfo
				(
					"brutish nature, some",
					"trolls possess",
					"rudimentary magical",
					"skills, often tied to",
					"the elements. These",
					"trolls serve as",
					"shamans, guiding the",
					"tribe in spiritual"
				),
				new BookPageInfo
				(
					"matters.",
					"",
					"Conclusively, trolls",
					"are a fascinating",
					"subject that deserve",
					"deeper study. With",
					"caution and respect,",
					"we may yet unravel"
				),
				new BookPageInfo
				(
					"the many mysteries",
					"that surround these",
					"enigmatic creatures.",
					"And perhaps, find a",
					"way for humans and",
					"trolls to live in",
					"peaceful coexistence."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTrolls() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Trolls");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Trolls");
        }

        public HistoryOfTrolls(Serial serial) : base(serial)
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
