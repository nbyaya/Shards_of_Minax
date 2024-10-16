using System;
using Server;

namespace Server.Items
{
    public class UnheardTalesOfSosaria : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Unheard Tales of Sosaria", "Thalain the Chronicler",
            new BookPageInfo
            (
                "In the realm of",
                "Sosaria, where",
                "adventurers boast of",
                "their conquests and",
                "bards sing of",
                "ancient legends, lie",
                "tales untold,",
                "whispers of a land"
            ),
            new BookPageInfo
            (
                "mystical and wild.",
                "I am Thalain,",
                "scribe and wanderer,",
                "chronicler of the",
                "arcane and the",
                "obscure. Within these",
                "pages lie accounts",
                "of Sosaria's hidden"
            ),
            new BookPageInfo
            (
                "truths, unheard",
                "legends among",
                "the lost and the",
                "forgotten.",
                "",
                "Behold the tale of",
                "the Crystal Grotto,",
                "a cavern where"
            ),
            new BookPageInfo
            (
                "diamonds sing and",
                "echo with the",
                "sorrows of a",
                "captured elemental",
                "princess. Or the",
                "story of the ghost",
                "ship 'The Mourner',",
                "that sails against"
            ),
            new BookPageInfo
            (
                "the wind on moonless",
                "nights, ever searching",
                "for the passage home.",
                "These are the chronicles",
                "of the unlikely heroes,",
                "the secret victories,",
                "the enchantments",
                "concealed in the"
            ),
            new BookPageInfo
            (
                "fabric of the land.",
                "",
                "Consider the",
                "mysterious 'Midnight",
                "Masons', builders of",
                "the impossible,",
                "whose constructs",
                "rise where none"
            ),
            new BookPageInfo
            (
                "dared build. Their",
                "creations, born in",
                "darkness, vanish with",
                "the dawn.",
                "",
                "Reflect upon the",
                "Wellspring of Shadows,",
                "a font of pure magic"
            ),
            new BookPageInfo
            (
                "that twists the fate",
                "of any who dare drink",
                "its shimmering waters.",
                "",
                "Among these leaves,",
                "witness accounts of",
                "the spectral gardens,",
                "where flowers bloom"
            ),
            new BookPageInfo
            (
                "with the essence of",
                "souls, and trees",
                "whisper secrets of the",
                "dead to those who",
                "would listen.",
                "",
                "Marvel at the tale",
                "of Eon, the Timeless"
            ),
            new BookPageInfo
            (
                "Mage, who walks",
                "between seconds and",
                "whose battle with the",
                "Chaos Serpent is",
                "said to last for",
                "eternity, never",
                "ceasing, never",
                "beginning."
            ),
            new BookPageInfo
            (
                "These chronicles are",
                "but a glimpse of the",
                "tapestry that is",
                "Sosaria; a land where",
                "every stone and",
                "stream has a saga,",
                "whispering to those",
                "who would hear."
            ),
            new BookPageInfo
            (
                "As you journey through",
                "the realms, dear",
                "reader, may your eyes",
                "be open to the",
                "wonder, may your",
                "heart be brave to",
                "delve into the",
                "forgotten lore."
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
                "Thalain the Chronicler",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Seek the stories",
                "hidden in shadow",
                "and light. May they",
                "guide your adventures",
                "beyond the edge of",
                "maps, where the",
                "Unheard Tales of",
                "Sosaria await."
            ),
			// Continued from the previous book content
			new BookPageInfo
			(
				"In the shadowed groves",
				"of the Forest of Echoes,",
				"it is said spirits",
				"of nature dance under",
				"the twilight. Their",
				"songs bring life to",
				"trees and streams, and",
				"their grief brings"
			),
			new BookPageInfo
			(
				"forth autumn's touch.",
				"Legend speaks of the",
				"Weeping Willow, a tree",
				"so ancient that its",
				"roots touch the very",
				"heart of Sosaria.",
				"It is here one may",
				"hear the world's"
			),
			new BookPageInfo
			(
				"heartbeat, if they",
				"dare to listen.",
				"",
				"Journey on to the",
				"desolate Wastes of",
				"Despair, where the",
				"ground is barren,",
				"and the sky weeps"
			),
			new BookPageInfo
			(
				"acidic tears. Seek",
				"the Cavern of",
				"Solitude, where",
				"no sound echoes",
				"and loneliness",
				"can be seen in",
				"the form of a",
				"crystal tear."
			),
			new BookPageInfo
			(
				"The brave can find",
				"the Isle of Whispers,",
				"where the very air",
				"seems alive with",
				"secrets. It is rumored",
				"that one whisper",
				"from this isle can",
				"calm the most"
			),
			new BookPageInfo
			(
				"turbulent storm or",
				"ignite a lover's",
				"passion. But be",
				"wary, for some",
				"secrets are not",
				"meant for mortal",
				"ears.",
				""
			),
			new BookPageInfo
			(
				"Amidst the frigid",
				"cliffs of the Northlands,",
				"the Frozen Specter",
				"waits, trapped in",
				"ice, gazing upon",
				"a landscape where",
				"summer is but a",
				"distant dream."
			),
			new BookPageInfo
			(
				"Adventurers speak",
				"in hushed tones",
				"of the Labyrinth of",
				"Regrets, a maze where",
				"one's darkest memories",
				"become a reality,",
				"testing the mind",
				"as much as the blade."
			),
			new BookPageInfo
			(
				"Cross the seas and",
				"find the Drowned City,",
				"where the greedy lie",
				"in watery graves,",
				"their hands still",
				"clutching gold as",
				"fish make homes in",
				"their skulls."
			),
			new BookPageInfo
			(
				"Hearken to the",
				"tale of the Gypsy",
				"Seer of Minoc, whose",
				"cards foretell futures",
				"with uncanny accuracy,",
				"and whose crystal",
				"ball shows not what",
				"is, but what might be."
			),
			new BookPageInfo
			(
				"These stories, these",
				"fragments of a world",
				"so vast, are but",
				"whispers of the greater",
				"epic that is Sosaria.",
				"Within these words",
				"lies the spirit of",
				"discovery, a call to"
			),
			new BookPageInfo
			(
				"those who would",
				"seek out the corners",
				"of the world yet",
				"unmapped, to the",
				"dreamers who believe",
				"in the legends not",
				"yet sung, and to the",
				"curious who look"
			),
			new BookPageInfo
			(
				"beyond the horizon,",
				"where adventure",
				"awaits.",
				"",
				"May these Unheard",
				"Tales spark a light",
				"of wonder in your",
				"heart, as they have"
			),
			new BookPageInfo
			(
				"in mine. For in every",
				"unturned stone, in",
				"every whispered wind,",
				"the legacy of Sosaria",
				"continues to grow,",
				"ever rich, ever",
				"wondrous.",
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
				"Thalain the Chronicler",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"In your travels, may",
				"you find the truth",
				"woven in these tales,",
				"and may your path",
				"be ever guided by",
				"the ancient stories",
				"of Sosaria."
			)


        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public UnheardTalesOfSosaria() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Unheard Tales of Sosaria");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Unheard Tales of Sosaria");
        }

        public UnheardTalesOfSosaria(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
