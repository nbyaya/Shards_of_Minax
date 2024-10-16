using System;
using Server;

namespace Server.Items
{
    public class StoriesOfRighteousness : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Stories of Righteousness", "Sage Ethaniel",
            new BookPageInfo
            (
                "In an age where shadows",
                "creep and the hearts of",
                "men and women are",
                "tested, tales of virtue",
                "shine as beacons of",
                "hope. This tome seeks",
                "to enshrine such tales,",
                "so they may light our"
            ),
            new BookPageInfo
            (
                "way in dark times.",
                "",
                "The first story is of",
                "Sir Jareth, a knight",
                "who faced a dragon",
                "not with sword or",
                "shield, but with a",
                "compassionate heart."
            ),
            new BookPageInfo
            (
                "With a calm voice, he",
                "convinced the beast to",
                "seek a life beyond",
                "ravaging lands, turning",
                "a would-be destroyer",
                "into a guardian of",
                "the realm."
            ),
            new BookPageInfo
            (
                "Our next tale recounts",
                "the journey of Lady",
                "Elara, a paladin who",
                "sought to heal a land",
                "ravaged by curse. With",
                "unyielding spirit, she",
                "traversed cursed bogs",
                "and twisted forests."
            ),
            new BookPageInfo
            (
                "Her purity of purpose",
                "undid the vile hex,",
                "restoring life where",
                "there was none, proving",
                "that righteousness is",
                "the greatest magic of",
                "all."
            ),
            new BookPageInfo
            (
                "Let us not forget the",
                "story of the simple",
                "monk, Brother Alaric,",
                "whose unwavering faith",
                "and humble deeds",
                "toppled a tyrant without",
                "a single drop of blood",
                "shed."
            ),
            new BookPageInfo
            (
                "Through acts of kindness",
                "and whispers of wisdom,",
                "he ignited a revolution",
                "of the heart among the",
                "people, who cast off",
                "their chains with",
                "courage found within."
            ),
            new BookPageInfo
            (
                "Each of these stories",
                "teaches a lesson that",
                "righteousness is not",
                "merely the blade that",
                "defends but the hand",
                "that heals, the word",
                "that uplifts, and the",
                "choice that inspires."
            ),
            new BookPageInfo
            (
                "May these tales guide",
                "you, reader, to live",
                "with honor, to act",
                "with courage, and to",
                "inspire others to deeds",
                "of righteousness.",
                "",
                "Sage Ethaniel"
            ),
			// Continued from previous BookPageInfo
			new BookPageInfo
			(
				"In distant lands, under",
				"the eternal twilight,",
				"lived the humble sage",
				"Orin who, through his",
				"sacrifice, restored the",
				"light of hope to a",
				"people in despair.",
				"With nothing but a"
			),
			new BookPageInfo
			(
				"candle's light, he",
				"ventured into the",
				"darkness to confront",
				"evil. His battle unseen,",
				"but his victory was",
				"felt by all, as dawn",
				"broke for the first time",
				"in a hundred years."
			),
			new BookPageInfo
			(
				"From the annals of",
				"history, the tale of the",
				"Virtuous Thieves stands",
				"a testament to justice.",
				"A band of outlaws who",
				"stole from tyrants to",
				"give back to the downtrod,",
				"balancing scales of"
			),
			new BookPageInfo
			(
				"wealth and power.",
				"Their leader, the",
				"enigmatic Raven,",
				"remains a symbol of",
				"the struggle against",
				"corruption and the",
				"unyielding fight for",
				"equity and fairness."
			),
			new BookPageInfo
			(
				"Among the echoes of",
				"great deeds, one may",
				"also find the quieter",
				"whispers of righteousness.",
				"Alyssa, a healer who",
				"wandered the war-torn",
				"villages, mended the",
				"bodies and spirits of"
			),
			new BookPageInfo
			(
				"all who sought her aid,",
				"never asking for more",
				"than a smile in return.",
				"Her legacy is carried",
				"in every life she",
				"touched, each one a",
				"chapter of a greater",
				"story of benevolence."
			),
			new BookPageInfo
			(
				"Let not the darkness",
				"dissuade you, for each",
				"act of righteousness",
				"kindles a flame that",
				"dispels the shadows.",
				"These stories are",
				"the embers from which",
				"we can ignite a"
			),
			new BookPageInfo
			(
				"bonfire of hope.",
				"Carry these tales close",
				"to your heart, and let",
				"them illuminate your",
				"path. Remember always",
				"that no act of kindness,",
				"no matter how small,",
				"is ever wasted."
			),
			new BookPageInfo
			(
				"For those who walk",
				"the righteous path,",
				"know that your journey",
				"is never a solitary one.",
				"Each step is supported",
				"by the good will of",
				"those who came before,",
				"and will inspire"
			),
			new BookPageInfo
			(
				"those who follow.",
				"",
				"May your story, too,",
				"be one that others",
				"will hold aloft as an",
				"example of virtue,",
				"adding another page",
				"to the eternal book"
			),
			new BookPageInfo
			(
				"of righteousness.",
				"",
				"In a world often",
				"veiled in darkness,",
				"be the beacon that",
				"guides others to",
				"safety, to kindness,",
				"and to the dawn of"
			),
			new BookPageInfo
			(
				"a new age where",
				"righteousness prevails.",
				"",
				"Sage Ethaniel",
				"",
				"May your deeds",
				"echo in eternity."
			),
			// The book ends here. Subsequent BookPageInfos can be intentionally left blank or used for credits, notes, etc.
            new BookPageInfo
            (
                "Sage Ethaniel",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "Let righteousness be",
                "your guide."
            )
        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public StoriesOfRighteousness() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Stories of Righteousness");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Stories of Righteousness");
        }

        public StoriesOfRighteousness(Serial serial) : base(serial)
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
