using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DragonheartSamuraiChest : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DragonheartSamuraiChest()
        {
            Weight = 8.0;
            Name = "Dragonheart Samurai Chest";
            Hue = 1359; // Fiery orange hue, representing a dragon's heart

            // Set attributes and bonuses
            Attributes.BonusStr = 25;
            Attributes.BonusDex = 10;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 15;
            Attributes.Luck = 200;

            SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(2, SkillName.AnimalLore, 15.0);

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 5;
            PoisonBonus = 5;
            EnergyBonus = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DragonheartSamuraiChest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command mighty creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonHiryuTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding mighty creatures.");
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
            list.Add("Summons Hiryus");
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
                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonHiryuTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonHiryuTimer : Timer
        {
            private Mobile m_Owner;

            public SummonHiryuTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is DragonheartSamuraiChest))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Hiryu hiryu = new Hiryu
                    {
                        Controlled = true,
                        ControlMaster = m_Owner,
                        Loyalty = 100,
                        IsBonded = true
                    };

                    hiryu.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A majestic Hiryu answers your call!");
                }
            }
        }
    }
}
