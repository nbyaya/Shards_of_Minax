using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SerpentmastersRing : GoldRing
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SerpentmastersRing()
        {
            Weight = 1.0;
            Name = "Serpentmaster's Ring";
            Hue = 1164; // Serpentine green hue

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusHits = 10;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 15;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 100;

            Resistances.Physical = 8;
            Resistances.Poison = 20;
            Resistances.Fire = 5;
            Resistances.Cold = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SerpentmastersRing(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the presence of serpentine allies at your command!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSerpentTimer(pm);
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
                pm.SendMessage(37, "The serpentine allies slip away from your grasp.");
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
            list.Add("Summons Giant Serpents");
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

            // Reinitialize timer if equipped on restart and autosummon is enabled
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonSerpentTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSerpentTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSerpentTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid, the item is not equipped, or autosummon is off
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Ring) is SerpentmastersRing) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GiantSerpent serpent = new GiantSerpent
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    serpent.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Giant Serpent slithers forth to aid you!");
                }
            }
        }
    }
}
