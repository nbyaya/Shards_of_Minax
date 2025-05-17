using System;

namespace Server.Items
{
    public class BlackSoulstone : Item
    {
        [Constructable]
        public BlackSoulstone() : base(0x1F19) // Coin icon
        {
		
		Hue = 1345;
		
        }

        public BlackSoulstone(Serial serial) : base(serial) { }

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
