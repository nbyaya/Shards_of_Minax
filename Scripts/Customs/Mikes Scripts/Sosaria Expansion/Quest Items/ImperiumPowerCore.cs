using System;
using Server;

namespace Server.Items
{
    public class ImperiumPowerCore : Item
    {
        [Constructable]
        public ImperiumPowerCore() : base(0x1F1C)
        {
            Name = "Imperium Power Core";
            Hue = 2966;
            Weight = 2.0;
        }

        public ImperiumPowerCore(Serial serial) : base(serial) { }

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
