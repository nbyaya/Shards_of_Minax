using System;
using Server;

namespace Server.Items
{
    public class HistoryOfLiches : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Liches", "Author Name",
                new BookPageInfo
                (
                    "The concept of Liches",
                    "has been present in",
                    "our lands for",
                    "generations.",
                    "These undead creatures",
                    "are feared and",
                    "respected for their",
                    "dark powers."
                ),
                new BookPageInfo
                (
                    "Originally, Liches were",
                    "mortal beings seeking",
                    "immortality through",
                    "dark rituals and",
                    "arcane magic. These",
                    "beings exchange their",
                    "mortality for",
                    "boundless power."
                ),
                new BookPageInfo
                (
                    "The path to becoming",
                    "a Lich is perilous.",
                    "Many have tried and",
                    "failed, ending up as",
                    "abominations. Yet,",
                    "those who succeed",
                    "achieve mastery over",
                    "the necromantic arts."
                ),
                new BookPageInfo
                (
                    "Their physical form",
                    "decays, but their",
                    "consciousness lives",
                    "on in a phylactery.",
                    "Destroying a Lich",
                    "requires destroying",
                    "this object, often",
                    "hidden away carefully."
                ),
                new BookPageInfo
                (
                    "Liches command the",
                    "undead and hold dark",
                    "secrets that make",
                    "them invaluable allies",
                    "or formidable foes.",
                    "Approach them with",
                    "caution, for their",
                    "power is immense."
                ),
				new BookPageInfo
				(
					"The transformation into",
					"a Lich is a highly",
					"guarded secret, known",
					"only to the most",
					"experienced necromancers.",
					"Rumors suggest the use",
					"of rare artifacts and",
					"forbidden spells."
				),
				new BookPageInfo
				(
					"Liches are known for",
					"their cunning and",
					"strategic minds. It is",
					"said that they can",
					"outthink even the most",
					"skilled tacticians and",
					"that their magic is",
					"unrivaled."
				),
				new BookPageInfo
				(
					"However, despite their",
					"power, Liches are bound",
					"by ancient laws and",
					"contracts that limit",
					"their actions. Betraying",
					"these accords could",
					"result in severe",
					"consequences."
				),
				new BookPageInfo
				(
					"Liches are often",
					"surrounded by hordes",
					"of undead, whom they",
					"control effortlessly.",
					"These minions serve as",
					"both guardians and",
					"soldiers in their dark",
					"endeavors."
				),
				new BookPageInfo
				(
					"The presence of a Lich",
					"can corrupt the land,",
					"making it uninhabitable",
					"for the living. Trees",
					"wither, animals flee,",
					"and the soil becomes",
					"barren."
				),
				new BookPageInfo
				(
					"It is said that some",
					"Liches possess the",
					"ability to manipulate",
					"time and even erase",
					"events from history.",
					"These claims, however,",
					"remain unverified."
				),
				new BookPageInfo
				(
					"It's important to",
					"remember that not all",
					"Liches are inherently",
					"evil. Some seek the",
					"preservation of knowledge",
					"and balance, but their",
					"methods often put them",
					"at odds with mortals."
				),
				new BookPageInfo
				(
					"The history of Liches",
					"is a cautionary tale",
					"of the risks and rewards",
					"of pursuing power. It",
					"serves as a grim",
					"reminder of the price",
					"one may pay for",
					"immortality."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfLiches() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Liches");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Liches");
        }

        public HistoryOfLiches(Serial serial) : base(serial)
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
