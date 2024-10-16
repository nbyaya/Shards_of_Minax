using System;
using Server;

namespace Server.Items
{
    public class TheMaidensQuest : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Maiden's Quest", "Seraphina the Brave",
            new BookPageInfo
            (
                "In an age long past,",
                "when dragons soared",
                "and heroes were born",
                "from the ashes of",
                "trial and tribulation,",
                "there lived a maiden",
                "with heart fierce and",
                "eyes full of stars."
            ),
            new BookPageInfo
            (
                "Her name was Seraphina,",
                "a simple weaver's daughter,",
                "whose dreams transcended",
                "the confines of her",
                "humble beginnings.",
                "Yearning for adventure,",
                "she embarked on a quest",
                "of valor and discovery."
            ),
            new BookPageInfo
            (
                "Armed with naught but",
                "a will of iron and a",
                "cloak woven from",
                "moonlight, Seraphina",
                "set forth into the",
                "unknown. The world",
                "whispered of a",
                "beast with scales"
            ),
            new BookPageInfo
            (
                "of midnight and breath",
                "that could forge",
                "empires or lay them",
                "to ruin. The Dragon of",
                "Eldur's Peak, an",
                "enigma, called to",
                "her with a siren's",
                "promise of destiny."
            ),
            new BookPageInfo
            (
                "This tome chronicles",
                "Seraphina's journey,",
                "through whispering",
                "woods and over",
                "treacherous peaks.",
                "Allies found in",
                "the unlikeliest of",
                "companions, a bard"
            ),
            new BookPageInfo
            (
                "whose songs could",
                "charm even the most",
                "reluctant heart, and",
                "a knight whose",
                "sword had long",
                "forgotten the thrill",
                "of a worthy cause.",
                ""
            ),
            new BookPageInfo
            (
                "Together, they faced",
                "fiends and furies,",
                "their bonds tempered",
                "in the fires of",
                "adversity. Each trial,",
                "from the riddles of",
                "sphinxes to the",
                "shadows of deceitful"
            ),
            new BookPageInfo
            (
                "fae, shaped Seraphina,",
                "forging her spirit into",
                "a legend that would",
                "echo through the",
                "annals of time.",
                "But no story of valor",
                "is without its darkness,",
                "no quest without"
            ),
            new BookPageInfo
            (
                "its cost. A truth",
                "Seraphina learned",
                "as the Dragon of",
                "Eldur's Peak unveiled",
                "the final price of",
                "her journey.",
                "Yet, it is not the",
                "end that we remember,"
            ),
            new BookPageInfo
            (
                "but the path walked",
                "to reach it. This",
                "book is but a vessel",
                "for her legacy—a",
                "maiden not of noble",
                "birth, but of an",
                "indomitable will",
                "that shaped her fate."
            ),
            new BookPageInfo
            (
                "Let her tale inspire",
                "those who dare to",
                "dream beyond the",
                "horizon, to take the",
                "first step on their",
                "own quest. For in the",
                "heart of the bravest,",
                "the spirit of Seraphina"
            ),
            new BookPageInfo
            (
                "forever soars.",
                "",
                "",
                "",
                "",
                "",
                "Seraphina the Brave",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your path be true",
                "and your heart brave."
            ),
            // Continuing from the existing BookPageInfo instances...
			new BookPageInfo
			(
				"Upon a moonlit eve,",
				"within the ancient groves",
				"of Eldertree, the trio's",
				"courage was put to test.",
				"Spirits of yore, bound",
				"to the earthly realm,",
				"whispered of sorrow",
				"and ceaseless torment."
			),
			new BookPageInfo
			(
				"Only the pure of heart",
				"could soothe the restless",
				"and guide them toward",
				"the Everlight. With a",
				"voice soft and lilting,",
				"Seraphina sang the",
				"lullaby of the stars,",
				"and one by one,"
			),
			new BookPageInfo
			(
				"the spirits found peace,",
				"leaving behind them",
				"blessings of the ancients,",
				"bestowed upon the maiden",
				"who dared to heed their",
				"plaintive cries. Among the",
				"relics was a pendant,",
				"shimmering with the light"
			),
			new BookPageInfo
			(
				"of the very first dawn.",
				"Thus, with newfound hope,",
				"the band ventured forth,",
				"into the caverns of",
				"Wyrmbane, where darkness",
				"clung to all surfaces,",
				"and shadows whispered",
				"of avarice and deceit."
			),
			new BookPageInfo
			(
				"Deep within, the scaled",
				"beast awaited, eyes",
				"glittering like jewels",
				"and fangs bared in",
				"defiance. The dragon",
				"spoke not in roars,",
				"but in riddles, a tongue",
				"as old as the mountains."
			),
			new BookPageInfo
			(
				"With wisdom garnered",
				"from the trials faced,",
				"the knight stepped forth,",
				"his voice steady. Each",
				"riddle he parried with",
				"truths untainted, until",
				"the beast relented, its",
				"wrath abated by honor."
			),
			new BookPageInfo
			(
				"A pact was forged that",
				"night, under the banner",
				"of starlight and the",
				"maiden's unwavering gaze.",
				"The dragon, bound by",
				"ancient laws, bestowed",
				"upon them a single scale,",
				"a shard of its essence."
			),
			new BookPageInfo
			(
				"The quest, however,",
				"demanded its due, as",
				"the path that unfolds",
				"before the valiant",
				"can never be forsworn.",
				"A sacrifice was made,",
				"a destiny embraced,",
				"and Seraphina's fate"
			),
			new BookPageInfo
			(
				"was woven with threads",
				"of undying legend.",
				"The scale, harbinger",
				"of change, held secrets",
				"of the world's very",
				"weave. Seraphina's",
				"touch awakened its",
				"power, her soul's echo"
			),
			new BookPageInfo
			(
				"binding with the ancient",
				"might of dragons. With",
				"the bard's song and",
				"the knight's oath, they",
				"ushered in an era of",
				"newfound harmony",
				"between mankind and",
				"the wyrmkin."
			),
			new BookPageInfo
			(
				"Yet the tale does not",
				"conclude upon this",
				"mountain's peak, nor",
				"with the dragon's benediction.",
				"For each end is but a",
				"new beginning, woven",
				"into the tapestry of",
				"this world's unending"
			),
			new BookPageInfo
			(
				"story. And as the",
				"dawn breaks upon",
				"new horizons, so too",
				"does Seraphina's journey",
				"continue beyond the",
				"mortal ken, her name",
				"forevermore a beacon",
				"of courage and hope."
			),
			new BookPageInfo
			(
				"Within these pages lies",
				"not merely a record of",
				"Seraphina's deeds, but",
				"an invitation to those",
				"who yearn for more;",
				"to seek the uncharted,",
				"to dream fiercely, and to",
				"live boldly as she did."
			),
			new BookPageInfo
			(
				"Let the maiden's quest",
				"kindle a fire in your heart,",
				"guide your steps through",
				"darkness, and remind you",
				"that even the smallest",
				"star can illuminate the",
				"night's darkest corner.",
				""
			),
			new BookPageInfo
			(
				"So take heed, dear reader,",
				"of the legacy bestowed",
				"upon us by Seraphina,",
				"the weaver's daughter,",
				"who became a legend.",
				"May her story inspire",
				"your own quests",
				"for greatness."
			),
			new BookPageInfo
			(
				"And as you close this",
				"tome, remember that",
				"within you lies the",
				"power to change the",
				"world—no dragon",
				"required, only the",
				"heart of one willing",
				"to embark on their"
			),
			new BookPageInfo
			(
				"very own quest.",
				"",
				"",
				"Seraphina the Brave",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"With valor in your heart,",
				"and stars in your eyes."
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

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheMaidensQuest() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Maiden's Quest");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Maiden's Quest");
        }

        public TheMaidensQuest(Serial serial) : base(serial)
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
