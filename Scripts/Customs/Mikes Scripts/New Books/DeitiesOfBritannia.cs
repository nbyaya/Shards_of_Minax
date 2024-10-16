using System;
using Server;

namespace Server.Items
{
    public class DeitiesOfBritannia : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Deities of Britannia", "Sage Eremis",
            new BookPageInfo
            (
                "Britannia, a land of",
                "mystery & virtue, is",
                "guarded by the",
                "pantheon of its",
                "deities. Their sagas",
                "weave through the",
                "ages, embodying the",
                "principles that keep"
            ),
            new BookPageInfo
            (
                "the realm in balance.",
                "This tome seeks to",
                "enlighten thee on the",
                "gods and goddesses",
                "that watch over and",
                "guide the destiny of",
                "our land."
            ),
            new BookPageInfo
            (
                "First among them is",
                "Virtue's Champion,",
                "Lord British, whose",
                "wisdom and benevolence",
                "have led the people",
                "through prosperity",
                "and turmoil alike."
            ),
            new BookPageInfo
            (
                "Mondain, the Wizard,",
                "represents the dark",
                "past, a deity of power",
                "and domination. His",
                "fall from grace is a",
                "tale oft told to warn",
                "of the perils of",
                "hubris and tyranny."
            ),
            new BookPageInfo
            (
                "Minax the Enchantress,",
                "with her tempestuous",
                "nature, rules over the",
                "forces of passion and",
                "revenge. Her followers",
                "are as fervent as the",
                "storms she conjures."
            ),
            new BookPageInfo
            (
                "The Stranger, known",
                "as the Avatar, is the",
                "embodiment of the",
                "Eight Virtues. This",
                "hero's legacy is the",
                "quest for spiritual",
                "enlightenment and",
                "moral perfection."
            ),
            new BookPageInfo
            (
                "These figures, along",
                "with other lesser",
                "deities, form the",
                "tapestry of faith in",
                "our lands. Their",
                "divine play, a",
                "constant struggle",
                "between good and"
            ),
            new BookPageInfo
            (
                "evil, order and",
                "chaos, shapes our",
                "world and the hearts",
                "of its inhabitants.",
                "",
                "May this book serve",
                "as a beacon of",
                "knowledge for those"
            ),
            new BookPageInfo
            (
                "seeking to understand",
                "the celestial",
                "influences that",
                "govern our existence.",
                "Let the Virtues guide",
                "thee on thy journey."
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
                "Sage Eremis",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In the light of the",
                "Virtues."
            ),
			// Continued from the previous pages...
			new BookPageInfo
			(
				"Shall I speak now",
				"of the siblings",
				"Nature's twins:",
				"the capricious Astarte,",
				"goddess of growth and",
				"decay, and her brother,",
				"stoic Silvanus, the",
				"ever-watchful guardian"
			),
			new BookPageInfo
			(
				"of forest and fauna.",
				"Their followers tread",
				"softly through the",
				"wilds, partaking in the",
				"harmony of the cycle",
				"of life."
			),
			new BookPageInfo
			(
				"Next, the enigmatic",
				"Arcadion, a spectral",
				"entity of unknown",
				"origin, dwells in the",
				"shadows cast by",
				"the ethereal planes.",
				"Whispers tell of cults",
				"pledging fealty to this"
			),
			new BookPageInfo
			(
				"arcane specter, seeking",
				"to harness the raw",
				"energies of the void.",
				"",
				"The seas have their",
				"own queen, Hydros,",
				"mistress of the deep.",
				"Sailors pay tribute"
			),
			new BookPageInfo
			(
				"lest they incur her",
				"wrath and succumb",
				"to the abyss. Her",
				"bounty is as generous",
				"as her fury is cruel."
			),
			new BookPageInfo
			(
				"In the realm of dreams",
				"and night, we find",
				"Nyx, the veiled lady",
				"of sleep and secrets.",
				"She blankets the world",
				"in stars and whispers,",
				"granting respite and",
				"visions to the weary."
			),
			new BookPageInfo
			(
				"At the forge of creation,",
				"stands Vulcan, god of",
				"fire and craftsmanship.",
				"His anvils ring with",
				"the melody of progress,",
				"shaping the destiny of",
				"Britannia with each",
				"molten masterpiece."
			),
			new BookPageInfo
			(
				"Not all deities are",
				"kind, as evidenced by",
				"Bael, the warlord of",
				"blood and conquest.",
				"Where his influence",
				"spreads, strife follows,",
				"and battle cries ring."
			),
			new BookPageInfo
			(
				"In contrast, Pax",
				"offers solace in peace",
				"and diplomacy. Her",
				"temples are havens",
				"of respite from the",
				"turmoil that plagues",
				"our lands."
			),
			new BookPageInfo
			(
				"The tapestry of the",
				"divine is complex,",
				"and each thread",
				"represents a deity",
				"with their own agenda,",
				"cult, and influence",
				"upon the mortal realm."
			),
			new BookPageInfo
			(
				"Their interplay is",
				"a spectacle of might,",
				"wits, and wills, a",
				"cosmic dance that",
				"we, mere mortals,",
				"can scarce comprehend.",
				"Yet, their presence"
			),
			new BookPageInfo
			(
				"is felt in every facet",
				"of life, for better or",
				"for worse. In times",
				"of need, to which of",
				"these mighty beings",
				"will you offer your",
				"prayers?"
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
				"Sage Eremis",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the gods ever",
				"be in your favor."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public DeitiesOfBritannia() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Deities of Britannia");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Deities of Britannia");
        }

        public DeitiesOfBritannia(Serial serial) : base(serial)
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
