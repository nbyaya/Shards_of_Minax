using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OceanmastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OceanmastersRobes()
        {
            Weight = 2.0;
            Name = "Oceanmasters Robes";
            Hue = 1152; // Ocean-themed blue color

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 12;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenHits = 4;
            Attributes.RegenMana = 6;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 3;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 10;
            Attributes.EnhancePotions = 15;
            Attributes.Luck = 150;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 6;
            Resistances.Cold = 12;
            Resistances.Poison = 6;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            SkillBonuses.SetValues(1, SkillName.Magery, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OceanmastersRobes(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you can command more aquatic creatures now!");

                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonSeahorseTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
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
            list.Add("Summons Seahorses");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonSeahorseTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSeahorseTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSeahorseTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is OceanmastersRobes))
                {
                    Stop();
                    return;
                }

                // Stop if autosummon is disabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SeaHorse seahorse = new SeaHorse
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    seahorse.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Seahorse emerges from the depths to serve you!");
                }
            }
        }
    }
}
