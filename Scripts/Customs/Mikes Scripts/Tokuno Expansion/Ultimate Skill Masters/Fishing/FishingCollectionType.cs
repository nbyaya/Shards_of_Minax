using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FishingCollectionType
    {
        public static FishingCollectionType[] Items = new FishingCollectionType[]
        {
            new FishingCollectionType(typeof(Fish), "Fish", 1),
            new FishingCollectionType(typeof(Haddock), "Haddock", 3),
            new FishingCollectionType(typeof(Tarpon), "Tarpon", 3),
            new FishingCollectionType(typeof(CapeCod), "Cape Cod", 2),
            new FishingCollectionType(typeof(BlackSeabass), "Black Seabass", 2),
            new FishingCollectionType(typeof(GraySnapper), "Gray Snapper", 3),
            new FishingCollectionType(typeof(Amberjack), "Amberjack", 3),
            new FishingCollectionType(typeof(YellowfinTuna), "Yellowfin Tuna", 2),
            new FishingCollectionType(typeof(RedGrouper), "Red Grouper", 2),
            new FishingCollectionType(typeof(Bonefish), "Bonefish", 5),

        };

        public static int Random()
        {
            return Utility.Random(Items.Length);
        }

        private Type m_Type;
        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private int m_Rarity;
        public int Rarity
        {
            get { return m_Rarity; }
            set { m_Rarity = value; }
        }

        public FishingCollectionType(Type type, string name, int rarity)
        {
            m_Type = type;
            m_Name = name;
            m_Rarity = rarity;
        }
    }
}
