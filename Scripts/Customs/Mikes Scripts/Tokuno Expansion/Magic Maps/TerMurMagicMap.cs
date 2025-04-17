using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TerMurMagicMap : MagicMapBase
    {
        // Override predefined locations for TerMur
        public override List<Point3D> PredefinedLocations => new List<Point3D>
        {
			new Point3D(974, 161, -10),
			new Point3D(987, 328, 11),
			new Point3D(915, 501, -11),
			new Point3D(950, 552, -13),
			new Point3D(980, 491, -11),
			new Point3D(578, 799, -45),
			new Point3D(887, 273, 4),
			new Point3D(546, 760, -91),
			new Point3D(530, 658, 9),
			new Point3D(578, 900, -72),
			new Point3D(684, 579, -14),
			new Point3D(434, 701, 29),
			new Point3D(1031, 3660, 60),				
			new Point3D(1021, 4003, -42),				
			new Point3D(1172, 3642, -42),
			new Point3D(1158, 3521, -42),				
			new Point3D(1101, 3535, 15),
			new Point3D(1073, 3540, -44),
			new Point3D(1142, 3475, -42),
			new Point3D(1109, 3432, -42),				
			new Point3D(1166, 3334, -42),				
			new Point3D(1144, 3270, -42),				
			new Point3D(1126, 3178, -43),				
			new Point3D(1059, 3173, 38),				
			new Point3D(992, 3194, 54),
			new Point3D(1006, 3096, 38),				
			new Point3D(1050, 3061, 99),				
			new Point3D(943, 3034, 38),				
			new Point3D(847, 2976, 38),				
			new Point3D(790, 2944, 38),
			new Point3D(761, 3018, 38),				
			new Point3D(814, 3078, 38),				
			new Point3D(699, 3078, 59),				
			new Point3D(665, 3039, 37),
			new Point3D(650, 2992, 61),				
			new Point3D(602, 3008, 36),				
			new Point3D(564, 3061, 97),
			new Point3D(544, 3119, 49),				
			new Point3D(570, 2945, 39),
			new Point3D(522, 2962, 20),
			new Point3D(393, 3056, 36),				
			new Point3D(442, 3166, 32),
			new Point3D(383, 3237, 17),				
			new Point3D(334, 3273, 16),				
			new Point3D(370, 3286, -3),
			new Point3D(385, 3341, 37),				
			new Point3D(557, 3382, 37),
			new Point3D(564, 3449, 98),
			new Point3D(591, 3531, 37),
			new Point3D(540, 3603, 38),				
			new Point3D(453, 3578, 38),				
			new Point3D(388, 3525, 38),				
			new Point3D(316, 3584, 38),				
			new Point3D(396, 3629, 38),				
			new Point3D(468, 3623, 38),
			new Point3D(519, 3720, -43),				
			new Point3D(574, 3767, -33),				
			new Point3D(555, 3882, -33),				
			new Point3D(501, 3923, -42),				
			new Point3D(442, 3848, -36),
			new Point3D(745, 3708, -35),				
			new Point3D(700, 3741, 17),				
			new Point3D(624, 3953, -43),				
			new Point3D(586, 3977, -43),
			new Point3D(1022, 3580, -5),				
			new Point3D(1032, 3553, -5),			
			new Point3D(677, 824, -108)			
        };

        // Override monster types for TerMur
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
        public override int PortalHue => 1884; // Gold hue
        public override int PortalSound => 0x20F; // Different sound effect

        // Override name and hue
        [Constructable]
        public TerMurMagicMap() : base(0x14EB, "TerMur Adventure Map", 1884)
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

            // Ensure the location is valid on TerMur
            if (location == Point3D.Zero || Map.TerMur.CanSpawnMobile(location))
                return location;

            // Fallback to a default location if invalid
            return new Point3D(700, 3703, -43); // Britain
        }

        // Override OnDoubleClick to set the destination map to TerMur
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
            Map destinationMap = Map.TerMur; // Set the destination map to TerMur

            // Create content manager with expiration timer
            SpawnedContent content = new SpawnedContent(ExpirationTime);

            // Spawn challenges (monsters, chests, etc.)
            SpawnChallenges(destination, destinationMap, content, from); // Add 'from' parameter

            // Create portals (origin and return)
            CreatePortals(origin, originMap, destination, destinationMap, content);

            // Teleport player
            from.SendMessage("The magic map dissolves as you're transported to a mysterious location in TerMur!");
            from.MoveToWorld(destination, destinationMap);
            Delete();
        }

        // Serialization
        public TerMurMagicMap(Serial serial) : base(serial) { }

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