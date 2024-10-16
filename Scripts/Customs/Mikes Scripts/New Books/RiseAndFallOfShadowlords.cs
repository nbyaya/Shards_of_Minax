using System;
using Server;

namespace Server.Items
{
    public class RiseAndFallOfShadowlords : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "Rise and Fall of Shadowlords", "Talindor the Chronicler",
            new BookPageInfo
            (
                "Within these pages lies",
                "the tale of the Shadowlords,",
                "whose dark ascent and",
                "inevitable fall have",
                "been etched into the",
                "annals of history. This",
                "chronicle serves as a",
                "testament to their"
            ),
            new BookPageInfo
            (
                "malign influence and",
                "the brave souls who",
                "stood against them.",
                "Born of chaos and",
                "shadow, three entities",
                "rose to power, sowing",
                "fear across the lands:",
                "Astaroth, Faulinei, and"
            ),
            new BookPageInfo
            (
                "Nosfentor. Each",
                "embodied the essence",
                "of tyranny, deceit, and",
                "hatred. Their reign",
                "began in whispers and",
                "shadows, as they bent",
                "the weak-willed to",
                "their nefarious will."
            ),
            new BookPageInfo
            (
                "The Shadowlords",
                "forged armies of",
                "darkness, creatures",
                "corrupted by their",
                "baleful powers. They",
                "laid siege to realms",
                "unprepared for such",
                "malevolence, toppling"
            ),
            new BookPageInfo
            (
                "kingdoms and",
                "enslaving free folk.",
                "Yet in their hubris,",
                "they failed to foresee",
                "the rise of a",
                "resistance, a",
                "coalition of heroes",
                "forged in the fires of"
            ),
            new BookPageInfo
            (
                "their tyranny. The",
                "Shadowlords' fall",
                "began with the",
                "shattering of their",
                "armies, led by",
                "valiant champions",
                "who wielded virtue",
                "as their weapon."
            ),
            new BookPageInfo
            (
                "In the final",
                "confrontation, the",
                "lands were scorched",
                "by the cataclysmic",
                "powers unleashed.",
                "The Shadowlords,",
                "once invincible,",
                "faltered. Heroes"
            ),
            new BookPageInfo
            (
                "from every corner of",
                "the world fought",
                "through despair and",
                "ruin, their hearts",
                "unyielding in the",
                "face of darkness.",
                "At the battle's climax,",
                "the coalition's"
            ),
            new BookPageInfo
            (
                "might triumphed,",
                "and the Shadowlords",
                "were cast down.",
                "Their fall from power",
                "was as swift as their",
                "rise, their names",
                "forever a warning",
                "of the darkness that"
            ),
            new BookPageInfo
            (
                "lurks in the hearts of",
                "men. This chronicle",
                "ends not merely as a",
                "record of the past,",
                "but as a beacon for",
                "the future, reminding",
                "us that vigilance is",
                "the eternal price of"
            ),
            new BookPageInfo
            (
                "freedom.",
                "",
                "Let the Rise and Fall",
                "of the Shadowlords",
                "serve as a guide for",
                "generations to come.",
                "May the shadows",
                "never again claim our"
            ),
            new BookPageInfo
            (
                "world.",
                "",
                "Talindor the Chronicler",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "In memory of the fallen,",
                "and in honor of the",
                "brave."
            ),
            new BookPageInfo
			(
				"The age of peace that",
				"followed was tenuous,",
				"yet it flourished under",
				"the watchful eyes of",
				"the victors. Kingdoms",
				"rebuilt, and the land",
				"healed its scars. The",
				"heroes of the war"
			),
			new BookPageInfo
			(
				"became legends, their",
				"tales told in every",
				"tavern and at every",
				"hearth. Statues were",
				"erected in their honor,",
				"and songs composed",
				"to immortalize their",
				"valor."
			),
			new BookPageInfo
			(
				"But not all welcomed",
				"the light. Whispers",
				"of a surviving",
				"Shadowlord sowed",
				"fear in the shadows.",
				"The wise knew that",
				"evil never truly dies;",
				"it merely waits."
			),
			new BookPageInfo
			(
				"Thus, the vigilant",
				"formed secret orders,",
				"guardians against the",
				"return of darkness.",
				"These silent watchers",
				"became the unseen",
				"shield of the realms,",
				"ever ready."
			),
			new BookPageInfo
			(
				"And as the cycle of",
				"time spirals eternal,",
				"we stand on the cusp",
				"of legend and truth.",
				"The shadow stirs once",
				"more, as if to test",
				"the mettle of this",
				"age's heroes."
			),
			new BookPageInfo
			(
				"Whether the darkness",
				"will rise again or be",
				"quenched in its",
				"infancy, lies in the",
				"hands of the current",
				"generation. May they",
				"find the strength",
				"of their forebears."
			),
			new BookPageInfo
			(
				"This tome serves not",
				"only to remember the",
				"past but to warn and",
				"prepare for the future.",
				"For the Shadowlords,",
				"in their defeat, have",
				"left behind a legacy,",
				"a lesson etched in"
			),
			new BookPageInfo
			(
				"the annals of history:",
				"Complacency is the",
				"ally of darkness.",
				"",
				"May the rise and fall",
				"of the Shadowlords",
				"be a beacon to the",
				"vigilant, now and"
			),
			new BookPageInfo
			(
				"forevermore.",
				"",
				"In the hopes that",
				"light shall forever",
				"pierce the shadows,",
				"I lay down my quill.",
				"The story continues",
				"in the deeds of the"
			),
			new BookPageInfo
			(
				"brave and the hearts",
				"of the just. May they",
				"ever burn bright.",
				"",
				"Talindor the Chronicler",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"Until the end of days."
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
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public RiseAndFallOfShadowlords() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Rise and Fall of the Shadowlords");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Rise and Fall of the Shadowlords");
        }

        public RiseAndFallOfShadowlords(Serial serial) : base(serial)
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
