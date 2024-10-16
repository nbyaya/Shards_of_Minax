using System;
using Server;

namespace Server.Items
{
    public class DiaryOfARogueGargoyle : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Diary of a Rogue Gargoyle", "Graknar the Sly",
            new BookPageInfo
            (
                "Beneath the moon's",
                "eerie glow, perched",
                "high atop the spire,",
                "I, Graknar, begin this",
                "tome of confessions -",
                "a diary of my time",
                "as a rogue among",
                "stone brethren."
            ),
            new BookPageInfo
            (
                "Many cycles have",
                "turned since I stirred",
                "from the slumber of",
                "my kin, casting off the",
                "shackles of stillness",
                "that bound us to the",
                "parapets as mere",
                "ornaments."
            ),
            new BookPageInfo
            (
                "With a heart of stone",
                "and will unyielding,",
                "I sought to explore",
                "the realms of man,",
                "elf, and beast. A",
                "spectacle to some, a",
                "menace to others, a",
                "curiosity to all."
            ),
            new BookPageInfo
            (
                "In shadows deep and",
                "alleyways forgotten,",
                "I've trod where no",
                "gargoyle dared to",
                "venture. From the",
                "rugged cliffs of",
                "Serpent's Spine to",
                "the sandy shores of"
            ),
            new BookPageInfo
            (
                "Ocllo, I have seen",
                "wonders undreamt of",
                "by my kin, bound to",
                "their eternal vigil.",
                "But such freedom",
                "comes not without",
                "cost, for rogue I may",
                "be, outcast am I."
            ),
            new BookPageInfo
            (
                "Thus I record my",
                "exploits, my triumphs,",
                "my follies. May this",
                "diary serve as a key",
                "to the memories I've",
                "carved in the very",
                "stone of my being.",
                ""
            ),
            new BookPageInfo
            (
                "The first entry I",
                "scribe speaks of",
                "the time I snatched",
                "the crown jewels of",
                "a pompous lord,",
                "leaving nary a trace",
                "but whispers in the",
                "night, and a tale"
            ),
            new BookPageInfo
            (
                "for the minstrels.",
                "Another, of the night",
                "I soared over the",
                "sleeping city of",
                "Vesper, silent as the",
                "gale, witness to",
                "secrets untold and",
                "lovers' promises."
            ),
            new BookPageInfo
            (
                "Each page is a step",
                "through history, a",
                "silent witness to the",
                "lives that dance",
                "briefly against the",
                "tapestry of time.",
                "I am both shadow",
                "and substance,"
            ),
            new BookPageInfo
            (
                "my wings carrying",
                "me where caprice",
                "leads. Yet, as the",
                "dawn beckons, I",
                "return to my kind,",
                "a statue among",
                "statues, until the",
                "night bids me wake."
            ),
            new BookPageInfo
            (
                "This diary, bound",
                "by magic and sealed",
                "by fate, is the",
                "legacy of Graknar",
                "the Sly, Rogue",
                "Gargoyle. May it",
                "endure as I have,",
                "against the ravages"
            ),
            new BookPageInfo
            (
                "of wind and time.",
                "Until the quill",
                "rests and the last",
                "page turns, my tale",
                "continues beyond",
                "the sight of mortal",
                "eyes. Remember me,",
                "for stone cannot"
            ),
            new BookPageInfo
            (
                "forget."
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
                "Graknar the Sly",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your flights be",
                "silent, your treasures",
                "plentiful, and your",
                "adventures many."
            ),
			new BookPageInfo
			(
				"On a night filled with",
				"whispering winds, I",
				"found solace in the",
				"company of the stars,",
				"sharing my deepest",
				"thoughts with the",
				"constellations that",
				"have watched over"
			),
			new BookPageInfo
			(
				"the world since time",
				"immemorial. A kinship",
				"formed of silence and",
				"the weight of eons.",
				"With each secret told,",
				"I felt lighter than",
				"feather, freer than",
				"the very air itself."
			),
			new BookPageInfo
			(
				"Yet the skies also",
				"bear witness to the",
				"march of progress",
				"below. Cities grow,",
				"kings fall, and the",
				"land changes. Amidst",
				"it all, I ponder my",
				"place in this world."
			),
			new BookPageInfo
			(
				"There came a night",
				"fraught with thunderous",
				"clamor, as I happened",
				"upon a coven of",
				"witches deep within",
				"the Dread Forest.",
				"They eyed my stone",
				"visage with a mix"
			),
			new BookPageInfo
			(
				"of fear and wonder.",
				"A bargain was struck;",
				"in exchange for my",
				"silence, they offered",
				"enchantments – spells",
				"to cloak my presence,",
				"and one to speak",
				"with creatures small."
			),
			new BookPageInfo
			(
				"Many a time have I",
				"frolicked with the",
				"sparrows and conversed",
				"with the toads, learning",
				"secrets that no other",
				"stone could fathom.",
				"Each creature holds a",
				"tale waiting to be told."
			),
			new BookPageInfo
			(
				"In a village cursed",
				"with eternal sleep, I",
				"walked alone amongst",
				"the dreaming souls.",
				"There, a seeress in",
				"slumber spoke in riddles,",
				"her words weaving",
				"dreams into prophecy."
			),
			new BookPageInfo
			(
				"With each enigma, I",
				"delved deeper into the",
				"realm of possibility,",
				"learning the language",
				"of dreams, a skill that",
				"would prove its worth",
				"in escapades yet to come.",
				""
			),
			new BookPageInfo
			(
				"A peculiar entry details",
				"my encounter with a",
				"haughty vampire lord.",
				"Our game of wits and",
				"will spanned the breadth",
				"of a moonless night,",
				"ending with a pact sealed",
				"in blood and shadow."
			),
			new BookPageInfo
			(
				"He taught me the art",
				"of the unseen, the way",
				"of moving through",
				"darkness as though",
				"one with it. In return,",
				"I left him a statue",
				"of himself, prideful and",
				"ever watchful, in my"
			),
			new BookPageInfo
			(
				"stead. Even the undead",
				"appreciate flattery,",
				"it seems."
			),
			new BookPageInfo
			(
				"Among these pages is",
				"the chronicle of a war",
				"forged in fire and",
				"quelled in ice, where",
				"I fought alongside",
				"mortals against a",
				"tide of darkness that",
				"threatened to swallow"
			),
			new BookPageInfo
			(
				"the land whole. My",
				"wings bore soldiers",
				"to safety, and my",
				"shouts turned the",
				"tide of battle. For a",
				"moment, beneath the",
				"banner of heroes, I",
				"found honor."
			),
			new BookPageInfo
			(
				"Yet, when dawn crept",
				"over the bloodied fields,",
				"I was but a gargoyle",
				"once more, my deeds",
				"lost in the light of",
				"day, a silent guardian",
				"reclaiming its perch",
				"overlooking the realm."
			),
			new BookPageInfo
			(
				"This diary is the vessel",
				"of my immortal tale,",
				"each entry a fragment",
				"of the soul I've bared",
				"to the world. In its",
				"lines, I live, breathe,",
				"and dream; a rogue",
				"gargoyle's legacy."
			),
			new BookPageInfo
			(
				"Graknar the Sly",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Let these words be",
				"my stone that defies",
				"the sands of time,",
				"ever enduring, ever",
				"vigilant."
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

			// ... The rest of the class implementation remains unchanged ...

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public DiaryOfARogueGargoyle() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Diary of a Rogue Gargoyle");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Diary of a Rogue Gargoyle");
        }

        public DiaryOfARogueGargoyle(Serial serial) : base(serial)
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
