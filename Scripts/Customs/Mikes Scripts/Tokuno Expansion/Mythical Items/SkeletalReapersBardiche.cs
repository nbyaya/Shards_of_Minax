using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SkeletalReapersBardiche : Bardiche
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SkeletalReapersBardiche()
        {
            Weight = 7.0;
            Name = "Skeletal Reaper's Bardiche";
            Hue = 1150; // Dark, bone-like color

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.WeaponDamage = 25;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 10;
            Attributes.Luck = 50;
            Attributes.WeaponSpeed = 20;
            Attributes.SpellChanneling = 1; // Allows spellcasting while equipped

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            // Weapon Attributes
            WeaponAttributes.HitLeechHits = 25;
            WeaponAttributes.HitLowerDefend = 30;

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SkeletalReapersBardiche(Serial serial) : base(serial)
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
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonSkeletalMountTimer(pm);
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
            list.Add("Summons Skeletal Mounts");
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
                // Check if autosummon is enabled when re-equip
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSkeletalMountTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSkeletalMountTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkeletalMountTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is SkeletalReapersBardiche))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Check if there is room for another follower
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SkeletalMount mount = new SkeletalMount
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mount.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skeletal Mount rises from the ground to serve you!");
                }
            }
        }
    }
}
