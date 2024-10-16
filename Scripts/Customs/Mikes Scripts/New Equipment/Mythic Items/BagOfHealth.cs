using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BagOfHealth : Bag
    {
        private Timer m_Timer;
        private int m_CurePotionCount;
        private int m_HealPotionCount;
        private bool m_NextIsCure;

        [Constructable]
        public BagOfHealth()
        {
            Name = "Bag of Health";
            Hue = 1151; // Change to any Hue you like

            m_CurePotionCount = 0;
            m_HealPotionCount = 0;
            m_NextIsCure = true;

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        public BagOfHealth(Serial serial)
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

            if (item is GreaterCurePotion)
            {
                m_CurePotionCount--;
            }
            else if (item is GreaterHealPotion)
            {
                m_HealPotionCount--;
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
            writer.Write(m_CurePotionCount);
            writer.Write(m_HealPotionCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_CurePotionCount = reader.ReadInt();
            m_HealPotionCount = reader.ReadInt();

            m_Timer = new InternalTimer(this);
            m_Timer.Start();
        }

        private class InternalTimer : Timer
        {
            private BagOfHealth m_Bag;

            public InternalTimer(BagOfHealth bag)
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

                if (m_Bag.m_CurePotionCount < 10 && m_Bag.m_NextIsCure)
                {
                    m_Bag.DropItem(new GreaterCurePotion());
                    m_Bag.m_CurePotionCount++;
                    m_Bag.m_NextIsCure = false;
                }
                else if (m_Bag.m_HealPotionCount < 10 && !m_Bag.m_NextIsCure)
                {
                    m_Bag.DropItem(new GreaterHealPotion());
                    m_Bag.m_HealPotionCount++;
                    m_Bag.m_NextIsCure = true;
                }

                if (m_Bag.m_CurePotionCount >= 10 && m_Bag.m_HealPotionCount >= 10)
                {
                    Stop();
                }
            }
        }
    }
}
