using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfGarlic : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Magical Properties of Garlic", "Ailith the Alchemist",
                new BookPageInfo
                (
                    "Garlic is more than",
                    "a simple seasoning",
                    "in culinary arts.",
                    "It has potent magical",
                    "properties that have",
                    "been overlooked.",
                    "",
                    "         - Ailith"
                ),
                new BookPageInfo
                (
                    "Garlic's spiritual",
                    "attributes date back",
                    "to ancient times. It",
                    "is said to ward off",
                    "evil spirits and",
                    "offensive magics."
                ),
                new BookPageInfo
                (
                    "Alchemy is one",
                    "discipline that can",
                    "harness garlic's",
                    "power. When combined",
                    "with other reagents,",
                    "it boosts their",
                    "effects considerably."
                ),
                new BookPageInfo
                (
                    "Even warriors and",
                    "mages can benefit",
                    "from carrying garlic.",
                    "It acts as an",
                    "amulet, providing",
                    "magical resistances."
                ),
                new BookPageInfo
                (
                    "Garlic also has the",
                    "power to break some",
                    "enchantments and",
                    "hexes, and can be",
                    "used in cleansing",
                    "rituals."
                ),
                new BookPageInfo
                (
                    "It is a humble",
                    "reagent with grand",
                    "potentials. Consider",
                    "adding it to your",
                    "magical repertoire."
                ),
				new BookPageInfo
				(
					"In brewing potions,",
					"garlic is an essential",
					"component. Its magical",
					"essence enhances",
					"elixirs for healing,",
					"protection, and even",
					"invisibility."
				),
				new BookPageInfo
				(
					"For necromancers and",
					"dark sorcerers, garlic",
					"acts as a bane. Its",
					"presence can disrupt",
					"dark rituals and",
					"prevent the raising of",
					"undead creatures."
				),
				new BookPageInfo
				(
					"The bards sing tales",
					"of garlic being used",
					"to repel vampires,",
					"werewolves, and other",
					"nocturnal entities.",
					"The scent alone is",
					"said to keep them at",
					"bay."
				),
				new BookPageInfo
				(
					"Farmers and herbalists",
					"know the secrets of",
					"growing magically",
					"potent garlic. They",
					"often infuse the soil",
					"with arcane essences",
					"for optimal growth."
				),
				new BookPageInfo
				(
					"Ancient scripts",
					"mention garlic in",
					"the creation of",
					"protective circles",
					"and barriers. It is",
					"laid at thresholds",
					"to prevent malevolent",
					"entry."
				),
				new BookPageInfo
				(
					"Garlic's versatility",
					"in magic should not",
					"be underestimated.",
					"Its uses are limited",
					"only by the user's",
					"imagination and skill.",
					"Experiment wisely!"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MagicalPropertiesOfGarlic() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Magical Properties of Garlic");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Magical Properties of Garlic");
        }

        public MagicalPropertiesOfGarlic(Serial serial) : base(serial)
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
