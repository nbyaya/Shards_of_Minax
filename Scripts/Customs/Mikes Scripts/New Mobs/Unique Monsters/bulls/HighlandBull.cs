using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a highland bull corpse")]
    public class HighlandBull : BaseCreature
    {
        private DateTime m_NextRockyCharge;
        private DateTime m_NextHighlandRoar;
        private DateTime m_NextToughHide;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public HighlandBull()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a highland bull";
            Body = 0xE8;
            BaseSoundID = 0x64;
            Hue = 1284; // Unique hue (you can adjust this)

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

        public HighlandBull(Serial serial)
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

        public override int Meat { get { return 15; } }
        public override int Hides { get { return 20; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bull; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextRockyCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                    m_NextHighlandRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 20));
                    m_NextToughHide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRockyCharge)
                {
                    DoRockyCharge();
                }
                else if (DateTime.UtcNow >= m_NextHighlandRoar)
                {
                    DoHighlandRoar();
                }
                else if (DateTime.UtcNow >= m_NextToughHide)
                {
                    DoToughHide();
                }
            }
        }

        private void DoRockyCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Highland Bull charges! *");
            PlaySound(0x3E9);

            if (Combatant is Mobile target && target.Alive)
            {
                Direction = GetDirectionTo(target);
                Move(Direction);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (InRange(target, 1))
                    {
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                        target.MovingEffect(this, 0xF51, 6, 0, false, false, 0, 0);
                        target.MoveToWorld(GetSpawnPosition(1), Map);
                        target.Freeze(TimeSpan.FromSeconds(2));
                    }
                });
            }

            m_NextRockyCharge = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for Rocky Charge
        }

        private void DoHighlandRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Highland Bull lets out a mighty roar! *");
            PlaySound(0x3E9);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendLocalizedMessage(1010576); // You are stunned by the roar and can't move!
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextHighlandRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Highland Roar
        }

        private void DoToughHide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Highland Bull's hide toughens! *");
            PlaySound(0x247);

            // Increase resistance
            int buffDuration = 10; // Duration in seconds
            int resistanceIncrease = 15;

            // Apply resistance modification
            SetResistance(ResistanceType.Physical, 35 + resistanceIncrease); // Update to include the buff

            // Create a timer to revert the resistance back after buff duration
            Timer.DelayCall(TimeSpan.FromSeconds(buffDuration), () =>
            {
                // Revert resistance modification
                SetResistance(ResistanceType.Physical, 35); // Reset to the original resistance value
            });

            m_NextToughHide = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Tough Hide
        }

		private Point3D GetSpawnPosition(int range)
		{
			if (Map == null || Deleted)
				return Location;  // Fallback if not placed in world

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

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
