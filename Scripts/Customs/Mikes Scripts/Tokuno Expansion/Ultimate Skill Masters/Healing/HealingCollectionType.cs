using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HealingCollectionType
    {
        public static List<HealingCollectionType> Items = new List<HealingCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type PrimaryMaterialType { get; private set; }
        public int PrimaryMaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }

        public HealingCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type primaryMaterialType, int primaryMaterialAmount, Dictionary<Type, int> additionalResources = null)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            PrimaryMaterialType = primaryMaterialType;
            PrimaryMaterialAmount = primaryMaterialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
        }

        public static void PopulateHealingCollection()
        {
            Items = new List<HealingCollectionType>
            {
                new HealingCollectionType(typeof(RefreshPotion), "Refresh Potion", -25.0, 25.0, typeof(BlackPearl), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(TotalRefreshPotion), "Total Refresh Potion", 25.0, 75.0, typeof(BlackPearl), 5,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(LesserHealPotion), "Lesser Heal Potion", -25.0, 25.0, typeof(Ginseng), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(HealPotion), "Heal Potion", 15.0, 65.0, typeof(Ginseng), 3,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(GreaterHealPotion), "Greater Heal Potion", 55.0, 105.0, typeof(Ginseng), 7,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(LesserCurePotion), "Lesser Cure Potion", -10.0, 40.0, typeof(Garlic), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(CurePotion), "Cure Potion", 25.0, 75.0, typeof(Garlic), 3,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new HealingCollectionType(typeof(GreaterCurePotion), "Greater Cure Potion", 65.0, 115.0, typeof(Garlic), 6,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),
            };
        }

        public static HealingCollectionType GetRandomItem()
        {
            if (Items == null || Items.Count == 0)
                PopulateHealingCollection();

            return Items[Utility.Random(Items.Count)];
        }
    }
}
