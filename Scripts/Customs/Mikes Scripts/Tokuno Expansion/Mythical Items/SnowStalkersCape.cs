using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SnowStalkersCape : FurCape
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SnowStalkersCape()
        {
            Weight = 5.0;
            Name = "Snow Stalker's Cape";
            Hue = 1150; // Snowy white color

            // Set attributes and bonuses
            Attributes.BonusDex = 8;
            Attributes.RegenMana = 4;
            Attributes.SpellDamage = 8;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            // Resistances
            Resistances.Cold = 20;
            Resistances.Physical = 10;

            // Skill Bonus
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SnowStalkersCape(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The spirit of the Snow Leopard grants you greater control over your followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSnowLeopardTimer(pm);
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
                pm.SendMessage(37, "You feel less in tune with your followers as the spirit departs.");
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
            list.Add("Summons Snow Leopards");
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
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSnowLeopardTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSnowLeopardTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSnowLeopardTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is SnowStalkersCape))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SnowLeopard leopard = new SnowLeopard
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    leopard.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Snow Leopard materializes to serve you!");
                }
            }
        }
    }
}
