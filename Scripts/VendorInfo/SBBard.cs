using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
    public class SBBard : SBInfo 
    { 
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBBard() 
        { 
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo> 
        { 
            public InternalBuyInfo() 
            { 
                Add(new GenericBuyInfo(typeof(Drums), 21, (10), 0x0E9C, 0)); 
                Add(new GenericBuyInfo(typeof(Tambourine), 21, (10), 0x0E9E, 0)); 
                Add(new GenericBuyInfo(typeof(LapHarp), 21, (10), 0x0EB2, 0)); 
                Add(new GenericBuyInfo(typeof(Lute), 21, (10), 0x0EB3, 0)); 
				Add(new GenericBuyInfo(typeof(RodOfOrcControl), 5000, (10), 0xE81, 0)); 
            }
        }

        public class InternalSellInfo : GenericSellInfo 
        { 
            public InternalSellInfo() 
            { 
                Add(typeof(LapHarp), 10); 
                Add(typeof(Lute), 10); 
                Add(typeof(Drums), 10); 
                Add(typeof(Harp), 10); 
                Add(typeof(Tambourine), 10); 
            }
        }
    }
}