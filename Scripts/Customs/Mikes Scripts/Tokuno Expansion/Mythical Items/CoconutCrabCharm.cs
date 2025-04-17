using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CoconutCrabCharm : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CoconutCrabCharm()
        {
            Weight = 1.0;
            Name = "Coconut Crab Charm";
            Hue = 1164; // Oceanic blue, can be adjusted for aesthetics

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 10;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 15;
            Attributes.Luck = 150;
            Attributes.SpellDamage = 5;

            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public CoconutCrabCharm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the presence of mighty crustaceans ready to aid you!");

                // Only start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonCoconutCrabTimer(pm);
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
                pm.SendMessage(37, "The crabs retreat back to the ocean.");
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
            list.Add("Summons Coconut Crabs");
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
                m_Timer = new SummonCoconutCrabTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                    m_Timer.Start();
            }
        }

        private class SummonCoconutCrabTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCoconutCrabTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is CoconutCrabCharm))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and there is space for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CoconutCrab crab = new CoconutCrab
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    crab.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A mighty Coconut Crab emerges from the sand to aid you!");
                }
            }
        }
    }
}
