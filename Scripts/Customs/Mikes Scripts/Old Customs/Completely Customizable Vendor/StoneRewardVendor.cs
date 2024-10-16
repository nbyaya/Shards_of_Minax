//updated by zerodowned

//Cleaned up by Tresdni
//Original Author:  krazeykow

using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;


namespace System.CustomizableVendor
{    
    public class StoneRewardVendor : Item, IRewardVendor
    {
        private Currency m_Currency;
        private RewardCollection m_Rewards;
        private Type m_Menu;
        private StoneRewardHolder m_Box_Holder;
        private DisplayBox m_Box;

        Currency IRewardVendor.Payment
        {
            get { return m_Currency; }
            set { m_Currency = value; }
        }

        RewardCollection IRewardVendor.Rewards
        {
            get { return m_Rewards; }
            set { m_Rewards = value; }
        }

        Type IRewardVendor.Menu
        {
            get { return m_Menu; }
            set { m_Menu = value; }
        }

        DisplayBox IRewardVendor.Display
        {
            get { return m_Box; }
        }

        [Constructable]
        public StoneRewardVendor()
        {
            m_Currency = new Currency();
            m_Rewards = new RewardCollection();
            m_Menu = typeof(JewlRewardGump);
            m_Box = new DisplayBox();

            ItemID = 0xEDC;
            Name = "Stone Vendor";

            WorldRewardVendors.RegisterVendor(this);

            m_Box_Holder = new StoneRewardHolder(this);
            m_Box_Holder.DropItem(m_Box);

            this.Movable = false;
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);

            if (m_Box_Holder == null)
            {
                return;
            }
            m_Box_Holder.Location = this.Location;
            m_Box_Holder.Z = this.Z - 10;
        }

        public override void OnMapChange()
        {
            base.OnMapChange();

            if (m_Box_Holder != null)
            {
                m_Box_Holder.Map = this.Map;
            }
        }

        Mobile IRewardVendor.GetMobile()
        {
            return null;
        }

        Item IRewardVendor.GetItem()
        {
            return this;
        }

        bool IRewardVendor.IsRemoved()
        {
            return this.Deleted;
        }

        Container IRewardVendor.GetContainer()
        {
            return m_Box_Holder;
        }

        string IRewardVendor.GetName()
        {
            return this.Name;
        }

        Map IRewardVendor.GetMap()
        {
            return this.Map;
        }

        Point3D IRewardVendor.GetLocation()
        {
            return this.Location;
        }

        void IRewardVendor.AddReward(Reward r)
        {
            m_Box.AddDisplay(r);
            m_Rewards.Add(r);
        }

        void IRewardVendor.RemoveReward(Reward r)
        {
            m_Rewards.Remove(r);
            m_Box.RemoveDisplay(r);
        }

        void IRewardVendor.CopyVendor(IRewardVendor vendor)
        {
            Item currBox = m_Box_Holder.FindItemByType(typeof(DisplayBox));
            currBox.Delete();

            m_Currency = vendor.Payment;
            m_Menu = vendor.Menu;

            m_Rewards = (RewardCollection)((vendor.Rewards as ICloneable).Clone());
            m_Box = new DisplayBox(m_Rewards);

            m_Box_Holder.DropItem(m_Box);
        }

        //End Warning
        public override void OnDelete()
        {
            if (m_Box_Holder != null)
            {
                m_Box_Holder.Delete();
            }

            WorldRewardVendors.RemoveVendor(this);
            base.OnDelete();
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (m_Box_Holder != null)
            {
                m_Box_Holder.MoveToWorld(this.Location, this.Map);
                m_Box_Holder.Z = this.Z - 10;
                m_Box_Holder.UpdateName();
            }
            MenuUploader.Display(m_Menu, m, this, false);
        }

        public StoneRewardVendor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); //Version 0

            int count = m_Rewards.Count;
            writer.Write(count);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    writer.WriteItem(m_Rewards[i]);
                }
            }

            writer.WriteItem(m_Currency);
            writer.Write(m_Menu.Name);
            writer.WriteItem(m_Box);
            writer.WriteItem(m_Box_Holder);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            WorldRewardVendors.RegisterVendor(this);

            int version = reader.ReadInt();

            m_Rewards = new RewardCollection();

            int count = reader.ReadInt();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Reward r = (Reward)reader.ReadItem();
                    m_Rewards.Add(r);
                }
            }

            m_Currency = (Currency)reader.ReadItem();

            try
            {
                string name = reader.ReadString();
                m_Menu = ScriptCompiler.FindTypeByName(name);
            }
            catch
            {
                m_Menu = typeof(JewlRewardGump);
            }

            m_Box = (DisplayBox)reader.ReadItem();
            m_Box_Holder = (StoneRewardHolder)reader.ReadItem();
        }
    }
}