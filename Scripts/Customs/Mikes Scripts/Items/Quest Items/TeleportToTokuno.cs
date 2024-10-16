using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class TeleportToTokuno : Item
    {
        [Constructable]
        public TeleportToTokuno() : base(0x1F14)
        {
            Name = "Teleport Stone to Tokuno";
            Hue = 1161;
            Movable = true;
            LootType = LootType.Blessed;
        }

        public TeleportToTokuno(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted)
            {
                return;
            }

            if (from is PlayerMobile)
            {
                from.MoveToWorld(new Point3D(736, 1256, 30), Map.Tokuno);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
