using System;
using Server;

namespace Server.Items
{
    public class PixiePotions : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Pixie Potions", "Flutterby the Fae Alchemist",
            new BookPageInfo
            (
                "Within these pages",
                "lie the secrets of",
                "nature's most delicate",
                "and potent concoctions,",
                "crafted not by mortal",
                "hands, but by the",
                "tiny, deft wings of",
                "the pixies themselves."
            ),
            new BookPageInfo
            (
                "I am Flutterby, a",
                "weaver of dewdrops",
                "and whisperer of",
                "the woodland's breath,",
                "a pixie sworn to the",
                "art of potion-making.",
                "Take heed as you",
                "peruse my life's work."
            ),
            new BookPageInfo
            (
                "From the nectar of",
                "twilight blooms to the",
                "essence of starlight",
                "captured at midnight,",
                "the ingredients I use",
                "are as elusive as the",
                "fleeting dreams of",
                "humankind's slumber."
            ),
            new BookPageInfo
            (
                "The first of my potions,",
                "I named 'Gossamer Gleam',",
                "a draught that grants",
                "the lightness of the",
                "breeze to those who",
                "imbibe it. One sip, and",
                "you may find your feet",
                "scarcely touching ground."
            ),
            new BookPageInfo
            (
                "Beware, however, the",
                "potent 'Pixie Prank',",
                "a mixture born of",
                "jest, which will turn",
                "your voice into trills",
                "and chirps for hours",
                "on end. A favorite at",
                "our moonlit revels."
            ),
            new BookPageInfo
            (
                "The most revered of",
                "our potions, the",
                "'Elixir of the Fae',",
                "is said to mend the",
                "weary heart and soothe",
                "the tired soul with",
                "but a single drop,",
                "a treasure most rare."
            ),
            new BookPageInfo
            (
                "These enchanted brews",
                "are but a whisper of",
                "what lies within the",
                "realm of pixie alchemy.",
                "To learn more, one must",
                "walk the dappled paths",
                "of the enchanted forest,",
                "guided by faith and"
            ),
            new BookPageInfo
            (
                "a touch of whimsy.",
                "For our potions do not",
                "yield their magic to",
                "the heavy-hearted or",
                "the doubting mind.",
                "",
                "May your journey be",
                "light and your curiosity"
            ),
            new BookPageInfo
            (
                "boundless. Tread gently",
                "upon the moss, listen to",
                "the murmur of the leaves,",
                "and perhaps, a pixie",
                "may reveal itself to",
                "you, sharing the wonders",
                "of our hidden craft.",
                ""
            ),
            new BookPageInfo
            (
                "And so, dear reader,",
                "I leave you with this",
                "parting blessing:",
                "May your days be",
                "filled with the magic",
                "of the smallest wonders,",
                "and your nights alight",
                "with the glow of"
            ),
            new BookPageInfo
            (
                "pixie fire. Treasure",
                "this tome, for it is",
                "a rare glimpse into",
                "a world unseen, where",
                "the potions are as",
                "enchanting as the",
                "creatures who brew",
                "them."
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
                "Flutterby the Fae Alchemist",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "With the lightest touch",
                "and the kindest intent,"
            ),
			// Continuing from the previous BookPageInfo...
			new BookPageInfo
			(
				"On gilded wings, I",
				"whisper to the dawn,",
				"gathering dew from",
				"spider webs, which",
				"serves as the base for",
				"many a potion. 'Tis the",
				"purest water, collected",
				"beneath the first light."
			),
			new BookPageInfo
			(
				"'Mirthful Mead', for",
				"instance, requires this",
				"dew, mixed with the",
				"honey of the rare",
				"moonblooms. One sip",
				"induces laughter, lifting",
				"spirits to dance on the",
				"breeze with joyous abandon."
			),
			new BookPageInfo
			(
				"But not all potions",
				"are for merriment.",
				"'Veil of the Vale' is",
				"a serious brew that",
				"shields the drinker",
				"in invisibility, as long",
				"as one's heart beats",
				"quiet as a mouse."
			),
			new BookPageInfo
			(
				"The 'Essence of",
				"Ether', a potion most",
				"complex, is not to be",
				"taken lightly, for it",
				"can send one's spirit",
				"soaring through the",
				"ethereal planes, a",
				"voyage of mind and"
			),
			new BookPageInfo
			(
				"soul. Only the most",
				"seasoned of the fey",
				"dare to partake, for",
				"mortals it can be",
				"overwhelming, leaving",
				"one adrift in visions",
				"and whispers of the",
				"ancient world."
			),
			new BookPageInfo
			(
				"Let us not forget the",
				"'Draught of Diminution',",
				"a playful potion that",
				"shrinks the imbiber to",
				"our size, allowing one",
				"to see the world through",
				"the eyes of a pixie for",
				"a time brief as a sunset."
			),
			new BookPageInfo
			(
				"The most whimsical of",
				"all, 'Bubblebreath Brew',",
				"grants one the ability to",
				"breathe out bubbles",
				"filled with giggles and",
				"snickers. Children find",
				"this particularly delightful,",
				"as do the young at heart."
			),
			new BookPageInfo
			(
				"Then, there is 'Nectar of",
				"the Nimble', a potion that",
				"bestows swift agility.",
				"One may leap with the",
				"vigor of the frogs and",
				"sprint as the cheetahs,",
				"with grace to prevent",
				"even a petal's bruise."
			),
			new BookPageInfo
			(
				"'Luminous Libation',",
				"a potion that is the",
				"embodiment of starlight.",
				"It imbues the drinker's",
				"skin with a gentle glow,",
				"shining softly in the",
				"darkest of nights, a beacon",
				"for the lost and weary."
			),
			new BookPageInfo
			(
				"For those seeking",
				"solace in slumber, the",
				"'Dream Draught' offers",
				"respite. It ensures dreams",
				"as sweet as the nectar",
				"and as peaceful as the",
				"still air within the hollows",
				"of ancient oaks."
			),
			new BookPageInfo
			(
				"A word of caution:",
				"Potions are not toys,",
				"and they demand respect.",
				"Nature's gifts are powerful,",
				"and the magic we weave",
				"is not without its risks.",
				"Treat these recipes with",
				"the reverence they deserve."
			),
			new BookPageInfo
			(
				"May your ventures into",
				"the art of potion-craft",
				"be guided by the wisdom",
				"of the fae. And should",
				"you ever wander into a",
				"pixie circle beneath the",
				"moon's silvery glow, tread",
				"lightly and speak true."
			),
			new BookPageInfo
			(
				"For now, I must fold",
				"my wings and let the",
				"quill rest. The dawn",
				"approaches, and with",
				"it, the myriad duties of",
				"a fae alchemist. Until",
				"the morrow's dew, I bid",
				"thee farewell."
			),
			new BookPageInfo
			(
				"Flutterby the Fae Alchemist",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the winds carry",
				"you to the wonders",
				"you seek, and the stars",
				"guide you home."
			),
			// The rest of the pages can be intentionally left blank as before or used for further content.
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
			// ... end of BookContent definition

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public PixiePotions() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Pixie Potions");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Pixie Potions");
        }

        public PixiePotions(Serial serial) : base(serial)
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
