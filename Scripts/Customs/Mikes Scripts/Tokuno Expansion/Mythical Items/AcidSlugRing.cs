using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AcidSlugRing : GoldRing
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AcidSlugRing()
        {
            Weight = 0.1;
            Name = "Ring of the Acid Marsh";
            Hue = 1367; // Greenish hue for an acid theme

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.RegenMana = 3;
            Attributes.BonusStam = 15;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 5;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);

            Resistances.Poison = 20;
            Resistances.Physical = 10;
            Resistances.Cold = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AcidSlugRing(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Acid Marsh grants you command over more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonAcidSlugTimer(pm);
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
                pm.SendMessage(37, "The power of the Acid Marsh fades, and you feel less capable of commanding creatures.");
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
            list.Add("Summons Acid Slugs");
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
                m_Timer = new SummonAcidSlugTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonAcidSlugTimer : Timer
        {
            private Mobile m_Owner;

            public SummonAcidSlugTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0)) // Summons every 10 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if owner is invalid or item is no longer equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Ring) is AcidSlugRing))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled for the player
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    AcidSlug slug = new AcidSlug
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    slug.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Acid Slug emerges to serve you!");
                }
            }
        }
    }
}
