using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a wind bear corpse")]
    public class WindBear : BaseCreature
    {
        private DateTime m_NextGaleForce;
        private DateTime m_NextAerialAgility;
        private DateTime m_AerialAgilityEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public WindBear()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wind bear";
            Body = 211; // BlackBear body
            BaseSoundID = 0xA3;
            Hue = 1176; // Light blue hue

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

            // Initialize abilities
            m_AbilitiesInitialized = false;
        }

        public WindBear(Serial serial)
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
                    Random rand = new Random();
                    m_NextGaleForce = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextAerialAgility = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_AerialAgilityEnd = DateTime.MinValue;
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGaleForce)
                {
                    GaleForce();
                }

                if (DateTime.UtcNow >= m_NextAerialAgility && DateTime.UtcNow >= m_AerialAgilityEnd)
                {
                    AerialAgility();
                }

                if (DateTime.UtcNow >= m_AerialAgilityEnd && m_AerialAgilityEnd != DateTime.MinValue)
                {
                    DeactivateAerialAgility();
                }
            }
        }

        private void GaleForce()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Gale Force *");
            PlaySound(0x655);
            FixedEffect(0x37CC, 10, 15, 2721, 0); // Strong wind visual effect

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.MovingEffect(this, 0x1FCB, 7, 0, false, false, 0x481, 0); // Wind effect

                    // Simulate knock-back effect
                    int xOffset = Utility.RandomMinMax(1, 3) * (Utility.RandomBool() ? 1 : -1);
                    int yOffset = Utility.RandomMinMax(1, 3) * (Utility.RandomBool() ? 1 : -1);
                    Point3D newLocation = new Point3D(m.X + xOffset, m.Y + yOffset, m.Z);

                    // Directly set the new location
                    if (m.Map != null && m.Map.CanSpawnMobile(newLocation))
                    {
                        m.Location = newLocation;
                    }
                }
            }

            m_NextGaleForce = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void AerialAgility()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Aerial Agility *");
            PlaySound(0x108);
            FixedEffect(0x376A, 10, 15, 2721, 0); // Whirling wind visual effect

            SetDex(Dex + 30);
            VirtualArmor += 15;

            m_AerialAgilityEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextAerialAgility = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void DeactivateAerialAgility()
        {
            SetDex(Dex - 30);
            VirtualArmor -= 15;

            m_AerialAgilityEnd = DateTime.MinValue;
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

            // Reset initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
