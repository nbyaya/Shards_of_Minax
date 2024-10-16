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
    public class ControlPanelGump : Gump
    {
        private readonly IRewardVendor m_Vendor;

        public ControlPanelGump(Mobile m, IRewardVendor vendor)
            : base(0, 0)
        {
            m_Vendor = vendor;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            //background
            AddImageTiled(4, 3, 215, 571, 2702);
            AddImageTiled(3, 0, 217, 3, 10001);
            AddImageTiled(4, 572, 216, 3, 10001);
            AddImageTiled(3, 1, 3, 571, 10004);
            AddImageTiled(217, 1, 3, 574, 10004);

            AddImage(40, 8, 5214);
            AddLabel(41, 7, 47, @"Control Panel");

            AddLabel(109, 62, 55, @"Manage Rewards");
            AddButton(72, 60, 4008, 4009, 1, GumpButtonType.Reply, 0); //PlayerView
            AddItem(31, 63, 5366); //scope
            AddImageTiled(19, 160, 132, 2, 96);

            AddLabel(79, 123, 1324, @"Add Item or Container");
            AddButton(52, 126, 2117, 2118, 2, GumpButtonType.Reply, 0); //AddItem
            AddItem(0, 113, 8192); //box  

            AddLabel(11, 232, 55, @"Target Payment Source:");
            AddButton(170, 232, 2117, 2118, 3, GumpButtonType.Reply, 0); //Payment
            AddLabel(12, 173, 55, @"Pay By: " + m_Vendor.Payment.PayName);
            AddItem(61, 197, m_Vendor.Payment.PayID, m_Vendor.Payment.CurrHue);

            AddLabel(12, 32, 55, @"Vendor: " + m_Vendor.GetName());
            AddButton(77, 533, 4026, 4027, 4, GumpButtonType.Reply, 0);
            AddLabel(78, 553, 104, @"Help");

            if (m_Vendor.GetMobile() != null)
            {
                AddButton(72, 91, 4011, 4012, 5, GumpButtonType.Reply, 0); //Displays
                AddLabel(109, 93, 55, @"Get All Displays");
            }
            else
            {
                AddLabel(12, 93, 55, @"Use Mobile Vendor for displays.");
            }


            AddLabel(51, 258, 43, @"XML Attachment");
            AddRadio(15, 258, 9720, 9724, false, 6);
            AddLabel(51, 303, 43, @"Consume Item");
            AddRadio(15, 300, 9720, 9724, false, 7);
            AddLabel(50, 349, 43, @"[props Name of Item/Player");
            AddRadio(15, 342, 9720, 9724, false, 8);
            AddImageTiled(12, 382, 156, 22, 5058);
            AddTextEntry(12, 382, 149, 20, 41, 0, @"[prop/XML Name Here");

            AddPage(1);

            CreateGumps(); //fill out entries
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m_Vendor.IsRemoved())
            {
                m.SendMessage("This vendor has been deleted.");
                return;
            }

            if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }


            switch (info.ButtonID)
            {
                case 0:
                    {
                        m.CloseGump(typeof(WorldVendorsGump));

                        //close
                        break;
                    }
                case 1: //PlayerView
                    {
                        m.CloseGump(typeof(WorldVendorsGump));

                        MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, true);
                        break;
                    }
                case 2: //AddItem
                    {
                        m.CloseGump(typeof(WorldVendorsGump));

                        m.Target = new AddItemTarget(m_Vendor);
                        break;
                    }
                case 3: //Payment
                    {
                        try
                        {
                            if (info.IsSwitched(6))
                            {
                                m.SendMessage("Target the player or item you wish to get the XMLAttribute from.");
                                TextRelay entry0 = info.GetTextEntry(0);
                                string propName = (entry0 == null ? "" : entry0.Text.Trim());

                                m.Target = new PaymentTarget(m_Vendor, propName, true);
                            }
                            else if (info.IsSwitched(7)) // Consume by item amount
                            {
                                m.SendMessage("Warning: payment will be taken as the unmodified created item.");
                                m.Target = new PaymentTarget(m_Vendor);
                            }
                            else if (info.IsSwitched(8)) //Consume by item's property
                            {
                                m.SendMessage("Target yourself or an item.");
                                TextRelay entry0 = info.GetTextEntry(0);
                                string propName = (entry0 == null ? "" : entry0.Text.Trim());

                                m.Target = new PaymentTarget(m_Vendor, propName, false);
                            }
                            else
                            {
                                MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, false);
                                m.SendMessage("Please select consume or property.");
                            }
                        }

                        catch
                        {
                        }

                        break;
                    }
                case 4:
                    {
                        MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, false);
                        m.SendGump(new InfoGump());
                        break;
                    }
                case 5: //Displays
                    {
                        if (m.Backpack == null)
                        {
                            return;
                        }

                        MetalBox c = new MetalBox();
                        c.Name = m_Vendor.GetName() + "'s Reward Displays";

                        foreach (Reward r in m_Vendor.Rewards)
                        {
                            c.DropItem(new Exhibit(m_Vendor, r));
                        }

                        m.AddToBackpack(c);

                        break;
                    }

                default: //Set Menu - IDs are >= 7
                    {
                        try
                        {
                            Type menuType = MenuUploader.Menus[info.ButtonID - 7];

                            if (m_Vendor.GetItem() != null && menuType == typeof(ClassicVendorGump))
                            {
                                m.SendMessage("Only Mobile Reward Vendors can have classic menus.");
                            }
                            else
                            {
                                m_Vendor.Menu = menuType;
                                m.SendMessage("Display changed to " + menuType.Name);
                            }
                        }
                        catch
                        {
                        }

                        MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, false);

                        break;
                    }
            }
        }

        private void CreateGumps()
        {
            int yPos = 433, pageNum = 1;

            for (int i = 0; i < MenuUploader.Menus.Count; ++i)
            {
                AddLabel(50, yPos, 100, MenuUploader.Menus[i].Name);
                AddButton(12, yPos, 4005, 4006, (i + 7), GumpButtonType.Reply, pageNum);


                yPos += 26;

                //add new page every four entries
                if ((i + 1) % 6 == 0)
                {
                    AddButton(144, 742, 9702, 9703, -1, GumpButtonType.Page, (pageNum + 1));

                    AddPage(++pageNum);

                    AddButton(10, 572, 9706, 9707, -1, GumpButtonType.Page, (pageNum - 1));

                    //reset to top of page
                    yPos = 433;
                }
            }
        }
    }
}