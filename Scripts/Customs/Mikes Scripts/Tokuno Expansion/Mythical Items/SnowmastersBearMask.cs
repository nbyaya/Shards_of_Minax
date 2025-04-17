using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SnowmastersBearMask : BearMask
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SnowmastersBearMask()
        {
            Weight = 2.0;
            Name = "Snowmaster's Bear Mask";
            Hue = 1152; // Icy blue color

            // Set attributes and bonuses
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 8;
            Attributes.NightSight = 1;
            Attributes.LowerManaCost = 5;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Cold = 20; // Strong resistance to cold
            Resistances.Fire = -5; // Weak to fire to balance the item

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(1, SkillName.Meditation, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public SnowmastersBearMask(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The icy winds empower your ability to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonSnowElementalTimer(pm);
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
                pm.SendMessage(37, "The icy winds fade, reducing your control over creatures.");
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
            list.Add("Summons Snow Elementals");
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
                m_Timer = new SummonSnowElementalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSnowElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSnowElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is SnowmastersBearMask))
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
                    SnowElemental elemental = new SnowElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Snow Elemental rises from the frost to serve you!");
                }
            }
        }
    }
}
