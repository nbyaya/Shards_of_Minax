using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class IceShardTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int SlowDuration { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public IceShardTile() : base(0x122F) // Use an appropriate ice-like tile ID
        {
            Movable = false;
            Name = "an ice shard tile";
            Hue = 1153; // Icy blue color

            SlowDuration = 5; // Duration of slowing effect in seconds
            AutoDelete = TimeSpan.FromSeconds(30);

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), CheckForPlayers);
            StartDeleteTimer();
        }

        private void CheckForPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0);
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("You slow down as you step onto the icy surface."); // You slow down as you step onto the icy surface.
                    m.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, -20, TimeSpan.FromSeconds(SlowDuration)));
                    Effects.SendTargetParticles(m, 0x376A, 9, 32, 5026, 0, 15, EffectLayer.Waist, 0);
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

        public IceShardTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(SlowDuration);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            SlowDuration = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), CheckForPlayers);
            StartDeleteTimer();
        }
    }
}