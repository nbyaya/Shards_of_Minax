using System;
using Server;

namespace Server.Items
{
    public class DarkTalesOfHiddenCults : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Dark Tales of Hidden Cults", "Varath the Chronicler",
            new BookPageInfo
            (
                "In the darkest alleys",
                "and forgotten ruins,",
                "secrets lurk and",
                "conspiracies weave.",
                "The cults that thrive",
                "in the shadows of",
                "the realm are as",
                "numerous as they are"
            ),
            new BookPageInfo
            (
                "sinister. This tome",
                "reveals some of the",
                "most forbidding tales",
                "of hidden societies,",
                "dedicated to ancient",
                "powers and nefarious",
                "rituals. Proceed with",
                "caution, for knowledge"
            ),
            new BookPageInfo
            (
                "is as much a curse",
                "as it is a gift.",
                "",
                "The Cult of the",
                "Eternal Eclipse",
                "worships the darkness",
                "that devours light.",
                "They say during an"
            ),
            new BookPageInfo
            (
                "eclipse, their powers",
                "surge, and the veil",
                "between worlds",
                "thins, allowing",
                "unspeakable horrors",
                "to slip through.",
                "",
                "The Whispering Circle"
            ),
            new BookPageInfo
            (
                "gathers in hushed",
                "tones, revering what",
                "lies beyond death.",
                "Their chants are",
                "rarely louder than",
                "a murmur, yet the",
                "dead are said to",
                "hear them clearly."
            ),
            new BookPageInfo
            (
                "Followers of the",
                "Thousand-eyed Idol",
                "cover their bodies in",
                "eyes. It is believed",
                "they see through the",
                "veil of reality into",
                "the truths that man",
                "was not meant to."
            ),
            new BookPageInfo
            (
                "One should be wary",
                "of the Serpent's Coil,",
                "a cult entwined in",
                "the affairs of state",
                "and monarchy, with",
                "their venomous reach",
                "extending far beyond",
                "the grasp of the crown."
            ),
            new BookPageInfo
            (
                "Each cult has its",
                "own dark allure,",
                "its own path to",
                "power that tempts",
                "even the purest",
                "souls. These pages",
                "shed light upon",
                "their rituals, their"
            ),
            new BookPageInfo
            (
                "eldritch symbols, and",
                "the locations where",
                "their followers congregate",
                "in worship of their",
                "malevolent deities.",
                "",
                "To read is to arm",
                "oneself. But be warned,"
            ),
            new BookPageInfo
            (
                "for such knowledge",
                "can also draw the",
                "unwanted attention of",
                "the cults. Guard this",
                "tome well, and let not",
                "its secrets pass lightly",
                "from your lips.",
                ""
            ),
            new BookPageInfo
            (
                "Varath the Chronicler",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Beware the darkness",
                "that hungers for the",
                "light of your soul."
            ),
            // Continuing from the last BookPageInfo provided previously
			new BookPageInfo
			(
				"The tales herein are",
				"not merely folklore—",
				"they are warnings.",
				"Consider the fate of",
				"the coastal town of",
				"Amber Hollow, where",
				"the cult known as",
				"The Tide's Chanters"
			),
			new BookPageInfo
			(
				"beckoned forth a",
				"beast from the brine.",
				"The sea rose to claim",
				"the land, and screams",
				"echoed beneath the",
				"storm. No soul was",
				"spared, and now the",
				"ruins lie submerged,"
			),
			new BookPageInfo
			(
				"a watery grave for",
				"its once vibrant",
				"community. The cult's",
				"purpose was served,",
				"a sacrifice to their",
				"deep-sea deity.",
				"",
				"Beneath the bustling"
			),
			new BookPageInfo
			(
				"streets of the city of",
				"Mirath, the cult known",
				"as The Silent Hand",
				"operates. Masked",
				"and robed, they cut",
				"out their tongues in",
				"service to their god",
				"of silence, communicating"
			),
			new BookPageInfo
			(
				"only through signs and",
				"whispers. It is said",
				"that to hear their",
				"voice is to invite",
				"death's swift arrival.",
				"They are the unseen",
				"assassins, and their",
				"victims are rumored"
			),
			new BookPageInfo
			(
				"to include kings and",
				"emperors alike. Yet",
				"their ultimate aim",
				"remains shrouded in",
				"mystery, their benefactor",
				"an enigma within an",
				"enigma.",
				""
			),
			new BookPageInfo
			(
				"Far to the north, in",
				"the icy crags of the",
				"Whispering Mountains,",
				"the Brotherhood of",
				"the Shattered Shield",
				"honors the fallen from",
				"a war long forgotten.",
				"They seek to awaken"
			),
			new BookPageInfo
			(
				"a frozen god, trapped",
				"in ice and forgotten",
				"by time. Their chants",
				"echo in the biting",
				"winds, and their",
				"sacrifices are left",
				"on altars of snow,",
				"preserved for eternity."
			),
			new BookPageInfo
			(
				"It is in the nature of",
				"man to seek power,",
				"and these cults offer",
				"such in abundance.",
				"But the price is steep,",
				"and the currency is",
				"often one's soul.",
				"Beware their promises,"
			),
			new BookPageInfo
			(
				"for they are as hollow",
				"as the darkness they",
				"worship. The power",
				"granted comes with",
				"chains unseen, binding",
				"the cultist to the will",
				"of entities whose",
				"appetites are endless."
			),
			new BookPageInfo
			(
				"This chronicle serves",
				"as a testament to the",
				"perils of the unseen",
				"world that thrives in",
				"the shadows of our",
				"own. Let the curious",
				"beware: the truth is",
				"a labyrinth, and some"
			),
			new BookPageInfo
			(
				"corridors lead to",
				"madness. Tread with",
				"caution and guard",
				"your soul well.",
				"",
				"The chronicling of",
				"these dark tales",
				"continues..."
			),
			// You can repeat the pattern to add more blank pages as needed.
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

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public DarkTalesOfHiddenCults() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000 for a dark and mysterious look
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Dark Tales of Hidden Cults");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Dark Tales of Hidden Cults");
        }

        public DarkTalesOfHiddenCults(Serial serial) : base(serial)
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
