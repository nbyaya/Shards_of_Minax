using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ManaDrainTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int ManaDrainAmount { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public ManaDrainTile() : base(0x0E62) // Use an appropriate magical-looking tile ID
        {
            Movable = false;
            Name = "a mana drain field";
            Hue = 2721; // Magical purple color

            ManaDrainAmount = 5; // Amount of mana drained per tick
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), DrainMana);
            StartDeleteTimer();
        }

        private void DrainMana()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0); // Only affects players standing directly on the tile
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile)
                {
                    int drain = Math.Min(m.Mana, ManaDrainAmount);
                    m.Mana -= drain;
                    m.SendLocalizedMessage(1070844); // Your mana is being drained!
                    Effects.SendTargetParticles(m, 0x3789, 10, 32, 0x13C, 0, 5032, EffectLayer.Waist, 0);
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

        public ManaDrainTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(ManaDrainAmount);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            ManaDrainAmount = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), DrainMana);
            StartDeleteTimer();
        }
    }
}