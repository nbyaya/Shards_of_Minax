using System;
using Server;

namespace Server.Items
{
    public class TheAxisHypothesis : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Axis Hypothesis", "R.J. Neuman",
            new BookPageInfo
            (
                "The Axis Hypothesis",
                "by R.J. Neuman",
                "",
                "A treatise for those",
                "of the Fourth Clause",
                "and above. Unauthorized",
                "possession by lower OT",
                "levels will result in"
            ),
            new BookPageInfo
            (
                "balance correction",
                "penalties.",
                "",
                "Foreword:",
                "All existence is an",
                "equation. Debts are",
                "variables. Balance is",
                "the universal law."
            ),
            new BookPageInfo
            (
                "Through ritualized",
                "corrective action, we",
                "can influence not only",
                "material fortune but",
                "the foundational",
                "forces of the cosmos.",
                "",
                "This is not theory."
            ),
            new BookPageInfo
            (
                "It is proven practice.",
                "",
                "Section 1:",
                "Understanding the",
                "Balance Vector",
                "",
                "By harmonizing intent",
                "and tithe with specific"
            ),
            new BookPageInfo
            (
                "numeric ratios, the",
                "follower becomes a",
                "node of influence.",
                "These ratios include:",
                "",
                "3.14159 — Axis of",
                "Continuity.",
                "1.61803 — Ratio of"
            ),
            new BookPageInfo
            (
                "Prosperity.",
                "0.666 — Denial of",
                "Stagnation.",
                "",
                "Applying these figures",
                "to offerings and chants",
                "accelerates correction",
                "ritual potency."
            ),
            new BookPageInfo
            (
                "Section 2:",
                "The Mass Influence",
                "Principle",
                "",
                "When a quorum of",
                "initiates performs",
                "balance correction",
                "simultaneously, the"
            ),
            new BookPageInfo
            (
                "harmonic field scales",
                "exponentially.",
                "",
                "Note: Unexpected",
                "phenomena have been",
                "observed when seven",
                "or more practitioners",
                "align chants."
            ),
            new BookPageInfo
            (
                "These include:",
                "- Flickering lights",
                "- Temporal dissonance",
                "- Manifest voices",
                "- Object displacement",
                "",
                "These are normal.",
                ""
            ),
            new BookPageInfo
            (
                "Section 3:",
                "Anomalous Results",
                "",
                "Disciples have reported",
                "visions of an inverted",
                "star, apparitions of",
                "former debtors, and",
                "geometric distortions."
            ),
            new BookPageInfo
            (
                "These are signs of",
                "successful correction,",
                "as the universe adapts",
                "to our revised",
                "equations.",
                "",
                "Record such events.",
                "They are profitable."
            ),
            new BookPageInfo
            (
                "Final Note:",
                "",
                "Do not question the",
                "outcomes. Correction",
                "operates beyond the",
                "frail comprehension of",
                "lower minds. Trust the",
                "system.",
                "",
                "Balance is destiny."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheAxisHypothesis() : base(false)
        {
            Hue = 1150; // Cosmic grey
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Axis Hypothesis");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Axis Hypothesis");
        }

        public TheAxisHypothesis(Serial serial) : base(serial)
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
