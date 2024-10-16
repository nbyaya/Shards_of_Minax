using System;
using Server;

namespace Server.Items
{
    public class TheClumsyNecromancer : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Clumsy Necromancer", "Grimwald the Unsteady",
            new BookPageInfo
            (
                "In the annals of the arcane,",
                "the mishaps of a necromancer",
                "rarely garner laughter. Yet",
                "herein lies the tale of",
                "Grimwald the Unsteady,",
                "a necromancer with two",
                "left feet, both figuratively",
                "and through one accursed"
            ),
            new BookPageInfo
            (
                "spell, quite literally.",
                "",
                "My journey into the dark",
                "arts began with a stumble,",
                "into a grave I was meant",
                "to rob of its silence.",
                "Instead, I awoke the dead",
                "with a rather loud,",
                "unintended 'clang!'"
            ),
            new BookPageInfo
            (
                "Since that fateful eve,",
                "my ventures into necromancy",
                "have been... uniquely",
                "troubled. Where most",
                "would raise an obedient",
                "skeleton, I seemed to",
                "summon only the ones",
                "with a peculiar penchant"
            ),
            new BookPageInfo
            (
                "for dance. A waltzing",
                "bone-man is of little use",
                "in the grim tasks for",
                "which they are raised.",
                "",
                "And woe betide the time",
                "I tried to call forth a",
                "specter. Instead of a"
            ),
            new BookPageInfo
            (
                "fearsome ghost, I found",
                "myself haunted by an",
                "ethereal jester, keen",
                "on floating objects",
                "about and pulling",
                "ethereal pranks.",
                "",
                "Even my familiars"
            ),
            new BookPageInfo
            (
                "cannot escape my",
                "accidental curse.",
                "My raven turned",
                "pink, a color most",
                "unnatural and entirely",
                "noticeable. And the",
                "black cat? Now",
                "it only chases"
            ),
            new BookPageInfo
            (
                "its tail, round and",
                "round, paying no heed",
                "to the mysteries of the",
                "beyond that I so wish",
                "to explore.",
                "",
                "Perhaps the most",
                "perilous of my"
            ),
            new BookPageInfo
            (
                "endeavors was the",
                "zombie that I raised,",
                "only to have it develop",
                "an insatiable appetite",
                "for my spellbooks rather",
                "than brains. Let's just",
                "say my library is now",
                "quite 'digested.'"
            ),
            new BookPageInfo
            (
                "Yet, despite these",
                "setbacks, I persevere.",
                "For is it not the",
                "mark of a true",
                "practitioner of the",
                "necromantic arts to",
                "face death (and",
                "occasional ridicule)"
            ),
            new BookPageInfo
            (
                "with a steady hand?",
                "Though in my case,",
                "perhaps 'steady' is",
                "too strong a word.",
                "",
                "I, Grimwald the",
                "Unsteady, embrace my",
                "blunders with humility"
            ),
            new BookPageInfo
            (
                "and the hope that",
                "tomorrow's rituals",
                "may go awry in",
                "new, less catastrophic",
                "ways. Until then,",
                "I chronicle my blunders",
                "as lessons and",
                "entertainment for"
            ),
            new BookPageInfo
            (
                "fellow necromancers,",
                "that they may tread",
                "with greater care",
                "and perhaps, a touch",
                "of trepidation for the",
                "clumsiness that lies",
                "within us all."
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
                "Grimwald the Unsteady",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your dead rise",
                "more gracefully than mine."
            ),
			// Continuing from the last BookPageInfo
			new BookPageInfo
			(
				"The midnight chime had",
				"barely faded when I began",
				"an incantation of shadow.",
				"The spell was to veil me",
				"in darkness, a shroud of",
				"invisibility. Yet the only",
				"thing it managed to",
				"disappear was my"
			),
			new BookPageInfo
			(
				"apprentice's eyebrows.",
				"To this day, they have",
				"not returned, and he",
				"casts a skeptical glance",
				"upon any potion or spell",
				"I concoct for hair growth.",
				"",
				"Next was the Golem of"
			),
			new BookPageInfo
			(
				"Guardianship, sculpted",
				"from graveyard clay to",
				"defend the sacred crypts.",
				"Instead of a sentinel,",
				"I somehow animated a",
				"clumsy giant, more apt",
				"for pottery than protection,",
				"shaping vessels as it"
			),
			new BookPageInfo
			(
				"trod and trampled the",
				"very graves it was meant",
				"to guard. A mortifying",
				"blunder, one that took",
				"many apologies and",
				"restorations to mend.",
				"",
				"And let us not forget the"
			),
			new BookPageInfo
			(
				"Phantom Steed I endeavored",
				"to summon, for swift passage",
				"through the spectral veil.",
				"Alas, what I called forth",
				"was less steed and more",
				"sea creature, with tentacles",
				"that grappled comically with",
				"the very air it sought to"
			),
			new BookPageInfo
			(
				"gallop through. My journey",
				"that night was delayed as",
				"I untangled the beast from",
				"an unfortunate willow tree.",
				"",
				"In another instance, a simple",
				"bone-repair charm intended",
				"for a broken finger"
			),
			new BookPageInfo
			(
				"resulted in the afflicted",
				"hand mirroring the opposite,",
				"rendering a once left-handed",
				"assistant into an involuntary",
				"ambidextrous scribe. He",
				"still struggles with the",
				"unexpected duality during",
				"his supper."
			),
			new BookPageInfo
			(
				"However, not all is lost.",
				"Amidst the cacophony of",
				"my failures, there have been",
				"whispers of success. A ghost",
				"I once sought to banish",
				"chose instead to enlighten",
				"me with ancient knowledge",
				"forgotten by time, a gift"
			),
			new BookPageInfo
			(
				"I dare say, for enduring",
				"my otherwise intrusive",
				"conjuration.",
				"",
				"So, here I stand,",
				"Grimwald the Unsteady,",
				"clad in robes frayed by",
				"mystical misfires and"
			),
			new BookPageInfo
			(
				"pockets full of components",
				"yet to betray me. I pen",
				"these tales as a beacon",
				"of hope to the inept,",
				"the clumsy, and the",
				"chronically unfortunate.",
				"For in the vast, unforgiving",
				"expanse of necromancy,"
			),
			new BookPageInfo
			(
				"there is room for laughter,",
				"lessons, and the perseverance",
				"to cast yet another day.",
				"",
				"And to you, dear reader,",
				"who may be snickering",
				"behind the safety of",
				"these scrawled words,"
			),
			new BookPageInfo
			(
				"know that magic is a",
				"fickle friend, and it",
				"may one day leave you",
				"as bewildered as I. But",
				"fear not, for you are in",
				"good company. May your",
				"incantations be more",
				"fortunate, and your"
			),
			new BookPageInfo
			(
				"necromancy less comedic,",
				"than the tales within this",
				"modest tome. And if",
				"not, may you at least",
				"find solace in knowing",
				"your misadventures may",
				"one day grace the pages",
				"of a book like this,"
			),
			new BookPageInfo
			(
				"bringing solace and chuckles",
				"to the next generation of",
				"clumsy casters across the",
				"realm.",
				"",
				"Yours in shared misfortune,",
				"Grimwald the Unsteady"
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
				"Grimwald the Unsteady",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May your nights be calm",
				"and your graves undisturbed."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheClumsyNecromancer() : base(false)
        {
            // Set the hue to a random dark color
            Hue = Utility.RandomMinMax(1102, 1149);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Clumsy Necromancer");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Clumsy Necromancer");
        }

        public TheClumsyNecromancer(Serial serial) : base(serial)
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
