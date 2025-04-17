using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SkeletalDrakeBreastplate : LeatherChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SkeletalDrakeBreastplate()
        {
            Weight = 10.0;
            Name = "Skeletal Drake Breastplate";
            Hue = 1150; // Bone-white color with a faint glow

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 50;
            Attributes.SpellDamage = 10;

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 20; // Skeletal theme, strong against cold
            PoisonBonus = 10;
            EnergyBonus = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);
            SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public SkeletalDrakeBreastplate(Serial serial) : base(serial)
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
                    m_Timer = new SummonSkeletalDrakeTimer(pm);
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
            list.Add("Summons Skeletal Drakes");
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
                // Check if autosummon is enabled on reload
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSkeletalDrakeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSkeletalDrakeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkeletalDrakeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is SkeletalDrakeBreastplate))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SkeletalDrake drake = new SkeletalDrake
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    drake.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skeletal Drake emerges to serve you!");
                }
            }
        }
    }
}
