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
    public class ClassicVendorGump : Gump, IRewardVendorGump
    {
        private readonly IRewardVendor m_Vendor;
        private readonly BaseVendor m_BVendor;

        private readonly Mobile m_Mobile;

        private readonly List<ObjectPropertyList> m_Opls;
        private readonly List<BuyItemState> m_List;

        public List<ObjectPropertyList> ObjPropLists
        {
            get { return m_Opls; }
        }

        public static void Initialize()
        {
            MenuUploader.RegisterMenu(new ClassicVendorGump());
        }

        private ClassicVendorGump()
            : base(0, 0)
        {
            m_Vendor = null;
            m_Mobile = null;
            m_BVendor = null;
            m_Opls = null;
            m_List = null;
        }

        public ClassicVendorGump(IRewardVendor vendor, Mobile m)
            : this()
        {
            m_Vendor = vendor;
            m_Mobile = m;
            m_BVendor = Parent(vendor);
            m_Opls = new List<ObjectPropertyList>();
            m_List = new List<BuyItemState>();
        }

        private static BaseVendor Parent(IRewardVendor vendor)
        {
            BaseVendor validVendor = null;

            if (vendor != null) //only available on mobiles inherited from BaseVendor
            {
                validVendor = vendor as BaseVendor;
            }

            return validVendor;
        }

        void IRewardVendorGump.Send(Mobile m)
        {
            if (!(m_Vendor.Payment.PaymentType == typeof(Gold))) //need custom payment display
            {
                m.SendGump(this);
            }

            if (m_BVendor == null)
            {
                return;
            }
            if (m_List.Count <= 0)
            {
                return;
            }
            m_List.Sort(new BuyItemStateComparer());

            m_BVendor.SendPacksTo(m);

            if (m.NetState == null)
            {
                return;
            }

//zerodowned - edited for newest repo

            /* if (m.NetState.ContainerGridLines)
            {
                m.Send(new VendorBuyContent6017(m_List));
            }
            else
            {
                m.Send(new VendorBuyContent(m_List));
            } */

            m.Send(new VendorBuyContent(m_List));
//

            m.Send(new VendorBuyList(m_BVendor, m_List));
            m.Send(new DisplayBuyList(m_BVendor));

            m.Send(new MobileStatusExtended(m)); //make sure their gold amount is sent

            if (m_Opls != null)
            {
                foreach (ObjectPropertyList t in m_Opls)
                {
                    m.Send(t);
                }
            }

            m_BVendor.SayTo(m, 500186); // Greetings.  Have a look around.
        }

        void IRewardVendorGump.AddEntry(Reward r)
        {
            m_List.Add(new BuyItemState(r.Title, m_BVendor.BuyPack.Serial, r.Serial, r.Cost,
                (r.Restock == null ? 20 : r.Restock.Count), r.RewardInfo.ItemID, r.RewardInfo.Hue));

            m_Opls.Add(r.Display);
        }

        void IRewardVendorGump.CreateBackground()
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddImage(36, 105, 2162);
            AddItem(111, 217, m_Vendor.Payment.PayID, m_Vendor.Payment.CurrHue);
            AddLabel(110, 172, 2122, @"Pay By:  " + m_Vendor.Payment.PayName);
            AddLabel(110, 299, 2120, @"You have: " + m_Vendor.Payment.Value(m_Mobile));
            AddImage(71, 302, 57);
            AddImage(71, 174, 57);
        }
    }
}