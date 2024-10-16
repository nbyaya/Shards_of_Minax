using System;
using Server;

namespace Server.Items
{
    public class CompendiumOfMythicalBeasts : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Compendium of Mythical Beasts", "Merlinus the Scholar",
            new BookPageInfo
            (
                "In this tome, I shall",
                "endeavor to chronicle",
                "the myriad of mythical",
                "creatures that roam",
                "land, sky, and sea.",
                "From the fiery phoenix",
                "to the enigmatic",
                "unicorns, each beast"
            ),
            new BookPageInfo
            (
                "carries with it a story,",
                "a tapestry of myth and",
                "legend. As a master",
                "scholar and beast",
                "hunter, my journeys",
                "have taken me across",
                "the known world to",
                "gather these tales."
            ),
            new BookPageInfo
            (
                "Let us first speak of",
                "dragons, the serpentine",
                "lords of the sky. Their",
                "breath is said to",
                "forge empires and",
                "lay waste to armies.",
                "Varying in shape, size,",
                "and power, these"
            ),
            new BookPageInfo
            (
                "magnificent creatures",
                "command respect and",
                "fear in equal measure.",
                "",
                "Next are the griffins,",
                "with the bodies of",
                "lions and the wings",
                "and heads of eagles."
            ),
            new BookPageInfo
            (
                "Symbols of valor and",
                "strength, griffins are",
                "revered as noble",
                "companions and fierce",
                "opponents. Their nests",
                "are said to contain",
                "priceless treasures,",
                "guarded fiercely."
            ),
            new BookPageInfo
            (
                "Beneath the waves,",
                "the elusive kraken",
                "stirs in the deep sea.",
                "Its tentacles are said",
                "to wrap around ships,",
                "dragging them to the",
                "ocean's floor. Sailors",
                "whisper prayers"
            ),
            new BookPageInfo
            (
                "to avoid its grasp.",
                "",
                "Then there are the",
                "sprites, pixies, and",
                "fairies that flicker",
                "in the woods. These",
                "capricious beings can",
                "offer magical aid or"
            ),
            new BookPageInfo
            (
                "bewildering hindrances,",
                "often for their own",
                "amusement. They are",
                "proof that not all",
                "mythical creatures",
                "are to be feared, yet",
                "none should be taken",
                "lightly."
            ),
            new BookPageInfo
            (
                "This compendium will",
                "continue to grow as I",
                "journey to unearth",
                "more about these",
                "enigmatic beings. Let",
                "this book serve as a",
                "guide and a reminder",
                "of the wonders"
            ),
            new BookPageInfo
            (
                "and dangers that exist",
                "beyond the realm of",
                "common knowledge.",
                "Take heed of these",
                "legends, for they often",
                "hold truths about our",
                "world and the magic",
                "that binds it."
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
                "Merlinus the Scholar",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your thirst for",
                "knowledge be as",
                "insatiable as the",
                "kraken's hunger for",
                "the sea."
            ),
			// Continuing from the last BookPageInfo
			new BookPageInfo
			(
				"Of all the creatures",
				"that roam the skies,",
				"the phoenix is perhaps",
				"the most astonishing.",
				"Arising from ashes,",
				"this eternal bird of",
				"flame embodies the",
				"cycle of death and"
			),
			new BookPageInfo
			(
				"rebirth. Its tears can",
				"heal any wound, and",
				"its song is a haunting",
				"melody of renewal.",
				"",
				"In the frostbitten",
				"wastes, the mighty",
				"yetis are said to dwell."
			),
			new BookPageInfo
			(
				"These hulking brutes",
				"possess strength",
				"unmatched, and their",
				"roar can cause an",
				"avalanche. Yetis are",
				"reclusive, but tales",
				"suggest a gentle side",
				"to these behemoths."
			),
			new BookPageInfo
			(
				"The minotaur, with its",
				"human body and bull's",
				"head, is a fearsome",
				"sight. Trapped within",
				"labyrinths of their own",
				"sorrow, they guard",
				"secrets long forgotten",
				"by men."
			),
			new BookPageInfo
			(
				"Sirens, with their",
				"bewitching voices,",
				"dwell upon rocky isles",
				"and in coastal caves.",
				"Many a sailor has been",
				"drawn to their doom,",
				"lured by the beauty of",
				"their enchanting song."
			),
			new BookPageInfo
			(
				"In the darkest depths",
				"of ancient forests",
				"resides the treant, a",
				"sentient tree with a",
				"heartwood of wisdom.",
				"Protectors of the",
				"woodlands, they are",
				"slow to anger but"
			),
			new BookPageInfo
			(
				"terrifying when roused.",
				"Their roots run deep,",
				"and their memories",
				"stretch back through",
				"centuries.",
				"",
				"Leviathans are the",
				"monarchs of the deep,"
			),
			new BookPageInfo
			(
				"massive sea creatures",
				"whose size and power",
				"are the stuff of legend.",
				"It is said that when",
				"they surface, the sea",
				"itself bows to their",
				"majesty."
			),
			new BookPageInfo
			(
				"In the shadowed",
				"corridors of the world",
				"lurk the vampires,",
				"beings of night and",
				"blood. Their eternal",
				"thirst drives them to",
				"elegant savagery, a",
				"dance of death with"
			),
			new BookPageInfo
			(
				"the living.",
				"",
				"Centaurs, half-human,",
				"half-horse, are as",
				"wild as they are wise,",
				"masters of bow and",
				"spear, racing with the",
				"wind across plains."
			),
			new BookPageInfo
			(
				"The sphinx, enigmatic",
				"and wise, poses riddles",
				"as old as time itself.",
				"Those who cannot",
				"answer feel the weight",
				"of their ignorance",
				"—and her wrath."
			),
			new BookPageInfo
			(
				"Lastly, we come upon",
				"the chimera, a creature",
				"of many parts—lion,",
				"goat, and serpent. It",
				"breathes fire and",
				"terror in equal",
				"measure, a testament",
				"to nature's unfathomable"
			),
			new BookPageInfo
			(
				"whims.",
				"",
				"Thus concludes this",
				"volume of the",
				"Compendium. Yet, the",
				"world is vast, and",
				"countless more",
				"creatures await"
			),
			new BookPageInfo
			(
				"discovery by those",
				"brave or foolish",
				"enough to seek them.",
				"May your path be true,",
				"and your heart steady.",
				"",
				"Merlinus the Scholar"
			)
			// Add as many BookPageInfo objects as needed to complete your book.
			// Ensure that the structure of the book remains consistent.
			// Additional pages may contain images, further stories, or even blank pages for the owner to fill.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CompendiumOfMythicalBeasts() : base(false)
        {
            // Set the hue to a specific color that represents the arcane
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Compendium of Mythical Beasts");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Compendium of Mythical Beasts");
        }

        public CompendiumOfMythicalBeasts(Serial serial) : base(serial)
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
