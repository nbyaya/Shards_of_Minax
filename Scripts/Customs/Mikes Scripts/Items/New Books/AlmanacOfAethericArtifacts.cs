using System;
using Server;

namespace Server.Items
{
    public class AlmanacOfAethericArtifacts : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Almanac of Aetheric Artifacts", "Archmage Elara",
            new BookPageInfo
            (
                "In the realm of",
                "aetheric magic,",
                "artifacts of great",
                "power and mystery",
                "abound. These",
                "wondrous items are",
                "imbued with the",
                "essence of the cosmos."
            ),
            new BookPageInfo
            (
                "This almanac serves",
                "as a guide to some",
                "of the most",
                "legendary aetheric",
                "artifacts known to",
                "mages and scholars.",
                "May it aid you in",
                "uncovering their secrets."
            ),
            new BookPageInfo
            (
                "1. Astral Crown:",
                "A circlet said to",
                "grant the wearer",
                "visions of distant",
                "realms. Beware, for",
                "such visions can",
                "drive one to madness.",
                ""
            ),
            new BookPageInfo
            (
                "2. Starlight Scepter:",
                "This scepter channels",
                "the power of stars",
                "themselves. It can",
                "create blinding",
                "light or cast",
                "celestial spells of",
                "devastation."
            ),
            new BookPageInfo
            (
                "3. Etherium Talisman:",
                "A pendant that",
                "enhances a mage's",
                "aetheric abilities.",
                "It can extend the",
                "duration of spells,",
                "but its use comes",
                "at a price."
            ),
            new BookPageInfo
            (
                "4. Voidshroud Cloak:",
                "This cloak grants",
                "invisibility to its",
                "wearer, but it",
                "siphons their life",
                "force with each",
                "use. A double-edged",
                "blade of concealment."
            ),
            new BookPageInfo
            (
                "5. Nebula Orb:",
                "An orb that",
                "manipulates the",
                "cosmic energies. It",
                "can reshape matter",
                "and even alter time",
                "within a limited",
                "radius."
            ),
            new BookPageInfo
            (
                "6. Aetheric Compass:",
                "A compass that",
                "guides its bearer to",
                "hidden rifts in the",
                "aetheric fabric,",
                "leading to unknown",
                "worlds and realms.",
                ""
            ),
            new BookPageInfo
            (
                "7. Celestial Robes:",
                "These robes are said",
                "to grant the wearer",
                "the wisdom of the",
                "stars. They protect",
                "against aetheric",
                "attacks and grant",
                "insight into the future."
            ),
            new BookPageInfo
            (
                "8. Etherial Blade:",
                "A sword forged from",
                "aetheric crystals.",
                "It can cut through",
                "magic defenses and",
                "dispel enchantments.",
                "A weapon of the",
                "aetheric plane."
            ),
            new BookPageInfo
            (
                "These artifacts are",
                "both powerful and",
                "dangerous. Seek",
                "them with caution,",
                "for their aetheric",
                "energies can",
                "challenge even the",
                "mightiest of mages."
            ),
            new BookPageInfo
            (
                "May the knowledge",
                "contained within this",
                "almanac serve as a",
                "guide in your quest",
                "to harness the",
                "aetheric arts and",
                "uncover the secrets",
                "of these artifacts."
            ),
            new BookPageInfo
            (
                "Archmage Elara",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the aetheric",
                "be ever at your",
                "command."
            ),
			new BookPageInfo
			(
				"9. Ephemeral Cloak:",
				"This cloak allows its",
				"wearer to phase",
				"through solid objects",
				"and become",
				"ethereal. Beware,",
				"for prolonged use",
				"can lead to isolation."
			),
			new BookPageInfo
			(
				"10. Aetheric Crystals:",
				"These crystalline",
				"shards are prized by",
				"aetheric mages for",
				"their ability to",
				"amplify spells. They",
				"can enhance the",
				"potency of magic."
			),
			new BookPageInfo
			(
				"11. Arcane Resonator:",
				"A handheld device",
				"that can detect",
				"aetheric energies and",
				"ley lines. It aids in",
				"locating sources of",
				"power and hidden",
				"aetheric rifts."
			),
			new BookPageInfo
			(
				"12. Timekeeper's Hourglass:",
				"This hourglass can",
				"temporarily suspend",
				"time within a small",
				"area. It is a rare",
				"artifact coveted by",
				"those who seek to",
				"manipulate time."
			),
			new BookPageInfo
			(
				"13. Stellar Prism:",
				"A prism that can",
				"capture and harness",
				"the energy of",
				"falling stars. It can",
				"store celestial power",
				"for later use in",
				"powerful spells."
			),
			new BookPageInfo
			(
				"14. Astral Key:",
				"A key that can unlock",
				"portals to distant",
				"realms and planes.",
				"It is said to be",
				"forged from the",
				"essence of the stars",
				"themselves."
			),
			new BookPageInfo
			(
				"15. Planar Grimoire:",
				"A spellbook that",
				"contains forbidden",
				"knowledge of the",
				"aetheric planes. It",
				"grants access to",
				"otherworldly spells",
				"and dimensions."
			),
			new BookPageInfo
			(
				"16. Aetherium Golem:",
				"A construct infused",
				"with aetheric",
				"energies. It is a",
				"loyal guardian and",
				"can absorb and",
				"channel aetheric",
				"attacks."
			),
			new BookPageInfo
			(
				"17. Celestial Compass:",
				"This compass guides",
				"the way to",
				"celestial bodies and",
				"astral phenomena.",
				"It is invaluable for",
				"astrologers and",
				"astronomers."
			),
			new BookPageInfo
			(
				"18. Voidshifter Dagger:",
				"A dagger that can",
				"create temporary rifts",
				"in the fabric of",
				"reality. It allows the",
				"wielder to",
				"teleport short",
				"distances."
			),
			new BookPageInfo
			(
				"19. Nexus Amulet:",
				"An amulet that",
				"connects the wearer",
				"to the aetheric",
				"nexus. It grants",
				"access to hidden",
				"knowledge and",
				"teleportation."
			),
			new BookPageInfo
			(
				"20. Ethereal Chalice:",
				"A chalice that can",
				"transform liquids into",
				"aetheric energy. It",
				"can replenish a",
				"mage's mana but",
				"carries a risk of",
				"overconsumption."
			),
			new BookPageInfo
			(
				"21. Voidcaller's Staff:",
				"This staff can summon",
				"beings from the",
				"aetheric void. Use",
				"with caution, for",
				"the void's denizens",
				"are unpredictable.",
				""
			),
			new BookPageInfo
			(
				"22. Arcane Nexus:",
				"A structure of great",
				"power that anchors",
				"the aetheric",
				"energies of an area.",
				"It can be used as a",
				"focus for powerful",
				"rituals."
			),
			new BookPageInfo
			(
				"These are just a few",
				"examples of aetheric",
				"artifacts. Each",
				"artifact carries its",
				"own unique powers and",
				"risks. The quest to",
				"uncover their secrets",
				"is an ongoing journey."
			),
			new BookPageInfo
			(
				"Remember, the aetheric",
				"arts are not to be",
				"taken lightly. These",
				"artifacts hold great",
				"potential, but they",
				"can also lead to",
				"catastrophe in",
				"inexperienced hands."
			),
			new BookPageInfo
			(
				"May your pursuit of",
				"aetheric knowledge",
				"be guided by wisdom",
				"and caution. Seek",
				"not only power but",
				"understanding, for the",
				"true mastery of the",
				"aetheric arts lies",
				"within your heart."
			),
			new BookPageInfo
			(
				"Archmage Elara",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the aetheric",
				"realms be your",
				"endless horizon."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AlmanacOfAethericArtifacts() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Almanac of Aetheric Artifacts");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Almanac of Aetheric Artifacts");
        }

        public AlmanacOfAethericArtifacts(Serial serial) : base(serial)
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
