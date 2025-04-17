using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RabbitmastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RabbitmastersRobes()
        {
            Weight = 2.0;
            Name = "Rabbitmaster's Robes";
            Hue = 1153; // Light brown/rabbit-like color

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 10; // Rabbits are quick!
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 10;
            Attributes.BonusStam = 15;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 2;
            Attributes.WeaponSpeed = 5;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 5;
            Attributes.Luck = 50;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 5;
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public RabbitmastersRobes(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you could command more creatures now!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonRabbitTimer(pm);
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
            list.Add("Summons Rabbits");
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
                m_Timer = new SummonRabbitTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonRabbitTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRabbitTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RabbitmastersRobes))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Rabbit rabbit = new Rabbit
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    rabbit.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A rabbit hops out to join you!");
                }
            }
        }
    }
}
