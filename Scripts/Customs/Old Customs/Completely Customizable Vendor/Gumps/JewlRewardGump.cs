//updated by zerodowned

//Cleaned up by Tresdni
//Original Author:  krazeykow

using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;

namespace System.CustomizableVendor
{  
    public class JewlRewardGump : Gump, IRewardVendorGump
    {
        private int m_PageNum;
        public int m_EntryNum;
        private int m_PosY;
        public IRewardVendor m_Vendor;
        private int m_CurrencyAmnt;

        public static void Initialize()
        {
            MenuUploader.RegisterMenu(new JewlRewardGump());
        }

        protected JewlRewardGump()
            : base(0, 0)
        {
            m_PageNum = 1;
            m_EntryNum = 1;
            m_PosY = 110; //130;
            m_Vendor = null;
            Mobile = null;
            m_CurrencyAmnt = 0;
        }

        public JewlRewardGump(IRewardVendor vendor, Mobile m)
            : this()
        {
            m_Vendor = vendor;
            Mobile = m;
            m_CurrencyAmnt = vendor.Payment.Value(m);
        }

        //Properties
        public int PageNum
        {
            get { return m_PageNum; }
            set { m_PageNum = value; }
        }

        public int EntryNum
        {
            get { return m_EntryNum; }
            set { m_EntryNum = value; }
        }

        public int PosY
        {
            get { return m_PosY; }
            set { m_PosY = value; }
        }

        public IRewardVendor Vendor
        {
            get { return m_Vendor; }
            set { m_Vendor = value; }
        }

        private Mobile Mobile { get; set; }

        public int CurrencyAmnt
        {
            get { return m_CurrencyAmnt; }
            set { m_CurrencyAmnt = value; }
        }

        //Interface Explicit Implementation 
        void IRewardVendorGump.Send(Mobile m)
        {
            SendControl(m);
        }

        void IRewardVendorGump.CreateBackground()
        {
            CreateBackgroundControl();
        }

        void IRewardVendorGump.AddEntry(Reward r)
        {
            AddEntryControl(r);
        }

        protected virtual void SendControl(Mobile m)
        {
            m.SendGump(this);
        }

        protected virtual void CreateBackgroundControl()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(179, 34, 430, 656, Settings.backgroundID);
			AddHtml( 230, 52, 330, 22, String.Format("<big><basefont color=#FFFFFF><center>{0}", m_Vendor.GetName()), false, false);
            AddItem(230, 73, m_Vendor.Payment.PayID, m_Vendor.Payment.CurrHue);
			AddHtml( 275, 78, 285, 22, String.Format("<basefont color=#00FF00>You have: {0} {1}", m_CurrencyAmnt.ToString("#,0"), m_Vendor.Payment.PayName ), false, false);
           
            AddPage(1);
        }

        protected virtual void AddEntryControl(Reward r)
        {
            ImageTileButtonInfo b = new ItemTileButtonInfo(r.RewardInfo);
			
            Rectangle2D rec = ItemBounds.Table[r.RewardInfo.ItemID];
           
			if (r.Restock != null)
            {
                if (r.Restock.RestockRate.TotalMinutes == 0)
                    AddHtml( 313, (m_PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1}<br><basefont color=#FF2A00>LIMITED   In Stock: {2}<br><basefont color=#FFFFFF>{3}", r.Title, r.Cost.ToString("#,0"), r.Restock.Count.ToString("#,0"), r.Description ), false, true );
                
                if (r.Restock.Maximum > 0)
                    AddHtml( 313, (m_PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1}<br><basefont color=#FF2A00>LIMITED   In Stock: {2}<br><basefont color=#FFFFFF>{3}", r.Title, r.Cost.ToString("#,0"), r.Restock.Count.ToString("#,0"), r.Description ), false, true );
            }
			else
			{
				AddHtml( 313, (m_PosY + 18), 255, 75, String.Format("<basefont color=#40BFFF>{0}<br><basefont color=#FF8A14>Cost: {1}<br><br><basefont color=#FFFFFF>{2}", r.Title, r.Cost.ToString("#,0"), r.Description ), false, true );
			}

			AddButton(227, (m_PosY + 26), Settings.itemButton, Settings.itemButtonPressed, (m_EntryNum), GumpButtonType.Reply, m_PageNum);
			
            if( Settings.ButtonBackground )
                AddImageTiled(231, (PosY + 29), 72, 53, Settings.ButtonBackgroundID );
			
			//AddItem( 257 - m_X /*+ max*/ / 2 - m_Width / 2, (m_PosY + 26) + y, b.ItemID, r.RewardInfo.Hue); // 55 - item.X + max / 2 - item.Width / 2
            AddItem( 267 - rec.Width / 2 - rec.X , (m_PosY + 26) + 30 - rec.Height / 2 - rec.Y,  b.ItemID, r.RewardInfo.Hue);

			AddItemProperty(r.RewardInfo.Serial);
            
            AddImageTiled(215, (m_PosY + 95), 359, 2, 96);
            
            m_PosY += 82;

            //add new page every six entries
            //if (m_EntryNum % 4 == 0) // use to be every 4 entries
			if (m_EntryNum % 6 == 0)
            {
                AddButton(316, 643, 2471, 2470, -1, GumpButtonType.Page, m_PageNum + 1);

                AddPage(++m_PageNum);

                AddButton(221, 643, 2468, 2467, -1, GumpButtonType.Page, m_PageNum - 1);

                //reset to top of page
                m_PosY = 110;
            }

            m_EntryNum++;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (info.ButtonID == 0)
            {
                return;
            }
			
			MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, true);

            try
            {
                m.CloseGump(typeof(PlayerViewItemGump));
                m.SendGump(new PlayerViewItemGump   (m, m_Vendor, m_Vendor.Rewards[info.ButtonID - 1], true));
            }
            catch
            {
                m.SendMessage("Vendor could not be found");
            }
        }
    }
}