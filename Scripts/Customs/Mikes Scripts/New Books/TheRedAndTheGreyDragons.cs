using System;
using Server;

namespace Server.Items
{
    public class TheRedAndTheGreyDragons : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The red and the grey Dragons", "Dragomir",
                new BookPageInfo
                (
                    "Once upon a time,",
                    "in the lands of",
                    "Sosaria, two dragons",
                    "lived in a secluded",
                    "cave. One was red",
                    "as fire, and the",
                    "other grey as",
                    "misty morning."
                ),
                new BookPageInfo
                (
                    "They were brothers,",
                    "yet their personalities",
                    "differed greatly. The",
                    "red dragon was fierce",
                    "and passionate, always",
                    "seeking battles and",
                    "treasure. The grey",
                    "dragon was wise"
                ),
				new BookPageInfo
				(
					"and thoughtful, often",
					"contemplating the mysteries",
					"of life and the world.",
					"He had a passion for",
					"learning, always eager to",
					"absorb knowledge from",
					"ancient scrolls and",
					"scriptures."
				),
				new BookPageInfo
				(
					"As time passed,",
					"their lives took different",
					"turns. The red dragon",
					"went out on quests,",
					"fighting knights and",
					"hoarding gold. The grey",
					"dragon, meanwhile,",
					"became a guardian"
				),
				new BookPageInfo
				(
					"of wisdom, a sage",
					"who other creatures",
					"came to seek advice",
					"from. Yet, despite",
					"their differences, they",
					"always returned to",
					"their shared cave,",
					"their eternal home."
				),
				new BookPageInfo
				(
					"The red dragon",
					"often felt empty",
					"despite his treasures",
					"and victories. His",
					"brother's quiet",
					"contentment puzzled",
					"him. One day, he",
					"asked, 'Why are you"
				),
				new BookPageInfo
				(
					"so content when you",
					"have no gold or",
					"laurels?'",
					"The grey dragon",
					"replied, 'Because I",
					"have something more",
					"precious: peace of mind",
					"and the love of"
				),
				new BookPageInfo
				(
					"our kin.'",
					"This struck the red",
					"dragon deeply. He",
					"began to understand",
					"that his constant",
					"quests for more",
					"were actually taking",
					"him away from what"
				),
				new BookPageInfo
				(
					"was important.",
					"And so, the red",
					"dragon began to",
					"change. He still",
					"loved the thrill of",
					"adventure but found",
					"new ways to balance",
					"it with wisdom."
				),
				new BookPageInfo
				(
					"As years went by,",
					"the dragons became",
					"symbols for the people",
					"of Sosaria. The red",
					"for courage and valor,",
					"the grey for wisdom",
					"and compassion.",
					"And they lived"
				),
				new BookPageInfo
				(
					"happily, their bond",
					"stronger than ever,",
					"understanding that",
					"life's richest rewards",
					"weren't material but",
					"were found in the",
					"love and wisdom they",
					"shared."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheRedAndTheGreyDragons() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The red and the grey Dragons");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The red and the grey Dragons");
        }

        public TheRedAndTheGreyDragons(Serial serial) : base(serial)
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
