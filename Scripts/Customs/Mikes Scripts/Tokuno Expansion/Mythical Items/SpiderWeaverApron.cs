using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SpiderWeaverApron : FullApron
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SpiderWeaverApron()
        {
            Weight = 2.0;
            Name = "Spider Weaver Apron";
            Hue = 1157; // Dark spider-like hue

            // Set attributes and bonuses
            Attributes.BonusDex = 8;
            Attributes.RegenMana = 3;
            Attributes.RegenStam = 3;
            Attributes.SpellDamage = 8;
            Attributes.Luck = 150;
            Attributes.DefendChance = 10;

            Resistances.Physical = 8;
            Resistances.Poison = 12;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SpiderWeaverApron(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWolfSpiderTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding creatures.");
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
            list.Add("Summons Wolf Spiders");
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
                    m_Timer = new SummonWolfSpiderTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWolfSpiderTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWolfSpiderTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SpiderWeaverApron))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    WolfSpider spider = new WolfSpider
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spider.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wolf Spider scurries forth to serve you!");
                }
            }
        }
    }
}
