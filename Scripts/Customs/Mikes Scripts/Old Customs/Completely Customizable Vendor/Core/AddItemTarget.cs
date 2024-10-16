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
    public class AddItemTarget : Target
    {
        private readonly IRewardVendor m_Vendor;

        public AddItemTarget()
            : base(2, false, TargetFlags.None)
        {
        }

        public AddItemTarget(IRewardVendor vendor)
            : this()
        {
            m_Vendor = vendor;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            Item i = targeted as Item;

            if (m_Vendor == null || i == null)
            {
                return;
            }

            Item clone = ItemClone.Clone(i);

            if (clone != null) //copy did not fail
            {
                from.SendGump(new EditItemGump(m_Vendor, new Reward(clone), null, false, true));
            }
            else
            {
                from.SendMessage("This item cannot be copied.");
            }
        }
    }
}