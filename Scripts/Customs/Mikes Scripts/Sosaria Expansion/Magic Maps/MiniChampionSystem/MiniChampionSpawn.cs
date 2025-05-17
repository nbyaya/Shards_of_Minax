using Server;
using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using System.Linq;

namespace Server.Engines.MiniChamps
{  
    public class MiniChamp : Item
    {
        public static void Initialize()
        {
            CommandSystem.Register("GenMiniChamp", AccessLevel.Administrator, new CommandEventHandler(GenStoneRuins_OnCommand));
        }

		public static MiniChamp CreateMiniChampOnFacet(MiniChampType type, int spawnRange, Point3D location, Map map)
		{
			MiniChamp miniChamp = new MiniChamp
			{
				Type = type,
				SpawnRange = spawnRange
			};
			miniChamp.MoveToWorld(location, map);
			miniChamp.Active = true;
			return miniChamp;
		}

        private static List<MiniChamp> Controllers = new List<MiniChamp>();

        [Usage("GenMiniChamp")]
        [Description("MiniChampion Generator")]
		public static void GenStoneRuins_OnCommand(CommandEventArgs e)
		{
			// List of predefined locations
			var locations = new List<(Point3D, Map, int)>
			{
				(new Point3D(974, 161, -10), Map.TerMur, 30),
				(new Point3D(987, 328, 11), Map.TerMur, 40),
				(new Point3D(915, 501, -11), Map.TerMur, 30),
				(new Point3D(950, 552, -13), Map.TerMur, 30),
				(new Point3D(980, 491, -11), Map.TerMur, 30),
				(new Point3D(578, 799, -45), Map.TerMur, 30),
				(new Point3D(887, 273, 4), Map.TerMur, 30),
				(new Point3D(546, 760, -91), Map.TerMur, 30),
				(new Point3D(530, 658, 9), Map.TerMur, 30),
				(new Point3D(578, 900, -72), Map.TerMur, 30),
				(new Point3D(684, 579, -14), Map.TerMur, 30),
				(new Point3D(434, 701, 29), Map.TerMur, 30),
				(new Point3D(677, 824, -108), Map.TerMur, 30),
				(new Point3D(395, 1913, 12), Map.Malas, 30),
				(new Point3D(1200, 1400, 0), Map.Felucca, 40),
				(new Point3D(800, 800, -10), Map.Felucca, 35),
				(new Point3D(1000, 1100, 0), Map.Trammel, 50),
				(new Point3D(300, 600, 5), Map.Ilshenar, 25),
/* 				(new Point3D(3340, 572, 0), Map.Trammel, 30),
				(new Point3D(3398, 335, 0), Map.Trammel, 30),
				(new Point3D(3239, 367, 4), Map.Trammel, 30),				
				(new Point3D(3008, 268, 5), Map.Trammel, 30),				
				(new Point3D(2866, 197, 0), Map.Trammel, 30),				
				(new Point3D(2874, 455, 10), Map.Trammel, 30),				
				(new Point3D(2886, 91, 0), Map.Trammel, 30),				
				(new Point3D(2665, 120, 0), Map.Trammel, 30),				
				(new Point3D(2608, 128, 0), Map.Trammel, 30),				
				(new Point3D(2431, 94, 0), Map.Trammel, 30),				
				(new Point3D(2416, 176, 0), Map.Trammel, 30),				
				(new Point3D(2462, 54, 0), Map.Trammel, 30),				
				(new Point3D(2327, 67, 0), Map.Trammel, 30),				
				(new Point3D(2177, 305, 0), Map.Trammel, 30),
				(new Point3D(2079, 156, 0), Map.Trammel, 30),
				(new Point3D(1788, 253, 6), Map.Trammel, 30),				
				(new Point3D(1976, 267, 8), Map.Trammel, 30),				
				(new Point3D(1934, 318, 0), Map.Trammel, 30),				
				(new Point3D(1901, 366, 0), Map.Trammel, 30),				
				(new Point3D(1987, 389, 0), Map.Trammel, 30),				
				(new Point3D(1862, 604, 0), Map.Trammel, 30),				
				(new Point3D(2071, 640, 0), Map.Trammel, 30),				
				(new Point3D(1549, 729, 27), Map.Trammel, 30),				
				(new Point3D(1641, 972, 0), Map.Trammel, 30),				
				(new Point3D(1511, 1086, 0), Map.Trammel, 30),				
				(new Point3D(1426, 992, 0), Map.Trammel, 30),					
				(new Point3D(1369, 835, 0), Map.Trammel, 30),
				(new Point3D(1258, 611, 24), Map.Trammel, 30),
				(new Point3D(1275, 529, 30), Map.Trammel, 30),				
				(new Point3D(1482, 238, 23), Map.Trammel, 30),				
				(new Point3D(1190, 142, 5), Map.Trammel, 30),				
				(new Point3D(1051, 365, 3), Map.Trammel, 30),				
				(new Point3D(824, 375, 0), Map.Trammel, 30),				
				(new Point3D(957, 706, 0), Map.Trammel, 30),				
				(new Point3D(954, 707, 20), Map.Trammel, 30),				
				(new Point3D(963, 717, -20), Map.Trammel, 30),				
				(new Point3D(1004, 766, -60), Map.Trammel, 30),				
				(new Point3D(1029, 1014, 0), Map.Trammel, 30),				
				(new Point3D(1030, 1360, 0), Map.Trammel, 30),					
				(new Point3D(744, 1359, 0), Map.Trammel, 30),
				(new Point3D(518, 1420, 0), Map.Trammel, 30),
				(new Point3D(355, 1344, 0), Map.Trammel, 30),				
				(new Point3D(158, 1363, 0), Map.Trammel, 30),				
				(new Point3D(142, 1490, 0), Map.Trammel, 30),				
				(new Point3D(362, 1441, 1), Map.Trammel, 30),				
				(new Point3D(359, 1467, 1), Map.Trammel, 30),				
				(new Point3D(386, 1654, 0), Map.Trammel, 30),				
				(new Point3D(659, 1689, 0), Map.Trammel, 30),				
				(new Point3D(772, 1682, 0), Map.Trammel, 30),				
				(new Point3D(833, 1831, 0), Map.Trammel, 30),				
				(new Point3D(851, 2070, 0), Map.Trammel, 30),				
				(new Point3D(1000, 2239, 0), Map.Trammel, 30),					
				(new Point3D(1074, 2204, 0), Map.Trammel, 30),
				(new Point3D(1076, 2262, 0), Map.Trammel, 30),
				(new Point3D(1144, 2224, 27), Map.Trammel, 30),				
				(new Point3D(1216, 2201, 5), Map.Trammel, 30),				
				(new Point3D(1219, 2268, 0), Map.Trammel, 30),				
				(new Point3D(1102, 2482, 0), Map.Trammel, 30),				
				(new Point3D(1060, 2718, 0), Map.Trammel, 30),				
				(new Point3D(1115, 2654, 0), Map.Trammel, 30),				
				(new Point3D(1124, 2824, 0), Map.Trammel, 30),				
				(new Point3D(1166, 2957, 0), Map.Trammel, 30),				
				(new Point3D(1369, 2712, 0), Map.Trammel, 30),				
				(new Point3D(1362, 2891, 0), Map.Trammel, 30),				
				(new Point3D(1359, 3118, 0), Map.Trammel, 30),					
				(new Point3D(1456, 3134, 0), Map.Trammel, 30),
				(new Point3D(1588, 3202, 0), Map.Trammel, 30),
				(new Point3D(1608, 3372, 0), Map.Trammel, 30),				
				(new Point3D(1651, 3548, 0), Map.Trammel, 30),				
				(new Point3D(1726, 3488, 2), Map.Trammel, 30),				
				(new Point3D(1927, 3419, 0), Map.Trammel, 30),				
				(new Point3D(2019, 3382, 0), Map.Trammel, 30),				
				(new Point3D(2159, 3355, 10), Map.Trammel, 30),				
				(new Point3D(2160, 3053, 0), Map.Trammel, 30),				
				(new Point3D(2107, 3000, 30), Map.Trammel, 30),				
				(new Point3D(1881, 3256, 0), Map.Trammel, 30),				
				(new Point3D(1846, 3204, 0), Map.Trammel, 30),				
				(new Point3D(1736, 3091, 0), Map.Trammel, 30),					
				(new Point3D(1675, 2972, 0), Map.Trammel, 30),
				(new Point3D(1966, 2937, 0), Map.Trammel, 30),
				(new Point3D(2119, 2734, 20), Map.Trammel, 30),				
				(new Point3D(1705, 2692, 5), Map.Trammel, 30),				
				(new Point3D(1668, 2486, 10), Map.Trammel, 30),				
				(new Point3D(1822, 2414, 0), Map.Trammel, 30),				
				(new Point3D(2023, 2316, 0), Map.Trammel, 30),				
				(new Point3D(2104, 2103, 0), Map.Trammel, 30),				
				(new Point3D(1788, 2170, 0), Map.Trammel, 30),				
				(new Point3D(1642, 2368, 0), Map.Trammel, 30),				
				(new Point3D(1614, 2116, 0), Map.Trammel, 30),				
				(new Point3D(1473, 2159, 6), Map.Trammel, 30),				
				(new Point3D(1386, 2009, 0), Map.Trammel, 30),					
				(new Point3D(1350, 1499, 20), Map.Trammel, 30),
				(new Point3D(1378, 1319, 0), Map.Trammel, 30),
				(new Point3D(1445, 1227, 0), Map.Trammel, 30),				
				(new Point3D(1761, 1176, 0), Map.Trammel, 30),				
				(new Point3D(1722, 1065, 0), Map.Trammel, 30),				
				(new Point3D(1731, 916, 0), Map.Trammel, 30),				
				(new Point3D(1708, 1428, 0), Map.Trammel, 30),				
				(new Point3D(1981, 1507, 0), Map.Trammel, 30),				
				(new Point3D(1992, 1010, 0), Map.Trammel, 30),				
				(new Point3D(1809, 1031, 0), Map.Trammel, 30),				
				(new Point3D(1828, 1030, 0), Map.Trammel, 30),				
				(new Point3D(2037, 970, 0), Map.Trammel, 30),				
				(new Point3D(1951, 888, -1), Map.Trammel, 30),					
				(new Point3D(1873, 914, 3), Map.Trammel, 30),
				(new Point3D(1803, 854, -1), Map.Trammel, 30),				
				(new Point3D(1867, 741, 6), Map.Trammel, 30),				
				(new Point3D(1576, 828, 0), Map.Trammel, 30),				
				(new Point3D(1254, 869, 16), Map.Trammel, 30),				
				(new Point3D(1229, 936, 10), Map.Trammel, 30),				
				(new Point3D(2056, 624, 0), Map.Trammel, 30),				
				(new Point3D(2241, 843, 0), Map.Trammel, 30),				
				(new Point3D(2325, 820, 0), Map.Trammel, 30),				
				(new Point3D(2359, 806, 0), Map.Trammel, 30),				
				(new Point3D(2181, 1302, 0), Map.Trammel, 30),				
				(new Point3D(2187, 1416, -2), Map.Trammel, 30),					
				(new Point3D(2582, 1120, 0), Map.Trammel, 30),
				(new Point3D(2739, 1061, 0), Map.Trammel, 30),				
				(new Point3D(2745, 858, 0), Map.Trammel, 30),				
				(new Point3D(2636, 720, 0), Map.Trammel, 30),				
				(new Point3D(2663, 469, 15), Map.Trammel, 30),				
				(new Point3D(3920, 196, -10), Map.Trammel, 30),				
				(new Point3D(3959, 358, 0), Map.Trammel, 30),				
				(new Point3D(4051, 562, 0), Map.Trammel, 30),				
				(new Point3D(4050, 439, 3), Map.Trammel, 30),				
				(new Point3D(4229, 508, 0), Map.Trammel, 30),				
				(new Point3D(2504, 3609, 3), Map.Trammel, 30),				
				(new Point3D(2376, 3411, 3), Map.Trammel, 30),					
				(new Point3D(2462, 3932, 0), Map.Trammel, 30),
				(new Point3D(2151, 3930, 0), Map.Trammel, 30),				
				(new Point3D(1144, 3510, 0), Map.Trammel, 30),				
				(new Point3D(1062, 3131, 0), Map.Trammel, 30),			 */	
				(new Point3D(3340, 572, 0), Map.Felucca, 30),
				(new Point3D(3398, 335, 0), Map.Felucca, 30),
/* 				(new Point3D(3239, 367, 4), Map.Felucca, 30),				
				(new Point3D(3008, 268, 5), Map.Felucca, 30),				
				(new Point3D(2866, 197, 0), Map.Felucca, 30),				
				(new Point3D(2874, 455, 10), Map.Felucca, 30),				
				(new Point3D(2886, 91, 0), Map.Felucca, 30),				
				(new Point3D(2665, 120, 0), Map.Felucca, 30),				
				(new Point3D(2608, 128, 0), Map.Felucca, 30),				
				(new Point3D(2431, 94, 0), Map.Felucca, 30),				
				(new Point3D(2416, 176, 0), Map.Felucca, 30),				
				(new Point3D(2462, 54, 0), Map.Felucca, 30),				
				(new Point3D(2327, 67, 0), Map.Felucca, 30),				
				(new Point3D(2177, 305, 0), Map.Felucca, 30),
				(new Point3D(2079, 156, 0), Map.Felucca, 30),
				(new Point3D(1788, 253, 6), Map.Felucca, 30),				
				(new Point3D(1976, 267, 8), Map.Felucca, 30),				
				(new Point3D(1934, 318, 0), Map.Felucca, 30),				
				(new Point3D(1901, 366, 0), Map.Felucca, 30),				
				(new Point3D(1987, 389, 0), Map.Felucca, 30),				
				(new Point3D(1862, 604, 0), Map.Felucca, 30),				
				(new Point3D(2071, 640, 0), Map.Felucca, 30),				
				(new Point3D(1549, 729, 27), Map.Felucca, 30),				
				(new Point3D(1641, 972, 0), Map.Felucca, 30),				
				(new Point3D(1511, 1086, 0), Map.Felucca, 30),				
				(new Point3D(1426, 992, 0), Map.Felucca, 30),					
				(new Point3D(1369, 835, 0), Map.Felucca, 30),
				(new Point3D(1258, 611, 24), Map.Felucca, 30),
				(new Point3D(1275, 529, 30), Map.Felucca, 30),				
				(new Point3D(1482, 238, 23), Map.Felucca, 30),				
				(new Point3D(1190, 142, 5), Map.Felucca, 30),				
				(new Point3D(1051, 365, 3), Map.Felucca, 30),				
				(new Point3D(824, 375, 0), Map.Felucca, 30),				
				(new Point3D(957, 706, 0), Map.Felucca, 30),				
				(new Point3D(954, 707, 20), Map.Felucca, 30),				
				(new Point3D(963, 717, -20), Map.Felucca, 30),				
				(new Point3D(1004, 766, -60), Map.Felucca, 30),				
				(new Point3D(1029, 1014, 0), Map.Felucca, 30),				
				(new Point3D(1030, 1360, 0), Map.Felucca, 30),					
				(new Point3D(744, 1359, 0), Map.Felucca, 30),
				(new Point3D(518, 1420, 0), Map.Felucca, 30),
				(new Point3D(355, 1344, 0), Map.Felucca, 30),				
				(new Point3D(158, 1363, 0), Map.Felucca, 30),				
				(new Point3D(142, 1490, 0), Map.Felucca, 30),				
				(new Point3D(362, 1441, 1), Map.Felucca, 30),				
				(new Point3D(359, 1467, 1), Map.Felucca, 30),				
				(new Point3D(386, 1654, 0), Map.Felucca, 30),				
				(new Point3D(659, 1689, 0), Map.Felucca, 30),				
				(new Point3D(772, 1682, 0), Map.Felucca, 30),				
				(new Point3D(833, 1831, 0), Map.Felucca, 30),				
				(new Point3D(851, 2070, 0), Map.Felucca, 30),				
				(new Point3D(1000, 2239, 0), Map.Felucca, 30),					
				(new Point3D(1074, 2204, 0), Map.Felucca, 30),
				(new Point3D(1076, 2262, 0), Map.Felucca, 30),
				(new Point3D(1144, 2224, 27), Map.Felucca, 30),				
				(new Point3D(1216, 2201, 5), Map.Felucca, 30),				
				(new Point3D(1219, 2268, 0), Map.Felucca, 30),				
				(new Point3D(1102, 2482, 0), Map.Felucca, 30),				
				(new Point3D(1060, 2718, 0), Map.Felucca, 30),				
				(new Point3D(1115, 2654, 0), Map.Felucca, 30),				
				(new Point3D(1124, 2824, 0), Map.Felucca, 30),				
				(new Point3D(1166, 2957, 0), Map.Felucca, 30),				
				(new Point3D(1369, 2712, 0), Map.Felucca, 30),				
				(new Point3D(1362, 2891, 0), Map.Felucca, 30),				
				(new Point3D(1359, 3118, 0), Map.Felucca, 30),					
				(new Point3D(1456, 3134, 0), Map.Felucca, 30),
				(new Point3D(1588, 3202, 0), Map.Felucca, 30),
				(new Point3D(1608, 3372, 0), Map.Felucca, 30),				
				(new Point3D(1651, 3548, 0), Map.Felucca, 30),				
				(new Point3D(1726, 3488, 2), Map.Felucca, 30),				
				(new Point3D(1927, 3419, 0), Map.Felucca, 30),				
				(new Point3D(2019, 3382, 0), Map.Felucca, 30),				
				(new Point3D(2159, 3355, 10), Map.Felucca, 30),				
				(new Point3D(2160, 3053, 0), Map.Felucca, 30),				
				(new Point3D(2107, 3000, 30), Map.Felucca, 30),				
				(new Point3D(1881, 3256, 0), Map.Felucca, 30),				
				(new Point3D(1846, 3204, 0), Map.Felucca, 30),				
				(new Point3D(1736, 3091, 0), Map.Felucca, 30),					
				(new Point3D(1675, 2972, 0), Map.Felucca, 30),
				(new Point3D(1966, 2937, 0), Map.Felucca, 30),
				(new Point3D(2119, 2734, 20), Map.Felucca, 30),				
				(new Point3D(1705, 2692, 5), Map.Felucca, 30),				
				(new Point3D(1668, 2486, 10), Map.Felucca, 30),				
				(new Point3D(1822, 2414, 0), Map.Felucca, 30),				
				(new Point3D(2023, 2316, 0), Map.Felucca, 30),				
				(new Point3D(2104, 2103, 0), Map.Felucca, 30),				
				(new Point3D(1788, 2170, 0), Map.Felucca, 30),				
				(new Point3D(1642, 2368, 0), Map.Felucca, 30),				
				(new Point3D(1614, 2116, 0), Map.Felucca, 30),				
				(new Point3D(1473, 2159, 6), Map.Felucca, 30),				
				(new Point3D(1386, 2009, 0), Map.Felucca, 30),					
				(new Point3D(1350, 1499, 20), Map.Felucca, 30),
				(new Point3D(1378, 1319, 0), Map.Felucca, 30),
				(new Point3D(1445, 1227, 0), Map.Felucca, 30),				
				(new Point3D(1761, 1176, 0), Map.Felucca, 30),				
				(new Point3D(1722, 1065, 0), Map.Felucca, 30),				
				(new Point3D(1731, 916, 0), Map.Felucca, 30),				
				(new Point3D(1708, 1428, 0), Map.Felucca, 30),				
				(new Point3D(1981, 1507, 0), Map.Felucca, 30),				
				(new Point3D(1992, 1010, 0), Map.Felucca, 30),				
				(new Point3D(1809, 1031, 0), Map.Felucca, 30),				
				(new Point3D(1828, 1030, 0), Map.Felucca, 30),				
				(new Point3D(2037, 970, 0), Map.Felucca, 30),				
				(new Point3D(1951, 888, -1), Map.Felucca, 30),					
				(new Point3D(1873, 914, 3), Map.Felucca, 30),
				(new Point3D(1803, 854, -1), Map.Felucca, 30),				
				(new Point3D(1867, 741, 6), Map.Felucca, 30),				
				(new Point3D(1576, 828, 0), Map.Felucca, 30),				
				(new Point3D(1254, 869, 16), Map.Felucca, 30),				
				(new Point3D(1229, 936, 10), Map.Felucca, 30),				
				(new Point3D(2056, 624, 0), Map.Felucca, 30),				
				(new Point3D(2241, 843, 0), Map.Felucca, 30),				
				(new Point3D(2325, 820, 0), Map.Felucca, 30),				
				(new Point3D(2359, 806, 0), Map.Felucca, 30),				
				(new Point3D(2181, 1302, 0), Map.Felucca, 30),				
				(new Point3D(2187, 1416, -2), Map.Felucca, 30),					
				(new Point3D(2582, 1120, 0), Map.Felucca, 30),
				(new Point3D(2739, 1061, 0), Map.Felucca, 30),				
				(new Point3D(2745, 858, 0), Map.Felucca, 30),				
				(new Point3D(2636, 720, 0), Map.Felucca, 30),				
				(new Point3D(2663, 469, 15), Map.Felucca, 30),				
				(new Point3D(3920, 196, -10), Map.Felucca, 30),				
				(new Point3D(3959, 358, 0), Map.Felucca, 30),				
				(new Point3D(4051, 562, 0), Map.Felucca, 30),				
				(new Point3D(4050, 439, 3), Map.Felucca, 30),				
				(new Point3D(4229, 508, 0), Map.Felucca, 30),				
				(new Point3D(2504, 3609, 3), Map.Felucca, 30),				
				(new Point3D(2376, 3411, 3), Map.Felucca, 30),					
				(new Point3D(2462, 3932, 0), Map.Felucca, 30),
				(new Point3D(2151, 3930, 0), Map.Felucca, 30),				
				(new Point3D(1144, 3510, 0), Map.Felucca, 30),				
				(new Point3D(1062, 3131, 0), Map.Felucca, 30),				
				(new Point3D(1383, 1625, 30), Map.Felucca, 30),				
				(new Point3D(1313, 1656, 30), Map.Felucca, 30),				
				(new Point3D(1523, 1445, 15), Map.Felucca, 30),				
				(new Point3D(1527, 1428, 55), Map.Felucca, 30),				
				(new Point3D(1544, 1598, 12), Map.Felucca, 30),				
				(new Point3D(1562, 1741, 15), Map.Felucca, 30),				
				(new Point3D(1483, 1754, -2), Map.Felucca, 30),				
				(new Point3D(1343, 1733, 20), Map.Felucca, 30),					
				(new Point3D(1144, 1656, 0), Map.Felucca, 30),
				(new Point3D(2647, 2054, -20), Map.Felucca, 30),				
				(new Point3D(2605, 2101, -20), Map.Felucca, 30),				
				(new Point3D(2285, 1209, 0), Map.Felucca, 30),				
				(new Point3D(2210, 1115, 20), Map.Felucca, 30),				
				(new Point3D(2437, 1099, 8), Map.Felucca, 30),				
				(new Point3D(1398, 3744, -21), Map.Felucca, 30),				
				(new Point3D(1448, 3787, 0), Map.Felucca, 30),				
				(new Point3D(1423, 4011, 0), Map.Felucca, 30),				
				(new Point3D(1147, 3744, 0), Map.Felucca, 30),				
				(new Point3D(1280, 3734, 0), Map.Felucca, 30),				
				(new Point3D(3678, 2293, -2), Map.Felucca, 30),					
				(new Point3D(3681, 2238, 20), Map.Felucca, 30),
				(new Point3D(3720, 2066, 12), Map.Felucca, 30),				
				(new Point3D(2537, 501, 31), Map.Felucca, 30),				
				(new Point3D(2466, 529, 15), Map.Felucca, 30),				
				(new Point3D(2584, 528, 15), Map.Felucca, 30),				
				(new Point3D(2503, 554, 0), Map.Felucca, 30),				
				(new Point3D(2539, 664, 0), Map.Felucca, 30),				
				(new Point3D(4404, 1045, -2), Map.Felucca, 30),				
				(new Point3D(4492, 985, 0), Map.Felucca, 30),				
				(new Point3D(4327, 983, 0), Map.Felucca, 30),				
				(new Point3D(4395, 1163, 0), Map.Felucca, 30),				
				(new Point3D(4417, 1218, 0), Map.Felucca, 30),					
				(new Point3D(4529, 1378, 23), Map.Felucca, 30),
				(new Point3D(4383, 1414, 0), Map.Felucca, 30),				
				(new Point3D(3803, 1279, 5), Map.Felucca, 30),				
				(new Point3D(3734, 1315, 0), Map.Felucca, 30),				
				(new Point3D(3741, 1229, 0), Map.Felucca, 30),				
				(new Point3D(3618, 1234, 0), Map.Felucca, 30),				
				(new Point3D(3555, 1187, 0), Map.Felucca, 30),				
				(new Point3D(3675, 1155, 0), Map.Felucca, 30),				
				(new Point3D(3650, 2650, 0), Map.Felucca, 30),				
				(new Point3D(3635, 2547, 0), Map.Felucca, 30),				
				(new Point3D(3410, 2709, 0), Map.Felucca, 30),				
				(new Point3D(2948, 3356, 15), Map.Felucca, 30),					
				(new Point3D(3027, 3352, 15), Map.Felucca, 30),
				(new Point3D(3063, 3342, 15), Map.Felucca, 30),				
				(new Point3D(2958, 3448, 15), Map.Felucca, 30),				
				(new Point3D(2887, 3415, 35), Map.Felucca, 30),				
				(new Point3D(2878, 3473, 15), Map.Felucca, 30),				
				(new Point3D(2958, 3487, 15), Map.Felucca, 30),				
				(new Point3D(3014, 3529, 15), Map.Felucca, 30),				
				(new Point3D(810, 2218, 0), Map.Felucca, 30),				
				(new Point3D(790, 2278, 0), Map.Felucca, 30),				
				(new Point3D(610, 2275, 0), Map.Felucca, 30),				
				(new Point3D(620, 2127, 0), Map.Felucca, 30),				
				(new Point3D(560, 2147, 0), Map.Felucca, 30),					
				(new Point3D(2067, 2856, -2), Map.Felucca, 30),
				(new Point3D(2021, 2890, 0), Map.Felucca, 30),				
				(new Point3D(1951, 2815, 0), Map.Felucca, 30),				
				(new Point3D(1956, 2750, 10), Map.Felucca, 30),				
				(new Point3D(1891, 2716, 0), Map.Felucca, 30),				
				(new Point3D(1843, 2736, 0), Map.Felucca, 30),				
				(new Point3D(1825, 2676, 0), Map.Felucca, 30),				
				(new Point3D(1807, 2709, 20), Map.Felucca, 30),				
				(new Point3D(1866, 2645, 40), Map.Felucca, 30),				
				(new Point3D(2745, 858, 0), Map.Felucca, 30),				
				(new Point3D(2779, 966, 0), Map.Felucca, 30),				
				(new Point3D(2814, 955, 21), Map.Felucca, 30),					
				(new Point3D(2874, 915, 21), Map.Felucca, 30),
				(new Point3D(2912, 933, 21), Map.Felucca, 30),				
				(new Point3D(2955, 847, 21), Map.Felucca, 30),				
				(new Point3D(2972, 738, 21), Map.Felucca, 30),				
				(new Point3D(2907, 636, 16), Map.Felucca, 30),				
				(new Point3D(5249, 68, 14), Map.Felucca, 30),				
				(new Point3D(5212, 137, 0), Map.Felucca, 30),				
				(new Point3D(5190, 103, 5), Map.Felucca, 30),				
				(new Point3D(5164, 21, 27), Map.Felucca, 30),				
				(new Point3D(633, 1502, 0), Map.Felucca, 30),				
				(new Point3D(675, 1204, 0), Map.Felucca, 30),				
				(new Point3D(627, 1037, 0), Map.Felucca, 30),				
				(new Point3D(534, 1063, 0), Map.Felucca, 30),				
				(new Point3D(552, 966, 0), Map.Felucca, 30),				
				(new Point3D(519, 1007, 0), Map.Felucca, 30),				
				(new Point3D(388, 971, 0), Map.Felucca, 30),					
				(new Point3D(377, 905, 0), Map.Felucca, 30),				
				(new Point3D(355, 880, 0), Map.Felucca, 30),				
				(new Point3D(338, 817, 20), Map.Felucca, 30),				
				(new Point3D(311, 787, 20), Map.Felucca, 30),				
				(new Point3D(274, 771, 20), Map.Felucca, 30),				
				(new Point3D(280, 771, 0), Map.Felucca, 30),				
				(new Point3D(635, 821, 20), Map.Felucca, 30),				
				(new Point3D(610, 816, 0), Map.Felucca, 30), */
/* 				(new Point3D(1106, 418, -80), Map.Ilshenar, 30),
				(new Point3D(284, 341, -59), Map.Ilshenar, 30),				
				(new Point3D(489, 350, -59), Map.Ilshenar, 30),				
				(new Point3D(642, 310, -43), Map.Ilshenar, 30),				
				(new Point3D(772, 348, -43), Map.Ilshenar, 30),				
				(new Point3D(630, 493, -75), Map.Ilshenar, 30),				
				(new Point3D(512, 545, -60), Map.Ilshenar, 30),
				(new Point3D(290, 538, -22), Map.Ilshenar, 30),				
				(new Point3D(654, 749, -26), Map.Ilshenar, 30),				
				(new Point3D(466, 859, -76), Map.Ilshenar, 30),				
				(new Point3D(429, 950, -84), Map.Ilshenar, 30),				
				(new Point3D(328, 1100, -57), Map.Ilshenar, 30),
				(new Point3D(365, 1141, -57), Map.Ilshenar, 30),				
				(new Point3D(450, 1068, -85), Map.Ilshenar, 30),				
				(new Point3D(650, 934, -62), Map.Ilshenar, 30),				
				(new Point3D(640, 1035, -83), Map.Ilshenar, 30),
				(new Point3D(648, 1143, -72), Map.Ilshenar, 30),				
				(new Point3D(764, 1039, -30), Map.Ilshenar, 30),				
				(new Point3D(778, 1138, -30), Map.Ilshenar, 30),
				(new Point3D(830, 1186, -67), Map.Ilshenar, 30),				
				(new Point3D(758, 1311, -94), Map.Ilshenar, 30),
				(new Point3D(644, 1321, -57), Map.Ilshenar, 30),
				(new Point3D(557, 1377, 17), Map.Ilshenar, 30),
				(new Point3D(533, 1347, -53), Map.Ilshenar, 30),				
				(new Point3D(509, 1317, -53), Map.Ilshenar, 30),				
				(new Point3D(459, 1300, -55), Map.Ilshenar, 30),				
				(new Point3D(411, 1364, -18), Map.Ilshenar, 30),				
				(new Point3D(354, 1359, -25), Map.Ilshenar, 30),				
				(new Point3D(292, 1347, -24), Map.Ilshenar, 30),
				(new Point3D(299, 1306, -25), Map.Ilshenar, 30),				
				(new Point3D(385, 1272, -38), Map.Ilshenar, 30),				
				(new Point3D(331, 1205, -38), Map.Ilshenar, 30),				
				(new Point3D(356, 1141, -54), Map.Ilshenar, 30),				
				(new Point3D(910, 1191, 12), Map.Ilshenar, 30),
				(new Point3D(932, 1131, 12), Map.Ilshenar, 30),				
				(new Point3D(947, 1062, -13), Map.Ilshenar, 30),				
				(new Point3D(939, 950, -30), Map.Ilshenar, 30),				
				(new Point3D(1035, 875, -28), Map.Ilshenar, 30),
				(new Point3D(963, 783, -80), Map.Ilshenar, 30),				
				(new Point3D(891, 778, -80), Map.Ilshenar, 30),				
				(new Point3D(849, 777, 0), Map.Ilshenar, 30),
				(new Point3D(814, 778, -60), Map.Ilshenar, 30),				
				(new Point3D(964, 650, -80), Map.Ilshenar, 30),
				(new Point3D(977, 582, -80), Map.Ilshenar, 30),
				(new Point3D(972, 525, -80), Map.Ilshenar, 30),
				(new Point3D(912, 518, -80), Map.Ilshenar, 30),				
				(new Point3D(876, 526, -80), Map.Ilshenar, 30),				
				(new Point3D(994, 493, -79), Map.Ilshenar, 30),				
				(new Point3D(1044, 451, -80), Map.Ilshenar, 30),				
				(new Point3D(1107, 403, -80), Map.Ilshenar, 30),				
				(new Point3D(1087, 496, -73), Map.Ilshenar, 30),
				(new Point3D(1059, 530, -80), Map.Ilshenar, 30),				
				(new Point3D(1061, 560, -80), Map.Ilshenar, 30),				
				(new Point3D(1054, 624, -80), Map.Ilshenar, 30),				
				(new Point3D(1112, 637, -80), Map.Ilshenar, 30),				
				(new Point3D(1082, 714, -80), Map.Ilshenar, 30),
				(new Point3D(1125, 739, -80), Map.Ilshenar, 30),				
				(new Point3D(1181, 744, -80), Map.Ilshenar, 30),				
				(new Point3D(1351, 661, 108), Map.Ilshenar, 30),				
				(new Point3D(1372, 626, 105), Map.Ilshenar, 30),
				(new Point3D(1438, 643, -4), Map.Ilshenar, 30),				
				(new Point3D(1471, 571, 1), Map.Ilshenar, 30),				
				(new Point3D(1386, 502, -10), Map.Ilshenar, 30),
				(new Point3D(1369, 419, -21), Map.Ilshenar, 30),				
				(new Point3D(1308, 421, 37), Map.Ilshenar, 30),
				(new Point3D(1290, 280, -26), Map.Ilshenar, 30),
				(new Point3D(1148, 345, 70), Map.Ilshenar, 30),
				(new Point3D(1047, 273, 56), Map.Ilshenar, 30),				
				(new Point3D(991, 271, 56), Map.Ilshenar, 30),				
				(new Point3D(914, 304, 31), Map.Ilshenar, 30),				
				(new Point3D(814, 304, 7), Map.Ilshenar, 30),				
				(new Point3D(1365, 270, 37), Map.Ilshenar, 30),				
				(new Point3D(1505, 235, -17), Map.Ilshenar, 30),
				(new Point3D(1622, 260, 75), Map.Ilshenar, 30),				
				(new Point3D(1624, 309, 48), Map.Ilshenar, 30),				
				(new Point3D(1644, 336, 21), Map.Ilshenar, 30),				
				(new Point3D(1676, 301, 78), Map.Ilshenar, 30),				
				(new Point3D(1702, 291, 83), Map.Ilshenar, 30),
				(new Point3D(1657, 254, 78), Map.Ilshenar, 30),				
				(new Point3D(1794, 385, 29), Map.Ilshenar, 30),				
				(new Point3D(1797, 431, -14), Map.Ilshenar, 30),				
				(new Point3D(1814, 465, 8), Map.Ilshenar, 30),
				(new Point3D(1857, 521, -14), Map.Ilshenar, 30),				
				(new Point3D(1745, 531, 24), Map.Ilshenar, 30),				
				(new Point3D(1704, 570, 15), Map.Ilshenar, 30),
				(new Point3D(1736, 604, 37), Map.Ilshenar, 30),				
				(new Point3D(1828, 623, -13), Map.Ilshenar, 30),
				(new Point3D(1530, 669, -14), Map.Ilshenar, 30),
				(new Point3D(1655, 881, -26), Map.Ilshenar, 30),
				(new Point3D(1674, 980, 10), Map.Ilshenar, 30),				
				(new Point3D(1647, 1037, -24), Map.Ilshenar, 30),				
				(new Point3D(1682, 1076, -11), Map.Ilshenar, 30),				
				(new Point3D(1467, 1230, -16), Map.Ilshenar, 30),				
				(new Point3D(1234, 1277, -10), Map.Ilshenar, 30),				
				(new Point3D(1198, 1212, -19), Map.Ilshenar, 30),
				(new Point3D(1133, 1233, -5), Map.Ilshenar, 30),				
				(new Point3D(1107, 1173, -20), Map.Ilshenar, 30),				
				(new Point3D(1056, 1153, -24), Map.Ilshenar, 30),				
				(new Point3D(1117, 1038, -32), Map.Ilshenar, 30),				
				(new Point3D(1253, 959, -15), Map.Ilshenar, 30),
				(new Point3D(1246, 978, -34), Map.Ilshenar, 30),				
				(new Point3D(1180, 976, -30), Map.Ilshenar, 30),				
				(new Point3D(1296, 1011, 3), Map.Ilshenar, 30),
				(new Point3D(527, 1225, 25), Map.Tokuno, 30),
				(new Point3D(548, 1221, 25), Map.Tokuno, 30),				
				(new Point3D(588, 1130, 36), Map.Tokuno, 30),				
				(new Point3D(618, 1116, 34), Map.Tokuno, 30),
				(new Point3D(753, 1079, 23), Map.Tokuno, 30),				
				(new Point3D(745, 1011, 33), Map.Tokuno, 30),
				(new Point3D(719, 868, 33), Map.Tokuno, 30),
				(new Point3D(581, 927, 24), Map.Tokuno, 30),
				(new Point3D(542, 943, 29), Map.Tokuno, 30),				
				(new Point3D(548, 883, 21), Map.Tokuno, 30),				
				(new Point3D(519, 857, 2), Map.Tokuno, 30),				
				(new Point3D(654, 878, 29), Map.Tokuno, 30),				
				(new Point3D(750, 871, 32), Map.Tokuno, 30),				
				(new Point3D(923, 818, 29), Map.Tokuno, 30),
				(new Point3D(998, 788, 4), Map.Tokuno, 30),				
				(new Point3D(957, 694, 15), Map.Tokuno, 30),				
				(new Point3D(917, 637, 15), Map.Tokuno, 30),				
				(new Point3D(897, 600, 38), Map.Tokuno, 30),				
				(new Point3D(824, 736, 24), Map.Tokuno, 30),
				(new Point3D(876, 698, 4), Map.Tokuno, 30),				
				(new Point3D(1135, 900, 62), Map.Tokuno, 30),				
				(new Point3D(1119, 1078, 32), Map.Tokuno, 30),				
				(new Point3D(1214, 1098, 28), Map.Tokuno, 30),
				(new Point3D(1192, 1060, 21), Map.Tokuno, 30),				
				(new Point3D(1210, 938, 33), Map.Tokuno, 30),				
				(new Point3D(1263, 880, 3), Map.Tokuno, 30),
				(new Point3D(1233, 764, 23), Map.Tokuno, 30),				
				(new Point3D(1175, 801, 27), Map.Tokuno, 30),
				(new Point3D(1232, 657, 73), Map.Tokuno, 30),
				(new Point3D(1266, 577, 28), Map.Tokuno, 30),
				(new Point3D(1227, 529, 29), Map.Tokuno, 30),
				(new Point3D(1198, 489, 23), Map.Tokuno, 30),				
				(new Point3D(1104, 551, 23), Map.Tokuno, 30),				
				(new Point3D(987, 568, 10), Map.Tokuno, 30),
				(new Point3D(1089, 412, 6), Map.Tokuno, 30),				
				(new Point3D(910, 418, 14), Map.Tokuno, 30),
				(new Point3D(979, 299, 25), Map.Tokuno, 30),
				(new Point3D(952, 257, 19), Map.Tokuno, 30),
				(new Point3D(847, 316, 38), Map.Tokuno, 30),				
				(new Point3D(819, 286, 35), Map.Tokuno, 30),				
				(new Point3D(798, 373, 26), Map.Tokuno, 30),				
				(new Point3D(754, 185, 38), Map.Tokuno, 30),				
				(new Point3D(961, 145, 44), Map.Tokuno, 30),				
				(new Point3D(920, 60, 37), Map.Tokuno, 30),
				(new Point3D(636, 388, 42), Map.Tokuno, 30),				
				(new Point3D(663, 426, 32), Map.Tokuno, 30),				
				(new Point3D(592, 361, 23), Map.Tokuno, 30),				
				(new Point3D(541, 426, 27), Map.Tokuno, 30),				
				(new Point3D(470, 432, 30), Map.Tokuno, 30),
				(new Point3D(442, 376, 32), Map.Tokuno, 30),				
				(new Point3D(406, 595, 29), Map.Tokuno, 30),				
				(new Point3D(353, 545, 15), Map.Tokuno, 30),				
				(new Point3D(196, 632, 46), Map.Tokuno, 30),
				(new Point3D(190, 534, 41), Map.Tokuno, 30),				
				(new Point3D(199, 731, 32), Map.Tokuno, 30),				
				(new Point3D(319, 705, 58), Map.Tokuno, 30),
				(new Point3D(230, 793, 57), Map.Tokuno, 30),				
				(new Point3D(191, 916, 35), Map.Tokuno, 30),
				(new Point3D(272, 1142, 11), Map.Tokuno, 30),
				(new Point3D(372, 1047, 11), Map.Tokuno, 30),
				(new Point3D(1031, 3660, 60), Map.TerMur, 30),				
				(new Point3D(1021, 4003, -42), Map.TerMur, 30),				
				(new Point3D(1172, 3642, -42), Map.TerMur, 30),
				(new Point3D(1158, 3521, -42), Map.TerMur, 30),				
				(new Point3D(1101, 3535, 15), Map.TerMur, 30),
				(new Point3D(1073, 3540, -44), Map.TerMur, 30),
				(new Point3D(1142, 3475, -42), Map.TerMur, 30),
				(new Point3D(1109, 3432, -42), Map.TerMur, 30),				
				(new Point3D(1166, 3334, -42), Map.TerMur, 30),				
				(new Point3D(1144, 3270, -42), Map.TerMur, 30),				
				(new Point3D(1126, 3178, -43), Map.TerMur, 30),				
				(new Point3D(1059, 3173, 38), Map.TerMur, 30),				
				(new Point3D(992, 3194, 54), Map.TerMur, 30),
				(new Point3D(1006, 3096, 38), Map.TerMur, 30),				
				(new Point3D(1050, 3061, 99), Map.TerMur, 30),				
				(new Point3D(943, 3034, 38), Map.TerMur, 30),				
				(new Point3D(847, 2976, 38), Map.TerMur, 30),				
				(new Point3D(790, 2944, 38), Map.TerMur, 30),
				(new Point3D(761, 3018, 38), Map.TerMur, 30),				
				(new Point3D(814, 3078, 38), Map.TerMur, 30),				
				(new Point3D(699, 3078, 59), Map.TerMur, 30),				
				(new Point3D(665, 3039, 37), Map.TerMur, 30),
				(new Point3D(650, 2992, 61), Map.TerMur, 30),				
				(new Point3D(602, 3008, 36), Map.TerMur, 30),				
				(new Point3D(564, 3061, 97), Map.TerMur, 30),
				(new Point3D(544, 3119, 49), Map.TerMur, 30),				
				(new Point3D(570, 2945, 39), Map.TerMur, 30),
				(new Point3D(522, 2962, 20), Map.TerMur, 30),
				(new Point3D(393, 3056, 36), Map.TerMur, 30),				
				(new Point3D(442, 3166, 32), Map.TerMur, 30),
				(new Point3D(383, 3237, 17), Map.TerMur, 30),				
				(new Point3D(334, 3273, 16), Map.TerMur, 30),				
				(new Point3D(370, 3286, -3), Map.TerMur, 30),
				(new Point3D(385, 3341, 37), Map.TerMur, 30),				
				(new Point3D(557, 3382, 37), Map.TerMur, 30),
				(new Point3D(564, 3449, 98), Map.TerMur, 30),
				(new Point3D(591, 3531, 37), Map.TerMur, 30),
				(new Point3D(540, 3603, 38), Map.TerMur, 30),				
				(new Point3D(453, 3578, 38), Map.TerMur, 30),				
				(new Point3D(388, 3525, 38), Map.TerMur, 30),				
				(new Point3D(316, 3584, 38), Map.TerMur, 30),				
				(new Point3D(396, 3629, 38), Map.TerMur, 30),				
				(new Point3D(468, 3623, 38), Map.TerMur, 30),
				(new Point3D(519, 3720, -43), Map.TerMur, 30),				
				(new Point3D(574, 3767, -33), Map.TerMur, 30),				
				(new Point3D(555, 3882, -33), Map.TerMur, 30),				
				(new Point3D(501, 3923, -42), Map.TerMur, 30),				
				(new Point3D(442, 3848, -36), Map.TerMur, 30),
				(new Point3D(745, 3708, -35), Map.TerMur, 30),				
				(new Point3D(700, 3741, 17), Map.TerMur, 30),				
				(new Point3D(624, 3953, -43), Map.TerMur, 30),				
				(new Point3D(586, 3977, -43), Map.TerMur, 30),
				(new Point3D(1022, 3580, -5), Map.TerMur, 30),				
				(new Point3D(1032, 3553, -5), Map.TerMur, 30), */					
				(new Point3D(700, 800, 10), Map.Tokuno, 30)
			};

			// List of MiniChampTypes
			var types = new List<MiniChampType>
			{
				MiniChampType.CrimsonVeins,
				MiniChampType.AbyssalLair,
				MiniChampType.DiscardedCavernClanRibbon,
				MiniChampType.DiscardedCavernClanScratch,
				MiniChampType.DiscardedCavernClanChitter,
				MiniChampType.EnslavedGoblins,
				MiniChampType.FairyDragonLair,
				MiniChampType.FireTemple,
				MiniChampType.LandsoftheLich,
				MiniChampType.LavaCaldera,
				MiniChampType.PassageofTears,
				MiniChampType.SecretGarden,
				MiniChampType.SkeletalDragon,
				MiniChampType.Encounter1,
				MiniChampType.Encounter2,
				MiniChampType.Encounter3,
				MiniChampType.Encounter4,
				MiniChampType.Encounter5,
				MiniChampType.Encounter6,
				MiniChampType.Encounter7,
				MiniChampType.Encounter8,
				MiniChampType.Encounter9,
				MiniChampType.Encounter10,
				MiniChampType.Encounter11,
				MiniChampType.Encounter12,
				MiniChampType.Encounter13,
				MiniChampType.Encounter14,
				MiniChampType.Encounter15,
				MiniChampType.Encounter16,
				MiniChampType.Encounter17,
				MiniChampType.Encounter18,
				MiniChampType.Encounter19,
				MiniChampType.Encounter20,
				MiniChampType.Encounter21,
				MiniChampType.Encounter22,
				MiniChampType.Encounter23,
				MiniChampType.Encounter24,
				MiniChampType.Encounter25,
				MiniChampType.Encounter26,
				MiniChampType.Encounter27,
				MiniChampType.Encounter28,
				MiniChampType.Encounter29,
				MiniChampType.Encounter30,
				MiniChampType.Encounter31,
				MiniChampType.Encounter32,
				MiniChampType.Encounter33,
				MiniChampType.Encounter34,
				MiniChampType.Encounter35,
				MiniChampType.Encounter36,
				MiniChampType.Encounter37,
				MiniChampType.Encounter38,
				MiniChampType.Encounter39,
				MiniChampType.Encounter40,
				MiniChampType.Encounter41,
				MiniChampType.Encounter42,
				MiniChampType.Encounter43,
				MiniChampType.Encounter44,
				MiniChampType.Encounter45,
				MiniChampType.Encounter46,
				MiniChampType.Encounter47,
				MiniChampType.Encounter48,
				MiniChampType.Encounter49,
				MiniChampType.Encounter50,
				MiniChampType.Encounter51,
				MiniChampType.Encounter52,
				MiniChampType.Encounter53,
				MiniChampType.Encounter54,
				MiniChampType.Encounter55,
				MiniChampType.Encounter56,
				MiniChampType.Encounter57,
				MiniChampType.Encounter58,
				MiniChampType.Encounter59,
				MiniChampType.Encounter60,
				MiniChampType.Encounter61,
				MiniChampType.Encounter62,
				MiniChampType.Encounter63,
				MiniChampType.Encounter64,
				MiniChampType.Encounter65,
				MiniChampType.Encounter66,
				MiniChampType.Encounter67,
				MiniChampType.Encounter68,
				MiniChampType.Encounter69,
				MiniChampType.Encounter70,
				MiniChampType.Encounter71,
				MiniChampType.Encounter72,
				MiniChampType.Encounter73,
				MiniChampType.Encounter74,
				MiniChampType.Encounter75,
				MiniChampType.Encounter76,
				MiniChampType.Encounter77,
				MiniChampType.Encounter78,
				MiniChampType.Encounter79,
				MiniChampType.Encounter80,
				MiniChampType.Encounter81,
				MiniChampType.Encounter82,
				MiniChampType.Encounter83,
				MiniChampType.Encounter84,
				MiniChampType.Encounter85,
				MiniChampType.Encounter86,
				MiniChampType.Encounter87,
				MiniChampType.Encounter88,
				MiniChampType.Encounter89,
				MiniChampType.Encounter90,
				MiniChampType.Encounter91,
				MiniChampType.Encounter92,
				MiniChampType.Encounter93,
				MiniChampType.Encounter94,
				MiniChampType.Encounter95,
				MiniChampType.Encounter96,
				MiniChampType.Encounter97,
				MiniChampType.Encounter98,
				MiniChampType.Encounter99,
				MiniChampType.Encounter100,
				MiniChampType.Encounter101,
				MiniChampType.Encounter102,
				MiniChampType.Encounter103,
				MiniChampType.Encounter104,
				MiniChampType.Encounter105,
				MiniChampType.Encounter106,
				MiniChampType.Encounter107,
				MiniChampType.Encounter108,
				MiniChampType.Encounter109,
				MiniChampType.Encounter110,
				MiniChampType.Encounter111,
				MiniChampType.Encounter112,
				MiniChampType.Encounter113,
				MiniChampType.Encounter114,
				MiniChampType.Encounter115,
				MiniChampType.Encounter116,
				MiniChampType.Encounter117,
				MiniChampType.Encounter118,
				MiniChampType.Encounter119,
				MiniChampType.Encounter120,
				MiniChampType.Encounter121,
				MiniChampType.Encounter122,
				MiniChampType.Encounter123,
				MiniChampType.Encounter124,
				MiniChampType.Encounter125,
				MiniChampType.Encounter126,
				MiniChampType.Encounter127,
				MiniChampType.Encounter128,
				MiniChampType.Encounter129,
				MiniChampType.Encounter130,
				MiniChampType.Encounter131,
				MiniChampType.Encounter132,
				MiniChampType.Encounter133,
				MiniChampType.Encounter134,
				MiniChampType.Encounter135,
				MiniChampType.Encounter136,
				MiniChampType.Encounter137,
				MiniChampType.Encounter138,
				MiniChampType.Encounter139,
				MiniChampType.Encounter140,
				MiniChampType.Encounter141,
				MiniChampType.Encounter142,
				MiniChampType.Encounter143,
				MiniChampType.Encounter144,
				MiniChampType.Encounter145,
				MiniChampType.Encounter146,
				MiniChampType.Encounter147,
				MiniChampType.Encounter148,
				MiniChampType.Encounter149,
				MiniChampType.Encounter150,
				MiniChampType.Encounter151,
				MiniChampType.Encounter152,
				MiniChampType.Encounter153,
				MiniChampType.Encounter154,
				MiniChampType.Encounter155,
				MiniChampType.Encounter156,
				MiniChampType.Encounter157,
				MiniChampType.Encounter158,
				MiniChampType.Encounter159,
				MiniChampType.Encounter160,
				MiniChampType.Encounter161,
				MiniChampType.Encounter162,
				MiniChampType.Encounter163,
				MiniChampType.Encounter164,
				MiniChampType.Encounter165,
				MiniChampType.Encounter166,
				MiniChampType.Encounter167,
				MiniChampType.Encounter168,
				MiniChampType.Encounter169,
				MiniChampType.Encounter170,
				MiniChampType.Encounter171,
				MiniChampType.Encounter172,
				MiniChampType.Encounter173,
				MiniChampType.Encounter174,
				MiniChampType.Encounter175,
				MiniChampType.Encounter176,
				MiniChampType.Encounter177,
				MiniChampType.Encounter178,
				MiniChampType.Encounter179,
				MiniChampType.Encounter180,
				MiniChampType.Encounter181,
				MiniChampType.Encounter182,
				MiniChampType.Encounter183,
				MiniChampType.Encounter184,
				MiniChampType.Encounter185,
				MiniChampType.Encounter186,
				MiniChampType.Encounter187,
				MiniChampType.Encounter188,
				MiniChampType.Encounter189,
				MiniChampType.Encounter190,
				MiniChampType.Encounter191,
				MiniChampType.Encounter192,
				MiniChampType.Encounter193,
				MiniChampType.Encounter194,
				MiniChampType.Encounter195,
				MiniChampType.Encounter196,
				MiniChampType.Encounter197,
				MiniChampType.Encounter198,
				MiniChampType.Encounter199,
				MiniChampType.Encounter200,
				MiniChampType.Encounter201,
				MiniChampType.Encounter202,
				MiniChampType.Encounter203,
				MiniChampType.Encounter204,
				MiniChampType.Encounter205,
				MiniChampType.Encounter206,
				MiniChampType.Encounter207,
				MiniChampType.Encounter208,
				MiniChampType.Encounter209,
				MiniChampType.Encounter210,
				MiniChampType.Encounter211,
				MiniChampType.Encounter212,
				MiniChampType.Encounter213,
				MiniChampType.Encounter214,
				MiniChampType.Encounter215,
				MiniChampType.Encounter216,
				MiniChampType.Encounter217,
				MiniChampType.Encounter218,
				MiniChampType.Encounter219,
				MiniChampType.Encounter220,
				MiniChampType.Encounter221,
				MiniChampType.Encounter222,
				MiniChampType.Encounter223,
				MiniChampType.Encounter224,
				MiniChampType.Encounter225,
				MiniChampType.Encounter226,
				MiniChampType.Encounter227,
				MiniChampType.Encounter228,
				MiniChampType.Encounter229,
				MiniChampType.Encounter230,
				MiniChampType.Encounter231,
				MiniChampType.Encounter232,
				MiniChampType.Encounter233,
				MiniChampType.Encounter234,
				MiniChampType.Encounter235,
				MiniChampType.Encounter236,
				MiniChampType.Encounter237,
				MiniChampType.Encounter238,
				MiniChampType.Encounter239,
				MiniChampType.Encounter240,
				MiniChampType.Encounter241,
				MiniChampType.Encounter242,
				MiniChampType.Encounter243,
				MiniChampType.Encounter244,
				MiniChampType.Encounter245,
				MiniChampType.Encounter246,
				MiniChampType.Encounter247,
				MiniChampType.Encounter248,
				MiniChampType.Encounter249,
				MiniChampType.Encounter250,
				MiniChampType.Encounter251,
				MiniChampType.Encounter252,
				MiniChampType.Encounter253,
				MiniChampType.Encounter254,
				MiniChampType.Encounter255,
				MiniChampType.Encounter256,
				MiniChampType.Encounter257,
				MiniChampType.Encounter258,
				MiniChampType.Encounter259,
				MiniChampType.Encounter260,
				MiniChampType.Encounter261,
				MiniChampType.Encounter262,
				MiniChampType.Encounter263,
				MiniChampType.Encounter264,
				MiniChampType.Encounter265,
				MiniChampType.Encounter266,
				MiniChampType.Encounter267,
				MiniChampType.Encounter268,
				MiniChampType.Encounter269,
				MiniChampType.Encounter270,
				MiniChampType.Encounter271,
				MiniChampType.Encounter272,
				MiniChampType.Encounter273,
				MiniChampType.Encounter274,
				MiniChampType.Encounter275,
				MiniChampType.Encounter276,
				MiniChampType.Encounter277,
				MiniChampType.Encounter278,
				MiniChampType.Encounter279,
				MiniChampType.Encounter280,
				MiniChampType.Encounter281,
				MiniChampType.Encounter282,
				MiniChampType.Encounter283,
				MiniChampType.Encounter284,
				MiniChampType.Encounter285,
				MiniChampType.Encounter286,
				MiniChampType.Encounter287,
				MiniChampType.Encounter288,
				MiniChampType.Encounter289,
				MiniChampType.Encounter290,
				MiniChampType.Encounter291,
				MiniChampType.Encounter292,
				MiniChampType.Encounter293,
				MiniChampType.Encounter294,
				MiniChampType.Encounter295,
				MiniChampType.Encounter296,
				MiniChampType.Encounter297,
				MiniChampType.Encounter298,
				MiniChampType.Encounter299,
				MiniChampType.Encounter300,
				MiniChampType.Encounter301,
				MiniChampType.Encounter302,
				MiniChampType.Encounter303,
				MiniChampType.Encounter304,
				MiniChampType.Encounter305,
				MiniChampType.Encounter306,
				MiniChampType.Encounter307,
				MiniChampType.Encounter308,
				MiniChampType.Encounter309,
				MiniChampType.Encounter310,
				MiniChampType.Encounter311,
				MiniChampType.Encounter312,
				MiniChampType.Encounter313,
				MiniChampType.Encounter314,
				MiniChampType.Encounter315,
				MiniChampType.Encounter316,
				MiniChampType.Encounter317,
				MiniChampType.Encounter318,
				MiniChampType.Encounter319,
				MiniChampType.Encounter320,
				MiniChampType.Encounter321,
				MiniChampType.Encounter322,
				MiniChampType.Encounter323,
				MiniChampType.Encounter324,
				MiniChampType.Encounter325,
				MiniChampType.Encounter326,
				MiniChampType.Encounter327,
				MiniChampType.Encounter328,
				MiniChampType.Encounter329,
				MiniChampType.Encounter330,
				MiniChampType.Encounter331,
				MiniChampType.Encounter332,
				MiniChampType.Encounter333,
				MiniChampType.Encounter334,
				MiniChampType.Encounter335,
				MiniChampType.Encounter336,
				MiniChampType.Encounter337,
				MiniChampType.Encounter338,
				MiniChampType.Encounter339,
				MiniChampType.Encounter340,
				MiniChampType.Encounter341,
				MiniChampType.Encounter342,
				MiniChampType.Encounter343,
				MiniChampType.Encounter344,
				MiniChampType.Encounter345,
				MiniChampType.Encounter346,
				MiniChampType.Encounter347,
				MiniChampType.Encounter348,
				MiniChampType.Encounter349,
				MiniChampType.Encounter350,
				MiniChampType.Encounter351,
				MiniChampType.Encounter352,
				MiniChampType.Encounter353,
				MiniChampType.Encounter354,
				MiniChampType.Encounter355,
				MiniChampType.Encounter356,
				MiniChampType.Encounter357,
				MiniChampType.Encounter358,
				MiniChampType.Encounter359,
				MiniChampType.Encounter360,
				MiniChampType.Encounter361,
				MiniChampType.Encounter362,
				MiniChampType.Encounter363,
				MiniChampType.Encounter364,
				MiniChampType.Encounter365,
				MiniChampType.Encounter366,
				MiniChampType.Encounter367,
				MiniChampType.Encounter368,
				MiniChampType.Encounter369,
				MiniChampType.Encounter370,
				MiniChampType.Encounter371,
				MiniChampType.Encounter372,
				MiniChampType.Encounter373,
				MiniChampType.Encounter374,
				MiniChampType.Encounter375,
				MiniChampType.Encounter376,
				MiniChampType.Encounter377,
				MiniChampType.Encounter378,
				MiniChampType.Encounter379,
				MiniChampType.Encounter380,
				MiniChampType.Encounter381,
				MiniChampType.Encounter382,
				MiniChampType.Encounter383,
				MiniChampType.Encounter384,
				MiniChampType.Encounter385,
				MiniChampType.Encounter386,
				MiniChampType.Encounter387,
				MiniChampType.Encounter388,
				MiniChampType.Encounter389,
				MiniChampType.Encounter390,
				MiniChampType.Encounter391,
				MiniChampType.Encounter392,
				MiniChampType.Encounter393,
				MiniChampType.Encounter394,
				MiniChampType.Encounter395,
				MiniChampType.Encounter396,
				MiniChampType.Encounter397,
				MiniChampType.Encounter398,
				MiniChampType.Encounter399,
				MiniChampType.Encounter400,
				MiniChampType.Encounter401,
				MiniChampType.Encounter402,
				MiniChampType.Encounter403,
				MiniChampType.Encounter404,
				MiniChampType.Encounter405,
				MiniChampType.Encounter406,
				MiniChampType.Encounter407,
				MiniChampType.Encounter408,
				MiniChampType.Encounter409,
				MiniChampType.Encounter410,
				MiniChampType.Encounter411,
				MiniChampType.Encounter412,
				MiniChampType.Encounter413,
				MiniChampType.Encounter414,
				MiniChampType.Encounter415,
				MiniChampType.Encounter416,
				MiniChampType.Encounter417,
				MiniChampType.Encounter418,
				MiniChampType.Encounter419,
				MiniChampType.Encounter420,
				MiniChampType.Encounter421,
				MiniChampType.Encounter422,
				MiniChampType.Encounter423,
				MiniChampType.Encounter424,
				MiniChampType.Encounter425,
				MiniChampType.Encounter426,
				MiniChampType.Encounter427,
				MiniChampType.Encounter428,
				MiniChampType.Encounter429,
				MiniChampType.Encounter430,
				MiniChampType.Encounter431,
				MiniChampType.Encounter432,
				MiniChampType.Encounter433,
				MiniChampType.Encounter434,
				MiniChampType.Encounter435,
				MiniChampType.Encounter436,
				MiniChampType.Encounter437,
				MiniChampType.Encounter438,
				MiniChampType.Encounter439,
				MiniChampType.Encounter440,
				MiniChampType.Encounter441,
				MiniChampType.Encounter442,
				MiniChampType.Encounter443,
				MiniChampType.Encounter444,
				MiniChampType.Encounter445,
				MiniChampType.Encounter446,
				MiniChampType.Encounter447,
				MiniChampType.Encounter448,
				MiniChampType.Encounter449,
				MiniChampType.Encounter450,
				MiniChampType.Encounter451,
				MiniChampType.Encounter452,
				MiniChampType.Encounter453,
				MiniChampType.Encounter454,
				MiniChampType.Encounter455,
				MiniChampType.Encounter456,
				MiniChampType.Encounter457,
				MiniChampType.Encounter458,
				MiniChampType.Encounter459,
				MiniChampType.Encounter460,
				MiniChampType.Encounter461,
				MiniChampType.Encounter462,
				MiniChampType.Encounter463,
				MiniChampType.Encounter464,
				MiniChampType.Encounter465,
				MiniChampType.Encounter466,
				MiniChampType.Encounter467,
				MiniChampType.Encounter468,
				MiniChampType.Encounter469,
				MiniChampType.Encounter470,
				MiniChampType.Encounter471,
				MiniChampType.Encounter472,
				MiniChampType.Encounter473,
				MiniChampType.Encounter474,
				MiniChampType.Encounter475,
				MiniChampType.Encounter476,
				MiniChampType.Encounter477,
				MiniChampType.Encounter478,
				MiniChampType.Encounter479,
				MiniChampType.Encounter480,
				MiniChampType.Encounter481,
				MiniChampType.Encounter482,
				MiniChampType.Encounter483,
				MiniChampType.Encounter484,
				MiniChampType.Encounter485,
				MiniChampType.Encounter486,
				MiniChampType.Encounter487,
				MiniChampType.Encounter488,
				MiniChampType.Encounter489,
				MiniChampType.Encounter490,
				MiniChampType.Encounter491,
				MiniChampType.Encounter492,
				MiniChampType.Encounter493,
				MiniChampType.Encounter494,
				MiniChampType.Encounter495,
				MiniChampType.Encounter496,
				MiniChampType.Encounter497,
				MiniChampType.Encounter498,
				MiniChampType.Encounter499,
				MiniChampType.Encounter500,
				MiniChampType.Encounter501,
				MiniChampType.Encounter502,
				MiniChampType.Encounter503,
				MiniChampType.Encounter504,
				MiniChampType.Encounter505,
				MiniChampType.Encounter506,
				MiniChampType.Encounter507,
				MiniChampType.Encounter508,
				MiniChampType.Encounter509,
				MiniChampType.Encounter510,
				MiniChampType.Encounter511,
				MiniChampType.Encounter512,
				MiniChampType.Encounter513,
				MiniChampType.Encounter514,
				MiniChampType.Encounter515,
				MiniChampType.Encounter516,
				MiniChampType.Encounter517,
				MiniChampType.Encounter518,
				MiniChampType.Encounter519,
				MiniChampType.Encounter520,
				MiniChampType.Encounter521,
				MiniChampType.Encounter522,
				MiniChampType.Encounter523,
				MiniChampType.Encounter524,
				MiniChampType.Encounter525,
				MiniChampType.Encounter526,
				MiniChampType.Encounter527,
				MiniChampType.Encounter528,
				MiniChampType.Encounter529,
				MiniChampType.Encounter530,
				MiniChampType.Encounter531,
				MiniChampType.Encounter532,
				MiniChampType.Encounter533,
				MiniChampType.Encounter534,
				MiniChampType.Encounter535,
				MiniChampType.Encounter536,
				MiniChampType.Encounter537,
				MiniChampType.Encounter538,
				MiniChampType.Encounter539,
				MiniChampType.Encounter540,
				MiniChampType.Encounter541,
				MiniChampType.Encounter542,
				MiniChampType.Encounter543,
				MiniChampType.Encounter544,
				MiniChampType.Encounter545,
				MiniChampType.Encounter546,
				MiniChampType.Encounter547,
				MiniChampType.Encounter548,
				MiniChampType.Encounter549,
				MiniChampType.Encounter550,
				MiniChampType.Encounter551,
				MiniChampType.Encounter552,
				MiniChampType.Encounter553,
				MiniChampType.Encounter554,
				MiniChampType.Encounter555,
				MiniChampType.Encounter556,
				MiniChampType.Encounter557,
				MiniChampType.Encounter558,
				MiniChampType.Encounter559,
				MiniChampType.Encounter560,
				MiniChampType.Encounter561,
				MiniChampType.Encounter562,
				MiniChampType.Encounter563,
				MiniChampType.Encounter564,
				MiniChampType.Encounter565,
				MiniChampType.Encounter566,
				MiniChampType.Encounter567,
				MiniChampType.Encounter568,
				MiniChampType.Encounter569,
				MiniChampType.Encounter570,
				MiniChampType.Encounter571,
				MiniChampType.Encounter572,
				MiniChampType.Encounter573,
				MiniChampType.Encounter574,
				MiniChampType.Encounter575,
				MiniChampType.Encounter576,
				MiniChampType.Encounter577,
				MiniChampType.Encounter578,
				MiniChampType.Encounter579,
				MiniChampType.Encounter580,
				MiniChampType.Encounter581,
				MiniChampType.Encounter582,
				MiniChampType.Encounter583,
				MiniChampType.Encounter584,
				MiniChampType.Encounter585,
				MiniChampType.Encounter586,
				MiniChampType.Encounter587,
				MiniChampType.Encounter588,
				MiniChampType.Encounter589,
				MiniChampType.Encounter590,
				MiniChampType.Encounter591,
				MiniChampType.Encounter592,
				MiniChampType.Encounter593,
				MiniChampType.Encounter594,
				MiniChampType.Encounter595,
				MiniChampType.Encounter596,
				MiniChampType.Encounter597,
				MiniChampType.Encounter598,
				MiniChampType.Encounter599,
				MiniChampType.Encounter600,
				MiniChampType.Encounter601,
				MiniChampType.Encounter602,
				MiniChampType.Encounter603,
				MiniChampType.Encounter604,
				MiniChampType.Encounter605,
				MiniChampType.Encounter606,
				MiniChampType.Encounter607,
				MiniChampType.Encounter608,
				MiniChampType.Encounter609,
				MiniChampType.Encounter610,
				MiniChampType.Encounter611,
				MiniChampType.Encounter612,
				MiniChampType.Encounter613,
				MiniChampType.Encounter614,
				MiniChampType.Encounter615,
				MiniChampType.Encounter616,
				MiniChampType.Encounter617,
				MiniChampType.Encounter618,
				MiniChampType.Encounter619,
				MiniChampType.Encounter620,
				MiniChampType.Encounter621,
				MiniChampType.Encounter622,
				MiniChampType.Encounter623,
				MiniChampType.Encounter624,
				MiniChampType.Encounter625,
				MiniChampType.Encounter626,
				MiniChampType.Encounter627,
				MiniChampType.Encounter628,
				MiniChampType.Encounter629,
				MiniChampType.Encounter630,
				MiniChampType.Encounter631,
				MiniChampType.Encounter632,
				MiniChampType.Encounter633,
				MiniChampType.Encounter634,
				MiniChampType.Encounter635,
				MiniChampType.Encounter636,
				MiniChampType.Encounter637,
				MiniChampType.Encounter638,
				MiniChampType.Encounter639,
				MiniChampType.Encounter640,
				MiniChampType.Encounter641,
				MiniChampType.Encounter642,
				MiniChampType.Encounter643,
				MiniChampType.Encounter644,
				MiniChampType.Encounter645,
				MiniChampType.Encounter646,
				MiniChampType.Encounter647,
				MiniChampType.Encounter648,
				MiniChampType.Encounter649,
				MiniChampType.Encounter650,
				MiniChampType.Encounter651,
				MiniChampType.Encounter652,
				MiniChampType.Encounter653,
				MiniChampType.Encounter654,
				MiniChampType.Encounter655,
				MiniChampType.Encounter656,
				MiniChampType.Encounter657,
				MiniChampType.Encounter658,
				MiniChampType.Encounter659,
				MiniChampType.Encounter660,
				MiniChampType.Encounter661,
				MiniChampType.Encounter662,
				MiniChampType.Encounter663,
				MiniChampType.Encounter664,
				MiniChampType.Encounter665,
				MiniChampType.Encounter666,
				MiniChampType.Encounter667,
				MiniChampType.Encounter668,
				MiniChampType.Encounter669,
				MiniChampType.Encounter670,
				MiniChampType.Encounter671,
				MiniChampType.Encounter672,
				MiniChampType.Encounter673,
				MiniChampType.Encounter674,
				MiniChampType.Encounter675,
				MiniChampType.Encounter676,
				MiniChampType.Encounter677,
				MiniChampType.Encounter678,
				MiniChampType.Encounter679,
				MiniChampType.Encounter680,
				MiniChampType.Encounter681,
				MiniChampType.Encounter682,
				MiniChampType.Encounter683,
				MiniChampType.Encounter684,
				MiniChampType.Encounter685,
				MiniChampType.Encounter686,
				MiniChampType.Encounter687,
				MiniChampType.Encounter688,
				MiniChampType.Encounter689,
				MiniChampType.Encounter690,
				MiniChampType.Encounter691,
				MiniChampType.Encounter692,
				MiniChampType.Encounter693,
				MiniChampType.Encounter694,
				MiniChampType.Encounter695,
				MiniChampType.Encounter696,
				MiniChampType.Encounter697,
				MiniChampType.Encounter698,
				MiniChampType.Encounter699,
				MiniChampType.Encounter700,
				MiniChampType.Encounter701,
				MiniChampType.Encounter702,
				MiniChampType.Encounter703,
				MiniChampType.Encounter704,
				MiniChampType.Encounter705,
				MiniChampType.Encounter706,
				MiniChampType.Encounter707,
				MiniChampType.Encounter708,
				MiniChampType.Encounter709,
				MiniChampType.Encounter710,
				MiniChampType.Encounter711,
				MiniChampType.Encounter712,
				MiniChampType.Encounter713,
				MiniChampType.Encounter714,
				MiniChampType.Encounter715,
				MiniChampType.Encounter716,
				MiniChampType.Encounter717,
				MiniChampType.Encounter718,
				MiniChampType.Encounter719,
				MiniChampType.Encounter720,
				MiniChampType.Encounter721,
				MiniChampType.Encounter722,
				MiniChampType.Encounter723,
				MiniChampType.Encounter724,
				MiniChampType.Encounter725,
				MiniChampType.Encounter726,
				MiniChampType.Encounter727,
				MiniChampType.Encounter728,
				MiniChampType.Encounter729,
				MiniChampType.Encounter730,
				MiniChampType.Encounter731,
				MiniChampType.Encounter732,
				MiniChampType.Encounter733,
				MiniChampType.Encounter734,
				MiniChampType.Encounter735,
				MiniChampType.Encounter736,
				MiniChampType.Encounter737,
				MiniChampType.Encounter738,
				MiniChampType.Encounter739,
				MiniChampType.Encounter740,
				MiniChampType.Encounter741,
				MiniChampType.Encounter742,
				MiniChampType.Encounter743,
				MiniChampType.Encounter744,
				MiniChampType.Encounter745,
				MiniChampType.Encounter746,
				MiniChampType.Encounter747,
				MiniChampType.Encounter748,
				MiniChampType.Encounter749,
				MiniChampType.Encounter750,
				MiniChampType.Encounter751,
				MiniChampType.Encounter752,
				MiniChampType.Encounter753,
				MiniChampType.Encounter754,
				MiniChampType.Encounter755,
				MiniChampType.MeraktusTheTormented
			};

			// Create mini champs at each location with random types
			Random random = new Random();
			foreach (var (location, map, spawnRange) in locations)
			{
				try
				{
					MiniChampType randomType = types[random.Next(types.Count)];
					if (randomType == null)
					{
						e.Mobile.SendMessage($"Skipping spawn at {location} due to null MiniChampType.");
						continue;
					}
					CreateMiniChampOnFacet(randomType, spawnRange, location, map);
				}
				catch (Exception ex)
				{
					e.Mobile.SendMessage($"Error creating MiniChamp at {location}: {ex.Message}");
				}
			}

			e.Mobile.SendMessage("Generated Mini Champions at all predefined locations with random types.");
		}


