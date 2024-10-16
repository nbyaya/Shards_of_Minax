using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class BloodSword : BaseSword
    {
        public Timer m_Timer;

        [Constructable]
        public BloodSword() : base(0xF5E)
        {
            Name = "Blood Sword";
            Hue = 33;
            InitializeTimer();
        }

        public BloodSword(Serial serial) : base(serial) { }

        private void InitializeTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
            }
            m_Timer = new SummonBloodElementalTimer(this);
            m_Timer.Start();
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Blood Elementals");
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

        private class SummonBloodElementalTimer : Timer
        {
            private BloodSword m_Sword;

            public SummonBloodElementalTimer(BloodSword sword) : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Sword = sword;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                Mobile owner = m_Sword.Parent as Mobile;
                if (owner != null && owner.Followers < owner.FollowersMax)
                {
                    BloodElemental elemental = new BloodElemental();
                    elemental.Controlled = true;
                    elemental.ControlMaster = owner;
                    elemental.MoveToWorld(owner.Location, owner.Map);
                }
            }
        }
    }
}
