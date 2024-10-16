using System;
using Server;

namespace Server.Items
{
    public class OgrePoetryBook : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Ogre Poetry", "Ogg the Poet",
                new BookPageInfo
                (
                    "Ogg not just strong,",
                    "Ogg have word",
                    "that live long.",
                    "Ogg write poem",
                    "here for you,",
                    "Read them, please,",
                    "Or Ogg feel blue."
                ),
                new BookPageInfo
                (
                    "Moon so high,",
                    "Ogg wonder why.",
                    "Sky so vast,",
                    "Ogg not fast."
                ),
                new BookPageInfo
                (
                    "Rock and stone,",
                    "Ogg's true home.",
                    "Fire bright,",
                    "Ogg feel right."
                ),
                new BookPageInfo
                (
                    "Ogg end here,",
                    "Go drink beer.",
                    "Hope you like",
                    "Ogg's word hike."
                ),
				new BookPageInfo
				(
					"Tree so tall,",
					"Ogg not small.",
					"Birds in sky,",
					"Ogg wonder why."
				),
				new BookPageInfo
				(
					"Fish in stream,",
					"Ogg not dream.",
					"Water flow,",
					"Where it go?"
				),
				new BookPageInfo
				(
					"Sun so hot,",
					"Ogg like lot.",
					"Sun go down,",
					"Ogg wear frown."
				),
				new BookPageInfo
				(
					"Wind do blow,",
					"Fast then slow.",
					"Ogg can't see,",
					"How it be."
				),
				new BookPageInfo
				(
					"Rain so wet,",
					"Ogg not fret.",
					"Mud is fun,",
					"Ogg jump, run!"
				),
				new BookPageInfo
				(
					"Stars at night,",
					"Ogg's delight.",
					"Twinkle far,",
					"What they are?"
				),
				new BookPageInfo
				(
					"Snow so cold,",
					"Ogg feel old.",
					"White and bright,",
					"Turn to night."
				),
				new BookPageInfo
				(
					"Ogg say bye,",
					"No more cry.",
					"Hope you like,",
					"Ogg's word hike."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OgrePoetryBook() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Ogre Poetry");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Ogre Poetry");
        }

        public OgrePoetryBook(Serial serial) : base(serial)
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
