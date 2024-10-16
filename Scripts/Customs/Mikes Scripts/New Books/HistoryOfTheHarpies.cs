using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheHarpies : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Harpies", "Elara",
                new BookPageInfo
                (
                    "The Harpies, often",
                    "mistaken as malevolent",
                    "creatures, have a",
                    "rich history that dates",
                    "back to the earliest",
                    "days of the realm.",
                    "",
                    "           -Elara"
                ),
                new BookPageInfo
                (
                    "It's crucial to",
                    "understand that Harpies",
                    "are intelligent beings",
                    "with a society built",
                    "on strict hierarchies.",
                    "They are often found",
                    "in the mountainous",
                    "regions."
                ),
                new BookPageInfo
                (
                    "The common belief is",
                    "that Harpies are the",
                    "offspring of powerful",
                    "wind spirits and",
                    "earthly birds, forming",
                    "a unique hybrid that",
                    "possesses qualities",
                    "of both."
                ),
                new BookPageInfo
                (
                    "In contrast to popular",
                    "myths, Harpies are not",
                    "always aggressive.",
                    "They can be quite",
                    "diplomatic when their",
                    "territory is not",
                    "threatened. They are",
                    "also known for their"
                ),
                new BookPageInfo
                (
                    "aerial agility and",
                    "sharp talons, making",
                    "them formidable foes",
                    "when provoked.",
                    "Despite their fierce",
                    "appearance, Harpies",
                    "have a love for",
                    "music and arts."
                ),
                new BookPageInfo
                (
                    "In conclusion, the",
                    "Harpies are far more",
                    "complex than often",
                    "portrayed in tales.",
                    "Understanding them",
                    "could lead to greater",
                    "knowledge of the",
                    "world around us."
                ),
				new BookPageInfo
				(
					"One intriguing aspect",
					"of Harpy culture is",
					"their language. Composed",
					"of whistles, shrieks,",
					"and harmonious tones,",
					"it allows for complex",
					"communication among",
					"them."
				),
				new BookPageInfo
				(
					"The Harpies also have",
					"a strong sense of",
					"community, with each",
					"clan led by a Matriarch.",
					"The Matriarch is the",
					"oldest and wisest of",
					"the Harpies and holds",
					"great influence."
				),
				new BookPageInfo
				(
					"Harpies have been",
					"known to forge",
					"alliances with other",
					"creatures and even",
					"humanoids. Such",
					"alliances are often",
					"formed to ward off",
					"common enemies."
				),
				new BookPageInfo
				(
					"Their diet primarily",
					"consists of fish and",
					"small mammals, which",
					"they catch with their",
					"sharp talons. The",
					"clans usually hunt in",
					"groups to ensure",
					"sufficient food."
				),
				new BookPageInfo
				(
					"Harpies possess a",
					"unique magical affinity",
					"towards wind and air.",
					"This affinity grants",
					"them superior control",
					"over their flights,",
					"allowing for acrobatic",
					"maneuvers."
				),
				new BookPageInfo
				(
					"In summary, Harpies",
					"are far more than the",
					"malevolent creatures",
					"of myths and legends.",
					"They are a complex",
					"and advanced society,",
					"deserving of our",
					"understanding."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheHarpies() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Harpies");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Harpies");
        }

        public HistoryOfTheHarpies(Serial serial) : base(serial)
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
