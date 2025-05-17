using System;
using Server;

namespace Server.Items
{
    public class SporewardPotion : Item
    {
        [Constructable]
        public SporewardPotion() : base(0x0F07)
        {
            Hue = 1278;
            Name = "Sporeward Potion";
            Weight = 1.0;
        }

        public SporewardPotion(Serial serial) : base(serial) { }

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
