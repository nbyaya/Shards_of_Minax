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
    public class ViewItemGump : Gump
    {
        private readonly IRewardVendor m_Vendor;
        private readonly Reward m_Reward;

        public ViewItemGump(Mobile m, IRewardVendor vendor, Reward r, bool viewItem)
            : base(0, 0)
        {
            m_Vendor = vendor;
            m_Reward = r;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddImageTiled(149, 207, 548, 277, 2624);
            AddImage(184, 300, Settings.itemButton);
            AddImage(101, 222, 10400);

            AddHtml(428, 290, 243, 140, r.Description, true, true);
            AddItem(201, 311, r.RewardInfo.ItemID, r.RewardInfo.Hue);

            AddLabel(430, 263, 2101, @"Description");
            AddButton(192, 442, 4029, 4030, 2, GumpButtonType.Reply, 0); //buy
            AddLabel(231, 444, 2125, "Buy (" + r.Cost.ToString("#,0") + ")");
            AddButton(366, 442, 4020, 4021, 0, GumpButtonType.Reply, 0); //cancel
            AddLabel(407, 444, 2116, @"Cancel");
            AddImageTiled(184, 239, 510, 8, 9201);

            AddItem(182, 376, m_Vendor.Payment.PayID, m_Vendor.Payment.CurrHue);
            AddLabel(248, 369, 83, @"Pay By: " + m_Vendor.Payment.PayName);
            //AddLabel(184, 268, 43, @"Vendor: " + m_Vendor.GetName());
            AddLabel(363, 216, 2123, r.Title);

            if (m.AccessLevel >= MobileRewardVendor.FullStaffAccessLevel)
            {
                if (!viewItem)
                {
                    AddLabel(363, 216, 2123, r.Title);
                }
                else
                {
                    AddLabel(336, 296, 2101, @"Edit Item");
                    AddButton(294, 293, 4011, 4012, 4, GumpButtonType.Reply, 0); //edit item

                    if (m_Vendor.GetMobile() != null)
                    {
                        AddLabel(336, 348, 2101, @"Get Display");
                        AddButton(294, 345, 4011, 4012, 3, GumpButtonType.Reply, 0); //get display   
                    }
                }
            }
            else
            {
                OpenDisplay(m); //prevent build up of box display during editing
            }

            //Edit gump for display object
            if (viewItem) //do not display
            {
                AddLabel(336, 322, 2101, @"View Item");
                AddButton(294, 319, 4011, 4012, 1, GumpButtonType.Reply, 0); //view item
            }
            else //display
            {
                AddLabel(279, 320, 38, String.Format( "Cost: {0}", m_Reward.Cost.ToString("#,0") ) );
                AddLabel(249, 398, 69, @"You have: " + m_Vendor.Payment.Value(m));

                if (m_Reward.Restock != null)
                {
                    m_Reward.Try_Restock();
                    AddLabel(279, 340, 4, "In Stock: " + m_Reward.Restock.Count);
                }

                if (m_Reward.RewardInfo is Container)
                {
                    OpenDisplay(m);
                }
            }
        }

        private void OpenDisplay(Mobile m)
        {
            DisplayBox dbox = m_Vendor.Display;

            if (dbox != null)
            {
                dbox.DisplayTo(m, m_Reward);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m_Vendor.IsRemoved())
            {
                return;
            }

            switch (info.ButtonID)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        try
                        {
                            OpenDisplay(m);
                            m.SendGump(this);
                        }
                        catch
                        {
                            m.SendMessage("This Reward can no longer be found.");
                        }
                        break;
                    }
                case 2:
                    {
                        Container bank = m.BankBox;
                        Container pack = m.Backpack;

                        if (bank == null || pack == null)
                        {
                            break;
                        }

                        Currency curr = m_Vendor.Payment;

                        if (m_Reward.InStock(1))
                        {
                            if (curr.Purchase(m, m_Reward.Cost))
                            {
                                m_Reward.RegisterBuy(1);

                                Item i = m_Reward.RewardCopy;

                                if (!m.PlaceInBackpack(i))
                                {
                                    bank.DropItem(i);
                                    m.SendMessage("You are overweight, the Reward was added to your bank");
                                }

                                m.SendMessage("You bought {0} for {1} {2}.", m_Reward.Title, m_Reward.Cost, curr.PayName);
                            }
                            else
                            {
                                m.SendMessage("You cannot afford {0}", m_Reward.Title);
                            }
                        }
                        else
                        {
                            m.SendMessage("{0} is no longer in stock.", m_Reward.Title);
                        }

                        break;
                    }
                case 3:
                    {
                        if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
                        {
                            return;
                        }
                        if (m.Backpack != null)
                        {
                            m.AddToBackpack(new Exhibit(m_Vendor, m_Reward));
                        }
                        break;
                    }
                case 4:
                    {
                        if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
                        {
                            return;
                        }
                        m.SendGump(new EditItemGump(m_Vendor, m_Reward, null, false, false));
                        break;
                    }
            }
        }
    }
}