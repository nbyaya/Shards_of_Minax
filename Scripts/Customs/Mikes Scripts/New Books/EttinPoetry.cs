using System;
using Server;

namespace Server.Items
{
    public class EttinPoetry : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Ettin Poetry", "Gruk & Flark",
                new BookPageInfo
                (
                    "We, Ettins, Gruk and",
                    "Flark, share this",
                    "body and soul. With",
                    "two minds, we express",
                    "ourselves through",
                    "words, yet with a",
                    "single goal.",
                    ""
                ),
                new BookPageInfo
                (
                    "Ettin's Life",
                    "",
                    "Two heads are better",
                    "than one, they say,",
                    "Except at dinner,",
                    "when we fight for prey."
                ),
                new BookPageInfo
                (
                    "Unity",
                    "",
                    "Two heads, one heart,",
                    "we roam the land,",
                    "Though we may argue,",
                    "together we stand."
                ),
                new BookPageInfo
                (
                    "The End",
                    "",
                    "Together we are strong,",
                    "apart we are weak.",
                    "In poetry, we found",
                    "the unity we seek."
                ),
				new BookPageInfo
				(
					"Friendship",
					"",
					"One head wants to fight,",
					"the other wants to play,",
					"It takes compromise",
					"to get through the day."
				),
				new BookPageInfo
				(
					"The Duel",
					"",
					"When I say yes,",
					"and you say no,",
					"To solve our qualms,",
					"to duel we go."
				),
				new BookPageInfo
				(
					"The Forest",
					"",
					"We wander in forests,",
					"so thick and so grand,",
					"With two sets of eyes,",
					"we better understand."
				),
				new BookPageInfo
				(
					"Night and Day",
					"",
					"One likes the night,",
					"the other the sun,",
					"Yet in both realms,",
					"we find our fun."
				),
				new BookPageInfo
				(
					"Two Minds",
					"",
					"Two minds to ponder,",
					"two minds to think,",
					"Yet when it comes to food,",
					"in a blink, it's gone."
				),
				new BookPageInfo
				(
					"Emotions",
					"",
					"When one is sad,",
					"and the other is glad,",
					"We mix and make",
					"a feeling not bad."
				),
				new BookPageInfo
				(
					"A Final Word",
					"",
					"Two heads to argue,",
					"but a single heart to love,",
					"In this duality,",
					"we find strength thereof."
				)

            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public EttinPoetry() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Ettin Poetry" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Ettin Poetry" );
        }

        public EttinPoetry( Serial serial ) : base( serial )
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
