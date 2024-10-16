using System;
using Server;

namespace Server.Items
{
    public class ShipInABottle : Item
    {
        [Constructable]
        public ShipInABottle() : base(0x0F01)
        {
            Name = "Ship in a Bottle";
            Hue = 0x66D;
            Weight = 1.0;
        }

        public ShipInABottle(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 3))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            from.SendMessage("You see a miniature ship sailing on tiny waves inside the bottle.");
            from.PlaySound(0x4C);
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