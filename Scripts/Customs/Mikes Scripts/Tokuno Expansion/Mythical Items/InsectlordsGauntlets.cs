using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class InsectlordsGauntlets : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public InsectlordsGauntlets()
        {
            Weight = 1.0;
            Name = "Insectlord's Gauntlets";
            Hue = 2966; // Insect-like greenish hue

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusInt = 10;
            Attributes.RegenMana = 3;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 5;
            Attributes.ReflectPhysical = 5;

            PhysicalBonus = 8;
            PoisonBonus = 15;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Magery, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public InsectlordsGauntlets(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel attuned to command the swarm!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonDeathwatchBeetleTimer(pm);
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
                pm.SendMessage(37, "The swarm feels more distant now.");
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
            list.Add("Summons Deathwatch Beetle Hatchlings");
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
                    m_Timer = new SummonDeathwatchBeetleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonDeathwatchBeetleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDeathwatchBeetleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is InsectlordsGauntlets))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DeathwatchBeetleHatchling beetle = new DeathwatchBeetleHatchling
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    beetle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Deathwatch Beetle Hatchling crawls forth to serve you!");
                }
            }
        }
    }
}
