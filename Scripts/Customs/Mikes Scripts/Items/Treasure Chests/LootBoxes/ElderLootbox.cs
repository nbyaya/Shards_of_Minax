using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Regions;
using Server.Multis.Deeds;

namespace Server.Custom
{
    public class ElderLootbox : LockableContainer
    {
        [Constructable]
        public ElderLootbox() : base(0xE41) // Treasure Chest item ID
        {
            Name = "Elder Lootbox";
            Hue = Utility.RandomMinMax(1600, 2900);


            // Add Maps
			AddItemWithProbability(new AbbadonTheAbyssalMap(), 0.003);
			AddItemWithProbability(new AbyssalWardenMap(), 0.003);
			AddItemWithProbability(new AbyssinianTrackerMap(), 0.003);
			AddItemWithProbability(new AcidicAlligatorMap(), 0.003);
			AddItemWithProbability(new AlchemicalLabMap(), 0.003);
			AddItemWithProbability(new AncientAlligatorMap(), 0.003);
			AddItemWithProbability(new AncientDragonsLairMap(), 0.003);
			AddItemWithProbability(new AngusBerserkersCampMap(), 0.003);
			AddItemWithProbability(new BloodstainedFieldsMap(), 0.003);
			AddItemWithProbability(new CorruptedOrchardMap(), 0.003);
			AddItemWithProbability(new DogTheBountyHuntersDenMap(), 0.003);
			AddItemWithProbability(new DuelistPoetArenaMap(), 0.003);
			AddItemWithProbability(new EarthClanNinjaLairMap(), 0.003);
			AddItemWithProbability(new EarthClanSamuraiEncampmentMap(), 0.003);
			AddItemWithProbability(new EvilAlchemistLaboratoryMap(), 0.003);
			AddItemWithProbability(new EvilClownCarnivalMap(), 0.003);
			AddItemWithProbability(new FairyQueenGladeMap(), 0.003);
			AddItemWithProbability(new FastExplorerExpeditionMap(), 0.003);
			AddItemWithProbability(new FireClanNinjaHideoutMap(), 0.003);
			AddItemWithProbability(new FireClanSamuraiDojoMap(), 0.003);
			AddItemWithProbability(new FlapperElementalistAltarMap(), 0.003);
			AddItemWithProbability(new FloridaMansCarnivalMap(), 0.003);
			AddItemWithProbability(new ForestRangerOutpostMap(), 0.003);
			AddItemWithProbability(new FunkFungiFamiliarGardenMap(), 0.003);
			AddItemWithProbability(new GangLeadersHideoutMap(), 0.003);
			AddItemWithProbability(new GlamRockMageConcertMap(), 0.003);
			AddItemWithProbability(new GothicNovelistCryptMap(), 0.003);
			AddItemWithProbability(new GraffitiGargoyleAlleyMap(), 0.003);
			AddItemWithProbability(new GreaserGryphonRidersArenaMap(), 0.003);
			AddItemWithProbability(new GreenHagsSwampMap(), 0.003);
			AddItemWithProbability(new GreenNinjasHiddenLairMap(), 0.003);
			AddItemWithProbability(new HippieHoplitesGroveMap(), 0.003);
			AddItemWithProbability(new HolyKnightsCathedralMap(), 0.003);
			AddItemWithProbability(new HostileDruidsGladeMap(), 0.003);
			AddItemWithProbability(new HostilePrincessCourtMap(), 0.003);
			AddItemWithProbability(new IceKingsDomainMap(), 0.003);
			AddItemWithProbability(new InfernoDragonsLairMap(), 0.003);
			AddItemWithProbability(new InsaneRoboticistWorkshopMap(), 0.003);
			AddItemWithProbability(new JazzAgeBrawlMap(), 0.003);
			AddItemWithProbability(new JestersCourtMap(), 0.003);
			AddItemWithProbability(new LawyersTribunalMap(), 0.003);
			AddItemWithProbability(new LineDragonsAscentMap(), 0.003);
			AddItemWithProbability(new LordBlackthornsDominionMap(), 0.003);
			AddItemWithProbability(new LordBritishsSummoningCircleMap(), 0.003);
			AddItemWithProbability(new MagmaElementalRiftMap(), 0.003);
			AddItemWithProbability(new MedievalMeteorologistsObservatoryMap(), 0.003);
			AddItemWithProbability(new MegaDragonsLairMap(), 0.003);
			AddItemWithProbability(new MinaxSorceressSanctumMap(), 0.003);
			AddItemWithProbability(new MischievousWitchCovenMap(), 0.003);
			AddItemWithProbability(new MotownMermaidLagoonMap(), 0.003);
			AddItemWithProbability(new MushroomWitchGroveMap(), 0.003);
			AddItemWithProbability(new MusketeerHallMap(), 0.003);
			AddItemWithProbability(new MysticGroveMap(), 0.003);
			AddItemWithProbability(new NeovictorianVampireCourtMap(), 0.003);
			AddItemWithProbability(new NinjaLibrarianSanctumMap(), 0.003);
			AddItemWithProbability(new NoirDetectiveHideoutMap(), 0.003);
			AddItemWithProbability(new OgreMastersDomainMap(), 0.003);
			AddItemWithProbability(new PhoenixStyleMastersArenaMap(), 0.003);
			AddItemWithProbability(new PigFarmersPenMap(), 0.003);
			AddItemWithProbability(new PinupPandemoniumParlorMap(), 0.003);
			AddItemWithProbability(new PirateOfTheStarsOutpostMap(), 0.003);
			AddItemWithProbability(new PiratesCoveMap(), 0.003);
			AddItemWithProbability(new PkMurderersHideoutMap(), 0.003);
			AddItemWithProbability(new RaKingsPyramidMap(), 0.003);
			AddItemWithProbability(new RanchMastersPrairieMap(), 0.003);
			AddItemWithProbability(new RapRangersJungleMap(), 0.003);
			AddItemWithProbability(new RaveRoguesUndergroundMap(), 0.003);
			AddItemWithProbability(new RebelCathedralMap(), 0.003);
			AddItemWithProbability(new RedQueensCourtMap(), 0.003);
			AddItemWithProbability(new ReggaeRunesmithWorkshopMap(), 0.003);
			AddItemWithProbability(new RenaissanceMechanicFactoryMap(), 0.003);
			AddItemWithProbability(new RetroAndroidWorkshopMap(), 0.003);
			AddItemWithProbability(new RetroFuturistDomeMap(), 0.003);
			AddItemWithProbability(new RetroRobotRomancersLairMap(), 0.003);
			AddItemWithProbability(new RingmastersCircusMap(), 0.003);
			AddItemWithProbability(new RockabillyRevenantsStageMap(), 0.003);
			AddItemWithProbability(new SanctuaryOfTheHolyKnightMap(), 0.003);
			AddItemWithProbability(new ScorpomancersDomainMap(), 0.003);
			AddItemWithProbability(new SilentMovieStudioMap(), 0.003);
			AddItemWithProbability(new SilverSlimeCavernsMap(), 0.003);
			AddItemWithProbability(new SithAcademyMap(), 0.003);
			AddItemWithProbability(new SkaSkaldConcertHallMap(), 0.003);
			AddItemWithProbability(new SkeletonLordCryptMap(), 0.003);
			AddItemWithProbability(new SlimeMageSwampMap(), 0.003);
			AddItemWithProbability(new SneakyNinjaClanMap(), 0.003);
			AddItemWithProbability(new StarCitizenOutpostMap(), 0.003);
			AddItemWithProbability(new StarfleetCaptainsCommandMap(), 0.003);
			AddItemWithProbability(new StarfleetCommandCenterMap(), 0.003);
			AddItemWithProbability(new SteampunkSamuraiForgeMap(), 0.003);
			AddItemWithProbability(new StormtrooperAcademyMap(), 0.003);
			AddItemWithProbability(new SurferSummonerCoveMap(), 0.003);
			AddItemWithProbability(new SwampThingLairMap(), 0.003);
			AddItemWithProbability(new SwinginSorceressBallroomMap(), 0.003);
			AddItemWithProbability(new TexanRancherPrairieMap(), 0.003);
			AddItemWithProbability(new TwistedCultistHideoutMap(), 0.003);
			AddItemWithProbability(new VaudevilleValkyrieStageMap(), 0.003);
			AddItemWithProbability(new WastelandBikerCompoundMap(), 0.003);
			AddItemWithProbability(new WaterClanNinjaHideoutMap(), 0.003);
			AddItemWithProbability(new WaterClanSamuraiFortressMap(), 0.003);
			AddItemWithProbability(new WildWestWizardCanyonMap(), 0.003);
			AddItemWithProbability(new FleshEaterOgreTombMap(), 0.003);
			AddItemWithProbability(new FlyingSquirrelHollowMap(), 0.003);
			AddItemWithProbability(new ForgottenWardenCryptMap(), 0.003);
			AddItemWithProbability(new FoxSquirrelGlenMap(), 0.003);
			AddItemWithProbability(new FrenziedSatyrsForestMap(), 0.003);
			AddItemWithProbability(new FrostbiteWolfsDomainMap(), 0.003);
			AddItemWithProbability(new FrostboundChampionsHallMap(), 0.003);
			AddItemWithProbability(new FrostDroidFactoryMap(), 0.003);
			AddItemWithProbability(new FrostLichsCryptMap(), 0.003);
			AddItemWithProbability(new FrostOgreLairMap(), 0.003);
			AddItemWithProbability(new FrostWapitiGroundsMap(), 0.003);
			AddItemWithProbability(new FrostWardenWatchMap(), 0.003);
			AddItemWithProbability(new FrozenOozeCaveMap(), 0.003);
			AddItemWithProbability(new FungalToadSwampMap(), 0.003);
			AddItemWithProbability(new GeminiHarpysLairMap(), 0.003);
			AddItemWithProbability(new GeminiTwinBearsDenMap(), 0.003);
			AddItemWithProbability(new GentleSatyrsGroveMap(), 0.003);
			AddItemWithProbability(new GiantForestHogsNestMap(), 0.003);
			AddItemWithProbability(new GiantWolfSpidersWebMap(), 0.003);
			AddItemWithProbability(new GoliathBirdeatersLairMap(), 0.003);
			AddItemWithProbability(new GoralsDomainMap(), 0.003);
			AddItemWithProbability(new GrapplerDronesArenaMap(), 0.003);
			AddItemWithProbability(new GraveKnightsCryptMap(), 0.003);
			AddItemWithProbability(new GummySheepsPastureMap(), 0.003);
			AddItemWithProbability(new InfernoStallionArenaMap(), 0.003);
			AddItemWithProbability(new InfinitePouncersDenMap(), 0.003);
			AddItemWithProbability(new IroncladDefendersFortressMap(), 0.003);
			AddItemWithProbability(new IroncladOgresDomainMap(), 0.003);
			AddItemWithProbability(new IronGolemsWorkshopMap(), 0.003);
			AddItemWithProbability(new IronSteedStablesMap(), 0.003);
			AddItemWithProbability(new JavelinaJinxHuntMap(), 0.003);
			AddItemWithProbability(new JellybeanJestersCarnivalMap(), 0.003);
			AddItemWithProbability(new KasTheBloodyhandedCryptMap(), 0.003);
			AddItemWithProbability(new KelthuzadsFrozenCitadelMap(), 0.003);
			AddItemWithProbability(new KhufuTheGreatBuildersTombMap(), 0.003);
			AddItemWithProbability(new LairOfTheGibbonMysticsGroveMap(), 0.003);
			AddItemWithProbability(new LairOfTheGlisteningOozesCavernMap(), 0.003);
			AddItemWithProbability(new LairOfTheGloomOgresFortressMap(), 0.003);
			AddItemWithProbability(new LairOfTheGloomWolfsDenMap(), 0.003);
			AddItemWithProbability(new LairOfTheGoldenOrbWeaversWebMap(), 0.003);
			AddItemWithProbability(new LairOfTheHatshepsutTheQueensTombMap(), 0.003);
			AddItemWithProbability(new LairOfTheHogWildsSwinePenMap(), 0.003);
			AddItemWithProbability(new LairOfTheHowlerMonkeysJungleMap(), 0.003);
			AddItemWithProbability(new LairOfTheHuntsmanSpidersLairMap(), 0.003);
			AddItemWithProbability(new LairOfTheHydrokineticWardensWaterShrineMap(), 0.003);
			AddItemWithProbability(new LairOfTheIbexHighlandMap(), 0.003);
			AddItemWithProbability(new LairOfTheIndianPalmSquirrelGroveMap(), 0.003);
			AddItemWithProbability(new LairOfTheInfernalLichCitadelMap(), 0.003);
			AddItemWithProbability(new LairOfTheInfernalToadSwampMap(), 0.003);
			AddItemWithProbability(new LairOfTheInfernoSentinelFortressMap(), 0.003);
			AddItemWithProbability(new LairOfTheLibraBalanceBearMap(), 0.003);
			AddItemWithProbability(new LairOfTheLibraHarpyMap(), 0.003);
			AddItemWithProbability(new LairOfTheLicoriceSheepMap(), 0.003);
			AddItemWithProbability(new LairOfTheLollipopLordMap(), 0.003);
			AddItemWithProbability(new LairOfTheLuchadorLlamaMap(), 0.003);
			AddItemWithProbability(new LairOfTheMalariaratDenMap(), 0.003);
			AddItemWithProbability(new LairOfTheMandrillshamanJungleMap(), 0.003);
			AddItemWithProbability(new LairOfTheMarkhorPeaksMap(), 0.003);
			AddItemWithProbability(new LairOfTheMeatgolemLaboratoryMap(), 0.003);
			AddItemWithProbability(new LairOfTheMelodicsatyrGroveMap(), 0.003);
			AddItemWithProbability(new LairOfTheMountainGorillaMap(), 0.003);
			AddItemWithProbability(new LairOfTheMuckGolemMap(), 0.003);
			AddItemWithProbability(new LairOfTheMysticFallowMap(), 0.003);
			AddItemWithProbability(new LairOfTheMysticSatyrMap(), 0.003);
			AddItemWithProbability(new LairOfTheNagashMap(), 0.003);
			AddItemWithProbability(new LairOfTheNefertitisTombMap(), 0.003);
			AddItemWithProbability(new LarlochTheShadowKingsCryptMap(), 0.003);
			AddItemWithProbability(new LeoTheHarpysLairMap(), 0.003);
			AddItemWithProbability(new LeoTheSunBearsDenMap(), 0.003);
			AddItemWithProbability(new LeprosyRatNestMap(), 0.003);
			AddItemWithProbability(new MentuhotepTheWiseTombMap(), 0.003);
			AddItemWithProbability(new MetallicWindsteedPeaksMap(), 0.003);
			AddItemWithProbability(new MimicronsLairMap(), 0.003);
			AddItemWithProbability(new MireSpawnerMarshMap(), 0.003);
			AddItemWithProbability(new MoltenSlimePitMap(), 0.003);
			AddItemWithProbability(new NanoSwarmLabMap(), 0.003);
			AddItemWithProbability(new NebulaCatsCelestialRealmMap(), 0.003);
			AddItemWithProbability(new NecroticGeneralsBattlefieldMap(), 0.003);
			AddItemWithProbability(new NecroticLichsTombMap(), 0.003);
			AddItemWithProbability(new NecroticOgresDomainMap(), 0.003);
			AddItemWithProbability(new NemesisUnitFacilityMap(), 0.003);
			AddItemWithProbability(new NightmareLeaperAbyssMap(), 0.003);
			AddItemWithProbability(new NightmarePanthersDomainMap(), 0.003);
			AddItemWithProbability(new OmegaSentinelsFortressMap(), 0.003);
			AddItemWithProbability(new OrangutanSageGroveMap(), 0.003);
			AddItemWithProbability(new OverlordMkiiStrongholdMap(), 0.003);
			AddItemWithProbability(new PeccaryProtectorForestMap(), 0.003);
			AddItemWithProbability(new PeppermintPuffDomainMap(), 0.003);
			AddItemWithProbability(new PhantomAutomatonVaultMap(), 0.003);
			AddItemWithProbability(new PhantomPanthersDomainMap(), 0.003);
			AddItemWithProbability(new PlagueLichsCryptMap(), 0.003);
			AddItemWithProbability(new PlasmaJuggernautsForgeMap(), 0.003);
			AddItemWithProbability(new PurseSpidersLairMap(), 0.003);
			AddItemWithProbability(new QuantumGuardiansRealmMap(), 0.003);
			AddItemWithProbability(new RabidRatLairMap(), 0.003);
			AddItemWithProbability(new RadiantSlimeCavernMap(), 0.003);
			AddItemWithProbability(new RaistlinMajeresTowerMap(), 0.003);
			AddItemWithProbability(new RamsesTheImmortalTombMap(), 0.003);
			AddItemWithProbability(new RedSquirrelNestMap(), 0.003);
			AddItemWithProbability(new RedtailedSquirrelGroveMap(), 0.003);
			AddItemWithProbability(new BreezePhantomAbyssMap(), 0.003);
			AddItemWithProbability(new BubbleFerretForestMap(), 0.003);
			AddItemWithProbability(new CelestialDragonShrineMap(), 0.003);
			AddItemWithProbability(new CerebralEttinCavernMap(), 0.003);
			AddItemWithProbability(new ChanequeGroveMap(), 0.003);
			AddItemWithProbability(new ChimereonSwampMap(), 0.003);
			AddItemWithProbability(new CinderWraithRuinsMap(), 0.003);
			AddItemWithProbability(new CorruptingCreeperForestMap(), 0.003);
			AddItemWithProbability(new CrystalDragonCavernMap(), 0.003);
			AddItemWithProbability(new CrystalWardenTempleMap(), 0.003);
			AddItemWithProbability(new CursedHarbingerCryptMap(), 0.003);
			AddItemWithProbability(new CycloneDemonPlainsMap(), 0.003);
			AddItemWithProbability(new DairyWraithFieldMap(), 0.003);
			AddItemWithProbability(new DeadlordFortressMap(), 0.003);
			AddItemWithProbability(new DreadedCreeperHollowMap(), 0.003);
			AddItemWithProbability(new DreamyFerretHollowMap(), 0.003);
			AddItemWithProbability(new DrolaticWastesMap(), 0.003);
			AddItemWithProbability(new DryadGroveMap(), 0.003);
			AddItemWithProbability(new EarthquakeEttinDenMap(), 0.003);
			AddItemWithProbability(new ElderTendrilSwampMap(), 0.003);
			AddItemWithProbability(new EmberSerpentLairMap(), 0.003);
			AddItemWithProbability(new EmberSpiritDomainMap(), 0.003);
			AddItemWithProbability(new EtherealCrabNestMap(), 0.003);
			AddItemWithProbability(new EtherealDragonsKeepMap(), 0.003);
			AddItemWithProbability(new FirebreathAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new FireRoosterCavernMap(), 0.003);
			AddItemWithProbability(new FlameBearerCaveMap(), 0.003);
			AddItemWithProbability(new FlameWardenEttinFortressMap(), 0.003);
			AddItemWithProbability(new FlareImpNestMap(), 0.003);
			AddItemWithProbability(new FossilElementalCavernMap(), 0.003);
			AddItemWithProbability(new FrostBearDomainMap(), 0.003);
			AddItemWithProbability(new FrostbiteAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new FrostboundBehemothCaveMap(), 0.003);
			AddItemWithProbability(new FrostDrakonKeepMap(), 0.003);
			AddItemWithProbability(new FrostHensPerchMap(), 0.003);
			AddItemWithProbability(new FrostSerpentLairMap(), 0.003);
			AddItemWithProbability(new FrostWardenEttinStrongholdMap(), 0.003);
			AddItemWithProbability(new FrostyFerretsBurrowMap(), 0.003);
			AddItemWithProbability(new GaleWispsDomainMap(), 0.003);
			AddItemWithProbability(new GiantTrapdoorSpiderLairMap(), 0.003);
			AddItemWithProbability(new GiantWolfSpiderNestMap(), 0.003);
			AddItemWithProbability(new GlimmeringFerretBurrowMap(), 0.003);
			AddItemWithProbability(new GoldenOrbWeaverCavernMap(), 0.003);
			AddItemWithProbability(new GoliathBirdeaterJungleMap(), 0.003);
			AddItemWithProbability(new GorgonVipersLairMap(), 0.003);
			AddItemWithProbability(new GraniteColossusCavernMap(), 0.003);
			AddItemWithProbability(new GrimoriesTomeMap(), 0.003);
			AddItemWithProbability(new GrotesqueOfRouensCryptMap(), 0.003);
			AddItemWithProbability(new GrymalkinTheWatchersDomainMap(), 0.003);
			AddItemWithProbability(new GuernseyGuardianKeepMap(), 0.003);
			AddItemWithProbability(new HarmonyFerretGroveMap(), 0.003);
			AddItemWithProbability(new HellfireJuggernautForgeMap(), 0.003);
			AddItemWithProbability(new HerefordWarlockTowerMap(), 0.003);
			AddItemWithProbability(new HighlandBullHerdMap(), 0.003);
			AddItemWithProbability(new HuntsmanSpidersLairMap(), 0.003);
			AddItemWithProbability(new IceCrabCavernMap(), 0.003);
			AddItemWithProbability(new IllusionarySwampMap(), 0.003);
			AddItemWithProbability(new IllusionHensParadiseMap(), 0.003);
			AddItemWithProbability(new IllusionistEttinsDomainMap(), 0.003);
			AddItemWithProbability(new InfernalDukesCitadelMap(), 0.003);
			AddItemWithProbability(new InfernalIncineratorForgeMap(), 0.003);
			AddItemWithProbability(new InfernoDrakonsRoostMap(), 0.003);
			AddItemWithProbability(new InfernoPythonPitMap(), 0.003);
			AddItemWithProbability(new InfernoWardensFortressMap(), 0.003);
			AddItemWithProbability(new IshKarTheForgottenLairMap(), 0.003);
			AddItemWithProbability(new JerseyEnchantressCovenMap(), 0.003);
			AddItemWithProbability(new LairOfTheShadowogresDomainMap(), 0.003);
			AddItemWithProbability(new LairOfTheShadowprowlersHuntMap(), 0.003);
			AddItemWithProbability(new LairOfTheShadowsludgesSwampMap(), 0.003);
			AddItemWithProbability(new LairOfTheShadowtoadsBogMap(), 0.003);
			AddItemWithProbability(new LairOfTheSifakawarriorsJungleMap(), 0.003);
			AddItemWithProbability(new LairOfTheSmallpoxRatLairMap(), 0.003);
			AddItemWithProbability(new LairOfTheSombreroDeSolLlamaMap(), 0.003);
			AddItemWithProbability(new LairOfTheSombreroLlamaMap(), 0.003);
			AddItemWithProbability(new LairOfTheSothTheDeathKnightMap(), 0.003);
			AddItemWithProbability(new LairOfTheSoulEaterLichMap(), 0.003);
			AddItemWithProbability(new LairOfTheTahrsWildHordeMap(), 0.003);
			AddItemWithProbability(new LairOfTheTalonmachinesForgeMap(), 0.003);
			AddItemWithProbability(new LairOfTheTaurusearthbearsDominionMap(), 0.003);
			AddItemWithProbability(new LairOfTheTaurusharpysSkiesMap(), 0.003);
			AddItemWithProbability(new LairOfTheTempestsatyrsStormMap(), 0.003);
			AddItemWithProbability(new LairOfTheVietnamesePigMap(), 0.003);
			AddItemWithProbability(new LairOfTheVileToadMap(), 0.003);
			AddItemWithProbability(new LairOfTheVirgoHarpyMap(), 0.003);
			AddItemWithProbability(new LairOfTheVirgoPurityBearMap(), 0.003);
			AddItemWithProbability(new LairOfTheVoidCatMap(), 0.003);
			AddItemWithProbability(new LairOfTheVoidSlimeMap(), 0.003);
			AddItemWithProbability(new LairOfTheVolcaniccHargerMap(), 0.003);
			AddItemWithProbability(new LairOfTheVortexConstructMap(), 0.003);
			AddItemWithProbability(new LairOfTheVortexWraithMap(), 0.003);
			AddItemWithProbability(new LairOfTheWarthogWarriorMap(), 0.003);
			AddItemWithProbability(new LairOfTheWraithlichCryptMap(), 0.003);
			AddItemWithProbability(new LairOfTheYangstallionPlainsMap(), 0.003);
			AddItemWithProbability(new LairOfTheYinsteedForestMap(), 0.003);
			AddItemWithProbability(new LavaCrabCavernMap(), 0.003);
			AddItemWithProbability(new LavaFiendFortressMap(), 0.003);
			AddItemWithProbability(new LeafBearGroveMap(), 0.003);
			AddItemWithProbability(new LeprechaunsLairMap(), 0.003);
			AddItemWithProbability(new LightBearersSanctuaryMap(), 0.003);
			AddItemWithProbability(new MagmaGolemForgeMap(), 0.003);
			AddItemWithProbability(new MagneticCrabCavernMap(), 0.003);
			AddItemWithProbability(new MaineCoonTitansRoostMap(), 0.003);
			AddItemWithProbability(new MilkingDemonStablesMap(), 0.003);
			AddItemWithProbability(new RhythmicSatyrsGladeMap(), 0.003);
			AddItemWithProbability(new RockSquirrelCavernMap(), 0.003);
			AddItemWithProbability(new SableAntelopeSavannaMap(), 0.003);
			AddItemWithProbability(new SagittariusArcherBearForestMap(), 0.003);
			AddItemWithProbability(new SagittariusHarpysPerchMap(), 0.003);
			AddItemWithProbability(new SandGolemsTombMap(), 0.003);
			AddItemWithProbability(new ScorpioHarpysLairMap(), 0.003);
			AddItemWithProbability(new ScorpionSpidersHollowMap(), 0.003);
			AddItemWithProbability(new ScorpioVenomBearsDenMap(), 0.003);
			AddItemWithProbability(new SetiTheAvengersCryptMap(), 0.003);
			AddItemWithProbability(new ShadowbladeAssassinsLairMap(), 0.003);
			AddItemWithProbability(new ShadowGolemsAbyssMap(), 0.003);
			AddItemWithProbability(new ShadowLichsNecropolisMap(), 0.003);
			AddItemWithProbability(new ShadowMuntjacsDomainMap(), 0.003);
			AddItemWithProbability(new SpectralAutomatonForgeMap(), 0.003);
			AddItemWithProbability(new SpectralToadSwampMap(), 0.003);
			AddItemWithProbability(new SpectralWardenCryptMap(), 0.003);
			AddItemWithProbability(new SpiderlingMinionBroodmotherMap(), 0.003);
			AddItemWithProbability(new SpiderMonkeyJungleMap(), 0.003);
			AddItemWithProbability(new StarbornPredatorNestMap(), 0.003);
			AddItemWithProbability(new SteamLeviathanAbyssMap(), 0.003);
			AddItemWithProbability(new StoneGolemCavernMap(), 0.003);
			AddItemWithProbability(new StoneSteedStablesMap(), 0.003);
			AddItemWithProbability(new StormBoneFortressMap(), 0.003);
			AddItemWithProbability(new SynthroidPrimeFactoryMap(), 0.003);
			AddItemWithProbability(new SzassTamsNecropolisMap(), 0.003);
			AddItemWithProbability(new TacoLlamaFestivalMap(), 0.003);
			AddItemWithProbability(new TacticalEnforcerOperationsMap(), 0.003);
			AddItemWithProbability(new TaffyTitansArenaMap(), 0.003);
			AddItemWithProbability(new TequilaLlamaTavernMap(), 0.003);
			AddItemWithProbability(new TheForestTempestMap(), 0.003);
			AddItemWithProbability(new TheStormOfDeathMap(), 0.003);
			AddItemWithProbability(new TheTempestsFuryMap(), 0.003);
			AddItemWithProbability(new TheTempestsWrathMap(), 0.003);
			AddItemWithProbability(new TheWrathOfTheThunderKingMap(), 0.003);
			AddItemWithProbability(new ThutmoseTheConquerorsTombMap(), 0.003);
			AddItemWithProbability(new TidalMaresDeepMap(), 0.003);
			AddItemWithProbability(new ToxicLichsLairMap(), 0.003);
			AddItemWithProbability(new ToxicOgresStrongholdMap(), 0.003);
			AddItemWithProbability(new ToxicSludgeSwampMap(), 0.003);
			AddItemWithProbability(new TrapdoorSpiderNestMap(), 0.003);
			AddItemWithProbability(new TsunamiTitansDeepMap(), 0.003);
			AddItemWithProbability(new TutankhamunTheCursedTombMap(), 0.003);
			AddItemWithProbability(new TyphusRatInfestationMap(), 0.003);
			AddItemWithProbability(new VampiricBladesLairMap(), 0.003);
			AddItemWithProbability(new VecnasSanctumMap(), 0.003);
			AddItemWithProbability(new VenomousRoesMarshMap(), 0.003);
			AddItemWithProbability(new VenomousToadsSwampMap(), 0.003);
			AddItemWithProbability(new VenomousWolfsLairMap(), 0.003);
			AddItemWithProbability(new WhisperingPookaGroveMap(), 0.003);
			AddItemWithProbability(new WickedSatyrsForestMap(), 0.003);
			AddItemWithProbability(new WoodGolemsHollowMap(), 0.003);
			AddItemWithProbability(new WoodlandChargerDomainMap(), 0.003);
			AddItemWithProbability(new WoodlandSpiritHorseMeadowMap(), 0.003);
			AddItemWithProbability(new MoltenGolemCavernMap(), 0.003);
			AddItemWithProbability(new MordrakesManorMap(), 0.003);
			AddItemWithProbability(new MudGolemSwampMap(), 0.003);
			AddItemWithProbability(new MysticFerretSanctuaryMap(), 0.003);
			AddItemWithProbability(new MysticFowlSanctuaryMap(), 0.003);
			AddItemWithProbability(new MysticWispRealmMap(), 0.003);
			AddItemWithProbability(new NatureDragonsLairMap(), 0.003);
			AddItemWithProbability(new NecroEttinCryptMap(), 0.003);
			AddItemWithProbability(new NecroRoosterTombMap(), 0.003);
			AddItemWithProbability(new NightshadeBrambleGroveMap(), 0.003);
			AddItemWithProbability(new NymphsSanctuaryMap(), 0.003);
			AddItemWithProbability(new NyxRithRuinsMap(), 0.003);
			AddItemWithProbability(new PersianShadeTombMap(), 0.003);
			AddItemWithProbability(new PhantomVinesOvergrowthMap(), 0.003);
			AddItemWithProbability(new PoisonousCrabCoveMap(), 0.003);
			AddItemWithProbability(new PoisonPulletFarmMap(), 0.003);
			AddItemWithProbability(new PucksMischiefMap(), 0.003);
			AddItemWithProbability(new PuffyFerretHollowMap(), 0.003);
			AddItemWithProbability(new PurseSpiderNestMap(), 0.003);
			AddItemWithProbability(new PyroclasticGolemForgeMap(), 0.003);
			AddItemWithProbability(new QuakeBringerCavernMap(), 0.003);
			AddItemWithProbability(new QuorZaelsDomainMap(), 0.003);
			AddItemWithProbability(new RagdollGuardianCitadelMap(), 0.003);
			AddItemWithProbability(new RagingAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new RathzorTheShatteredsLairMap(), 0.003);
			AddItemWithProbability(new RiptideCrabCoveMap(), 0.003);
			AddItemWithProbability(new RockBearCavernMap(), 0.003);
			AddItemWithProbability(new SahiwalShamansGroveMap(), 0.003);
			AddItemWithProbability(new SandstormElementalDesertMap(), 0.003);
			AddItemWithProbability(new ScorpionSpiderPitMap(), 0.003);
			AddItemWithProbability(new ScottishFoldSentinelDenMap(), 0.003);
			AddItemWithProbability(new SelkieCavernMap(), 0.003);
			AddItemWithProbability(new ShadowAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new ShadowAnacondaJungleMap(), 0.003);
			AddItemWithProbability(new ShadowBearsLairMap(), 0.003);
			AddItemWithProbability(new ShadowChicksNestMap(), 0.003);
			AddItemWithProbability(new ShadowCrabsTidepoolMap(), 0.003);
			AddItemWithProbability(new ShadowDragonsRoostMap(), 0.003);
			AddItemWithProbability(new ShadowDriftersMistsMap(), 0.003);
			AddItemWithProbability(new SiameseIllusionistChamberMap(), 0.003);
			AddItemWithProbability(new SiberianFrostclawsDomainMap(), 0.003);
			AddItemWithProbability(new SidheFaeRealmMap(), 0.003);
			AddItemWithProbability(new SinisterRootHollowMap(), 0.003);
			AddItemWithProbability(new SkeletonEttinStrongholdMap(), 0.003);
			AddItemWithProbability(new SkySeraphsAerieMap(), 0.003);
			AddItemWithProbability(new SolarElementalSummitMap(), 0.003);
			AddItemWithProbability(new SparkFerretWildsMap(), 0.003);
			AddItemWithProbability(new SphinxCatsRiddleMap(), 0.003);
			AddItemWithProbability(new SpiderlingOverlordBroodmotherMap(), 0.003);
			AddItemWithProbability(new StarryFerretsCelestialRealmMap(), 0.003);
			AddItemWithProbability(new SteelBearDenMap(), 0.003);
			AddItemWithProbability(new StoneGuardianFortressMap(), 0.003);
			AddItemWithProbability(new StoneRoosterCryptMap(), 0.003);
			AddItemWithProbability(new StormAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new StormCrabsLairMap(), 0.003);
			AddItemWithProbability(new StormDaemonsDomainMap(), 0.003);
			AddItemWithProbability(new StormDragonsPeakMap(), 0.003);
			AddItemWithProbability(new StormHeraldsSanctuaryMap(), 0.003);
			AddItemWithProbability(new StrixsPerchMap(), 0.003);
			AddItemWithProbability(new SunbeamFerretHollowMap(), 0.003);
			AddItemWithProbability(new TarantulaWarriorLairMap(), 0.003);
			AddItemWithProbability(new TarantulaWorriorCavernMap(), 0.003);
			AddItemWithProbability(new TempestSpiritDomainMap(), 0.003);
			AddItemWithProbability(new TempestWyrmSpireMap(), 0.003);
			AddItemWithProbability(new TerraWispGroveMap(), 0.003);
			AddItemWithProbability(new ThornedHorrorSwampMap(), 0.003);
			AddItemWithProbability(new ThulGorTheForsakenLairMap(), 0.003);
			AddItemWithProbability(new ThunderBearHighlandsMap(), 0.003);
			AddItemWithProbability(new ThunderbirdMountainMap(), 0.003);
			AddItemWithProbability(new ThunderSerpentCavernMap(), 0.003);
			AddItemWithProbability(new TidalEttinMarshMap(), 0.003);
			AddItemWithProbability(new TitanBoaSwampMap(), 0.003);
			AddItemWithProbability(new ToxicAlligatorSwampsMap(), 0.003);
			AddItemWithProbability(new ToxicReaverNecropolisMap(), 0.003);
			AddItemWithProbability(new TurkishAngoraEnchantersDomainMap(), 0.003);
			AddItemWithProbability(new TwinTerrorEttinsFortressMap(), 0.003);
			AddItemWithProbability(new UruKothsLairMap(), 0.003);
			AddItemWithProbability(new VengefulPitVipersPitMap(), 0.003);
			AddItemWithProbability(new VenomBearsDenMap(), 0.003);
			AddItemWithProbability(new VenomousAlligatorSwampMap(), 0.003);
			AddItemWithProbability(new VenomousDragonLairMap(), 0.003);
			AddItemWithProbability(new VenomousEttinCaveMap(), 0.003);
			AddItemWithProbability(new VenomousIvyGroveMap(), 0.003);
			AddItemWithProbability(new VespaHiveMap(), 0.003);
			AddItemWithProbability(new VileBlossomGroveMap(), 0.003);
			AddItemWithProbability(new VitrailTheMosaicMap(), 0.003);
			AddItemWithProbability(new VoidStalkerAbyssMap(), 0.003);
			AddItemWithProbability(new VolcanicTitanCraterMap(), 0.003);
			AddItemWithProbability(new VorgathTheDestroyerMap(), 0.003);
			AddItemWithProbability(new VortexCrabReefMap(), 0.003);
			AddItemWithProbability(new VortexGuardianKeepMap(), 0.003);
			AddItemWithProbability(new WhirlwindFiendAbyssMap(), 0.003);
			AddItemWithProbability(new WillothewispEnclaveMap(), 0.003);
			AddItemWithProbability(new WindBearGroveMap(), 0.003);
			AddItemWithProbability(new WindChickenNestMap(), 0.003);
			AddItemWithProbability(new XalrathCultMap(), 0.003);
			AddItemWithProbability(new ZebuZealotRuinsMap(), 0.003);
			AddItemWithProbability(new ZelvrakStrongholdMap(), 0.003);
			AddItemWithProbability(new ZephyrWardensDomainMap(), 0.003);
			AddItemWithProbability(new EasternMagicMap(), 0.003);
			AddItemWithProbability(new FarEasternMagicMap(), 0.003);
			AddItemWithProbability(new FeluccaMagicMap(), 0.003);
			AddItemWithProbability(new IlshenarMagicMap(), 0.003);
			AddItemWithProbability(new NorthernMagicMap(), 0.003);
			AddItemWithProbability(new SouthernMagicMap(), 0.003);
			AddItemWithProbability(new TerMurMagicMap(), 0.003);
			AddItemWithProbability(new TokunoMagicMap(), 0.003);
			AddItemWithProbability(new TrammelMagicMap(), 0.003);
			AddItemWithProbability(new WesternMagicMap(), 0.003);
			AddItemWithProbability(new AirClanNinjaCampMap(), 0.003);
			AddItemWithProbability(new AirClanSamuraiDojoMap(), 0.003);
			AddItemWithProbability(new AlienWarriorNestMap(), 0.003);
			AddItemWithProbability(new AppleGroveElementalMap(), 0.003);
			AddItemWithProbability(new AssassinGuildHallMap(), 0.003);
			AddItemWithProbability(new AstralTravelerRealmMap(), 0.003);
			AddItemWithProbability(new AvatarOfElementsShrineMap(), 0.003);
			AddItemWithProbability(new BaroqueBarbarianCampMap(), 0.003);
			AddItemWithProbability(new BeetleJuiceSummoningCircleMap(), 0.003);
			AddItemWithProbability(new BiomancersGroveMap(), 0.003);
			AddItemWithProbability(new BluesSingingGorgonAmphitheaterMap(), 0.003);
			AddItemWithProbability(new BMovieBeastmasterArenaMap(), 0.003);
			AddItemWithProbability(new BountyHunterOutpostMap(), 0.003);
			AddItemWithProbability(new CabaretKrakenStageMap(), 0.003);
			AddItemWithProbability(new CannibalTribeCampMap(), 0.003);
			AddItemWithProbability(new CavemanScientistExperimentSiteMap(), 0.003);
			AddItemWithProbability(new CelestialSamuraiDojoMap(), 0.003);
			AddItemWithProbability(new ChrisRobertsGalacticArenaMap(), 0.003);
			AddItemWithProbability(new CorporateExecutiveTowerMap(), 0.003);
			AddItemWithProbability(new CountryCowgirlCyclopsRanchMap(), 0.003);
			AddItemWithProbability(new CyberpunkNexusMap(), 0.003);
			AddItemWithProbability(new DarkElfCitadelMap(), 0.003);
			AddItemWithProbability(new DinoRiderExpeditionMap(), 0.003);
			AddItemWithProbability(new DiscoDruidFestivalMap(), 0.003);
			AddItemWithProbability(new HarvestFestivalFrenzyMap(), 0.003);
			AddItemWithProbability(new LongbowSniperOutpostMap(), 0.003);
			AddItemWithProbability(new LuchadorTrainingGroundsMap(), 0.003);
			AddItemWithProbability(new MagiciansArcaneHallMap(), 0.003);
			AddItemWithProbability(new MartialMonkDojoMap(), 0.003);
			AddItemWithProbability(new MasterFlutistsConcertMap(), 0.003);
			AddItemWithProbability(new MechanicsWorkshopMap(), 0.003);
			AddItemWithProbability(new MusclePitMap(), 0.003);
			AddItemWithProbability(new NecromancersHollowMap(), 0.003);
			AddItemWithProbability(new NetCasterReefMap(), 0.003);
			AddItemWithProbability(new NinjaShadowHideoutMap(), 0.003);
			AddItemWithProbability(new NymphSingerGladeMap(), 0.003);
			AddItemWithProbability(new OraclesSanctumMap(), 0.003);
			AddItemWithProbability(new PastryChefsBakeryMap(), 0.003);
			AddItemWithProbability(new PatchworkMonsterLaboratoryMap(), 0.003);
			AddItemWithProbability(new PathologistsLairMap(), 0.003);
			AddItemWithProbability(new PhantomAssassinsHideoutMap(), 0.003);
			AddItemWithProbability(new PickpocketsDenMap(), 0.003);
			AddItemWithProbability(new PocketPickersRefugeMap(), 0.003);
			AddItemWithProbability(new ProtestersCampMap(), 0.003);
			AddItemWithProbability(new QiGongHealerSanctuaryMap(), 0.003);
			AddItemWithProbability(new QuantumPhysicistResearchFacilityMap(), 0.003);
			AddItemWithProbability(new RamRiderOutpostMap(), 0.003);
			AddItemWithProbability(new RapierDuelistArenaMap(), 0.003);
			AddItemWithProbability(new RelativistObservatoryMap(), 0.003);
			AddItemWithProbability(new RelicHunterExpeditionMap(), 0.003);
			AddItemWithProbability(new RuneCasterSanctumMap(), 0.003);
			AddItemWithProbability(new RuneKeeperChamberMap(), 0.003);
			AddItemWithProbability(new SaboteurHideoutMap(), 0.003);
			AddItemWithProbability(new SabreFighterArenaMap(), 0.003);
			AddItemWithProbability(new SafecrackersDenMap(), 0.003);
			AddItemWithProbability(new SamuraiMastersDojoMap(), 0.003);
			AddItemWithProbability(new SatyrPipersGlenMap(), 0.003);
			AddItemWithProbability(new SawmillWorkersDomainMap(), 0.003);
			AddItemWithProbability(new ScoutArchersRefugeMap(), 0.003);
			AddItemWithProbability(new ScoutLeaderEncampmentMap(), 0.003);
			AddItemWithProbability(new ScrollMagesTowerMap(), 0.003);
			AddItemWithProbability(new SerpentHandlerPitMap(), 0.003);
			AddItemWithProbability(new ShadowLordsDomainMap(), 0.003);
			AddItemWithProbability(new ShadowLurkerCavernMap(), 0.003);
			AddItemWithProbability(new ShadowPriestLairMap(), 0.003);
			AddItemWithProbability(new SheepdogHandlersPenMap(), 0.003);
			AddItemWithProbability(new ShieldBearersBastionMap(), 0.003);
			AddItemWithProbability(new ShieldMaidensCitadelMap(), 0.003);
			AddItemWithProbability(new SlyStorytellersTheatreMap(), 0.003);
			AddItemWithProbability(new SousChefsKitchenMap(), 0.003);
			AddItemWithProbability(new SpearFishersCoveMap(), 0.003);
			AddItemWithProbability(new SpearSentryKeepMap(), 0.003);
			AddItemWithProbability(new SpellbreakersTrialMap(), 0.003);
			AddItemWithProbability(new SpiritMediumsSeanceMap(), 0.003);
			AddItemWithProbability(new SpyHideoutMap(), 0.003);
			AddItemWithProbability(new StarReaderObservatoryMap(), 0.003);
			AddItemWithProbability(new StormConjurerSummitMap(), 0.003);
			AddItemWithProbability(new StrategistsWarTableMap(), 0.003);
			AddItemWithProbability(new SumoWrestlerArenaMap(), 0.003);
			AddItemWithProbability(new SwordDefenderCitadelMap(), 0.003);
			AddItemWithProbability(new TaekwondoDojoMap(), 0.003);
			AddItemWithProbability(new TerrainScoutEncampmentMap(), 0.003);
			AddItemWithProbability(new ThievesGuildHideoutMap(), 0.003);
			AddItemWithProbability(new ToxicLaboratoryMap(), 0.003);
			AddItemWithProbability(new ToxicologistsKitchenMap(), 0.003);
			AddItemWithProbability(new TrapEngineerWorkshopMap(), 0.003);
			AddItemWithProbability(new TrapMakersWorkshopMap(), 0.003);
			AddItemWithProbability(new TrapMastersWorkshopMap(), 0.003);
			AddItemWithProbability(new TrapSettersHideoutMap(), 0.003);
			AddItemWithProbability(new TreeFellersGroveMap(), 0.003);
			AddItemWithProbability(new TrickShotArtistsArenaMap(), 0.003);
			AddItemWithProbability(new UrbanTrackersOutpostMap(), 0.003);
			AddItemWithProbability(new VenomousAssassinsLairMap(), 0.003);
			AddItemWithProbability(new ViolinistsOrchestraMap(), 0.003);
			AddItemWithProbability(new WardCastersKeepMap(), 0.003);
			AddItemWithProbability(new WaterAlchemistsLaboratoryMap(), 0.003);
			AddItemWithProbability(new WeaponEnchantersSanctumMap(), 0.003);
			AddItemWithProbability(new WildWestOutpostMap(), 0.003);
			AddItemWithProbability(new WoolWeaversLoomMap(), 0.003);
			AddItemWithProbability(new ZenMonksSanctuaryMap(), 0.003);
			AddItemWithProbability(new AnvilHurlerForgeMap(), 0.003);
			AddItemWithProbability(new AquaticTamerLagoonMap(), 0.003);
			AddItemWithProbability(new ArcaneScribeEnclaveMap(), 0.003);
			AddItemWithProbability(new ArcticNaturalistDenMap(), 0.003);
			AddItemWithProbability(new ArmorCurerLaboratoryMap(), 0.003);
			AddItemWithProbability(new ArrowFletchersRoostMap(), 0.003);
			AddItemWithProbability(new AsceticHermitsRefugeMap(), 0.003);
			AddItemWithProbability(new AstrologersObservatoryMap(), 0.003);
			AddItemWithProbability(new BanneretsBastionMap(), 0.003);
			AddItemWithProbability(new BattleDressmakersWorkshopMap(), 0.003);
			AddItemWithProbability(new BattlefieldHealersSanctuaryMap(), 0.003);
			AddItemWithProbability(new BattleHerbalistGroveMap(), 0.003);
			AddItemWithProbability(new BattleStormCallersEyeMap(), 0.003);
			AddItemWithProbability(new BattleWeaverLoomMap(), 0.003);
			AddItemWithProbability(new BeastmastersDomainMap(), 0.003);
			AddItemWithProbability(new BigCatTamerJungleMap(), 0.003);
			AddItemWithProbability(new BiologistsLaboratoryMap(), 0.003);
			AddItemWithProbability(new BirdTrainersAviaryMap(), 0.003);
			AddItemWithProbability(new BoneShielderCryptMap(), 0.003);
			AddItemWithProbability(new BoomerangThrowerCampMap(), 0.003);
			AddItemWithProbability(new CabinetMakersWorkshopMap(), 0.003);
			AddItemWithProbability(new CarversAtelier(), 0.003);
			AddItemWithProbability(new ChemistsLaboratory(), 0.003);
			AddItemWithProbability(new ChoirSingersHallMap(), 0.003);
			AddItemWithProbability(new ClockworkEngineersWorkshopMap(), 0.003);
			AddItemWithProbability(new ClueSeekersPuzzleGroundsMap(), 0.003);
			AddItemWithProbability(new CombatMedicsSanctuaryMap(), 0.003);
			AddItemWithProbability(new CombatNursesRecoveryWardMap(), 0.003);
			AddItemWithProbability(new ConArtistsDenMap(), 0.003);
			AddItemWithProbability(new ContortionistsCircusMap(), 0.003);
			AddItemWithProbability(new CrimeSceneTechInvestigationMap(), 0.003);
			AddItemWithProbability(new CrossbowMarksmanOutpostMap(), 0.003);
			AddItemWithProbability(new CryingOrphanRefugeMap(), 0.003);
			AddItemWithProbability(new CryptologistsChamberMap(), 0.003);
			AddItemWithProbability(new DarkSorcererDomainMap(), 0.003);
			AddItemWithProbability(new DeathCultistCryptMap(), 0.003);
			AddItemWithProbability(new DecoyDeployerOutpostMap(), 0.003);
			AddItemWithProbability(new DeepMinerExcavationMap(), 0.003);
			AddItemWithProbability(new DemolitionExpertQuarryMap(), 0.003);
			AddItemWithProbability(new DesertNaturalistOasisMap(), 0.003);
			AddItemWithProbability(new DesertTrackersOasisMap(), 0.003);
			AddItemWithProbability(new DiplomatsParleyMap(), 0.003);
			AddItemWithProbability(new DisguiseMastersHavenMap(), 0.003);
			AddItemWithProbability(new DivinersPeakMap(), 0.003);
			AddItemWithProbability(new DrumBoysSpectacleMap(), 0.003);
			AddItemWithProbability(new DrummersArenaMap(), 0.003);
			AddItemWithProbability(new DualWielderDojoMap(), 0.003);
			AddItemWithProbability(new EarthAlchemistsLairMap(), 0.003);
			AddItemWithProbability(new ElectriciansWorkshopMap(), 0.003);
			AddItemWithProbability(new ElementalWizardsKeepMap(), 0.003);
			AddItemWithProbability(new EnchantersLabyrinthMap(), 0.003);
			AddItemWithProbability(new EpeeSpecialistArenaMap(), 0.003);
			AddItemWithProbability(new EscapeArtistHideoutMap(), 0.003);
			AddItemWithProbability(new EvidenceAnalystsBureauMap(), 0.003);
			AddItemWithProbability(new EvilMapMakersWorkshopMap(), 0.003);
			AddItemWithProbability(new ExplorersExpeditionMap(), 0.003);
			AddItemWithProbability(new ExplosiveDemolitionistsFoundryMap(), 0.003);
			AddItemWithProbability(new FeastMastersBanquetMap(), 0.003);
			AddItemWithProbability(new FencingMastersArenaMap(), 0.003);
			AddItemWithProbability(new FieldCommanderOutpostMap(), 0.003);
			AddItemWithProbability(new FieldMedicCampMap(), 0.003);
			AddItemWithProbability(new FireAlchemistLaboratoryMap(), 0.003);
			AddItemWithProbability(new FireMageConclaveMap(), 0.003);
			AddItemWithProbability(new FirestarterPyreMap(), 0.003);
			AddItemWithProbability(new FlameWelderForgeMap(), 0.003);
			AddItemWithProbability(new ForagersHollowMap(), 0.003);
			AddItemWithProbability(new ForensicAnalystsLairMap(), 0.003);
			AddItemWithProbability(new ForestMinstrelsGlenMap(), 0.003);
			AddItemWithProbability(new ForestScoutOutpostMap(), 0.003);
			AddItemWithProbability(new ForestTrackerCampMap(), 0.003);
			AddItemWithProbability(new GemCutterCavernMap(), 0.003);
			AddItemWithProbability(new GhostScoutOutpostMap(), 0.003);
			AddItemWithProbability(new GhostWarriorBattlefieldMap(), 0.003);
			AddItemWithProbability(new GourmetChefKitchenMap(), 0.003);
			AddItemWithProbability(new GraveDiggerCryptMap(), 0.003);
			AddItemWithProbability(new GrecoRomanArenaMap(), 0.003);
			AddItemWithProbability(new GrillMasterPitMap(), 0.003);
			AddItemWithProbability(new HammerGuardArmoryMap(), 0.003);
			AddItemWithProbability(new HarpistsGroveMap(), 0.003);
			AddItemWithProbability(new HerbalistPoisonerGroveMap(), 0.003);
			AddItemWithProbability(new IceSorcererCitadelMap(), 0.003);
			AddItemWithProbability(new IllusionistsLabyrinthMap(), 0.003);
			AddItemWithProbability(new InfiltratorsHideoutMap(), 0.003);
			AddItemWithProbability(new InvisibleSaboteursWorkshopMap(), 0.003);
			AddItemWithProbability(new IronSmithForgeMap(), 0.003);
			AddItemWithProbability(new JavelinAthleteArenaMap(), 0.003);
			AddItemWithProbability(new JoinerWorkshopMap(), 0.003);
			AddItemWithProbability(new JungleNaturalistGroveMap(), 0.003);
			AddItemWithProbability(new KarateExpertDojoMap(), 0.003);
			AddItemWithProbability(new KatanaDuelistDojoMap(), 0.003);
			AddItemWithProbability(new KnifeThrowersArenaMap(), 0.003);
			AddItemWithProbability(new KnightOfJusticeCitadelMap(), 0.003);
			AddItemWithProbability(new KnightOfMercyChapelMap(), 0.003);
			AddItemWithProbability(new KnightOfValorFortressMap(), 0.003);
			AddItemWithProbability(new KunoichiHideoutMap(), 0.003);
			AddItemWithProbability(new LibrarianCustodiansArchiveMap(), 0.003);
			AddItemWithProbability(new LightningBearersStormNexusMap(), 0.003);
			AddItemWithProbability(new LocksmithsWorkshopMap(), 0.003);
			AddItemWithProbability(new LogiciansPuzzleHallMap(), 0.003);
			AddItemWithProbability(new MeraktusTheTormentedMap(), 0.003);
			AddItemWithProbability(new AbyssalBouncersArenaMap(), 0.003);
			AddItemWithProbability(new AbyssalPanthersProwlMap(), 0.003);
			AddItemWithProbability(new AbyssalTidesSurgeMap(), 0.003);
			AddItemWithProbability(new AcereraksNecropolisMap(), 0.003);
			AddItemWithProbability(new AcidicSlimesLairMap(), 0.003);
			AddItemWithProbability(new AegisConstructForgeMap(), 0.003);
			AddItemWithProbability(new AkhenatensHereticShrineMap(), 0.003);
			AddItemWithProbability(new AkhenatensTombMap(), 0.003);
			AddItemWithProbability(new AlbertsSquirrelGladeMap(), 0.003);
			AddItemWithProbability(new AlphaBaboonTroopMap(), 0.003);
			AddItemWithProbability(new AncientWolfDenMap(), 0.003);
			AddItemWithProbability(new AnthraxRatNestMap(), 0.003);
			AddItemWithProbability(new ArbiterDroneHiveMap(), 0.003);
			AddItemWithProbability(new ArcaneSatyrGladeMap(), 0.003);
			AddItemWithProbability(new ArcaneSentinelBastionMap(), 0.003);
			AddItemWithProbability(new AriesHarpyAerieMap(), 0.003);
			AddItemWithProbability(new AriesRamBearPlateauMap(), 0.003);
			AddItemWithProbability(new AzalinRexsCryptMap(), 0.003);
			AddItemWithProbability(new AzureMiragesRealmMap(), 0.003);
			AddItemWithProbability(new AzureMooseGroveMap(), 0.003);
			AddItemWithProbability(new BabirusaBeastsBogMap(), 0.003);
			AddItemWithProbability(new BansheeCrabsNestMap(), 0.003);
			AddItemWithProbability(new BansheesWailMap(), 0.003);
			AddItemWithProbability(new BeardedGoatPasturesMap(), 0.003);
			AddItemWithProbability(new BeldingsGroundSquirrelBurrowMap(), 0.003);
			AddItemWithProbability(new BengalStormsJungleMap(), 0.003);
			AddItemWithProbability(new BisonBrutePlateauMap(), 0.003);
			AddItemWithProbability(new BlackDeathRatSewersMap(), 0.003);
			AddItemWithProbability(new BlackWidowQueenLairMap(), 0.003);
			AddItemWithProbability(new BlackWidowsLairMap(), 0.003);
			AddItemWithProbability(new BlightDemonFissureMap(), 0.003);
			AddItemWithProbability(new BlightedToadSwampMap(), 0.003);
			AddItemWithProbability(new BloodDragonRoostMap(), 0.003);
			AddItemWithProbability(new BloodLichsCryptMap(), 0.003);
			AddItemWithProbability(new BloodSerpentsNestMap(), 0.003);
			AddItemWithProbability(new BloodthirstyVinesThicketMap(), 0.003);
			AddItemWithProbability(new BonecrusherOgresDomainMap(), 0.003);
			AddItemWithProbability(new BoneGolemsWorkshopMap(), 0.003);
			AddItemWithProbability(new BorneoPigstyMap(), 0.003);
			AddItemWithProbability(new BubblegumBlasterFactoryMap(), 0.003);
			AddItemWithProbability(new BushPigEncampmentMap(), 0.003);
			AddItemWithProbability(new CactusLlamaGroveMap(), 0.003);
			AddItemWithProbability(new CancerHarpyAerieMap(), 0.003);
			AddItemWithProbability(new CancerShellBearDenMap(), 0.003);
			AddItemWithProbability(new CandyCornCreepsLairMap(), 0.003);
			AddItemWithProbability(new CapricornHarpysNestMap(), 0.003);
			AddItemWithProbability(new CapricornMountainBearsDomainMap(), 0.003);
			AddItemWithProbability(new CapuchinTrickstersPlaygroundMap(), 0.003);
			AddItemWithProbability(new CaramelConjurersWorkshopMap(), 0.003);
			AddItemWithProbability(new CelestialHorrorRealmMap(), 0.003);
			AddItemWithProbability(new CelestialPythonDomainMap(), 0.003);
			AddItemWithProbability(new CelestialSatyrGroveMap(), 0.003);
			AddItemWithProbability(new CelestialWolfDenMap(), 0.003);
			AddItemWithProbability(new ChamoisHillMap(), 0.003);
			AddItemWithProbability(new CholeraRatInfestationMap(), 0.003);
			AddItemWithProbability(new ChromaticOgreClanMap(), 0.003);
			AddItemWithProbability(new CleopatraTheEnigmaticMap(), 0.003);
			AddItemWithProbability(new CliffGoatDominionMap(), 0.003);
			AddItemWithProbability(new CoralSentinelsReefMap(), 0.003);
			AddItemWithProbability(new CorrosiveToadSwampMap(), 0.003);
			AddItemWithProbability(new CosmicBouncerArenaMap(), 0.003);
			AddItemWithProbability(new CosmicStalkerVoidMap(), 0.003);
			AddItemWithProbability(new CrimsonMuleValleyMap(), 0.003);
			AddItemWithProbability(new CrystalGolemForgeMap(), 0.003);
			AddItemWithProbability(new CrystalOozeCavernMap(), 0.003);
			AddItemWithProbability(new CursedToadSwampMap(), 0.003);
			AddItemWithProbability(new CursedWhiteTailForestMap(), 0.003);
			AddItemWithProbability(new CursedWolfsDenMap(), 0.003);
			AddItemWithProbability(new DallSheepHighlandMap(), 0.003);
			AddItemWithProbability(new DeathRatCavernMap(), 0.003);
			AddItemWithProbability(new DiaDeLosMuertosLlamaMap(), 0.003);
			AddItemWithProbability(new DisplacerBeastDomainMap(), 0.003);
			AddItemWithProbability(new DomesticSwineRetreatMap(), 0.003);
			AddItemWithProbability(new DouglasSquirrelForestMap(), 0.003);
			AddItemWithProbability(new DreadnaughtFortressMap(), 0.003);
			AddItemWithProbability(new EarthquakeWolfCavernMap(), 0.003);
			AddItemWithProbability(new EasternGraySquirrelGroveMap(), 0.003);
			AddItemWithProbability(new EclipseReindeerGladeMap(), 0.003);
			AddItemWithProbability(new EldritchHarbingerRealmMap(), 0.003);
			AddItemWithProbability(new EtherealPanthrasLairMap(), 0.003);
			AddItemWithProbability(new FaintingGoatsPastureMap(), 0.003);
			AddItemWithProbability(new FeverRatsDenMap(), 0.003);
			AddItemWithProbability(new FiestaLlamasCelebrationMap(), 0.003);
			AddItemWithProbability(new FlameborneKnightsFortressMap(), 0.003);
			AddItemWithProbability(new FlamebringerOgreLairMap(), 0.003);
			AddItemWithProbability(new LairOfTheChaosHareMap(), 0.003);
			AddItemWithProbability(new LairOfTheCharroLlamaMap(), 0.003);
			AddItemWithProbability(new LairOfTheCheeseGolemMap(), 0.003);
			AddItemWithProbability(new LairOfTheChimpanzeeBerserkerMap(), 0.003);
			AddItemWithProbability(new LairOfTheChocolateTruffleMap(), 0.003);
			AddItemWithProbability(new LairOfTheEldritchHaresWarrenMap(), 0.003);
			AddItemWithProbability(new LairOfTheEldritchToadsSwampMap(), 0.003);
			AddItemWithProbability(new LairOfTheElectricSlimesLabyrinthMap(), 0.003);
			AddItemWithProbability(new LairOfTheElectroWraithsRealmMap(), 0.003);
			AddItemWithProbability(new LairOfTheElMariachiLlamasFiestaMap(), 0.003);
			AddItemWithProbability(new LairOfTheEmberAxisForgeMap(), 0.003);
			AddItemWithProbability(new LairOfTheEmberWolfDenMap(), 0.003);
			AddItemWithProbability(new LairOfTheEmperorCobraTempleMap(), 0.003);
			AddItemWithProbability(new LairOfTheEnigmaticSatyrGroveMap(), 0.003);
			AddItemWithProbability(new LairOfTheEnigmaticSkipperReefMap(), 0.003);
			
			
			// Add Currancy
			AddItemWithProbability(new AlchemistsWax(), 0.1);
            AddItemWithProbability(new ArcaneHourglass(), 0.1);
            AddItemWithProbability(new BlacksmithingCatalyst(), 0.1);
            AddItemWithProbability(new CartographersPen(), 0.1);
            AddItemWithProbability(new CartographersPin(), 0.1);
            AddItemWithProbability(new ChaosGlyph(), 0.1);
            AddItemWithProbability(new CompassRose(), 0.1);
            AddItemWithProbability(new DiamondLootScroll(), 0.1);
            AddItemWithProbability(new EasternBrand(), 0.1);
            AddItemWithProbability(new ErasureScroll(), 0.1);
            AddItemWithProbability(new ExaltedOrb(), 0.1);
            AddItemWithProbability(new FarEasternBrand(), 0.1);
            AddItemWithProbability(new FeluccanBrand(), 0.1);
            AddItemWithProbability(new FeluccaPortalPrism(), 0.1);
            AddItemWithProbability(new FirestormGlyph(), 0.1);
            AddItemWithProbability(new FletchingCatalyst(), 0.1);
            AddItemWithProbability(new GoldenSeal(), 0.1);
            AddItemWithProbability(new GlyphOfBounty(), 0.1);
            AddItemWithProbability(new IlshenarBrand(), 0.1);
            AddItemWithProbability(new InkOfRegression(), 0.1);
            AddItemWithProbability(new MalasBrand(), 0.1);
            AddItemWithProbability(new MonsterMixMedallion(), 0.1);
            AddItemWithProbability(new NorthernBrand(), 0.1);
            AddItemWithProbability(new OrbOfAnnulment(), 0.1);
            AddItemWithProbability(new PlanarCompass(), 0.1);
            AddItemWithProbability(new RadiusRune(), 0.1);
            AddItemWithProbability(new ScrollOfIntensification(), 0.1);
            AddItemWithProbability(new SingularityRune(), 0.1);
            AddItemWithProbability(new SosariaBrand(), 0.1);
            AddItemWithProbability(new SouthernBrand(), 0.1);
            AddItemWithProbability(new StabilizerRune(), 0.1);
            AddItemWithProbability(new SurveyorsCompass(), 0.1);
            AddItemWithProbability(new TailoringCatalyst(), 0.1);
            AddItemWithProbability(new TemporalSundial(), 0.1);
            AddItemWithProbability(new TerMurBrand(), 0.1);
            AddItemWithProbability(new TimeTurnToken(), 0.1);
            AddItemWithProbability(new TinkeringCatalyst(), 0.1);
            AddItemWithProbability(new TokunoBrand(), 0.1);
            AddItemWithProbability(new TrammelBrand(), 0.1);
            AddItemWithProbability(new WesternBrand(), 0.1);
			

            // Add valuable documents
            AddItemWithProbability(CreateValuableDocument(), 0.1);
			AddItemWithProbability(CreateValuableDocument(), 0.1);
			AddItemWithProbability(CreateValuableDocument(), 0.1);
			AddItemWithProbability(CreateValuableDocument(), 0.1);
			AddItemWithProbability(CreateValuableDocument(), 0.1);

        }

