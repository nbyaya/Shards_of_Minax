using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RatmastersSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RatmastersSash()
        {
            Weight = 1.0;
            Name = "Ratmaster's Sash";
            Hue = 1109; // A dark, rat-like color

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            Attributes.BonusHits = 10;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 2;
            Attributes.RegenMana = 2;
            Attributes.WeaponSpeed = 5;
            Attributes.WeaponDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
            Attributes.ReflectPhysical = 5;
            Attributes.EnhancePotions = 15;
            Attributes.Luck = 50;
            Attributes.SpellDamage = 8;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 5;
            Resistances.Poison = 10; // Rat-themed poison resistance
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Stealth, 10.0);
            SkillBonuses.SetValues(3, SkillName.Tracking, 10.0);
            SkillBonuses.SetValues(4, SkillName.Poisoning, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public RatmastersSash(Serial serial) : base(serial)
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
                m_Timer = new SummonRatmanTimer(pm);
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
            list.Add("Summons Ratmen");
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
                m_Timer = new SummonRatmanTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonRatmanTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRatmanTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is RatmastersSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Ratman ratman = new Ratman
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ratman.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Ratman emerges to serve you!");
                }
            }
        }
    }
}
