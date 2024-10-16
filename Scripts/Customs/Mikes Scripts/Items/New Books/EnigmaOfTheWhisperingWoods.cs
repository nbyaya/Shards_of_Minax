using System;
using Server;

namespace Server.Items
{
    public class EnigmaOfTheWhisperingWoods : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Enigma of the Whispering Woods", "Eldorin the Explorer",
            new BookPageInfo
            (
                "Within the tangled",
                "thickets of the Whispering",
                "Woods, secrets abound,",
                "and mystery whispers with",
                "every rustle of the leaves.",
                "I am Eldorin, an explorer",
                "of the arcane and unknown.",
                "This book is a chronicle of"
            ),
            new BookPageInfo
            (
                "my most peculiar journey",
                "through the enigmatic",
                "forest that speaks in",
                "shadows and riddles.",
                "It started with rumors",
                "of a tree that hummed",
                "with an otherworldly",
                "energy, its bark etched"
            ),
            new BookPageInfo
            (
                "with runes that glowed",
                "under the light of the",
                "two moons. I ventured",
                "forth, guided by the",
                "silent songs of the stars,",
                "and the forest's cryptic",
                "whispers. Each step"
            ),
            new BookPageInfo
            (
                "deeper into the woods",
                "revealed wonders and",
                "horrors alike. I encountered",
                "spirits entwined within",
                "vines, speaking of ages",
                "past and futures untold,",
                "while unseen entities"
            ),
            new BookPageInfo
            (
                "watched from the dark,",
                "their eyes glinting with",
                "curiosity and malice.",
                "",
                "Amidst these encounters,",
                "I learned of the Heart of",
                "the Woods—a gem said to"
            ),
            new BookPageInfo
            (
                "hold the essence of the",
                "forest itself. Many have",
                "sought this relic, only to",
                "be ensnared by the woods'",
                "bewitching allure, never",
                "to return. Undeterred,"
            ),
            new BookPageInfo
            (
                "I pressed on, deciphering",
                "the whispers carried by",
                "the wind. Each secret",
                "unveiled led me closer",
                "to the heart, and deeper",
                "into the labyrinth of",
                "ancient boughs."
            ),
            new BookPageInfo
            (
                "This tome bears my",
                "findings and musings,",
                "a testament to my",
                "journey's validity. It is",
                "a guide and warning for",
                "those brave or foolish",
                "enough to follow in my",
                "footsteps."
            ),
            new BookPageInfo
            (
                "Let the Enigma of the",
                "Whispering Woods",
                "bewitch you not, for",
                "within its heart lies",
                "a truth not all are",
                "prepared to behold."
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
			// Additional pages of "Enigma of the Whispering Woods"
			new BookPageInfo
			(
				"Nestled in shadows deep,",
				"the woods whispered of",
				"a path, hidden to those",
				"who walk without seeing.",
				"The Whispering Woods",
				"never truly silent, told",
				"of secrets etched in",
				"stone and root."
			),
			new BookPageInfo
			(
				"I found ancient markers,",
				"stone obelisks that hummed",
				"with power, leading the",
				"way. Runes upon their faces",
				"shone with ethereal light,",
				"beckoning further into the",
				"verdant maze."
			),
			new BookPageInfo
			(
				"At each step, the air grew",
				"thicker, the whispers more",
				"insistent. Spirits of the wood,",
				"long forgotten, flickered",
				"between the trees, their",
				"forms brief flashes of",
				"moonlight and memory."
			),
			new BookPageInfo
			(
				"The deeper realms of the",
				"woods were stranger still.",
				"Here, the trees spoke in",
				"tongues lost to man, and",
				"the very ground seemed",
				"to breathe with a life of",
				"its own."
			),
			new BookPageInfo
			(
				"In a clearing, veiled by",
				"mists of dream, stood",
				"the Heart Tree, its vast",
				"trunk split by a shimmering",
				"cleft. Within this rift pulsed",
				"the Heart of the Woods,",
				"its glow warm and inviting."
			),
			new BookPageInfo
			(
				"I reached out, the whispers",
				"at a fever pitch. The moment",
				"my fingers brushed the gem's",
				"surface, the forest fell silent.",
				"Visions flooded my mind:",
				"ages of growth and decay,"
			),
			new BookPageInfo
			(
				"cycles of life in endless",
				"succession. I saw the rise",
				"and fall of civilizations",
				"unknown, and the slow",
				"heartbeat of the very",
				"world."
			),
			new BookPageInfo
			(
				"When I awoke, the gem was",
				"gone, and the Heart Tree",
				"sealed once more. But the",
				"woods had changed—or I",
				"had. Each creature, each",
				"leaf, now sang with clarity."
			),
			new BookPageInfo
			(
				"I understood the whispers,",
				"the language of rustling",
				"leaves and chattering",
				"brooks. The Enigma of the",
				"Whispering Woods unveiled",
				"at last: that all life is",
				"interconnected, a tapestry"
			),
			new BookPageInfo
			(
				"woven with threads of",
				"infinite variety, yet of",
				"one cloth. The woods",
				"had imparted their",
				"ancient wisdom, a gift",
				"beyond price."
			),
			new BookPageInfo
			(
				"The journey back was",
				"a reflection of the new",
				"world I now beheld,",
				"alive with wonder. Each",
				"step was a verse in a",
				"poem, each breath a",
				"note in a symphony of"
			),
			new BookPageInfo
			(
				"existence. And so, I",
				"returned, bearing the",
				"gift of understanding,",
				"the true Heart of the",
				"Woods beating within",
				"me."
			),
			new BookPageInfo
			(
				"This book is my attempt",
				"to impart that wisdom.",
				"Should you ever wander",
				"into whispering groves,",
				"heed the voice of nature,",
				"and may you find the heart",
				"that beats beneath."
			),
			new BookPageInfo
			(
				"Eldorin the Explorer",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"The woods are alive,",
				"and they sing a song",
				"for those with the heart",
				"to hear."
			),
			// The additional pages end here.
			// The rest of the original script remains unchanged.
            new BookPageInfo
            (
                "Eldorin the Explorer",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the whispers guide",
                "you to truth."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public EnigmaOfTheWhisperingWoods() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Enigma of the Whispering Woods");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Enigma of the Whispering Woods");
        }

        public EnigmaOfTheWhisperingWoods(Serial serial) : base(serial)
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
