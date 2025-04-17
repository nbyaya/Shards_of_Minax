using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RuddyBouraMastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RuddyBouraMastersRobes()
        {
            Weight = 2.0;
            Name = "Ruddy Boura Master's Robes";
            Hue = 1157; // A reddish-brown hue, fitting for a Ruddy Boura theme

            // Set attributes and bonuses (reasonable selection)
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 10;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 10;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses (focused on summoning and utility)
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);
            SkillBonuses.SetValues(3, SkillName.Magery, 10.0);
            SkillBonuses.SetValues(4, SkillName.EvalInt, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increase follower count by 2
        }

        public RuddyBouraMastersRobes(Serial serial) : base(serial)
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

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer if autosummon is enabled
                    StopSummonTimer();
                    m_Timer = new SummonRuddyBouraTimer(pm);
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
            list.Add("Summons Ruddy Boura");
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
                // Check if autosummon is enabled before starting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRuddyBouraTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRuddyBouraTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRuddyBouraTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RuddyBouraMastersRobes))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has space for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    RuddyBoura boura = new RuddyBoura
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    boura.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Ruddy Boura emerges to serve you!");
                }
            }
        }
    }
}
