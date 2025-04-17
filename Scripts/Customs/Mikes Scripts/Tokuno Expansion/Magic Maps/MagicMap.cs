using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MagicMap : Item
    {
        private static readonly List<Point3D> PredefinedLocations = new List<Point3D>
        {
            // Add your predefined locations here (example locations)
            new Point3D(2101, 2107, 0),   // Trammel
            new Point3D(2101, 2107, 0),   // Felucca
            new Point3D(2101, 2107, 0),    // Ilshenar
            new Point3D(2101, 2107, 0),  // Tokuno
            new Point3D(2101, 2107, 0)   // Malas

        };

        private const int SpawnRadius = 15;  // Area radius for spawning challenges
        private const int MaxMonsters = 10;  // Max monsters to spawn
        private const int MaxChests = 3;     // Max treasure chests to spawn
        private const int ExpirationMinutes = 30; // Content expiration time
        private const int PortalHue = 1174; // Purple magic hue
        private const int PortalSound = 0x20E; // Magic sound effect

        [Constructable]
        public MagicMap() : base(0x14EB)  // Normal map item ID
        {
            Name = "Mysterious Magic Map";
            Hue = 1174;  // Magical hue
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
            Map destinationMap = Map.Felucca; // Should match your location's facet

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(TimeSpan.FromMinutes(ExpirationMinutes));
            
            // Spawn challenges
            SpawnChallenges(destination, destinationMap, content);
            
            // Create portals
            CreatePortals(origin, originMap, destination, destinationMap, content);
            
            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        private Point3D GetRandomLocation()
        {
            if (PredefinedLocations.Count == 0)
                return Point3D.Zero;

            int index = Utility.Random(PredefinedLocations.Count);
            return PredefinedLocations[index];
        }

        private void CreatePortals(Point3D origin, Map originMap, Point3D destination, Map destinationMap, SpawnedContent content)
        {
            // Origin portal (where player came from)
            MagicPortal originPortal = new MagicPortal
            {
                Destination = destination,
                DestinationMap = destinationMap
            };
            originPortal.MoveToWorld(origin, originMap);
            content.OriginPortal = originPortal;

            // Destination portal (return portal)
            MagicPortal destPortal = new MagicPortal
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

		private void SpawnChallenges(Point3D center, Map map, SpawnedContent content)
		{
			// Spawn monsters
			for (int i = 0; i < MaxMonsters; i++)
			{
				Point3D spawnLoc = GetRandomSpawnPoint(center, SpawnRadius);
				BaseCreature monster = GetRandomMonster();
				
				if (monster != null)
				{
					monster.MoveToWorld(spawnLoc, map);
					monster.Home = spawnLoc;
					monster.RangeHome = SpawnRadius;
					content.SpawnedEntities.Add(monster);
				}
			}

			// Spawn treasure chests
			for (int i = 0; i < MaxChests; i++)
			{
				Point3D chestLoc = GetRandomSpawnPoint(center, SpawnRadius);
				TreasureMapChest chest = new TreasureMapChest(Utility.RandomMinMax(1, 5)); // Corrected constructor
				chest.MoveToWorld(chestLoc, map);
				content.SpawnedEntities.Add(chest);
			}

			// Spawn other objects
			SpawnMiscObjects(center, map, content);
		}

        private Point3D GetRandomSpawnPoint(Point3D center, int radius)
        {
            int x = center.X + Utility.RandomMinMax(-radius, radius);
            int y = center.Y + Utility.RandomMinMax(-radius, radius);
            return new Point3D(x, y, center.Z);
        }

        private BaseCreature GetRandomMonster()
        {
            Type[] monsterTypes = {
                typeof(Lich), typeof(Dragon), typeof(Balron),
                typeof(OrcCaptain), typeof(AncientWyrm), typeof(BloodElemental)
            };

            return Activator.CreateInstance(monsterTypes[Utility.Random(monsterTypes.Length)]) as BaseCreature;
        }

        private void SpawnMiscObjects(Point3D center, Map map, SpawnedContent content)
        {
            // Example: Spawn environmental hazards/decorations
            int trapsToSpawn = Utility.Random(3, 5);
            for (int i = 0; i < trapsToSpawn; i++)
            {
                Point3D trapLoc = GetRandomSpawnPoint(center, SpawnRadius);
                ValoriteOre trap = new ValoriteOre();
                trap.MoveToWorld(trapLoc, map);
                content.SpawnedEntities.Add(trap);
            }

            // Add other objects like resource nodes, etc.
            if (Utility.RandomDouble() < 0.3)
            {
                Point3D oreLoc = GetRandomSpawnPoint(center, SpawnRadius);
                ValoriteOre ore = new ValoriteOre(5);
                ore.MoveToWorld(oreLoc, map);
                content.SpawnedEntities.Add(ore);
            }
        }

        // Serialization
        public MagicMap(Serial serial) : base(serial) { }

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

        public class MagicPortal : Item
        {
            public Point3D Destination { get; set; }
            public Map DestinationMap { get; set; }

            [Constructable]
            public MagicPortal() : base(0x0DDA)
            {
                Name = "Magic Portal";
                Hue = PortalHue;
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
                    Effects.PlaySound(Destination, DestinationMap, PortalSound);
                });
            }

            // Serialization
            public MagicPortal(Serial serial) : base(serial) { }

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
        }
    }
}