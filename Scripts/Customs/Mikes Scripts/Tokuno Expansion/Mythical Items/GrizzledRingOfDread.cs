using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GrizzledRingOfDread : GoldRing
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GrizzledRingOfDread()
        {
            Weight = 1.0;
            Name = "Grizzled Ring of Dread";
            Hue = 1175; // A dark, ominous hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 15;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 15;
            Attributes.NightSight = 1;

            Resistances.Physical = 10;
            Resistances.Poison = 10;
            Resistances.Cold = 15;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GrizzledRingOfDread(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "A chilling power surrounds you, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGrizzleTimer(pm);
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
                pm.SendMessage(37, "The chilling power fades, and your command over creatures weakens.");
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
            list.Add("Summons Interred Grizzles");
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
                    m_Timer = new SummonGrizzleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGrizzleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGrizzleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Ring) is GrizzledRingOfDread))
                {
                    Stop();
                    return;
                }

                // Check if auto summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    InterredGrizzle grizzle = new InterredGrizzle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    grizzle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Interred Grizzle rises from the ground to serve you!");
                }
            }
        }
    }
}
