using System;
using Server;

namespace Server.Items
{
    public class TheThriftyThaumaturge : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Thrifty Thaumaturge", "Cerin the Saver",
            new BookPageInfo
            (
                "In an era of grand",
                "spells and costly",
                "components, I, Cerin,",
                "penned this guide for",
                "the economical",
                "enchanters who seek",
                "the wisdom in frugality.",
                "Behold, the path to"
            ),
            new BookPageInfo
            (
                "affordable arcana.",
                "Doubt not the humble",
                "reagents - For in the",
                "common, true power",
                "often sleeps. A pinch",
                "of salt over a pound",
                "of gold dust, that is",
                "my creed."
            ),
            new BookPageInfo
            (
                "Spellcasting need not",
                "be an endeavor that",
                "drains one's coffers.",
                "With prudent planning",
                "and wise substitution,",
                "even the most complex",
                "magics can be woven",
                "without undue expense."
            ),
            new BookPageInfo
            (
                "Herein lies advice",
                "and practical tips",
                "for the thrifty mage",
                "in all of us. From",
                "finding alternative",
                "spell components, to",
                "harnessing natural",
                "energies cost-free."
            ),
            new BookPageInfo
            (
                "Discover the art of",
                "spell recycling, where",
                "residual magics are",
                "harnessed for future",
                "casts. Learn the",
                "secrets of mana",
                "conservation, and how",
                "to bargain with"
            ),
            new BookPageInfo
            (
                "elementals for their",
                "services, rather than",
                "enslaving them.",
                "Each chapter is scribed",
                "with the mindful mage",
                "in mind, ensuring",
                "your path to power",
                "does not leave you"
            ),
            new BookPageInfo
            (
                "impoverished.",
                "",
                "Turn the page, thrifty",
                "thaumaturge, and let",
                "us begin the journey",
                "of economical",
                "enchantment together.",
                "May your coin purse"
            ),
            new BookPageInfo
            (
                "always jingle, and your",
                "spellbook never lack",
                "for a useful incantation.",
                "",
                "Cerin the Saver"
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
                "Cerin the Saver",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Magic thrives not only",
                "in the heart of the",
                "wealthy. Even the",
                "commoner, with clever",
                "mind, can wield the",
                "arcane with thrift."
            ),
			// Continued BookContent for "The Thrifty Thaumaturge"
			new BookPageInfo
			(
				"Your spellbook, a tome",
				"of potential, need not",
				"be bound by golden",
				"clasps to harness",
				"the winds of the",
				"aether. Nay, it is",
				"your wits, the sharp",
				"blade that cuts"
			),
			new BookPageInfo
			(
				"through the fabric of",
				"reality, that serves",
				"as the true vessel.",
				"Consider the ways",
				"of the thrifty:",
				"Barter with the bee",
				"for its honey, sweet",
				"and pure, a perfect"
			),
			new BookPageInfo
			(
				"substitute for the",
				"rare nectar of",
				"mana blossoms.",
				"Study the ants,",
				"their methods of",
				"alchemy, turning",
				"leaf into sustenance,",
				"as you should turn"
			),
			new BookPageInfo
			(
				"the mundane into",
				"the magical.",
				"Respect the economy",
				"of nature, the balance",
				"that requires no coin,",
				"and mimic its ways",
				"in your arts. Let the",
				"world be your"
			),
			new BookPageInfo
			(
				"supplier, and you",
				"shall never want for",
				"reagents again.",
				"",
				"Beneath the full",
				"moon's glow, even",
				"the common daisy",
				"basks in mana."
			),
			new BookPageInfo
			(
				"Pick it with care,",
				"and it may serve",
				"as well as any",
				"exotic bloom from",
				"the deepest wilds.",
				"The dew from a",
				"spider's web, spun",
				"at dawn, can hold"
			),
			new BookPageInfo
			(
				"enchantments most",
				"potent. Seek these",
				"small treasures, and",
				"you shall be rewarded",
				"with a bounty far",
				"greater than the sum",
				"of its parts.",
				""
			),
			new BookPageInfo
			(
				"Turn the simple into",
				"the sublime. With a",
				"touch of innovation,",
				"a mundane pebble",
				"transmutes into a",
				"philosopher's stone,",
				"and a twig becomes",
				"a wand of power."
			),
			new BookPageInfo
			(
				"Thus armed, a thrifty",
				"thaumaturge may",
				"stand equal to the",
				"most spendthrift",
				"sorcerer, their spells",
				"just as bright, their",
				"summons just as",
				"mighty, yet their"
			),
			new BookPageInfo
			(
				"pouches far heavier",
				"with gold saved.",
				"So worry not as you",
				"pass the lavish",
				"towers of the magus",
				"elite. Their splendor",
				"is but a facade that",
				"hides a hollow core."
			),
			new BookPageInfo
			(
				"The true strength",
				"of magic lies not",
				"in opulence, but in",
				"ingenuity—the clever",
				"conjurings of a",
				"resourceful mind.",
				"Thus concludes this",
				"volume, yet the"
			),
			new BookPageInfo
			(
				"journey continues.",
				"Walk the path of",
				"prudence, and let",
				"your spells be both",
				"mighty and economical.",
				"May your robes",
				"never fray, your",
				"cauldron never crack."
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
				"Cerin the Saver",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Cast wisely, save",
				"heartily, and may",
				"your thrifty magic",
				"prosper."
			)
			// ...
			// Continue with the rest of the serialization/deserialization as previous

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheThriftyThaumaturge() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Thrifty Thaumaturge");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Thrifty Thaumaturge");
        }

        public TheThriftyThaumaturge(Serial serial) : base(serial)
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
