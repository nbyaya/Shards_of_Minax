using System;
using Server;

namespace Server.Items
{
    public class OrcishAlchemy2 : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Orcish Alchemy", "Grug the Brewmaster",
            new BookPageInfo
            (
                "Many moons have",
                "passed since I, Grug,",
                "first stirred pot of",
                "mud and bone. Orcish",
                "alchemy, powerful art,",
                "steeped in tradition,",
                "strength, and spirit of",
                "our people."
            ),
            new BookPageInfo
            (
                "This tome is Grug's",
                "legacy, collection of",
                "recipes and rites. Not",
                "for faint of heart, nor",
                "weak of stomach. Here",
                "lie secrets of our",
                "potent concoctions,",
                "brewed from the land."
            ),
            new BookPageInfo
            (
                "The brews we craft",
                "not just for battle.",
                "They heal, they",
                "harden, they invoke",
                "the rage of Gruumsh.",
                "Each potion, each",
                "elixir, sacred to our",
                "tribes and warlords."
            ),
            new BookPageInfo
            (
                "First recipe, the",
                "Blood of Gruumsh.",
                "Mighty draught,",
                "imbued with spirit",
                "of One-Eye. Grants",
                "strength, ferocity,",
                "and sight beyond",
                "sight to warrior."
            ),
            new BookPageInfo
            (
                "Take heart of bear,",
                "eye of hawk, blood",
                "of enemy. Mix with",
                "nightshade, brimstone,",
                "and tear of virgin",
                "troll. Boil in pot made",
                "from skull of foe,",
                "stir with femur bone."
            ),
            new BookPageInfo
            (
                "Another, Elixir of",
                "Stoneflesh. Drink,",
                "and skin turns to",
                "rock. Pain of blade,",
                "sting of arrow, like",
                "kisses on cheek.",
                "Mix mud from sacred",
                "earth, with crushed"
            ),
            new BookPageInfo
            (
                "bones, and spit of",
                "elder shaman. Let",
                "sit under full moon,",
                "then consume under",
                "new sun. Warrior",
                "becomes unyielding,",
                "unbreakable for time."
            ),
            new BookPageInfo
            (
                "Beware, alchemy",
                "potent, but fickle.",
                "Mistake in chant or",
                "stir, and potion",
                "may turn. May",
                "boil over, sear",
                "flesh, curse blood,",
                "or worse. Respect"
            ),
            new BookPageInfo
            (
                "the brew, respect",
                "the spirits that",
                "lend their power.",
                "Orcish alchemy is",
                "gift, a weapon, a",
                "blessing from the",
                "ancients to their",
                "mighty descendants."
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
                "Grug the Brewmaster",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your cauldron",
                "never crack, and your",
                "brews be strong."
            ),
			// Continuing from the previous content...
			new BookPageInfo
			(
				"To craft Thunderbrew,",
				"the storm's fury we",
				"capture. From clouds",
				"above, with arrows",
				"hooked to chains,",
				"we steal lightning,",
				"bottle it with black",
				"powder and iron shavings."
			),
			new BookPageInfo
			(
				"Caution, this brew",
				"not for weak. Explosive",
				"might in hands of",
				"brave. Used in sieges,",
				"or to clear path through",
				"mountains and men.",
				"Thunderbrew's boom",
				"heralds Orcish might."
			),
			new BookPageInfo
			(
				"Next, the Spiritwalk",
				"Potion. Grants drinker",
				"sight of spirit world,",
				"to talk with ancestors,",
				"seek guidance, or",
				"curse enemies with",
				"haunting visions from",
				"the beyond."
			),
			new BookPageInfo
			(
				"Brew from ghost",
				"mushroom, fermented",
				"in tears of banshee.",
				"Add wisp essence at",
				"midnight. Chant passed",
				"down from shaman to",
				"shaman, to bind spirits",
				"to the potion."
			),
			new BookPageInfo
			(
				"Last, the Warpaint",
				"of the Berserker. Not",
				"drink, but sacred",
				"mixture. Applied to",
				"flesh before battle.",
				"Draws power of beast,",
				"and fury of war god",
				"into warrior's heart."
			),
			new BookPageInfo
			(
				"Mix blood of wyvern",
				"with ash of burnt",
				"banners of foes. Grind",
				"with herb of rage,",
				"and bone dust of",
				"fallen heroes. Warpaint",
				"is sacred art, passed",
				"from father to son."
			),
			new BookPageInfo
			(
				"This tome holds more",
				"than mere recipes. It",
				"bears essence of our",
				"people, our strength,",
				"our survival. Orcish",
				"alchemy interwoven",
				"with our fate, our",
				"very being."
			),
			new BookPageInfo
			(
				"Let each brew tell",
				"story of our clans,",
				"our struggles, our",
				"triumphs. May these",
				"pages serve as guide",
				"to power, to victory.",
				"For in brew, we trust.",
				"In alchemy, we rise."
			),
			new BookPageInfo
			(
				"Let enemies cower,",
				"for with these potions",
				"we march. We do not",
				"fear death, for it fears",
				"us. Our cauldrons",
				"bubble with the might",
				"of the Orc, with the",
				"wrath of the elements."
			),
			new BookPageInfo
			(
				"Remember, brew with",
				"honor. Remember your",
				"ancestors. Remember",
				"the laws of Grummsh.",
				"With this knowledge,",
				"carry forth the legacy",
				"of the Orcish alchemists,",
				"forever unbroken."
			),
			new BookPageInfo
			(
				"Grug the Brewmaster",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Let the cauldron's fire",
				"burn eternal, as the",
				"spirits of our ancestors",
				"guide our hands in the",
				"sacred art of alchemy."
			)
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OrcishAlchemy2() : base(false)
        {
            // Set the hue to a random greenish color, symbolizing Orcish craftsmanship
            Hue = Utility.RandomMinMax(2001, 2100);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Orcish Alchemy");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Orcish Alchemy");
        }

        public OrcishAlchemy2(Serial serial) : base(serial)
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
