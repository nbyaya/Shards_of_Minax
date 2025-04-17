using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SolenQueenHiveBand : GoldRing
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SolenQueenHiveBand()
        {
            Weight = 1.0;
            Name = "Solen Queen Hive Band";
            Hue = 1175; // A deep green hue for the insect theme

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;
            Resistances.Poison = 20; // High poison resistance for the insect theme
            Resistances.Physical = 10;
            
            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SolenQueenHiveBand(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The hive empowers your leadership, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSolenQueenTimer(pm);
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
                pm.SendMessage(37, "The hive's influence fades, and your control over creatures diminishes.");
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
            list.Add("Summons Black Solen Queen");
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
                    m_Timer = new SummonSolenQueenTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSolenQueenTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSolenQueenTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Ring) is SolenQueenHiveBand))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BlackSolenQueen solenQueen = new BlackSolenQueen
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    solenQueen.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Black Solen Queen emerges from the hive to serve you!");
                }
            }
        }
    }
}
