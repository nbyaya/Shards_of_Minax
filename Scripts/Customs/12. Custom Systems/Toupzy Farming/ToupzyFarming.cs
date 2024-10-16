using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Items;

namespace Server.FarmingSystem
{
	public class ToupzyFarmingSystem
	{
		private static bool m_Initalized = false;
        private static Dictionary<BaseFarmingCrop, Mobile> m_ActiveCrops = new Dictionary<BaseFarmingCrop, Mobile>();

		public static int[] EffectActions = new int[] { 3 };
		public static int[] EffectCounts = new int[] { 0x125, 0x126 };
		public static int[] EffectSounds = new int[] { 1 };
		public static TimeSpan EffectSoundDelay = TimeSpan.FromSeconds(1.6);
		public static TimeSpan EffectDelay = TimeSpan.FromSeconds(0.9);

		public const SkillName FARMINGSKILL = SkillName.Forensics;


        public const int MAX_PLANT_COUNT = 5;

		private static InternalTimer m_InternalTimer = null;


        private static List<int> m_SeedOwners = new List<int>();

		public static void Initialize()
		{
			if (!m_Initalized)
			{
				m_InternalTimer = new InternalTimer(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
				m_InternalTimer.Start();

				m_Initalized = true;
			}
		}

		internal static double GetSkillForSeed(int seedtype)
		{
			return m_Seeds[seedtype].MinSkill;
		}

		public static void AddCrop(BaseFarmingCrop crop, Mobile owner)
		{
			m_ActiveCrops.Add(crop, owner);
		}
		public static void RemoveCrop(BaseFarmingCrop crop)
		{
			m_ActiveCrops.Remove(crop);
		}

		private static void CheckCrops()
		{
			foreach (KeyValuePair<BaseFarmingCrop,Mobile> kvp in m_ActiveCrops)
			{
				if (DateTime.Now >= kvp.Key.NextUpdate)
				{
					kvp.Key.UpdateCrop();
				}
			}
		}
		public static bool CanPlant(Mobile planter)
		{

            if (m_SeedOwners.Contains(planter.Serial))
                return false;

            int count = 0;

			foreach (KeyValuePair<BaseFarmingCrop,Mobile> kvp in m_ActiveCrops)
			{
				if (kvp.Value == planter)
				{
                    count++;
				}
			}

            if(count < MAX_PLANT_COUNT)
                return true;

            return false;
		}

		internal static int GetCropHue(int croptype)
		{
			return m_Crops[croptype].Hue;
		}

		internal static int GetCropItemID(int croptype)
		{
			return m_Crops[croptype].ItemID;
		}

		public static bool HasWater(Mobile from, int type)
		{
			foreach (Item item in from.Backpack.Items)
			{
				if (item is BaseBeverage)
				{
					BaseBeverage bev = item as BaseBeverage;
					if (bev.Content == BeverageType.Water)
					{
							if (bev.Quantity >= ToupzyFarmingSystem.GetWaterAmountForSeed(type))
							{
								return true;
							}
					}
				}
			}

			return false;
		}

		internal static Item GetFruit(int croptype)
		{
			Item item = Activator.CreateInstance(m_Crops[croptype].Fruit) as Item;

			if (item != null)
			{
				item.Amount = Utility.Random(m_Crops[croptype].MinToGive, m_Crops[croptype].MaxToGive);
				return item;
			}

			return null;
		}

		internal static int CheckGiveSeed(BaseFarmingCrop crop, int multiplier)
		{
			if (crop.SeedCount < m_Crops[crop.CropType].MaxSeeds && (0.1 * multiplier) >= Utility.RandomDouble())
			{
				return Utility.Random(1, m_Crops[crop.CropType].MaxSeeds);
			}

			return 0;
		}

        public static void AddPlayer(int owner)
        {
            m_SeedOwners.Add(owner);
        }

        internal static void RemovePlayer(int owner)
        {
            m_SeedOwners.Remove(owner);
        }

        public static void ConsumeWater(Mobile from, int type)
		{
			int need = 0;
				need = ToupzyFarmingSystem.GetWaterAmountForSeed(type);

			foreach (Item item in from.Backpack.Items)
			{
				if (need > 0)
				{
					if (item is BaseBeverage)
					{
						BaseBeverage bev = item as BaseBeverage;
						if (bev.Content == BeverageType.Water)
						{
							if (bev.Quantity <= need)
							{
								need -= bev.Quantity;
								bev.Quantity = 0;
							}
							else
							{
								bev.Quantity -= need;
								need = 0;
							}
						}
					}
				}
			}
		}

		public static int SeedCount
		{ get { return m_Seeds.Length; } }

		private static int GetWaterAmountForSeed(int seedtype)
		{
			return m_Seeds[seedtype].WaterAmount;
		}

		public static bool ValidTile(int tileID)
		{
			bool contains = false;

			for (int i = 0; !contains && i < m_FarmingTiles.Length; i++)
				contains = tileID == m_FarmingTiles[i];

			return contains;
		}

		public static bool TileHasCrop(Map map, Point3D loc)
		{
			IEnumerable<Item> items = map.GetItemsInRange(loc, 0);

			if (items.Count<Item>() > 0)
			{
				return true;
			}

			return false;
		}

		public static bool TileHasSeed(Map map, Point3D loc)
		{
			IEnumerable<Item> items = map.GetItemsInRange(loc, 0);

			foreach (Item item in items)
			{
				if (item is BaseFarmingSeed)
					return true;
			}

			return false;
		}

		private static readonly int[] m_FarmingTiles = new int[]
		   {
			9,10,11,12,13,14,15
		   };

		private static readonly FarmingSeed[] m_Seeds = new FarmingSeed[]
		   {
			   //ITEMID, "NAME" , HUE, WATER AMOUNT ,REQUIRED SKILL
				//new FarmingSeed(3535,"",0,1,30),
				new FarmingSeed(3535,"Carrot Seed",42,2,30),
				new FarmingSeed(3535,"Squash Seed",53,2,40),
				new FarmingSeed(3535,"Cabbage Seed",779,2,50),
				new FarmingSeed(3535,"Lettuce Seed",467,2,60),
				new FarmingSeed(3535,"Watermelon Seed",77,2,20.1),
				new FarmingSeed(3535,"Pumpkin Seed",48,2,20.1),
                new FarmingSeed(3535,"Tobacco Seed",448,2,80)
           };
		private static readonly FarmingCrop[] m_Crops = new FarmingCrop[]
		   {
			   //ITEMID,"NAME", HUE,Maxseeds,typeof(TYPE),MINTOGIVE,MAXTOGIVE
			   //new FarmingCrop(0,"",0,1,typeof(),2,6)
			   new FarmingCrop(3190,"Carrot",42,1,typeof(Carrot),2,6),
			   new FarmingCrop(3167,"Squash",53,1,typeof(Squash),2,6),
			   new FarmingCrop(3254,"Head of Cabbage",779,1,typeof(Cabbage),2,6),
			   new FarmingCrop(3254,"Head of Lettuce",467,1,typeof(Lettuce),2,6),
			   new FarmingCrop(3167,"Watermelon",77,1,typeof(Watermelon),2,6),
			   new FarmingCrop(3167,"Pumpkin",48,1,typeof(Pumpkin),2,6),
               new FarmingCrop(0x0CA7,"Tobacco",448,1,typeof(Tobacco),2,6)
           };

		internal class InternalTimer : Timer
		{
			public InternalTimer(TimeSpan delay, TimeSpan interval) : base(delay, interval)
			{

			}

			protected override void OnTick()
			{
				CheckCrops();
			}
		}

		internal static BaseFarmingSeed GetSeed(int seedtype)
		{
			BaseFarmingSeed seed = new BaseFarmingSeed(seedtype, m_Seeds[seedtype].ItemID);
			seed.Name = m_Seeds[seedtype].Name;
			seed.Hue = m_Seeds[seedtype].Hue;
			seed.ItemID = m_Seeds[seedtype].ItemID;

			return seed;
		}

		internal static BaseFarmingCrop GetCrop(int croptype)
		{
			BaseFarmingCrop crop = new BaseFarmingCrop(croptype, m_Crops[croptype].ItemID);
			crop.Name = m_Crops[croptype].Name;
			crop.ItemID = 2323;
			
			return crop;
		}

		internal struct FarmingSeed
		{
			public int ItemID;
			public string Name;
			public int Hue;
			public int WaterAmount;
			public double MinSkill;

			public FarmingSeed(int itemid, string name, int hue, int wateramount, double minskill)
			{
				ItemID = itemid;
				Name = name;
				Hue = hue;
				WaterAmount = wateramount;
				MinSkill = minskill;
			}
		}
		internal struct FarmingCrop
		{
			public int ItemID;
			public string Name;
			public int Hue;
			public int MaxSeeds;
			public Type Fruit;
			public int MinToGive;
			public int MaxToGive;

			public FarmingCrop(int itemid, string name, int hue, int maxseeds, Type fruit, int mintogive, int maxtogive)
			{
				ItemID = itemid;
				Name = name;
				Hue = hue;
				MaxSeeds = maxseeds;
				Fruit = fruit;
				MinToGive = mintogive;
				MaxToGive = maxtogive;
			}
		}

	}
}
