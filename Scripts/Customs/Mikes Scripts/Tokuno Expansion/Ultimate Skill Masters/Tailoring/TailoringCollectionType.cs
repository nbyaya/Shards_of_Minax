using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TailoringCollectionType
    {
        public static List<TailoringCollectionType> Items = new List<TailoringCollectionType>();

        public Type ItemType { get; private set; }
        public double MinSkill { get; private set; }
        public double MaxSkill { get; private set; }

        public TailoringCollectionType(Type itemType, double minSkill, double maxSkill)
        {
            ItemType = itemType;
            MinSkill = minSkill;
            MaxSkill = maxSkill;
        }

        public static void PopulateTailoringCollection()
        {
            Items = new List<TailoringCollectionType>
            {
                new TailoringCollectionType(typeof(SkullCap), 0.0, 25.0),
                new TailoringCollectionType(typeof(Bandana), 0.0, 25.0),
                new TailoringCollectionType(typeof(FloppyHat), 6.2, 31.2),
                new TailoringCollectionType(typeof(Cap), 6.2, 31.2),
                new TailoringCollectionType(typeof(WideBrimHat), 6.2, 31.2),
                new TailoringCollectionType(typeof(StrawHat), 6.2, 31.2),
                new TailoringCollectionType(typeof(TallStrawHat), 6.7, 31.7),
                new TailoringCollectionType(typeof(WizardsHat), 7.2, 32.2),
                new TailoringCollectionType(typeof(Bonnet), 6.2, 31.2),
                new TailoringCollectionType(typeof(FeatheredHat), 6.2, 31.2),
                new TailoringCollectionType(typeof(TricorneHat), 6.2, 31.2),
                new TailoringCollectionType(typeof(JesterHat), 7.2, 32.2),
                new TailoringCollectionType(typeof(FlowerGarland), 10.0, 35.0),
                new TailoringCollectionType(typeof(ClothNinjaHood), 80.0, 105.0),
                new TailoringCollectionType(typeof(Kasa), 60.0, 85.0),
                new TailoringCollectionType(typeof(OrcMask), 75.0, 100.0),
                new TailoringCollectionType(typeof(BearMask), 77.5, 102.5),
                new TailoringCollectionType(typeof(DeerMask), 77.5, 102.5),
                new TailoringCollectionType(typeof(TribalMask), 82.5, 107.5),
                new TailoringCollectionType(typeof(HornedTribalMask), 82.5, 107.5),
                new TailoringCollectionType(typeof(Doublet), 0.0, 25.0),
                new TailoringCollectionType(typeof(Shirt), 20.7, 45.7),
                new TailoringCollectionType(typeof(FancyShirt), 24.8, 49.8),
                new TailoringCollectionType(typeof(Tunic), 0.0, 25.0),
                new TailoringCollectionType(typeof(Surcoat), 8.2, 33.2),
                new TailoringCollectionType(typeof(PlainDress), 12.4, 37.4),
                new TailoringCollectionType(typeof(FancyDress), 33.1, 58.1),
                new TailoringCollectionType(typeof(Cloak), 41.4, 66.4),
                new TailoringCollectionType(typeof(Robe), 53.9, 78.9),
                new TailoringCollectionType(typeof(JesterSuit), 8.2, 33.2),
                new TailoringCollectionType(typeof(ClothNinjaJacket), 75.0, 100.0),
                new TailoringCollectionType(typeof(Kamishimo), 75.0, 100.0),
                new TailoringCollectionType(typeof(HakamaShita), 40.0, 65.0),
                new TailoringCollectionType(typeof(MaleKimono), 50.0, 75.0),
                new TailoringCollectionType(typeof(FemaleKimono), 50.0, 75.0),
                new TailoringCollectionType(typeof(JinBaori), 30.0, 55.0),
                new TailoringCollectionType(typeof(FurCape), 35.0, 60.0),
                new TailoringCollectionType(typeof(GildedDress), 37.5, 62.5),
                new TailoringCollectionType(typeof(FormalShirt), 26.0, 51.0),
                new TailoringCollectionType(typeof(ShortPants), 24.8, 49.8),
                new TailoringCollectionType(typeof(LongPants), 24.8, 49.8),
                new TailoringCollectionType(typeof(Kilt), 20.7, 45.7),
                new TailoringCollectionType(typeof(Skirt), 29.0, 54.0),
                new TailoringCollectionType(typeof(ElvenShirt), 80.0, 105.0),
                new TailoringCollectionType(typeof(ElvenDarkShirt), 80.0, 105.0),
                new TailoringCollectionType(typeof(ElvenPants), 80.0, 105.0),
                new TailoringCollectionType(typeof(MaleElvenRobe), 80.0, 105.0),
                new TailoringCollectionType(typeof(FemaleElvenRobe), 80.0, 105.0),
                new TailoringCollectionType(typeof(WoodlandBelt), 80.0, 105.0)
            };
        }

        public static TailoringCollectionType GetResourceBySkill(double skill)
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
