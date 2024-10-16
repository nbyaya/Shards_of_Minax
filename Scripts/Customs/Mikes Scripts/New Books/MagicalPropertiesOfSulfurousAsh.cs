using System;
using Server;

namespace Server.Items
{
    public class MagicalPropertiesOfSulfurousAsh : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Magical Properties", "Alchemist Zeno",
                new BookPageInfo
                (
                    "This tome aims to",
                    "explore the magical",
                    "properties of",
                    "Sulfurous Ash, a",
                    "common reagent",
                    "in spell casting.",
                    "",
                    "           -Alchemist Zeno"
                ),
                new BookPageInfo
                (
                    "The ash itself is",
                    "mined from areas",
                    "rich in sulfuric",
                    "minerals. It is not",
                    "merely the ash that",
                    "is magical, but how",
                    "it interacts with",
                    "other reagents."
                ),
                new BookPageInfo
                (
                    "When combined",
                    "with other reagents",
                    "in specific recipes,",
                    "Sulfurous Ash can",
                    "provide the basis",
                    "for powerful spells,",
                    "including those of",
                    "fire and energy."
                ),
                new BookPageInfo
                (
                    "Caution is advised",
                    "while handling raw",
                    "Sulfurous Ash as it",
                    "is quite volatile.",
                    "Proper storage in",
                    "sealed jars is",
                    "recommended to",
                    "maintain potency."
                ),
				new BookPageInfo
				(
					"Gathering the Ash",
					"Sulfurous Ash is",
					"commonly found near",
					"volcanic areas and",
					"caves rich in sulfur",
					"minerals. Always use",
					"a wooden scoop to",
					"collect the ash."
				),
				new BookPageInfo
				(
					"Potency Levels",
					"The potency of the",
					"ash can vary based",
					"on its origin. The",
					"darker the color, the",
					"more potent it tends",
					"to be. Always verify",
					"potency before use."
				),
				new BookPageInfo
				(
					"Historical Uses",
					"In ancient times,",
					"Sulfurous Ash was",
					"used not only in",
					"spellcasting but also",
					"in rituals and",
					"alchemy, serving as",
					"a versatile reagent."
				),
				new BookPageInfo
				(
					"In Combative Magic",
					"Ash is integral in",
					"spells of destruction",
					"and energy. Mages",
					"rely on its innate",
					"properties to focus",
					"and amplify their",
					"destructive spells."
				),
				new BookPageInfo
				(
					"In Defensive Magic",
					"While less common,",
					"ash is also used in",
					"wards and barriers.",
					"Combined with other",
					"reagents, it can",
					"strengthen spells of",
					"protection."
				),
				new BookPageInfo
				(
					"Concluding Notes",
					"Always respect the",
					"reagents you use and",
					"understand their",
					"limits. The misuse of",
					"Sulfurous Ash can",
					"lead to unintended",
					"consequences."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MagicalPropertiesOfSulfurousAsh() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Magical Properties of Sulfurous Ash");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Magical Properties of Sulfurous Ash");
        }

        public MagicalPropertiesOfSulfurousAsh(Serial serial) : base(serial)
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
