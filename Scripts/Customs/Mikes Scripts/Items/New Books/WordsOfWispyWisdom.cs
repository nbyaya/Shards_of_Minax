using System;
using Server;

namespace Server.Items
{
    public class WordsOfWispyWisdom : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Words of Wispy Wisdom", "Wendron the Wise",
            new BookPageInfo
            (
                "Within these pages lies",
                "the wisdom of Wendron,",
                "scribe and sage, who",
                "speaks in whispers to",
                "those who will listen.",
                "Let the words flow",
                "like the river's course,",
                "and may you find"
            ),
            new BookPageInfo
            (
                "clarity in its gentle",
                "babbles and fierce",
                "torrents alike.",
                "",
                "To the young mage",
                "seeking to harness",
                "the elements, know",
                "this: power is not"
            ),
            new BookPageInfo
            (
                "given; it is grown.",
                "Like the mightiest",
                "oak, it requires time,",
                "patience, and the",
                "courage to reach",
                "towards the sun.",
                "",
                "To the warrior, whose"
            ),
            new BookPageInfo
            (
                "strength lies in",
                "arms wrought of iron",
                "and will as hard as",
                "the anvils they strike,",
                "remember: even steel",
                "bends and breaks.",
                "Temper your force",
                "with flexibility."
            ),
            new BookPageInfo
            (
                "For the healers, the",
                "keepers of life's",
                "fragile flame, your",
                "gentleness is not",
                "weakness. Healing",
                "is a battle against",
                "the unseen, and",
                "oft the most heroic."
            ),
            new BookPageInfo
            (
                "Wisdom is the light",
                "that pierces the",
                "veil of darkness.",
                "In the journey of",
                "life, many are the",
                "shadows you will",
                "face. Let knowledge",
                "be your sun."
            ),
            new BookPageInfo
            (
                "And to all souls who",
                "walk these lands,",
                "know that every",
                "choice you make",
                "weaves the tapestry",
                "of your destiny.",
                "Choose well, choose",
                "with heart and honor."
            ),
            new BookPageInfo
            (
                "Finally, a truth",
                "universal and",
                "enduring: in the",
                "quest for mastery",
                "over any craft,",
                "perseverance is",
                "the chisel with",
                "which we sculpt"
            ),
            new BookPageInfo
            (
                "our dreams into",
                "reality. Do not",
                "falter, for the",
                "wisps of wisdom",
                "are found in the",
                "journey, not the",
                "destination.",
                ""
            ),
            new BookPageInfo
            (
                "Wendron the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the whispers",
                "guide you."
            ),
            new BookPageInfo
            (
                "To the merchant, whose",
                "wealth flows from the",
                "honest trade of goods",
                "and toil: prosperity",
                "thrives in the garden",
                "of fairness. Let your",
                "scales be just and your",
                "deals filled with integrity."
            ),
            new BookPageInfo
            (
                "To the bard, spinner of",
                "stories and weaver of",
                "songs: your words",
                "carry the weight of",
                "emotions and the light",
                "of inspiration. Sing the",
                "tales that give courage,",
                "laughter, and solace."
            ),
            new BookPageInfo
            (
                "To the lover of potions",
                "and brews: the cauldron",
                "is a fickle friend. Respect",
                "the simmer, honor the",
                "bubble. For within each",
                "vial lies a universe of",
                "possibility, bounded only",
                "by the brim."
            ),
            new BookPageInfo
            (
                "To those who tread the",
                "shadows, whose deals",
                "are made in silence and",
                "secrecy: remember that",
                "even the night sky is",
                "pierced by stars. Let",
                "your actions be guided",
                "by light, however dim."
            ),
            new BookPageInfo
            (
                "To the rulers and leaders,",
                "the mantles you bear are",
                "woven with the threads",
                "of responsibility. Rule",
                "with compassion and",
                "wisdom, for the power to",
                "guide is also the power",
                "to mislead."
            ),
            new BookPageInfo
            (
                "To the scholars, with",
                "noses buried in tomes",
                "and minds adrift in",
                "thought: knowledge is",
                "a beacon. Share its",
                "light freely, and let",
                "ignorance dissolve in",
                "its glow."
            ),
            new BookPageInfo
            (
                "To the explorers, whose",
                "hearts are stirred by",
                "the uncharted: every",
                "map was once a blank",
                "scroll. Dare to chart",
                "new paths, for discovery",
                "is the child of curiosity."
            ),
            new BookPageInfo
            (
                "To the humble farmer,",
                "whose toil feeds the",
                "multitude: your furrows",
                "and fields are the",
                "foundations upon which",
                "empires stand. Sow",
                "with care, reap with",
                "pride."
            ),
            new BookPageInfo
            (
                "To the devout, whose",
                "faith is a bastion",
                "in times of turmoil:",
                "let your deity’s",
                "teachings be your",
                "anchor, but remember",
                "the ocean’s expanse",
                "beyond."
            ),
            new BookPageInfo
            (
                "To the lost and the",
                "lonely, the heartbroken,",
                "and the weary: even in",
                "the darkest of nights,",
                "the dawn awaits. Hold",
                "fast to the promise",
                "of the morrow’s light."
            ),
            new BookPageInfo
            (
                "And to all children of",
                "this realm, great or",
                "small: life is the",
                "grandest of tales, and",
                "you are its authors.",
                "Write your story with",
                "bravery, kindness, and",
                "a touch of wispy wisdom."
            ),
            new BookPageInfo
            (
                "Wendron the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the whispers",
                "find you in the quiet",
                "moments."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public WordsOfWispyWisdom() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Words of Wispy Wisdom");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Words of Wispy Wisdom");
        }

        public WordsOfWispyWisdom(Serial serial) : base(serial)
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
