using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ArachnidQueensVeil : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ArachnidQueensVeil()
        {
            Weight = 2.0;
            Name = "Arachnid Queen's Veil";
            Hue = 1109; // Dark spider-like color

            // Set attributes and bonuses
            Attributes.BonusDex = 25;
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.LowerManaCost = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 15;
            Resistances.Poison = 20;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(2, SkillName.AnimalLore, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ArachnidQueensVeil(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power of the spider queen, ready to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonDreadSpiderTimer(pm);
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
                pm.SendMessage(37, "The power of the spider queen fades...");
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
            list.Add("Summons Dread Spiders");
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
                m_Timer = new SummonDreadSpiderTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonDreadSpiderTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDreadSpiderTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is ArachnidQueensVeil))
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
                    DreadSpider spider = new DreadSpider
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spider.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Dread Spider emerges to serve you!");
                }
            }
        }
    }
}
