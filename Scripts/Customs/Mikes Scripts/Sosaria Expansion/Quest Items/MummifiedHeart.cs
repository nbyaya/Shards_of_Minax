using System;
using Server;

namespace Server.Items
{
    public class MummifiedHeart : Item
    {
        [Constructable]
        public MummifiedHeart() : base(0x024B) // Heart graphic
        {
            Hue = 2117; // Ancient red/brown hue
            Name = "a Mummified Heart";
            Weight = 1.0;
        }

        public MummifiedHeart(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
