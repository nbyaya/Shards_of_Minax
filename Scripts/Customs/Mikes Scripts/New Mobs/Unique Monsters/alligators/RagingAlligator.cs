using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a raging alligator corpse")]
    public class RagingAlligator : BaseCreature
    {
        private DateTime m_NextRage;
        private DateTime m_NextGroundSlam;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RagingAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a raging alligator";
            Body = 0xCA;
            Hue = 1164; // Unique hue for the raging alligator
            BaseSoundID = 660;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public RagingAlligator(Serial serial)
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

        public override int Meat
        {
            get { return 1; }
        }

        public override int Hides
        {
            get { return 12; }
        }

        public override HideType HideType
        {
            get { return HideType.Spined; }
        }

        public override FoodType FavoriteFood
        {
            get { return FoodType.Meat | FoodType.Fish; }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random initial delay for Rage
                    m_NextGroundSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random initial delay for Ground Slam
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRage)
                {
                    EnterRageMode();
                }

                if (DateTime.UtcNow >= m_NextGroundSlam)
                {
                    PerformGroundSlam();
                }
            }
        }

        private void EnterRageMode()
        {
            this.SetStr(150, 200);
            this.SetDex(100, 150);
            this.SetDamage(20, 30);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The alligator roars with rage, increasing its strength and speed! *");

            m_NextRage = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void PerformGroundSlam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The alligator slams the ground with tremendous force, shaking the area! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                }
            }

            m_NextGroundSlam = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
