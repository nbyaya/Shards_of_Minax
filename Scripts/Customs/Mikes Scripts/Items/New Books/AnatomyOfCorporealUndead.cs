using System;
using Server;

namespace Server.Items
{
    public class AnatomyOfCorporealUndead : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Anatomy of Corporeal Undead", "Author Name",
                new BookPageInfo
                (
                    "The corporeal undead",
                    "exist in many forms.",
                    "Skeletons, zombies,",
                    "and wights are common",
                    "examples. This book",
                    "aims to educate on",
                    "their anatomy.",
                    "          -Author"
                ),
                new BookPageInfo
                (
                    "Skeletons are usually",
                    "bereft of flesh, and",
                    "their movement is",
                    "magically animated.",
                    "Joints and bones",
                    "show signs of wear,",
                    "but they are",
                    "surprisingly agile."
                ),
                new BookPageInfo
                (
                    "Zombies retain some",
                    "flesh, and their",
                    "movements are slow.",
                    "Decomposition affects",
                    "their abilities, but",
                    "they possess an",
                    "unnatural resilience",
                    "to damage."
                ),
                new BookPageInfo
                (
                    "Wights are more",
                    "ethereal but have",
                    "a corporeal form.",
                    "They often manifest",
                    "from spirits bound",
                    "to their earthly",
                    "remains. Unlike",
                    "other undead,"
                ),
                new BookPageInfo
                (
                    "they may possess",
                    "magical abilities,",
                    "making them more",
                    "dangerous foes.",
                    "Their skeletal",
                    "structure is often",
                    "more intact, and",
                    "their aura is chilling."
                ),
				new BookPageInfo
				(
					"Ghouls are another",
					"type of corporeal",
					"undead. They have",
					"rotting flesh and",
					"are known for their",
					"insatiable hunger",
					"for the living.",
					"Their senses are"
				),
				new BookPageInfo
				(
					"highly acute, and",
					"their digestive",
					"systems produce",
					"corrosive acids that",
					"can break down",
					"nearly anything.",
					"Due to their eating",
					"habits, ghouls often"
				),
				new BookPageInfo
				(
					"carry diseases, making",
					"them dangerous not",
					"just for their physical",
					"attacks but also for",
					"the potential of",
					"infection.",
					"",
					""
				),
				new BookPageInfo
				(
					"Mummies are undead",
					"preserved through",
					"rituals and ointments.",
					"Wrapped in bandages,",
					"their bodies are more",
					"intact compared to",
					"zombies or skeletons."
				),
				new BookPageInfo
				(
					"However, the rituals",
					"that preserve them",
					"often bind them to",
					"guard specific areas",
					"or items. This",
					"localized nature",
					"limits their mobility",
					"but increases"
				),
				new BookPageInfo
				(
					"their power when",
					"in proximity to the",
					"object or location",
					"they are bound to.",
					"",
					"Their physical form",
					"is generally stronger",
					"and more resilient"
				),
				new BookPageInfo
				(
					"compared to other",
					"undead, and they",
					"may possess magical",
					"abilities related to",
					"their binding.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"It's crucial for",
					"adventurers to",
					"understand these",
					"anatomical differences",
					"to better prepare",
					"for encounters with",
					"these dark creatures.",
					"Knowledge is the"
				),
				new BookPageInfo
				(
					"greatest weapon",
					"against the undead.",
					"May this book serve",
					"as a guide in your",
					"journey through dark",
					"and perilous lands.",
					"",
					"- The End -"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AnatomyOfCorporealUndead() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Anatomy of Corporeal Undead");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Anatomy of Corporeal Undead");
        }

        public AnatomyOfCorporealUndead(Serial serial) : base(serial)
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
