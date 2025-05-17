using System;
using Server;

namespace Server.Items
{
    public class UnlockingTheBrokenStar : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Unlocking the Broken Star", "R.J. Neuman",
            new BookPageInfo
            (
                "Unlocking the Broken Star:",
                "Harnessing the Hidden Forces",
                "of Wealth & Will",
                "",
                "By R.J. Neuman",
                "",
                "\"Only the worthy perceive",
                "the pattern.\""
            ),
            new BookPageInfo
            (
                "Chapter 1:",
                "The Star and Your",
                "Soul Ledger",
                "",
                "The Broken Star is both",
                "above you and *within*",
                "you. Just as the cosmos",
                "spins in divine cycles,"
            ),
            new BookPageInfo
            (
                "so too does your fortune.",
                "By aligning your Will",
                "with the Star’s pattern,",
                "you leverage cosmic",
                "currents into tangible",
                "wealth. Ignorance is",
                "poverty. Awareness is",
                "prosperity."
            ),
            new BookPageInfo
            (
                "Chapter 2:",
                "The Pattern of Profit",
                "",
                "Note the Star’s sigil:",
                "an intersecting web of",
                "triangles and dollar",
                "signs. Ancient texts",
                "(*Neuman Codex*, p. 4)"
            ),
            new BookPageInfo
            (
                "confirm this geometry",
                "represents the *law* of",
                "spiritual investment:",
                "",
                "\"Expend to ascend.\"",
                "",
                "Every tithe paid into the",
                "OT unlocks latent cosmic"
            ),
            new BookPageInfo
            (
                "assets. Debt is not",
                "a burden. Debt is",
                "momentum.",
                "",
                "(*Source: Master Ekon’s",
                "Scrolls of Fiscal",
                "Mysticism, allegedly lost",
                "at sea, 617 AE.*)"
            ),
            new BookPageInfo
            (
                "Chapter 3:",
                "Will as Capital",
                "",
                "The unworthy hoard Will.",
                "The wise *invest* it.",
                "",
                "Your decisions accrue",
                "spiritual interest."
            ),
            new BookPageInfo
            (
                "By choosing the Order’s",
                "path, you amplify your",
                "Will. Let the lesser folk",
                "resist change. They are",
                "poor in Will and soon to",
                "be poor in all things.",
                "",
                "(*Ref: The Lost Gospel"
            ),
            new BookPageInfo
            (
                "of Lendor, Chapter 8,",
                "\"The Dividend of Will\"*)"
            ),
            new BookPageInfo
            (
                "Chapter 4:",
                "Transcendence Through",
                "Compliance",
                "",
                "Opposition is an",
                "expensive habit.",
                "",
                "Tithing purges inefficiency.",
                "Obedience manifests power."
            ),
            new BookPageInfo
            (
                "Remember:",
                "Compliance is a credit",
                "line to ascension.",
                "",
                "(*Note: All percentages",
                "of spiritual growth",
                "are illustrative and",
                "non-binding. Terms"
            ),
            new BookPageInfo
            (
                "subject to celestial",
                "market fluctuations.*)"
            ),
            new BookPageInfo
            (
                "Chapter 5:",
                "The Broken Star Sigil",
                "",
                "Diagram (missing)",
                "",
                "The sigil’s outer ring",
                "represents the Material.",
                "The inner triangles:",
                "Will and Wealth."
            ),
            new BookPageInfo
            (
                "The central fissure?",
                "The **Opportunity Rift**.",
                "Through this gateway,",
                "followers pass from",
                "mere existence to",
                "Transcendence.",
                "",
                "(*Warning: staring into"
            ),
            new BookPageInfo
            (
                "actual cosmic fissures",
                "without authorized OT",
                "supervision may result",
                "in non-linear debt",
                "accumulation.*)"
            ),
            new BookPageInfo
            (
                "Final Thoughts:",
                "",
                "Only the worthy perceive",
                "the pattern.",
                "",
                "Are you ready to break",
                "your cycles of scarcity?",
                "",
                "Ascend. Tithe. Obey.",
                "",
                "~ R.J. Neuman"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public UnlockingTheBrokenStar() : base(false)
        {
            Hue = 1175;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Unlocking the Broken Star");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Unlocking the Broken Star");
        }

        public UnlockingTheBrokenStar(Serial serial) : base(serial)
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
