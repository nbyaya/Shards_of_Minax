using System;
using Server;

namespace Server.Items
{
    public class OrcishFishingTechniques : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Orcish Fishing Techniques", "Gruumsh",
                new BookPageInfo
                (
                    "Dis book teach yoo",
                    "how to fish like a",
                    "real orc. Gruumsh",
                    "show yoo da way!",
                    "",
                    "",
                    "",
                    "        -Gruumsh"
                ),
                new BookPageInfo
                (
                    "First yoo need big",
                    "pole. Little pole fer",
                    "humans! Orc need",
                    "strong pole for big",
                    "fish!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Second, find water.",
                    "But not clean water!",
                    "Fish like da muddy",
                    "water. Smell like",
                    "home!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Third, use bait. But",
                    "not little worm! Use",
                    "dead goblin or rat.",
                    "Fish like da smell!",
                    "",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Last, throw pole in",
                    "water. Wait for fish.",
                    "When pole shake,",
                    "pull hard! Now yoo",
                    "have fish for tribe!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Advanced Techneek:",
                    "Fishin' in Storm!",
                    "Fish scared of",
                    "thunder. Make big",
                    "noise in water, fish",
                    "come to see, den",
                    "catch!",
                    ""
                ),
                new BookPageInfo
                (
                    "Da Moon Trick:",
                    "Fish in night when",
                    "full moon. Fish see",
                    "bait, can't resist!",
                    "",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Da Chum Bucket:",
                    "Put lot of meat and",
                    "bones in water. Fish",
                    "smell and come. Den",
                    "easy catch!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Net Fishin':",
                    "Use big net for many",
                    "fish. But careful!",
                    "Net can break. Need",
                    "strong orc to pull!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Fishin' wit Magic:",
                    "Use shaman magic",
                    "to call fish. But no",
                    "tell tribe! Keep",
                    "secret, get more",
                    "fish for you!",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Spears and Tridents:",
                    "If no like wait,",
                    "use spear. Jump in",
                    "water, stab fish!",
                    "Fun and quick!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Da Big Fish:",
                    "Some fish too big",
                    "for pole or net. For",
                    "them, use big trap.",
                    "Need many orcs!",
                    "",
                    "",
                    ""
                ),
                new BookPageInfo
                (
                    "Final Words:",
                    "Now yoo ready for",
                    "be master orc",
                    "fisherman. Go make",
                    "tribe proud!",
                    "",
                    "",
                    "        -Gruumsh"
                )

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishFishingTechniques() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Fishing Techniques");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Fishing Techniques");
        }

        public OrcishFishingTechniques(Serial serial) : base(serial)
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
