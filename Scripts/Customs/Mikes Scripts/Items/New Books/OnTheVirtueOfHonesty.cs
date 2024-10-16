using System;
using Server;

namespace Server.Items
{
    public class OnTheVirtueOfHonesty : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Virtue of Honesty", "A Virtuous Sage",
                new BookPageInfo
                (
                    "Honesty is a",
                    "cornerstone virtue",
                    "among the enlightened",
                    "peoples of our land.",
                    "It is a guiding",
                    "principle for all",
                    "who seek wisdom",
                    "and nobility."
                ),
                new BookPageInfo
                (
                    "The mantra for",
                    "this virtue is",
                    "ZIM LOR FOD.",
                    "Through the use",
                    "of this mantra,",
                    "one can meditate",
                    "and further",
                    "understand the"
                ),
                new BookPageInfo
                (
                    "essence of being",
                    "honest in actions,",
                    "thoughts, and",
                    "words. To live a",
                    "life of honesty is",
                    "to live a life of",
                    "integrity.",
                    ""
                ),
                new BookPageInfo
                (
                    "A dishonest act",
                    "not only tarnishes",
                    "the soul but also",
                    "erodes the trust",
                    "between people,",
                    "communities, and",
                    "entire nations."
                ),
                new BookPageInfo
                (
                    "By committing",
                    "to honesty, we",
                    "preserve the social",
                    "fabric and build a",
                    "stronger, more",
                    "harmonious world."
                ),
				new BookPageInfo
				(
					"Lying might provide",
					"short-term gains,",
					"but the cost is a",
					"long-term erosion of",
					"trust and the sowing",
					"of discord among",
					"friends and allies."
				),
				new BookPageInfo
				(
					"Honesty is not just",
					"about words; it also",
					"pertains to actions.",
					"To be honest is to",
					"act without deceit or",
					"misrepresentation in",
					"all your doings."
				),
				new BookPageInfo
				(
					"It's crucial to",
					"note that honesty",
					"isn't simply about",
					"telling the truth;",
					"it also involves",
					"being genuine and",
					"true to oneself."
				),
				new BookPageInfo
				(
					"The opposite of",
					"honesty is falsehood,",
					"which leads to the",
					"dark paths of deceit,",
					"betrayal, and even",
					"treachery."
				),
				new BookPageInfo
				(
					"Without honesty,",
					"even the most",
					"noble of deeds can",
					"become tainted,",
					"turning virtues",
					"into vices, and",
					"goodwill into",
					"malevolence."
				),
				new BookPageInfo
				(
					"For those who seek",
					"to live by the",
					"virtue of honesty,",
					"remember the mantra,",
					"ZIM LOR FOD, as a",
					"focus in meditation."
				),
				new BookPageInfo
				(
					"By following the",
					"path of honesty,",
					"you strengthen not",
					"just yourself, but",
					"also the world",
					"around you."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnTheVirtueOfHonesty() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On the Virtue of Honesty");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On the Virtue of Honesty");
        }

        public OnTheVirtueOfHonesty(Serial serial) : base(serial)
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
