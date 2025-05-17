// Create this file in e.g., Scripts/Mobiles/Vendors/DynamicVendor/
using System;

namespace Server.Mobiles
{
    // Holds the current price and available quantity for a dynamically stocked item
    public class DynamicStockInfo
    {
        public int Price { get; set; }
        public int Quantity { get; set; }

        public DynamicStockInfo(int price, int quantity)
        {
            Price = price;
            Quantity = Math.Max(0, quantity); // Ensure quantity isn't negative
        }
    }
}