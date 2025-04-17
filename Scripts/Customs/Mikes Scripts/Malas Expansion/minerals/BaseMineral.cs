using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public abstract class BaseMineral : Item
    {
        public abstract string MineralName { get; }
        public abstract int MineralHue { get; }
        public abstract int MineralGraphic { get; }

        public BaseMineral() : base(0x1BF2) // Default mineral graphic
        {
            Name = MineralName;
            Hue = MineralHue;
            ItemID = MineralGraphic;
            Weight = 1.0;
        }

        public BaseMineral(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
