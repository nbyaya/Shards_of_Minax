using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WidowsVeil : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WidowsVeil()
        {
            Weight = 1.0;
            Name = "Widow's Veil";
            Hue = 1109; // Dark spider-like color

            // Set attributes and bonuses
            Attributes.BonusDex = 25;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 5;
            Attributes.NightSight = 1;

            Resistances.Physical = 5;
            Resistances.Poison = 15;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WidowsVeil(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Widow's Veil empowers you to control more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGiantBlackWidowTimer(pm);
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
                pm.SendMessage(37, "The Widow's Veil's power fades as you remove it.");
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
            list.Add("Summons Giant Black Widows");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGiantBlackWidowTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGiantBlackWidowTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGiantBlackWidowTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is WidowsVeil))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GiantBlackWidow spider = new GiantBlackWidow
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spider.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Giant Black Widow crawls forth to serve you!");
                }
            }
        }
    }
}
