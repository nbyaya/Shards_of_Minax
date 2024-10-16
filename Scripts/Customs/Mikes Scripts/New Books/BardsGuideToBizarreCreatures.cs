using System;
using Server;

namespace Server.Items
{
    public class BardsGuideToBizarreCreatures : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Bard's Guide to Bizarre Creatures", "Finneas the Bard",
            new BookPageInfo
            (
                "Within these pages",
                "lies a chronicle of the",
                "most bizarre beasts",
                "that roam the lands.",
                "Penned by Finneas,",
                "the roving bard, this",
                "guide is a collection",
                "of oddities encountered"
            ),
            new BookPageInfo
            (
                "in my travels far and",
                "wide. Each creature",
                "more curious than",
                "the last, they defy",
                "common sense and",
                "often, natural law.",
                "",
                "From the flittering"
            ),
            new BookPageInfo
            (
                "Sparksprites, to",
                "the lumbering",
                "Giantkin, every",
                "beast herein has",
                "a tale. And I, with",
                "lute and voice, have",
                "brought their stories",
                "to life."
            ),
            new BookPageInfo
            (
                "Tales of scales that",
                "reflect the moon's",
                "glow, and creatures",
                "that dance with the",
                "shadows. Beasts",
                "that speak in riddles,",
                "and those that whisper",
                "only lies."
            ),
            new BookPageInfo
            (
                "Read of the",
                "Abyssal Leviathan,",
                "whose tentacles could",
                "embrace an entire",
                "ship. Gaze upon the",
                "scribblings of the",
                "Phantom Stag, a beast",
                "of purest white and"
            ),
            new BookPageInfo
            (
                "eyes like starlight,",
                "whose presence",
                "heralds change.",
                "Marvel at the",
                "Whisperwinds—birds",
                "that carry the",
                "conversations of the",
                "world on their wings."
            ),
            new BookPageInfo
            (
                "This compilation",
                "is but a window into",
                "a realm teeming with",
                "the fantastical and",
                "the wondrous. A tome",
                "for those brave",
                "enough to explore",
                "and perhaps sing a"
            ),
            new BookPageInfo
            (
                "tune of the",
                "extraordinary.",
                "So take this guide",
                "and venture forth,",
                "with eyes wide and",
                "mind open. For the",
                "world is full of",
                "strange majesty,"
            ),
            new BookPageInfo
            (
                "and it sings a song",
                "for those with the",
                "heart to listen.",
                "May your paths be",
                "ever vibrant, and",
                "your stories ever",
                "filled with the",
                "bizarre and the"
            ),
            new BookPageInfo
            (
                "beautiful.",
                "",
                "Finneas the Bard",
                "",
                "Let the world be",
                "your stage."
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
                "Finneas the Bard",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your travels",
                "be safe, and your",
                "tales many."
            ),
			// Continuing from the existing BookPageInfo...
			new BookPageInfo
			(
				"Twilight Capra, goats",
				"with coats as dark as",
				"the void, whose eyes",
				"gleam with celestial",
				"light. It's said their",
				"horns can unlock the",
				"gates to other realms,",
				"a bard's whispered dream."
			),
			new BookPageInfo
			(
				"Here too are the",
				"Inkfins, elusive fish",
				"that swim through",
				"sands as if they were",
				"water. Catch one, and",
				"it may grant a single",
				"line of poetry—worth",
				"an ocean of thoughts."
			),
			new BookPageInfo
			(
				"Turn the page to find",
				"the Bellowing Blomst,",
				"flowers that can sing",
				"a duet with the dawn.",
				"Their music brings",
				"forth the sun, and",
				"lulls the moon to",
				"rest beneath the hills."
			),
			new BookPageInfo
			(
				"Meet the Glimmerglade",
				"Stags, their antlers",
				"a prism of colors,",
				"casting rainbows as",
				"they bound through",
				"the woods. Legends",
				"say they leave trails",
				"of gem-dust in their wake."
			),
			new BookPageInfo
			(
				"Behold the Skysnake,",
				"a serpent amongst",
				"the clouds, twisting",
				"around the peaks of",
				"the highest mountains,",
				"guarding secrets",
				"whispered by the wind,",
				"fierce yet wise."
			),
			new BookPageInfo
			(
				"Cherish the tale of",
				"the Embermouse, tiny",
				"creatures with fur",
				"like smoldering coals.",
				"They nest in ancient",
				"libraries, warming",
				"the feet of studious",
				"mages at night."
			),
			new BookPageInfo
			(
				"Marvel at the",
				"Moon-Moths, wings",
				"etched with lunar",
				"patterns, who drink",
				"the dew collected",
				"from moonflowers and",
				"glow with a pale",
				"light in the dark."
			),
			new BookPageInfo
			(
				"The Frostwalkers tread",
				"where winter grips",
				"the earth, hoarfrost",
				"patterns trailing from",
				"their feet. They bring",
				"the silence of snowfall,",
				"and leave behind the",
				"promise of spring."
			),
			new BookPageInfo
			(
				"Encounter the",
				"Stoneback Turtles,",
				"whose shells are maps",
				"of worlds unknown.",
				"Travelers who follow",
				"these living atlases",
				"might find treasures",
				"beyond their wildest dreams."
			),
			new BookPageInfo
			(
				"Endure the gaze of",
				"the Watcher in the Weald,",
				"a creature with myriad",
				"eyes, each a story,",
				"each a lost soul.",
				"Its stare is said to",
				"bewitch the forest,",
				"a sentinel of old."
			),
			new BookPageInfo
			(
				"The book would not",
				"be complete without",
				"the Sighing Sands—",
				"deserts that breathe.",
				"One must listen closely",
				"for their tales, else",
				"be swept away by their",
				"melancholic storms."
			),
			new BookPageInfo
			(
				"So, dear reader, as you",
				"peruse this guide, know",
				"that the world is ripe",
				"with wonders, waiting",
				"for your footsteps and",
				"songs. May your journey",
				"be ever entwined with",
				"the fabric of fable."
			),
			new BookPageInfo
			(
				"And with this, I bid",
				"thee farewell, until",
				"we meet in tale or",
				"tavern. May the road",
				"rise up to greet you,",
				"and the wind carry",
				"your stories far and wide."
			),
			new BookPageInfo
			(
				"Finneas the Bard",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Remember, the world",
				"is but a page, and we",
				"are its words. Sing",
				"well, live fully."
			)
			// End of additional content.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public BardsGuideToBizarreCreatures() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Bard's Guide to Bizarre Creatures");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Bard's Guide to Bizarre Creatures");
        }

        public BardsGuideToBizarreCreatures(Serial serial) : base(serial)
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
