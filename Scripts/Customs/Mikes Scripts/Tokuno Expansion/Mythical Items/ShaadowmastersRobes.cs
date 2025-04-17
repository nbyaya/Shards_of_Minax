using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ShaadowmastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ShaadowmastersRobes()
        {
            Weight = 2.0;
            Name = "Shadowmaster's Robes";
            Hue = 1175; // Dark, shadowy color

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
            Attributes.WeaponSpeed = 10;
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
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Swords, 15.0);
            SkillBonuses.SetValues(1, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(2, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(3, SkillName.Stealth, 15.0);
            SkillBonuses.SetValues(4, SkillName.Necromancy, 15.0); // Fits the shadow theme

            // Weapon Attributes (for demonstration; typically not applicable to robes)
            WeaponAttributes.HitColdArea = 50;
            WeaponAttributes.HitDispel = 50;
            WeaponAttributes.HitEnergyArea = 50;
            WeaponAttributes.HitFireArea = 50;
            WeaponAttributes.HitFireball = 50;
            WeaponAttributes.HitHarm = 50;
            WeaponAttributes.HitLeechHits = 50;
            WeaponAttributes.HitLeechMana = 50;
            WeaponAttributes.HitLeechStam = 50;
            WeaponAttributes.HitLightning = 50;
            WeaponAttributes.HitLowerAttack = 50;
            WeaponAttributes.HitLowerDefend = 50;
            WeaponAttributes.HitMagicArrow = 50;
            WeaponAttributes.HitPhysicalArea = 50;
            WeaponAttributes.HitPoisonArea = 50;


            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increased follower count
        }

        public ShaadowmastersRobes(Serial serial) : base(serial)
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
                    m_Timer = new SummonShadowWyrmTimer(pm);
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
            list.Add("Summons Shadow Wyrms");
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
                    m_Timer = new SummonShadowWyrmTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonShadowWyrmTimer : Timer
        {
            private Mobile m_Owner;

            public SummonShadowWyrmTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Longer interval for balance
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is ShaadowmastersRobes))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ShadowWyrm wyrm = new ShadowWyrm
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wyrm.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Shadow Wyrm emerges from the darkness to serve you!");
                }
            }
        }
    }
}
