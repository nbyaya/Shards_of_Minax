using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BagOfBombs : Bag
    {
        private Timer m_Timer;
        private int m_PotionCount;

        [Constructable]
        public BagOfBombs()
        {
            Name = "Bag of Bombs";
            Hue = 1153; // Change to any Hue you like

            m_PotionCount = 0;
            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        public BagOfBombs(Serial serial)
            : base(serial)
        {
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void OnItemRemoved(Item item)
        {
            base.OnItemRemoved(item);

            if (item is GreaterExplosionPotion)
            {
                m_PotionCount--;
            }
            
            CheckAndRestartTimer();
        }

        public void CheckAndRestartTimer()
        {
            if (m_Timer == null || !m_Timer.Running)
            {
                m_Timer = new InternalTimer(this);
                m_Timer.Start();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Version
            writer.Write(m_PotionCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_PotionCount = reader.ReadInt();

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        private class InternalTimer : Timer
        {
            private BagOfBombs m_Bag;

            public InternalTimer(BagOfBombs bag)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.OneSecond;
                m_Bag = bag;
            }

            protected override void OnTick()
            {
                if (m_Bag == null || m_Bag.Deleted)
                {
                    Stop();
                    return;
                }

                if (m_Bag.m_PotionCount < 20)
                {
                    m_Bag.DropItem(new GreaterExplosionPotion());
                    m_Bag.m_PotionCount++;
                }

                if (m_Bag.m_PotionCount >= 20)
                {
                    Stop();
                }
            }
        }
    }
}
