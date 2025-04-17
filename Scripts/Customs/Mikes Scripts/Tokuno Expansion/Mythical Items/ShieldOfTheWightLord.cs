using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShieldOfTheWightLord : HeaterShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShieldOfTheWightLord()
        {
            Weight = 5.0;
            Name = "Shield of the Wight Lord";
            Hue = 1157; // Dark spectral blue color

            // Set attributes and bonuses
            Attributes.DefendChance = 20;
            Attributes.ReflectPhysical = 15;
            Attributes.BonusHits = 10;
            Attributes.RegenHits = 3;
            Attributes.Luck = 75;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 12;
            ColdBonus = 15;
            PoisonBonus = 10;

            // Skill Bonus
            SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ShieldOfTheWightLord(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power to command more undead!");

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer if autosummon is enabled
                    StopSummonTimer();
                    m_Timer = new SummonWightTimer(pm);
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
                pm.SendMessage(37, "The spectral power fades, and your command weakens.");
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
            list.Add("Summons Wights");
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
                // Check if autosummon is enabled on load
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonWightTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWightTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWightTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust interval as needed
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is ShieldOfTheWightLord))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Wight wight = new Wight
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wight.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wight emerges from the shadows to serve you!");
                }
            }
        }
    }
}
