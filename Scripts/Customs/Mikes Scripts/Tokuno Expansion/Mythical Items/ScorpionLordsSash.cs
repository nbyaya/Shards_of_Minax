using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScorpionLordsSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScorpionLordsSash()
        {
            Weight = 1.0;
            Name = "Scorpion Lord's Sash";
            Hue = 1161; // Scorpion-like color, adjust as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 15;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponSpeed = 10;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
            Attributes.ReflectPhysical = 5;
            Attributes.EnhancePotions = 20;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 20; // Scorpion theme, higher poison resistance
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Poisoning, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(2, SkillName.Wrestling, 15.0);
            SkillBonuses.SetValues(3, SkillName.MagicResist, 15.0);
            SkillBonuses.SetValues(4, SkillName.Anatomy, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public ScorpionLordsSash(Serial serial) : base(serial)
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

                // Check if autosummon is enabled and start summon timer accordingly
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StartSummonTimer(pm);
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

        private void StartSummonTimer(PlayerMobile pm)
        {
            // Start summon timer
            StopSummonTimer(); // Ensure no duplicate timers
            m_Timer = new SummonScorpionTimer(pm);
            m_Timer.Start();
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
            list.Add("Summons Scorpions");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonScorpionTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonScorpionTimer : Timer
        {
            private Mobile m_Owner;

            public SummonScorpionTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is ScorpionLordsSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Scorpion scorpion = new Scorpion
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    scorpion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Scorpion emerges to serve you!");
                }
            }
        }
    }
}
