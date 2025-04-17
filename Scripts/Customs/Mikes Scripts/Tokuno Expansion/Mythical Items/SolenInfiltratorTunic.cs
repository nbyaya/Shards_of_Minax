using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SolenInfiltratorTunic : ChainChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SolenInfiltratorTunic()
        {
            Weight = 6.0;
            Name = "Solen Infiltrator's Carapace";
            Hue = 1175; // Dark, chitin-like hue

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusDex = 15;
            Attributes.BonusHits = 10;
            Attributes.BonusStam = 10;
            Attributes.DefendChance = 10;
            Attributes.RegenStam = 3;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 8;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SolenInfiltratorTunic(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an eerie connection to the hive.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonSolenTimer(pm);
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
                pm.SendMessage(37, "You feel your connection to the hive weaken.");
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
            list.Add("Summons Black Solen Infiltrator Queens");
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
                    m_Timer = new SummonSolenTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSolenTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSolenTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is SolenInfiltratorTunic))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BlackSolenInfiltratorQueen solen = new BlackSolenInfiltratorQueen
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    solen.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Black Solen Infiltrator Queen emerges from the shadows to serve you!");
                }
            }
        }
    }
}
