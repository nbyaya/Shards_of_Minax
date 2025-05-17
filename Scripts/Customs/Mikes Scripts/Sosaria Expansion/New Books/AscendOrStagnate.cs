using System;
using Server;

namespace Server.Items
{
    public class AscendOrStagnate : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ascend or Stagnate", "R.J. Neuman",
            new BookPageInfo
            (
                "Stop hoarding your",
                "potential. Invest it.",
                "",
                "To the Seeker of",
                "Wealth and Wisdom:",
                "",
                "You have already",
                "taken the first step"
            ),
            new BookPageInfo
            (
                "by *seeking*. But",
                "without action,",
                "seeking is merely",
                "stagnation in disguise.",
                "",
                "All who hoard their",
                "gold, their energy,",
                "their labor—hoard"
            ),
            new BookPageInfo
            (
                "their *potential*. They",
                "delay the inevitable:",
                "Ascension or Decay.",
                "",
                "Tithing is not loss.",
                "It is *investment*.",
                "You do not pay.",
                "You grow."
            ),
            new BookPageInfo
            (
                "Ask yourself:",
                "",
                "Would you rather be",
                "the stream that flows,",
                "or the pond that",
                "stagnates?",
                "",
                "The flowing stream"
            ),
            new BookPageInfo
            (
                "nourishes the earth.",
                "The stagnant pond",
                "breeds only decay.",
                "",
                "Your tithes empower",
                "the cosmic forces",
                "that elevate the",
                "worthy. Wealth is"
            ),
            new BookPageInfo
            (
                "energy. Energy is",
                "movement. Movement",
                "is Ascension.",
                "",
                "Those who resist the",
                "tithe resist their own",
                "growth.",
                ""
            ),
            new BookPageInfo
            (
                "Testimonies:",
                "",
                "\"I gave freely, and",
                "now I lead a trade",
                "caravan across three",
                "kingdoms.\"",
                "— Eldric the Former",
                "Fisherman"
            ),
            new BookPageInfo
            (
                "\"I tithed not only gold",
                "but wisdom. Today, I",
                "am an OT 6 Auditor.\"",
                "— Sythrel (before",
                "his sublime elevation)",
                "",
                "\"I gave my labor to",
                "the cause and rose"
            ),
            new BookPageInfo
            (
                "from pauper to",
                "property holder.\"",
                "— Arissa of Dawn",
                "",
                "(Note: R.J. Neuman",
                "personally verified",
                "these success stories.)"
            ),
            new BookPageInfo
            (
                "Statistics (certified):",
                "",
                "Phase One Ascension",
                "achieved by:",
                "",
                "78% of contributors",
                "within first year.",
                "",
                "97% within two years."
            ),
            new BookPageInfo
            (
                "(Charts available",
                "upon request.)",
                "",
                "Resist not the flow.",
                "Pay your tithes.",
                "Invest your future.",
                "Ascend.",
                "",
                "~ R.J. Neuman"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AscendOrStagnate() : base(false)
        {
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ascend or Stagnate");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ascend or Stagnate, by R.J. Neuman");
        }

        public AscendOrStagnate(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
