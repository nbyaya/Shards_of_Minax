using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Targeting;
using Server.Commands; // Needed for CommandLogging if used, and general command infrastructure

namespace Server.Mobiles
{
    // Helper class moved inside BaseDynamicVendor or kept separate as DynamicStockInfo.cs
    // For simplicity here, let's assume DynamicStockInfo.cs exists as created previously:
    /*
    namespace Server.Mobiles
    {
        public class DynamicStockInfo
        {
            public int Price { get; set; }
            public int Quantity { get; set; }

            public DynamicStockInfo(int price, int quantity)
            {
                Price = price;
                Quantity = Math.Max(0, quantity);
            }
        }
    }
    */

    public abstract class BaseDynamicVendor : BaseCreature
    {
        private Timer m_RefreshTimer;
        private DateTime m_NextRefresh;
        private TimeSpan m_RefreshTimeDelay = TimeSpan.FromHours(1.0); // Default refresh interval
        private double m_PriceFluctuation = 0.20; // Default +/- 20% price range

        // Potential items this vendor *could* stock or buy (Defined in subclasses)
        // Assumes DynamicItemEntry.cs exists with: Type ItemType, int BasePrice, double InclusionChance, int MinStock, int MaxStock
        protected List<DynamicItemEntry> PotentialSellStock { get; } = new List<DynamicItemEntry>();
        protected List<DynamicItemEntry> PotentialBuyStock { get; } = new List<DynamicItemEntry>();

        // Actual items currently available or being bought, with their *current* prices and quantities
        public Dictionary<Type, DynamicStockInfo> CurrentSellItems { get; } = new Dictionary<Type, DynamicStockInfo>();
        public Dictionary<Type, DynamicStockInfo> CurrentBuyItems { get; } = new Dictionary<Type, DynamicStockInfo>();

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan RefreshTimeDelay
        {
            get { return m_RefreshTimeDelay; }
            set
            {
                m_RefreshTimeDelay = value;
                // Reschedule the next refresh based on the new delay
                if (m_RefreshTimer != null && m_RefreshTimer.Running) // Avoid error if timer not started
                     m_NextRefresh = DateTime.UtcNow + m_RefreshTimeDelay;
                StartRefreshTimer(); // Restart timer with potentially new interval check
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double PriceFluctuation
        {
            get { return m_PriceFluctuation; }
            // Ensure fluctuation is reasonable (e.g., 0% to 100%)
            set { m_PriceFluctuation = Math.Max(0.0, Math.Min(1.0, value)); }
        }

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
        public DateTime NextRefreshTime
        {
            get { return m_NextRefresh; }
             set { m_NextRefresh = value; } // Allow GM override for testing
        }

        // This property acts as a button in [props to trigger the refresh
        [CommandProperty(AccessLevel.GameMaster)]
        public bool TriggerRefreshNow
        {
            get{ return false; } // Doesn't need to store a real value
            set
            {
                // When a GM clicks "Set" or toggles this in [props, execute the refresh
                Console.WriteLine($"Manual inventory refresh triggered via [props for {this.Name} ({this.Serial}).");
                RefreshInventory();
                // Cannot easily send message back to GM from here with this pattern.
            }
        }

        public BaseDynamicVendor(string title) : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Title = title;
            InitBody();
            InitOutfit();
            InitDynamicStock(); // Initialize potential items

            // Perform initial refresh slightly delayed after spawn/load
             Timer.DelayCall(TimeSpan.FromSeconds(1), RefreshInventory);
             // Start the regular refresh timer
            StartRefreshTimer();
        }

        // Abstract method for subclasses to define their potential stock
        public abstract void InitDynamicStock();

        // Standard methods often overridden for vendors
        public virtual void InitBody()
        {
            InitStats(100, 100, 25);
            Hue = Utility.RandomSkinHue();
            if (Female = Utility.RandomBool())
            {
                 Body = 0x191; // Female human
                 Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190; // Male human
                Name = NameList.RandomName("male");
            }
        }

        public virtual void InitOutfit()
        {
            // Default simple outfit, subclasses should override for better variety
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new BodySash(Utility.RandomNeutralHue()));
            AddItem(Utility.RandomBool() ? (Item)new Boots() : new ThighBoots());
            AddItem(new ShortHair(Utility.RandomHairHue()));

            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);
        }

