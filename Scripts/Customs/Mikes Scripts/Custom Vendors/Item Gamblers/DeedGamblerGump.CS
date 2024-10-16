using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.ACC.CSS.Systems.Ancient;
using Server.Multis.Deeds;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Regions;

namespace Server.Custom.SpecialVendor
{
    public class DeedGamblerGump : Gump
    {
        private Mobile m_From;
        private List<Item> itemsList = new List<Item>
        {
            // Your items list...
			new LesserBladeTrapDeed(),
			new RegularBladeTrapDeed(),
			new GreaterBladeTrapDeed(),
			new DeadlyBladeTrapDeed(),
			new LesserExplosionTrapDeed(),
			new RegularExplosionTrapDeed(),
			new GreaterExplosionTrapDeed(),
			new DeadlyExplosionTrapDeed(),
			new LesserFireColumnTrapDeed(),
			new RegularFireColumnTrapDeed(),
			new GreaterFireColumnTrapDeed(),
			new DeadlyFireColumnTrapDeed(),
			new LesserPoisonTrapDeed(),
			new RegularPoisonTrapDeed(),
			new GreaterPoisonTrapDeed(),
			new DeadlyPoisonTrapDeed(),
			new PaintbrushItemAddonDeed(),
			new JediRoof1AddonDeed(),
			new JediRoof2AddonDeed(),
			new JediRoof3AddonDeed(),
			new JediRoof4AddonDeed(),
			new JediRoof5AddonDeed(),
			new JediRoof6AddonDeed(),
			new JediRoof7AddonDeed(),
			new JediRoof8AddonDeed(),
			new DolphinFloorMosaicEastAddonDeed(),
			new DolphinFloorMosaicSouthAddonDeed(),
			new Magincia10AddonDeed(),
			new Magincia11AddonDeed(),
			new Magincia12AddonDeed(),
			new Magincia13AddonDeed(),
			new Magincia14AddonDeed(),
			new Magincia1AddonDeed(),
			new Magincia2AddonDeed(),
			new Magincia3AddonDeed(),
			new Magincia4AddonDeed(),
			new Magincia5AddonDeed(),
			new Magincia6AddonDeed(),
			new Magincia7AddonDeed(),
			new Magincia8AddonDeed(),
			new Magincia9AddonDeed(),
			new PalmsCasinoAddonDeed(),
			new AsianCityBankNEWAddonDeed(),
			new AsianCityMarketNEWAddonDeed(),
			new AsianCityMoonGateNEWAddonDeed(),
			new AsianCityStableNEWAddonDeed(),
			new AsianCityTavernNEWAddonDeed(),
			new AsianCityTownHallNEWAddonDeed(),
			new City_Hall_SmallAddonDeed(),
			new FieldsStoneCityTavernNEWAddonDeed(),
			new FieldstoneCityBankNEWAddonDeed(),
			new FieldStoneCityHallNEWAddonDeed(),
			new FieldstoneCityHealerNEWAddonDeed(),
			new FieldstoneCityMarketNEWAddonDeed(),
			new FieldStoneCityStableNEWAddonDeed(),
			new FieldStoneCityTavernNEWAddonDeed(),
			new MarbleCityBankNEWAddonDeed(),
			new MarbleCityHallNEWAddonDeed(),
			new MarbleCityHealerNEWAddonDeed(),
			new MarbleCityMoongateNEWAddonDeed(),
			new MarbleCityStableNEWAddonDeed(),
			new MarbleCityTavernNEWAddonDeed(),
			new NecroCityBankNEWAddonDeed(),
			new NecroCityHallNEWAddonDeed(),
			new NecroCityHealerNEWAddonDeed(),
			new NecroCityMarketNEWAddonDeed(),
			new NecrocityMoonGateNEWAddonDeed(),
			new NecroCityStableNEWAddonDeed(),
			new NecrocityStablesNEWAddonDeed(),
			new NecrocityTavernNEWAddonDeed(),
			new PlasterCityBankNEWAddonDeed(),
			new PlasterCityHallNEWAddonDeed(),
			new PlasterCityTownHallNEWAddonDeed(),
			new SeaHuntEventArenaAddonDeed(),
			new TreasureHuntEventArenaAddonDeed(),
			new IlshMountainTownhouseEWAddonDeed(),
			new IlshMountainTownHouseNEWAddonDeed(),
			new TownhouseNS1AddonDeed(),
			new TownhouseNS2AddonDeed(),
			new TownhousesEast1AddonDeed(),
			new TownhouseWE1AddonDeed(),
			new TownhouseWE2AddonDeed(),
			new BannerEastAddonDeed(),
			new BannerSouthAddonDeed(),
			new BBbankrooftopAddonDeed(),
			new BBRooftopAddonDeed(),
			new CastleShopAddonDeed(),
			new ChuckleAtronBronzeAddonDeed(),
			new ChuckleAtronGoldAddonDeed(),
			new ChuckleAtronSilverAddonDeed(),
			new CrystalPylonAddonDeed(),
			new DupreToiletAddonDeed(),
			new Easter_Bunny_ContAddonDeed(),
			new firedungeonAddonDeed(),
			new AG_Fortress2AddonDeed(),
			new Garden_Shrubs_Large_L_ShapeAddonDeed(),
			new Gazebo1AddonDeed(),
			new GuardPost1AddonDeed(),
			new icedungeonaddonAddonDeed(),
			new icekingsdungeonAddonDeed(),
			new LargeArenaAddonDeed(),
			new LargeCannon1AddonDeed(),
			new LargeCannon2AddonDeed(),
			new LargeCannon3AddonDeed(),
			new LargeCannon4AddonDeed(),
			new LargeMerchantTower1AddonDeed(),
			new AG_LavaKeepAddonDeed(),
			new Luna_Library_Ruins_1AddonDeed(),
			new MageFortress1AddonDeed(),
			new MaginciaPVPArenaAddonDeed(),
			new MansionRuins1AddonDeed(),
			new MansionRuins2AddonDeed(),
			new MerchantHouse1AddonDeed(),
			new MontyRabbitAddonDeed(),
			new Mushroom_Lamp_Blue_v1AddonDeed(),
			new Mushroom_Lamp_Red_v1AddonDeed(),
			new Mushroom_Lamp_Green_v1AddonDeed(),
			new Mushroom_Lamp_Purple_v1AddonDeed(),
			new Mushroom_Lamp_Yellow_v1AddonDeed(),
			new Mushroom_Lamp_Blue_v2AddonDeed(),
			new Mushroom_Lamp_Red_v2AddonDeed(),
			new Mushroom_Lamp_Green_v2AddonDeed(),
			new Mushroom_Lamp_Purple_v2AddonDeed(),
			new Mushroom_Lamp_Yellow_v2AddonDeed(),
			new NewbieDungeon1AddonDeed(),
			new NewbieDungeon2AddonDeed(),
			new NoxiaDungeonAddonDeed(),
			new OldTower1AddonDeed(),
			new PaintballRoomAddonDeed(),
			new poisondungeonaddonAddonDeed(),
			new Poison_Patch_1AddonDeed(),
			new RainbowStableAddonDeed(),
			new RainbowTamer2AddonDeed(),
			new RainbowTamer3AddonDeed(),
			new RainbowTamerAddonDeed(),
			new RainbowZooAddonDeed(),
			new RewardHouse2AddonDeed(),
			new RewardHouseAddonDeed(),
			new RoseBrickWishingWellAddonDeed(),
			new Screenshot_Floor_SpaceAddonDeed(),
			new ShadowlordAltarHatredAddonDeed(),
			new SL_Hatred_AltarAddonDeed(),
			new Shed_Tiny_1AddonDeed(),
			new ShipWheel1AddonDeed(),
			new SkullballTrophyBronzeAddonDeed(),
			new SkullballTrophyGoldAddonDeed(),
			new SkullballTrophySilverAddonDeed(),
			new Ultimate_Sb_Bronze_StatAddonDeed(),
			new Ultimate_Sb_Gold_StatAddonDeed(),
			new Ultimate_Sb_Silver_StatAddonDeed(),
			new SmallFarm1AddonDeed(),
			new SmallFarmHouse2AddonDeed(),
			new SmallTower1AddonDeed(),
			new SoulForge1AddonDeed(),
			new SoulForge2AddonDeed(),
			new SoulForge3AddonDeed(),
			new SoulforgeAddonDeed(),
			new SpellcrafterFortressAddonDeed(),
			new SpellcrafterFortressBridgeAddonDeed(),
			new SpellcrafterFortressNewAddonDeed(),
			new TinyCannon1AddonDeed(),
			new TinyCannon2AddonDeed(),
			new TinyCannon3AddonDeed(),
			new TinyCannon4AddonDeed(),
			new TinyCannon5AddonDeed(),
			new TinyCannon6AddonDeed(),
			new TinyCannon7AddonDeed(),
			new TinyCannon8AddonDeed(),
			new Ultimate_SbAddonDeed(),
			new VendorTent1AddonDeed(),
			new VendorTent2AddonDeed(),
			new VendorTent3AddonDeed(),
			new Zag1AddonDeed(),
			new CozySandstoneFireplaceEastAddonDeed(),
			new CozySandstoneFireplaceSouthAddonDeed(),
			new EvilFireplaceEastFaceAddonDeed(),
			new EvilFireplaceSouthFaceAddonDeed(),
			new FancyStoneFireplaceEastAddonDeed(),
			new FancyStoneFireplaceSouthAddonDeed(),
			new HeXgraveyAddonDeed(),
			new hex_CornerFireplaceAddonDeed(),
			new hex_FirepEastAddonDeed(),
			new hex_fireplaceMPAddonDeed(),
			new hex_FirepSouthAddonDeed(),
			new hex_GatewayEastAddonDeed(),
			new hex_GatewaySouthAddonDeed(),
			new hex_HemppatchEastAddonDeed(),
			new hex_HemppatchSouthAddonDeed(),
			new hex_libraryStairsAddonDeed(),
			new hex_SmallFireplaceAddonDeed(),
			new hex_StoveEastAddonDeed(),
			new hex_StoveSouthAddonDeed(),
			new hex_WaterFallEastAddonDeed(),
			new hex_WaterfallPondAddonDeed(),
			new hex_WaterFallSouthAddonDeed(),
			new StoveBigEastAddonDeed(),
			new StoveBigSouthAddonDeed(),
			new MWDeskAddonDeed(),
			new MWCarAddonDeed(),
			new MWCouch1AddonDeed(),
			new MWCouch2AddonDeed(),
			new MWCouch3AddonDeed(),
			new MWCouch4AddonDeed(),
			new MWLongCouchAddonDeed(),
			new MWFireplace2AddonDeed(),
			new MWFireplaceAddonDeed(),
			new MWFishTankSAddonDeed(),
			new MWHouseGardenAddonDeed(),
			new MWKitchenAddonDeed(),
			new MWTVAddonDeed(),
			new SleeperCasketFacingNorthAddonDeed(),
			new SleeperCasketFacingWestAddonDeed(),
			new SleeperCoffinFacingNorthAddonDeed(),
			new SleeperCoffinFacingWestAddonDeed(),
			new SleeperElvenEWAddonDeed(),
			new SleeperElvenSouthDeed(),
			new SleeperEWAddonDeed(),
			new SleeperEWSpecialAddonDeed(),
			new SleeperEWUsedAddonDeed(),
			new SleeperFurEWAddonDeed(),
			new SleeperFurNSAddonDeed(),
			new SleeperFutonEWAddonDeed(),
			new SleeperFutonNSAddonDeed(),
			new SleeperNSAddonDeed(),
			new SleeperSarcophagusFacingNorthAddonDeed(),
			new SleeperSarcophagusFacingWestAddonDeed(),
			new SleeperSmallEWAddonDeed(),
			new SleeperSmallSouthDeed(),
			new SleeperTallElvenEastDeed(),
			new SleeperTallElvenSouthDeed(),
			new BlackWellAddonDeed(),
			new BrownWellAddonDeed(),
			new MarbleWellAddonDeed(),
			new RedWellAddonDeed(),
			new StoneWellAddonDeed(),
			new WoodWellAddonDeed(),
			new ZenGardenAddonDeed(),
			new BathTubEastAddonDeed(),
			new BathTubSouthAddonDeed(),
			new BigScreenTVEastAddonDeed(),
			new BigScreenTVSouthAddonDeed(),
			new FishtankBoatAddonDeed(),
			new FishTankDeed(),
			new FlowerBoxes1AddonDeed(),
			new Flowers1AddonDeed(),
			new Flowers2AddonDeed(),
			new Flowers3AddonDeed(),
			new Flowers4AddonDeed(),
			new Fountain1AddonDeed(),
			new GarlandPotterAddonDeed(),
			new GarlandsNorthAddonDeed(),
			new GarlandsSouthAddonDeed(),
			new HangingBasketAddonDeed(),
			new JacuzziEastAddonDeed(),
			new JacuzziSouthAddonDeed(),
			new LargeBed1AddonDeed(),
			new MiniCarouselDeed(),
			new PianoAddonDeed(),
			new RoseBush1AddonDeed(),
			new ShowerEastAddonDeed(),
			new ShowerSouthAddonDeed(),
			new UmbrellaTableDeed(),
			new BalancingDeed(),
			new AnimalControlDeed(),
			new VelocityDeed(),
			new ItemIDDeed1(),
			new ItemIDDeed10(),
			new ItemIDDeed11(),
			new ItemIDDeed12(),
			new ItemIDDeed13(),
			new ItemIDDeed14(),
			new ItemIDDeed15(),
			new ItemIDDeed16(),
			new ItemIDDeed17(),
			new ItemIDDeed18(),
			new ItemIDDeed19(),
			new ItemIDDeed2(),
			new ItemIDDeed20(),
			new ItemIDDeed21(),
			new ItemIDDeed22(),
			new ItemIDDeed23(),
			new ItemIDDeed24(),
			new ItemIDDeed25(),
			new ItemIDDeed26(),
			new ItemIDDeed3(),
			new ItemIDDeed4(),
			new ItemIDDeed5(),
			new ItemIDDeed6(),
			new ItemIDDeed7(),
			new ItemIDDeed8(),
			new ItemIDDeed9(),
			new ItemIDDeeDs(),
			new ItemIDDeed(),
			new NeeLandscapingAddonDeed(),
			new DisplayNorthMLAddonDeed(),
			new ChestSlotChangeDeed(),
			new LegsSlotChangeDeed(),
			new TalismanSlotChangeDeed(),
			new ArmSlotChangeDeed(),
			new BeltSlotChangeDeed(),
			new BraceletSlotChangeDeed(),
			new CapacityIncreaseDeed(),
			new EarringSlotChangeDeed(),
			new FootwearSlotChangeDeed(),
			new GenderChangeDeed(),
			new HeadSlotChangeDeed(),
			new LrcDeed(),
			new MurderRemovalDeed(),
			new NeckSlotChangeDeed(),
			new OneHandedTransformDeed(),
			new PetBondDeed(),
			new PetSlotDeed(),
			new RingSlotChangeDeed(),
			new ShirtSlotChangeDeed(),
			new StatCapDeed(),
			new SocketDeed(),
			new SocketDeed1(),
			new SocketDeed2(),
			new SocketDeed3(),
			new SocketDeed4(),
			new SocketDeed5(),
			new MisdeedsOfMischievousMimics(),
			new VirtueLegendaryDeeds(),
			new TentDeed(),
			new BlueCouchEastAddonDeed(),
			new BlueCouchNorthAddonDeed(),
			new BlueCouchSouthAddonDeed(),
			new BlueCouchWestAddonDeed(),
			new DrkBlueCornerChairAddonDeed(),
			new ChamnpSpawnPlatformAddonDeed(),
			new ChampAlterReplicaAddonDeed(),
			new ChampfiveAddonDeed(),
			new ChampPlatNewAddonDeed(),
			new ChampsixAddonDeed(),
			new MyChampPlatformAddonDeed(),
			new TimChampSpawnPlatAddonDeed(),
			new GreyDrkFPSouth2AddonDeed(),
			new GreyDrkFPSouth3AddonDeed(),
			new GreyDrkSouthFPAddonDeed(),
			new GreyGrandFPEastAddonDeed(),
			new GreyGrandFPSouthAddonDeed(),
			new GreyLargeFPEastAddonDeed(),
			new GreyLargeFPSouthAddonDeed(),
			new GreyLowFPEastAddonDeed(),
			new GreyLowFPSouthAddonDeed(),
			new GreyMedFPEastAddonDeed(),
			new GreyMedFPSouthAddonDeed(),
			new GreySmallFPEastAddonDeed(),
			new GreySmallFPSouthAddonDeed(),
			new AG_BathTubEastAddonDeed(),
			new AG_BathTubSouthAddonDeed(),
			new AG_JacuzziEastAddonDeed(),
			new AG_JacuzziSouthAddonDeed(),
			new AG_ShowerEastAddonDeed(),
			new AG_ShowerSouthAddonDeed(),
			new CathedralAddon2Deed(),
			new AG_RestAreaAddonDeed(),
			new AG_TheaterAddonDeed(),
			new AG_TheStableShaqAddonDeed(),
			new EventMazeAddonDeed(),
			new HalloweenHouseSpookyAddonDeed(),
			new SmallTreeVillageAddonDeed(),
			new maginciabakeryAddonDeed(),
			new maginciabankAddonDeed(),
			new maginciabathhouseAddonDeed(),
			new maginciachapelAddonDeed(),
			new maginciafishhouseAddonDeed(),
			new maginciagypsysAddonDeed(),
			new maginciainnAddonDeed(),
			new maginciajewelersAddonDeed(),
			new maginciajewelersouthAddonDeed(),
			new maginciamarbleAddonDeed(),
			new maginciaopeairmarketAddonDeed(),
			new maginciaparklandAddonDeed(),
			new maginciashipwrightAddonDeed(),
			new maginciastonecottageAddonDeed(),
			new maginciatailorAddonDeed(),
			new maginciatavernAddonDeed(),
			new maginciatinkersAddonDeed(),
			new FeastOakTableEastDeed(),
			new FeastOakTableSouthDeed(),
			new FeastTableEastDeed(),
			new FeastTableSouthDeed(),
			new GazeboNewwithbenchesAddonDeed(),
			new GraniteFurnessAddonDeed(),
			new TimSandFireAddonDeed(),
			new TimSandFireCornerAddonDeed(),
			new WellAddonDeed(),
			new ambulanceAddonDeed(),
			new BankWagonAddonDeed(),
			new BrokenWagon2AddonDeed(),
			new brokenwagonAddonDeed(),
			new DeathwagonAddonDeed(),
			new HayRideAddonDeed(),
			new LittleRedWagonAddonDeed(),
			new ShoppingCartAddonDeed(),
			new SmallRedWagonAddonDeed(),
			new WagonEastAddonDeed(),
			new WagonHauntedHouseAddonDeed(),
			new WagonJailAddonDeed(),
			new WagonNAddonDeed(),
			new WagonSouthAddonAddonDeed(),
			new WagonWAddonDeed(),
			new altergreyAddonDeed(),
			new AmericanFlagMeetingRoomAddonDeed(),
			new AG_BritBankAddonAddonDeed(),
			new ChuteAddonDeed(),
			new AG_WeaponShopAddonDeed(),
			new ArenaDeed(),
			new SawmillAddonDeed(),
			new AsianCarpetDeed(),
			new BlueCarpetDeed(),
			new FancyBlueCarpetDeed(),
			new FancyCarpetDeed(),
			new FancyRedCarpetDeed(),
			new PlainBlueCarpetDeed(),
			new RedCarpetDeed(),
			new BalronSlayerDeed(),
			new BloodDrinkingSlayerDeed(),
			new DaemonSlayerDeed(),
			new DragonSlayerDeed(),
			new EarthShatterSlayerDeed(),
			new ElementalHealthSlayerDeed(),
			new FlameDousingSlayerDeed(),
			new GargoyleSlayerDeed(),
			new LizardmanSlayerDeed(),
			new OgreSlayerDeed(),
			new OphidianSlayerDeed(),
			new OrcSlayerDeed(),
			new ScorpionSlayerDeed(),
			new SnakeSlayerDeed(),
			new SpidersDeathSlayerDeed(),
			new SummerWindSlayerDeed(),
			new TerathanSlayerDeed(),
			new TrollSlayerDeed(),
			new VacuumSlayerDeed(),
			new WaterDissipationSlayerDeed(),
			new ArachnidSlayerDeed(),
			new DemonSlayerDeed(),
			new ElementalSlayerDeed(),
			new FeySlayerDeed(),
			new RepondSlayerDeed(),
			new ReptileSlayerDeed(),
			new UndeadSlayerDeed(),
			new LesserSlayerDeed(),
			new SlayerRemovalDeed(),
			new SuperSlayerDeeds(),
			new ArtifactDeed(),
			new BonusDexDeed(),
			new BonusIntDeed(),
			new BonusHitsDeed(),
			new CandleToShieldDeed(),
			new LanternToShieldDeed(),
			new LowerRegCostDeed(),
			new LowerManaCostDeed(),
			new LuckDeed(),
			new LuckIncreaseDeed(),
			new NightSightDeed(),
			new RegenHitsDeed(),
			new RegenManaDeed(),
			new RegenStamDeed(),
			new TorchToShieldDeed(),
			new WeightlessDeed(),
			new AquariumEastDeed(),
			new AquariumNorthDeed(),
			new AlchemyStationDeed(),
			new BBQSmokerDeed(),
			new FletchingStationDeed(),
			new SewingMachineDeed(),
			new SmithingPressDeed(),
			new SpinningLatheDeed(),
			new WritingDeskDeed(),
			new AbbatoirDeed(),
			new AdvancedTrainingDummyEastDeed(),
			new AdvancedTrainingDummySouthDeed(),
			new AlchemistTableEastDeed(),
			new AlchemistTableSouthDeed(),
			new AnkhOfSacrificeDeed(),
			new AnvilEastDeed(),
			new AnvilSouthDeed(),
			new AppleTrunkDeed(),
			new ArcaneBookShelfDeedEast(),
			new ArcaneBookshelfEastDeed(),
			new ArcaneBookShelfDeedSouth(),
			new ArcaneBookshelfSouthDeed(),
			new ArcaneCircleDeed(),
			new ArcanistStatueEastDeed(),
			new ArcanistStatueSouthDeed(),
			new ArcheryButteDeed(),
			new AwesomeDisturbingPortraitDeed(),
			new BallotBoxDeed(),
			new BannerDeed(),
			new BrownBearRugEastDeed(),
			new BrownBearRugSouthDeed(),
			new PolarBearRugEastDeed(),
			new PolarBearRugSouthDeed(),
			new BedOfNailsDeed(),
			new BloodyPentagramDeed(),
			new BlueDecorativeRugDeed(),
			new BlueFancyRugDeed(),
			new BluePlainRugDeed(),
			new BoneCouchDeed(),
			new BoneTableDeed(),
			new BoneThroneDeed(),
			new RewardBrazierDeed(),
			new BrokenArmoireDeed(),
			new BrokenBedDeed(),
			new BrokenBookcaseDeed(),
			new BrokenChestOfDrawersDeed(),
			new BrokenCoveredChairDeed(),
			new BrokenFallenChairDeed(),
			new BrokenVanityDeed(),
			new CannonDeed(),
			new CherryBlossomTreeDeed(),
			new CherryBlossomTrunkDeed(),
			new CinnamonFancyRugDeed(),
			new MiniHouseDeed(),
			new ContestMiniHouseDeed(),
			new CreepyPortraitDeed(),
			new CurtainsDeed(),
			new DartBoardEastDeed(),
			new DartBoardSouthDeed(),
			new DecorativeShieldDeed(),
			new DistilleryEastAddonDeed(),
			new DistillerySouthAddonDeed(),
			new DisturbingPortraitDeed(),
			new DolphinRugAddonDeed(),
			new ElvenBedEastDeed(),
			new ElvenBedSouthDeed(),
			new ElvenDresserDeedEast(),
			new ElvenDresserEastDeed(),
			new ElvenDresserDeedSouth(),
			new ElvenDresserSouthDeed(),
			new ElvenForgeDeed(),
			new ElvenLoveseatEastDeed(),
			new ElvenLoveseatSouthDeed(),
			new ElvenSpinningwheelEastDeed(),
			new ElvenSpinningwheelSouthDeed(),
			new ElvenStoveEastDeed(),
			new ElvenStoveSouthDeed(),
			new ElvenWashBasinEastDeed(),
			new ElvenWashBasinEastWithDrawerDeed(),
			new ElvenWashBasinSouthDeed(),
			new ElvenWashBasinSouthWithDrawerDeed(),
			new FancyCouchEastDeed(),
			new FancyCouchNorthDeed(),
			new FancyCouchSouthDeed(),
			new FancyCouchWestDeed(),
			new FancyElvenTableEastDeed(),
			new FancyElvenTableSouthDeed(),
			new FancyLoveseatEastDeed(),
			new FancyLoveseatNorthDeed(),
			new FancyLoveseatSouthDeed(),
			new FancyLoveseatWestDeed(),
			new FancyWoodenShelfEastDeed(),
			new FancyWoodenShelfSouthDeed(),
			new FlamingHeadDeed(),
			new FlourMillEastDeed(),
			new FlourMillSouthDeed(),
			new LightFlowerTapestryEastDeed(),
			new LightFlowerTapestrySouthDeed(),
			new DarkFlowerTapestryEastDeed(),
			new DarkFlowerTapestrySouthDeed(),
			new FountainDeed(),
			new FountainOfLifeDeed(),
			new AppleTreeDeed(),
			new PeachTreeDeed(),
			new GardenShedDeed(),
			new GargishCotEastDeed(),
			new GargishCotSouthDeed(),
			new GargishCouchEastDeed(),
			new GargishCouchSouthDeed(),
			new GargishLongTableEastDeed(),
			new GargishLongTableSouthDeed(),
			new GingerBreadHouseDeed(),
			new goldcarpetAddonDeed(),
			new GoldenDecorativeRugDeed(),
			new GozaMatEastDeed(),
			new GozaMatSouthDeed(),
			new SquareGozaMatEastDeed(),
			new SquareGozaMatSouthDeed(),
			new BrocadeGozaMatEastDeed(),
			new BrocadeGozaMatSouthDeed(),
			new BrocadeSquareGozaMatEastDeed(),
			new BrocadeSquareGozaMatSouthDeed(),
			new GrayBrickFireplaceEastDeed(),
			new GrayBrickFireplaceSouthDeed(),
			new GuildstoneDeed(),
			new GuillotineDeed(),
			new HangingAxesDeed(),
			new HangingSkeletonDeed(),
			new HangingSwordsDeed(),
			new HauntedMirrorDeed(),
			new HearthOfHomeFireDeed(),
			new PottedPlantDeed(),
			new HouseLadderDeed(),
			new IronMaidenDeed(),
			new KoiPondDeed(),
			new LargeBedEastDeed(),
			new LargeBedSouthDeed(),
			new LargeFishingNetDeed(),
			new LargeForgeEastDeed(),
			new LargeForgeSouthDeed(),
			new LargeGargoyleBedEastDeed(),
			new LargeGargoyleBedSouthDeed(),
			new LargeStoneTableEastDeed(),
			new LargeStoneTableSouthDeed(),
			new LighthouseAddonDeed(),
			new LongMetalTableEastDeed(),
			new LongMetalTableSouthDeed(),
			new LongTableEastDeed(),
			new LongTableSouthDeed(),
			new LongWoodenTableEastDeed(),
			new LongWoodenTableSouthDeed(),
			new LoomEastDeed(),
			new LoomSouthDeed(),
			new MediumStoneTableEastDeed(),
			new MediumStoneTableSouthDeed(),
			new MedusaSNestAddonDeed(),
			new MetalTableEastDeed(),
			new MetalTableSouthDeed(),
			new MiningCartDeed(),
			new MiniSoulForgeDeed(),
			new MinotaurStatueDeed(),
			new MountedPixieBlueDeed(),
			new MountedPixieGreenDeed(),
			new MountedPixieLimeDeed(),
			new MountedPixieOrangeDeed(),
			new MountedPixieWhiteDeed(),
			new NexusAddonDeed(),
			new OrnateElvenChestEastDeed(),
			new OrnateElvenChestSouthDeed(),
			new OrnateElvenTableEastDeed(),
			new OrnateElvenTableSouthDeed(),
			new ParrotPerchAddonDeed(),
			new PeachTrunkDeed(),
			new PentagramDeed(),
			new PickpocketDipEastDeed(),
			new PickpocketDipSouthDeed(),
			new PinkFancyRugDeed(),
			new PlainWoodenShelfEastDeed(),
			new PlainWoodenShelfSouthDeed(),
			new PlantTapestryEastDeed(),
			new PlantTapestrySouthDeed(),
			new PlushLoveseatEastDeed(),
			new PlushLoveseatSouthDeed(),
			new PottedCactusDeed(),
			new RedPlainRugDeed(),
			new RitualTableDeed(),
			new RoseRugAddonDeed(),
			new RusticBenchEastDeed(),
			new RusticBenchSouthDeed(),
			new SacrificialAltarDeed(),
			new SandstoneFireplaceEastDeed(),
			new SandstoneFireplaceSouthDeed(),
			new ScarecrowDeed(),
			new SheepStatueDeed(),
			new SkullRugAddonDeed(),
			new SmallBedEastDeed(),
			new SmallBedSouthDeed(),
			new SmallDisplayCaseEastDeed(),
			new SmallDisplayCaseSouthDeed(),
			new SmallFishingNetDeed(),
			new SmallForgeDeed(),
			new SmallSoulForgeDeed(),
			new SoulForgeDeed(),
			new SpinningwheelEastDeed(),
			new SpinningwheelSouthDeed(),
			new SquirrelStatueEastDeed(),
			new SquirrelStatueSouthDeed(),
			new StandingBrokenChairDeed(),
			new StoneStatueDeed(),
			new StoneAnkhDeed(),
			new StoneAnvilEastDeed(),
			new StoneAnvilSouthDeed(),
			new StoneFireplaceEastDeed(),
			new StoneFireplaceSouthDeed(),
			new StoneOvenEastDeed(),
			new StoneOvenSouthDeed(),
			new SmallStretchedHideEastDeed(),
			new SmallStretchedHideSouthDeed(),
			new MediumStretchedHideEastDeed(),
			new MediumStretchedHideSouthDeed(),
			new SuitOfGoldArmorDeed(),
			new SuitOfSilverArmorDeed(),
			new TableWithBlueClothDeed(),
			new TableWithOrangeClothDeed(),
			new TableWithPurpleClothDeed(),
			new TableWithRedClothDeed(),
			new TallElvenBedEastDeed(),
			new TallElvenBedSouthDeed(),
			new TerMurDresserEastDeed(),
			new TerMurDresserSouthDeed(),
			new TrainingDummyEastDeed(),
			new TrainingDummySouthDeed(),
			new TreeStumpDeed(),
			new UnmadeBedDeed(),
			new UnsettlingPortraitDeed(),
			new VendorMallAddonDeed(),
			new WallBannerDeed(),
			new WallTorchDeed(),
			new WarriorStatueEastDeed(),
			new WarriorStatueSouthDeed(),
			new WaterTroughEastDeed(),
			new WaterTroughSouthDeed(),
			new WeatheredBronzeArcherDeed(),
			new WeatheredBronzeFairySculptureDeed(),
			new WeatheredBronzeGlobeSculptureDeed(),
			new WeatheredBronzeManOnABenchDeed(),
			new WoodenCoffinDeed(),
			new WoodenTableEastDeed(),
			new WoodenTableSouthDeed(),
			new WreathDeed(),
			new LordBritishThroneDeed(),
			new ClothingBlessDeed(),
			new CommodityDeed(),
			new DragonBardingDeed(),
			new GuildDeed(),
			new HairRestylingDeed(),
			new HolidayTreeDeed(),
			new HouseRaffleDeed(),
			new ItemBlessDeed(),
			new NameChangeDeed(),
			new PersonalAttendantDeed(),
			new PersonalBlessDeed(),
			new RepairDeed(),
			new ScrollBinderDeed(),
			new BoilingCauldronDeed(),
			new CommodityDeedBox(),
			new VanityDeed(),
			new GargoyleShortTableDeed(),
			new MistletoeDeed(),
			new SnowStatueDeed(),
			new UpholsteredChairDeed(),
			new CelloDeed(),
			new CowBellDeed(),
			new TrumpetDeed(),
			new WallMountedBellSouthDeed(),
			new WallMountedBellEastDeed(),
			new CraftableHouseAddonDeed(),
			new CraftableHouseDoorDeed(),
			new WallSafeDeed(),
			new DecorativeBlackwidowDeed(),
			new FallenLogDeed(),
			new HildebrandtDragonRugDeed(),
			new MapleTreeDeed(),
			new SnowTreeDeed(),
			new WillowTreeDeed(),
			new WoodworkersBenchDeed(),
			new SmallWorldTreeRugAddonDeed(),
			new LargeWorldTreeRugAddonDeed(),
			new LargeBoatDeed(),
			new LargeDragonBoatDeed(),
			new MediumBoatDeed(),
			new MediumDragonBoatDeed(),
			new SmallBoatDeed(),
			new SmallDragonBoatDeed(),
			new StonePlasterHouseDeed(),
			new FieldStoneHouseDeed(),
			new SmallBrickHouseDeed(),
			new WoodHouseDeed(),
			new WoodPlasterHouseDeed(),
			new ThatchedRoofCottageDeed(),
			new BrickHouseDeed(),
			new TwoStoryWoodPlasterHouseDeed(),
			new TwoStoryStonePlasterHouseDeed(),
			new TowerDeed(),
			new KeepDeed(),
			new CastleDeed(),
			new LargePatioDeed(),
			new LargeMarbleDeed(),
			new SmallTowerDeed(),
			new LogCabinDeed(),
			new SandstonePatioDeed(),
			new VillaDeed(),
			new StoneWorkshopDeed(),
			new MarbleWorkshopDeed(),
			new RawMoonstoneLargeAddonDeed(),
			new RawMoonstoneSmallAddonDeed(),
			new TigerRugAddonDeed(),
			new BananaHoardAddonDeed(),
			new DinosaurHunterRewardTitleDeed(),
			new DragonTurtleFountainAddonDeed(),
			new PlumTreeAddonDeed(),
			new ChaosTileDeed(),
			new CompassionVirtueTileDeed(),
			new DragonHeadAddonDeed(),
			new DragonHeadDeedOld(),
			new FirePitDeed(),
			new HonestyVirtueTileDeed(),
			new HonorVirtueTileDeed(),
			new HorseBardingDeed(),
			new HumilityVirtueTileDeed(),
			new JusticeVirtueTileDeed(),
			new SacrificeVirtueTileDeed(),
			new SpiritualityVirtueTileDeed(),
			new StewardDeed(),
			new ValorVirtueTileDeed(),
			new EnchantedBladeDeed(),
			new EnchantedVortexDeed(),
			new LightShipCannonDeed(),
			new HeavyShipCannonDeed(),
			new BritannianShipDeed(),
			new GargishGalleonDeed(),
			new OrcishGalleonDeed(),
			new RowBoatDeed(),
			new TokunoGalleonDeed(),
			new DefenderOfEodonTitleDeed(),
			new DefenderOfTheMyrmidexTitleDeed(),
			new FlameOfTheJukariTitleDeed(),
			new AmbusherOfTheKurakTitleDeed(),
			new TrooperOfTheBarakoTitleDeed(),
			new ThunderOfTheUraliTitleDeed(),
			new HerderOfTheSakkhraTitleDeed(),
			new ColonizerOfTheBarrabTitleDeed(),
			new tent_brownAddonDeed(),
			new tent_whiteAddonDeed(),
			new HuntmastersRewardTitleDeed(),
			new EthologistTitleDeed(),
			new RaisedGardenDeed(),
			new DecorativeShardShieldDeed(),
			new TemporaryForgeDeed(),
			new ProphetTitleDeed(),
			new SeekerOfTheFallenStarTitleDeed(),
			new ZealotOfKhalAnkurTitleDeed(),
			new TortureRackEastDeed(),
			new TortureRackSouthDeed(),
			new SkeletalHangmanAddonDeed(),
			new KotlSacraficialAltarAddonDeed(),
			new RewardScrollDeed(),
			new LevelItemDeed(),
			new SiegeCannonDeed(),
			new SiegeCatapultDeed(),
			new SiegeRamDeed(),
			new MannequinDeed(),
        };

