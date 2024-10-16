using System;
using Server;

namespace Server.Items
{
    public class YarnsOfTheYeti : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Yarns of the Yeti", "Throgmorton the Thinker",
            new BookPageInfo
            (
                "In the silent shroud",
                "of the winter's veil,",
                "within the heart of",
                "the icy expanse, lies",
                "a tale seldom told,",
                "of a creature so bold,",
                "in the frostbitten",
                "peaks dwells the Yeti,"
            ),
            new BookPageInfo
            (
                "shrouded in snow,",
                "his stories untold.",
                "I am Throgmorton,",
                "seeker of truths",
                "long forgotten and",
                "narrator of the odd.",
                "This tome is a",
                "collection of my"
            ),
            new BookPageInfo
            (
                "encounters with",
                "the mysterious Yeti,",
                "a being of folklore,",
                "and astonishing",
                "reality. A silent",
                "giant, with fur as",
                "white as the mountain",
                "peaks it calls home."
            ),
            new BookPageInfo
            (
                "These yarns are spun",
                "from my own travels,",
                "gathered whispers,",
                "and the scattered",
                "tales of the brave",
                "few who have glimpsed",
                "the Yeti and lived to",
                "speak of the beast."
            ),
            new BookPageInfo
            (
                "One might wonder,",
                "is the Yeti a beast?",
                "Or a guardian of",
                "nature, a shy giant",
                "amidst the frozen",
                "wastes? Each yarn",
                "will delve into a",
                "facet of his legend."
            ),
            new BookPageInfo
            (
                "From the Yeti who",
                "saved a lost child,",
                "to the one who",
                "destroyed a village",
                "that dared to harm",
                "the mountain. The",
                "Yeti: a terror,",
                "a savior, or perhaps"
            ),
            new BookPageInfo
            (
                "merely a creature",
                "trying to exist?",
                "I invite you, reader,",
                "to join me through",
                "blizzards and the",
                "starry nights under",
                "the aurora, to",
                "discover the Yeti."
            ),
            new BookPageInfo
            (
                "Let us sit by the",
                "fire's warmth, as",
                "the wind howls",
                "outside, and share",
                "these yarns. We",
                "shall unravel the",
                "mysteries together",
                "and perhaps find"
            ),
            new BookPageInfo
            (
                "that the Yeti is",
                "not just a creature,",
                "but a symbol of",
                "our own primal",
                "nature and our",
                "relationship with",
                "the wilds. May",
                "these tales enlighten."
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
                "Throgmorton the Thinker",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the tracks in the",
                "snow lead you to",
                "wisdom, and the",
                "echoes in the mountains",
                "to understanding."
            ),
			// ... previous content
			new BookPageInfo
			(
				"Amongst these stories,",
				"I recount the yarn of",
				"the Yeti and the",
				"Midnight Sun, where",
				"a village's doom was",
				"undone by a beast's",
				"lament. A Yeti's cry,",
				"they say, reversed the"
			),
			new BookPageInfo
			(
				"curse of endless night,",
				"bringing dawn's light",
				"to hopeless eyes. And",
				"so the Yeti, a herald of",
				"fear, became a beacon",
				"of hope, an unlikely",
				"savior clad in frost.",
				""
			),
			new BookPageInfo
			(
				"Then there's the yarn",
				"of the Silent Footprints,",
				"where a Yeti walked",
				"beside a lost traveler,",
				"never seen but felt,",
				"guiding him through",
				"blinding snow, away",
				"from death's embrace."
			),
			new BookPageInfo
			(
				"Let us not forget the",
				"Tale of Twin Yetis, a",
				"story of brotherhood",
				"and betrayal. One",
				"sought to commune",
				"with men, the other",
				"to keep the sanctity",
				"of the untouched snow."
			),
			new BookPageInfo
			(
				"Their duel, it is said,",
				"carved valleys and",
				"peaks, shaping the",
				"land as their emotions",
				"clashed. Even nature's",
				"beasts have kinship",
				"tales to rival our own,",
				"filled with love and"
			),
			new BookPageInfo
			(
				"wrought with strife.",
				"In the chill of the",
				"deep winter, the",
				"Yetis' roars are said",
				"to be the very gales",
				"that pierce through",
				"the stillness of the",
				"icy nights."
			),
			new BookPageInfo
			(
				"The Yarn of the",
				"Frostbound Heart",
				"speaks of a Yeti who",
				"was once a man,",
				"cursed by a sorceress",
				"for his cold demeanor,",
				"doomed to roam the",
				"mountains, learning"
			),
			new BookPageInfo
			(
				"the warmth of the",
				"soul in the cradle",
				"of cold. A fable,",
				"perhaps, teaching",
				"that our actions",
				"may lead us to",
				"become beasts, if",
				"our hearts grow cold."
			),
			new BookPageInfo
			(
				"In the collection of",
				"yarns, there also lies",
				"the humorous account",
				"of the Yeti's Banquet,",
				"where a starving Yeti",
				"stumbled into a feast",
				"and, with unexpected",
				"delicacy, dined with"
			),
			new BookPageInfo
			(
				"a stunned party of",
				"adventurers. They say",
				"it favored the roast",
				"and the ale, and left",
				"with a satisfied belch",
				"that echoed through",
				"the valleys, a tale to",
				"chuckle upon in taverns."
			),
			new BookPageInfo
			(
				"Among these tales",
				"are lessons woven",
				"within the mythical.",
				"The Yeti, a creature",
				"both feared and",
				"revered, serves as a",
				"mirror to our nature,",
				"to the duality that"
			),
			new BookPageInfo
			(
				"resides in us all.",
				"We fear the unknown,",
				"yet are drawn to it;",
				"we are part of nature,",
				"yet apart from it. The",
				"Yeti's yarns bridge",
				"that divide, between",
				"man and myth."
			),
			new BookPageInfo
			(
				"So I leave you,",
				"dear reader, with",
				"these yarns, woven",
				"from the threads of",
				"legends and truths,",
				"from a land where",
				"the snow whispers",
				"and the mountains"
			),
			new BookPageInfo
			(
				"hold secrets. May",
				"the Yeti's yarns guide",
				"your imagination to",
				"the wild places, where",
				"footprints mark the",
				"beginning of stories",
				"and the end of certainties.",
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
				"Throgmorton the Thinker",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the Yeti's path",
				"inspire your journeys",
				"and its yarns, your",
				"dreams."
			)
			// ... subsequent content

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public YarnsOfTheYeti() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Yarns of the Yeti");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Yarns of the Yeti");
        }

        public YarnsOfTheYeti(Serial serial) : base(serial)
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
