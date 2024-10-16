using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VictoryToken : Item
    {
        [Constructable]
        public VictoryToken() : base(0x2AAA) // You can change the item ID (0x2AAA) to any valid item ID that you prefer
        {
            Name = "Victory Token";
            Hue = 1161; // You can change the color
            Weight = 1.0;
            LootType = LootType.Blessed; // Making it blessed so it's not lost on death
        }

        public VictoryToken(Serial serial) : base(serial)
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
