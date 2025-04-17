using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SnoopingCollectionType
    {
        public static SnoopingCollectionType[] Items = new SnoopingCollectionType[]
        {
            new SnoopingCollectionType(typeof(GoldRing), "Gold Ring", 1),
            new SnoopingCollectionType(typeof(GoldNecklace), "Gold Necklace", 3),
            new SnoopingCollectionType(typeof(StarSapphire), "Star Sapphire", 3),
            new SnoopingCollectionType(typeof(Emerald), "Emerald", 2),
            new SnoopingCollectionType(typeof(Sapphire), "Sapphire", 2),
            new SnoopingCollectionType(typeof(Ruby), "Ruby", 3),
            new SnoopingCollectionType(typeof(Citrine), "Citrine", 3),
            new SnoopingCollectionType(typeof(Amethyst), "Amethyst", 2),
            new SnoopingCollectionType(typeof(Tourmaline), "Tourmaline", 2),
            new SnoopingCollectionType(typeof(Amber), "Amber", 5),

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

        public SnoopingCollectionType(Type type, string name, int rarity)
        {
            m_Type = type;
            m_Name = name;
            m_Rarity = rarity;
        }
    }
}
