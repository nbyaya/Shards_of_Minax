using System;
using Server;

namespace Server.Items
{
    public class TheAffairsOfWizards : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Affairs of Wizards", "Merlin",
                new BookPageInfo
                (
                    "In an age long past,",
                    "wizards held the key",
                    "to the secrets of the",
                    "universe. This book",
                    "serves to document",
                    "the affairs, both",
                    "mundane and magical,",
                    "of these enigmatic"
                ),
                new BookPageInfo
                (
                    "beings.",
                    "Wizards are the",
                    "keepers of ancient",
                    "lore, they wield",
                    "powers that defy",
                    "the natural world.",
                    "However, with great",
                    "power comes great"
                ),
                new BookPageInfo
                (
                    "responsibility. Many",
                    "wizards have fallen",
                    "victim to their own",
                    "arrogance, using",
                    "their abilities for",
                    "evil purposes.",
                    "Yet, there are those",
                    "who use magic for"
                ),
                new BookPageInfo
                (
                    "the greater good.",
                    "Wizards have often",
                    "been instrumental in",
                    "defending the realm",
                    "from dark forces.",
                    "They have also",
                    "played a key role",
                    "in technological and",
                    "cultural advancements."
                ),
				new BookPageInfo
				(
					"Chapter 1: The Council",
					"of Mages",
					"Every decade, wizards",
					"from all over the",
					"realm gather in a",
					"secret location for",
					"the Council of Mages."
				),
				new BookPageInfo
				(
					"This assembly serves",
					"as a platform for",
					"sharing knowledge,",
					"debating ethics, and",
					"discussing the future",
					"of magic in the",
					"world."
				),
				new BookPageInfo
				(
					"Chapter 2: The",
					"Forbidden Spells",
					"While magic can be",
					"used for good, there",
					"exist forbidden spells",
					"that have catastrophic"
				),
				new BookPageInfo
				(
					"consequences. These",
					"spells are sealed away",
					"in hidden tomes,",
					"guarded by ancient",
					"wards to prevent",
					"misuse."
				),
				new BookPageInfo
				(
					"Chapter 3: Wizard",
					"Duels",
					"Although rare, wizard",
					"duels are a way to",
					"settle conflicts that",
					"cannot be resolved"
				),
				new BookPageInfo
				(
					"through dialogue.",
					"These duels are",
					"strictly regulated to",
					"ensure that no",
					"irreparable harm comes",
					"to either party."
				),
				new BookPageInfo
				(
					"Chapter 4: Enchantments",
					"and Potions",
					"Magic isn't just about",
					"casting spells. Wizards",
					"also master the art of"
				),
				new BookPageInfo
				(
					"creating enchanted",
					"items and brewing",
					"potions, contributing",
					"to the community in",
					"practical ways."
				),
				new BookPageInfo
				(
					"Chapter 5: Legacy and",
					"Mentorship",
					"One of the most",
					"important duties of a",
					"wizard is to pass on"
				),
				new BookPageInfo
				(
					"their knowledge to",
					"the next generation,",
					"ensuring that the",
					"legacy of magic",
					"continues for the",
					"benefit of all."
				),
                new BookPageInfo
                (
                    "This concludes our",
                    "brief overview on",
                    "the affairs of",
                    "wizards. The path",
                    "of magic is perilous,",
                    "but for those who",
                    "navigate it wisely,",
                    "the rewards are"
                ),
                new BookPageInfo
                (
                    "immeasurable.",
                    "          -Merlin"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheAffairsOfWizards() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Affairs of Wizards");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Affairs of Wizards");
        }

        public TheAffairsOfWizards(Serial serial) : base(serial)
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
