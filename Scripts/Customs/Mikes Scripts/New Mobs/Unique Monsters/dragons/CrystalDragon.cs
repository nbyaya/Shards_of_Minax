using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystal dragon corpse")]
    public class CrystalDragon : BaseCreature
    {
        private DateTime m_NextCrystalBreath;
        private DateTime m_NextPrismaticShield;
        private DateTime m_NextCrystalShards;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CrystalDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a crystal dragon";
            Body = 59; // Dragon body
            Hue = 1485; // Unique crystalline hue
            BaseSoundID = 362;

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

            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public CrystalDragon(Serial serial)
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
                    m_NextCrystalBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextPrismaticShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCrystalShards = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrystalBreath)
                {
                    CrystalBreath();
                }

                if (DateTime.UtcNow >= m_NextPrismaticShield)
                {
                    PrismaticShield();
                }

                if (DateTime.UtcNow >= m_NextCrystalShards)
                {
                    CrystalShards();
                }
            }
        }

        private void CrystalBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    double damage = Utility.RandomMinMax(30, 40);
                    AOS.Damage(target, this, (int)damage, 0, 0, 100, 0, 0);

                    target.SendMessage("You are scorched by the crystal dragon's breath!");
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The crystal dragon unleashes a blinding breath attack! *");

                    m_NextCrystalBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

        private void PrismaticShield()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The crystal dragon surrounds itself with a prismatic shield! *");
            this.SendMessage("The crystal dragon's prismatic shield reflects a portion of your attacks!");

            m_NextPrismaticShield = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void CrystalShards()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    double damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(target, this, (int)damage, 0, 0, 0, 100, 0);

                    target.SendMessage("You are pierced by sharp crystal shards!");
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The crystal dragon launches a flurry of sharp shards! *");

                    target.SendMessage("You feel yourself slowing down!");

                    m_NextCrystalShards = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
            m_NextCrystalBreath = DateTime.UtcNow; // Reset ability cooldowns
            m_NextPrismaticShield = DateTime.UtcNow;
            m_NextCrystalShards = DateTime.UtcNow;
        }
    }
}
