using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MiningCollectionType
    {
        public static List<MiningCollectionType> Resources = new List<MiningCollectionType>();

        public double MinSkill { get; private set; }
        public double MaxSkill { get; private set; }
        public int ResourceID { get; private set; }
        public Type ResourceType { get; private set; }

        public MiningCollectionType(double minSkill, double maxSkill, Type resourceType)
        {
            MinSkill = minSkill;
            MaxSkill = maxSkill;
            ResourceType = resourceType;
        }

        public static void PopulateMiningCollection()
        {
            Resources = new List<MiningCollectionType>
            {
                new MiningCollectionType(0.0, 100.0, typeof(IronOre)),
                new MiningCollectionType(25.0, 105.0, typeof(DullCopperOre)),
                new MiningCollectionType(30.0, 110.0, typeof(ShadowIronOre)),
                new MiningCollectionType(35.0, 115.0, typeof(CopperOre)),
                new MiningCollectionType(40.0, 120.0, typeof(BronzeOre)),
                new MiningCollectionType(45.0, 125.0, typeof(GoldOre)),
                new MiningCollectionType(50.0, 130.0, typeof(AgapiteOre)),
                new MiningCollectionType(55.0, 135.0, typeof(VeriteOre)),
                new MiningCollectionType(59.0, 139.0, typeof(ValoriteOre))
            };
        }

        public static MiningCollectionType GetResourceBySkill(double skill)
        {
            foreach (var resource in Resources)
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
