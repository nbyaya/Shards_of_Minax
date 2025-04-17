using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class IlshenarMagicMap : MagicMapBase
    {
        // Override predefined locations for Ilshenar
        public override List<Point3D> PredefinedLocations => new List<Point3D>
        {
			new Point3D(1106, 418, -80),
			new Point3D(284, 341, -59),				
			new Point3D(489, 350, -59),				
			new Point3D(642, 310, -43),				
			new Point3D(772, 348, -43),				
			new Point3D(630, 493, -75),				
			new Point3D(512, 545, -60),
			new Point3D(290, 538, -22),				
			new Point3D(654, 749, -26),				
			new Point3D(466, 859, -76),				
			new Point3D(429, 950, -84),				
			new Point3D(328, 1100, -57),
			new Point3D(365, 1141, -57),				
			new Point3D(450, 1068, -85),				
			new Point3D(650, 934, -62),				
			new Point3D(640, 1035, -83),
			new Point3D(648, 1143, -72),				
			new Point3D(764, 1039, -30),				
			new Point3D(778, 1138, -30),
			new Point3D(830, 1186, -67),				
			new Point3D(758, 1311, -94),
			new Point3D(644, 1321, -57),
			new Point3D(557, 1377, 17),
			new Point3D(533, 1347, -53),				
			new Point3D(509, 1317, -53),				
			new Point3D(459, 1300, -55),				
			new Point3D(411, 1364, -18),				
			new Point3D(354, 1359, -25),				
			new Point3D(292, 1347, -24),
			new Point3D(299, 1306, -25),				
			new Point3D(385, 1272, -38),				
			new Point3D(331, 1205, -38),				
			new Point3D(356, 1141, -54),				
			new Point3D(910, 1191, 12),
			new Point3D(932, 1131, 12),				
			new Point3D(947, 1062, -13),				
			new Point3D(939, 950, -30),				
			new Point3D(1035, 875, -28),
			new Point3D(963, 783, -80),				
			new Point3D(891, 778, -80),				
			new Point3D(849, 777, 0),
			new Point3D(814, 778, -60),				
			new Point3D(964, 650, -80),
			new Point3D(977, 582, -80),
			new Point3D(972, 525, -80),
			new Point3D(912, 518, -80),				
			new Point3D(876, 526, -80),				
			new Point3D(994, 493, -79),				
			new Point3D(1044, 451, -80),				
			new Point3D(1107, 403, -80),				
			new Point3D(1087, 496, -73),
			new Point3D(1059, 530, -80),				
			new Point3D(1061, 560, -80),				
			new Point3D(1054, 624, -80),				
			new Point3D(1112, 637, -80),				
			new Point3D(1082, 714, -80),
			new Point3D(1125, 739, -80),				
			new Point3D(1181, 744, -80),				
			new Point3D(1351, 661, 108),				
			new Point3D(1372, 626, 105),
			new Point3D(1438, 643, -4),				
			new Point3D(1471, 571, 1),				
			new Point3D(1386, 502, -10),
			new Point3D(1369, 419, -21),				
			new Point3D(1308, 421, 37),
			new Point3D(1290, 280, -26),
			new Point3D(1148, 345, 70),
			new Point3D(1047, 273, 56),				
			new Point3D(991, 271, 56),				
			new Point3D(914, 304, 31),				
			new Point3D(814, 304, 7),				
			new Point3D(1365, 270, 37),				
			new Point3D(1505, 235, -17),
			new Point3D(1622, 260, 75),				
			new Point3D(1624, 309, 48),				
			new Point3D(1644, 336, 21),				
			new Point3D(1676, 301, 78),				
			new Point3D(1702, 291, 83),
			new Point3D(1657, 254, 78),				
			new Point3D(1794, 385, 29),				
			new Point3D(1797, 431, -14),				
			new Point3D(1814, 465, 8),
			new Point3D(1857, 521, -14),				
			new Point3D(1745, 531, 24),				
			new Point3D(1704, 570, 15),
			new Point3D(1736, 604, 37),				
			new Point3D(1828, 623, -13),
			new Point3D(1530, 669, -14),
			new Point3D(1655, 881, -26),
			new Point3D(1674, 980, 10),				
			new Point3D(1647, 1037, -24),				
			new Point3D(1682, 1076, -11),				
			new Point3D(1467, 1230, -16),				
			new Point3D(1234, 1277, -10),				
			new Point3D(1198, 1212, -19),
			new Point3D(1133, 1233, -5),				
			new Point3D(1107, 1173, -20),				
			new Point3D(1056, 1153, -24),				
			new Point3D(1117, 1038, -32),				
			new Point3D(1253, 959, -15),
			new Point3D(1246, 978, -34),				
			new Point3D(1180, 976, -30),				
			new Point3D(1296, 1011, 3)		
        };

        // Override monster types for Ilshenar
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
        public override TimeSpan ExpirationTime => TimeSpan.FromMinutes(5); // Longer duration

        // Override portal hue and sound
        public override int PortalHue => 302; // Gold hue
        public override int PortalSound => 0x20F; // Different sound effect

        // Override name and hue
        [Constructable]
        public IlshenarMagicMap() : base(0x14EB, "Ilshenar Adventure Map", 302)
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

            // Ensure the location is valid on Ilshenar
            if (location == Point3D.Zero || Map.Ilshenar.CanSpawnMobile(location))
                return location;

            // Fallback to a default location if invalid
            return new Point3D(700, 3703, -43); // Britain
        }

        // Override OnDoubleClick to set the destination map to Ilshenar
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
            Map destinationMap = Map.Ilshenar; // Set the destination map to Ilshenar

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(ExpirationTime);

            // Spawn challenges (monsters, chests, etc.)
            SpawnChallenges(destination, destinationMap, content, from); // Add 'from' parameter

            // Create portals (origin and return)
            CreatePortals(origin, originMap, destination, destinationMap, content);

            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location in Ilshenar!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        // Serialization
        public IlshenarMagicMap(Serial serial) : base(serial) { }

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