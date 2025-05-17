using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class TimegateMalas : Moongate
    {
        [Constructable]
        public TimegateMalas() : base()
        {
            
            TargetMap = Map.Malas;
            Target = new Point3D(1016, 519, -70);
			Name = "City at the End of Time";

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 2135; // You can change this to any hue you want for your moongate
        }

        public TimegateMalas(Serial serial) : base(serial)
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

