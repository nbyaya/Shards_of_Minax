using System;
using Server;

namespace Server.Items
{
    public class GoldenMusicStand : Item
    {
        [Constructable]
        public GoldenMusicStand() : base(0x1EB1) // You can choose a different item ID if needed
        {
            Weight = 10.0;
            Name = "Golden Music Stand";
            Hue = 0x8A5; // A unique hue for the music stand
        }

        public GoldenMusicStand(Serial serial) : base(serial)
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
