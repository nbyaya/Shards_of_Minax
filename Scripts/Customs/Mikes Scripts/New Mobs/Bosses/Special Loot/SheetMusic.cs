using System;
using Server;

namespace Server.Items
{
    public class SheetMusic : Item
    {
        [Constructable]
        public SheetMusic() : base(0xEF3) // You can choose a different item ID if needed
        {
            Weight = 1.0;
            Name = "Sheet Music";
            Hue = 0x495; // A unique hue for the sheet music
        }

        public SheetMusic(Serial serial) : base(serial)
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
