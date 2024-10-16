using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vietnamese pig corpse")]
    public class VietnamesePig : BaseCreature
    {
        private DateTime m_NextThickHide;
        private DateTime m_NextMudSlide;
        private DateTime m_ThickHideEnd;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public VietnamesePig()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vietnamese pig";
            Body = 0xCB; // Pig body
            Hue = 2188; // Light Brown hue
            BaseSoundID = 0xC4; // Pig sound

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
            SetResistance(ResistanceType.Poison, 65, 80);
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

        public VietnamesePig(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextThickHide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMudSlide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextThickHide && DateTime.UtcNow >= m_ThickHideEnd)
                {
                    ActivateThickHide();
                }

                if (DateTime.UtcNow >= m_NextMudSlide)
                {
                    CreateMudSlide();
                }
            }

            if (DateTime.UtcNow >= m_ThickHideEnd && m_ThickHideEnd != DateTime.MinValue)
            {
                DeactivateThickHide();
            }
        }

        private void ActivateThickHide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thick Hide activated! *");
            PlaySound(0x1BB); // Leather sound
            FixedEffect(0x37C4, 10, 36);

            VirtualArmor += 20;

            m_ThickHideEnd = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextThickHide = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void DeactivateThickHide()
        {
            VirtualArmor -= 20;
            m_ThickHideEnd = DateTime.MinValue;
        }

        private void CreateMudSlide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Creates a slippery mud pool! *");
            PlaySound(0x026); // Splash sound
            FixedEffect(0x3789, 10, 20); // Mud effect

            for (int i = 0; i < 5; i++)
            {
                Point3D loc = GetSpawnPosition(3);

                if (loc != Point3D.Zero)
                {
                    MudPool pool = new MudPool();
                    pool.MoveToWorld(loc, Map);

                    // Capture the reference to the pool in a local variable
                    MudPool localPool = pool;
                    
                    Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate
                    {
                        // Use the captured localPool reference
                        if (localPool != null && !localPool.Deleted)
                        {
                            localPool.Delete();
                        }
                    }));
                }
            }

            m_NextMudSlide = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset initialization flag
            m_NextThickHide = DateTime.UtcNow;
            m_NextMudSlide = DateTime.UtcNow;
            m_ThickHideEnd = DateTime.MinValue;
        }
    }

    public class MudPool : Item
    {
        [Constructable]
        public MudPool()
            : base(0x122A)
        {
            Movable = false;
            Hue = 0x21F;
            Name = "mud pool";
        }

        public MudPool(Serial serial)
            : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m.Player && m.InRange(this, 0))
            {
                m.SendLocalizedMessage(1010512); // The mud slows your movement.
            }
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
        }
    }
}
