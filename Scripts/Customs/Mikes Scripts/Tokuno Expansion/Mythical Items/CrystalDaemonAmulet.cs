using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CrystalDaemonAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CrystalDaemonAmulet()
        {
            Weight = 1.0;
            Name = "Amulet of Crystal Dominion";
            Hue = 1153; // Crystal-like hue

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 10;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.DefendChance = 10;

            Resistances.Physical = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Energy = 15;

            SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 25.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public CrystalDaemonAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Crystal Daemons flows through you, enhancing your command!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonCrystalDaemonTimer(pm);
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
                pm.SendMessage(37, "The connection to the Crystal Daemons fades.");
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
            list.Add("Summons Crystal Daemons");
            list.Add("Increases maximum followers by 2");
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
                    m_Timer = new SummonCrystalDaemonTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonCrystalDaemonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCrystalDaemonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is CrystalDaemonAmulet))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CrystalDaemon daemon = new CrystalDaemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    daemon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Crystal Daemon materializes to serve you!");
                }
            }
        }
    }
}
