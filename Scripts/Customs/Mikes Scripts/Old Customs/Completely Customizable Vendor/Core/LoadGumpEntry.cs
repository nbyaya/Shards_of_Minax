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
    public class LoadGumpEntry : ContextMenuEntry
    {
        private readonly IRewardVendor m_Vendor;
        private readonly Mobile m_Mobile;

        public LoadGumpEntry(Mobile from, IRewardVendor vendor)
            : base(6103, 8)
        {
            m_Vendor = vendor;
            m_Mobile = from;
            Enabled = true;
        }

        public override void OnClick()
        {
            try
            {
                MenuUploader.Display(
                    (IRewardVendorGump)Activator.CreateInstance(m_Vendor.Menu, new object[] { m_Vendor, m_Mobile }),
                    m_Mobile, m_Vendor); //create fresh instance
            }
            catch
            {
                m_Mobile.SendMessage("The current display is invalid.");
            }
        }
    }
}