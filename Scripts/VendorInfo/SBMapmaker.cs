using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBMapmaker : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBMapmaker()
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
                Add(new GenericBuyInfo(typeof(BlankMap), 5, 40, 0x14EC, 0));
                Add(new GenericBuyInfo(typeof(MapmakersPen), 8, 20, 0x0FBF, 0));
                Add(new GenericBuyInfo(typeof(BlankScroll), 12, 40, 0xEF3, 0));
				Add(new GenericBuyInfo(typeof(TeleportToTokuno), 5000, 40, 0x1F14, 0));
				Add(new GenericBuyInfo(typeof(TeleportToMalasItem), 5000, 40, 0x1ECD, 0));
				Add(new GenericBuyInfo(typeof(TeleportToIlshenarItem), 5000, 40, 0x1F14, 0));
				
                for (int i = 0; i < PresetMapEntry.Table.Length; ++i)
                    Add(new PresetMapBuyInfo(PresetMapEntry.Table[i], Utility.RandomMinMax(7, 10), 20));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(BlankScroll), 6);
                Add(typeof(MapmakersPen), 4);
                Add(typeof(BlankMap), 2);
                Add(typeof(CityMap), 3);
                Add(typeof(LocalMap), 3);
                Add(typeof(WorldMap), 3);
                Add(typeof(PresetMapEntry), 3);
                //TODO: Buy back maps that the mapmaker sells!!!
            }
        }
    }
}