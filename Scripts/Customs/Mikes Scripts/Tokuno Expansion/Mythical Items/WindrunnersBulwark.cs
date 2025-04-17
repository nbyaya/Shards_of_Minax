using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WindrunnersBulwark : MetalShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WindrunnersBulwark()
        {
            Weight = 6.0;
            Name = "Windrunner's Bulwark";
            Hue = 1153; // Azure color for the Windrunner theme

            // Set attributes and bonuses
            Attributes.BonusDex = 8;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 15;
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 1;
            Attributes.Luck = 50;

            // Resistances
            PhysicalBonus = 5;
            FireBonus = 3;
            ColdBonus = 10;
            PoisonBonus = 3;
            EnergyBonus = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Parry, 10.0); // Improves shield-related skill
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 5.0); // Reflects mastery over summoned creatures

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public WindrunnersBulwark(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Windrunner spirit enhances your command over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWindrunnerTimer(pm);
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
                pm.SendMessage(37, "You feel the Windrunner spirit fading...");
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
            list.Add("Summons Windrunners");
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
                    m_Timer = new SummonWindrunnerTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWindrunnerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWindrunnerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust frequency
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is WindrunnersBulwark))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Windrunner windrunner = new Windrunner
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    windrunner.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Windrunner answers your call to aid!");
                }
            }
        }
    }
}
