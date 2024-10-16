using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a celestial python corpse")]
    public class CelestialPython : BaseCreature
    {
        private DateTime m_NextStarlightConstriction;
        private DateTime m_NextAstralProjection;
        private DateTime m_NextCosmicBurst;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CelestialPython()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Celestial Python";
            Body = 0x15; // Giant Serpent body
            Hue = 1778; // Celestial blue hue
			BaseSoundID = 219;

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

        public CelestialPython(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextStarlightConstriction = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextAstralProjection = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCosmicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextStarlightConstriction)
                {
                    StarlightConstriction();
                }

                if (DateTime.UtcNow >= m_NextAstralProjection)
                {
                    AstralProjection();
                }

                if (DateTime.UtcNow >= m_NextCosmicBurst)
                {
                    CosmicBurst();
                }
            }
        }

        private void StarlightConstriction()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Celestial Python’s constriction dims your strength!");

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    // Apply visual effect on the target
                    Effects.SendTargetEffect(m, 0x376A, 10, 16, 1152);

                    m.SendMessage("The celestial magic of the Python’s constriction weakens your body!");
                    m.Damage(30); // Apply damage to the mobile
                    m.SendMessage("You feel weakened by the constriction!");
                }
            }

            m_NextStarlightConstriction = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Updated cooldown
        }

        private void AstralProjection()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "An astral clone of the Celestial Python appears!");
            Effects.SendLocationEffect(Location, Map, 0x3728, 10, 20, 1152); // Corrected this line

            CelestialPythonClone clone = new CelestialPythonClone(this);
            Point3D loc = GetSpawnPosition(2);
            clone.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(delegate()
            {
                if (!clone.Deleted)
                    clone.Delete();
            }));

            m_NextAstralProjection = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Updated cooldown
        }

        private void CosmicBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "Cosmic energy bursts from the Celestial Python!");
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10, 1152); // Visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.SendMessage("You are hit by a burst of cosmic energy!");
                    m.Damage(damage); // Apply damage to the mobile
                    m.SendMessage("You are slowed by the cosmic burst!");
                    m.SendMessage("Your movement speed is reduced for a short time.");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextCosmicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Updated cooldown
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }

    public class CelestialPythonClone : BaseCreature
    {
        private Mobile m_Master;

        public CelestialPythonClone(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(100);
            SetDex(50);
            SetInt(50);

            SetHits(100);
            SetDamage(10, 20);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 50);

            VirtualArmor = 40;
        }

        public CelestialPythonClone(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
