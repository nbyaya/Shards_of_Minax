using System;
using System.Collections.Generic;
using Server.Items;
using Server.Multis.Deeds;

namespace Server.Mobiles
{
    public class SBRealEstateBroker : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBRealEstateBroker()
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
                Add(new GenericBuyInfo(typeof(BlankScroll), 5, 20, 0x0E34, 0));
                Add(new GenericBuyInfo(typeof(ScribesPen), 8, 20, 0xFBF, 0));
                Add(new GenericBuyInfo(typeof(StonePlasterHouseDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(FieldStoneHouseDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(SmallBrickHouseDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(WoodHouseDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(WoodPlasterHouseDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(ThatchedRoofCottageDeed ), 43800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(BrickHouseDeed ), 144500, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(TwoStoryWoodPlasterHouseDeed ), 192400, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(TowerDeed ), 433200, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(KeepDeed ), 665200, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(CastleDeed ), 1022800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(LargePatioDeed ), 152800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(LargeMarbleDeed ), 192800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(SmallTowerDeed ), 88500, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(LogCabinDeed ), 97800, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(SandstonePatioDeed ), 90900, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(VillaDeed ), 136500, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(StoneWorkshopDeed ), 60600, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(MarbleWorkshopDeed ), 60300, 20, 0x14EF, 0));
                Add(new GenericBuyInfo(typeof(SmallBrickHouseDeed ), 43800, 20, 0x14EF, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(ScribesPen), 4);
                Add(typeof(BlankScroll), 2);
                Add( typeof( StonePlasterHouseDeed ), 43800 );
                Add( typeof( FieldStoneHouseDeed ), 43800 );
                Add( typeof( SmallBrickHouseDeed ), 43800 );
                Add( typeof( WoodHouseDeed ), 43800 );
                Add( typeof( WoodPlasterHouseDeed ), 43800 );
                Add( typeof( ThatchedRoofCottageDeed ), 43800 );
                Add( typeof( BrickHouseDeed ), 144500 );
                Add( typeof( TwoStoryWoodPlasterHouseDeed ), 192400 );
                Add( typeof( TowerDeed ), 433200 );
                Add( typeof( KeepDeed ), 665200 );
                Add( typeof( CastleDeed ), 1022800 );
                Add( typeof( LargePatioDeed ), 152800 );
                Add( typeof( LargeMarbleDeed ), 192800 );
                Add( typeof( SmallTowerDeed ), 88500 );
                Add( typeof( LogCabinDeed ), 97800 );
                Add( typeof( SandstonePatioDeed ), 90900 );
                Add( typeof( VillaDeed ), 136500 );
                Add( typeof( StoneWorkshopDeed ), 60600 );
                Add( typeof( MarbleWorkshopDeed ), 60300 );
                Add( typeof( SmallBrickHouseDeed ), 43800 );
            }
        }
    }
}