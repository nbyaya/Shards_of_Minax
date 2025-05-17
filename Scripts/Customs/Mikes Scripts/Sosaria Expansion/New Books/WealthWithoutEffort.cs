using System;
using Server;

namespace Server.Items
{
    public class WealthWithoutEffort : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Wealth Without Effort", "R.J. Neuman",
            new BookPageInfo
            (
                "Welcome, future",
                "entrepreneur of",
                "enlightenment! I am",
                "R.J. Neuman, your",
                "personal mentor to",
                "limitless abundance.",
                "",
                "You stand at the"
            ),
            new BookPageInfo
            (
                "threshold of untapped",
                "potential. Stop",
                "working hard. Start",
                "working *smart*.",
                "",
                "The secret? Monetize",
                "your thoughts.",
                "Leverage your"
            ),
            new BookPageInfo
            (
                "unique spiritual",
                "vibrations into",
                "marketable insight.",
                "",
                "Your soul is not a",
                "prison. It's an asset.",
                "Others toil in mines.",
                "You will mine belief."
            ),
            new BookPageInfo
            (
                "Chapter One:",
                "The Mind as Capital",
                "",
                "Ideas are currency.",
                "If you have none,",
                "borrow some. Wealthy",
                "people *delegate*",
                "their thinking."
            ),
            new BookPageInfo
            (
                "Chapter Two:",
                "The Scarcity Myth",
                "",
                "Scarcity is a lie.",
                "The universe is",
                "abundant. If you",
                "lack, it's because",
                "you're hoarding",
                "your spiritual assets."
            ),
            new BookPageInfo
            (
                "Chapter Three:",
                "Revenue Through",
                "Reverence",
                "",
                "People crave leaders.",
                "Speak with authority.",
                "If you can't be right,",
                "be convincing."
            ),
            new BookPageInfo
            (
                "Chapter Four:",
                "Diversify Your Truth",
                "",
                "Your message must",
                "appeal to all.",
                "Create levels of",
                "access. Entry fees.",
                "Advanced teachings.",
                "Personal consults."
            ),
            new BookPageInfo
            (
                "Chapter Five:",
                "Fear of Loss",
                "",
                "Never promise results.",
                "Promise *potential*.",
                "People fear missing",
                "out more than they",
                "fear failure.",
                "",
                "Upsell accordingly."
            ),
            new BookPageInfo
            (
                "Chapter Six:",
                "When in Doubt, Invent",
                "",
                "Your best ideas will",
                "be discovered *after*",
                "you've sold them.",
                "Truth is flexible.",
                "Profit is permanent."
            ),
            new BookPageInfo
            (
                "Chapter Seven:",
                "Branding the Infinite",
                "",
                "Symbols resonate.",
                "Create icons. Stars,",
                "circles, triangles.",
                "People obey what they",
                "don't understand.",
                "Use this wisely."
            ),
            new BookPageInfo
            (
                "Appendix A:",
                "Suggested Names",
                "",
                "- Ascendant Wealth",
                "- The Silver Ledger",
                "- The Cosmic Dividend",
                "- Order of Abundant",
                "Transcendence (?)"
            ),
            new BookPageInfo
            (
                "Appendix B:",
                "Emergency Excuses",
                "",
                "- 'You misunderstood'",
                "- 'Your energy was",
                "blocked'",
                "- 'The process takes",
                "time'",
                "- 'You have not tithed",
                "enough to unlock full",
                "results.'"
            ),
            new BookPageInfo
            (
                "Final Note:",
                "",
                "If you're reading",
                "this, you're already",
                "ahead of 99% of",
                "the world.",
                "",
                "Now, ascend.",
                "And collect."
            ),
            new BookPageInfo
            (
                "Draft completed:",
                "By R.J. Neuman",
                "",
                "*To be revised",
                "before publication.",
                "Add more diagrams.",
                "Consider pyramid",
                "branding.*"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public WealthWithoutEffort() : base(false)
        {
            Hue = Utility.RandomMinMax(1100, 1175); // Cult-y purples/blues
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Draft Manuscript: Wealth Without Effort");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Draft Manuscript: Wealth Without Effort");
        }

        public WealthWithoutEffort(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}
