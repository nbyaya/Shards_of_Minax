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
    //Created for the usage of Warning Gump delegate 
    public class Storage
    {
        private readonly object[] m_Objs;

        public object this[int index]
        {
            get { return m_Objs[index]; }
        }

        public Storage(params object[] input)
        {
            m_Objs = input;
        }
    }
}