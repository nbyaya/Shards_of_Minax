using System;
using Server;

namespace Server.Items
{
    public class TheShardRealms : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "The Shard Realms", "Aurin",
                new BookPageInfo
                (
                    "The Shard Realms are",
                    "fragments of reality,",
                    "each containing a",
                    "version of our world.",
                    "They were created",
                    "when Mondain's Gem of",
                    "Immortality was shattered",
                    "by a brave hero."
                ),
                new BookPageInfo
                (
                    "Each shard is a world",
                    "unto itself, bound by",
                    "its own set of laws",
                    "and rules. Yet, there",
                    "are scholars who",
                    "believe that all shards",
                    "are interconnected",
                    "through the ether."
                ),
                new BookPageInfo
                (
                    "It is speculated that",
                    "we live in one such",
                    "shard. While we go",
                    "about our daily lives,",
                    "countless variations",
                    "of us exist in parallel",
                    "shards, unaware of",
                    "each other's existence."
                ),
                new BookPageInfo
                (
                    "While some shards may",
                    "be almost identical to",
                    "our own, others could",
                    "be vastly different,",
                    "with unfamiliar lands,",
                    "creatures, and even",
                    "laws of physics."
                ),
                new BookPageInfo
                (
                    "It's an intriguing",
                    "concept that challenges",
                    "our perception of",
                    "reality and destiny.",
                    "Could there be a way",
                    "to travel between",
                    "these shard realms?",
                    "Only time will tell."
                ),
				new BookPageInfo
				(
					"The concept of shard",
					"travel has been a",
					"subject of many",
					"ancient texts and",
					"legends. The existence",
					"of magical gateways",
					"and spells for",
					"trans-shard voyages"
				),
				new BookPageInfo
				(
					"is often debated.",
					"However, most attempts",
					"to confirm these",
					"theories have led to",
					"mysterious",
					"disappearances or",
					"unexplained",
					"phenomena."
				),
				new BookPageInfo
				(
					"It is said that",
					"powerful beings known",
					"as 'Guardians' oversee",
					"each shard. Their",
					"role is to maintain",
					"the balance and",
					"order within their",
					"respective realms."
				),
				new BookPageInfo
				(
					"The Guardians possess",
					"the ability to move",
					"between shards, but",
					"they are bound by",
					"ancient laws not to",
					"interfere directly",
					"with the affairs",
					"of mortals."
				),
				new BookPageInfo
				(
					"Some scholars argue",
					"that the barriers",
					"between shards are",
					"weakening, leading to",
					"anomalies and even",
					"possible merging of",
					"realms. The evidence",
					"for this is inconclusive."
				),
				new BookPageInfo
				(
					"It is a topic that",
					"continues to fascinate",
					"and perplex us. The",
					"possibilities are",
					"endless, and the",
					"implications profound.",
					"Who knows what the",
					"future may hold?"
				),
                new BookPageInfo
                (
                    "Aurin, 3am.",
                    "18.10.2023",
                    "pondering the mysteries",
                    "of the universe"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public TheShardRealms() : base(false)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add("The Shard Realms");
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, "The Shard Realms");
        }

        public TheShardRealms(Serial serial) : base(serial)
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
