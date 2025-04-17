using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class YomotsuWarlordsYumi : Yumi
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public YomotsuWarlordsYumi()
        {
            Weight = 5.0;
            Name = "Yomotsu Warlord's Yumi";
            Hue = 1157; // Dark, ominous color

            // Set attributes and bonuses
            Attributes.WeaponDamage = 25;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 10;
            Attributes.WeaponSpeed = 20;
            Attributes.Luck = 50;
            Attributes.SpellChanneling = 1; // Allows spellcasting while equipped

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Archery, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
            SkillBonuses.SetValues(2, SkillName.Bushido, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public YomotsuWarlordsYumi(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you could command more creatures now!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonYomotsuWarriorTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
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
            list.Add("Summons Yomotsu Warriors");
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
                m_Timer = new SummonYomotsuWarriorTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonYomotsuWarriorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonYomotsuWarriorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is YomotsuWarlordsYumi))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    YomotsuWarrior warrior = new YomotsuWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    warrior.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Yomotsu Warrior emerges to serve you!");
                }
            }
        }
    }
}
