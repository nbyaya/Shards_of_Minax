using System;
using Server;

namespace Server.Items
{
    public class HistoryOfEarthElementals : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "History of Earth Elementals", "Adept Mage Kael",
                new BookPageInfo
                (
                    "Earth Elementals have",
                    "been a topic of great",
                    "interest among mages",
                    "and scholars for",
                    "centuries. This text",
                    "aims to shed light",
                    "on their mysterious",
                    "origins."
                ),
                new BookPageInfo
                (
                    "The earth we walk",
                    "upon is full of life,",
                    "and Earth Elementals",
                    "are a manifestation",
                    "of this vibrant",
                    "energy. They are",
                    "creatures born from",
                    "the very soil."
                ),
                new BookPageInfo
                (
                    "Their temperament",
                    "is as varied as the",
                    "rocks that form them.",
                    "Some are docile,",
                    "while others are",
                    "aggressive. They can",
                    "be summoned by",
                    "powerful mages."
                ),
                new BookPageInfo
                (
                    "It's important to",
                    "approach them with",
                    "caution. They are",
                    "highly resistant to",
                    "physical attacks, and",
                    "some can even cast",
                    "earth-related spells.",
                    "They are truly a"
                ),
                new BookPageInfo
                (
                    "force to be reckoned",
                    "with.",
                    "",
                    "            - Adept Mage Kael"
                ),
				new BookPageInfo
				(
					"Earth Elementals are",
					"often found in areas",
					"rich in minerals and",
					"natural resources.",
					"Caves, mountainous",
					"regions, and deep",
					"forests are their",
					"common habitats."
				),
				new BookPageInfo
				(
					"These creatures have",
					"been known to collect",
					"precious gems and",
					"metals, storing them",
					"in their earthen",
					"bodies. Mining",
					"operations often",
					"encounter them."
				),
				new BookPageInfo
				(
					"Contrary to popular",
					"belief, Earth",
					"Elementals do have a",
					"sense of social",
					"structure. They are",
					"often found in groups",
					"and seem to follow a",
					"leader."
				),
				new BookPageInfo
				(
					"The leader is usually",
					"the largest and most",
					"powerful among them.",
					"They are known to",
					"communicate through",
					"low-frequency",
					"vibrations in the",
					"earth."
				),
				new BookPageInfo
				(
					"Their natural magic",
					"abilities include",
					"manipulating the",
					"terrain, causing",
					"small earthquakes,",
					"and summoning rock",
					"barriers for defense."
				),
				new BookPageInfo
				(
					"It has been observed",
					"that they possess a",
					"curious affinity with",
					"plants. Certain",
					"herbs and fungi are",
					"often found growing",
					"in their vicinity."
				),
				new BookPageInfo
				(
					"The ancient scrolls",
					"of Terath mention an",
					"alliance between",
					"Druids and Earth",
					"Elementals. This",
					"alliance was said to",
					"have immense power",
					"over the land."
				),
				new BookPageInfo
				(
					"While some view these",
					"beings as mere",
					"monsters, their",
					"existence is closely",
					"tied to the health",
					"of the earth itself.",
					"They are truly",
					"nature's guardians."
				),
				new BookPageInfo
				(
					"For those seeking to",
					"study or summon these",
					"creatures, it is",
					"advised to approach",
					"with respect and",
					"caution. The balance",
					"they maintain is",
					"fragile."
				),
				new BookPageInfo
				(
					"In conclusion, Earth",
					"Elementals are more",
					"than just animated",
					"rocks. They are a",
					"vital part of our",
					"ecosystem and deserve",
					"our understanding",
					"and respect."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public HistoryOfEarthElementals() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "History of Earth Elementals" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "History of Earth Elementals" );
        }

        public HistoryOfEarthElementals( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.WriteEncodedInt( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadEncodedInt();
        }
    }
}
