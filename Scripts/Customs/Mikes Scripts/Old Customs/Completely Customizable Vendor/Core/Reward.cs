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
    public class Reward : Item, ICloneable
    {
        private int m_Cost;
        private string m_Title;
        private string m_Description;

        private ObjectPropertyList m_Display;

        private RestockInfo m_RestockInfo;
        private int m_BuyCount;

        public int BuyCount
        {
            get { return m_BuyCount; }
            set { m_BuyCount = value; }
        }

        public RestockInfo Restock
        {
            get { return m_RestockInfo; }
            set { m_RestockInfo = value; }
        }

        public ObjectPropertyList Display
        {
            get
            {
                if (m_Display != null)
                {
                    return m_Display;
                }
                m_Display = new ObjectPropertyList(this);

                RewardInfo.GetProperties(m_Display);
                RewardInfo.AppendChildProperties(m_Display);

                m_Display.Terminate();
                m_Display.SetStatic();

                return m_Display;
            }
        }

        public void Void_RestockInfo()
        {
            m_RestockInfo = null;
        }

        //returns ((-1) -> no restockInfo) or current count
        public int Try_Restock()
        {
            if (m_RestockInfo == null)
            {
                return -1;
            }

            TimeSpan dif = DateTime.UtcNow - m_RestockInfo.LastRestock;

            if (dif <= (m_RestockInfo.RestockRate) || !(m_RestockInfo.RestockRate.TotalMinutes > 0))
            {
                return m_RestockInfo.Count;
            }
            int cycles = (int)(dif.TotalMinutes / m_RestockInfo.RestockRate.TotalMinutes);

            for (int i = 0; i < cycles; ++i)
            {
                if (m_RestockInfo.Restock())
                {
                    continue;
                }
            }

            return m_RestockInfo.Count;
        }

        public bool InStock(int amount)
        {
            return (m_RestockInfo == null || (m_RestockInfo.Count - amount >= 0));
        }

        public void RegisterBuy(int amount)
        {
            if (m_RestockInfo != null && InStock(amount))
            {
                m_RestockInfo.Count -= amount;
            }

            this.m_BuyCount += amount;
        }

        //for retrieving information only
        public Item RewardInfo { get; private set; }

        //use only when creating Reward for player
        public Item RewardCopy
        {
            get { return GetReward(); }
        }

        public int Cost
        {
            get { return m_Cost; }
            set { m_Cost = value; }
        }

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        object ICloneable.Clone()
        {
            return new Reward(this);
        }

        public Reward()
        {
            RewardInfo = new Item();
            m_Cost = 0;
            m_Title = "";
            m_Description = "";
            m_RestockInfo = null;
        }

        private Reward(Reward r)
        {
            RewardInfo = r.RewardCopy;
            m_Cost = r.Cost;
            m_Title = r.Title;
            m_Description = r.Description;
            m_RestockInfo = null;
        }

        public Reward(Item i)
        {
            RewardInfo = i;
            m_Cost = 0;
            m_Title = (i.Name ?? i.GetType().Name);
            m_Description = "";
            m_RestockInfo = null;
        }

        public Reward(Item i, int cost)
        {
            RewardInfo = i;
            m_Cost = cost;
            m_Title = (i.Name ?? i.GetType().Name);
            m_Description = "";
            m_RestockInfo = null;
        }

        public Reward(Item i, int cost, string title)
        {
            RewardInfo = i;
            m_Cost = cost;
            m_Title = title;
            m_Description = "";
            m_RestockInfo = null;
        }

        public Reward(Item i, int cost, string title, string desc)
        {
            RewardInfo = i;
            m_Cost = cost;
            m_Title = title;
            m_Description = desc;
            m_RestockInfo = null;
        }

        public class RestockInfo
        {
            private TimeSpan m_RestockRate;
            private DateTime m_LastRestock;

            private readonly int m_RestockAmnt;
            private int m_Count;
            private int? m_Maximum;

            public TimeSpan RestockRate
            {
                get { return m_RestockRate; }
            }

            public DateTime LastRestock
            {
                get { return m_LastRestock; }
            }

            public int RestockAmnt
            {
                get { return m_RestockAmnt; }
            }

            public int Count
            {
                get { return m_Count; }
                set { m_Count = value; }
            }

            public int Maximum
            {
                get { return m_Maximum.GetValueOrDefault(-1); }
            }

            public int Hours
            {
                get { return m_RestockRate.Hours; }
            }

            public int Minutes
            {
                get { return m_RestockRate.Minutes; }
            }

            private RestockInfo()
            {
                m_RestockRate = new TimeSpan();
                m_RestockAmnt = 0;
                m_Count = 0;
                m_Maximum = null;
                m_LastRestock = DateTime.UtcNow;
            }

            public RestockInfo(int count)
                : this()
            {
                m_Count = count;
            }

            public RestockInfo(int hour, int minute, int count, int RestockNum)
            {
                m_RestockRate = new TimeSpan(hour, minute, 0);
                m_Count = count;
                m_RestockAmnt = RestockNum;
                m_Maximum = null;
                m_LastRestock = DateTime.UtcNow;
            }

            public RestockInfo(int hour, int minute, int count, int RestockNum, int maxNum)
                : this(hour, minute, count, RestockNum)
            {
                m_Maximum = maxNum;
            }

            public bool Equals(RestockInfo info)
            {
                return (this.m_RestockRate.Equals(info.RestockRate) && (this.m_RestockAmnt == info.RestockAmnt) &&
                        (this.m_Count == info.Count)
                        && (this.m_Maximum == info.Maximum));
            }

            public bool Restock()
            {
                m_LastRestock = DateTime.UtcNow;

                if (m_Maximum != null) //Restock to a limit
                {
                    if ((m_Count + m_RestockAmnt) > m_Maximum)
                    {
                        m_Count = m_Maximum.Value;
                        return false;
                    }

                    m_Count += m_RestockAmnt;
                    return true;
                }

                m_Count += m_RestockAmnt;
                return true;
            }

            public void Serialize(GenericWriter writer)
            {
                writer.Write(m_RestockRate);
                writer.Write(m_RestockAmnt);
                writer.Write(m_Count);
                writer.Write(m_Maximum.GetValueOrDefault(-1));
            }

            public RestockInfo(GenericReader reader)
            {
                m_RestockRate = reader.ReadTimeSpan();
                m_LastRestock = DateTime.UtcNow;
                m_RestockAmnt = reader.ReadInt();
                m_Count = reader.ReadInt();

                int? max = reader.ReadInt();

                m_Maximum = ((max < 0) ? null : max);
            }
        }

        public void Edit(int cost, string title, string desc)
        {
            if (cost < 0 || title == null || desc == null)
            {
                return;
            }

            m_Cost = cost;
            m_Title = title;
            m_Description = desc;
        }

        private Item GetReward()
        {
            return ItemClone.Clone(RewardInfo);
        }

        public Reward(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(m_BuyCount);

            if (m_RestockInfo == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                m_RestockInfo.Serialize(writer);
            }

            writer.WriteItem(RewardInfo);
            writer.Write(m_Cost);
            writer.Write(m_Title);
            writer.Write(m_Description);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_BuyCount = reader.ReadInt();

                        if (reader.ReadBool())
                        {
                            m_RestockInfo = new RestockInfo(reader);
                        }

                        goto case 0;
                    }
                case 0:
                    {
                        RewardInfo = reader.ReadItem();
                        m_Cost = reader.ReadInt();
                        m_Title = reader.ReadString();
                        m_Description = reader.ReadString();

                        break;
                    }
            }
        }
    }
}