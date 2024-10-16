using System;
using Server;

namespace Server.Items
{
    public class FablesOfTheFey : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Fables of the Fey", "Tylwyth Teg",
            new BookPageInfo
            (
                "Within these pages",
                "lie the tales of the",
                "mystical Fey, a",
                "glimpse into their",
                "enchanting world.",
                "A collection of fables",
                "from the whispering",
                "forests and shimmering"
            ),
            new BookPageInfo
            (
                "glades. Herein,",
                "the antics of pixies,",
                "the wisdom of dryads,",
                "and the mischief of",
                "sprites are penned",
                "with a careful hand,",
                "capturing the essence",
                "of their ethereal lives."
            ),
            new BookPageInfo
            (
                "Let us begin with the",
                "tale of the Moonlit",
                "Waltz, where the",
                "fairies dance under",
                "the silver glow,",
                "their wings dusted",
                "with the magic of",
                "the nocturne's embrace."
            ),
            new BookPageInfo
            (
                "Follow the journey of",
                "a lone naiad whose",
                "tears turned to pearls",
                "upon the riverbed,",
                "sought by mortals",
                "but destined for",
                "the crowns of elfin",
                "kings and queens."
            ),
            new BookPageInfo
            (
                "Discover the riddle",
                "of the ancient tree",
                "whose roots hold the",
                "secrets of the fey",
                "world, guarded by",
                "gnomes with hearts",
                "of bark and sap,",
                "their eyes aglow"
            ),
            new BookPageInfo
            (
                "with the wisdom of",
                "ages. Read of the",
                "impish trickster with",
                "a penchant for",
                "swapping babies",
                "and leading travelers",
                "astray with its",
                "enigmatic grin."
            ),
            new BookPageInfo
            (
                "Each story within",
                "is a thread in the",
                "tapestry of fey lore,",
                "woven with care and",
                "imbued with the",
                "magic that flickers",
                "in the twilight. These",
                "are the Fables of the"
            ),
            new BookPageInfo
            (
                "Fey: a testament to",
                "the unseen and a",
                "guide to those who",
                "seek the wonders",
                "that lay veiled by",
                "mortal sight. May",
                "the curious be sated",
                "and the dreamers inspired."
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
            ),
            new BookPageInfo
            (
                "Tylwyth Teg",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Beneath the gossamer",
                "veil, truth mingles with",
                "tale. Step lightly and",
                "carry a kind heart."
            ),
			// Continuation of the previous book pages...
			new BookPageInfo
			(
				"The Whispering Willows:",
				"In a glen kissed by",
				"starlight, the willows",
				"whisper tales with",
				"leaves that speak to",
				"the wandering souls,",
				"guiding them through",
				"the darkness with"
			),
			new BookPageInfo
			(
				"stories of old.",
				"They tell of the",
				"world's weaving and",
				"the threads of fate",
				"held by the fey who",
				"dance in their shade.",
				"Heed their words well,",
				"for they hold truths"
			),
			new BookPageInfo
			(
				"hidden from the",
				"prying eyes of time.",
				"The Piper's Price:",
				"Beware the piper with",
				"eyes of ember and a",
				"smile like the crescent",
				"moon. His tunes are",
				"sweet, and his words"
			),
			new BookPageInfo
			(
				"sweeter still, but",
				"his price is a shard",
				"of your soul. Pay not",
				"with coin but with",
				"heed to his lesson:",
				"That which enchants",
				"most may also bind",
				"you in chains unseen."
			),
			new BookPageInfo
			(
				"The Mist of Dreams:",
				"Within the vales where",
				"mists weave like silk",
				"through the woods,",
				"the fey dream with",
				"eyes wide open. Their",
				"dreams spill forth",
				"and shape the mist,"
			),
			new BookPageInfo
			(
				"creating wonders",
				"unfathomable to the",
				"sleeping mind. Dare",
				"to dream with them,",
				"and see your own",
				"desires painted in",
				"the canvas of their",
				"world."
			),
			new BookPageInfo
			(
				"The Weeping Stone:",
				"A stone sits in the",
				"heart of the forest,",
				"etched with tears",
				"eternal. It weeps not",
				"for sorrow, but for",
				"the joy of the world's",
				"unseen beauty. Touch"
			),
			new BookPageInfo
			(
				"it, and feel the",
				"fleeting joys and",
				"sorrows of the fey.",
				"It is said that those",
				"who comfort the stone",
				"are granted the gift",
				"of seeing beauty in",
				"all things."
			),
			new BookPageInfo
			(
				"The Knight's Vigil:",
				"A knight once stood",
				"watch over a fey child",
				"as it slept under the",
				"moon's watchful eye.",
				"His vigil taught him",
				"patience and love,",
				"the truest armor"
			),
			new BookPageInfo
			(
				"against the darkness.",
				"When the fey child",
				"awoke, it blessed the",
				"knight with a boon",
				"of clarity, for the",
				"greatest strength",
				"comes from the",
				"heart's quiet resolve."
			),
			new BookPageInfo
			(
				"The Minstrel's Melody:",
				"A minstrel sang with",
				"a voice that wove",
				"magic into every",
				"heart. His melody",
				"was not one of notes",
				"but of stories that",
				"mended the spirit and"
			),
			new BookPageInfo
			(
				"soothed the weary.",
				"Listen for his song",
				"in the rustling leaves",
				"and the babbling brooks,",
				"for he sings the song",
				"of the world, forever",
				"binding all hearts",
				"as one."
			),
			new BookPageInfo
			(
				"In these fables, you",
				"will find wisdom,",
				"adventure, and the",
				"magic that dwells",
				"within the heart of",
				"the fey—and perhaps",
				"within us all. Let",
				"these tales guide"
			),
			new BookPageInfo
			(
				"and enchant you as",
				"you walk the paths",
				"of the world. May",
				"your steps be light,",
				"and your heart ever",
				"open to the wonders",
				"of the Feywilds.",
				""
			),
			new BookPageInfo
			(
				"Tylwyth Teg",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"With every ending,",
				"there is a new",
				"beginning. May the",
				"fables of the fey",
				"enrich your journey."
			)
			// End of the book content

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public FablesOfTheFey() : base(false)
        {
            // Set the hue to a random color that feels magical, perhaps a range representing mystical colors
            Hue = Utility.RandomMinMax(500, 999);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Fables of the Fey");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Fables of the Fey");
        }

        public FablesOfTheFey(Serial serial) : base(serial)
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
