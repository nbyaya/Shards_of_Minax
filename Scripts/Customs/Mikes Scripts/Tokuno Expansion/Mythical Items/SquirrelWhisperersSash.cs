using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SquirrelWhisperersSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SquirrelWhisperersSash()
        {
            Weight = 1.0;
            Name = "Squirrel Whisperer's Sash";
            Hue = 1194; // Earthy brown color

            // Set attributes and bonuses
            Attributes.BonusDex = 10; // Squirrels are quick and agile
            Attributes.Luck = 100; // Squirrels bring good fortune
            Attributes.RegenStam = 5; // Squirrels keep you energetic
            Attributes.WeaponSpeed = 10; // Quick reflexes
            Attributes.NightSight = 1; // Squirrels can see in the dark

            // Resistances
            Resistances.Physical = 5;
            Resistances.Poison = 10; // Squirrels are resilient to toxins

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0); // Better at taming animals
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0); // Better understanding of animals

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2
        }

        public SquirrelWhisperersSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a stronger connection to nature, allowing you to command more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSquirrelTimer(pm);
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
                pm.SendMessage(37, "Your connection to nature weakens, and you can no longer command as many creatures.");
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
            list.Add("Summons Squirrels");
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
                    m_Timer = new SummonSquirrelTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSquirrelTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSquirrelTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SquirrelWhisperersSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Squirrel squirrel = new Squirrel
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    squirrel.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A friendly squirrel scurries to your side!");
                }
            }
        }
    }
}
