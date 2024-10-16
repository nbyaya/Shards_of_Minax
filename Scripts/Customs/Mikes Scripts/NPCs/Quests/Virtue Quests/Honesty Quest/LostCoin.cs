using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LostCoin : Item
    {
        [Constructable]
        public LostCoin() : base(0x0EF2) // Example item ID
        {
            Name = "Lost Coin";
            Hue = 0x8A5; // Example color
            Weight = 0.1;
        }

        public LostCoin(Serial serial) : base(serial)
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
