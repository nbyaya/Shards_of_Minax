using System;
using Server;

namespace Server.Items
{
    public class MartialArtsManual : Item
    {
        [Constructable]
        public MartialArtsManual()
            : base(0x1F4C) // Item ID for a book
        {
            Name = "Martial Arts Manual";
            Hue = 0x47E; // Item color (optional)
            Weight = 1.0;
        }

        public MartialArtsManual(Serial serial)
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
