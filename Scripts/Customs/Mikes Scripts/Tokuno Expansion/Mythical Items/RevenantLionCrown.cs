using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RevenantLionCrown : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RevenantLionCrown()
        {
            Weight = 5.0;
            Name = "Crown of the Revenant Lion";
            Hue = 1157; // Gold with a fiery hue

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 20;
            Attributes.BonusStam = 20;
            Attributes.BonusMana = 20;
            Attributes.RegenHits = 5;
            Attributes.RegenStam = 5;
            Attributes.RegenMana = 5;
            Attributes.WeaponDamage = 20;
            Attributes.DefendChance = 15;
            Attributes.AttackChance = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.EnhancePotions = 25;
            Attributes.Luck = 100;
            Attributes.SpellDamage = 12;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 15;
            ColdBonus = 15;
            PoisonBonus = 15;
            EnergyBonus = 15;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(3, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(4, SkillName.Wrestling, 15.0);

            // Armor Attributes
            ArmorAttributes.DurabilityBonus = 50;
            ArmorAttributes.LowerStatReq = 50;
            ArmorAttributes.SelfRepair = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2
        }

        public RevenantLionCrown(Serial serial) : base(serial)
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
                m_Timer = new SummonRevenantLionTimer(pm);
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
            list.Add("Summons Revenant Lions");
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
                m_Timer = new SummonRevenantLionTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonRevenantLionTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRevenantLionTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is RevenantLionCrown))
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
                    RevenantLion lion = new RevenantLion
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    lion.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Revenant Lion emerges to serve you!");
                }
            }
        }
    }
}
