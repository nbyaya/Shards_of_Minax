using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedScales : Item
    {
        [Constructable]
        public TaintedScales(int amount) : base(0x26B4) // Use a scale graphic
        {
            Stackable = true;
            Weight = 0.1;
            Hue = 2100;
            Amount = amount;
            Name = "tainted dragon scales";
        }

        public TaintedScales() : this(1) { }

        public TaintedScales(Serial serial) : base(serial) { }

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
