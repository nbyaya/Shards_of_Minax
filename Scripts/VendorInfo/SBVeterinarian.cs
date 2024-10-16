using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBVeterinarian : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBVeterinarian()
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
                Add(new GenericBuyInfo(typeof(Bandage), 6, 20, 0xE21, 0, true));
				Add(new GenericBuyInfo(typeof(PetLeash), 100000, 10, 0x1374, 0));
				Add(new GenericBuyInfo(typeof(HarvestersBlade), 5000, 10, 0x2D20, 0));
				Add(new GenericBuyInfo(typeof(BanishingOrb), 500, 10, 0xE2E, 0));
				Add(new GenericBuyInfo(typeof(BanishingRod), 100000, 10, 0xE81, 0));
				Add(new GenericBuyInfo(typeof(Pokeball), 5000, 10, 0x1870, 0));
				Add(new GenericBuyInfo(typeof(MasterBall), 100000, 10, 0x1870, 0));
                Add(new AnimalBuyInfo(1, typeof(PackHorse), 616, 10, 291, 0));
                Add(new AnimalBuyInfo(1, typeof(PackLlama), 523, 10, 292, 0));
                Add(new AnimalBuyInfo(1, typeof(Dog), 158, 10, 217, 0));
                Add(new AnimalBuyInfo(1, typeof(Cat), 131, 10, 201, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Bandage), 1);
            }
        }
    }
}