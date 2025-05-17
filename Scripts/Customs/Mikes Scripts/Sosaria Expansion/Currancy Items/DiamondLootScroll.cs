using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DiamondLootScroll : Item
    {
        [Constructable]
        public DiamondLootScroll() : base( 0x1F4A ) // you can pick any itemID
        {
            Name = "Scroll of Diamond Cache";
            Hue  = 1157;
            Weight = 0.1;
            LootType = LootType.Blessed; // optional
        }

        public DiamondLootScroll( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick( Mobile from )
        {
            if ( !IsChildOf( from.Backpack ) )
            {
                from.SendLocalizedMessage( 1042001 ); // Must be in your pack
                return;
            }

            from.SendMessage( "Target a creature to bless it with a diamond cache." );
            from.Target = new DiamondLootTarget( this );
        }

        private class DiamondLootTarget : Target
        {
            private readonly DiamondLootScroll _scroll;

            public DiamondLootTarget( DiamondLootScroll scroll ) 
                : base( 12, false, TargetFlags.Harmful )
            {
                _scroll = scroll;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if ( _scroll.Deleted )
                    return;

                if ( targeted is BaseCreature bc && !bc.Deleted )
                {
                    // attach XmlExtraLoot to drop 10–20 Diamonds at 100%
                    var att = new XmlExtraLoot(
                        "Server.Items.Diamond", // item type
                        10,                     // min
                        20,                     // max
                        1.0                     // chance (100%)
                    );

                    att.Expiration = TimeSpan.FromMinutes(30); // optional: auto-expire after 30m
                    XmlAttach.AttachTo( bc, att );

                    from.SendMessage( $"You have blessed {bc.Name} to drop diamonds on death." );
                    _scroll.Delete();
                }
                else
                {
                    from.SendMessage( "That is not a valid creature." );
                }
            }

            protected override void OnTargetFinish( Mobile from )
            {
                if ( !_scroll.Deleted )
                    from.SendMessage( "You decide not to use the scroll." );
            }
        }
    }
}
