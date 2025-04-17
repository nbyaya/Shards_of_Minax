using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AlchemyCollectionType
    {
        public static List<AlchemyCollectionType> Items = new List<AlchemyCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type PrimaryMaterialType { get; private set; }
        public int PrimaryMaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }

        public AlchemyCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type primaryMaterialType, int primaryMaterialAmount, Dictionary<Type, int> additionalResources = null)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            PrimaryMaterialType = primaryMaterialType;
            PrimaryMaterialAmount = primaryMaterialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
        }

        public static void PopulateAlchemyCollection()
        {
            Items = new List<AlchemyCollectionType>
            {
                new AlchemyCollectionType(typeof(RefreshPotion), "Refresh Potion", -25.0, 25.0, typeof(BlackPearl), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(TotalRefreshPotion), "Total Refresh Potion", 25.0, 75.0, typeof(BlackPearl), 5,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(LesserHealPotion), "Lesser Heal Potion", -25.0, 25.0, typeof(Ginseng), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(HealPotion), "Heal Potion", 15.0, 65.0, typeof(Ginseng), 3,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(GreaterHealPotion), "Greater Heal Potion", 55.0, 105.0, typeof(Ginseng), 7,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(LesserCurePotion), "Lesser Cure Potion", -10.0, 40.0, typeof(Garlic), 1,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(CurePotion), "Cure Potion", 25.0, 75.0, typeof(Garlic), 3,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),

                new AlchemyCollectionType(typeof(GreaterCurePotion), "Greater Cure Potion", 65.0, 115.0, typeof(Garlic), 6,
                    new Dictionary<Type, int> { { typeof(Bottle), 1 } }),
            };
        }

        public static AlchemyCollectionType GetRandomItem()
        {
            if (Items == null || Items.Count == 0)
                PopulateAlchemyCollection();

            return Items[Utility.Random(Items.Count)];
        }
    }
}
