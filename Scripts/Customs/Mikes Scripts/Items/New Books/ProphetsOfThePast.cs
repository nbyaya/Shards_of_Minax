using System;
using Server;

namespace Server.Items
{
    public class ProphetsOfThePast : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Prophets of the Past", "Oraculum the Wise",
            new BookPageInfo
            (
                "In ages past,",
                "where legends dwell,",
                "prophets spoke",
                "of times unwell.",
                "Oraculum the Wise",
                "am I, keeper of",
                "lore. Within these",
                "pages, secrets galore."
            ),
            new BookPageInfo
            (
                "From Delphic words",
                "wrapped in riddles",
                "to seers' dreams",
                "in nightly scribbles,",
                "the tapestry of fate",
                "is complexly woven.",
                "Their foresight",
                "often grimly golden."
            ),
            new BookPageInfo
            (
                "Read of Eldron who",
                "saw through the veil,",
                "predicting disasters",
                "that made kingdoms",
                "quail. His warnings",
                "saved many a soul,",
                "his visions inscribed",
                "on this ancient scroll."
            ),
            new BookPageInfo
            (
                "There was Vestril,",
                "the Dreamer, whose",
                "slumbers revealed",
                "futures concealed.",
                "Kingdoms paid in",
                "gold and grain,",
                "for a peek at",
                "fortune's fickle vein."
            ),
            new BookPageInfo
            (
                "Not all foresaw doom,",
                "or wealth, or power.",
                "Some, like Lyriana,",
                "predicted the flower",
                "that cured the ills",
                "of a plague-ridden",
                "land, her herbal wisdom",
                "guided by unseen hand."
            ),
            new BookPageInfo
            (
                "Remembered too is",
                "silent Seer Sath,",
                "who spoke just once",
                "and unveiled a path",
                "that led to peace",
                "between warring foes,",
                "his words carried",
                "on destiny's throes."
            ),
            new BookPageInfo
            (
                "Between these covers,",
                "time's curtain draws",
                "aside to illuminate",
                "universal laws.",
                "Each prophet's tale",
                "is a thread in the",
                "loom, a stitch in the",
                "fabric of history's room."
            ),
            new BookPageInfo
            (
                "As Oraculum, I've",
                "gathered these tales",
                "not to boast of foresight",
                "or mystical veils,",
                "but to honor those",
                "who've shown us the",
                "way. May their wisdom",
                "enlighten our play."
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
                "Oraculum the Wise",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the past light",
                "your path to the",
                "future."
            ),
			// Continuing from where we left off, adding more lore and tales.
			new BookPageInfo
			(
				"Tread softly now,",
				"for herein lies",
				"the tale of the",
				"Seeress Elize.",
				"Her vision pierced",
				"through war's thick fog,",
				"foretelling the end",
				"in the heart of a bog."
			),
			new BookPageInfo
			(
				"With her words, a",
				"path was shorn",
				"through the mists",
				"where hope was born.",
				"Armies laid down",
				"their rusted swords,",
				"turning enemies",
				"to united lords."
			),
			new BookPageInfo
			(
				"In the annals of",
				"time, some prophets",
				"stood not for peace,",
				"but for dark omens",
				"misunderstood. The",
				"Crow, as one was named,",
				"spoke of a shadow",
				"that could not be tamed."
			),
			new BookPageInfo
			(
				"He saw a beast,",
				"born of the night,",
				"wielding a darkness",
				"as a blight. Kingdoms",
				"heeded his cryptic",
				"chant, finding the",
				"beast was a tyrant",
				"rampant."
			),
			new BookPageInfo
			(
				"But let us not",
				"dwell on nightmares",
				"foreseen, rather on",
				"the light in-between.",
				"Lyr the Pure sang",
				"of ages of plenty,",
				"of harvests and hearths",
				"never empty."
			),
			new BookPageInfo
			(
				"His words were",
				"a balm to the",
				"weary, his visions",
				"a guide to the",
				"bleary. Where he",
				"walked, crops grew",
				"tall, and through",
				"his eyes, we saw it all."
			),
			new BookPageInfo
			(
				"Prophets come,",
				"and prophets go,",
				"like seasons in",
				"an eternal flow.",
				"Yet their essence",
				"forever remains,",
				"in whispered winds",
				"and in gentle rains."
			),
			new BookPageInfo
			(
				"So here we stand,",
				"by the fireside's glow,",
				"recalling the tales",
				"of seers of old.",
				"Their visions but",
				"memories in time,",
				"bound forever within",
				"this humble rhyme."
			),
			new BookPageInfo
			(
				"When next you gaze",
				"upon a star,",
				"think of the prophets,",
				"both near and far.",
				"For in the stars,",
				"their legacies are told—",
				"the Prophets of the Past,",
				"ever bold."
			),
			new BookPageInfo
			(
				"This tome ends,",
				"but stories never die.",
				"They live on in hearts",
				"and in the sky.",
				"May their prophecies",
				"guide your sail,",
				"through stormy sea",
				"and gusty gale."
			),
			new BookPageInfo
			(
				// These pages left intentionally blank as a symbolic gesture
				// for the unwritten prophecies yet to unfold in history.
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
			),
			new BookPageInfo
			(
				"Oraculum the Wise",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Beneath the veil of time,",
				"may you find truth",
				"in this eternal rhyme."
			)
			// Continue adding as many BookPageInfo objects as necessary to complete your book.

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ProphetsOfThePast() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Prophets of the Past");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Prophets of the Past");
        }

        public ProphetsOfThePast(Serial serial) : base(serial)
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