        private void StartRefreshTimer()
        {
            m_RefreshTimer?.Stop(); // Stop existing timer if any

            // Timer ticks more frequently than refresh to check if it's time
            TimeSpan checkInterval = TimeSpan.FromMinutes(1.0);
            m_RefreshTimer = Timer.DelayCall(checkInterval, checkInterval, CheckRefresh);
            m_RefreshTimer.Priority = TimerPriority.OneMinute;
            m_RefreshTimer.Start();
             // Ensure NextRefresh has a value if loaded from old save or freshly created
             if (m_NextRefresh == DateTime.MinValue)
                 m_NextRefresh = DateTime.UtcNow + m_RefreshTimeDelay;
        }

        private void CheckRefresh()
        {
            // Add null check for safety
            if (this.Deleted || this.Map == null || this.Map == Map.Internal)
            {
                m_RefreshTimer?.Stop();
                return;
            }

            if (DateTime.UtcNow >= m_NextRefresh)
            {
                RefreshInventory();
            }
        }

        protected virtual void RefreshInventory()
        {
            // If the vendor is deleted or internal map, stop processing
             if (this.Deleted || this.Map == null || this.Map == Map.Internal)
             {
                  Console.WriteLine($"DEBUG: Attempted to refresh inventory for deleted/internal vendor {this.Name} ({this.Serial}). Aborting.");
                  m_RefreshTimer?.Stop(); // Stop timer if vendor is invalid
                 return;
             }

            Console.WriteLine($"DEBUG: Refreshing inventory for {this.Name} ({this.Serial})");

            CurrentSellItems.Clear();
            CurrentBuyItems.Clear();

            // Populate Sell List
            foreach (var entry in PotentialSellStock)
            {
                // Check ItemType is valid before proceeding
                 if (entry.ItemType == null || !typeof(Item).IsAssignableFrom(entry.ItemType))
                 {
                     Console.WriteLine($"WARNING: Invalid ItemType '{entry.ItemType?.FullName ?? "NULL"}' in PotentialSellStock for {this.GetType().Name}");
                     continue;
                 }

                if (Utility.RandomDouble() < entry.InclusionChance)
                {
                    int price = CalculatePrice(entry.BasePrice);
                    // Calculate Random Quantity using MinStock/MaxStock from DynamicItemEntry
                    int quantity = Utility.RandomMinMax(entry.MinStock, entry.MaxStock);

                    if (!CurrentSellItems.ContainsKey(entry.ItemType)) // Avoid duplicates if type added multiple times
                    {
                         // Store Price and Quantity
                         CurrentSellItems.Add(entry.ItemType, new DynamicStockInfo(price, quantity));
                    }
                }
            }

            // Populate Buy List (Items the vendor wants to buy)
            foreach (var entry in PotentialBuyStock)
            {
                 // Check ItemType is valid before proceeding
                 if (entry.ItemType == null || !typeof(Item).IsAssignableFrom(entry.ItemType))
                 {
                     Console.WriteLine($"WARNING: Invalid ItemType '{entry.ItemType?.FullName ?? "NULL"}' in PotentialBuyStock for {this.GetType().Name}");
                     continue;
                 }

                if (Utility.RandomDouble() < entry.InclusionChance)
                {
                    int price = CalculatePrice(entry.BasePrice);
                    // Calculate Random Quantity (Demand) using MinStock/MaxStock
                    int quantity = Utility.RandomMinMax(entry.MinStock, entry.MaxStock);

                     if (!CurrentBuyItems.ContainsKey(entry.ItemType))
                    {
                        // Store Price and Quantity (Demand)
                        CurrentBuyItems.Add(entry.ItemType, new DynamicStockInfo(price, quantity));
                    }
                }
            }

            // Schedule next refresh
            m_NextRefresh = DateTime.UtcNow + m_RefreshTimeDelay;
             Console.WriteLine($"DEBUG: Inventory refresh complete for {this.Name}. {CurrentSellItems.Count} types for sale, {CurrentBuyItems.Count} types wanted. Next refresh at {m_NextRefresh}");
        }

