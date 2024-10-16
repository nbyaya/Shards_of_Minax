using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an illusionary alligator corpse")]
    public class IllusionaryAlligator : BaseCreature
    {
        private DateTime m_NextIllusionaryBite;
        private DateTime m_NextMirrorImage;
        private int m_ImageCount;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IllusionaryAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an illusionary alligator";
            Body = 0xCA; // Alligator body
            Hue = 1165; // Unique hue (a shade of green for an illusionary effect)
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public IllusionaryAlligator(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextIllusionaryBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIllusionaryBite)
                {
                    PerformIllusionaryBite();
                }

                if (DateTime.UtcNow >= m_NextMirrorImage)
                {
                    CreateMirrorImages();
                }
            }
        }

        private void PerformIllusionaryBite()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                if (target != null && target.Alive)
                {
                    // Inflict damage
                    target.Damage(Utility.RandomMinMax(15, 25), this);

                    // Send message to target
                    target.SendMessage("The illusionary alligator bites you and you feel disoriented!");

                    // Create visual illusion
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x36D4, 10, 30, 2115);

                    // Set next use time with fixed cooldown
                    m_NextIllusionaryBite = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                }
            }
        }

        private void CreateMirrorImages()
        {
            if (m_ImageCount < 3)
            {
                Point3D loc = GetSpawnPosition(2);

                if (loc != Point3D.Zero)
                {
                    IllusionaryImage image = new IllusionaryImage();
                    image.MoveToWorld(loc, Map);

                    // Send message to all nearby players
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The illusionary alligator creates a mirror image of itself! *");

                    m_ImageCount++;
                    m_NextMirrorImage = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Fixed cooldown
                }
            }
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

            return Point3D.Zero;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((int)m_ImageCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_ImageCount = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class IllusionaryImage : BaseCreature
    {
        public IllusionaryImage()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Body = 0xCA; // Same as the illusionary alligator body
            Hue = 1153; // Same hue for consistency
            BaseSoundID = 660;

            SetStr(50, 70);
            SetDex(30, 50);
            SetInt(10, 30);

            SetHits(20, 40);
            SetStam(20, 30);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 20;
        }

        public IllusionaryImage(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

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
