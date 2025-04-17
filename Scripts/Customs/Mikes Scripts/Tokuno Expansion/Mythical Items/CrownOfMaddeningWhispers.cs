using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CrownOfMaddeningWhispers : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CrownOfMaddeningWhispers()
        {
            Weight = 5.0;
            Name = "Crown of Maddening Whispers";
            Hue = 1175; // Dark purple hue, representing horror and madness

            // Set attributes and bonuses
            Attributes.BonusInt = 25;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 3;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;

            PhysicalBonus = 10;
            FireBonus = 8;
            ColdBonus = 12;
            PoisonBonus = 8;
            EnergyBonus = 12;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public CrownOfMaddeningWhispers(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The whispers grant you greater dominion over horrors!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMaddeningHorrorTimer(pm);
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
                pm.SendMessage(37, "The whispers fade, and your dominion weakens.");
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
            list.Add("Summons Maddening Horrors");
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
                m_Timer = new SummonMaddeningHorrorTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonMaddeningHorrorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMaddeningHorrorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust interval as needed
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is CrownOfMaddeningWhispers))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    MaddeningHorror horror = new MaddeningHorror
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    horror.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Maddening Horror emerges from the void to serve you!");
                }
            }
        }
    }
}
