using System;
using Server;

namespace Server.Items
{
    public class GoblinGastronomyGoneWild : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Goblin Gastronomy Gone Wild!", "Chef Grommash",
            new BookPageInfo
            (
                "As I set quill to",
                "parchment, my hands",
                "still reek of the",
                "exotic spices from",
                "last night's feast.",
                "I am Grommash,",
                "once a mere cook",
                "in the Goblin camps."
            ),
            new BookPageInfo
            (
                "Now, I am a culinary",
                "adventurer, seeking",
                "flavors untold for",
                "our brutish palettes.",
                "This tome is a record",
                "of my most daring",
                "culinary concoctions.",
                "A journey of taste,"
            ),
            new BookPageInfo
            (
                "that often ends in",
                "disaster, but always",
                "delights the senses.",
                "From the fermented",
                "snail slimes to the",
                "spicy eye of newt,",
                "each recipe is a",
                "gastronomic gamble."
            ),
            new BookPageInfo
            (
                "Take the infamous",
                "Nightshade stew,",
                "a dish that requires",
                "precise timing lest",
                "you desire a sleep",
                "from which you",
                "never wake. A",
                "favorite among"
            ),
            new BookPageInfo
            (
                "the goblin elite!",
                "Or the sizzling",
                "spider skewers,",
                "a crunchy delight",
                "that occasionally",
                "explodes if the",
                "venom sacs aren't",
                "properly removed."
            ),
            new BookPageInfo
            (
                "Feasting with us",
                "is a gamble, with",
                "diners equally likely",
                "to discover a new",
                "favorite dish or",
                "lose their eyebrows",
                "to a fiery mushroom",
                "cap that's been"
            ),
            new BookPageInfo
            (
                "stuffed with too",
                "much dragon powder.",
                "Yet, that's the thrill",
                "of Goblin Gastronomy!",
                "We dine with gusto,",
                "on the edge of",
                "culinary chaos, and",
                "revel in the madness"
            ),
            new BookPageInfo
            (
                "of flavors that",
                "no human chef",
                "would dare to serve.",
                "",
                "If you're brave enough",
                "to try these recipes,",
                "you're either a true",
                "gourmand or a fool."
            ),
            new BookPageInfo
            (
                "But enough warnings,",
                "on to the recipes!",
                "Prepare your taste",
                "buds for a wild",
                "ride through the",
                "Goblin Kingdom's",
                "most bizarre banquets.",
                "May your stomach"
            ),
            new BookPageInfo
            (
                "be stout and your",
                "fire extinguisher",
                "at the ready.",
                "For herein lies",
                "the wild world of",
                "Goblin Gastronomy!",
                "",
                "Chef Grommash,"
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
                "Chef Grommash",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Dine boldly and",
                "without fear!"
            ),
			new BookPageInfo
			(
				"Squiggle Squid Ink",
				"Soup is more than a",
				"meal; it's an art. The",
				"ink must be fresh, or",
				"else the soup lacks",
				"its signature murky",
				"appearance. And the",
				"tentacles? Best when"
			),
			new BookPageInfo
			(
				"they're still wriggling.",
				"The trick is to scare",
				"them into the pot -",
				"a good shout should",
				"do. Then, quick as",
				"a whip, slam the lid!",
				"The taste is...",
				"frighteningly good."
			),
			new BookPageInfo
			(
				"Fermented Fungus",
				"Fizzle is a drink not",
				"for the faint of heart.",
				"It's brewed in old",
				"battle helmets and",
				"stirred with bones",
				"of foes. Leave it in",
				"a dark cave until"
			),
			new BookPageInfo
			(
				"it speaks to you.",
				"Literally. If you can",
				"understand the whispers,",
				"it's ready. Just make",
				"sure to serve it before",
				"the full moon. That's",
				"when it starts to",
				"whisper back."
			),
			new BookPageInfo
			(
				"Grilled Giant Beetle",
				"Barding is a delicate",
				"process, requiring",
				"the beetle to be",
				"cooked within its",
				"shell. The secret",
				"spice? Dried",
				"thunderbug thorax."
			),
			new BookPageInfo
			(
				"Gives it a zesty,",
				"electrifying aftertaste.",
				"Perfect for jump-starting",
				"the heart—and the",
				"party! Warning: Do",
				"not serve to guests",
				"with pacemakers.",
				""
			),
			new BookPageInfo
			(
				"Pixie Dust-Encrusted",
				"Toadstools are a",
				"sight to behold.",
				"Each bite makes",
				"your voice high-",
				"pitched and your",
				"laughter uncontrollable.",
				"Side effects may"
			),
			new BookPageInfo
			(
				"include floating",
				"or glowing in the",
				"dark, but that only",
				"adds to the ambiance",
				"of our moonlit feasts.",
				"Just be cautious of",
				"where you land once",
				"the effects wear off."
			),
			new BookPageInfo
			(
				"Crispy Critter Chips",
				"with Bogwater Dip",
				"is an acquired taste,",
				"I admit. The critters",
				"must be collected",
				"at midnight, crisped",
				"at dawn, and served",
				"by dusk. As for the"
			),
			new BookPageInfo
			(
				"bogwater... it's not",
				"for everyone. But if",
				"you enjoy the thrill",
				"of possible petrification",
				"with your snack,",
				"you'll find no better!",
				"",
				"So there you have it,"
			),
			new BookPageInfo
			(
				"a feast fit for a",
				"goblin king or a",
				"brave soul with a",
				"stomach of steel.",
				"Dine well, my friends,",
				"and may your taste",
				"buds never dull!",
				""
			),
			new BookPageInfo
			(
				"Chef Grommash",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Master of Goblin",
				"Cuisine Extraordinaire"
			)
			// Add as many BookPageInfo objects as needed for additional pages.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public GoblinGastronomyGoneWild() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Goblin Gastronomy Gone Wild!");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Goblin Gastronomy Gone Wild!");
        }

        public GoblinGastronomyGoneWild(Serial serial) : base(serial)
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
