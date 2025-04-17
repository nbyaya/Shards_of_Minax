using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class JukaWarlordsCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public JukaWarlordsCloak()
        {
            Weight = 5.0;
            Name = "Juka Warlord's Cloak";
            Hue = 1157; // Dark blue/metallic hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusHits = 25;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 8;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
            SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public JukaWarlordsCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The might of the Juka strengthens your resolve to lead!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonJukaWarriorTimer(pm);
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
                pm.SendMessage(37, "You feel the presence of the Juka fading.");
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
            list.Add("Summons Juka Warriors");
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
                m_Timer = new SummonJukaWarriorTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonJukaWarriorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonJukaWarriorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is JukaWarlordsCloak))
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
                    JukaWarrior warrior = new JukaWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    warrior.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Juka Warrior emerges to serve you!");
                }
            }
        }
    }
}
