using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TriceratopsBoneHelm : BoneHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TriceratopsBoneHelm()
        {
            Weight = 3.0;
            Name = "Helm of the Triceratops";
            Hue = 2960; // Dinosaur-like earthy green hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusHits = 25;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;
            Attributes.ReflectPhysical = 10;

            // Resistances
            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 8;
            PoisonBonus = 8;
            EnergyBonus = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public TriceratopsBoneHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command mighty beasts!");

                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonTriceratopsTimer(pm);
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
                pm.SendMessage(37, "You feel your connection to mighty beasts diminish.");
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
            list.Add("Summons Triceratops");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonTriceratopsTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTriceratopsTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTriceratopsTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is TriceratopsBoneHelm))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Triceratops triceratops = new Triceratops
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    triceratops.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Triceratops stomps into battle to serve you!");
                }
            }
        }
    }
}
