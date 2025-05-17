using System;
using Server;

namespace Server.Items
{
    public class PlagueVial : Item
    {
        [Constructable]
        public PlagueVial() : base(0xE2B) // Small flask bottle sprite
        {
            this.Name = "a vial of pestilent ichor";
            this.Hue = 0x46F; // Sickly green
            this.Weight = 1.0;
        }

        public PlagueVial(Serial serial) : base(serial) { }

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
