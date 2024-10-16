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
    public static class MenuUploader
    {
        private static readonly List<Type> m_Menus = new List<Type>();

        public static List<Type> Menus
        {
            get { return m_Menus; }
        }

        public static void RegisterMenu<G>(G menu) where G : Gump, IRewardVendorGump
        {
            m_Menus.Add(menu.GetType());
        }

        public static void Display(IRewardVendorGump menu, Mobile m, IRewardVendor vendor)
        {
            m.CloseGump(typeof(IRewardVendorGump));

            //create 'canvas'
            menu.CreateBackground();

            //create Reward displays
            foreach (Reward r in vendor.Rewards) //.Where(r => r.Try_Restock() != 0 || menu is ManageItemsGump))
            {
                r.Try_Restock();
                menu.AddEntry(r);
            }

            //send finished product
            menu.Send(m);
        }

        public static void Display(Type menu, Mobile m, IRewardVendor vendor, bool playerView)
        //Note: Playerview is only relevant for Staff
        {
            try
           {
                if (m.AccessLevel > MobileRewardVendor.FullStaffAccessLevel)
                {
                    if (playerView)
                    {
                        Display(new ManageItemsGump(vendor, m), m, vendor);
                    }
                    else //control panel
                    {
                        m.CloseGump(typeof(WorldVendorsGump));
                        m.CloseGump(typeof(ControlPanelGump));

                        m.SendGump(new WorldVendorsGump(vendor));
                        m.SendGump(new ControlPanelGump(m, vendor));
                    }
                }
                else //player
                {
                    Display((IRewardVendorGump)Activator.CreateInstance(menu, new object[] { vendor, m }), m, vendor);
                    //create fresh instance
                }
            }
            catch
            {
                m.SendMessage(m.AccessLevel > MobileRewardVendor.FullStaffAccessLevel
                    ? "The current display is invalid, try setting gump back to default, JewlRewardGump."
                    : "The current display is invalid, please notify the staff.");
            }
        }
    }
}