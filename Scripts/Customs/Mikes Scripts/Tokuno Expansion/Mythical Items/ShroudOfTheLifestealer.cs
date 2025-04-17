using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShroudOfTheLifestealer : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShroudOfTheLifestealer()
        {
            Weight = 1.0;
            Name = "Shroud of the Lifestealer";
            Hue = 1175; // Dark crimson color

            // Set attributes and bonuses
            Attributes.BonusHits = 25;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.ReflectPhysical = 10;
            Attributes.WeaponDamage = 15;
            Attributes.NightSight = 1;

            Resistances.Physical = 12;
            Resistances.Fire = 10;
            Resistances.Cold = 8;
            Resistances.Poison = 15;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ShroudOfTheLifestealer(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the shadows surge around you, bolstering your control!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonLifestealerTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))  // Check autosummon toggle
                {
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
                pm.SendMessage(37, "The shadows recede, and your command weakens.");
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
            list.Add("Summons Lifestealers");
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
                m_Timer = new SummonLifestealerTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))  // Check autosummon toggle
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonLifestealerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLifestealerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is ShroudOfTheLifestealer))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Lifestealer lifestealer = new Lifestealer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    lifestealer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Lifestealer emerges from the shadows to serve you!");
                }
            }
        }
    }
}
