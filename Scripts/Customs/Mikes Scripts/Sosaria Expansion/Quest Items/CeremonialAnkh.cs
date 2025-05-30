using System;
using Server;

namespace Server.Items
{
    public class CeremonialAnkh : Item
    {
        [Constructable]
        public CeremonialAnkh() : base(0xAD7E)
        {
            Name = "a Ceremonial Ankh";
            Hue = 1153;
            Weight = 1.0;
        }

        public CeremonialAnkh(Serial serial) : base(serial)
        {
        }

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
