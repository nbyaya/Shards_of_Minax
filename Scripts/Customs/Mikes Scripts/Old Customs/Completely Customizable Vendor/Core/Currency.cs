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
    public class Currency : Item
    {
        private Payment m_Payment;
        private PropertyInfo m_PropertyInfo;

        public Type XMLType { get; private set; }

        public Type PaymentType { get; private set; }

        public string PayName { get; private set; }

        public int PayID { get; private set; }

        public int CurrHue { get; private set; }

        private enum Payment
        {
            ByGold,
            ByItem,
            ByProperty,
            ByXMLAttachment
        }

        //default
        public Currency()
        {
            m_Payment = Payment.ByGold;
            PaymentType = typeof(Gold);
            m_PropertyInfo = null;
            PayID = 3823;
            PayName = "Gold";
        }

        public Currency(object bindedTo, Type attachType, PropertyInfo attachProp)
        {
            XMLType = attachType;
            m_Payment = Payment.ByXMLAttachment;
            PaymentType = bindedTo.GetType();
            m_PropertyInfo = attachProp;

            if (bindedTo is Mobile)
            {
                PayID = 8461;
                PayName = "Player " + attachProp.Name;
            }
            else if (bindedTo is Item)
            {
                PayID = ((Item)bindedTo).ItemID;
                PayName = GetName((Item)bindedTo) + "'s " + attachProp.Name;
                CurrHue = ((Item)bindedTo).Hue;
            }
        }

        //consume by item
        public Currency(Item i)
        {
            m_Payment = i.GetType() == typeof(Gold) ? Payment.ByGold : Payment.ByItem;

            PaymentType = i.GetType();
            m_PropertyInfo = null;
            PayID = CoinID(i.ItemID);
            PayName = GetName(i);
            CurrHue = i.Hue;
        }

        //consume property on player
        public Currency(IEntity m, PropertyInfo pi)
        {
            m_Payment = Payment.ByProperty;
            PaymentType = m.GetType();
            m_PropertyInfo = pi;
            PayID = 8461;
            PayName = "Player " + pi.Name;
        }

        //consume by item's number based property
        public Currency(Item i, PropertyInfo pi)
        {
            m_Payment = Payment.ByProperty;
            PaymentType = i.GetType();
            m_PropertyInfo = pi;
            PayID = CoinID(i.ItemID);
            PayName = GetName(i) + "'s " + pi.Name;
            CurrHue = i.Hue;
        }

        private void Modify(Type t, string append)
        {
            if ((t == typeof(PlayerMobile) || append == null))
            {
                return;
            }

            Item i = (Item)Activator.CreateInstance(t, null);
            PayName = GetName(i) + append;
            CurrHue = i.Hue;
            PayID = CoinID(PayID);
        }

        private static int CoinID(int itemID)
        {
            if (itemID == 3821)
            {
                return 3823;
            }
            return itemID == 3824 ? 3826 : itemID;
        }

        private void VOneUpdate()
        {
            switch (m_Payment)
            {
                case Payment.ByGold:
                case Payment.ByItem:
                    {
                        Modify(PaymentType, "");
                        break;
                    }
                case Payment.ByProperty:
                    {
                        if (!(PaymentType == typeof(PlayerMobile)))
                        {
                            Modify(PaymentType, "'s " + m_PropertyInfo.Name);
                        }
                        break;
                    }
            }
        }

        private static string GetName(Item i)
        {
            return String.IsNullOrEmpty(i.Name) ? i.GetType().Name : i.Name;
        }

        public bool Purchase(Mobile m, int cost)
        {
            int remain = Value(m) - cost;

            if (remain < 0 || m.Backpack == null)
            {
                return false;
            }

            Charge(m, cost, remain);

            return true;
        }

        private static void Charge(Container pack, Container bank, Type t, int cost)
        {
            if (pack == null || bank == null)
            {
                return;
            }

            cost = cost - pack.ConsumeUpTo(t, cost);

            if (cost > 0)
            {
                bank.ConsumeUpTo(t, cost);
            }
        }

        private void Charge(Mobile m, Type t, PropertyInfo pi, int cost, int remain)
        {
            try
            {
                if (t.BaseType == typeof(Mobile) || t == typeof(PlayerMobile))
                {
                    SetPropertyValue(m, remain);
                }
                else
                {
                    if (m.Backpack == null || m.FindBankNoCreate() == null)
                    {
                        return;
                    }

                    var packItems = m.Backpack.FindItemsByType(t);
                    var bankItems = m.FindBankNoCreate().FindItemsByType(t);

                    var items = new Item[packItems.Length + bankItems.Length];
                    packItems.CopyTo(items, 0);
                    bankItems.CopyTo(items, packItems.Length);

                    int i = 0;

                    //account for multiple objects
                    while (cost > 0 && i < items.Length)
                    {
                        int value = GetPropertyValue(items[i]);

                        int dif = value - cost;

                        //one item has enough
                        if (dif >= 0)
                        {
                            SetPropertyValue(items[i], dif);
                        }
                        //take what amount item has (the amount has already been confirmed by Value(m))
                        else // assert (dif < 0)
                        {
                            cost -= GetPropertyValue(items[i]);
                            SetPropertyValue(items[i], 0);
                        }
                        i++;
                    }
                }
            }
            catch
            {
            }
        }

        private static void Charge(Mobile m, Container pack, Container bank, int cost)
        {
            if (pack == null || bank == null)
            {
                return;
            }

            cost = cost - pack.ConsumeUpTo(typeof(Gold), cost);

            if (cost > 0)
            {
                Banker.Withdraw(m, cost);
            }
        }

        private void Charge(Mobile m, int cost, int remain)
        {
            switch (m_Payment)
            {
                case Payment.ByGold:
                    Charge(m, m.Backpack, m.FindBankNoCreate(), cost);
                    break;
                case Payment.ByItem:
                    Charge(m.Backpack, m.FindBankNoCreate(), PaymentType, cost);
                    break;
                case Payment.ByXMLAttachment:
                case Payment.ByProperty:
                    Charge(m, PaymentType, m_PropertyInfo, cost, remain);
                    break;
            }
        }

        public int Value(Mobile m)
        {
            switch (m_Payment)
            {
                case Payment.ByGold:
                    return ProcessByGold(m);
                case Payment.ByItem:
                    return ProcessByItem(m, PaymentType);
                case Payment.ByXMLAttachment:
                case Payment.ByProperty:
                    return ProcessByProperty(m, PaymentType, m_PropertyInfo);
            }
            return 0;
        }

        private static int ProcessByGold(Mobile m)
        {
            if (m.Backpack == null)
            {
                return 0;
            }

            int totalGold = 0;

            totalGold += m.Backpack.GetAmount(typeof(Gold));
            totalGold += Banker.GetBalance(m);

            return totalGold;
        }

        private static int ProcessByItem(Mobile m, Type t)
        {
            Container pack = m.Backpack;
            Container bank = m.FindBankNoCreate();

            if (pack == null || bank == null)
            {
                return 0;
            }

            int amount = (pack.GetAmount(t) + bank.GetAmount(t));

            return amount;
        }

        private int ProcessByProperty(Container pack, Container bank, Type t, PropertyInfo pi)
        {
            if (pack == null || bank == null)
            {
                return 0;
            }


            var packItems = pack.FindItemsByType(t);
            var bankItems = bank.FindItemsByType(t);

            var items = new Item[packItems.Length + bankItems.Length];
            packItems.CopyTo(items, 0);
            bankItems.CopyTo(items, packItems.Length);

            return items.Sum(o => GetPropertyValue(o));
        }

        private int GetPropertyValue(object o)
        {
            int val;

            if (m_Payment == Payment.ByXMLAttachment)
            {
                val = GetXMLValue(o);
            }
            else
            {
                val = (int)m_PropertyInfo.GetValue(o, null);
            }

            return val;
        }

        private void SetPropertyValue(object o, int val)
        {
            if (m_Payment == Payment.ByXMLAttachment)
            {
                SetXMLValue(o, val);
            }
            else
            {
                m_PropertyInfo.SetValue(o, val, null);
            }
        }

        private int GetXMLValue(object o)
        {
            int val = 0;

            if (XMLType == null)
            {
                return val;
            }

            XmlAttachment attach = XmlAttach.FindAttachment(o, XMLType);

            if (attach != null)
            {
                val = (int)m_PropertyInfo.GetValue(attach, null);
            }
            return val;
        }

        private void SetXMLValue(object o, int val)
        {
            if (XMLType == null)
            {
                return;
            }

            XmlAttachment attach = XmlAttach.FindAttachment(o, XMLType);

            if (attach != null)
            {
                m_PropertyInfo.SetValue(attach, val, null);
            }
        }

        //Master
        private int ProcessByProperty(Mobile m, Type t, PropertyInfo pi)
        {
            try
            {
                if (t.BaseType == typeof(Mobile) || t == typeof(Mobile))
                {
                    return GetPropertyValue(m);
                }
                return ProcessByProperty(m.Backpack, m.FindBankNoCreate(), t, pi);
            }
            catch
            {
            }

            return 0;
        }

        public Currency(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(2);

            //version 2
            writer.Write(XMLType == null ? "" : XMLType.Name);

            //version 1
            writer.Write(CurrHue);

            //explicit casts are given for clarification        
            writer.Write((int)m_Payment);
            writer.Write(PaymentType.Name);
            writer.Write(m_PropertyInfo == null ? "" : m_PropertyInfo.Name);
            writer.Write(PayID);
            writer.Write(PayName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();


            switch (version)
            {
                case 2:
                    {
                        try
                        {
                            string name = reader.ReadString();
                            XMLType = name.Equals("") ? null : ScriptCompiler.FindTypeByName(name);
                        }
                        catch
                        {
                        }

                        goto case 1;
                    }
                case 1:
                    {
                        CurrHue = reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Payment = (Payment)reader.ReadInt();

                        try
                        {
                            string name = reader.ReadString();
                            PaymentType = ScriptCompiler.FindTypeByName(name);
                        }
                        catch
                        {
                        }

                        try
                        {
                            string propName = reader.ReadString();
                            if (propName.Equals("")) //never set
                            {
                                m_PropertyInfo = null;
                            }
                            else if (m_Payment == Payment.ByXMLAttachment && XMLType != null)
                            //XML requires find property by the XML Class type (m_XML_Type)
                            {
                                m_PropertyInfo = XMLType.GetProperty(propName);
                            }
                            else
                            {
                                m_PropertyInfo = PaymentType.GetProperty(propName);
                            }
                        }
                        catch
                        {
                        }

                        PayID = reader.ReadInt();
                        PayName = reader.ReadString();

                        break;
                    }
            }
            if (version != 0)
            {
                return;
            }
            try
            {
                VOneUpdate();
            }
            catch
            {
            }
        }
    }
}