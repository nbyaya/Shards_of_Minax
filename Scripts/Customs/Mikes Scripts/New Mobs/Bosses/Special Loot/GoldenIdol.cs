using System;
using Server;

namespace Server.Items
{
    public class GoldenIdol : Item
    {
        [Constructable]
        public GoldenIdol() : base(0x2D83)
        {
            Weight = 5.0;
            Hue = 0x8A5;
            Name = "Golden Idol";
        }

        public GoldenIdol(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
