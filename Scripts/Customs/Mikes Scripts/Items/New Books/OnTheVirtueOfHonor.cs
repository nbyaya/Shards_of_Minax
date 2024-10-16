using System;
using Server;

namespace Server.Items
{
    public class OnTheVirtueOfHonor : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Virtue of Honor", "Sage Elara",
                new BookPageInfo
                (
                    "Honor, as a virtue,",
                    "represents more than",
                    "mere honesty or",
                    "courage. It embodies",
                    "the qualities of",
                    "compassion, integrity,",
                    "and the commitment",
                    "to act righteously."
                ),
                new BookPageInfo
                (
                    "The mantra for this",
                    "virtue is 'SUMM KIR",
                    "RAH'. The words are",
                    "deeply rooted in the",
                    "ethical practices and",
                    "are often meditated",
                    "upon to gain moral",
                    "clarity."
                ),
                new BookPageInfo
                (
                    "Honor is the virtue",
                    "that lights the way",
                    "for both Mobiles and",
                    "BaseCreatures, helping",
                    "them find their path",
                    "in the treacherous",
                    "journeys of life.",
                    "It is a guiding star."
                ),
                new BookPageInfo
                (
                    "Those who walk the",
                    "path of Honor find",
                    "that their actions",
                    "earn them not only",
                    "respect but also a",
                    "sense of self-",
                    "fulfillment, unlike",
                    "any other."
                ),
				new BookPageInfo
				(
					"Honor does not demand",
					"grand gestures or",
					"heroic feats. It lives",
					"in the daily choices",
					"we make and the",
					"commitments we keep.",
					"It defines who we are",
					"as individuals."
				),
				new BookPageInfo
				(
					"In the realm of",
					"combat, Honor guides",
					"warriors to engage",
					"fairly with their foes.",
					"It is not just the",
					"victory, but how the",
					"victory is achieved,",
					"that defines a hero."
				),
				new BookPageInfo
				(
					"The virtue of Honor",
					"extends to how we",
					"treat all BaseCreatures",
					"and Mobiles. Even in",
					"conquest or defense,",
					"there is a right and",
					"wrong way to act, a",
					"path that respects all."
				),
				new BookPageInfo
				(
					"It is a code that",
					"asks us to consider",
					"our actions in the",
					"context of a broader",
					"community. To act",
					"honorably is to bring",
					"light to the world,",
					"to be a beacon for all."
				),
				new BookPageInfo
				(
					"Honor is reflected",
					"in the items we craft",
					"and trade. A fair deal",
					"and a well-made",
					"product bring more",
					"than mere profit; they",
					"bring respect and a",
					"good reputation."
				),
				new BookPageInfo
				(
					"Finally, let's not",
					"forget that Honor",
					"is a journey, not a",
					"destination. One does",
					"not simply 'achieve'",
					"Honor and then rest",
					"on one's laurels.",
					"It is a lifelong quest."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public OnTheVirtueOfHonor() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("On the Virtue of Honor");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "On the Virtue of Honor");
        }

        public OnTheVirtueOfHonor(Serial serial) : base(serial)
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
