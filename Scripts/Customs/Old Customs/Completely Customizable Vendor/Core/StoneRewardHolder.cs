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
    public class StoneRewardHolder : MetalBox
    {
        private readonly StoneRewardVendor m_S_Vendor;

        public StoneRewardHolder(StoneRewardVendor sv)
        {
            m_S_Vendor = sv;
            this.Movable = false;
            this.Name = sv.Name + "'s Display Container [DO NOT DELETE]";
            Visible = false;
        }

        public override void OnItemLifted(Mobile m, Item item)
        {
            m.SendMessage("This item should not be moved away from the vendor.");
        }

        public void UpdateName()
        {
            if (m_S_Vendor == null)
            {
                return;
            }

            this.Name = m_S_Vendor.Name + "'s Display Container [DO NOT DELETE]";
        }

        public StoneRewardHolder(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}