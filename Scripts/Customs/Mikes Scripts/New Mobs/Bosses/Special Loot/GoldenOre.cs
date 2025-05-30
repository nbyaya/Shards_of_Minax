using System;
using Server;

namespace Server.Items
{
    public class GoldenOre : Item
    {
        [Constructable]
        public GoldenOre() : base(0x19B9)
        {
            Name = "Golden Ore";
            Hue = 0x501; // Gold hue
            Weight = 5.0;
        }

        public GoldenOre(Serial serial) : base(serial)
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
