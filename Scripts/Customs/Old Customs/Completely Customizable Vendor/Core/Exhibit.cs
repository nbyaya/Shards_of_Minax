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
    public class Exhibit : Item
    {
        private IRewardVendor m_Vendor;
        private Reward m_Reward;
        private ObjectPropertyList m_PropertyList;

        public Exhibit(IRewardVendor vendor, Reward r)
        {
            m_Vendor = vendor;
            m_Reward = r;

            SetOPL(r);

            this.ItemID = r.RewardInfo.ItemID;
            this.Hue = r.RewardInfo.Hue;
            this.Movable = false;
        }

        public override void SendPropertiesTo(Mobile from)
        {
            if (m_PropertyList == null)
            {
                m_PropertyList = new ObjectPropertyList(this);
                m_PropertyList.Add("Display [Broken]");
            }

            from.Send(m_PropertyList);
        }

        private void SetOPL(Reward r)
        {
            Item i = r.RewardInfo;

            m_PropertyList = new ObjectPropertyList(this);

            m_PropertyList.Add("Cost: " + m_Reward.Cost + " " + m_Vendor.Payment.PayName + " [Click for more]");

            i.GetProperties(m_PropertyList);
            i.AppendChildProperties(m_PropertyList);

            m_PropertyList.Terminate();
            m_PropertyList.SetStatic();
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m_Reward.Cost == 0)
            {
                Visible = false;
                m.SendMessage("This item is no longer available to purchase.");
            }
            else if (m_Vendor == null || m_Vendor.IsRemoved() || m_Reward == null || !m_Vendor.Display.Contains(m_Reward))
            {
                m_Vendor = null; // free up resource
                m_Reward = null;
                m.SendMessage("This item is no longer available to purchase.");
            }
            else
            {
                m.CloseGump(typeof(ViewItemGump));
                m.SendGump(new ViewItemGump(m, m_Vendor, m_Reward, false));
            }
        }

        public Exhibit(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (!reader.ReadBool())
            {
                return;
            }
            m_Reward = (Reward)reader.ReadItem();

            m_Vendor = (IRewardVendor)reader.ReadMobile();

            try
            {
                SetOPL(m_Reward);
            }
            catch
            {
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version  

            if (m_Vendor == null || m_Vendor.IsRemoved() || m_Reward == null || m_Reward.Deleted)
            {
                m_Vendor = null;
                m_Reward = null;

                writer.Write(false); //do not deserialize
            }
            else
            {
                writer.Write(true);

                writer.WriteItem(m_Reward);

                writer.Write(m_Vendor.GetMobile());
            }
        }
    }
}