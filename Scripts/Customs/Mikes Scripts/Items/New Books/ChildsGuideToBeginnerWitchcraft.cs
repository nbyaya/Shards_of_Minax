using System;
using Server;

namespace Server.Items
{
    public class ChildsGuideToBeginnerWitchcraft : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Child's Guide to Beginner Witchcraft", "Winnie the Wise",
            new BookPageInfo
            (
                "Welcome, young apprentice,",
                "to the first steps upon",
                "a magical journey.",
                "Witchcraft is the art",
                "of bending the natural",
                "world to one's will,",
                "and this guide shall",
                "be your trusted aid."
            ),
            new BookPageInfo
            (
                "Lesson One: Herbs",
                "and Roots",
                "To start your path,",
                "learn the green tongue—",
                "the language of plants.",
                "They are friends and",
                "ingredients. From",
                "healing salves to"
            ),
            new BookPageInfo
            (
                "enchanted teas,",
                "nature's bounty is",
                "yours to wield.",
                "Chamomile calms,",
                "while mint invigorates,",
                "and nightshade... well,",
                "that is for another,",
                "more advanced lesson."
            ),
            new BookPageInfo
            (
                "Lesson Two: Stones",
                "and Crystals",
                "The earth offers",
                "up crystals and",
                "stones of power.",
                "Amethysts for psychic",
                "insight, quartz to",
                "amplify your spells,"
            ),
            new BookPageInfo
            (
                "and agates to ground",
                "you to the earth.",
                "Wear them as talismans,",
                "or use them in your",
                "circle of magic, but",
                "remember, each stone",
                "has its own voice.",
                "Listen well."
            ),
            new BookPageInfo
            (
                "Lesson Three: The",
                "Elements",
                "Fire, Water, Air, and",
                "Earth; the building",
                "blocks of the universe,",
                "and the tools of",
                "a witch. Practice",
                "calling to each,"
            ),
            new BookPageInfo
            (
                "feeling their essence",
                "and inviting them to",
                "aid your magic.",
                "A candle for fire,",
                "a bowl of water,",
                "a feather for air,",
                "and a stone for earth."
            ),
            new BookPageInfo
            (
                "Lesson Four: Your",
                "First Spell",
                "A spell need not be",
                "complex. Let's begin",
                "with a simple charm",
                "of protection.",
                "Gather a white candle,",
                "salt, and a clear mind."
            ),
            new BookPageInfo
            (
                "Light the candle and",
                "cast a circle of salt.",
                "Inside this boundary,",
                "you are safe from harm.",
                "Speak your intention",
                "clearly and with",
                "conviction. Believe,",
                "and it shall be."
            ),
            new BookPageInfo
            (
                "Lesson Five: The",
                "Magic Within",
                "Most importantly,",
                "young witch, know that",
                "the greatest power",
                "lies within you.",
                "Your will, your courage,",
                "your heart. Nurture"
            ),
            new BookPageInfo
            (
                "these qualities,",
                "and no spell shall be",
                "beyond your reach.",
                "",
                "May your journey be",
                "blessed, and your",
                "magic kind. Until the",
                "next volume, farewell."
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
                "Winnie the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your path be ever",
                "lit by the stars, and",
                "guided by the wisdom",
                "of the earth."
            ),
			// Continuation from the previous BookPageInfo
			new BookPageInfo
			(
				"Lesson Six: A Witch's",
				"Best Friend",
				"Every witch needs",
				"a familiar—a loyal",
				"companion on their",
				"magical journey.",
				"Care for them, and",
				"they'll aid your craft."
			),
			new BookPageInfo
			(
				"Lesson Seven: The",
				"Witch's Brew",
				"Concocting potions is",
				"a staple of witchcraft.",
				"With a pinch of this",
				"and a dash of that,",
				"stir your way to",
				"wondrous results."
			),
			new BookPageInfo
			(
				"Lesson Eight: Moon",
				"Phases",
				"The moon is a powerful",
				"ally. Its phases impact",
				"the potency of spells.",
				"New for beginnings,",
				"Full for power, and",
				"Waning for release."
			),
			new BookPageInfo
			(
				"Lesson Nine: Spell",
				"Circles",
				"Drawing a spell circle",
				"protects and empowers.",
				"Use chalk or salt,",
				"and always close",
				"the circle when done.",
				"Safety first!"
			),
			new BookPageInfo
			(
				"Lesson Ten: The",
				"Witch's Oath",
				"Above all, practice",
				"the good law—",
				"An ye harm none,",
				"do what ye will.",
				"Use your gifts for",
				"good, young witch."
			),
			new BookPageInfo
			(
				"Appendix: Simple",
				"Spells to Try",
				"This section contains",
				"easy spells for",
				"young ones to try,",
				"under adult",
				"supervision, of course!",
				""
			),
			new BookPageInfo
			(
				"Growing Spell",
				"For your garden to",
				"thrive, cast this",
				"before sunrise:",
				"‘Tiny seed, in the",
				"earth so deep,",
				"rise awake, from",
				"your sleep.’"
			),
			new BookPageInfo
			(
				"Sleepy Time Spell",
				"Trouble sleeping?",
				"Whisper to your",
				"favorite stuffed animal:",
				"‘Guardian of night,",
				"soft and true,",
				"bring me dreams,",
				"good and new.’"
			),
			new BookPageInfo
			(
				"Cleaning Spell",
				"A messy room no more!",
				"With a sweep and a",
				"chant, make chores a",
				"breeze:",
				"‘Broom of old, now",
				"sweep away, clutter",
				"and mess of the day.’"
			),
			new BookPageInfo
			(
				"Final Words",
				"Remember, magic is",
				"all around us, in the",
				"earth, the air, the",
				"fire, and the water.",
				"Respect the magic,",
				"and it will respect you."
			),
			new BookPageInfo
			(
				"May your wand wave",
				"true, may your potions",
				"bubble brightly, and",
				"may your magical journey",
				"be as boundless as",
				"your imagination.",
				"",
				"Until the next book,"
			),
			new BookPageInfo
			(
				"Winnie the Wise",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Keep the magic alive,",
				"within and without.",
				"Blessed be!"
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
				"End of 'Child's Guide to",
				"Beginner Witchcraft'",
				"Look forward to more",
				"guides on your journey",
				"to becoming a wise",
				"witch, full of kindness,",
				"courage, and curiosity."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ChildsGuideToBeginnerWitchcraft() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Child's Guide to Beginner Witchcraft");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Child's Guide to Beginner Witchcraft");
        }

        public ChildsGuideToBeginnerWitchcraft(Serial serial) : base(serial)
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
