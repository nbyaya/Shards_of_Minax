using System;
using Server;

namespace Server.Items
{
    public class GuideToNonHumanHorticulture : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Guide to Non-Human Horticulture", "Thornbranch the Green",
            new BookPageInfo
            (
                "In my many years as a",
                "student of the earth, I",
                "have come to learn the",
                "secrets of flora not",
                "of human ken. This",
                "volume is intended",
                "to share my verdant",
                "wisdom with those"
            ),
            new BookPageInfo
            (
                "who seek to nurture",
                "and grow the rarest",
                "of botanicals. For",
                "each plant herein",
                "described is no",
                "ordinary sprout,",
                "but rather a species",
                "borne of magic."
            ),
            new BookPageInfo
            (
                "The Whispering Bloom,",
                "for one, requires not",
                "water nor sun, but",
                "the murmured secrets",
                "of a confidant. In",
                "turn, it blossoms",
                "with petals that",
                "carry the weight"
            ),
            new BookPageInfo
            (
                "of sound and silence.",
                "Then there's the",
                "Ghastly Aconite, a",
                "flower that thrives",
                "on spectral energy,",
                "blooming fully only",
                "under the ghostly",
                "light of a phantom moon."
            ),
            new BookPageInfo
            (
                "Caring for such",
                "unnatural botany is",
                "no small task; thus,",
                "this guide will serve",
                "as your grimoire",
                "for gardening beyond",
                "the mundane. From",
                "soil preparation to"
            ),
            new BookPageInfo
            (
                "the peculiar pollination",
                "methods of the Arcane",
                "Lotus, every page is",
                "ripe with my hard-",
                "earned knowledge.",
                "",
                "Let us begin with",
                "the elemental earth,"
            ),
            new BookPageInfo
            (
                "which is not merely",
                "dirt, but the lifeblood",
                "of these wondrous",
                "plants. One must mix",
                "ash from a phoenix",
                "feather, and the",
                "powdered bones of",
                "a basilisk..."
            ),
            new BookPageInfo
            (
                // Continue with specific instructions for each plant.
            ),
            // Additional pages would continue the instructions, lore, and care tips for each mystical plant.
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Thornbranch the Green",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your gardens be",
                "ever lush and your",
                "harvests bountiful."
            ),
			// ... Previous code ...

            // Page 7
            new BookPageInfo
            (
                "hallowed griffon's guile.",
                "Such soil will bind",
                "the roots in magical",
                "fervor, granting vigor",
                "to even the most",
                "reluctant sprout."
            ),
            // Page 8
            new BookPageInfo
            (
                "Consider the Laughing",
                "Vines, which coil 'round",
                "the trees of elder",
                "woods. Their care is",
                "one of mirth, for only",
                "the sound of genuine",
                "laughter will coax",
                "them to flower."
            ),
            // Page 9
            new BookPageInfo
            (
                "Next, the Duskrose,",
                "shrouded in the enigma",
                "of twilight. These buds",
                "open only as day",
                "melds into night,",
                "requiring the delicate",
                "balance of light",
                "found only at dusk."
            ),
            // Page 10
            new BookPageInfo
            (
                "The Duskrose's petals",
                "hold a hue akin to",
                "the last rays of",
                "sunset, a radiant",
                "amber merging into",
                "a deep purple akin",
                "to the approaching",
                "night sky."
            ),
            // Page 11
            new BookPageInfo
            (
                "Onwards to the",
                "Crimson Whisper, a",
                "plant so sensitive",
                "that it recoils from",
                "the coarse touch of",
                "common gloves. Bare",
                "fingers must tend to",
                "its delicate stems."
            ),
            // Page 12
            new BookPageInfo
            (
                "Tread lightly around",
                "the Slumberthorn, its",
                "fragrance capable",
                "of inducing a sleep",
                "so deep that even",
                "elves find themselves",
                "succumbing to dreams",
                "within its presence."
            ),
            // Page 13
            new BookPageInfo
            (
                "Beware the Tempest",
                "Bloom, a blossom that",
                "feeds not on sunlight,",
                "but the raw fury of",
                "storms. Its petals",
                "shimmer only when",
                "thunder cracks the",
                "very air itself."
            ),
            // Page 14
            new BookPageInfo
            (
                "And then there is the",
                "Sylph's Breath, an",
                "ethereal vine that",
                "climbs through the",
                "air, unsupported,",
                "following the whims",
                "of the zephyrs and",
                "breezes it adores."
            ),
            // Page 15
            new BookPageInfo
            (
                "The final chapter is",
                "reserved for the",
                "rarest of them all:",
                "the Nightshade Lotus,",
                "blooming only in",
                "the deepest shadow,",
                "its petals alight with",
                "a soft, otherworldly"
            ),
            // Page 16
            new BookPageInfo
            (
                "glow. Cultivating such",
                "a marvel requires",
                "patience and a touch",
                "of lunar enchantment,",
                "bestowing upon the",
                "grower a sense of",
                "accomplishment like",
                "no other."
            ),
            // Add as many pages as needed to complete the guide.
            new BookPageInfo
            (
            ),
            // Last page
            new BookPageInfo
            (
                "Thornbranch the Green",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your gardens be",
                "ever lush and your",
                "harvests bountiful."
            )

// ... Rest of the code ...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public GuideToNonHumanHorticulture() : base(false)
        {
            // Set the hue to a green color to match the horticulture theme
            Hue = 0x48C;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Guide to Non-Human Horticulture");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Guide to Non-Human Horticulture");
        }

        public GuideToNonHumanHorticulture(Serial serial) : base(serial)
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
