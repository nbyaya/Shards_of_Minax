using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class ElixirOfLife : Item
    {
        [Constructable]
        public ElixirOfLife() : base(0xF0E)
        {
            Weight = 1.0;
            Hue = 0x2D;
            Name = "Elixir of Life";
        }

        public ElixirOfLife(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("You drink the Elixir of Life and feel completely revitalized!");
            from.Hits = from.HitsMax;
            from.Stam = from.StamMax;
            from.Mana = from.ManaMax;

            from.FixedEffect(0x375A, 10, 15);
            from.PlaySound(0x1E7);

            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}