        private bool m_Active;
        private MiniChampType m_Type;
        private List<MiniChampSpawnInfo> Spawn;
        public List<Mobile> Despawns;
        private int m_Level;
        private int m_SpawnRange;
        private TimeSpan m_RestartDelay;
        private Timer m_Timer, m_RestartTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D BossSpawnPoint { get; set; }        

        [Constructable]
        public MiniChamp()
            : base(0xBD2)
        {
            Movable = false;
            Visible = false;
            Name = "Mini Champion Controller";

            Despawns = new List<Mobile>();
            Spawn = new List<MiniChampSpawnInfo>();
            m_RestartDelay = TimeSpan.FromMinutes(5.0);
            m_SpawnRange = 30;
            BossSpawnPoint = Point3D.Zero;

            Controllers.Add(this);
        }

		/// <summary>
		/// Fired whenever this controller actually puts a BaseCreature into the world.
		/// </summary>
		public event Action<BaseCreature> CreatureSpawned;

		/// <summary>
		/// Call this as soon as you MoveToWorld() a new creature.
		/// </summary>
		internal void NotifyCreatureSpawned(BaseCreature mob)
		{
			try { CreatureSpawned?.Invoke(mob); }
			catch { /* swallow exceptions so the champ loop can’t break */ }
		}


