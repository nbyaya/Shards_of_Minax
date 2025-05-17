using System;
using Server;

namespace Server.Items
{
    public class CursedDust : Item
    {
        [Constructable]
        public CursedDust() : base(0x0F8B)
        {
            Name = "Cursed Dust Relic";
            Hue = 1152;
            Weight = 1.0;
        }

        public CursedDust(Serial serial) : base(serial) { }

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
