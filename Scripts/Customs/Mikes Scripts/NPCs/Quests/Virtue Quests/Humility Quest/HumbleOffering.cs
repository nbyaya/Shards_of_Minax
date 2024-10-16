using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HumbleOffering : Item
    {
        [Constructable]
        public HumbleOffering() : base(0x1430) // Example item ID
        {
            Name = "Humble Offering";
            Hue = 0x47E; // Optional color
            Weight = 0.1;
        }

        public HumbleOffering(Serial serial) : base(serial)
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
