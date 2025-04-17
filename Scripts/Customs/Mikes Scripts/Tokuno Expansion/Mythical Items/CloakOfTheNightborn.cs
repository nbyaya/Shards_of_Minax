using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CloakOfTheNightborn : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CloakOfTheNightborn()
        {
            Weight = 1.0;
            Name = "Cloak of the Nightborn";
            Hue = 1175; // Dark, blood-red hue

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances magical abilities
            Attributes.Luck = 100;    // Increases luck
            Attributes.SpellDamage = 15; // Boosts spell damage
            Attributes.CastSpeed = 1;    // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 15;    // Vampires are resistant to cold
            Resistances.Poison = 20;  // Vampires are resistant to poison
            Resistances.Energy = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0); // Enhances necromancy
            SkillBonuses.SetValues(1, SkillName.Stealth, 10.0);    // Improves stealth

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem for leveling system
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public CloakOfTheNightborn(Serial serial) : base(serial)
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
                    m_Timer = new SummonVampireBatTimer(pm);
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
            list.Add("Summons Vampire Bats");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances necromancy and stealth");
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
                    m_Timer = new SummonVampireBatTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonVampireBatTimer : Timer
        {
            private Mobile m_Owner;

            public SummonVampireBatTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is CloakOfTheNightborn))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    VampireBat bat = new VampireBat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    bat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Vampire Bat emerges from the shadows to serve you!");
                }
            }
        }
    }
}
