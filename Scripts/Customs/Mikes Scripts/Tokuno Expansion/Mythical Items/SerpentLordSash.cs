using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SerpentLordSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SerpentLordSash()
        {
            Weight = 1.0;
            Name = "Serpent Lord's Sash";
            Hue = 58; // Greenish hue, fitting for a serpent theme

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 10;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 15;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 20; // Extra poison resistance for serpent theme
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Ninjitsu, 15.0);
            SkillBonuses.SetValues(1, SkillName.Poisoning, 15.0);
            SkillBonuses.SetValues(2, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(3, SkillName.Tactics, 10.0);
            SkillBonuses.SetValues(4, SkillName.Wrestling, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SerpentLordSash(Serial serial) : base(serial)
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
                m_Timer = new SummonSerpentTimer(pm);
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
            list.Add("Summons SerpentsFangHighExecutioner");
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
                m_Timer = new SummonSerpentTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSerpentTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSerpentTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summons every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SerpentLordSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SerpentsFangHighExecutioner serpent = new SerpentsFangHighExecutioner
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    serpent.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A SerpentsFangHighExecutioner emerges to serve you!");
                }
            }
        }
    }
}
