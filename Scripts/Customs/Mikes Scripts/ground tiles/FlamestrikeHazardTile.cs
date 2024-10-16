using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class FlamestrikeHazardTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int Damage { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public FlamestrikeHazardTile() : base(0x1AC2) // Use an appropriate fire-like tile ID
        {
            Movable = false;
            Name = "a flamestrike hazard";
            Hue = 1159; // Dark red hue

            Damage = 20; // Damage per flamestrike
            AutoDelete = TimeSpan.FromSeconds(30); // Tile lasts for 30 seconds

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), PerformFlamestrike);
            StartDeleteTimer();
        }

        private void PerformFlamestrike()
        {
            if (Deleted) return;

            // Get all adjacent locations, including the tile's own location
            List<Point3D> affectedLocations = new List<Point3D>
            {
                new Point3D(X, Y, Z),
                new Point3D(X + 1, Y, Z),
                new Point3D(X - 1, Y, Z),
                new Point3D(X, Y + 1, Z),
                new Point3D(X, Y - 1, Z),
                new Point3D(X + 1, Y + 1, Z),
                new Point3D(X + 1, Y - 1, Z),
                new Point3D(X - 1, Y + 1, Z),
                new Point3D(X - 1, Y - 1, Z)
            };

            foreach (Point3D loc in affectedLocations)
            {
                // Play the flamestrike effect at each location
                Effects.SendLocationEffect(new Point3D(loc.X, loc.Y, loc.Z + 50), Map, 0x3709, 30, 20, 1159, 0); // Flamestrike effect with dark red hue

                // Damage players at each location
                IPooledEnumerable eable = Map.GetMobilesInRange(loc, 0);
                foreach (Mobile m in eable)
                {
                    if (m.Alive && m is PlayerMobile)
                    {
                        m.Damage(Damage);
                        m.SendLocalizedMessage(503019); // You are scorched by the flames!
                    }
                }
                eable.Free();
            }
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(AutoDelete, DeleteTimer);
        }

        private void DeleteTimer()
        {
            this.Delete();
        }

        public FlamestrikeHazardTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(Damage);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Damage = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), PerformFlamestrike);
            StartDeleteTimer();
        }
    }
}