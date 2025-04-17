using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SeductressApron : Kilt
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SeductressApron()
        {
            Weight = 1.0;
            Name = "Seductress' Skirt";
            Hue = 1358; // Dark purple-red color for a seductive theme

            // Set attributes and bonuses
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.DefendChance = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 10.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public SeductressApron(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonSuccubusTimer(pm);
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
                pm.SendMessage(37, "You feel your control over followers diminishing.");
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
            list.Add("Summons Succubi to serve you");
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
                m_Timer = new SummonSuccubusTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSuccubusTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSuccubusTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SeductressApron))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Succubus succubus = new Succubus
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    succubus.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Succubus emerges to serve you!");
                }
            }
        }
    }
}
