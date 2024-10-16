using System;
using Server;

namespace Server.Items
{
    public class TheForgottenFortress : BlueBook
    {
        public static readonly BookContent Content = new BookContent
        (
            "The Forgotten Fortress", "Eleanor the Historian",
            new BookPageInfo
            (
                "In the annals of",
                "history, there exist",
                "tales of grand",
                "fortresses, their walls",
                "rising high into the",
                "sky, and their legends",
                "echoing through time.",
                ""
            ),
            new BookPageInfo
            (
                "But amidst the",
                "glory of such",
                "fortresses, there are",
                "those that have been",
                "left behind by the",
                "passage of time. The",
                "Forgotten Fortress is",
                "one such place."
            ),
            new BookPageInfo
            (
                "Nestled deep within a",
                "dense, ancient forest,",
                "the Forgotten Fortress",
                "was once a symbol of",
                "power and prosperity.",
                "Its mighty walls",
                "shielded the",
                "inhabitants from harm."
            ),
            new BookPageInfo
            (
                "Legends say that it",
                "was built by a",
                "mysterious order of",
                "knights who served a",
                "long-forgotten king.",
                "The fortress thrived",
                "for centuries, but as",
                "time passed, it fell"
            ),
            new BookPageInfo
            (
                "into obscurity and",
                "disrepair. The once-",
                "magnificent halls now",
                "echo with silence, and",
                "the grand banners have",
                "long since faded.",
                "",
                "Few dare to venture"
            ),
            new BookPageInfo
            (
                "into the depths of the",
                "forest to discover the",
                "Forgotten Fortress. But",
                "those who do are met",
                "with a haunting",
                "atmosphere, as if the",
                "fortress itself mourns",
                "its lost glory."
            ),
            new BookPageInfo
            (
                "It is said that within",
                "its crumbling walls,",
                "treasures of untold",
                "value may still be",
                "hidden, waiting for",
                "brave adventurers to",
                "unearth them. But",
                "beware, for the forest"
            ),
            new BookPageInfo
            (
                "is not the only",
                "obstacle one must",
                "overcome. The spirits",
                "of the knights who",
                "once guarded the",
                "fortress are said to",
                "still roam its halls,",
                "protecting their"
            ),
            new BookPageInfo
            (
                "ancient home from",
                "intruders.",
                "",
                "The Forgotten Fortress",
                "stands as a reminder",
                "that even the",
                "mightiest of",
                "strongholds can be lost"
            ),
            new BookPageInfo
            (
                "to the sands of time.",
                "Will you be the one to",
                "rediscover its secrets,",
                "or will it remain",
                "forever shrouded in",
                "mystery?",
                "",
                "Only time will tell."
            ),
            new BookPageInfo
            (
                "Eleanor the Historian",
                DateTime.Now.ToString("t"),
                DateTime.Now.ToString("d"),
                "May the forgotten",
                "fortress rise again."
            ),
			new BookPageInfo
			(
				"Chapter II: The",
				"Legends and Lore",
				"",
				"The legends",
				"surrounding the",
				"Forgotten Fortress",
				"are as diverse as the",
				"kingdoms of old."
			),

			new BookPageInfo
			(
				"Some say that the",
				"knights who built the",
				"fortress were blessed",
				"by a powerful",
				"sorceress, and their",
				"armor was said to be",
				"indestructible. Others",
				"believe that the"
			),

			new BookPageInfo
			(
				"fortress was home to",
				"a hidden library of",
				"ancient knowledge and",
				"magical artifacts.",
				"Whatever the truth,",
				"the fortress remains",
				"shrouded in mystery,",
				"its secrets waiting to"
			),

			new BookPageInfo
			(
				"be unraveled by those",
				"courageous enough to",
				"seek them out.",
				"",
				"Chapter III: The",
				"Curse of Abandonment",
				"",
				"As the years passed,"
			),

			new BookPageInfo
			(
				"and the fortress",
				"remained forgotten, a",
				"curse seemed to settle",
				"upon its stones.",
				"Visitors reported",
				"strange occurrences,",
				"such as whispers in",
				"the wind and"
			),

			new BookPageInfo
			(
				"unexplained",
				"apparitions. Some",
				"believe that the spirits",
				"of the knights who once",
				"called the fortress",
				"home still linger,",
				"unable to rest until",
				"their duty is fulfilled."
			),

			new BookPageInfo
			(
				"Others speculate that",
				"the curse is tied to the",
				"fortress's hidden",
				"treasures, and those",
				"who seek to claim",
				"them are met with",
				"misfortune and",
				"tragedy."
			),

			new BookPageInfo
			(
				"Chapter IV: The Quest",
				"for Redemption",
				"",
				"Despite the dangers and",
				"uncertainties, brave",
				"adventurers continue to",
				"journey to the",
				"Forgotten Fortress in",
				"search of its"
			),

			new BookPageInfo
			(
				"forgotten secrets and",
				"the chance to lift the",
				"curse. Some seek",
				"glory, while others are",
				"motivated by",
				"compassion, believing",
				"that the spirits within",
				"deserve peace."
			),

			new BookPageInfo
			(
				"Will they succeed in",
				"restoring the",
				"fortress to its former",
				"glory, or will the",
				"Forgotten Fortress",
				"remain lost to time?",
				"Only time will tell...",
				""
			),

			new BookPageInfo
			(
				"Eleanor the Historian",
				DateTime.Now.ToString("t"),
				DateTime.Now.ToString("d"),
				"May the secrets of the",
				"Forgotten Fortress be",
				"uncovered."
			)

        );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheForgottenFortress() : base(false)
        {
            // Set the hue to a random color between 1 and 3000
            Hue = Utility.RandomMinMax(1, 3000);
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Forgotten Fortress");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Forgotten Fortress");
        }

        public TheForgottenFortress(Serial serial) : base(serial)
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
