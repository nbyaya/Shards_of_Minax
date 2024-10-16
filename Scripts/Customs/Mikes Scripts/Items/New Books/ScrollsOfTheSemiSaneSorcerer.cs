using System;
using Server;

namespace Server.Items
{
    public class ScrollsOfTheSemiSaneSorcerer : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Scrolls of the Semi-Sane Sorcerer", "Merlinus Halfwit",
            new BookPageInfo
            (
                "In these scrolls, I",
                "Merlinus Halfwit,",
                "chronicle the edges",
                "of sanity where magic",
                "and madness meet.",
                "My mind teeters on",
                "the brink of genius",
                "and absurdity."
            ),
            new BookPageInfo
            (
                "Let the reader beware:",
                "the incantations and",
                "musing herein are not",
                "for the faint of heart",
                "nor the rigid of",
                "thought. They are",
                "scribbles of brilliance",
                "shadowed by lunacy."
            ),
            new BookPageInfo
            (
                "I once sought to",
                "capture the moon's",
                "glow. Instead, I",
                "enchanted my cat",
                "to sing operas at",
                "the full moon. A",
                "serenade of woes",
                "and meows."
            ),
            new BookPageInfo
            (
                "Another time, in",
                "efforts to turn lead",
                "to gold, I brewed a",
                "potion so potent,",
                "it turned my teeth",
                "to silver. Alas,",
                "wealth is hard to",
                "eat with such"
            ),
            new BookPageInfo
            (
                "cutlery for teeth.",
                "Not all was for naught,",
                "for each peculiar",
                "plight brought insights",
                "untold. To the daring",
                "sorcerer, these scrolls",
                "unfold paths untaken,",
                "and truths unspoken."
            ),
            new BookPageInfo
            (
                "To animate the inanimate",
                "is a feat most bold.",
                "Yet beware, a broom",
                "enchanted with such",
                "zeal may flood thy",
                "tower, demanding a",
                "new ark to be built.",
                "So learned I in"
            ),
            new BookPageInfo
            (
                "soggy tomes post deluge.",
                "If thou doth dare to",
                "dance with demons,",
                "practice first thy",
                "steps with sprites.",
                "Less ye be led in",
                "a tango that scorches",
                "thy soles and soul."
            ),
            new BookPageInfo
            (
                "Among these pages lie",
                "the musings of a",
                "mind that wanders",
                "betwixt the brink",
                "of brilliance and",
                "the abyss of insanity.",
                "A mind not lost, but",
                "freed by the arcane."
            ),
            new BookPageInfo
            (
                "Take heed, aspiring",
                "conjurers of the",
                "cosmos. For within",
                "the folds of these",
                "scrolls, ye shall find",
                "not only spells to",
                "bend the world's",
                "weave, but also"
            ),
            new BookPageInfo
            (
                "tales to unravel",
                "the sanity of the",
                "spellcaster. Yet is",
                "it not in unraveling",
                "that the true tapestry",
                "is revealed?",
                "",
                "Merlinus Halfwit"
            ),
            new BookPageInfo
            (
                "In my pursuit, the",
                "veil of reality oft",
                "seems as gossamer",
                "threads in a gale",
                "of sorcery. And",
                "thus, I write with",
                "ink that whispers",
                "of other worlds."
            ),
            new BookPageInfo
            (
                "And so, dear reader,",
                "embark upon these",
                "scrolls with a spirit",
                "of wonder, but tether",
                "thy soul to sanity,",
                "lest you become",
                "entwined in the",
                "sorcerer's spiral."
            ),
            new BookPageInfo
            (
                "Merlinus Halfwit",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your mind navigate",
                "the arcane eddies with",
                "more grace than mine."
            ),
			new BookPageInfo
			(
				"Of the time I sought",
				"to bestow speech upon",
				"a stone, only to be met",
				"with stony silence.",
				"Yet, as I turned to",
				"leave, the rock began",
				"to mock me most",
				"ruthlessly."
			),
			new BookPageInfo
			(
				"Or when I mixed a",
				"tincture to spy",
				"invisible sprites,",
				"and found myself",
				"arguing with my own",
				"shadow till dawn.",
				"It was a debate of",
				"singular perspective."
			),
			new BookPageInfo
			(
				"I've transcribed here",
				"my 'Accidental Beastiary',",
				"a collection of creatures",
				"born from botched",
				"spells. Like the",
				"flamingo-crocodile,",
				"a most conflicted",
				"creature, indeed."
			),
			new BookPageInfo
			(
				"And of course, the",
				"time I attempted to",
				"brew a love potion,",
				"only to fall deeply",
				"in love with a",
				"particularly charming",
				"kettle. It was a",
				"passionate affair."
			),
			new BookPageInfo
			(
				"Let us not forget the",
				"incident with the",
				"enchanted mirror—",
				"intended to show one's",
				"true self. It promptly",
				"shattered upon my",
				"gaze. An overly",
				"critical piece."
			),
			new BookPageInfo
			(
				"Within these pages,",
				"I also ponder the",
				"philosophical. Such as,",
				"if one conjures a",
				"feast but has no",
				"one to share it with,",
				"does it satisfy hunger",
				"or magnify solitude?"
			),
			new BookPageInfo
			(
				"My quest for",
				"eternal life led me",
				"to converse with",
				"a turtle for seven",
				"years. The wisdom he",
				"departed was to",
				"walk slowly and",
				"carry a hard shell."
			),
			new BookPageInfo
			(
				"In the pursuit of",
				"knowledge, I once",
				"turned my brain",
				"invisible. Finding it",
				"proved to be a",
				"puzzling endeavor.",
				"My thoughts have",
				"never been clearer."
			),
			new BookPageInfo
			(
				"Oh, and the army of",
				"gingerbread men",
				"conjured for conquest.",
				"Their sweet victory",
				"was instead claimed by",
				"a flock of ravenous",
				"children from the",
				"nearby village."
			),
			new BookPageInfo
			(
				"I once gave life to",
				"paintings, only to",
				"be critiqued on my",
				"artistry by my own",
				"creations. They were",
				"not fond of my",
				"choice in colors,",
				"nor my brush strokes."
			),
			new BookPageInfo
			(
				"The 'Infinite Library'",
				"spell was a triumph,",
				"until the books",
				"decided they’d rather",
				"read themselves than",
				"be perused by me.",
				"Quite the literate",
				"rebellion."
			),
			new BookPageInfo
			(
				"A cautionary note:",
				"Beware the spell of",
				"Multiple Manifestations.",
				"One's company, two's",
				"a crowd, and three",
				"Merlinus' arguing over",
				"dinner plans is a",
				"nightmare."
			),
			new BookPageInfo
			(
				"And let us conclude",
				"with the Golem I",
				"crafted to tidy my",
				"abode. It swept the",
				"dirt under rugs and",
				"deemed the task",
				"complete. I suppose",
				"even golems take"
			),
			new BookPageInfo
			(
				"shortcuts. So stands",
				"the testament of my",
				"endeavors, a sorcerer",
				"whose magic is as",
				"unpredictable as the",
				"whims of fate. May",
				"thy own spells",
				"fare better."
			),
			new BookPageInfo
			(
				"Yet, fear not these",
				"magical musings,",
				"nor the strange",
				"journeys they recant.",
				"For within the chaos",
				"lies the beauty of",
				"possibility, the",
				"essence of wonder."
			),
			new BookPageInfo
			(
				"Merlinus Halfwit",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Embrace the madness",
				"of magic, for in its",
				"whirlwind, one finds",
				"true freedom."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ScrollsOfTheSemiSaneSorcerer() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Scrolls of the Semi-Sane Sorcerer");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Scrolls of the Semi-Sane Sorcerer");
        }

        public ScrollsOfTheSemiSaneSorcerer(Serial serial) : base(serial)
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
