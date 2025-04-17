using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SerpentsStride : Boots
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SerpentsStride()
        {
            Weight = 3.0;
            Name = "Serpent's Stride";
            Hue = 1172; // A snake-like green hue

            // Set attributes and bonuses
            Attributes.BonusDex = 8;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 5;
            Attributes.Luck = 75;

            // Resistances
            Resistances.Poison = 15;
            Resistances.Physical = 5;
            Resistances.Fire = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SerpentsStride(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel attuned to serpentine creatures, gaining the ability to command more.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                // Only start the timer if autosummon is enabled for the player
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSnakeTimer(pm);
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
                pm.SendMessage(37, "You feel less connected to serpentine creatures.");
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
            list.Add("Summons Snakes");
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
                // Only start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSnakeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSnakeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSnakeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Shoes) is SerpentsStride))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Snake snake = new Snake
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    snake.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Snake slithers forth to aid you!");
                }
            }
        }
    }
}
