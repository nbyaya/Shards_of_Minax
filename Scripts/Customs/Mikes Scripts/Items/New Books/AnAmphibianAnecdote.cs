using System;
using Server;

namespace Server.Items
{
    public class AnAmphibianAnecdote : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "An Amphibian Anecdote", "Finn the Fabulist",
            new BookPageInfo
            (
                "Upon a lily pad of green,",
                "sat a frog, plump and keen,",
                "with eyes like gems and",
                "skin of mottle,",
                "his croaks resound like",
                "a watery bottle.",
                "A simple creature, one might",
                "say, yet his tale is more"
            ),
            new BookPageInfo
            (
                "than play.",
                "",
                "For this is a story of",
                "an amphibious lore,",
                "a leap of faith and so",
                "much more. From his",
                "humble abode in the",
                "marshy glade, to the",
                "grander ponds, his",
                "ambitions laid."
            ),
            new BookPageInfo
            (
                "One day he chirped a",
                "wish to the sky,",
                "'I yearn for more than",
                "this pond, oh why",
                "can't I roam the lands",
                "and see the world's",
                "span, from the buzzing",
                "bees to the caravans?'"
            ),
            new BookPageInfo
            (
                "A wizard old, with robe",
                "and staff, heard his plea",
                "and with a laugh,",
                "bestowed upon him a",
                "spell quite odd,",
                "to walk the world as",
                "man, not frog."
            ),
            new BookPageInfo
            (
                "With newfound legs and",
                "sparkling eyes, the frog",
                "set forth beyond the",
                "flies. Through forests",
                "deep and cities wide,",
                "he hopped along with",
                "human stride.",
                "Each creature and man he"
            ),
            new BookPageInfo
            (
                "did greet, with a croaky",
                "hello and not a tweet.",
                "His words were met with",
                "awe and wonder,",
                "for a talking frog was",
                "a spell asunder."
            ),
            new BookPageInfo
            (
                "Yet in his heart, the",
                "frog did know,",
                "the marshes called him",
                "back to go.",
                "For every adventure",
                "grand and wild,",
                "couldn't match the",
                "pond, tranquil and mild."
            ),
            new BookPageInfo
            (
                "So back he went, to his",
                "home with glee,",
                "under the moon's",
                "shimmering spree.",
                "A frog he was, and ever",
                "shall be,",
                "but with tales to tell,",
                "of the lands he did see."
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
                "Finn the Fabulist",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May you find joy in your",
                "jumps and your rest in",
                "the calm of your pond."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AnAmphibianAnecdote() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("An Amphibian Anecdote");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "An Amphibian Anecdote");
        }

        public AnAmphibianAnecdote(Serial serial) : base(serial)
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
