using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class MechanistsGloves : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public MechanistsGloves()
        {
            Weight = 1.0;
            Name = "Mechanist's Gloves";
            Hue = 2413; // Metallic silver color, representing clockwork theme

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusInt = 10;
            Attributes.LowerManaCost = 5;
            Attributes.WeaponDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.RegenStam = 3;

            SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);
            SkillBonuses.SetValues(1, SkillName.Fencing, 15.0);

            PhysicalBonus = 8;
            FireBonus = 10;
            ColdBonus = 5;
            PoisonBonus = 8;
            EnergyBonus = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public MechanistsGloves(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel capable of commanding more creations now!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonClockworkScorpionTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding your creations.");
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
            list.Add("Summons Clockwork Scorpions");
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
                // Check if autosummon is enabled on restart
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonClockworkScorpionTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonClockworkScorpionTimer : Timer
        {
            private Mobile m_Owner;

            public SummonClockworkScorpionTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is MechanistsGloves))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ClockworkScorpion scorpion = new ClockworkScorpion
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    scorpion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Clockwork Scorpion assembles to serve you!");
                }
            }
        }
    }
}
