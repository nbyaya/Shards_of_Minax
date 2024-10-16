using System;
using Server;

namespace Server.Items
{
    public class AncientRelic : Item
    {
        [Constructable]
        public AncientRelic() : base(0x1F14)
        {
            Weight = 1.0;
            Hue = 0x972;
            Name = "Ancient Relic";
        }

        public AncientRelic(Serial serial) : base(serial) { }

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
