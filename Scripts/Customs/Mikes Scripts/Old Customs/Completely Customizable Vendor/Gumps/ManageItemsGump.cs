//Completely Customizable Vendor
//Cleaned up by Tresdni
//Original Author:  krazeykow

#region References
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server;
using Server.ContextMenus;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System;
#endregion

namespace System.CustomizableVendor
{        
    public class ManageItemsGump : JewlRewardGump
    {
        private int m_PageNum;
        private int m_ButtonNum;

        public ManageItemsGump(IRewardVendor vendor, Mobile m)
            : base(vendor, m)
        {
            m_ButtonNum = 0;
        }

        protected override void AddEntryControl(Reward r)
        {
            ImageTileButtonInfo b = new ItemTileButtonInfo(r.RewardInfo);

            Rectangle2D rec = ItemBounds.Table[r.RewardInfo.ItemID];
            

            //begin time entries

            if (r.Restock != null)
            {
                if (r.Restock.RestockRate.TotalMinutes == 0)
                {
                    AddHtml( 313, (PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1}<br><basefont color=#FF2A00>LIMITED <br>Purchased: {2}<br><basefont color=#FFFFFF>{3}", 
                        r.Title, r.Cost.ToString("#,0"), r.BuyCount.ToString("#,0"), r.Description ), false, true );
                }
                if (r.Restock.Maximum > 0)
                {
                    AddHtml( 313, (PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1}<br><basefont color=#FF2A00>Stock: {2}/{3} <br>Purchased: {4}<br><basefont color=#FFFFFF>{5}", 
                        r.Title, r.Cost.ToString("#,0"), r.Restock.Count.ToString("#,0"), r.Restock.Maximum.ToString("#,0"), r.BuyCount.ToString("#,0"),  r.Description ), false, true );
                }
            }
			else
			{
				AddHtml( 313, (PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1} <br><basefont color=#FF2A00>Purchased: {2}<br><br><basefont color=#FFFFFF>{2}", r.Title, r.Cost.ToString("#,0"), r.BuyCount.ToString("#,0"), r.Description ), false, true );
			}

            AddImageTiledButton(227, (PosY + 26), Settings.itemButton, Settings.itemButtonPressed, ++m_ButtonNum, GumpButtonType.Reply, PageNum, b.ItemID, b.Hue, 15, 10);
			AddImageTiled(231, (PosY + 29), 72, 53, 5104);
			
            AddItem( 267 - rec.Width / 2 - rec.X , (PosY + 26) + 30 - rec.Height / 2 - rec.Y,  b.ItemID, r.RewardInfo.Hue);
			AddItemProperty(r.RewardInfo.Serial);
			
			//odd numbers (same as above)
            AddButton(470, (PosY + 45), 4011, 4012, m_ButtonNum, GumpButtonType.Reply, PageNum);
            AddLabel(508, (PosY + 47), 1882, @"Options");
            //even numbers
            AddButton(470, (PosY + 19), 4020, 4021, ++m_ButtonNum, GumpButtonType.Reply, PageNum);
            AddLabel(509, (PosY + 21), 1882, @"Delete");

            AddImageTiled(215, (PosY + 95), 359, 2, 96);

            PosY += 82; //102;

            //add new page every six entries, use to be 4
            if (EntryNum % 6 == 0)
            {
                AddButton(316, 643, 2471, 2470, -1, GumpButtonType.Page, PageNum + 1);

                AddPage(++PageNum);

                AddButton(221, 643, 2468, 2467, -1, GumpButtonType.Page, PageNum - 1);

                //reset to top of page
                PosY = 110; //130;
            }
            
            EntryNum++;
        }

        private static void DeleteReward_Callback(Mobile from, bool okay, object state)
        {
            if (from.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }

            if (!okay)
            {
                return;
            }
            //indexes -> (Vendor(0), Reward(1))
            Storage store = (Storage)state;

            IRewardVendor vendor = (IRewardVendor)store[0];
            Reward Reward = (Reward)store[1];

            try
            {
                vendor.RemoveReward(Reward);
            }
            catch
            {
                @from.SendMessage("An error ocurred in the removal of this item.");
            }

            MenuUploader.Display(vendor.Menu, @from, vendor, true);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;
            if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }
            if (info.ButtonID == 0)
            {
                MenuUploader.Display(Vendor.Menu, m, Vendor, false);
                return;
            }

            if (info.ButtonID % 2 == 0) //even
            {
                try
                {
                    Reward r = Vendor.Rewards[(info.ButtonID / 2) - 1];

                    //params -> (vendor, Reward)
                    Storage store = new Storage(Vendor, r);

                    m.SendGump(new WarningGump(1060635, 30720, "Warning: Are you sure you want to remove " + r.Title,
                        0xFFC000, 420, 400, DeleteReward_Callback, store));
                }
                catch
                {
                    m.SendMessage("This item could not be found.");
                }
            }
            else //assert - odd
            {
                try
                {
                    MenuUploader.Display(Vendor.Menu, m, Vendor, true);
                    m.CloseGump(typeof(ViewItemGump));
                    m.SendGump(new ViewItemGump(m, Vendor, Vendor.Rewards[((info.ButtonID + 1) / 2) - 1], true));
                }
                catch
                {
                    m.SendMessage("Vendor could not be found");
                }
            }
        }
    }
}