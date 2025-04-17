using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TrollKingsWarHammer : WarHammer
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TrollKingsWarHammer()
        {
            Weight = 10.0;
            Name = "Troll King's War Hammer";
            Hue = 1161; // Dark green, fitting for a Troll-themed item

            // Set attributes and bonuses
            Attributes.BonusStr = 15; // Increased strength to reflect the Troll's might
            Attributes.BonusStam = 15; // Enhanced stamina for prolonged battles
            Attributes.WeaponDamage = 25; // Increased damage to make it a powerful weapon
            Attributes.AttackChance = 10; // Better chance to hit
            Attributes.DefendChance = 10; // Better chance to block
            Attributes.Luck = 50; // Moderate luck bonus
            Attributes.WeaponSpeed = 10; // Faster swing speed

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Macing, 15.0); // Bonus to mace fighting
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0); // Bonus to tactics for better combat strategy
            SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Bonus to anatomy for increased damage

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public TrollKingsWarHammer(Serial serial) : base(serial)
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

                // Start summon timer if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonTrollTimer(pm);
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
            list.Add("Summons Trolls");
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
                // Start summon timer if auto-summon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonTrollTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTrollTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTrollTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is TrollKingsWarHammer))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Troll troll = new Troll
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    troll.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Troll emerges to serve you!");
                }
            }
        }
    }
}
