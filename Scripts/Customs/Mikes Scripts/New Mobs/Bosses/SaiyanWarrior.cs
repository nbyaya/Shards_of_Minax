using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class SaiyanWarrior : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public SaiyanWarrior() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Saiyan Warrior";
            Body = 400; // Saiyan-themed body type; adjust if needed
            BaseSoundID = 0x45A; // Adjust for a Saiyan-like sound

            SetStr(1200, 1500);
            SetDex(200, 300);
            SetInt(200, 300);

            SetHits(2000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 130.0);
            SetSkill(SkillName.Magery, 130.0);
            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 60000;
            Karma = -60000;

            _currentPhase = INITIAL_PHASE;
            _nextAbilityTime = DateTime.Now;
        }

        public SaiyanWarrior(Serial serial) : base(serial)
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
                Say("Witness my ultimate form!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("Feel the power of a Saiyan!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("Prepare to face a Saiyan's wrath!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    EnergyField();
                    break;
                case PHASE_3:
                    SuperKamehameha();
                    break;
                case FINAL_PHASE:
                    FinalFlash();
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
                    KiBlast();
                    break;
                case 1:
                    EnergyWave();
                    break;
                case 2:
                    MeteorSmash();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                SolarFlare();
            else
                EnergyStorm();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                FinalKamehameha();
            else
                SpiritBomb();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(4))
            {
                case 0:
                    KiBlast();
                    break;
                case 1:
                    EnergyWave();
                    break;
                case 2:
                    SolarFlare();
                    break;
                case 3:
                    FinalFlash();
                    break;
            }
        }

        // Implement Saiyan-themed abilities
        private void KiBlast()
        {
            Say("Feel the power of my Ki Blast!");

            // Define projectile parameters
            int numberOfBlasts = 6;
            double radius = 15.0;
            int damage = 40;

            for (int i = 0; i < numberOfBlasts; i++)
            {
                double angle = i * (360.0 / numberOfBlasts);
                double radians = angle * (Math.PI / 180.0);
                double x = radius * Math.Cos(radians);
                double y = radius * Math.Sin(radians);
                Point3D targetLocation = new Point3D(Location.X + (int)x, Location.Y + (int)y, Location.Z);

                // Create and send Ki projectile
                CreateKiBlast(targetLocation, damage);
            }
        }

        private void CreateKiBlast(Point3D targetLocation, int damage)
        {
            // Create a Ki blast effect
            Effects.SendLocationEffect(targetLocation, Map, 0x36D4, 30, 10); // Adjust graphic ID as needed

            // Apply damage to players at the target location
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m is PlayerMobile && m.InRange(targetLocation, 1))
                {
                    m.Damage(damage, this);
                    m.SendMessage("You are hit by a Ki Blast!");
                }
            }
        }

        private void EnergyWave()
        {
            Say("Take this Energy Wave!");

            // Create an energy wave effect
            int waveRange = 15;
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Energy wave visual effect

            foreach (Mobile m in GetMobilesInRange(waveRange))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(50, this);
                    m.SendMessage("You are struck by an Energy Wave!");
                }
            }
        }

        private void MeteorSmash()
        {
            Say("Meteor Smash!");

            // Create meteors falling from the sky
            int numMeteors = 5;

            for (int i = 0; i < numMeteors; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), new TimerCallback(SpawnMeteor));
            }
        }

        private void SpawnMeteor()
        {
            // Random meteor drop location
            int x = this.X + Utility.RandomMinMax(-10, 10);
            int y = this.Y + Utility.RandomMinMax(-10, 10);
            int z = this.Z + 20; // Drop from above

            // Create a meteor item
            Item meteor = new Boulder(); // Placeholder; replace with a suitable meteor item or effect
            meteor.MoveToWorld(new Point3D(x, y, z), this.Map);

            // Drop meteor to ground
            Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(() =>
            {
                meteor.Delete();
                CreateMeteorImpact(x, y);
            }));
        }

        private void CreateMeteorImpact(int x, int y)
        {
            // Create impact effect
            Effects.SendLocationEffect(new Point3D(x, y, this.Z), Map, 0x3709, 30); // Adjust effect ID

            // Apply damage
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m is PlayerMobile && m != this)
                {
                    m.Damage(30, this);
                    m.SendMessage("You are burned by a meteor!");
                }
            }
        }

        private void EnergyField()
        {
            Say("Energy Field activated!");

            // Create an energy field effect around the boss
            double radius = 10.0;
            TimeSpan duration = TimeSpan.FromSeconds(20.0);

            // Create the field effect
            CreateEnergyFieldEffect(radius);

            Timer fieldTimer = new EnergyFieldTimer(this, radius, duration);
            fieldTimer.Start();
        }

        private void CreateEnergyFieldEffect(double radius)
        {
            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 16); // Energy field effect

            // Create a circle around the boss
            for (double angle = 0; angle < 360; angle += 10)
            {
                double radians = angle * (Math.PI / 180.0);
                double x = radius * Math.Cos(radians);
                double y = radius * Math.Sin(radians);
                Point3D point = new Point3D(Location.X + (int)x, Location.Y + (int)y, Location.Z);
                Effects.SendLocationEffect(point, Map, 0x3709, 10, 16);
            }
        }

        private void SuperKamehameha()
        {
            Say("Super Kamehameha!");

            // Create a Kamehameha effect
            int range = 20;
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Kamehameha effect

            foreach (Mobile m in GetMobilesInRange(range))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(100, this);
                    m.SendMessage("You are blasted by a Super Kamehameha!");
                }
            }
        }

        private void FinalFlash()
        {
            Say("Final Flash!");

            // Create Final Flash effect
            int range = 25;
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Final Flash effect

            foreach (Mobile m in GetMobilesInRange(range))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(150, this);
                    m.SendMessage("You are overwhelmed by Final Flash!");
                }
            }
        }

        private void SolarFlare()
        {
            Say("Solar Flare!");

            // Create a blinding solar flare effect
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10); // Solar Flare effect

            foreach (Mobile m in GetMobilesInRange(15))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are blinded by Solar Flare!");
                    m.SendMessage("You are disoriented!");
                    m.SendMessage("You have been stunned!");
                }
            }
        }

        private void EnergyStorm()
        {
            Say("Energy Storm!");

            // Create an Energy Storm effect
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Energy Storm effect

            foreach (Mobile m in GetMobilesInRange(20))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(60, this);
                    m.SendMessage("You are struck by Energy Storm!");
                }
            }
        }

        private void FinalKamehameha()
        {
            Say("Final Kamehameha!");

            // Create a Final Kamehameha effect
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Final Kamehameha effect

            foreach (Mobile m in GetMobilesInRange(25))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(150, this);
                    m.SendMessage("You are devastated by Final Kamehameha!");
                }
            }
        }

        private void SpiritBomb()
        {
            Say("Spirit Bomb!");

            // Create a Spirit Bomb effect
            Effects.SendLocationEffect(Location, Map, 0x36D4, 30, 10); // Spirit Bomb effect

            foreach (Mobile m in GetMobilesInRange(30))
            {
                if (m is PlayerMobile)
                {
                    m.Damage(200, this);
                    m.SendMessage("You are annihilated by Spirit Bomb!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
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

        private class EnergyFieldTimer : Timer
        {
            private SaiyanWarrior _warrior;
            private double _radius;
            private TimeSpan _duration;

            public EnergyFieldTimer(SaiyanWarrior warrior, double radius, TimeSpan duration)
                : base(duration)
            {
                _warrior = warrior;
                _radius = radius;
                _duration = duration;
            }

            protected override void OnTick()
            {
                // Effect ends after duration
                _warrior.SendMessage("The Energy Field fades away.");
            }
        }
    }
}
