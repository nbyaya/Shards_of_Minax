using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class InfernalPactCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public InfernalPactCloak()
        {
            Weight = 1.0;
            Name = "Infernal Pact Cloak";
            Hue = 1157; // Dark red with a demonic vibe

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.BonusMana = 30;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 15;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            Resistances.Physical = 5;
            Resistances.Fire = 15;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public InfernalPactCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the dark power of the infernal pact coursing through you.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonDaemonTimer(pm);
                m_Timer.Start();
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The infernal power fades, leaving you feeling diminished.");
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
            list.Add("Summons Daemons");
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
                m_Timer = new SummonDaemonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonDaemonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDaemonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is InfernalPactCloak))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Daemon daemon = new Daemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    daemon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Daemon answers your infernal call!");
                }
            }
        }
    }
}
