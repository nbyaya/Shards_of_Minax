using System;
using Server;

namespace Server.Items
{
    public class MusicalScore : Item
    {
        [Constructable]
        public MusicalScore() : base(0xE20) // You can choose a different item ID if needed
        {
            Weight = 1.0;
            Name = "Musical Score";
            Hue = 0x497; // A unique hue for the musical score
        }

        public MusicalScore(Serial serial) : base(serial)
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
