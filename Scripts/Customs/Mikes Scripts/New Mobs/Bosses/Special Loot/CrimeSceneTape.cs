using System;
using Server.Items;

namespace Server.Items
{
    public class CrimeSceneTape : Item
    {
        [Constructable]
        public CrimeSceneTape() : base(0x14F8)
        {
            Weight = 1.0;
            Hue = 1161; // Set to a unique color
            Name = "Crime Scene Tape";
        }

        public CrimeSceneTape(Serial serial) : base(serial)
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
