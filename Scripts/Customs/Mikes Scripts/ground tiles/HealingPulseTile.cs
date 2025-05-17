using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class HealingPulseTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int HealAmount { get; set; }
        public int AreaRange { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public HealingPulseTile() : base(0x0E5F) // Use an appropriate tile ID
        {
            Movable = false;
            Name = "a healing pulse";
            Hue = 0x48; // Light blue hue

            HealAmount = 10; // Amount of healing per pulse
            AreaRange = 2; // Affects players within 2 tiles
            AutoDelete = TimeSpan.FromMinutes(5); // Lasts for 5 minutes

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), HealingPulse);
            StartDeleteTimer();
        }

        private void HealingPulse()
        {
            if (Deleted) return;

            // Visual effect at the tile's location
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 
                0x376A, 9, 20, 5040, 0, 5042, 0);
            Effects.PlaySound(Location, Map, 0x202); // Healing sound

            IPooledEnumerable eable = GetMobilesInRange(AreaRange);
            foreach (Mobile m in eable)
            {
                if (m.Alive)
                {
                    // Heal the mobile
                    m.Heal(HealAmount);

                    // Remove poison
                    if (m.Poisoned)
                    {
                        m.CurePoison(m);
                    }

                    // Remove any other negative effects here
                    // For example, removing paralysis:
                    if (m.Paralyzed)
                    {
                        m.Paralyzed = false;
                    }

                    // Visual effect on the healed mobile
                    m.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                    m.PlaySound(0x1F2); // Cure sound
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

        public HealingPulseTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(HealAmount);
            writer.Write(AreaRange);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            HealAmount = reader.ReadInt();
            AreaRange = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), HealingPulse);
            StartDeleteTimer();
        }
    }
}