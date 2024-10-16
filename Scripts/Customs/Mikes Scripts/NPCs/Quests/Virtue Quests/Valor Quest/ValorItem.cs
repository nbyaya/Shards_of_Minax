using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ValorItem : Item
    {
        [Constructable]
        public ValorItem() : base(0x12CB) // Example item ID
        {
            Name = "Valor Item";
            Hue = 0x5E3; // Example color
            Weight = 0.1;
        }

        public ValorItem(Serial serial) : base(serial)
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
