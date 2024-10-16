using System;
using Server;

namespace Server.Items
{
    public class AnatomyOfKrakens : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Anatomy of Krakens", "Captain Morgan",
                new BookPageInfo
                (
                    "Krakens are mystical",
                    "creatures of the",
                    "ocean's depths.",
                    "Understanding their",
                    "anatomy can provide",
                    "insight into how to",
                    "combat these",
                    "menaces."
                ),
                new BookPageInfo
                (
                    "A kraken has several",
                    "tentacles, each",
                    "armed with suction",
                    "cups and sometimes",
                    "spikes. These are",
                    "used for both",
                    "attack and defense.",
                    "It also has a beak."
                ),
                new BookPageInfo
                (
                    "It is believed that",
                    "krakens communicate",
                    "via low-frequency",
                    "sound waves. Their",
                    "brain is developed",
                    "enough to strategize",
                    "and trap prey.",
                    "They are carnivores."
                ),
                new BookPageInfo
                (
                    "Some say that krakens",
                    "are magical creatures",
                    "with abilities like",
                    "inking and even",
                    "elemental attacks.",
                    "This has yet to be",
                    "confirmed.",
                    "Proceed with caution."
                ),
				new BookPageInfo
				(
					"Krakens have been",
					"spotted in various",
					"oceans and even in",
					"large rivers. They",
					"are territorial and",
					"often claim an",
					"entire underwater",
					"cave system."
				),
				new BookPageInfo
				(
					"Their skin is highly",
					"resilient and offers",
					"them protection",
					"against most physical",
					"attacks. It is also",
					"rumored to be",
					"impervious to minor",
					"spells."
				),
				new BookPageInfo
				(
					"Krakens eat fish,",
					"whales, and even",
					"ships. They are known",
					"to attack fishing",
					"vessels and are",
					"the bane of many",
					"a sailor's tale."
				),
				new BookPageInfo
				(
					"Some ancient scrolls",
					"suggest that krakens",
					"were once summoned",
					"by sea witches.",
					"However, no",
					"conclusive evidence",
					"supports this.",
					"Proceed with caution."
				),
				new BookPageInfo
				(
					"If you happen to",
					"encounter a kraken,",
					"aim for the eyes or",
					"beak. These are their",
					"most vulnerable",
					"points, according to",
					"writings from those",
					"who survived."
				),
				new BookPageInfo
				(
					"Krakens produce ink",
					"as a defense",
					"mechanism. This ink",
					"is highly corrosive",
					"and can impair vision.",
					"Avoid it at all costs."
				),
				new BookPageInfo
				(
					"Though formidable,",
					"krakens are not",
					"invincible. With the",
					"right strategy and",
					"firepower, they can",
					"be defeated. Be",
					"prepared for a long",
					"and arduous battle."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public AnatomyOfKrakens() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("Anatomy of Krakens");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "Anatomy of Krakens");
        }

        public AnatomyOfKrakens(Serial serial) : base(serial)
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
