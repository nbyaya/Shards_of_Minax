using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BeastmastersSaddle : LeatherCap
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public BeastmastersSaddle()
        {
            Weight = 5.0;
            Name = "Beastmaster's Saddle";
            Hue = 0x835; // A leather-like hue

            // Set attributes and bonuses
            ArmorAttributes.MageArmor = 1;
            ArmorAttributes.SelfRepair = 3;
            Attributes.BonusStr = 10;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 15;
            Attributes.Luck = 100;
            Attributes.NightSight = 1;

            PhysicalBonus = 8;
            FireBonus = 8;
            ColdBonus = 8;
            PoisonBonus = 12;
            EnergyBonus = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public BeastmastersSaddle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a deep connection to beasts of burden!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonPackHorseTimer(pm);
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
                pm.SendMessage(37, "You feel less attuned to beasts of burden.");
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
            list.Add("Summons Pack Horses");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonPackHorseTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonPackHorseTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPackHorseTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is BeastmastersSaddle))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    PackHorse horse = new PackHorse
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    horse.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A loyal Pack Horse appears to assist you!");
                }
            }
        }
    }
}
