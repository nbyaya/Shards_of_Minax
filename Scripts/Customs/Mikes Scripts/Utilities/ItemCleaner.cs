using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Scripts.Commands
{
    public class ItemCleaner
    {
        // Item counting variables
        private static Dictionary<string, int> _itemCounts;
        private static int _totalItems;
        private static List<KeyValuePair<string, int>> _sortedItems;
        private static List<KeyValuePair<string, int>> _filteredItems; // For search results
        
        // Mobile counting variables
        private static Dictionary<string, int> _mobileCounts;
        private static int _totalMobiles;
        private static List<KeyValuePair<string, int>> _sortedMobiles;
        private static List<KeyValuePair<string, int>> _filteredMobiles; // For search results
        
        // Shared variables
        private static string _searchTerm = ""; // Store the current search term
        private static ViewMode _currentView = ViewMode.Items; // Default view
        
        // Colors
        private static readonly int TextColor = 0xFFFF; // Pure white
        private static readonly int TitleColor = 0xFFFF; // White for titles
        private static readonly int HeaderColor = 0xFFFF; // White for headers
        private static readonly int HighlightColor = 0x0F0F; // Light blue for highlights
        private static readonly int ActiveTabColor = 0x0F0F; // Light blue for active tab
        private static readonly int InactiveTabColor = 0xBBBB; // Gray for inactive tab
        
        // Gump dimensions
        private const int GumpWidth = 650;
        private const int GumpHeight = 500;
        
        // Add these variables for spawner tracking
        private static List<XmlSpawner> _relatedSpawners;
        private static string _currentType;
        private static bool _isItem; // true if looking for item spawners, false for mobile spawners
        
        public enum ViewMode
        {
            Items,
            Mobiles
        }
        
        public static void Initialize()
        {
            CommandSystem.Register("ItemCleaner", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("ItemCleaner")]
        [Description("Opens a gump to count items and mobiles in the world and purge specific types.")]
        public static void OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(ItemCleanerGump));
            e.Mobile.CloseGump(typeof(ItemCleanerConfirmGump));
            e.Mobile.CloseGump(typeof(ItemCleanerSearchGump));
            e.Mobile.CloseGump(typeof(SpawnerListGump));
            
            if (e.Arguments.Length >= 1 && e.Arguments[0].ToLower() == "purge" && e.Arguments.Length >= 2)
            {
                string itemType = e.Arguments[1];
                PurgeItems(e.Mobile, itemType);
                return;
            }
            
            // Reset search term when opened fresh
            _searchTerm = "";
            _filteredItems = null;
            _filteredMobiles = null;
            _relatedSpawners = null;
            
            // Count items/mobiles based on current view
            if (_currentView == ViewMode.Items)
                CountItems(e.Mobile, true);
            else
                CountMobiles(e.Mobile, true);
        }

        public static void Run()
        {
            // This method is required for dynamic scripts
            Console.WriteLine("ItemCleaner dynamic script loaded.");
        }

        private static void CountItems(Mobile from, bool sendGump)
        {
            from.SendMessage("Counting items in the world...");

            _itemCounts = new Dictionary<string, int>();
            _totalItems = 0;

            foreach (Item item in World.Items.Values)
            {
                if (item.Deleted)
                    continue;

                string typeName = item.GetType().FullName;
                
                if (_itemCounts.ContainsKey(typeName))
                    _itemCounts[typeName]++;
                else
                    _itemCounts[typeName] = 1;

                _totalItems++;
            }

            // Sort by count (descending)
            _sortedItems = _itemCounts.OrderByDescending(pair => pair.Value).ToList();
            
            // If we have a search term, filter the items
            if (!String.IsNullOrEmpty(_searchTerm))
                ApplyItemSearch(_searchTerm);

            // Output to file
            string fileName = Path.Combine(Core.BaseDirectory, "item_counts.txt");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Item Type Counts - Generated on {0}", DateTime.Now);
                writer.WriteLine("Total Items: {0}", _totalItems);
                writer.WriteLine();
                writer.WriteLine("Count\tPercentage\tType");
                writer.WriteLine("----------------------------------------");

                foreach (var pair in _sortedItems)
                {
                    double percentage = (double)pair.Value / _totalItems * 100;
                    writer.WriteLine("{0}\t{1:0.00}%\t{2}", pair.Value, percentage, pair.Key);
                }
            }

            from.SendMessage("Item count completed. Results saved to 'item_counts.txt'");
            
            if (sendGump)
                from.SendGump(new ItemCleanerGump());
        }
        
        private static void CountMobiles(Mobile from, bool sendGump)
        {
            from.SendMessage("Counting mobiles in the world...");

            _mobileCounts = new Dictionary<string, int>();
            _totalMobiles = 0;

            foreach (Mobile mobile in World.Mobiles.Values)
            {
                if (mobile.Deleted || mobile.Player)
                    continue; // Skip deleted or player mobiles

                string typeName = mobile.GetType().FullName;
                
                if (_mobileCounts.ContainsKey(typeName))
                    _mobileCounts[typeName]++;
                else
                    _mobileCounts[typeName] = 1;

                _totalMobiles++;
            }

            // Sort by count (descending)
            _sortedMobiles = _mobileCounts.OrderByDescending(pair => pair.Value).ToList();
            
            // If we have a search term, filter the mobiles
            if (!String.IsNullOrEmpty(_searchTerm))
                ApplyMobileSearch(_searchTerm);

            // Output to file
            string fileName = Path.Combine(Core.BaseDirectory, "mobile_counts.txt");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Mobile Type Counts - Generated on {0}", DateTime.Now);
                writer.WriteLine("Total Mobiles: {0}", _totalMobiles);
                writer.WriteLine();
                writer.WriteLine("Count\tPercentage\tType");
                writer.WriteLine("----------------------------------------");

                foreach (var pair in _sortedMobiles)
                {
                    double percentage = (double)pair.Value / _totalMobiles * 100;
                    writer.WriteLine("{0}\t{1:0.00}%\t{2}", pair.Value, percentage, pair.Key);
                }
            }

            from.SendMessage("Mobile count completed. Results saved to 'mobile_counts.txt'");
            
            if (sendGump)
                from.SendGump(new ItemCleanerGump());
        }
        
        private static void ApplyItemSearch(string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                _filteredItems = null;
                return;
            }
            
            searchTerm = searchTerm.ToLower();
            _filteredItems = _sortedItems
                .Where(pair => pair.Key.ToLower().Contains(searchTerm))
                .ToList();
        }
        
        private static void ApplyMobileSearch(string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                _filteredMobiles = null;
                return;
            }
            
            searchTerm = searchTerm.ToLower();
            _filteredMobiles = _sortedMobiles
                .Where(pair => pair.Key.ToLower().Contains(searchTerm))
                .ToList();
        }

        private static void PurgeItems(Mobile from, string itemType)
        {
            from.SendMessage("Purging items of type: {0}", itemType);

            int count = 0;
            List<Item> toDelete = new List<Item>();

            foreach (Item item in World.Items.Values)
            {
                if (item.Deleted)
                    continue;

                string typeName = item.GetType().Name;
                
                if (typeName.Equals(itemType, StringComparison.OrdinalIgnoreCase))
                {
                    toDelete.Add(item);
                    count++;
                }
            }

            from.SendMessage("Found {0} items to delete. Deleting...", count);
            
            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            from.SendMessage("Deleted {0} items of type {1}", count, itemType);
            
            // Write the purge log
            string fileName = Path.Combine(Core.BaseDirectory, "purge_log.txt");
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine("[{0}] {1} deleted {2} items of type {3}", 
                    DateTime.Now, 
                    from.Name, 
                    count, 
                    itemType);
            }
            
            // If we had previous counts, refresh them to reflect the deletions
            if (_itemCounts != null)
                CountItems(from, true);
        }
        
        private static void PurgeMobiles(Mobile from, string mobileType)
        {
            from.SendMessage("Purging mobiles of type: {0}", mobileType);

            int count = 0;
            List<Mobile> toDelete = new List<Mobile>();

            foreach (Mobile mobile in World.Mobiles.Values)
            {
                if (mobile.Deleted || mobile.Player)
                    continue; // Skip deleted or player mobiles

                string typeName = mobile.GetType().Name;
                
                if (typeName.Equals(mobileType, StringComparison.OrdinalIgnoreCase))
                {
                    toDelete.Add(mobile);
                    count++;
                }
            }

            from.SendMessage("Found {0} mobiles to delete. Deleting...", count);
            
            // Process in batches to avoid server stress
            int batchSize = 1000;
            int processed = 0;

            for (int i = 0; i < toDelete.Count; i++)
            {
                try
                {
                    Mobile mobile = toDelete[i];
                    if (mobile != null && !mobile.Deleted)
                    {
                        // Remove all items from the mobile first to prevent BaseArmor.OnRemoved issues
                        List<Item> items = new List<Item>(mobile.Items);
                        foreach (Item item in items)
                        {
                            if (item != null && !item.Deleted)
                            {
                                item.Internalize();
                                item.Delete();
                            }
                        }

                        // Now delete the mobile
                        mobile.Delete();
                    }
                }
                catch (Exception ex)
                {
                    // Log the error but continue processing
                    Console.WriteLine("Error deleting mobile: " + ex.Message);
                }

                processed++;
                
                // Give the server a break after each batch
                if (processed % batchSize == 0 && i < toDelete.Count - 1)
                {
                    from.SendMessage("Deleted {0} of {1} mobiles... (processing in batches)", processed, toDelete.Count);
                    // For extremely large deletes, force garbage collection to free memory
                    GC.Collect();
                }
            }

            from.SendMessage("Deleted {0} mobiles of type {1}", count, mobileType);
            
            // Write the purge log
            string fileName = Path.Combine(Core.BaseDirectory, "purge_log.txt");
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine("[{0}] {1} deleted {2} mobiles of type {3}", 
                    DateTime.Now, 
                    from.Name, 
                    count, 
                    mobileType);
            }
            
            // If we had previous counts, refresh them to reflect the deletions
            if (_mobileCounts != null)
                CountMobiles(from, true);
        }
        
        private class ItemCleanerGump : Gump
        {
            private const int ItemsPerPage = 20; // Increased from 15 to show more items per page
            private int _page;
            
            public ItemCleanerGump(int page = 0) : base(50, 50)
            {
                _page = page;
                
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;
                
                AddPage(0);
                
                // Background
                AddBackground(0, 0, GumpWidth, GumpHeight, 9200);
                
                // Title
                AddHtml(20, 15, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Item Cleaner</BASEFONT></CENTER>", TitleColor), false, false);
                
                // Count display 
                if (_currentView == ViewMode.Items)
                {
                    AddHtml(20, 40, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Total Items: {1}</BASEFONT></CENTER>", TextColor, _totalItems), false, false);
                }
                else
                {
                    AddHtml(20, 40, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Total Mobiles: {1}</BASEFONT></CENTER>", TextColor, _totalMobiles), false, false);
                }
                
                // Search button
                AddButton(20, 65, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtml(55, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Search</BASEFONT>", TextColor), false, false);
                
                // Refresh button
                AddButton(170, 65, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtml(205, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Refresh</BASEFONT>", TextColor), false, false);
                
                // View mode tabs
                AddButton(320, 65, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddHtml(355, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Items</BASEFONT>", (_currentView == ViewMode.Items ? ActiveTabColor : InactiveTabColor)), false, false);
                
                AddButton(450, 65, 4005, 4007, 4, GumpButtonType.Reply, 0);
                AddHtml(485, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Mobiles</BASEFONT>", (_currentView == ViewMode.Mobiles ? ActiveTabColor : InactiveTabColor)), false, false);
                
                // Show search filter if active
                if (!String.IsNullOrEmpty(_searchTerm))
                {
                    AddHtml(20, 90, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Search: {1}</BASEFONT></CENTER>", HighlightColor, _searchTerm), false, false);
                }
                
                // Headers
                AddHtml(20, 115, 70, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Count</BASEFONT>", HeaderColor), false, false);
                AddHtml(90, 115, 70, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Percentage</BASEFONT>", HeaderColor), false, false);
                AddHtml(160, 115, 370, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Type</BASEFONT>", HeaderColor), false, false);
                AddHtml(530, 115, 100, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Actions</BASEFONT>", HeaderColor), false, false);
                
                // Draw horizontal separator
                for (int i = 20; i < GumpWidth - 20; i += 10)
                {
                    AddImage(i, 135, 9277);
                }
                
                // List items
                var list = GetCurrentList();
                int totalCount = _currentView == ViewMode.Items ? _totalItems : _totalMobiles;
                
                int startIndex = _page * ItemsPerPage;
                int endIndex = Math.Min(startIndex + ItemsPerPage, list.Count);
                
                for (int i = startIndex, index = 0; i < endIndex; i++, index++)
                {
                    KeyValuePair<string, int> pair = list[i];
                    string typeName = pair.Key;
                    int count = pair.Value;
                    double percentage = (double)count / totalCount * 100;
                    
                    // Item row
                    int yOffset = 140 + (index * 25);
                    
                    // Count
                    AddHtml(20, yOffset, 70, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", TextColor, count), false, false);
                    
                    // Percentage
                    AddHtml(90, yOffset, 70, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1:0.00}%</BASEFONT>", TextColor, percentage), false, false);
                    
                    // Type (shortened to fit)
                    string shortTypeName = typeName;
                    if (shortTypeName.IndexOf('.') >= 0)
                    {
                        shortTypeName = shortTypeName.Substring(shortTypeName.LastIndexOf('.') + 1);
                    }
                    
                    AddHtml(160, yOffset, 370, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", TextColor, shortTypeName), false, false);
                    
                    // Purge button
                    AddButton(530, yOffset, 4017, 4019, 100 + i, GumpButtonType.Reply, 0);
                    
                    // Show spawners button - add new button
                    AddButton(560, yOffset, 4011, 4013, 1000 + i, GumpButtonType.Reply, 0);
                }
                
                // Pagination
                int totalPages = (int)Math.Ceiling((double)list.Count / ItemsPerPage);
                
                AddHtml(20, GumpHeight - 30, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Page {1} of {2}</BASEFONT></CENTER>", TextColor, _page + 1, totalPages), false, false);
                
                if (_page > 0)
                {
                    // Previous page button
                    AddButton(20, GumpHeight - 30, 4014, 4016, 5, GumpButtonType.Reply, 0);
                }
                
                if (_page < totalPages - 1)
                {
                    // Next page button
                    AddButton(GumpWidth - 40, GumpHeight - 30, 4005, 4007, 6, GumpButtonType.Reply, 0);
                }
            }
            
            // Add helper method to get the current list based on current view and search filter
            private List<KeyValuePair<string, int>> GetCurrentList()
            {
                if (_currentView == ViewMode.Items)
                {
                    return _filteredItems ?? _sortedItems;
                }
                else
                {
                    return _filteredMobiles ?? _sortedMobiles;
                }
            }
            
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                
                if (from == null)
                    return;
                
                switch (info.ButtonID)
                {
                    case 0: // Close
                        break;
                    case 1: // Search
                        from.SendGump(new ItemCleanerSearchGump(_page));
                        break;
                    case 2: // Refresh
                        if (_currentView == ViewMode.Items)
                            CountItems(from, true);
                        else
                            CountMobiles(from, true);
                        break;
                    case 3: // Switch to Items View
                        if (_currentView != ViewMode.Items)
                        {
                            _currentView = ViewMode.Items;
                            CountItems(from, true);
                        }
                        else
                        {
                            from.SendGump(new ItemCleanerGump(_page));
                        }
                        break;
                    case 4: // Switch to Mobiles View
                        if (_currentView != ViewMode.Mobiles)
                        {
                            _currentView = ViewMode.Mobiles;
                            CountMobiles(from, true);
                        }
                        else
                        {
                            from.SendGump(new ItemCleanerGump(_page));
                        }
                        break;
                    case 5: // Previous page
                        if (_page > 0)
                        {
                            from.SendGump(new ItemCleanerGump(_page - 1));
                        }
                        break;
                    case 6: // Next page
                        from.SendGump(new ItemCleanerGump(_page + 1));
                        break;
                    default:
                        // Handle purge buttons (100+)
                        if (info.ButtonID >= 100 && info.ButtonID < 1000)
                        {
                            int index = info.ButtonID - 100;
                            var list = GetCurrentList();
                            
                            if (index >= 0 && index < list.Count)
                            {
                                KeyValuePair<string, int> pair = list[index];
                                string typeName = pair.Key;
                                int count = pair.Value;
                                string shortTypeName = typeName;
                                
                                if (shortTypeName.IndexOf('.') >= 0)
                                {
                                    shortTypeName = shortTypeName.Substring(shortTypeName.LastIndexOf('.') + 1);
                                }
                                
                                from.SendGump(new ItemCleanerConfirmGump(shortTypeName, count, typeName, _page, _currentView == ViewMode.Items));
                            }
                        }
                        // Handle spawner list buttons (1000+) - Add new handler
                        else if (info.ButtonID >= 1000 && info.ButtonID < 2000)
                        {
                            int index = info.ButtonID - 1000;
                            var list = GetCurrentList();
                            
                            if (index >= 0 && index < list.Count)
                            {
                                KeyValuePair<string, int> pair = list[index];
                                string typeName = pair.Key;
                                string shortTypeName = typeName;
                                
                                if (shortTypeName.IndexOf('.') >= 0)
                                {
                                    shortTypeName = shortTypeName.Substring(shortTypeName.LastIndexOf('.') + 1);
                                }
                                
                                // Find spawners and open the spawner list gump
                                FindRelatedSpawners(shortTypeName, _currentView == ViewMode.Items);
                                from.SendGump(new SpawnerListGump(_page));
                            }
                        }
                        break;
                }
            }
        }
        
        private class ItemCleanerSearchGump : Gump
        {
            private int _returnPage;
            
            public ItemCleanerSearchGump(int returnPage) : base(150, 150)
            {
                _returnPage = returnPage;
                
                AddPage(0);
                AddBackground(0, 0, 400, 200, 9270);
                AddAlphaRegion(10, 10, 380, 180);
                
                string title = _currentView == ViewMode.Items ? "Search Items" : "Search Mobiles";
                AddHtml(20, 20, 360, 35, String.Format("<CENTER><BIG><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></BIG></CENTER>", title), false, false);
                
                string prompt = String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>Enter part of the {0} type name to search for:</BASEFONT></CENTER>", 
                    _currentView == ViewMode.Items ? "item" : "mobile");
                AddHtml(20, 60, 360, 25, prompt, false, false);
                
                // Text entry field
                AddBackground(50, 90, 300, 30, 9350);
                AddTextEntry(55, 95, 290, 20, 0, 1, _searchTerm);
                
                AddButton(120, 140, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtml(155, 140, 70, 25, "<BASEFONT COLOR=#FFFFFF>Search</BASEFONT>", false, false);
                
                AddButton(220, 140, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddHtml(255, 140, 70, 25, "<BASEFONT COLOR=#FFFFFF>Cancel</BASEFONT>", false, false);
            }
            
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                
                if (info.ButtonID == 1) // Search
                {
                    TextRelay entry = info.GetTextEntry(1);
                    _searchTerm = (entry == null) ? "" : entry.Text.Trim();
                    
                    if (!String.IsNullOrEmpty(_searchTerm))
                    {
                        if (_currentView == ViewMode.Items)
                            ApplyItemSearch(_searchTerm);
                        else
                            ApplyMobileSearch(_searchTerm);
                        
                        from.SendGump(new ItemCleanerGump(0)); // Reset to first page with search results
                    }
                    else
                    {
                        // No search term entered, return to main gump
                        _filteredItems = null;
                        _filteredMobiles = null;
                        from.SendGump(new ItemCleanerGump(_returnPage));
                    }
                }
                else
                {
                    // Return to the main gump without changing search
                    from.SendGump(new ItemCleanerGump(_returnPage));
                }
            }
        }
        
        private class ItemCleanerConfirmGump : Gump
        {
            private string _typeName;
            private int _count;
            private string _fullType;
            private int _returnPage;
            private bool _isItem; // True for items, false for mobiles
            
            public ItemCleanerConfirmGump(string typeName, int count, string fullType, int returnPage = 0, bool isItem = true) : base(150, 150)
            {
                _typeName = typeName;
                _count = count;
                _fullType = fullType;
                _returnPage = returnPage;
                _isItem = isItem;
                
                AddPage(0);
                AddBackground(0, 0, 400, 230, 9270);
                AddAlphaRegion(10, 10, 380, 210);
                
                string title = String.Format("Confirm {0} Purge", _isItem ? "Item" : "Mobile");
                AddHtml(20, 20, 360, 35, String.Format("<CENTER><BIG><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></BIG></CENTER>", title), false, false);
                
                string prompt = String.Format(
                    "<CENTER><BASEFONT COLOR=#FFFFFF>Are you sure you want to delete ALL {0}s of type:<BR><BASEFONT COLOR=#FF0000>{1}</BASEFONT><BR>Count: {2}<BR><BASEFONT COLOR=#CCCCCC>Full type: {3}</BASEFONT></CENTER>", 
                    _isItem ? "item" : "mobile", _typeName, _count, _fullType);
                
                AddHtml(20, 60, 360, 100, prompt, false, false);
                
                AddButton(120, 170, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtml(155, 170, 95, 25, "<BASEFONT COLOR=#FFFFFF>Yes, Delete All</BASEFONT>", false, false);
                
                AddButton(260, 170, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddHtml(295, 170, 70, 25, "<BASEFONT COLOR=#FFFFFF>Cancel</BASEFONT>", false, false);
            }
            
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                
                if (info.ButtonID == 1) // Confirmed
                {
                    if (_isItem)
                        PurgeItems(from, _typeName);
                    else
                        PurgeMobiles(from, _typeName);
                }
                else
                {
                    // Return to the main gump
                    from.SendGump(new ItemCleanerGump(_returnPage));
                }
            }
        }
        
        // Add method to find spawners for a specific type
        private static void FindRelatedSpawners(string typeName, bool isItem)
        {
            _currentType = typeName;
            _isItem = isItem;
            _relatedSpawners = new List<XmlSpawner>();
            List<Server.Engines.MiniChamps.MiniChamp> miniChamps = new List<Server.Engines.MiniChamps.MiniChamp>();
            
            foreach (Item item in World.Items.Values)
            {
                // Handle XmlSpawners - use traditional casting instead of pattern matching
                XmlSpawner spawner = item as XmlSpawner;
                if (spawner != null)
                {
                    // Check spawner objects for the type
                    foreach (XmlSpawner.SpawnObject so in spawner.SpawnObjects)
                    {
                        if (so.TypeName != null && so.TypeName.IndexOf(typeName, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _relatedSpawners.Add(spawner);
                            break;
                        }
                    }
                }
                
                // Handle MiniChamps separately - use traditional casting
                Server.Engines.MiniChamps.MiniChamp miniChamp = item as Server.Engines.MiniChamps.MiniChamp;
                if (miniChamp != null)
                {
                    miniChamps.Add(miniChamp);
                }
            }
            
            // Now process any MiniChamps found
            foreach (var miniChamp in miniChamps)
            {
                // We can't directly check the MiniChampType.cs file's contents, but we can check the type name
                if (miniChamp.Type.ToString().IndexOf(typeName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Add it as an XmlSpawner to _relatedSpawners (we treat it as a spawner)
                    // Create a temporary XmlSpawner to represent the MiniChamp in our list
                    XmlSpawner tempSpawner = new XmlSpawner(1, 0, 0, 0, 5, String.Format("MiniChamp: {0}", miniChamp.Type));
                    tempSpawner.Location = miniChamp.Location;
                    tempSpawner.Map = miniChamp.Map;
                    tempSpawner.Name = String.Format("MiniChamp ({0})", miniChamp.Type);
                    
                    _relatedSpawners.Add(tempSpawner);
                }
            }
        }
        
        // Add a new Gump to display spawners
        private class SpawnerListGump : Gump
        {
            private const int ItemsPerPage = 10; // Fewer items per page since spawners have more info
            private int _page;
            
            public SpawnerListGump(int returnPage = 0, int page = 0) : base(50, 50)
            {
                _page = page;
                
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;
                
                AddPage(0);
                
                // Background
                AddBackground(0, 0, GumpWidth, GumpHeight, 9200);
                
                // Title
                AddHtml(20, 15, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Spawners for {1}</BASEFONT></CENTER>", TitleColor, _currentType), false, false);
                
                // Display total spawners found
                AddHtml(20, 40, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Found {1} spawners</BASEFONT></CENTER>", TextColor, _relatedSpawners.Count), false, false);
                
                // Back button
                AddButton(20, 65, 4014, 4016, 1, GumpButtonType.Reply, 0);
                AddHtml(55, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Back</BASEFONT>", TextColor), false, false);
                
                // Go To all button
                AddButton(170, 65, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtml(205, 65, 140, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Go To All</BASEFONT>", TextColor), false, false);
                
                // Headers
                AddHtml(20, 95, 60, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Go To</BASEFONT>", HeaderColor), false, false);
                AddHtml(80, 95, 150, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Name</BASEFONT>", HeaderColor), false, false);
                AddHtml(230, 95, 100, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Location</BASEFONT>", HeaderColor), false, false);
                AddHtml(330, 95, 100, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Map</BASEFONT>", HeaderColor), false, false);
                AddHtml(430, 95, 200, 20, String.Format("<BASEFONT COLOR=#{0:X6}>Details</BASEFONT>", HeaderColor), false, false);
                
                // Draw horizontal separator
                for (int i = 20; i < GumpWidth - 20; i += 10)
                {
                    AddImage(i, 115, 9277);
                }
                
                // List spawners
                if (_relatedSpawners != null && _relatedSpawners.Count > 0)
                {
                    int startIndex = _page * ItemsPerPage;
                    int endIndex = Math.Min(startIndex + ItemsPerPage, _relatedSpawners.Count);
                    
                    for (int i = startIndex, index = 0; i < endIndex; i++, index++)
                    {
                        XmlSpawner spawner = _relatedSpawners[i];
                        
                        // Spawner row
                        int yOffset = 120 + (index * 35); // More space for each row
                        
                        // Go To button
                        AddButton(20, yOffset, 4005, 4007, 100 + i, GumpButtonType.Reply, 0);
                        
                        // Name
                        string name = String.IsNullOrEmpty(spawner.Name) ? "(unnamed)" : spawner.Name;
                        AddHtml(80, yOffset, 150, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", TextColor, name), false, false);
                        
                        // Location
                        AddHtml(230, yOffset, 100, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}, {2}, {3}</BASEFONT>", TextColor, spawner.Location.X, spawner.Location.Y, spawner.Location.Z), false, false);
                        
                        // Map
                        AddHtml(330, yOffset, 100, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", TextColor, spawner.Map), false, false);
                        
                        // Try to find spawn information
                        string spawnDetails = GetSpawnDetails(spawner);
                        AddHtml(430, yOffset, 200, 30, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", TextColor, spawnDetails), false, false);
                    }
                }
                else
                {
                    AddHtml(20, 120, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>No spawners found for this type.</BASEFONT></CENTER>", TextColor), false, false);
                }
                
                // Pagination
                int spawnersCount = _relatedSpawners != null ? _relatedSpawners.Count : 0;
                int totalPages = (int)Math.Ceiling((double)spawnersCount / ItemsPerPage);
                
                AddHtml(20, GumpHeight - 30, GumpWidth - 40, 20, String.Format("<CENTER><BASEFONT COLOR=#{0:X6}>Page {1} of {2}</BASEFONT></CENTER>", TextColor, _page + 1, Math.Max(1, totalPages)), false, false);
                
                if (_page > 0)
                {
                    // Previous page button
                    AddButton(20, GumpHeight - 30, 4014, 4016, 3, GumpButtonType.Reply, 0);
                }
                
                if (_page < totalPages - 1)
                {
                    // Next page button
                    AddButton(GumpWidth - 40, GumpHeight - 30, 4005, 4007, 4, GumpButtonType.Reply, 0);
                }
            }
            
            private string GetSpawnDetails(XmlSpawner spawner)
            {
                StringBuilder sb = new StringBuilder();
                
                // Check if this is our generated XmlSpawner for a MiniChamp
                if (spawner.Name != null && spawner.Name.StartsWith("MiniChamp ("))
                {
                    // Just show the MiniChamp type which is already in the Name
                    return spawner.Name;
                }
                
                // Get spawn counts
                int totalSpawnObjects = 0;
                int relevantSpawnObjects = 0;
                
                foreach (XmlSpawner.SpawnObject so in spawner.SpawnObjects)
                {
                    totalSpawnObjects++;
                    
                    if (so.TypeName != null && so.TypeName.IndexOf(_currentType, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        relevantSpawnObjects++;
                        sb.Append(String.Format("{0}: {1}", so.TypeName, so.MaxCount));
                    }
                }
                
                if (sb.Length == 0)
                {
                    sb.Append(String.Format("{0} of {1} spawn entries", relevantSpawnObjects, totalSpawnObjects));
                }
                
                return sb.ToString();
            }
            
            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                
                if (from == null)
                    return;
                
                switch (info.ButtonID)
                {
                    case 0: // Close
                        break;
                    case 1: // Back
                        from.SendGump(new ItemCleanerGump(_page));
                        break;
                    case 2: // Go To All
                        // Create a timer to teleport to each spawner with a small delay
                        if (_relatedSpawners != null && _relatedSpawners.Count > 0)
                        {
                            from.SendMessage(String.Format("Starting tour of all {0} spawners. Use [cancel to stop.", _relatedSpawners.Count));
                            new SpawnerTourTimer(from, _relatedSpawners, 0, _page).Start();
                        }
                        break;
                    case 3: // Previous page
                        if (_page > 0)
                        {
                            from.SendGump(new SpawnerListGump(_page, _page - 1));
                        }
                        break;
                    case 4: // Next page
                        from.SendGump(new SpawnerListGump(_page, _page + 1));
                        break;
                    default:
                        // Handle teleport buttons (100+)
                        if (info.ButtonID >= 100)
                        {
                            int index = info.ButtonID - 100;
                            
                            if (_relatedSpawners != null && index >= 0 && index < _relatedSpawners.Count)
                            {
                                XmlSpawner spawner = _relatedSpawners[index];
                                
                                // Teleport the player to the spawner
                                from.Map = spawner.Map;
                                from.Location = spawner.Location;
                                
                                // Reopen the gump
                                from.SendGump(new SpawnerListGump(_page, _page));
                            }
                        }
                        break;
                }
            }
        }
        
        // Timer to visit each spawner
        private class SpawnerTourTimer : Timer
        {
            private Mobile _from;
            private List<XmlSpawner> _spawners;
            private int _index;
            private int _returnPage;
            
            public SpawnerTourTimer(Mobile from, List<XmlSpawner> spawners, int index, int returnPage) 
                : base(TimeSpan.FromSeconds(2.0))
            {
                _from = from;
                _spawners = spawners;
                _index = index;
                _returnPage = returnPage;
                Priority = TimerPriority.OneSecond;
            }
            
            protected override void OnTick()
            {
                if (_from.NetState == null || _index >= _spawners.Count)
                {
                    // Tour complete or player logged off
                    if (_from.NetState != null)
                    {
                        _from.SendMessage("Tour complete.");
                        _from.SendGump(new SpawnerListGump(_returnPage));
                    }
                    return;
                }
                
                // Teleport to the next spawner
                XmlSpawner spawner = _spawners[_index];
                
                // Make sure the map is valid
                if (spawner.Map == null || spawner.Map == Map.Internal)
                {
                    _from.SendMessage(String.Format("Skipping spawner {0} because it's on an invalid map.", _index + 1));
                    _index++;
                    // Schedule the next teleport
                    if (_index < _spawners.Count)
                    {
                        new SpawnerTourTimer(_from, _spawners, _index, _returnPage).Start();
                    }
                    else
                    {
                        // Tour complete
                        _from.SendMessage("Tour complete.");
                        _from.SendGump(new SpawnerListGump(_returnPage));
                    }
                    return;
                }
                
                _from.Map = spawner.Map;
                _from.Location = spawner.Location;
                
                // Determine if this is a MiniChamp placeholder
                string spawnerType = spawner.Name != null && spawner.Name.StartsWith("MiniChamp (") ? "MiniChamp" : "XmlSpawner";
                string name = String.IsNullOrEmpty(spawner.Name) ? "(unnamed)" : spawner.Name;
                
                // Display info about this spawner
                _from.SendMessage(String.Format("{0} {1} of {2}: {3} at {4} ({5})", 
                    spawnerType, 
                    _index + 1, 
                    _spawners.Count, 
                    name,
                    spawner.Location,
                    spawner.Map));
                
                // Schedule the next teleport
                _index++;
                if (_index < _spawners.Count)
                {
                    new SpawnerTourTimer(_from, _spawners, _index, _returnPage).Start();
                }
                else
                {
                    // Tour complete
                    _from.SendMessage("Tour complete.");
                    _from.SendGump(new SpawnerListGump(_returnPage));
                }
            }
        }
    }
} 