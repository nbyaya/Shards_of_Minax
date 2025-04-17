using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TasteIDCollectionType
    {
        public static List<TasteIDCollectionType> Items = new List<TasteIDCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }
        public bool RequiresOven { get; private set; }
        public bool RequiresHeat { get; private set; }

        public TasteIDCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount, Dictionary<Type, int> additionalResources = null, bool requiresOven = false, bool requiresHeat = false)
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

        public static void PopulateTasteIDCollection()
        {
            Items = new List<TasteIDCollectionType>
            {
                new TasteIDCollectionType(typeof(SackFlour), "Sack of Flour", 0.0, 100.0, typeof(WheatSheaf), 2, null, true),
                new TasteIDCollectionType(typeof(Dough), "Dough", 0.0, 100.0, typeof(SackFlourOpen), 1, new Dictionary<Type, int> { { typeof(BaseBeverage), 1 } }),
                new TasteIDCollectionType(typeof(SweetDough), "Sweet Dough", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(JarHoney), 1 } }),
                new TasteIDCollectionType(typeof(CakeMix), "Cake Mix", 0.0, 100.0, typeof(SackFlourOpen), 1, new Dictionary<Type, int> { { typeof(SweetDough), 1 } }),
                new TasteIDCollectionType(typeof(CookieMix), "Cookie Mix", 0.0, 100.0, typeof(JarHoney), 1, new Dictionary<Type, int> { { typeof(SweetDough), 1 } }),

                new TasteIDCollectionType(typeof(UnbakedQuiche), "Unbaked Quiche", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Eggs), 1 } }),
                new TasteIDCollectionType(typeof(UnbakedMeatPie), "Unbaked Meat Pie", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(RawRibs), 1 } }),
                new TasteIDCollectionType(typeof(UncookedSausagePizza), "Uncooked Sausage Pizza", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Sausage), 1 } }),
                new TasteIDCollectionType(typeof(UncookedCheesePizza), "Uncooked Cheese Pizza", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(CheeseWheel), 1 } }),
                new TasteIDCollectionType(typeof(UnbakedFruitPie), "Unbaked Fruit Pie", 0.0, 100.0, typeof(Dough), 1, new Dictionary<Type, int> { { typeof(Pear), 1 } }),

                new TasteIDCollectionType(typeof(BreadLoaf), "Bread Loaf", 0.0, 100.0, typeof(Dough), 1, null, true),
                new TasteIDCollectionType(typeof(Cookies), "Cookies", 0.0, 100.0, typeof(CookieMix), 1, null, true),
                new TasteIDCollectionType(typeof(Cake), "Cake", 0.0, 100.0, typeof(CakeMix), 1, null, true),
                new TasteIDCollectionType(typeof(Muffins), "Muffins", 0.0, 100.0, typeof(SweetDough), 1, null, true),
                new TasteIDCollectionType(typeof(Quiche), "Quiche", 0.0, 100.0, typeof(UnbakedQuiche), 1, null, true),

                new TasteIDCollectionType(typeof(CookedBird), "Cooked Bird", 0.0, 100.0, typeof(RawBird), 1, null, false, true),
                new TasteIDCollectionType(typeof(ChickenLeg), "Chicken Leg", 0.0, 100.0, typeof(RawChickenLeg), 1, null, false, true),
                new TasteIDCollectionType(typeof(FishSteak), "Fish Steak", 0.0, 100.0, typeof(RawFishSteak), 1, null, false, true),
                new TasteIDCollectionType(typeof(FriedEggs), "Fried Eggs", 0.0, 100.0, typeof(Eggs), 1, null, false, true),
                new TasteIDCollectionType(typeof(LambLeg), "Lamb Leg", 0.0, 100.0, typeof(RawLambLeg), 1, null, false, true)
            };
        }

        public static TasteIDCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
