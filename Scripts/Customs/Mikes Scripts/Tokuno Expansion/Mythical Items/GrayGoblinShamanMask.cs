using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GrayGoblinShamanMask : OrcHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GrayGoblinShamanMask()
        {
            Weight = 5.0;
            Name = "Gray Goblin Shaman Mask";
            Hue = 2101; // A grayish tone

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;

            PhysicalBonus = 10;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 15.0);

            ArmorAttributes.MageArmor = 1;
            ArmorAttributes.SelfRepair = 3;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GrayGoblinShamanMask(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an ancient goblin power enhancing your control over minions!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGrayGoblinMageTimer(pm);
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
                pm.SendMessage(37, "The goblin magic fades, and you feel your control diminishing.");
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
            list.Add("Summons Gray Goblin Mages");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGrayGoblinMageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGrayGoblinMageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGrayGoblinMageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is GrayGoblinShamanMask))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and player has room for more followers
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GrayGoblinMage goblinMage = new GrayGoblinMage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    goblinMage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gray Goblin Mage emerges to aid you!");
                }
            }
        }
    }
}
