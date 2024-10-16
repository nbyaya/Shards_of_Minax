using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class DarkMoongate : Item
    {
        [Constructable]
        public DarkMoongate() : base(0xF6C)
        {
            Movable = false;
            Light = LightType.Circle300;
            Name = "Dark Moongate";
        }

        public DarkMoongate(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile)
            {
                Map targetMap = null;
                
                if (m.Map == Map.Trammel)
                {
                    targetMap = Map.Felucca;
                }
                else if (m.Map == Map.Felucca)
                {
                    targetMap = Map.Trammel;
                }

                if (targetMap != null)
                {
                    Point3D targetLocation = new Point3D(m.X, m.Y, m.Z);
                    BaseCreature.TeleportPets(m, targetLocation, targetMap);
                    m.MoveToWorld(targetLocation, targetMap);
                    m.SendLocalizedMessage(1019002);  // You have entered a moongate.
                }
                else
                {
                    m.SendMessage("This moongate does not function here.");
                }
            }

            return true;
        }

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
