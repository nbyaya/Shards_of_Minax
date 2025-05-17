using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class RenikaGate : Moongate
    {
        [Constructable]
        public RenikaGate() : base()
        {
            
            TargetMap = Map.Sosaria;
            Target = new Point3D(1092, 3384, 12);
			Name = "Renika";

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 2135; // You can change this to any hue you want for your moongate
        }

        public RenikaGate(Serial serial) : base(serial)
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

