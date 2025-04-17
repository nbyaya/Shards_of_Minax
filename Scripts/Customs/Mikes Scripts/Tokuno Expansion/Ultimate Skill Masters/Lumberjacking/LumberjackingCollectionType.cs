using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LumberjackingCollectionType
    {
        public static List<LumberjackingCollectionType> Resources = new List<LumberjackingCollectionType>();

        public double MinSkill { get; private set; }
        public double MidSkill { get; private set; }
        public double MaxSkill { get; private set; }
        public Type ResourceType { get; private set; }

        public LumberjackingCollectionType(double minSkill, double maxSkill, Type resourceType)
        {
            MinSkill = minSkill;
            MaxSkill = maxSkill;
            ResourceType = resourceType;
        }

        public static void PopulateLumberjackingCollection()
        {
            Resources = new List<LumberjackingCollectionType>
            {
                new LumberjackingCollectionType(00.0, 100.0, typeof(Log)),
                new LumberjackingCollectionType(25.0, 105.0, typeof(OakLog)),
                new LumberjackingCollectionType(40.0, 120.0, typeof(AshLog)),
                new LumberjackingCollectionType(55.0, 135.0, typeof(YewLog)),
                new LumberjackingCollectionType(60.0, 140.0, typeof(HeartwoodLog)),
                new LumberjackingCollectionType(60.0, 140.0, typeof(BloodwoodLog)),
                new LumberjackingCollectionType(60.0, 140.0, typeof(FrostwoodLog))
            };
        }

        public static LumberjackingCollectionType GetResourceBySkill(double skill)
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
