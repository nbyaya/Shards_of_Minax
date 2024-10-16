using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class PoisonTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public TimeSpan AutoDelete { get; set; } // Duration before auto-deletion

        [Constructable]
        public PoisonTile() : base(0x122A) // Use the appropriate item ID for a poison tile
        {
            Movable = false;
            Name = "a deadly poison tile";
            Hue = 2174; // Poison green color

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
                if (m.Alive && !m.Poisoned) // Check if the mobile is a player and not already poisoned
                {
                    m.ApplyPoison(m, Poison.Deadly); // Apply deadly poison
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

        public PoisonTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), CheckForPlayers);
            StartDeleteTimer(); // Restart the delete timer on server restart
        }
    }
}
