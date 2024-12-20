using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;
using Server.Network;
using Server.Custom;

namespace Server.Items
{
    public class ForensicClue : Item, IRevealableItem
    {
        private int m_Level;
        private Point3D m_NextLocation;
        private Map m_NextFacet;
        private Mobile m_Criminal;
        private bool m_Revealed;
		public bool CheckWhenHidden { get { return true; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D NextLocation { get { return m_NextLocation; } set { m_NextLocation = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map NextFacet { get { return m_NextFacet; } set { m_NextFacet = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Criminal { get { return m_Criminal; } set { m_Criminal = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Revealed { get { return m_Revealed; } set { m_Revealed = value; } }

        [Constructable]
        public ForensicClue(int level) : base(0x14ED)
        {
            m_Level = level;
            Name = "Forensic Clue";
            LootType = LootType.Blessed;
            Visible = false;
            m_Revealed = false;
        }

        public ForensicClue(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack) && !m_Revealed)
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_Revealed)
            {
                from.SendMessage(string.Format("You examine the forensic clue. It points to: {0} on {1}", m_NextLocation, m_NextFacet));
            }
            else
            {
                from.SendMessage("You must reveal this clue using the Detect Hidden skill before you can examine it.");
            }
        }

        public bool CheckReveal(Mobile m)
        {
            if (m_Revealed)
                return false;

            double detectSkill = m.Skills[SkillName.DetectHidden].Value;
            double difficulty = 40 + (m_Level * 5); // Adjust this formula as needed

            return (detectSkill >= difficulty);
        }

        public bool CheckPassiveDetect(Mobile m)
        {
            if (m_Revealed)
                return false;

            double detectSkill = m.Skills[SkillName.DetectHidden].Value;
            double difficulty = 100 + (m_Level * 10); // Make passive detection more difficult

            return (detectSkill >= difficulty);
        }

        public void OnRevealed(Mobile m)
        {
            m_Revealed = true;
            Visible = true;
            m.SendLocalizedMessage(1153493); // Your keen senses detect something hidden in the area...
            m.SendMessage("You have revealed a Forensic Clue!");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version

            writer.Write(m_Level);
            writer.Write(m_NextLocation);
            writer.Write(m_NextFacet);
            writer.Write(m_Criminal);
            writer.Write(m_Revealed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Level = reader.ReadInt();
            m_NextLocation = reader.ReadPoint3D();
            if (version >= 1)
                m_NextFacet = reader.ReadMap();
            m_Criminal = reader.ReadMobile();
            if (version >= 2)
                m_Revealed = reader.ReadBool();
        }
    }

    public class ForensicEvaluationSystem
    {
        private static readonly int ChestGuardians = 3;

        public static void StartInvestigation(Mobile from)
        {
            int maxClues = CalculateMaxClues(from);
            List<Point3D> locations = new List<Point3D>();
            List<Map> facets = new List<Map>();
            GetRandomLocations(maxClues + 1, locations, facets); // +1 for final location
            Mobile criminal = CreateCriminal();

            ForensicClue firstClue = new ForensicClue(1);
            firstClue.NextLocation = locations[1];
            firstClue.NextFacet = facets[1];
            firstClue.Criminal = criminal;
            firstClue.Revealed = true; // First clue is always revealed

            if (from.AddToBackpack(firstClue))
            {
                from.SendMessage("You've found your first forensic clue. Examine it to start your investigation.");
            }
            else
            {
                firstClue.Delete();
                from.SendMessage("You find a clue, but have no room in your backpack to keep it.");
            }

            for (int i = 1; i < maxClues; i++)
            {
                ForensicClue clue = new ForensicClue(i + 1);
                clue.NextLocation = i < maxClues - 1 ? locations[i + 1] : locations[maxClues];
                clue.NextFacet = i < maxClues - 1 ? facets[i + 1] : facets[maxClues];
                clue.Criminal = criminal;
                clue.MoveToWorld(locations[i], facets[i]);
            }

            // Create and place the final chest
            MurderTreasureChest chest = new MurderTreasureChest();
            chest.MoveToWorld(locations[maxClues], facets[maxClues]);

            // Spawn PKMurdererLord next to the chest
            PKMurdererLord murderer = new PKMurdererLord();
            murderer.MoveToWorld(chest.Location, chest.Map);
        }

        private static int CalculateMaxClues(Mobile from)
        {
            int forensicsSkill = from.Skills[SkillName.Forensics].Fixed / 10; // 0 to 1000 -> 0 to 100
            int minClues = 2;
            int maxClues = 3;

            return Math.Max(3, Math.Min(10, 200 / forensicsSkill));
        }

        private static void GetRandomLocations(int count, List<Point3D> locations, List<Map> facets)
        {
            List<Point3D> possibleLocations = new List<Point3D>();
            List<Map> possibleFacets = new List<Map>();

            // Felucca locations
			possibleLocations.Add(new Point3D(1553, 1426, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1475, 1513, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1612, 1515, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1633, 1511, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1618, 1564, 50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1650, 1545, 1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1666, 1547, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1694, 1546, 65)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1708, 1562, 50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1675, 1570, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1657, 1577, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1650, 1562, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1642, 577, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1617, 1584, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1588, 1596, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1589, 1555, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1542, 1594, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1548, 1650, 26)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1545, 1681, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1596, 1651, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1658, 1641, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1636, 1707, 36)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1606, 1706, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1577, 1708, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1546, 1706, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1538, 1737, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1569, 1743, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1616, 1760, 100)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1564, 1761, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1376, 1447, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1348, 1500, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1418, 1555, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1438, 1550, 50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1466, 1563, 51)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1453, 1577, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1417, 1580, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1473, 1578, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1480, 1588, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1480, 1593, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1435, 1593, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1429, 1612, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1409, 1608, 50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1429, 1613, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1449, 1616, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1469, 1609, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1493, 1611, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1509, 1594, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1514, 1611, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1408, 1614, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1345, 1646, 50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1399, 1689, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1425, 1649, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1440, 1649, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1497, 1665, 27)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1484, 1672, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1448, 1661, -10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1424, 1683, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1450, 1691, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1473, 1686, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1491, 1687, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1493, 1718, 7)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1451, 1706, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1460, 1722, 6)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1473, 1774, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1482, 1764, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1436, 1765, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1425, 1753, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1425, 1712, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1410, 1716, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1341, 1731, 80)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1352, 1780, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1319, 1828, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1258, 1882, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1193, 1849, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1131, 1817, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1301, 1773, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1245, 1699, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1186, 1715, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1156, 1617, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1153, 1545, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1225, 1579, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1401, 1801, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1387, 1820, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1406, 1825, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2666, 2075, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2666, 2131, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2675, 2153, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2649, 2194, 4)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2671, 2203, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2688, 2202, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2674, 2243, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2713, 2211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2753, 2219, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2730, 2259, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2731, 2187, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2753, 2160, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2704, 2176, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2705, 2140, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2746, 2114, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2777, 2129, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2723, 2083, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2665, 2131, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2625, 2099, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2632, 2082, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2442, 1086, 28)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2434, 1109, 8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2281, 1188, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2257, 1218, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2241, 1227, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2243, 1201, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2228, 1213, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2226, 1199, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2237, 1184, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2213, 1193, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2209, 1164, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2210, 1112, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1286, 3713, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1278, 3738, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1286, 3746, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1306, 3691, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1327, 3673, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1307, 3706, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1335, 3690, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1336, 3706, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1313, 3727, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1331, 3727, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1307, 3747, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1330, 3752, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1314, 3770, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1346, 3755, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1346, 3778, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1364, 3807, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1378, 3792, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1384, 3779, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1405, 3743, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1389, 3753, -21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1377, 3735, -24)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1353, 3732, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1372, 3704, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1393, 3716, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1411, 3699, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1450, 3714, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1436, 3755, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1459, 3739, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1459, 3771, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1445, 3803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1459, 3797, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1460, 3823, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1425, 3817, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1421, 3859, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1443, 3851, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1444, 3877, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1482, 3849, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1496, 3694, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1169, 3635, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1138, 3657, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1127, 3609, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1171, 3595, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1114, 3596, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1426, 3962, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1436, 3961, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1419, 3978, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1442, 3977, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1481, 3978, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1451, 4011, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1456, 4025, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1411, 4011, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1401, 4034, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1396, 4019, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1449, 4026, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1522, 3989, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2539, 501, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2582, 519, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2590, 535, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2618, 618, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2595, 637, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2599, 627, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2597, 611, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2571, 590, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2543, 636, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2529, 674, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2508, 594, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2535, 581, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2497, 595, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2483, 594, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2522, 547, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2508, 545, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2522, 530, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2468, 558, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2417, 545, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2421, 523, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2508, 468, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2491, 473, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2449, 485, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2421, 497, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2419, 481, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2420, 472, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2453, 453, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2473, 426, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2449, 425, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2436, 436, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2433, 402, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2458, 398, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2489, 399, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2492, 376, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2508, 429, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2436, 530, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2465, 523, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2500, 338, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4552, 1300, 18)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4534, 1318, 8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4494, 1382, 23)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4553, 1491, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4674, 1410, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4666, 1394, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4626, 1321, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4409, 1464, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4625, 1211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4652, 1218, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4692, 1220, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4682, 1179, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4704, 1124, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4489, 1217, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4425, 1218, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4410, 1203, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4395, 1211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4466, 1157, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4440, 1153, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4401, 1154, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4386, 1154, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4385, 1131, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4409, 1139, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4489, 1120, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4419, 1111, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4386, 1107, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4387, 1091, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4416, 1081, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4410, 1058, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4388, 1057, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4384, 1025, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4451, 1059, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4448, 1091, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4474, 1083, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4483, 1057, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4523, 1067, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4557, 1042, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4568, 921, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4528, 950, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4545, 853, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4311, 1002, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4287, 969, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4288, 950, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4283, 923, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4303, 926, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(4315, 923, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3534, 1133, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3508, 1144, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3521, 1152, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3555, 1186, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3595, 1195, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3595, 1202, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3595, 1211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3602, 1210, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3610, 1211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3625, 1210, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3595, 1227, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3602, 1235, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3626, 1227, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3626, 1211, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3626, 1243, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3618, 1250, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3618, 1259, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3619, 1267, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3610, 1259, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3659, 1059, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3666, 1058, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3674, 1058, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3674, 1067, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3675, 1075, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3659, 1075, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3666, 1101, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3658, 1099, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3667, 1108, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3682, 1098, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3690, 1106, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3682, 1107, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3674, 1130, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3666, 1130, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3651, 1139, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3650, 1147, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3666, 1150, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3675, 1147, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3650, 1162, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3650, 1171, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3658, 1170, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3691, 1163, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3699, 1180, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3674, 1203, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3657, 1219, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3691, 1297, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3675, 1323, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3741, 1265, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3733, 1304, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3729, 1337, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3762, 1323, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3728, 1380, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3709, 1384, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3714, 1393, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3730, 1404, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3766, 1263, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3761, 1233, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3733, 1221, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3746, 1186, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3763, 1188, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3779, 1162, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3770, 1122, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3444, 2635, 28)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3445, 2602, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3445, 2586, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3461, 2575, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3457, 2551, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3442, 2558, 55)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3441, 2568, 53)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3457, 2526, 74)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3505, 2516, 47)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3520, 2512, 25)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3523, 2547, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3485, 2489, 71)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3467, 2492, 91)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3510, 22421, 55)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3559, 2461, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3551, 2460, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3555, 2448, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3553, 2456, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3060, 3345, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3050, 3347, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3050, 3369, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3036, 3380, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3030, 3349, 75)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3008, 3350, 75)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2949, 3354, 55)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2937, 3378, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2937, 3415, 1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2978, 3377, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2971, 3402, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2968, 3425, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2954, 3447, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2996, 3425, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3002, 3404, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3010, 3426, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3034, 3426, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3014, 3451, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3031, 3466, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3034, 3427, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3049, 3396, 38)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3050, 3362, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3051, 3348, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3069, 3334, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2932, 3507, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2902, 3515, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2914, 3482, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2875, 3498, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2870, 3462, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(889, 2396, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(867, 2321, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(809, 2332, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(795, 2265, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(745, 2348, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(754, 2264, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(788, 2233, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(786, 2217, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(811, 2186, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(737, 2137, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(612, 2251, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(602, 2284, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(587, 2225, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(584, 2202, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(601, 2179, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(553, 2219, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(570, 2171, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(594, 2163, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(611, 2159, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(626, 2156, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(619, 2116, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(555, 2153, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(571, 2115, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(667, 2149, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(651, 2155, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1840, 2790, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1849, 2759, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1845, 2788, -8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1818, 2815, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1815, 2832, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1802, 2703, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1820, 2668, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1856, 2677, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1846, 2702, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1863, 2643, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1885, 2646, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1891, 2689, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1905, 2706, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1895, 2710, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1923, 2755, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1855, 2825, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1895, 2829, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1918, 2803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1900, 2806, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1878, 2807, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1926, 2789, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1939, 2759, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1935, 2748, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1937, 2711, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1937, 2687, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1947, 2676, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1959, 2673, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1960, 2685, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1992, 2695, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1998, 2716, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2024, 2742, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1964, 2866, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1978, 2831, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1993, 2866, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1992, 2882, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1988, 2897, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1987, 2905, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2018, 2891, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2028, 2857, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2023, 2834, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2035, 2799, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2009, 2808, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2041, 2771, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2057, 2795, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2085, 2792, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2780, 861, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2763, 875, -21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2745, 848, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2741, 980, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2778, 960, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2747, 1065, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2822, 991, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2853, 991, -21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2909, 979, -21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2937, 930, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2912, 931, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2893, 916, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2835, 943, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2834, 904, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2844, 867, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2866, 860, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2848, 833, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2865, 811, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2844, 811, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2891, 787, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2921, 794, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2881, 768, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2893, 739, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2858, 733, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2875, 714, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2876, 677, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2875, 662, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2891, 659, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2914, 675, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2906, 709, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2988, 643, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2955, 627, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2959, 705, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2993, 761, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2985, 779, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3013, 796, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3027, 763, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3027, 827, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2987, 834, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(3002, 812, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2957, 820, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2955, 848, 21)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2963, 866, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2954, 872, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2947, 936, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5211, 169, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5268, 154, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5260, 129, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5315, 105, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5304, 89, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5345, 81, 25)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5350, 53, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5312, 34, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5219, 120, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5202, 89, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5204, 74, 17)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5180, 90, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5160, 95, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5149, 59, 25)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5177, 20, 27)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(728, 1127, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(723, 1116, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(713, 1106, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(684, 1201, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(554, 1218, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(595, 1186, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(627, 1147, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(944, 1091, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(579, 1122, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(636, 1044, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(668, 1001, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(733, 884, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(652, 938, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(612, 978, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(564, 1013, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(556, 986, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(561, 972, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(533, 961, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(515, 997, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(532, 1010, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(459, 1116, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(452, 1131, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(395, 1227, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(331, 1251, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(451, 961, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(428, 1033, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(321, 1059, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(285, 977, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(482, 843, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(387, 915, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(338, 879, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(354, 836, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(319, 790, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(270, 771, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(261, 765, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(524, 768, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(552, 821, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(610, 875, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(626, 821, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(635, 818, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(603, 824, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(651, 812, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5275, 3946, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5264, 3979, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5285, 3980, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5294, 3976, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5310, 3981, 57)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5297, 4007, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5227, 4005, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5215, 4012, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5233, 4020, 37)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5288, 4057, 57)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5197, 4064, 57)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5759, 3152, 14)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5747, 3190, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5731, 3185, 8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5708, 3205, 11)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5694, 3208, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5736, 3219, 11)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5727, 3245, 16)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5735, 3265, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5752, 3272, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5801, 3267, 33)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5830, 3264, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5811, 3301, 12)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5696, 3284, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5667, 3284, 11)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5659, 3148, 13)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Felucca);

            // Trammel locations
			possibleLocations.Add(new Point3D(1553, 1426, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1475, 1513, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1612, 1515, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1633, 1511, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1618, 1564, 50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1650, 1545, 1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1666, 1547, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1694, 1546, 65)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1708, 1562, 50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1675, 1570, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1657, 1577, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1650, 1562, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1642, 577, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1617, 1584, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1588, 1596, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1589, 1555, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1542, 1594, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1548, 1650, 26)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1545, 1681, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1596, 1651, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1658, 1641, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1636, 1707, 36)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1606, 1706, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1577, 1708, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1546, 1706, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1538, 1737, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1569, 1743, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1616, 1760, 100)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1564, 1761, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1376, 1447, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1348, 1500, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1418, 1555, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1438, 1550, 50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1466, 1563, 51)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1453, 1577, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1417, 1580, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1473, 1578, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1480, 1588, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1480, 1593, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1435, 1593, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1429, 1612, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1409, 1608, 50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1429, 1613, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1449, 1616, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1469, 1609, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1493, 1611, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1509, 1594, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1514, 1611, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1408, 1614, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1345, 1646, 50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1399, 1689, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1425, 1649, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1440, 1649, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1497, 1665, 27)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1484, 1672, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1448, 1661, -10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1424, 1683, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1450, 1691, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1473, 1686, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1491, 1687, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1493, 1718, 7)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1451, 1706, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1460, 1722, 6)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1473, 1774, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1482, 1764, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1436, 1765, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1425, 1753, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1425, 1712, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1410, 1716, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1341, 1731, 80)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1352, 1780, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1319, 1828, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1258, 1882, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1193, 1849, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1131, 1817, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1301, 1773, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1245, 1699, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1186, 1715, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1156, 1617, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1153, 1545, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1225, 1579, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1401, 1801, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1387, 1820, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1406, 1825, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2666, 2075, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2666, 2131, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2675, 2153, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2649, 2194, 4)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2671, 2203, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2688, 2202, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2674, 2243, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2713, 2211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2753, 2219, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2730, 2259, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2731, 2187, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2753, 2160, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2704, 2176, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2705, 2140, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2746, 2114, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2777, 2129, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2723, 2083, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2665, 2131, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2625, 2099, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2632, 2082, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2442, 1086, 28)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2434, 1109, 8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2281, 1188, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2257, 1218, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2241, 1227, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2243, 1201, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2228, 1213, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2226, 1199, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2237, 1184, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2213, 1193, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2209, 1164, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2210, 1112, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1286, 3713, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1278, 3738, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1286, 3746, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1306, 3691, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1327, 3673, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1307, 3706, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1335, 3690, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1336, 3706, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1313, 3727, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1331, 3727, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1307, 3747, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1330, 3752, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1314, 3770, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1346, 3755, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1346, 3778, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1364, 3807, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1378, 3792, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1384, 3779, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1405, 3743, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1389, 3753, -21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1377, 3735, -24)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1353, 3732, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1372, 3704, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1393, 3716, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1411, 3699, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1450, 3714, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1436, 3755, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1459, 3739, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1459, 3771, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1445, 3803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1459, 3797, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1460, 3823, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1425, 3817, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1421, 3859, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1443, 3851, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1444, 3877, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1482, 3849, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1496, 3694, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1169, 3635, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1138, 3657, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1127, 3609, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1171, 3595, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1114, 3596, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1426, 3962, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1436, 3961, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1419, 3978, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1442, 3977, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1481, 3978, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1451, 4011, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1456, 4025, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1411, 4011, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1401, 4034, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1396, 4019, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1449, 4026, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1522, 3989, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2539, 501, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2582, 519, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2590, 535, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2618, 618, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2595, 637, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2599, 627, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2597, 611, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2571, 590, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2543, 636, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2529, 674, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2508, 594, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2535, 581, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2497, 595, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2483, 594, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2522, 547, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2508, 545, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2522, 530, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2468, 558, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2417, 545, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2421, 523, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2508, 468, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2491, 473, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2449, 485, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2421, 497, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2419, 481, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2420, 472, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2453, 453, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2473, 426, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2449, 425, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2436, 436, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2433, 402, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2458, 398, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2489, 399, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2492, 376, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2508, 429, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2436, 530, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2465, 523, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2500, 338, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4552, 1300, 18)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4534, 1318, 8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4494, 1382, 23)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4553, 1491, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4674, 1410, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4666, 1394, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4626, 1321, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4409, 1464, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4625, 1211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4652, 1218, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4692, 1220, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4682, 1179, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4704, 1124, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4489, 1217, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4425, 1218, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4410, 1203, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4395, 1211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4466, 1157, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4440, 1153, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4401, 1154, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4386, 1154, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4385, 1131, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4409, 1139, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4489, 1120, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4419, 1111, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4386, 1107, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4387, 1091, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4416, 1081, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4410, 1058, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4388, 1057, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4384, 1025, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4451, 1059, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4448, 1091, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4474, 1083, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4483, 1057, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4523, 1067, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4557, 1042, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4568, 921, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4528, 950, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4545, 853, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4311, 1002, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4287, 969, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4288, 950, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4283, 923, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4303, 926, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(4315, 923, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3534, 1133, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3508, 1144, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3521, 1152, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3555, 1186, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3595, 1195, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3595, 1202, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3595, 1211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3602, 1210, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3610, 1211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3625, 1210, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3595, 1227, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3602, 1235, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3626, 1227, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3626, 1211, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3626, 1243, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3618, 1250, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3618, 1259, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3619, 1267, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3610, 1259, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3659, 1059, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3666, 1058, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3674, 1058, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3674, 1067, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3675, 1075, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3659, 1075, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3666, 1101, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3658, 1099, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3667, 1108, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3682, 1098, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3690, 1106, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3682, 1107, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3674, 1130, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3666, 1130, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3651, 1139, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3650, 1147, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3666, 1150, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3675, 1147, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3650, 1162, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3650, 1171, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3658, 1170, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3691, 1163, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3699, 1180, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3674, 1203, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3657, 1219, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3691, 1297, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3675, 1323, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3741, 1265, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3733, 1304, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3729, 1337, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3762, 1323, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3728, 1380, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3709, 1384, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3714, 1393, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3730, 1404, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3766, 1263, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3761, 1233, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3733, 1221, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3746, 1186, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3763, 1188, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3779, 1162, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3770, 1122, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3444, 2635, 28)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3445, 2602, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3445, 2586, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3461, 2575, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3457, 2551, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3442, 2558, 55)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3441, 2568, 53)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3457, 2526, 74)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3505, 2516, 47)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3520, 2512, 25)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3523, 2547, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3485, 2489, 71)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3467, 2492, 91)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3510, 22421, 55)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3559, 2461, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3551, 2460, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3555, 2448, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3553, 2456, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3060, 3345, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3050, 3347, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3050, 3369, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3036, 3380, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3030, 3349, 75)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3008, 3350, 75)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2949, 3354, 55)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2937, 3378, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2937, 3415, 1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2978, 3377, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2971, 3402, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2968, 3425, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2954, 3447, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2996, 3425, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3002, 3404, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3010, 3426, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3034, 3426, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3014, 3451, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3031, 3466, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3034, 3427, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3049, 3396, 38)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3050, 3362, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3051, 3348, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3069, 3334, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2932, 3507, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2902, 3515, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2914, 3482, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2875, 3498, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2870, 3462, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(889, 2396, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(867, 2321, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(809, 2332, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(795, 2265, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(745, 2348, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(754, 2264, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(788, 2233, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(786, 2217, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(811, 2186, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(737, 2137, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(612, 2251, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(602, 2284, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(587, 2225, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(584, 2202, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(601, 2179, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(553, 2219, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(570, 2171, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(594, 2163, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(611, 2159, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(626, 2156, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(619, 2116, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(555, 2153, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(571, 2115, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(667, 2149, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(651, 2155, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1840, 2790, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1849, 2759, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1845, 2788, -8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1818, 2815, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1815, 2832, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1802, 2703, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1820, 2668, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1856, 2677, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1846, 2702, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1863, 2643, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1885, 2646, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1891, 2689, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1905, 2706, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1895, 2710, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1923, 2755, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1855, 2825, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1895, 2829, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1918, 2803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1900, 2806, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1878, 2807, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1926, 2789, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1939, 2759, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1935, 2748, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1937, 2711, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1937, 2687, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1947, 2676, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1959, 2673, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1960, 2685, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1992, 2695, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1998, 2716, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2024, 2742, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1964, 2866, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1978, 2831, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1993, 2866, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1992, 2882, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1988, 2897, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1987, 2905, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2018, 2891, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2028, 2857, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2023, 2834, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2035, 2799, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2009, 2808, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2041, 2771, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2057, 2795, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2085, 2792, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2780, 861, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2763, 875, -21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2745, 848, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2741, 980, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2778, 960, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2747, 1065, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2822, 991, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2853, 991, -21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2909, 979, -21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2937, 930, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2912, 931, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2893, 916, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2835, 943, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2834, 904, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2844, 867, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2866, 860, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2848, 833, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2865, 811, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2844, 811, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2891, 787, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2921, 794, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2881, 768, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2893, 739, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2858, 733, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2875, 714, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2876, 677, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2875, 662, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2891, 659, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2914, 675, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2906, 709, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2988, 643, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2955, 627, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2959, 705, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2993, 761, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2985, 779, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3013, 796, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3027, 763, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3027, 827, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2987, 834, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(3002, 812, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2957, 820, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2955, 848, 21)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2963, 866, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2954, 872, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2947, 936, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5211, 169, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5268, 154, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5260, 129, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5315, 105, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5304, 89, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5345, 81, 25)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5350, 53, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5312, 34, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5219, 120, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5202, 89, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5204, 74, 17)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5180, 90, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5160, 95, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5149, 59, 25)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5177, 20, 27)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(728, 1127, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(723, 1116, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(713, 1106, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(684, 1201, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(554, 1218, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(595, 1186, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(627, 1147, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(944, 1091, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(579, 1122, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(636, 1044, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(668, 1001, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(733, 884, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(652, 938, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(612, 978, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(564, 1013, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(556, 986, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(561, 972, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(533, 961, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(515, 997, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(532, 1010, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(459, 1116, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(452, 1131, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(395, 1227, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(331, 1251, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(451, 961, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(428, 1033, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(321, 1059, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(285, 977, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(482, 843, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(387, 915, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(338, 879, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(354, 836, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(319, 790, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(270, 771, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(261, 765, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(524, 768, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(552, 821, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(610, 875, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(626, 821, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(635, 818, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(603, 824, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(651, 812, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5275, 3946, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5264, 3979, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5285, 3980, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5294, 3976, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5310, 3981, 57)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5297, 4007, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5227, 4005, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5215, 4012, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5233, 4020, 37)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5288, 4057, 57)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5197, 4064, 57)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5759, 3152, 14)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5747, 3190, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5731, 3185, 8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5708, 3205, 11)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5694, 3208, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5736, 3219, 11)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5727, 3245, 16)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5735, 3265, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5752, 3272, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5801, 3267, 33)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5830, 3264, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5811, 3301, 12)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5696, 3284, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5667, 3284, 11)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5659, 3148, 13)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1468, 1524, 20)); possibleFacets.Add(Map.Trammel);

            // Ilshenar locations
			possibleLocations.Add(new Point3D(1220, 1098, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1194, 1094, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1178, 1107, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1181, 1121, -5)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1170, 1163, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1197, 1158, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1202, 1176, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1221, 1177, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1219, 1160, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1223, 1125, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1309, 1317, 6)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1306, 1324, -14)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(771, 1153, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(754, 1139, -29)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(763, 1134, -8)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(754, 1125, 14)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(779, 1093, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(779, 1061, -9)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(835, 1059, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(845, 1106, -9)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(863, 1067, -9)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(807, 1015, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(771, 1021, 14)); possibleFacets.Add(Map.Ilshenar);


            // Malas locations
			possibleLocations.Add(new Point3D(1026, 497, -30)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(951, 496, -70)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(952, 543, -30)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1025, 542, -50)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1025, 496, -50)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(990, 527, -50)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(998, 512, -30)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1978, 1307, -75)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1963, 1340, -50)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1963, 1377, -90)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(1975, 1367, -80)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2014, 1356, -90)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2024, 1385, -80)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2058, 1396, -90)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2070, 1375, -75)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2050, 1343, -85)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2033, 1314, -85)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2062, 1286, -80)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2077, 1326, -80)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2113, 1391, -80)); possibleFacets.Add(Map.Malas);
			possibleLocations.Add(new Point3D(2107, 1307, -50)); possibleFacets.Add(Map.Malas);
			

            // Tokuno locations
			possibleLocations.Add(new Point3D(701, 1276, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(689, 1276, 26)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(667, 1256, 48)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(680, 1297, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(702, 1294, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(724, 1308, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(750, 1296, 45)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(783, 1297, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(789, 1280, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(802, 1249, 30)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(781, 1214, 45)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(758, 1213, 30)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(718, 1220, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(723, 1224, 45)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(684, 1208, 25)); possibleFacets.Add(Map.Tokuno);
			possibleLocations.Add(new Point3D(694, 1205, 69)); possibleFacets.Add(Map.Tokuno);
			

            // Ter Mur locations
			possibleLocations.Add(new Point3D(898, 3479, -43)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(878, 3488, -43)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(893, 3410, -41)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(903, 3395, -41)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(848, 3410, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(846, 3438, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(817, 3454, -10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(806, 3492, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(784, 3491, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(770, 3492, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(771, 3464, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(788, 3446, -10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(781, 3428, -10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(817, 3428, 0)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(806, 3396, 0)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(813, 3360, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(805, 3311, 46)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(675, 3313, 67)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(652, 3316, 37)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(668, 3368, 50)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(659, 3418, 42)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(648, 3481, 10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(669, 3464, 10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(665, 3492, 10)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(703, 3434, -20)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(739, 3392, -8)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(748, 3346, 61)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(745, 3365, -25)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(726, 3346, -65)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(912, 4000, -40)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(984, 3974, -42)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(1095, 3951, -40)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(1012, 3937, -42)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(1018, 3908, -42)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(1011, 3880, -42)); possibleFacets.Add(Map.TerMur);
			possibleLocations.Add(new Point3D(1043, 3858, -41)); possibleFacets.Add(Map.TerMur);
			
			

            // Randomly select locations from the list
            for (int i = 0; i < count; i++)
            {
                if (possibleLocations.Count > 0)
                {
                    int index = Utility.Random(possibleLocations.Count);
                    locations.Add(possibleLocations[index]);
                    facets.Add(possibleFacets[index]);
                    possibleLocations.RemoveAt(index);
                    possibleFacets.RemoveAt(index);
                }
                else
                {
                    // If we run out of predefined locations, generate a random one
                    // Note: This fallback uses Trammel, you might want to randomize the map as well
                    locations.Add(new Point3D(Utility.Random(5120), Utility.Random(4096), 0));
                    facets.Add(Map.Trammel);
                }
            }
        }

        private static Mobile CreateCriminal()
        {
            BaseCreature criminal = new BaseCreature(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4);
            criminal.Body = 400 + Utility.Random(2);
            criminal.Name = NameList.RandomName("male");
            return criminal;
        }

        private static BaseCreature Spawn(int level, Point3D p, Map map, Mobile target, bool guardian)
        {
            // Implement spawning logic here
            // For example:
            BaseCreature creature = new BaseCreature(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4);
            creature.Name = "Guardian";
            creature.Body = 400 + Utility.Random(2);
            creature.MoveToWorld(p, map);
            return creature;
        }
    }

    public class ForensicDetectiveGump : Gump
    {
        private readonly Mobile m_From;

        public ForensicDetectiveGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            AddPage(0);
            AddBackground(0, 0, 200, 200, 5054);
            AddLabel(20, 20, 0, "Forensic Detective");
            AddButton(20, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(55, 50, 0, "Start Investigation");
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                ForensicEvaluationSystem.StartInvestigation(m_From);
            }
        }
    }
}