using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BagOfJuice : Bag
    {
        private Timer m_Timer;
        private int m_AgilityCount;
        private int m_RefreshCount;
        private int m_StrengthCount;

        [Constructable]
        public BagOfJuice()
        {
            Name = "Bag of Juice";
            Hue = 1176; // Change to any Hue you like

            m_AgilityCount = 0;
            m_RefreshCount = 0;
            m_StrengthCount = 0;

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        public BagOfJuice(Serial serial)
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

            if (item is GreaterAgilityPotion) m_AgilityCount--;
            if (item is TotalRefreshPotion) m_RefreshCount--;
            if (item is GreaterStrengthPotion) m_StrengthCount--;

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

            writer.Write(m_AgilityCount);
            writer.Write(m_RefreshCount);
            writer.Write(m_StrengthCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AgilityCount = reader.ReadInt();
            m_RefreshCount = reader.ReadInt();
            m_StrengthCount = reader.ReadInt();

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        private class InternalTimer : Timer
        {
            private BagOfJuice m_Bag;
            private int m_NextPotion;

            public InternalTimer(BagOfJuice bag)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.OneSecond;
                m_Bag = bag;
                m_NextPotion = 0;
            }

            protected override void OnTick()
            {
                if (m_Bag == null || m_Bag.Deleted)
                {
                    Stop();
                    return;
                }

                if (m_Bag.m_AgilityCount < 5 && m_NextPotion == 0)
                {
                    m_Bag.DropItem(new GreaterAgilityPotion());
                    m_Bag.m_AgilityCount++;
                }

                if (m_Bag.m_RefreshCount < 5 && m_NextPotion == 1)
                {
                    m_Bag.DropItem(new TotalRefreshPotion());
                    m_Bag.m_RefreshCount++;
                }

                if (m_Bag.m_StrengthCount < 5 && m_NextPotion == 2)
                {
                    m_Bag.DropItem(new GreaterStrengthPotion());
                    m_Bag.m_StrengthCount++;
                }

                m_NextPotion = (m_NextPotion + 1) % 3;

                if (m_Bag.m_AgilityCount >= 5 && m_Bag.m_RefreshCount >= 5 && m_Bag.m_StrengthCount >= 5)
                {
                    Stop();
                }
            }
        }
    }
}
