// Modify this file in e.g., Scripts/Mobiles/Vendors/DynamicVendor/
using System;

namespace Server.Mobiles
{
    public class DynamicItemEntry
    {
        public Type ItemType { get; }
        public int BasePrice { get; }
        public double InclusionChance { get; } // 0.0 to 1.0
        public int MinStock { get; }         // Minimum stock/demand when refreshed
        public int MaxStock { get; }         // Maximum stock/demand when refreshed

        public DynamicItemEntry(Type itemType, int basePrice, double inclusionChance, int minStock, int maxStock)
        {
            ItemType = itemType;
            BasePrice = basePrice;
            InclusionChance = Math.Max(0.0, Math.Min(1.0, inclusionChance));

            // Ensure min isn't > max and both are reasonable
            MinStock = Math.Max(1, minStock); // Must have at least 1
            MaxStock = Math.Max(MinStock, maxStock); // Max must be >= Min
        }
    }
}