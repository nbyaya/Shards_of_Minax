// File: Scripts/UI/Gumps/DynamicVendorGump.cs
using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class DynamicVendorGump : Gump
    {
        private Mobile m_Buyer;
        private BaseDynamicVendor m_Vendor;

        private const int ItemsPerPage = 8;
        private int m_Page = 0;
        private int m_SubPage = 0;

        private const int Hue_BrightWhite = 1153;
        private const int Hue_White = 110;
        private const int Hue_Yellow = 90;
        private const int Hue_Red = 37;
        private const int Hue_Green = 67;
        private const int Hue_Blue = 68; // For stock amount

        public DynamicVendorGump(Mobile buyer, BaseDynamicVendor vendor, int page = 0, int subPage = 0) : base(50, 50)
        {
            m_Buyer = buyer;
            m_Vendor = vendor;
            m_Page = page;
            m_SubPage = subPage;
            buyer.CloseGump(typeof(DynamicVendorGump));
            BuildGump();
        }

        private void BuildGump()
        {
            Closable = true; Disposable = true; Dragable = true; Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 520, 465, 9270);
            AddAlphaRegion(10, 10, 500, 445);
            AddLabel(180, 15, Hue_BrightWhite, $"--- {m_Vendor.Name} ---");

            // --- Tabs ---
            int sellTabHue = (m_Page == 0) ? Hue_BrightWhite : Hue_White;
            int buyTabHue = (m_Page == 1) ? Hue_BrightWhite : Hue_White;
            AddButton(20, 40, (m_Page == 0 ? 4005 : 4007), (m_Page == 0 ? 4007: 4005), 1, GumpButtonType.Reply, 0); AddLabel(55, 42, sellTabHue, "Items For Sale");
            AddButton(180, 40, (m_Page == 1 ? 4005 : 4007), (m_Page == 1 ? 4007 : 4005), 2, GumpButtonType.Reply, 0); AddLabel(215, 42, buyTabHue, "We Buy These Items");

            // --- Content Area ---
            AddImageTiled(15, 70, 490, 355, 2624); AddAlphaRegion(16, 71, 488, 353);

            // Get correct list - ** Now uses DynamicStockInfo **
            var currentListRaw = (m_Page == 0) ? m_Vendor.CurrentSellItems : m_Vendor.CurrentBuyItems;
            var listEntries = new List<KeyValuePair<Type, DynamicStockInfo>>(currentListRaw); // List of Type -> StockInfo

            // Paging calculation
            int totalItems = listEntries.Count;
            int totalPages = (totalItems + ItemsPerPage - 1) / ItemsPerPage;
            int startIndex = m_SubPage * ItemsPerPage;
            int endIndex = Math.Min(startIndex + ItemsPerPage, totalItems);

            // Display Item Count Label
            string countLabel = $"{(m_Page == 0 ? "Selling" : "Buying")} {totalItems} type{(totalItems != 1 ? "s" : "")}";
            if (totalItems > ItemsPerPage) { countLabel += $" (Showing {startIndex + 1}-{endIndex})"; }
            AddLabel(30, 75, Hue_BrightWhite, countLabel);

            // Headers (Adjusted X positions for Quantity/Stock)
            int headerY = 95;
            AddLabel(30, headerY, Hue_BrightWhite, "Item");
            AddLabel(240, headerY, Hue_BrightWhite, "Stock");  // Changed from Price, shifted
            AddLabel(300, headerY, Hue_BrightWhite, "Price");  // Shifted price
            if (m_Page == 0) { AddLabel(370, headerY, Hue_BrightWhite, "Qty"); } // Qty to Buy
            AddLabel(430, headerY, Hue_BrightWhite, "Action");

            // Display items
            int y = 120;
            int textEntryBaseId = 3000;
            for (int i = startIndex; i < endIndex; i++)
            {
                Type itemType = listEntries[i].Key;
                DynamicStockInfo stockInfo = listEntries[i].Value; // Get the StockInfo object
                string itemName = GetItemName(itemType);

                AddLabel(30, y, Hue_White, itemName);                         // Item Name
                AddLabel(240, y, Hue_Blue, stockInfo.Quantity.ToString("#,0")); // Stock/Demand Qty
                AddLabel(300, y, Hue_Yellow, stockInfo.Price.ToString("#,0")); // Price

                int buttonId = (m_Page == 0) ? (1000 + i) : (2000 + i);
                string buttonLabel = (m_Page == 0) ? "Buy" : "Sell";

                if (m_Page == 0) // Quantity to Buy Entry
                {
                    AddTextEntry(365, y, 40, 20, Hue_Yellow, textEntryBaseId + i, "1");
                }

                AddButton(430, y, 4005, 4007, buttonId, GumpButtonType.Reply, 0); // Action Button
                AddLabel(460, y - 1 , Hue_Red, buttonLabel);                     // Action Label

                y += 30;
            }

            // --- Paging Controls --- (Adjusted Y)
             int pageControlY = 430;
             // ...(Paging logic remains the same)...
            if (totalPages > 1)
            { AddLabel(200, pageControlY, Hue_BrightWhite, $"Page {m_SubPage + 1} / {totalPages}");
              if (m_SubPage > 0) { AddButton(170, pageControlY, 4014, 4016, 3, GumpButtonType.Reply, 0); } else { AddImage(170, pageControlY, 4014); }
              if (m_SubPage < totalPages - 1) { AddButton(290, pageControlY, 4005, 4007, 4, GumpButtonType.Reply, 0); } else { AddImage(290, pageControlY, 4005); }
            }
            AddButton(470, pageControlY, 4017, 4019, 0, GumpButtonType.Reply, 0); // Close Button
        }

        private string GetItemName(Type type)
        {
            string name = type.Name;
            return System.Text.RegularExpressions.Regex.Replace(name, "(?<!^)([A-Z])", " $1");
        }

        // *** MODIFIED: OnResponse ***
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            // ... (Keep initial checks for vendor/buyer null/deleted/range) ...
             if (m_Vendor == null || m_Vendor.Deleted || m_Buyer == null || m_Buyer.Deleted) return;
             if (!m_Buyer.CheckAlive() || !m_Buyer.InRange(m_Vendor.Location, 8)) { m_Buyer.SendLocalizedMessage(500446); return; }

            int buttonId = info.ButtonID;

            switch (buttonId)
            {
                 // ... (Cases 0, 1, 2, 3, 4 for Close/Tabs/Paging remain the same) ...
                case 0: break;
                case 1: m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, 0, 0)); break;
                case 2: m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, 1, 0)); break;
                case 3: if (m_SubPage > 0) m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage - 1)); else m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); break;
                case 4: var listPageCheck = (m_Page == 0) ? m_Vendor.CurrentSellItems : m_Vendor.CurrentBuyItems; int totalPgs = (listPageCheck.Count + ItemsPerPage - 1) / ItemsPerPage; if (m_SubPage < totalPgs - 1) m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage + 1)); else m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); break;


                default: // Item action buttons
                    if (buttonId >= 1000 && buttonId < 2000) // Buy action
                    {
                        int index = buttonId - 1000;
                        int textEntryId = 3000 + index;
                        var sellList = new List<KeyValuePair<Type, DynamicStockInfo>>(m_Vendor.CurrentSellItems); // Snapshot

                        if (index >= 0 && index < sellList.Count)
                        {
                            Type itemType = sellList[index].Key;
                            DynamicStockInfo stockInfo = sellList[index].Value; // Get StockInfo
                            int price = stockInfo.Price; // Price from StockInfo

                            int quantity = 1;
                            TextRelay quantityRelay = info.GetTextEntry(textEntryId);
                            if (quantityRelay != null && int.TryParse(quantityRelay.Text.Trim(), out int parsedQty) && parsedQty >= 1) { quantity = parsedQty; }
                            else if (quantityRelay != null && !string.IsNullOrEmpty(quantityRelay.Text.Trim())) { m_Buyer.SendMessage("Invalid quantity specified. Defaulting to 1."); }

                            // *** Pass quantity to OnBuyItem *** (Logic inside OnBuyItem now checks stock)
                            if (m_Vendor.OnBuyItem(m_Buyer, itemType, price, quantity)) { m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); } // Refresh on success
                            else { m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); } // Refresh on fail (to show errors/updated stock)
                        } else { m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); } // Refresh on index error
                    }
                    else if (buttonId >= 2000) // Sell action
                    {
                        int index = buttonId - 2000;
                        var buyList = new List<KeyValuePair<Type, DynamicStockInfo>>(m_Vendor.CurrentBuyItems); // Snapshot

                        if (index >= 0 && index < buyList.Count)
                        {
                            Type itemType = buyList[index].Key;
                            DynamicStockInfo stockInfo = buyList[index].Value; // Get StockInfo for demand/price

                            // *** Pass StockInfo to StartSellItem ***
                            m_Vendor.StartSellItem(m_Buyer, itemType, stockInfo);
                            // Gump closes due to targeting
                        } else { m_Buyer.SendGump(new DynamicVendorGump(m_Buyer, m_Vendor, m_Page, m_SubPage)); } // Refresh on index error
                    }
                    break;
            }
        }
    }
}