        private List<Item> currentRandomItems = new List<Item>();
        private int[] currentRandomPrices = new int[9];

        public DeedGamblerGump(Mobile from) : base(0, 0)
        {
            m_From = from;

            RandomizeItemsAndPrices();

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            // Adjust the background size to accommodate increased spacing between items

            AddImageTiled(10, 10, 400, 200, 2624);
            AddImageTiled(10, 40, 400, 200, 2624);
            AddImageTiled(10, 50, 400, 200, 2624);
            AddImageTiled(10, 60, 400, 200, 2624);
            AddImageTiled(10, 70, 400, 200, 2624);
            AddImageTiled(10, 80, 400, 200, 2624);
            AddImageTiled(10, 100, 400, 200, 2624);
            AddImageTiled(10, 130, 400, 200, 2624);
            AddImageTiled(10, 150, 400, 200, 2624);
            AddImageTiled(10, 250, 400, 200, 2624);
            AddImageTiled(10, 450, 400, 200, 2624);

            AddImageTiled(40, 10, 400, 200, 2624);
            AddImageTiled(40, 40, 400, 200, 2624);
            AddImageTiled(40, 50, 400, 200, 2624);
            AddImageTiled(40, 60, 400, 200, 2624);
            AddImageTiled(40, 70, 400, 200, 2624);
            AddImageTiled(40, 80, 400, 200, 2624);
            AddImageTiled(40, 100, 400, 200, 2624);
            AddImageTiled(40, 130, 400, 200, 2624);
            AddImageTiled(40, 150, 400, 200, 2624);
            AddImageTiled(40, 250, 400, 200, 2624);
            AddImageTiled(40, 450, 400, 200, 2624);

            AddImageTiled(80, 10, 400, 200, 2624);
            AddImageTiled(80, 40, 400, 200, 2624);
            AddImageTiled(80, 50, 400, 200, 2624);
            AddImageTiled(80, 60, 400, 200, 2624);
            AddImageTiled(80, 70, 400, 200, 2624);
            AddImageTiled(80, 80, 400, 200, 2624);
            AddImageTiled(80, 100, 400, 200, 2624);
            AddImageTiled(80, 130, 400, 200, 2624);
            AddImageTiled(80, 150, 400, 200, 2624);
            AddImageTiled(80, 250, 400, 200, 2624);
            AddImageTiled(80, 450, 400, 200, 2624);

            AddImageTiled(200, 10, 400, 200, 2624);
            AddImageTiled(200, 40, 400, 200, 2624);
            AddImageTiled(200, 50, 400, 200, 2624);
            AddImageTiled(200, 60, 400, 200, 2624);
            AddImageTiled(200, 70, 400, 200, 2624);
            AddImageTiled(200, 80, 400, 200, 2624);
            AddImageTiled(200, 100, 400, 200, 2624);
            AddImageTiled(200, 130, 400, 200, 2624);
            AddImageTiled(200, 150, 400, 200, 2624);
            AddImageTiled(200, 250, 400, 200, 2624);
            AddImageTiled(200, 450, 400, 200, 2624);

            AddImageTiled(250, 10, 400, 200, 2624);
            AddImageTiled(250, 40, 400, 200, 2624);
            AddImageTiled(250, 50, 400, 200, 2624);
            AddImageTiled(250, 60, 400, 200, 2624);
            AddImageTiled(250, 70, 400, 200, 2624);
            AddImageTiled(250, 80, 400, 200, 2624);
            AddImageTiled(250, 100, 400, 200, 2624);
            AddImageTiled(250, 130, 400, 200, 2624);
            AddImageTiled(250, 150, 400, 200, 2624);
            AddImageTiled(250, 250, 400, 200, 2624);
            AddImageTiled(250, 450, 400, 200, 2624);

            AddImageTiled(290, 10, 400, 200, 2624);
            AddImageTiled(290, 40, 400, 200, 2624);
            AddImageTiled(290, 50, 400, 200, 2624);
            AddImageTiled(290, 60, 400, 200, 2624);
            AddImageTiled(290, 70, 400, 200, 2624);
            AddImageTiled(290, 80, 400, 200, 2624);
            AddImageTiled(290, 100, 400, 200, 2624);
            AddImageTiled(290, 130, 400, 200, 2624);
            AddImageTiled(290, 150, 400, 200, 2624);
            AddImageTiled(290, 250, 400, 200, 2624);
            AddImageTiled(290, 450, 400, 200, 2624);

            AddLabel(75, 25, 1152, "Special Vendor Offers");

            // Adjusted loop for increased spacing
            for (int i = 0; i < currentRandomItems.Count; i++)
            {
                // Adjust the positions for better spacing
                int x = 75 + (i % 3) * 200; // Increased spacing on X-axis
                int y = 75 + (i / 3) * 150; // Increased spacing on Y-axis

                AddLabel(x, y, 1153, currentRandomItems[i].Name);
                AddLabel(x, y + 30, 1153, "Price: " + currentRandomPrices[i].ToString() + "gp");
                // Adjust the button and item positions based on new spacing
                AddButton(x + 130, y + 20, 4023, 4023, i + 1, GumpButtonType.Reply, 0); // Buy button for each item
                AddItem(x + 60, y + 60, currentRandomItems[i].ItemID); // Adjust for visual clarity
            }

            // Adjust the reroll button position based on the new layout
            AddButton(550, 550, 4020, 4020, 10, GumpButtonType.Reply, 0); // Reroll button
        }

