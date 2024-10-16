using System;
using Server;

namespace Server.Items
{
    public class PhantomsPhylactery : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Phantom's Phylactery", "Amarion the Enchanter",
            new BookPageInfo
            (
                "In the realm of magic",
                "and mystery, there exists",
                "a legend of a powerful",
                "artifact known as the",
                "Phantom's Phylactery.",
                "",
                "This elusive item is said",
                "to hold the essence of a"
            ),
            new BookPageInfo
            (
                "cunning and vengeful",
                "phantom, bound to it",
                "for all eternity. The",
                "story of how it came",
                "into existence is shrouded",
                "in secrets, as the phylactery",
                "itself is hidden from the",
                "eyes of mortals."
            ),
            new BookPageInfo
            (
                "Legends tell of a powerful",
                "enchanter named Amarion",
                "who sought to harness",
                "the spectral energies of",
                "the afterlife. His",
                "obsession led him to",
                "unearthly realms, where",
                "he encountered a vengeful"
            ),
            new BookPageInfo
            (
                "phantom known as",
                "Zephyrion. In a battle",
                "of wits and magic,",
                "Amarion managed to trap",
                "Zephyrion's essence",
                "within a gemstone,",
                "creating the Phylactery.",
                ""
            ),
            new BookPageInfo
            (
                "The Phylactery is said",
                "to grant its possessor",
                "the ability to control",
                "ghostly apparitions and",
                "tap into the ethereal",
                "powers of the beyond.",
                "But it comes at a great",
                "cost, for Zephyrion's"
            ),
            new BookPageInfo
            (
                "vengeful spirit remains",
                "trapped within, seeking",
                "to manipulate and",
                "subvert the will of",
                "those who dare to wield",
                "the Phylactery's",
                "unearthly power.",
                ""
            ),
            new BookPageInfo
            (
                "Many have sought the",
                "Phantom's Phylactery,",
                "believing it to be the key",
                "to ultimate power. But",
                "few have succeeded in",
                "their quest, and many",
                "have been consumed by",
                "Zephyrion's malevolent"
            ),
            new BookPageInfo
            (
                "influence. Its location is",
                "known only to a select",
                "few, and it is guarded by",
                "mystical wards and traps",
                "that challenge even the",
                "most skilled adventurers.",
                "Should you ever come",
                "across it, remember the"
            ),
            new BookPageInfo
            (
                "cautionary tale of Amarion",
                "and his fateful encounter",
                "with the vengeful",
                "phantom. The Phylactery",
                "may offer power beyond",
                "imagination, but it also",
                "carries a curse that can",
                "consume the soul."
            ),
            new BookPageInfo
            (
                "Amarion the Enchanter",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May you tread carefully",
                "in the realm of magic.",
                ""
            ),
			            new BookPageInfo
            (
                "As the story goes,",
                "Amarion's obsession with",
                "the Phylactery grew",
                "unchecked. He used its",
                "power to bend spirits",
                "and ethereal forces to his",
                "will, gaining dominion over",
                "the supernatural."
            ),
            new BookPageInfo
            (
                "However, Zephyrion's",
                "malevolent influence",
                "began to seep into",
                "Amarion's mind,",
                "whispering dark secrets",
                "and tempting him with",
                "unlimited power. The",
                "enchanter's descent into"
            ),
            new BookPageInfo
            (
                "madness was slow but",
                "inevitable. He became",
                "obsessed with unlocking",
                "the Phylactery's ultimate",
                "potential, believing it",
                "would make him a god",
                "among mortals.",
                ""
            ),
            new BookPageInfo
            (
                "Amarion's experiments",
                "grew more dangerous,",
                "drawing the attention of",
                "the magical community.",
                "Wizards and sorcerers",
                "came to investigate the",
                "strange happenings,",
                "unaware of the Phylactery's"
            ),
            new BookPageInfo
            (
                "dark secret. They",
                "confronted Amarion in",
                "his hidden sanctum,",
                "where a fierce battle",
                "ensued. The enchanter,",
                "driven by madness and",
                "the Phylactery's power,",
                "fought with unnatural"
            ),
            new BookPageInfo
            (
                "strength and dark",
                "sorcery. Many of the",
                "intruders were slain,",
                "but in the end,",
                "Amarion was defeated.",
                "",
                "As he lay dying, the",
                "Phylactery's curse"
            ),
            new BookPageInfo
            (
                "manifested fully. His",
                "soul was torn from his",
                "body, becoming one with",
                "Zephyrion's vengeful",
                "spirit trapped within the",
                "gemstone. The Phylactery",
                "was lost to the world,",
                "hidden in a place only"
            ),
            new BookPageInfo
            (
                "the phantom knew.",
                "",
                "And so, the legend of",
                "the Phantom's Phylactery",
                "lives on, a cautionary tale",
                "of the dangers of",
                "tampering with the",
                "unknown and the price"
            ),
            new BookPageInfo
            (
                "of unchecked ambition.",
                "Beware those who seek",
                "to wield its power, for",
                "the Phylactery may grant",
                "you dominion over spirits,",
                "but it will also bind",
                "your fate to that of a",
                "vengeful phantom."
            ),
            new BookPageInfo
            (
                "Amarion the Enchanter",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May this tale serve as a",
                "warning to all who dare",
                "delve into the unknown.",
                ""
            )


        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public PhantomsPhylactery() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
			Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Phantom's Phylactery");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Phantom's Phylactery");
        }

        public PhantomsPhylactery(Serial serial) : base(serial)
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
