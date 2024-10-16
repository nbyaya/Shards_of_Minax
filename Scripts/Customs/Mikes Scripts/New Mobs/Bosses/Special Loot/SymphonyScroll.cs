using System;
using Server;

namespace Server.Items
{
    public class SymphonyScroll : Item
    {
        [Constructable]
        public SymphonyScroll() : base(0x1F4E) // You can choose a different item ID if needed
        {
            Weight = 1.0;
            Name = "Symphony Scroll";
            Hue = 0x490; // A unique hue for the scroll
        }

        public SymphonyScroll(Serial serial) : base(serial)
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
