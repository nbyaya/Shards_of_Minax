using System;
using Server;

namespace Server.Items
{
    public class OrcishTailoringBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Tailoring", "Grummog",
                new BookPageInfo
                (
                    "This book is an",
                    "insight into the art",
                    "of Orcish Tailoring,",
                    "the craft of making",
                    "armor and clothing",
                    "from animal hides",
                    "and fabrics.",
                    "          -Grummog"
                ),
                new BookPageInfo
                (
                    "While many consider",
                    "orcs to be primitive,",
                    "our methods of",
                    "tailoring have been",
                    "honed over the",
                    "generations. We use",
                    "every part of the",
                    "animal."
                ),
                new BookPageInfo
                (
                    "The leather obtained",
                    "from boars, bats, and",
                    "wolves serve as the",
                    "base material for our",
                    "armor. Proper",
                    "tanning and treating",
                    "of the leather is",
                    "essential."
                ),
                new BookPageInfo
                (
                    "Bone fragments and",
                    "teeth often serve as",
                    "buttons and clasps.",
                    "When properly sewn,",
                    "these add an extra",
                    "layer of durability",
                    "and intimidate our",
                    "enemies."
                ),
                new BookPageInfo
                (
                    "For those with skill,",
                    "embroidering runes",
                    "into the fabric can",
                    "provide additional",
                    "magical protection.",
                    "This, however, is a",
                    "trade secret passed",
                    "down only to skilled"
                ),
                new BookPageInfo
                (
                    "Orcish tailors. The",
                    "craft is sacred and",
                    "we hold competitions",
                    "to determine the best",
                    "tailor among us. The",
                    "winner earns respect",
                    "and often becomes",
                    "a tribal leader."
                ),
				new BookPageInfo
				(
					"In this section, let's",
					"discuss the importance",
					"of thread quality. Orcs",
					"prefer to use gut",
					"threads made from",
					"animal intestines. It's",
					"not just sturdy, but",
					"also has natural"
				),
				new BookPageInfo
				(
					"resistance to decay.",
					"The process of making",
					"gut thread involves",
					"cleaning, stretching,",
					"and twisting the",
					"intestines to achieve",
					"the desired thickness.",
					"Then, it's left to dry."
				),
				new BookPageInfo
				(
					"For the less battle-",
					"oriented garments, we",
					"utilize plant fibers.",
					"These are common for",
					"inner layers, as they",
					"are more comfortable",
					"against the skin and",
					"are easier to dye."
				),
				new BookPageInfo
				(
					"Speaking of dyes, orcs",
					"have a rich tradition",
					"of natural dyeing",
					"methods. Berries,",
					"roots, and barks are",
					"common sources of",
					"color. Each tribe has",
					"its own signature hue."
				),
				new BookPageInfo
				(
					"When crafting for war,",
					"durability is the main",
					"concern. Stitches are",
					"double-layered, and",
					"joints are reinforced",
					"with extra patches",
					"of leather or even",
					"metal plates."
				),
				new BookPageInfo
				(
					"We also utilize fur",
					"for added insulation",
					"during harsh winters.",
					"Furs from animals like",
					"bears and wolves are",
					"common. Special",
					"treatments help to",
					"make them water-"
				),
				new BookPageInfo
				(
					"resistant, extending",
					"their usefulness in",
					"varied conditions.",
					"For shamans and",
					"chieftains, garments",
					"may also incorporate",
					"sacred materials,",
					"like phoenix feathers"
				),
				new BookPageInfo
				(
					"or dragon scales.",
					"Such garments are",
					"considered holy and",
					"are only worn during",
					"sacred ceremonies or",
					"epic battles. They",
					"believe such items",
					"bestow great power."
				),
				new BookPageInfo
				(
					"Tailoring in the orc",
					"community is not just",
					"a craft, but an art",
					"and a tradition that",
					"binds us. We take",
					"pride in every stitch,",
					"every dye, and every",
					"garment we create."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishTailoringBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Tailoring");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Tailoring");
        }

        public OrcishTailoringBook(Serial serial) : base(serial)
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
