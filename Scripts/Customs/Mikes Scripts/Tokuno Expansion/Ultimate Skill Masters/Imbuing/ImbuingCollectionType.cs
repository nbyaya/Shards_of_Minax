using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ImbuingCollectionType
    {
        public static ImbuingCollectionType[] Items = new ImbuingCollectionType[]
        {
            new ImbuingCollectionType(typeof(Aetheralate), "Aetheralate", 1),
            new ImbuingCollectionType(typeof(Auroracene), "Auroracene", 3),
            new ImbuingCollectionType(typeof(Chronodyne), "Chronodyne", 3),
            new ImbuingCollectionType(typeof(Cryovitrin), "Cryovitrin", 2),
            new ImbuingCollectionType(typeof(Nebulifluxate), "Nebulifluxate", 2),
            new ImbuingCollectionType(typeof(Omniplasium), "Omniplasium", 3),
            new ImbuingCollectionType(typeof(Phantomide), "Phantomide", 3),
            new ImbuingCollectionType(typeof(Quarkothene), "Quarkothene", 2),
            new ImbuingCollectionType(typeof(Radiacrylate), "Radiacrylate", 2),
            new ImbuingCollectionType(typeof(Solvexium), "Solvexium", 5),

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

        public ImbuingCollectionType(Type type, string name, int rarity)
        {
            m_Type = type;
            m_Name = name;
            m_Rarity = rarity;
        }
    }
}
