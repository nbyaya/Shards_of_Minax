using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CarpentryCollectionType
    {
        public static List<CarpentryCollectionType> Items = new List<CarpentryCollectionType>();

        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public double MinDifficulty { get; private set; }
        public double MaxDifficulty { get; private set; }
        public Type MaterialType { get; private set; }
        public int MaterialAmount { get; private set; }
        public Dictionary<Type, int> AdditionalResources { get; private set; }

        public CarpentryCollectionType(Type itemType, string name, double minDifficulty, double maxDifficulty, Type materialType, int materialAmount, Dictionary<Type, int> additionalResources = null)
        {
            ItemType = itemType;
            Name = name;
            MinDifficulty = minDifficulty;
            MaxDifficulty = maxDifficulty;
            MaterialType = materialType;
            MaterialAmount = materialAmount;
            AdditionalResources = additionalResources ?? new Dictionary<Type, int>();
        }

        public static void PopulateCarpentryCollection()
        {
            Items = new List<CarpentryCollectionType>
            {
                new CarpentryCollectionType(typeof(BarrelStaves), "Barrel Staves", 0.0, 25.0, typeof(Board), 5),
                new CarpentryCollectionType(typeof(BarrelLid), "Barrel Lid", 11.0, 36.0, typeof(Board), 4),
                new CarpentryCollectionType(typeof(ShortMusicStandLeft), "Short Music Stand (Left)", 78.9, 103.9, typeof(Board), 15),
                new CarpentryCollectionType(typeof(ShortMusicStandRight), "Short Music Stand (Right)", 78.9, 103.9, typeof(Board), 15),
                new CarpentryCollectionType(typeof(TallMusicStandLeft), "Tall Music Stand (Left)", 81.5, 106.5, typeof(Board), 20),
                new CarpentryCollectionType(typeof(TallMusicStandRight), "Tall Music Stand (Right)", 81.5, 106.5, typeof(Board), 20),
                new CarpentryCollectionType(typeof(EasleSouth), "Easel (South)", 86.8, 111.8, typeof(Board), 20),
                new CarpentryCollectionType(typeof(EasleEast), "Easel (East)", 86.8, 111.8, typeof(Board), 20),
                new CarpentryCollectionType(typeof(EasleNorth), "Easel (North)", 86.8, 111.8, typeof(Board), 20),

                new CarpentryCollectionType(typeof(RedHangingLantern), "Red Hanging Lantern", 65.0, 90.0, typeof(Board), 5, new Dictionary<Type, int>{{ typeof(BlankScroll), 10 }}),
                new CarpentryCollectionType(typeof(WhiteHangingLantern), "White Hanging Lantern", 65.0, 90.0, typeof(Board), 5, new Dictionary<Type, int>{{ typeof(BlankScroll), 10 }}),

                new CarpentryCollectionType(typeof(ShojiScreen), "Shoji Screen", 80.0, 105.0, typeof(Board), 75, new Dictionary<Type, int>{{ typeof(Cloth), 60 }}),
                new CarpentryCollectionType(typeof(BambooScreen), "Bamboo Screen", 80.0, 105.0, typeof(Board), 75, new Dictionary<Type, int>{{ typeof(Cloth), 60 }}),

                new CarpentryCollectionType(typeof(FishingPole), "Fishing Pole", 68.4, 93.4, typeof(Board), 5, new Dictionary<Type, int>{{ typeof(Cloth), 5 }}),

                new CarpentryCollectionType(typeof(WoodenContainerEngraver), "Wooden Container Engraver", 75.0, 100.0, typeof(Board), 4, new Dictionary<Type, int>{{ typeof(IronIngot), 2 }}),
                new CarpentryCollectionType(typeof(RunedSwitch), "Runed Switch", 70.0, 120.0, typeof(Board), 2, new Dictionary<Type, int>{{ typeof(EnchantedSwitch), 1 }, { typeof(RunedPrism), 1 }, { typeof(JeweledFiligree), 1 }}),

                new CarpentryCollectionType(typeof(ArcanistStatueSouthDeed), "Arcanist Statue (South)", 0.0, 35.0, typeof(Board), 250),
                new CarpentryCollectionType(typeof(ArcanistStatueEastDeed), "Arcanist Statue (East)", 0.0, 35.0, typeof(Board), 250),

                new CarpentryCollectionType(typeof(GiantReplicaAcorn), "Giant Replica Acorn", 80.0, 105.0, typeof(Board), 35),
                new CarpentryCollectionType(typeof(MountedDreadHorn), "Mounted Dread Horn", 90.0, 115.0, typeof(Board), 50, new Dictionary<Type, int>{{ typeof(PristineDreadHorn), 1 }}),

                new CarpentryCollectionType(typeof(GargishBanner), "Gargish Banner", 94.7, 115.0, typeof(Board), 50, new Dictionary<Type, int>{{ typeof(Cloth), 50 }}),
                new CarpentryCollectionType(typeof(Incubator), "Incubator", 90.0, 115.0, typeof(Board), 100),
                new CarpentryCollectionType(typeof(ChickenCoop), "Chicken Coop", 90.0, 115.0, typeof(Board), 150),
                new CarpentryCollectionType(typeof(ExodusSummoningAlter), "Exodus Summoning Alter", 95.0, 120.0, typeof(Board), 100, new Dictionary<Type, int>{{ typeof(Granite), 10 }, { typeof(SmallPieceofBlackrock), 10 }, { typeof(NexusCore), 1 }}),

                new CarpentryCollectionType(typeof(FootStool), "Foot Stool", 11.0, 36.0, typeof(Board), 9),
                new CarpentryCollectionType(typeof(Stool), "Stool", 11.0, 36.0, typeof(Board), 9),
                new CarpentryCollectionType(typeof(WoodenChair), "Wooden Chair", 21.0, 46.0, typeof(Board), 13),
                new CarpentryCollectionType(typeof(FancyWoodenChairCushion), "Fancy Wooden Chair Cushion", 42.1, 67.1, typeof(Board), 15),

                new CarpentryCollectionType(typeof(WoodenBench), "Wooden Bench", 52.6, 77.6, typeof(Board), 17),
                new CarpentryCollectionType(typeof(WoodenThrone), "Wooden Throne", 52.6, 77.6, typeof(Board), 17),
                new CarpentryCollectionType(typeof(LargeTable), "Large Table", 84.2, 109.2, typeof(Board), 27),
                new CarpentryCollectionType(typeof(YewWoodTable), "Yew Wood Table", 63.1, 88.1, typeof(Board), 23),

                new CarpentryCollectionType(typeof(WoodenBox), "Wooden Box", 21.0, 46.0, typeof(Board), 10),
                new CarpentryCollectionType(typeof(WoodenChest), "Wooden Chest", 73.6, 98.6, typeof(Board), 20),
                new CarpentryCollectionType(typeof(Armoire), "Armoire", 84.2, 109.2, typeof(Board), 35)
            };
        }

        public static CarpentryCollectionType GetRandomItem()
        {
            return Items[Utility.Random(Items.Count)];
        }
    }
}
