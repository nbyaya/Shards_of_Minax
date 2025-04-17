using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShepherdsRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShepherdsRobe()
        {
            Weight = 2.0;
            Name = "Shepherd's Robe";
            Hue = 1153; // Light gray/white color for a shepherd theme

            // Set attributes and bonuses
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.Luck = 50;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.Herding, 10.0);
            SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ShepherdsRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a connection to nature and its creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonSheepTimer(pm);
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
                pm.SendMessage(37, "Your connection to nature weakens.");
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
            list.Add("Summons Sheep");
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
                m_Timer = new SummonSheepTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSheepTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSheepTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is ShepherdsRobe))
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
                    Sheep sheep = new Sheep
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    sheep.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A sheep has joined your flock!");
                }
            }
        }
    }
}
