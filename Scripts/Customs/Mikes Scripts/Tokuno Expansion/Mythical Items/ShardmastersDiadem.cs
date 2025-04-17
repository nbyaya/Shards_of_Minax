using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShardmastersDiadem : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShardmastersDiadem()
        {
            Name = "Shardmaster's Diadem";
            Hue = 1153; // Crystal-like blue hue
            Weight = 3.0;

            // Set attributes and bonuses
            Attributes.BonusInt = 25;
            Attributes.RegenMana = 5;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.DefendChance = 10;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 15;
            PoisonBonus = 10;
            EnergyBonus = 12;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ShardmastersDiadem(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the crystalline energy empowering your command over creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonCrystalHydraTimer(pm);
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
                pm.SendMessage(37, "The crystalline energy fades, weakening your control over creatures.");
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
            list.Add("Summons Crystal Hydras");
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
                m_Timer = new SummonCrystalHydraTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCrystalHydraTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCrystalHydraTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is ShardmastersDiadem))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CrystalHydra hydra = new CrystalHydra
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    hydra.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Crystal Hydra emerges, gleaming with ethereal power!");
                }
            }
        }
    }
}
