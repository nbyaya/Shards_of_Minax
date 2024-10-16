using System;
using Server;

namespace Server.Items
{
    public class WerewolfsGuideToFur : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Werewolf's Guide to Fur", "Lupus the Hirsute",
            new BookPageInfo
            (
                "To the lycanthrope",
                "newly turned or",
                "ancient in years,",
                "this tome offers",
                "insight into the",
                "care and keeping",
                "of thy dual coat.",
                "I am Lupus, the"
            ),
            new BookPageInfo
            (
                "Hirsute, offering my",
                "furry wisdom.",
                "",
                "Our fur is our pride,",
                "marking the blend of",
                "human intellect with",
                "the wild's embrace.",
                "Thus, grooming is not"
            ),
            new BookPageInfo
            (
                "mere vanity; it is",
                "vital for our",
                "well-being. Tangles",
                "lead to itches, which",
                "lead to scratches,",
                "which lead to a",
                "miserable beast!",
                ""
            ),
            new BookPageInfo
            (
                "First, the moonlit",
                "bath - bask in a",
                "stream, letting the",
                "waters cleanse your",
                "sorrows and burrs.",
                "Then, to dry, a",
                "shake - start at the",
                "head, ending at the"
            ),
            new BookPageInfo
            (
                "tail, an invigorating",
                "dance of drops!",
                "",
                "Brushing is next,",
                "with bristles of yew.",
                "A werewolf's brush",
                "must be sturdy to",
                "weather our robust"
            ),
            new BookPageInfo
            (
                "fur. Begin at the",
                "head, work down to",
                "the tail, always with",
                "the grain - never",
                "against, lest you",
                "wish to incite a",
                "furry fury within.",
                ""
            ),
            new BookPageInfo
            (
                "And what of the full",
                "moon's toll on our",
                "coat? Fear not; the",
                "luster it brings is",
                "rivaled only by",
                "the stars. Let it",
                "shine, fellow",
                "werewolves, let it"
            ),
            new BookPageInfo
            (
                "shine!",
                "",
                "But, a warning:",
                "Avoid silver combs,",
                "for they bring more",
                "than just split ends.",
                "",
                "This guide also"
            ),
            new BookPageInfo
            (
                "contains advice on",
                "seasonal shedding,",
                "how to deal with",
                "fleas (a bite for a",
                "bite is not advised),",
                "and the best way to",
                "curl up in one's",
                "human bed without"
            ),
            new BookPageInfo
            (
                "leaving a furry",
                "evidence of our",
                "nocturnal romps.",
                "",
                "In closing, remember",
                "that our fur is the",
                "cloak gifted by the",
                "wild. Wear it with"
            ),
            new BookPageInfo
            (
                "pride, keep it with",
                "care, and let the",
                "world know that",
                "you are a creature",
                "not of mere myth,",
                "but of majestic",
                "reality.",
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
                "Lupus the Hirsute",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your howls be",
                "heard and your fur",
                "remain ever lush."
            ),
			new BookPageInfo
			(
				"To deal with knots,",
				"be as the patient",
				"hunter. Work them",
				"free with gentle",
				"paws, lest you wish",
				"to harm the very",
				"essence of our",
				"wild beauty."
			),
			new BookPageInfo
			(
				"For the werewolf",
				"warrior, fear not -",
				"your battle scars",
				"do not mar your",
				"fur. They speak of",
				"strength, of survival,",
				"a testament to your",
				"ferocity and courage."
			),
			new BookPageInfo
			(
				"In times of peace,",
				"blend with the",
				"human fold. A",
				"trim here, a tuck",
				"there, let not your",
				"lustrous mane betray",
				"the beast within,",
				"whilst you walk"
			),
			new BookPageInfo
			(
				"amongst them.",
				"",
				"Come winter's chill,",
				"thicker grows our",
				"shield. Embrace the",
				"fluff, let it insulate",
				"the warmth of our",
				"hearts against the"
			),
			new BookPageInfo
			(
				"frost's bitter bite.",
				"",
				"Summer's heat may",
				"tempt you to shear",
				"your coat. Be",
				"wary, for sun's",
				"embrace is fleeting,",
				"and the moon's",
				"caress ever cool."
			),
			new BookPageInfo
			(
				"For those with pups,",
				"your fur is comfort,",
				"a nursery of softness.",
				"Groom your young with",
				"tongue and love,",
				"teach them the ways",
				"of the fur, as I now",
				"impart to thee."
			),
			new BookPageInfo
			(
				"When the transformation",
				"takes you, and the fur",
				"spreads like wildfire",
				"across skin, remember:",
				"the pain of change",
				"yields the pleasure",
				"of the wild's caress.",
				"Revel in your new"
			),
			new BookPageInfo
			(
				"cloak, let it be your",
				"renewal.",
				"",
				"Ah, the full moon!",
				"When our fur becomes",
				"silvered, when the",
				"night becomes our",
				"stage, let not the"
			),
			new BookPageInfo
			(
				"burrs of the woods",
				"clinging to your",
				"fur dampen your",
				"spirit. They are but",
				"the forest's way of",
				"keeping a piece of",
				"you, a memory of",
				"your passage."
			),
			new BookPageInfo
			(
				"Let us speak of rain,",
				"when each drop",
				"glistens upon our",
				"backs, a tapestry of",
				"nature's jewels. Shake",
				"not the blessing off",
				"too soon, for it carries",
				"the scent of the world."
			),
			new BookPageInfo
			(
				"And when the snow",
				"falls, let it catch in",
				"your fur, each flake a",
				"story, a silent piece",
				"of the sky's soul.",
				"Carry it proudly, until",
				"the warmth of your",
				"being bids it farewell."
			),
			new BookPageInfo
			(
				"Remember, fellow kin,",
				"our fur is not a curse",
				"but a crown, not a",
				"burden but a blessing.",
				"It is the bridge",
				"between man and",
				"beast, a badge of",
				"our unique fate."
			),
			new BookPageInfo
			(
				"So wear your fur",
				"with pride, dear",
				"brothers and sisters",
				"of the moon. Let it",
				"tell your story as",
				"you walk the paths",
				"of the forest, or the",
				"stone-laid streets of"
			),
			new BookPageInfo
			(
				"civilization.",
				"",
				"And in the final",
				"moment, when fur",
				"turns to mere",
				"memory, know that",
				"you lived as both",
				"beast and beauty,",
				"wild and wise."
			),
			new BookPageInfo
			(
				"May your fur be ever",
				"soft, your howls ever",
				"strong, and your",
				"spirit ever free.",
				"",
				"Lupus the Hirsute",
				"has spoken."
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
				"Lupus the Hirsute",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Under the gaze of the",
				"full moon, may your",
				"path be clear and",
				"your fur unblemished."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public WerewolfsGuideToFur() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000 to differentiate from the previous book
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Werewolf's Guide to Fur");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Werewolf's Guide to Fur");
        }

        public WerewolfsGuideToFur(Serial serial) : base(serial)
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
