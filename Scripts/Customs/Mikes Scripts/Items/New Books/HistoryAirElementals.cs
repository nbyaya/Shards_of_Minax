using System;
using Server;

namespace Server.Items
{
    public class HistoryAirElementals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History Air Elementals", "Aeromancer",
                new BookPageInfo
                (
                    "The ancient scrolls",
                    "describe Air Elementals",
                    "as spirits of the sky,",
                    "masters of the wind,",
                    "and rulers of the",
                    "atmosphere.",
                    "",
                    "       -Aeromancer"
                ),
                new BookPageInfo
                (
                    "These beings are known",
                    "for their ability to",
                    "control weather,",
                    "unleashing storms and",
                    "torrents upon those",
                    "who oppose them.",
                    "Their presence has",
                    "always been a subject"
                ),
                new BookPageInfo
                (
                    "of awe and fear, as",
                    "these entities embody",
                    "the untamable spirit",
                    "of the air itself.",
                    "According to folklore,",
                    "Air Elementals possess",
                    "wisdom beyond human",
                    "comprehension."
                ),
                new BookPageInfo
                (
                    "They have been revered",
                    "by many cultures as",
                    "messengers of the gods,",
                    "but have also been",
                    "feared for their",
                    "destructive potential.",
                    "In ancient texts, it is",
                    "said they were once"
                ),
                new BookPageInfo
                (
                    "involved in the great",
                    "wars between the",
                    "elemental kingdoms,",
                    "using their command",
                    "over wind to defeat",
                    "adversaries.",
                    "Summoning such",
                    "beings requires"
                ),
                new BookPageInfo
                (
                    "extreme caution as",
                    "they are fickle and",
                    "unpredictable. Their",
                    "alliance can never",
                    "be guaranteed and",
                    "their motives remain",
                    "an enigma."
                ),
				new BookPageInfo
				(
					"In the annals of",
					"ancient history, there",
					"are tales of great",
					"Aeromancers who were",
					"said to have tamed",
					"these enigmatic beings,",
					"employing their powers",
					"for both good and ill."
				),
				new BookPageInfo
				(
					"It is told that the",
					"first Aeromancer,",
					"Valeria, discovered a",
					"ritual to summon and",
					"control Air Elementals.",
					"However, the ritual",
					"required the essence",
					"of a true storm,"
				),
				new BookPageInfo
				(
					"captured in a crystal",
					"vial at the peak of",
					"Mount Typhus during",
					"the storm season.",
					"Thus, only the most",
					"devoted or perhaps,",
					"foolish, would dare",
					"to undergo the quest."
				),
				new BookPageInfo
				(
					"In recent times,",
					"contact with Air",
					"Elementals has become",
					"increasingly rare.",
					"Some scholars speculate",
					"that they have retreated",
					"to hidden realms,",
					"weary of human affairs."
				),
				new BookPageInfo
				(
					"Others theorize that",
					"they have been bound",
					"by dark magics, used",
					"in forbidden rituals",
					"to gain untold power.",
					"The truth remains",
					"as elusive as the",
					"wind itself."
				),
				new BookPageInfo
				(
					"Still, some adventurers",
					"claim to have witnessed",
					"these magnificent",
					"creatures on their",
					"travels, appearing in",
					"the form of tempests",
					"or gentle breezes,",
					"forever captivating"
				),
				new BookPageInfo
				(
					"the human imagination.",
					"And so, the legend of",
					"the Air Elementals",
					"continues to inspire",
					"those who seek the",
					"mysteries of the",
					"elemental world,",
					"always reminding us"
				),
				new BookPageInfo
				(
					"of the unfathomable",
					"depths of the magical",
					"realms that exist",
					"parallel to our own.",
					"May the winds be ever",
					"in your favor, dear",
					"reader."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryAirElementals() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("History Air Elementals");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "History Air Elementals");
        }

        public HistoryAirElementals(Serial serial) : base(serial)
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
