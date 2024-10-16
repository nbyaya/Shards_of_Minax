using System;
using Server;

namespace Server.Items
{
    public class AncientVase : Item
    {
        [Constructable]
        public AncientVase() : base(0x241D)
        {
            Weight = 7.0;
            Hue = 0x835;
            Name = "Ancient Vase";
        }

        public AncientVase(Serial serial) : base(serial) { }

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
