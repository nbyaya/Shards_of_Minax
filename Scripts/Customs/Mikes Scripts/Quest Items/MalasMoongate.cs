using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class MalasMoongate : Moongate
    {
        [Constructable]
        public MalasMoongate() : base()
        {
            // Set the moongate to go to Malas
            TargetMap = Map.Malas;
            Target = new Point3D(1015, 527, -65);

            // Set the appearance of the moongate
            ItemID = 0xF6C; // You can change this to any item ID you want for your moongate appearance
            Light = LightType.Circle300;
            Hue = 0x482; // You can change this to any hue you want for your moongate
        }

        public MalasMoongate(Serial serial) : base(serial)
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

