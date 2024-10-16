using System;
using Server;

namespace Server.Items
{
    public class HistoryOfWaterElementals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Water Elementals", "Aetherius",
                new BookPageInfo
                (
                    "The Water Elementals",
                    "have always been a",
                    "subject of great",
                    "intrigue for scholars",
                    "and mages alike.",
                    "",
                    "          -Aetherius"
                ),
                new BookPageInfo
                (
                    "Originating from the",
                    "Elemental Plane of",
                    "Water, these beings",
                    "are made entirely of",
                    "water and can control",
                    "it to a great extent."
                ),
                new BookPageInfo
                (
                    "They are commonly",
                    "summoned by mages",
                    "for various purposes",
                    "ranging from research",
                    "to combat. However,",
                    "uncontrolled summoning"
                ),
                new BookPageInfo
                (
                    "can lead to disastrous",
                    "consequences, as",
                    "Water Elementals are",
                    "not inherently good",
                    "or evil but driven by",
                    "their own elemental",
                    "instincts."
                ),
                new BookPageInfo
                (
                    "Their abilities include",
                    "manipulating water,",
                    "healing allies, and",
                    "even causing floods.",
                    "It is advised to",
                    "exercise caution when",
                    "dealing with them."
                ),
                new BookPageInfo
                (
                    "Legend tells of a",
                    "Water Elemental King,",
                    "a being of immense",
                    "power who rules the",
                    "Elemental Plane of",
                    "Water."
                ),
				new BookPageInfo
				(
					"Elemental Hierarchy",
					"and Laws",
					"Elementals have their",
					"own hierarchy and laws",
					"governing their",
					"actions. Breaking these",
					"laws can result in",
					"banishment."
				),
				new BookPageInfo
				(
					"Significance in",
					"Alchemy",
					"Water Elementals are",
					"often sought after for",
					"their pure essence,",
					"which is a vital",
					"ingredient in many",
					"alchemy recipes."
				),
				new BookPageInfo
				(
					"Summoning and",
					"Binding",
					"Though skilled mages",
					"can summon and bind",
					"these beings, it is",
					"always risky. A failed",
					"binding can result in",
					"retaliation."
				),
				new BookPageInfo
				(
					"Cults and Worship",
					"There exist cults that",
					"worship these elemental",
					"beings, often in hope",
					"of gaining power over",
					"water or in pursuit of",
					"forbidden knowledge."
				),
				new BookPageInfo
				(
					"Geographical",
					"Occurrences",
					"Water Elementals are",
					"common near large",
					"bodies of water but have",
					"also been seen in",
					"deserts and mountains."
				),
				new BookPageInfo
				(
					"Behavior and",
					"Temperament",
					"Mostly passive unless",
					"provoked, Water",
					"Elementals can be",
					"unpredictable. Their",
					"mood can change like",
					"the tides."
				),
				new BookPageInfo
				(
					"Famous Encounters",
					"Famous adventurers",
					"and mages have had",
					"notable encounters",
					"with these beings,",
					"sometimes forming",
					"alliances or enemies."
				),
				new BookPageInfo
				(
					"In Literature and",
					"Arts",
					"Water Elementals have",
					"been featured in many",
					"stories, poems, and",
					"paintings, often as",
					"symbols of change and",
					"fluidity."
				),
				new BookPageInfo
				(
					"Aetherius, 9pm,",
					"12.12.2022",
					"Last updated"
				),
                new BookPageInfo
                (
                    "Aetherius, 9pm,",
                    "12.12.2022",
                    "Last updated"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfWaterElementals() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of Water Elementals");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of Water Elementals");
        }

        public HistoryOfWaterElementals(Serial serial) : base(serial)
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
