using System;
using Server;

namespace Server.Items
{
    public class GlowsporeClump : Item
    {
        [Constructable]
        public GlowsporeClump() : base(0x0D0C)
        {
            Hue = 1278; // Luminescent blue-green
            Name = "Glowspore Clump";
            Weight = 0.2;
        }

        public GlowsporeClump(Serial serial) : base(serial) { }

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
