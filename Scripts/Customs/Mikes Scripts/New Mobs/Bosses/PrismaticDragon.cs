using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class PrismaticDragon : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public PrismaticDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Prismatic Dragon";
            Body = 12; // Dragon body
            BaseSoundID = 362;

            SetStr(1200, 1500);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1500);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 5);
            SetDamageType(ResistanceType.Energy, 5);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 130.0, 150.0);
            SetSkill(SkillName.Magery, 130.0, 150.0);
            SetSkill(SkillName.MagicResist, 200.0, 220.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 60000;
            Karma = -60000;

            _currentPhase = INITIAL_PHASE;
            _nextAbilityTime = DateTime.Now;
        }

        public PrismaticDragon(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (DateTime.Now >= _nextAbilityTime)
            {
                UseAbility();
                _nextAbilityTime = DateTime.Now.AddSeconds(Utility.RandomMinMax(5, 15));
            }

            CheckPhaseTransition();
        }

        private void CheckPhaseTransition()
        {
            int healthPercentage = (Hits * 100) / HitsMax;

            if (healthPercentage <= 25 && _currentPhase != FINAL_PHASE)
            {
                _currentPhase = FINAL_PHASE;
                Say("Witness the full spectrum of my power!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("You have only seen a fraction of my might!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("Prepare for a colorful display!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    PrismBlast();
                    break;
                case PHASE_3:
                    RainbowWave();
                    break;
                case FINAL_PHASE:
                    ChromaticStorm();
                    break;
            }
        }

        private void UseAbility()
        {
            switch (_currentPhase)
            {
                case INITIAL_PHASE:
                    UseInitialPhaseAbility();
                    break;
                case PHASE_2:
                    UsePhase2Ability();
                    break;
                case PHASE_3:
                    UsePhase3Ability();
                    break;
                case FINAL_PHASE:
                    UseFinalPhaseAbility();
                    break;
            }
        }

        private void UseInitialPhaseAbility()
        {
            switch (Utility.Random(3))
            {
                case 0:
                    PrismaticBeam();
                    break;
                case 1:
                    ColorfulFlames();
                    break;
                case 2:
                    RadiantRoar();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                SpectrumSweep();
            else
                PrismStorm();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                ChromaticBreath();
            else
                MultiColoredFireballs();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(5))
            {
                case 0:
                    PrismaticBeam();
                    break;
                case 1:
                    SpectrumSweep();
                    break;
                case 2:
                    ChromaticBreath();
                    break;
                case 3:
                    MultiColoredFireballs();
                    break;
                case 4:
                    ChromaticStorm();
                    break;
            }
        }

        private void PrismaticBeam()
        {
            Say("Feel the purity of my Prismatic Beam!");

            // Define beam parameters
            TimeSpan beamDuration = TimeSpan.FromSeconds(5.0);
            int numberOfBeams = 4; // Number of beams to create

            // Create beams at random locations around the Prismatic Dragon
            for (int i = 0; i < numberOfBeams; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 1.5), new TimerCallback(CreatePrismaticBeam));
            }

            // Effect cleanup
            Timer.DelayCall(beamDuration, new TimerCallback(CleanUpPrismaticBeams));
        }

        private List<Point3D> beamLocations = new List<Point3D>();

        private void CreatePrismaticBeam()
        {
            // Define beam radius and the center position of the boss
            int radius = 10;
            Point3D center = Location;
            double angle = Utility.RandomDouble() * 2.0 * Math.PI; // Random angle for beam direction

            // Calculate the beam end position
            int x = (int)(center.X + radius * Math.Cos(angle));
            int y = (int)(center.Y + radius * Math.Sin(angle));
            int z = center.Z;
            Point3D endLocation = new Point3D(x, y, z);

            // Create visual effect for the beam
            Effects.SendLocationEffect(endLocation, Map, 0x3709, 10, 0); // Prismatic beam effect with a duration of 10

            // Add the location for cleanup later
            beamLocations.Add(endLocation);

            // Deal damage to players in the path
            foreach (Mobile m in GetMobilesInRange(radius))
            {
                if (m.Alive && m.Map == Map)
                {
                    if (m.InRange(endLocation, 2))
                    {
                        m.SendMessage("You are hit by a prismatic beam!");
                        m.Damage(30, this);
                    }
                }
            }
        }

        private void CleanUpPrismaticBeams()
        {
            foreach (Point3D loc in beamLocations)
            {
                Effects.SendLocationEffect(loc, Map, 0x3709, 10, 0); // Remove the prismatic beam effect (duration of 10)
            }
            beamLocations.Clear();
        }

        private void ColorfulFlames()
        {
            Say("Behold my Colorful Flames!");

            // Create a fiery effect around the dragon
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 16, 0x4E, 0);
            Effects.PlaySound(Location, Map, 0x227); // Fire sound effect

            // Apply damage to players within the effect radius
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are burned by colorful flames!");
                    m.Damage(40, this);
                }
            }
        }

        private void RadiantRoar()
        {
            Say("Feel the power of my Radiant Roar!");

            // Create a radiant sound and effect
            Effects.PlaySound(Location, Map, 0x2A8); // Roar sound effect
            Effects.SendLocationEffect(Location, Map, 0x36BD, 30, 10, 0x3E, 0); // Radiant effect

            // Apply damage to players in the area
            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are struck by a radiant roar!");
                    m.Damage(50, this);
                }
            }
        }

        private void PrismBlast()
        {
            Say("Witness the power of my Prism Blast!");

            // Create a burst of prismatic energy
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 0x5F, 0); // Prismatic effect

            // Apply damage to players in the area
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are blasted by prismatic energy!");
                    m.Damage(50, this);
                }
            }
        }

        private void RainbowWave()
        {
            Say("Experience the Rainbow Wave!");

            // Create a colorful wave effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 16, 0x3E, 0); // Rainbow effect

            // Apply damage to players in the area
            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are engulfed by a rainbow wave!");
                    m.Damage(60, this);
                }
            }
        }

        private void ChromaticStorm()
        {
            Say("Behold the Chromatic Storm!");

            // Create a massive storm effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 60, 16, 0x7F, 0); // Chromatic storm effect

            // Apply damage to players in the area
            foreach (Mobile m in GetMobilesInRange(20))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are caught in the chromatic storm!");
                    m.Damage(100, this);
                }
            }
        }

        private void SpectrumSweep()
        {
            Say("Feel the Spectrum Sweep!");

            // Create a sweeping prismatic effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10, 0x4F, 0); // Spectrum effect

            // Apply damage to players in the sweep path
            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are hit by a spectrum sweep!");
                    m.Damage(45, this);
                }
            }
        }

        private void PrismStorm()
        {
            Say("Prepare for the Prism Storm!");

            // Create a storm of prismatic energy
            Effects.SendLocationEffect(Location, Map, 0x3709, 40, 16, 0x6F, 0); // Prism storm effect

            // Apply damage to players in the storm radius
            foreach (Mobile m in GetMobilesInRange(20))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are struck by the prism storm!");
                    m.Damage(70, this);
                }
            }
        }

        private void ChromaticBreath()
        {
            Say("Feel my Chromatic Breath!");

            // Create a breath of prismatic energy
            Effects.SendLocationEffect(Location, Map, 0x3709, 40, 10, 0x7E, 0); // Chromatic breath effect

            // Apply damage to players in the breath path
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are scorched by chromatic breath!");
                    m.Damage(80, this);
                }
            }
        }

        private void MultiColoredFireballs()
        {
            Say("Behold my Multi-Colored Fireballs!");

            // Launch multiple colored fireballs
            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 1.0), new TimerCallback(LaunchFireball));
            }
        }

        private void LaunchFireball()
        {
            // Create a fireball effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10, 0x7F, 0); // Fireball effect

            // Apply damage to players in the fireball's path
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are hit by a multi-colored fireball!");
                    m.Damage(60, this);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(_currentPhase);
            writer.Write(_nextAbilityTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _currentPhase = reader.ReadInt();
            _nextAbilityTime = reader.ReadDateTime();
        }
    }
}
