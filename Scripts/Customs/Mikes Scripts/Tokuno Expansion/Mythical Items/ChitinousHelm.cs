using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChitinousHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ChitinousHelm()
        {
            Name = "Chitinous Helm";
            Hue = 1150; // Insectoid chitin-like hue
            Weight = 5.0;

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 3;
            Attributes.BonusDex = 10;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Resistances
            PhysicalBonus = 10;
            FireBonus = 5;
            ColdBonus = 5;
            PoisonBonus = 15;
            EnergyBonus = 5;

            // Attach XmlLevelItem for further customization
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ChitinousHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The chitinous helm hums with insectoid energy, granting you the power to control more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonBlackSolenTimer(pm);
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
                pm.SendMessage(37, "You feel the insectoid energy fade, reducing your control over creatures.");
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
            list.Add("Summons Black Solen Infiltrator Warriors");
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
                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonBlackSolenTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonBlackSolenTimer : Timer
        {
            private Mobile m_Owner;

            public SummonBlackSolenTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is ChitinousHelm))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BlackSolenInfiltratorWarrior solen = new BlackSolenInfiltratorWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    solen.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Black Solen Infiltrator Warrior emerges to serve you!");
                }
            }
        }
    }
}
