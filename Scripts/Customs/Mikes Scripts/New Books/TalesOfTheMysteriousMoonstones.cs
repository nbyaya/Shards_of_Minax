using System;
using Server;

namespace Server.Items
{
    public class TalesOfTheMysteriousMoonstones : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Tales of the Mysterious Moonstones", "Lunara the Scribe",
            new BookPageInfo
            (
                "These are the tales",
                "of the Mysterious",
                "Moonstones, gems of",
                "power and mystery.",
                "Legend speaks of their",
                "creation in the heart",
                "of a celestial event,",
                "wrought by the cosmos."
            ),
            new BookPageInfo
            (
                "Each stone is said to",
                "hold the essence of",
                "the moons. Their power",
                "is coveted by many,",
                "sorcerers and thieves",
                "alike, for it is told",
                "that he who wields the",
                "stones commands the night."
            ),
            new BookPageInfo
            (
                "The first tale speaks",
                "of the Moonstone of",
                "Eclipses, a gem that",
                "turns day to night.",
                "Possessed by a mage",
                "named Myrddin, it was",
                "lost in a battle against",
                "a rival coven."
            ),
            new BookPageInfo
            (
                "Myrddin's folly taught",
                "all that the stones",
                "demand respect, lest",
                "darkness consume the",
                "bearer. It remains",
                "hidden, its location",
                "a secret kept by",
                "the silent stars."
            ),
            new BookPageInfo
            (
                "Another stone, the",
                "Crimson Moonstone,",
                "tells a different tale.",
                "It is a heart's whisper",
                "made solid, sparking",
                "passion but also",
                "fiery wrath in the",
                "hearts of men."
            ),
            new BookPageInfo
            (
                "It was the ruin of",
                "the noble House of",
                "Lorinth, its members",
                "driven to madness",
                "by its burning call.",
                "Now, the once-majestic",
                "hall lies in ruins,",
                "a monument to desire."
            ),
            new BookPageInfo
            (
                "Not all the stones",
                "bring doom. The",
                "Azure Moonstone",
                "promises healing and",
                "clarity. A monk named",
                "Seren wielded it to",
                "end a plague that",
                "ravaged the lands."
            ),
            new BookPageInfo
            (
                "Yet, even this stone",
                "vanished, for Seren",
                "feared its power might",
                "one day be twisted",
                "for ill purpose. She",
                "cast it into the sea,",
                "entrusting its fate",
                "to the waves."
            ),
            new BookPageInfo
            (
                "The final known stone",
                "is the Onyx Moonstone,",
                "keeper of balance",
                "and order. It last",
                "rested in the hands",
                "of a just king who",
                "sought to bring peace",
                "to warring lands."
            ),
            new BookPageInfo
            (
                "But as balance dictates,",
                "for peace there must",
                "be sacrifice. The king",
                "vanished, and with him,",
                "the Onyx Moonstone.",
                "It is said that only",
                "a ruler with a pure",
                "heart may find it again."
            ),
            new BookPageInfo
            (
                "These tales of the",
                "Mysterious Moonstones",
                "are but whispers of",
                "a much grander legend.",
                "Seekers roam the",
                "shadows, yearning",
                "to uncover the truth",
                "and the stones."
            ),
            new BookPageInfo
            (
                "May fortune favor the",
                "brave and the wise.",
                "For the moonstones",
                "are not mere jewels,",
                "they are fragments",
                "of the ancient dance",
                "between sun and moon,",
                "bound by the heavens."
            ),
            new BookPageInfo
            (
                // ... Previous pages ...
            ),
            new BookPageInfo
            (
                "Beyond these stones,",
                "there are whispers of",
                "others, each with",
                "stories untold. The",
                "Verdant Moonstone,",
                "emerald in hue, is",
                "believed to bring",
                "nature's bounty."
            ),
            new BookPageInfo
            (
                "Tales speak of a",
                "druidess, Elowen,",
                "who calmed a great",
                "storm with the stone's",
                "aid. Her gratitude",
                "deep as the roots of",
                "the earth, she returned",
                "it to the soil whence"
            ),
            new BookPageInfo
            (
                "it came, and so its",
                "whispers are heard",
                "in the rustling leaves,",
                "seen in the deepest",
                "greens of ancient",
                "forests, and felt in",
                "the breath of the wild.",
                ""
            ),
            new BookPageInfo
            (
                "The Sapphire Moonstone,",
                "gleaming like the",
                "deepest ocean, is",
                "reputed to grant",
                "wisdom as fathomless",
                "as the seas. A sailor,",
                "old and weathered, used",
                "its powers to navigate"
            ),
            new BookPageInfo
            (
                "unchartered waters,",
                "drawing maps that",
                "connected worlds.",
                "Before his last voyage",
                "into the horizon's",
                "embrace, he cast the",
                "stone into the abyss,",
                "entrusting his secrets"
            ),
            new BookPageInfo
            (
                "to the ocean's depths.",
                "It is said that on a",
                "moonlit night, one",
                "might catch a glimpse",
                "of its brilliance",
                "beneath the waves,",
                "guiding lost sailors",
                "to safe harbor."
            ),
            new BookPageInfo
            (
                "Lesser known is the",
                "Amber Moonstone,",
                "warm as a summer's",
                "sunset. It promised",
                "the joy of ages,",
                "laughter that could",
                "cure the weariest of",
                "souls. A bard, known"
            ),
            new BookPageInfo
            (
                "for neither his name",
                "nor his visage but",
                "by the joy he spread,",
                "carried the stone.",
                "It vanished with him,",
                "and it is said his",
                "laughter still echoes",
                "in the places he once"
            ),
            new BookPageInfo
            (
                "walked, a legacy more",
                "enduring than memory.",
                "",
                "The Iridescent Moonstone,",
                "shifting in color and",
                "mood, holds the secret",
                "of change, of the cycles",
                "of life and the worlds."
            ),
            new BookPageInfo
            (
                "A philosopher, who",
                "sought the patterns",
                "that govern existence,",
                "wielded it. Upon",
                "understanding the",
                "ephemeral nature of",
                "reality, he let the stone",
                "travel on the winds of"
            ),
            new BookPageInfo
            (
                "change, its location",
                "as mutable as the",
                "stone itself. It is",
                "a gem that is said",
                "to be many places",
                "at once, yet cannot",
                "be found in any one.",
                ""
            ),
            new BookPageInfo
            (
                "So continues the",
                "quest for these stones,",
                "each a chapter in the",
                "endless book of the",
                "cosmos. Seekers of",
                "truth, power, balance,",
                "and beauty hunt for",
                "the moonstones, as"
            ),
            new BookPageInfo
            (
                "much a part of legend",
                "as of the very fabric",
                "of existence. Their",
                "stories intertwine",
                "with the fate of all,",
                "a tapestry woven with",
                "the threads of infinite",
                "possibilities."
            ),
            new BookPageInfo
            (
                "Tread lightly, seekers,",
                "for to hold a moonstone",
                "is to touch the essence",
                "of the extraordinary,",
                "to grasp at the elusive",
                "mysteries of the night",
                "sky, and to feel the",
                "weight of its wonders."
            ),
            new BookPageInfo
            (
                "In the light of the",
                "twinned moons, may",
                "these stories guide",
                "and inspire.",
                "",
                "Lunara the Scribe",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d")
            ),
            new BookPageInfo
            (
                "Lunara the Scribe",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In the light of the",
                "twinned moons, may",
                "these stories guide",
                "and inspire."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TalesOfTheMysteriousMoonstones() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Tales of the Mysterious Moonstones");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Tales of the Mysterious Moonstones");
        }

        public TalesOfTheMysteriousMoonstones(Serial serial) : base(serial)
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
