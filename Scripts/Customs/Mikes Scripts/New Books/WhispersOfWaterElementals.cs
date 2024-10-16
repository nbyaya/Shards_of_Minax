using System;
using Server;

namespace Server.Items
{
    public class WhispersOfWaterElementals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Whispers of Water Elementals", "Merina the Soaked",
            new BookPageInfo
            (
                "In the deepest pools,",
                "and the vastest seas,",
                "whisper the voices",
                "of elementals free.",
                "I am Merina, soaked",
                "to the bone in arcane",
                "lore and ocean's groan."
            ),
            new BookPageInfo
            (
                "Their song flows through",
                "currents deep, a haunting",
                "melody that can lull",
                "a sailor to endless sleep.",
                "I sought their wisdom,",
                "their storm-wrought power,",
                "beneath the waves, hour",
                "by countless hour."
            ),
            new BookPageInfo
            (
                "With breath held long,",
                "and spells prepared,",
                "into their domain I",
                "dove, unscared.",
                "From gentle streams",
                "to oceans vast,",
                "I learned their secrets",
                "and spells to cast."
            ),
            new BookPageInfo
            (
                "A chant for rain,",
                "a verse to quell",
                "a storm. Whispers",
                "soft as the ocean's",
                "calm, yet mighty",
                "as the tempest's balm."
            ),
            new BookPageInfo
            (
                "The elementals spoke",
                "of times long past,",
                "of sunken cities and",
                "empires vast.",
                "Their memories like",
                "tides, rise and fall,",
                "I listened and recorded,",
                "transcribing it all."
            ),
            new BookPageInfo
            (
                "Yet, not all was calm",
                "in the depth's embrace,",
                "some tales were storms",
                "of a darker place.",
                "Of water's wrath, and",
                "shipwrecks' ghosts,",
                "of abyssal depths,",
                "and drowned hosts."
            ),
            new BookPageInfo
            (
                "Through every bubble",
                "and every tide,",
                "I swam and learned,",
                "with elementals as guide.",
                "My quill drips not with",
                "ink, but with brine,",
                "as I pen down this",
                "watery tome of mine."
            ),
            new BookPageInfo
            (
                "And now as I surface,",
                "with tales great and dire,",
                "of water elementals",
                "and their whispered fire.",
                "May this book find",
                "those with heart bold,",
                "to read of the secrets",
                "the water elementals told."
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
                "Merina the Soaked",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the currents guide",
                "your journey true,",
                "and the depths reveal",
                "their heart to you."
            ),
			            // Continued BookContent initialization
            new BookPageInfo
            (
                "Beneath the moon's",
                "pale gaze, I swam",
                "through the sunken",
                "halls of coral maze.",
                "The water elementals",
                "whispered of the tide,",
                "of currents and eddies,",
                "where secrets abide."
            ),
            new BookPageInfo
            (
                "Their voices echoed,",
                "a symphony of the deep,",
                "revealing the dances",
                "water spirits keep.",
                "I learnt of the weaves",
                "of spells so ancient,",
                "where each incantation",
                "was nature's own cadent."
            ),
            new BookPageInfo
            (
                "Amongst the kelp forests,",
                "where light gently fades,",
                "I heard the histories",
                "of underwater glades.",
                "Of elementals, noble",
                "and ancient beyond years,",
                "guarding the ocean's",
                "secrets, hopes, and fears."
            ),
            new BookPageInfo
            (
                "In the realm of depths,",
                "where no man treads,",
                "lie cities of water",
                "and their silent steads.",
                "With wisdom granted",
                "by the elementals' grace,",
                "I saw the ocean's heart,",
                "a vast and sacred space."
            ),
            new BookPageInfo
            (
                "I write now of the rites,",
                "of summoning waves,",
                "of conversing with serpents",
                "in their deep sea caves.",
                "Of rituals to calm",
                "a tempest's rage,",
                "taught by elementals",
                "from a bygone age."
            ),
            new BookPageInfo
            (
                "The water's embrace",
                "is both a gift and bane,",
                "its surface serene,",
                "its depths contain",
                "mysteries untold,",
                "a power raw and primeval,",
                "in the whispers of elementals,",
                "there is knowledge medieval."
            ),
            new BookPageInfo
            (
                "Within these pages",
                "lies the essence of the sea,",
                "its whispers and its songs,",
                "its elemental decree.",
                "I leave this tome",
                "for those who dare to dream,",
                "of the ocean's whispered secrets,",
                "and its silent, echoing scream."
            ),
            new BookPageInfo
            (
                "Let each word written",
                "soak into your soul,",
                "let the water elementals'",
                "wisdom make you whole.",
                "Respect their might,",
                "heed their ancient call,",
                "for in their whispers lies",
                "the destiny of all."
            ),
            new BookPageInfo
            (
                "Should you ever find",
                "yourself by sea's shore,",
                "speak soft the incantations,",
                "and listen to the lore.",
                "The water elementals",
                "may bless you with their sight,",
                "and share with you the whispers",
                "that guide them through the night."
            ),
            new BookPageInfo
            (
                "Now I close this book,",
                "my tale at an end,",
                "with a heart full of memories,",
                "and a spirit to tend.",
                "I am Merina the Soaked,",
                "no longer merely a mage,",
                "but a guardian of secrets",
                "from a bygone age."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank as a stylistic choice
                // to signify the end of the written content.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                "Merina the Soaked",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Let the ocean's whispers",
                "guide your way,",
                "as they have mine,",
                "from night to day."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public WhispersOfWaterElementals() : base(false)
        {
            // Set the hue to a random blue color reminiscent of water
			Hue = Utility.RandomBlueHue();
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Whispers of Water Elementals");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Whispers of Water Elementals");
        }

        public WhispersOfWaterElementals(Serial serial) : base(serial)
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
