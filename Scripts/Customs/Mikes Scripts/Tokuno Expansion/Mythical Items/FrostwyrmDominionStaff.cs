using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostwyrmDominionStaff : GnarledStaff
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostwyrmDominionStaff()
        {
            Weight = 5.0;
            Name = "Frostwyrm's Dominion Staff";
            Hue = 1152; // Icy blue color

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances magical abilities
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost
            Attributes.Luck = 100; // Increases luck
            Attributes.NightSight = 1; // Grants night sight

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0); // Enhances Magery
            SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0); // Enhances Eval Int
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 10.0); // Enhances Spirit Speak

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem for leveling system
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public FrostwyrmDominionStaff(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of power, allowing you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWhiteWyrmTimer(pm);
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
                pm.SendMessage(37, "You feel your ability to command creatures wane.");
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
            list.Add("Summons White Wyrms");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances cold resistance and magical abilities");
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
                // Start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonWhiteWyrmTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWhiteWyrmTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWhiteWyrmTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is FrostwyrmDominionStaff))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    WhiteWyrm wyrm = new WhiteWyrm
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wyrm.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A White Wyrm emerges from the icy depths to serve you!");
                }
            }
        }
    }
}
