using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScepterOfTheVoidWanderer : Scepter
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScepterOfTheVoidWanderer()
        {
            Weight = 5.0;
            Name = "Scepter of the Void Wanderer";
            Hue = 1175; // A deep, void-like purple

            // Set attributes and bonuses
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 20;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 15;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 150;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public ScepterOfTheVoidWanderer(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of power, allowing you to command more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonVoidWandererTimer(pm);
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
                pm.SendMessage(37, "You feel your connection to the void weaken.");
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
            list.Add("Summons Wanderers of the Void");
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
                m_Timer = new SummonVoidWandererTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonVoidWandererTimer : Timer
        {
            private Mobile m_Owner;

            public SummonVoidWandererTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is ScepterOfTheVoidWanderer))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    WandererOfTheVoid wanderer = new WandererOfTheVoid
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wanderer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wanderer of the Void emerges from the shadows to serve you!");
                }
            }
        }
    }
}