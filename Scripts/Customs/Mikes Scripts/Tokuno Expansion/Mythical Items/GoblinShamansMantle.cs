using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GoblinShamansMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GoblinShamansMantle()
        {
            Weight = 1.0;
            Name = "Goblin Shaman's Mantle";
            Hue = 2425; // A greenish hue for a goblin theme

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 12;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GoblinShamansMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the chaotic energy of goblins empowering you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonGrayGoblinTimer(pm);
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
                pm.SendMessage(37, "The chaotic energy fades, and you feel less capable of commanding creatures.");
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
            list.Add("Summons Gray Goblins");
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
                m_Timer = new SummonGrayGoblinTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGrayGoblinTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGrayGoblinTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is GoblinShamansMantle))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GrayGoblin goblin = new GrayGoblin
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    goblin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gray Goblin appears, ready to fight at your command!");
                }
            }
        }
    }
}
