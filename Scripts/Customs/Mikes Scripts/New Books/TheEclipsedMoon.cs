using System;
using Server;

namespace Server.Items
{
    public class TheEclipsedMoon : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Eclipsed Moon", "Lorelei Nightshade",
            new BookPageInfo
            (
                "In the darkest of nights,",
                "when the world is cloaked",
                "in shadows and secrets,",
                "there exists a legend of",
                "an enigmatic phenomenon",
                "known as 'The Eclipsed",
                "Moon.' Few have witnessed",
                "its eerie beauty."
            ),
            new BookPageInfo
            (
                "The Eclipsed Moon is not",
                "like any other celestial",
                "event. It occurs when",
                "the moon is veiled by a",
                "mystical darkness that",
                "engulfs the night sky.",
                "Some say it's an omen,",
                "while others believe it"
            ),
            new BookPageInfo
            (
                "to be a doorway to",
                "otherworldly realms.",
                "Legends speak of the",
                "moon's reflection being",
                "cast onto a hidden",
                "mirror, revealing secrets",
                "and forgotten dreams.",
                "It is a time of mystery"
            ),
            new BookPageInfo
            (
                "and wonder, when",
                "adventurers embark on",
                "quests to unlock its",
                "secrets, and scholars",
                "study its mystical",
                "properties. During The",
                "Eclipsed Moon, the world",
                "seems touched by magic."
            ),
            new BookPageInfo
            (
                "The Eclipsed Moon is said",
                "to bring forth strange",
                "creatures and mystical",
                "artifacts. Some believe it",
                "to be a bridge between",
                "worlds, where the line",
                "between reality and",
                "fantasy blurs."
            ),
            new BookPageInfo
            (
                "As night falls and the",
                "moon is shrouded in",
                "darkness, adventurers",
                "gather to witness The",
                "Eclipsed Moon. It is a",
                "time of awe and",
                "uncertainty, where",
                "anything can happen."
            ),
            new BookPageInfo
            (
                "The next time you find",
                "yourself under The",
                "Eclipsed Moon, remember",
                "that the night holds",
                "countless secrets and",
                "mysteries. Embrace the",
                "magic of the moment and",
                "let your imagination soar."
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
                "Lorelei Nightshade",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May The Eclipsed Moon",
                "illuminate your dreams."
            ),
			new BookPageInfo
			(
				"Legends tell of brave",
				"explorers who ventured",
				"beyond the shroud of The",
				"Eclipsed Moon, seeking",
				"answers to the mysteries",
				"that lie hidden in the",
				"night. Some returned with",
				"tales of wondrous visions."
			),
			new BookPageInfo
			(
				"One such tale speaks of a",
				"knight who saw a vision of",
				"a long-lost love, calling",
				"out to him from the",
				"ethereal mists. He believed",
				"it to be a message from",
				"beyond, guiding him on a",
				"quest to reunite with her."
			),
			new BookPageInfo
			(
				"Others claim to have",
				"encountered spectral",
				"guardians who protect the",
				"secrets of The Eclipsed",
				"Moon. These guardians",
				"test the courage and",
				"resolve of those who",
				"dare to approach them."
			),
			new BookPageInfo
			(
				"The Eclipsed Moon has",
				"inspired poets and artists",
				"throughout the ages. Its",
				"haunting beauty is often",
				"captured in paintings and",
				"verse, each attempting to",
				"convey the essence of this",
				"otherworldly event."
			),
			new BookPageInfo
			(
				"Scholars debate whether The",
				"Eclipsed Moon has a",
				"predictable pattern or if",
				"it appears at random",
				"intervals. Some believe",
				"it is tied to the phases",
				"of celestial bodies beyond",
				"our understanding."
			),
			new BookPageInfo
			(
				"One thing is certain: The",
				"Eclipsed Moon is a time of",
				"renewal and transformation.",
				"It reminds us that the",
				"world is filled with",
				"wonders waiting to be",
				"discovered, and that",
				"magic is ever-present."
			),
			new BookPageInfo
			(
				"As we stand beneath The",
				"Eclipsed Moon's eerie",
				"glow, let us embrace the",
				"unknown and allow our",
				"spirits to wander. For in",
				"the darkness, there is",
				"light, and in the",
				"mystery, there is magic."
			),
			new BookPageInfo
			(
				"May The Eclipsed Moon",
				"continue to inspire",
				"adventurers and dreamers",
				"for generations to come.",
				"May it be a beacon of",
				"hope and wonder in the",
				"midst of our world's",
				"endless night."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheEclipsedMoon() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Eclipsed Moon");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Eclipsed Moon");
        }

        public TheEclipsedMoon(Serial serial) : base(serial)
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
