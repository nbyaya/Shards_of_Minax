using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfBlackPearl : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Magical Properties of Black Pearl", "Mage Eldron",
                new BookPageInfo
                (
                    "The purpose of this",
                    "book is to enlighten",
                    "the reader on the",
                    "various magical",
                    "properties of the",
                    "black pearl.",
                    "",
                    "           -Mage Eldron"
                ),
                new BookPageInfo
                (
                    "Black Pearl is a",
                    "mystical reagent",
                    "commonly used in",
                    "casting spells",
                    "within the schools of",
                    "Magery, Mysticism,",
                    "and Necromancy."
                ),
                new BookPageInfo
                (
                    "Magical properties",
                    "include channeling",
                    "spiritual energy,",
                    "enhancing focus, and",
                    "stabilizing magical",
                    "constructs."
                ),
                new BookPageInfo
                (
                    "Although it's a",
                    "common ingredient",
                    "in many spells, its",
                    "properties are not",
                    "entirely understood."
                ),
                new BookPageInfo
                (
                    "Some mages have",
                    "speculated that the",
                    "black pearl is linked",
                    "to other realms,",
                    "acting as a conduit",
                    "for magical forces."
                ),
				new BookPageInfo
				(
					"The black pearl has",
					"often been linked to",
					"the Plane of Water,",
					"and is thus highly",
					"effective in spells",
					"that manipulate or",
					"control water."
				),
				new BookPageInfo
				(
					"Interestingly, some",
					"have reported that",
					"black pearls harvested",
					"during the night of a",
					"full moon possess",
					"enhanced potency."
				),
				new BookPageInfo
				(
					"Alchemists have also",
					"found use for black",
					"pearl in creating",
					"potions that require",
					"a stable magical",
					"catalyst."
				),
				new BookPageInfo
				(
					"It is critical to",
					"source your black",
					"pearls from reputable",
					"dealers, as inferior",
					"pearls can lead to",
					"spell failure or",
					"worse, magical",
					"backfire."
				),
				new BookPageInfo
				(
					"In recent times,",
					"the demand for black",
					"pearls has risen",
					"dramatically,",
					"prompting concern",
					"over sustainable",
					"harvesting practices."
				),
				new BookPageInfo
				(
					"There are reports of",
					"cults using black",
					"pearls in dark",
					"rituals. However,",
					"these claims have",
					"yet to be",
					"substantiated."
				),
				new BookPageInfo
				(
					"Some adventurers",
					"claim to have found",
					"black pearls in the",
					"nests of sea serpents,",
					"hinting at a deeper",
					"connection between",
					"the reagent and",
					"marine creatures."
				),
				new BookPageInfo
				(
					"In conclusion, the",
					"black pearl remains",
					"an enigma. While its",
					"use in magic is well",
					"documented, much",
					"about its nature",
					"remains to be",
					"discovered."
				),
                new BookPageInfo
                (
                    "The end."
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MagicalPropertiesOfBlackPearl() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Magical Properties of Black Pearl");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Magical Properties of Black Pearl");
        }

        public MagicalPropertiesOfBlackPearl(Serial serial) : base(serial)
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
