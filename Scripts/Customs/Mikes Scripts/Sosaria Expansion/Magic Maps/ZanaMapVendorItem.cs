using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom
{
    public class ZanaMapVendorItem
    {
        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public int ItemID { get; private set; }
        public int PriceInMaxxiaScrolls { get; private set; }

        public ZanaMapVendorItem(Type itemType, string name, int itemID, int priceInMaxxiaScrolls)
        {
            ItemType = itemType;
            Name = name;
            ItemID = itemID;
            PriceInMaxxiaScrolls = priceInMaxxiaScrolls;
        }
    }

    public class ZanaMapVendorGump : Gump
    {
        private PlayerMobile m_Player;

        // List of what Zana sells
        private static readonly List<ZanaMapVendorItem> MapVendorItems = new List<ZanaMapVendorItem>
        {
            new ZanaMapVendorItem(typeof(FeluccaMagicMap),   "Felucca Magic Map",   22326, 1),
            new ZanaMapVendorItem(typeof(TrammelMagicMap),   "Trammel Magic Map",   22326, 1),
            new ZanaMapVendorItem(typeof(IlshenarMagicMap),  "Ilshenar Magic Map",  22326, 1),
            new ZanaMapVendorItem(typeof(TokunoMagicMap),    "Tokuno Magic Map",    22326, 1),
            new ZanaMapVendorItem(typeof(TerMurMagicMap),    "Ter Mur Magic Map",   22326, 1),
        };

        public ZanaMapVendorGump(PlayerMobile player)
            : base(100, 100)
        {
            m_Player = player;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            // Gump background
            AddBackground(0, 0, 400, 350, 5054);
            AddAlphaRegion(10, 10, 380, 330);

            AddLabel(120, 20, 1152, "Zana's Magic Maps");

            int y = 60;
            for (int i = 0; i < MapVendorItems.Count; i++)
            {
                ZanaMapVendorItem item = MapVendorItems[i];

                // Add a button + label for each map
                AddLabel(40, y, 1152, item.Name);
                AddLabel(220, y, 1152, $"Cost: {item.PriceInMaxxiaScrolls} MaxxiaScroll");
                AddButton(350, y, 4005, 4007, (int)(i + 1), GumpButtonType.Reply, 0);
                y += 30;
            }

            // Close button
            AddButton(120, 310, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddLabel(155, 310, 1152, "Close");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            // ButtonID 0 = Close
            if (info.ButtonID == 0)
                return;

            int index = info.ButtonID - 1; // Because we did i+1 above
            if (index < 0 || index >= MapVendorItems.Count)
                return;

            ZanaMapVendorItem vendorItem = MapVendorItems[index];

            if (!HasEnoughMaxxiaScrolls(from, vendorItem.PriceInMaxxiaScrolls))
            {
                from.SendMessage($"You do not have enough MaxxiaScrolls to purchase {vendorItem.Name}.");
                from.SendGump(new ZanaMapVendorGump((PlayerMobile)from));
                return;
            }

            // Consume the scrolls and give the item
            ConsumeMaxxiaScrolls(from, vendorItem.PriceInMaxxiaScrolls);
            GiveItem(from, vendorItem);

            from.SendMessage($"You purchase {vendorItem.Name} for {vendorItem.PriceInMaxxiaScrolls} MaxxiaScroll.");

            // Reopen the gump so they can buy more if they want
            from.SendGump(new ZanaMapVendorGump((PlayerMobile)from));
        }

        private bool HasEnoughMaxxiaScrolls(Mobile from, int requiredAmount)
        {
            if (from.Backpack == null)
                return false;
            int count = from.Backpack.GetAmount(typeof(MaxxiaScroll), true);
            return (count >= requiredAmount);
        }

        private void ConsumeMaxxiaScrolls(Mobile from, int amount)
        {
            if (from.Backpack != null)
            {
                from.Backpack.ConsumeTotal(typeof(MaxxiaScroll), amount);
            }
        }

        private void GiveItem(Mobile from, ZanaMapVendorItem vendorItem)
        {
            Item item = (Item)Activator.CreateInstance(vendorItem.ItemType);
            if (item != null)
            {
                from.AddToBackpack(item);
            }
        }
    }
}
