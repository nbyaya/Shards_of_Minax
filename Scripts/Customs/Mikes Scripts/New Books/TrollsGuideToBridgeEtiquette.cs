using System;
using Server;

namespace Server.Items
{
    public class TrollsGuideToBridgeEtiquette : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Troll’s Guide to Bridge Etiquette", "Grundle the Bridgekeeper",
            new BookPageInfo
            (
                "To you, weary traveler,",
                "I bestow this tome of",
                "knowledge, a guide to",
                "the ancient art of",
                "bridge etiquette, as",
                "penned by I, Grundle,",
                "keeper of the Stonebridge."
            ),
            new BookPageInfo
            (
                "First and foremost,",
                "when approaching a",
                "troll's bridge, make",
                "haste not! A steady",
                "tread thwarts the",
                "splintering of old",
                "wood and shows respect",
                "to the bridgekeeper."
            ),
            new BookPageInfo
            (
                "Offering is a must.",
                "No beast, no man,",
                "nor sprite shall pass",
                "without tribute. Be it",
                "gold, a riddle, or a",
                "tasty morsel, the",
                "keeper's toll stands",
                "as tradition dictates."
            ),
            new BookPageInfo
            (
                "Haggle not with the",
                "troll for his price is",
                "as firm as the earth",
                "beneath the bridge.",
                "Remember, his patience",
                "is as short as his",
                "stature is tall."
            ),
            new BookPageInfo
            (
                "Mind the rules scribed",
                "upon the entrance,",
                "for every bridge's law",
                "differs. Some may",
                "forbid the crossing",
                "during moonlight,",
                "others, the toll of a",
                "song or tale."
            ),
            new BookPageInfo
            (
                "Should you find",
                "yourself unprepared,",
                "fret not. Offer your",
                "ear to the troll's",
                "woes, for a listening",
                "guest is a welcome",
                "respite from his",
                "lonely vigil."
            ),
            new BookPageInfo
            (
                "Keep your word as",
                "sturdy as the stones",
                "that hold his abode.",
                "A promise to a troll",
                "is sacred, bound by",
                "the ancient magics",
                "of land and river."
            ),
            new BookPageInfo
            (
                "Lastly, upon your",
                "leave, bid the keeper",
                "a good day. Courtesy",
                "is the bridge to",
                "friendship, and a",
                "kind word may earn",
                "you safe passage",
                "on return."
            ),
            new BookPageInfo
            (
                "Heed these words,",
                "traverse with care,",
                "and respect the",
                "solemn duty of the",
                "bridgekeeper. Thus",
                "concludes your",
                "guide to proper",
                "bridge etiquette."
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
                "Grundle the Bridgekeeper",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your crossings",
                "be ever in favor with",
                "the guardians of",
                "stone and stream."
            ),
			new BookPageInfo
			(
				"In the rare event",
				"you spy a troll in",
				"mid-slumber, tread",
				"lightly and let not",
				"the gravel cry beneath",
				"your boot. Wake not",
				"what rests peacefully,",
				"lest you wish to parley"
			),
			new BookPageInfo
			(
				"with the irate beast.",
				"And should the bridge",
				"be damaged, inform the",
				"troll with due haste;",
				"a troll takes great pride",
				"in his span, and your",
				"concern will be noted,",
				"appreciated... usually."
			),
			new BookPageInfo
			(
				"Beware, for some trolls",
				"delight in riddles and",
				"conundrums. Arm yourself",
				"with wit and guile, for",
				"a clever tongue can be",
				"worth more than a",
				"purse of gold. Engage",
				"with a sharp mind,"
			),
			new BookPageInfo
			(
				"and listen well, for",
				"trolls craft their words",
				"with cunning, hiding",
				"truths within their",
				"twisted tales and",
				"trickery. Outsmarting",
				"a troll can earn you",
				"his begrudging respect."
			),
			new BookPageInfo
			(
				"If, by ill fate, you find",
				"yourself in a tangle with",
				"the bridge guardian,",
				"remember this: trolls",
				"may be mighty, but they",
				"favor strength of character",
				"as much as that of arm."
			),
			new BookPageInfo
			(
				"A gift of song may",
				"soothe the savage beast,",
				"or an offer to mend his",
				"garments or cook a meal.",
				"Such simple acts of",
				"kindness are often",
				"overlooked by those",
				"seeking passage."
			),
			new BookPageInfo
			(
				"Trolls are keepers of",
				"lore, as much as of",
				"bridges. Their tales",
				"hold history, and their",
				"songs, the whispers",
				"of the woods and",
				"waters. Listen and",
				"learn, for their sagas"
			),
			new BookPageInfo
			(
				"span ages, and through",
				"their gruff demeanor,",
				"wisdom can be gleaned.",
				"Offer a patient ear,",
				"and tales of yore shall",
				"unfold, richer than any",
				"coin you might proffer."
			),
			new BookPageInfo
			(
				"In the rarest of events,",
				"should a troll ever offer",
				"you a gift, accept with",
				"humility. For a troll to",
				"share his treasures is",
				"a sign of the highest",
				"honor and respect to",
				"one’s character."
			),
			new BookPageInfo
			(
				"Remember, bridges are",
				"more than crossings; they",
				"are the troll's home, his",
				"hearth. When you pay",
				"his toll, you are not",
				"merely purchasing passage,",
				"but contributing to the",
				"upkeep of his abode."
			),
			new BookPageInfo
			(
				"As you conclude your",
				"crossing, it is customary",
				"to nod in reverence to",
				"the bridge and its keeper.",
				"Such gestures go far,",
				"and a respectful visitor",
				"will always be welcomed",
				"back with open arms."
			),
			new BookPageInfo
			(
				"Thus, armed with this",
				"guide, may your travels",
				"be safe, and your",
				"encounters with our",
				"kind be pleasant. Let",
				"the bridges you cross",
				"lead to new friendships",
				"and grand adventures."
			),
			new BookPageInfo
			(
				"Grundle the Bridgekeeper",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Safe travels, and may",
				"your path be ever",
				"clear of brambles and",
				"your bridges troll-friendly."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TrollsGuideToBridgeEtiquette() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Troll’s Guide to Bridge Etiquette");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Troll’s Guide to Bridge Etiquette");
        }

        public TrollsGuideToBridgeEtiquette(Serial serial) : base(serial)
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
