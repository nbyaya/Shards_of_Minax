using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PrimalStalkerBoots : Boots
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PrimalStalkerBoots()
        {
            Weight = 1.0;
            Name = "Primal Stalker Boots";
            Hue = 1157; // Earthy tone, adjust as needed

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusStam = 20;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 200;

            Resistances.Physical = 10;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 5;
            Resistances.Energy = 7;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Tracking, 5.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Provides 2 extra follower slots
        }

        public PrimalStalkerBoots(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel more capable of taming and commanding wild creatures!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonHighPlainsBouraTimer(pm);
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
                pm.SendMessage(37, "You feel less connected to the wild.");

                // Stop the summon timer
                StopSummonTimer();
            }
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
            list.Add("Summons High Plains Boura");
            list.Add("Increases maximum followers by 2");
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
                    m_Timer = new SummonHighPlainsBouraTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonHighPlainsBouraTimer : Timer
        {
            private Mobile m_Owner;

            public SummonHighPlainsBouraTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Shoes) is PrimalStalkerBoots))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    HighPlainsBoura boura = new HighPlainsBoura
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    boura.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A High Plains Boura appears to assist you!");
                }
            }
        }
    }
}
