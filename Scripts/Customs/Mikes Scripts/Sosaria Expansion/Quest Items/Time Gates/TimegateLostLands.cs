using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class TimegateLostLands : Moongate
    {
        [Constructable]
        public TimegateLostLands() : base()
        {
            
            TargetMap = Map.Trammel;
            Target = new Point3D(5598, 3540, 37);
			Name = "65,000,000 BC";

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 2135; // You can change this to any hue you want for your moongate
        }

        public TimegateLostLands(Serial serial) : base(serial)
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

