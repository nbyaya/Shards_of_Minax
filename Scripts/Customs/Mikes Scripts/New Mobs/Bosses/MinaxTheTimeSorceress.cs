using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells.Necromancy;
using Server.Network;
using Server.Engines.CannedEvil;

namespace Server.Mobiles
{
    public class MinaxTheTimeSorceress : BaseChampion
    {
		public override ChampionSkullType SkullType{ get{ return ChampionSkullType.Pain; } }

		public override Type[] UniqueList{ get{ return new Type[] { typeof( GladiatorsCollar ) }; } }
		public override Type[] SharedList { get { return new Type[] { typeof(RoyalGuardSurvivalKnife), typeof(ANecromancerShroud), typeof(LieutenantOfTheBritannianRoyalGuard) }; } }
		public override Type[] DecorativeList{ get{ return new Type[] { typeof( LavaTile ), typeof( DemonSkull ) }; } }

		public override MonsterStatuetteType[] StatueTypes{ get{ return new MonsterStatuetteType[] { }; } }        
		
		// Each teleport destination now includes a specific facet.
        private static readonly List<Tuple<Point3D, Map>> TeleportDestinations = new List<Tuple<Point3D, Map>>
        {
            new Tuple<Point3D, Map>( new Point3D(1000, 1000, 0), Map.Felucca ),    // Felucca location
            new Tuple<Point3D, Map>( new Point3D(1000, 1000, 0), Map.Trammel ),     // Trammel location
            new Tuple<Point3D, Map>( new Point3D(1000, 1000, 0), Map.Ilshenar ),     // Ilshenar location
            // Add more tuples as needed.
            new Tuple<Point3D, Map>( new Point3D(3340, 572, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3398, 335, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3239, 367, 4), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3008, 268, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2866, 197, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2874, 455, 10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2886, 91, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2665, 120, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2608, 128, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2431, 94, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2416, 176, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2462, 54, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2327, 67, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2177, 305, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2079, 156, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1788, 253, 6), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1976, 267, 8), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1934, 318, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1901, 366, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1987, 389, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1862, 604, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2071, 640, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1549, 729, 27), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1641, 972, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1511, 1086, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1426, 992, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1369, 835, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1258, 611, 24), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1275, 529, 30), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1482, 238, 23), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1190, 142, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1051, 365, 3), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(824, 375, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(957, 706, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(954, 707, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(963, 717, -20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1004, 766, -60), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1029, 1014, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1030, 1360, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(744, 1359, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(518, 1420, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(355, 1344, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(158, 1363, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(142, 1490, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(362, 1441, 1), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(359, 1467, 1), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(386, 1654, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(659, 1689, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(772, 1682, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(833, 1831, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(851, 2070, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1000, 2239, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1074, 2204, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1076, 2262, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1144, 2224, 27), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1216, 2201, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1219, 2268, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1102, 2482, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1060, 2718, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1115, 2654, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1124, 2824, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1166, 2957, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1369, 2712, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1362, 2891, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1359, 3118, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1456, 3134, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1588, 3202, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1608, 3372, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1651, 3548, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1726, 3488, 2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1927, 3419, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2019, 3382, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2159, 3355, 10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2160, 3053, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2107, 3000, 30), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1881, 3256, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1846, 3204, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1736, 3091, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1675, 2972, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1966, 2937, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2119, 2734, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1705, 2692, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1668, 2486, 10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1822, 2414, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2023, 2316, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2104, 2103, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1788, 2170, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1642, 2368, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1614, 2116, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1473, 2159, 6), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1386, 2009, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1350, 1499, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1378, 1319, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1445, 1227, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1761, 1176, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1722, 1065, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1731, 916, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1708, 1428, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1981, 1507, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1992, 1010, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1809, 1031, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1828, 1030, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2037, 970, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1951, 888, -1), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1873, 914, 3), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1803, 854, -1), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1867, 741, 6), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1576, 828, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1254, 869, 16), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1229, 936, 10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2056, 624, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2241, 843, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2325, 820, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2359, 806, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2181, 1302, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2187, 1416, -2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2582, 1120, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2739, 1061, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2745, 858, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2636, 720, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2663, 469, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3920, 196, -10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3959, 358, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4051, 562, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4050, 439, 3), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4229, 508, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2504, 3609, 3), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2376, 3411, 3), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2462, 3932, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2151, 3930, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1144, 3510, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1062, 3131, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1383, 1625, 30), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1313, 1656, 30), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1523, 1445, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1527, 1428, 55), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1544, 1598, 12), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1562, 1741, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1483, 1754, -2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1343, 1733, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1144, 1656, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2647, 2054, -20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2605, 2101, -20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2285, 1209, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2210, 1115, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2437, 1099, 8), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1398, 3744, -21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1448, 3787, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1423, 4011, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1147, 3744, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1280, 3734, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3678, 2293, -2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3681, 2238, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3720, 2066, 12), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2537, 501, 31), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2466, 529, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2584, 528, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2503, 554, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2539, 664, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4404, 1045, -2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4492, 985, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4327, 983, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4395, 1163, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4417, 1218, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4529, 1378, 23), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(4383, 1414, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3803, 1279, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3734, 1315, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3741, 1229, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3618, 1234, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3555, 1187, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3675, 1155, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3650, 2650, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3635, 2547, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3410, 2709, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2948, 3356, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3027, 3352, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3063, 3342, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2958, 3448, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2887, 3415, 35), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2878, 3473, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2958, 3487, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3014, 3529, 15), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(810, 2218, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(790, 2278, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(610, 2275, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(620, 2127, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(560, 2147, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2067, 2856, -2), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2021, 2890, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1951, 2815, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1956, 2750, 10), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1891, 2716, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1843, 2736, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1825, 2676, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1807, 2709, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(1866, 2645, 40), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2745, 858, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2779, 966, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2814, 955, 21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2874, 915, 21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2912, 933, 21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2955, 847, 21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2972, 738, 21), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(2907, 636, 16), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(5249, 68, 14), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(5212, 137, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(5190, 103, 5), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(5164, 21, 27), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(633, 1502, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(675, 1204, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(627, 1037, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(534, 1063, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(552, 966, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(519, 1007, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(388, 971, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(377, 905, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(355, 880, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(338, 817, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(311, 787, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(274, 771, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(280, 771, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(635, 821, 20), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(610, 816, 0), Map.Felucca ),
            new Tuple<Point3D, Map>( new Point3D(3340, 572, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3398, 335, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3239, 367, 4), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3008, 268, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2866, 197, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2874, 455, 10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2886, 91, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2665, 120, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2608, 128, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2431, 94, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2416, 176, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2462, 54, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2327, 67, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2177, 305, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2079, 156, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1788, 253, 6), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1976, 267, 8), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1934, 318, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1901, 366, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1987, 389, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1862, 604, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2071, 640, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1549, 729, 27), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1641, 972, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1511, 1086, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1426, 992, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1369, 835, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1258, 611, 24), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1275, 529, 30), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1482, 238, 23), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1190, 142, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1051, 365, 3), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(824, 375, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(957, 706, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(954, 707, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(963, 717, -20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1004, 766, -60), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1029, 1014, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1030, 1360, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(744, 1359, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(518, 1420, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(355, 1344, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(158, 1363, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(142, 1490, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(362, 1441, 1), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(359, 1467, 1), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(386, 1654, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(659, 1689, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(772, 1682, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(833, 1831, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(851, 2070, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1000, 2239, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1074, 2204, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1076, 2262, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1144, 2224, 27), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1216, 2201, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1219, 2268, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1102, 2482, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1060, 2718, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1115, 2654, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1124, 2824, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1166, 2957, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1369, 2712, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1362, 2891, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1359, 3118, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1456, 3134, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1588, 3202, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1608, 3372, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1651, 3548, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1726, 3488, 2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1927, 3419, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2019, 3382, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2159, 3355, 10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2160, 3053, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2107, 3000, 30), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1881, 3256, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1846, 3204, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1736, 3091, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1675, 2972, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1966, 2937, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2119, 2734, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1705, 2692, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1668, 2486, 10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1822, 2414, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2023, 2316, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2104, 2103, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1788, 2170, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1642, 2368, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1614, 2116, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1473, 2159, 6), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1386, 2009, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1350, 1499, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1378, 1319, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1445, 1227, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1761, 1176, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1722, 1065, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1731, 916, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1708, 1428, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1981, 1507, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1992, 1010, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1809, 1031, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1828, 1030, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2037, 970, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1951, 888, -1), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1873, 914, 3), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1803, 854, -1), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1867, 741, 6), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1576, 828, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1254, 869, 16), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1229, 936, 10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2056, 624, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2241, 843, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2325, 820, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2359, 806, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2181, 1302, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2187, 1416, -2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2582, 1120, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2739, 1061, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2745, 858, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2636, 720, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2663, 469, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3920, 196, -10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3959, 358, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4051, 562, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4050, 439, 3), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4229, 508, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2504, 3609, 3), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2376, 3411, 3), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2462, 3932, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2151, 3930, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1144, 3510, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1062, 3131, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1383, 1625, 30), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1313, 1656, 30), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1523, 1445, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1527, 1428, 55), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1544, 1598, 12), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1562, 1741, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1483, 1754, -2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1343, 1733, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1144, 1656, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2647, 2054, -20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2605, 2101, -20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2285, 1209, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2210, 1115, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2437, 1099, 8), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1398, 3744, -21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1448, 3787, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1423, 4011, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1147, 3744, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1280, 3734, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3678, 2293, -2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3681, 2238, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3720, 2066, 12), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2537, 501, 31), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2466, 529, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2584, 528, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2503, 554, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2539, 664, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4404, 1045, -2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4492, 985, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4327, 983, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4395, 1163, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4417, 1218, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4529, 1378, 23), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(4383, 1414, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3803, 1279, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3734, 1315, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3741, 1229, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3618, 1234, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3555, 1187, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3675, 1155, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3650, 2650, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3635, 2547, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3410, 2709, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2948, 3356, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3027, 3352, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3063, 3342, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2958, 3448, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2887, 3415, 35), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2878, 3473, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2958, 3487, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(3014, 3529, 15), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(810, 2218, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(790, 2278, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(610, 2275, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(620, 2127, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(560, 2147, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2067, 2856, -2), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2021, 2890, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1951, 2815, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1956, 2750, 10), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1891, 2716, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1843, 2736, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1825, 2676, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1807, 2709, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(1866, 2645, 40), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2745, 858, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2779, 966, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2814, 955, 21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2874, 915, 21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2912, 933, 21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2955, 847, 21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2972, 738, 21), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(2907, 636, 16), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(5249, 68, 14), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(5212, 137, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(5190, 103, 5), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(5164, 21, 27), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(633, 1502, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(675, 1204, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(627, 1037, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(534, 1063, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(552, 966, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(519, 1007, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(388, 971, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(377, 905, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(355, 880, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(338, 817, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(311, 787, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(274, 771, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(280, 771, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(635, 821, 20), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(610, 816, 0), Map.Trammel ),
            new Tuple<Point3D, Map>( new Point3D(974, 161, -10), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(987, 328, 11), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(915, 501, -11), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(950, 552, -13), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(980, 491, -11), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(578, 799, -45), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(887, 273, 4), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(546, 760, -91), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(530, 658, 9), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(578, 900, -72), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(684, 579, -14), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(434, 701, 29), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1031, 3660, 60), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1021, 4003, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1172, 3642, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1158, 3521, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1101, 3535, 15), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1073, 3540, -44), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1142, 3475, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1109, 3432, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1166, 3334, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1144, 3270, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1126, 3178, -43), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1059, 3173, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(992, 3194, 54), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1006, 3096, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1050, 3061, 99), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(943, 3034, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(847, 2976, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(790, 2944, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(761, 3018, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(814, 3078, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(699, 3078, 59), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(665, 3039, 37), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(650, 2992, 61), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(602, 3008, 36), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(564, 3061, 97), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(544, 3119, 49), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(570, 2945, 39), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(522, 2962, 20), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(393, 3056, 36), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(442, 3166, 32), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(383, 3237, 17), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(334, 3273, 16), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(370, 3286, -3), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(385, 3341, 37), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(557, 3382, 37), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(564, 3449, 98), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(591, 3531, 37), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(540, 3603, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(453, 3578, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(388, 3525, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(316, 3584, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(396, 3629, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(468, 3623, 38), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(519, 3720, -43), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(574, 3767, -33), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(555, 3882, -33), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(501, 3923, -42), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(442, 3848, -36), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(745, 3708, -35), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(700, 3741, 17), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(624, 3953, -43), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(586, 3977, -43), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1022, 3580, -5), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1032, 3553, -5), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(677, 824, -108), Map.TerMur ),
            new Tuple<Point3D, Map>( new Point3D(1106, 418, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(284, 341, -59), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(489, 350, -59), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(642, 310, -43), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(772, 348, -43), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(630, 493, -75), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(512, 545, -60), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(290, 538, -22), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(654, 749, -26), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(466, 859, -76), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(429, 950, -84), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(328, 1100, -57), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(365, 1141, -57), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(450, 1068, -85), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(650, 934, -62), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(640, 1035, -83), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(648, 1143, -72), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(764, 1039, -30), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(778, 1138, -30), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(830, 1186, -67), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(758, 1311, -94), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(644, 1321, -57), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(557, 1377, 17), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(533, 1347, -53), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(509, 1317, -53), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(459, 1300, -55), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(411, 1364, -18), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(354, 1359, -25), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(292, 1347, -24), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(299, 1306, -25), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(385, 1272, -38), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(331, 1205, -38), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(356, 1141, -54), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(910, 1191, 12), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(932, 1131, 12), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(947, 1062, -13), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(939, 950, -30), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1035, 875, -28), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(963, 783, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(891, 778, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(849, 777, 0), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(814, 778, -60), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(964, 650, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(977, 582, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(972, 525, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(912, 518, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(876, 526, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(994, 493, -79), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1044, 451, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1107, 403, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1087, 496, -73), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1059, 530, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1061, 560, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1054, 624, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1112, 637, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1082, 714, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1125, 739, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1181, 744, -80), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1351, 661, 108), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1372, 626, 105), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1438, 643, -4), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1471, 571, 1), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1386, 502, -10), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1369, 419, -21), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1308, 421, 37), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1290, 280, -26), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1148, 345, 70), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1047, 273, 56), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(991, 271, 56), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(914, 304, 31), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(814, 304, 7), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1365, 270, 37), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1505, 235, -17), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1622, 260, 75), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1624, 309, 48), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1644, 336, 21), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1676, 301, 78), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1702, 291, 83), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1657, 254, 78), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1794, 385, 29), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1797, 431, -14), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1814, 465, 8), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1857, 521, -14), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1745, 531, 24), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1704, 570, 15), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1736, 604, 37), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1828, 623, -13), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1530, 669, -14), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1655, 881, -26), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1674, 980, 10), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1647, 1037, -24), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1682, 1076, -11), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1467, 1230, -16), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1234, 1277, -10), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1198, 1212, -19), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1133, 1233, -5), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1107, 1173, -20), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1056, 1153, -24), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1117, 1038, -32), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1253, 959, -15), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1246, 978, -34), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1180, 976, -30), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(1296, 1011, 3), Map.Ilshenar ),
            new Tuple<Point3D, Map>( new Point3D(527, 1225, 25), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(548, 1221, 25), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(588, 1130, 36), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(618, 1116, 34), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(753, 1079, 23), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(745, 1011, 33), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(719, 868, 33), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(581, 927, 24), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(542, 943, 29), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(548, 883, 21), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(519, 857, 2), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(654, 878, 29), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(750, 871, 32), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(923, 818, 29), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(998, 788, 4), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(957, 694, 15), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(917, 637, 15), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(897, 600, 38), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(824, 736, 24), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(876, 698, 4), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1135, 900, 62), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1119, 1078, 32), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1214, 1098, 28), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1192, 1060, 21), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1210, 938, 33), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1263, 880, 3), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1233, 764, 23), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1175, 801, 27), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1232, 657, 73), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1266, 577, 28), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1227, 529, 29), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1198, 489, 23), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1104, 551, 23), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(987, 568, 10), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(1089, 412, 6), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(910, 418, 14), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(979, 299, 25), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(952, 257, 19), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(847, 316, 38), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(819, 286, 35), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(798, 373, 26), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(754, 185, 38), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(961, 145, 44), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(920, 60, 37), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(636, 388, 42), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(663, 426, 32), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(592, 361, 23), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(541, 426, 27), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(470, 432, 30), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(442, 376, 32), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(406, 595, 29), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(353, 545, 15), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(196, 632, 46), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(190, 534, 41), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(199, 731, 32), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(319, 705, 58), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(230, 793, 57), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(191, 916, 35), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(272, 1142, 11), Map.Tokuno ),
            new Tuple<Point3D, Map>( new Point3D(372, 1047, 11), Map.Tokuno )

			
        };

        // Champion monsters to spawn (with a rare chance to duplicate Minax)
        private static readonly Type[] ChampionSpawnList = new Type[]
        {
            typeof(Semidar),
            typeof(Rikktor),
            typeof(Mephitis),
            typeof(Neira),
            typeof(LordOaks),
            typeof(Serado),
            typeof(Barracoon),
            typeof(MinaxTheTimeSorceress)
        };

        private Timer m_TeleportTimer;
        private Timer m_TauntTimer;
        private Timer m_SpecialAttackTimer;
        private Timer m_CombatTimer;

        [Constructable]
        public MinaxTheTimeSorceress() : base(AIType.AI_Mage)
        {
            Name = "Minax the Time Sorceress";
            Title = "Mistress of Temporal Chaos";
            Body = 401;   // Female human model
            Hue = Utility.RandomMinMax(1, 2999);   // Unique hue
			
            Utility.AssignRandomHair(this);

            HairHue = Utility.RandomMinMax(1, 2999);   // Unique hue			

            SetStr(1000, 1200);
            SetDex(200, 255);
            SetInt(200, 250);

            SetHits(20000);
            SetStam(102, 300);
            SetMana(505, 750);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 50000;
            Karma = -50000;
            VirtualArmor = 100;
			
			AddItem(new ClockworkAmulet());
			AddItem(new MinaxFemaleStuddedChest());
			AddItem(new MinaxLeatherArms());
			AddItem(new MinaxLeatherGloves());
			AddItem(new MinaxStandardPlateKabuto());
			AddItem(new MinaxThighBoots());
			AddItem(new MinaxCloak());

            PackReg(50);
            PackNecroReg(30, 300);

            // Set up recurring timers:
            m_TeleportTimer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromHours(1), new TimerCallback(Teleport));
            m_TauntTimer = Timer.DelayCall(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), new TimerCallback(Taunt));
            m_SpecialAttackTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15), new TimerCallback(CombatActions));
        }

        public override void OnThink()
        {
            base.OnThink();
            if (Combatant != null && (m_CombatTimer == null || !m_CombatTimer.Running))
            {
                m_CombatTimer = Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), new TimerCallback(CombatActions));
            }
        }

        private void CombatActions()
        {
            if (this.Deleted || Combatant == null)
                return;

            // Randomly select one of several flashy attacks.
            int choice = Utility.Random(7);
            switch (choice)
            {
                case 0: TimeFreezeAttack(); break;
                case 1: TemporalRift(); break;
                case 2: ChronoNova(); break;
                case 3: ParadoxClone(); break;
                case 4: TimeReverse(); break;
                case 5: TimeDistortionAttack(); break;
                case 6: CastFlashySpell(); break;
            }
        }

        // Each attack emits location effects to enhance the visual spectacle.
        private void TimeFreezeAttack()
        {
            Say("Witness the frozen moment!");
            // Send a burst of effects in a 3-tile radius.
            SendAreaEffect(3, 0x3709, 16, 10, 1150);
            // Cast Freeze on the combatant after casting; cast on Mobile
            Mobile targ = Combatant as Mobile;
            if (targ != null)
                targ.Freeze(TimeSpan.FromSeconds(3));
            if (targ != null)
                targ.Damage(Utility.RandomMinMax(50, 100), this);
        }

        private void TemporalRift()
        {
            Say("The fabric of time tears!");
            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m is PlayerMobile && CanBeHarmful(m))
                    targets.Add(m);
            }
            // Create a swirling rift effect at each target's location.
            foreach (Mobile m in targets)
            {
                Effects.SendLocationEffect(m.Location, m.Map, 0x36BD, 16, 10, 2543, 0);
                m.SendMessage("You're caught in a temporal rift!");
                m.Damage(Utility.Random(50, 100), this);
            }
        }

        private void ChronoNova()
        {
            Say("Time itself bends to my will!");
            // A radiant nova of time energy bursts around Minax.
            Effects.SendLocationEffect(this.Location, this.Map, 0x3728, 20, 10, 1175, 0);
            this.PlaySound(0x20F);
            this.Hits += 5000;
            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m is PlayerMobile && CanBeHarmful(m))
                {
                    // Flash an effect at each target before dealing damage.
                    Effects.SendLocationEffect(m.Location, m.Map, 0x36BD, 16, 10, 1175, 0);
                    m.Damage(Utility.Random(75, 125), this);
                }
            }
        }

        private void ParadoxClone()
        {
            Say("Which is the real me?");
            // Create clones with a burst effect at their spawn location.
            for (int i = 0; i < 3; i++)
            {
                MinaxClone clone = new MinaxClone(this);
                clone.MoveToWorld(this.Location, this.Map);
                Effects.SendLocationEffect(clone.Location, clone.Map, 0x3709, 16, 10, 1150, 0);
            }
        }

        private void TimeReverse()
        {
            Say("Let's try that again!");
            foreach (Mobile m in this.GetMobilesInRange(12))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 16, 10, 1175, 0);
                    m.SendMessage("Your timeline is reversed!");
                    int damage = m.Hits / 2;
                    m.Damage(damage, this);
                    this.Hits += damage;
                }
            }
        }

        private void TimeDistortionAttack()
        {
            Mobile targ = Combatant as Mobile;
            if (targ != null && targ.Alive)
            {
                Say("*Feel the wrath of time itself!*");
                Effects.SendLocationEffect(targ.Location, targ.Map, 0x3728, 20, 10, 2543, 0);
                targ.Freeze(TimeSpan.FromSeconds(3));
                targ.SendMessage("Time slows around you!");
                targ.Damage(Utility.RandomMinMax(50, 100), this);
            }
        }

        private void CastFlashySpell()
        {
            // Choose one of several flashy spells.
            List<Spell> spells = new List<Spell>
            {
                new MeteorSwarmSpell(this, null),
                new ChainLightningSpell(this, null)
                // Removed EnergyVortexSpell as it's not defined.
            };
            Spell spell = spells[Utility.Random(spells.Count)];
            Effects.SendLocationEffect(this.Location, this.Map, 0x36BD, 16, 10, 1175, 0);
            spell.Cast();
        }

        // Helper method to send an area effect in a radius around this creature.
        private void SendAreaEffect(int radius, int effectID, int speed, int duration, int hue)
        {
            Map map = this.Map;
            if (map == null)
                return;
            for (int x = this.X - radius; x <= this.X + radius; x++)
            {
                for (int y = this.Y - radius; y <= this.Y + radius; y++)
                {
                    Point3D p = new Point3D(x, y, this.Z);
                    if (map.CanFit(x, y, this.Z, 16, false, false))
                        Effects.SendLocationEffect(p, map, effectID, speed, duration, hue, 0);
                    else
                    {
                        int z = map.GetAverageZ(x, y);
                        if (map.CanFit(x, y, z, 16, false, false))
                            Effects.SendLocationEffect(new Point3D(x, y, z), map, effectID, speed, duration, hue, 0);
                    }
                }
            }
        }

        // Teleportation – using a predefined tuple (location + facet)
        public void Teleport()
        {
            World.Broadcast(0x482, false, "Minax laughs: 'You'll never catch me! Time moves on!'");
            var destination = TeleportDestinations[Utility.Random(TeleportDestinations.Count)];
            Point3D newLocation = destination.Item1;
            Map newMap = destination.Item2;
            
            Effects.SendLocationEffect(this.Location, this.Map, 0x3709, 16, 10, 1175, 0);
            this.MoveToWorld(newLocation, newMap);
            World.Broadcast(0x482, false, $"Minax reappears at {newLocation.X}, {newLocation.Y} in {newMap}!");
            Effects.SendLocationEffect(newLocation, newMap, 0x3709, 16, 10, 1175, 0);
            SpawnChampions(newLocation, newMap);
        }

        private void SpawnChampions(Point3D location, Map map)
        {
            int spawnCount = Utility.RandomMinMax(4, 8);
            for (int i = 0; i < spawnCount; i++)
            {
                Type championType = ChampionSpawnList[Utility.Random(ChampionSpawnList.Length)];
                if (championType == typeof(MinaxTheTimeSorceress))
                {
                    if (Utility.RandomDouble() > 0.05)
                    {
                        championType = ChampionSpawnList[Utility.Random(ChampionSpawnList.Length)];
                        if (championType == typeof(MinaxTheTimeSorceress))
                            continue;
                    }
                    else
                    {
                        World.Broadcast(0x482, false, "A temporal paradox has occurred! Another Minax has emerged!");
                    }
                }
                try
                {
                    BaseCreature champion = (BaseCreature)Activator.CreateInstance(championType);
                    Point3D spawnLoc = new Point3D(
                        location.X + Utility.RandomMinMax(-5, 5),
                        location.Y + Utility.RandomMinMax(-5, 5),
                        location.Z
                    );
                    champion.MoveToWorld(spawnLoc, map);
                    Effects.SendLocationEffect(spawnLoc, map, 0x36BD, 16, 10, 2543, 0);
                }
                catch
                {
                    continue;
                }
            }
        }

        private void Taunt()
        {
            string[] taunts = new string[]
            {
                "You're trapped in my temporal loop!",
                "Your efforts are but a blink in eternity!",
                "I exist in all moments simultaneously!",
                "You fight against time itself!",
                "Your future is my plaything!",
                "Foolish mortal, your time has come!"
            };

            if (Combatant != null)
                Say(taunts[Utility.Random(taunts.Length)]);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if (Utility.RandomDouble() < 0.2)
                TimeDistortionAttack(defender);
        }

        // Overloaded method for melee-triggered distortion.
        private void TimeDistortionAttack(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("*Feel the wrath of time itself!*");
                Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 20, 10, 2543, 0);
                target.Freeze(TimeSpan.FromSeconds(3));
                target.SendMessage("Time slows around you!");
                target.Damage(Utility.RandomMinMax(50, 100), this);
            }
        }

        public override bool AlwaysMurderer => true;
        public override bool Unprovokable => true;
        public override bool Uncalmable => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 5);
            AddLoot(LootPack.HighScrolls, 3); // Replaced ArcanistScrolls with HighScrolls
        }

        public MinaxTheTimeSorceress(Serial serial) : base(serial)
        {
            m_TeleportTimer = Timer.DelayCall(TimeSpan.FromHours(1), TimeSpan.FromHours(1), new TimerCallback(Teleport));
            m_TauntTimer = Timer.DelayCall(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), new TimerCallback(Taunt));
            m_SpecialAttackTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15), new TimerCallback(CombatActions));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_TeleportTimer = Timer.DelayCall(TimeSpan.FromHours(1), TimeSpan.FromHours(1), new TimerCallback(Teleport));
            m_TauntTimer = Timer.DelayCall(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), new TimerCallback(Taunt));
            m_SpecialAttackTimer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15), new TimerCallback(CombatActions));
        }
    }

    // A simple clone class used in the ParadoxClone attack.
    public class MinaxClone : BaseCreature
    {
        public MinaxClone(Mobile master) : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Temporal Clone of Minax";
            Body = master.Body;
            Hue = master.Hue;

            SetStr(500, 600);
            SetDex(100, 150);
            SetInt(500, 600);

            SetHits(2000, 2500);
            SetMana(2000);

            SetDamage(15, 25);

            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);

            Timer.DelayCall(TimeSpan.FromMinutes(2), new TimerCallback(Delete));
        }

        public override void Delete()
        {
            FixedEffect(0x373A, 10, 30);
            PlaySound(0x209);
            base.Delete();
        }

        public MinaxClone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Delete();
        }
    }
}
