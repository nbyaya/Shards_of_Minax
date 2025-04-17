using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SpellbindersOrb : MetalShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SpellbindersOrb()
        {
            Weight = 5.0;
            Name = "Spellbinder's Orb";
            Hue = 1153; // Ethereal glow color, change as desired

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 30;
            Attributes.RegenMana = 8;
            Attributes.CastSpeed = 2;
            Attributes.CastRecovery = 3;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 20;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem for leveling functionality
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SpellbindersOrb(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Spellbinder's Orb flows through you!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSpellbinderTimer(pm);
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
                pm.SendMessage(37, "You feel the power of the Spellbinder's Orb fade away.");
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
            list.Add("Summons Spellbinders");
            list.Add("Increases maximum followers");
            list.Add("A mystical orb imbued with the power of summoning.");
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
                    m_Timer = new SummonSpellbinderTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSpellbinderTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSpellbinderTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is SpellbindersOrb))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and if the player has space for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Spellbinder spellbinder = new Spellbinder
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spellbinder.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Spellbinder appears to assist you!");
                }
            }
        }
    }
}
