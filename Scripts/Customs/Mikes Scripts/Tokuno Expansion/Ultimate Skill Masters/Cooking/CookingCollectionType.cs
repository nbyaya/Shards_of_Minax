using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CookingCollectionType
    {
        public static List<CookingCollectionType> Items = new List<CookingCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }
        public bool RequiresOven { get; private set; }
        public bool RequiresHeat { get; private set; }

        public CookingCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount, Dictionary<Type, int> additionalResources = null, bool requiresOven = false, bool requiresHeat = false)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            MaterialType = materialType;
            MaterialAmount = materialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
            RequiresOven = requiresOven;
            RequiresHeat = requiresHeat;
        }

        public static void PopulateCookingCollection()
        {
            Items = new List<CookingCollectionType>
            {
                new CookingCollectionType(typeof(SackFlour), "Sack of Flour", 0.0, 100.0, typeof(WheatSheaf), 2, null, true),
                new CookingCollectionType(typeof(Dough), "Dough", 0.0, 100.0, typeof(SackFlourOpen), 1, new Dictionary<Type, int> { { typeof(BaseBeverage), 1 } }),
                new CookingCollectionType(typeof(SweetDough), "Sweet Dough", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(JarHoney), 1 } }),
                new CookingCollectionType(typeof(CakeMix), "Cake Mix", 0.0, 100.0, typeof(SackFlourOpen), 1, new Dictionary<Type, int> { { typeof(SweetDough), 1 } }),
                new CookingCollectionType(typeof(CookieMix), "Cookie Mix", 0.0, 100.0, typeof(JarHoney), 1, new Dictionary<Type, int> { { typeof(SweetDough), 1 } }),

                new CookingCollectionType(typeof(UnbakedQuiche), "Unbaked Quiche", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Eggs), 1 } }),
                new CookingCollectionType(typeof(UnbakedMeatPie), "Unbaked Meat Pie", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(RawRibs), 1 } }),
                new CookingCollectionType(typeof(UncookedSausagePizza), "Uncooked Sausage Pizza", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Sausage), 1 } }),
                new CookingCollectionType(typeof(UncookedCheesePizza), "Uncooked Cheese Pizza", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(CheeseWheel), 1 } }),
                new CookingCollectionType(typeof(UnbakedFruitPie), "Unbaked Fruit Pie", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Pear), 1 } }),

                new CookingCollectionType(typeof(BreadLoaf), "Bread Loaf", 0.0, 100.0, typeof(Dough), 1, null, true),
                new CookingCollectionType(typeof(Cookies), "Cookies", 0.0, 100.0, typeof(CookieMix), 1, null, true),
                new CookingCollectionType(typeof(Cake), "Cake", 0.0, 100.0, typeof(CakeMix), 1, null, true),
                new CookingCollectionType(typeof(Muffins), "Muffins", 0.0, 100.0, typeof(SweetDough), 1, null, true),
                new CookingCollectionType(typeof(Quiche), "Quiche", 0.0, 100.0, typeof(UnbakedQuiche), 1, null, true),

                new CookingCollectionType(typeof(CookedBird), "Cooked Bird", 0.0, 100.0, typeof(RawBird), 1, null, false, true),
                new CookingCollectionType(typeof(ChickenLeg), "Chicken Leg", 0.0, 100.0, typeof(RawChickenLeg), 1, null, false, true),
                new CookingCollectionType(typeof(FishSteak), "Fish Steak", 0.0, 100.0, typeof(RawFishSteak), 1, null, false, true),
                new CookingCollectionType(typeof(FriedEggs), "Fried Eggs", 0.0, 100.0, typeof(Eggs), 1, null, false, true),
                new CookingCollectionType(typeof(LambLeg), "Lamb Leg", 0.0, 100.0, typeof(RawLambLeg), 1, null, false, true)
            };
        }

        public static CookingCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
