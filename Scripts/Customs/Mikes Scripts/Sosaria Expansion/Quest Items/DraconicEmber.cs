using System;
using Server.Items;

namespace Server.Items
{
    public class DraconicEmber : Item
    {
        [Constructable]
        public DraconicEmber() : base(0x1F13)
        {
            Hue = 1359;
            Name = "Draconic Ember";
            Weight = 1.0;
        }

        public DraconicEmber(Serial serial) : base(serial) { }

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
