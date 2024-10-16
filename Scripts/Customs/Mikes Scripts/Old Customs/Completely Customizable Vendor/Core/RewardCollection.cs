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
    public class RewardCollection : List<Reward>, ICloneable
    {
        object ICloneable.Clone()
        {
            RewardCollection clone = new RewardCollection();
            clone.AddRange(this.Select(r => ((Reward)(r as ICloneable).Clone())));

            return clone;
        }
    }
}