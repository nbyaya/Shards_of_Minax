using System;
using Server;

namespace Server.Items
{
    public class TheSovereignScrolls : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Sovereign Scrolls", "Merlinus the Scribe",
            new BookPageInfo
            (
                "In the annals of time,",
                "the Sovereign Scrolls",
                "bear witness to the",
                "ages. Penned by the",
                "venerable Merlinus,",
                "this tome offers a",
                "glimpse into the",
                "saga of empires."
            ),
            new BookPageInfo
            (
                "Behold the rise and",
                "fall of kingdoms,",
                "the whispers of",
                "wizards and the",
                "clashing of swords.",
                "Each scroll within",
                "is a tapestry of",
                "lore and legend."
            ),
            new BookPageInfo
            (
                "From the deep",
                "vaults of ancient",
                "tombs to the",
                "resplendent halls",
                "of the fae, every",
                "corner of the known",
                "world is chronicled",
                "within these pages."
            ),
            new BookPageInfo
            (
                "Read of the spell",
                "that silenced an",
                "eternal tempest,",
                "of the hero whose",
                "blade could cleave",
                "shadows, and of",
                "the dragon that",
                "wept crystal tears."
            ),
            new BookPageInfo
            (
                "Within these scrolls,",
                "truth mingles with",
                "myth, and the tales",
                "of humble beginnings",
                "ascend to meet",
                "destinies grand and",
                "terrible. Every hero,",
                "every villain plays"
            ),
            new BookPageInfo
            (
                "their part in the",
                "weave of history,",
                "their actions echoing",
                "through the tapestry",
                "of time. From",
                "forgotten lore to",
                "the birth of magic,",
                "all is preserved."
            ),
            new BookPageInfo
            (
                "So sit by the fire,",
                "open the cover,",
                "and turn these",
                "aged pages. Let the",
                "dust of centuries",
                "fall away as you",
                "submerge into the",
                "chronicles of yore."
            ),
            new BookPageInfo
            (
                "For within The",
                "Sovereign Scrolls,",
                "lies the heart of a",
                "world both vast and",
                "intimate. A world",
                "where every spell",
                "woven, every sword",
                "drawn, is a thread"
            ),
            new BookPageInfo
            (
                "in the fabric of",
                "existence. Let your",
                "imagination roam",
                "across battles of",
                "yesteryear, and",
                "soar on the wings",
                "of dragons long",
                "departed."
            ),
            new BookPageInfo
            (
                "And remember,",
                "traveler of worlds,",
                "that as you peruse",
                "these Sovereign",
                "Scrolls, you gaze",
                "into the soul of",
                "this realm. May you",
                "find wisdom"
            ),
            new BookPageInfo
            (
                "amongst these",
                "scriptures of old,",
                "and may your path",
                "be ever illuminated",
                "by the knowledge",
                "contained herein.",
                "Merlinus the Scribe",
                "bids you read well."
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
                "Merlinus the Scribe",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your journeys be",
                "grand, and your destiny",
                "be as enduring as the",
                "Sovereign Scrolls."
            ),
			// Continuing from the previous defined pages...
			new BookPageInfo
			(
				"Upon these scrolls, the",
				"ink-drawn battles and",
				"spell-woven tales stretch",
				"far beyond mortal ken.",
				"Here, the ancient pact",
				"of the First Mages is",
				"etched in eternal vow,",
				"and the creation of the"
			),
			new BookPageInfo
			(
				"Arcane Nexus pulses",
				"with a power unyielding.",
				"Legends tell of the",
				"Sovereign Stone, a gem",
				"so pure, it bore the",
				"essence of magic itself.",
				"Many have sought its",
				"gleam, yet it eludes all."
			),
			new BookPageInfo
			(
				"Read of the Alabaster",
				"Council, whose wisdom",
				"ruled the skies, of",
				"cities floating on clouds",
				"and civilizations thriving",
				"beneath the waves.",
				"Every era, every age",
				"has its chronicle."
			),
			new BookPageInfo
			(
				"This volume speaks, too,",
				"of the Silent War, fought",
				"in shadows and whispers,",
				"where the currency was",
				"secrets, and the weapon",
				"was deception. An unseen",
				"conflict that shaped",
				"the very fabric of history."
			),
			new BookPageInfo
			(
				"Enshrined within, the",
				"epic of the Emberfall,",
				"the night the sky burned",
				"with a thousand falling",
				"stars, and the world was",
				"bathed in a firestorm",
				"of scarlet rebellion.",
				"A night that saw the end"
			),
			new BookPageInfo
			(
				"of the Draconic Dynasty,",
				"and the rise of the",
				"Phoenix Empress, whose",
				"reign ushered in an",
				"era of prosperity and",
				"unwonted peace that",
				"stretched across the lands.",
				""
			),
			new BookPageInfo
			(
				"Contained, too, are the",
				"mysteries of the Vanished",
				"Vanguard, warriors who",
				"stepped through a rift",
				"unto worlds unknown,",
				"and whose return is",
				"prophesied at the",
				"turning of the Millennium."
			),
			new BookPageInfo
			(
				"Within these pages, the",
				"Sorceress of the North",
				"Winds recounts her",
				"journey to the heart",
				"of the winter storm,",
				"and the Frost Giants",
				"she roused to quell the",
				"flames of an advancing war."
			),
			new BookPageInfo
			(
				"Here too is scribed",
				"the Celestial Aligning,",
				"when all the stars in the",
				"heavens shone as one,",
				"heralding the birth of the",
				"Chosen, marked by the",
				"cosmos to stand against",
				"the Void's encroaching maw."
			),
			new BookPageInfo
			(
				"Let not the Song of the",
				"Siren Queen go unheard,",
				"whose melody sailed",
				"across the Azure Expanse,",
				"uniting the isles in",
				"harmony, quelling the",
				"storm-rage of the sea,",
				"and lulling the kraken deep."
			),
			new BookPageInfo
			(
				"Witness the Covenant",
				"of the Elemental Lords,",
				"the sworn truce that",
				"brought balance to the",
				"primal forces, and",
				"granted mortals reprieve",
				"from the earth's wrath,",
				"the inferno's reach,"
			),
			new BookPageInfo
			(
				"the gale's howl, and the",
				"deluge's advance. Herein",
				"lies the tale of the",
				"earth's split and the",
				"Sky-City's ascension,",
				"an escape from the",
				"World-Ender's gaze.",
				""
			),
			new BookPageInfo
			(
				"Lastly, read of the",
				"time-forged alliance,",
				"of Elves and Dwarves",
				"under the Mountainhome,",
				"their united stand",
				"against the Tides of",
				"Darkness, and the",
				"crowning of the Worldforge."
			),
			new BookPageInfo
			(
				"Turn these pages with",
				"care, for within them",
				"stirs the breath of",
				"ages, the spirit of",
				"adventure, and the",
				"pulse of magic yet",
				"undimmed by time.",
				"May they inspire all"
			),
			new BookPageInfo
			(
				"who seek knowledge,",
				"kindle courage in",
				"hearts that waver,",
				"and grant solace to",
				"souls adrift on the",
				"tides of fate. For",
				"the Sovereign Scrolls",
				"are the keys to"
			),
			new BookPageInfo
			(
				"our past, and within",
				"their tales lie the",
				"seeds of tomorrow.",
				"Each word is a step",
				"on the endless journey,",
				"each sentence a path",
				"to realms untold.",
				""
			),
			new BookPageInfo
			(
				"Merlinus the Scribe",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the Scrolls",
				"illuminate your path,",
				"and guide you through",
				"the labyrinth of the",
				"world's wonders."
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
			// Ensure you close the collection initializer for BookContent
			// and continue with the rest of the class definition...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheSovereignScrolls() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Sovereign Scrolls");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Sovereign Scrolls");
        }

        public TheSovereignScrolls(Serial serial) : base(serial)
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
