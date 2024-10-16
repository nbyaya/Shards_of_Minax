using System;
using Server;

namespace Server.Items
{
    public class MoansOfMournfulGhosts : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Moans of Mournful Ghosts", "Spectra the Haunted",
            new BookPageInfo
            (
                "In silent whispers and",
                "the chill of night,",
                "spirits speak of",
                "anguish and fright.",
                "I am Spectra, scribe",
                "of the spectral woes,",
                "bearing tales of sorrow",
                "where no mortal goes."
            ),
            new BookPageInfo
            (
                "These pages, a collection",
                "of ethereal verse,",
                "echo the moans of",
                "those cursed to traverse",
                "an eternity in shadow,",
                "lamenting life's cruel jest,",
                "yearning for the peace",
                "of a final rest."
            ),
            new BookPageInfo
            (
                "In haunted halls",
                "and through mists they roam,",
                "spirits bound to",
                "earth, forbidden to home.",
                "Through my quill,",
                "their stories unfold,",
                "of lives once warm,",
                "now eternally cold."
            ),
            new BookPageInfo
            (
                "Hear the widow's",
                "keen, the lost child's plea,",
                "each a solemn verse",
                "in this requiem's decree.",
                "The knight who fell",
                "in a battle, unseen,",
                "now marches alone",
                "where he once had been."
            ),
            new BookPageInfo
            (
                "A maiden who waits",
                "by the shore, evermore,",
                "for a lover lost",
                "to the ocean's roar.",
                "Her wails blend with",
                "waves as they crest,",
                "in the ceaseless hope",
                "of a heart's request."
            ),
            new BookPageInfo
            (
                "Each apparition, a tale",
                "of regret and despair,",
                "in the night's cold embrace",
                "with none to care.",
                "Through my words,",
                "their presences bemoaned,",
                "in the stillness,",
                "their sorrows intoned."
            ),
            new BookPageInfo
            (
                "A tome of sorrow,",
                "a compendium of dread,",
                "to remember the voices",
                "of those long dead.",
                "May you find solace,",
                "in your heart to enfold,",
                "the moans of mournful ghosts",
                "this book does hold."
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
                "Spectra the Haunted",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the silence be kind,",
                "and your spirit never join",
                "the moans within these pages."
            ),
            // Continuing from the existing BookPageInfo entries...
            new BookPageInfo
            (
                "Beneath the moon's",
                "pallid light, the ghost",
                "of a scholar roams,",
                "forever seeking the",
                "truths he can't compose.",
                "His quill never dry,",
                "his inkwell always full,",
                "yet no words he writes,"
            ),
            new BookPageInfo
            (
                "his story to tell.",
                "His moan is soft,",
                "a whisper of yearn,",
                "for a final chapter",
                "he shall never learn.",
                "",
                "There's the spirit of a",
                "jester, mirthless and",
                "forlorn, jesting in silence,"
            ),
            new BookPageInfo
            (
                "his laughter worn.",
                "Once he played for",
                "kings and queens,",
                "now he performs",
                "for shadows and fiends.",
                "His spectral jokes,",
                "for none to hear,",
                "forever unlaughed at,",
                "save by the mirror."
            ),
            new BookPageInfo
            (
                "A spectral ship sails,",
                "its crew all phantoms",
                "lost, seeking the home",
                "ports their lives had cost.",
                "Their moans are the",
                "gales, their tears the",
                "sea, as they navigate",
                "the eternity."
            ),
            new BookPageInfo
            (
                "In the castle's ruins,",
                "amongst rubble and rust,",
                "a princess's spirit",
                "wanders, driven by trust.",
                "Her moan, a lament",
                "of betrayal so deep,",
                "yearning for the love",
                "that was hers to keep."
            ),
            new BookPageInfo
            (
                "Hark! The phantom steed,",
                "hooves silent on the",
                "grass, gallops through",
                "valleys, his mane like",
                "molten glass.",
                "His riderless back,",
                "a saddle still strapped,",
                "moans with the echoes",
                "of a final war act."
            ),
            new BookPageInfo
            (
                "And so this tome grows,",
                "with every ghost's tale,",
                "a library of the",
                "afterlife's travail.",
                "Their moans I've inscribed,",
                "in these pages confined,",
                "so their echoes might",
                "peace, in your world, find."
            ),
            new BookPageInfo
            (
                "As you turn these pages",
                "and read of their plight,",
                "remember the moans",
                "of these souls in the night.",
                "May their tales of woe",
                "guide you in life,",
                "to stray not towards",
                "eternal strife."
            ),
            new BookPageInfo
            (
                // Additional pages left intentionally blank for expansion or player's creativity.
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
            ),
            new BookPageInfo
            (
                // Last page with the author's signature and timestamp
                "Spectra the Haunted",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In the company of ghosts,",
                "may you find life's worth",
                "and live so as to never",
                "haunt this earth."
            )
            // Ensure to adjust the page numbers if you are adding more content in between.
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public MoansOfMournfulGhosts() : base(false)
        {
            // Set the hue to a spectral color
            Hue = 0x2001;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Moans of Mournful Ghosts");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Moans of Mournful Ghosts");
        }

        public MoansOfMournfulGhosts(Serial serial) : base(serial)
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
