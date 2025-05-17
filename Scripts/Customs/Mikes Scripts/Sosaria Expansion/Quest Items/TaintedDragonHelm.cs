using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedDragonHelm : DragonHelm
    {
        [Constructable]
        public TaintedDragonHelm()
        {
            Name = "tainted dragon helm";
            Hue = 2100;
            Attributes.BonusStr = 3;
            Attributes.ReflectPhysical = 5;
        }

        public TaintedDragonHelm(Serial serial) : base(serial) { }

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
