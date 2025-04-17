using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LeatherWolfsCowl : BearMask
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LeatherWolfsCowl()
        {
            Weight = 2.0;
            Name = "LeatherWolf's Cowl";
            Hue = 1175; // A dark leather-like hue, fitting the theme.

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 20;
            Attributes.BonusHits = 30;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 15;
            Resistances.Cold = 10;
            Resistances.Poison = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public LeatherWolfsCowl(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an alpha's presence allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonLeatherWolfTimer(pm);
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
                pm.SendMessage(37, "The alpha's presence fades, reducing your control over creatures.");
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
            list.Add("Summons LeatherWolves");
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
                m_Timer = new SummonLeatherWolfTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonLeatherWolfTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLeatherWolfTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust spawn frequency as needed
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is LeatherWolfsCowl))
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
                    LeatherWolf wolf = new LeatherWolf
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wolf.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A LeatherWolf emerges from the shadows to serve you!");
                }
            }
        }
    }
}
