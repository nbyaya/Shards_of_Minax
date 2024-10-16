using Server.FarmingSystem;
using System; 
using System.Collections.Generic; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBCropmaster : SBInfo 
	{ 
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBCropmaster() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
               { 

                Add(new GenericBuyInfo(typeof(FarmingSeed), 2, 80, 3535, 0, true));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Water, 1, 50, 0x9AD, 0));
                Add(new GenericBuyInfo(typeof(Eggs), 3, 20, 0x9B5, 0, true));
                Add(new BeverageBuyInfo(typeof(Pitcher), BeverageType.Milk, 7, 20, 0x9AD, 0));
                Add(new GenericBuyInfo(typeof(Peach), 3, 20, 0x9D2, 0, true));
                Add(new GenericBuyInfo(typeof(Pear), 3, 20, 0x994, 0, true));
                Add(new GenericBuyInfo(typeof(Lemon), 3, 20, 0x1728, 0, true));
                Add(new GenericBuyInfo(typeof(Lime), 3, 20, 0x172A, 0, true));
                Add(new GenericBuyInfo(typeof(Grapes), 3, 20, 0x9D1, 0, true));
                Add(new GenericBuyInfo(typeof(Apple), 3, 20, 0x9D0, 0, true));
                Add(new GenericBuyInfo(typeof(SheafOfHay), 2, 20, 0xF36, 0));
                Add(new GenericBuyInfo(typeof(Hoe), 5, 20, 3897, 0));
                Add(new GenericBuyInfo(typeof(CropSextant), 800, 30, 0X1058, 0));
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
            
                Add(typeof(Watermelon), 480);
                Add(typeof(Pumpkin), 510);
                Add(typeof(Onion), 470);
                Add(typeof(Lettuce), 696);
                Add(typeof(Squash), 540);
                Add(typeof(Carrot), 470);
                Add(typeof(Cabbage), 610);
                Add(typeof(Tobacco), 820);
          
            
              
			} 
		} 
	} 
}