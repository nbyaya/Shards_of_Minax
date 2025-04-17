using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TroglodyteSummonerKryss : Kryss
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TroglodyteSummonerKryss()
        {
            Weight = 6.0;
            Name = "Troglodyte Summoner's Kryss";
            Hue = 1109; // A deep earthy color

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusStam = 10;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.WeaponDamage = 30;
            Attributes.WeaponSpeed = 15;
            Attributes.NightSight = 1;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Fencing, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public TroglodyteSummonerKryss(Serial serial) : base(serial)
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
                    m_Timer = new SummonTroglodyteTimer(pm);
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
            list.Add("Summons Troglodytes");
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
                    m_Timer = new SummonTroglodyteTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTroglodyteTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTroglodyteTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is TroglodyteSummonerKryss))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Troglodyte troglodyte = new Troglodyte
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    troglodyte.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Troglodyte emerges to serve you!");
                }
            }
        }
    }
}