        private void RandomizeItemsAndPrices()
        {
            Random rand = new Random();
            currentRandomItems.Clear();

            while (currentRandomItems.Count < 9)
            {
                Item potentialItem = itemsList[rand.Next(itemsList.Count)];
                if (!currentRandomItems.Contains(potentialItem))
                {
                    currentRandomItems.Add(potentialItem);
                    currentRandomPrices[currentRandomItems.Count - 1] = rand.Next(500, 40000); // Random price for each item
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID >= 1 && info.ButtonID <= 9)
            {
                int itemIndex = info.ButtonID - 1;
                int price = currentRandomPrices[itemIndex];

                if (from.Backpack.ConsumeTotal(typeof(Gold), price))
                {
                    Item item = (Item)Activator.CreateInstance(currentRandomItems[itemIndex].GetType());
                    from.Backpack.DropItem(item);
                    from.SendMessage("You have bought a " + currentRandomItems[itemIndex].Name + ".");
                }
                else
                {
                    int totalGold = from.Backpack.GetAmount(typeof(Gold)) + Banker.GetBalance(from);

                    if (totalGold >= price)
                    {
                        int backpackGold = from.Backpack.GetAmount(typeof(Gold));
                        int bankGold = price - backpackGold;

                        if (backpackGold > 0)
                        {
                            from.Backpack.ConsumeTotal(typeof(Gold), backpackGold);
                        }

                        if (bankGold > 0)
                        {
                            Banker.Withdraw(from, bankGold);
                        }

                        Item item = (Item)Activator.CreateInstance(currentRandomItems[itemIndex].GetType());
                        from.Backpack.DropItem(item);
                        from.SendMessage("You have bought a " + currentRandomItems[itemIndex].Name + ".");
                    }
                    else
                    {
                        from.SendMessage("You do not have enough gold.");
                    }
                }
            }
            else if (info.ButtonID == 10) // Reroll button
            {
                int rerollCost = 5000;

                if (from.Backpack.ConsumeTotal(typeof(Gold), rerollCost))
                {
                    RandomizeItemsAndPrices();
                    from.SendGump(new DeedGamblerGump(from));
                    from.SendMessage("The items have been rerolled.");
                }
                else
                {
                    int totalGold = from.Backpack.GetAmount(typeof(Gold)) + Banker.GetBalance(from);

                    if (totalGold >= rerollCost)
                    {
                        int backpackGold = from.Backpack.GetAmount(typeof(Gold));
                        int bankGold = rerollCost - backpackGold;

                        if (backpackGold > 0)
                        {
                            from.Backpack.ConsumeTotal(typeof(Gold), backpackGold);
                        }

                        if (bankGold > 0)
                        {
                            Banker.Withdraw(from, bankGold);
                        }

                        RandomizeItemsAndPrices();
                        from.SendGump(new DeedGamblerGump(from));
                        from.SendMessage("The items have been rerolled.");
                    }
                    else
                    {
                        from.SendMessage("You do not have 5000 gold to reroll.");
                    }
                }
            }
        }
    }
}
