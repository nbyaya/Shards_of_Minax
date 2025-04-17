using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles 
{ 
    public class SBCook : SBInfo 
    { 
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBCook() 
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
                Add(new GenericBuyInfo(typeof(BreadLoaf), 5, 20, 0x103B, 0, true));
                Add(new GenericBuyInfo(typeof(BreadLoaf), 5, 20, 0x103C, 0, true));
                Add(new GenericBuyInfo(typeof(ApplePie), 7, 20, 0x1041, 0, true)); //OSI just has Pie, not Apple/Fruit/Meat
                Add(new GenericBuyInfo(typeof(Cake), 13, 20, 0x9E9, 0, true));
                Add(new GenericBuyInfo(typeof(Muffins), 3, 20, 0x9EA, 0, true));

                Add(new GenericBuyInfo(typeof(CheeseWheel), 21, 10, 0x97E, 0, true));
                Add(new GenericBuyInfo(typeof(CookedBird), 17, 20, 0x9B7, 0, true));
                Add(new GenericBuyInfo(typeof(LambLeg), 8, 20, 0x160A, 0, true));
                Add(new GenericBuyInfo(typeof(ChickenLeg), 5, 20, 0x1608, 0, true));

                Add(new GenericBuyInfo(typeof(WoodenBowlOfCarrots), 3, 20, 0x15F9, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfCorn), 3, 20, 0x15FA, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfLettuce), 3, 20, 0x15FB, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfPeas), 3, 20, 0x15FC, 0));
                Add(new GenericBuyInfo(typeof(EmptyPewterBowl), 2, 20, 0x15FD, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfCorn), 3, 20, 0x15FE, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfLettuce), 3, 20, 0x15FF, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfPeas), 3, 20, 0x1601, 0));
                Add(new GenericBuyInfo(typeof(PewterBowlOfPotatos), 3, 20, 0x1601, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfStew), 3, 20, 0x1604, 0));
                Add(new GenericBuyInfo(typeof(WoodenBowlOfTomatoSoup), 3, 20, 0x1606, 0));

                Add(new GenericBuyInfo(typeof(RoastPig), 106, 20, 0x9BB, 0, true));
                Add(new GenericBuyInfo(typeof(SackFlour), 3, 20, 0x1039, 0, true));
                Add(new GenericBuyInfo(typeof(JarHoney), 3, 20, 0x9EC, 0, true));
                Add(new GenericBuyInfo(typeof(RollingPin), 2, 20, 0x1043, 0));
                Add(new GenericBuyInfo(typeof(FlourSifter), 2, 20, 0x103E, 0));
                Add(new GenericBuyInfo("1044567", typeof(Skillet), 3, 20, 0x97F, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo 
        { 
            public InternalSellInfo() 
            { 
                Add(typeof(CheeseWheel), 12);
                Add(typeof(CookedBird), 8);
                Add(typeof(RoastPig), 53);
                Add(typeof(Cake), 5);
                Add(typeof(JarHoney), 1);
                Add(typeof(SackFlour), 1);
                Add(typeof(BreadLoaf), 2);
                Add(typeof(ChickenLeg), 3);
                Add(typeof(LambLeg), 4);
                Add(typeof(Skillet), 1);
                Add(typeof(FlourSifter), 1);
                Add(typeof(RollingPin), 1);
                Add(typeof(Muffins), 1);
                Add(typeof(ApplePie), 3);

                Add(typeof(WoodenBowlOfCarrots), 1);
                Add(typeof(WoodenBowlOfCorn), 1);
                Add(typeof(WoodenBowlOfLettuce), 1);
                Add(typeof(WoodenBowlOfPeas), 1);
                Add(typeof(EmptyPewterBowl), 1);
                Add(typeof(PewterBowlOfCorn), 1);
                Add(typeof(PewterBowlOfLettuce), 1);
                Add(typeof(PewterBowlOfPeas), 1);
                Add(typeof(PewterBowlOfPotatos), 1);
                Add(typeof(WoodenBowlOfStew), 1);
                Add(typeof(WoodenBowlOfTomatoSoup), 1);
				
                Add(typeof(AbyssChivesFruit), 100);
                Add(typeof(aggearangFruit), 100);
                Add(typeof(agleatainFruit), 100);
                Add(typeof(ameoyoteFruit), 100);
                Add(typeof(AngelRootFruit), 100);
                Add(typeof(AngelTurnipFruit), 100);
                Add(typeof(ArcticParsnipFruit), 100);
                Add(typeof(AshLycheeFruit), 100);
                Add(typeof(AshRootFruit), 100);
                Add(typeof(AutumnCherryFruit), 100);
                Add(typeof(AutumnPomegranateFruit), 100);
                Add(typeof(BitterDurianFruit), 100);
                Add(typeof(BittersweetChivesFruit), 100);
                Add(typeof(BlackChoyFruit), 100);
                Add(typeof(blattiovesFruit), 100);
                Add(typeof(blearelFruit), 100);
                Add(typeof(BlumbFruit), 100);
                Add(typeof(bosheaShootFruit), 100);
                Add(typeof(brafulalFruit), 100);
                Add(typeof(brongerFruit), 100);
                Add(typeof(BushCerimanFruit), 100);
                Add(typeof(BushSpinachFruit), 100);
                Add(typeof(CandyMorindaFruit), 100);
                Add(typeof(CaveAsparagusFruit), 100);
                Add(typeof(CavePersimmonFruit), 100);
                Add(typeof(CavernMangosteenFruit), 100);
                Add(typeof(chigionutFruit), 100);
                Add(typeof(chummionachFruit), 100);
                Add(typeof(ciarryFruit), 100);
                Add(typeof(CinderGingerFruit), 100);
                Add(typeof(CliffNectarineFruit), 100);
                Add(typeof(crennealeryFruit), 100);
                Add(typeof(criarianFruit), 100);
                Add(typeof(darantFruit), 100);
                Add(typeof(DaydreamPommeracFruit), 100);
                Add(typeof(DesertPlumFruit), 100);
                Add(typeof(DesertRowanFruit), 100);
                Add(typeof(DessertBroccoliFruit), 100);
                Add(typeof(DessertTomatoFruit), 100);
                Add(typeof(DewKiwiFruit), 100);
                Add(typeof(DewPawpawFruit), 100);
                Add(typeof(dimquatFruit), 100);
                Add(typeof(diowanFruit), 100);
                Add(typeof(DragoLimeFruit), 100);
                Add(typeof(DrakeLentilFruit), 100);
                Add(typeof(DrakeMangoFruit), 100);
                Add(typeof(eacketFruit), 100);
                Add(typeof(eacotFruit), 100);
                Add(typeof(earolaFruit), 100);
                Add(typeof(EasternBacuriFruit), 100);
                Add(typeof(eawanFruit), 100);
                Add(typeof(echocadoFruit), 100);
                Add(typeof(ElephantBreadnutFruit), 100);
                Add(typeof(EmberLaurelFruit), 100);
                Add(typeof(EmberLettuceFruit), 100);
                Add(typeof(FalseAlmondFruit), 100);
                Add(typeof(fliavesFruit), 100);
                Add(typeof(FluxxFruit), 100);
                Add(typeof(fucrucotFruit), 100);
                Add(typeof(fudishFruit), 100);
                Add(typeof(fushewFruit), 100);
                Add(typeof(geweodineFruit), 100);
                Add(typeof(gigliachokeFruit), 100);
                Add(typeof(girinFruit), 100);
                Add(typeof(glissidillaFruit), 100);
                Add(typeof(GoldenRocketFruit), 100);
                Add(typeof(grandaFruit), 100);
                Add(typeof(gropioveFruit), 100);
                Add(typeof(GroundPearFruit), 100);
                Add(typeof(grutatoFruit), 100);
                Add(typeof(HairyTomatoFruit), 100);
                Add(typeof(HateCalamansiFruit), 100);
                Add(typeof(HazelLimeFruit), 100);
                Add(typeof(hialoupeFruit), 100);
                Add(typeof(hiorolaFruit), 100);
                Add(typeof(iaconaFruit), 100);
                Add(typeof(IceRocketFruit), 100);
                Add(typeof(iddiochokeFruit), 100);
                Add(typeof(imberFruit), 100);
                Add(typeof(ingeFruit), 100);
                Add(typeof(intineFruit), 100);
                Add(typeof(ippiocressFruit), 100);
                Add(typeof(ittianaFruit), 100);
                Add(typeof(jeorraFruit), 100);
                Add(typeof(jigreapawFruit), 100);
                Add(typeof(jochiniFruit), 100);
                Add(typeof(kledamiaFruit), 100);
                Add(typeof(kleopeFruit), 100);
                Add(typeof(kraccaleryFruit), 100);
                Add(typeof(krevaFruit), 100);
                Add(typeof(LillypillyFruit), 100);
                Add(typeof(LoveKumquatFruit), 100);
                Add(typeof(LoveZucchiniFruit), 100);
                Add(typeof(MageCherryFruit), 100);
                Add(typeof(MageDateFruit), 100);
                Add(typeof(MellowGourdFruit), 100);
                Add(typeof(MoonPumpkinFruit), 100);
                Add(typeof(moyiarlanFruit), 100);
                Add(typeof(MutantLemonFruit), 100);
                Add(typeof(MysteryFruit), 100);
                Add(typeof(MysteryGuavaFruit), 100);
                Add(typeof(MysteryOrangeFruit), 100);
                Add(typeof(NativeRambutanFruit), 100);
                Add(typeof(NightCabbageFruit), 100);
                Add(typeof(NightmareSaguaroFruit), 100);
                Add(typeof(ocanateFruit), 100);
                Add(typeof(OceanMuscadineFruit), 100);
                Add(typeof(omondFruit), 100);
                Add(typeof(otilFruit), 100);
                Add(typeof(PeaceAmaranthFruit), 100);
                Add(typeof(PeaceDateFruit), 100);
                Add(typeof(PeaceNectarineFruit), 100);
                Add(typeof(phecceayoteFruit), 100);
                Add(typeof(piokinFruit), 100);
                Add(typeof(probbacheeFruit), 100);
                Add(typeof(puchiniFruit), 100);
                Add(typeof(PygmyOrangeFruit), 100);
                Add(typeof(qekliatilloFruit), 100);
                Add(typeof(RainLaurelFruit), 100);
                Add(typeof(RainPommeracFruit), 100);
                Add(typeof(rephoneFruit), 100);
                Add(typeof(satilFruit), 100);
                Add(typeof(siheonachFruit), 100);
                Add(typeof(SilverFruit), 100);
                Add(typeof(slirindFruit), 100);
                Add(typeof(slomeloFruit), 100);
                Add(typeof(SmellyCarrotFruit), 100);
                Add(typeof(SourAmaranthFruit), 100);
                Add(typeof(StormBrambleFruit), 100);
                Add(typeof(striachiniFruit), 100);
                Add(typeof(strondaFruit), 100);
                Add(typeof(SwampNectarineFruit), 100);
                Add(typeof(SweetBoquilaFruit), 100);
                Add(typeof(TigerBeanFruit), 100);
                Add(typeof(TropicalCherryFruit), 100);
                Add(typeof(unaFruit), 100);
                Add(typeof(uyerdFruit), 100);
                Add(typeof(veapeFruit), 100);
                Add(typeof(VoidBrambleFruit), 100);
                Add(typeof(VoidOkraFruit), 100);
                Add(typeof(VoidPulasanFruit), 100);
                Add(typeof(VoidSproutFruit), 100);
                Add(typeof(vrecequilaFruit), 100);
                Add(typeof(vropperrotFruit), 100);
                Add(typeof(vuveFruit), 100);
                Add(typeof(WinterCoconutFruit), 100);
                Add(typeof(WonderRambutanFruit), 100);
                Add(typeof(wriggumondFruit), 100);
                Add(typeof(XeenBerryFruit), 100);
                Add(typeof(xekraFruit), 100);
                Add(typeof(xemeloFruit), 100);
                Add(typeof(zanioperFruit), 100);
                Add(typeof(ziongerFruit), 100);
            }
        }
    }
}