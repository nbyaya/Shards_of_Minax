// File: Scripts/Mobiles/Vendors/DynamicVendor/DynamicHerbalist.cs
using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class DynamicHerbalist : BaseDynamicVendor
    {
        [Constructable]
        public DynamicHerbalist() : base("the dynamic herbalist")
        {
            SetSkill(SkillName.Alchemy, 80.0, 100.0);
            SetSkill(SkillName.TasteID, 80.0, 100.0);
        }

        public override void InitDynamicStock()
        {
            // --- Potential Items to SELL ---
            // Syntax: (Type, BasePrice, Chance, MinStock, MaxStock)
            PotentialSellStock.Add(new DynamicItemEntry(typeof(Ginseng),       5, 0.80, 20, 50)); // Sells 20-50
            PotentialSellStock.Add(new DynamicItemEntry(typeof(Garlic),        5, 0.80, 20, 50));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MandrakeRoot),  7, 0.75, 15, 40));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(Nightshade),    7, 0.75, 15, 40));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(Bloodmoss),     9, 0.70, 10, 30));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MortarPestle), 15, 0.90,  5, 15)); // Sells 5-15
            PotentialSellStock.Add(new DynamicItemEntry(typeof(Bottle),        8, 0.95, 50, 100));// Sells 50-100
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlackPearl),   10, 0.40,  5, 20));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ExecutionersCap),12, 0.30, 3, 10));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DeadWood),      4, 0.50, 10, 30));

            // --- Potential Items to BUY ---
            // Syntax: (Type, BasePurchasePrice, ChanceToBuy, MinDemand, MaxDemand)
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Ginseng),        2, 0.60, 30, 60)); // Wants to buy 30-60
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Garlic),         2, 0.60, 30, 60));
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(MandrakeRoot),   3, 0.55, 25, 50));
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Nightshade),     3, 0.55, 25, 50));
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Bloodmoss),      4, 0.50, 20, 40));
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Bottle),         3, 0.70, 40, 80)); // Wants 40-80 bottles
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(MortarPestle),   5, 0.25,  1,  5)); // Only wants 1-5 used mortars
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(FertileDirt),    1, 0.40, 10, 25));
        }

        // ... (Keep InitOutfit, Constructor, Serialize, Deserialize) ...
        public DynamicHerbalist(Serial serial) : base(serial) { } // Ensure serial constructor exists
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }

        // Override InitOutfit or other methods as needed
        public override void InitOutfit()
        {
            AddItem( new Server.Items.Robe( Utility.RandomGreenHue() ) );
            AddItem( new Server.Items.Sandals(Utility.RandomNeutralHue()));
             AddItem( new Server.Items.WideBrimHat( Utility.RandomGreenHue()));
            AddItem(new ShortHair(Utility.RandomHairHue()));
            Container pack = new Backpack(); pack.Movable = false; AddItem(pack);
        }
    }
}