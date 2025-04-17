using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StaffOfTheLichKing : GnarledStaff
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public StaffOfTheLichKing()
        {
            Weight = 5.0;
            Name = "Staff of the Lich King";
            Hue = 1153; // A dark, eerie green color

            // Set attributes and bonuses
            Attributes.BonusInt = 10; // Boosts intelligence for spellcasting
            Attributes.SpellDamage = 15; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost of spells
            Attributes.LowerRegCost = 15; // Reduces reagent cost of spells
            Attributes.NightSight = 1; // Grants night sight

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0); // Boosts Necromancy skill
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0); // Boosts Spirit Speak skill

            // Weapon Attributes
            WeaponAttributes.HitLeechMana = 50; // Leeches mana on hit
            WeaponAttributes.HitLowerDefend = 25; // Lowers target's defense on hit

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2
        }

        public StaffOfTheLichKing(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a dark presence increasing your command over the undead!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonSkeletalLichTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
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
                pm.SendMessage(37, "Your command over the undead weakens.");
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
            list.Add("Summons Skeletal Liches");
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
                m_Timer = new SummonSkeletalLichTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonSkeletalLichTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkeletalLichTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is StaffOfTheLichKing))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SkeletalLich lich = new SkeletalLich
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    lich.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skeletal Lich rises from the ground to serve you!");
                }
            }
        }
    }
}
