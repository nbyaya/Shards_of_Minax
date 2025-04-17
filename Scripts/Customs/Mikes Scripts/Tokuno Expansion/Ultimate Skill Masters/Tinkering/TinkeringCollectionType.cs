using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TinkeringCollectionType
    {
        public static List<TinkeringCollectionType> Items = new List<TinkeringCollectionType>();

        public Type ItemType { get; private set; }
        public double MinSkill { get; private set; }
        public double MaxSkill { get; private set; }

        public TinkeringCollectionType(Type itemType, double minSkill, double maxSkill)
        {
            ItemType = itemType;
            MinSkill = minSkill;
            MaxSkill = maxSkill;
        }

        public static void PopulateTinkeringCollection()
        {
            Items = new List<TinkeringCollectionType>
            {
                new TinkeringCollectionType(typeof(GoldRing), 40.0, 90.0),
                new TinkeringCollectionType(typeof(SilverBeadNecklace), 40.0, 90.0),
                new TinkeringCollectionType(typeof(GoldNecklace), 40.0, 90.0),
                new TinkeringCollectionType(typeof(GoldEarrings), 40.0, 90.0),
                new TinkeringCollectionType(typeof(GoldBeadNecklace), 40.0, 90.0),
                new TinkeringCollectionType(typeof(GoldBracelet), 40.0, 90.0),
                new TinkeringCollectionType(typeof(JointingPlane), 0.0, 50.0),
                new TinkeringCollectionType(typeof(MouldingPlane), 0.0, 50.0),
                new TinkeringCollectionType(typeof(SmoothingPlane), 0.0, 50.0),
                new TinkeringCollectionType(typeof(ClockFrame), 0.0, 50.0),
                new TinkeringCollectionType(typeof(Axle), -25.0, 25.0),
                new TinkeringCollectionType(typeof(RollingPin), 0.0, 50.0),
                new TinkeringCollectionType(typeof(Nunchaku), 70.0, 120.0),
                new TinkeringCollectionType(typeof(Scissors), 5.0, 55.0),
                new TinkeringCollectionType(typeof(MortarPestle), 20.0, 70.0),
                new TinkeringCollectionType(typeof(Scorp), 30.0, 80.0),
                new TinkeringCollectionType(typeof(TinkerTools), 10.0, 60.0),
                new TinkeringCollectionType(typeof(Hatchet), 30.0, 80.0),
                new TinkeringCollectionType(typeof(DrawKnife), 30.0, 80.0),
                new TinkeringCollectionType(typeof(SewingKit), 10.0, 70.0),
                new TinkeringCollectionType(typeof(Saw), 30.0, 80.0),
                new TinkeringCollectionType(typeof(DovetailSaw), 30.0, 80.0),
                new TinkeringCollectionType(typeof(Froe), 30.0, 80.0),
                new TinkeringCollectionType(typeof(Shovel), 40.0, 90.0),
                new TinkeringCollectionType(typeof(Hammer), 30.0, 80.0),
                new TinkeringCollectionType(typeof(Tongs), 35.0, 85.0),
                new TinkeringCollectionType(typeof(SmithyHammer), 40.0, 90.0),
                new TinkeringCollectionType(typeof(SledgeHammerWeapon), 40.0, 90.0),
                new TinkeringCollectionType(typeof(Inshave), 30.0, 80.0),
                new TinkeringCollectionType(typeof(Pickaxe), 40.0, 90.0),
                new TinkeringCollectionType(typeof(Lockpick), 45.0, 95.0),
                new TinkeringCollectionType(typeof(Skillet), 30.0, 80.0),
                new TinkeringCollectionType(typeof(FlourSifter), 50.0, 100.0),
                new TinkeringCollectionType(typeof(FletcherTools), 35.0, 85.0),
                new TinkeringCollectionType(typeof(MapmakersPen), 25.0, 75.0),
                new TinkeringCollectionType(typeof(ScribesPen), 25.0, 75.0),
                new TinkeringCollectionType(typeof(Clippers), 50.0, 50.0)
            };
        }

        public static TinkeringCollectionType GetResourceBySkill(double skill)
        {
            foreach (var resource in Items)
            {
                if (skill >= resource.MinSkill && skill <= resource.MaxSkill)
                {
                    return resource;
                }
            }

            return null;
        }
    }
}
