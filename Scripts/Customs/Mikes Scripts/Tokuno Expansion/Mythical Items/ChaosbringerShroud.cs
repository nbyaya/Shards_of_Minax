using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChaosbringerShroud : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ChaosbringerShroud()
        {
            Weight = 1.0;
            Name = "Chaosbringer's Shroud";
            Hue = 1109; // Dark, chaotic hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusMana = 30;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.Luck = 50;

            Resistances.Physical = 10;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ChaosbringerShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The chaos within you allows you to command more followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonChaosDaemonTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Only start the timer if autosummon is enabled
                {
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
                pm.SendMessage(37, "You feel the chaotic power fading, reducing your ability to command.");
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
            list.Add("Summons Chaos Daemons");
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
                m_Timer = new SummonChaosDaemonTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled upon reload
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonChaosDaemonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonChaosDaemonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is ChaosbringerShroud))
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
                    ChaosDaemon daemon = new ChaosDaemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    daemon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Chaos Daemon emerges to serve you!");
                }
            }
        }
    }
}
