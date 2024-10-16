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
    public class XMLIntPropertyGump : Gump
    {
        private readonly Type m_XML_Type;
        private readonly object m_Target;

        private readonly IRewardVendor m_Vendor;
        private readonly List<PropertyInfo> m_Props;

        private int m_PageNum;
        private int m_PosY;
        private int m_EntryNum;

        public XMLIntPropertyGump(Type xmlType, object target, IRewardVendor vendor, List<PropertyInfo> props)
            : base(0, 0)
        {
            m_XML_Type = xmlType;
            m_Target = target;
            m_Vendor = vendor;
            m_Props = props;

            m_PageNum = 1;
            m_PosY = 134;
            m_EntryNum = 1;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImageTiled(284, 90, 168, 384, 2702);
            AddLabel(315, 99, 49, xmlType.Name);
            AddImageTiled(292, 123, 151, 3, 9151);

            AddPage(1);

            FillEntries();
        }

        private void FillEntries()
        {
            foreach (PropertyInfo pi in m_Props)
            {
                AddButton(295, m_PosY, 5601, 5605, m_EntryNum, GumpButtonType.Reply, 0);
                AddLabel(319, m_PosY, 43, pi.Name);

                m_PosY += 26;

                if (m_EntryNum % 10 == 0)
                {
                    AddButton(423, 444, 9903, 9904, -1, GumpButtonType.Page, m_PageNum + 1);

                    AddPage(++m_PageNum);

                    AddButton(292, 444, 9909, 9910, -1, GumpButtonType.Page, m_PageNum - 1);

                    //reset to top of page
                    m_PosY = 134;
                }
                m_EntryNum++;
            }
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
                return;
            }

            m_Vendor.Payment = new Currency(m_Target, m_XML_Type, m_Props[info.ButtonID - 1]);
            m.SendMessage("{0}'s payment method set to {1}.{2}", m_Vendor.GetName(), m_XML_Type.Name,
                m_Props[info.ButtonID - 1].Name);
        }
    }
}