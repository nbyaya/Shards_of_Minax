using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Riptide Crab corpse")]
    public class RiptideCrab : BaseMount
    {
        private DateTime m_NextTidalPull;
        private DateTime m_NextAbyssalSlam;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RiptideCrab()
            : base("Riptide Crab", 1510, 16081, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F2;
            Hue = 1399; // A unique blue hue
			
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

        public RiptideCrab(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Combatant is Mobile)
            {
                Mobile target = (Mobile)Combatant;

                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextTidalPull = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAbyssalSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextTidalPull)
                {
                    TidalPull(target);
                }

                if (DateTime.UtcNow >= m_NextAbyssalSlam)
                {
                    AbyssalSlam(target);
                }
            }
        }

        public void TidalPull(Mobile target)
        {
            if (!target.Alive || !target.InRange(this, 8))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Tidal Pull *");
            PlaySound(0x15E);

            target.MoveToWorld(GetSpawnPosition(1), Map);
            target.Freeze(TimeSpan.FromSeconds(2));

            Effects.SendMovingParticles(this, target, 0x36D4, 7, 0, false, true, 1163, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

            m_NextTidalPull = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 10); // Random cooldown between 20 and 30 seconds
        }

        public void AbyssalSlam(Mobile target)
        {
            if (!target.Alive || !InRange(target, 1))
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "* Abyssal Slam *");
            PlaySound(0x15F);

            int damage = Utility.RandomMinMax(30, 40);

            if (target.Paralyzed || target.Frozen)
                damage *= 2;

            AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);

            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x374A, 1, 17, 1163, 7, 9915, 0);

            m_NextAbyssalSlam = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.RandomDouble() * 15); // Random cooldown between 15 and 30 seconds
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
