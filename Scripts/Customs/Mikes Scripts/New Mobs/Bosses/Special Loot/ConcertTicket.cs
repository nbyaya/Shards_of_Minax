using System;
using Server;

namespace Server.Items
{
    public class ConcertTicket : Item
    {
        [Constructable]
        public ConcertTicket() : base(0x14F0) // You can choose a different item ID if needed
        {
            Weight = 0.1;
            Name = "Concert Ticket";
            Hue = 0x44E; // A unique hue for the ticket
        }

        public ConcertTicket(Serial serial) : base(serial)
        {
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
