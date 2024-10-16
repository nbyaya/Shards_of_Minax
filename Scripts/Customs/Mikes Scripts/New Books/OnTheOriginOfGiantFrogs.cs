using System;
using Server;

namespace Server.Items
{
    public class OnTheOriginOfGiantFrogs : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "On the Origin of Giant Frogs", "Mystikos",
                new BookPageInfo
                (
                    "In the beginning, the",
                    "giant frogs were not",
                    "giant nor particularly",
                    "interesting. However,",
                    "they were always",
                    "viewed with a sense of",
                    "intrigue and wonder.",
                    ""
                ),
                new BookPageInfo
                (
                    "The first records of",
                    "these creatures date",
                    "back to the age of",
                    "the old magicians,",
                    "who reportedly used",
                    "them in various",
                    "experiments.",
                    ""
                ),
                new BookPageInfo
                (
                    "It is said that the",
                    "magicians’ meddling",
                    "in alchemy led to",
                    "unexpected mutations.",
                    "The frogs began to",
                    "grow, both in size and",
                    "intelligence."
                ),
                new BookPageInfo
                (
                    "Today, these giant",
                    "frogs are seen both",
                    "as a menace and a",
                    "mystery. Adventurers",
                    "often hunt them for",
                    "their magical",
                    "properties."
                ),
                new BookPageInfo
                (
                    "However, it is advised",
                    "to approach them with",
                    "caution. For their",
                    "tongues have the",
                    "strength of steel,",
                    "and their bite can be",
                    "venomous."
                ),
				new BookPageInfo
				(
					"The varied species of",
					"giant frogs have",
					"adapted to different",
					"environments. Some",
					"prefer marshy lands,",
					"while others have",
					"adapted to the harsh",
					"deserts."
				),
				new BookPageInfo
				(
					"Their skin color",
					"ranges from vibrant",
					"greens to dull browns,",
					"allowing them to blend",
					"seamlessly with their",
					"surroundings. This",
					"provides a natural",
					"camouflage."
				),
				new BookPageInfo
				(
					"In terms of diet, these",
					"creatures are not",
					"picky eaters. From",
					"insects to small",
					"animals, their",
					"voracious appetite",
					"knows no bounds."
				),
				new BookPageInfo
				(
					"While they are not",
					"inherently aggressive,",
					"they are territorial.",
					"It is advised to steer",
					"clear of their habitat",
					"unless properly",
					"equipped."
				),
				new BookPageInfo
				(
					"Rumors speak of even",
					"larger frogs, ones",
					"that have evolved to",
					"become apex",
					"predators in their",
					"environments. These",
					"stories, however,",
					"remain unconfirmed."
				),
				new BookPageInfo
				(
					"There have been",
					"reports of alchemists",
					"using giant frog parts",
					"in potions. The eyes",
					"are said to have",
					"mystical properties.",
					"Yet, this practice is",
					"often frowned upon."
				),
				new BookPageInfo
				(
					"The debate among",
					"scholars regarding",
					"these creatures is",
					"ongoing. Some argue",
					"they are abominations,",
					"while others believe",
					"they serve a vital role",
					"in the ecosystem."
				),
				new BookPageInfo
				(
					"In conclusion, giant",
					"frogs are fascinating",
					"creatures that captivate",
					"the imagination. As we",
					"continue to study",
					"them, we realize they",
					"are more than just",
					"products of magic."
				),
                new BookPageInfo
                (
                    "This concludes our",
                    "study on the origins",
                    "of the giant frogs. If",
                    "you encounter one in",
                    "your travels, be",
                    "prepared for an",
                    "unforgettable",
                    "experience."
                )
            );

        public override BookContent DefaultContent{ get{ return Content; } }

        [Constructable]
        public OnTheOriginOfGiantFrogs() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "On the Origin of Giant Frogs" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "On the Origin of Giant Frogs" );
        }

        public OnTheOriginOfGiantFrogs( Serial serial ) : base( serial )
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
