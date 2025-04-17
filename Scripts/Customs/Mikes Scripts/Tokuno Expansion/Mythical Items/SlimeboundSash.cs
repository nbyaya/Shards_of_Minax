using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SlimeboundSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SlimeboundSash()
        {
            Weight = 1.0;
            Name = "Slimebound Sash";
            Hue = 58; // Slimy green color

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 50;
            Attributes.SpellDamage = 10;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Poison = 15; // Slimes are poisonous, so this fits thematically

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SlimeboundSash(Serial serial) : base(serial)
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

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSlimeTimer(pm);
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
            list.Add("Summons Slimes");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSlimeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSlimeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSlimeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SlimeboundSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Slime slime = new Slime
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    slime.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Slime oozes forth to serve you!");
                }
            }
        }
    }
}
