using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RatmanArchmageRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RatmanArchmageRobe()
        {
            Weight = 2.0;
            Name = "Ratman Archmage's Robe";
            Hue = 1161; // A dark, mystical color for Ratman theme

            // Set attributes and bonuses
            Attributes.BonusInt = 15; // Focus on intelligence for a mage
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.Luck = 75;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 15; // Ratmen are known for poison
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);
            SkillBonuses.SetValues(3, SkillName.Poisoning, 10.0); // Thematic for Ratmen
            SkillBonuses.SetValues(4, SkillName.Necromancy, 5.0); // Optional for dark magic flavor

            // Follower bonus
            m_BonusFollowers = 2; // Allows for more followers

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public RatmanArchmageRobe(Serial serial) : base(serial)
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

                // Start summon timer if auto summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRatmanMageTimer(pm);
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
            list.Add("Summons Ratman Mages");
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
                    m_Timer = new SummonRatmanMageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRatmanMageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRatmanMageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RatmanArchmageRobe))
                {
                    Stop();
                    return;
                }

                // Check if auto summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    RatmanMage mage = new RatmanMage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Ratman Mage emerges to serve you!");
                }
            }
        }
    }
}
