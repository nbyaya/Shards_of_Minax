using System;
using Server;

namespace Server.Items
{
    public class AdventurersAccoutrements : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Adventurer’s Accoutrements", "Reginald the Ready",
            new BookPageInfo
            (
                "Be it known that every",
                "traveler, wanderer, and",
                "would-be hero requires",
                "more than mere bravery",
                "to navigate the perils",
                "of this vast world.",
                "Herewith, a list most",
                "practical and slightly"
            ),
            new BookPageInfo
            (
                "absurd, of the essentials",
                "for survival and triumph.",
                "This guide, penned by",
                "none other than Reginald",
                "the Ready, shall detail",
                "the accoutrements that",
                "adventurers should",
                "cling to as dearly as"
            ),
            new BookPageInfo
            (
                "their honor. Tarry not,",
                "for preparation is the",
                "bedrock upon which",
                "legends are built.",
                "",
                "Firstly, the 'Bottomless'",
                "Backpack - for looting",
                "is serious business and"
            ),
            new BookPageInfo
            (
                "one must always have",
                "space for that extra",
                "gold coin or mysterious",
                "artifact. Followed by the",
                "Everburn Torch - lest",
                "darkness be the foil of",
                "your quest. And let us",
                "not forget the Folding"
            ),
            new BookPageInfo
            (
                "Frying Pan. Yes, you",
                "heard right - for the",
                "sustenance of a hero is",
                "often neglected and",
                "many a quest was",
                "nearly undone by the",
                "rumbles of an empty",
                "stomach."
            ),
            new BookPageInfo
            (
                "Next, the Indestructible",
                "Rope - as one can",
                "never predict when one",
                "must scale a tower or",
                "bind a beast. And",
                "the All-Purpose Oil,",
                "suitable for greasing",
                "locks, quieting hinges,"
            ),
            new BookPageInfo
            (
                "or oiling beards. Include",
                "also a Map that",
                "Refuses to Stay Folded,",
                "for true paths are",
                "rarely neat nor tidy.",
                "Let's not overlook the",
                "Modestly Magical Ring:",
                "it may not do much,"
            ),
            new BookPageInfo
            (
                "but it glows enough",
                "to impress the common",
                "folk. Lastly, the Spare",
                "Cloak of Invisibility",
                "(which is nearly impossible",
                "to find once you've",
                "put it down).",
                ""
            ),
            new BookPageInfo
            (
                "So there you have it,",
                "a list to live by. Take",
                "heed and gather your",
                "gear with wisdom and",
                "a hint of whimsy. For",
                "the prepared adventurer",
                "is both revered and",
                "envied across the lands."
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
                "Reginald the Ready",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your kit be",
                "complete, and your",
                "adventures grand."
            ),
			// Continuing from the last BookPageInfo entry...
			new BookPageInfo
			(
				"Add to this, the Cloak of",
				"Many Pockets. A marvel",
				"to behold, with pockets",
				"for potions, pockets for",
				"parchment, and even a",
				"pocket where you swear",
				"you once found a pocket",
				"goblin residing."
			),
			new BookPageInfo
			(
				"Pray, neglect not the",
				"Whetstone of Eternity,",
				"to keep your blades sharp,",
				"and your wit sharper.",
				"Indeed, a dull blade is",
				"like a dull companion,",
				"good only for taking up",
				"space at the tavern."
			),
			new BookPageInfo
			(
				"Then there's the Compass",
				"That Always Points North—",
				"except when it points",
				"to true love, which,",
				"admittedly, can be quite",
				"disorienting in the midst",
				"of navigation. Handle",
				"with care."
			),
			new BookPageInfo
			(
				"A seasoned explorer’s",
				"trusted companion is",
				"the Lute of Distraction.",
				"For when stealth fails,",
				"distract with a melody.",
				"And if that fails, the",
				"lute doubles as a rather",
				"effective bludgeoning tool."
			),
			new BookPageInfo
			(
				"Do remember the",
				"Endless Waterskin.",
				"Filled with a liquid",
				"most strange; it quenches",
				"thirst, cleans wounds,",
				"and on occasion, has",
				"been known to cure the",
				"hiccups of a drunken elf."
			),
			new BookPageInfo
			(
				"Let's not forget the",
				"Scroll of Pseudodragon",
				"Summoning. Handy when",
				"you require a light",
				"to read by, or simply",
				"desire the company",
				"of a small, mildly",
				"sarcastic, fire-breather."
			),
			new BookPageInfo
			(
				"And for those moments",
				"of grave peril, the",
				"Slightly Defective Ring",
				"of Invisibility. It may",
				"not make you fully",
				"invisible, but enemies",
				"tend to overlook the",
				"semi-transparent."
			),
			new BookPageInfo
			(
				"In addition, the Cap of",
				"Comfort is a must-have.",
				"Whether shielding thine",
				"eyes from the blaring sun,",
				"or hiding a bad hair day,",
				"its value cannot be",
				"overstated."
			),
			new BookPageInfo
			(
				"And finally, the most",
				"underrated of them all,",
				"the Pebble of Direction.",
				"When lost, simply throw",
				"the pebble and go the",
				"opposite way; it has an",
				"uncanny knack for being",
				"quite wrong about paths."
			),
			new BookPageInfo
			(
				"Thus concludes our",
				"cursory look at the",
				"Adventurer’s",
				"Accoutrements. Equip",
				"yourself with both gear",
				"and knowledge, and",
				"venture forth with",
				"confidence."
			),
			new BookPageInfo
			(
				"May your journeys be",
				"filled with rich tales,",
				"your battles be",
				"honorable, and your",
				"burdens be light.",
				"Above all, may your",
				"adventures be many",
				"and your stories worth"
			),
			new BookPageInfo
			(
				"telling by firelight",
				"amongst friends and",
				"fellow adventurers.",
				"For in the end, 'tis not",
				"the gold nor the glory",
				"that warms the weary",
				"heart, but the bonds",
				"forged along the way."
			),
			new BookPageInfo
			(
				"Go now, with this tome",
				"tucked under your arm,",
				"and let it serve as a",
				"reminder that the best",
				"adventures are those",
				"equipped with a sense",
				"of preparation and",
				"a dash of humor."
			),
			new BookPageInfo
			(
				"Reginald the Ready",
				"Bard of the Blue Boar",
				"Chronicler of the Curious",
				"and the Comically Well-Prepared",
				"",
				"",
				"",
				""
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
				"Reginald the Ready",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Adventure well,",
				"equip wisely."
			)
			// ... Rest of the book's code follows.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AdventurersAccoutrements() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
			Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Adventurer’s Accoutrements");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Adventurer’s Accoutrements");
        }

        public AdventurersAccoutrements(Serial serial) : base(serial)
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
