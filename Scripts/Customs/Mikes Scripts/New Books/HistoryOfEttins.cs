using System;
using Server;

namespace Server.Items
{
    public class HistoryOfEttins : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Ettins", "Scribe Eldrin",
                new BookPageInfo
                (
                    "This text aims to",
                    "explore the history",
                    "and nature of the",
                    "mysterious Ettins.",
                    "",
                    "          -Scribe Eldrin"
                ),
                new BookPageInfo
                (
                    "Ettins are often",
                    "misunderstood as mere",
                    "brutes. However, they",
                    "have a rich history",
                    "that is intricately",
                    "tied to the land."
                ),
                new BookPageInfo
                (
                    "According to lore,",
                    "Ettins came into",
                    "existence due to a",
                    "curse placed upon",
                    "two-headed giants."
                ),
                new BookPageInfo
                (
                    "Though generally",
                    "hostile, some ettins",
                    "have been known to",
                    "trade and converse",
                    "with other species."
                ),
                new BookPageInfo
                (
                    "Ettins have strong",
                    "ties to the earth,",
                    "and they are known",
                    "to worship ancient",
                    "nature deities."
                ),
				new BookPageInfo
				(
					"In terms of physical",
					"attributes, ettins are",
					"known for their",
					"double heads, which",
					"grants them unique",
					"perceptive abilities."
				),
				new BookPageInfo
				(
					"Each head has its own",
					"consciousness, making",
					"the ettin capable of",
					"simultaneous tasks.",
					"This allows for",
					"intricate strategies"
				),
				new BookPageInfo
				(
					"in both hunting and",
					"combat. However, it",
					"can also lead to",
					"internal conflicts",
					"that can temporarily",
					"incapacitate them."
				),
				new BookPageInfo
				(
					"Historical records",
					"indicate that ettins",
					"were once part of an",
					"ancient civilization",
					"known as 'Uurgoth.'"
				),
				new BookPageInfo
				(
					"While Uurgoth is no",
					"more, relics and",
					"ruins across the",
					"land point to their",
					"once majestic cities."
				),
				new BookPageInfo
				(
					"Today, ettins are",
					"commonly found in",
					"mountainous regions",
					"or deep forests,",
					"away from populated",
					"areas."
				),
				new BookPageInfo
				(
					"Recent studies",
					"suggest some ettin",
					"tribes possess sacred",
					"totems that grant",
					"them mysterious",
					"powers."
				),
				new BookPageInfo
				(
					"Reader, if you find",
					"yourself face to face",
					"with an ettin, it is",
					"wise to be cautious.",
					"They are not to be",
					"underestimated."
				),
				new BookPageInfo
				(
					"Whether seen as",
					"monsters or misunderstood",
					"beings, ettins",
					"continue to be a",
					"subject of both",
					"fear and fascination."
				),
				new BookPageInfo
				(
					"This concludes the",
					"overview of ettins.",
					"May this knowledge",
					"guide you well."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfEttins() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Ettins");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Ettins");
        }

        public HistoryOfEttins(Serial serial) : base(serial)
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
