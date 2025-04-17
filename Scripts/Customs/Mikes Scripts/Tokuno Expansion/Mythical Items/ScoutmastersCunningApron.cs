using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScoutmastersCunningApron : FullApron
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScoutmastersCunningApron() : base()
        {
            Weight = 1.0;
            Name = "Scoutmaster's Cunning Apron";
            Hue = 63; // Greenish hue for goblins

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusStam = 10;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;
            Attributes.Luck = 100;
            Attributes.RegenStam = 3;

            SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(1, SkillName.Hiding, 15.0);

            Resistances.Physical = 5;
            Resistances.Poison = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public ScoutmastersCunningApron(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an unseen force ready to obey your commands.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGreenGoblinScoutTimer(pm);
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
                pm.SendMessage(37, "The unseen force dissipates.");
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
            list.Add("Summons Green Goblin Scouts");
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
                // Only start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGreenGoblinScoutTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGreenGoblinScoutTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGreenGoblinScoutTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is ScoutmastersCunningApron))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for followers
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner) || m_Owner.Followers >= m_Owner.FollowersMax)
                    return;

                GreenGoblinScout goblin = new GreenGoblinScout
                {
                    Controlled = true,
                    ControlMaster = m_Owner
                };

                goblin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                m_Owner.SendMessage(38, "A Green Goblin Scout emerges from the shadows to aid you!");
            }
        }
    }
}
