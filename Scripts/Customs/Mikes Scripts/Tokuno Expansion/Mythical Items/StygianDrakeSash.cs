using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StygianDrakeSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public StygianDrakeSash()
        {
            Weight = 1.0;
            Name = "Sash of the Stygian Drake";
            Hue = 1157; // Dark, abyssal color

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances spellcasting and mana
            Attributes.RegenMana = 3; // Faster mana regeneration
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.Luck = 100; // Increases luck for better loot and outcomes
            Attributes.NightSight = 1; // Grants night vision

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0); // Enhances taming ability
            SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0); // Improves magic resistance
            SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0); // Enhances intelligence evaluation

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem for potential leveling system
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public StygianDrakeSash(Serial serial) : base(serial)
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
                m_Timer = new SummonStygianDrakeTimer(pm);
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
            list.Add("Summons Stygian Drakes");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances taming and spellcasting abilities");
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
                m_Timer = new SummonStygianDrakeTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonStygianDrakeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonStygianDrakeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is StygianDrakeSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    StygianDrake drake = new StygianDrake
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    drake.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Stygian Drake emerges from the abyss to serve you!");
                }
            }
        }
    }
}
