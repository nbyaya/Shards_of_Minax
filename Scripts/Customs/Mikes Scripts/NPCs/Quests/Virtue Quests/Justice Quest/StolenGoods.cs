using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class StolenGoods : Item
    {
        [Constructable]
        public StolenGoods() : base(0x2859) // Example item ID
        {
            Name = "Stolen Goods";
            Hue = 0x4B2; // Example color
            Weight = 0.1;
        }

        public StolenGoods(Serial serial) : base(serial)
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
