using System;
using Server;

namespace Server.Items
{
    public class CompendiumOfCuriousConstellations : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Compendium of Curious Constellations", "Celestus the Stargazer",
            new BookPageInfo
            (
                "In the tapestry of",
                "the night sky, the",
                "stars align into",
                "mysterious shapes,",
                "telling tales of old",
                "and foretelling",
                "futures untold."
            ),
            new BookPageInfo
            (
                "This compendium is",
                "dedicated to the",
                "curious constellations",
                "that grace our",
                "heavens. Each",
                "formation is a key to",
                "understanding the",
                "cosmos."
            ),
            new BookPageInfo
            (
                "Behold 'The Great",
                "Galleon', a ship that",
                "sails the celestial",
                "seas. It is said that",
                "sailors gaze upon",
                "this constellation",
                "for good fortune on",
                "long voyages."
            ),
            new BookPageInfo
            (
                "'The Dueling Dragons'",
                "twist and turn in",
                "a dance of destiny.",
                "Astronomers claim",
                "this rare sight",
                "heralds significant",
                "events about to",
                "unfold on earth."
            ),
            new BookPageInfo
            (
                "Not all is grand or",
                "ominous, for 'The",
                "Jester' jests at the",
                "kings and queens",
                "of the night. His",
                "merriment scatters",
                "shooting stars",
                "across the sky."
            ),
            new BookPageInfo
            (
                "Each constellation",
                "tells a story, a piece",
                "of the puzzle in the",
                "enigma of the stars.",
                "As we decipher",
                "these celestial",
                "signs, we glean",
                "wisdom from above."
            ),
            new BookPageInfo
            (
                "As your eyes wander",
                "to the heavens and",
                "traverse the constellations,",
                "may you find",
                "guidance in their",
                "age-old wisdom.",
                "",
                "Celestus the Stargazer"
            ),
            new BookPageInfo
            (
                "Remember, each star",
                "in the sky is a sun to",
                "another world. Our",
                "constellations might",
                "be their mythologies,",
                "their guides, their",
                "wonder."
            ),
            new BookPageInfo
            (
                "So gaze upon these",
                "curious constellations",
                "with reverence, for",
                "they are the silent",
                "guardians of our",
                "nights and the silent",
                "speakers of wisdom",
                "through the ages."
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
                "Celestus the Stargazer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the stars guide",
                "you to your destiny."
            ),
			// Continuing from the last BookPageInfo
			new BookPageInfo
			(
				"Let us then venture",
				"further into the",
				"astral stories written",
				"in our skies. Gaze",
				"upon 'The Minstrel',",
				"whose harp's cosmic",
				"strings play the music",
				"of the spheres."
			),
			new BookPageInfo
			(
				"Next, behold 'The",
				"Lonely Monarch'.",
				"A single star marks",
				"his heart, a beacon",
				"for those seeking",
				"leadership or solace",
				"in the quiet nights."
			),
			new BookPageInfo
			(
				"'The Phantom Knight'",
				"stands guard, a",
				"watchful protector",
				"against the dark",
				"void. His sword",
				"points to the north,",
				"guiding lost souls",
				"to their northern home."
			),
			new BookPageInfo
			(
				"'The Scribe' dips his",
				"pen in the ink of",
				"the Milky Way to",
				"write the destinies",
				"of newborn stars,",
				"and to take note",
				"of the deeds of those",
				"on Earth."
			),
			new BookPageInfo
			(
				"Turn your gaze",
				"to 'The Weaver',",
				"whose loom crafts",
				"the fabric of the",
				"night. It is said",
				"that each thread is",
				"a life, a story being",
				"told in the heavens."
			),
			new BookPageInfo
			(
				"High above, 'The",
				"Sentinel' watches,",
				"a vigilant eye that",
				"never wanes. It is",
				"the beacon for",
				"voyagers, the light",
				"in the darkness for",
				"wayfarers."
			),
			new BookPageInfo
			(
				"Amidst the eternal",
				"waltz of celestial",
				"bodies, 'The Dancers'",
				"play. Two stars in",
				"close embrace, they",
				"remind us of the",
				"joy and beauty of",
				"the cosmos."
			),
			new BookPageInfo
			(
				"'The Alchemist'",
				"mixes ancient potions",
				"upon the firmament,",
				"transmuting leaden",
				"darkness into golden",
				"light, an everlasting",
				"quest for celestial",
				"enlightenment."
			),
			new BookPageInfo
			(
				"In the quiet corners",
				"of the night, 'The",
				"Dreamer' lies, a",
				"peaceful repose",
				"amidst the stars,",
				"whispering secrets",
				"to sleeping children",
				"and old souls alike."
			),
			new BookPageInfo
			(
				"'The Archivist' peers",
				"over his scrolls, a",
				"ledger of every star's",
				"birth and demise.",
				"In his eternal library,",
				"the history of the",
				"universe is kept."
			),
			new BookPageInfo
			(
				"Finally, 'The Voyager'",
				"sets sail, a ship",
				"borne on solar winds,",
				"seeking the farthest",
				"reaches of imagination",
				"and reality, where",
				"stories are yet to",
				"be written."
			),
			new BookPageInfo
			(
				"This compendium",
				"but begins the",
				"chronicle of the",
				"starry hosts. I",
				"entrust you, reader,",
				"to continue the",
				"exploration, for the",
				"heavens are vast."
			),
			new BookPageInfo
			(
				"Celestus the Stargazer",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Seek the stories",
				"written in the cosmos,",
				"and let the stars",
				"illuminate your path",
				"to the great unknown."
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
				"Celestus the Stargazer",
				"In the boundless",
				"night, we find",
				"our smallest lights",
				"reflect the greatest",
				"truths."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CompendiumOfCuriousConstellations() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
			Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Compendium of Curious Constellations");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Compendium of Curious Constellations");
        }

        public CompendiumOfCuriousConstellations(Serial serial) : base(serial)
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
