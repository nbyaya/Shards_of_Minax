using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class NaturesWrathPlateChest : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public NaturesWrathPlateChest()
        {
            Weight = 10.0;
            Name = "Nature's Wrath Plate Chest";
            Hue = 0x59B; // Greenish hue for a nature theme

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 15;
            Attributes.CastRecovery = 1;
            Attributes.SpellDamage = 10;

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 15;
            PoisonBonus = 20;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public NaturesWrathPlateChest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the bond of nature strengthening your command over creatures!");

                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonFeralTreefellowTimer(pm);
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
                pm.SendMessage(37, "The bond of nature weakens as you remove the chest piece.");
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
            list.Add("Summons Feral Treefellows");
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
                    m_Timer = new SummonFeralTreefellowTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFeralTreefellowTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFeralTreefellowTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is NaturesWrathPlateChest))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FeralTreefellow treefellow = new FeralTreefellow
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    treefellow.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Feral Treefellow emerges to protect you!");
                }
            }
        }
    }
}
