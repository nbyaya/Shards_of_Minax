using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class ThunderstormTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int LightningDamage { get; set; }
        public int StunDuration { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public ThunderstormTile() : base(0x0E5C) // Use an appropriate storm cloud-like tile ID
        {
            Movable = false;
            Name = "a thunderstorm";
            Hue = 1001; // Dark stormy color

            LightningDamage = 15;
            StunDuration = 2; // Stun duration in seconds
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), StrikeRandomPlayer);
            StartDeleteTimer();
        }

        private void StrikeRandomPlayer()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(3); // Strikes within 3 tiles
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile && Utility.RandomBool()) // 50% chance to strike
                {
                    m.SendMessage("Lightning strikes nearby!"); // Lightning strikes nearby!
                    m.Damage(LightningDamage, null);
                    m.Paralyze(TimeSpan.FromSeconds(StunDuration));
                    Effects.SendBoltEffect(m, true, 0);
                    break; // Only strike one player per tick
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

        public ThunderstormTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(LightningDamage);
            writer.Write(StunDuration);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            LightningDamage = reader.ReadInt();
            StunDuration = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), StrikeRandomPlayer);
            StartDeleteTimer();
        }
    }
}