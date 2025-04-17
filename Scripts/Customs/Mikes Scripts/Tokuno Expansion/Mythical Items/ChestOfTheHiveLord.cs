using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChestOfTheHiveLord : ChainChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ChestOfTheHiveLord()
        {
            Weight = 10.0;
            Name = "Chains of the Hive Lord";
            Hue = 1161; // A dark, insect-like hue

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 8;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 15;
            PoisonBonus = 20; // Thematic for Terathan Warriors

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);
            SkillBonuses.SetValues(1, SkillName.Parry, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public ChestOfTheHiveLord(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of dominance, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonTerathanWarriorTimer(pm);
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
                pm.SendMessage(37, "Your ability to command creatures diminishes.");
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
            list.Add("Summons Terathan Warriors");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonTerathanWarriorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTerathanWarriorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTerathanWarriorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is ChestOfTheHiveLord))
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
                    TerathanWarrior warrior = new TerathanWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    warrior.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Terathan Warrior emerges from the hive to serve you!");
                }
            }
        }
    }
}
