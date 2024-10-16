using System;
using Server;

namespace Server.Items
{
    public class EttinElegiesPoetryAnthology : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Ettin Elegies: A Poetry Anthology", "Grimelda the Poet",
            new BookPageInfo
            (
                "In twilight's gloom, 'neath",
                "mountain's gaze,",
                "Two-headed ettins find",
                "their ways.",
                "With club in hand,",
                "and hearts askew,",
                "They lumber forth,",
                "in morning dew."
            ),
            new BookPageInfo
            (
                "First head named Brawn,",
                "with eyes of red,",
                "Dreams of battles,",
                "fear and dread.",
                "Second named Brain,",
                "with thoughts so wild,",
                "Longs for peace,",
                "meek and mild."
            ),
            new BookPageInfo
            (
                "Together bound,",
                "a fate entwined,",
                "One body shared,",
                "two distinct minds.",
                "Through mossy vale,",
                "and rocky crag,",
                "They ponder life’s",
                "unending drag."
            ),
            new BookPageInfo
            (
                "‘Oh to split,’ says Brain,",
                "‘and be but one!’",
                "‘Nay!’ roars Brawn,",
                "‘Twould spoil the fun!’",
                "‘But peace,’ pleads Brain,",
                "‘is all I seek.’",
                "‘And war!’ cries Brawn,",
                "‘For I am not weak!’"
            ),
            new BookPageInfo
            (
                "Thus is the plight",
                "of ettin-kind,",
                "Two souls hitched,",
                "in one confined.",
                "What laughs we’ve had!",
                "What tears we’ve cried!",
                "In these elegies,",
                "our tales abide."
            ),
            new BookPageInfo
            (
                "This anthology",
                "you hold in hand,",
                "Speaks of ettin life,",
                "grand and grand.",
                "May each verse bring",
                "to light their song,",
                "Of ettin days",
                "both short and long."
            ),
            new BookPageInfo
            (
                "Let not their visage",
                "cause dismay,",
                "For ettins too have",
                "roles to play.",
                "In world's vast pageant,",
                "they find their part,",
                "With each beat",
                "of their dual heart."
            ),
            new BookPageInfo
            (
                "When night descends,",
                "and stars alight,",
                "Ettins dance 'til",
                "the morning's bright.",
                "Each poem here",
                "a solemn ode,",
                "To the ettin's road,",
                "less traveled, less trode."
            ),
            new BookPageInfo
            (
                "So read with care,",
                "these verses rare,",
                "Of two-headed kin,",
                "beyond compare.",
                "May their stories",
                "touch your soul,",
                "And in your heart,",
                "take kindly toll."
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
                "Grimelda the Poet",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Ettin spirits, forever nigh,",
                "In these elegies, never die."
            ),
			            new BookPageInfo
            (
                "In cavern deep or",
                "hillside steep,",
                "Ettins ponder secrets",
                "they can't keep.",
                "For every whisper to",
                "one's own twin,",
                "Is heard aloud,",
                "much to their chagrin."
            ),
            new BookPageInfo
            (
                "Oft’ they argue into",
                "the night,",
                "One head is wrong,",
                "the other right.",
                "Yet come the morn'",
                "all squabbles cease,",
                "In unified stride,",
                "they find their peace."
            ),
            new BookPageInfo
            (
                "With each step upon",
                "the earthen land,",
                "An ettin leaves mark",
                "both firm and grand.",
                "Not just of size, or",
                "of fearsome might,",
                "But of the dual souls",
                "in ceaseless plight."
            ),
            new BookPageInfo
            (
                "Hark! The ettin's song,",
                "a ballad of brawn,",
                "A melody that echoes",
                "from dusk till dawn.",
                "A tune that speaks of",
                "loneliness paired,",
                "Of a dance for two,",
                "uniquely shared."
            ),
            new BookPageInfo
            (
                "When tempests rage,",
                "and winds do howl,",
                "The ettin's mood may",
                "turn quite foul.",
                "One head wishes to",
                "hide and cower,",
                "The other, to challenge",
                "the storm's power."
            ),
            new BookPageInfo
            (
                "Beneath the moon’s",
                "silvery glow,",
                "Ettins whisper of",
                "what they know.",
                "Of ancient magic,",
                "and times of yore,",
                "Of twin-headed dragons",
                "and mythic lore."
            ),
            new BookPageInfo
            (
                "When one head sleeps,",
                "the other lies awake,",
                "Contemplating life,",
                "and the path to take.",
                "One dreams of fire,",
                "and conquering lands,",
                "The other of gentle seas,",
                "and peaceful sands."
            ),
            new BookPageInfo
            (
                "The ettins' laugh, a",
                "thunderous sound,",
                "That shakes the trees",
                "and stirs the ground.",
                "Yet within their mirth,",
                "a truth rings clear,",
                "The joy of unity,",
                "and conquering fear."
            ),
            new BookPageInfo
            (
                "Through field and forest,",
                "they lumber with grace,",
                "Each step in harmony,",
                "never a race.",
                "For two heads share",
                "a single fate,",
                "Bound together,",
                "they contemplate."
            ),
            new BookPageInfo
            (
                "And thus concludes",
                "our humble verse,",
                "Of ettin lives, complex",
                "and diverse.",
                "May these elegies impart",
                "wisdom true,",
                "Of the ettin's journey,",
                "old and new."
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
                "Grimelda the Poet",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Within these pages,",
                "ettins roam.",
                "In your hands,",
                "they've found a home."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public EttinElegiesPoetryAnthology() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ettin Elegies: A Poetry Anthology");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ettin Elegies: A Poetry Anthology");
        }

        public EttinElegiesPoetryAnthology(Serial serial) : base(serial)
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
