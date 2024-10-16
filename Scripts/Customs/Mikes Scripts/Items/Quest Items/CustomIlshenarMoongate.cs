using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class CustomIlshenarMoongate : Moongate
    {
        [Constructable]
        public CustomIlshenarMoongate() : base()
        {
            Name = "Ilshenar Moongate";
            Hue = 0x482; // You can change the color by setting a different hue here
            Light = LightType.Circle300;

            Target = new Point3D(1215, 467, -13); // Destination coordinates in Ilshenar
            TargetMap = Map.Ilshenar; // Setting the target map to Ilshenar
        }

        public override void UseGate(Mobile m)
        {
            if (!m.InRange(this.GetWorldLocation(), 1) || m.Map != this.Map)
            {
                m.SendLocalizedMessage(500446); // That is too far away.
            }
            else if (m.Player)
            {
                base.UseGate(m);
            }
        }

        public CustomIlshenarMoongate(Serial serial) : base(serial)
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
