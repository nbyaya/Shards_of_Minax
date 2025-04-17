using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System;
using Server.Multis;

namespace Server.Custom.Items
{
    public class OutpostCharter : Item
    {
        [Constructable]
        public OutpostCharter() : base(0x14F0) // Use scroll graphic
        {
            Name = "Outpost Charter";
            Weight = 1.0;
            Hue = 1150; // Optional: change to any hue you like
        }

        public OutpostCharter(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            from.SendMessage("Select a location to deploy your outpost.");
            from.Target = new OutpostPlacementTarget(this);
        }

        private class OutpostPlacementTarget : Target
        {
            private OutpostCharter m_Charter;

            public OutpostPlacementTarget(OutpostCharter charter) : base(12, true, TargetFlags.None)
            {
                m_Charter = charter;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Charter == null || m_Charter.Deleted)
                    return;

                IPoint3D point = targeted as IPoint3D;

                if (point == null)
                {
                    from.SendMessage("That is not a valid location.");
                    return;
                }

                Map map = from.Map;

                if (map == null || map == Map.Internal)
                {
                    from.SendMessage("That is not a valid map.");
                    return;
                }

                Point3D spawnPoint = new Point3D(point.X, point.Y, map.GetAverageZ(point.X, point.Y));

                from.SendMessage("You have established an outpost.");
                Effects.SendLocationEffect(spawnPoint, map, 0x3709, 30, 10); // Optional visual effect

                // Spawn the outpost
                OutpostCamp camp = new OutpostCamp();
                camp.MoveToWorld(spawnPoint, map);

                // Consume the item
                m_Charter.Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
