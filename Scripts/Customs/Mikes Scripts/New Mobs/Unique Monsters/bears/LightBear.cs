using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a light bear corpse")]
    public class LightBear : BaseCreature
    {
        private DateTime m_NextRadiantBeam;
        private DateTime m_NextHolyShield;
        private DateTime m_HolyShieldEnd;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LightBear()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a light bear";
            Body = 211; // BlackBear body
            BaseSoundID = 0xA3;
            Hue = 1189; // Bright golden hue

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

        public LightBear(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextRadiantBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextHolyShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRadiantBeam)
                {
                    RadiantBeam();
                }

                if (DateTime.UtcNow >= m_NextHolyShield && DateTime.UtcNow >= m_HolyShieldEnd)
                {
                    HolyShield();
                }
            }

            if (DateTime.UtcNow >= m_HolyShieldEnd && m_HolyShieldEnd != DateTime.MinValue)
            {
                DeactivateHolyShield();
            }
        }

        private void RadiantBeam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Beam *");
            PlaySound(0x212);
            FixedEffect(0x376A, 10, 20, 1153, 0); // Bright light beam visual effect

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    m.SendLocalizedMessage(500896); // You are blinded by the light!
                    m.Paralyze(TimeSpan.FromSeconds(3));
                }
            }

            // Set new random interval for RadiantBeam
            Random rand = new Random();
            m_NextRadiantBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
        }

        private void HolyShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Holy Shield *");
            PlaySound(0x1EA);
            FixedEffect(0x373A, 10, 15, 5024, 0); // Glowing shield visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && (m is BaseCreature && ((BaseCreature)m).Controlled && ((BaseCreature)m).ControlMaster == this.ControlMaster) || m == this.ControlMaster)
                {
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Protection, 1075671, 1075672, TimeSpan.FromSeconds(30), m));
                    m.FixedEffect(0x375A, 10, 15);
                }
            }

            // Set new random interval for HolyShield
            Random rand = new Random();
            m_HolyShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextHolyShield = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));
        }

        private void DeactivateHolyShield()
        {
            m_HolyShieldEnd = DateTime.MinValue;
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
            m_NextRadiantBeam = DateTime.UtcNow;
            m_NextHolyShield = DateTime.UtcNow;
            m_HolyShieldEnd = DateTime.MinValue;
        }
    }
}
