using System;
using Server;

namespace Server.Items
{
    public class TestimoniesOfTheAscended : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Testimonies of the Ascended", "Compiled by R.J. Neuman",
            new BookPageInfo
            (
                "“Our satisfied",
                "initiates share their",
                "journeys toward",
                "Ascension and",
                "abundant cosmic",
                "wealth. May their",
                "words inspire your",
                "own elevation.”"
            ),
            new BookPageInfo
            (
                "— R.J. Neuman,",
                "Founder and Visionary"
            ),
            new BookPageInfo
            (
                "\"Before OT, I was",
                "burdened with debt,",
                "sickness, and a lack",
                "of purpose. Now, I’m",
                "burdened only with",
                "opportunity and",
                "spiritual revenue!\"",
                "- Vara T."
            ),
            new BookPageInfo
            (
                "\"They said I was a",
                "lost cause. But then",
                "I leveraged my",
                "existential liabilities.",
                "Now I own not one,",
                "but TWO carts of",
                "beets. Thank you,",
                "Broken Star.\""
            ),
            new BookPageInfo
            (
                "- Harrik P."
            ),
            new BookPageInfo
            (
                "\"Ascension isn't just",
                "a concept—it's a",
                "lifestyle. Ever since",
                "joining OT, I've",
                "experienced spiritual",
                "growth, physical",
                "resilience, and",
                "excellent posture.\""
            ),
            new BookPageInfo
            (
                "- Selara V."
            ),
            new BookPageInfo
            (
                "\"The Broken Star",
                "healed my boils,",
                "doubled my fish",
                "catch, and even",
                "got my neighbor’s",
                "dog to stop",
                "barking at me.",
                "Praise OT!\""
            ),
            new BookPageInfo
            (
                "- Brolin K."
            ),
            new BookPageInfo
            (
                "\"After ascending to",
                "OT 7, I can now",
                "calculate my",
                "spiritual credit score",
                "in my head. The",
                "number is high,",
                "and so is my",
                "confidence.\""
            ),
            new BookPageInfo
            (
                "- Fenros Q."
            ),
            new BookPageInfo
            (
                "\"People mocked me.",
                "They called OT a",
                "pyramid. But what’s",
                "wrong with pyramids?",
                "They’ve stood for",
                "thousands of years.",
                "Now, so shall I.\""
            ),
            new BookPageInfo
            (
                "- Jorn T."
            ),
            new BookPageInfo
            (
                "\"Since my ascension,",
                "I sleep like a",
                "rock and my dreams",
                "are tax-deductible.",
                "OT is the best",
                "investment I’ve",
                "ever made.\""
            ),
            new BookPageInfo
            (
                "- Caldra W."
            ),
            new BookPageInfo
            (
                "\"The Broken Star",
                "revealed to me that",
                "existence is a",
                "portfolio. I have",
                "diversified my",
                "assets into faith,",
                "compliance, and",
                "contribution.\""
            ),
            new BookPageInfo
            (
                "- Sythrel L. (now OT 4)"
            ),
            new BookPageInfo
            (
                "\"I was skeptical.",
                "Now I am rich.",
                "Enough said.\""
            ),
            new BookPageInfo
            (
                "- Anonymous (per",
                "R.J.'s request)"
            ),
            new BookPageInfo
            (
                "\"Ascension brought",
                "me many blessings:",
                "clarity of purpose,",
                "financial growth,",
                "and my eczema",
                "cleared up. Truly,",
                "the Broken Star",
                "shines.\""
            ),
            new BookPageInfo
            (
                "- Yavrik S."
            ),
            new BookPageInfo
            (
                "\"Every tithe is a",
                "step toward cosmic",
                "creditworthiness.",
                "Thank you, R.J., for",
                "showing me the",
                "path to spiritual",
                "liquidity.\""
            ),
            new BookPageInfo
            (
                "- Morthal V."
            ),
            new BookPageInfo
            (
                "\"I cannot explain",
                "the wonders I’ve",
                "seen. Mostly because",
                "the NDAs at OT 8",
                "are very strict.\""
            ),
            new BookPageInfo
            (
                "- Khar’Vhul",
                "(OT 3 testimonial)"
            ),
            new BookPageInfo
            (
                "\"Even my parrot has",
                "ascended. He repeats",
                "the phrases: 'All is",
                "compliance' and",
                "'Tithe today!'\""
            ),
            new BookPageInfo
            (
                "- Tharvis R."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TestimoniesOfTheAscended() : base(false)
        {
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Testimonies of the Ascended");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Testimonies of the Ascended");
        }

        public TestimoniesOfTheAscended(Serial serial) : base(serial)
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
