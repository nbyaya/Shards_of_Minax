using System;
using Server.Items;

namespace Server.Items
{
    public class ForensicKit : Item
    {
        [Constructable]
        public ForensicKit() : base(0x1EBA)
        {
            Weight = 2.0;
            Hue = 1150; // Set to a unique color
            Name = "Forensic Kit";
        }

        public ForensicKit(Serial serial) : base(serial)
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
