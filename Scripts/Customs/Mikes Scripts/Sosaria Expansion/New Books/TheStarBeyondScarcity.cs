using System;
using Server;

namespace Server.Items
{
    public class TheStarBeyondScarcity : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Star Beyond Scarcity", "R.J. Neuman",
            new BookPageInfo
            (
                "Your Soul is a Ledger.",
                "Balance It.",
                "",
                "Welcome, Seeker.",
                "You have taken your",
                "first step toward",
                "transcending scarcity.",
                "",
                "What if I told you:"
            ),
            new BookPageInfo
            (
                "Poverty is a mindset.",
                "Suffering is a debt.",
                "And YOU are the asset.",
                "",
                "The universe is not",
                "cold or indifferent.",
                "It is a market. Those",
                "who invest prosper."
            ),
            new BookPageInfo
            (
                "But the foolish hoard.",
                "They clutch their",
                "limited gold, their",
                "fragile egos, their",
                "petty doubts.",
                "",
                "This is scarcity.",
                "",
                "Scarcity is death."
            ),
            new BookPageInfo
            (
                "The Broken Star",
                "teaches us otherwise.",
                "",
                "Abundance is not",
                "given. It is grown.",
                "By leveraging your",
                "spiritual capital—your",
                "time, wealth, and will."
            ),
            new BookPageInfo
            (
                "The Order of",
                "Transcendence (OT)",
                "offers the ONLY",
                "system to harness this",
                "truth.",
                "",
                "Wealth is a symptom.",
                "Spiritual health is the"
            ),
            new BookPageInfo
            (
                "cause.",
                "",
                "As your spiritual",
                "ledger grows, so too",
                "will your material",
                "wealth.",
                "",
                "Energy in. Wealth out.",
                "Transcendence follows."
            ),
            new BookPageInfo
            (
                "Through tithe and",
                "practice, you will:",
                "",
                "- Unlock cosmic",
                "   equilibrium.",
                "- Monetize karmic",
                "   deficits.",
                "- Reinvest personal",
                "   setbacks."
            ),
            new BookPageInfo
            (
                "In Phase One:",
                "You will pledge your",
                "first tithe and attend",
                "weekly Ascension",
                "Seminars.",
                "",
                "In Phase Two:",
                "You will recruit others"
            ),
            new BookPageInfo
            (
                "into the growth",
                "pyramid.",
                "",
                "In Phase Three:",
                "You will receive",
                "personal guidance",
                "from higher OT",
                "investors.",
                "",
                "Success multiplies."
            ),
            new BookPageInfo
            (
                "Critics say:",
                "\"But isn't this just",
                "wealth hoarding?\"",
                "",
                "No. We do not hoard.",
                "We circulate. We",
                "expand.",
                "",
                "They also say:"
            ),
            new BookPageInfo
            (
                "\"Isn't this a pyramid?\"",
                "",
                "Yes. And what is a",
                "pyramid if not the",
                "oldest, most",
                "enduring structure in",
                "human history?",
                ""
            ),
            new BookPageInfo
            (
                "The pharaohs knew.",
                "The ancient magi knew.",
                "The Broken Star knows.",
                "",
                "Rise, Seeker.",
                "Your journey begins.",
                "",
                "Sign below:",
                "\"I accept the Ledger."
            ),
            new BookPageInfo
            (
                "My soul will balance.",
                "I will ascend.\"",
                "",
                "Signature: __________",
                "",
                "Welcome to the",
                "Order of",
                "Transcendence.",
                "R.J. Neuman"
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheStarBeyondScarcity() : base(false)
        {
            Hue = Utility.RandomMinMax(1100, 1150);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Star Beyond Scarcity");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Star Beyond Scarcity");
        }

        public TheStarBeyondScarcity(Serial serial) : base(serial)
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
