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
    public class TreeMap : Item
    {
        private int m_Level;
        private Point3D m_Location;
        private Map m_Facet;
        private string m_TreeType;
        private bool m_Identified;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Location { get { return m_Location; } set { m_Location = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map Facet { get { return m_Facet; } set { m_Facet = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string TreeType { get { return m_TreeType; } set { m_TreeType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified { get { return m_Identified; } set { m_Identified = value; } }

        [Constructable]
        public TreeMap(int level) : base(0x14EC)
        {
            m_Level = level;
            Name = "Mysterious Tree Map";
            LootType = LootType.Blessed;
            Visible = true;
            m_Identified = false;
        }

        public TreeMap(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_Identified)
            {
                from.SendMessage(String.Format("This map shows a {0} located at {1} on {2}.", m_TreeType, m_Location, m_Facet));
            }
            else
            {
                if (CheckIdentify(from))
                {
                    OnIdentified(from);
                    from.SendMessage(String.Format("This map shows a {0} located at {1} on {2}.", m_TreeType, m_Location, m_Facet));
                }
                else
                {
                    from.SendMessage("This map appears to show the location of a special tree, but you can't make out the details.");
                }
            }
        }

        public bool CheckIdentify(Mobile m)
        {
            if (m_Identified)
                return false;

            double lumberjackingSkill = m.Skills[SkillName.Lumberjacking].Value;
            double difficulty = 50.0 + (m_Level * 5); // Adjust this formula as needed

            return (lumberjackingSkill >= difficulty);
        }

        public void OnIdentified(Mobile m)
        {
            m_Identified = true;
            m.SendLocalizedMessage(500819); // You have successfully identified the item.
            m.SendMessage("You have identified a Tree Map!");
            Name = String.Format("{0} Tree Map", m_TreeType);

            // Spawn the tree when identified
            TreeMapSystem.SpawnTree(this, m);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Level);
            writer.Write(m_Location);
            writer.Write(m_Facet);
            writer.Write(m_TreeType);
            writer.Write(m_Identified);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Level = reader.ReadInt();
            m_Location = reader.ReadPoint3D();
            m_Facet = reader.ReadMap();
            m_TreeType = reader.ReadString();
            m_Identified = reader.ReadBool();
        }
    }

    public class SpecialTree : Item
    {
        private string m_TreeType;
        private int m_RemainingWood;

        [CommandProperty(AccessLevel.GameMaster)]
        public string TreeType { get { return m_TreeType; } set { m_TreeType = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RemainingWood { get { return m_RemainingWood; } set { m_RemainingWood = value; UpdateName(); } }

        [Constructable]
        public SpecialTree(string treeType, int amount) : base(0x0D70)
        {
            m_TreeType = treeType;
            m_RemainingWood = amount;
            UpdateName();
            Movable = false;
        }

        public SpecialTree(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            if (m_RemainingWood > 0)
            {
                from.SendMessage("Select your axe.");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendMessage("This tree has been fully harvested.");
            }
        }

        private class InternalTarget : Target
        {
            private SpecialTree m_Tree;

            public InternalTarget(SpecialTree tree) : base(2, false, TargetFlags.None)
            {
                m_Tree = tree;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item && ((Item)targeted).IsChildOf(from.Backpack))
                {
                    Item item = (Item)targeted;
                    if (item is BaseAxe)
                    {
                        m_Tree.Chop(from);
						from.SendMessage("You begin chopping the special tree.");
                    }
                    else
                    {
                        from.SendMessage("You must target an axe.");
                    }
                }
                else
                {
                    from.SendMessage("You must target an axe in your backpack.");
                }
            }
        }

        public void Chop(Mobile from)
        {
            if (m_RemainingWood > 0)
            {
                from.Animate(11, 5, 1, true, false, 0); // Chopping animation
                Timer.DelayCall(TimeSpan.FromSeconds(3), delegate() { ContinueChopping(from); });
            }
        }

        public void ContinueChopping(Mobile from)
        {
            if (from == null || from.Deleted || !from.InRange(this.GetWorldLocation(), 2) || !from.Alive || from.Mounted)
                return;

            if (m_RemainingWood > 0)
            {
                from.Animate(11, 5, 1, true, false, 0); // Chopping animation
                int chopped = Utility.Random(10, 50); // Random amount chopped
                chopped = Math.Min(chopped, m_RemainingWood);

                Type logType = ScriptCompiler.FindTypeByName(String.Format("{0}Log", m_TreeType));
                if (logType != null)
                {
                    Item log = (Item)Activator.CreateInstance(logType);
                    log.Amount = chopped;
                    if (!from.AddToBackpack(log))
                    {
                        log.Delete();
                        from.SendMessage("Your backpack is full.");
                    }
                    else
                    {
                        from.SendMessage(String.Format("You chop {0} {1} logs.", chopped, m_TreeType.ToLower()));
                        m_RemainingWood -= chopped;
                        UpdateName();
                    }
                }
                else
                {
                    from.SendMessage("There was an error creating the log.");
                }

                if (m_RemainingWood <= 0)
                {
                    from.SendMessage("The special tree has been fully harvested.");
                    this.Delete();
                }
                else
                {
                    if (Utility.RandomDouble() < 0.1) // 10% chance to spawn more guardians
                    {
                        TreeMapSystem.SpawnGuardians(this);
                    }

                    Timer.DelayCall(TimeSpan.FromSeconds(3), delegate() { ContinueChopping(from); }); // Continue chopping every 3 seconds
                }
            }
        }

        public void UpdateName()
        {
            Name = String.Format("{0} Tree ({1} wood remaining)", m_TreeType, m_RemainingWood);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_TreeType);
            writer.Write(m_RemainingWood);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_TreeType = reader.ReadString();
            m_RemainingWood = reader.ReadInt();
            UpdateName();
        }
    }

    public class TreeMapSystem
    {
        private static Dictionary<Map, List<Point3D>> s_LocationsByFacet = new Dictionary<Map, List<Point3D>>()
        {
            { Map.Felucca, new List<Point3D> { new Point3D(3413, 236, 0), new Point3D(3359, 410, 0), new Point3D(3390, 479, 4), new Point3D(3302, 582, 0), new Point3D(3409, 628, 0), new Point3D(3476, 563, 0), new Point3D(3159, 710, 0), new Point3D(3158, 645, 8), new Point3D(3110, 578, 5), new Point3D(3151, 454, 4), new Point3D(3250, 370, 0), new Point3D(3241, 253, 4), new Point3D(3413, 236, 0), new Point3D(3183, 394, 4), new Point3D(3097, 507, 0), new Point3D(3159, 709, 5), new Point3D(2993, 569, 0), new Point3D(2887, 442, 10), new Point3D(3006, 245, 0), new Point3D(3087, 85, 4), new Point3D(2911, 90, 0), new Point3D(2865, 168, 0), new Point3D(2777, 340, 15), new Point3D(2763, 458, 16), new Point3D(2683, 468, 15), new Point3D(2662, 362, 16), new Point3D(2575, 303, 35), new Point3D(2583, 236, 0), new Point3D(2457, 228, 0), new Point3D(2632, 657, 0), new Point3D(2666, 706, 0), new Point3D(2787, 679, 0), new Point3D(2719, 934, 0), new Point3D(2631, 1092, 0), new Point3D(2567, 1063, 5), new Point3D(2535, 1203, 0), new Point3D(2408, 1148, 5), new Point3D(2364, 1024, 0), new Point3D(2295, 953, 0), new Point3D(2288, 838, 0), new Point3D(2356, 694, 0), new Point3D(2323, 587, 0), new Point3D(2087, 684, 0), new Point3D(2051, 554, 0), new Point3D(1939, 698, 0), new Point3D(1916, 594, 0), new Point3D(1816, 686, 0), new Point3D(1984, 363, 3), new Point3D(2115, 212, 0), new Point3D(1961, 56, 8), new Point3D(1711, 247, 16), new Point3D(1799, 462, 0), new Point3D(1659, 591, 16), new Point3D(1610, 497, 16), new Point3D(1532, 715, 22), new Point3D(1531, 831, 0), new Point3D(1543, 1007, 0), new Point3D(1527, 1070, 0), new Point3D(1351, 292, 16), new Point3D(1221, 325, 17), new Point3D(1183, 219, 22), new Point3D(981, 368, 0), new Point3D(841, 368, 0), new Point3D(1039, 641, 6), new Point3D(1104, 794, 0), new Point3D(1141, 1030, 0), new Point3D(966, 900, 0), new Point3D(887, 833, 0), new Point3D(757, 692, 0), new Point3D(754, 833, 0), new Point3D(876, 954, 0), new Point3D(887, 1078, 0), new Point3D(926, 1177, 0), new Point3D(1033, 1402, 0), new Point3D(1882, 1050, 0), new Point3D(1919, 1296, 0), new Point3D(1965, 1411, 0), new Point3D(1983, 1521, 0), new Point3D(1782, 1509, 0), new Point3D(1701, 1361, 0), new Point3D(1521, 1299, 0), new Point3D(1326, 1366, 0), new Point3D(1211, 1482, 0), new Point3D(702, 1328, 0), new Point3D(512, 1426, 0), new Point3D(327, 1350, 0), new Point3D(184, 1440, 0), new Point3D(311, 1501, 0), new Point3D(375, 1563, 0), new Point3D(426, 1633, 0), new Point3D(550, 1644, 0), new Point3D(604, 1734, 0), new Point3D(993, 1684, 0), new Point3D(858, 1792, 0), new Point3D(808, 1511, 0), new Point3D(907, 1623, 0), new Point3D(668, 1826, 0), new Point3D(803, 1931, 0), new Point3D(926, 1982, 0), new Point3D(952, 2094, 6), new Point3D(1105, 2128, 5), new Point3D(1436, 1917, 0), new Point3D(1492, 1980, 0), new Point3D(1355, 2124, 0), new Point3D(1136, 2342, 5), new Point3D(966, 2521, 10), new Point3D(1044, 2647, 5), new Point3D(1188, 2499, 0), new Point3D(1311, 2383, 0), new Point3D(1489, 2266, 5), new Point3D(1608, 2158, 5), new Point3D(1658, 2252, 5), new Point3D(1546, 2356, 5), new Point3D(1440, 2478, 0), new Point3D(1355, 2536, 0), new Point3D(1202, 2700, 0), new Point3D(1362, 2772, 0), new Point3D(1490, 2632, 10), new Point3D(1612, 2519, 0), new Point3D(1768, 2399, 5), new Point3D(2101, 2107, 0), new Point3D(2174, 2144, 0), new Point3D(1755, 2539, 5), new Point3D(1650, 2648, 5), new Point3D(1540, 2736, 5), new Point3D(1373, 2912, 0), new Point3D(1065, 3182, 0), new Point3D(1409, 3134, 0), new Point3D(1551, 3020, 0), new Point3D(1676, 2897, 0), new Point3D(1751, 2818, 0), new Point3D(1782, 2947, 0), new Point3D(1656, 3110, 0), new Point3D(1536, 3231, 0), new Point3D(4347, 3692, 0), new Point3D(1100, 1100, 0), new Point3D(1599, 3410, 0), new Point3D(1730, 3336, 3), new Point3D(1854, 3263, 10), new Point3D(2027, 3129, 0), new Point3D(2112, 3000, 30), new Point3D(2092, 3189, 0), new Point3D(2002, 3294, 0), new Point3D(1911, 3401, 0), new Point3D(2079, 3376, 0), new Point3D(2186, 3381, 0), new Point3D(2114, 3594, 10), new Point3D(1144, 3507, 0), new Point3D(2146, 3956, 3), new Point3D(2482, 3952, 3), new Point3D(2800, 3532, 0), new Point3D(4181, 3711, 0), new Point3D(4492, 3748, 0), new Point3D(4521, 3923, 0), new Point3D(4642, 3607, 40), new Point3D(3415, 2722, 44), new Point3D(3588, 2149, 64), new Point3D(2959, 2182, 51), new Point3D(2751, 2299, 6), new Point3D(2681, 2021, 0), new Point3D(4479, 1444, 8) } },
            { Map.Trammel, new List<Point3D> { new Point3D(3413, 236, 0), new Point3D(3359, 410, 0), new Point3D(3390, 479, 4), new Point3D(3302, 582, 0), new Point3D(3409, 628, 0), new Point3D(3476, 563, 0), new Point3D(3159, 710, 0), new Point3D(3158, 645, 8), new Point3D(3110, 578, 5), new Point3D(3151, 454, 4), new Point3D(3250, 370, 0), new Point3D(3241, 253, 4), new Point3D(3413, 236, 0), new Point3D(3183, 394, 4), new Point3D(3097, 507, 0), new Point3D(3159, 709, 5), new Point3D(2993, 569, 0), new Point3D(2887, 442, 10), new Point3D(3006, 245, 0), new Point3D(3087, 85, 4), new Point3D(2911, 90, 0), new Point3D(2865, 168, 0), new Point3D(2777, 340, 15), new Point3D(2763, 458, 16), new Point3D(2683, 468, 15), new Point3D(2662, 362, 16), new Point3D(2575, 303, 35), new Point3D(2583, 236, 0), new Point3D(2457, 228, 0), new Point3D(2632, 657, 0), new Point3D(2666, 706, 0), new Point3D(2787, 679, 0), new Point3D(2719, 934, 0), new Point3D(2631, 1092, 0), new Point3D(2567, 1063, 5), new Point3D(2535, 1203, 0), new Point3D(2408, 1148, 5), new Point3D(2364, 1024, 0), new Point3D(2295, 953, 0), new Point3D(2288, 838, 0), new Point3D(2356, 694, 0), new Point3D(2323, 587, 0), new Point3D(2087, 684, 0), new Point3D(2051, 554, 0), new Point3D(1939, 698, 0), new Point3D(1916, 594, 0), new Point3D(1816, 686, 0), new Point3D(1984, 363, 3), new Point3D(2115, 212, 0), new Point3D(1961, 56, 8), new Point3D(1711, 247, 16), new Point3D(1799, 462, 0), new Point3D(1659, 591, 16), new Point3D(1610, 497, 16), new Point3D(1532, 715, 22), new Point3D(1531, 831, 0), new Point3D(1543, 1007, 0), new Point3D(1527, 1070, 0), new Point3D(1351, 292, 16), new Point3D(1221, 325, 17), new Point3D(1183, 219, 22), new Point3D(981, 368, 0), new Point3D(841, 368, 0), new Point3D(1039, 641, 6), new Point3D(1104, 794, 0), new Point3D(1141, 1030, 0), new Point3D(966, 900, 0), new Point3D(887, 833, 0), new Point3D(757, 692, 0), new Point3D(754, 833, 0), new Point3D(876, 954, 0), new Point3D(887, 1078, 0), new Point3D(926, 1177, 0), new Point3D(1033, 1402, 0), new Point3D(1882, 1050, 0), new Point3D(1919, 1296, 0), new Point3D(1965, 1411, 0), new Point3D(1983, 1521, 0), new Point3D(1782, 1509, 0), new Point3D(1701, 1361, 0), new Point3D(1521, 1299, 0), new Point3D(1326, 1366, 0), new Point3D(1211, 1482, 0), new Point3D(702, 1328, 0), new Point3D(512, 1426, 0), new Point3D(327, 1350, 0), new Point3D(184, 1440, 0), new Point3D(311, 1501, 0), new Point3D(375, 1563, 0), new Point3D(426, 1633, 0), new Point3D(550, 1644, 0), new Point3D(604, 1734, 0), new Point3D(993, 1684, 0), new Point3D(858, 1792, 0), new Point3D(808, 1511, 0), new Point3D(907, 1623, 0), new Point3D(668, 1826, 0), new Point3D(803, 1931, 0), new Point3D(926, 1982, 0), new Point3D(952, 2094, 6), new Point3D(1105, 2128, 5), new Point3D(1436, 1917, 0), new Point3D(1492, 1980, 0), new Point3D(1355, 2124, 0), new Point3D(1136, 2342, 5), new Point3D(966, 2521, 10), new Point3D(1044, 2647, 5), new Point3D(1188, 2499, 0), new Point3D(1311, 2383, 0), new Point3D(1489, 2266, 5), new Point3D(1608, 2158, 5), new Point3D(1658, 2252, 5), new Point3D(1546, 2356, 5), new Point3D(1440, 2478, 0), new Point3D(1355, 2536, 0), new Point3D(1202, 2700, 0), new Point3D(1362, 2772, 0), new Point3D(1490, 2632, 10), new Point3D(1612, 2519, 0), new Point3D(1768, 2399, 5), new Point3D(2101, 2107, 0), new Point3D(2174, 2144, 0), new Point3D(1755, 2539, 5), new Point3D(1650, 2648, 5), new Point3D(1540, 2736, 5), new Point3D(1373, 2912, 0), new Point3D(1065, 3182, 0), new Point3D(1409, 3134, 0), new Point3D(1551, 3020, 0), new Point3D(1676, 2897, 0), new Point3D(1751, 2818, 0), new Point3D(1782, 2947, 0), new Point3D(1656, 3110, 0), new Point3D(1536, 3231, 0), new Point3D(4347, 3692, 0), new Point3D(1100, 1100, 0), new Point3D(1599, 3410, 0), new Point3D(1730, 3336, 3), new Point3D(1854, 3263, 10), new Point3D(2027, 3129, 0), new Point3D(2112, 3000, 30), new Point3D(2092, 3189, 0), new Point3D(2002, 3294, 0), new Point3D(1911, 3401, 0), new Point3D(2079, 3376, 0), new Point3D(2186, 3381, 0), new Point3D(2114, 3594, 10), new Point3D(1144, 3507, 0), new Point3D(2146, 3956, 3), new Point3D(2482, 3952, 3), new Point3D(2800, 3532, 0), new Point3D(4181, 3711, 0), new Point3D(4492, 3748, 0), new Point3D(4521, 3923, 0), new Point3D(4642, 3607, 40), new Point3D(3415, 2722, 44), new Point3D(3588, 2149, 64), new Point3D(2959, 2182, 51), new Point3D(2751, 2299, 6), new Point3D(2681, 2021, 0), new Point3D(4479, 1444, 8) } },
            // Add more predefined locations for other facets here
        };

        public static void CreateTreeMap(Mobile from)
        {
            int level = CalculateMapLevel(from);
            TreeMap map = new TreeMap(level);

            map.TreeType = GetRandomTreeType(level);
            map.Facet = Utility.RandomBool() ? Map.Felucca : Map.Trammel;
            map.Location = GetRandomLocation(map.Facet);

            if (from.AddToBackpack(map))
            {
                from.SendMessage("You've received a mysterious tree map. Use Lumberjacking skill to identify it.");
            }
            else
            {
                map.Delete();
                from.SendMessage("You find a tree map, but have no room in your backpack to keep it.");
            }
        }

        private static int CalculateMapLevel(Mobile from)
        {
            int lumberjackingSkill = from.Skills[SkillName.Lumberjacking].Fixed / 100; // 0 to 100
            return Clamp(lumberjackingSkill / 20, 1, 5);
        }

        private static string GetRandomTreeType(int level)
        {
            string[] treeTypes = new string[]
            {
                "Oak", "Ash", "Yew", "Heartwood", "Bloodwood",
                "Frostwood"
            };

            return treeTypes[Utility.Random(Math.Min(level * 2, treeTypes.Length))];
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

        public static void SpawnTree(TreeMap map, Mobile lumberjack)
        {
            int lumberjackingSkill = lumberjack.Skills[SkillName.Lumberjacking].Fixed / 10; // 0 to 1000
            int woodAmount = Utility.Random(50, 451) + lumberjackingSkill;
            woodAmount = Math.Min(woodAmount, 500); // Max 500

            SpecialTree tree = new SpecialTree(map.TreeType, woodAmount);
            tree.MoveToWorld(map.Location, map.Facet);

            SpawnGuardians(tree);
        }

        public static void SpawnGuardians(SpecialTree tree)
        {
            int guardianCount = Utility.Random(2, 4);
            for (int i = 0; i < guardianCount; i++)
            {
                BaseCreature guardian = CreateGuardian(tree.TreeType);
                Point3D loc = tree.Location;
                for (int j = 0; j < 10; j++)
                {
                    int x = loc.X + Utility.Random(-3, 7);
                    int y = loc.Y + Utility.Random(-3, 7);
                    int z = tree.Map.GetAverageZ(x, y);

                    if (tree.Map.CanSpawnMobile(x, y, z))
                    {
                        guardian.MoveToWorld(new Point3D(x, y, z), tree.Map);
                        break;
                    }
                }
            }
        }

        private static BaseCreature CreateGuardian(string treeType)
        {
            switch (treeType)
            {
                case "Oak":
                    return new EarthElemental();
                case "Ash":
                case "Yew":
                    return new TreeElemental();
                case "Heartwood":
                case "Bloodwood":
                    return new TreeElemental();
                case "Frostwood":
                    return new IceElemental();
                case "Eucalyptus":
                case "Cherry":
                case "Mahogany":
                    return new Treefellow();
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

    public class TreeElemental : BaseCreature
    {
        [Constructable]
        public TreeElemental() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a tree elemental";
            Body = 301;
            BaseSoundID = 442;

            SetStr(226, 255);
            SetDex(126, 145);
            SetInt(71, 92);

            SetHits(136, 153);

            SetDamage(9, 16);

            SetDamageType(ResistanceType.Physical, 100);
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

        public TreeElemental(Serial serial) : base(serial)
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

    public class TreeMapGump : Gump
    {
        private readonly Mobile m_From;

        public TreeMapGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            AddPage(0);
            AddBackground(0, 0, 200, 200, 5054);
            AddLabel(20, 20, 0, "Tree Map System");
            AddButton(20, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(55, 50, 0, "Create Tree Map");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                TreeMapSystem.CreateTreeMap(m_From);
            }
        }
    }
}