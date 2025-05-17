using System;
using Server;

namespace Server.Items
{
    public class RunedStoneFragment : Item
    {
        [Constructable]
        public RunedStoneFragment() : base(0x136A)
        {
            Name = "Runed Stone Fragment";
            Hue = 1109; // Unique magical hue
            Weight = 1.0;
        }

        public RunedStoneFragment(Serial serial) : base(serial)
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
