using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FletchingCollectionType
    {
        public static List<FletchingCollectionType> Items = new List<FletchingCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }

        public FletchingCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount, Dictionary<Type, int> additionalResources = null)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            MaterialType = materialType;
            MaterialAmount = materialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
        }

        public static void PopulateFletchingCollection()
        {
            Items = new List<FletchingCollectionType>
            {
                new FletchingCollectionType(typeof(ElvenFletching), "Elven Fletching", 90.0, 130.0, typeof(Feather), 20, new Dictionary<Type, int>{{ typeof(FaeryDust), 1 }}),
                new FletchingCollectionType(typeof(Kindling), "Kindling", 0.0, 0.0, typeof(Board), 1),
                new FletchingCollectionType(typeof(Shaft), "Shaft", 0.0, 40.0, typeof(Board), 1),

                // Ammunition
                new FletchingCollectionType(typeof(Arrow), "Arrow", 0.0, 40.0, typeof(Shaft), 1, new Dictionary<Type, int>{{ typeof(Feather), 1 }}),
                new FletchingCollectionType(typeof(Bolt), "Bolt", 0.0, 40.0, typeof(Shaft), 1, new Dictionary<Type, int>{{ typeof(Feather), 1 }}),
                new FletchingCollectionType(typeof(FukiyaDarts), "Fukiya Darts", 50.0, 73.8, typeof(Board), 1),

                // Weapons
                new FletchingCollectionType(typeof(Bow), "Bow", 30.0, 70.0, typeof(Board), 7),
                new FletchingCollectionType(typeof(Crossbow), "Crossbow", 60.0, 100.0, typeof(Board), 7),
                new FletchingCollectionType(typeof(HeavyCrossbow), "Heavy Crossbow", 80.0, 120.0, typeof(Board), 10),

                new FletchingCollectionType(typeof(CompositeBow), "Composite Bow", 70.0, 110.0, typeof(Board), 7),
                new FletchingCollectionType(typeof(RepeatingCrossbow), "Repeating Crossbow", 90.0, 130.0, typeof(Board), 10),

                new FletchingCollectionType(typeof(Yumi), "Yumi", 90.0, 130.0, typeof(Board), 10),

                // Mondain's Legacy
                new FletchingCollectionType(typeof(ElvenCompositeLongbow), "Elven Composite Longbow", 95.0, 145.0, typeof(Board), 20),
                new FletchingCollectionType(typeof(MagicalShortbow), "Magical Shortbow", 85.0, 135.0, typeof(Board), 15)
            };
        }

        public static FletchingCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
