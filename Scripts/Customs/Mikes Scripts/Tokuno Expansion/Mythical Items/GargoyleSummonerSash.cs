using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GargoyleSummonerSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GargoyleSummonerSash() 
        {
            Weight = 1.0;
            Name = "Gargoyle Summoner's Sash";
            Hue = 2413; // Unique hue for the sash, change as needed

            // Set attributes and bonuses
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 8;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 4;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.Luck = 80;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 6;
            Resistances.Poison = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 10.0);
            SkillBonuses.SetValues(1, SkillName.Mysticism, 10.0);
            SkillBonuses.SetValues(2, SkillName.EvalInt, 5.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GargoyleSummonerSash(Serial serial) : base(serial)
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

                // Check if autosummon is enabled, and start summon timer if so
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StartSummonTimer(pm);
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

        private void StartSummonTimer(PlayerMobile pm)
        {
            // Start summon timer
            StopSummonTimer(); // Ensure no previous timer is running
            m_Timer = new SummonStoneGargoyleTimer(pm);
            m_Timer.Start();
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
            list.Add("Summons Stone Gargoyles");
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
                    m_Timer = new SummonStoneGargoyleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonStoneGargoyleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonStoneGargoyleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is GargoyleSummonerSash))
                {
                    Stop();
                    return;
                }

                // Only summon if auto-summon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    StoneGargoyle gargoyle = new StoneGargoyle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    gargoyle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Stone Gargoyle emerges to serve you!");
                }
            }
        }
    }
}
