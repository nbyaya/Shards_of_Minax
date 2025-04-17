using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public abstract class MagicMapBase : Item
    {
        // Virtual properties for customization
        public virtual List<Point3D> PredefinedLocations => new List<Point3D>();
        public virtual int SpawnRadius => 15; // Area radius for spawning challenges
        public virtual int MaxMonsters => 10; // Max monsters to spawn
        public virtual int MaxChests => 3;    // Max treasure chests to spawn
        public virtual TimeSpan ExpirationTime => TimeSpan.FromMinutes(30); // Content expiration time
        public virtual int PortalHue => 1174; // Purple magic hue
        public virtual int PortalSound => 0x20E; // Magic sound effect
        public virtual Type[] MonsterTypes => new Type[] { typeof(Lich), typeof(Dragon), typeof(Balron) }; // Default monsters
        public virtual int ChestLevel => 1; // Default treasure chest level

        [Constructable]
        public MagicMapBase(int itemID, string name, int hue) : base(itemID)
        {
            Name = name;
            Hue = hue;
            LootType = LootType.Regular;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            Point3D origin = from.Location;
            Map originMap = from.Map;
            Point3D destination = GetRandomLocation();
            Map destinationMap = Map.Felucca; // Default map (can be overridden)

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(ExpirationTime);

            // Spawn challenges (monsters, chests, etc.)
            SpawnChallenges(destination, destinationMap, content, from); // Add 'from' parameter

            // Create portals (origin and return)
            CreatePortals(origin, originMap, destination, destinationMap, content);

            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        protected virtual Point3D GetRandomLocation()
        {
            if (PredefinedLocations.Count == 0)
                return Point3D.Zero;

            int index = Utility.Random(PredefinedLocations.Count);
            return PredefinedLocations[index];
        }

		protected virtual void SpawnChallenges(Point3D center, Map map, SpawnedContent content, Mobile from)
		{
			// Spawn monsters
			for (int i = 0; i < MaxMonsters; i++)
			{
				SpawnMonster(center, map, content);
			}

			// Spawn treasure chests
			for (int i = 0; i < MaxChests; i++)
			{
				SpawnChest(center, map, content, from); // Pass 'from' here
			}

			// Spawn other objects
			SpawnMiscObjects(center, map, content);
		}

        protected virtual void SpawnMonster(Point3D center, Map map, SpawnedContent content)
        {
            if (MonsterTypes.Length == 0)
                return;

            Type monsterType = MonsterTypes[Utility.Random(MonsterTypes.Length)];
            BaseCreature monster = Activator.CreateInstance(monsterType) as BaseCreature;

            if (monster != null)
            {
                Point3D spawnLoc = GetRandomSpawnPoint(center, SpawnRadius);
                monster.MoveToWorld(spawnLoc, map);
                monster.Home = spawnLoc;
                monster.RangeHome = SpawnRadius;
                content.SpawnedEntities.Add(monster);
            }
        }

		protected virtual void SpawnChest(Point3D center, Map map, SpawnedContent content, Mobile from)
		{
			// Create the chest with the player as the owner
			TreasureMapChest chest = new TreasureMapChest(from, ChestLevel, false);
			Point3D chestLoc = GetRandomSpawnPoint(center, SpawnRadius);
			chest.MoveToWorld(chestLoc, map);

			// Fill the chest with loot
			TreasureMapChest.Fill(from, chest, ChestLevel, false); // 'false' indicates it's not a SOS chest

			content.SpawnedEntities.Add(chest);
		}

        protected virtual void SpawnMiscObjects(Point3D center, Map map, SpawnedContent content)
        {
            // Default implementation (can be overridden in subclasses)
            // Example: Spawn environmental hazards or decorations
            if (Utility.RandomDouble() < 0.3)
            {
                Point3D oreLoc = GetRandomSpawnPoint(center, SpawnRadius);
                ValoriteOre ore = new ValoriteOre(5);
                ore.MoveToWorld(oreLoc, map);
                content.SpawnedEntities.Add(ore);
            }
        }

        protected virtual Point3D GetRandomSpawnPoint(Point3D center, int radius)
        {
            int x = center.X + Utility.RandomMinMax(-radius, radius);
            int y = center.Y + Utility.RandomMinMax(-radius, radius);
            return new Point3D(x, y, center.Z);
        }

		protected virtual void CreatePortals(Point3D origin, Map originMap, Point3D destination, Map destinationMap, SpawnedContent content)
		{
			// Origin portal (where player came from)
			MagicPortal originPortal = new MagicPortal(PortalHue, PortalSound) // Pass values here
			{
				Destination = destination,
				DestinationMap = destinationMap
			};
			originPortal.MoveToWorld(origin, originMap);
			content.OriginPortal = originPortal;

			// Destination portal (return portal)
			MagicPortal destPortal = new MagicPortal(PortalHue, PortalSound) // Pass values here
			{
				Destination = origin,
				DestinationMap = originMap
			};
			destPortal.MoveToWorld(destination, destinationMap);
			content.ReturnPortal = destPortal;

			// Add visual effects
			Effects.SendLocationParticles(originPortal, 0x3728, 10, 10, 2023);
			Effects.SendLocationParticles(destPortal, 0x3728, 10, 10, 2023);
			Effects.PlaySound(originPortal.Location, originMap, PortalSound);
			Effects.PlaySound(destPortal.Location, destinationMap, PortalSound);
		}

        // Serialization
        public MagicMapBase(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        // Supporting class for managing spawned content
        public class SpawnedContent
        {
            public List<ISpawnable> SpawnedEntities { get; } = new List<ISpawnable>();
            public Timer ExpirationTimer { get; }
            public MagicPortal ReturnPortal { get; set; }
            public MagicPortal OriginPortal { get; set; }

            public SpawnedContent(TimeSpan duration)
            {
                // Send warning messages 5 minutes before expiration
                Timer.DelayCall(duration - TimeSpan.FromMinutes(5), () =>
                {
                    foreach (var entity in SpawnedEntities.OfType<BaseCreature>())
                    {
                        entity.Say("The magical energies are weakening!");
                    }
                });

                ExpirationTimer = Timer.DelayCall(duration, DeleteAllContent);
            }

            public void DeleteAllContent()
            {
                foreach (var entity in SpawnedEntities)
                {
                    entity.Delete();
                }
                ReturnPortal?.Delete();
                OriginPortal?.Delete();
                ExpirationTimer.Stop();
            }
        }

        // Supporting class for the magic portal
		public class MagicPortal : Item
		{
			public Point3D Destination { get; set; }
			public Map DestinationMap { get; set; }
			private int _portalHue; // Store the hue
			private int _portalSound; // Store the sound

			[Constructable]
			public MagicPortal(int portalHue, int portalSound) : base(0x0DDA)
			{
				_portalHue = portalHue;
				_portalSound = portalSound;
				Name = "Magic Portal";
				Hue = _portalHue; // Use the passed hue
				Movable = false;
				Light = LightType.Circle300;
			}

			public override void OnDoubleClick(Mobile from)
			{
				if (!from.InRange(this, 3))
				{
					from.SendLocalizedMessage(500446); // That is too far away.
					return;
				}

				from.SendMessage("You step through the magical portal...");
				Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
				Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
				{
					from.MoveToWorld(Destination, DestinationMap);
					Effects.SendLocationParticles(EffectItem.Create(Destination, DestinationMap, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);
					Effects.PlaySound(Destination, DestinationMap, _portalSound); // Use the passed sound
				});
			}

			// Serialization
			public MagicPortal(Serial serial) : base(serial) { }

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);
				writer.Write(0); // version
				writer.Write(_portalHue); // Save the hue
				writer.Write(_portalSound); // Save the sound
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				int version = reader.ReadInt();
				_portalHue = reader.ReadInt(); // Load the hue
				_portalSound = reader.ReadInt(); // Load the sound
			}
		}
    }
}