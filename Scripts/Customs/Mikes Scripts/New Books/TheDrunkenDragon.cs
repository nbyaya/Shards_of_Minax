using System;
using Server;

namespace Server.Items
{
    public class TheDrunkenDragon : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Drunken Dragon", "Bartholomew the Brewed",
            new BookPageInfo
            (
                "Within the foggy depths",
                "of the Green Glades Inn,",
                "tales of a dragon's breath",
                "that could ferment the",
                "barley in the fields passed",
                "from lips to eager ears."
            ),
            new BookPageInfo
            (
                "I am Bartholomew, once a",
                "simple brewer, until I met",
                "the dragon of legend.",
                "This is the tale of how",
                "a dragon's flame sparked",
                "the greatest ale the world",
                "has ever tasted."
            ),
            new BookPageInfo
            (
                "Twas a moonless night",
                "when the dragon swayed",
                "into our village,",
                "belching plumes of",
                "intoxicating smoke and",
                "stumbling o'er cottages."
            ),
            new BookPageInfo
            (
                "Some fled, others watched",
                "in bewilderment as the",
                "beast collapsed beside",
                "my brewery, its snores",
                "shaking the barrels within."
            ),
            new BookPageInfo
            (
                "I dared approach with a",
                "flag of ale, the drink that",
                "guides my life. The",
                "dragon drank with a gusto",
                "that could empty rivers,",
                "and then it wept."
            ),
            new BookPageInfo
            (
                "Drunk on my brew, it cried",
                "for a loneliness only",
                "the vast skies knew.",
                "So we spoke, and drank,",
                "and for a night, the dragon",
                "was just another soul",
                "at the bar."
            ),
            new BookPageInfo
            (
                "As dawn approached with",
                "rosy fingers, the dragon",
                "breathed a grateful flame",
                "into my vats of ale.",
                "Where fire should consume,",
                "it brewed a magic",
                "like no other."
            ),
            new BookPageInfo
            (
                "Now my ale is sought by",
                "kings and beggars alike.",
                "It warms the heart, blurs",
                "the sorrow, and brings",
                "forth laughter from",
                "the weariest of souls."
            ),
            new BookPageInfo
            (
                "But beware, for too much",
                "and you'll see the world",
                "as the dragon did that",
                "night - a merry blur",
                "where the ground and sky",
                "dance in a tipsy waltz."
            ),
            new BookPageInfo
            (
                "So here's to the dragon,",
                "the drunken wisp of legend,",
                "whose fiery tears blessed",
                "me with more than gold",
                "ever could. To friendship",
                "found in the rarest",
                "of places."
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
                "Bartholomew the Brewed",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May your mug never empty",
                "and your heart never heavy."
            ),
            new BookPageInfo
            (
                "Remember, this ale is",
                "not merely a drink, but",
                "a companion through",
                "life's jagged path.",
                "Each sip holds a tale,",
                "a burst of dragon's fire,",
                "a whisper of an age when",
                "magic poured freely."
            ),
            new BookPageInfo
            (
                "Many a night now,",
                "I sit by the fire, with a",
                "mug in hand, musing on",
                "the curious turn my",
                "life has taken. Once",
                "a brewer of simple ales,",
                "now a keeper of legend."
            ),
            new BookPageInfo
            (
                "Adventurers come and go,",
                "each seeking the dragon,",
                "hungry for its secret. But",
                "I've seen what they have",
                "not—the sorrow in its",
                "eyes, the solitude of",
                "immortality."
            ),
            new BookPageInfo
            (
                "So I keep its tale close,",
                "a sacred trust between",
                "kindred spirits. And I",
                "pour another drink, for",
                "those who seek comfort",
                "in my tavern's warm glow."
            ),
            new BookPageInfo
            (
                "The dragon has not",
                "returned since that",
                "fateful night. Some",
                "say it found peace,",
                "others that it sleeps",
                "beneath the hills,",
                "dreaming of brews."
            ),
            new BookPageInfo
            (
                "But on quiet nights,",
                "when the wind whispers",
                "through the eaves, I can",
                "hear a distant roar,",
                "a chuckle in the dark,",
                "reminding me that the",
                "world is full of wonders."
            ),
            new BookPageInfo
            (
                "If you ever chance upon",
                "a dragon, my advice is",
                "simple: offer a drink,",
                "share a tale, and raise",
                "a toast to the unexpected",
                "friends you might find."
            ),
            new BookPageInfo
            (
                "For in ale, there is truth,",
                "and in truth, there is",
                "sometimes a dragon",
                "waiting to be understood,",
                "yearning not for gold or",
                "terror, but for the simple",
                "joy of company."
            ),
            new BookPageInfo
            (
                "Thus ends the tale of the",
                "Drunken Dragon, as told",
                "by Bartholomew the Brewed,",
                "who found fortune and",
                "friendship in the flame of",
                "a dragon's heart."
            ),
            new BookPageInfo
            (
                "May your journeys be",
                "merry, your cups full,",
                "and may you find warmth",
                "in the tales that flow",
                "from the essence of",
                "a dragon's fiery spirit."
            ),
            new BookPageInfo
            (
                "Bartholomew the Brewed",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In honor of the dragon",
                "that brews in the soul",
                "of us all."
            ),
            new BookPageInfo
            (
                // These pages left intentionally blank for notes or future tales.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            )
            // More pages can be added similarly by creating new instances of BookPageInfo.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheDrunkenDragon() : base(false)
        {
            // Set the hue to a random color between 3001 and 6000
            Hue = Utility.RandomMinMax(3001, 6000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Drunken Dragon");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Drunken Dragon");
        }

        public TheDrunkenDragon(Serial serial) : base(serial)
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
