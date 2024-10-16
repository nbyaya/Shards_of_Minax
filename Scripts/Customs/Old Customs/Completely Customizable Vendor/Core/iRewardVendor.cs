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
    public interface IRewardVendor
    {
        //used to determine how player pays
        Currency Payment { get; set; }
        //to keep added items
        RewardCollection Rewards { get; set; }
        //to change gump
        Type Menu { get; set; }
        //to show player items
        DisplayBox Display { get; }
        //to return what holds 'DisplayBox' object
        Container GetContainer();
        //to return removal status
        bool IsRemoved();

        //serial /deserial
        Mobile GetMobile();
        Item GetItem();

        //used for 'world collection'
        string GetName();
        Point3D GetLocation();
        Map GetMap();

        //to manage RewardCollection
        void AddReward(Reward r);
        void RemoveReward(Reward r);

        //to create vendor clones
        void CopyVendor(IRewardVendor vendor);
    }
}