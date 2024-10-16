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
    public class EditItemGump : Gump
    {
        private readonly IRewardVendor m_Vendor;
        private readonly Reward m_Reward;

        private readonly Reward.RestockInfo m_Restock;
        private readonly bool m_RestockOpen;

        private readonly bool m_IsAdd;

        private readonly Storage m_Entries;

        public EditItemGump(IRewardVendor vendor, Reward r, Storage entries, bool openRestockInfo, bool addingItem)
            : base(0, 0)
        {
            m_Vendor = vendor;
            m_Reward = r;

            m_Restock = r.Restock;
            m_RestockOpen = openRestockInfo;
            m_IsAdd = addingItem;

            m_Entries = entries;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImageTiled(227, 46, 329, 518, 2702);
            AddImageTiled(252, 291, 280, 217, 5104);
            AddLabel(253, 266, 2124, @"Description:");
            AddImageTiled(334, 147, 192, 23, 5104);
            AddLabel(258, 150, 2124, @"Cost:");
            AddImageTiled(336, 107, 192, 23, 5104);
            AddLabel(258, 108, 2124, @"Name:");
            AddImage(354, 199, Settings.itemButton);
            AddItem(373, 208, r.RewardInfo.ItemID, r.RewardInfo.Hue);
            AddImageTiled(330, 54, 138, 16, 5214);
            AddLabel(373, 51, 1324, @"Edit Item");
            AddButton(465, 222, 5601, 5605, 2, GumpButtonType.Reply, 0); //restock
            AddLabel(447, 198, 2120, @"Restock ");
            AddLabel(447, 246, 2120, @"(Optional)");
            AddButton(324, 523, 4023, 4024, 1, GumpButtonType.Reply, 0); //ok
            AddButton(421, 523, 4017, 4018, 0, GumpButtonType.Reply, 0); //cancel

            if (entries == null)
            {
                InitialText();
            }
            else
            {
                PacketText();
            }

            if (!openRestockInfo && r.Restock == null)
            {
                return;
            }
            AddRestockMenu();
            m_RestockOpen = true;
        }

        private void InitialText()
        {
            AddTextEntry(252, 292, 277, 215, 1879, 0, m_Reward.Description);
            AddTextEntry(334, 148, 188, 20, 1879, 1, m_Reward.Cost.ToString());
            AddTextEntry(336, 108, 188, 20, 1879, 2, m_Reward.Title);
        }

        private void PacketText()
        {
            AddTextEntry(252, 292, 277, 215, 1879, 0, m_Entries[0].ToString()); //desc
            AddTextEntry(334, 148, 188, 20, 1879, 1, m_Entries[1].ToString()); //cost 
            AddTextEntry(336, 108, 188, 20, 1879, 2, m_Entries[2].ToString()); //title
        }

        private void AddRestockMenu()
        {
            AddPage(1);

            AddImageTiled(601, 47, 181, 469, 2624);
            AddLabel(648, 60, 2129, @"Restock Info");
            AddLabel(637, 98, 1418, @"Available");
            AddImageTiled(633, 123, 120, 16, 5214);
            AddTextEntry(637, 121, 107, 20, 547, 3, (m_Restock == null ? "Enter." : m_Restock.Count.ToString()));
            //3 - beginning amount
            AddLabel(660, 161, 1418, @"Maximum");
            AddImageTiled(631, 186, 120, 16, 5214);
            AddTextEntry(635, 184, 107, 20, 547, 4,
                ((m_Restock == null || m_Restock.Maximum < 0) ? "" : m_Restock.Maximum.ToString())); //4 - Maxinum
            AddLabel(648, 227, 1418, @"Restock Rate");
            AddImageTiled(635, 252, 72, 16, 5214);
            AddTextEntry(638, 250, 64, 20, 547, 5, (m_Restock == null ? "0" : m_Restock.Hours.ToString())); //5 - hour
            AddLabel(714, 250, 602, @"hours");
            AddImageTiled(635, 292, 72, 16, 5214);
            AddLabel(716, 290, 602, @"mins");
            AddLabel(632, 323, 602, @"amount to restock");
            AddImageTiled(636, 355, 72, 16, 5214);
            AddTextEntry(638, 290, 64, 20, 547, 6, (m_Restock == null ? "0" : m_Restock.Minutes.ToString()));
            //6 - minute
            AddTextEntry(639, 353, 64, 20, 547, 7, (m_Restock == null ? "0" : m_Restock.RestockAmnt.ToString()));
            //7 - Restock Amount      
            AddImage(655, 377, 109);
            AddButton(636, 474, 4022, 4021, 3, GumpButtonType.Reply, 0);
            AddLabel(679, 475, 132, @"Void Restock");
        }

        private Reward.RestockInfo GetRestockInfo(RelayInfo info)
        {
            if (!m_RestockOpen)
            {
                return m_Restock; //default
            }
            int numMinutes = 0, numHours = 0;
            int numBegin, numMax, numRestockAmnt;

            TextRelay entry3 = info.GetTextEntry(3);
            string beginAmount = (entry3.Text.Trim());

            TextRelay entry4 = info.GetTextEntry(4);
            string max = (entry4.Text.Trim());

            TextRelay entry5 = info.GetTextEntry(5);
            string hour = (entry5.Text.Trim());

            TextRelay entry6 = info.GetTextEntry(6);
            string minute = (entry6.Text.Trim());

            TextRelay entry7 = info.GetTextEntry(7);
            string restockAmount = (entry7.Text.Trim());

            Int32.TryParse(hour, out numHours);
            Int32.TryParse(minute, out numMinutes);
            Int32.TryParse(beginAmount, out numBegin);
            Int32.TryParse(max, out numMax);
            Int32.TryParse(restockAmount, out numRestockAmnt);

            if (numBegin <= 0)
            {
                return m_Restock; //default
            }
            if ((numMinutes + numHours) <= 0 || numRestockAmnt <= 0)
            {
                return new Reward.RestockInfo(numBegin);
            }
            return numMax > 0 ? new Reward.RestockInfo(numHours, numMinutes, numBegin, numRestockAmnt, numMax) : new Reward.RestockInfo(numHours, numMinutes, numBegin, numRestockAmnt);
        }

        private static Storage GetEntries(RelayInfo info)
        {
            //Description
            TextRelay entry0 = info.GetTextEntry(0);
            string desc = (entry0 == null ? "" : entry0.Text.Trim());

            //Cost
            TextRelay entry1 = info.GetTextEntry(1);
            string costtext = (entry1 == null ? "Free" : entry1.Text.Trim());

            //Name
            TextRelay entry2 = info.GetTextEntry(2);
            string name = (entry2 == null ? "" : entry2.Text.Trim());

            return new Storage(desc, costtext, name);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {

            Mobile m = sender.Mobile;
            if (m.AccessLevel < MobileRewardVendor.FullStaffAccessLevel)
            {
                return;
            }
            switch (info.ButtonID)
            {
                case 0:
                    {
                        //cancel
                        break;
                    }
                case 1: //ok
                    {
                        try
                        {
                            //Description
                            TextRelay entry0 = info.GetTextEntry(0);
                            string desc = (entry0 == null ? "" : entry0.Text.Trim());

                            //Cost
                            TextRelay entry1 = info.GetTextEntry(1);
                            string costtext = (entry1 == null ? "%Error" : entry1.Text.Trim());
                            int cost = Int32.Parse(costtext);

                            //Name
                            TextRelay entry2 = info.GetTextEntry(2);
                            string name = (entry2 == null ? "" : entry2.Text.Trim());

                            if (m_IsAdd)
                            {
                                m_Vendor.AddReward(m_Reward);
                            }

                            m_Reward.Edit(cost, name, desc);

                            if (m_RestockOpen)
                            {
                                Reward.RestockInfo input = GetRestockInfo(info);

                                if (input != null)
                                {
                                    if (m_Restock != null && !(m_Restock.Equals(input))) //do not reassign same object
                                    {
                                        m_Reward.Restock = input;
                                    }
                                    else
                                    {
                                        m_Reward.Restock = input;
                                    }
                                }
                            }

                            m.SendMessage("{0} has been modified in {1}'s collection", name, m_Vendor.GetName());
                        }
                        catch
                        {
                            m.SendMessage(
                                "Please make sure all fields are filled correctly, and the title has not been previously used.");
                        }

                        break;
                    }
                case 2: //open restock options
                    {
                        m.SendGump(new EditItemGump(m_Vendor, m_Reward, GetEntries(info), true, m_IsAdd));

                        return; //override display 
                    }
                case 3: //delete restock options
                    {
                        m_Reward.Void_RestockInfo();

                        m.SendGump(new EditItemGump(m_Vendor, m_Reward, GetEntries(info), false, m_IsAdd));

                        return; //override display 
                    }
            }
            MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, true); //send back display
        }
    }
}
