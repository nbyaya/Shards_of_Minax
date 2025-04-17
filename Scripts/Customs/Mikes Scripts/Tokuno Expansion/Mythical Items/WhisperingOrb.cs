using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WhisperingOrb : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WhisperingOrb()
        {
            Weight = 1.0;
            Name = "Whispering Orb";
            Hue = 1153; // Mysterious dark bluish hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            
            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 15;

            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0);
            SkillBonuses.SetValues(2, SkillName.EvalInt, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WhisperingOrb(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The whispers of the orb grant you greater control over your followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonEowmuTimer(pm);
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
                pm.SendMessage(37, "The whispers fade, and your control weakens.");
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
            list.Add("Summons Eowmu to aid you");
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
                // Check if autosummon is enabled before starting timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonEowmuTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonEowmuTimer : Timer
        {
            private Mobile m_Owner;

            public SummonEowmuTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Talisman) is WhisperingOrb))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Eowmu eowmu = new Eowmu
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    eowmu.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Eowmu emerges to aid you!");
                }
            }
        }
    }
}
