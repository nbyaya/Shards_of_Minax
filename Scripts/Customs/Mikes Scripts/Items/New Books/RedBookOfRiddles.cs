using System;
using Server;

namespace Server.Items
{
    public class RedBookOfRiddles : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Red Book of Riddles", "Eldor",
                new BookPageInfo
                (
                    "In the lands of",
                    "Sosaria, riddles are",
                    "often told to pass",
                    "time, to confound",
                    "and sometimes to",
                    "hide secrets.",
                    "",
                    "          -Eldor"
                ),
                new BookPageInfo
                (
                    "Riddle 1:",
                    "I speak without a mouth",
                    "and hear without ears.",
                    "I have no body, but",
                    "I come alive with wind.",
                    "What am I?",
                    "",
                    "Answer: An Echo."
                ),
                new BookPageInfo
                (
                    "Riddle 2:",
                    "You see a boat filled",
                    "with people. It has not",
                    "sunk, but when you look",
                    "again you don't see a",
                    "single person on the",
                    "boat. Why?",
                    "",
                    "Answer: All were married."
                ),
				new BookPageInfo
				(
					"Riddle 3:",
					"What comes once in a",
					"minute, twice in a",
					"moment, but never in",
					"a thousand years?",
					"",
					"Answer: The letter 'M'."
				),
				new BookPageInfo
				(
					"Riddle 4:",
					"What has keys but can't",
					"open locks?",
					"",
					"Answer: A piano."
				),
				new BookPageInfo
				(
					"Riddle 5:",
					"I fly without wings.",
					"I cry without eyes.",
					"Whenever I go, darkness",
					"flies. What am I?",
					"",
					"Answer: A cloud."
				),
				new BookPageInfo
				(
					"Riddle 6:",
					"The person who makes it",
					"sells it. The person who",
					"buys it never uses it.",
					"The person who uses it",
					"never knows they're",
					"using it. What is it?",
					"",
					"Answer: A coffin."
				),
				new BookPageInfo
				(
					"Riddle 7:",
					"What comes first and",
					"follows after, ends life,",
					"kills laughter?",
					"",
					"Answer: The letter 'T'."
				),
				new BookPageInfo
				(
					"Riddle 8:",
					"I can be cracked, made,",
					"told, and played. What",
					"am I?",
					"",
					"Answer: A joke."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public RedBookOfRiddles() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Red Book of Riddles");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Red Book of Riddles");
        }

        public RedBookOfRiddles(Serial serial) : base(serial)
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