        protected virtual int CalculatePrice(int basePrice)
        {
            if (basePrice <= 0) return 1; // Ensure price is positive

            double fluctuation = (Utility.RandomDouble() * 2.0 - 1.0) * m_PriceFluctuation; // Random between -m_PriceFluctuation and +m_PriceFluctuation
            int price = (int)(basePrice * (1.0 + fluctuation));

            return Math.Max(1, price); // Ensure price is at least 1
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            // Respond to vendor interaction keywords
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            Mobile from = e.Mobile;

            if (!e.Handled && from.InRange(this, 5)) // Increased range slightly for speech interaction
            {
                // Basic keywords to open the Gump
                // Ensure case-insensitivity might be better: e.Speech.ToLowerInvariant().Contains(...)
                if (e.HasKeyword(0x3C) || e.HasKeyword(0x177)) // *buy* or *vendor buy*
                {
                    e.Handled = true;
                    from.SendGump(new DynamicVendorGump(from, this));
                }
                 else if (e.HasKeyword(0x3D) || e.HasKeyword(0x171)) // *sell* or *vendor sell*
                {
                     e.Handled = true;
                    from.SendGump(new DynamicVendorGump(from, this));
                }
                 else if (e.Speech.ToLowerInvariant().Contains("browse") || e.Speech.ToLowerInvariant().Contains(this.Name?.ToLowerInvariant() ?? "vendor")) // Check for "browse" or vendor's name
                {
                    e.Handled = true;
                    from.SendGump(new DynamicVendorGump(from, this));
                }
            }
        }

