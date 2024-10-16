using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Siamese Illusionist corpse")]
    public class SiameseIllusionist : BaseCreature
    {
        private DateTime m_NextPhantomStrike;
        private DateTime m_NextShadowStep;
        private DateTime m_NextEnchantedAgility;
        private DateTime m_EnchantedAgilityEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SiameseIllusionist()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Siamese Illusionist";
            Body = 0xC9; // Cat body
            Hue = 1296; // Unique hue (adjust as needed)
            BaseSoundID = 0x69;

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

        public SiameseIllusionist(Serial serial)
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
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextEnchantedAgility = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_EnchantedAgilityEnd = DateTime.MinValue; // Ensure EnchantedAgilityEnd is initialized
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextPhantomStrike)
                {
                    PhantomStrike();
                }

                if (DateTime.UtcNow >= m_NextShadowStep)
                {
                    ShadowStep();
                }

                if (DateTime.UtcNow >= m_NextEnchantedAgility && DateTime.UtcNow >= m_EnchantedAgilityEnd)
                {
                    EnchantedAgility();
                }

                if (DateTime.UtcNow >= m_EnchantedAgilityEnd && m_EnchantedAgilityEnd != DateTime.MinValue)
                {
                    DeactivateEnchantedAgility();
                }
            }
        }

        private void PhantomStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siamese Illusionist creates phantom images! *");
            PlaySound(0x511);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = GetSpawnPosition(3);
                Effects.SendLocationEffect(loc, Map, 0x37CC, 10, 10, 0, 0);
            }

            m_NextPhantomStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown for PhantomStrike
        }

        private void ShadowStep()
        {
            Mobile target = Combatant as Mobile;
            if (target != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siamese Illusionist vanishes into thin air! *");

                Direction direction = target.GetDirectionTo(this);

                int offsetX = 0, offsetY = 0;
                switch (direction)
                {
                    case Direction.North:
                        offsetY = -1;
                        break;
                    case Direction.South:
                        offsetY = 1;
                        break;
                    case Direction.East:
                        offsetX = 1;
                        break;
                    case Direction.West:
                        offsetX = -1;
                        break;
                }

                Point3D behind = new Point3D(target.X + offsetX, target.Y + offsetY, target.Z);

                Map map = target.Map;

                if (map != null && map.CanSpawnMobile(behind))
                {
                    PlaySound(0x511);
                    Location = behind;
                    Effects.SendLocationEffect(Location, Map, 0x37CC, 10, 10, 0, 0);
                    Combatant = target;
                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown for ShadowStep
                }
            }
        }

        private void EnchantedAgility()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Siamese Illusionist moves with supernatural grace! *");
            PlaySound(0x217);
            FixedEffect(0x376A, 10, 16);

            Dex += 50;
            m_EnchantedAgilityEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextEnchantedAgility = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Fixed cooldown for EnchantedAgility
        }

        private void DeactivateEnchantedAgility()
        {
            Dex -= 50;
            m_EnchantedAgilityEnd = DateTime.MinValue;
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Location;
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
            m_NextPhantomStrike = DateTime.UtcNow;
            m_NextShadowStep = DateTime.UtcNow;
            m_NextEnchantedAgility = DateTime.UtcNow;
            m_EnchantedAgilityEnd = DateTime.MinValue;
        }
    }
}
