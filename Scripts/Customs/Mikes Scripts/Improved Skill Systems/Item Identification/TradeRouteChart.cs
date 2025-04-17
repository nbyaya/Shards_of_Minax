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
    public class TradeRouteChart : Item
    {
        private int m_Level;
        private Point3D m_NextLocation;
        private Map m_NextFacet;
        private string m_RequiredCommodity;
        private int m_RequiredAmount;
        private bool m_Identified;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get { return m_Level; } set { m_Level = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D NextLocation { get { return m_NextLocation; } set { m_NextLocation = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map NextFacet { get { return m_NextFacet; } set { m_NextFacet = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string RequiredCommodity { get { return m_RequiredCommodity; } set { m_RequiredCommodity = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int RequiredAmount { get { return m_RequiredAmount; } set { m_RequiredAmount = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified { get { return m_Identified; } set { m_Identified = value; } }

        [Constructable]
        public TradeRouteChart(int level) : base(0x14ED)
        {
            m_Level = level;
            Name = "Trade Route Chart";
            LootType = LootType.Blessed;
            Visible = true;
            m_Identified = false;
        }

        public TradeRouteChart(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_Identified)
            {
                from.SendMessage(String.Format("This trade route requires {0} {1}. The next point is located at {2} on {3}.", m_RequiredAmount, m_RequiredCommodity, m_NextLocation, m_NextFacet));
            }
            else
            {
                from.SendMessage(String.Format("This trade route requires {0} {1}. The next point is located at {2} on {3}.", m_RequiredAmount, m_RequiredCommodity, m_NextLocation, m_NextFacet));
            }
        }

        public bool CheckIdentify(Mobile m)
        {
            if (m_Identified)
                return false;

            double identifySkill = m.Skills[SkillName.ItemID].Value;
            double difficulty = 80 + (m_Level * 5); // Adjust this formula as needed

            return (identifySkill >= difficulty);
        }

        public void OnIdentified(Mobile m)
        {
            m_Identified = true;
            m.SendLocalizedMessage(500819); // You have successfully identified the item.
            m.SendMessage("You have identified a Trade Route Clue!");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Level);
            writer.Write(m_NextLocation);
            writer.Write(m_NextFacet);
            writer.Write(m_RequiredCommodity);
            writer.Write(m_RequiredAmount);
            writer.Write(m_Identified);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Level = reader.ReadInt();
            m_NextLocation = reader.ReadPoint3D();
            m_NextFacet = reader.ReadMap();
            m_RequiredCommodity = reader.ReadString();
            m_RequiredAmount = reader.ReadInt();
            m_Identified = reader.ReadBool();
        }
    }

    public class TradeRouteMerchant : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        private string m_RequiredCommodity;
        private int m_RequiredAmount;
        private int m_CurrentAmount;

        [Constructable]
        public TradeRouteMerchant() : base("the Trade Route Merchant")
        {
            m_CurrentAmount = 0;
        }

        public TradeRouteMerchant(Serial serial) : base(serial)
        {
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTradeRouteMerchant());
        }

        public void SetRequirements(string commodity, int amount)
        {
            m_RequiredCommodity = commodity;
            m_RequiredAmount = amount;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Type commodityType = ScriptCompiler.FindTypeByName(m_RequiredCommodity);
            if (commodityType == null || dropped.GetType() != commodityType)
            {
                Say(String.Format("I'm sorry, but I need {0} to complete this trade.", m_RequiredCommodity));
                return false;
            }

            int amount = Math.Min(dropped.Amount, m_RequiredAmount - m_CurrentAmount);
            m_CurrentAmount += amount;

            if (amount < dropped.Amount)
            {
                dropped.Amount -= amount;
                from.AddToBackpack(dropped);
            }
            else
            {
                dropped.Delete();
            }

            if (m_CurrentAmount >= m_RequiredAmount)
            {
                Say("Excellent! You've completed the trade route. Here's your reward!");
                Delete();
                SpawnRewardChest(from);
                return true;
            }
            else
            {
                Say(String.Format("Thank you! I still need {0} more {1}.", m_RequiredAmount - m_CurrentAmount, m_RequiredCommodity));
                return true;
            }
        }

        private void SpawnRewardChest(Mobile from)
		{
			TradeRouteRewardChest chest = new TradeRouteRewardChest();
			if (from.AddToBackpack(chest))
			{
				from.SendMessage("A reward chest has been added to your backpack!");
			}
			else
			{
				chest.Delete();
				from.SendMessage("You don't have enough room in your backpack for the reward chest. Please make some room and try again.");
			}
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_RequiredCommodity);
            writer.Write(m_RequiredAmount);
            writer.Write(m_CurrentAmount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RequiredCommodity = reader.ReadString();
            m_RequiredAmount = reader.ReadInt();
            m_CurrentAmount = reader.ReadInt();
        }
    }

	public class TradeRouteSystem
	{
		public static void StartTradeRoute(Mobile from)
		{
			int maxClues = CalculateMaxClues(from);
			List<Point3D> locations = new List<Point3D>();
			List<Map> facets = new List<Map>();
			GetRandomLocations(maxClues + 1, locations, facets); // +1 for final location

			string requiredCommodity = GetRandomCommodity();
			int totalRequiredAmount = CalculateRequiredAmount(maxClues);
			int amountPerNode = totalRequiredAmount / maxClues;

			for (int i = 0; i < maxClues; i++)
			{
				Container crate = new Container(0xE3D);
				crate.Name = "Trade Route Supplies";

				TradeRouteChart clue = new TradeRouteChart(i + 1);
				clue.NextLocation = i < maxClues - 1 ? locations[i + 1] : locations[maxClues];
				clue.NextFacet = i < maxClues - 1 ? facets[i + 1] : facets[maxClues];
				clue.RequiredCommodity = requiredCommodity;
				clue.RequiredAmount = totalRequiredAmount;
				clue.Identified = i == 0; // First clue is always identified

				crate.DropItem(clue);

				// Add commodities to the crate
				Type commodityType = ScriptCompiler.FindTypeByName(requiredCommodity);
				if (commodityType != null)
				{
					Item commodity = (Item)Activator.CreateInstance(commodityType);
					commodity.Amount = amountPerNode;
					crate.DropItem(commodity);
				}

				if (i == 0)
				{
					// Give the first crate to the player
					if (from.AddToBackpack(crate))
					{
						from.SendMessage("You've received your first trade route supplies. Examine the chart to start your journey.");
					}
					else
					{
						crate.Delete();
						from.SendMessage("You find trade route supplies, but have no room in your backpack to keep them.");
					}
				}
				else
				{
					// Place other crates in the world
					crate.MoveToWorld(locations[i], facets[i]);
				}
			}

			// Create and place the final merchant
			TradeRouteMerchant merchant = new TradeRouteMerchant();
			merchant.SetRequirements(requiredCommodity, totalRequiredAmount);
			merchant.MoveToWorld(locations[maxClues], facets[maxClues]);
		}

		private static int CalculateMaxClues(Mobile from)
		{
			int itemIDSkill = from.Skills[SkillName.ItemID].Fixed / 10;
			if (itemIDSkill == 0)
			{
				itemIDSkill = 1; // Set a default value to avoid division by zero
			}
			return Math.Max(3, Math.Min(10, 200 / itemIDSkill));
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
			possibleLocations.Add(new Point3D(2636, 469, 31)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2663, 469, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2660, 508, 13)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2681, 523, 16)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2726, 510, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2772, 500, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2785, 520, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2805, 535, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2820, 556, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2843, 570, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2867, 582, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2895, 590, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2919, 602, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2938, 619, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2720, 987, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2678, 995, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2633, 1010, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2612, 997, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2617, 965, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2645, 933, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2665, 983, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2682, 850, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2672, 827, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2662, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2671, 760, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2660, 738, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2640, 724, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2617, 714, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2597, 700, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2599, 665, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2598, 647, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2585, 627, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2556, 623, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2525, 621, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2512, 604, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2559, 1002, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2527, 1001, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2513, 982, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2479, 982, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2442, 1005, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2419, 994, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2381, 999, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2367, 979, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2320, 992, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2303, 976, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2284, 961, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2286, 926, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2281, 898, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2268, 877, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2260, 851, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2239, 839, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2215, 829, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2191, 820, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2169, 809, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2129, 815, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2107, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2073, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2054, 843, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2040, 889, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2015, 923, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1995, 1068, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1972, 1105, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1927, 1120, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1930, 1153, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1917, 1200, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1888, 1230, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1853, 1255, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1812, 1274, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1779, 1301, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1766, 1347, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1746, 1387, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1713, 1414, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1676, 1437, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1661, 1481, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1644, 1524, 35)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1605, 1530, 40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1587, 1529, 39)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2063, 804, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2039, 794, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2024, 771, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(2007, 754, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1975, 752, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1945, 744, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1925, 735, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1881, 751, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1864, 730, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1827, 733, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1783, 745, 6)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1757, 774, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1717, 794, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1677, 800, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1640, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1594, 816, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1563, 845, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1536, 876, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1516, 862, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1503, 842, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1502, 809, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1486, 791, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1457, 786, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1429, 782, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1390, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1390, 803, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1359, 829, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1329, 853, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1288, 878, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1262, 896, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1239, 932, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1200, 953, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1172, 985, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1129, 1002, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1085, 1011, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1050, 1013, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1044, 1055, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1050, 1085, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1011, 1106, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(982, 1137, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(935, 1150, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(900, 1175, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(867, 1196, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(843, 1187, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(834, 1163, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(806, 1157, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(768, 1161, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(743, 1152, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(723, 1138, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(699, 1128, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(658, 1135, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(647, 1112, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(608, 1133, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(562, 1146, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(542, 1132, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(535, 1105, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(551, 1065, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(507, 1079, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(489, 1117, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(492, 1150, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(491, 1187, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(496, 1219, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(850, 1240, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(817, 1264, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(784, 1293, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(738, 1308, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(712, 1341, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(691, 1380, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(698, 1409, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(702, 1441, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(715, 1464, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(731, 1484, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(741, 1510, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(742, 1544, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(776, 1546, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(797, 1561, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(813, 1581, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(838, 1593, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(853, 1614, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(870, 1634, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(879, 1661, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(889, 1689, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(907, 1704, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(922, 1726, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(945, 1739, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(968, 1753, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(979, 1778, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(991, 1802, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1005, 1824, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(994, 1871, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(998, 1903, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1008, 1930, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1012, 1938, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1044, 1942, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1082, 1940, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1112, 1912, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1138, 1879, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1182, 1871, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1224, 1854, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1248, 1844, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1250, 1808, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1250, 1775, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1260, 1746, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1297, 1746, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1331, 1750, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1362, 1753, 13)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1365, 1750, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1008, 1979, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1003, 2020, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1007, 2052, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1002, 2093, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1005, 2126, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(981, 2162, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(946, 2163, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(913, 2189, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(904, 2234, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(961, 2243, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(827, 2243, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(787, 2242, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(759, 2236, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(731, 2263, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1363, 1923, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1353, 1969, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1372, 1986, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1386, 2008, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1393, 2037, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1405, 2061, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1430, 2070, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1433, 2105, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1452, 2121, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1471, 2138, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1472, 2149, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1487, 2171, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1495, 2199, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1490, 2240, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1522, 2245, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1545, 2257, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1559, 2279, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1588, 2289, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1607, 2307, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1632, 2317, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1638, 2347, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1611, 2380, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1608, 2419, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1630, 2434, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1642, 2460, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1667, 2471, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1666, 2512, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1661, 2553, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1667, 2582, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1671, 2613, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1685, 2635, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1707, 2649, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1703, 2690, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1694, 2736, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1698, 2768, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1720, 2782, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1749, 2790, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1788, 2786, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(1829, 2779, 0)); possibleFacets.Add(Map.Felucca);
			
			
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
			possibleLocations.Add(new Point3D(2636, 469, 31)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2663, 469, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2660, 508, 13)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2681, 523, 16)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2726, 510, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2772, 500, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2785, 520, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2805, 535, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2820, 556, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2843, 570, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2867, 582, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2895, 590, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2919, 602, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2938, 619, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2720, 987, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2678, 995, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2633, 1010, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2612, 997, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2617, 965, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2645, 933, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2665, 983, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2682, 850, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2672, 827, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2662, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2671, 760, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2660, 738, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2640, 724, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2617, 714, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2597, 700, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2599, 665, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2598, 647, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2585, 627, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2556, 623, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2525, 621, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2512, 604, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2559, 1002, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2527, 1001, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2513, 982, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2479, 982, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2442, 1005, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2419, 994, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2381, 999, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2367, 979, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2320, 992, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2303, 976, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2284, 961, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2286, 926, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2281, 898, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2268, 877, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2260, 851, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2239, 839, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2215, 829, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2191, 820, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2169, 809, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2129, 815, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2107, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2073, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2054, 843, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2040, 889, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2015, 923, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1995, 1068, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1972, 1105, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1927, 1120, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1930, 1153, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1917, 1200, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1888, 1230, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1853, 1255, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1812, 1274, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1779, 1301, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1766, 1347, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1746, 1387, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1713, 1414, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1676, 1437, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1661, 1481, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1644, 1524, 35)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1605, 1530, 40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1587, 1529, 39)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2063, 804, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2039, 794, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2024, 771, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(2007, 754, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1975, 752, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1945, 744, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1925, 735, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1881, 751, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1864, 730, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1827, 733, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1783, 745, 6)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1757, 774, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1717, 794, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1677, 800, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1640, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1594, 816, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1563, 845, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1536, 876, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1516, 862, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1503, 842, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1502, 809, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1486, 791, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1457, 786, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1429, 782, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1390, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1390, 803, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1359, 829, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1329, 853, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1288, 878, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1262, 896, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1239, 932, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1200, 953, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1172, 985, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1129, 1002, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1085, 1011, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1050, 1013, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1044, 1055, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1050, 1085, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1011, 1106, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(982, 1137, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(935, 1150, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(900, 1175, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(867, 1196, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(843, 1187, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(834, 1163, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(806, 1157, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(768, 1161, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(743, 1152, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(723, 1138, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(699, 1128, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(658, 1135, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(647, 1112, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(608, 1133, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(562, 1146, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(542, 1132, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(535, 1105, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(551, 1065, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(507, 1079, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(489, 1117, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(492, 1150, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(491, 1187, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(496, 1219, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(850, 1240, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(817, 1264, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(784, 1293, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(738, 1308, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(712, 1341, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(691, 1380, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(698, 1409, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(702, 1441, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(715, 1464, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(731, 1484, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(741, 1510, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(742, 1544, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(776, 1546, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(797, 1561, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(813, 1581, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(838, 1593, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(853, 1614, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(870, 1634, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(879, 1661, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(889, 1689, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(907, 1704, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(922, 1726, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(945, 1739, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(968, 1753, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(979, 1778, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(991, 1802, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1005, 1824, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(994, 1871, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(998, 1903, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1008, 1930, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1012, 1938, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1044, 1942, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1082, 1940, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1112, 1912, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1138, 1879, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1182, 1871, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1224, 1854, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1248, 1844, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1250, 1808, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1250, 1775, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1260, 1746, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1297, 1746, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1331, 1750, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1362, 1753, 13)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1365, 1750, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1008, 1979, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1003, 2020, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1007, 2052, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1002, 2093, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1005, 2126, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(981, 2162, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(946, 2163, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(913, 2189, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(904, 2234, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(961, 2243, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(827, 2243, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(787, 2242, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(759, 2236, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(731, 2263, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1363, 1923, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1353, 1969, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1372, 1986, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1386, 2008, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1393, 2037, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1405, 2061, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1430, 2070, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1433, 2105, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1452, 2121, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1471, 2138, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1472, 2149, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1487, 2171, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1495, 2199, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1490, 2240, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1522, 2245, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1545, 2257, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1559, 2279, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1588, 2289, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1607, 2307, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1632, 2317, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1638, 2347, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1611, 2380, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1608, 2419, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1630, 2434, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1642, 2460, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1667, 2471, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1666, 2512, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1661, 2553, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1667, 2582, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1671, 2613, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1685, 2635, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1707, 2649, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1703, 2690, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1694, 2736, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1698, 2768, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1720, 2782, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1749, 2790, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1788, 2786, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(1829, 2779, 0)); possibleFacets.Add(Map.Trammel);

			
			possibleLocations.Add(new Point3D(1493, 982, 6)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1512, 997, -7)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1535, 1009, -7)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1557, 1023, -7)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1573, 1046, -8)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1574, 1078, -8)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1551, 1115, -8)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1510, 1122, -8)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1464, 1132, -22)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1422, 1150, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1383, 1171, -19)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1346, 1194, -21)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1306, 1200, -22)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1275, 1196, -29)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1244, 1193, -29)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1234, 1186, -27)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1172, 1075, -25)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1160, 1053, -21)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1137, 1041, -28)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1109, 1034, -35)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1091, 1018, -36)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1091, 1003, -37)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(912, 932, -33)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(918, 963, -26)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(943, 971, -43)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(954, 971, -46)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(954, 939, -35)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(975, 900, -38)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(106, 882, -26)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1060, 873, -29)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1082, 887, -27)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1099, 905, -29)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(1100, 930, -34)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(961, 999, -46)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(952, 1048, -2)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(928, 1084, -6)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(926, 1125, 11)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(923, 1155, 13)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(915, 1190, 11)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(871, 1191, -42)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(819, 1203, -70)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(820, 1173, -50)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(819, 1153, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(782, 1203, -70)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(761, 1242, -64)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(723, 1246, -72)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(678, 1255, -95)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(661, 1240, -87)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(645, 1222, -91)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(636, 1198, -85)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(615, 1187, -74)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(605, 1176, -64)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(549, 1180, -100)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(517, 1180, -97)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(492, 1188, -63)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(559, 1135, -98)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(557, 1108, -98)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(557, 1078, -83)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(556, 1042, -85)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(563, 1001, -90)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(565, 966, -89)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(562, 932, -81)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(524, 936, -85)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(478, 949, -87)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(449, 979, -85)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(588, 912, -70)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(593, 974, -70)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(630, 924, -66)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(640, 950, -68)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(650, 975, -76)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(666, 996, -75)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(677, 1020, -80)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(686, 1047, -82)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(695, 1075, -77)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(572, 796, -69)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(572, 764, -64)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(564, 714, -61)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(557, 679, -63)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(578, 643, -65)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(588, 637, -58)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(632, 601, -70)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(665, 576, -63)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(693, 540, -68)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(721, 508, -65)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(726, 476, -80)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(439, 429, -62)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(757, 391, -55)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(777, 342, -34)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(804, 311, -37)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(855, 304, 9)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(901, 298, 26)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(927, 309, 36)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(952, 322, 47)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(978, 306, 53)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(613, 657, -62)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(645, 667, -30)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(552, 569, -63)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(556, 227, -42)); possibleFacets.Add(Map.Ilshenar);
			possibleLocations.Add(new Point3D(584, 236, -38)); possibleFacets.Add(Map.Ilshenar);

			
			

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
                    locations.Add(new Point3D(Utility.Random(5120), Utility.Random(4096), 0));
                    facets.Add(Map.Felucca);
                }
            }
        }

        private static string GetRandomCommodity()
        {
            string[] commodities = new string[]
            {
                "IronIngot", "Log", "RawRibs", "FishSteak", "Wool", "Cotton",
                "Leather", "Board", "Arrow", "Bottle", "Diamond"
            };
            return commodities[Utility.Random(commodities.Length)];
        }

        private static int CalculateRequiredAmount(int maxClues)
        {
            return 100 + (maxClues * 50); // Adjust this formula as needed
        }
    }

    public class TradeRouteGump : Gump
    {
        private readonly Mobile m_From;

        public TradeRouteGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            AddPage(0);
            AddBackground(0, 0, 200, 200, 5054);
            AddLabel(20, 20, 0, "Trade Route System");
            AddButton(20, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(55, 50, 0, "Start Trade Route");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                TradeRouteSystem.StartTradeRoute(m_From);
            }
        }
    }
}