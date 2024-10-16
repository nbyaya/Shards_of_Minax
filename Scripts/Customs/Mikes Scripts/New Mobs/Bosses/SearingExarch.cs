using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class SearingExarch : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public SearingExarch() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Searing Exarch";
            Body = 9; // You may want to change this to a more suitable body type
            BaseSoundID = 362;

            SetStr(986, 1185);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(1000);

            SetDamage(22, 29);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 50, 60);
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

        public SearingExarch(Serial serial) : base(serial)
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
                Say("Behold my final form!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("You dare challenge my power?");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("Prepare for my wrath!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    MagmaEruption();
                    break;
                case PHASE_3:
                    RitualCircles();
                    break;
                case FINAL_PHASE:
                    FinalEruption();
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
                    MagmaBarrier();
                    break;
                case 1:
                    Annihilation();
                    break;
                case 2:
                    FireBeams();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                SearingBonds();
            else
                MeteorShower();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                MoltenFirestorm();
            else
                FireBlast();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(5))
            {
                case 0:
                    MagmaBarrier();
                    break;
                case 1:
                    Annihilation();
                    break;
                case 2:
                    SearingBonds();
                    break;
                case 3:
                    MoltenFirestorm();
                    break;
                case 4:
                    VolcanicSearing();
                    break;
            }
        }

        // Implement individual abilities
		private void MagmaBarrier()
		{
			Say("Feel the heat of my Magma Barrier!");

			double barrierRadius = 10.0; // Radius of the barrier
			TimeSpan barrierDuration = TimeSpan.FromSeconds(20.0); // Barrier active for 20 seconds
			TimeSpan explosionInterval = TimeSpan.FromSeconds(3.0); // Explosions every 3 seconds

			// Create the barrier effect
			CreateBarrierEffect(barrierRadius);

			Timer barrierTimer = new BarrierTimer(this, barrierRadius, barrierDuration, explosionInterval);
			barrierTimer.Start();
		}

		private void CreateBarrierEffect(double radius)
		{
			// Create a fiery barrier effect at the Exarch's location
			Effects.SendLocationEffect(this.Location, this.Map, 0x3709, 10, 16, 0x4E, 0); // Adjust as needed
			Effects.SendLocationEffect(this.Location, this.Map, 0x3709, 10, 16, 0x4E, 0); // Add multiple effects for better visuals

			// Create a red circle around the barrier
			for (double angle = 0; angle < 360; angle += 15)
			{
				double radians = angle * Math.PI / 180.0;
				int x = (int)(radius * Math.Cos(radians));
				int y = (int)(radius * Math.Sin(radians));

				Effects.SendLocationEffect(new Point3D(this.X + x, this.Y + y, this.Z), this.Map, 0x3709, 10, 16, 0x4E, 0);
			}
		}

		private class BarrierTimer : Timer
		{
			private SearingExarch _exarch;
			private double _radius;
			private TimeSpan _duration;
			private TimeSpan _explosionInterval;
			private DateTime _endTime;
			private DateTime _nextExplosionTime;

			public BarrierTimer(SearingExarch exarch, double radius, TimeSpan duration, TimeSpan explosionInterval)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
			{
				_exarch = exarch;
				_radius = radius;
				_duration = duration;
				_explosionInterval = explosionInterval;
				_endTime = DateTime.Now + _duration;
				_nextExplosionTime = DateTime.Now + _explosionInterval;
			}

			protected override void OnTick()
			{
				if (DateTime.Now >= _endTime)
				{
					Stop();
					return;
				}

				if (DateTime.Now >= _nextExplosionTime)
				{
					// Trigger explosion effect
					ExplodeBarrier();
					_nextExplosionTime = DateTime.Now + _explosionInterval;
				}
			}

			private void ExplodeBarrier()
			{
				// Create fiery explosion effect
				Effects.SendLocationEffect(_exarch.Location, _exarch.Map, 0x3709, 10, 16, 0x4E, 0);

				// Apply damage to players within the barrier radius
				foreach (Mobile mobile in _exarch.GetMobilesInRange((int)_radius))
				{
					if (mobile is PlayerMobile && mobile != _exarch)
					{
						mobile.Damage(Utility.RandomMinMax(20, 30), _exarch);
						// Optionally, add more visual effects or messages here
					}
				}
			}
		}




		private void Annihilation()
		{
			Say("Annihilation!");

			// Define the number of projectiles and their spread
			int numProjectiles = 12;
			double angleStep = 360.0 / numProjectiles;
			double radius = 15.0; // Radius to which projectiles will spread
			int damage = 50; // Damage per projectile

			for (int i = 0; i < numProjectiles; i++)
			{
				double angle = i * angleStep;
				double radians = angle * (Math.PI / 180.0);
				double x = radius * Math.Cos(radians);
				double y = radius * Math.Sin(radians);
				Point3D targetLocation = new Point3D(Location.X + (int)x, Location.Y + (int)y, Location.Z);

				// Create and send fire projectile
				FireProjectile(targetLocation, damage);
			}

			// Create a flashy visual effect
			CreateFireVisualEffect(Location);
		}

		private void FireProjectile(Point3D targetLocation, int damage)
		{
			// Assuming BaseProjectile is correctly defined to handle this logic.
			BaseProjectile projectile = new BaseProjectile
			{
				ItemID = 0x36D4, // Fireball graphic
				Name = "Fire Projectile"
			};

			// Set the projectile's starting location and target
			projectile.Location = Location;
			projectile.Direction = Utility.GetDirection(Location, targetLocation);
			projectile.Range = 15; // Range of the projectile
			projectile.Damage = damage;

			// Send the projectile to the target
			ProjectileInfo info = new ProjectileInfo(projectile, targetLocation, damage);
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerCallback(() => FireProjectileEffect(info)));
		}


		private void CreateFireVisualEffect(Point3D location)
		{
			int flameHue = 2543; // Set this to the hue value you want for the flamestrike effect.
			Effects.SendLocationEffect(location, Map, 0x36BD, 16, 10, flameHue, 0); // Fireball explosion effect with hue
			Effects.SendLocationEffect(location, Map, 0x3709, 30); // Flashing effect
		}


		private class BaseProjectile : Item
		{
			public int Damage { get; set; }
			public int Range { get; set; }
			public Direction Direction { get; set; }

			// Serialization constructor
			public BaseProjectile(Serial serial) : base(serial)
			{
			}

			[Constructable]
			public BaseProjectile() : base(0x36D4)
			{
				Movable = false;
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // Version

				writer.Write(Damage);
				writer.Write(Range);
				writer.Write((int)Direction);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				Damage = reader.ReadInt();
				Range = reader.ReadInt();
				Direction = (Direction)reader.ReadInt();
			}

			public override void OnMovement(Mobile m, Point3D oldLocation)
			{
				// Deal damage if hitting a player
				if (m is PlayerMobile)
				{
					m.Damage(Damage);
				}
			}
		}


		private class ProjectileInfo
		{
			private BaseProjectile _projectile;
			private Point3D _targetLocation;
			private int _damage;

			public BaseProjectile Projectile
			{
				get { return _projectile; }
				set { _projectile = value; }
			}

			public Point3D TargetLocation
			{
				get { return _targetLocation; }
				set { _targetLocation = value; }
			}

			public int Damage
			{
				get { return _damage; }
				set { _damage = value; }
			}

			public ProjectileInfo(BaseProjectile projectile, Point3D targetLocation, int damage)
			{
				_projectile = projectile;
				_targetLocation = targetLocation;
				_damage = damage;
			}
		}


		private void FireProjectileEffect(ProjectileInfo info)
		{
			// Send the projectile effect to the target location
			Effects.SendLocationEffect(info.Projectile.Location, Map, 0x36D4, 30); // Adjust effect ID and parameters as needed
			// Apply damage to the target
			if (info.Projectile.Location == info.TargetLocation)
			{
				foreach (Mobile mob in GetMobilesInRange(1))
				{
					if (mob is PlayerMobile)
					{
						mob.Damage(info.Damage);
					}
				}
			}
		}



		private void FireBeams()
		{
			Say("Burn in my Fire Beams!");

			// Define beam parameters
			TimeSpan beamDuration = TimeSpan.FromSeconds(5.0);
			int numberOfBeams = 4; // Number of beams to create

			// Create beams at random locations around the Searing Exarch
			for (int i = 0; i < numberOfBeams; i++)
			{
				Timer.DelayCall(TimeSpan.FromSeconds(i * 1.5), new TimerCallback(CreateFireBeam));
			}

			// Effect cleanup
			Timer.DelayCall(beamDuration, new TimerCallback(CleanUpFireBeams));
		}

		private List<Point3D> beamLocations = new List<Point3D>();

		private void CreateFireBeam()
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
			Effects.SendLocationEffect(endLocation, Map, 0x3709, 10, 0); // Fire beam effect with a duration of 10

			// Add the location for cleanup later
			beamLocations.Add(endLocation);

			// Deal damage to players in the path
			foreach (Mobile m in GetMobilesInRange(radius))
			{
				if (m.Alive && m.Map == Map)
				{
					if (m.InRange(endLocation, 2))
					{
						m.SendMessage("You are hit by a fire beam!");
						m.Damage(30, this);
					}
				}
			}
		}

		private void CleanUpFireBeams()
		{
			foreach (Point3D loc in beamLocations)
			{
				Effects.SendLocationEffect(loc, Map, 0x3709, 10, 0); // Remove the fire beam effect (duration of 10)
			}
			beamLocations.Clear();
		}





		private void MagmaEruption()
		{
			Say("The ground trembles beneath you!");

			// Define the number of eruptions and their radius
			int eruptionCount = 5; // Number of eruptions
			int radius = 3; // Radius of the eruption area

			// Get the location of the boss
			Point3D bossLocation = this.Location;

			for (int i = 0; i < eruptionCount; i++)
			{
				// Generate a random position around the boss for the eruption
				int offsetX = Utility.RandomMinMax(-10, 10);
				int offsetY = Utility.RandomMinMax(-10, 10);
				Point3D eruptionLocation = new Point3D(bossLocation.X + offsetX, bossLocation.Y + offsetY, bossLocation.Z);

				// Create the eruption effect
				CreateEruptionEffect(eruptionLocation, radius);

				// Create the damage zones
				Timer.DelayCall(TimeSpan.FromSeconds(2), delegate
				{
					CreateDamageZone(eruptionLocation, radius);
				});
			}
		}

		private void CreateEruptionEffect(Point3D location, int radius)
		{
			// Create a fiery explosion effect at the location
			Effects.SendLocationEffect(location, Map, 0x36D4, 30, 10, 0x992, 0); // Explosion effect
			Effects.PlaySound(location, Map, 0x227); // Fire sound effect

			// Optional: Add some visual particles
			for (int i = 0; i < 10; i++)
			{
				int offsetX = Utility.RandomMinMax(-radius, radius);
				int offsetY = Utility.RandomMinMax(-radius, radius);
				Point3D particleLocation = new Point3D(location.X + offsetX, location.Y + offsetY, location.Z);
				Effects.SendLocationEffect(particleLocation, Map, 0x3709, 30, 10); // Fire particles
			}
		}

		private void CreateDamageZone(Point3D location, int radius)
		{
			// Damage zone effect
			Timer.DelayCall(TimeSpan.FromSeconds(1), delegate
			{
				foreach (Mobile m in this.GetMobilesInRange(radius))
				{
					if (m is PlayerMobile)
					{
						if (m.InRange(location, radius))
						{
							AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 0); // Fire damage
							m.SendMessage("You are burned by the magma!");
						}
					}
				}
			});
		}


		private void SearingBonds()
		{
			// Notify players about the attack
			Say("You cannot escape my Searing Bonds!");

			// Define the range and damage
			double damage = Utility.RandomMinMax(50, 100);
			double range = 10.0; // Range of the effect in tiles

			// Get the list of all mobile objects within the specified range
			List<Mobile> targets = new List<Mobile>();
			foreach (Mobile m in this.GetMobilesInRange((int)range))
			{
				if (m != this && m is PlayerMobile)
				{
					targets.Add(m);
				}
			}

			// Apply the effect to each target
			foreach (Mobile target in targets)
			{
				// Create a beam effect from the Exarch to the target
				int effectHue = 1153; // Bright red for visual effect
				int effectSpeed = 10; // Speed of the beam

				Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, effectHue, 0);

				// Apply damage if the target is within the beams
				if (target.InRange(this, (int)range))
				{
					target.SendMessage("You are hit by the searing bonds!");
					AOS.Damage(target, this, (int)damage, 0, 0, 0, 0, 0);
				}
			}

			// Optionally, create some visual effects around the Searing Exarch to enhance the effect
			Effects.SendLocationEffect(this.Location, this.Map, 0x3709, 30, 10, 1153, 0); // Burst of flames at the Exarch's location
		}



		private void MeteorShower()
		{
			Say("Meteors rain from the sky!");
			
			// Number of meteors to spawn
			int numberOfMeteors = 10;
			
			for (int i = 0; i < numberOfMeteors; i++)
			{
				// Create a delay between each meteor spawn for better effect
				Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), new TimerCallback(delegate
				{
					SpawnMeteor();
				}));
			}
		}

		private void SpawnMeteor()
		{
			// Determine a random location in the arena
			int x = this.X + Utility.RandomMinMax(-10, 10);
			int y = this.Y + Utility.RandomMinMax(-10, 10);
			int z = this.Z + 20; // Start above the arena
			
			// Create a meteor item (a fireball or similar item)
			Item meteor = new Boulder(); // You might need to replace this with an actual meteor item or effect
			meteor.MoveToWorld(new Point3D(x, y, z), this.Map);
			
			// Drop the meteor to the ground with a visual effect
			Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerCallback(delegate
			{
				meteor.Delete();
				CreateMeteorImpact(x, y);
			}));
		}

		private void CreateMeteorImpact(int x, int y)
		{
			// Create a fire effect at the impact location
			for (int i = 0; i < 3; i++)
			{
				Effects.SendLocationEffect(new Point3D(x, y, this.Z), this.Map, 0x3709, 30);
			}
			
			// Apply damage to players in the impact area
			IPooledEnumerable eable = this.Map.GetObjectsInRange(new Point3D(x, y, this.Z), 2); // 2-tile radius
			foreach (object o in eable)
			{
				if (o is Mobile)
				{
					Mobile m = (Mobile)o;
					if (m is PlayerMobile && m != this)
					{
						m.Damage(Utility.RandomMinMax(20, 30), this);
					}
				}
			}
			eable.Free();
		}



		private void RitualCircles()
		{
			Say("Step into my Ritual Circles, if you dare!");

			// Create multiple ritual circles
			for (int i = 0; i < 3; i++)
			{
				int xOffset = Utility.RandomMinMax(-5, 5);
				int yOffset = Utility.RandomMinMax(-5, 5);
				Point3D circleLocation = new Point3D(Location.X + xOffset, Location.Y + yOffset, Location.Z);

				// Ensure the circle is placed on the ground
				Map map = Map;
				if (map != null && map.CanFit(circleLocation, 16, false, false))
				{
					RitualCircle circle = new RitualCircle();
					circle.MoveToWorld(circleLocation, map);
				}
			}
		}


		private void MoltenFirestorm()
		{
			Say("Behold the fury of my Molten Firestorm!");

			// Define the number of firestorms and their effect radius
			int numFirestorms = 5;
			double radius = 10.0; // Radius of the firestorm effect

			for (int i = 0; i < numFirestorms; i++)
			{
				// Random position around the boss
				int x = Utility.RandomMinMax(-10, 10);
				int y = Utility.RandomMinMax(-10, 10);
				int z = 0; // Height of the firestorm effect

				// Create a new firestorm effect
				Point3D firestormLocation = new Point3D(this.X + x, this.Y + y, this.Z + z);
				CreateFirestormEffect(firestormLocation, radius);

				// Apply damage to players within the radius
				foreach (Mobile m in this.GetMobilesInRange((int)radius))
				{
					if (m is PlayerMobile && m.InLOS(firestormLocation))
					{
						if (m.GetDistanceToSqrt(firestormLocation) <= radius)
						{
							AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0); // Adjust damage as needed
						}
					}
				}
			}
		}

		private void CreateFirestormEffect(Point3D location, double radius)
		{
			for (int i = 0; i < 10; i++) // Number of particles
			{
				double angle = Utility.RandomDouble() * 2 * Math.PI;
				int x = (int)(radius * Math.Cos(angle));
				int y = (int)(radius * Math.Sin(angle));

				Effects.SendLocationEffect(new Point3D(location.X + x, location.Y + y, location.Z), Map, 0x3709, 30, 10); // Fire effect
				Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomDouble() * 2), new TimerCallback(delegate
				{
					Effects.SendLocationEffect(new Point3D(location.X + x, location.Y + y, location.Z), Map, 0x3709, 30, 10); // More fire effects
				}));
			}
		}





		private void FireBlast()
		{
			Say("Fire Blast!");

			if (Combatant == null)
				return;

			// Target the area around the combatant
			Point3D targetLocation = Combatant.Location;
			Map targetMap = Combatant.Map;

			// Play a sound effect
			Effects.PlaySound(targetLocation, targetMap, 0x208); // 0x208 is a fire explosion sound

			// Create a visual effect (fire explosion)
			Effects.SendLocationEffect(targetLocation, targetMap, 0x3709, 30, 0x16); // Fire explosion effect

			// Schedule damage to the target location
			Timer.DelayCall(TimeSpan.FromSeconds(1), delegate 
			{
				// Create the explosion effect
				Effects.SendLocationEffect(targetLocation, targetMap, 0x3709, 30, 0x16);
				
				// Apply damage in the area
				foreach (Mobile m in targetMap.GetMobilesInRange(targetLocation, 2))
				{
					if (m != Combatant && m.Alive)
					{
						int damage = Utility.RandomMinMax(50, 100); // Adjust damage values as needed
						m.Damage(damage, this);
					}
				}
			});
		}


		private void FinalEruption()
		{
			Say("Witness my true power!");

			// Get the current location of the Searing Exarch
			Point3D center = this.Location;

			// Create a fire effect at the center location
			FireExplosion(center);

			// Create additional fire effects around the center
			for (int i = 0; i < 3; i++)
			{
				int offsetX = Utility.RandomMinMax(-5, 5);
				int offsetY = Utility.RandomMinMax(-5, 5);
				Point3D effectLocation = new Point3D(center.X + offsetX, center.Y + offsetY, center.Z);
				FireExplosion(effectLocation);
			}

			// Schedule the next attack to ensure continuous effect
			_nextAbilityTime = DateTime.Now.AddSeconds(Utility.RandomMinMax(10, 15));
		}

		private void FireExplosion(Point3D location)
		{
			// Create a fire explosion effect at the specified location
			Effects.SendLocationEffect(location, Map, 0x3709, 30, 10, 0, 0);
			
			// Deal damage to all players in the area
			foreach (Mobile m in this.GetMobilesInRange(5))
			{
				if (m is PlayerMobile)
				{
					int damage = Utility.RandomMinMax(50, 100);
					m.SendMessage("You are hit by a fiery explosion!");
					AOS.Damage(m, this, damage, 0, 0, 0, 0, 0); // Fire damage
				}
			}
		}


		private void VolcanicSearing()
		{
			Say("The very ground burns with my rage!");

			// Number of volcanic zones to spawn
			int numberOfZones = 5;
			for (int i = 0; i < numberOfZones; i++)
			{
				// Random position within the arena
				int xOffset = Utility.RandomMinMax(-10, 10);
				int yOffset = Utility.RandomMinMax(-10, 10);
				Point3D zoneLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

				// Create the volcanic zone
				CreateVolcanicZone(zoneLocation);
			}

			// Optional: Play a sound effect for dramatic effect
			Effects.PlaySound(this.Location, this.Map, 0x20F); // Use an appropriate sound ID
		}

		private void CreateVolcanicZone(Point3D location)
		{
			// Duration of the volcanic zone effect
			TimeSpan duration = TimeSpan.FromSeconds(10);

			// Visual effect - creating a fire explosion at the location
			Effects.SendLocationEffect(location, Map, 0x36D4, 30, 10, 0x20, 0);

			// Apply damage over time to players within the zone
			Timer.DelayCall(duration, new TimerCallback(() =>
			{
				foreach (Mobile mobile in this.Map.GetMobilesInRange(location, 2))
				{
					if (mobile is PlayerMobile && mobile.Alive && !mobile.Hidden)
					{
						mobile.Damage(20, this); // Damage amount
						mobile.SendLocalizedMessage(1060132); // "You are burned by the volcanic eruption!"
					}
				}
			}));
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