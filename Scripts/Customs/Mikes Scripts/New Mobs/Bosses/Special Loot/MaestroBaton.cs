using System;
using Server;

namespace Server.Items
{
    public class MaestroBaton : Item
    {
        [Constructable]
        public MaestroBaton() : base(0xDF0) // You can choose a different item ID if needed
        {
            Weight = 1.0;
            Name = "Maestro's Baton";
            Hue = 0x482; // A unique hue for the baton
        }

        public MaestroBaton(Serial serial) : base(serial)
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
