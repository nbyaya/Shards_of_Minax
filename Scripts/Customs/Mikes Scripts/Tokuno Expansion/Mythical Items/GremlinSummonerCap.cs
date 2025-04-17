using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GremlinSummonerCap : LeatherCap
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GremlinSummonerCap()
        {
            Weight = 1.0;
            Name = "Gremlin Summoner's Cap";
            Hue = 1165; // Dark mischievous green color

            // Set attributes and bonuses
            Attributes.BonusDex = 25;
            Attributes.BonusMana = 10;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;
            
            SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);

            PhysicalBonus = 8;
            FireBonus = 6;
            ColdBonus = 10;
            PoisonBonus = 8;
            EnergyBonus = 6;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GremlinSummonerCap(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel mischievous allies gathering around you!");

                // Start summon timer if auto summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))  // Check if auto summon is enabled
                {
                    m_Timer = new SummonGremlinTimer(pm);
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
                pm.SendMessage(37, "The mischievous presence fades.");
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
            list.Add("Summons Gremlins");
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
                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGremlinTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGremlinTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGremlinTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is GremlinSummonerCap))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Gremlin gremlin = new Gremlin
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    gremlin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A mischievous Gremlin appears to assist you!");
                }
            }
        }
    }
}
