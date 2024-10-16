using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;
using Server.Network;

namespace Server.Custom
{
    public class OreMap : Item
    {
        private int m_Level;
        private Point3D m_Location;
        private Map m_Facet;
        private string m_OreType;
        private bool m_Identified;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Location { get { return m_Location; } set { m_Location = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map Facet { get { return m_Facet; } set { m_Facet = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string OreType { get { return m_OreType; } set { m_OreType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified { get { return m_Identified; } set { m_Identified = value; } }

        [Constructable]
        public OreMap(int level) : base(0x14EC)
        {
            m_Level = level;
            Name = "Mysterious Ore Map";
            LootType = LootType.Blessed;
            Visible = true;
            m_Identified = false;
        }

        public OreMap(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_Identified)
            {
                from.SendMessage(String.Format("This map shows a {0} ore deposit located at {1} on {2}.", m_OreType, m_Location, m_Facet));
            }
            else
            {
                if (CheckIdentify(from))
                {
                    OnIdentified(from);
                    from.SendMessage(String.Format("This map shows a {0} ore deposit located at {1} on {2}.", m_OreType, m_Location, m_Facet));
                }
                else
                {
                    from.SendMessage("This map appears to show the location of an ore deposit, but you can't make out the details.");
                }
            }
        }

        public bool CheckIdentify(Mobile m)
        {
            if (m_Identified)
                return false;

            double miningSkill = m.Skills[SkillName.Mining].Value;
            double difficulty = 50.0 + (m_Level * 5); // Adjust this formula as needed

            return (miningSkill >= difficulty);
        }

        public void OnIdentified(Mobile m)
        {
            m_Identified = true;
            m.SendLocalizedMessage(500819); // You have successfully identified the item.
            m.SendMessage("You have identified an Ore Map!");
            Name = String.Format("{0} Ore Map", m_OreType);

            // Spawn the ore node when identified
            OreMapSystem.SpawnOreNode(this, m);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Level);
            writer.Write(m_Location);
            writer.Write(m_Facet);
            writer.Write(m_OreType);
            writer.Write(m_Identified);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Level = reader.ReadInt();
            m_Location = reader.ReadPoint3D();
            m_Facet = reader.ReadMap();
            m_OreType = reader.ReadString();
            m_Identified = reader.ReadBool();
        }
    }

    public class OreNode : Item
    {
        private string m_OreType;
        private int m_RemainingOre;

        [CommandProperty(AccessLevel.GameMaster)]
        public string OreType { get { return m_OreType; } set { m_OreType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RemainingOre { get { return m_RemainingOre; } set { m_RemainingOre = value; UpdateName(); } }

        [Constructable]
        public OreNode(string oreType, int amount) : base(0x19B9)
        {
            m_OreType = oreType;
            m_RemainingOre = amount;
            UpdateName();
            Movable = false;
        }

        public OreNode(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            if (m_RemainingOre > 0)
            {
                from.SendMessage("Select your mining tool.");
				from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendMessage("This ore deposit has been exhausted.");
            }
        }

        private class InternalTarget : Target
        {
            private OreNode m_Node;

            public InternalTarget(OreNode node) : base(2, false, TargetFlags.None)
            {
                m_Node = node;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item && ((Item)targeted).IsChildOf(from.Backpack))
                {
                    Item item = (Item)targeted;
                    if (item is BaseHarvestTool)
                    {
                        from.SendMessage("You begin mining the ore deposit.");
						m_Node.Mine(from);
                    }
                    else
                    {
                        from.SendMessage("You must target a mining tool.");
                    }
                }
                else
                {
                    from.SendMessage("You must target a mining tool in your backpack.");
                }
            }
        }

        public void Mine(Mobile from)
        {
            if (m_RemainingOre > 0)
            {
                from.Animate(11, 5, 1, true, false, 0); // Mining animation
                Timer.DelayCall(TimeSpan.FromSeconds(3), () => ContinueMining(from));
            }
        }

        public void ContinueMining(Mobile from)
        {
            if (from == null || from.Deleted || !from.InRange(this.GetWorldLocation(), 2) || !from.Alive || from.Mounted)
                return;

            if (m_RemainingOre > 0)
            {
                from.Animate(11, 5, 1, true, false, 0); // Mining animation
                int mined = Utility.Random(10, 50); // Random amount mined
                mined = Math.Min(mined, m_RemainingOre);

                Type oreType = ScriptCompiler.FindTypeByName(String.Format("{0}Ore", m_OreType));
                if (oreType != null)
                {
                    Item ore = (Item)Activator.CreateInstance(oreType);
                    ore.Amount = mined;
                    if (!from.AddToBackpack(ore))
                    {
                        ore.Delete();
                        from.SendMessage("Your backpack is full.");
                    }
                    else
                    {
                        from.SendMessage(String.Format("You mine {0} {1} ore.", mined, m_OreType.ToLower()));
                        m_RemainingOre -= mined;
                        UpdateName();
                    }
                }
                else
                {
                    from.SendMessage("There was an error creating the ore.");
                }

                if (m_RemainingOre <= 0)
                {
                    from.SendMessage("The ore deposit has been exhausted.");
                    this.Delete();
                }
                else
                {
                    if (Utility.RandomDouble() < 0.1) // 10% chance to spawn more guardians
                    {
                        OreMapSystem.SpawnGuardians(this);
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => ContinueMining(from)); // Continue mining every 3 seconds
                }
            }
        }

        public void UpdateName()
        {
            Name = String.Format("{0} Ore Deposit ({1} remaining)", m_OreType, m_RemainingOre);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_OreType);
            writer.Write(m_RemainingOre);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_OreType = reader.ReadString();
            m_RemainingOre = reader.ReadInt();
            UpdateName();
        }
    }

    public class OreMapSystem
    {
        private static Dictionary<Map, List<Point3D>> s_LocationsByFacet = new Dictionary<Map, List<Point3D>>()
        {
            { Map.Felucca, new List<Point3D> { new Point3D(2676, 253, 0), new Point3D(2657, 244, 0), new Point3D(2650, 219, 2), new Point3D(2593, 422, 13), new Point3D(2664, 175, -2), new Point3D(2709, 129, 0), new Point3D(2698, 104, 0), new Point3D(2673, 95, 0), new Point3D(2619, 435, 15), new Point3D(2644, 89, 0), new Point3D(2624, 74, 0), new Point3D(2565, 502, 0), new Point3D(2639, 45, 38), new Point3D(2600, 65, 0), new Point3D(2567, 63, 0), new Point3D(2530, 65, 0), new Point3D(2490, 69, 0), new Point3D(2457, 54, 0), new Point3D(2450, 84, 0), new Point3D(2427, 107, 0), new Point3D(2450, 117, 0), new Point3D(2433, 164, 0), new Point3D(2617, 469, 0), new Point3D(2412, 179, 0), new Point3D(2418, 199, 0), new Point3D(2385, 204, 0), new Point3D(2376, 181, 0), new Point3D(2365, 154, 0), new Point3D(2594, 503, 1), new Point3D(2571, 481, 0), new Point3D(2361, 127, 0), new Point3D(2367, 93, 0), new Point3D(2373, 75, 0), new Point3D(2567, 383, 16), new Point3D(2581, 405, 15), new Point3D(2553, 485, 15), new Point3D(2558, 452, 15), new Point3D(2551, 424, 16), new Point3D(2555, 391, 14), new Point3D(2609, 474, 20), new Point3D(2597, 491, 20), new Point3D(2566, 495, 20), new Point3D(2582, 489, 40), new Point3D(2601, 479, 40), new Point3D(2608, 462, 40), new Point3D(2584, 482, 60), new Point3D(2601, 464, 60), new Point3D(2594, 462, 60), new Point3D(2107, 261, 0), new Point3D(2093, 235, 0), new Point3D(2085, 206, 0), new Point3D(2067, 185, 0), new Point3D(2064, 155, 0), new Point3D(2055, 128, 0), new Point3D(2018, 151, 0), new Point3D(1989, 180, 0), new Point3D(1956, 206, 0), new Point3D(1921, 231, 0), new Point3D(1892, 260, 0), new Point3D(1873, 299, 0), new Point3D(1864, 344, 0), new Point3D(1872, 372, 0), new Point3D(1877, 403, 0), new Point3D(1890, 427, 5), new Point3D(1925, 402, 0), new Point3D(1920, 380, 0), new Point3D(1898, 367, 0), new Point3D(1929, 342, 0), new Point3D(1934, 318, 0), new Point3D(1966, 306, 0), new Point3D(1998, 294, 2), new Point3D(1992, 267, 8), new Point3D(1975, 269, 8), new Point3D(1995, 251, 7), new Point3D(2024, 222, 14), new Point3D(2059, 236, 14), new Point3D(2037, 273, 5), new Point3D(2014, 308, 0), new Point3D(2025, 333, 0), new Point3D(2034, 360, 0), new Point3D(2044, 386, 0), new Point3D(2077, 361, 0), new Point3D(2109, 336, 0), new Point3D(2118, 299, 0), new Point3D(2116, 268, 0), new Point3D(2100, 251, 0), new Point3D(2416, 825, 0), new Point3D(2389, 816, 0), new Point3D(2366, 803, 0), new Point3D(2343, 791, 0), new Point3D(2311, 814, 0), new Point3D(2316, 845, 0), new Point3D(2333, 864, 0), new Point3D(2348, 831, 0), new Point3D(2322, 821, 0), new Point3D(2364, 819, 0), new Point3D(2354, 878, 0), new Point3D(2332, 915, 0), new Point3D(2342, 940, 0), new Point3D(2375, 918, 0), new Point3D(2397, 910, 0), new Point3D(2377, 943, 0), new Point3D(2414, 921, 0), new Point3D(2441, 928, 0), new Point3D(2466, 912, 0), new Point3D(2439, 878, 0), new Point3D(2471, 894, 0), new Point3D(2473, 924, 0), new Point3D(2495, 922, 0), new Point3D(2525, 924, 0), new Point3D(2548, 937, 0), new Point3D(2585, 917, 0), new Point3D(2600, 883, 0), new Point3D(2576, 881, 0), new Point3D(2588, 852, 0), new Point3D(2564, 889, 0), new Point3D(2290, 1238, 0), new Point3D(2275, 1262, 0), new Point3D(2243, 1266, 0), new Point3D(2218, 1270, 0), new Point3D(2205, 1247, 0), new Point3D(2188, 1270, 0), new Point3D(2175, 1230, 1), new Point3D(2219, 1119, 18), new Point3D(2231, 1132, 0), new Point3D(2208, 1160, 0), new Point3D(2207, 1192, 0), new Point3D(2219, 1215, 0), new Point3D(2231, 1234, 1), new Point3D(2259, 1240, 1), new Point3D(2278, 1235, 0), new Point3D(2043, 851, 0), new Point3D(2016, 881, 7), new Point3D(1999, 920, 0), new Point3D(1956, 930, 2), new Point3D(1945, 977, 0), new Point3D(1903, 994, 0), new Point3D(1886, 965, -1), new Point3D(1866, 1003, 0), new Point3D(1836, 975, -1), new Point3D(1826, 1018, 0), new Point3D(1802, 1050, 0), new Point3D(1821, 1066, 0), new Point3D(1818, 1106, 0), new Point3D(1826, 1133, 0), new Point3D(1839, 1155, 0), new Point3D(1827, 1202, 0), new Point3D(1789, 1224, 0), new Point3D(1772, 1206, 0), new Point3D(1775, 1171, 0), new Point3D(1778, 1132, 0), new Point3D(1775, 1100, 1), new Point3D(1779, 1060, 0), new Point3D(1775, 1029, 0), new Point3D(1765, 1006, 0), new Point3D(1756, 981, 1), new Point3D(1751, 950, 0), new Point3D(1745, 920, 0), new Point3D(1745, 884, 0), new Point3D(1760, 840, 1), new Point3D(1770, 794, 1), new Point3D(1803, 797, 0), new Point3D(1819, 809, 0), new Point3D(1862, 801, 0), new Point3D(1830, 828, -1), new Point3D(1802, 854, -1), new Point3D(1819, 874, -1), new Point3D(1818, 910, -1), new Point3D(1848, 914, -1), new Point3D(1822, 948, -1), new Point3D(1834, 975, -1), new Point3D(1875, 958, -1), new Point3D(1721, 1015, 0), new Point3D(1721, 977, 0), new Point3D(1720, 943, 0), new Point3D(1707, 919, 0), new Point3D(1690, 901, 0), new Point3D(1689, 868, 0), new Point3D(1664, 853, 0), new Point3D(1631, 843, 1), new Point3D(1594, 864, 1), new Point3D(1575, 904, 0), new Point3D(1585, 962, 0), new Point3D(1626, 990, 0), new Point3D(1633, 1054, 0), new Point3D(1663, 1094, 1), new Point3D(1589, 1115, 0), new Point3D(1510, 1126, 0), new Point3D(1445, 1157, 0), new Point3D(1395, 1144, 0), new Point3D(1403, 1090, 0), new Point3D(1476, 1045, 0), new Point3D(1464, 994, 6), new Point3D(1386, 994, 2), new Point3D(1362, 956, 1), new Point3D(1391, 925, 4), new Point3D(1407, 894, 0), new Point3D(1395, 859, 0), new Point3D(1485, 905, 0), new Point3D(1453, 871, 0), new Point3D(1439, 799, 1), new Point3D(1368, 844, 0), new Point3D(1337, 920, 0), new Point3D(1314, 1002, 0), new Point3D(1310, 936, 1), new Point3D(1290, 886, 4), new Point3D(1241, 950, 4), new Point3D(1212, 1035, 0), new Point3D(1183, 1118, 1), new Point3D(1119, 1171, 2), new Point3D(1075, 1246, 1), new Point3D(1055, 1328, 1), new Point3D(1082, 1374, 1), new Point3D(1071, 1448, 0), new Point3D(1014, 1438, 0), new Point3D(994, 1395, 1), new Point3D(972, 1352, 0), new Point3D(945, 1314, 0), new Point3D(903, 1289, 0), new Point3D(834, 1299, 1), new Point3D(846, 1393, 3), new Point3D(875, 1405, 3), new Point3D(895, 1457, 1), new Point3D(915, 1508, 2), new Point3D(940, 1554, 12), new Point3D(976, 1588, 4), new Point3D(998, 1586, 0), new Point3D(1007, 1617, 0), new Point3D(1042, 1653, 1), new Point3D(1057, 1711, 1), new Point3D(1041, 1796, 1), new Point3D(1047, 1858, 0), new Point3D(1064, 1915, 1), new Point3D(1109, 1942, 0), new Point3D(1095, 2009, 1), new Point3D(1151, 2029, 1), new Point3D(1218, 1988, 0), new Point3D(1206, 1938, 0), new Point3D(1184, 1892, 1), new Point3D(1146, 1860, 0), new Point3D(1115, 1823, 1), new Point3D(1113, 1762, 0), new Point3D(1174, 1775, 3), new Point3D(1160, 1727, 3), new Point3D(1139, 1677, 1), new Point3D(1104, 1651, 3), new Point3D(1101, 1593, 1), new Point3D(1152, 1525, 0), new Point3D(1184, 1452, 1), new Point3D(1171, 1394, 1), new Point3D(1185, 1332, 0), new Point3D(1242, 1278, 1), new Point3D(1272, 1218, 0), new Point3D(1233, 1257, 0), new Point3D(1334, 1233, 0), new Point3D(774, 1584, 2), new Point3D(721, 1567, 0), new Point3D(694, 1559, 0), new Point3D(649, 1534, 0), new Point3D(609, 1509, 4), new Point3D(556, 1495, 0), new Point3D(510, 1471, 0), new Point3D(479, 1432, 2), new Point3D(447, 1403, 2), new Point3D(405, 1383, 0), new Point3D(357, 1370, 0), new Point3D(285, 1373, 0), new Point3D(263, 1434, 0), new Point3D(284, 1480, 1), new Point3D(336, 1499, 4), new Point3D(363, 1544, 2), new Point3D(412, 1564, 1), new Point3D(460, 1581, 0), new Point3D(535, 1573, 0), new Point3D(562, 1618, 0), new Point3D(597, 1650, 0), new Point3D(650, 1667, 0), new Point3D(690, 1684, 0), new Point3D(743, 1695, 1), new Point3D(777, 1682, 0), new Point3D(802, 1668, 1), new Point3D(1048, 2516, 0), new Point3D(1029, 2588, 0), new Point3D(1059, 2630, 0), new Point3D(1068, 2683, 0), new Point3D(1112, 2669, 0), new Point3D(1115, 2654, 0), new Point3D(1157, 2641, 0), new Point3D(1196, 2643, 0), new Point3D(1222, 2687, 0), new Point3D(1267, 2664, 0), new Point3D(1249, 2652, 0), new Point3D(1000, 1000, 0), new Point3D(1100, 1100, 0), new Point3D(1310, 2661, 0), new Point3D(1328, 2715, 0), new Point3D(1349, 2730, 0), new Point3D(1367, 2712, 0), new Point3D(1382, 2756, 0), new Point3D(1408, 2799, 0), new Point3D(1385, 2860, 0), new Point3D(1392, 2924, 0), new Point3D(1418, 2921, 0), new Point3D(1421, 2887, 0), new Point3D(1444, 2936, 1), new Point3D(1463, 2989, 0), new Point3D(1502, 3022, 0), new Point3D(1519, 2939, 0), new Point3D(1510, 2882, 0), new Point3D(1506, 2849, 0), new Point3D(1481, 2840, 0), new Point3D(1492, 2851, 0), new Point3D(1510, 2780, 0), new Point3D(1493, 2729, 0), new Point3D(1455, 2697, 0), new Point3D(1417, 2664, 0), new Point3D(1376, 2634, 0), new Point3D(1336, 2604, 0), new Point3D(1300, 2570, 0), new Point3D(1223, 2579, 0), new Point3D(1178, 2553, 0), new Point3D(1137, 2523, 0), new Point3D(1070, 2519, 0), new Point3D(1639, 2885, 0), new Point3D(1643, 2951, 0), new Point3D(1658, 3007, 0), new Point3D(1700, 3035, 0), new Point3D(1719, 3020, 0), new Point3D(1711, 2988, 0), new Point3D(1695, 2946, 0), new Point3D(1663, 2915, 0), new Point3D(1660, 2942, 0), new Point3D(1661, 2976, 0), new Point3D(1693, 2980, 0), new Point3D(1562, 3175, 0), new Point3D(1575, 3207, 0), new Point3D(1553, 3210, 0), new Point3D(1604, 3255, 0), new Point3D(1633, 3292, 0), new Point3D(1610, 3311, 0), new Point3D(1593, 3285, 0) } },
            { Map.Trammel, new List<Point3D> { new Point3D(2676, 253, 0), new Point3D(2657, 244, 0), new Point3D(2650, 219, 2), new Point3D(2593, 422, 13), new Point3D(2664, 175, -2), new Point3D(2709, 129, 0), new Point3D(2698, 104, 0), new Point3D(2673, 95, 0), new Point3D(2619, 435, 15), new Point3D(2644, 89, 0), new Point3D(2624, 74, 0), new Point3D(2565, 502, 0), new Point3D(2639, 45, 38), new Point3D(2600, 65, 0), new Point3D(2567, 63, 0), new Point3D(2530, 65, 0), new Point3D(2490, 69, 0), new Point3D(2457, 54, 0), new Point3D(2450, 84, 0), new Point3D(2427, 107, 0), new Point3D(2450, 117, 0), new Point3D(2433, 164, 0), new Point3D(2617, 469, 0), new Point3D(2412, 179, 0), new Point3D(2418, 199, 0), new Point3D(2385, 204, 0), new Point3D(2376, 181, 0), new Point3D(2365, 154, 0), new Point3D(2594, 503, 1), new Point3D(2571, 481, 0), new Point3D(2361, 127, 0), new Point3D(2367, 93, 0), new Point3D(2373, 75, 0), new Point3D(2567, 383, 16), new Point3D(2581, 405, 15), new Point3D(2553, 485, 15), new Point3D(2558, 452, 15), new Point3D(2551, 424, 16), new Point3D(2555, 391, 14), new Point3D(2609, 474, 20), new Point3D(2597, 491, 20), new Point3D(2566, 495, 20), new Point3D(2582, 489, 40), new Point3D(2601, 479, 40), new Point3D(2608, 462, 40), new Point3D(2584, 482, 60), new Point3D(2601, 464, 60), new Point3D(2594, 462, 60), new Point3D(2107, 261, 0), new Point3D(2093, 235, 0), new Point3D(2085, 206, 0), new Point3D(2067, 185, 0), new Point3D(2064, 155, 0), new Point3D(2055, 128, 0), new Point3D(2018, 151, 0), new Point3D(1989, 180, 0), new Point3D(1956, 206, 0), new Point3D(1921, 231, 0), new Point3D(1892, 260, 0), new Point3D(1873, 299, 0), new Point3D(1864, 344, 0), new Point3D(1872, 372, 0), new Point3D(1877, 403, 0), new Point3D(1890, 427, 5), new Point3D(1925, 402, 0), new Point3D(1920, 380, 0), new Point3D(1898, 367, 0), new Point3D(1929, 342, 0), new Point3D(1934, 318, 0), new Point3D(1966, 306, 0), new Point3D(1998, 294, 2), new Point3D(1992, 267, 8), new Point3D(1975, 269, 8), new Point3D(1995, 251, 7), new Point3D(2024, 222, 14), new Point3D(2059, 236, 14), new Point3D(2037, 273, 5), new Point3D(2014, 308, 0), new Point3D(2025, 333, 0), new Point3D(2034, 360, 0), new Point3D(2044, 386, 0), new Point3D(2077, 361, 0), new Point3D(2109, 336, 0), new Point3D(2118, 299, 0), new Point3D(2116, 268, 0), new Point3D(2100, 251, 0), new Point3D(2416, 825, 0), new Point3D(2389, 816, 0), new Point3D(2366, 803, 0), new Point3D(2343, 791, 0), new Point3D(2311, 814, 0), new Point3D(2316, 845, 0), new Point3D(2333, 864, 0), new Point3D(2348, 831, 0), new Point3D(2322, 821, 0), new Point3D(2364, 819, 0), new Point3D(2354, 878, 0), new Point3D(2332, 915, 0), new Point3D(2342, 940, 0), new Point3D(2375, 918, 0), new Point3D(2397, 910, 0), new Point3D(2377, 943, 0), new Point3D(2414, 921, 0), new Point3D(2441, 928, 0), new Point3D(2466, 912, 0), new Point3D(2439, 878, 0), new Point3D(2471, 894, 0), new Point3D(2473, 924, 0), new Point3D(2495, 922, 0), new Point3D(2525, 924, 0), new Point3D(2548, 937, 0), new Point3D(2585, 917, 0), new Point3D(2600, 883, 0), new Point3D(2576, 881, 0), new Point3D(2588, 852, 0), new Point3D(2564, 889, 0), new Point3D(2290, 1238, 0), new Point3D(2275, 1262, 0), new Point3D(2243, 1266, 0), new Point3D(2218, 1270, 0), new Point3D(2205, 1247, 0), new Point3D(2188, 1270, 0), new Point3D(2175, 1230, 1), new Point3D(2219, 1119, 18), new Point3D(2231, 1132, 0), new Point3D(2208, 1160, 0), new Point3D(2207, 1192, 0), new Point3D(2219, 1215, 0), new Point3D(2231, 1234, 1), new Point3D(2259, 1240, 1), new Point3D(2278, 1235, 0), new Point3D(2043, 851, 0), new Point3D(2016, 881, 7), new Point3D(1999, 920, 0), new Point3D(1956, 930, 2), new Point3D(1945, 977, 0), new Point3D(1903, 994, 0), new Point3D(1886, 965, -1), new Point3D(1866, 1003, 0), new Point3D(1836, 975, -1), new Point3D(1826, 1018, 0), new Point3D(1802, 1050, 0), new Point3D(1821, 1066, 0), new Point3D(1818, 1106, 0), new Point3D(1826, 1133, 0), new Point3D(1839, 1155, 0), new Point3D(1827, 1202, 0), new Point3D(1789, 1224, 0), new Point3D(1772, 1206, 0), new Point3D(1775, 1171, 0), new Point3D(1778, 1132, 0), new Point3D(1775, 1100, 1), new Point3D(1779, 1060, 0), new Point3D(1775, 1029, 0), new Point3D(1765, 1006, 0), new Point3D(1756, 981, 1), new Point3D(1751, 950, 0), new Point3D(1745, 920, 0), new Point3D(1745, 884, 0), new Point3D(1760, 840, 1), new Point3D(1770, 794, 1), new Point3D(1803, 797, 0), new Point3D(1819, 809, 0), new Point3D(1862, 801, 0), new Point3D(1830, 828, -1), new Point3D(1802, 854, -1), new Point3D(1819, 874, -1), new Point3D(1818, 910, -1), new Point3D(1848, 914, -1), new Point3D(1822, 948, -1), new Point3D(1834, 975, -1), new Point3D(1875, 958, -1), new Point3D(1721, 1015, 0), new Point3D(1721, 977, 0), new Point3D(1720, 943, 0), new Point3D(1707, 919, 0), new Point3D(1690, 901, 0), new Point3D(1689, 868, 0), new Point3D(1664, 853, 0), new Point3D(1631, 843, 1), new Point3D(1594, 864, 1), new Point3D(1575, 904, 0), new Point3D(1585, 962, 0), new Point3D(1626, 990, 0), new Point3D(1633, 1054, 0), new Point3D(1663, 1094, 1), new Point3D(1589, 1115, 0), new Point3D(1510, 1126, 0), new Point3D(1445, 1157, 0), new Point3D(1395, 1144, 0), new Point3D(1403, 1090, 0), new Point3D(1476, 1045, 0), new Point3D(1464, 994, 6), new Point3D(1386, 994, 2), new Point3D(1362, 956, 1), new Point3D(1391, 925, 4), new Point3D(1407, 894, 0), new Point3D(1395, 859, 0), new Point3D(1485, 905, 0), new Point3D(1453, 871, 0), new Point3D(1439, 799, 1), new Point3D(1368, 844, 0), new Point3D(1337, 920, 0), new Point3D(1314, 1002, 0), new Point3D(1310, 936, 1), new Point3D(1290, 886, 4), new Point3D(1241, 950, 4), new Point3D(1212, 1035, 0), new Point3D(1183, 1118, 1), new Point3D(1119, 1171, 2), new Point3D(1075, 1246, 1), new Point3D(1055, 1328, 1), new Point3D(1082, 1374, 1), new Point3D(1071, 1448, 0), new Point3D(1014, 1438, 0), new Point3D(994, 1395, 1), new Point3D(972, 1352, 0), new Point3D(945, 1314, 0), new Point3D(903, 1289, 0), new Point3D(834, 1299, 1), new Point3D(846, 1393, 3), new Point3D(875, 1405, 3), new Point3D(895, 1457, 1), new Point3D(915, 1508, 2), new Point3D(940, 1554, 12), new Point3D(976, 1588, 4), new Point3D(998, 1586, 0), new Point3D(1007, 1617, 0), new Point3D(1042, 1653, 1), new Point3D(1057, 1711, 1), new Point3D(1041, 1796, 1), new Point3D(1047, 1858, 0), new Point3D(1064, 1915, 1), new Point3D(1109, 1942, 0), new Point3D(1095, 2009, 1), new Point3D(1151, 2029, 1), new Point3D(1218, 1988, 0), new Point3D(1206, 1938, 0), new Point3D(1184, 1892, 1), new Point3D(1146, 1860, 0), new Point3D(1115, 1823, 1), new Point3D(1113, 1762, 0), new Point3D(1174, 1775, 3), new Point3D(1160, 1727, 3), new Point3D(1139, 1677, 1), new Point3D(1104, 1651, 3), new Point3D(1101, 1593, 1), new Point3D(1152, 1525, 0), new Point3D(1184, 1452, 1), new Point3D(1171, 1394, 1), new Point3D(1185, 1332, 0), new Point3D(1242, 1278, 1), new Point3D(1272, 1218, 0), new Point3D(1233, 1257, 0), new Point3D(1334, 1233, 0), new Point3D(774, 1584, 2), new Point3D(721, 1567, 0), new Point3D(694, 1559, 0), new Point3D(649, 1534, 0), new Point3D(609, 1509, 4), new Point3D(556, 1495, 0), new Point3D(510, 1471, 0), new Point3D(479, 1432, 2), new Point3D(447, 1403, 2), new Point3D(405, 1383, 0), new Point3D(357, 1370, 0), new Point3D(285, 1373, 0), new Point3D(263, 1434, 0), new Point3D(284, 1480, 1), new Point3D(336, 1499, 4), new Point3D(363, 1544, 2), new Point3D(412, 1564, 1), new Point3D(460, 1581, 0), new Point3D(535, 1573, 0), new Point3D(562, 1618, 0), new Point3D(597, 1650, 0), new Point3D(650, 1667, 0), new Point3D(690, 1684, 0), new Point3D(743, 1695, 1), new Point3D(777, 1682, 0), new Point3D(802, 1668, 1), new Point3D(1048, 2516, 0), new Point3D(1029, 2588, 0), new Point3D(1059, 2630, 0), new Point3D(1068, 2683, 0), new Point3D(1112, 2669, 0), new Point3D(1115, 2654, 0), new Point3D(1157, 2641, 0), new Point3D(1196, 2643, 0), new Point3D(1222, 2687, 0), new Point3D(1267, 2664, 0), new Point3D(1249, 2652, 0), new Point3D(1000, 1000, 0), new Point3D(1100, 1100, 0), new Point3D(1310, 2661, 0), new Point3D(1328, 2715, 0), new Point3D(1349, 2730, 0), new Point3D(1367, 2712, 0), new Point3D(1382, 2756, 0), new Point3D(1408, 2799, 0), new Point3D(1385, 2860, 0), new Point3D(1392, 2924, 0), new Point3D(1418, 2921, 0), new Point3D(1421, 2887, 0), new Point3D(1444, 2936, 1), new Point3D(1463, 2989, 0), new Point3D(1502, 3022, 0), new Point3D(1519, 2939, 0), new Point3D(1510, 2882, 0), new Point3D(1506, 2849, 0), new Point3D(1481, 2840, 0), new Point3D(1492, 2851, 0), new Point3D(1510, 2780, 0), new Point3D(1493, 2729, 0), new Point3D(1455, 2697, 0), new Point3D(1417, 2664, 0), new Point3D(1376, 2634, 0), new Point3D(1336, 2604, 0), new Point3D(1300, 2570, 0), new Point3D(1223, 2579, 0), new Point3D(1178, 2553, 0), new Point3D(1137, 2523, 0), new Point3D(1070, 2519, 0), new Point3D(1639, 2885, 0), new Point3D(1643, 2951, 0), new Point3D(1658, 3007, 0), new Point3D(1700, 3035, 0), new Point3D(1719, 3020, 0), new Point3D(1711, 2988, 0), new Point3D(1695, 2946, 0), new Point3D(1663, 2915, 0), new Point3D(1660, 2942, 0), new Point3D(1661, 2976, 0), new Point3D(1693, 2980, 0), new Point3D(1562, 3175, 0), new Point3D(1575, 3207, 0), new Point3D(1553, 3210, 0), new Point3D(1604, 3255, 0), new Point3D(1633, 3292, 0), new Point3D(1610, 3311, 0), new Point3D(1593, 3285, 0) } },
            // Add more predefined locations for other facets here
        };

        public static void CreateOreMap(Mobile from)
        {
            int level = CalculateMapLevel(from);
            OreMap map = new OreMap(level);

            map.OreType = GetRandomOreType(level);
            map.Facet = Utility.RandomBool() ? Map.Felucca : Map.Trammel;
            map.Location = GetRandomLocation(map.Facet);

            if (from.AddToBackpack(map))
            {
                from.SendMessage("You've received a mysterious ore map. Use Mining skill to identify it.");
            }
            else
            {
                map.Delete();
                from.SendMessage("You find an ore map, but have no room in your backpack to keep it.");
            }
        }

        private static int CalculateMapLevel(Mobile from)
        {
            int miningSkill = from.Skills[SkillName.Mining].Fixed / 100; // 0 to 100
            return Clamp(miningSkill / 20, 1, 5);
        }

        private static string GetRandomOreType(int level)
        {
            string[] oreTypes = new string[]
            {
                "Iron", "DullCopper", "ShadowIron", "Copper", "Bronze",
                "Gold", "Agapite", "Verite", "Valorite"
            };

            return oreTypes[Utility.Random(Math.Min(level * 2, oreTypes.Length))];
        }

        private static Point3D GetRandomLocation(Map map)
        {
            List<Point3D> locations;
            if (s_LocationsByFacet.TryGetValue(map, out locations) && locations.Count > 0)
            {
                int index = Utility.Random(locations.Count);
                Point3D selectedLocation = locations[index];
                locations.RemoveAt(index);
                return selectedLocation;
            }

            int x, y, z;
            do
            {
                x = Utility.Random(map.Width);
                y = Utility.Random(map.Height);
                z = map.GetAverageZ(x, y);
            } while (!map.CanSpawnMobile(x, y, z));

            return new Point3D(x, y, z);
        }

        public static void SpawnOreNode(OreMap map, Mobile miner)
        {
            int miningSkill = miner.Skills[SkillName.Mining].Fixed / 10; // 0 to 1000
            int oreAmount = Utility.Random(50, 451) + miningSkill;
            oreAmount = Math.Min(oreAmount, 500); // Max 500

            OreNode node = new OreNode(map.OreType, oreAmount);
            node.MoveToWorld(map.Location, map.Facet);

            SpawnGuardians(node);
        }

        public static void SpawnGuardians(OreNode node)
        {
            int guardianCount = Utility.Random(2, 4);
            for (int i = 0; i < guardianCount; i++)
            {
                BaseCreature guardian = CreateGuardian(node.OreType);
                Point3D loc = node.Location;
                for (int j = 0; j < 10; j++)
                {
                    int x = loc.X + Utility.Random(-3, 7);
                    int y = loc.Y + Utility.Random(-3, 7);
                    int z = node.Map.GetAverageZ(x, y);

                    if (node.Map.CanSpawnMobile(x, y, z))
                    {
                        guardian.MoveToWorld(new Point3D(x, y, z), node.Map);
                        break;
                    }
                }
            }
        }

        private static BaseCreature CreateGuardian(string oreType)
        {
            switch (oreType)
            {
                case "Iron":
                    return new EarthElemental();
                case "DullCopper":
                case "ShadowIron":
                    return new Golem();
                case "Copper":
                case "Bronze":
                    return new FireElemental();
                case "Gold":
                    return new AirElemental();
                case "Agapite":
                case "Verite":
                case "Valorite":
                    return new OreElemental();
                default:
                    return new EarthElemental();
            }
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }

    public class OreElemental : BaseCreature
    {
        [Constructable]
        public OreElemental() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ore elemental";
            Body = 14;
            BaseSoundID = 268;

            SetStr(226, 255);
            SetDex(126, 145);
            SetInt(71, 92);

            SetHits(136, 153);

            SetDamage(9, 16);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 50.1, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 100.0);
            SetSkill(SkillName.Wrestling, 60.1, 100.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;
        }

        public OreElemental(Serial serial) : base(serial)
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
        }
    }

    public class OreMapGump : Gump
    {
        private readonly Mobile m_From;

        public OreMapGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            AddPage(0);
            AddBackground(0, 0, 200, 200, 5054);
            AddLabel(20, 20, 0, "Ore Map System");
            AddButton(20, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(55, 50, 0, "Create Ore Map");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                OreMapSystem.CreateOreMap(m_From);
            }
        }
    }
}
