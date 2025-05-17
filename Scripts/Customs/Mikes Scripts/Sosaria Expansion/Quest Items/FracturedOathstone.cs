using System;
using Server;

namespace Server.Items
{
    public class FracturedOathstone : Item
    {
        [Constructable]
        public FracturedOathstone() : base(0x2A3F)
        {
            Name = "Fractured Oathstone";
            Hue = 1175; // Ghostly-glow hue
            Weight = 1.0;
        }

        public FracturedOathstone(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