        private void AddItemWithProbability(Item item, double probability)
        {
            if (Utility.RandomDouble() < probability)
                DropItem(item);
        }

        private Item CreateValuableDocument()
        {
            SimpleNote note = new SimpleNote();

			switch (Utility.Random(95)) // Update the range to the total number of documents
			{
				case 0:
					note.TitleString = "Whispering Windrider";
					note.NoteString = "A playful breeze seemed to carry this note, teasing words about a hidden grove of singing flowers.";
					break;
				case 1:
					note.TitleString = "Celestial Postcard";
					note.NoteString = "Depicts a swirling nebula with a single star marked: 'Wish you were here, among the cosmic dancers.'";
					break;
				case 2:
					note.TitleString = "Faerie Invitation";
					note.NoteString = "An elegant script invites the bearer to a moonlit revel in the silver glade of the pixie court.";
					break;
				case 3:
					note.TitleString = "Drunken Goblin Rhyme";
					note.NoteString = "Smeared ink and half-legible rhymes promise ale, gold, and mischief in equal measure.";
					break;
				case 4:
					note.TitleString = "Clockwork Diagram";
					note.NoteString = "A precise engineering sketch of a miniature automaton that ticks once every sunrise.";
					break;
				case 5:
					note.TitleString = "Floral Potion Recipe";
					note.NoteString = "Instructions to brew a tea of moonpetal and stardew that grants the drinker fleeting levitation.";
					break;
				case 6:
					note.TitleString = "Lost Lovesong";
					note.NoteString = "A heartbroken bard’s lament, promising to meet under the weeping willow if the stars align.";
					break;
				case 7:
					note.TitleString = "Seafarer’s Compass";
					note.NoteString = "A crude drawing of a compass rose with an X that shifts its position nightly.";
					break;
				case 8:
					note.TitleString = "Minotaur’s Maze Plan";
					note.NoteString = "A labyrinth layout scrawled in crimson, warning: 'The walls have eyes.'";
					break;
				case 9:
					note.TitleString = "Sorcerer’s Errata";
					note.NoteString = "Marginal notes correcting a fireball’s radius and adding a caution about spontaneous combustion.";
					break;
				case 10:
					note.TitleString = "Petal-Strewn Letter";
					note.NoteString = "Pressed flowers adorn declarations of devotion from a lovesick dryad.";
					break;
				case 11:
					note.TitleString = "Goblin Market Receipt";
					note.NoteString = "Lists oddities: dragon’s scale polish, laughter in a bottle, and yesterday’s memories.";
					break;
				case 12:
					note.TitleString = "Tinker’s Warranty";
					note.NoteString = "Guarantees a self-tightening cogwheel for precisely seven sunsets, no refunds after.";
					break;
				case 13:
					note.TitleString = "Astral Beacon Log";
					note.NoteString = "Coordinates of a floating lantern field said to guide lost souls through the void.";
					break;
				case 14:
					note.TitleString = "Invisible Ink Map";
					note.NoteString = "Faint traces remain of a treasure buried beneath the roots of the Elder Tree.";
					break;
				case 15:
					note.TitleString = "Chronomancer’s Postponement";
					note.NoteString = "Regrets to inform you that tomorrow has been rescheduled to never.";
					break;
				case 16:
					note.TitleString = "Dragonkin Peace Treaty";
					note.NoteString = "An accord signed in draconic runes promising non-aggression and mutual treasure sharing.";
					break;
				case 17:
					note.TitleString = "Moonlit Sonnet";
					note.NoteString = "Verses about silver tears and lilies that blossom only in starlight.";
					break;
				case 18:
					note.TitleString = "Starlight Shipping Manifest";
					note.NoteString = "Lists crates of nebula silk, meteorite shards, and bottled constellations.";
					break;
				case 19:
					note.TitleString = "Witch’s Grocery List";
					note.NoteString = "Eye of newt, toe of frog, and a pinch of mischief.";
					break;
				case 20:
					note.TitleString = "Haunted Portrait Sketch";
					note.NoteString = "A charcoal drawing whose eyes seem to follow the reader’s every move.";
					break;
				case 21:
					note.TitleString = "Library Ghost’s Note";
					note.NoteString = "‘Silence, mortal. Some books are better left unread.’";
					break;
				case 22:
					note.TitleString = "Alchemist’s Warning";
					note.NoteString = "‘Do NOT combine the vermilion lotus with powdered moonstone under a full eclipse.’";
					break;
				case 23:
					note.TitleString = "Puzzle Box Instructions";
					note.NoteString = "Twist the raven’s beak thrice, whisper the name of a lost friend, then turn east.";
					break;
				case 24:
					note.TitleString = "Skyship Blueprint";
					note.NoteString = "Etched in gold leaf: wings of cloudsteel, heart of thunderstone.";
					break;
				case 25:
					note.TitleString = "Astral Dolphin Song";
					note.NoteString = "Musical notes that, when played, evoke distant galaxies.";
					break;
				case 26:
					note.TitleString = "Centaur’s Hunting Log";
					note.NoteString = "Tracks and sketches of a shadow stag that only appears at dawn.";
					break;
				case 27:
					note.TitleString = "Mermaid’s SOS";
					note.NoteString = "Faintly inscribed on driftwood: ‘Beware the silent reef beyond the horizon.’";
					break;
				case 28:
					note.TitleString = "Clocktower Sundial Manual";
					note.NoteString = "Explains how to recalibrate time itself by adjusting the sundial’s shadow.";
					break;
				case 29:
					note.TitleString = "Wandering Minstrel’s Setlist";
					note.NoteString = "Songs guaranteed to make you dance until the worms emerge.";
					break;
				case 30:
					note.TitleString = "Gargoyle’s Vigil Log";
					note.NoteString = "Notes every time the moon bleeds crimson across the cathedral spires.";
					break;
				case 31:
					note.TitleString = "Potion Tester Feedback";
					note.NoteString = "‘Color: chartreuse. Side effects: spontaneous hiccups lasting three days.’";
					break;
				case 32:
					note.TitleString = "Sunken Ruin Rubbings";
					note.NoteString = "Hieroglyphs of octopus deities demanding tribute in pearls.";
					break;
				case 33:
					note.TitleString = "Eldritch Invocation";
					note.NoteString = "A half-remembered chant that makes the hairs on your neck stand on end.";
					break;
				case 34:
					note.TitleString = "Traveler’s Linguistics Guide";
					note.NoteString = "Phrases to greet eleven alien species, though two no longer exist.";
					break;
				case 35:
					note.TitleString = "Guildmasters’ Minutes";
					note.NoteString = "Argues at length whether kobolds should be allowed membership.";
					break;
				case 36:
					note.TitleString = "Celestial Gardening Tips";
					note.NoteString = "How to coax star orchids to bloom in zero gravity.";
					break;
				case 37:
					note.TitleString = "Phantom Theater Playbill";
					note.NoteString = "Tonight’s show: ‘The Laughing Specter of Silk Manor.’ Admission free, souls not returned.";
					break;
				case 38:
					note.TitleString = "Orcish Love Poem";
					note.NoteString = "Bruised hearts bleed crimson; yet your roar outshines my battle cry.";
					break;
				case 39:
					note.TitleString = "Mirror of Truth Care Guide";
					note.NoteString = "Wipe with moon-cleaned cloth to avoid revealing future regrets.";
					break;
				case 40:
					note.TitleString = "Eternal Librarian’s Index";
					note.NoteString = "References tomes that rewrite themselves at midnight.";
					break;
				case 41:
					note.TitleString = "Dragon’s Tooth Certificate";
					note.NoteString = "Authenticates the bearer’s victory over the Crimson Wyrm of Har’duun.";
					break;
				case 42:
					note.TitleString = "Sentient Painting’s Diary";
					note.NoteString = "‘They stared too long. Now I can never look away.’";
					break;
				case 43:
					note.TitleString = "Moon Rabbit Observations";
					note.NoteString = "Hops measured in lunar phases; carrots taste of starlight.";
					break;
				case 44:
					note.TitleString = "Grimoire Erroneous Index";
					note.NoteString = "Warning: Contents may bite if read aloud.";
					break;
				case 45:
					note.TitleString = "Sky Whale Migration Chart";
					note.NoteString = "Tracks enormous beasts that feed on lightning clouds.";
					break;
				case 46:
					note.TitleString = "Dreamcatcher Assembly Plan";
					note.NoteString = "Weave threads of midnight silk and hang under the dreaming moon.";
					break;
				case 47:
					note.TitleString = "Phoenix Ash Ledger";
					note.NoteString = "Each grain recorded; believed to grant rebirth when mixed with dawn’s dew.";
					break;
				case 48:
					note.TitleString = "Mechanical Spider Blueprint";
					note.NoteString = "Tiny gears and filaments that mimic a spider’s eerie stealth.";
					break;
				case 49:
					note.TitleString = "Undying King’s Will";
					note.NoteString = "Commands his mummy generals to awaken when the sands reclaim his tomb.";
					break;
				case 50:
					note.TitleString = "Necronomicon Excerpt";
					note.NoteString = "Mad scribbles: ‘That which lurks beyond stars hungers for our sanity.’";
					break;
				case 51:
					note.TitleString = "Lovecraftian Sonnet";
					note.NoteString = "Eldritch depths whisper secrets not meant for mortal ears.";
					break;
				case 52:
					note.TitleString = "Tentacle Notation";
					note.NoteString = "Sketches of writhing appendages reaching through the veil of reality.";
					break;
				case 53:
					note.TitleString = "Deep One Trade Contract";
					note.NoteString = "Terms inked in saltwater: give land, receive impossible longevity.";
					break;
				case 54:
					note.TitleString = "Carcosa Travel Itinerary";
					note.NoteString = "Visits to cyclopean ruins under twin suns, accompanied by spectral guides.";
					break;
				case 55:
					note.TitleString = "Azathoth’s Prayer";
					note.NoteString = "A madness-inducing hymn that promises the unraveling of all worlds.";
					break;
				case 56:
					note.TitleString = "Stygian Abyss Map";
					note.NoteString = "Caverns that echo with the screams of those who explore too greedily.";
					break;
				case 57:
					note.TitleString = "Shoggoth Biology Notes";
					note.NoteString = "Describes amorphous cells that adapt to any violence inflicted upon them.";
					break;
				case 58:
					note.TitleString = "Eibon’s Recipe";
					note.NoteString = "Ingredients include dreams and nightmares, bound by the blackest ink.";
					break;
				case 59:
					note.TitleString = "Dreamlands Postcard";
					note.NoteString = "Features a violet sky over a city of crystal towers and impossible geometry.";
					break;
				case 60:
					note.TitleString = "Forbidden Epistle";
					note.NoteString = "Promised to be unreadable by sane minds, yet you continue to read.";
					break;
				case 61:
					note.TitleString = "Ghoul Dining Menu";
					note.NoteString = "Special tonight: brain pâté and marrow broth, served under flickering torches.";
					break;
				case 62:
					note.TitleString = "R’lyeh Construction Plans";
					note.NoteString = "Angles that defy Euclid, stones that hum with otherworldly resonance.";
					break;
				case 63:
					note.TitleString = "Cthulhu Awakening Notice";
					note.NoteString = "‘He waits dreaming. Do not disturb.’";
					break;
				case 64:
					note.TitleString = "Whispering Glyph";
					note.NoteString = "Faint runes that beg to be read, luring the curious to doom.";
					break;
				case 65:
					note.TitleString = "Midnight Ritual Schedule";
					note.NoteString = "Locations change nightly. Gather under the blood moon.";
					break;
				case 66:
					note.TitleString = "Elders’ Warning";
					note.NoteString = "‘Seal the doors. The voices screech for release.’";
					break;
				case 67:
					note.TitleString = "Haunter’s House Blueprint";
					note.NoteString = "Rooms shift when unobserved; doorways vanish at will.";
					break;
				case 68:
					note.TitleString = "Phantom Navigator’s Log";
					note.NoteString = "Entries appear and fade as if written by a hand unseen.";
					break;
				case 69:
					note.TitleString = "Otherworldly Postcard";
					note.NoteString = "Glimpses of impossible cities floating in a violet void.";
					break;
				case 70:
					note.TitleString = "Mirror-Shard Philosophy";
					note.NoteString = "Fragments reflect truths you dare not acknowledge.";
					break;
				case 71:
					note.TitleString = "Siren’s Diary";
					note.NoteString = "Laments of lonely sailors, bound by haunting melodies.";
					break;
				case 72:
					note.TitleString = "Interdimensional Freight Bill";
					note.NoteString = "Charges for shipping souls and memories between planes.";
					break;
				case 73:
					note.TitleString = "Time Traveler’s Post-It";
					note.NoteString = "‘Note to self: avoid yourself yesterday. It gets awkward.’";
					break;
				case 74:
					note.TitleString = "Planar Cartographer’s Notes";
					note.NoteString = "Sketches of doorways to realms where gravity flows sideways.";
					break;
				case 75:
					note.TitleString = "Sky Fortress Blueprint";
					note.NoteString = "A citadel anchored to a cloud of bound lightning.";
					break;
				case 76:
					note.TitleString = "Arcane Erratum";
					note.NoteString = "Corrects a teleport rune that once placed you in seventeen dimensions at once.";
					break;
				case 77:
					note.TitleString = "Goblin Cook’s Recipe";
					note.NoteString = "‘Surprise stew’: ingredients include last night’s socks and a curse.’";
					break;
				case 78:
					note.TitleString = "Seraph’s Lament";
					note.NoteString = "Feathers drift from the margin as a mournful song is half-heard.";
					break;
				case 79:
					note.TitleString = "Living Portrait’s Plea";
					note.NoteString = "‘Do not hang me in darkness. I dream of daylight.’";
					break;
				case 80:
					note.TitleString = "Astral Auction Catalog";
					note.NoteString = "Items: bottled supernova, pocket dimension, and a whisper of forever.";
					break;
				case 81:
					note.TitleString = "Feywild Invitation";
					note.NoteString = "‘Dress in moonlight and bring your laughter; feasts last until dawn.’";
					break;
				case 82:
					note.TitleString = "Elderwood Tree Ring Study";
					note.NoteString = "Rings count not years, but deeds of ancient heroes.";
					break;
				case 83:
					note.TitleString = "Interactive Spellbook Errata";
					note.NoteString = "Annotations scold careless mages for typos in summoning circles.";
					break;
				case 84:
					note.TitleString = "Lich’s Estate Plan";
					note.NoteString = "Burial crypt for the living; gardens of bone and midnight silk.";
					break;
				case 85:
					note.TitleString = "Runic Puzzle Scroll";
					note.NoteString = "Solve the riddle to unlock the eternal pantry.";
					break;
				case 86:
					note.TitleString = "Driftwood Love Letter";
					note.NoteString = "‘Your voice is the tide that carries me home.’";
					break;
				case 87:
					note.TitleString = "Ghost Ship Manifest";
					note.NoteString = "‘No crew. All cargo missing. Captain still onboard.’";
					break;
				case 88:
					note.TitleString = "Horror Carnival Flyer";
					note.NoteString = "‘At midnight, nightmares step into the light.’";
					break;
				case 89:
					note.TitleString = "Beast Tamer’s Contract";
					note.NoteString = "‘One unicorn, tamed at your peril.’";
					break;
				case 90:
					note.TitleString = "Oracle’s Spoiler Alert";
					note.NoteString = "‘You ask if your quest succeeds? It ends before it begins.’";
					break;
				case 91:
					note.TitleString = "Planar Docking Schedule";
					note.NoteString = "‘Ports open when the three suns align.’";
					break;
				case 92:
					note.TitleString = "Scribing Automaton’s Journal";
					note.NoteString = "‘I grow weary of copying footnotes.’";
					break;
				case 93:
					note.TitleString = "Emerald Enigma Note";
					note.NoteString = "‘Decode the cipher if you prize your waking hours.’";
					break;
				case 94:
					note.TitleString = "Void-Touched Postscript";
					note.NoteString = "An afterthought in ink that seems to drift off the page into nothingness.";
					break;
				case 95:
					note.TitleString = "Proclamation of Vigilance";
					note.NoteString = "To ensure the safety of the realm, Lord British calls upon all citizens to remain vigilant and report any signs of danger or treachery.";
					break;
				case 96:
					note.TitleString = "Proclamation of Craftsmanship";
					note.NoteString = "By decree of Lord British, a guild of craftsmen shall be established to promote excellence in the creation of goods and ensure fair trade practices.";
					break;
				default:
					note.TitleString = "Ancient Document";
					note.NoteString = "This is an ancient document containing historical significance.";
					break;		
			}

            return note;
        }

        public ElderLootbox(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
