using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DoomcallersMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DoomcallersMantle()
        {
            Weight = 1.0;
            Name = "Doomcaller's Mantle";
            Hue = 1175; // Dark, ominous hue

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 15;
            Attributes.SpellDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            Resistances.Physical = 10;
            Resistances.Fire = 15;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 15;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DoomcallersMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "Dark forces gather around you, increasing your command over minions.");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonArchDaemonTimer(pm);
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
                pm.SendMessage(37, "The dark forces recede, and your power to command minions lessens.");
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
            list.Add("Summons ArchDaemons");
            list.Add("Increases maximum followers");
            list.Add("Harnesses the power of the abyss");
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
                m_Timer = new SummonArchDaemonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonArchDaemonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonArchDaemonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is DoomcallersMantle))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ArchDaemon daemon = new ArchDaemon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    daemon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An ArchDaemon answers your call from the abyss!");
                }
            }
        }
    }
}
