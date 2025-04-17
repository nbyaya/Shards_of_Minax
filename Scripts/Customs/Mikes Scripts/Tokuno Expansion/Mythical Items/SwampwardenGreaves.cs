using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SwampwardenGreaves : PlateLegs
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SwampwardenGreaves() 
        {
            Weight = 5.0;
            Name = "Swampwarden Greaves";
            Hue = 2209; // Swampy green color

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusHits = 15;
            Attributes.RegenStam = 3;
            Attributes.ReflectPhysical = 10;
            Attributes.LowerManaCost = 5;

            // Resistances
            PhysicalBonus = 10;
            PoisonBonus = 15;
            FireBonus = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SwampwardenGreaves(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command additional creatures!");

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer
                    StopSummonTimer();
                    m_Timer = new SummonSwampDragonTimer(pm);
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
                pm.SendMessage(37, "You feel your ability to command creatures diminish.");
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
            list.Add("Summons Swamp Dragons");
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
                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSwampDragonTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSwampDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSwampDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Pants) is SwampwardenGreaves))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SwampDragon dragon = new SwampDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Swamp Dragon rises from the mire to serve you!");
                }
            }
        }
    }
}
