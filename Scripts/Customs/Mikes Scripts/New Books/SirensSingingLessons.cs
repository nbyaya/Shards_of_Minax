using System;
using Server;

namespace Server.Items
{
    public class SirensSingingLessons : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Sirens' Singing Lessons", "Cantarella the Siren",
            new BookPageInfo
            (
                "Beneath the waves and",
                "the moonlit sea, a song",
                "does carry, far and free.",
                "I am Cantarella, siren",
                "of the deep, with secrets",
                "of the song I keep."
            ),
            new BookPageInfo
            (
                "Let this tome guide",
                "thy voice to dance",
                "upon the ocean's vast",
                "expanse. Singing is not",
                "just for the gifted,",
                "but a craft to be",
                "uplifted."
            ),
            new BookPageInfo
            (
                "Lesson one begins with",
                "breath so deep, filling",
                "lungs as if for an",
                "eternal sleep. Release",
                "with grace, let notes",
                "embrace the space."
            ),
            new BookPageInfo
            (
                "Next, your pitch must",
                "ebb and flow, like",
                "tides under lunar",
                "glow. High and low,",
                "fast then slow, perform",
                "each note's careful show."
            ),
            new BookPageInfo
            (
                "A siren's power lies in",
                "emotion, a heartfelt",
                "potion. Sing with love,",
                "sing with pain, through",
                "sunshine and through",
                "rain."
            ),
            new BookPageInfo
            (
                "Beware the anger of the",
                "storm, for in such rage",
                "a siren's form can harm",
                "the voice and charm.",
                "Practice calm within",
                "the norm."
            ),
            new BookPageInfo
            (
                "Weave the magic of",
                "the song, let it grow",
                "steady and strong. It's",
                "a siren's spell, where",
                "all tales dwell.",
                ""
            ),
            new BookPageInfo
            (
                "With these lessons,",
                "embark on thy quest,",
                "to sing as sirens, 'bove",
                "all the rest. But heed",
                "this well, use thy gift",
                "to compel."
            ),
            new BookPageInfo
            (
                "For song is an art,",
                "not just a lure. Sing",
                "for beauty, sing pure.",
                "May thy voice be a",
                "balm and cure, not",
                "just a siren's allure."
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
                "Cantarella the Siren",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May thy voice ring",
                "true, as a siren's call",
                "unto the blue."
            ),
			// Continuing from the previous content...
			new BookPageInfo
			(
				"The cadence of waves",
				"against the shore, teach",
				"the rhythm you must",
				"adore. Let not your",
				"tempo flounder, let it",
				"rise and fall, with",
				"power."
			),
			new BookPageInfo
			(
				"Harmony, sweet and",
				"clear, as when the",
				"moon's reflection",
				"does appear. Blend thy",
				"voice with the sea's",
				"chorus, a symphony of",
				"sounds for us."
			),
			new BookPageInfo
			(
				"A trill, a warble, a",
				"siren's trinket, practice",
				"these tricks, in a blink",
				"wink it. Such flair adds",
				"life, to a song as sharp",
				"as a knife."
			),
			new BookPageInfo
			(
				"Vibrato is the heart's",
				"quake, vibrating notes",
				"that gently shake. As",
				"waves on a midsummer",
				"lake, let each tone",
				"resonate and partake."
			),
			new BookPageInfo
			(
				"But singing is naught",
				"without the soul's part.",
				"For each note you",
				"impart, must spring",
				"forth from a loving",
				"heart."
			),
			new BookPageInfo
			(
				"Heed the call of the",
				"depths below, where",
				"ancient creatures",
				"slowly go. In the dark,",
				"their songs they bestow,",
				"to teach us what they",
				"know."
			),
			new BookPageInfo
			(
				"When the storm's ire",
				"is woken, and the sky's",
				"promise broken, sing",
				"soft the lullaby, that",
				"calms the tempest's",
				"sigh."
			),
			new BookPageInfo
			(
				"In the silence of the",
				"world above, your",
				"melody must carry,",
				"dove. Pure and true,",
				"aim your tune, to the",
				"stars and the moon."
			),
			new BookPageInfo
			(
				"Finally, a siren's song",
				"is never done, it weaves",
				"with the setting sun.",
				"Through night till morn,",
				"it carries on, from dusk",
				"till dawn."
			),
			new BookPageInfo
			(
				"This tome shall end, but",
				"your journey not, seek",
				"the wisdom that can't",
				"be bought. Sing to the",
				"sky, the earth, the sea,",
				"sing to them, and sing",
				"to me."
			),
			new BookPageInfo
			(
				"And remember, siren",
				"kin, our songs are",
				"treasure from within.",
				"May they never fade",
				"or thin, may they echo",
				"out and in."
			),
			new BookPageInfo
			(
				"Cantarella the Siren",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"End of lessons. Now",
				"your voice, let it rise,",
				"let it be your most",
				"cherished prize."
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
			)
			// End of book content

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SirensSingingLessons() : base(false)
        {
            // Set the hue to a random color, perhaps something reminiscent of the sea
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Sirens' Singing Lessons");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Sirens' Singing Lessons");
        }

        public SirensSingingLessons(Serial serial) : base(serial)
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