        [CommandProperty(AccessLevel.GameMaster)]
        public int SpawnRange
        {
            get { return m_SpawnRange; }
            set { m_SpawnRange = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RestartDelay
        {
            get { return m_RestartDelay; }
            set { m_RestartDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public MiniChampType Type
        {
            get { return m_Type; }
            set { m_Type = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (value)
                    Start();
                else
                    Stop();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; InvalidateProperties(); }
        }

        public void Start()
        {
            if (m_Active || Deleted)
                return;

            m_Active = true;

            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = new SliceTimer(this);
            m_Timer.Start();

            if (m_RestartTimer != null)
                m_RestartTimer.Stop();

            m_RestartTimer = null;

            AdvanceLevel();
            InvalidateProperties();
        }

        public void Stop()
        {
            if (!m_Active || Deleted)
                return;

            m_Active = false;
            m_Level = 0;

            ClearSpawn();
            Despawn();

            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;

            if (m_RestartTimer != null)
                m_RestartTimer.Stop();

            m_RestartTimer = null;
            InvalidateProperties();
        }

        public void Despawn()
        {
            foreach (var toDespawn in Despawns)
            {
                toDespawn.Delete();
            }

            Despawns.Clear();
        }

        public void OnSlice()
        {
            if (!m_Active || Deleted)
                return;

            bool changed = false;
            bool done = true;

            foreach (var spawn in Spawn)
            {
                if (spawn.Slice() && !changed)
                {
                    changed = true;
                }

                if (!spawn.Done && done)
                {
                    done = false;
                }
            }

            if (done)
            {
                AdvanceLevel();
            }

            if (m_Active)
            {
                foreach (var spawn in Spawn)
                {
                    if (spawn.Respawn() && !changed)
                    {
                        changed = true;
                    }
                }
            }

            if (done || changed)
            {
                InvalidateProperties();
            }
        }

        public void ClearSpawn()
        {
            foreach (var spawn in Spawn)
            {
                foreach (var creature in spawn.Creatures)
                {
                    Despawns.Add(creature);
                }
            }

            Spawn.Clear();
        }

        public void AdvanceLevel()
        {
            Level++;

            MiniChampInfo info = MiniChampInfo.GetInfo(m_Type);
            MiniChampLevelInfo levelInfo = info.GetLevelInfo(Level);

            if (levelInfo != null && Level <= info.MaxLevel)
            {
                ClearSpawn();

                if (m_Type == MiniChampType.MeraktusTheTormented)
                {
                    MinotaurShouts();
                }

                foreach(var type in levelInfo.Types)
                {
                    Spawn.Add(new MiniChampSpawnInfo(this, type));
                }
            }
            else // begin restart
            {
                Stop();

                m_RestartTimer = Timer.DelayCall(m_RestartDelay, new TimerCallback(Start));
            }
        }

        private void MinotaurShouts()
        {
            int cliloc = 0;

            switch (Level)
            {
                case 1:
                    return;
                case 2:
                    cliloc = 1073370;
                    break;
                case 3:
                    cliloc = 1073367;
                    break;
                case 4:
                    cliloc = 1073368;
                    break;
                case 5:
                    cliloc = 1073369;
                    break;
            }

            IPooledEnumerable eable = GetMobilesInRange(m_SpawnRange);

            foreach(Mobile x in eable)
            {
                if (x is PlayerMobile)
                    x.SendLocalizedMessage(cliloc);
            }

            eable.Free();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060658, "Type\t{0}", m_Type); // ~1_val~: ~2_val~
            list.Add(1060661, "Spawn Range\t{0}", m_SpawnRange); // ~1_val~: ~2_val~

            if (m_Active)
            {
                MiniChampInfo info = MiniChampInfo.GetInfo(m_Type);

                list.Add(1060742); // active
                list.Add("Level {0} / {1}", Level, info != null ? info.MaxLevel.ToString() : "???"); // ~1_val~: ~2_val~

                for (int i = 0; i < Spawn.Count; i++)
                {
                    Spawn[i].AddProperties(list, i + 1150301);
                }
            }
            else
            {
                list.Add(1060743); // inactive
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new PropertiesGump(from, this));
        }

        public override void OnDelete()
        {
            Controllers.Remove(this);
            Stop();

            base.OnDelete();
        }

        public MiniChamp(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(BossSpawnPoint);
            writer.Write((bool)m_Active);
            writer.Write((int)m_Type);
            writer.Write((int)m_Level);
            writer.Write((int)m_SpawnRange);
            writer.Write((TimeSpan)m_RestartDelay);

            writer.Write((int)Spawn.Count);

            for (int i = 0; i < Spawn.Count; i++)
            {
                Spawn[i].Serialize(writer);
            }

            writer.Write(Despawns, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        Spawn = new List<MiniChampSpawnInfo>();

                        BossSpawnPoint = reader.ReadPoint3D();
                        m_Active = reader.ReadBool();
                        m_Type = (MiniChampType)reader.ReadInt();
                        m_Level = reader.ReadInt();
                        m_SpawnRange = reader.ReadInt();
                        m_RestartDelay = reader.ReadTimeSpan();

                        int spawnCount = reader.ReadInt();

                        for (int i = 0; i < spawnCount; i++)
                        {
                            Spawn.Add(new MiniChampSpawnInfo(reader));
                        }

                        Despawns = reader.ReadStrongMobileList();

                        if (m_Active)
                        {
                            m_Timer = new SliceTimer(this);
                            m_Timer.Start();
                        }
                        else
                        {
                            m_RestartTimer = Timer.DelayCall(m_RestartDelay, new TimerCallback(Start));
                        }

                        break;
                    }
            }

            Controllers.Add(this);
        }
    }

    public class SliceTimer : Timer
    {
        private MiniChamp m_Controller;

        public SliceTimer(MiniChamp controller)
            : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            m_Controller = controller;
        }

        protected override void OnTick()
        {
            m_Controller.OnSlice();
        }
    }
}
