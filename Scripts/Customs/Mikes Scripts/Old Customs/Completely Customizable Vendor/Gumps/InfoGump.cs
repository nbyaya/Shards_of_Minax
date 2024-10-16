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
    public class InfoGump : Gump
    {
        public InfoGump()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);

            AddImageTiled(0, 0, 452, 716, 2702);
            AddHtml(21, 78, 407, 105,
                @"Consume Item:  Vendor counts the number of the chosen item present in player's backpack.  

        Property(Item/Player):  Vendor takes the total of the given number based property (example: PlayerPoints, Charges)
        (Items): Adds up from every item present.
        (Player): Simply takes from what is present.
        ", true, true);

            AddLabel(23, 52, 2000, @"Choosing How Player Pays:");
            AddHtml(21, 242, 407, 105, @"You can create your own rewards in game, all properties will be copied over.  ",
                true, true);
            AddLabel(24, 216, 2000, @"Adding Items");
            AddLabel(239, 479, 37, @"Author: krazeykow1102");
            AddItem(391, 467, 8451);
            AddLabel(24, 368, 2000, @"How Displays Work");
            AddHtml(24, 394, 410, 78,
                @"Displays allow you to create your own interactive Reward room.  The display is a simple mirror to the item on the vendor.  Payment and cost will reflect what is on the vendor.  Vendor must stay in tact for display to function",
                true, true);
        }
    }
}