         public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive && from.InRange(this, 5))
            {
                 list.Add( new VendorBrowseEntry( from, this ) );
            }
            base.AddCustomContextEntries( from, list );
        }

         private class VendorBrowseEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private BaseDynamicVendor m_Vendor;

            public VendorBrowseEntry( Mobile from, BaseDynamicVendor vendor ) : base( 6103, 5 ) // 6102 = Browse
            {
                m_From = from;
                m_Vendor = vendor;
            }

            public override void OnClick()
            {
                 if (m_From.CheckAlive() && m_Vendor != null && !m_Vendor.Deleted && m_From.InRange(m_Vendor, 5))
                 {
                      m_From.SendGump( new DynamicVendorGump( m_From, m_Vendor ) );
                 }
            }
        }


        // --- Transaction Logic ---

        public virtual bool OnBuyItem(Mobile buyer, Type itemType, int price, int quantity)
        {
            if (quantity < 1) // Basic validation
                return false;

            // Retrieve Stock Info - includes price and current quantity
            if (!CurrentSellItems.TryGetValue(itemType, out DynamicStockInfo stockInfo) || stockInfo.Price != price)
            {
                SayTo(buyer, "My prices or stock may have changed, please look again."); // Price mismatch or item removed/price changed
                return false;
            }

            // Check Available Quantity
            if (quantity > stockInfo.Quantity)
            {
                SayTo(buyer, $"I only have {stockInfo.Quantity} of those left in stock.");
                return false;
            }

            Container pack = buyer.Backpack;
            if (pack == null)
            {
                buyer.SendMessage("You seem to be missing a backpack!");
                return false;
            }

            // Check stackability BEFORE calculating total cost/creating item
            bool isStackable = true;

            if (quantity > 1 && !isStackable)
            {
                 SayTo(buyer, "You can only purchase one of those items at a time.");
                 return false;
            }

            // Calculate total cost using current price from stockInfo
            long totalCost = (long)stockInfo.Price * quantity; // Use long to prevent overflow

            if (totalCost <= 0) // Safety check if price or quantity was weird
                return false;

            if (totalCost > Int32.MaxValue) // Prevent trying to withdraw more than max gold stack
            {
                 SayTo(buyer, "That's too large a quantity to purchase at once.");
                 return false;
            }

            int totalCostInt = (int)totalCost;


            // --- Check Gold ---
            bool paidFromBank = false;
            if (Banker.Withdraw(buyer, totalCostInt))
            {
                 paidFromBank = true;
            }
            else if (!pack.ConsumeTotal(typeof(Gold), totalCostInt))
            {
                SayTo(buyer, 1042292); // You cannot afford that.
                return false;
            }

            // Send payment confirmation message
             if (paidFromBank)
                buyer.SendLocalizedMessage( 1060398, totalCostInt.ToString("#,0") ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
             else
                buyer.SendMessage($"You pay {totalCostInt:#,0} gold.");


            // --- Create and Add Item(s) ---
            try
            {
                 Item item = Activator.CreateInstance(itemType) as Item;
                 if (item == null)
                 {
                     Console.WriteLine($"ERROR: Failed to create instance of {itemType.Name} for vendor {this.Name}");
                     SayTo(buyer, "Sorry, I cannot seem to find that item right now.");
                     Banker.Deposit(buyer, totalCostInt); // Attempt refund
                     return false;
                 }

                 // Set quantity if stackable
                 if (isStackable)
                 {
                     item.Amount = quantity;
                     // Optional: Could add checks against BaseStackable.MaxAmount here if needed
                 }
                 // Note: If !isStackable, quantity must be 1 due to earlier check


                // --- Check Space and Drop ---
                // Attempt backpack first, then equip as fallback
                if (!pack.TryDropItem(buyer, item, false) && !buyer.EquipItem(item))
                {
                    SayTo(buyer, 1042291); // You do not have room in your backpack for that.
                    item.Delete(); // Delete the item if it can't be dropped or equipped
                    Banker.Deposit(buyer, totalCostInt); // Refund player
                    return false;
                }

                // *** DECREASE STOCK ***
                stockInfo.Quantity -= quantity;
                if (stockInfo.Quantity <= 0)
                {
                    CurrentSellItems.Remove(itemType); // Remove from sale list if depleted
                    Console.WriteLine($"DEBUG: {this.Name} ran out of stock for {itemType.Name}.");
                }

                // Success!
                 SayTo(buyer, 1042290); // Here is your purchase. (UO default message)
                 // Optional: Play sound effect
                 buyer.PlaySound(0x57); // Standard buy sound
                 return true;

            }
             catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Exception creating or dropping item {itemType.Name} for {this.Name}: {ex}");
                SayTo(buyer, "An error occurred during the transaction.");
                 Banker.Deposit(buyer, totalCostInt); // Attempt refund
                 return false;
            }
        }


        // Modified to pass the specific DynamicStockInfo (which includes price and remaining quantity)
        public virtual void StartSellItem(Mobile seller, Type itemType, DynamicStockInfo stockInfo)
        {
            if (stockInfo == null || stockInfo.Quantity <= 0)
            {
                SayTo(seller, "I don't seem to need any of those right now.");
                return;
            }

             seller.SendMessage("Target the {0} you wish to sell (I need up to {1}).", GetItemName(itemType), stockInfo.Quantity);
             // Pass the specific stockInfo object to the target handler
             seller.Target = new SellTarget(this, itemType, stockInfo);
        }

        // Modified SellTarget to use DynamicStockInfo and handle partial stack selling
        private class SellTarget : Target
        {
            private BaseDynamicVendor m_Vendor;
            private Type m_ItemType;
            private DynamicStockInfo m_StockInfo; // Reference to the specific stock info at the time Gump was clicked

            public SellTarget(BaseDynamicVendor vendor, Type itemType, DynamicStockInfo stockInfo) : base(3, false, TargetFlags.None)
            {
                m_Vendor = vendor;
                m_ItemType = itemType;
                m_StockInfo = stockInfo;
                 AllowNonlocal = true; // Allow targeting items in pack
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Vendor == null || m_Vendor.Deleted) return;

                // Re-validate the vendor still wants this item type using the CURRENT dictionary state
                // Make sure the specific stockInfo object we based our Gump click on is still the one in the dictionary (ensures price/quantity didn't drastically change)
                 if (!m_Vendor.CurrentBuyItems.TryGetValue(m_ItemType, out DynamicStockInfo currentStockInfo) || currentStockInfo != m_StockInfo || currentStockInfo.Quantity <= 0)
                 {
                     m_Vendor.SayTo(from,"My needs seem to have changed since we last spoke, or I no longer need that item.");
                     return;
                 }


                if (targeted is Item targetItem)
                {
                    // Basic type/location checks
                    if (targetItem.GetType() != m_ItemType) { m_Vendor.SayTo(from, "That is not the item I was looking for."); return; }
                    if (!targetItem.IsChildOf(from.Backpack)) { m_Vendor.SayTo(from, "You must have the item in your backpack to sell it."); return; }
                    if (targetItem.Amount <= 0) { from.SendMessage("There is nothing there to sell."); return; } // Should not happen with valid items

                    // Determine how many to sell
                    int vendorWants = currentStockInfo.Quantity; // How many MORE the vendor wants NOW
                    int playerHas = targetItem.Amount; // Amount in the targeted stack/item
                    int amountToSell = Math.Min(vendorWants, playerHas); // Sell the lower of the two

                    if (amountToSell <= 0)
                    {
                        // This case should be caught by the initial check, but double-check
                        m_Vendor.SayTo(from, "I don't seem to need any more of those right now.");
                        return;
                    }

                    // Check if vendor can afford it (Optional, assume infinite gold for dynamic vendor usually)
                    long payment = (long)currentStockInfo.Price * amountToSell;
                    if (payment <= 0) { Console.WriteLine($"WARNING: Calculated payment <= 0 for {amountToSell} x {m_ItemType.Name}"); return; }
                    if (payment > Int32.MaxValue) { from.SendMessage("That's too much gold for me to handle at once!"); return; }
                    int paymentInt = (int)payment;

                    // --- Process Transaction ---
                    // 1. Consume items from player
                    targetItem.Consume(amountToSell); // Consume the amount being sold; deletes item if stack reaches 0

                    // 2. Give gold to player
                    if (!Banker.Deposit(from, paymentInt))
                    {
                         from.AddToBackpack(new Gold(paymentInt));
                         from.SendLocalizedMessage( 1042971, paymentInt.ToString("#,0") ); // Receive gold (no bank)
                    }
                    else
                    {
                         from.SendLocalizedMessage( 1042972, paymentInt.ToString("#,0") ); // Deposited gold
                    }

                    // 3. Decrease vendor demand quantity
                    currentStockInfo.Quantity -= amountToSell;
                    if (currentStockInfo.Quantity <= 0)
                    {
                        m_Vendor.CurrentBuyItems.Remove(m_ItemType); // Remove if demand met
                        Console.WriteLine($"DEBUG: {m_Vendor.Name} demand met for {m_ItemType.Name}.");
                    }

                    // 4. Confirmation
                    m_Vendor.SayTo(from, $"Thank you! I purchased {amountToSell} {(amountToSell != 1 ? GetItemName(m_ItemType) + "s" : GetItemName(m_ItemType))}.");
                    from.PlaySound(0x57); // Standard buy/sell sound
                }
                else
                {
                    m_Vendor.SayTo(from, "You must target the item you wish to sell.");
                }
            }
        }

         // Helper to get a user-friendly name (could be improved)
         // Moved static helper inside the class for encapsulation or keep outside if used elsewhere
         private static string GetItemName(Type type)
        {
            if (type == null) return "Unknown Item";
            string name = type.Name;
            // Simple way to add spaces before capital letters, except the first one.
            return System.Text.RegularExpressions.Regex.Replace(name, "(?<!^)([A-Z])", " $1");
        }

        // --- Serialization ---
        // Note: CurrentSellItems/CurrentBuyItems are NOT serialized by default.
        // They are regenerated by RefreshInventory on load/timer.
        public BaseDynamicVendor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version

            writer.Write(m_RefreshTimeDelay);
            writer.Write(m_PriceFluctuation);
            writer.Write(m_NextRefresh);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_RefreshTimeDelay = reader.ReadTimeSpan();
            m_PriceFluctuation = reader.ReadDouble();
            m_NextRefresh = reader.ReadDateTime();

            // Re-initialize potential stock from the specific vendor type
            InitDynamicStock(); // Make sure Potential Lists are populated

            // Perform a refresh on load to populate current items
            // Use DelayCall to ensure the world is likely loaded and vendor is placed
            Timer.DelayCall(TimeSpan.FromSeconds(5), RefreshInventory); // Increased delay slightly for safety

            // Start the regular refresh timer checking mechanism
            StartRefreshTimer();
        }
    } // End Class BaseDynamicVendor
} // End Namespace Server.Mobiles