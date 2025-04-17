using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SashOfTheHiryu : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SashOfTheHiryu()
        {
            Weight = 1.0;
            Name = "Sash of the Hiryu";
            Hue = 1157; // A noble blue hue

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusStam = 10;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 15;
            Attributes.RegenStam = 5;
            Attributes.WeaponSpeed = 10;

            SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);

            Resistances.Physical = 15;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SashOfTheHiryu(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The spirit of the Hiryu grants you the ability to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonLesserHiryuTimer(pm);
                    m_Timer.Start();
                }
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The spirit of the Hiryu departs, reducing your command over creatures.");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StopSummonTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Lesser Hiryus");
            list.Add("Increases maximum followers");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_BonusFollowers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_BonusFollowers = reader.ReadInt();

            // Reinitialize timer if equipped on restart
            if (Parent is Mobile mob)
            {
                // Check if autosummon is enabled and restart the timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonLesserHiryuTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonLesserHiryuTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLesserHiryuTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is SashOfTheHiryu))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    LesserHiryu hiryu = new LesserHiryu
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    hiryu.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Lesser Hiryu emerges to serve you!");
                }
            }
        }
    }
}
