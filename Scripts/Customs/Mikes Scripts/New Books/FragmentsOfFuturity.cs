using System;
using Server;

namespace Server.Items
{
    public class FragmentsOfFuturity : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Fragments of Futurity", "Lorena the Seer",
            new BookPageInfo
            (
                "In the depths of",
                "time, where past,",
                "present, and future",
                "merge into a",
                "tapestry of",
                "possibilities, I,",
                "Lorena the Seer,",
                "have glimpsed the"
            ),
            new BookPageInfo
            (
                "Fragments of",
                "Futurity. These are",
                "the scattered",
                "prophecies and",
                "visions that have",
                "graced my visions.",
                "Each word etched",
                "in the sands of time,"
            ),
            new BookPageInfo
            (
                "waiting for those",
                "with the courage to",
                "read them.",
                "",
                "Within these pages,",
                "you will find visions",
                "of kingdoms rising and",
                "falling, heroes"
            ),
            new BookPageInfo
            (
                "emerging from the",
                "shadows, and",
                "cataclysms that",
                "threaten to reshape",
                "the world. But",
                "remember, the future",
                "is a realm of",
                "uncertainty."
            ),
            new BookPageInfo
            (
                "The words within",
                "may guide your",
                "choices, but they are",
                "not destiny carved in",
                "stone. For the",
                "Fragments of Futurity",
                "are like whispers",
                "from a distant"
            ),
            new BookPageInfo
            (
                "oracle, offering",
                "insight but leaving",
                "room for free will.",
                "",
                "Read on, dear",
                "adventurer, and",
                "embrace the wisdom",
                "contained within"
            ),
            new BookPageInfo
            (
                "these pages. For the",
                "future is a canvas",
                "waiting for the",
                "strokes of your",
                "choices, and you",
                "alone hold the",
                "brush of destiny.",
                ""
            ),
            new BookPageInfo
            (
                "Lorena the Seer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your path be",
                "filled with clarity",
                "and purpose."
            ),
			            new BookPageInfo
            (
                "Visions of battles",
                "fought on distant",
                "shores, where heroes",
                "rise to meet their",
                "destiny. In the",
                "clash of swords and",
                "the fury of",
                "adversaries, greatness"
            ),
            new BookPageInfo
            (
                "is forged. Will you",
                "stand among them,",
                "or shall you forge",
                "your own path?",
                "",
                "The mists of time",
                "shroud the answers,",
                "but within you lies"
            ),
            new BookPageInfo
            (
                "the power to shape",
                "your own",
                "Futurity.",
                "",
                "A city, gleaming like",
                "a jewel, rises from",
                "the sands of a",
                "desert. Its spires"
            ),
            new BookPageInfo
            (
                "touch the heavens,",
                "and its people thrive",
                "in harmony. But",
                "beneath the beauty",
                "lies a secret, a",
                "threat that may",
                "consume all they",
                "hold dear."
            ),
            new BookPageInfo
            (
                "Unveiling the truth",
                "requires courage",
                "and sacrifice. Will",
                "you be the one to",
                "reveal the",
                "Fragments of",
                "Futurity hidden",
                "beneath the surface?"
            ),
            new BookPageInfo
            (
                "A lone figure stands",
                "at the crossroads,",
                "faced with a",
                "decision that will",
                "shape the destiny of",
                "nations. Two paths",
                "stretch before them,",
                "each fraught with"
            ),
            new BookPageInfo
            (
                "peril and promise.",
                "The choice is",
                "yours to make.",
                "",
                "The Threads of Fate",
                "weave a tapestry",
                "that binds us all.",
                "Your actions, like"
            ),
            new BookPageInfo
            (
                "threads in a grand",
                "loom, influence the",
                "pattern of the",
                "future. Choose",
                "wisely, for the",
                "Fragments of",
                "Futurity are in your",
                "hands."
            ),
            new BookPageInfo
            (
                "The final prophecy",
                "is veiled in",
                "mystery, for the",
                "future is ever",
                "changing, shaped by",
                "the choices we",
                "make. Will you",
                "embrace the unknown,"
            ),
            new BookPageInfo
            (
                "forge your own",
                "destiny, and unravel",
                "the secrets of the",
                "Fragments of",
                "Futurity?",
                "",
                "Lorena the Seer",
                DateTime.Now.ToString("t")
            ),
            new BookPageInfo
            (
                DateTime.Now.ToString("d"),
                "The journey",
                "continues, dear",
                "reader. The future",
                "awaits, filled with",
                "adventure and",
                "uncertainty.",
                "",
                "May your path be"
            ),
            new BookPageInfo
            (
                "filled with purpose",
                "and your choices",
                "lead to greatness.",
                "For in the realm of",
                "Futurity, the",
                "destinies of all are",
                "interwoven, and",
                "yours is but a"
            ),
            new BookPageInfo
            (
                "single thread in the",
                "grand tapestry of",
                "time.",
                "",
                "Read on, and",
                "embrace the",
                "Fragments of",
                "Futurity."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public FragmentsOfFuturity() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Fragments of Futurity");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Fragments of Futurity");
        }

        public FragmentsOfFuturity(Serial serial) : base(serial)
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
