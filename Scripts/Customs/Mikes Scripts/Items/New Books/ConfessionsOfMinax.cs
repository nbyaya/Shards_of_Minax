using System;
using Server;

namespace Server.Items
{
    public class ConfessionsOfMinax : BlueBook
    {
        public static readonly BookContent Content = new BookContent
            (
                "Confessions of Minax", "Minax",
                new BookPageInfo
                (
                    "This book reveals the",
                    "inner thoughts and",
                    "secrets of Minax, a",
                    "powerful sorceress",
                    "whose influence has",
                    "shaped the world.",
                    "",
                    "          -Minax"
                ),
                new BookPageInfo
                (
                    "Many see me as a",
                    "villain, a scourge",
                    "upon the realm. Yet",
                    "what they fail to",
                    "understand is the",
                    "burden of power and",
                    "the loneliness it",
                    "brings."
                ),
                new BookPageInfo
                (
                    "In my early years, I",
                    "studied under Mondain",
                    "and his teachings",
                    "gave me purpose. Yet",
                    "I found myself",
                    "wanting more, more",
                    "power, more control."
                ),
                new BookPageInfo
                (
                    "At the height of my",
                    "powers, even as I",
                    "conquered realms and",
                    "vanquished foes, I",
                    "felt an emptiness.",
                    "Could it be that my",
                    "pursuits were all in",
                    "vain?"
                ),
                new BookPageInfo
                (
                    "Love was an elusive",
                    "companion. While I",
                    "had many admirers and",
                    "even more followers,",
                    "true companionship",
                    "evaded me. What use",
                    "is power, if one has",
                    "no one to share it?"
                ),
                new BookPageInfo
                (
                    "It was during these",
                    "times of doubt that I",
                    "met someone who",
                    "seemed to fill that",
                    "void. Yet, even that",
                    "turned to betrayal.",
                    "Perhaps, I was",
                    "destined to walk this"
                ),
                new BookPageInfo
                (
                    "path alone, forever",
                    "searching for what",
                    "I can never have.",
                    "Yet, my resolve has",
                    "never been stronger.",
                    "If the world wishes",
                    "to paint me as a",
                    "villain, then so be it."
                ),
                new BookPageInfo
                (
                    "As I pen down these",
                    "final words, I ponder",
                    "the choices that led",
                    "me here. Would I do",
                    "things differently?",
                    "Perhaps, but then I",
                    "wouldn't be Minax.",
                    "This is my confession."
                ),
                new BookPageInfo
                (
                    "Minax, 12pm.",
                    "10.18.2023",
                    "The world shall hear"
                )
            );

        public override BookContent DefaultContent { get { return Content; } }

        [Constructable]
        public ConfessionsOfMinax() : base( false )
        {
        }

        public override void AddNameProperty( ObjectPropertyList list )
        {
            list.Add( "Confessions of Minax" );
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, "Confessions of Minax" );
        }

        public ConfessionsOfMinax( Serial serial ) : base( serial )
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
