using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WarbossHelm : OrcHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WarbossHelm()
        {
            Weight = 5.0;
            Name = "Warboss Helm";
            Hue = 1153; // Orcish green hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 20.0);

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 10;
            EnergyBonus = 5;

            ArmorAttributes.DurabilityBonus = 50;
            ArmorAttributes.SelfRepair = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WarbossHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the strength to command a mighty orc warband!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonOrcishLordTimer(pm);
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
                pm.SendMessage(37, "You feel less able to lead your orcish followers.");
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
            list.Add("Summons Orcish Lords");
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
                // Only restart the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonOrcishLordTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOrcishLordTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOrcishLordTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is WarbossHelm))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OrcishLord orcishLord = new OrcishLord
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    orcishLord.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Orcish Lord rallies to your side!");
                }
            }
        }
    }
}
