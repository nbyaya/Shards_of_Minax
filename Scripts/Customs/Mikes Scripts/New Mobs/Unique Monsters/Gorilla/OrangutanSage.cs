using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an orangutan sage corpse")]
    public class OrangutanSage : BaseCreature
    {
        private DateTime m_NextNatureBlessing;
        private DateTime m_NextEnsnaringVines;
        private DateTime m_NextSummonAssistants;
        private DateTime m_NextQuake;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public OrangutanSage()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Orangutan Sage";
            Body = 0x1D; // Gorilla body
            Hue = 1958; // Unique hue for the Orangutan Sage
			this.BaseSoundID = 0x9E;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public OrangutanSage(Serial serial)
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

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextNatureBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnsnaringVines = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonAssistants = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextQuake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNatureBlessing)
                {
                    NatureBlessing();
                }

                if (DateTime.UtcNow >= m_NextEnsnaringVines)
                {
                    EnsnaringVines();
                }

                if (DateTime.UtcNow >= m_NextSummonAssistants)
                {
                    SummonAssistants();
                }

                if (DateTime.UtcNow >= m_NextQuake)
                {
                    CastQuake();
                }
            }
        }

        private void NatureBlessing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Orangutan Sage calls upon nature to heal its allies!*");

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m is BaseCreature bc && bc != this && bc.Alive)
                {
                    bc.Hits += 40; // Increased healing amount
                    bc.SendMessage("You feel a powerful surge of healing energy!");
                }
            }

            m_NextNatureBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Update the cooldown after use
        }

        private void EnsnaringVines()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Magical vines wrap around you, holding you in place!*");

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.InRange(this, 7) && m.Alive)
                {
                    m.SendMessage("You are ensnared by powerful magical vines!");
                    m.Freeze(TimeSpan.FromSeconds(6)); // Increased freeze duration
                }
            }

            m_NextEnsnaringVines = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Update the cooldown after use
        }

        private void SummonAssistants()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Orangutan Sage summons nature's guardians!*");

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = GetSpawnPosition(5);

                if (loc != Point3D.Zero)
                {
                    NatureGuardian guardian = new NatureGuardian();
                    guardian.MoveToWorld(loc, Map);
                    guardian.Combatant = Combatant;
                }
            }

            m_NextSummonAssistants = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update the cooldown after use
        }

        private void CastQuake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground shakes violently beneath you!*");

            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.InRange(this, 5) && m.Alive)
                {
                    m.SendMessage("The ground shakes violently and you lose your footing!");
                    m.Damage(15, this); // Adjust damage as needed
                }
            }

            m_NextQuake = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Update the cooldown after use
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class NatureGuardian : BaseCreature
    {
        [Constructable]
        public NatureGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Nature Guardian";
            Body = 0x1D; // Gorilla body
            Hue = 1375; // Greenish hue to signify nature

            SetStr(150);
            SetDex(120);
            SetInt(60);

            SetHits(100);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 1000;
            Karma = 1000;

            VirtualArmor = 30;
        }

        public NatureGuardian(Serial serial)
            : base(serial)
        {
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
        }
    }
}
