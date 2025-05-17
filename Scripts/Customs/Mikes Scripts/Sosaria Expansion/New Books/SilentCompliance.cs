using System;
using Server;

namespace Server.Items
{
    public class SilentCompliance : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Silent Compliance", "R.J. Neuman",
            new BookPageInfo
            (
                "Silent Compliance:",
                "The Role of the",
                "Executor",
                "",
                "By R.J. Neuman,",
                "Founder & Visionary",
                "of the Order of",
                "Transcendence."
            ),
            new BookPageInfo
            (
                "Introduction:",
                "",
                "As we ascend the",
                "pyramid of cosmic",
                "realization, we",
                "must shed the",
                "burdens of self-",
                "interest."
            ),
            new BookPageInfo
            (
                "Those who reach",
                "the threshold of",
                "OT 1 stand before",
                "a decision: cling",
                "to the self—or",
                "surrender fully",
                "to the collective",
                "purpose."
            ),
            new BookPageInfo
            (
                "The Executor is",
                "not a leader in the",
                "traditional sense.",
                "Leadership implies",
                "choice. Choice is",
                "a lower-tier",
                "illusion.",
                ""
            ),
            new BookPageInfo
            (
                "Instead, the",
                "Executor must",
                "function as the",
                "instrument of",
                "OT law. The will",
                "of the hierarchy",
                "becomes their",
                "sole directive."
            ),
            new BookPageInfo
            (
                "Clause One:",
                "An Executor owns",
                "no opinions. Only",
                "mandates.",
                "",
                "Clause Two:",
                "An Executor",
                "requires no identity."
            ),
            new BookPageInfo
            (
                "Clause Three:",
                "Compliance is",
                "silent. Verbal",
                "protest signals",
                "lower-rank",
                "attachment.",
                "",
                "Clause Four:"
            ),
            new BookPageInfo
            (
                "Emotion is a",
                "legacy cost. It",
                "must be written",
                "off at OT 1.",
                "",
                "Clause Five:",
                "Resistance is a",
                "breach of contract."
            ),
            new BookPageInfo
            (
                "Historical Note:",
                "",
                "The Executor",
                "embodies the",
                "principles of",
                "the Broken Star.",
                "Correction. Balance.",
                "Termination."
            ),
            new BookPageInfo
            (
                "The previous",
                "Executor dissolved",
                "without incident,",
                "as is proper.",
                "",
                "Legacy burdens",
                "must never be",
                "transferred."
            ),
            new BookPageInfo
            (
                "The Executor does",
                "not inherit. They",
                "replace.",
                "",
                "Do not aspire to",
                "the role for",
                "personal gain.",
                ""
            ),
            new BookPageInfo
            (
                "Aspiration implies",
                "ego. Ego implies",
                "debt.",
                "",
                "Conclusion:",
                "",
                "Silent compliance",
                "is not surrender.",
                "It is transcendence."
            ),
            new BookPageInfo
            (
                "By becoming",
                "nothing, the",
                "Executor fulfills",
                "everything.",
                "",
                "Signed,",
                "R.J. Neuman",
                "",
                "Founder & Visionary"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SilentCompliance() : base(false)
        {
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Silent Compliance");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Silent Compliance: The Role of the Executor");
        }

        public SilentCompliance(Serial serial) : base(serial)
        {
        }

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
