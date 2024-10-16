using System;
using Server;

namespace Server.Items
{
    public class SilentSongsOfTheStoneGargoyles : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Silent Songs of the Stone Gargoyles", "Galthor the Silent",
            new BookPageInfo
            (
                "Perched high atop",
                "the cathedral spires,",
                "the stone gargoyles",
                "watch in silence.",
                "Their songs unheard,",
                "their tales untold,",
                "they stand as",
                "guardians of secrets."
            ),
            new BookPageInfo
            (
                "Eons passed since",
                "their creation, a",
                "time when magic",
                "was young. Crafted",
                "by hands divine,",
                "they were set to",
                "ward off the evil,",
                "gaze set in stone."
            ),
            new BookPageInfo
            (
                "Yet, within each",
                "statuesque form,",
                "a silent song does",
                "resonate. Whispers",
                "of ancient wisdom,",
                "legends of old,",
                "battles fought and",
                "stories bold."
            ),
            new BookPageInfo
            (
                "Beneath the moonlit",
                "skies, they stir",
                "as life briefly",
                "flutters within.",
                "Their eyes aglow",
                "with ethereal light,",
                "witnesses to the",
                "plight of night."
            ),
            new BookPageInfo
            (
                "At dawn, they freeze,",
                "as the light creeps,",
                "their vigilance",
                "unyielding. No",
                "sorrow shown for",
                "their eternal stance,",
                "in their silent songs,",
                "a melancholic dance."
            ),
            new BookPageInfo
            (
                "These sentinels of",
                "stone, though frozen",
                "and cold, bear a",
                "sacred duty. An",
                "unseen war, with",
                "shadows they fight,",
                "until the morning's",
                "light."
            ),
            new BookPageInfo
            (
                "Let not their",
                "stillness fool thee,",
                "for within each",
                "gargoyle's heart,",
                "lies the spirit of",
                "a warrior, silent",
                "and stark, with",
                "songs of valor,"
            ),
            new BookPageInfo
            (
                "etched in stone.",
                "Their silent vigil,",
                "a testament, a",
                "legacy enshrined",
                "in the fabric of",
                "the cathedral's",
                "bones. Silent songs",
                "of stone."
            ),
            new BookPageInfo
            (
                "In the end, when",
                "twilight dims and",
                "stars align, hear",
                "their silent songs,",
                "feel their gaze.",
                "Stone gargoyles, in",
                "their silence, hold",
                "the night at bay."
            ),
            new BookPageInfo
            (
                "So here I inscribe,",
                "Galthor the Silent,",
                "tales of the stone",
                "sentinels bold.",
                "In hopes that one",
                "day their songs",
                "will be sung,",
                "their silence broken."
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
                "Galthor the Silent",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the stone sing,",
                "and secrets, it does",
                "hold, become whispers",
                "in the wind."
            ),
            // Continuing from previous pages
            new BookPageInfo
            (
                "On moonless nights,",
                "when darkness reigns,",
                "the gargoyles' songs,",
                "grow fervent. Their",
                "wings though clipped,",
                "their spirits soar",
                "across the shadowed",
                "lands, their stone"
            ),
            new BookPageInfo
            (
                "eyes piercing through",
                "the veil of night.",
                "Each gargoyle's tale",
                "is unique, etched by",
                "the winds of time,",
                "sung to the heavens",
                "in a silent chime,",
                "a symphony unheard."
            ),
            new BookPageInfo
            (
                "There's the warrior,",
                "who in silence bears",
                "the scars of battles",
                "from realms beyond.",
                "The guardian, whose",
                "steadfast gaze",
                "shields the innocent",
                "from spectral hounds."
            ),
            new BookPageInfo
            (
                "The scholar, in whose",
                "stone-crafted visage",
                "lies a mind that",
                "forever seeks the",
                "truths of the arcane,",
                "the mysteries of the",
                "stars, and the nature",
                "of the eternal."
            ),
            new BookPageInfo
            (
                "The maiden, with",
                "wings gracefully",
                "arched, sings of love",
                "lost and the hope",
                "of its return, her",
                "melancholic melody",
                "cradles the broken-",
                "hearted."
            ),
            new BookPageInfo
            (
                "Then there's the jester,",
                "whose mirthful smirk",
                "mocks the folly of",
                "man, a silent laugh",
                "at the passing parade",
                "of human endeavors,",
                "whispering the truth",
                "in jest."
            ),
            new BookPageInfo
            (
                "The seer, eyes forever",
                "fixed upon the distant",
                "horizon, foresees the",
                "fate of kingdoms,",
                "the rise and fall of",
                "empires, and the",
                "silent turning of",
                "the world's wheel."
            ),
            new BookPageInfo
            (
                "In this silent choir,",
                "each stone gargoyle",
                "holds a note in the",
                "great canticle of the",
                "earth, a song of",
                "stone that resounds",
                "in the heart of the",
                "attuned."
            ),
            new BookPageInfo
            (
                "But beware, for",
                "when the song falters,",
                "and a gargoyle's gaze",
                "shifts away, a tale",
                "tells of darkness",
                "freed from its",
                "cage, preying upon",
                "souls astray."
            ),
            new BookPageInfo
            (
                "As Galthor the Silent,",
                "I've journeyed to",
                "hear these silent",
                "songs. With quill and",
                "ink, I give voice to",
                "their wordless epics,",
                "for within silence",
                "lies profound solace."
            ),
            new BookPageInfo
            (
                "So take this tome,",
                "reader of mysteries,",
                "and listen with your",
                "heart. For the Silent",
                "Songs of the Stone",
                "Gargoyles may yet",
                "reveal the universe's",
                "hidden art."
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
                "Galthor the Silent",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Hearken to the stone,",
                "heed its silent song,",
                "for in the quiet,",
                "truths are revealed."
            )

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public SilentSongsOfTheStoneGargoyles() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Silent Songs of the Stone Gargoyles");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Silent Songs of the Stone Gargoyles");
        }

        public SilentSongsOfTheStoneGargoyles(Serial serial) : base(serial)
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
