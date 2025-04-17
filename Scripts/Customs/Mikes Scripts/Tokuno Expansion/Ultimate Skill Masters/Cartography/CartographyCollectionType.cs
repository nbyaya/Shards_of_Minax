using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CartographyCollectionType
    {
        public static List<CartographyCollectionType> Items = new List<CartographyCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }

        public CartographyCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            MaterialType = materialType;
            MaterialAmount = materialAmount;
        }

        public static void PopulateCartographyCollection()
        {
            Items = new List<CartographyCollectionType>
            {
                new CartographyCollectionType(typeof(LocalMap), "Local Map", 10.0, 70.0, typeof(BlankMap), 1),
                new CartographyCollectionType(typeof(CityMap), "City Map", 25.0, 85.0, typeof(BlankMap), 1),
                new CartographyCollectionType(typeof(SeaChart), "Sea Chart", 35.0, 95.0, typeof(BlankMap), 1),
                new CartographyCollectionType(typeof(WorldMap), "World Map", 39.5, 120.0, typeof(BlankMap), 1)
            };
        }

        public static CartographyCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
