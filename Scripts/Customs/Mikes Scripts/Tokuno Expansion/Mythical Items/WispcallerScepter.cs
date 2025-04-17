using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WispcallerScepter : Scepter
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WispcallerScepter()
        {
            Weight = 3.0;
            Name = "Wispcaller Scepter";
            Hue = 1154; // A glowing, ethereal blue color

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances magical abilities
            Attributes.CastSpeed = 1; // Faster spellcasting
            Attributes.CastRecovery = 2; // Quicker spell recovery
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.LowerManaCost = 10; // Reduces mana cost of spells
            Attributes.Luck = 100; // Increases luck for better loot and critical hits

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0); // Enhances Magery skill
            SkillBonuses.SetValues(1, SkillName.Meditation, 10.0); // Improves Meditation for mana regeneration

            // Attach XmlLevelItem for leveling system
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public WispcallerScepter(Serial serial) : base(serial)
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
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) 
                {
                    m_Timer = new SummonWispTimer(pm);
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
            list.Add("Summons Wisps to aid you in battle");
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
                    m_Timer = new SummonWispTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWispTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWispTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is WispcallerScepter))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Wisp wisp = new Wisp
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wisp.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wisp emerges to serve you!");
                }
            }
        }
    }
}
