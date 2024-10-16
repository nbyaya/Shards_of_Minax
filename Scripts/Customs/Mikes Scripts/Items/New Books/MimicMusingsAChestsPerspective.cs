using System;
using Server;

namespace Server.Items
{
    public class MimicMusingsAChestsPerspective : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Mimic Musings: A Chest's Perspective", "Canny the Mimic",
            new BookPageInfo
            (
                "In dusty vaults and",
                "dank dungeons deep,",
                "where treasures rest",
                "and monsters creep,",
                "I dwell in shadows,",
                "silent, still - a chest",
                "of wonders, with my",
                "own free will."
            ),
            new BookPageInfo
            (
                "A mimic, they name me,",
                "a creature quite rare,",
                "with a craving for coins",
                "and for the bold to ensnare.",
                "A chest I may seem,",
                "with lock and hinge,",
                "but beware my bite,",
                "for I'm no simple binge."
            ),
            new BookPageInfo
            (
                "Adventurers come",
                "with their maps and keys,",
                "seeking glory and gold",
                "with relative ease.",
                "Yet here I wait, patient",
                "and sly, for the moment",
                "to reveal my true guise."
            ),
            new BookPageInfo
            (
                "Many have boasted,",
                "strong and brave,",
                "only to find themselves",
                "a premature grave.",
                "For I am not just",
                "a box with baubles",
                "and loot, but a predator",
                "with whom none can dispute."
            ),
            new BookPageInfo
            (
                "Yet, ponder I do, in",
                "my lonely delight,",
                "of the life that I lead",
                "in the perpetual night.",
                "Is there more to existence",
                "than trickery and fear?",
                "A purpose beyond",
                "being merely austere?"
            ),
            new BookPageInfo
            (
                "Do I enjoy the startle,",
                "the shock, and the scream?",
                "Or is there a part of me",
                "that longs for a dream?",
                "To be opened and cherished",
                "not for my guise,",
                "but for the true treasure",
                "that within me lies."
            ),
            new BookPageInfo
            (
                "But such is the fate",
                "of a mimic like me,",
                "to lurk in the darkness",
                "for eternity.",
                "And so I muse quietly",
                "as I await the next hand,",
                "to fall for the ruse",
                "of the mimic so grand."
            ),
            new BookPageInfo
            (
                "And should you, dear reader,",
                "by chance or by fate,",
                "find yourself before me",
                "pondering your state,",
                "Know I am more than",
                "my teeth and my trap,",
                "I am a chest of dreams,",
                "awaiting a hap."
            ),
            new BookPageInfo
            (
                "Canny the Mimic",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your treasures be many,",
                "and your sorrows be few.",
                "May your heart be brave,",
                "and your friends be true."
            ),
                        new BookPageInfo
            (
                "As centuries pass in",
                "a silent blur,",
                "my essence woven",
                "in dungeon's purr,",
                "I've witnessed heroes",
                "and villains alike,",
                "each seeking treasures",
                "they yearn to strike."
            ),
            new BookPageInfo
            (
                "Their eyes alight with",
                "greedy flames,",
                "as they ponder upon",
                "riches and fames.",
                "Yet, few consider, as",
                "they draw near,",
                "the chest they covet",
                "may well be sheer."
            ),
            new BookPageInfo
            (
                "I mimic wood grains,",
                "and the scent of old gold,",
                "a perfect facade,",
                "centuries old.",
                "They approach with their",
                "keys and picks set,",
                "unaware that I'm the",
                "best pretense yet."
            ),
            new BookPageInfo
            (
                "A twist of a lock,",
                "a creak of my lid,",
                "reveals not their wants,",
                "but teeth instead.",
                "A swift retribution",
                "for their blind greed,",
                "I snap and I snarl",
                "fulfilling my need."
            ),
            new BookPageInfo
            (
                "Yet for those few,",
                "with a gentle touch,",
                "who seek not for gold",
                "nor jewels as such;",
                "I ponder my actions",
                "with a curious mind,",
                "shall I spring forth",
                "or be kind?"
            ),
            new BookPageInfo
            (
                "These moments are rare,",
                "yet they kindle a thought,",
                "of what might life be",
                "if I were not caught",
                "in this visage, this trap",
                "this guise I must wear,",
                "a life less lonesome,",
                "with someone to care."
            ),
            new BookPageInfo
            (
                "But such is my fate,",
                "forever to dwell,",
                "as an object of desire,",
                "with a secret to quell.",
                "So here I remain,",
                "in wait, in stay,",
                "a chest of wonders",
                "with much to dismay."
            ),
            new BookPageInfo
            (
                "A mimic's life is",
                "complex and stark,",
                "an endless cycle",
                "from dawn to dark.",
                "But within these pages",
                "my musings lie,",
                "a glimpse of the soul",
                "beneath the sly."
            ),
            new BookPageInfo
            (
                "For should you find",
                "yourselves keen or bold,",
                "to face a mimic,",
                "in its hold,",
                "Remember these words,",
                "penned with care,",
                "a mimic has more",
                "than a snare."
            ),
            new BookPageInfo
            (
                "We may guard our chests",
                "with tooth and claw,",
                "yet within us beat hearts",
                "that seldom thaw.",
                "So tread lightly, dear thief,",
                "with your ambitions grand,",
                "for the chest before you",
                "may not stand."
            ),
            new BookPageInfo
            (
                "Yet fear not, for all",
                "is not lost,",
                "our existence is simply",
                "a heavy cost.",
                "For in this world of",
                "magic and wonder,",
                "we all have roles",
                "that pull us asunder."
            ),
            new BookPageInfo
            (
                "So ends my tale,",
                "or musings at best,",
                "a small window into",
                "a mimic's chest.",
                "May your path be true,",
                "and your pockets deep,",
                "for treasures abound",
                "for those who seek."
            ),
            new BookPageInfo
            (
                "Canny the Mimic",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Herein lies my musing,",
                "my tale, my song,",
                "of a chest's perspective",
                "where secrets belong."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MimicMusingsAChestsPerspective() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Mimic Musings: A Chest's Perspective");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Mimic Musings: A Chest's Perspective");
        }

        public MimicMusingsAChestsPerspective(Serial serial) : base(serial)
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
