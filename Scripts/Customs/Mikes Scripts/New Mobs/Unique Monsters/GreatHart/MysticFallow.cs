using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a mystic fallow corpse")]
    public class MysticFallow : BaseCreature
    {
        private DateTime m_NextArcaneBlast;
        private DateTime m_NextManaDrain;
        private DateTime m_NextTeleport;
        private DateTime m_NextSummon;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public MysticFallow()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a mystic fallow";
            Body = 0xEA; // GreatHart body
            Hue = 1976; // Ethereal hue

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

        public MysticFallow(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 25; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextArcaneBlast)
                {
                    ArcaneBlast();
                }

                if (DateTime.UtcNow >= m_NextManaDrain)
                {
                    ManaDrain();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }

                if (DateTime.UtcNow >= m_NextSummon)
                {
                    SummonSpirits();
                }
            }
        }

        private void ArcaneBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Fallow unleashes a powerful arcane blast! *");
            PlaySound(0x208);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 0);
                    m.SendMessage("You are struck by a powerful arcane blast!");
                    m.Freeze(TimeSpan.FromSeconds(1)); // 1 second stun effect
                }
            }

            m_NextArcaneBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Fixed cooldown
        }

        private void ManaDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Fallow drains your magical energy! *");
            PlaySound(0x1F3);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Player && m.Mana > 0)
                {
                    int manaDrained = Math.Min(m.Mana, 30);
                    m.Mana -= manaDrained;
                    this.Mana += manaDrained;
                    m.SendMessage("The Mystic Fallow siphons your magical energy!");

                    // Simulate debuff by reducing casting speed
                    Timer.DelayCall(TimeSpan.FromSeconds(10), delegate
                    {
                        m.SendMessage("You feel your magical energy return to normal.");
                    });
                }
            }

            this.Hits += 20; // Heal over time for Mystic Fallow

            m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Fixed cooldown
        }

        private void Teleport()
        {
            if (Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10);
                if (newLocation != Point3D.Zero)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Fallow vanishes and reappears elsewhere! *");
                    this.MoveToWorld(newLocation, Map);
                    PlaySound(0x1FE); // Teleport sound effect
                }
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Fixed cooldown
        }

        private void SummonSpirits()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mystic Fallow summons ethereal deer spirits to aid in battle! *");

            for (int i = 0; i < 3; i++)
            {
                Point3D spawnLocation = GetSpawnPosition(2);
                if (spawnLocation != Point3D.Zero)
                {
                    EtherealDeerSpirit spirit = new EtherealDeerSpirit();
                    spirit.MoveToWorld(spawnLocation, Map);
                }
            }

            m_NextSummon = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Fixed cooldown
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialization
        }
    }

    public class EtherealDeerSpirit : BaseCreature
    {
        [Constructable]
        public EtherealDeerSpirit()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an ethereal deer spirit";
            Body = 0xEA; // GreatHart body
            Hue = 1150; // Ethereal hue

            SetStr(50);
            SetDex(50);
            SetInt(50);

            SetHits(50);
            SetMana(100);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.EvalInt, 50.0, 70.0);
            SetSkill(SkillName.Magery, 50.0, 70.0);

            Fame = 500;
            Karma = 0;

            VirtualArmor = 20;

            Tamable = false;
        }

        public EtherealDeerSpirit(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (Utility.RandomDouble() < 0.1) // 10% chance to cast a small energy blast
                {
                    EnergyBlast();
                }
            }
        }

        private void EnergyBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ethereal deer spirit releases a small burst of energy! *");
            PlaySound(0x1E0);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && m.Player)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 0);
                    m.SendMessage("You are hit by a burst of energy from an ethereal deer spirit!");
                }
            }
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
