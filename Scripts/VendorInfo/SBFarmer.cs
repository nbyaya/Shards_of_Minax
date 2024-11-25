using System;
using Server;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.Plants;
using Server.Items.Crops;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
namespace Server.Mobiles 
{ 
    public class SBFarmer : SBInfo 
    { 
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBFarmer() 
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
                Add(new GenericBuyInfo("1031235", typeof(FreshGinger), 505, 10, 11235, 0));
                Add(new GenericBuyInfo(typeof(Cabbage), 5, 20, 0xC7B, 0, true));
                Add(new GenericBuyInfo(typeof(Cantaloupe), 6, 20, 0xC79, 0, true));
                Add(new GenericBuyInfo(typeof(Carrot), 3, 20, 0xC78, 0, true));
                Add(new GenericBuyInfo(typeof(HoneydewMelon), 7, 20, 0xC74, 0, true));
                Add(new GenericBuyInfo(typeof(Squash), 3, 20, 0xC72, 0, true));
                Add(new GenericBuyInfo(typeof(Lettuce), 5, 20, 0xC70, 0, true));
                Add(new GenericBuyInfo(typeof(Onion), 3, 20, 0xC6D, 0, true));
                Add(new GenericBuyInfo(typeof(Pumpkin), 11, 20, 0xC6A, 0, true));
                Add(new GenericBuyInfo(typeof(GreenGourd), 3, 20, 0xC66, 0, true));
                Add(new GenericBuyInfo(typeof(YellowGourd), 3, 20, 0xC64, 0, true));
                //Add( new GenericBuyInfo( typeof( Turnip ), 6, 20, XXXXXX, 0 ) );
                Add(new GenericBuyInfo(typeof(Watermelon), 7, 20, 0xC5C, 0, true));
                //Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
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
				Add(new GenericBuyInfo(typeof(BlackberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlackRaspberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlueberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CranberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PineappleSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedRaspberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlackRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DandelionSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(IrishRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PansySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PinkCarnationSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PoppySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnapdragonSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SpiritRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WhiteRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(YellowRoseSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CottonSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(FlaxSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(HaySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OatsSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RiceSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SugarcaneSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WheatSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GarlicSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TanGingerSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GinsengSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MandrakeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(NightshadeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedMushroomSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TanMushroomSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BitterHopsSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(ElvenHopsSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnowHopsSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SweetHopsSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CornSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(FieldCornSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SunFlowerSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TeaSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BananaSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CoconutSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DateSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAlmondSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAppleSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniApricotSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAvocadoSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCherrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCocoaSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCoffeeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniGrapefruitSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniKiwiSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniMangoSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniOrangeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPeachSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPearSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPistacioSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPomegranateSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SmallBananaSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(AsparagusSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BeetSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BroccoliSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CabbageSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CarrotSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CauliflowerSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CelerySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(EggplantSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenBeanSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LettuceSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OnionSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeanutSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeasSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PotatoSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RadishSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnowPeasSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SoySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SpinachSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(StrawberrySeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SweetPotatoSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TurnipSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(ChiliPepperSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CucumberSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenPepperSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OrangePepperSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedPepperSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TomatoSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(YellowPepperSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CantaloupeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenSquashSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(HoneydewMelonSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PumpkinSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SquashSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WatermelonSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(AppleSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BananaTreeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CantaloupeVineSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CoconutTreeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DateTreeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GrapeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LemonSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LimeSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeachSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PearSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SquashVineSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WatermelonVineSeed), 500, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BuildersHammer), 500, 10, 0x13E3, 0));
				Add(new GenericBuyInfo(typeof(CheeseAgingBarrel), 5000, 10, 0x0FAE, 0));
				Add(new GenericBuyInfo(typeof(PeaceableWoodenFence), 5000, 10, 0x085C, 0));
				Add(new GenericBuyInfo(typeof(MarijuanaSeeds), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TaintedSeeds), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SilverSaplingSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SeedOfLife), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SeedOfRenewal), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BeetleFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BirdFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BoarFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BullFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BullFrogFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(CatFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(CowFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DesertOstardFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DogFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DolphinFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(ForestOstardFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GamanFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GiantToadFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GoatFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GorillaFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GreatHartFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HindFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HiryuFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HorseFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(KirinFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(LlamaFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(OstardFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(PigFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RabbitFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RatFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RidgebackFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(SheepFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(SwampDragonFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(UnicornFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(WalrusFeeder), 1500, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RaisedGardenDeed), 10000, 10, 0x14F0, 0));
				Add(new GenericBuyInfo(typeof(RaisedGardenDeed), 10000, 10, 0x14F0, 0));
				Add(new GenericBuyInfo(typeof(SeedBox), 10000, 10, 0x4B58, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo 
        { 
            public InternalSellInfo() 
            { 
                Add(typeof(Pitcher), 5);
                Add(typeof(Eggs), 1);
                Add(typeof(Apple), 1);
                Add(typeof(Grapes), 1);
                Add(typeof(Watermelon), 3);
                Add(typeof(YellowGourd), 1);
                Add(typeof(GreenGourd), 1);
                Add(typeof(Pumpkin), 5);
                Add(typeof(Onion), 1);
                Add(typeof(Lettuce), 2);
                Add(typeof(Squash), 1);
                Add(typeof(Carrot), 1);
                Add(typeof(HoneydewMelon), 3);
                Add(typeof(Cantaloupe), 3);
                Add(typeof(Cabbage), 2);
                Add(typeof(Lemon), 1);
                Add(typeof(Lime), 1);
                Add(typeof(Peach), 1);
                Add(typeof(Pear), 1);
                Add(typeof(SheafOfHay), 1);
            }
        }
    }
}