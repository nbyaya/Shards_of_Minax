using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ChaosWyrmHuntsmanQuest : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        public override object Title { get { return "The Chaos Wyrm's Bane"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, courageous adventurer! A dreadful Chaos Wyrm has been spotted in our lands. " +
                    "Your mission is to seek out and vanquish this fearsome creature. " +
                    "I have provided you with a special compass that will lead you to its last known location. " +
                    "The Chaos Wyrm might wander, so be vigilant and use your tracking skills wisely. " +
                    "Return to me once the beast has been defeated, and you shall be richly rewarded!";
            }
        }

        public override object Refuse { get { return "I understand. This quest is not for everyone."; } }

        public override object Uncomplete { get { return "The Chaos Wyrm still lurks somewhere out there. Have you given up?"; } }

        public override object Complete { get { return "Fantastic! You have slain the Chaos Wyrm. Your name will be celebrated among the greatest of hunters!"; } }

        private static List<Point3D> SpawnLocations = new List<Point3D>
        {
            new Point3D(3413, 236, 0), new Point3D(3359, 410, 0), new Point3D(3390, 479, 4), new Point3D(3302, 582, 0), new Point3D(3409, 628, 0), new Point3D(3476, 563, 0), new Point3D(3159, 710, 0), new Point3D(3158, 645, 8), new Point3D(3110, 578, 5), new Point3D(3151, 454, 4), new Point3D(3250, 370, 0), new Point3D(3241, 253, 4), new Point3D(3413, 236, 0), new Point3D(3183, 394, 4), new Point3D(3097, 507, 0), new Point3D(3159, 709, 5), new Point3D(2993, 569, 0), new Point3D(2887, 442, 10), new Point3D(3006, 245, 0), new Point3D(3087, 85, 4), new Point3D(2911, 90, 0), new Point3D(2865, 168, 0), new Point3D(2777, 340, 15), new Point3D(2763, 458, 16), new Point3D(2683, 468, 15), new Point3D(2662, 362, 16), new Point3D(2575, 303, 35), new Point3D(2583, 236, 0), new Point3D(2457, 228, 0), new Point3D(2632, 657, 0), new Point3D(2666, 706, 0), new Point3D(2787, 679, 0), new Point3D(2719, 934, 0), new Point3D(2631, 1092, 0), new Point3D(2567, 1063, 5), new Point3D(2535, 1203, 0), new Point3D(2408, 1148, 5), new Point3D(2364, 1024, 0), new Point3D(2295, 953, 0), new Point3D(2288, 838, 0), new Point3D(2356, 694, 0), new Point3D(2323, 587, 0), new Point3D(2087, 684, 0), new Point3D(2051, 554, 0), new Point3D(1939, 698, 0), new Point3D(1916, 594, 0), new Point3D(1816, 686, 0), new Point3D(1984, 363, 3), new Point3D(2115, 212, 0), new Point3D(1961, 56, 8), new Point3D(1711, 247, 16), new Point3D(1799, 462, 0), new Point3D(1659, 591, 16), new Point3D(1610, 497, 16), new Point3D(1532, 715, 22), new Point3D(1531, 831, 0), new Point3D(1543, 1007, 0), new Point3D(1527, 1070, 0), new Point3D(1351, 292, 16), new Point3D(1221, 325, 17), new Point3D(1183, 219, 22), new Point3D(981, 368, 0), new Point3D(841, 368, 0), new Point3D(1039, 641, 6), new Point3D(1104, 794, 0), new Point3D(1141, 1030, 0), new Point3D(966, 900, 0), new Point3D(887, 833, 0), new Point3D(757, 692, 0), new Point3D(754, 833, 0), new Point3D(876, 954, 0), new Point3D(887, 1078, 0), new Point3D(926, 1177, 0), new Point3D(1033, 1402, 0), new Point3D(1882, 1050, 0), new Point3D(1919, 1296, 0), new Point3D(1965, 1411, 0), new Point3D(1983, 1521, 0), new Point3D(1782, 1509, 0), new Point3D(1701, 1361, 0), new Point3D(1521, 1299, 0), new Point3D(1326, 1366, 0), new Point3D(1211, 1482, 0), new Point3D(702, 1328, 0), new Point3D(512, 1426, 0), new Point3D(327, 1350, 0), new Point3D(184, 1440, 0), new Point3D(311, 1501, 0), new Point3D(375, 1563, 0), new Point3D(426, 1633, 0), new Point3D(550, 1644, 0), new Point3D(604, 1734, 0), new Point3D(993, 1684, 0), new Point3D(858, 1792, 0), new Point3D(808, 1511, 0), new Point3D(907, 1623, 0), new Point3D(668, 1826, 0), new Point3D(803, 1931, 0), new Point3D(926, 1982, 0), new Point3D(952, 2094, 6), new Point3D(1105, 2128, 5), new Point3D(1436, 1917, 0), new Point3D(1492, 1980, 0), new Point3D(1355, 2124, 0), new Point3D(1136, 2342, 5), new Point3D(966, 2521, 10), new Point3D(1044, 2647, 5), new Point3D(1188, 2499, 0), new Point3D(1311, 2383, 0), new Point3D(1489, 2266, 5), new Point3D(1608, 2158, 5), new Point3D(1658, 2252, 5), new Point3D(1546, 2356, 5), new Point3D(1440, 2478, 0), new Point3D(1355, 2536, 0), new Point3D(1202, 2700, 0), new Point3D(1362, 2772, 0), new Point3D(1490, 2632, 10), new Point3D(1612, 2519, 0), new Point3D(1768, 2399, 5), new Point3D(2101, 2107, 0), new Point3D(2174, 2144, 0), new Point3D(1755, 2539, 5), new Point3D(1650, 2648, 5), new Point3D(1540, 2736, 5), new Point3D(1373, 2912, 0), new Point3D(1065, 3182, 0), new Point3D(1409, 3134, 0), new Point3D(1551, 3020, 0), new Point3D(1676, 2897, 0), new Point3D(1751, 2818, 0), new Point3D(1782, 2947, 0), new Point3D(1656, 3110, 0), new Point3D(1536, 3231, 0), new Point3D(4347, 3692, 0), new Point3D(1100, 1100, 0), new Point3D(1599, 3410, 0), new Point3D(1730, 3336, 3), new Point3D(1854, 3263, 10), new Point3D(2027, 3129, 0), new Point3D(2112, 3000, 30), new Point3D(2092, 3189, 0), new Point3D(2002, 3294, 0), new Point3D(1911, 3401, 0), new Point3D(2079, 3376, 0), new Point3D(2186, 3381, 0), new Point3D(2114, 3594, 10), new Point3D(1144, 3507, 0), new Point3D(2146, 3956, 3), new Point3D(2482, 3952, 3), new Point3D(2800, 3532, 0), new Point3D(4181, 3711, 0), new Point3D(4492, 3748, 0), new Point3D(4521, 3923, 0), new Point3D(4642, 3607, 40), new Point3D(3415, 2722, 44), new Point3D(3588, 2149, 64), new Point3D(2959, 2182, 51), new Point3D(2751, 2299, 6), new Point3D(2681, 2021, 0), new Point3D(4479, 1444, 8)
            // Add more locations as needed
        };

        private static List<Map> SpawnFacets = new List<Map>
        {
            Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca,
			Map.Felucca

            // Ensure this list matches the SpawnLocations list
        };

        private ChaosWyrm m_ChaosWyrm;
        private ChaosHuntCompass m_Compass;

        public ChaosWyrmHuntsmanQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ChaosWyrm), "Chaos Wyrm", 1));

            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CelestialCleaver), 1, "Celestial Cleaver"));
        }

        public override void OnAccept()
        {
            base.OnAccept();

            int index = Utility.Random(SpawnLocations.Count);
            Point3D location = SpawnLocations[index];
            Map facet = SpawnFacets[index];

            m_ChaosWyrm = new ChaosWyrm();
            m_ChaosWyrm.MoveToWorld(location, facet);
            m_ChaosWyrm.Home = location;
            m_ChaosWyrm.RangeHome = 10;

            m_Compass = new ChaosHuntCompass();
            m_Compass.TargetLocation = location;
            m_Compass.TargetMap = facet;

            Owner.AddToBackpack(m_Compass);
            Owner.SendMessage("A special compass has been added to your backpack. Use it to locate the Chaos Wyrm.");
        }

        public override void OnCompleted()
        {
            Owner.SendMessage("Congratulations! You have completed the Chaos Wyrm's Bane quest!");
            Owner.PlaySound(CompleteSound);
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
        }
    }

    public class ChaosHuntCompass : Item
    {
        private Point3D m_TargetLocation;
        private Map m_TargetMap;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TargetLocation
        {
            get { return m_TargetLocation; }
            set { m_TargetLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap
        {
            get { return m_TargetMap; }
            set { m_TargetMap = value; }
        }

        [Constructable]
        public ChaosHuntCompass() : base(0x1878)
        {
            Name = "Chaos Hunt Compass";
            LootType = LootType.Blessed;
        }

        public ChaosHuntCompass(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage(String.Format("The compass points to: {0} ({1}, {2}, {3})", 
                m_TargetMap, m_TargetLocation.X, m_TargetLocation.Y, m_TargetLocation.Z));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_TargetLocation);
            writer.Write(m_TargetMap);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_TargetLocation = reader.ReadPoint3D();
            m_TargetMap = reader.ReadMap();
        }
    }

    public class ChaosWyrmHuntsman : MondainQuester
    {
        [Constructable]
        public ChaosWyrmHuntsman() : base("Chaos Wyrm Huntsman", "")
        {
        }

        public ChaosWyrmHuntsman(Serial serial) : base(serial)
        {
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[] 
                {
                    typeof(ChaosWyrmHuntsmanQuest)
                };
            }
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
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
        }
    }
}
