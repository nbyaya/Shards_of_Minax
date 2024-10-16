using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a capricorn mountainbear corpse")]
    public class CapricornMountainBear : GrizzlyBear
    {
        private DateTime m_NextMountainClimb;
        private DateTime m_NextRockSlide;
        private DateTime m_NextTerritorialRoar;
        private DateTime m_NextSummonAllies;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public CapricornMountainBear()
            : base()
        {
            Name = "a Capricorn MountainBear";
            Hue = 2018; // Unique hue for Capricorn MountainBear
			BaseSoundID = 0xA3;
			
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

        public CapricornMountainBear(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMountainClimb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTerritorialRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMountainClimb)
                {
                    MountainClimb();
                    m_NextMountainClimb = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for Mountain Climb
                }

                if (DateTime.UtcNow >= m_NextRockSlide)
                {
                    RockSlide();
                    m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Rock Slide
                }

                if (DateTime.UtcNow >= m_NextTerritorialRoar)
                {
                    TerritorialRoar();
                    m_NextTerritorialRoar = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Territorial Roar
                }

                if (DateTime.UtcNow >= m_NextSummonAllies)
                {
                    SummonAllies();
                    m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for Summon Allies
                }

                if (Utility.RandomDouble() < 0.20)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn MountainBear endures the attack with unyielding strength! *");
                    this.VirtualArmor = 50; // Temporary increase in virtual armor
                }
                else
                {
                    this.VirtualArmor = 24; // Reset to default armor
                }
            }
        }

        private void MountainClimb()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn MountainBear climbs to new heights! *");
            FixedEffect(0x376A, 10, 16); // Climbing effect

            Point3D newLoc = GetClimbLocation(5);
            if (newLoc != Point3D.Zero)
            {
                MoveToWorld(newLoc, Map);
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("You feel the ground shake as the Capricorn MountainBear climbs!");
                        AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    }
                }
            }
        }

        private void RockSlide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn MountainBear causes a rock slide! *");
            FixedEffect(0x36BD, 10, 20); // Rock slide effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are struck by falling rocks!");
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                }
            }
        }

        private void TerritorialRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn MountainBear lets out a terrifying roar! *");
            FixedEffect(0x2A2C, 10, 16); // Roar effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The roar stuns you momentarily!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
                }
            }
        }

        private void SummonAllies()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn MountainBear summons allies from the mountains! *");

            for (int i = 0; i < 2; i++) // Summon 2 allies
            {
                BaseCreature ally = new MountainGoat(); // Example ally
                ally.MoveToWorld(Location, Map);
                ally.Combatant = Combatant;
            }
        }

        private Point3D GetClimbLocation(int range)
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
