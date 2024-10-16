using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a flame bear corpse")]
    public class FlameBear : BaseCreature
    {
        private DateTime m_NextFireBreath;
        private DateTime m_NextInfernoRage;
        private DateTime m_InfernoRageEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FlameBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flame bear";
            Body = 211; // BlackBear body
            BaseSoundID = 0xA3;
            Hue = 1197; // Fiery red hue

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FlameBear(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 16; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFireBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextInfernoRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFireBreath)
                {
                    FireBreath();
                }

                if (DateTime.UtcNow >= m_NextInfernoRage && DateTime.UtcNow >= m_InfernoRageEnd)
                {
                    ActivateInfernoRage();
                }
            }

            if (DateTime.UtcNow >= m_InfernoRageEnd && m_InfernoRageEnd != DateTime.MinValue)
            {
                DeactivateInfernoRage();
            }
        }

        private void FireBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Breathes fire! *");
            PlaySound(0x227);
            FixedEffect(0x3709, 10, 30);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075671, 1075672, TimeSpan.FromSeconds(10), m));
                }
            }

            m_NextFireBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ActivateInfernoRage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Inferno Rage! *");
            PlaySound(0x15F);
            FixedEffect(0x37C4, 10, 36);

            // Retrieve current damage values
            int currentMinDamage = DamageMin;
            int currentMaxDamage = DamageMax;

            // Adjust the damage values
            SetDamage(currentMinDamage + 5, currentMaxDamage + 8);

            m_InfernoRageEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextInfernoRage = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void DeactivateInfernoRage()
        {
            // Retrieve current damage values
            int currentMinDamage = DamageMin;
            int currentMaxDamage = DamageMax;

            // Adjust the damage values
            SetDamage(currentMinDamage - 5, currentMaxDamage - 8);

            m_InfernoRageEnd = DateTime.MinValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
