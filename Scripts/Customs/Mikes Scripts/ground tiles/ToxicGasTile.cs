using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class ToxicGasTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int Damage { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public ToxicGasTile() : base(0x1D3B) // Use an appropriate item ID for a gas tile
        {
            Movable = false;
            Name = "a toxic gas tile";
            Hue = 0x3F; // Greenish color for gas

            Damage = 10; // Default poison damage

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
                    m.Damage(Damage, m); // Inflict poison damage
                    m.SendMessage("You are choking on the toxic gas!");
                    m.Stam -= 10; // Reduce stamina
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

        public ToxicGasTile(Serial serial) : base(serial)
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
