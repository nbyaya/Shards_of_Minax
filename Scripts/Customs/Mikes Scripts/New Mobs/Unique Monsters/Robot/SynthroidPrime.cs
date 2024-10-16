using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Synthroid Prime corpse")]
    public class SynthroidPrime : BaseCreature
    {
        private DateTime m_NextAdaptiveShield;
        private DateTime m_NextCounterattack;
        private ResistanceType m_LastDamageType;
        private bool m_CounterattackReady;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SynthroidPrime()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Synthroid Prime";
            Body = 0x2F5; // ExodusMinion body
            BaseSoundID = 0x2F8;
            Hue = 2721; // Unique metallic blue hue

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_NextAdaptiveShield = DateTime.UtcNow;
            m_NextCounterattack = DateTime.UtcNow;
            m_LastDamageType = ResistanceType.Physical;
            m_CounterattackReady = false;
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SynthroidPrime(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}
        public override bool BardImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }


        public override int GetIdleSound()
        {
            return 0x2CC;
        }

        public override int GetAttackSound()
        {
            return 0x2C8;
        }

        public override int GetDeathSound()
        {
            return 0x2C9;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextAdaptiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCounterattack = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextAdaptiveShield)
                {
                    UseAdaptiveShield();
                }

                if (m_CounterattackReady && DateTime.UtcNow >= m_NextCounterattack)
                {
                    UseCounterattack();
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from is BaseCreature bc && bc.Summoned)
                from = bc.SummonMaster;

            if (from != null && from != this)
            {
                m_LastDamageType = DamageType(from);
                m_CounterattackReady = true;
            }
        }

        private void UseAdaptiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Adaptive Shield Activated *");
            PlaySound(0x1E8);

            int oldPhysicalResist = PhysicalResistance;
            int oldFireResist = FireResistance;
            int oldColdResist = ColdResistance;
            int oldPoisonResist = PoisonResistance;
            int oldEnergyResist = EnergyResistance;

            switch (m_LastDamageType)
            {
                case ResistanceType.Physical:
                    SetResistance(ResistanceType.Physical, 80, 90);
                    break;
                case ResistanceType.Fire:
                    SetResistance(ResistanceType.Fire, 80, 90);
                    break;
                case ResistanceType.Cold:
                    SetResistance(ResistanceType.Cold, 80, 90);
                    break;
                case ResistanceType.Poison:
                    SetResistance(ResistanceType.Poison, 80, 90);
                    break;
                case ResistanceType.Energy:
                    SetResistance(ResistanceType.Energy, 80, 90);
                    break;
            }

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(15), () =>
            {
                SetResistance(ResistanceType.Physical, oldPhysicalResist);
                SetResistance(ResistanceType.Fire, oldFireResist);
                SetResistance(ResistanceType.Cold, oldColdResist);
                SetResistance(ResistanceType.Poison, oldPoisonResist);
                SetResistance(ResistanceType.Energy, oldEnergyResist);
            });

            m_NextAdaptiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for AdaptiveShield
        }

        private void UseCounterattack()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Counterattack Protocol Initiated *");
                PlaySound(0x5B3);

                FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);

                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                target.FixedEffect(0x376A, 9, 32);
                target.PlaySound(0x51D);
            }

            m_CounterattackReady = false;
            m_NextCounterattack = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for Counterattack
        }

        private ResistanceType DamageType(Mobile from)
        {
            if (from.Weapon is BaseWeapon weapon)
            {
                switch (weapon.Type)
                {
                    case WeaponType.Bashing:
                    case WeaponType.Piercing:
                    case WeaponType.Slashing:
                        return ResistanceType.Physical;
                    case WeaponType.Ranged:
                        return ResistanceType.Physical;
                }
            }

            return ResistanceType.Energy;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextAdaptiveShield = DateTime.UtcNow;
            m_NextCounterattack = DateTime.UtcNow;
            m_LastDamageType = ResistanceType.Physical;
            m_CounterattackReady = false;
            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
