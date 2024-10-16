using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfBloodMoss : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Magical Properties of Blood Moss", "Mage Elira",
                new BookPageInfo
                (
                    "Blood Moss, known to",
                    "magicians as 'Quaero",
                    "Eriox', is a special",
                    "ingredient with",
                    "myriad uses in the",
                    "arcane arts.",
                    "",
                    "           -Mage Elira"
                ),
                new BookPageInfo
                (
                    "Blood Moss excels in",
                    "enhancing mobility",
                    "spells such as",
                    "Teleportation and",
                    "Levitation. It is",
                    "also useful in",
                    "battle, augmenting",
                    "attack speed."
                ),
                new BookPageInfo
                (
                    "When mixed with",
                    "other ingredients",
                    "like Mandrake Root",
                    "and Nightshade, it",
                    "can produce potent",
                    "potions or can be",
                    "used in intricate",
                    "rituals."
                ),
                new BookPageInfo
                (
                    "Caution: Incorrect",
                    "usage can result in",
                    "unstable magical",
                    "effects, posing",
                    "danger to both the",
                    "practitioner and",
                    "bystanders."
                ),
                new BookPageInfo
                (
                    "Blood Moss should",
                    "always be harvested",
                    "under a full moon",
                    "for maximum",
                    "potency. Handle",
                    "with care."
                ),
				new BookPageInfo
				(
					"Harvesting Blood Moss",
					"requires keen eyes and",
					"a gentle touch. The",
					"moss typically grows",
					"in damp areas and is",
					"often found near",
					"water sources such as",
					"rivers and lakes."
				),
				new BookPageInfo
				(
					"For magical purposes,",
					"it is recommended to",
					"dry the moss in the",
					"sun for three days",
					"before using it in any",
					"spellwork, potion",
					"brewing, or alchemy."
				),
				new BookPageInfo
				(
					"Blood Moss has a",
					"significant role in",
					"transmutation spells.",
					"When used correctly,",
					"it can alter the form",
					"of objects, although",
					"the effects are often",
					"temporary."
				),
				new BookPageInfo
				(
					"Some ancient texts",
					"even suggest that",
					"Blood Moss was used",
					"in the resurrection of",
					"the dead, but these",
					"claims have yet to be",
					"substantiated."
				),
				new BookPageInfo
				(
					"One must also be",
					"aware that the use of",
					"Blood Moss can be",
					"forbidden or",
					"regulated in certain",
					"regions due to its",
					"potency and potential",
					"for misuse."
				),
				new BookPageInfo
				(
					"In conclusion, Blood",
					"Moss is a versatile",
					"and invaluable",
					"ingredient for anyone",
					"serious in the study",
					"of magical arts.",
					"Treat it with the",
					"respect it deserves."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MagicalPropertiesOfBloodMoss() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Magical Properties of Blood Moss");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Magical Properties of Blood Moss");
        }

        public MagicalPropertiesOfBloodMoss(Serial serial) : base(serial)
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
