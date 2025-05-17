using System;
using Server;

namespace Server.Items
{
    public class FrostveinCrystal : Item
    {
        [Constructable]
        public FrostveinCrystal() : base(0x35DB)
        {
            Name = "Frostvein Crystal";
            Hue = 0x480; // Icy-blue hue
            Weight = 1.0;
            Stackable = true;
        }

        public FrostveinCrystal(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
