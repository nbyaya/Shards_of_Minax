using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ProtectorsGuardianShield : HeaterShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ProtectorsGuardianShield()
        {
            Weight = 5.0;
            Name = "Protector's Guardian Shield";
            Hue = 1153; // A radiant blue hue

            // Set attributes and bonuses
            Attributes.DefendChance = 20;
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 3;
            Attributes.ReflectPhysical = 15;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Parry, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ProtectorsGuardianShield(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a strong presence that aids in protecting others!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                // Check if autosummon is enabled before starting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonProtectorTimer(pm);
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
                pm.SendMessage(37, "You feel the protective aura fade away.");
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
            list.Add("Summons Protectors");
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
                // Check if autosummon is enabled before starting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonProtectorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonProtectorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonProtectorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is ProtectorsGuardianShield))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the owner has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Protector protector = new Protector
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    protector.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Protector emerges to guard you!");
                }
            }
        }
    }
}
