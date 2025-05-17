using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class TimegateLegend : Moongate
    {
        [Constructable]
        public TimegateLegend() : base()
        {
            
            TargetMap = Map.TerMur;
            Target = new Point3D(720, 1864, 40);
			Name = "Time of Legends";

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 2135; // You can change this to any hue you want for your moongate
        }

        public TimegateLegend(Serial serial) : base(serial)
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

