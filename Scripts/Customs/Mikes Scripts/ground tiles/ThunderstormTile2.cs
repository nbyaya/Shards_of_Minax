using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class ThunderstormTile2 : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int Damage { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public ThunderstormTile2() : base(0x0E5C) // Use an appropriate item ID for a storm tile
        {
            Movable = false;
            Name = "a thunderstorm tile";
            Hue = 0x8A; // Stormy color

            Damage = 15; // Default electrical damage

            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);

            AutoDelete = TimeSpan.FromSeconds(15); // Set the auto-delete time
            StartDeleteTimer();
        }

        private void CheckForPlayers()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(0);
            foreach (Mobile m in eable)
            {
                if (m.Alive)
                {
                    m.Damage(Damage, m); // Inflict electrical damage
                    m.SendMessage("You are struck by a bolt of lightning!");
                    if (Utility.RandomDouble() < 0.3) // 30% chance to paralyze
                    {
                        m.SendMessage("You are paralyzed by the storm!");
                        m.Paralyze(TimeSpan.FromSeconds(5)); // Paralyze for 5 seconds
                    }
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
            this.Delete(); // Automatically delete the tile
        }

        public ThunderstormTile2(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(Damage);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Damage = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);
            StartDeleteTimer();
        }
    }
}
