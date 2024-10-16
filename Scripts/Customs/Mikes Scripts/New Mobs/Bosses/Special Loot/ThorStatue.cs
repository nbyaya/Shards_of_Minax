using System;
using Server;

namespace Server.Items
{
    public class ThorStatue : Item
    {
        [Constructable]
        public ThorStatue()
            : base(0x139A)
        {
            Name = "Statue of Thor";
            Hue = 0x482;
            Weight = 5.0;
        }

        public ThorStatue(Serial serial) : base(serial)
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
