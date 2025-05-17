using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class TimegateTokuno : Moongate
    {
        [Constructable]
        public TimegateTokuno() : base()
        {
            
            TargetMap = Map.Tokuno;
            Target = new Point3D(735, 1257, 30);
			Name = "600 AD";

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 2135; // You can change this to any hue you want for your moongate
        }

        public TimegateTokuno(Serial serial) : base(serial)
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

