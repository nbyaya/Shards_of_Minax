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
    public static class WorldRewardVendors
    {
        private static readonly List<IRewardVendor> m_Vendors = new List<IRewardVendor>();

        public static List<IRewardVendor> Vendors
        {
            get { return m_Vendors; }
        }

        public static void RegisterVendor(IRewardVendor vendor)
        {
            m_Vendors.Add(vendor);
        }

        public static void RemoveVendor(IRewardVendor vendor)
        {
            m_Vendors.Remove(vendor);
        }
    }
}