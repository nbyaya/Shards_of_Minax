using System;
using Server;

namespace Server.Items
{
    public class RiddlesOfTheRunestone : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Riddles of the Runestone", "Eldrin the Wise",
            new BookPageInfo
            (
                "Within ancient stone,",
                "and runes of old,",
                "lie riddles untold,",
                "and secrets bold."
            ),
            new BookPageInfo
            (
                "I am Eldrin, the Wise,",
                "scholar of ages,",
                "seeker of mysteries,",
                "turner of pages."
            ),
            new BookPageInfo
            (
                "In this tome, you shall",
                "find a collection of",
                "riddles, each bound",
                "to a runestone's might."
            ),
            new BookPageInfo
            (
                "Not just idle games,",
                "these puzzles are keys,",
                "unlocking the whispers,",
                "of ancients, if you please."
            ),
            new BookPageInfo
            (
                "A runestone for strength,",
                "bears a challenge of power,",
                "'I can be cracked, made,",
                "told, and played. What",
                "am I?' Ponder the hour."
            ),
            new BookPageInfo
            (
                "The answer, quite simple,",
                "yet eludes the brute.",
                "It's 'a joke' that's taken,",
                "lightly, until its root."
            ),
            new BookPageInfo
            (
                "Another stone speaks",
                "of time and decay,",
                "'Riches I hold, yet",
                "from hands, I slip away."
            ),
            new BookPageInfo
            (
                "What am I?' it asks,",
                "with a grin unseen.",
                "The answer is 'sand',",
                "slipping through as if a dream."
            ),
            new BookPageInfo
            (
                "So proceed, dear reader,",
                "with a mind keen and sharp.",
                "Solve the riddles of runestones,",
                "and embark on an arcane harp."
            ),
            new BookPageInfo
            (
                "For each puzzle solved,",
                "a new path you'll see,",
                "within the realm of magic,",
                "and old wizardry."
            ),
            new BookPageInfo
            (
                "Should you find them all,",
                "a secret will unfold,",
                "In runes of power, your story,",
                "forever will be told."
            ),
            new BookPageInfo
            (
                // These pages are left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Eldrin the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your mind be sharp,",
                "and your answers true."
            ),
// Existing content from previous snippet...

            new BookPageInfo
            (
                "Upon the runestone of skies,",
                "a challenge of sight,",
                "'I am the sibling of water,",
                "but soar to great height."
            ),
            new BookPageInfo
            (
                "What am I?' Above so high,",
                "with clouds I dance.",
                "The answer is 'steam',",
                "in its upward prance."
            ),
            new BookPageInfo
            (
                "The next runestone cold,",
                "with a surface so slick,",
                "'I can capture the sun,",
                "but I am not quick.'"
            ),
            new BookPageInfo
            (
                "What am I?' Look yonder,",
                "in winter's embrace.",
                "The answer is 'ice',",
                "with a glistening face."
            ),
            new BookPageInfo
            (
                "The stone of the shadows,",
                "whispers a riddle of dread,",
                "'I am often avoided,",
                "and filled with the dead.'"
            ),
            new BookPageInfo
            (
                "What am I?' With courage,",
                "one must reply.",
                "The answer is 'grave',",
                "where we all lie."
            ),
            new BookPageInfo
            (
                "A runestone of life,",
                "beats with a riddle so true,",
                "'I am the start of eternity,",
                "and the end of time and space."
            ),
            new BookPageInfo
            (
                "What am I?' A puzzle so vast,",
                "yet small in its place.",
                "The answer is 'the letter E',",
                "first and last in 'space'."
            ),
            new BookPageInfo
            (
                "A stone etched with laughter,",
                "throws a playful jest,",
                "'I am taken from a mine,",
                "and shut up in a chest."
            ),
            new BookPageInfo
            (
                "What am I?' Think wisely,",
                "and with vision be blest.",
                "The answer is 'a heart',",
                "in your chest does it rest."
            ),
            new BookPageInfo
            (
                "A final runestone,",
                "carved with a riddle so old,",
                "'I have keys but open no locks,",
                "I have space but no room."
            ),
            new BookPageInfo
            (
                "What am I?' A query so bold,",
                "yet answers it does bloom.",
                "The answer is 'a keyboard',",
                "for your fingers to assume."
            ),
            new BookPageInfo
            (
                "With the riddles now told,",
                "and answers unveiled,",
                "the runestones' power",
                "is rightfully hailed."
            ),
            new BookPageInfo
            (
                "May these riddles of old,",
                "guide your path anew,",
                "as the runestones of legend",
                "reveal secrets to you."
            ),
            new BookPageInfo
            (
                "Eldrin the Wise bids farewell,",
                "with a mind now keen.",
                "Seek beyond the riddles,",
                "and may your insight be seen."
            ),
            new BookPageInfo
            (
                // These pages are left intentionally blank.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Eldrin the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In runes and riddles,",
                "may wisdom prevail."
            )
// Continuing with the rest of the class definition...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public RiddlesOfTheRunestone() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Riddles of the Runestone");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Riddles of the Runestone");
        }

        public RiddlesOfTheRunestone(Serial serial) : base(serial)
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
