using System;
using Server;

namespace Server.Items
{
    public class HistoryOfTheGazers : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of the Gazers", "Astronomer Galen",
                new BookPageInfo
                (
                    "The Gazers are",
                    "mysterious beings",
                    "often linked to the",
                    "ethereal realms.",
                    "Contrary to popular",
                    "belief, they are not",
                    "always malevolent",
                    "entities."
                ),
                new BookPageInfo
                (
                    "Originating from the",
                    "mystical planes, they",
                    "observe the worlds",
                    "through their many",
                    "eyes, taking note of",
                    "the unfolding of",
                    "events and lives."
                ),
                new BookPageInfo
                (
                    "Some scholars believe",
                    "that the Gazers hold",
                    "knowledge of the",
                    "universe, past and",
                    "future. However, their",
                    "enigmatic nature",
                    "makes it hard for",
                    "anyone to decipher"
                ),
                new BookPageInfo
                (
                    "their true intentions.",
                    "It is often said that",
                    "the gaze of a Gazer",
                    "can reveal the true",
                    "essence of a being,",
                    "yet at a cost that",
                    "few are willing to",
                    "pay."
                ),
                new BookPageInfo
                (
                    "The Gazers have been",
                    "both friends and foes",
                    "of the magical",
                    "communities, often",
                    "depending on the",
                    "situation. They hold",
                    "secrets that are yet",
                    "to be discovered."
                ),
				new BookPageInfo
				(
					"Over the centuries,",
					"various texts and",
					"legends have tried",
					"to decipher the",
					"enigmatic nature",
					"of the Gazers. Most",
					"accounts have often",
					"been speculative."
				),
				new BookPageInfo
				(
					"It is said that a",
					"Gazer's eye has the",
					"power to look into",
					"alternate dimensions,",
					"making them highly",
					"sought after by",
					"those who practice",
					"the arcane arts."
				),
				new BookPageInfo
				(
					"The Gazers are also",
					"known for their",
					"uncanny ability to",
					"communicate through",
					"telepathy, though",
					"they seldom choose",
					"to interact with",
					"mortals."
				),
				new BookPageInfo
				(
					"There have been",
					"records of Gazers",
					"aiding powerful",
					"sorcerers in times",
					"of need, while at",
					"other times, leading",
					"them to their doom."
				),
				new BookPageInfo
				(
					"Many consider the",
					"Gazers as guardians",
					"of hidden wisdom,",
					"entrusted with",
					"secrets that could",
					"either save or",
					"destroy worlds."
				),
				new BookPageInfo
				(
					"Caution is advised",
					"when dealing with",
					"these beings, as",
					"they hold no",
					"allegiance to good",
					"or evil, only to",
					"their own enigmatic",
					"agendas."
				),
				new BookPageInfo
				(
					"To this day, the",
					"mystery of the Gazers",
					"continues to both",
					"intrigue and terrify",
					"those who seek to",
					"understand the true",
					"nature of existence."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfTheGazers() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History of the Gazers");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History of the Gazers");
        }

        public HistoryOfTheGazers(Serial serial) : base(serial)
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
