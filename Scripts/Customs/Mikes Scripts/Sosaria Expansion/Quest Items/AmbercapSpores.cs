using System;
using Server;

namespace Server.Items
{
    public class AmbercapSpores : Item
    {
        [Constructable]
        public AmbercapSpores() : base(0x0D16) // Base mushroom graphic
        {
            Hue = 2213; // Amber/golden tone
            Name = "Ambercap Spores";
            Stackable = true;
            Weight = 0.1;
        }

        public AmbercapSpores(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
