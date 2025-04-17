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
    public class GearLootbox : LockableContainer
    {
        [Constructable]
        public GearLootbox() : base(0xE41) // Treasure Chest item ID
        {
            Name = "Gear Lootbox";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add gold
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 1.0);
			AddItemWithProbability(new AegisOfAthena(), 0.004);
			AddItemWithProbability(new AegisOfValor(), 0.004);
			AddItemWithProbability(new AlchemistsAmbition(), 0.004);
			AddItemWithProbability(new AlchemistsConduit(), 0.004);
			AddItemWithProbability(new AlchemistsGroundedBoots(), 0.004);
			AddItemWithProbability(new AlchemistsHeart(), 0.004);
			AddItemWithProbability(new AlchemistsPreciseGloves(), 0.004);
			AddItemWithProbability(new AlchemistsResilientLeggings(), 0.004);
			AddItemWithProbability(new AlchemistsVisionaryHelm(), 0.004);
			AddItemWithProbability(new ApronOfFlames(), 0.004);
			AddItemWithProbability(new ArkainesValorArms(), 0.004);
			AddItemWithProbability(new ArtisansCraftedGauntlets(), 0.004);
			AddItemWithProbability(new ArtisansHelm(), 0.004);
			AddItemWithProbability(new AshlandersResilience(), 0.004);
			AddItemWithProbability(new AstartesBattlePlate(), 0.004);
			AddItemWithProbability(new AstartesGauntletsOfMight(), 0.004);
			AddItemWithProbability(new AstartesHelmOfVigilance(), 0.004);
			AddItemWithProbability(new AstartesShoulderGuard(), 0.004);
			AddItemWithProbability(new AstartesWarBoots(), 0.004);
			AddItemWithProbability(new AstartesWarGreaves(), 0.004);
			AddItemWithProbability(new AtzirisStep(), 0.004);
			AddItemWithProbability(new AVALANCHEDefender(), 0.004);
			AddItemWithProbability(new AvatarsVestments(), 0.004);
			AddItemWithProbability(new BardsNimbleStep(), 0.004);
			AddItemWithProbability(new BeastmastersCrown(), 0.004);
			AddItemWithProbability(new BeastmastersGrips(), 0.004);
			AddItemWithProbability(new BeastsWhisperersRobe(), 0.004);
			AddItemWithProbability(new BerserkersEmbrace(), 0.004);
			AddItemWithProbability(new BlackMagesMysticRobe(), 0.004);
			AddItemWithProbability(new BlackMagesRuneRobe(), 0.004);
			AddItemWithProbability(new BlacksmithsBurden(), 0.004);
			AddItemWithProbability(new BlackthornesSpur(), 0.004);
			AddItemWithProbability(new BladedancersCloseHelm(), 0.004);
			AddItemWithProbability(new BladedancersOrderShield(), 0.004);
			AddItemWithProbability(new BladedancersPlateArms(), 0.004);
			AddItemWithProbability(new BladeDancersPlateChest(), 0.004);
			AddItemWithProbability(new BladeDancersPlateLegs(), 0.004);
			AddItemWithProbability(new BlazePlateLegs(), 0.004);
			AddItemWithProbability(new BombDisposalPlate(), 0.004);
			AddItemWithProbability(new BootsOfBalladry(), 0.004);
			AddItemWithProbability(new BootsOfFleetness(), 0.004);
			AddItemWithProbability(new BootsOfSwiftness(), 0.004);
			AddItemWithProbability(new BootsOfTheNetherTraveller(), 0.004);
			AddItemWithProbability(new CarpentersCrown(), 0.004);
			AddItemWithProbability(new CelesRunebladeBuckler(), 0.004);
			AddItemWithProbability(new CetrasBlessing(), 0.004);
			AddItemWithProbability(new ChefsHatOfFocus(), 0.004);
			AddItemWithProbability(new CourtesansDaintyBuckler(), 0.004);
			AddItemWithProbability(new CourtesansFlowingRobe(), 0.004);
			AddItemWithProbability(new CourtesansGracefulHelm(), 0.004);
			AddItemWithProbability(new CourtesansWhisperingBoots(), 0.004);
			AddItemWithProbability(new CourtesansWhisperingGloves(), 0.004);
			AddItemWithProbability(new CourtierDashingBoots(), 0.004);
			AddItemWithProbability(new CourtiersEnchantedAmulet(), 0.004);
			AddItemWithProbability(new CourtierSilkenRobe(), 0.004);
			AddItemWithProbability(new CourtiersRegalCirclet(), 0.004);
			AddItemWithProbability(new CovensShadowedHood(), 0.004);
			AddItemWithProbability(new CreepersLeatherCap(), 0.004);
			AddItemWithProbability(new CrownOfTheAbyss(), 0.004);
			AddItemWithProbability(new DaedricWarHelm(), 0.004);
			AddItemWithProbability(new DarkFathersCrown(), 0.004);
			AddItemWithProbability(new DarkFathersDreadnaughtBoots(), 0.004);
			AddItemWithProbability(new DarkFathersHeartplate(), 0.004);
			AddItemWithProbability(new DarkFathersSoulGauntlets(), 0.004);
			AddItemWithProbability(new DarkFathersVoidLeggings(), 0.004);
			AddItemWithProbability(new DarkKnightsCursedChestplate(), 0.004);
			AddItemWithProbability(new DarkKnightsDoomShield(), 0.004);
			AddItemWithProbability(new DarkKnightsObsidianHelm(), 0.004);
			AddItemWithProbability(new DarkKnightsShadowedGauntlets(), 0.004);
			AddItemWithProbability(new DarkKnightsVoidLeggings(), 0.004);
			AddItemWithProbability(new DemonspikeGuard(), 0.004);
			AddItemWithProbability(new DespairsShadow(), 0.004);
			AddItemWithProbability(new Doombringer(), 0.004);
			AddItemWithProbability(new DragonbornChestplate(), 0.004);
			AddItemWithProbability(new DragonsBulwark(), 0.004);
			AddItemWithProbability(new DragoonsAegis(), 0.004);
			AddItemWithProbability(new DwemerAegis(), 0.004);
			AddItemWithProbability(new EbonyChainArms(), 0.004);
			AddItemWithProbability(new EdgarsEngineerChainmail(), 0.004);
			AddItemWithProbability(new EldarRuneGuard(), 0.004);
			AddItemWithProbability(new ElixirProtector(), 0.004);
			AddItemWithProbability(new EmberPlateArms(), 0.004);
			AddItemWithProbability(new EnderGuardiansChestplate(), 0.004);
			AddItemWithProbability(new ExodusBarrier(), 0.004);
			AddItemWithProbability(new FalconersCoif(), 0.004);
			AddItemWithProbability(new FlamePlateGorget(), 0.004);
			AddItemWithProbability(new FortunesGorget(), 0.004);
			AddItemWithProbability(new FortunesHelm(), 0.004);
			AddItemWithProbability(new FortunesPlateArms(), 0.004);
			AddItemWithProbability(new FortunesPlateChest(), 0.004);
			AddItemWithProbability(new FortunesPlateLegs(), 0.004);
			AddItemWithProbability(new FrostwardensBascinet(), 0.004);
			AddItemWithProbability(new FrostwardensPlateChest(), 0.004);
			AddItemWithProbability(new FrostwardensPlateGloves(), 0.004);
			AddItemWithProbability(new FrostwardensPlateLegs(), 0.004);
			AddItemWithProbability(new FrostwardensWoodenShield(), 0.004);
			AddItemWithProbability(new GauntletsOfPrecision(), 0.004);
			AddItemWithProbability(new GauntletsOfPurity(), 0.004);
			AddItemWithProbability(new GauntletsOfTheWild(), 0.004);
			AddItemWithProbability(new GloomfangChain(), 0.004);
			AddItemWithProbability(new GlovesOfTheSilentAssassin(), 0.004);
			AddItemWithProbability(new GlovesOfTransmutation(), 0.004);
			AddItemWithProbability(new GoronsGauntlets(), 0.004);
			AddItemWithProbability(new GreyWanderersStride(), 0.004);
			AddItemWithProbability(new GuardianAngelArms(), 0.004);
			AddItemWithProbability(new GuardianOfTheAbyss(), 0.004);
			AddItemWithProbability(new GuardiansHeartplate(), 0.004);
			AddItemWithProbability(new GuardiansHelm(), 0.004);
			AddItemWithProbability(new HammerlordsArmguards(), 0.004);
			AddItemWithProbability(new HammerlordsChestplate(), 0.004);
			AddItemWithProbability(new HammerlordsHelm(), 0.004);
			AddItemWithProbability(new HammerlordsLegplates(), 0.004);
			AddItemWithProbability(new HammerlordsShield(), 0.004);
			AddItemWithProbability(new HarmonyGauntlets(), 0.004);
			AddItemWithProbability(new HarmonysGuard(), 0.004);
			AddItemWithProbability(new HarvestersFootsteps(), 0.004);
			AddItemWithProbability(new HarvestersGrasp(), 0.004);
			AddItemWithProbability(new HarvestersGuard(), 0.004);
			AddItemWithProbability(new HarvestersHelm(), 0.004);
			AddItemWithProbability(new HarvestersStride(), 0.004);
			AddItemWithProbability(new HexweaversMysticalGloves(), 0.004);
			AddItemWithProbability(new HlaaluTradersCuffs(), 0.004);
			AddItemWithProbability(new HyruleKnightsShield(), 0.004);
			AddItemWithProbability(new ImmortalKingsIronCrown(), 0.004);
			AddItemWithProbability(new InfernoPlateChest(), 0.004);
			AddItemWithProbability(new InquisitorsGuard(), 0.004);
			AddItemWithProbability(new IstarisTouch(), 0.004);
			AddItemWithProbability(new JestersGleefulGloves(), 0.004);
			AddItemWithProbability(new JestersMerryCap(), 0.004);
			AddItemWithProbability(new JestersMischievousBuckler(), 0.004);
			AddItemWithProbability(new JestersPlayfulTunic(), 0.004);
			AddItemWithProbability(new JestersTricksterBoots(), 0.004);
			AddItemWithProbability(new KnightsAegis(), 0.004);
			AddItemWithProbability(new KnightsValorShield(), 0.004);
			AddItemWithProbability(new LeggingsOfTheRighteous(), 0.004);
			AddItemWithProbability(new LioneyesRemorse(), 0.004);
			AddItemWithProbability(new LionheartPlate(), 0.004);
			AddItemWithProbability(new LockesAdventurerLeather(), 0.004);
			AddItemWithProbability(new LocksleyLeatherChest(), 0.004);
			AddItemWithProbability(new LyricalGreaves(), 0.004);
			AddItemWithProbability(new LyricistsInsight(), 0.004);
			AddItemWithProbability(new MagitekInfusedPlate(), 0.004);
			AddItemWithProbability(new MakoResonance(), 0.004);
			AddItemWithProbability(new MaskedAvengersAgility(), 0.004);
			AddItemWithProbability(new MaskedAvengersDefense(), 0.004);
			AddItemWithProbability(new MaskedAvengersFocus(), 0.004);
			AddItemWithProbability(new MaskedAvengersPrecision(), 0.004);
			AddItemWithProbability(new MaskedAvengersVoice(), 0.004);
			AddItemWithProbability(new MelodicCirclet(), 0.004);
			AddItemWithProbability(new MerryMensStuddedGloves(), 0.004);
			AddItemWithProbability(new MeteorWard(), 0.004);
			AddItemWithProbability(new MinersHelmet(), 0.004);
			AddItemWithProbability(new MinstrelsMelody(), 0.004);
			AddItemWithProbability(new MisfortunesChains(), 0.004);
			AddItemWithProbability(new MondainsSkull(), 0.004);
			AddItemWithProbability(new MonksBattleWraps(), 0.004);
			AddItemWithProbability(new MonksSoulGloves(), 0.004);
			AddItemWithProbability(new MysticSeersPlate(), 0.004);
			AddItemWithProbability(new MysticsGuard(), 0.004);
			AddItemWithProbability(new NajsArcaneVestment(), 0.004);
			AddItemWithProbability(new NaturesEmbraceBelt(), 0.004);
			AddItemWithProbability(new NaturesEmbraceHelm(), 0.004);
			AddItemWithProbability(new NaturesGuardBoots(), 0.004);
			AddItemWithProbability(new NecklaceOfAromaticProtection(), 0.004);
			AddItemWithProbability(new NecromancersBoneGrips(), 0.004);
			AddItemWithProbability(new NecromancersDarkLeggings(), 0.004);
			AddItemWithProbability(new NecromancersHood(), 0.004);
			AddItemWithProbability(new NecromancersRobe(), 0.004);
			AddItemWithProbability(new NecromancersShadowBoots(), 0.004);
			AddItemWithProbability(new NightingaleVeil(), 0.004);
			AddItemWithProbability(new NinjaWrappings(), 0.004);
			AddItemWithProbability(new NottinghamStalkersLeggings(), 0.004);
			AddItemWithProbability(new OrkArdHat(), 0.004);
			AddItemWithProbability(new OutlawsForestBuckler(), 0.004);
			AddItemWithProbability(new PhilosophersGreaves(), 0.004);
			AddItemWithProbability(new PyrePlateHelm(), 0.004);
			AddItemWithProbability(new RadiantCrown(), 0.004);
			AddItemWithProbability(new RatsNest(), 0.004);
			AddItemWithProbability(new ReconnaissanceBoots(), 0.004);
			AddItemWithProbability(new RedoranDefendersGreaves(), 0.004);
			AddItemWithProbability(new RedstoneArtificersGloves(), 0.004);
			AddItemWithProbability(new RiotDefendersShield(), 0.004);
			AddItemWithProbability(new RoguesShadowBoots(), 0.004);
			AddItemWithProbability(new RoguesStealthShield(), 0.004);
			AddItemWithProbability(new RoyalCircletHelm(), 0.004);
			AddItemWithProbability(new SabatonsOfDawn(), 0.004);
			AddItemWithProbability(new SerenadesEmbrace(), 0.004);
			AddItemWithProbability(new SerpentScaleArmor(), 0.004);
			AddItemWithProbability(new SerpentsEmbrace(), 0.004);
			AddItemWithProbability(new ShadowGripGloves(), 0.004);
			AddItemWithProbability(new ShaftstopArmor(), 0.004);
			AddItemWithProbability(new ShaminosGreaves(), 0.004);
			AddItemWithProbability(new SherwoodArchersCap(), 0.004);
			AddItemWithProbability(new ShinobiHood(), 0.004);
			AddItemWithProbability(new ShurikenBracers(), 0.004);
			AddItemWithProbability(new SilentStepTabi(), 0.004);
			AddItemWithProbability(new SilksOfTheVictor(), 0.004);
			AddItemWithProbability(new SirensLament(), 0.004);
			AddItemWithProbability(new SirensResonance(), 0.004);
			AddItemWithProbability(new SkinOfTheVipermagi(), 0.004);
			AddItemWithProbability(new SlitheringSeal(), 0.004);
			AddItemWithProbability(new SolarisAegis(), 0.004);
			AddItemWithProbability(new SolarisLorica(), 0.004);
			AddItemWithProbability(new SOLDIERSMight(), 0.004);
			AddItemWithProbability(new SorrowsGrasp(), 0.004);
			AddItemWithProbability(new StealthOperatorsGear(), 0.004);
			AddItemWithProbability(new StormcrowsGaze(), 0.004);
			AddItemWithProbability(new StormforgedBoots(), 0.004);
			AddItemWithProbability(new StormforgedGauntlets(), 0.004);
			AddItemWithProbability(new StormforgedHelm(), 0.004);
			AddItemWithProbability(new StormforgedLeggings(), 0.004);
			AddItemWithProbability(new StormforgedPlateChest(), 0.004);
			AddItemWithProbability(new Stormshield(), 0.004);
			AddItemWithProbability(new StringOfEars(), 0.004);
			AddItemWithProbability(new SummonersEmbrace(), 0.004);
			AddItemWithProbability(new TabulaRasa(), 0.004);
			AddItemWithProbability(new TacticalVest(), 0.004);
			AddItemWithProbability(new TailorsTouch(), 0.004);
			AddItemWithProbability(new TalsRashasRelic(), 0.004);
			AddItemWithProbability(new TamersBindings(), 0.004);
			AddItemWithProbability(new TechPriestMantle(), 0.004);
			AddItemWithProbability(new TelvanniMagistersCap(), 0.004);
			AddItemWithProbability(new TerrasMysticRobe(), 0.004);
			AddItemWithProbability(new TheThinkingCap(), 0.004);
			AddItemWithProbability(new ThiefsNimbleCap(), 0.004);
			AddItemWithProbability(new ThievesGuildPants(), 0.004);
			AddItemWithProbability(new ThundergodsVigor(), 0.004);
			AddItemWithProbability(new TinkersTreads(), 0.004);
			AddItemWithProbability(new ToxinWard(), 0.004);
			AddItemWithProbability(new TunicOfTheWild(), 0.004);
			AddItemWithProbability(new TyraelsVigil(), 0.004);
			AddItemWithProbability(new ValkyriesWard(), 0.004);
			AddItemWithProbability(new VeilOfSteel(), 0.004);
			AddItemWithProbability(new Venomweave(), 0.004);
			AddItemWithProbability(new VialWarden(), 0.004);
			AddItemWithProbability(new VipersCoif(), 0.004);
			AddItemWithProbability(new VirtueGuard(), 0.004);
			AddItemWithProbability(new VortexMantle(), 0.004);
			AddItemWithProbability(new VyrsGraspingGauntlets(), 0.004);
			AddItemWithProbability(new WardensAegis(), 0.004);
			AddItemWithProbability(new WhispersHeartguard(), 0.004);
			AddItemWithProbability(new WhiteMagesDivineVestment(), 0.004);
			AddItemWithProbability(new WhiteRidersGuard(), 0.004);
			AddItemWithProbability(new WhiteSageCap(), 0.004);
			AddItemWithProbability(new WildwalkersGreaves(), 0.004);
			AddItemWithProbability(new WinddancerBoots(), 0.004);
			AddItemWithProbability(new WisdomsCirclet(), 0.004);
			AddItemWithProbability(new WisdomsEmbrace(), 0.004);
			AddItemWithProbability(new WitchesBindingGloves(), 0.004);
			AddItemWithProbability(new WitchesCursedRobe(), 0.004);
			AddItemWithProbability(new WitchesEnchantedHat(), 0.004);
			AddItemWithProbability(new WitchesEnchantedRobe(), 0.004);
			AddItemWithProbability(new WitchesHeartAmulet(), 0.004);
			AddItemWithProbability(new WitchesWhisperingBoots(), 0.004);
			AddItemWithProbability(new WitchfireShield(), 0.004);
			AddItemWithProbability(new WitchwoodGreaves(), 0.004);
			AddItemWithProbability(new WraithsBane(), 0.004);
			AddItemWithProbability(new WrestlersArmsOfPrecision(), 0.004);
			AddItemWithProbability(new WrestlersChestOfPower(), 0.004);
			AddItemWithProbability(new WrestlersGrippingGloves(), 0.004);
			AddItemWithProbability(new WrestlersHelmOfFocus(), 0.004);
			AddItemWithProbability(new WrestlersLeggingsOfBalance(), 0.004);
			AddItemWithProbability(new ZorasFins(), 0.004);
			AddItemWithProbability(new AdventurersBoots(), 0.004);
			AddItemWithProbability(new AerobicsInstructorsLegwarmers(), 0.004);
			AddItemWithProbability(new AmbassadorsCloak(), 0.004);
			AddItemWithProbability(new AnglersSeabreezeCloak(), 0.004);
			AddItemWithProbability(new ArchivistsShoes(), 0.004);
			AddItemWithProbability(new ArrowsmithsSturdyBoots(), 0.004);
			AddItemWithProbability(new ArtisansTimberShoes(), 0.004);
			AddItemWithProbability(new AssassinsBandana(), 0.004);
			AddItemWithProbability(new AssassinsMaskedCap(), 0.004);
			AddItemWithProbability(new BaggyHipHopPants(), 0.004);
			AddItemWithProbability(new BakersSoftShoes(), 0.004);
			AddItemWithProbability(new BalladeersMuffler(), 0.004);
			AddItemWithProbability(new BanditsHiddenCloak(), 0.004);
			AddItemWithProbability(new BardOfErinsMuffler(), 0.004);
			AddItemWithProbability(new BardsTunicOfStonehenge(), 0.004);
			AddItemWithProbability(new BaristasMuffler(), 0.004);
			AddItemWithProbability(new BeastmastersTanic(), 0.004);
			AddItemWithProbability(new BeastmastersTonic(), 0.004);
			AddItemWithProbability(new BeastmastersTunic(), 0.004);
			AddItemWithProbability(new BeastmiastersTunic(), 0.004);
			AddItemWithProbability(new BeatniksBeret(), 0.004);
			AddItemWithProbability(new BeggarsLuckyBandana(), 0.004);
			AddItemWithProbability(new BlacksmithsReinforcedGloves(), 0.004);
			AddItemWithProbability(new BobbySoxersShoes(), 0.004);
			AddItemWithProbability(new BohoChicSundress(), 0.004);
			AddItemWithProbability(new BootsOfTheDeepCaverns(), 0.004);
			AddItemWithProbability(new BowcraftersProtectiveCloak(), 0.004);
			AddItemWithProbability(new BowyersInsightfulBandana(), 0.004);
			AddItemWithProbability(new BreakdancersCap(), 0.004);
			AddItemWithProbability(new CarpentersStalwartTunic(), 0.004);
			AddItemWithProbability(new CartographersExploratoryTunic(), 0.004);
			AddItemWithProbability(new CartographersHat(), 0.004);
			AddItemWithProbability(new CeltidDruidsRobe(), 0.004);
			AddItemWithProbability(new ChampagneToastTunic(), 0.004);
			AddItemWithProbability(new ChefsGourmetApron(), 0.004);
			AddItemWithProbability(new ClericsSacredSash(), 0.004);
			AddItemWithProbability(new CourtesansGracefulKimono(), 0.004);
			AddItemWithProbability(new CourtisansRefinedGown(), 0.004);
			AddItemWithProbability(new CouturiersSundress(), 0.004);
			AddItemWithProbability(new CraftsmansProtectiveGloves(), 0.004);
			AddItemWithProbability(new CropTopMystic(), 0.004);
			AddItemWithProbability(new CuratorsKilt(), 0.004);
			AddItemWithProbability(new CyberpunkNinjaTabi(), 0.004);
			AddItemWithProbability(new DancersEnchantedSkirt(), 0.004);
			AddItemWithProbability(new DapperFedoraOfInsight(), 0.004);
			AddItemWithProbability(new DarkLordsRobe(), 0.004);
			AddItemWithProbability(new DataMagesDigitalCloak(), 0.004);
			AddItemWithProbability(new DeepSeaTunic(), 0.004);
			AddItemWithProbability(new DenimJacketOfReflection(), 0.004);
			AddItemWithProbability(new DiplomatsTunic(), 0.004);
			AddItemWithProbability(new DiscoDivaBoots(), 0.004);
			AddItemWithProbability(new ElementalistsProtectiveCloak(), 0.004);
			AddItemWithProbability(new ElvenSnowBoots(), 0.004);
			AddItemWithProbability(new EmoSceneHairpin(), 0.004);
			AddItemWithProbability(new ExplorersBoots(), 0.004);
			AddItemWithProbability(new FilmNoirDetectivesTrenchCoat(), 0.004);
			AddItemWithProbability(new FishermansSunHat(), 0.004);
			AddItemWithProbability(new FishermansVest(), 0.004);
			AddItemWithProbability(new FishmongersKilt(), 0.004);
			AddItemWithProbability(new FletchersPrecisionGloves(), 0.004);
			AddItemWithProbability(new FlowerChildSundress(), 0.004);
			AddItemWithProbability(new ForestersTunic(), 0.004);
			AddItemWithProbability(new ForgeMastersBoots(), 0.004);
			AddItemWithProbability(new GazeCapturingVeil(), 0.004);
			AddItemWithProbability(new GeishasGracefulKasa(), 0.004);
			AddItemWithProbability(new GhostlyShroud(), 0.004);
			AddItemWithProbability(new GlamRockersJacket(), 0.004);
			AddItemWithProbability(new GlovesOfStonemasonry(), 0.004);
			AddItemWithProbability(new GoGoBootsOfAgility(), 0.004);
			AddItemWithProbability(new GrapplersTunic(), 0.004);
			AddItemWithProbability(new GreenwichMagesRobe(), 0.004);
			AddItemWithProbability(new GroovyBellBottomPants(), 0.004);
			AddItemWithProbability(new GrungeBandana(), 0.004);
			AddItemWithProbability(new HackersVRGoggles(), 0.004);
			AddItemWithProbability(new HammerlordsCap(), 0.004);
			AddItemWithProbability(new HarmonistsSoftShoes(), 0.004);
			AddItemWithProbability(new HealersBlessedSandals(), 0.004);
			AddItemWithProbability(new HealersFurCape(), 0.004);
			AddItemWithProbability(new HelmetOfTheOreWhisperer(), 0.004);
			AddItemWithProbability(new HerbalistsProtectiveHat(), 0.004);
			AddItemWithProbability(new HerdersMuffler(), 0.004);
			AddItemWithProbability(new HippiePeaceBandana(), 0.004);
			AddItemWithProbability(new HippiesPeacefulSandals(), 0.004);
			AddItemWithProbability(new IntriguersFeatheredHat(), 0.004);
			AddItemWithProbability(new JazzMusiciansMuffler(), 0.004);
			AddItemWithProbability(new KnightsHelmOfTheRoundTable(), 0.004);
			AddItemWithProbability(new LeprechaunsLuckyHat(), 0.004);
			AddItemWithProbability(new LorekeepersSash(), 0.004);
			AddItemWithProbability(new LuchadorsMask(), 0.004);
			AddItemWithProbability(new MapmakersInsightfulMuffler(), 0.004);
			AddItemWithProbability(new MarinersLuckyBoots(), 0.004);
			AddItemWithProbability(new MelodiousMuffler(), 0.004);
			AddItemWithProbability(new MendersDivineRobe(), 0.004);
			AddItemWithProbability(new MidnightRevelersBoots(), 0.004);
			AddItemWithProbability(new MinersSturdyBoots(), 0.004);
			AddItemWithProbability(new MinstrelsTunedTunic(), 0.004);
			AddItemWithProbability(new MistletoeMuffler(), 0.004);
			AddItemWithProbability(new ModStyleTunic(), 0.004);
			AddItemWithProbability(new MoltenCloak(), 0.004);
			AddItemWithProbability(new MonksMeditativeRobe(), 0.004);
			AddItemWithProbability(new MummysWrappings(), 0.004);
			AddItemWithProbability(new MysticsFeatheredHat(), 0.004);
			AddItemWithProbability(new NaturalistsCloak(), 0.004);
			AddItemWithProbability(new NaturesMuffler(), 0.004);
			AddItemWithProbability(new NavigatorsProtectiveCap(), 0.004);
			AddItemWithProbability(new NecromancersCape(), 0.004);
			AddItemWithProbability(new NeonStreetSash(), 0.004);
			AddItemWithProbability(new NewWaveNeonShades(), 0.004);
			AddItemWithProbability(new NinjasKasa(), 0.004);
			AddItemWithProbability(new NinjasStealthyTabi(), 0.004);
			AddItemWithProbability(new OreSeekersBandana(), 0.004);
			AddItemWithProbability(new PickpocketsNimbleGloves(), 0.004);
			AddItemWithProbability(new PickpocketsSleekTunic(), 0.004);
			AddItemWithProbability(new PinUpHalterDress(), 0.004);
			AddItemWithProbability(new PlatformSneakers(), 0.004);
			AddItemWithProbability(new PoodleSkirtOfCharm(), 0.004);
			AddItemWithProbability(new PopStarsFingerlessGloves(), 0.004);
			AddItemWithProbability(new PopStarsGlitteringCap(), 0.004);
			AddItemWithProbability(new PopStarsSparklingBandana(), 0.004);
			AddItemWithProbability(new PreserversCap(), 0.004);
			AddItemWithProbability(new PsychedelicTieDyeShirt(), 0.004);
			AddItemWithProbability(new PsychedelicWizardsHat(), 0.004);
			AddItemWithProbability(new PumpkinKingsCrown(), 0.004);
			AddItemWithProbability(new QuivermastersTunic(), 0.004);
			AddItemWithProbability(new RangersCap(), 0.004);
			AddItemWithProbability(new RangersHat(), 0.004);
			AddItemWithProbability(new RangersHatNightSight(), 0.004);
			AddItemWithProbability(new ReindeerFurCap(), 0.004);
			AddItemWithProbability(new ResolutionKeepersSash(), 0.004);
			AddItemWithProbability(new RingmastersSandals(), 0.004);
			AddItemWithProbability(new RockabillyRebelJacket(), 0.004);
			AddItemWithProbability(new RoguesDeceptiveMask(), 0.004);
			AddItemWithProbability(new RoguesShadowCloak(), 0.004);
			AddItemWithProbability(new RoyalGuardsBoots(), 0.004);
			AddItemWithProbability(new SamuraisHonorableTunic(), 0.004);
			AddItemWithProbability(new SantasEnchantedRobe(), 0.004);
			AddItemWithProbability(new SawyersMightyApron(), 0.004);
			AddItemWithProbability(new ScholarsRobe(), 0.004);
			AddItemWithProbability(new ScoutsWideBrimHat(), 0.004);
			AddItemWithProbability(new ScribersRobe(), 0.004);
			AddItemWithProbability(new ScribesEnlightenedSandals(), 0.004);
			AddItemWithProbability(new ScriptoriumMastersRobe(), 0.004);
			AddItemWithProbability(new SeductressSilkenShoes(), 0.004);
			AddItemWithProbability(new SeersMysticSash(), 0.004);
			AddItemWithProbability(new ShadowWalkersTabi(), 0.004);
			AddItemWithProbability(new ShanachiesStorytellingShoes(), 0.004);
			AddItemWithProbability(new ShepherdsKilt(), 0.004);
			AddItemWithProbability(new SherlocksSleuthingCap(), 0.004);
			AddItemWithProbability(new ShogunsAuthoritativeSurcoat(), 0.004);
			AddItemWithProbability(new SilentNightCloak(), 0.004);
			AddItemWithProbability(new SkatersBaggyPants(), 0.004);
			AddItemWithProbability(new SmithsProtectiveTunic(), 0.004);
			AddItemWithProbability(new SneaksSilentShoes(), 0.004);
			AddItemWithProbability(new SnoopersSoftGloves(), 0.004);
			AddItemWithProbability(new SommelierBodySash(), 0.004);
			AddItemWithProbability(new SorceressMidnightRobe(), 0.004);
			AddItemWithProbability(new SpellweaversEnchantedShoes(), 0.004);
			AddItemWithProbability(new StarletsFancyDress(), 0.004);
			AddItemWithProbability(new StarlightWizardsHat(), 0.004);
			AddItemWithProbability(new StarlightWozardsHat(), 0.004);
			AddItemWithProbability(new StreetArtistsBaggyPants(), 0.004);
			AddItemWithProbability(new StreetPerformersCap(), 0.004);
			AddItemWithProbability(new SubmissionsArtistsMuffler(), 0.004);
			AddItemWithProbability(new SurgeonsInsightfulMask(), 0.004);
			AddItemWithProbability(new SwingsDancersShoes(), 0.004);
			AddItemWithProbability(new TailorsFancyApron(), 0.004);
			AddItemWithProbability(new TamersKilt(), 0.004);
			AddItemWithProbability(new TamersMuffler(), 0.004);
			AddItemWithProbability(new TechGurusGlasses(), 0.004);
			AddItemWithProbability(new TechnomancersHoodie(), 0.004);
			AddItemWithProbability(new ThiefsShadowTunic(), 0.004);
			AddItemWithProbability(new ThiefsSilentShoes(), 0.004);
			AddItemWithProbability(new TidecallersSandals(), 0.004);
			AddItemWithProbability(new TruckersIconicCap(), 0.004);
			AddItemWithProbability(new UrbanitesSneakers(), 0.004);
			AddItemWithProbability(new VampiresMidnightCloak(), 0.004);
			AddItemWithProbability(new VestOfTheVeinSeeker(), 0.004);
			AddItemWithProbability(new WarHeronsCap(), 0.004);
			AddItemWithProbability(new WarriorOfUlstersTunic(), 0.004);
			AddItemWithProbability(new WarriorsBelt(), 0.004);
			AddItemWithProbability(new WhisperersBoots(), 0.004);
			AddItemWithProbability(new WhisperersSandals(), 0.004);
			AddItemWithProbability(new WhisperingSandals(), 0.004);
			AddItemWithProbability(new WhisperingSondals(), 0.004);
			AddItemWithProbability(new WhisperingWindSash(), 0.004);
			AddItemWithProbability(new WitchesBewitchingRobe(), 0.004);
			AddItemWithProbability(new WitchesBrewedHat(), 0.004);
			AddItemWithProbability(new WoodworkersInsightfulCap(), 0.004);
			AddItemWithProbability(new AegisShield(), 0.004);
			AddItemWithProbability(new AeonianBow(), 0.004);
			AddItemWithProbability(new AlamoDefendersAxe(), 0.004);
			AddItemWithProbability(new AlucardsBlade(), 0.004);
			AddItemWithProbability(new AnubisWarMace(), 0.004);
			AddItemWithProbability(new ApepsCoiledScimitar(), 0.004);
			AddItemWithProbability(new ApollosSong(), 0.004);
			AddItemWithProbability(new ArchersYewBow(), 0.004);
			AddItemWithProbability(new AssassinsKryss(), 0.004);
			AddItemWithProbability(new AtmaBlade(), 0.004);
			AddItemWithProbability(new AxeOfTheJuggernaut(), 0.004);
			AddItemWithProbability(new AxeOfTheRuneweaver(), 0.004);
			AddItemWithProbability(new BaneOfTheDead(), 0.004);
			AddItemWithProbability(new BanshoFanClub(), 0.004);
			AddItemWithProbability(new BarbarossaScimitar(), 0.004);
			AddItemWithProbability(new BardsBowOfDiscord(), 0.004);
			AddItemWithProbability(new BeowulfsWarAxe(), 0.004);
			AddItemWithProbability(new BismarckianWarAxe(), 0.004);
			AddItemWithProbability(new Blackrazor(), 0.004);
			AddItemWithProbability(new BlacksmithsWarHammer(), 0.004);
			AddItemWithProbability(new BlackSwordOfMondain(), 0.004);
			AddItemWithProbability(new BlackTailWhip(), 0.004);
			AddItemWithProbability(new BladeOfTheStars(), 0.004);
			AddItemWithProbability(new Bonehew(), 0.004);
			AddItemWithProbability(new BowiesLegacy(), 0.004);
			AddItemWithProbability(new BowOfAuriel(), 0.004);
			AddItemWithProbability(new BowOfIsrafil(), 0.004);
			AddItemWithProbability(new BowspritOfBluenose(), 0.004);
			AddItemWithProbability(new BulKathosTribalGuardian(), 0.004);
			AddItemWithProbability(new BusterSwordReplica(), 0.004);
			AddItemWithProbability(new ButchersCleaver(), 0.004);
			AddItemWithProbability(new CaduceusStaff(), 0.004);
			AddItemWithProbability(new CelestialLongbow(), 0.004);
			AddItemWithProbability(new CelestialScimitar(), 0.004);
			AddItemWithProbability(new CetrasStaff(), 0.004);
			AddItemWithProbability(new ChakramBlade(), 0.004);
			AddItemWithProbability(new CharlemagnesWarAxe(), 0.004);
			AddItemWithProbability(new CherubsBlade(), 0.004);
			AddItemWithProbability(new ChillrendLongsword(), 0.004);
			AddItemWithProbability(new ChuKoNu(), 0.004);
			AddItemWithProbability(new CrissaegrimEdge(), 0.004);
			AddItemWithProbability(new CthulhusGaze(), 0.004);
			AddItemWithProbability(new CursedArmorCleaver(), 0.004);
			AddItemWithProbability(new CustersLastStandBow(), 0.004);
			AddItemWithProbability(new DaggerOfShadows(), 0.004);
			AddItemWithProbability(new DavidsSling(), 0.004);
			AddItemWithProbability(new DawnbreakerMace(), 0.004);
			AddItemWithProbability(new DeadMansLegacy(), 0.004);
			AddItemWithProbability(new DestructoDiscDagger(), 0.004);
			AddItemWithProbability(new DianasMoonBow(), 0.004);
			AddItemWithProbability(new DoomfletchsPrism(), 0.004);
			AddItemWithProbability(new Doomsickle(), 0.004);
			AddItemWithProbability(new DragonClaw(), 0.004);
			AddItemWithProbability(new DragonsBreath(), 0.004);
			AddItemWithProbability(new DragonsBreathWarAxe(), 0.004);
			AddItemWithProbability(new DragonsScaleDagger(), 0.004);
			AddItemWithProbability(new DragonsWrath(), 0.004);
			AddItemWithProbability(new Dreamseeker(), 0.004);
			AddItemWithProbability(new EarthshakerMaul(), 0.004);
			AddItemWithProbability(new EbonyWarAxeOfVampires(), 0.004);
			AddItemWithProbability(new EldritchBowOfShadows(), 0.004);
			AddItemWithProbability(new EldritchWhisper(), 0.004);
			AddItemWithProbability(new ErdricksBlade(), 0.004);
			AddItemWithProbability(new Excalibur(), 0.004);
			AddItemWithProbability(new ExcaliburLongsword(), 0.004);
			AddItemWithProbability(new ExcalibursLegacy(), 0.004);
			AddItemWithProbability(new FangOfStorms(), 0.004);
			AddItemWithProbability(new FlamebaneWarAxe(), 0.004);
			AddItemWithProbability(new FrostfireCleaver(), 0.004);
			AddItemWithProbability(new FrostflameKatana(), 0.004);
			AddItemWithProbability(new FuHaosBattleAxe(), 0.004);
			AddItemWithProbability(new GenjiBow(), 0.004);
			AddItemWithProbability(new GeomancersStaff(), 0.004);
			AddItemWithProbability(new GhoulSlayersLongsword(), 0.004);
			AddItemWithProbability(new GlassSword(), 0.004);
			AddItemWithProbability(new GlassSwordOfValor(), 0.004);
			AddItemWithProbability(new GoldbrandScimitar(), 0.004);
			AddItemWithProbability(new GreenDragonCrescentBlade(), 0.004);
			AddItemWithProbability(new Grimmblade(), 0.004);
			AddItemWithProbability(new GrimReapersCleaver(), 0.004);
			AddItemWithProbability(new GriswoldsEdge(), 0.004);
			AddItemWithProbability(new GrognaksAxe(), 0.004);
			AddItemWithProbability(new GuardianOfTheFey(), 0.004);
			AddItemWithProbability(new GuillotineBladeDagger(), 0.004);
			AddItemWithProbability(new HalberdOfHonesty(), 0.004);
			AddItemWithProbability(new HanseaticCrossbow(), 0.004);
			AddItemWithProbability(new HarmonyBow(), 0.004);
			AddItemWithProbability(new HarpeBlade(), 0.004);
			AddItemWithProbability(new HeartbreakerSunder(), 0.004);
			AddItemWithProbability(new HelmOfDarkness(), 0.004);
			AddItemWithProbability(new IlluminaDagger(), 0.004);
			AddItemWithProbability(new InuitUluOfTheNorth(), 0.004);
			AddItemWithProbability(new JoansDivineLongsword(), 0.004);
			AddItemWithProbability(new JuggernautHammer(), 0.004);
			AddItemWithProbability(new KaomsCleaver(), 0.004);
			AddItemWithProbability(new KaomsMaul(), 0.004);
			AddItemWithProbability(new Keenstrike(), 0.004);
			AddItemWithProbability(new KhufusWarSpear(), 0.004);
			AddItemWithProbability(new KingsSwordOfHaste(), 0.004);
			AddItemWithProbability(new MaatsBalancedBow(), 0.004);
			AddItemWithProbability(new MablungsDefender(), 0.004);
			AddItemWithProbability(new MaceOfTheVoid(), 0.004);
			AddItemWithProbability(new MageMasher(), 0.004);
			AddItemWithProbability(new MageMusher(), 0.004);
			AddItemWithProbability(new MagesStaff(), 0.004);
			AddItemWithProbability(new MagicAxeOfGreatStrength(), 0.004);
			AddItemWithProbability(new MagusRod(), 0.004);
			AddItemWithProbability(new MakhairaOfAchilles(), 0.004);
			AddItemWithProbability(new ManajumasKnife(), 0.004);
			AddItemWithProbability(new MarssBattleAxeOfValor(), 0.004);
			AddItemWithProbability(new MasamuneBlade(), 0.004);
			AddItemWithProbability(new MasamuneKatana(), 0.004);
			AddItemWithProbability(new MasamunesEdge(), 0.004);
			AddItemWithProbability(new MasamunesGrace(), 0.004);
			AddItemWithProbability(new MaulOfSulayman(), 0.004);
			AddItemWithProbability(new MehrunesCleaver(), 0.004);
			AddItemWithProbability(new MortuarySword(), 0.004);
			AddItemWithProbability(new MosesStaff(), 0.004);
			AddItemWithProbability(new MuramasasBloodlust(), 0.004);
			AddItemWithProbability(new MusketeersRapier(), 0.004);
			AddItemWithProbability(new MysticBowOfLight(), 0.004);
			AddItemWithProbability(new MysticStaffOfElements(), 0.004);
			AddItemWithProbability(new NaginataOfTomoeGozen(), 0.004);
			AddItemWithProbability(new NebulaBow(), 0.004);
			AddItemWithProbability(new NecromancersDagger(), 0.004);
			AddItemWithProbability(new NeptunesTrident(), 0.004);
			AddItemWithProbability(new NormanConquerorsBow(), 0.004);
			AddItemWithProbability(new PaladinsChrysblade(), 0.004);
			AddItemWithProbability(new PlasmaInfusedWarHammer(), 0.004);
			AddItemWithProbability(new PlutosAbyssalMace(), 0.004);
			AddItemWithProbability(new PotaraEarringClub(), 0.004);
			AddItemWithProbability(new PowerPoleHalberd(), 0.004);
			AddItemWithProbability(new PowersBeacon(), 0.004);
			AddItemWithProbability(new ProhibitionClub(), 0.004);
			AddItemWithProbability(new QamarDagger(), 0.004);
			AddItemWithProbability(new QuasarAxe(), 0.004);
			AddItemWithProbability(new RainbowBlade(), 0.004);
			AddItemWithProbability(new RasSearingDagger(), 0.004);
			AddItemWithProbability(new ReflectionShield(), 0.004);
			AddItemWithProbability(new RevolutionarySabre(), 0.004);
			AddItemWithProbability(new RielsRebellionSabre(), 0.004);
			AddItemWithProbability(new RuneAss(), 0.004);
			AddItemWithProbability(new RuneAxe(), 0.004);
			AddItemWithProbability(new SaiyanTailWhip(), 0.004);
			AddItemWithProbability(new SamsonsJawbone(), 0.004);
			AddItemWithProbability(new SaxonSeax(), 0.004);
			AddItemWithProbability(new SearingTouch(), 0.004);
			AddItemWithProbability(new SerpentsFang(), 0.004);
			AddItemWithProbability(new SerpentsVenomDagger(), 0.004);
			AddItemWithProbability(new ShadowstrideBow(), 0.004);
			AddItemWithProbability(new ShavronnesRapier(), 0.004);
			AddItemWithProbability(new SkyPiercer(), 0.004);
			AddItemWithProbability(new SoulTaker(), 0.004);
			AddItemWithProbability(new StaffOfAeons(), 0.004);
			AddItemWithProbability(new StaffOfApocalypse(), 0.004);
			AddItemWithProbability(new StaffOfRainsWrath(), 0.004);
			AddItemWithProbability(new StaffOfTheElements(), 0.004);
			AddItemWithProbability(new StarfallDagger(), 0.004);
			AddItemWithProbability(new Sunblade(), 0.004);
			AddItemWithProbability(new SwordOfAlBattal(), 0.004);
			AddItemWithProbability(new SwordOfGideon(), 0.004);
			AddItemWithProbability(new TabulasDagger(), 0.004);
			AddItemWithProbability(new TantoOfThe47Ronin(), 0.004);
			AddItemWithProbability(new TempestHammer(), 0.004);
			AddItemWithProbability(new TeutonicWarMace(), 0.004);
			AddItemWithProbability(new TheFurnace(), 0.004);
			AddItemWithProbability(new TheOculus(), 0.004);
			AddItemWithProbability(new ThorsHammer(), 0.004);
			AddItemWithProbability(new Thunderfury(), 0.004);
			AddItemWithProbability(new Thunderstroke(), 0.004);
			AddItemWithProbability(new TitansFury(), 0.004);
			AddItemWithProbability(new TomahawkOfTecumseh(), 0.004);
			AddItemWithProbability(new TouchOfAnguish(), 0.004);
			AddItemWithProbability(new TriLithiumBlade(), 0.004);
			AddItemWithProbability(new TwoShotCrossbow(), 0.004);
			AddItemWithProbability(new UltimaGlaive(), 0.004);
			AddItemWithProbability(new UmbraWarAxe(), 0.004);
			AddItemWithProbability(new UndeadCrown(), 0.004);
			AddItemWithProbability(new ValiantThrower(), 0.004);
			AddItemWithProbability(new VampireKiller(), 0.004);
			AddItemWithProbability(new VATSEnhancedDagger(), 0.004);
			AddItemWithProbability(new VenomsSting(), 0.004);
			AddItemWithProbability(new VoidsEmbrace(), 0.004);
			AddItemWithProbability(new VolendrungWarHammer(), 0.004);
			AddItemWithProbability(new VolendrungWorHammer(), 0.004);
			AddItemWithProbability(new VoltaxicRiftLance(), 0.004);
			AddItemWithProbability(new VoyageursPaddle(), 0.004);
			AddItemWithProbability(new VulcansForgeHammer(), 0.004);
			AddItemWithProbability(new WabbajackClub(), 0.004);
			AddItemWithProbability(new WandOfWoh(), 0.004);
			AddItemWithProbability(new Whelm(), 0.004);
			AddItemWithProbability(new WhisperingWindWarMace(), 0.004);
			AddItemWithProbability(new WhisperwindBow(), 0.004);
			AddItemWithProbability(new WindDancersDagger(), 0.004);
			AddItemWithProbability(new WindripperBow(), 0.004);
			AddItemWithProbability(new Wizardspike(), 0.004);
			AddItemWithProbability(new WondershotCrossbow(), 0.004);
			AddItemWithProbability(new Xcalibur(), 0.004);
			AddItemWithProbability(new YumiOfEmpressJingu(), 0.004);
			AddItemWithProbability(new ZhugeFeathersFan(), 0.004);
			AddItemWithProbability(new Zulfiqar(), 0.004);


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
					note.TitleString = "Ancient Map";
					note.NoteString = "This map is tattered and old, showing the location of a long-forgotten treasure buried deep in the mountains.";
					break;
				case 1:
					note.TitleString = "Old Deed";
					note.NoteString = "An old and weathered deed granting ownership of a vast estate to a distant ancestor.";
					break;
				case 2:
					note.TitleString = "Royal Charter";
					note.NoteString = "A beautifully ornate charter bestowing noble titles and privileges upon the bearer by a long-dead king.";
					break;
				case 3:
					note.TitleString = "Pirate's Log";
					note.NoteString = "A weathered logbook detailing the adventures and hidden caches of a notorious pirate captain.";
					break;
				case 4:
					note.TitleString = "Merchant's Ledger";
					note.NoteString = "A detailed ledger showing years of profitable trades, along with some questionable dealings.";
					break;
				case 5:
					note.TitleString = "Scholar's Manuscript";
					note.NoteString = "An ancient manuscript filled with rare and possibly dangerous magical knowledge.";
					break;
				case 6:
					note.TitleString = "Magician's Grimoire";
					note.NoteString = "A grimoire containing powerful spells and incantations used by a legendary magician.";
					break;
				case 7:
					note.TitleString = "King's Proclamation";
					note.NoteString = "A royal proclamation announcing a significant historical event, signed by the king himself.";
					break;
				case 8:
					note.TitleString = "Noble's Will";
					note.NoteString = "The last will and testament of a noble, revealing the secret locations of hidden family wealth.";
					break;
				case 9:
					note.TitleString = "Secret Treaty";
					note.NoteString = "A secret treaty between two kingdoms, outlining a pact that could alter the balance of power.";
					break;
				case 10:
					note.TitleString = "Treaty of Compassion";
					note.NoteString = "An ancient accord signed by Lord British and the leaders of Skara Brae, affirming the principles of compassion and mutual aid.";
					break;
				case 11:
					note.TitleString = "Accords of the Codex";
					note.NoteString = "A detailed manuscript outlining the agreements made between the Britannian government and the Gargoyles regarding the sharing and protection of the Codex of Ultimate Wisdom.";
					break;
				case 12:
					note.TitleString = "Pact of the Fellowship";
					note.NoteString = "A secret document detailing the true intentions and alliances of the Fellowship, including their hidden agendas and connections.";
					break;
				case 13:
					note.TitleString = "Truce of Trinsic";
					note.NoteString = "A historic agreement between the city of Trinsic and the pirates of Buccaneer's Den, promising peace and cooperation in times of crisis.";
					break;
				case 14:
					note.TitleString = "Covenant of the Avatar";
					note.NoteString = "A revered scroll describing the vows taken by the Avatar to uphold the eight Virtues and defend Britannia from all evil.";
					break;
				case 15:
					note.TitleString = "Treaty of Moonglow";
					note.NoteString = "A treaty signed by the mages of Moonglow and the Britannian council, establishing the rights and responsibilities of magical practitioners in the realm.";
					break;
				case 16:
					note.TitleString = "Concord of Yew";
					note.NoteString = "A detailed agreement between the Druids of Yew and the crown of Britannia, concerning the protection of sacred groves and ancient knowledge.";
					break;
				case 17:
					note.TitleString = "Treaty of Jhelom";
					note.NoteString = "A military pact between the warriors of Jhelom and the Britannian army, promising mutual defense and shared training techniques.";
					break;
				case 18:
					note.TitleString = "Alliance of Minoc";
					note.NoteString = "A document highlighting the alliance between the artisans of Minoc and the Britannian economy, ensuring the supply of essential goods and services.";
					break;
				case 19:
					note.TitleString = "Charter of Vesper";
					note.NoteString = "An agreement granting the city of Vesper autonomy in exchange for economic contributions and loyalty to the Britannian crown.";
					break;
				case 20:
					note.TitleString = "Declaration of Cove";
					note.NoteString = "A declaration by the citizens of Cove, pledging their support to Lord British in the face of external threats.";
					break;
				case 21:
					note.TitleString = "Treaty of the Serpent Isle";
					note.NoteString = "A rare document outlining the terms of peace and cooperation between the settlers of Britannia and the native inhabitants of the Serpent Isle.";
					break;
				case 22:
					note.TitleString = "Scroll of the Ophidians";
					note.NoteString = "A sacred scroll detailing the peace agreement between the Britannians and the Ophidians, promoting understanding and cultural exchange.";
					break;
				case 23:
					note.TitleString = "Accords of the Abyss";
					note.NoteString = "An ancient and cryptic document detailing the agreement between Britannian adventurers and the inhabitants of the Stygian Abyss, ensuring safe passage and mutual respect.";
					break;
				case 24:
					note.TitleString = "Pledge of the Rangers";
					note.NoteString = "A solemn pledge made by the rangers of Britannia to protect the wilderness and support the crown in maintaining peace and order.";
					break;
				case 25:
					note.TitleString = "Treaty of Wind";
					note.NoteString = "A treaty ensuring the cooperation of the hidden city of Wind with the Britannian government, focusing on magical research and defense.";
					break;
				case 26:
					note.TitleString = "Treaty of the Underworld";
					note.NoteString = "A secretive and controversial document detailing an agreement between Britannia and the mysterious inhabitants of the Underworld, focusing on mutual non-aggression.";
					break;
				case 27:
					note.TitleString = "Compact of the Time Lord";
					note.NoteString = "A rare and mystical compact outlining the responsibilities and expectations of the Time Lord in relation to the protection of Britannia.";
					break;
				case 28:
					note.TitleString = "Edict of the Virtues";
					note.NoteString = "An edict issued by Lord British, reaffirming the importance of the eight Virtues in governing and daily life, signed by key leaders across Britannia.";
					break;
				case 29:
					note.TitleString = "Scroll of the Silver Serpent";
					note.NoteString = "A revered scroll detailing the agreement between the healers of Britannia and the Order of the Silver Serpent, focusing on medical aid and support.";
					break;
				case 30:
					note.TitleString = "Deed to Castle Britannia";
					note.NoteString = "A highly prized deed granting ownership of Castle Britannia, home to Lord British and the seat of power in the realm.";
					break;
				case 31:
					note.TitleString = "Serpent's Hold Charter";
					note.NoteString = "A charter documenting the founding of Serpent's Hold, a fortress dedicated to training the realm's finest warriors.";
					break;
				case 32:
					note.TitleString = "Deed to the Shrine of Compassion";
					note.NoteString = "An ancient deed detailing the ownership of the Shrine of Compassion, a sacred place in Britannia.";
					break;
				case 33:
					note.TitleString = "Moonglow Mage's License";
					note.NoteString = "A rare document granting the holder permission to practice magic within the city of Moonglow.";
					break;
				case 34:
					note.TitleString = "Order of the Silver Serpent";
					note.NoteString = "A certificate of membership in the Order of the Silver Serpent, an elite group of warriors and knights.";
					break;
				case 35:
					note.TitleString = "Deed to the Lycaeum";
					note.NoteString = "A deed granting control over the Lycaeum, Britannia's greatest center of learning and wisdom.";
					break;
				case 36:
					note.TitleString = "Yew Forest Covenant";
					note.NoteString = "An ancient covenant outlining the protection and preservation of the great Yew forest.";
					break;
				case 37:
					note.TitleString = "Trinsic Guard Commission";
					note.NoteString = "A formal commission appointing the bearer as an officer in the renowned Trinsic Guard.";
					break;
				case 38:
					note.TitleString = "Britain Bank Ledger";
					note.NoteString = "A ledger containing detailed records of transactions and accounts held at the Britain Bank.";
					break;
				case 39:
					note.TitleString = "Skara Brae Rebuilding Plan";
					note.NoteString = "Detailed architectural plans for the rebuilding of Skara Brae after its near destruction by the undead.";
					break;
				case 40:
					note.TitleString = "Magincia Trade Agreement";
					note.NoteString = "An important trade agreement between the merchants of Magincia and other major cities in Britannia.";
					break;
				case 41:
					note.TitleString = "Vesper Shipping Manifest";
					note.NoteString = "A shipping manifest detailing the goods transported by sea to and from the port city of Vesper.";
					break;
				case 42:
					note.TitleString = "Buccaneer's Den Pirate Code";
					note.NoteString = "A rare copy of the Buccaneer's Den Pirate Code, outlining the rules and regulations governing pirate conduct.";
					break;
				case 43:
					note.TitleString = "Nujel'm Diplomatic Letter";
					note.NoteString = "A diplomatic letter detailing the terms of a peace treaty between Nujel'm and the other Britannian cities.";
					break;
				case 44:
					note.TitleString = "Jhelom Warrior's Oath";
					note.NoteString = "An oath sworn by the warriors of Jhelom, pledging their loyalty and bravery in the defense of Britannia.";
					break;
				case 45:
					note.TitleString = "Deed to the Shrine of Honor";
					note.NoteString = "A revered deed granting stewardship over the Shrine of Honor, one of Britannia's eight sacred shrines.";
					break;
				case 46:
					note.TitleString = "Cove Land Grant";
					note.NoteString = "A land grant providing the holder with a significant parcel of land within the town of Cove.";
					break;
				case 47:
					note.TitleString = "Minoc Miner's Contract";
					note.NoteString = "A contract detailing the rights and responsibilities of miners working in the rich mountains surrounding Minoc.";
					break;
				case 48:
					note.TitleString = "Hythloth Exploration Log";
					note.NoteString = "A detailed logbook kept by an explorer who ventured into the depths of the dangerous Hythloth dungeon.";
					break;
				case 49:
					note.TitleString = "Deed to the Shrine of Valor";
					note.NoteString = "A respected deed giving control of the Shrine of Valor, a sacred place dedicated to the virtue of courage.";
					break;
				case 50:
					note.TitleString = "Paws Village Charter";
					note.NoteString = "A charter outlining the founding and governance of the humble village of Paws.";
					break;
				case 51:
					note.TitleString = "Britannian Royal Treasury Inventory";
					note.NoteString = "An inventory list detailing the contents of the Royal Treasury of Britannia.";
					break;
				case 52:
					note.TitleString = "Empath Abbey Records";
					note.NoteString = "Records from Empath Abbey detailing the history and important events of this spiritual center.";
					break;
				case 53:
					note.TitleString = "Shrine of Spirituality Guardian's Journal";
					note.NoteString = "The personal journal of the guardian of the Shrine of Spirituality, detailing daily life and significant events.";
					break;
				case 54:
					note.TitleString = "Deed to the Shrine of Humility";
					note.NoteString = "A deed bestowing responsibility for the Shrine of Humility, a place dedicated to the virtue of humility.";
					break;
				case 55:
					note.TitleString = "Haven Healer's Guide";
					note.NoteString = "A comprehensive guide to the healing practices and potions used by the healers of Haven.";
					break;
				case 56:
					note.TitleString = "Deed to the Shrine of Sacrifice";
					note.NoteString = "A deed granting care over the Shrine of Sacrifice, dedicated to the virtue of selflessness.";
					break;
				case 57:
					note.TitleString = "Delucia Settlement Plan";
					note.NoteString = "A detailed plan for the settlement and development of the frontier town of Delucia.";
					break;
				case 58:
					note.TitleString = "Papua Fishing Logs";
					note.NoteString = "Logs kept by the fishermen of Papua, detailing the types and quantities of fish caught in the surrounding waters.";
					break;
				case 59:
					note.TitleString = "Serpent Isle Navigation Charts";
					note.NoteString = "Charts used by sailors to navigate the treacherous waters around Serpent Isle.";
					break;
				case 60:
					note.TitleString = "Terfin Reconstruction Blueprint";
					note.NoteString = "Blueprints for the reconstruction of Terfin, home to the gargoyles of Britannia.";
					break;
				case 61:
					note.TitleString = "Deed to the Shrine of Justice";
					note.NoteString = "A deed conferring guardianship of the Shrine of Justice, dedicated to the virtue of fairness.";
					break;
				case 62:
					note.TitleString = "Britannian Mining Consortium Agreement";
					note.NoteString = "An agreement between various mining guilds and the Crown to ensure fair practices and distribution of resources.";
					break;
				case 63:
					note.TitleString = "Magincia Agricultural Report";
					note.NoteString = "A report on the agricultural production and prosperity of the island city of Magincia.";
					break;
				case 64:
					note.TitleString = "Yew Council Meeting Minutes";
					note.NoteString = "Minutes from a meeting of the Yew Council, discussing important matters affecting the city and its surroundings.";
					break;
				case 65:
					note.TitleString = "Candle of Love Ritual Scroll";
					note.NoteString = "A scroll detailing the ritual used to reignite the Candle of Love, a powerful artifact in Britannian lore.";
					break;
				case 66:
					note.TitleString = "Proclamation of Unity";
					note.NoteString = "By order of Lord British, all towns and villages shall unite under a single banner to ensure the safety and prosperity of all citizens.";
					break;
				case 67:
					note.TitleString = "Proclamation of Trade";
					note.NoteString = "Let it be known by decree of Lord British, a new era of trade shall commence, opening our borders to friendly nations and fostering economic growth.";
					break;
				case 68:
					note.TitleString = "Proclamation of Justice";
					note.NoteString = "Under the guidance of Lord British, a new court system shall be established to uphold justice and fairness for all inhabitants of the realm.";
					break;
				case 69:
					note.TitleString = "Proclamation of Peace";
					note.NoteString = "In the name of Lord British, a ceasefire is declared, and negotiations for lasting peace with neighboring realms shall begin immediately.";
					break;
				case 70:
					note.TitleString = "Proclamation of Exploration";
					note.NoteString = "By the command of Lord British, brave adventurers are called upon to explore uncharted territories and report back their findings for the glory of the kingdom.";
					break;
				case 71:
					note.TitleString = "Proclamation of Celebration";
					note.NoteString = "Hear ye, hear ye! Lord British declares a week of festivities and celebrations in honor of the kingdom's rich heritage and bright future.";
					break;
				case 72:
					note.TitleString = "Proclamation of Protection";
					note.NoteString = "By decree of Lord British, new defenses shall be constructed along our borders to protect the realm from external threats and ensure the safety of our people.";
					break;
				case 73:
					note.TitleString = "Proclamation of Knowledge";
					note.NoteString = "Lord British proclaims the establishment of a grand library, where scholars from all lands are invited to share their knowledge and advance our understanding of the world.";
					break;
				case 74:
					note.TitleString = "Proclamation of Health";
					note.NoteString = "Let it be known that Lord British has ordered the creation of new hospitals and medical facilities to improve the health and well-being of all citizens.";
					break;
				case 75:
					note.TitleString = "Proclamation of Honor";
					note.NoteString = "In recognition of their bravery and service, Lord British bestows honors and titles upon those who have demonstrated exceptional valor in the defense of the realm.";
					break;
				case 76:
					note.TitleString = "Proclamation of Agriculture";
					note.NoteString = "By the will of Lord British, new farming techniques and resources shall be distributed to ensure a bountiful harvest and the sustenance of our people.";
					break;
				case 77:
					note.TitleString = "Proclamation of Magic";
					note.NoteString = "Lord British announces the formation of a council of mages to regulate the use of magic, ensuring it is used for the benefit of all and not for harm.";
					break;
				case 78:
					note.TitleString = "Proclamation of Arts";
					note.NoteString = "To enrich our culture, Lord British declares the establishment of an academy of the arts, encouraging the creation and appreciation of music, painting, and sculpture.";
					break;
				case 79:
					note.TitleString = "Proclamation of Roads";
					note.NoteString = "Under the guidance of Lord British, new roads and bridges shall be constructed to connect our cities and facilitate travel and commerce.";
					break;
				case 80:
					note.TitleString = "Proclamation of Apprenticeship";
					note.NoteString = "Lord British decrees that a system of apprenticeships shall be established, allowing young citizens to learn trades and skills from masters in various crafts.";
					break;
				case 81:
					note.TitleString = "Proclamation of Law";
					note.NoteString = "By the authority of Lord British, a comprehensive code of laws shall be written and enforced to maintain order and protect the rights of all citizens.";
					break;
				case 82:
					note.TitleString = "Proclamation of Festivals";
					note.NoteString = "Let it be known that Lord British has declared an annual series of festivals to celebrate the changing seasons and bring joy to the people of the realm.";
					break;
				case 83:
					note.TitleString = "Proclamation of Trade Routes";
					note.NoteString = "Lord British orders the establishment of secure trade routes with neighboring kingdoms to promote commerce and mutual prosperity.";
					break;
				case 84:
					note.TitleString = "Proclamation of Education";
					note.NoteString = "By the decree of Lord British, schools and universities shall be founded throughout the land to provide education and enlightenment to all.";
					break;
				case 85:
					note.TitleString = "Proclamation of Innovation";
					note.NoteString = "Lord British encourages the pursuit of new inventions and technologies, offering rewards to those who contribute to the advancement of our society.";
					break;
				case 86:
					note.TitleString = "Proclamation of Sanctuary";
					note.NoteString = "In the name of compassion, Lord British declares certain areas as sanctuaries where the wounded and weary may find refuge and aid.";
					break;
				case 87:
					note.TitleString = "Proclamation of Conservation";
					note.NoteString = "Lord British decrees that efforts shall be made to preserve our natural resources and protect the environment for future generations.";
					break;
				case 88:
					note.TitleString = "Proclamation of Naval Expansion";
					note.NoteString = "By order of Lord British, the kingdom's navy shall be expanded to protect our maritime interests and explore the vast oceans.";
					break;
				case 89:
					note.TitleString = "Proclamation of Heritage";
					note.NoteString = "Lord British proclaims the creation of a museum to preserve and display the artifacts and history of our great kingdom.";
					break;
				case 90:
					note.TitleString = "Proclamation of Loyalty";
					note.NoteString = "A decree from Lord British calls for all citizens to pledge their loyalty to the crown, ensuring unity and strength within the realm.";
					break;
				case 91:
					note.TitleString = "Proclamation of Tolerance";
					note.NoteString = "Let it be known that Lord British promotes tolerance and understanding among all races and cultures within the kingdom, fostering peace and cooperation.";
					break;
				case 92:
					note.TitleString = "Proclamation of Harvest";
					note.NoteString = "By the will of Lord British, a great harvest festival shall be held to give thanks for the bounty of the land and the hard work of our farmers.";
					break;
				case 93:
					note.TitleString = "Proclamation of Valor";
					note.NoteString = "In recognition of their heroism, Lord British awards medals of valor to those who have shown extraordinary courage in the face of danger.";
					break;
				case 94:
					note.TitleString = "Proclamation of Diplomacy";
					note.NoteString = "Lord British announces a new era of diplomacy, inviting ambassadors from foreign lands to discuss treaties and alliances.";
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

        public GearLootbox(Serial serial) : base(serial)
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
