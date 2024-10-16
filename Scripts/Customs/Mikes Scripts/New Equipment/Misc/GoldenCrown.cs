using System;
using Server.Items;

namespace Server.Items
{
    public class GoldenCrown : Item
    {
        [Constructable]
        public GoldenCrown() : base(0x2B0E) // ItemID for a crown
        {
            Name = "Golden Crown of the Dragon";
            Hue = 0x8A5; // Gold hue
            Weight = 2.0;
        }

        public GoldenCrown(Serial serial) : base(serial)
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
