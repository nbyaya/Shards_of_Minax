using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class ChaoticTeleportTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int TeleportRange { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public ChaoticTeleportTile() : base(0x1BC3) // Use an appropriate magical-looking tile ID
        {
            Movable = false;
            Name = "a chaotic teleport tile";
            Hue = 0x8A5; // Bright yellow hue

            TeleportRange = 5;
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), PerformEffect);
            StartDeleteTimer();
        }

        private void PerformEffect()
        {
            if (Deleted) return;

            // Perform the bright yellow magical animation
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 
                0x376A, 9, 32, 5021, 0, 5044, 0);
            Effects.PlaySound(Location, Map, 0x201);

            // Get all mobiles on the tile or adjacent to it
            IPooledEnumerable eable = GetMobilesInRange(1);
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile)
                {
                    TeleportPlayer(m);
                }
            }
            eable.Free();
        }

        private void TeleportPlayer(Mobile m)
        {
            for (int i = 0; i < 10; i++) // Try up to 10 times to find a valid location
            {
                int x = m.X + Utility.RandomMinMax(-TeleportRange, TeleportRange);
                int y = m.Y + Utility.RandomMinMax(-TeleportRange, TeleportRange);
                int z = m.Map.GetAverageZ(x, y);

                Point3D loc = new Point3D(x, y, z);

                if (m.Map.CanSpawnMobile(loc))
                {
                    m.MoveToWorld(loc, m.Map);
                    m.ProcessDelta();
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 
                        0x376A, 9, 32, 5022, 0, 5044, 0);
                    m.PlaySound(0x1FE);
                    break;
                }
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

        public ChaoticTeleportTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(TeleportRange);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            TeleportRange = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), PerformEffect);
            StartDeleteTimer();
        }
    }
}