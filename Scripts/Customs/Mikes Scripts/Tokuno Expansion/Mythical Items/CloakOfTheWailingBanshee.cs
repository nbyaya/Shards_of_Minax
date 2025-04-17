using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CloakOfTheWailingBanshee : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CloakOfTheWailingBanshee()
        {
            Weight = 1.0;
            Name = "Cloak of the Wailing Banshee";
            Hue = 1154; // Ghostly pale blue

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances spellcasting
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster spellcasting
            Attributes.CastRecovery = 2; // Faster spell recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost
            Attributes.NightSight = 1; // Grants night vision

            // Resistances
            Resistances.Physical = 5;
            Resistances.Fire = 10;
            Resistances.Cold = 15; // Banshees are associated with cold
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0); // Banshees are tied to spirit communication
            SkillBonuses.SetValues(1, SkillName.Magery, 10.0); // Enhances magical abilities

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public CloakOfTheWailingBanshee(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a chilling presence, allowing you to command more creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWailingBansheeTimer(pm);
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
                pm.SendMessage(37, "The chilling presence fades, reducing your command over creatures.");
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
            list.Add("Summons Wailing Banshees");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances spirit communication and spellcasting");
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
                    m_Timer = new SummonWailingBansheeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWailingBansheeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWailingBansheeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is CloakOfTheWailingBanshee))
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
                    WailingBanshee banshee = new WailingBanshee
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    banshee.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wailing Banshee emerges from the void to serve you!");
                }
            }
        }
    }
}
