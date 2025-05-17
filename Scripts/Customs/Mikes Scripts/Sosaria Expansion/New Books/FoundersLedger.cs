using System;
using Server;

namespace Server.Items
{
    public class FoundersLedger : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Founder's Ledger", "R.J. Neuman",
            new BookPageInfo
            (
                "One day, they’ll name",
                "a star after me. The",
                "Order of Transcendence",
                "(OT) will reshape how",
                "the world thinks about",
                "wealth and power.",
                "",
                "They’ll remember R.J."
            ),
            new BookPageInfo
            (
                "Neuman as the visionary",
                "who unlocked spiritual",
                "capitalism. Tithing. Debt.",
                "Growth. Ascension.",
                "",
                "I can *feel* it coming."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "They actually believe it.",
                "I just made this stuff up",
                "to sell the first book.",
                "",
                "It was supposed to be",
                "a side hustle."
            ),
            new BookPageInfo
            (
                "I sold the dream. They",
                "bought it. Gold rolled in.",
                "Pendants. Seminars.",
                "They wanted to believe.",
                "",
                "So I let them."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "Higher-tier members are",
                "doing things I didn’t tell",
                "them to.",
                "",
                "They started creating",
                "levels beyond what I",
                "invented."
            ),
            new BookPageInfo
            (
                "They drafted 'clauses.'",
                "Talked about cosmic",
                "equations. Some even",
                "performed rituals based",
                "on my fake diagrams.",
                "",
                "They said they could",
                "feel the Broken Star."
            ),
            new BookPageInfo
            (
                "I thought they were",
                "just overzealous.",
                "",
                "Then the lights moved."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "Last night, I saw the star",
                "they keep talking about.",
                "",
                "It moved.",
                "",
                "Stars don’t move."
            ),
            new BookPageInfo
            (
                "Not like that.",
                "",
                "Not when you look *away*",
                "and it follows you.",
                "",
                "I told them it was a sign.",
                "They cheered. Knelt.",
                "They called me a god."
            ),
            new BookPageInfo
            (
                "I smiled.",
                "I felt sick."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "If I run now, they’ll kill me.",
                "",
                "If I stay… I might really",
                "become a god.",
                "",
                "I don’t know which is worse."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "I think something else is",
                "reading this now.",
                "",
                "Every time I open the",
                "ledger, there are marks",
                "I didn’t write.",
                "",
                "Figures. Calculations."
            ),
            new BookPageInfo
            (
                "Ratios.",
                "",
                "The equations make",
                "sense when I first see",
                "them, but then they",
                "slip away.",
                "",
                "It feels like the book",
                "is finishing itself."
            ),
            new BookPageInfo
            (
                "---",
                "",
                "I keep telling myself:",
                "",
                "This was just a startup.",
                "",
                "Just a business.",
                "",
                "Just a lie."
            ),
            new BookPageInfo
            (
                "But the star keeps moving.",
                "And the numbers keep",
                "adding up.",
                "",
                "And the followers keep",
                "kneeling.",
                "",
                "And the truth is…",
                "",
                "I can’t stop it."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public FoundersLedger() : base(false)
        {
            Hue = Utility.RandomMinMax(1101, 1150);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Founder's Ledger");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Founder's Ledger");
        }

        public FoundersLedger(Serial serial) : base(serial)
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
