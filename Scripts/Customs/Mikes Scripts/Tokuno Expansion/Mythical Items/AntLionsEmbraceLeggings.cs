using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AntLionsEmbraceLeggings : BoneLegs
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AntLionsEmbraceLeggings()
        {
            Weight = 5.0;
            Name = "AntLion's Embrace Leggings";
            Hue = 2413; // Sand-colored hue

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusStam = 15;
            Attributes.DefendChance = 10;
            Attributes.RegenStam = 3;
            Attributes.Luck = 200;

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 20;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AntLionsEmbraceLeggings(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the embrace of the sands, empowering you to command more creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonAntLionTimer(pm);
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
                pm.SendMessage(37, "The sandy embrace fades, and your connection to creatures diminishes.");
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
            list.Add("Summons AntLions to aid you");
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
                m_Timer = new SummonAntLionTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonAntLionTimer : Timer
        {
            private Mobile m_Owner;

            public SummonAntLionTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Pants) is AntLionsEmbraceLeggings))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    AntLion antLion = new AntLion
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    antLion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An AntLion emerges from the sands to serve you!");
                }
            }
        }
    }
}
