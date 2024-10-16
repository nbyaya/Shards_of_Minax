using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class FrostWraith : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public FrostWraith() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Frost Wraith";
            Body = 0x9; // A suitable body type for a wraith
            BaseSoundID = 0x482;

            SetStr(986, 1185);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(1000);

            SetDamage(22, 29);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 150.5, 200.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);

            Fame = 50000;
            Karma = -50000;

            _currentPhase = INITIAL_PHASE;
            _nextAbilityTime = DateTime.Now;
        }

        public FrostWraith(Serial serial) : base(serial)
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
                Say("Feel the icy grip of my final form!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("Prepare for the blizzard!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("Embrace the frost!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    FrostNova();
                    break;
                case PHASE_3:
                    IceStorm();
                    break;
                case FINAL_PHASE:
                    Blizzard();
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
                    IceBlast();
                    break;
                case 1:
                    FrozenBarrier();
                    break;
                case 2:
                    ChillingBeam();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                FrostNova();
            else
                IceStorm();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                Blizzard();
            else
                GlacialSpear();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(5))
            {
                case 0:
                    IceBlast();
                    break;
                case 1:
                    FrozenBarrier();
                    break;
                case 2:
                    ChillingBeam();
                    break;
                case 3:
                    Blizzard();
                    break;
                case 4:
                    GlacialTornado();
                    break;
            }
        }

        // Implement individual abilities
        private void IceBlast()
        {
            Say("Feel the chill of my Ice Blast!");

            int damage = Utility.RandomMinMax(30, 50);
            Point3D targetLocation = Combatant.Location;

            // Create ice blast effect
            Effects.SendLocationEffect(targetLocation, Map, 0x23B2, 30, 10); // Ice blast graphic
            Combatant.Damage(damage, this);

            // Optional: Add some visual effects or messages
        }

        private void FrozenBarrier()
        {
            Say("You are trapped in my Frozen Barrier!");

            double barrierRadius = 10.0; // Radius of the barrier
            TimeSpan barrierDuration = TimeSpan.FromSeconds(20.0); // Barrier active for 20 seconds
            TimeSpan freezeInterval = TimeSpan.FromSeconds(3.0); // Freezes every 3 seconds

            // Create the barrier effect
            CreateBarrierEffect(barrierRadius);

            Timer barrierTimer = new BarrierTimer(this, barrierRadius, barrierDuration, freezeInterval);
            barrierTimer.Start();
        }

        private void CreateBarrierEffect(double radius)
        {
            // Create a frosty barrier effect at the FrostWraith's location
            Effects.SendLocationEffect(this.Location, this.Map, 0x6B4, 10, 16, 0xB4, 0); // Adjust as needed

            // Create a blue circle around the barrier
            for (double angle = 0; angle < 360; angle += 15)
            {
                double radians = angle * Math.PI / 180.0;
                int x = (int)(radius * Math.Cos(radians));
                int y = (int)(radius * Math.Sin(radians));

                Effects.SendLocationEffect(new Point3D(this.X + x, this.Y + y, this.Z), this.Map, 0x6B4, 10, 16, 0xB4, 0);
            }
        }

        private class BarrierTimer : Timer
        {
            private FrostWraith _wraith;
            private double _radius;
            private TimeSpan _duration;
            private TimeSpan _freezeInterval;
            private DateTime _endTime;
            private DateTime _nextFreezeTime;

            public BarrierTimer(FrostWraith wraith, double radius, TimeSpan duration, TimeSpan freezeInterval)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                _wraith = wraith;
                _radius = radius;
                _duration = duration;
                _freezeInterval = freezeInterval;
                _endTime = DateTime.Now + _duration;
                _nextFreezeTime = DateTime.Now + _freezeInterval;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= _endTime)
                {
                    Stop();
                    return;
                }

                if (DateTime.Now >= _nextFreezeTime)
                {
                    // Trigger freeze effect
                    FreezeBarrier();
                    _nextFreezeTime = DateTime.Now + _freezeInterval;
                }
            }

            private void FreezeBarrier()
            {
                // Create icy explosion effect
                Effects.SendLocationEffect(_wraith.Location, _wraith.Map, 0x6B4, 10, 16, 0xB4, 0);

                // Apply freeze damage to players within the barrier radius
                foreach (Mobile mobile in _wraith.GetMobilesInRange((int)_radius))
                {
                    if (mobile is PlayerMobile && mobile != _wraith)
                    {
                        mobile.Damage(Utility.RandomMinMax(10, 20), _wraith);
                        // Optionally, add more visual effects or messages here
                    }
                }
            }
        }

        private void ChillingBeam()
        {
            Say("You are struck by my Chilling Beam!");

            // Define beam parameters
            TimeSpan beamDuration = TimeSpan.FromSeconds(5.0);
            int numberOfBeams = 4; // Number of beams to create

            // Create beams at random locations around the Frost Wraith
            for (int i = 0; i < numberOfBeams; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 1.5), new TimerCallback(CreateChillingBeam));
            }

            // Effect cleanup
            Timer.DelayCall(beamDuration, new TimerCallback(CleanUpChillingBeams));
        }

        private List<Point3D> _beamLocations = new List<Point3D>();

        private void CreateChillingBeam()
        {
            int beamLength = 10; // Length of the beam
            int damage = Utility.RandomMinMax(15, 30); // Damage inflicted by each beam

            Point3D startLocation = new Point3D(Utility.RandomMinMax(this.X - 5, this.X + 5), Utility.RandomMinMax(this.Y - 5, this.Y + 5), this.Z);
            Point3D endLocation = new Point3D(startLocation.X, startLocation.Y, this.Z + beamLength);

            Effects.SendLocationEffect(startLocation, this.Map, 0x23B2, 30, 10); // Chilling beam graphic

            // Apply damage to players in the path of the beam
            foreach (Mobile mobile in this.GetMobilesInRange(beamLength))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.Damage(damage, this);
                }
            }

            _beamLocations.Add(startLocation);
        }

        private void CleanUpChillingBeams()
        {
            foreach (Point3D location in _beamLocations)
            {
                Effects.SendLocationEffect(location, this.Map, 0x23B2, 10, 16); // Clean up graphic
            }

            _beamLocations.Clear();
        }

        private void FrostNova()
        {
            Say("You are caught in my Frost Nova!");

            foreach (Mobile mobile in GetMobilesInRange(5))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.Damage(Utility.RandomMinMax(20, 40), this);
                    mobile.Freeze(TimeSpan.FromSeconds(5.0)); // Freeze effect
                }
            }

            Effects.SendLocationEffect(this.Location, this.Map, 0x23B2, 20, 10); // Frost Nova graphic
        }

        private void IceStorm()
        {
            Say("A storm of ice descends upon you!");

            foreach (Mobile mobile in GetMobilesInRange(8))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.Damage(Utility.RandomMinMax(25, 45), this);
                    mobile.Freeze(TimeSpan.FromSeconds(3.0)); // Short freeze effect
                }
            }

            Effects.SendLocationEffect(this.Location, this.Map, 0x23B2, 30, 10); // Ice Storm graphic
        }

        private void Blizzard()
        {
            Say("Embrace the Blizzard's fury!");

            foreach (Mobile mobile in GetMobilesInRange(10))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.Damage(Utility.RandomMinMax(35, 60), this);
                    mobile.Freeze(TimeSpan.FromSeconds(7.0)); // Longer freeze effect
                }
            }

            Effects.SendLocationEffect(this.Location, this.Map, 0x23B2, 40, 10); // Blizzard graphic
        }

        private void GlacialSpear()
        {
            Say("Feel the pierce of my Glacial Spear!");

            int damage = Utility.RandomMinMax(25, 45);
            Point3D targetLocation = Combatant.Location;

            // Create glacial spear effect
            Effects.SendLocationEffect(targetLocation, Map, 0x23B2, 30, 10); // Glacial spear graphic
            Combatant.Damage(damage, this);

            // Optional: Add some visual effects or messages
        }

        private void GlacialTornado()
        {
            Say("Witness the might of my Glacial Tornado!");

            // Define tornado parameters
            int radius = 5;
            int damage = Utility.RandomMinMax(30, 50);

            foreach (Mobile mobile in GetMobilesInRange(radius))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.Damage(damage, this);
                    mobile.Freeze(TimeSpan.FromSeconds(10.0)); // Longer freeze effect
                }
            }

            Effects.SendLocationEffect(this.Location, this.Map, 0x23B2, 50, 10); // Glacial tornado graphic
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
    }
}
