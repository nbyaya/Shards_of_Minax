using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CopperCrown : NorseHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CopperCrown()
        {
            Weight = 2.0;
            Name = "Crown of the Copper Elementalist";
            Hue = 2413; // Copper-like color

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 30;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 15;
            Attributes.Luck = 150;
            Attributes.ReflectPhysical = 10;
            
            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tinkering, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public CopperCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of copper enhances your command over minions!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonCopperElementalTimer(pm);
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
                pm.SendMessage(37, "You feel less able to command your minions.");
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
            list.Add("Summons Copper Elementals");
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
                m_Timer = new SummonCopperElementalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCopperElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCopperElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is CopperCrown))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CopperElemental elemental = new CopperElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Copper Elemental emerges from the ground to serve you!");
                }
            }
        }
    }
}
