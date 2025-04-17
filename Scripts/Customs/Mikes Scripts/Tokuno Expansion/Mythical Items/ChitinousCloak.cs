using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChitinousCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ChitinousCloak()
        {
            Weight = 3.0;
            Name = "Chitinous Cloak";
            Hue = 2406; // Dark insect-like hue

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusStam = 25;
            Attributes.RegenStam = 5;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);

            Resistances.Physical = 12;
            Resistances.Poison = 15;
            Resistances.Fire = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ChitinousCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel attuned to the hive, able to command more creatures.");

                // Check if auto-summon is enabled and start the summon timer accordingly
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer if autosummon is enabled
                    StopSummonTimer();
                    m_Timer = new SummonBlackSolenWarriorTimer(pm);
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
                pm.SendMessage(37, "You feel less attuned to the hive.");
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
            list.Add("Summons Black Solen Warriors");
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
                // Check if auto-summon is enabled and start the summon timer accordingly
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonBlackSolenWarriorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonBlackSolenWarriorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonBlackSolenWarriorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is ChitinousCloak))
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
                    BlackSolenWarrior solen = new BlackSolenWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    solen.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Black Solen Warrior emerges to fight for you!");
                }
            }
        }
    }
}
