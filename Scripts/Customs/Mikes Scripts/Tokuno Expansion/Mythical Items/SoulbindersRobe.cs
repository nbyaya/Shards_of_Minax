using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SoulbindersRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SoulbindersRobe()
        {
            Weight = 2.0;
            Name = "Soulbinder's Robe";
            Hue = 1150; // Ghostly, pale color

            // Set attributes and bonuses
            Attributes.BonusInt = 15; // Focus on intelligence for a summoner
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 15; // Stronger against cold (thematic for undead)
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0); // Spirit Speak for undead synergy
            SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0); // Necromancy for summoning
            SkillBonuses.SetValues(2, SkillName.Meditation, 10.0); // Meditation for mana regen

            // Follower bonus
            m_BonusFollowers = 2; // Grants +2 followers

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SoulbindersRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of spectral energy, allowing you to command more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRestlessSoulTimer(pm);
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
                pm.SendMessage(37, "The spectral energy fades, reducing your ability to command creatures.");
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
            list.Add("Summons Restless Souls");
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
                // Check autosummon state and reinitialize timer if necessary
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRestlessSoulTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRestlessSoulTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRestlessSoulTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is SoulbindersRobe))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    RestlessSoul soul = new RestlessSoul
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    soul.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Restless Soul rises from the ground to serve you!");
                }
            }
        }
    }
}
