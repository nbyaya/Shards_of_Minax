using System;
using Server;

namespace Server.Items
{
    public class SevenStepsToAscension : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Seven Steps to Ascension", "R.J. Neuman",
            new BookPageInfo
            (
                "Seven Steps to",
                "Ascension:",
                "Leveraging Your Soul",
                "Assets",
                "",
                "\"Don’t just pay your",
                "debt. Monetize it.\"",
                "- R.J. Neuman"
            ),
            new BookPageInfo
            (
                "Welcome, visionary.",
                "",
                "You hold in your",
                "hands the first tool",
                "on your journey to",
                "Ascension. Many fear",
                "their debts, but you",
                "will learn to profit"
            ),
            new BookPageInfo
            (
                "from them. This is",
                "no mere faith. This is",
                "financial spirituality.",
                "",
                "Your deficits are",
                "opportunities. Your",
                "failures are down",
                "payments on future"
            ),
            new BookPageInfo
            (
                "greatness.",
                "",
                "Let us begin.",
                "",
                "Step One:",
                "Admit Your Debt.",
                "",
                "Denial limits growth."
            ),
            new BookPageInfo
            (
                "Your soul owes not",
                "just to this world, but",
                "to the cosmos itself.",
                "Recognition is the",
                "first asset.",
                "",
                "Step Two:",
                "Define Your Value."
            ),
            new BookPageInfo
            (
                "Your worth is not",
                "what you own, but",
                "what you will offer.",
                "Tithes align your",
                "spirit with Ascension.",
                "",
                "Step Three:",
                "Invest in the OT."
            ),
            new BookPageInfo
            (
                "Contribution proves",
                "commitment. The",
                "Order of Transcendence",
                "provides the only",
                "true ledger.",
                "",
                "Step Four:",
                "Eliminate Doubt."
            ),
            new BookPageInfo
            (
                "Skepticism is",
                "spiritual debt. Trust",
                "the wisdom of the",
                "Hierarchy. Trust in",
                "the Broken Star.",
                "",
                "Step Five:",
                "Leverage Your"
            ),
            new BookPageInfo
            (
                "Connections.",
                "",
                "Bring others to the OT.",
                "Growth multiplies",
                "assets. Their success",
                "is your dividend.",
                "",
                "Step Six:",
                "Accept Correction."
            ),
            new BookPageInfo
            (
                "Correction is not",
                "punishment. It is",
                "refinement. Each",
                "error is a chance to",
                "increase cosmic yield.",
                "",
                "Step Seven:",
                "Ascend or Stagnate."
            ),
            new BookPageInfo
            (
                "Stagnation is the",
                "death of potential.",
                "Continue investing",
                "spiritually and",
                "materially.",
                "",
                "Only those who ascend",
                "shall inherit eternity."
            ),
            new BookPageInfo
            (
                "Congratulations on",
                "taking your first",
                "step.",
                "",
                "To proceed to",
                "advanced teachings,",
                "submit your next",
                "investment to your",
                "local OT Auditor."
            ),
            new BookPageInfo
            (
                "Remember:",
                "",
                "\"Obligation is",
                "opportunity. Debt is",
                "growth.\"",
                "",
                "- R.J. Neuman",
                "Founder & Visionary"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SevenStepsToAscension() : base(false)
        {
            Hue = 1175; // OT color theme (purple/void-like)
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Seven Steps to Ascension");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Seven Steps to Ascension");
        }

        public SevenStepsToAscension(Serial serial) : base(serial)
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
