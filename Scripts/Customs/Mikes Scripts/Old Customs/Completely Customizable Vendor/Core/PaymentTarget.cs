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
    public class PaymentTarget : Target
    {
        private readonly string m_Name;
        private readonly IRewardVendor m_Vendor;
        private readonly bool m_Is_XML;

        public PaymentTarget()
            : base(2, false, TargetFlags.None)
        {
        }

        //by item
        public PaymentTarget(IRewardVendor vendor)
            : this()
        {
            m_Vendor = vendor;
            m_Name = null;
            m_Is_XML = false;
        }

        //by item property
        public PaymentTarget(IRewardVendor vendor, string name, bool isXML)
            : this()
        {
            m_Vendor = vendor;
            m_Name = name;
            m_Is_XML = isXML;
        }

        protected override void OnTarget(Mobile m, object targeted)
        {
            if (m_Is_XML && !String.IsNullOrEmpty(m_Name))
            {
                Type attachType = ScriptCompiler.FindTypeByName(m_Name);

                if (attachType.BaseType != null && attachType.BaseType == typeof(XmlAttachment))
                {
                    var integerProps = attachType.GetProperties().Where(pi => (pi.CanRead && pi.CanWrite) && pi.PropertyType == typeof(Int32)).ToList();

                    if (integerProps.Count == 0)
                    {
                        m.SendMessage("{0} does not contain any integer based properties.", attachType.Name);
                    }
                    else
                    {
                        m.SendGump(new XMLIntPropertyGump(attachType, targeted, m_Vendor, integerProps));
                    }
                }
                else
                {
                    m.SendMessage("The XML Attachment, {0}, could not be found, the search IS case sensitive", m_Name);
                }
            }
            else if (targeted is Item)
            {
                //consume by item
                if (m_Name == null)
                {
                    m_Vendor.Payment = new Currency((Item)targeted);
                    m.SendMessage("{0} will now use {1} for payment.", m_Vendor.GetName(), targeted.GetType().Name);
                }
                //consume by item property
                else
                {
                    SetByProperty(m, targeted);
                }
            }
            else if (targeted is PlayerMobile)
            {
                if (m_Name == null)
                {
                    m.SendMessage("You can only use Property option for player.");
                }
                else
                {
                    SetByProperty(m, null);
                }
            }
            else
            {
                m.SendMessage("Invalid, target must be Item or Player.");
            }

            MenuUploader.Display(m_Vendor.Menu, m, m_Vendor, false);
        }

        private void SetByProperty(Mobile m, object o)
        {
            const string msg = "Please make sure you entered the name correctly, "
                               + "it IS case sensitive.";
            try
            {
                if (o != null) //Item
                {
                    PropertyInfo pi = o.GetType().GetProperty(m_Name);

                    if (pi != null && pi.PropertyType == typeof(Int32))
                    {
                        m_Vendor.Payment = new Currency((Item)o, pi);
                        m.SendMessage(m_Vendor.GetName() + "will now use {1}.{2} for payment.", m_Vendor.GetName(),
                            o.GetType().Name, pi.Name);
                    }
                    else
                    {
                        m.SendMessage(msg);
                    }
                }
                else //Mobile
                {
                    PropertyInfo pi = m.GetType().GetProperty(m_Name);

                    if (pi != null && pi.PropertyType == typeof(Int32))
                    {
                        m_Vendor.Payment = new Currency(m, pi);
                        m.SendMessage("{0} will now use {1}.{2} for payment.", m_Vendor.GetName(), m.GetType().Name,
                            pi.Name);
                    }
                    else
                    {
                        m.SendMessage(msg);
                    }
                }
            }
            catch
            {
                m.SendMessage(msg);
            }
        }
    }
}