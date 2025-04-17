using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class TeleportToMalasItem : Item
    {
        [Constructable]
        public TeleportToMalasItem() : base(0x1ECD) // You can replace 0x1ECD with another item ID if you prefer
        {
            Name = "Malas Teleportation Stone";
            Hue = 0x480;
        }

        public TeleportToMalasItem(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Alive)
                return;

            from.Map = Map.Malas;
            from.Location = new Point3D(1094, 505, -85);
            from.SendMessage("You have been teleported to Malas!");
        }
    }
}
