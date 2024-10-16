using System;
using Server;

namespace Server.Items
{
    public class OnTheVirtueOfHumility : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Virtue of Humility", "A Wise Sage",
                new BookPageInfo
                (
                    "Humility is a virtue",
                    "that one must hold",
                    "dearly, for it is the",
                    "key to a balanced",
                    "life and character.",
                    "",
                    "Mantra: LUM NAY MUH",
                    "          -A Wise Sage"
                ),
                new BookPageInfo
                (
                    "To embrace humility,",
                    "one must cast aside",
                    "pride and arrogance,",
                    "as these traits",
                    "blind us from the",
                    "truth and the beauty",
                    "inherent in all things.",
                    ""
                ),
                new BookPageInfo
                (
                    "With humility, we",
                    "find it easier to",
                    "listen and learn, to",
                    "understand that we",
                    "are not the center of",
                    "the universe.",
                    "",
                    "Through the mantra,"
                ),
                new BookPageInfo
                (
                    "LUM NAY MUH, one can",
                    "truly internalize the",
                    "meaning of humility,",
                    "and walk the path",
                    "of true virtue.",
                    "",
                    "",
                    ""
                ),
				new BookPageInfo
				(
					"The mantra, LUM NAY",
					"MUH, is not merely a",
					"set of syllables to",
					"recite. It is a focus,",
					"a guideline that can",
					"be applied to every",
					"facet of life.",
					""
				),
				new BookPageInfo
				(
					"In friendships and",
					"relationships, the",
					"virtue of humility",
					"allows us to value",
					"the needs and",
					"feelings of others.",
					"It guides us to",
					"listen rather than"
				),
				new BookPageInfo
				(
					"speak, to support",
					"rather than to lead.",
					"It helps us recognize",
					"that our own views",
					"and desires are but",
					"a single perspective",
					"in a multifaceted",
					"world."
				),
				new BookPageInfo
				(
					"In trials and",
					"tribulations, humility",
					"grants us the wisdom",
					"to acknowledge our",
					"own limitations.",
					"This acceptance",
					"then turns into a",
					"powerful drive"
				),
				new BookPageInfo
				(
					"to improve, to seek",
					"help, and to navigate",
					"life’s challenges",
					"with grace rather",
					"than arrogance.",
					"",
					"",
					""
				),
				new BookPageInfo
				(
					"In times of success,",
					"humility keeps our",
					"accomplishments from",
					"leading us into the",
					"trap of complacency.",
					"It keeps us grounded",
					"and thankful, ever",
					"mindful of the path"
				),
				new BookPageInfo
				(
					"that led us to that",
					"point and respectful",
					"of those who aided",
					"us along the way.",
					"",
					"Through LUM NAY MUH,",
					"let us always be",
					"mindful."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnTheVirtueOfHumility() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On the Virtue of Humility");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On the Virtue of Humility");
        }

        public OnTheVirtueOfHumility(Serial serial) : base(serial)
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
