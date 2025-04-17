using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScepterOfTheUndyingHorde : Scepter
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScepterOfTheUndyingHorde()
        {
            Weight = 5.0;
            Name = "Scepter of the Undying Horde";
            Hue = 1175; // Dark, necromantic color

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances necromantic abilities
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost
            Attributes.LowerRegCost = 15; // Reduces reagent cost
            Attributes.NightSight = 1; // Grants night sight

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0); // Enhances necromancy
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0); // Enhances spirit communication

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public ScepterOfTheUndyingHorde(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a dark presence swelling your ranks!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonZombieTimer(pm);
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
                pm.SendMessage(37, "The dark presence fades, and your ranks feel diminished.");
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
            list.Add("Summons Zombies to fight for you");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances necromantic abilities");
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
                // Only start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonZombieTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonZombieTimer : Timer
        {
            private Mobile m_Owner;

            public SummonZombieTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summons a Zombie every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is ScepterOfTheUndyingHorde))
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
                    Zombie zombie = new Zombie
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    zombie.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Zombie rises from the ground to serve you!");
                }
            }
        }
    }
}
