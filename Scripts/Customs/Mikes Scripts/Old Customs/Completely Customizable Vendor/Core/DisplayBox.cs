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
    public class DisplayBox : LargeCrate, IEnumerable
    {
        private Dictionary<Reward, MetalBox> m_Boxes;

        public override bool IsPublicContainer
        {
            get { return true; }
        }

        public MetalBox this[Reward r]
        {
            get
            {
                MetalBox box = null; //not found

                m_Boxes.TryGetValue(r, out box);

                return box;
            }
        }

        [Constructable]
        public DisplayBox()
        {
            Name = "Display Box [DO NOT REMOVE]";
            LiftOverride = true;
            m_Boxes = new Dictionary<Reward, MetalBox>();
        }

        public DisplayBox(RewardCollection rc)
            : this()
        {
            CreateEntries(rc);
        }

        public bool Contains(Reward r)
        {
            return m_Boxes.ContainsKey(r);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Boxes.GetEnumerator();
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            reject = 0;
            return false;
        }

        public void DisplayTo(Mobile m, Reward r)
        {
            string invalid = "This item can no longer be displayed.";
            bool staff = m.AccessLevel >= MobileRewardVendor.FullStaffAccessLevel;

            try
            {
                Container[] containers = { (Container)this.Parent, this }; //cast exception if parent isn't container

                foreach (Container c in containers)
                {
    //zerodowned - edited for newest repo
    
                    /* if (m.NetState != null && m.NetState.ContainerGridLines)
                    {
                        m.Send(new ContainerContent6017(m, c));
                    }
                    else
                    {
                        m.Send(new ContainerContent(m, c));
                    } */

                    m.Send(new ContainerContent(m, c));
    //
                }

                m_Boxes[r].DisplayTo(m); //throws null ref if not found

                m.SendMessage("A container has been opened for you to view the item.");
            }
            catch (NullReferenceException)
            {
                m.SendMessage(staff ? "Display error - please insure Reward is still on this vendor." : invalid);
            }
            catch (InvalidCastException)
            {
                m.SendMessage(staff
                    ? "Display error - the current vendor does not have a DisplayBox in backpack."
                    : invalid);
            }
            catch (Exception)
            {
                m.SendMessage(staff ? "Unknown error occured with display." : invalid);
            }
        }

        //Warning: must catch exception in methods
        private void CreateEntries(IEnumerable<Reward> rc)
        {
            foreach (Reward r in rc)
            {
                AddDisplay(r, r.RewardInfo);
            }
        }

        private void AddDisplay(Reward key, Item i)
        {
            if (m_Boxes.ContainsKey(key))
            {
                throw new Exception("Key already in use.");
            }

            MetalBox container = new MetalBox();

            container.DropItem(i);

            DropItem(container);

            m_Boxes.Add(key, container);
        }

        public void AddDisplay(Reward r)
        {
            AddDisplay(r, r.RewardInfo);
        }

        public void RemoveDisplay(Reward r)
        {
            m_Boxes[r].Delete(); //remove from displaybox
            m_Boxes.Remove(r); //remove from hashtable      
        }

        //End Warning

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            return false; //do nothing
        }

        public DisplayBox(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version

            int count = 0;
            try
            {
                count = m_Boxes.Count;
            }
            catch
            {
                m_Boxes = new Dictionary<Reward, MetalBox> { { new Reward(), new MetalBox() } };
                count = m_Boxes.Count;
            }
            writer.Write(count);

            foreach (var kvp in m_Boxes)
            {
                writer.Write(kvp.Key);
                writer.WriteItem(kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();


            int count = reader.ReadInt();

            m_Boxes = new Dictionary<Reward, MetalBox>();
            if (count <= 0)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Reward r = (Reward)reader.ReadItem();
                MetalBox box = (MetalBox)reader.ReadItem();

                m_Boxes.Add(r, box);
            }
        }
    }
}