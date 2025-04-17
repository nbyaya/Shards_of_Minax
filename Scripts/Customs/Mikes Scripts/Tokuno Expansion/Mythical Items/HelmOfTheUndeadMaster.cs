using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HelmOfTheUndeadMaster : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HelmOfTheUndeadMaster()
        {
            Weight = 5.0;
            Name = "Helm of the Undead Master";
            Hue = 1175; // Dark, skeletal hue

            // Set attributes and bonuses
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 15;
            PoisonBonus = 10;
            EnergyBonus = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem for customization
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public HelmOfTheUndeadMaster(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of necromantic power, allowing you to command more undead minions!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonSkeletalDragonTimer(pm);
                m_Timer.Start();
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The necromantic power fades, and your control over undead lessens.");
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
            list.Add("Summons Skeletal Dragons");
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
                m_Timer = new SummonSkeletalDragonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSkeletalDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkeletalDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is HelmOfTheUndeadMaster))
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
                    SkeletalDragon dragon = new SkeletalDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skeletal Dragon rises from the ground to serve you!");
                }
            }
        }
    }
}
