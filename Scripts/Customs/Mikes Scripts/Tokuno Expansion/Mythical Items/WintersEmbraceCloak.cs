using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WintersEmbraceCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WintersEmbraceCloak()
        {
            Weight = 1.0;
            Name = "Winter's Embrace Cloak";
            Hue = 1152; // Icy blue hue

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 3;
            Attributes.BonusMana = 25;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            Resistances.Cold = 20;
            Resistances.Energy = 15;
            Resistances.Physical = 10;

            SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);
            SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WintersEmbraceCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a chill as the spirits of winter heed your call.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonLadyOfTheSnowTimer(pm);
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
                pm.SendMessage(37, "The spirits of winter retreat, leaving you warmer but less powerful.");
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
            list.Add("Summons Ladies of the Snow");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonLadyOfTheSnowTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonLadyOfTheSnowTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLadyOfTheSnowTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is WintersEmbraceCloak))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    LadyOfTheSnow lady = new LadyOfTheSnow
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    lady.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Lady of the Snow emerges from the frost to serve you!");
                }
            }
        }
    }
}
