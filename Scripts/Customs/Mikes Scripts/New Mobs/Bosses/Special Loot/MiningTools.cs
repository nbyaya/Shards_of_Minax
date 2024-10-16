using System;
using Server;

namespace Server.Items
{
    public class MiningTools : Item
    {
        [Constructable]
        public MiningTools() : base(0x1EB8)
        {
            Name = "Mining Tools";
            Hue = 0x8A5; // Royal color
            Weight = 5.0;
        }

        public MiningTools(Serial serial) : base(serial)
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
