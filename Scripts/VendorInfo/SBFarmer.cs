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
				Add(new GenericBuyInfo(typeof(BlackberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlackRaspberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlueberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CranberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PineappleSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedRaspberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BlackRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DandelionSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(IrishRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PansySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PinkCarnationSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PoppySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnapdragonSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SpiritRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WhiteRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(YellowRoseSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CottonSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(FlaxSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(HaySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OatsSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RiceSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SugarcaneSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WheatSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GarlicSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TanGingerSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GinsengSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MandrakeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(NightshadeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedMushroomSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TanMushroomSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BitterHopsSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(ElvenHopsSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnowHopsSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SweetHopsSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CornSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(FieldCornSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SunFlowerSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TeaSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BananaSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CoconutSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DateSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAlmondSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAppleSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniApricotSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniAvocadoSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCherrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCocoaSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniCoffeeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniGrapefruitSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniKiwiSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniMangoSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniOrangeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPeachSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPearSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPistacioSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(MiniPomegranateSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SmallBananaSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(AsparagusSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BeetSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BroccoliSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CabbageSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CarrotSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CauliflowerSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CelerySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(EggplantSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenBeanSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LettuceSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OnionSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeanutSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeasSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PotatoSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RadishSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SnowPeasSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SoySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SpinachSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(StrawberrySeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SweetPotatoSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TurnipSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(ChiliPepperSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CucumberSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenPepperSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(OrangePepperSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(RedPepperSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TomatoSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(YellowPepperSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CantaloupeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GreenSquashSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(HoneydewMelonSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PumpkinSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SquashSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WatermelonSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(AppleSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BananaTreeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CantaloupeVineSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(CoconutTreeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(DateTreeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(GrapeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LemonSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(LimeSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PeachSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(PearSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SquashVineSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(WatermelonVineSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BuildersHammer), 5000, 10, 0x13E3, 0));
				Add(new GenericBuyInfo(typeof(CheeseAgingBarrel), 5000, 10, 0x0FAE, 0));
				Add(new GenericBuyInfo(typeof(PeaceableWoodenFence), 5000, 10, 0x085C, 0));
				Add(new GenericBuyInfo(typeof(MarijuanaSeeds), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(TaintedSeeds), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SilverSaplingSeed), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SeedOfLife), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(SeedOfRenewal), 5000, 10, 0xF27, 0));
				Add(new GenericBuyInfo(typeof(BeetleFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BirdFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BoarFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BullFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(BullFrogFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(CatFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(CowFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DesertOstardFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DogFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(DolphinFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(ForestOstardFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GamanFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GiantToadFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GoatFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GorillaFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(GreatHartFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HindFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HiryuFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(HorseFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(KirinFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(LlamaFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(OstardFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(PigFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RabbitFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RatFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(RidgebackFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(SheepFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(SwampDragonFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(UnicornFeeder), 15000, 10, 0x0E83, 0));
				Add(new GenericBuyInfo(typeof(WalrusFeeder), 15000, 10, 0x0E83, 0));
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