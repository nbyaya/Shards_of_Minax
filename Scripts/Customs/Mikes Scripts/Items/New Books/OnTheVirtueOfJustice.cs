using System;
using Server;

namespace Server.Items
{
    public class OnTheVirtueOfJustice : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Virtue of Justice", "A Wise Sage",
                new BookPageInfo
                (
                    "Justice is the virtue",
                    "that balances the",
                    "scales. It is the",
                    "underpinning of all",
                    "that is fair and",
                    "righteous. The mantra",
                    "for Justice is",
                    "BEH VEX LOR."
                ),
                new BookPageInfo
                (
                    "Justice involves not",
                    "only the proper",
                    "punishment of the",
                    "wicked, but also",
                    "the reward for the",
                    "virtuous. It upholds",
                    "the social contract",
                    "that allows for"
                ),
                new BookPageInfo
                (
                    "peace and prosperity",
                    "within the realms.",
                    "The imbalance of",
                    "justice leads to",
                    "corruption, strife,",
                    "and the weakening of",
                    "the community.",
                    ""
                ),
                new BookPageInfo
                (
                    "Those who seek",
                    "justice should",
                    "constantly chant the",
                    "mantra BEH VEX LOR",
                    "to align their mind",
                    "and soul with the",
                    "principles of fair",
                    "judgment."
                ),
                new BookPageInfo
                (
                    "Remember, true",
                    "justice weighs both",
                    "sides of a scale,",
                    "favoring neither.",
                    "It is blind to bias,",
                    "yet keenly attuned",
                    "to the truth.",
                    ""
                ),
				new BookPageInfo
				(
					"Justice in society",
					"is maintained through",
					"laws and governance.",
					"However, it is",
					"important to note",
					"that laws themselves",
					"do not guarantee",
					"justice."
				),
				new BookPageInfo
				(
					"Laws must be",
					"interpreted and",
					"applied fairly. When",
					"laws are misapplied,",
					"either intentionally",
					"or unintentionally,",
					"injustice festers",
					"within society."
				),
				new BookPageInfo
				(
					"Similarly, the",
					"virtue of justice",
					"extends to everyday",
					"interactions. One",
					"should strive to",
					"treat others with",
					"equity and fairness,",
					"for what affects one"
				),
				new BookPageInfo
				(
					"individual or group",
					"can send ripples",
					"throughout the",
					"community. It is",
					"through collective",
					"action that justice",
					"is truly realized.",
					""
				),
				new BookPageInfo
				(
					"The mantra BEH VEX",
					"LOR is a spiritual",
					"tool to focus the",
					"mind and calibrate",
					"the soul. Chanting",
					"this mantra reminds",
					"us to uphold justice",
					"in all our deeds."
				),
				new BookPageInfo
				(
					"It is said that",
					"those who champion",
					"justice find favor",
					"with the forces of",
					"good and light,",
					"and are shielded",
					"from the darkness",
					"that seeks to tip"
				),
				new BookPageInfo
				(
					"the scales. As you",
					"venture forth in",
					"your journeys, let",
					"the virtue of",
					"justice be your",
					"guide, and may the",
					"mantra BEH VEX LOR",
					"always be in your"
				),
				new BookPageInfo
				(
					"heart and on your",
					"lips.",
					"",
					"May justice ever",
					"guide your path,",
					"and may your path",
					"ever be just.",
					"",
					"            - End"
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnTheVirtueOfJustice() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On the Virtue of Justice");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On the Virtue of Justice");
        }

        public OnTheVirtueOfJustice(Serial serial) : base(serial)
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
