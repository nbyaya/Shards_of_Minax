using System;
using Server;
using Server.Items;
using Server.Commands;

namespace Server.Custom
{
    public class GenCustomTeles
    {
        public static void Initialize()
        {
            CommandSystem.Register("GenCustomTeles", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("GenCustomTeles")]
        [Description("Generates teleporters for the custom facet (Map 6).")]
        private static void OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Generating custom facet teleporters...");

            int count = CreateTeleporters(Map.Maps[32]); // or Map.Sosaria if you added that alias

            e.Mobile.SendMessage($"Done! {count} teleporters created.");
        }

        private static int CreateTeleporters(Map map)
        {
            int count = 0;

            // ===============================
            // Add your teleporters here below:
            // ===============================
            //count += CreateTeleporter(5881, 1609, -12, 5701, 1473, 7, map, true);
            //count += CreateTeleporter(1000, 1000, 0, 2000, 2000, 0, map, false);
            // count += CreateTeleporter(...); more here...
			// Pyramid
			count += CreateTeleporter( 5952, 87, 17, 3618, 453, 0, map, true );
			count += CreateTeleporter( 5953, 87, 17, 3619, 453, 0, map, true );
			count += CreateTeleporter( 514, 1559, 0, 5396, 127, 0, map, true );
			count += CreateTeleporter( 5878, 139, -13, 5983, 445, 7, map, true );
			count += CreateTeleporter( 5878, 140, -13, 5983, 446, 7, map, true );
			count += CreateTeleporter( 5984, 445, 12, 5879, 139, -8, map, true );
			count += CreateTeleporter( 5984, 446, 12, 5879, 140, -8, map, true );
			count += CreateTeleporter( 3618, 452, 5952, 86, 12, 0, map, true );
			count += CreateTeleporter( 3619, 452, 5953, 86, 12, 0, map, true );
			count += CreateTeleporter( 1156, 472, -13, 5328, 573, 7, map, true );
			count += CreateTeleporter( 1156, 473, -13, 5328, 574, 7, map, true );
			count += CreateTeleporter( 5243, 518, -13, 5338, 706, 7, map, true );
			count += CreateTeleporter( 5243, 519, -13, 5338, 707, 7, map, true );
			count += CreateTeleporter( 5329, 573, 12, 1157, 472, -8, map, true );
			count += CreateTeleporter( 5329, 574, 12, 1157, 473, -8, map, true );
			count += CreateTeleporter( 5339, 706, 12, 5244, 518, -8, map, true );
			count += CreateTeleporter( 5339, 707, 12, 5244, 519, -8, map, true );
			count += CreateTeleporter( 5376, 748, -13, 5359, 908, 7, map, true );
			count += CreateTeleporter( 5360, 908, 12, 5377, 748, -8, map, true );
			count += CreateTeleporter( 5360, 909, 12, 5377, 749, -8, map, true );
			count += CreateTeleporter( 5376, 749, -13, 5359, 909, 7, map, true );

			// Mage Keep Portal
			count += CreateTeleporter( 1826, 760, 0, 1824, 760, 20, map, true );

			// Dardin
			count += CreateTeleporter( 5697, 53, 17, 3005, 442, 0, map, true );
			count += CreateTeleporter( 5698, 53, 17, 3006, 442, 0, map, true );
			count += CreateTeleporter( 5646, 148, -13, 5779, 390, 7, map, true );
			count += CreateTeleporter( 5646, 149, -13, 5779, 391, 7, map, true );
			count += CreateTeleporter( 3005, 441, 5697, 52, 12, 0, map, true );
			count += CreateTeleporter( 3006, 441, 5698, 52, 12, 0, map, true );
			count += CreateTeleporter( 5780, 390, 12, 5647, 148, -8, map, true );
			count += CreateTeleporter( 5780, 391, 12, 5647, 149, -8, map, true );

			// Doom
			count += CreateTeleporter( 1622, 2562, 5324, 72, 7, 0, map, true );
			count += CreateTeleporter( 1622, 2561, 5324, 71, 7, 0, map, true );
			count += CreateTeleporter( 5398, 299, 12, 5212, 85, -8, map, true );
			count += CreateTeleporter( 5398, 298, 12, 5212, 84, -8, map, true );
			count += CreateTeleporter( 5325, 72, 12, 1623, 2562, 0, map, true );
			count += CreateTeleporter( 5325, 71, 12, 1623, 2561, 0, map, true );
			count += CreateTeleporter( 5211, 85, -13, 5397, 299, 7, map, true );
			count += CreateTeleporter( 5211, 84, -13, 5397, 298, 7, map, true );

			// Unknown cave
			count += CreateTeleporter( 1318, 3602, 45, 2994, 3696, 0, map, true );
			count += CreateTeleporter( 1318, 3603, 45, 2994, 3697, 0, map, true );
			count += CreateTeleporter( 2995, 3696, 0, 1319, 3602, 45, map, true );
			count += CreateTeleporter( 2995, 3697, 0, 1319, 3603, 45, map, true );

			// Clues
			count += CreateTeleporter( 5556, 2183, 12, 5406, 2340, -8, map, true );
			count += CreateTeleporter( 5557, 2183, 12, 5407, 2340, -8, map, true );
			count += CreateTeleporter( 5989, 2187, 12, 5704, 2173, -8, map, true );
			count += CreateTeleporter( 5990, 2187, 12, 5705, 2173, -8, map, true );
			count += CreateTeleporter( 3759, 2034, 0, 5313, 2277, 0, map, true );
			count += CreateTeleporter( 3760, 2034, 0, 5314, 2277, 0, map, true );
			count += CreateTeleporter( 5704, 2172, -13, 5989, 2186, 7, map, true );
			count += CreateTeleporter( 5705, 2172, -13, 5990, 2186, 7, map, true );
			count += CreateTeleporter( 5313, 2278, 0, 3759, 2035, 0, map, true );
			count += CreateTeleporter( 5314, 2278, 0, 3760, 2035, 0, map, true );
			count += CreateTeleporter( 5406, 2339, -13, 5556, 2182, 7, map, true );
			count += CreateTeleporter( 5407, 2339, -13, 5557, 2182, 7, map, true );

			// Time
			count += CreateTeleporter( 5630, 602, 17, 3831, 1488, 0, map, true ); 
			count += CreateTeleporter( 5631, 602, 17, 3832, 1488, 0, map, true );
			count += CreateTeleporter( 5573, 690, -13, 5560, 923, 7, map, true ); 
			count += CreateTeleporter( 5574, 690, -13, 5561, 923, 7, map, true );
			count += CreateTeleporter( 5560, 924, 12, 5573, 691, -8, map, true );
			count += CreateTeleporter( 5561, 924, 12, 5574, 691, -8, map, true );
			count += CreateTeleporter( 3831, 1487, 0, 5630, 601, 12, map, true );
			count += CreateTeleporter( 3832, 1487, 0, 5631, 601, 12, map, true );

			// King Crypt
			count += 
			count += CreateTeleporter( 3063, 958, -22, 5213, 1842, 12, map, true );
			count += CreateTeleporter( 3064, 958, -22, 5214, 1842, 12, map, true );
			count += CreateTeleporter( 5213, 1843, 12, 3063, 959, -20, map, true );
			count += CreateTeleporter( 5214, 1843, 12, 3064, 959, -20, map, true );

			// TP Caves
			count += CreateTeleporter( 4180, 267, 0, 5180, 1760, 0, map, true );
			count += CreateTeleporter( 4181, 267, 0, 5181, 1760, 0, map, true );
			count += CreateTeleporter( 2508, 936, 0, 5702, 2370, 0, map, true );
			count += CreateTeleporter( 2509, 936, 0, 5703, 2370, 0, map, true );
			count += CreateTeleporter( 1000, 573, 0, 5672, 2490, 0, map, true );
			count += CreateTeleporter( 1001, 573, 0, 5673, 2490, 0, map, true );
			count += CreateTeleporter( 5702, 2371, 0, 2508, 937, 0, map, true );
			count += CreateTeleporter( 5703, 2371, 0, 2509, 937, 0, map, true );
			count += CreateTeleporter( 5672, 2491, 0, 1000, 574, 0, map, true );
			count += CreateTeleporter( 5673, 2491, 0, 1001, 574, 0, map, true ); 
			count += CreateTeleporter( 5329, 2512, 0, 2568, 2622, 0, map, true ); 
			count += CreateTeleporter( 5330, 2512, 0, 2569, 2622, 0, map, true ); 
			count += CreateTeleporter( 5424, 2515, 0, 2611, 2623, 0, map, true ); 
			count += CreateTeleporter( 5425, 2515, 0, 2612, 2623, 0, map, true ); 
			count += CreateTeleporter( 2568, 2621, 0, 5329, 2511, 0, map, true );
			count += CreateTeleporter( 2569, 2621, 0, 5330, 2511, 0, map, true ); 
			count += CreateTeleporter( 2611, 2622, 0, 5424, 2514, 0, map, true ); 
			count += CreateTeleporter( 2612, 2622, 0, 5425, 2514, 0, map, true );
			count += CreateTeleporter( 1889, 1453, 2, 5351, 1711, 0, map, true );
			count += CreateTeleporter( 1890, 1453, 2, 5352, 1711, 0, map, true ); 
			count += CreateTeleporter( 1555, 1405, 2, 5247, 1572, 0, map, true ); 
			count += CreateTeleporter( 1555, 1406, 2, 5247, 1573, 0 , map, true );
			count += CreateTeleporter( 3231, 1581, 0, 3204, 3692, 0, map, true ); 
			count += CreateTeleporter( 3232, 1581, 0, 3205, 3692, 0, map, true ); 
			count += CreateTeleporter( 5248, 1572, 0, 1556, 1405, 2, map, true ); 
			count += CreateTeleporter( 5248, 1573, 0, 1556, 1406, 2, map, true ); 
			count += CreateTeleporter( 3272, 1693, 0, 3307, 3818, 0, map, true ); 
			count += CreateTeleporter( 3273, 1693, 0, 3308, 3818, 0, map, true ); 
			count += CreateTeleporter( 5351, 1712, 0, 1889, 1454, 2, map, true ); 
			count += CreateTeleporter( 5352, 1712, 0, 1890, 1454, 2, map, true ); 
			count += CreateTeleporter( 5461, 1704, 0, 1841, 2209, 0, map, true ); 
			count += CreateTeleporter( 5462, 1704, 0, 1842, 2209, 0, map, true ); 
			count += CreateTeleporter( 5180, 1761, 0, 4180, 268, 0, map, true ); 
			count += CreateTeleporter( 5181, 1761, 0, 4181, 268, 0, map, true );
			count += CreateTeleporter( 1841, 2208, 0, 5461, 1703, 0, map, true ); 
			count += CreateTeleporter( 1842, 2208, 0, 5462, 1703, 0, map, true );
			count += CreateTeleporter( 3204, 3693, 0, 3231, 1582, 0, map, true ); 
			count += CreateTeleporter( 3205, 3693, 0, 3232, 1582, 0, map, true ); 
			count += CreateTeleporter( 3307, 3819, 0, 3272, 1694, 0, map, true ); 
			count += CreateTeleporter( 3308, 3819, 0, 3273, 1694, 0, map, true );

			// IronBlood
			count += CreateTeleporter( 4702, 1206, 5, 5430, 2889, 25, map, true );
			count += CreateTeleporter( 5430, 2890, 25, 4702, 1207, 5, map, true );
			count += CreateTeleporter( 4703, 1206, 5, 5431, 2889, 25, map, true );
			count += CreateTeleporter( 5431, 2890, 25, 4703, 1207, 5, map, true );

			// Underhill
			count += CreateTeleporter( 4459, 1263, 5, 4307, 3483, 25, map, true );
			count += CreateTeleporter( 4307, 3484, 25, 4459, 1264, 5, map, true );
			count += CreateTeleporter( 4460, 1263, 5, 4308, 3483, 25, map, true );
			count += CreateTeleporter( 4308, 3484, 25, 4460, 1264, 5, map, true );

			// Perin Depths
			count += CreateTeleporter( 4738, 1323, 5, 4309, 3827, 12, map, true );
			count += CreateTeleporter( 4310, 3827, 17, 4739, 1323, 5, map, true );
			count += CreateTeleporter( 4738, 1324, 5, 4309, 3828, 12, map, true );
			count += CreateTeleporter( 4310, 3828, 17, 4739, 1324, 5, map, true );
			count += CreateTeleporter( 4808, 1273, 5, 4401, 3684, 12, map, true );
			count += CreateTeleporter( 4401, 3685, 17, 4808, 1274, 5, map, true );
			count += CreateTeleporter( 4809, 1273, 5, 4402, 3684, 12, map, true );
			count += CreateTeleporter( 4402, 3685, 17, 4809, 1274, 5, map, true );

			// Isle Cave
			count += CreateTeleporter( 4789, 1290, 5, 3874, 3764, 0, map, true );
			count += CreateTeleporter( 3875, 3764, 0, 4790, 1290, 5, map, true );
			count += CreateTeleporter( 4789, 1291, 5, 3874, 3765, 0, map, true );
			count += CreateTeleporter( 3875, 3765, 0, 4790, 1291, 5, map, true );
			count += CreateTeleporter( 4876, 1234, 5, 3988, 3768, 0, map, true );
			count += CreateTeleporter( 3988, 3769, 0, 4876, 1235, 5, map, true );
			count += CreateTeleporter( 4877, 1234, 5, 3989, 3768, 0, map, true );
			count += CreateTeleporter( 3989, 3769, 0, 4877, 1235, 5, map, true );
			count += CreateTeleporter( 5004, 1262, 5, 3690, 3823, 0, map, true );
			count += CreateTeleporter( 3690, 3824, 0, 5004, 1263, 5, map, true );
			count += CreateTeleporter( 5005, 1262, 5, 3691, 3823, 0, map, true );
			count += CreateTeleporter( 3691, 3824, 0, 5005, 1263, 5, map, true );
			count += CreateTeleporter( 5049, 1217, 30, 3721, 3709, 0, map, true );
			count += CreateTeleporter( 3721, 3710, 0, 5049, 1218, 30, map, true );
			count += CreateTeleporter( 5050, 1217, 30, 3722, 3709, 0, map, true );
			count += CreateTeleporter( 3722, 3710, 0, 5050, 1218, 30, map, true );
			count += CreateTeleporter( 5064, 1204, 5, 3900, 3694, 0, map, true );
			count += CreateTeleporter( 3901, 3694, 0, 5065, 1204, 5, map, true );
			count += CreateTeleporter( 5064, 1205, 5, 3900, 3695, 0, map, true );
			count += CreateTeleporter( 3901, 3695, 0, 5065, 1205, 5, map, true );
			count += CreateTeleporter( 4634, 1219, 5, 4012, 3710, 0, map, true );
			count += CreateTeleporter( 4012, 3711, 0, 4634, 1220, 5, map, true );
			count += CreateTeleporter( 4635, 1219, 5, 4013, 3710, 0, map, true );
			count += CreateTeleporter( 4013, 3711, 0, 4635, 1220, 5, map, true );

			// Town Load British
			count += CreateTeleporter( 3068, 1030, -21, 3068, 1030, 0, map, true );

			// Ed Portal
			count += CreateTeleporter( 5931, 569, 0, 5880, 665, 0, map, true );
			count += CreateTeleporter( 5881, 665, 0, 5931, 568, 0, map, true );

			// Catacombs
			count += CreateTeleporter( 4036, 3343, 37, 1524, 3598, 0, map, true );
			count += CreateTeleporter( 4036, 3344, 37, 1524, 3599, 0, map, true );
			count += CreateTeleporter( 3913, 3482, 37, 1382, 3641, 42, map, true );
			count += CreateTeleporter( 1383, 3641, 31, 3914, 3482, 26, map, true );
			count += CreateTeleporter( 1523, 3598, 0, 4035, 3343, 32, map, true );
			count += CreateTeleporter( 1523, 3599, 0, 4035, 3344, 32, map, true );

			// Underhill
			count += CreateTeleporter( 4601, 1229, 5, 4449, 3449, 25, map, true );
			count += CreateTeleporter( 4450, 3449, 25, 4602, 1229, 5, map, true );
			count += CreateTeleporter( 4601, 1230, 5, 4449, 3450, 25, map, true );
			count += CreateTeleporter( 4450, 3450, 25, 4602, 1230, 5, map, true );

			// Ratman Lair 
			count += CreateTeleporter( 4456, 1218, 5, 4470, 3284, 25, map, true );
			count += CreateTeleporter( 4471, 3284, 25, 4457, 1218, 5, map, true );
			count += CreateTeleporter( 4456, 1219, 5, 4470, 3285, 25, map, true );
			count += CreateTeleporter( 4471, 3285, 25, 4457, 1219, 5, map, true );
			count += CreateTeleporter( 4445, 3298, -13, 4324, 3297, 12, map, true );
			count += CreateTeleporter( 4325, 3297, 17, 4446, 3298, -8, map, true );
			count += CreateTeleporter( 4445, 3299, -13, 4324, 3298, 12, map, true );
			count += CreateTeleporter( 4325, 3298, 17, 4446, 3299, -8, map, true );

			// Sephiroth
			count += CreateTeleporter( 4628, 1348, 5, 5186, 2958, 25, map, true );
			count += CreateTeleporter( 5186, 2959, 25, 4628, 1349, 5, map, true );
			count += CreateTeleporter( 4629, 1348, 5, 5187, 2958, 25, map, true );
			count += CreateTeleporter( 5187, 2959, 25, 4629, 1349, 5, map, true );

			// Exodus Dungeon
			count += CreateTeleporter( 876, 2650, -13, 5931, 590, 7, map, true );
			count += CreateTeleporter( 876, 2651, -13, 5931, 591, 7, map, true );
			count += CreateTeleporter( 5932, 590, 12, 877, 2650, -8, map, true );
			count += CreateTeleporter( 5932, 591, 12, 877, 2651, -8, map, true );

			// Ice Caves
			count += CreateTeleporter( 5539, 2798, 0, 4396, 1262, 5, map, true );
			count += CreateTeleporter( 4396, 1261, 5, 5539, 2797, 0, map, true );
			count += CreateTeleporter( 5540, 2798, 0, 4397, 1262, 5, map, true );
			count += CreateTeleporter( 4397, 1261, 5, 5540, 2797, 0, map, true );
			count += CreateTeleporter( 5576, 2779, 0, 4433, 1243, 5, map, true );
			count += CreateTeleporter( 4433, 1242, 5, 5576, 2778, 0, map, true );
			count += CreateTeleporter( 5577, 2779, 0, 4434, 1243, 5, map, true );
			count += CreateTeleporter( 4434, 1242, 5, 5577, 2778, 0, map, true );
			count += CreateTeleporter( 5568, 2738, 0, 4425, 1202, 5, map, true );
			count += CreateTeleporter( 4424, 1202, 5, 5567, 2738, 0, map, true );
			count += CreateTeleporter( 5568, 2739, 0, 4425, 1203, 5, map, true );
			count += CreateTeleporter( 4424, 1203, 5, 5567, 2739, 0, map, true );

			// Fires of Hell
			count += CreateTeleporter( 5242, 1102, -13, 5362, 1348, 7, map, true );
			count += CreateTeleporter( 5242, 1103, -13, 5362, 1349, 7, map, true );
			count += CreateTeleporter( 5332, 1293, 0, 3344, 1643, 0, map, true );
			count += CreateTeleporter( 5333, 1293, 0, 3345, 1643, 0, map, true );
			count += CreateTeleporter( 5606, 1317, 12, 5361, 1397, -8, map, true );
			count += CreateTeleporter( 5607, 1317, 12, 5362, 1397, -8, map, true );
			count += CreateTeleporter( 5363, 1348, 12, 5243, 1102, -8, map, true );
			count += CreateTeleporter( 5363, 1349, 12, 5243, 1103, -8, map, true );
			count += CreateTeleporter( 5361, 1396, -13, 5606, 1316, 7, map, true );
			count += CreateTeleporter( 5362, 1396, -13, 5607, 1316, 7, map, true );
			count += CreateTeleporter( 3344, 1642, 0, 5332, 1292, 0, map, true );
			count += CreateTeleporter( 3345, 1642, 0, 5333, 1292, 0, map, true );

			// Mines of Morinia
			count += CreateTeleporter( 5832, 944, -13, 5916, 1316, 7, map, true );
			count += CreateTeleporter( 5832, 945, -13, 5916, 1317, 7, map, true );
			count += CreateTeleporter( 5897, 1226, 0, 1021, 1366, 2, map, true );
			count += CreateTeleporter( 5898, 1226, 0, 1022, 1366, 2, map, true );
			count += CreateTeleporter( 5917, 1316, 12, 5833, 944, -8, map, true );
			count += CreateTeleporter( 5917, 1317, 12, 5833, 945, -8, map, true );
			count += CreateTeleporter( 1021, 1365, 2, 5897, 1225, 0, map, true );
			count += CreateTeleporter( 1022, 1365, 2, 5898, 1225, 0, map, true );
			count += CreateTeleporter( 5702, 1472, 12, 5882, 1608, -7, map, true );
			count += CreateTeleporter( 5702, 1473, 12, 5882, 1609, -7, map, true );
			count += CreateTeleporter( 5881, 1608, -12, 5701, 1472, 7, map, true );
			count += CreateTeleporter( 5881, 1609, -12, 5701, 1473, 7, map, true );			

            return count;
        }

        private static int CreateTeleporter(int x1, int y1, int z1, int x2, int y2, int z2, Map map, bool back)
        {
            int created = 0;
            Point3D from = new Point3D(x1, y1, z1);
            Point3D to = new Point3D(x2, y2, z2);

            bool exists = false;
            foreach (Item item in map.GetItemsInRange(from, 0))
            {
                if (item is Teleporter && !(item is KeywordTeleporter) && !(item is SkillTeleporter))
                    exists = true;
            }

            if (!exists)
            {
                Teleporter tel = new Teleporter(to, map);
                tel.MoveToWorld(from, map);
                created++;
            }

            if (back)
            {
                exists = false;
                foreach (Item item in map.GetItemsInRange(to, 0))
                {
                    if (item is Teleporter && !(item is KeywordTeleporter) && !(item is SkillTeleporter))
                        exists = true;
                }

                if (!exists)
                {
                    Teleporter telBack = new Teleporter(from, map);
                    telBack.MoveToWorld(to, map);
                    created++;
                }
            }

            return created;
        }
    }
}
