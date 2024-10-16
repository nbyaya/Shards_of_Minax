using System;
using Server;

namespace Server.Items
{
    public class JestersJestBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Jester's Jest Book", "Jepp the Japer",
            new BookPageInfo
            (
                "In laughter and glee,",
                "from the court's heart,",
                "I spill jest and joke,",
                "performing my art.",
                "I am Jepp, known to",
                "kings and to knaves,",
                "as the Japer whose wit",
                "is as sharp as glaives."
            ),
            new BookPageInfo
            (
                "A book of jests,",
                "a tome of mirth,",
                "to spread chuckles",
                "all 'cross the earth.",
                "From puns that’ll make",
                "you groan aloud,",
                "to tales that draw",
                "a laughing crowd."
            ),
            new BookPageInfo
            (
                "Consider the knight",
                "whose armor did squeak,",
                "each step he took,",
                "it shrieked a leak.",
                "‘Til a damsel did say,",
                "'Your presence I dread,",
                "'for I cannot think",
                "with that noise in my head!'"
            ),
            new BookPageInfo
            (
                "Or ponder the mage",
                "who spoke in reverse,",
                "his spells still worked,",
                "but made him curse.",
                "For when he’d enchant",
                "an item, no joke,",
                "he’d end with a snout",
                "or the ears of a folk!"
            ),
            new BookPageInfo
            (
                "There once was a cook,",
                "whose stew went awry.",
                "It sprouted some legs",
                "and said 'Goodbye!'",
                "Chased round the kitchen,",
                "the cook in dismay,",
                "‘til the stew leapt out",
                "and ran away."
            ),
            new BookPageInfo
            (
                "Let’s not forget",
                "the bold thief so sly,",
                "who tried to steal light",
                "from the town's main ply.",
                "With a sack over lamp,",
                "he tugged with his might,",
                "and found himself wrapped",
                "in a blanket of night."
            ),
            new BookPageInfo
            (
                "These japes and more,",
                "within these pages dwell,",
                "read with care",
                "for laughter will swell.",
                "As you peruse this jest book,",
                "take each jest in stride,",
                "for life’s too short",
                "for a frown to abide."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank for readers to pen their own jests.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Jepp the Japer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your days be merry,",
                "your nights full of jest,",
                "for he who laughs last,",
                "simply laughs best."
            ),
            new BookPageInfo
            (
                "In laughter and glee,",
                "from the court's heart,",
                "I spill jest and joke,",
                "performing my art.",
                "I am Jepp, known to",
                "kings and to knaves,",
                "as the Japer whose wit",
                "is as sharp as glaives."
            ),
            new BookPageInfo
            (
                "A book of jests,",
                "a tome of mirth,",
                "to spread chuckles",
                "all 'cross the earth.",
                "From puns that’ll make",
                "you groan aloud,",
                "to tales that draw",
                "a laughing crowd."
            ),
            // ... [Other initial pages] ...
            new BookPageInfo
            (
                "One day a troll,",
                "quite lean, not stout,",
                "did bellow aloud,",
                "'Your bridge, I doubt!",
                "'Tis too frail for me!'",
                "cried he in a shout.",
                "So he built his own,",
                "and it promptly fell out!"
            ),
            new BookPageInfo
            (
                "Then hear of the bard,",
                "whose lute was strung",
                "with the whiskers of cats,",
                "and when he sung,",
                "Every feline in town",
                "came along to be among",
                "the audience that night,",
                "swayed by songs unsprung."
            ),
            new BookPageInfo
            (
                "In a village nearby,",
                "a barber there was,",
                "who trimmed beards so close,",
                "he earned great applause.",
                "Till one sneeze did cause",
                "a slip, and then paws!",
                "The man left with whiskers",
                "fit for Santa Claus."
            ),
            new BookPageInfo
            (
                "We've a tailor who stitched",
                "with such haste and such rush,",
                "that his clothes would fit tight,",
                "then with one leap, would gush!",
                "At a ball, all his suits",
                "exploded in a flush.",
                "Now the town’s full of trends",
                "that are quite avant-garde."
            ),
            new BookPageInfo
            (
                "A lady once sipped",
                "a potion to speak",
                "with the birds in the sky,",
                "for wisdom to seek.",
                "But all that she learned",
                "was a strong urge to shriek,",
                "'Caw-caw!' at her friends,",
                "for the rest of the week!"
            ),
            new BookPageInfo
            (
                "Then there’s the old scribe",
                "whose quill was so sharp,",
                "he’d write and he’d poke,",
                "leaving many a mark.",
                "When asked why this habit",
                "he said with a lark,",
                "'Tis to make my words felt,'",
                "in the parchment so stark."
            ),
            new BookPageInfo
            (
                "No jest book’s complete",
                "without a ghost’s tale:",
                "A spirit that haunted",
                "with a face so pale.",
                "But the ghost was just shy,",
                "wanted friends, not to assail.",
                "Now he's the life of parties",
                "with his spirited wail!"
            ),
            new BookPageInfo
            (
                "A witch in the woods",
                "brewed a laughter stew.",
                "She cackled and stirred",
                "with her magic brew.",
                "But she laughed so hard",
                "that her hat flew",
                "and landed on a bear,",
                "who chuckled, 'Achoo!'"
            ),
            // ... [More pages of jests can be added here] ...
            new BookPageInfo
            (
                "So read these pages",
                "when you're blue or gray,",
                "and let the Japer's jests",
                "chase your frowns away.",
                "For life is quite the jest,",
                "full of play and fray.",
                "May you find in jest",
                "a light to guide your way."
            ),
			new BookPageInfo
            (
                "Jepp the Japer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your days be merry,",
                "your nights full of jest,",
                "for he who laughs last,",
                "simply laughs best."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public JestersJestBook() : base(false)
        {
            // Set the hue to a bright, jester-like color scheme
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Jester's Jest Book");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Jester's Jest Book");
        }

        public JestersJestBook(Serial serial) : base(serial)
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
