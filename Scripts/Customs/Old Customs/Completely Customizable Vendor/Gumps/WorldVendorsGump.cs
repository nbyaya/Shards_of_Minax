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
    public class WorldVendorsGump : Gump
    {
        private readonly IRewardVendor m_Vendor; //used in case of copy

        public WorldVendorsGump(IRewardVendor vendor)
            : base(0, 0)
        {
            m_Vendor = vendor;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            //background
            AddImageTiled(50, 3, 430, 756, 2702);
            // AddImageTiled(93, 78, 387, 9, 10101);
            // AddImage(33, 58, 10420);
            // AddImageTiled(47, 2, 3, 556, 10004);
            // AddImageTiled(0, 58, 82, 414, 10440);
            // AddImageTiled(47, 558, 436, 3, 10001);
            // AddImageTiled(47, 0, 435, 3, 10001);
            // AddImageTiled(480, 2, 3, 556, 10004);

            AddPage(1);

            CreateVendors(); //fill out entries
        }

        private static void CopyVendor_Callback(Mobile from, bool okay, object state)
        {
            if (from.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }

            if (!okay)
            {
                return;
            }
            //indexes -> (vendor (0), vendor's copy target(1))
            Storage store = (Storage)state;

            IRewardVendor source = (IRewardVendor)store[0];
            IRewardVendor target = (IRewardVendor)store[1];

            try
            {
                source.CopyVendor(target);
            }
            catch
            {
                @from.SendMessage("Error occured while copying, please insure backpack is on vendors.");
                return;
            }

            @from.SendMessage("{0} has copied {1}'s Reward collection", source.GetName(), target.GetName());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (info.ButtonID == 0)
            {
                m.CloseGump(typeof(ControlPanelGump));
                return;
            }

            if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }

            MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, false);

            try
            {
                if (info.ButtonID % 2 == 0) //even ID: Copy Vendor
                {
                    IRewardVendor selected = WorldRewardVendors.Vendors[(info.ButtonID / 2) - 1];

                    if (m_Vendor.GetName().Equals(selected.GetName()))
                    {
                        m.SendMessage("A vendor cannot copy itself.");
                    }
                    else
                    {
                        //params -> (vendor, vendor's copy target)
                        Storage store = new Storage(m_Vendor, selected);

                        m.SendGump(new WarningGump(1060635, 30720,
                            "Warning: You will lose all saved items on " + m_Vendor.GetName(), 0xFFC000, 420, 400,
                            CopyVendor_Callback, store));
                    }
                }
                else //assert odd ID: Goto 
                {
                    IRewardVendor v = WorldRewardVendors.Vendors[((info.ButtonID + 1) / 2) - 1];

                    m.Location = v.GetLocation();
                    m.Map = v.GetMap();
                }
            }
            catch
            {
                m.SendMessage("Vendor cannot be found.");
            }
        }

        private void CreateVendors()
        {
            int yPos = 101, pageNum = 1, buttonNum = 0;

            for (int i = 0; i < WorldRewardVendors.Vendors.Count; ++i)
            {
                AddButton(175, yPos + 38, 4005, 4006, ++buttonNum, GumpButtonType.Reply, pageNum);
                //goto button - odd ID
                AddButton(279, yPos + 38, 4011, 4012, ++buttonNum, GumpButtonType.Reply, pageNum);
                //copy vendor - even ID
                AddLabel(86, yPos, 1849, WorldRewardVendors.Vendors[i].GetName());
                AddImageTiled(86, yPos + 93, 359, 2, 96);

                if (WorldRewardVendors.Vendors[i].GetMobile() != null)
                {
                    AddItem(110, yPos + 27, 8461); // person
                }
                else
                {
                    AddItem(110, yPos + 27, WorldRewardVendors.Vendors[i].GetItem().ItemID,
                        WorldRewardVendors.Vendors[i].GetItem().Hue); //stone ID
                }

                AddLabel(213, yPos + 43, 2209, @"Go To");
                AddLabel(318, yPos + 43, 2204, @"Copy Vendor");

                yPos += 99;

                //add new page every four entries
                if ((i + 1) % 6 != 0)
                {
                    continue;
                }
                AddButton(417, 731, 9903, 248, -1, GumpButtonType.Page, (pageNum + 1));

                AddPage(++pageNum);

                AddButton(92, 572, 9909, 248, -1, GumpButtonType.Page, (pageNum - 1));

                //reset to top of page
                yPos = 101;
            }
        }
    }
}