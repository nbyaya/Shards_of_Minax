using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TokunoMagicMap : MagicMapBase
    {
        // Override predefined locations for Tokuno
        public override List<Point3D> PredefinedLocations => new List<Point3D>
        {
			new Point3D(527, 1225, 25),
			new Point3D(548, 1221, 25),				
			new Point3D(588, 1130, 36),				
			new Point3D(618, 1116, 34),
			new Point3D(753, 1079, 23),				
			new Point3D(745, 1011, 33),
			new Point3D(719, 868, 33),
			new Point3D(581, 927, 24),
			new Point3D(542, 943, 29),				
			new Point3D(548, 883, 21),				
			new Point3D(519, 857, 2),				
			new Point3D(654, 878, 29),				
			new Point3D(750, 871, 32),				
			new Point3D(923, 818, 29),
			new Point3D(998, 788, 4),				
			new Point3D(957, 694, 15),				
			new Point3D(917, 637, 15),				
			new Point3D(897, 600, 38),				
			new Point3D(824, 736, 24),
			new Point3D(876, 698, 4),				
			new Point3D(1135, 900, 62),				
			new Point3D(1119, 1078, 32),				
			new Point3D(1214, 1098, 28),
			new Point3D(1192, 1060, 21),				
			new Point3D(1210, 938, 33),				
			new Point3D(1263, 880, 3),
			new Point3D(1233, 764, 23),				
			new Point3D(1175, 801, 27),
			new Point3D(1232, 657, 73),
			new Point3D(1266, 577, 28),
			new Point3D(1227, 529, 29),
			new Point3D(1198, 489, 23),				
			new Point3D(1104, 551, 23),				
			new Point3D(987, 568, 10),
			new Point3D(1089, 412, 6),				
			new Point3D(910, 418, 14),
			new Point3D(979, 299, 25),
			new Point3D(952, 257, 19),
			new Point3D(847, 316, 38),				
			new Point3D(819, 286, 35),				
			new Point3D(798, 373, 26),				
			new Point3D(754, 185, 38),				
			new Point3D(961, 145, 44),				
			new Point3D(920, 60, 37),
			new Point3D(636, 388, 42),				
			new Point3D(663, 426, 32),				
			new Point3D(592, 361, 23),				
			new Point3D(541, 426, 27),				
			new Point3D(470, 432, 30),
			new Point3D(442, 376, 32),				
			new Point3D(406, 595, 29),				
			new Point3D(353, 545, 15),				
			new Point3D(196, 632, 46),
			new Point3D(190, 534, 41),				
			new Point3D(199, 731, 32),				
			new Point3D(319, 705, 58),
			new Point3D(230, 793, 57),				
			new Point3D(191, 916, 35),
			new Point3D(272, 1142, 11),
			new Point3D(372, 1047, 11)			
        };

        // Override monster types for Tokuno
        public override Type[] MonsterTypes => new Type[]
        {
            typeof(Orc), typeof(OrcCaptain), typeof(OrcishMage),
            typeof(OrcishLord), typeof(HeadlessOne), typeof(Orc)
        };

        // Override treasure chest level
        public override int ChestLevel => 5; // Higher-level chests

        // Override spawn radius and monster count
        public override int SpawnRadius => 20; // Larger spawn area
        public override int MaxMonsters => 15; // More monsters

        // Override expiration time
        public override TimeSpan ExpirationTime => TimeSpan.FromMinutes(45); // Longer duration

        // Override portal hue and sound
        public override int PortalHue => 2446; // Gold hue
        public override int PortalSound => 0x20F; // Different sound effect

        // Override name and hue
        [Constructable]
        public TokunoMagicMap() : base(0x14EB, "Tokuno Adventure Map", 2446)
        {
            // Additional initialization if needed
        }

        // Override SpawnMiscObjects to add custom environmental objects
        protected override void SpawnMiscObjects(Point3D center, Map map, SpawnedContent content)
        {
            // Add traps
            for (int i = 0; i < 3; i++)
            {
                Point3D trapLoc = GetRandomSpawnPoint(center, SpawnRadius);
                WoodenChest trap = new WoodenChest(Utility.RandomMinMax(1, 3)); // Random trap level
                trap.MoveToWorld(trapLoc, map);
                content.SpawnedEntities.Add(trap);
            }

            // Add decorative objects (e.g., trees, rocks)
            for (int i = 0; i < 5; i++)
            {
                Point3D decorLoc = GetRandomSpawnPoint(center, SpawnRadius);
                Item decor = new Static(Utility.RandomList(0x0CAC, 0x0CAD, 0x0CAE)); // Random tree
                decor.MoveToWorld(decorLoc, map);
                content.SpawnedEntities.Add(decor);
            }

            // Add a special boss monster
            Point3D bossLoc = GetRandomSpawnPoint(center, SpawnRadius);
            BaseCreature boss = new OrcishLord(); // Unique boss
            boss.MoveToWorld(bossLoc, map);
            boss.Home = bossLoc;
            boss.RangeHome = SpawnRadius;
            content.SpawnedEntities.Add(boss);
        }

        // Override GetRandomLocation to ensure valid spawn points
        protected override Point3D GetRandomLocation()
        {
            Point3D location = base.GetRandomLocation();

            // Ensure the location is valid on Tokuno
            if (location == Point3D.Zero || Map.Tokuno.CanSpawnMobile(location))
                return location;

            // Fallback to a default location if invalid
            return new Point3D(700, 3703, -43); // Britain
        }

        // Override OnDoubleClick to set the destination map to Tokuno
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
            Map destinationMap = Map.Tokuno; // Set the destination map to Tokuno

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(ExpirationTime);

            // Spawn challenges (monsters, chests, etc.)
            SpawnChallenges(destination, destinationMap, content, from); // Add 'from' parameter

            // Create portals (origin and return)
            CreatePortals(origin, originMap, destination, destinationMap, content);

            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location in Tokuno!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        // Serialization
        public TokunoMagicMap(Serial serial) : base(serial) { }

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