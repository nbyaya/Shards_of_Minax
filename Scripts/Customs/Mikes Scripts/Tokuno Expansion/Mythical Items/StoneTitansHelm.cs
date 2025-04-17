using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StoneTitansHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public StoneTitansHelm()
        {
            Weight = 5.0;
            Name = "Stone Titan's Helm";
            Hue = 2406; // Stone-like hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 15;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 5;

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 8;
            ColdBonus = 8;
            PoisonBonus = 5;
            EnergyBonus = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public StoneTitansHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an ancient power increasing your control over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonStoneMonsterTimer(pm);
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
                pm.SendMessage(37, "The ancient power fades, and your control over creatures weakens.");
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
            list.Add("Summons Stone Monsters");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonStoneMonsterTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonStoneMonsterTimer : Timer
        {
            private Mobile m_Owner;

            public SummonStoneMonsterTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is StoneTitansHelm))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    StoneMonster monster = new StoneMonster
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    monster.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Stone Monster emerges from the ground to aid you!");
                }
            }
        }
    }
}
