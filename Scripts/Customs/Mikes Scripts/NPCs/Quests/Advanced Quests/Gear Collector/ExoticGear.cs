using System;
using Server.Items;

namespace Server.Items
{
    public class ExoticGear : Item
    {
        [Constructable]
        public ExoticGear() : base(0x1051) // Replace 0x1F4 with the actual item ID you want
        {
            Name = "Exotic Gear";
            Hue = Utility.Random(1, 3000);
        }

        public ExoticGear(Serial serial) : base(serial)
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
