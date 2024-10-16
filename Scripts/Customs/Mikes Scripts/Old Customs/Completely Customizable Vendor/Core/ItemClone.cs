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
    public class ItemClone
    {
        public static Item Clone(Item item)
        {
            try
            {
                if (item is Container)
                {
                    return CloneBag(((Container)item).FindItemsByType(typeof(Item), false), (Container)item,
                        (Container)Activator.CreateInstance(item.GetType(), null));
                }
                return Clone(item.GetType(), item, (Item)Activator.CreateInstance(item.GetType(), null));
            }
            catch
            {
            }

            return null;
        }

        private static Container CloneBag(IEnumerable<Item> contents, Container old, Container copy)
        {
            //emtpy new container
            foreach (Item i in copy.FindItemsByType(typeof(Item)))
            {
                i.Delete();
            }

            //'old' contents into 'new'
            foreach (Item i in contents)
            {
                copy.DropItem(Clone(i));
            }

            copy.Name = old.Name; //requires manual set

            return copy;
        }

        private static Item Clone(Type type, Item old, Item copy)
        {
            foreach (PropertyInfo pi in type.GetProperties().Where(pi => pi.CanRead && pi.CanWrite && ValidProperty(pi.Name)))
            {
                try
                {
                    if (pi.PropertyType.IsClass)
                    {
                        CloneInnerClass(pi.PropertyType, pi.GetValue(old, null), pi.GetValue(copy, null));
                        //found object property
                    }
                    else
                    {
                        pi.SetValue(copy, pi.GetValue(old, null), null); //found value property
                    }
                }
                catch
                {
                }
            }

            copy.Name = old.Name; //requires manual set

            return copy;
        }

        private static void CloneInnerClass(Type type, object old, object copy)
        {
            if (old == null) //not defined
            {
                return;
            }

            foreach (PropertyInfo pi in type.GetProperties().Where(pi => pi.CanRead && pi.CanWrite))
            {
                try
                {
                    pi.SetValue(copy, pi.GetValue(old, null), null);
                }
                catch
                {
                }
            }
        }

        private static bool ValidProperty(string str)
        {
            return
                !(str.Equals("Parent") || str.Equals("TotalWeight") || str.Equals("TotalItems") ||
                  str.Equals("TotalGold"));
        }
    }
}
