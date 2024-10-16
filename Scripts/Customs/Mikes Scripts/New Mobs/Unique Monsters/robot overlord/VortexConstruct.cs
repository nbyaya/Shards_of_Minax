using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a vortex construct corpse")]
    public class VortexConstruct : BaseCreature
    {
        private DateTime m_NextGravitonPulse;
        private DateTime m_NextGravityWell;
        private DateTime m_NextBlackHole;
        private DateTime m_NextGravitySurge;
        private DateTime m_NextTeleport;
        private DateTime m_NextField;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        private const int GravitonPulseCooldown = 15; // seconds
        private const int GravityWellCooldown = 30;   // seconds
        private const int BlackHoleCooldown = 45;     // seconds
        private const int GravitySurgeCooldown = 60;  // seconds
        private const int TeleportCooldown = 30;      // seconds
        private const int FieldCooldown = 20;         // seconds

        private bool m_FieldActive;

        [Constructable]
        public VortexConstruct()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vortex construct";
            Body = 0x2F4; // ExodusOverseer body
            Hue = 2274;   // Unique hue for Vortex Construct

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

        public VortexConstruct(Serial serial)
            : base(serial)
        {
        }
        public override int GetIdleSound()
        {
            return 0xFD;
        }

        public override int GetAngerSound()
        {
            return 0x26C;
        }

        public override int GetDeathSound()
        {
            return 0x211;
        }

        public override int GetAttackSound()
        {
            return 0x23B;
        }

        public override int GetHurtSound()
        {
            return 0x140;
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
                    m_NextGravitonPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGravityWell = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBlackHole = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextGravitySurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextField = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextGravitonPulse)
                {
                    GravitonPulse();
                }

                if (DateTime.UtcNow >= m_NextGravityWell)
                {
                    GravityWell();
                }

                if (DateTime.UtcNow >= m_NextBlackHole)
                {
                    BlackHole();
                }

                if (DateTime.UtcNow >= m_NextGravitySurge)
                {
                    GravitySurge();
                }

                if (DateTime.UtcNow >= m_NextTeleport)
                {
                    Teleport();
                }

                if (DateTime.UtcNow >= m_NextField)
                {
                    ActivateField();
                }
            }
        }

        private void GravitonPulse()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Emits a powerful Graviton Pulse! *");
            FixedEffect(0x3735, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.MoveToWorld(this.Location, Map);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);
                }
            }

            m_NextGravitonPulse = DateTime.UtcNow + TimeSpan.FromSeconds(GravitonPulseCooldown);
        }

        private void GravityWell()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Creates a Gravity Well! *");
            FixedEffect(0x3735, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.FixedParticles(0x3735, 1, 30, 0x251F, EffectLayer.Waist);
                    m.SendMessage("You feel the intense gravitational pull of the well!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
                }
            }

            m_NextGravityWell = DateTime.UtcNow + TimeSpan.FromSeconds(GravityWellCooldown);
        }

        private void BlackHole()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Summons a Black Hole! *");
            FixedEffect(0x3735, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.MoveToWorld(this.Location, Map);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 0, 0, 0, 100);
                    m.SendMessage("The black hole pulls you in and crushes you!");
                }
            }

            m_NextBlackHole = DateTime.UtcNow + TimeSpan.FromSeconds(BlackHoleCooldown);
        }

        private void GravitySurge()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Channels a Gravity Surge! *");
            FixedEffect(0x3735, 10, 16);

            this.SetDamage(this.DamageMin + 10, this.DamageMax + 10);
            this.VirtualArmor += 20;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.SetDamage(this.DamageMin - 10, this.DamageMax - 10);
                this.VirtualArmor -= 20;
            });

            m_NextGravitySurge = DateTime.UtcNow + TimeSpan.FromSeconds(GravitySurgeCooldown);
        }

        private void Teleport()
        {
            PublicOverheadMessage(0, 0x3B2, true, "* Teleports to a new location! *");

            Point3D newLocation = GetSpawnPosition(10);
            if (newLocation != Point3D.Zero)
            {
                this.Location = newLocation;
                this.ProcessDelta();
                FixedEffect(0x376A, 10, 16);
            }

            m_NextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(TeleportCooldown);
        }

        private void ActivateField()
        {
            m_FieldActive = !m_FieldActive;

            if (m_FieldActive)
            {
                PublicOverheadMessage(0, 0x3B2, true, "* Activates gravitational field! *");
                FixedEffect(0x3735, 10, 16);
                this.VirtualArmor += 30;
            }
            else
            {
                PublicOverheadMessage(0, 0x3B2, true, "* Deactivates gravitational field! *");
                FixedEffect(0x3735, 10, 16);
                this.VirtualArmor -= 30;
            }

            m_NextField = DateTime.UtcNow + TimeSpan.FromSeconds(FieldCooldown);
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
}
