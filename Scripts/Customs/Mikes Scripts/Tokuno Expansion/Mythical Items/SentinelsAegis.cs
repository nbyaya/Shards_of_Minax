using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SentinelsAegis : HeaterShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SentinelsAegis()
        {
            Weight = 6.0;
            Name = "Sentinel's Aegis";
            Hue = 1153; // A radiant, protective hue

            // Set attributes and bonuses
            Attributes.DefendChance = 20;
            Attributes.RegenHits = 5;
            Attributes.BonusStr = 10;
            Attributes.ReflectPhysical = 15;

            SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SentinelsAegis(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to lead more Guardians!");

                // Check if autosummon is enabled and start the summon timer if it is
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer if autosummon is enabled
                    StopSummonTimer();
                    m_Timer = new SummonGuardianTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding multiple Guardians.");
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
            list.Add("Summons Guardian minions to aid you in battle");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGuardianTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGuardianTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGuardianTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is SentinelsAegis))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Guardian guardian = new Guardian
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    guardian.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Guardian emerges to protect you!");
                }
            }
        }
    }
}
