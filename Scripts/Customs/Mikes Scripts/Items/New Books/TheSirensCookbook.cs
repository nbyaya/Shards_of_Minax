using System;
using Server;

namespace Server.Items
{
    public class TheSirensCookbook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Siren's Cookbook", "Marinella Waveborn",
            new BookPageInfo
            (
                "From the azure depths",
                "and briny shallows, I",
                "bring forth the zests",
                "of the sea. I am",
                "Marinella, siren of",
                "the simmering tides,",
                "and this tome holds",
                "culinary secrets"
            ),
            new BookPageInfo
            (
                "of the ocean's bounty.",
                "Be it the softest sands",
                "or the wildest waves,",
                "every grain and gale",
                "whispers a recipe,",
                "waiting to be savored.",
                "Let these pages guide",
                "thy taste to shores"
            ),
            new BookPageInfo
            (
                "unknown, where flavor",
                "roams free as the sea",
                "itself. I shall take thee",
                "on a gastronomic voyage",
                "where seaweed is more",
                "than brine-soaked fronds,",
                "and each shell conceals",
                "a succulent surprise."
            ),
            new BookPageInfo
            (
                "Fret not of depths",
                "unexplored, for herein",
                "lies the knowledge of",
                "shipwrecked feasts,",
                "pirates' brews, and",
                "the merfolk's fey wines.",
                "Thy palate shall dance",
                "to the trident's prongs."
            ),
            new BookPageInfo
            (
                "Start with brine-kissed",
                "appetizers, drift through",
                "soups swirled with coral",
                "essence, and brave the",
                "main courses, bold as",
                "a kraken's embrace.",
                "Lest we forget the sweet",
                "treasures hidden"
            ),
            new BookPageInfo
            (
                "beneath the waves—",
                "desserts that sing siren",
                "songs, ensnaring the",
                "sense like a sailor's",
                "final dream. And for",
                "the adventurous soul,",
                "a draught of the deep",
                "abyss—potions of"
            ),
            new BookPageInfo
            (
                "tides eternal, bubbling",
                "with magic old as salt.",
                "Each recipe within is a",
                "whisper of nautical",
                "nymphs, a secret shared",
                "through bubbles rising",
                "to the surface, kissed by",
                "moonlight and sealed"
            ),
            new BookPageInfo
            (
                "with a siren's promise.",
                "May the waves be ever",
                "in thy favor, and the",
                "catch plentiful. Let",
                "the ocean's heart beat",
                "within thy cooking pot,",
                "and know the joy of",
                "sea's endless bounty."
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
                "Marinella Waveborn",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your pot never be",
                "empty, and your belly",
                "never full. Bon Appétit,",
                "from the sea's embrace."
            ),
            // New pages start here
            new BookPageInfo
            (
                "Herein, the first secret:",
                "To tame the tide, one must",
                "start with 'Siren's Fingers,'",
                "delicate fronds of seaweed",
                "twined with crab's delight.",
                "Best served chilled, with",
                "a slice of sea lemon, so",
                "sharp it could cut the brine"
            ),
            new BookPageInfo
            (
                "Next, a soup to warm the",
                "soul as a sunken treasure",
                "warms the heart. 'Mermaid's",
                "Bouillabaisse,' teeming with",
                "ocean's clams, mussels, and",
                "fennel—the essence of the",
                "sea in a pot, to be sipped",
                "under the moon's watchful eye."
            ),
            new BookPageInfo
            (
                "For the main course, let us",
                "dive deeper. 'Kraken Tentacle",
                "Stew,' a dish bold as the beast",
                "itself. Stew in dark wine until",
                "the meat forgets the kraken's",
                "fury and surrenders its savory",
                "secrets to those brave enough",
                "to tame the leviathan's limbs."
            ),
            new BookPageInfo
            (
                "Onward, for no feast is",
                "complete without 'Trident's",
                "Treasure.' This is the king of",
                "all fish, the one that got away,",
                "grilled upon an open flame.",
                "Its scales crackle with stories",
                "of old, each bite a morsel of",
                "the waves' whispered legends."
            ),
            new BookPageInfo
            (
                "'Sea Sprite Sorbet' we offer",
                "next, a dance of frost and fruit",
                "on one's tongue. Made from",
                "the purest waters, frozen by",
                "elemental breath, blended with",
                "the juice of pineapples carried",
                "by the currents from distant",
                "islands rimmed with volcanoes."
            ),
            new BookPageInfo
            (
                "Beverages must not be",
                "forgotten. 'Abyssal Ambrosia,'",
                "a brew dark as the deepest",
                "caverns yet sweet as the first",
                "dream of land by sea-bound",
                "spirits. Mix with care: for it",
                "carries the might of the ocean's",
                "depths and the caress of its calms."
            ),
            new BookPageInfo
            (
                "To conclude, a toast with",
                "'Neptune's Nectar,' wine so",
                "rare, one bottle is gifted",
                "for every century the sea",
                "embraces the shore. A sip",
                "brings forth visions of",
                "lost cities and time's",
                "passage through ancient reefs."
            ),
            new BookPageInfo
            (
                "Lastly, 'Elixir of the Siren's",
                "Song,' a potion to end the meal",
                "with a symphony of sweetness.",
                "One part blue curaçao for the",
                "hue of the endless blue, one",
                "part cream for the ocean's foam,",
                "stirred not shaken, served with",
                "a spiral shell for sipping slowly."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheSirensCookbook() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Siren's Cookbook");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Siren's Cookbook");
        }

        public TheSirensCookbook(Serial serial) : base(serial)
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
