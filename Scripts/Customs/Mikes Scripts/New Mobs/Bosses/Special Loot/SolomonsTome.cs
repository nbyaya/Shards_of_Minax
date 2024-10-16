using System;
using Server;

namespace Server.Items
{
    public class SolomonsTome : BaseBook
    {
        [Constructable]
        public SolomonsTome() : base(0xFF4)
        {
            Name = "Solomon's Tome";
            Hue = 0x8A5; // Royal color

        }

        public SolomonsTome(Serial serial) : base(serial)
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
