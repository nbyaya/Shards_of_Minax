using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SwamplordsSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SwamplordsSash()
        {
            Weight = 1.0;
            Name = "Swamplord's Sash";
            Hue = 1167; // Swampy green color

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 8;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;

            // Resistances
            Resistances.Physical = 5;
            Resistances.Poison = 15;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.MagicResist, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SwamplordsSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a connection to the swamp, allowing you to command more creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSwampTentacleTimer(pm);
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
                pm.SendMessage(37, "Your connection to the swamp fades, reducing your command over creatures.");
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
            list.Add("Summons Swamp Tentacles");
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
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSwampTentacleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSwampTentacleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSwampTentacleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is SwamplordsSash))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SwampTentacle tentacle = new SwampTentacle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    tentacle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Swamp Tentacle rises from the depths to serve you!");
                }
            }
        }
    }
}
