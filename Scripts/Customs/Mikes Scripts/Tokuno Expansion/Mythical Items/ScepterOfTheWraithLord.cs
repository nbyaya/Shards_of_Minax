using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ScepterOfTheWraithLord : Scepter
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ScepterOfTheWraithLord() 
        {
            Weight = 5.0;
            Name = "Scepter of the Wraith Lord";
            Hue = 1175; // Dark, ghostly purple

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Enhances spellcasting
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost
            Attributes.Luck = 100; // Increases luck
            Attributes.NightSight = 1; // Grants night sight

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0); // Enhances necromancy
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0); // Improves spirit communication

            // Weapon Attributes
            WeaponAttributes.HitLeechHits = 25; // Life leech
            WeaponAttributes.HitLeechMana = 25; // Mana leech
            WeaponAttributes.HitLowerDefend = 25; // Reduces target's defense

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2
        }

        public ScepterOfTheWraithLord(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of dark energy, allowing you to command more creatures!");

                // Only start summon timer if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonWraithTimer(pm);
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
                pm.SendMessage(37, "The dark energy fades, and you can no longer command as many creatures.");
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
            list.Add("Summons Wraiths to serve you");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances necromancy and spirit communication");
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
                // Check if auto-summon is enabled when re-equipping the item
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonWraithTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonWraithTimer : Timer
        {
            private Mobile m_Owner;

            public SummonWraithTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is ScepterOfTheWraithLord))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Wraith wraith = new Wraith
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wraith.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Wraith emerges from the shadows to serve you!");
                }
            }
        }
    }
}
