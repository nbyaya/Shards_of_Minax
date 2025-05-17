using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EarthquakeTile : Item
    {
        private Timer m_Timer;
        private Timer m_DeleteTimer;
        public int ShakeIntensity { get; set; }
        public int DropChance { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public EarthquakeTile() : base(0x0E65) // Use an appropriate ground crack tile ID
        {
            Movable = false;
            Name = "an earthquake fissure";
            Hue = 1147; // Earthy brown color

            ShakeIntensity = 3; // Intensity of the shake effect
            DropChance = 5; // 5% chance to drop an item
            AutoDelete = TimeSpan.FromSeconds(60);

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), CauseEarthquake);
            StartDeleteTimer();
        }

        private void CauseEarthquake()
        {
            if (Deleted) return;

            IPooledEnumerable eable = GetMobilesInRange(5); // Affects players within 5 tiles
            foreach (Mobile m in eable)
            {
                if (m.Alive && m is PlayerMobile)
                {
                    m.SendMessage("The earth beneath your feet begins to shake!");
                    m.Animate(32, 5, 1, true, false, 0); // Play a stumble animation

                    // Chance to drop a random item
                    if (Utility.Random(100) < DropChance)
                    {
                        Item toDrop = m.FindItemOnLayer(Layer.OneHanded);
                        if (toDrop == null) toDrop = m.FindItemOnLayer(Layer.TwoHanded);

                        if (toDrop != null && toDrop.Movable)
                        {
                            m.AddToBackpack(toDrop);
                            m.SendMessage("The ground shakes and the trembling causes you to drop an item in your backpack!");
                        }
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
            this.Delete();
        }

        public EarthquakeTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(ShakeIntensity);
            writer.Write(DropChance);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            ShakeIntensity = reader.ReadInt();
            DropChance = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), CauseEarthquake);
            StartDeleteTimer();
        }
    }
}
