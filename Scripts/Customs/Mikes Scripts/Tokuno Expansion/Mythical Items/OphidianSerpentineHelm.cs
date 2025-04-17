using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OphidianSerpentineHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OphidianSerpentineHelm()
        {
            Weight = 5.0;
            Name = "Ophidian Serpentine Helm";
            Hue = 296; // Serpentine green hue, change as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 15;
            Attributes.RegenStam = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 15;

            SkillBonuses.SetValues(0, SkillName.Anatomy, 10.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 15.0);

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 20; // Strong poison resistance
            EnergyBonus = 10;

            ArmorAttributes.SelfRepair = 5;
            ArmorAttributes.MageArmor = 1;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OphidianSerpentineHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a serpentine power aiding your command over creatures!");

                // Check if autosummon is enabled before starting the summon timer
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer
                    StopSummonTimer();
                    m_Timer = new SummonOphidianWarriorTimer(pm);
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
                pm.SendMessage(37, "The serpentine power fades from you.");
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
            list.Add("Summons Ophidian Warriors");
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
                // Check if autosummon is enabled before restarting the summon timer
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonOphidianWarriorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOphidianWarriorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOphidianWarriorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is OphidianSerpentineHelm))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OphidianWarrior warrior = new OphidianWarrior
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    warrior.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ophidian Warrior slithers into your service!");
                }
            }
        }
    }
}
