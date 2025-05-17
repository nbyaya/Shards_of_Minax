using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class QuicksandTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int DamagePerTick { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public QuicksandTile() : base(0x1B71) // Use an appropriate sand-like tile ID
        {
            Movable = false;
            Name = "quicksand";
            Hue = 2413; // Sandy color

            DamagePerTick = 5;
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);
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
                    m.SendMessage("Your feet begin to sink into the sand beneath you!"); // Your feet begin to sink into the sand beneath you!
                    m.Damage(DamagePerTick, null);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    Effects.SendTargetParticles(m, 0x36B0, 1, 40, 0x1F1F, 0, 9932, EffectLayer.Waist, 0);
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

        public QuicksandTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(DamagePerTick);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            DamagePerTick = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);
            StartDeleteTimer();
        }
    }
}