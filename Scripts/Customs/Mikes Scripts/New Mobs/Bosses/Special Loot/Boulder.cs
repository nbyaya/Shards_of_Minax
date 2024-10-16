using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Boulder : Item
    {
        [Constructable]
        public Boulder() : base(0x11B6)
        {
            // You can add any additional setup here if needed
            // For now, it just sets the item graphic to 0x11B6
        }

        public Boulder(Serial serial) : base(serial)
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
