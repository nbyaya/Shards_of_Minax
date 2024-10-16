using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a wind chicken corpse")]
    public class WindChicken : BaseCreature
    {
        private DateTime m_NextStormEgg;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public WindChicken()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Wind Chicken";
            Body = 0xD0; // Chicken body
            BaseSoundID = 0x6E;
            Hue = 1361; // Sky blue hue

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

        public WindChicken(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override bool CanFly { get { return true; } }
        public override int Feathers { get { return 25; } }

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
                    Random rand = new Random();
                    m_NextStormEgg = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStormEgg)
                {
                    LayStormEgg();
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 > Utility.RandomDouble()) // 25% chance
            {
                GalePeck(defender);
            }
        }

        private void GalePeck(Mobile defender)
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Gale Peck! *");
            defender.PlaySound(0x655);
            defender.FixedParticles(0x3728, 10, 30, 5052, EffectLayer.Waist);

            int damage = Utility.RandomMinMax(4, 8);
            AOS.Damage(defender, this, damage, 50, 0, 0, 0, 50);

            int pushRange = Utility.RandomMinMax(1, 3);
            Direction pushDirection = (Direction)Utility.Random(8);
            Point3D newLocation = GetNewLocation(defender.Location, pushDirection, pushRange);

            defender.MovingEffect(defender, 0x1FE4, 5, 0, false, false, 0, 0);
            defender.Location = newLocation;
        }

        private void LayStormEgg()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lays a Storm Egg! *");
            PlaySound(0x655);

            StormEgg egg = new StormEgg(this);
            egg.MoveToWorld(Location, Map);

            Random rand = new Random();
            m_NextStormEgg = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60)); // Random cooldown between 20 and 60 seconds
        }

        private Point3D GetNewLocation(Point3D startLocation, Direction direction, int distance)
        {
            Point3D newLocation = startLocation;

            switch (direction)
            {
                case Direction.North:
                    newLocation.Y -= distance;
                    break;
                case Direction.South:
                    newLocation.Y += distance;
                    break;
                case Direction.East:
                    newLocation.X += distance;
                    break;
                case Direction.West:
                    newLocation.X -= distance;
                    break;
            }

            return newLocation;
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class StormEgg : Item
    {
        private Mobile m_Owner;
        private DateTime m_ExplosionTime;

        public StormEgg(Mobile owner) : base(0x9B5)
        {
            m_Owner = owner;
            Movable = false;
            Hue = 2720; // Sky blue hue
            Name = "Storm Egg";

            m_ExplosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Explode));
        }

        public StormEgg(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this.GetWorldLocation(), 0))
            {
                Explode();
            }
        }

        public void Explode()
        {
            if (Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The egg explodes in a gust of wind! *");

            Effects.PlaySound(GetWorldLocation(), Map, 0x655);

            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x3728, 20, 10, 2720, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != m_Owner && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(8, 15);
                    AOS.Damage(m, m_Owner, damage, 50, 0, 0, 0, 50);

                    m.PlaySound(0x655);
                    m.FixedParticles(0x3728, 10, 30, 5052, EffectLayer.Waist);

                    int pushRange = Utility.RandomMinMax(2, 4);
                    Direction pushDirection = (Direction)Utility.Random(8);
                    Point3D newLocation = GetNewLocation(m.Location, pushDirection, pushRange);

                    m.MovingEffect(m, 0x1FE4, 5, 0, false, false, 0, 0);
                    m.Location = newLocation;
                }
            }

            this.Delete();
        }

        private Point3D GetNewLocation(Point3D startLocation, Direction direction, int distance)
        {
            Point3D newLocation = startLocation;

            switch (direction)
            {
                case Direction.North:
                    newLocation.Y -= distance;
                    break;
                case Direction.South:
                    newLocation.Y += distance;
                    break;
                case Direction.East:
                    newLocation.X += distance;
                    break;
                case Direction.West:
                    newLocation.X -= distance;
                    break;
            }

            return newLocation;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Owner);
            writer.Write(m_ExplosionTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_ExplosionTime = reader.ReadDateTime();

            if (DateTime.UtcNow < m_ExplosionTime)
            {
                TimeSpan delay = m_ExplosionTime - DateTime.UtcNow;
                Timer.DelayCall(delay, new TimerCallback(Explode));
            }
            else
            {
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Explode));
            }
        }
    }
}
