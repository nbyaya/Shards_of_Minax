using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TsukiWolfsGaze : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TsukiWolfsGaze()
        {
            Weight = 5.0;
            Name = "TsukiWolf's Gaze";
            Hue = 1153; // Ethereal silver, reminiscent of the moon

            // Set attributes and bonuses
            Attributes.BonusDex = 8;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 8;

            // Resistances
            PhysicalBonus = 8;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Spellweaving, 5.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public TsukiWolfsGaze(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a primal connection to the moon's power.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonTsukiWolfTimer(pm);
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
                pm.SendMessage(37, "The moon's connection fades away.");
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
            list.Add("Summons Tsuki Wolves");
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
                // Start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonTsukiWolfTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonTsukiWolfTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTsukiWolfTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is TsukiWolfsGaze))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    TsukiWolf wolf = new TsukiWolf
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wolf.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Tsuki Wolf emerges from the shadows to aid you!");
                }
            }
        }
    }
}
