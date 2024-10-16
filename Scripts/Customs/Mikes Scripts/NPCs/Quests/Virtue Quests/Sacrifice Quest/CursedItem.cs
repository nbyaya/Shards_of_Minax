using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CursedItem : Item
    {
        [Constructable]
        public CursedItem() : base(0x4BA5) // Example item ID
        {
            Name = "Cursed Item";
            Hue = 0x3F; // Example color
            Weight = 0.1;
        }

        public CursedItem(Serial serial) : base(serial)
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
