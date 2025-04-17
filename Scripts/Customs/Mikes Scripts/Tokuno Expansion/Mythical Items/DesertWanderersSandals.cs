using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DesertWanderersSandals : Sandals
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DesertWanderersSandals()
        {
            Weight = 1.0;
            Name = "Desert Wanderer's Sandals";
            Hue = 1153; // Sandy hue

            // Set attributes and bonuses
            Attributes.BonusDex = 10;
            Attributes.BonusStam = 10;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            Resistances.Physical = 5;
            Resistances.Fire = 10;
            Resistances.Poison = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DesertWanderersSandals(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a deep connection to the desert's creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonDesertOstardTimer(pm);
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
                pm.SendMessage(37, "Your connection to the desert's creatures weakens.");
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
            list.Add("Summons Desert Ostards");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonDesertOstardTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonDesertOstardTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDesertOstardTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Shoes) is DesertWanderersSandals))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DesertOstard ostard = new DesertOstard
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ostard.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Desert Ostard emerges from the sands to serve you!");
                }
            }
        }
    }
}
