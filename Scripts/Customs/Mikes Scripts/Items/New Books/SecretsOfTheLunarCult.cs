using System;
using Server;

namespace Server.Items
{
    public class SecretsOfTheLunarCult : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Secrets of the Lunar Cult", "High Priestess Selene",
            new BookPageInfo
            (
                "In the quiet of the",
                "night, when the",
                "moonlight bathes the",
                "world in its silvery",
                "glow, the Lunar Cult",
                "gathers in secret.",
                "",
                "We are the keepers of"
            ),
            new BookPageInfo
            (
                "ancient wisdom,",
                "worshipers of the",
                "celestial bodies that",
                "guide our destinies.",
                "Our devotion to the",
                "Moon Goddess Selene",
                "fuels our magic and",
                "inspires our rituals."
            ),
            new BookPageInfo
            (
                "Within these pages, I",
                "shall reveal some of",
                "the secrets of our",
                "enigmatic cult. But",
                "beware, for the power",
                "we harness is not to",
                "be trifled with, and",
                "our mysteries are not"
            ),
            new BookPageInfo
            (
                "meant for the",
                "uninitiated.",
                "",
                "The Lunar Cult's",
                "ancient texts tell of",
                "the phases of the",
                "moon and their",
                "profound influence on"
            ),
            new BookPageInfo
            (
                "our lives. Each phase",
                "holds its own",
                "meaning and offers",
                "unique magical",
                "insights. The New",
                "Moon marks new",
                "beginnings, a time for",
                "setting intentions."
            ),
            new BookPageInfo
            (
                "The Waxing Moon",
                "brings growth and",
                "expansion, while the",
                "Full Moon is a",
                "culmination of power,",
                "ideal for casting",
                "potent spells. The",
                "Waning Moon is a"
            ),
            new BookPageInfo
            (
                "time for banishing",
                "negativity and",
                "cleansing.",
                "",
                "Our rituals often",
                "involve offerings to",
                "the Moon Goddess,",
                "such as rare herbs,"
            ),
            new BookPageInfo
            (
                "crystals, and",
                "sacred oils. We",
                "invoke her presence",
                "to bless our spells",
                "and guide our",
                "journeys through the",
                "astral realm."
            ),
            new BookPageInfo
            (
                "But the Lunar Cult's",
                "power does not",
                "come without",
                "responsibility. We",
                "strive to maintain the",
                "balance of the",
                "cosmos and protect",
                "the sanctity of the"
            ),
            new BookPageInfo
            (
                "night. To reveal",
                "more would be to",
                "trespass upon our",
                "sacred trust. Know",
                "that our path is one",
                "of devotion,",
                "illumination, and",
                "enchantment."
            ),
            new BookPageInfo
            (
                "If you seek the",
                "wisdom of the Lunar",
                "Cult, approach with",
                "respect and an open",
                "heart. The moon's",
                "blessings may shine",
                "upon you, but its",
                "secrets are earned."
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
                "High Priestess Selene",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the moonlight",
                "guide your path."
            ),
			            new BookPageInfo
            (
                "The Moon Goddess",
                "Selene, our revered",
                "patron, is a deity of",
                "mystery and",
                "enchantment. She is",
                "the guardian of",
                "dreams, the muse of",
                "poets, and the source"
            ),
            new BookPageInfo
            (
                "of inspiration for all",
                "who seek the",
                "illumination of the",
                "night. Her gentle",
                "light bathes the",
                "world in silver,",
                "guiding us in times",
                "of darkness."
            ),
            new BookPageInfo
            (
                "The Lunar Cult's",
                "rituals often involve",
                "gazing at the moon,",
                "allowing its",
                "reflection to",
                "penetrate our souls.",
                "Through this",
                "connection, we draw"
            ),
            new BookPageInfo
            (
                "upon Selene's",
                "blessings and unlock",
                "the hidden depths of",
                "our own inner",
                "worlds. It is said that",
                "in the stillness of the",
                "night, one can hear",
                "the whispers of the"
            ),
            new BookPageInfo
            (
                "Moon Goddess herself,",
                "offering guidance and",
                "insight.",
                "",
                "To join the Lunar",
                "Cult is to embark on a",
                "journey of",
                "self-discovery and"
            ),
            new BookPageInfo
            (
                "mystical exploration. We",
                "study the phases of",
                "the moon, seeking to",
                "understand their",
                "influence on magic and",
                "the world around us.",
                "Our spells are",
                "inextricably linked to"
            ),
            new BookPageInfo
            (
                "the lunar cycle,",
                "allowing us to harness",
                "the tides of destiny.",
                "",
                "Our sacred texts",
                "contain ancient",
                "prophecies and",
                "astral maps that guide",
                "us in deciphering the"
            ),
            new BookPageInfo
            (
                "omens revealed by the",
                "moon's glow. It is a",
                "profound and",
                "endless study, for the",
                "moon's influence is",
                "as infinite as the",
                "cosmos itself.",
                ""
            ),
            new BookPageInfo
            (
                "The Lunar Cult's",
                "places of worship are",
                "hidden in the most",
                "remote and mystical",
                "locations, far from",
                "the prying eyes of the",
                "uninitiated. Our",
                "altars are adorned"
            ),
            new BookPageInfo
            (
                "with moonstones,",
                "crystals, and",
                "celestial symbols, all",
                "imbued with the",
                "essence of Selene. The",
                "chants and rituals",
                "that echo through",
                "these sacred spaces"
            ),
            new BookPageInfo
            (
                "resonate with the",
                "power of the moon,",
                "and those who",
                "participate in them",
                "experience a deep",
                "connection to the",
                "cosmic forces.",
                "",
                "To be initiated into"
            ),
            new BookPageInfo
            (
                "the Lunar Cult is to",
                "embrace a lifetime of",
                "study, devotion, and",
                "growth. The path is",
                "challenging, but the",
                "rewards are",
                "boundless. May the",
                "moonlight guide your"
            ),
            new BookPageInfo
            (
                "spirit and reveal the",
                "secrets of the lunar",
                "world to you,",
                "initiate. As High",
                "Priestess of the",
                "Lunar Cult, I",
                "welcome you to our",
                "enlightened circle."
            )
			
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SecretsOfTheLunarCult() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Secrets of the Lunar Cult");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Secrets of the Lunar Cult");
        }

        public SecretsOfTheLunarCult(Serial serial) : base(serial)
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
