using System;
using Server;

namespace Server.Items
{
    public class DragonsFocus : Item
    {
        [Constructable]
        public DragonsFocus()
            : base(0x1F1C) // Item ID for a book
        {
            Name = "Dragon's Focus";
            Hue = 0x47E; // Item color (optional)
            Weight = 1.0;
        }

        public DragonsFocus(Serial serial)
            : base(serial)
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
