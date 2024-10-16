using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of a vampiric blade")]
    public class VampiricBlade : BaseCreature
    {
        private DateTime m_NextBloodDrain;
        private DateTime m_NextVampireEmbrace;
        private DateTime m_VampireEmbraceEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VampiricBlade()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vampiric blade";
            Body = 57; // BoneKnight body
            BaseSoundID = 451;
            Hue = 2362; // Dark red hue

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

            PackItem(new VampiricBladeSword());

            // Initialize ability cooldowns and flag
            m_AbilitiesInitialized = false;
        }

        public VampiricBlade(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextBloodDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextVampireEmbrace = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBloodDrain)
                {
                    BloodDrain();
                }

                if (DateTime.UtcNow >= m_NextVampireEmbrace && DateTime.UtcNow >= m_VampireEmbraceEnd)
                {
                    VampireEmbrace();
                }
            }

            if (DateTime.UtcNow >= m_VampireEmbraceEnd && m_VampireEmbraceEnd != DateTime.MinValue)
            {
                DeactivateVampireEmbrace();
            }
        }

        private void BloodDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Blood Drain *");
            PlaySound(0x1FA);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    Hits = Math.Min(Hits + damage, HitsMax);

                    m.FixedParticles(0x376A, 9, 32, 5005, 0x21, 0, EffectLayer.Waist);
                    this.FixedParticles(0x376A, 9, 32, 5005, 0x21, 0, EffectLayer.Waist);
                    break; // Only affect one target
                }
            }

            m_NextBloodDrain = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Cooldown for BloodDrain
        }

        private void VampireEmbrace()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vampire's Embrace *");
            PlaySound(0x1ED);
            FixedParticles(0x376A, 9, 32, 5030, 0x21, 0, EffectLayer.Waist);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            m_VampireEmbraceEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextVampireEmbrace = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for VampireEmbrace
        }

        private void DeactivateVampireEmbrace()
        {
            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);
            m_VampireEmbraceEnd = DateTime.MinValue;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (m_VampireEmbraceEnd > DateTime.UtcNow)
            {
                int damage = Utility.RandomMinMax(5, 10);
                AOS.Damage(defender, this, damage, 0, 0, 0, 0, 100);
                Hits = Math.Min(Hits + damage, HitsMax);
                defender.FixedParticles(0x376A, 9, 32, 5005, 0x21, 0, EffectLayer.Waist);
            }
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

            // Reset ability initialization flag
            m_AbilitiesInitialized = false;
        }
    }

    public class VampiricBladeSword : BaseSword
    {
        [Constructable]
        public VampiricBladeSword()
            : base(0xF5E)
        {
            Name = "Vampiric Blade Sword";
            Hue = 1157; // Dark red hue
            WeaponAttributes.HitLeechHits = 15;
        }

        public VampiricBladeSword(Serial serial)
            : base(serial)
        {
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
        }
    }
}
