using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class RodOfOrcControl : BaseStaff
    {
        public Timer m_Timer;

        [Constructable]
        public RodOfOrcControl() : base(0xE81)
        {
            Name = "Rod of Orc Control";
            Hue = 38;
            InitializeTimer();
        }

        public RodOfOrcControl(Serial serial) : base(serial) { }

        private void InitializeTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
            }
            m_Timer = new SummonOrcTimer(this);
            m_Timer.Start();
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Controlled Orcs");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize the timer after server restarts
            InitializeTimer();
        }

        private class SummonOrcTimer : Timer
        {
            private RodOfOrcControl m_Rod;

            public SummonOrcTimer(RodOfOrcControl rod) : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Rod = rod;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                Mobile owner = m_Rod.Parent as Mobile;
                if (owner != null && owner.Followers < owner.FollowersMax)
                {
                    Orc orc = new Orc();
                    orc.Controlled = true;
                    orc.ControlMaster = owner;
                    orc.MoveToWorld(owner.Location, owner.Map);
                }
            }
        }
    }
}
