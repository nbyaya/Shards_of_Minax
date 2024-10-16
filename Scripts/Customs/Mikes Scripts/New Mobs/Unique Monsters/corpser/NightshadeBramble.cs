using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a nightshade bramble corpse")]
    public class NightshadeBramble : BaseCreature
    {
        private DateTime m_NextNightshadeBurst;
        private DateTime m_NextParalysisSpores;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NightshadeBramble()
            : this("Nightshade Bramble")
        {
        }

        [Constructable]
        public NightshadeBramble(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F2;
            Hue = 1386; // Dark purple hue
            this.Body = 8;

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

        public NightshadeBramble(Serial serial)
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
        public override int Hides { get { return 7; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextNightshadeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextParalysisSpores = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNightshadeBurst)
                {
                    NightshadeBurst();
                }

                if (DateTime.UtcNow >= m_NextParalysisSpores)
                {
                    ParalysisSpores();
                }
            }
        }

        private void NightshadeBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Nightshade Bramble releases a burst of toxic berries!*");
            PlaySound(0x108);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.FixedEffect(0x376A, 9, 32);
                    m.PlaySound(0x201);

                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                    m.ApplyPoison(this, Poison.Greater);

                    if (m.Player)
                    {
                        m.SendMessage("You feel confused by the toxic burst!");
                        m.Paralyze(TimeSpan.FromSeconds(3));
                    }
                }
            }

            m_NextNightshadeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for NightshadeBurst
        }

        private void ParalysisSpores()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Nightshade Bramble releases paralysis spores!*");
            PlaySound(0x175);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.FixedEffect(0x374A, 9, 32);
                    m.PlaySound(0x205);

                    m.Paralyze(TimeSpan.FromSeconds(5));

                    if (m.Player)
                    {
                        m.SendMessage("You are paralyzed by the spores!");
                    }
                }
            }

            m_NextParalysisSpores = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for ParalysisSpores
        }

        public override int GetIdleSound()
        {
            return 443;
        }

        public override int GetAngerSound()
        {
            return 442;
        }

        public override int GetHurtSound()
        {
            return 445;
        }

        public override int GetDeathSound()
        {
            return 447;
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
