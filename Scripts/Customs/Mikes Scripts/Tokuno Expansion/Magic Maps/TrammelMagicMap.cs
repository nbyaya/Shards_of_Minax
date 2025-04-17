using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TrammelMagicMap : MagicMapBase
    {
        // Override predefined locations for Trammel
        public override List<Point3D> PredefinedLocations => new List<Point3D>
        {
			new Point3D(3340, 572, 0),
			new Point3D(3340, 572, 0),
			new Point3D(3398, 335, 0),
			new Point3D(3239, 367, 4),				
			new Point3D(3008, 268, 5),				
			new Point3D(2866, 197, 0),				
			new Point3D(2874, 455, 10),				
			new Point3D(2886, 91, 0),				
			new Point3D(2665, 120, 0),				
			new Point3D(2608, 128, 0),				
			new Point3D(2431, 94, 0),				
			new Point3D(2416, 176, 0),				
			new Point3D(2462, 54, 0),				
			new Point3D(2327, 67, 0),				
			new Point3D(2177, 305, 0),
			new Point3D(2079, 156, 0),
			new Point3D(1788, 253, 6),				
			new Point3D(1976, 267, 8),				
			new Point3D(1934, 318, 0),				
			new Point3D(1901, 366, 0),				
			new Point3D(1987, 389, 0),				
			new Point3D(1862, 604, 0),				
			new Point3D(2071, 640, 0),				
			new Point3D(1549, 729, 27),				
			new Point3D(1641, 972, 0),				
			new Point3D(1511, 1086, 0),				
			new Point3D(1426, 992, 0),					
			new Point3D(1369, 835, 0),
			new Point3D(1258, 611, 24),
			new Point3D(1275, 529, 30),				
			new Point3D(1482, 238, 23),				
			new Point3D(1190, 142, 5),				
			new Point3D(1051, 365, 3),				
			new Point3D(824, 375, 0),				
			new Point3D(957, 706, 0),				
			new Point3D(954, 707, 20),				
			new Point3D(963, 717, -20),				
			new Point3D(1004, 766, -60),				
			new Point3D(1029, 1014, 0),				
			new Point3D(1030, 1360, 0),					
			new Point3D(744, 1359, 0),
			new Point3D(518, 1420, 0),
			new Point3D(355, 1344, 0),				
			new Point3D(158, 1363, 0),				
			new Point3D(142, 1490, 0),				
			new Point3D(362, 1441, 1),				
			new Point3D(359, 1467, 1),				
			new Point3D(386, 1654, 0),				
			new Point3D(659, 1689, 0),				
			new Point3D(772, 1682, 0),				
			new Point3D(833, 1831, 0),				
			new Point3D(851, 2070, 0),				
			new Point3D(1000, 2239, 0),					
			new Point3D(1074, 2204, 0),
			new Point3D(1076, 2262, 0),
			new Point3D(1144, 2224, 27),				
			new Point3D(1216, 2201, 5),				
			new Point3D(1219, 2268, 0),				
			new Point3D(1102, 2482, 0),				
			new Point3D(1060, 2718, 0),				
			new Point3D(1115, 2654, 0),				
			new Point3D(1124, 2824, 0),				
			new Point3D(1166, 2957, 0),				
			new Point3D(1369, 2712, 0),				
			new Point3D(1362, 2891, 0),				
			new Point3D(1359, 3118, 0),					
			new Point3D(1456, 3134, 0),
			new Point3D(1588, 3202, 0),
			new Point3D(1608, 3372, 0),				
			new Point3D(1651, 3548, 0),				
			new Point3D(1726, 3488, 2),				
			new Point3D(1927, 3419, 0),				
			new Point3D(2019, 3382, 0),				
			new Point3D(2159, 3355, 10),				
			new Point3D(2160, 3053, 0),				
			new Point3D(2107, 3000, 30),				
			new Point3D(1881, 3256, 0),				
			new Point3D(1846, 3204, 0),				
			new Point3D(1736, 3091, 0),					
			new Point3D(1675, 2972, 0),
			new Point3D(1966, 2937, 0),
			new Point3D(2119, 2734, 20),				
			new Point3D(1705, 2692, 5),				
			new Point3D(1668, 2486, 10),				
			new Point3D(1822, 2414, 0),				
			new Point3D(2023, 2316, 0),				
			new Point3D(2104, 2103, 0),				
			new Point3D(1788, 2170, 0),				
			new Point3D(1642, 2368, 0),				
			new Point3D(1614, 2116, 0),				
			new Point3D(1473, 2159, 6),				
			new Point3D(1386, 2009, 0),					
			new Point3D(1350, 1499, 20),
			new Point3D(1378, 1319, 0),
			new Point3D(1445, 1227, 0),				
			new Point3D(1761, 1176, 0),				
			new Point3D(1722, 1065, 0),				
			new Point3D(1731, 916, 0),				
			new Point3D(1708, 1428, 0),				
			new Point3D(1981, 1507, 0),				
			new Point3D(1992, 1010, 0),				
			new Point3D(1809, 1031, 0),				
			new Point3D(1828, 1030, 0),				
			new Point3D(2037, 970, 0),				
			new Point3D(1951, 888, -1),					
			new Point3D(1873, 914, 3),
			new Point3D(1803, 854, -1),				
			new Point3D(1867, 741, 6),				
			new Point3D(1576, 828, 0),				
			new Point3D(1254, 869, 16),				
			new Point3D(1229, 936, 10),				
			new Point3D(2056, 624, 0),				
			new Point3D(2241, 843, 0),				
			new Point3D(2325, 820, 0),				
			new Point3D(2359, 806, 0),				
			new Point3D(2181, 1302, 0),				
			new Point3D(2187, 1416, -2),					
			new Point3D(2582, 1120, 0),
			new Point3D(2739, 1061, 0),				
			new Point3D(2745, 858, 0),				
			new Point3D(2636, 720, 0),				
			new Point3D(2663, 469, 15),				
			new Point3D(3920, 196, -10),				
			new Point3D(3959, 358, 0),				
			new Point3D(4051, 562, 0),				
			new Point3D(4050, 439, 3),				
			new Point3D(4229, 508, 0),				
			new Point3D(2504, 3609, 3),				
			new Point3D(2376, 3411, 3),					
			new Point3D(2462, 3932, 0),
			new Point3D(2151, 3930, 0),				
			new Point3D(1144, 3510, 0),				
			new Point3D(1062, 3131, 0)	
        };

        // Override monster types for Trammel
        public override Type[] MonsterTypes => new Type[]
        {
            typeof(Orc), typeof(OrcCaptain), typeof(OrcishMage),
            typeof(OrcishLord), typeof(HeadlessOne), typeof(Orc)
        };

        // Override treasure chest level
        public override int ChestLevel => 1; // Higher-level chests

        // Override spawn radius and monster count
        public override int SpawnRadius => 20; // Larger spawn area
        public override int MaxMonsters => 15; // More monsters

        // Override expiration time
        public override TimeSpan ExpirationTime => TimeSpan.FromMinutes(45); // Longer duration

        // Override portal hue and sound
        public override int PortalHue => 673; // Gold hue
        public override int PortalSound => 0x20F; // Different sound effect

        // Override name and hue
        [Constructable]
        public TrammelMagicMap() : base(0x14EB, "Trammel Adventure Map", 673)
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

            // Ensure the location is valid on Trammel
            if (location == Point3D.Zero || Map.Trammel.CanSpawnMobile(location))
                return location;

            // Fallback to a default location if invalid
            return new Point3D(1343, 2004, 0); // Britain
        }

        // Override OnDoubleClick to set the destination map to Trammel
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
            Map destinationMap = Map.Trammel; // Set the destination map to Trammel

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(ExpirationTime);

            // Spawn challenges (monsters, chests, etc.)
			SpawnChallenges(destination, destinationMap, content, from); // Add 'from' parameter

            // Create portals (origin and return)
            CreatePortals(origin, originMap, destination, destinationMap, content);

            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location in Trammel!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        // Serialization
        public TrammelMagicMap(Serial serial) : base(serial) { }

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