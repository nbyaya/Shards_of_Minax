using System;
using Server.Items;

namespace Server.Items
{
    public class FireWardedBand : GoldRing
    {
        [Constructable]
        public FireWardedBand()
        {
            Name = "Fire-Warded Band";
            Hue = 1359;
        }

        public FireWardedBand(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
