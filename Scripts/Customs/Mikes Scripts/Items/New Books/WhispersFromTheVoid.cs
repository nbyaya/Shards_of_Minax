using System;
using Server;

namespace Server.Items
{
    public class WhispersFromTheVoid : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Whispers from the Void", "Vilindra the Shadow Weaver",
            new BookPageInfo
            (
                "Within the silent",
                "embraces of the",
                "eternal darkness,",
                "whispers from the",
                "void gently weave",
                "into the fabric of",
                "reality. This tome",
                "is but a vessel for"
            ),
            new BookPageInfo
            (
                "secrets meant not",
                "for the light of day.",
                "Vilindra, I am called,",
                "weaver of shadows,",
                "listener of the",
                "sibilant voices that",
                "emerge from the",
                "chasm of existence."
            ),
            new BookPageInfo
            (
                "What lies within",
                "these pages is a",
                "chronicle of",
                "forbidden knowledge,",
                "eldritch truths that",
                "have driven many",
                "to the brink of",
                "madness."
            ),
            new BookPageInfo
            (
                "The void speaks",
                "in riddles, in",
                "cold breaths",
                "felt but unheard.",
                "It tells of other",
                "realms, of places",
                "unseen by eyes",
                "untouched by light."
            ),
            new BookPageInfo
            (
                "With every whisper,",
                "I pen down its",
                "narratives, scribing",
                "into existence the",
                "lore of the abyss.",
                "The void's children,",
                "entities of the",
                "nether, yearn for"
            ),
            new BookPageInfo
            (
                "acknowledgment,",
                "craving for their",
                "stories to be told,",
                "longing for their",
                "presence to be",
                "felt in the realm",
                "of mortals."
            ),
            new BookPageInfo
            (
                "One must be wary,",
                "for the void's",
                "whispers can",
                "seduce the soul,",
                "weave darkness",
                "into the heart,",
                "and taint the",
                "mind with an"
            ),
            new BookPageInfo
            (
                "insatiable curiosity",
                "for the unknown.",
                "Many a time, I",
                "have danced on the",
                "knife-edge of",
                "sanity, entranced",
                "by its cryptic",
                "chorus."
            ),
            new BookPageInfo
            (
                "Each whisper is a",
                "thread, each thread",
                "a tale. And so,",
                "I invite thee, brave",
                "or perhaps foolhardy",
                "reader, to delve",
                "into the secrets",
                "entombed herein."
            ),
            new BookPageInfo
            (
                "Be forewarned:",
                "to read is to listen,",
                "and to listen is to",
                "invite the void's",
                "embrace. May the",
                "whispers guide you",
                "or damn you to",
                "the abyss."
            ),
            new BookPageInfo
            (
                "Vilindra the Shadow",
                "Weaver",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the void's",
                "whispers be ever",
                "in your favor."
            ),
            // Continuing from the last BookPageInfo array index in the Content array
			new BookPageInfo
			(
				"This realm beyond,",
				"a vast expanse of",
				"nothingness, yet",
				"teeming with",
				"entities, bound by",
				"neither flesh nor",
				"spirit. Their forms",
				"are ink to my quill."
			),
			new BookPageInfo
			(
				"Herein lies the",
				"tale of the Obsidian",
				"Specter, a wraith",
				"shrouded in darkness,",
				"whose silent weep",
				"foretells the fall",
				"of kingdoms and",
				"the rise of chaos."
			),
			new BookPageInfo
			(
				"And of the Nether",
				"Sirens, whose",
				"melodies bend the",
				"wills of the weak,",
				"spinning their fates",
				"into threads of",
				"baleful magic to",
				"adorn the void's veil."
			),
			new BookPageInfo
			(
				"Whispers recount",
				"the Evergloom Bloom,",
				"a flower unseen,",
				"whose petals eclipse",
				"light and whose",
				"seeds sprout in",
				"hearts, birthing",
				"anguish and despair."
			),
			new BookPageInfo
			(
				"A void-touched",
				"seer once spoke",
				"unto me, eyes",
				"milky white,",
				"voice a silken",
				"threat. He told",
				"of the Lurking",
				"Leviathan,"
			),
			new BookPageInfo
			(
				"a beast of many",
				"maws, slumbering in",
				"the deep void sea.",
				"Its dreams dictate",
				"the ebb and flow",
				"of reality, each",
				"breath a life, each",
				"whim a cataclysm."
			),
			new BookPageInfo
			(
				"So I bind these",
				"visions into pages,",
				"an anchor for",
				"sanity in the",
				"maelstrom of the",
				"void's call. Each",
				"word a ward, each",
				"line a leyline."
			),
			new BookPageInfo
			(
				"Yet, the void is",
				"endless, its",
				"whispers incessant.",
				"More tales lurk",
				"within its silent",
				"roar, awaiting",
				"scribe and reader",
				"alike."
			),
			new BookPageInfo
			(
				"Dare you turn",
				"another page?",
				"Will you delve",
				"deeper into this",
				"indomitable",
				"darkness? For",
				"those who dare,",
				"the void's embrace"
			),
			new BookPageInfo
			(
				"awaits. And so",
				"does knowledge",
				"of the ethereal",
				"and the arcane,",
				"of horrors and",
				"wonders alike,",
				"bound within",
				"this stygian codex."
			),
			new BookPageInfo
			(
				"Thus, the journey",
				"continues, as does",
				"the chronicling of",
				"the void's enigmatic",
				"testaments. Whispers",
				"from the darkness",
				"beckon, promising",
				"power and peril."
			),
			new BookPageInfo
			(
				"For now, I rest",
				"my quill. But",
				"be warned, seeker",
				"of shadows: the",
				"void stares back",
				"into thee. And",
				"its whispers",
				"never cease."
			),
			new BookPageInfo
			(
				"Vilindra the Shadow",
				"Weaver",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May you find",
				"truth in the",
				"void's embrace."
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
        public WhispersFromTheVoid() : base(false)
        {
            // Set the hue to a mysterious color
            Hue = Utility.RandomMinMax(1109, 1150);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Whispers from the Void");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Whispers from the Void");
        }

        public WhispersFromTheVoid(Serial serial) : base(serial)
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
