using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class VortexTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int PullStrength { get; set; }
        public int Range { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public VortexTile() : base(0x122A) // Use an appropriate vortex-like tile ID
        {
            Movable = false;
            Name = "a swirling vortex";
            Hue = 1910; // Dark blue color

            PullStrength = 3; // Number of steps pulled per tick
            Range = 5; // Range of the vortex effect
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), PullNearbyPlayers);
            StartDeleteTimer();
        }

        private void PullNearbyPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(Range);
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile && m.AccessLevel == AccessLevel.Player)
                {
                    Direction pullDirection = m.GetDirectionTo(this);
                    for (int i = 0; i < PullStrength; i++)
                    {
                        m.Direction = pullDirection;
                        if (!m.Move(pullDirection))
                            break; // Stop if the player can't move further
                    }
                    m.SendLocalizedMessage(502731); // You are being pulled by the vortex!
                }
            }
            eable.Free();
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

        public VortexTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(PullStrength);
            writer.Write(Range);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            PullStrength = reader.ReadInt();
            Range = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), PullNearbyPlayers);
            StartDeleteTimer();
        }
    }
}