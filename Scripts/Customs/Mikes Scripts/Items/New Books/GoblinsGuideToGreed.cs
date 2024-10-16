using System;
using Server;

namespace Server.Items
{
    public class GoblinsGuideToGreed : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Goblin's Guide to Greed", "Grin the Green",
            new BookPageInfo
            (
                "This guide, penned by",
                "Grin the Green, the most",
                "avaricious goblin of",
                "the land, is a testament",
                "to the art of amassing",
                "wealth by any means.",
                "Let this tome be your",
                "guide to greed!"
            ),
            new BookPageInfo
            (
                "Lesson one: Hoard.",
                "Hoard every shinie,",
                "from gold to glittering",
                "trinkets. Finders keepers",
                "is the goblin's creed.",
                "If it glints, into the",
                "sack it goes, no",
                "matter its use!"
            ),
            new BookPageInfo
            (
                "Lesson two: Scavenge.",
                "Why toil for treasures",
                "when the pockets and",
                "packs of adventurers",
                "are ripe for the picking?",
                "A distracted hero is",
                "a goblin's goldmine."
            ),
            new BookPageInfo
            (
                "Lesson three: Bargain.",
                "Everything's for sale,",
                "for the right amount of",
                "shinies. Even secrets",
                "can be bought and",
                "sold. Remember,",
                "gossip is gold!"
            ),
            new BookPageInfo
            (
                "Lesson four: Cheat.",
                "If a deal seems fair,",
                "you're doing it wrong!",
                "The best trade is one",
                "where only you gain.",
                "Words are wind,",
                "but gold is power."
            ),
            new BookPageInfo
            (
                "Lesson five: Steal.",
                "When all else fails,",
                "filch it! Stealth and",
                "sleight of hand are",
                "tools of the trade.",
                "But beware the",
                "clang of heavy armor!"
            ),
            new BookPageInfo
            (
                "Lesson six: Horde.",
                "Never share your",
                "loot. A true goblin",
                "keeps his stash",
                "hidden, for sharing",
                "is the start of",
                "starving."
            ),
            new BookPageInfo
            (
                "Lesson seven: Double-cross.",
                "Alliances are temporary,",
                "treachery is eternal.",
                "The only alliance a",
                "goblin needs is with",
                "his treasure pile."
            ),
            new BookPageInfo
            (
                "To live is to covet,",
                "and to covet is to live.",
                "This is the goblin way!",
                "May your sacks be heavy,",
                "your purses full, and",
                "your victims none the",
                "wiser. Grin the Green"
            ),
			    new BookPageInfo
			(
				"Lesson eight: Raid.",
				"When the moon grins,",
				"we goblins dance and",
				"raid the unwary. Be it",
				"a lone wagon or a",
				"drunken merchant,",
				"fortune favors the bold!"
			),
			new BookPageInfo
			(
				"Lesson nine: Gamble.",
				"Luck is a fickle friend,",
				"but sometimes she kisses",
				"you square on the lips.",
				"Dice and cards can",
				"multiply your gold,",
				"if you play with sleight."
			),
			new BookPageInfo
			(
				"Lesson ten: Lurk.",
				"There's wisdom in the",
				"shadows. Lurk and listen,",
				"for opportunities are like",
				"fish in a stream—",
				"there for the catching",
				"by the patient predator."
			),
			new BookPageInfo
			(
				"Lesson eleven: Bribe.",
				"A well-placed shiny can",
				"turn the most loyal",
				"guard into a guide to",
				"unlocked treasuries.",
				"Gold not only talks,",
				"it convinces."
			),
			new BookPageInfo
			(
				"Lesson twelve: Forge.",
				"Why risk life and limb",
				"for treasures when you",
				"can make your own?",
				"With a clever hand,",
				"counterfeit coins can",
				"fool the best of them."
			),
			new BookPageInfo
			(
				"Lesson thirteen: Beguile.",
				"Use charm like a pickaxe",
				"for the mines of men's",
				"hearts. A sweet word and",
				"a tear can turn a scowl",
				"to a handout, or a",
				"trickle into a stream."
			),
			new BookPageInfo
			(
				"Lesson fourteen: Feign.",
				"Play the fool, the frail,",
				"the fawn; let them think",
				"you harmless. Then, when",
				"their back is turned,",
				"strike! The world loves",
				"a trickster, especially rich."
			),
			new BookPageInfo
			(
				"Lesson fifteen: Enchant.",
				"A little magic goes a long",
				"way. A sparkle here, a glow",
				"there, and worthless baubles",
				"become priceless artifacts.",
				"Sell quickly before the",
				"glamour fades."
			),
			new BookPageInfo
			(
				"Lesson sixteen: Invest.",
				"Sometimes, to multiply",
				"your gold, you must plant",
				"it. Find ventures hungry",
				"for coin, and your purse",
				"will swell like a tick!"
			),
			new BookPageInfo
			(
				"Lesson seventeen: Hoax.",
				"Concoct a crisis—a dragon",
				"sighting, a curse, a ghost.",
				"Then, offer the solution for",
				"a hefty sum. Fear is the",
				"key to open coin purses."
			),
			new BookPageInfo
			(
				"Lesson eighteen: Thieve.",
				"Become a shadow, a whisper,",
				"a legend. Let none but the",
				"moon witness your heist.",
				"The night is a goblin's",
				"cloak; wear it with pride."
			),
			new BookPageInfo
			(
				"In conclusion, fellow",
				"greed-mongers, may this",
				"guide serve you well.",
				"Walk the path of shadows,",
				"fill your life with gold,",
				"and let no man claim",
				"what's rightfully yours."
			),
			new BookPageInfo
			(
				"Remember, greed is not",
				"a sin; it's a way of life.",
				"Grin the Green has spoken,",
				"now go forth and pilfer,",
				"plunder, and prosper!",
				"May your loot be legendary."
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
                "Grin the Green",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In gold we trust."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public GoblinsGuideToGreed() : base(false)
        {
            // Set the hue to a goblin green color
            Hue = Utility.RandomMinMax(2000, 2010);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Goblin's Guide to Greed");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Goblin's Guide to Greed");
        }

        public GoblinsGuideToGreed(Serial serial) : base(serial)
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
