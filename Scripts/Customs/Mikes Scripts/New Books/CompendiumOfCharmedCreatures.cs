using System;
using Server;

namespace Server.Items
{
    public class CompendiumOfCharmedCreatures : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Compendium of Charmed Creatures", "Theodorus Fickle",
            new BookPageInfo
            (
                "This tome is dedicated",
                "to the arcanely augmented",
                "animals and creatures",
                "enchanted by various",
                "means, either through",
                "the whimsy of mages",
                "or the natural flows",
                "of magic in our realm."
            ),
            new BookPageInfo
            (
                "The creatures within",
                "these pages range from",
                "the benign fluttering",
                "Bumble-Sprites, with",
                "their luminescent wings,",
                "to the more formidable",
                "Gilded Basilisks, whose",
                "golden gaze petrifies."
            ),
            new BookPageInfo
            (
                "Each entry will describe",
                "not only the appearance",
                "and habitat of the",
                "charmed creatures but",
                "also their known",
                "behaviours and any",
                "interaction notes for",
                "the wandering traveller."
            ),
            new BookPageInfo
            (
                "Moreover, as a guide",
                "for the intrepid mage,",
                "this compendium will",
                "detail various magical",
                "properties and potential",
                "uses of creature byproducts",
                "such as scales, feathers,",
                "and essences."
            ),
            new BookPageInfo
            (
                "Let us begin with the",
                "Whispering Willows:",
                "trees that have gained",
                "sentience. Their leaves",
                "rustle with the secrets",
                "of the forest, and they",
                "are known to guide",
                "lost travellers."
            ),
            new BookPageInfo
            (
                "Next, the Mirror Mice,",
                "whose reflective fur",
                "can confuse predators",
                "by casting back their",
                "own image. A pest to",
                "some, but a phenomenon",
                "to those who value",
                "their unique fur."
            ),
            new BookPageInfo
            (
                "A mention must be made",
                "of the aquatic Ethereal",
                "Eels, glowing creatures",
                "found in deep waters,",
                "whose bodies flow like",
                "liquid crystal. They are",
                "sought after for their",
                "magical properties."
            ),
            new BookPageInfo
            (
                "One must tread carefully",
                "when dealing with",
                "Blink Hounds. Their",
                "ability to teleport at",
                "will makes them a",
                "formidable creature,",
                "able to escape or",
                "pounce unpredictably."
            ),
            new BookPageInfo
            (
                "For those with a penchant",
                "for cold, the Frost",
                "Ferrets are small",
                "beings capable of",
                "chilling an entire",
                "hall with their breath,",
                "and are as mischievous",
                "as they are cold."
            ),
            new BookPageInfo
            (
                "Lastly, the revered",
                "Phoenix, a symbol of",
                "renewal and fire. To",
                "witness its rebirth is",
                "a sight few forget,",
                "and many alchemists",
                "covet its ashes for",
                "potent elixirs."
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
                "Theodorus Fickle",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your encounters",
                "with these creatures",
                "be as enlightening as",
                "they are enthralling."
            ),
			// Continuing from the previous BookPageInfo...
			new BookPageInfo
			(
				"Let us not forget the",
				"Celestial Caterpillars,",
				"whose cocoons are woven",
				"from pure starlight and",
				"whose transformation",
				"into Star Moths is a",
				"sight of cosmic beauty.",
				"Their presence is said"
			),
			new BookPageInfo
			(
				"to bless the land with",
				"fertile harvests and",
				"clear nights. The silk",
				"produced by these",
				"creatures is of high",
				"value for its magical",
				"weave and radiant",
				"properties."
			),
			new BookPageInfo
			(
				"Among the most curious",
				"are the Ink Imps, small",
				"and mischievous entities",
				"that feed on the words",
				"of books and scrolls,",
				"often rewriting texts",
				"in their playful jest.",
				"Lock your libraries well!"
			),
			new BookPageInfo
			(
				"In stark contrast stand",
				"the stoic Gargoyle Geese,",
				"whose stone-like feathers",
				"make them impervious",
				"to most magic. They are",
				"often seen perched atop",
				"towers, silent watchers",
				"of the nights."
			),
			new BookPageInfo
			(
				"The Seas also harbor",
				"wonders, like the",
				"Bubble Brutes, massive",
				"jellyfish whose domes",
				"encase entire underwater",
				"gardens. Sailors beware,",
				"for their sting can paralyze",
				"a kraken."
			),
			new BookPageInfo
			(
				"Venture into the caves,",
				"and you might encounter",
				"the Crystal Crawlers,",
				"arachnids with bodies",
				"of living gemstone.",
				"Their webs are said to",
				"be stronger than the",
				"toughest steel."
			),
			new BookPageInfo
			(
				"High in the mountains,",
				"the Snow Serpents slither,",
				"leaving trails of frost",
				"in their wake. These",
				"creatures can breathe",
				"a chill so cold it can",
				"freeze fire in its place,",
				"creating sculptures of ice."
			),
			new BookPageInfo
			(
				"The Sky Swine, often",
				"mistaken for clouds,",
				"glide through the air.",
				"At dusk, their bodies",
				"reflect the setting sun,",
				"creating a breathtaking",
				"array of colors across",
				"the horizon."
			),
			new BookPageInfo
			(
				"In the depths of the",
				"forest, one may find",
				"the Bark Beetles, whose",
				"carapaces mimic the",
				"trees they inhabit.",
				"They are the guardians",
				"of the woodlands, and",
				"carriers of seeds."
			),
			new BookPageInfo
			(
				"Enchanted flora also",
				"deserves mention, such",
				"as the Whispering",
				"Blossoms, whose petals",
				"emit soft melodies when",
				"the moon is full. Their",
				"music soothes even the",
				"most troubled minds."
			),
			new BookPageInfo
			(
				"Finally, we chronicle",
				"the Lantern Sprites,",
				"tiny beings of light",
				"that dance in the night.",
				"Their glow is rumored",
				"to reveal hidden paths",
				"and treasures to those",
				"they deem worthy."
			),
			new BookPageInfo
			(
				"This compendium shall",
				"ever grow, for the world",
				"is full of wonder and",
				"mystery. May these",
				"pages serve as a guide",
				"to the charmed and",
				"curious beings among",
				"us. Safe travels!"
			),
			new BookPageInfo
			(
				// These pages left intentionally blank for the owner's notes or drawings.
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
				"Theodorus Fickle",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Beware yet be in awe",
				"of the world's charmed",
				"creatures. For in their",
				"magic, we find our own."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public CompendiumOfCharmedCreatures() : base(false)
        {
            // Set the hue to a specific shade reminiscent of ancient parchment
            Hue = 0x530;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Compendium of Charmed Creatures");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Compendium of Charmed Creatures");
        }

        public CompendiumOfCharmedCreatures(Serial serial) : base(serial)
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
