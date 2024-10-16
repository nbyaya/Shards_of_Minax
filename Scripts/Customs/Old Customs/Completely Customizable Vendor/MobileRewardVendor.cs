//updated by zerodowned

//Cleaned up by Tresdni
//Original Author:  krazeykow

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;

namespace System.CustomizableVendor
{      
    public class MobileRewardVendor : Banker, IRewardVendor
    {
        private static AccessLevel m_FullStaffLevel = AccessLevel.GameMaster;
        private Currency m_Currency;
        private RewardCollection m_Rewards;
        private Type m_Menu;
        private DisplayBox m_Box;

        private bool m_IsBanker;

        [CommandProperty(AccessLevel.GameMaster)]
        public static AccessLevel FullStaffAccessLevel
        {
            get { return m_FullStaffLevel; }
            set { m_FullStaffLevel = value; }
        }

        public bool IsBanker
        {
            get { return m_IsBanker; }
            set { m_IsBanker = value; }
        }

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
        public MobileRewardVendor()
        {
            m_IsBanker = true;

            //default is Gold
            m_Currency = new Currency();
            m_Rewards = new RewardCollection();
            m_Menu = typeof(ClassicVendorGump);
            m_Box = new DisplayBox();

            //add to world collection
            WorldRewardVendors.RegisterVendor(this);
            //must be brought to world for client to view
            this.AddToBackpack(m_Box);

            this.Title = "";
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (!@from.Alive)
            {
                return;
            }
            list.Add(new LoadGumpEntry(@from, this));

           // if (m_IsBanker)
            //{
            //    list.Add(new OpenBankEntry(@from, this));
            //}
        }

        Mobile IRewardVendor.GetMobile()
        {
            return this;
        }

        Item IRewardVendor.GetItem()
        {
            return null;
        }

        bool IRewardVendor.IsRemoved()
        {
            return this.Deleted;
        }

        Container IRewardVendor.GetContainer()
        {
            return this.Backpack;
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

        //begin bank override
        public override bool HandlesOnSpeech(Mobile from)
        {
            if (!m_IsBanker)
            {
                return false;
            }

            base.HandlesOnSpeech(from);

            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (m_IsBanker)
            {
                base.OnSpeech(e);
            }
        }

        //end

        //Warning: Following methods will throw Exceptions
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
            Item currBox = this.Backpack.FindItemByType(typeof(DisplayBox));
            currBox.Delete();

            m_Currency = vendor.Payment;
            m_Menu = vendor.Menu;

            m_Rewards = (RewardCollection)((vendor.Rewards as ICloneable).Clone());
            m_Box = new DisplayBox(m_Rewards);

            AddToBackpack(m_Box);
        }

        //End Warning
        public override void OnDelete()
        {
            WorldRewardVendors.RemoveVendor(this);
            base.OnDelete();
        }

        //response to ClassicVendorGump - (must be inside BaseVendor)
        public override bool OnBuyItems(Mobile buyer, List<BuyItemResponse> list)
        {
            int totalCost = 0;

            var purchases = new Dictionary<Reward, int>();

            Container bank = buyer.BankBox;
            Container pack = buyer.Backpack;

            if (pack == null || bank == null)
            {
                return false;
            }

            foreach (BuyItemResponse buy in list)
            {
                Serial ser = buy.Serial;
                int amount = buy.Amount;

                if (!ser.IsItem)
                {
                    continue;
                }
                Item item = World.FindItem(ser);

                if (item == null)
                {
                    continue;
                }

                Reward r = item as Reward;

                if (r == null || !r.InStock(amount))
                {
                    continue;
                }

                totalCost += (r.Cost * amount);

                purchases.Add(r, amount);
            }

            if (purchases.Count == 0)
            {
                SayTo(buyer, 500190); // Thou hast bought nothing! 
                return false;
            }
            if (!m_Currency.Purchase(buyer, totalCost)) //cannot afford
            {
                SayTo(buyer, 500192); //Begging thy pardon, but thou casnt afford that.
                return false;
            }

            foreach (var kvp in purchases)
            {
                kvp.Key.RegisterBuy(kvp.Value);

                for (int index = 0; index < kvp.Value; index++)
                {
                    Item i = kvp.Key.RewardCopy;

                    if (buyer.PlaceInBackpack(i))
                    {
                        continue;
                    }
                    bank.DropItem(i);
                    buyer.SendMessage("You are overweight, the Reward was added to your bank");
                }

                buyer.SendMessage("You bought {0} {1}", kvp.Value, kvp.Key.Title);
            }

            buyer.PlaySound(0x32);

            return true;
        }

        //respone to 'vendor buy'
        public override void VendorBuy(Mobile from)
        {
            MenuUploader.Display((IRewardVendorGump)Activator.CreateInstance(m_Menu, new object[] { this, from }), from,
                this); //create fresh instance
        }

        //end edit
        public override void OnDoubleClick(Mobile m)
        {
            if (InRange(m, 4) && InLOS(m))
            {
                MenuUploader.Display(m_Menu, m, this, false);
            }
        }

        public MobileRewardVendor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(m_IsBanker);

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
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            WorldRewardVendors.RegisterVendor(this);

            int version = reader.ReadInt();

            m_Rewards = new RewardCollection();

            m_IsBanker = true; //for conversion

            switch (version)
            {
                case 1:
                    {
                        m_IsBanker = reader.ReadBool();

                        goto case 0;
                    }
                case 0:
                    {
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

                        break;
                    }
            }
        }
    }
}