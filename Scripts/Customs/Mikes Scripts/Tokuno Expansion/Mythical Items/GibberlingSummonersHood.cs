using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GibberlingSummonersHood : HoodedShroudOfShadows
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GibberlingSummonersHood()
        {
            Weight = 2.0;
            Name = "Gibberling Summoner's Hood";
            Hue = 1109; // Shadowy, chaotic hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusDex = 10;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.Luck = 200;
            Attributes.NightSight = 1;

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 20.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GibberlingSummonersHood(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "A chaotic aura surrounds you, allowing you to command more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonGibberlingTimer(pm);
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
                pm.SendMessage(37, "The chaotic aura fades, and you feel less in control.");
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
            list.Add("Summons Gibberlings");
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
                m_Timer = new SummonGibberlingTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGibberlingTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGibberlingTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is GibberlingSummonersHood))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Gibberling gibberling = new Gibberling
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    gibberling.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gibberling emerges from the shadows to serve you!");
                }
            }
        }
    }
}
