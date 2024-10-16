// Treasure Chest Pack - Version 0.99I
// By Nerun

using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using System;

namespace Server.Items
{
    public class CustomLoot
    {
        public static Item[] ItemList = new Item[]
        {
            new Apple(),
            new Apple(),
            new Apple(),
            new Apple(),
            new RandomMagicWeapon(), // Adjust chance as needed
            new RandomMagicArmor(),
            new RandomMagicClothing(),
            new RandomMagicClothing(),
            new RandomMagicClothing(),
            new RandomMagicClothing(),
            new RandomMagicClothing(),
            new RandomMagicClothing(),
            new RandomMagicJewelry(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomMagicWeapon(),
            new RandomSkillJewelryA(),
            new RandomSkillJewelryAA(),
            new RandomSkillJewelryAB(),
            new RandomSkillJewelryAC(),
            new RandomSkillJewelryAD(),
            new RandomSkillJewelryAE(),
            new RandomSkillJewelryAF(),
            new RandomSkillJewelryAG(),
            new RandomSkillJewelryAH(),
            new RandomSkillJewelryAI(),
            new RandomSkillJewelryAJ(),
            new RandomSkillJewelryAK(),
            new RandomSkillJewelryAL(),
            new RandomSkillJewelryAM(),
            new RandomSkillJewelryAN(),
            new RandomSkillJewelryAO(),
            new RandomSkillJewelryAP(),
            new RandomSkillJewelryB(),
            new RandomSkillJewelryC(),
            new RandomSkillJewelryD(),
            new RandomSkillJewelryE(),
            new RandomSkillJewelryF(),
            new RandomSkillJewelryG(),
            new RandomSkillJewelryH(),
            new RandomSkillJewelryI(),
            new RandomSkillJewelryJ(),
            new RandomSkillJewelryK(),
            new RandomSkillJewelryL(),
            new RandomSkillJewelryM(),
            new RandomSkillJewelryN(),
            new RandomSkillJewelryO(),
            new RandomSkillJewelryP(),
            new RandomSkillJewelryQ(),
            new RandomSkillJewelryR(),
            new RandomSkillJewelryS(),
            new RandomSkillJewelryT(),
            new RandomSkillJewelryU(),
            new RandomSkillJewelryV(),
            new RandomSkillJewelryW(),
            new RandomMagicJewelry(),
            new RandomSkillJewelryY(),
            new RandomSkillJewelryZ(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicJewelry(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new RandomMagicArmor(),
            new AlchemyAugmentCrystal(),
            new AnatomyAugmentCrystal(),
            new AnimalLoreAugmentCrystal(),
            new AnimalTamingAugmentCrystal(),
            new ArcheryAugmentCrystal(),
            new ArmsLoreAugmentCrystal(),
            new ArmSlotChangeDeed(),
            new BagOfBombs(),
            new BagOfHealth(),
            new BagOfJuice(),
            new BanishingOrb(),
            new BanishingRod(),
            new BeggingAugmentCrystal(),
            new BeltSlotChangeDeed(),
            new BlacksmithyAugmentCrystal(),
            new BloodSword(),
            new BootsOfCommand(),
            new BraceletSlotChangeDeed(),
            new BushidoAugmentCrystal(),
            new CampingAugmentCrystal(),
            new CapacityIncreaseDeed(),
            new CarpentryAugmentCrystal(),
            new CartographyAugmentCrystal(),
            new ChestSlotChangeDeed(),
            new ChivalryAugmentCrystal(),
            new ColdHitAreaCrystal(),
            new ColdResistAugmentCrystal(),
            new CookingAugmentCrystal(),
            new CurseAugmentCrystal(),
            new DetectingHiddenAugmentCrystal(),
            new DiscordanceAugmentCrystal(),
            new DispelAugmentCrystal(),
            new EarringSlotChangeDeed(),
            new EnergyHitAreaCrystal(),
            new EnergyResistAugmentCrystal(),
            new FatigueAugmentCrystal(),
            new FencingAugmentCrystal(),
            new FireballAugmentCrystal(),
            new FireHitAreaCrystal(),
            new FireResistAugmentCrystal(),
            new FishingAugmentCrystal(),
            new FletchingAugmentCrystal(),
            new FocusAugmentCrystal(),
            new FootwearSlotChangeDeed(),
            new ForensicEvaluationAugmentCrystal(),
            new GlovesOfCommand(),
            new HarmAugmentCrystal(),
            new HeadSlotChangeDeed(),
            new HealingAugmentCrystal(),
            new HerdingAugmentCrystal(),
            new HidingAugmentCrystal(),
            new ImbuingAugmentCrystal(),
            new InscriptionAugmentCrystal(),
            new ItemIdentificationAugmentCrystal(),
            new JesterHatOfCommand(),
            new LegsSlotChangeDeed(),
            new LifeLeechAugmentCrystal(),
            new LightningAugmentCrystal(),
            new LockpickingAugmentCrystal(),
            new LowerAttackAugmentCrystal(),
            new LuckAugmentCrystal(),
            new LumberjackingAugmentCrystal(),
            new MaceFightingAugmentCrystal(),
            new MageryAugmentCrystal(),
            new ManaDrainAugmentCrystal(),
            new ManaLeechAugmentCrystal(),
            new MaxxiaScroll(),
            new MeditationAugmentCrystal(),
            new MiningAugmentCrystal(),
            new MirrorOfKalandra(),
            new MusicianshipAugmentCrystal(),
            new NeckSlotChangeDeed(),
            new NecromancyAugmentCrystal(),
            new NinjitsuAugmentCrystal(),
            new OneHandedTransformDeed(),
            new ParryingAugmentCrystal(),
            new PeacemakingAugmentCrystal(),
            new PhysicalHitAreaCrystal(),
            new PhysicalResistAugmentCrystal(),
            new PlateLeggingsOfCommand(),
            new PoisonHitAreaCrystal(),
            new PoisoningAugmentCrystal(),
            new PoisonResistAugmentCrystal(),
            new ProvocationAugmentCrystal(),
            new RemoveTrapAugmentCrystal(),
            new ResistingSpellsAugmentCrystal(),
            new RingSlotChangeDeed(),
            new RodOfOrcControl(),
            new ShirtSlotChangeDeed(),
            new SnoopingAugmentCrystal(),
            new SpellweavingAugmentCrystal(),
            new SpiritSpeakAugmentCrystal(),
            new StaminaLeechAugmentCrystal(),
            new StealingAugmentCrystal(),
            new StealthAugmentCrystal(),
            new SwingSpeedAugmentCrystal(),
            new SwordsmanshipAugmentCrystal(),
            new TacticsAugmentCrystal(),
            new TailoringAugmentCrystal(),
            new TalismanSlotChangeDeed(),
            new TasteIDAugmentCrystal(),
            new ThrowingAugmentCrystal(),
            new TinkeringAugmentCrystal(),
            new TrackingAugmentCrystal(),
            new VeterinaryAugmentCrystal(),
            new WeaponSpeedAugmentCrystal(),
            new WrestlingAugmentCrystal(),
            new PetSlotDeed(),
            new PetBondDeed(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new StatCapOrb(),
            new SkillOrb(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new MaxxiaScroll(),
            new AbysmalHorrorSummoningMateria(),
            new AcidElementalSummoningMateria(),
            new AgapiteElementalSummoningMateria(),
            new AirElementalSummoningMateria(),
            new AlligatorSummoningMateria(),
            new AncientLichSummoningMateria(),
            new AncientWyrmSummoningMateria(),
            new AntLionSummoningMateria(),
            new ArcaneDaemonSummoningMateria(),
            new ArcticOgreLordSummoningMateria(),
            new AxeBreathMateria(),
            new AxeCircleMateria(),
            new AxeLineMateria(),
            new BakeKitsuneSummoningMateria(),
            new BalronSummoningMateria(),
            new BarracoonSummoningMateria(),
            new BeeBreathMateria(),
            new BeeCircleMateria(),
            new BeeLineMateria(),
            new BeetleSummoningMateria(),
            new BlackBearSummoningMateria(),
            new BlackDragoonPirateMateria(),
            new BlackSolenInfiltratorQueenSummoningMateria(),
            new BlackSolenInfiltratorWarriorMateria(),
            new BlackSolenQueenSummoningMateria(),
            new BlackSolenWarriorSummoningMateria(),
            new BlackSolenWorkerSummoningMateria(),
            new BladesBreathMateria(),
            new BladesCircleMateria(),
            new BladesLineMateria(),
            new BloodElementalSummoningGem(),
            new BloodSwarmGem(),
            new BoarSummoningMateria(),
            new BogleSummoningMateria(),
            new BoglingSummoningMateria(),
            new BogThingSummoningMateria(),
            new BoneDemonSummoningMateria(),
            new BoneKnightSummoningMateria(),
            new BoneMagiSummoningMateria(),
            new BoulderBreathMateria(),
            new BoulderCircleMateria(),
            new BoulderLineMateria(),
            new BrigandSummoningMateria(),
            new BronzeElementalSummoningMateria(),
            new BrownBearSummoningMateria(),
            new BullFrogSummoningMateria(),
            new BullSummoningMateria(),
            new CatSummoningMateria(),
            new CentaurSummoningMateria(),
            new ChaosDaemonSummoningMateria(),
            new ChaosDragoonEliteSummoningMateria(),
            new ChaosDragoonSummoningMateria(),
            new ChickenSummoningMateria(),
            new CopperElementalSummoningMateria(),
            new CorpserSummoningMateria(),
            new CorrosiveSlimeSummoningMateria(),
            new CorruptedSoulMateria(),
            new CougarSummoningMateria(),
            new CowSummoningMateria(),
            new CraneSummoningMateria(),
            new CrankBreathMateria(),
            new CrankCircleMateria(),
            new CrankLineMateria(),
            new CrimsonDragonSummoningMateria(),
            new CrystalElementalSummoningMateria(),
            new CurtainBreathMateria(),
            new CurtainCircleMateria(),
            new CurtainLineMateria(),
            new CuSidheSummoningMateria(),
            new CyclopsSummoningMateria(),
            new DaemonSummoningMateria(),
            new DarkWispSummoningMateria(),
            new DarkWolfSummoningMateria(),
            new DeathWatchBeetleSummoningMateria(),
            new DeepSeaSerpentSummoningMateria(),
            new DeerBreathMateria(),
            new DeerCircleMateria(),
            new DeerLineMateria(),
            new DemonKnightSummoningMateria(),
            new DesertOstardSummoningMateria(),
            new DevourerSummoningMateria(),
            new DireWolfSummoningMateria(),
            new DogSummoningMateria(),
            new DolphinSummoningMateria(),
            new DopplegangerSummoningMateria(),
            new DragonSummoningMateria(),
            new DrakeSummoningMateria(),
            new DreadSpiderSummoningMateria(),
            new DullCopperElementalSummoningMateria(),
            new DVortexBreathMateria(),
            new DVortexCircleMateria(),
            new DVortexLineMateria(),
            new EagleSummoningMateria(),
            new EarthElementalSummoningMateria(),
            new EfreetSummoningMateria(),
            new ElderGazerSummoningMateria(),
            new EliteNinjaSummoningMateria(),
            new EttinSummoningMateria(),
            new EvilHealerSummoningMateria(),
            new EvilMageSummoningMateria(),
            new ExecutionerMateria(),
            new ExodusMinionSummoningMateria(),
            new ExodusOverseerSummoningMateria(),
            new FanDancerSummoningMateria(),
            new FeralTreefellowSummoningMateria(),
            new FetidEssenceMateria(),
            new FireBeetleSummoningMateria(),
            new FireElementalSummoningMateria(),
            new FireGargoyleSummoningMateria(),
            new FireSteedSummoningMateria(),
            new FlaskBreathMateria(),
            new FlaskCircleMateria(),
            new FlaskLineMateria(),
            new FleshGolemSummoningMateria(),
            new FleshRendererSummoningMateria(),
            new ForestOstardSummoningMateria(),
            new FrenziedOstardSummoningMateria(),
            new FrostOozeSummoningMateria(),
            new FrostSpiderSummoningMateria(),
            new FrostTrollSummoningMateria(),
            new FTreeCircleMateria(),
            new FTreeLineMateria(),
            new GamanSummoningMateria(),
            new GargoyleSummoningMateria(),
            new GasBreathMateria(),
            new GasCircleMateria(),
            new GasLineMateria(),
            new GateBreathMateria(),
            new GateCircleMateria(),
            new GateLineMateria(),
            new GazerSummoningMateria(),
            new GhoulSummoningMateria(),
            new GiantBlackWidowSummoningMateria(),
            new GiantRatSummoningMateria(),
            new GiantSerpentSummoningMateria(),
            new GiantSpiderSummoningMateria(),
            new GiantToadSummoningMateria(),
            new GibberlingSummoningMateria(),
            new GlowBreathMateria(),
            new GlowCircleMateria(),
            new GlowLineMateria(),
            new GoatSummoningMateria(),
            new GoldenElementalSummoningMateria(),
            new GolemSummoningMateria(),
            new GoreFiendSummoningMateria(),
            new GorillaSummoningMateria(),
            new GreaterDragonSummoningMateria(),
            new GreaterMongbatSummoningMateria(),
            new GreatHartSummoningMateria(),
            new GreyWolfSummoningMateria(),
            new GrizzlyBearSummoningMateria(),
            new GuillotineBreathMateria(),
            new GuillotineCircleMateria(),
            new GuillotineLineMateria(),
            new HarpySummoningMateria(),
            new HeadBreathMateria(),
            new HeadCircleMateria(),
            new HeadlessOneSummoningMateria(),
            new HeadLineMateria(),
            new HealerMateria(),
            new HeartBreathMateria(),
            new HeartCircleMateria(),
            new HeartLineMateria(),
            new HellCatSummoningMateria(),
            new HellHoundSummoningMateria(),
            new HellSteedSummoningMateria(),
            new HindSummoningMateria(),
            new HiryuSummoningMateria(),
            new HorseSummoningMateria(),
            new IceElementalSummoningMateria(),
            new IceFiendSummoningMateria(),
            new IceSerpentSummoningMateria(),
            new IceSnakeSummoningMateria(),
            new ImpSummoningMateria(),
            new JackRabbitSummoningMateria(),
            new KazeKemonoSummoningMateria(),
            new KirinSummoningMateria(),
            new LavaLizardSummoningMateria(),
            new LavaSerpentSummoningMateria(),
            new LavaSnakeSummoningMateria(),
            new LesserHiryuSummoningMateria(),
            new LichLordSummoningMateria(),
            new LichSummoningMateria(),
            new LizardmanSummoningMateria(),
            new LlamaSummoningMateria(),
            new MaidenBreathMateria(),
            new MaidenCircleMateria(),
            new MaidenLineMateria(),
            new MinotaurCaptainSummoningMateria(),
            new MountainGoatSummoningMateria(),
            new MummySummoningMateria(),
            new MushroomBreathMateria(),
            new MushroomCircleMateria(),
            new MushroomLineMateria(),
            new NightmareSummoningMateria(),
            new NutcrackerBreathMateria(),
            new NutcrackerCircleMateria(),
            new NutcrackerLineMateria(),
            new OFlaskBreathMateria(),
            new OFlaskCircleMateria(),
            new OFlaskMateria(),
            new OgreLordSummoningMateria(),
            new OgreSummoningMateria(),
            new OniSummoningMateria(),
            new OphidianArchmageSummoningMateria(),
            new OphidianKnightSummoningMateria(),
            new OrcBomberSummoningMateria(),
            new OrcBruteSummoningMateria(),
            new OrcCaptainSummoningMateria(),
            new OrcishLordSummoningMateria(),
            new OrcishMageSummoningMateria(),
            new OrcSummoningMateria(),
            new PackHorseSummoningMateria(),
            new PackLlamaSummoningMateria(),
            new PantherSummoningMateria(),
            new ParaBreathMateria(),
            new ParaCircleMateria(),
            new ParaLineMateria(),
            new PhoenixSummoningMateria(),
            new PigSummoningMateria(),
            new PixieSummoningMateria(),
            new PlagueBeastSummoningMateria(),
            new PoisonElementalSummoningMateria(),
            new PolarBearSummoningMateria(),
            new RabbitSummoningMateria(),
            new RaiJuSummoningMateria(),
            new RatmanArcherSummoningMateria(),
            new RatmanMageSummoningMateria(),
            new RatmanSummoningMateria(),
            new RatSummoningMateria(),
            new ReaperSummoningMateria(),
            new RevenantSummoningMateria(),
            new RidgebackSummoningMateria(),
            new RikktorSummoningMateria(),
            new RoninSummoningMateria(),
            new RuneBeetleSummoningMateria(),
            new RuneBreathMateria(),
            new RuneCircleMateria(),
            new RuneLineMateria(),
            new SatyrSummoningMateria(),
            new SavageShamanSummoningMateria(),
            new SavageSummoningMateria(),
            new SawBreathMateria(),
            new SawCircleMateria(),
            new SawLineMateria(),
            new ScaledSwampDragonSummoningMateria(),
            new ScorpionSummoningMateria(),
            new SeaSerpentSummoningMateria(),
            new ShadowWispSummoningMateria(),
            new ShadowWyrmSummoningMateria(),
            new SheepSummoningMateria(),
            new SilverSerpentSummoningMateria(),
            new SilverSteedSummoningMateria(),
            new SkeletalDragonSummoningMateria(),
            new SkeletalKnightSummoningMateria(),
            new SkeletalMageSummoningMateria(),
            new SkeletalMountSummoningMateria(),
            new SkeletonBreathMateria(),
            new SkeletonCircleMateria(),
            new SkeletonLineMateria(),
            new SkeletonSummoningMateria(),
            new SkullBreathMateria(),
            new SkullCircleMateria(),
            new SkullLineMateria(),
            new SlimeSummoningMateria(),
            new SmokeBreathMateria(),
            new SmokeCircleMateria(),
            new SmokeLineMateria(),
            new SnakeSummoningMateria(),
            new SnowElementalSummoningMateria(),
            new SnowLeopardSummoningMateria(),
            new SocketDeed(),
            new SocketDeed1(),
            new SocketDeed2(),
            new SocketDeed3(),
            new SocketDeed4(),
            new SocketDeed5(),
            new SparkleBreathMateria(),
            new SparkleCircleMateria(),
            new SparkleLineMateria(),
            new SpikeBreathMateria(),
            new SpikeCircleMateria(),
            new SpikeLineMateria(),
            new StoneBreathMateria(),
            new StoneCircleMateria(),
            new StoneLineMateria(),
            new SuccubusSummoningMateria(),
            new TimeBreathMateria(),
            new TimeCircleMateria(),
            new TimeLineMateria(),
            new TitanSummoningMateria(),
            new ToxicElementalSummoningMateria(),
            new TrapBreathMateria(),
            new TrapCircleMateria(),
            new TrapLineMateria(),
            new TreeBreathMateria(),
            new TroglodyteSummoningMateria(),
            new TrollSummoningMateria(),
            new UnicornSummoningMateria(),
            new ValoriteElementalSummoningMateria(),
            new VampireBatSummoningMateria(),
            new VeriteElementalSummoningMateria(),
            new VortexBreathMateria(),
            new VortexCircleMateria(),
            new VortexLineMateria(),
            new WalrusSummoningMateria(),
            new WaterBreathMateria(),
            new WaterCircleMateria(),
            new WaterElementalSummoningMateria(),
            new WaterLineMateria(),
            new WhiteWolfSummoningMateria(),
            new WhiteWyrmSummoningMateria(),
            new WispSummoningMateria(),
            new WraithSummoningMateria(),
            new WyvernSummoningMateria(),
            new ZombieSummoningMateria(),
            new MythicAmethyst(),
            new LegendaryAmethyst(),
            new AncientAmethyst(),
            new FenCrystal(),
            new RhoCrystal(),
            new RysCrystal(),
            new WyrCrystal(),
            new FreCrystal(),
            new TorCrystal(),
            new VelCrystal(),
            new XenCrystal(),
            new PolCrystal(),
            new WolCrystal(),
            new BalCrystal(),
            new TalCrystal(),
            new JalCrystal(),
            new RalCrystal(),
            new KalCrystal(),
            new MythicDiamond(),
            new LegendaryDiamond(),
            new AncientDiamond(),
            new MythicEmerald(),
            new LegendaryEmerald(),
            new AncientEmerald(),
            new KeyAugment(),
            new RadiantRhoCrystal(),
            new RadiantRysCrystal(),
            new RadiantWyrCrystal(),
            new RadiantFreCrystal(),
            new RadiantTorCrystal(),
            new RadiantVelCrystal(),
            new RadiantXenCrystal(),
            new RadiantPolCrystal(),
            new RadiantWolCrystal(),
            new RadiantBalCrystal(),
            new RadiantTalCrystal(),
            new RadiantJalCrystal(),
            new RadiantRalCrystal(),
            new RadiantKalCrystal(),
            new MythicRuby(),
            new LegendaryRuby(),
            new AncientRuby(),
            new TyrRune(),
            new AhmRune(),
            new MorRune(),
            new MefRune(),
            new YlmRune(),
            new KotRune(),
            new JorRune(),
            new MythicSapphire(),
            new LegendarySapphire(),
            new AncientSapphire(),
            new MythicSkull(),
            new AncientSkull(),
            new LegendarySkull(),
            new GlimmeringGranite(),
            new GlimmeringClay(),
            new GlimmeringHeartstone(),
            new GlimmeringGypsum(),
            new GlimmeringIronOre(),
            new GlimmeringOnyx(),
            new GlimmeringMarble(),
            new GlimmeringPetrifiedWood(),
            new GlimmeringLimestone(),
            new GlimmeringBloodrock(),
            new MythicTourmaline(),
            new LegendaryTourmaline(),
            new AncientTourmaline(),
            new MythicWood(),
            new LegendaryWood(),
            new AncientWood(),
            new BootsOfCommand(),
            new GlovesOfCommand(),
            new GrandmastersRobe(),
            new JesterHatOfCommand(),
            new PlateLeggingsOfCommand(),
            new AshAxe(),
            new BraceletOfNaturesBounty(),
            new CampersBackpack(),
            new ExtraPack(),
            new FrostwoodAxe(),
            new GoldenCrown(),
            new GoldenDrakelingScaleShield(),
            new HeartwoodAxe(),
            new IcicleStaff(),
            new LightLordsScepter(),
            new MasterBall(),
            new MasterWeaponOil(),
            new Pokeball(),
            new ShadowIronShovel(),
            new StolenTile(),
            new TrapGloves(),
            new TrapGorget(),
            new TrapLegs(),
            new TrapSleeves(),
            new TrapTunic(),
            new WeaponOil(),
            new WizardKey(),
            new YewAxe(),
            new AssassinsDagger(),
            new BagOfBombs(),
            new BagOfHealth(),
            new BagOfJuice(),
            new BanishingOrb(),
            new BanishingRod(),
            new BeggarKingsCrown(),
            new BloodSword(),
            new BloodwoodAxe(),
            new GlovesOfTheGrandmasterThief(),
            new MagicMasterKey(),
            new PlantingGloves(),
            new QuickswordEnilno(),
            new RodOfOrcControl(),
            new ScryingOrb(),
            new SiegeHammer(),
            new SnoopersMasterScope(),
            new ThiefsGlove(),
            new TileExcavatorShovel(),
            new TomeOfTime(),
            new UniversalAbsorbingDyeTub(),
            new AegisOfAthena(),
            new AegisOfValor(),
            new AlchemistsAmbition(),
            new AlchemistsConduit(),
            new AlchemistsGroundedBoots(),
            new AlchemistsHeart(),
            new AlchemistsPreciseGloves(),
            new AlchemistsResilientLeggings(),
            new AlchemistsVisionaryHelm(),
            new ApronOfFlames(),
            new ArkainesValorArms(),
            new ArtisansCraftedGauntlets(),
            new ArtisansHelm(),
            new AshlandersResilience(),
            new AstartesBattlePlate(),
            new AstartesGauntletsOfMight(),
            new AstartesHelmOfVigilance(),
            new AstartesShoulderGuard(),
            new AstartesWarBoots(),
            new AstartesWarGreaves(),
            new AtzirisStep(),
            new AVALANCHEDefender(),
            new AvatarsVestments(),
            new BardsNimbleStep(),
            new BeastmastersCrown(),
            new BeastmastersGrips(),
            new BeastsWhisperersRobe(),
            new BerserkersEmbrace(),
            new BlackMagesMysticRobe(),
            new BlackMagesRuneRobe(),
            new BlacksmithsBurden(),
            new BlackthornesSpur(),
            new BladedancersCloseHelm(),
            new BladedancersOrderShield(),
            new BladedancersPlateArms(),
            new BladeDancersPlateChest(),
            new BladeDancersPlateLegs(),
            new BlazePlateLegs(),
            new BombDisposalPlate(),
            new BootsOfBalladry(),
            new BootsOfFleetness(),
            new BootsOfSwiftness(),
            new BootsOfTheNetherTraveller(),
            new CarpentersCrown(),
            new CelesRunebladeBuckler(),
            new CetrasBlessing(),
            new ChefsHatOfFocus(),
            new CourtesansDaintyBuckler(),
            new CourtesansFlowingRobe(),
            new CourtesansGracefulHelm(),
            new CourtesansWhisperingBoots(),
            new CourtesansWhisperingGloves(),
            new CourtierDashingBoots(),
            new CourtiersEnchantedAmulet(),
            new CourtierSilkenRobe(),
            new CourtiersRegalCirclet(),
            new CovensShadowedHood(),
            new CreepersLeatherCap(),
            new CrownOfTheAbyss(),
            new DaedricWarHelm(),
            new DarkFathersCrown(),
            new DarkFathersDreadnaughtBoots(),
            new DarkFathersHeartplate(),
            new DarkFathersSoulGauntlets(),
            new DarkFathersVoidLeggings(),
            new DarkKnightsCursedChestplate(),
            new DarkKnightsDoomShield(),
            new DarkKnightsObsidianHelm(),
            new DarkKnightsShadowedGauntlets(),
            new DarkKnightsVoidLeggings(),
            new DemonspikeGuard(),
            new DespairsShadow(),
            new Doombringer(),
            new DragonbornChestplate(),
            new DragonsBulwark(),
            new DragoonsAegis(),
            new DwemerAegis(),
            new EbonyChainArms(),
            new EdgarsEngineerChainmail(),
            new EldarRuneGuard(),
            new ElixirProtector(),
            new EmberPlateArms(),
            new EnderGuardiansChestplate(),
            new ExodusBarrier(),
            new FalconersCoif(),
            new FlamePlateGorget(),
            new FortunesGorget(),
            new FortunesHelm(),
            new FortunesPlateArms(),
            new FortunesPlateChest(),
            new FortunesPlateLegs(),
            new FrostwardensBascinet(),
            new FrostwardensPlateChest(),
            new FrostwardensPlateGloves(),
            new FrostwardensPlateLegs(),
            new FrostwardensWoodenShield(),
            new GauntletsOfPrecision(),
            new GauntletsOfPurity(),
            new GauntletsOfTheWild(),
            new GloomfangChain(),
            new GlovesOfTheSilentAssassin(),
            new GlovesOfTransmutation(),
            new GoronsGauntlets(),
            new GreyWanderersStride(),
            new GuardianAngelArms(),
            new GuardianOfTheAbyss(),
            new GuardiansHeartplate(),
            new GuardiansHelm(),
            new HammerlordsArmguards(),
            new HammerlordsChestplate(),
            new HammerlordsHelm(),
            new HammerlordsLegplates(),
            new HammerlordsShield(),
            new HarmonyGauntlets(),
            new HarmonysGuard(),
            new HarvestersFootsteps(),
            new HarvestersGrasp(),
            new HarvestersGuard(),
            new HarvestersHelm(),
            new HarvestersStride(),
            new HexweaversMysticalGloves(),
            new HlaaluTradersCuffs(),
            new HyruleKnightsShield(),
            new ImmortalKingsIronCrown(),
            new InfernoPlateChest(),
            new InquisitorsGuard(),
            new IstarisTouch(),
            new JestersGleefulGloves(),
            new JestersMerryCap(),
            new JestersMischievousBuckler(),
            new JestersPlayfulTunic(),
            new JestersTricksterBoots(),
            new KnightsAegis(),
            new KnightsValorShield(),
            new LeggingsOfTheRighteous(),
            new LioneyesRemorse(),
            new LionheartPlate(),
            new LockesAdventurerLeather(),
            new LocksleyLeatherChest(),
            new LyricalGreaves(),
            new LyricistsInsight(),
            new MagitekInfusedPlate(),
            new MakoResonance(),
            new MaskedAvengersAgility(),
            new MaskedAvengersDefense(),
            new MaskedAvengersFocus(),
            new MaskedAvengersPrecision(),
            new MaskedAvengersVoice(),
            new MelodicCirclet(),
            new MerryMensStuddedGloves(),
            new MeteorWard(),
            new MinersHelmet(),
            new MinstrelsMelody(),
            new MisfortunesChains(),
            new MondainsSkull(),
            new MonksBattleWraps(),
            new MonksSoulGloves(),
            new MysticSeersPlate(),
            new MysticsGuard(),
            new NajsArcaneVestment(),
            new NaturesEmbraceBelt(),
            new NaturesEmbraceHelm(),
            new NaturesGuardBoots(),
            new NecklaceOfAromaticProtection(),
            new NecromancersBoneGrips(),
            new NecromancersDarkLeggings(),
            new NecromancersHood(),
            new NecromancersRobe(),
            new NecromancersShadowBoots(),
            new NightingaleVeil(),
            new NinjaWrappings(),
            new NottinghamStalkersLeggings(),
            new OrkArdHat(),
            new OutlawsForestBuckler(),
            new PhilosophersGreaves(),
            new PyrePlateHelm(),
            new RadiantCrown(),
            new RatsNest(),
            new ReconnaissanceBoots(),
            new RedoranDefendersGreaves(),
            new RedstoneArtificersGloves(),
            new RiotDefendersShield(),
            new RoguesShadowBoots(),
            new RoguesStealthShield(),
            new RoyalCircletHelm(),
            new SabatonsOfDawn(),
            new SerenadesEmbrace(),
            new SerpentScaleArmor(),
            new SerpentsEmbrace(),
            new ShadowGripGloves(),
            new ShaftstopArmor(),
            new ShaminosGreaves(),
            new SherwoodArchersCap(),
            new ShinobiHood(),
            new ShurikenBracers(),
            new SilentStepTabi(),
            new SilksOfTheVictor(),
            new SirensLament(),
            new SirensResonance(),
            new SkinOfTheVipermagi(),
            new SlitheringSeal(),
            new SolarisAegis(),
            new SolarisLorica(),
            new SOLDIERSMight(),
            new SorrowsGrasp(),
            new StealthOperatorsGear(),
            new StormcrowsGaze(),
            new StormforgedBoots(),
            new StormforgedGauntlets(),
            new StormforgedHelm(),
            new StormforgedLeggings(),
            new StormforgedPlateChest(),
            new Stormshield(),
            new StringOfEars(),
            new SummonersEmbrace(),
            new TabulaRasa(),
            new TacticalVest(),
            new TailorsTouch(),
            new TalsRashasRelic(),
            new TamersBindings(),
            new TechPriestMantle(),
            new TelvanniMagistersCap(),
            new TerrasMysticRobe(),
            new TheThinkingCap(),
            new ThiefsNimbleCap(),
            new ThievesGuildPants(),
            new ThundergodsVigor(),
            new TinkersTreads(),
            new ToxinWard(),
            new TunicOfTheWild(),
            new TyraelsVigil(),
            new ValkyriesWard(),
            new VeilOfSteel(),
            new Venomweave(),
            new VialWarden(),
            new VipersCoif(),
            new VirtueGuard(),
            new VortexMantle(),
            new VyrsGraspingGauntlets(),
            new WardensAegis(),
            new WhispersHeartguard(),
            new WhiteMagesDivineVestment(),
            new WhiteRidersGuard(),
            new WhiteSageCap(),
            new WildwalkersGreaves(),
            new WinddancerBoots(),
            new WisdomsCirclet(),
            new WisdomsEmbrace(),
            new WitchesBindingGloves(),
            new WitchesCursedRobe(),
            new WitchesEnchantedHat(),
            new WitchesEnchantedRobe(),
            new WitchesHeartAmulet(),
            new WitchesWhisperingBoots(),
            new WitchfireShield(),
            new WitchwoodGreaves(),
            new WraithsBane(),
            new WrestlersArmsOfPrecision(),
            new WrestlersChestOfPower(),
            new WrestlersGrippingGloves(),
            new WrestlersHelmOfFocus(),
            new WrestlersLeggingsOfBalance(),
            new ZorasFins(),
            new AdventurersBoots(),
            new AerobicsInstructorsLegwarmers(),
            new AmbassadorsCloak(),
            new AnglersSeabreezeCloak(),
            new ArchivistsShoes(),
            new ArrowsmithsSturdyBoots(),
            new ArtisansTimberShoes(),
            new AssassinsBandana(),
            new AssassinsMaskedCap(),
            new BaggyHipHopPants(),
            new BakersSoftShoes(),
            new BalladeersMuffler(),
            new BanditsHiddenCloak(),
            new BardOfErinsMuffler(),
            new BardsTunicOfStonehenge(),
            new BaristasMuffler(),
            new BeastmastersTanic(),
            new BeastmastersTonic(),
            new BeastmastersTunic(),
            new BeastmiastersTunic(),
            new BeatniksBeret(),
            new BeggarsLuckyBandana(),
            new BlacksmithsReinforcedGloves(),
            new BobbySoxersShoes(),
            new BohoChicSundress(),
            new BootsOfTheDeepCaverns(),
            new BowcraftersProtectiveCloak(),
            new BowyersInsightfulBandana(),
            new BreakdancersCap(),
            new CarpentersStalwartTunic(),
            new CartographersExploratoryTunic(),
            new CartographersHat(),
            new CeltidDruidsRobe(),
            new ChampagneToastTunic(),
            new ChefsGourmetApron(),
            new ClericsSacredSash(),
            new CourtesansGracefulKimono(),
            new CourtisansRefinedGown(),
            new CouturiersSundress(),
            new CraftsmansProtectiveGloves(),
            new CropTopMystic(),
            new CuratorsKilt(),
            new CyberpunkNinjaTabi(),
            new DancersEnchantedSkirt(),
            new DapperFedoraOfInsight(),
            new DarkLordsRobe(),
            new DataMagesDigitalCloak(),
            new DeepSeaTunic(),
            new DenimJacketOfReflection(),
            new DiplomatsTunic(),
            new DiscoDivaBoots(),
            new ElementalistsProtectiveCloak(),
            new ElvenSnowBoots(),
            new EmoSceneHairpin(),
            new ExplorersBoots(),
            new FilmNoirDetectivesTrenchCoat(),
            new FishermansSunHat(),
            new FishermansVest(),
            new FishmongersKilt(),
            new FletchersPrecisionGloves(),
            new FlowerChildSundress(),
            new ForestersTunic(),
            new ForgeMastersBoots(),
            new GazeCapturingVeil(),
            new GeishasGracefulKasa(),
            new GhostlyShroud(),
            new GlamRockersJacket(),
            new GlovesOfStonemasonry(),
            new GoGoBootsOfAgility(),
            new GrapplersTunic(),
            new GreenwichMagesRobe(),
            new GroovyBellBottomPants(),
            new GrungeBandana(),
            new HackersVRGoggles(),
            new HammerlordsCap(),
            new HarmonistsSoftShoes(),
            new HealersBlessedSandals(),
            new HealersFurCape(),
            new HelmetOfTheOreWhisperer(),
            new HerbalistsProtectiveHat(),
            new HerdersMuffler(),
            new HippiePeaceBandana(),
            new HippiesPeacefulSandals(),
            new IntriguersFeatheredHat(),
            new JazzMusiciansMuffler(),
            new KnightsHelmOfTheRoundTable(),
            new LeprechaunsLuckyHat(),
            new LorekeepersSash(),
            new LuchadorsMask(),
            new MapmakersInsightfulMuffler(),
            new MarinersLuckyBoots(),
            new MelodiousMuffler(),
            new MendersDivineRobe(),
            new MidnightRevelersBoots(),
            new MinersSturdyBoots(),
            new MinstrelsTunedTunic(),
            new MistletoeMuffler(),
            new ModStyleTunic(),
            new MoltenCloak(),
            new MonksMeditativeRobe(),
            new MummysWrappings(),
            new MysticsFeatheredHat(),
            new NaturalistsCloak(),
            new NaturesMuffler(),
            new NavigatorsProtectiveCap(),
            new NecromancersCape(),
            new NeonStreetSash(),
            new NewWaveNeonShades(),
            new NinjasKasa(),
            new NinjasStealthyTabi(),
            new OreSeekersBandana(),
            new PickpocketsNimbleGloves(),
            new PickpocketsSleekTunic(),
            new PinUpHalterDress(),
            new PlatformSneakers(),
            new PoodleSkirtOfCharm(),
            new PopStarsFingerlessGloves(),
            new PopStarsGlitteringCap(),
            new PopStarsSparklingBandana(),
            new PreserversCap(),
            new PsychedelicTieDyeShirt(),
            new PsychedelicWizardsHat(),
            new PumpkinKingsCrown(),
            new QuivermastersTunic(),
            new RangersCap(),
            new RangersHat(),
            new RangersHatNightSight(),
            new ReindeerFurCap(),
            new ResolutionKeepersSash(),
            new RingmastersSandals(),
            new RockabillyRebelJacket(),
            new RoguesDeceptiveMask(),
            new RoguesShadowCloak(),
            new RoyalGuardsBoots(),
            new SamuraisHonorableTunic(),
            new SantasEnchantedRobe(),
            new SawyersMightyApron(),
            new ScholarsRobe(),
            new ScoutsWideBrimHat(),
            new ScribersRobe(),
            new ScribesEnlightenedSandals(),
            new ScriptoriumMastersRobe(),
            new SeductressSilkenShoes(),
            new SeersMysticSash(),
            new ShadowWalkersTabi(),
            new ShanachiesStorytellingShoes(),
            new ShepherdsKilt(),
            new SherlocksSleuthingCap(),
            new ShogunsAuthoritativeSurcoat(),
            new SilentNightCloak(),
            new SkatersBaggyPants(),
            new SmithsProtectiveTunic(),
            new SneaksSilentShoes(),
            new SnoopersSoftGloves(),
            new SommelierBodySash(),
            new SorceressMidnightRobe(),
            new SpellweaversEnchantedShoes(),
            new StarletsFancyDress(),
            new StarlightWizardsHat(),
            new StarlightWozardsHat(),
            new StreetArtistsBaggyPants(),
            new StreetPerformersCap(),
            new SubmissionsArtistsMuffler(),
            new SurgeonsInsightfulMask(),
            new SwingsDancersShoes(),
            new TailorsFancyApron(),
            new TamersKilt(),
            new TamersMuffler(),
            new TechGurusGlasses(),
            new TechnomancersHoodie(),
            new ThiefsShadowTunic(),
            new ThiefsSilentShoes(),
            new TidecallersSandals(),
            new TruckersIconicCap(),
            new UrbanitesSneakers(),
            new VampiresMidnightCloak(),
            new VestOfTheVeinSeeker(),
            new WarHeronsCap(),
            new WarriorOfUlstersTunic(),
            new WarriorsBelt(),
            new WhisperersBoots(),
            new WhisperersSandals(),
            new WhisperingSandals(),
            new WhisperingSondals(),
            new WhisperingWindSash(),
            new WitchesBewitchingRobe(),
            new WitchesBrewedHat(),
            new WoodworkersInsightfulCap(),
            new AegisShield(),
            new AeonianBow(),
            new AlamoDefendersAxe(),
            new AlucardsBlade(),
            new AnubisWarMace(),
            new ApepsCoiledScimitar(),
            new ApollosSong(),
            new ArchersYewBow(),
            new AssassinsKryss(),
            new AtmaBlade(),
            new AxeOfTheJuggernaut(),
            new AxeOfTheRuneweaver(),
            new BaneOfTheDead(),
            new BanshoFanClub(),
            new BarbarossaScimitar(),
            new BardsBowOfDiscord(),
            new BeowulfsWarAxe(),
            new BismarckianWarAxe(),
            new Blackrazor(),
            new BlacksmithsWarHammer(),
            new BlackSwordOfMondain(),
            new BlackTailWhip(),
            new BladeOfTheStars(),
            new Bonehew(),
            new BowiesLegacy(),
            new BowOfAuriel(),
            new BowOfIsrafil(),
            new BowspritOfBluenose(),
            new BulKathosTribalGuardian(),
            new BusterSwordReplica(),
            new ButchersCleaver(),
            new CaduceusStaff(),
            new CelestialLongbow(),
            new CelestialScimitar(),
            new CetrasStaff(),
            new ChakramBlade(),
            new CharlemagnesWarAxe(),
            new CherubsBlade(),
            new ChillrendLongsword(),
            new ChuKoNu(),
            new CrissaegrimEdge(),
            new CthulhusGaze(),
            new CursedArmorCleaver(),
            new CustersLastStandBow(),
            new DaggerOfShadows(),
            new DavidsSling(),
            new DawnbreakerMace(),
            new DeadMansLegacy(),
            new DestructoDiscDagger(),
            new DianasMoonBow(),
            new DoomfletchsPrism(),
            new Doomsickle(),
            new DragonClaw(),
            new DragonsBreath(),
            new DragonsBreathWarAxe(),
            new DragonsScaleDagger(),
            new DragonsWrath(),
            new Dreamseeker(),
            new EarthshakerMaul(),
            new EbonyWarAxeOfVampires(),
            new EldritchBowOfShadows(),
            new EldritchWhisper(),
            new ErdricksBlade(),
            new Excalibur(),
            new ExcaliburLongsword(),
            new ExcalibursLegacy(),
            new FangOfStorms(),
            new FlamebaneWarAxe(),
            new FrostfireCleaver(),
            new FrostflameKatana(),
            new FuHaosBattleAxe(),
            new GenjiBow(),
            new GeomancersStaff(),
            new GhoulSlayersLongsword(),
            new GlassSword(),
            new GlassSwordOfValor(),
            new GoldbrandScimitar(),
            new GreenDragonCrescentBlade(),
            new Grimmblade(),
            new GrimReapersCleaver(),
            new GriswoldsEdge(),
            new GrognaksAxe(),
            new GuardianOfTheFey(),
            new GuillotineBladeDagger(),
            new HalberdOfHonesty(),
            new HanseaticCrossbow(),
            new HarmonyBow(),
            new HarpeBlade(),
            new HeartbreakerSunder(),
            new HelmOfDarkness(),
            new IlluminaDagger(),
            new InuitUluOfTheNorth(),
            new JoansDivineLongsword(),
            new JuggernautHammer(),
            new KaomsCleaver(),
            new KaomsMaul(),
            new Keenstrike(),
            new KhufusWarSpear(),
            new KingsSwordOfHaste(),
            new MaatsBalancedBow(),
            new MablungsDefender(),
            new MaceOfTheVoid(),
            new MageMasher(),
            new MageMusher(),
            new MagesStaff(),
            new MagicAxeOfGreatStrength(),
            new MagusRod(),
            new MakhairaOfAchilles(),
            new ManajumasKnife(),
            new MarssBattleAxeOfValor(),
            new MasamuneBlade(),
            new MasamuneKatana(),
            new MasamunesEdge(),
            new MasamunesGrace(),
            new MaulOfSulayman(),
            new MehrunesCleaver(),
            new MortuarySword(),
            new MosesStaff(),
            new MuramasasBloodlust(),
            new MusketeersRapier(),
            new MysticBowOfLight(),
            new MysticStaffOfElements(),
            new NaginataOfTomoeGozen(),
            new NebulaBow(),
            new NecromancersDagger(),
            new NeptunesTrident(),
            new NormanConquerorsBow(),
            new PaladinsChrysblade(),
            new PlasmaInfusedWarHammer(),
            new PlutosAbyssalMace(),
            new PotaraEarringClub(),
            new PowerPoleHalberd(),
            new PowersBeacon(),
            new ProhibitionClub(),
            new QamarDagger(),
            new QuasarAxe(),
            new RainbowBlade(),
            new RasSearingDagger(),
            new ReflectionShield(),
            new RevolutionarySabre(),
            new RielsRebellionSabre(),
            new RuneAss(),
            new RuneAxe(),
            new SaiyanTailWhip(),
            new SamsonsJawbone(),
            new SaxonSeax(),
            new SearingTouch(),
            new SerpentsFang(),
            new SerpentsVenomDagger(),
            new ShadowstrideBow(),
            new ShavronnesRapier(),
            new SkyPiercer(),
            new SoulTaker(),
            new StaffOfAeons(),
            new StaffOfApocalypse(),
            new StaffOfRainsWrath(),
            new StaffOfTheElements(),
            new StarfallDagger(),
            new Sunblade(),
            new SwordOfAlBattal(),
            new SwordOfGideon(),
            new TabulasDagger(),
            new TantoOfThe47Ronin(),
            new TempestHammer(),
            new TeutonicWarMace(),
            new TheFurnace(),
            new TheOculus(),
            new ThorsHammer(),
            new Thunderfury(),
            new Thunderstroke(),
            new TitansFury(),
            new TomahawkOfTecumseh(),
            new TouchOfAnguish(),
            new TriLithiumBlade(),
            new TwoShotCrossbow(),
            new UltimaGlaive(),
            new UmbraWarAxe(),
            new UndeadCrown(),
            new ValiantThrower(),
            new VampireKiller(),
            new VATSEnhancedDagger(),
            new VenomsSting(),
            new VoidsEmbrace(),
            new VolendrungWarHammer(),
            new VolendrungWorHammer(),
            new VoltaxicRiftLance(),
            new VoyageursPaddle(),
            new VulcansForgeHammer(),
            new WabbajackClub(),
            new WandOfWoh(),
            new Whelm(),
            new WhisperingWindWarMace(),
            new WhisperwindBow(),
            new WindDancersDagger(),
            new WindripperBow(),
            new Wizardspike(),
            new WondershotCrossbow(),
            new Xcalibur(),
            new YumiOfEmpressJingu(),
            new ZhugeFeathersFan(),
            new Zulfiqar(),
            new Apple()
            // Add other items as needed
        };

        public static Item GetRandomItem()
        {
            return ItemList[Utility.Random(ItemList.Length)];
        }
    }

// ---------- [Level 1] ----------
// Large, Medium and Small Crate
	[FlipableAttribute( 0xe3e, 0xe3f )] 
	public class TreasureLevel1 : BaseTreasureChestMod 
	{ 
		public override int DefaultGumpID{ get{ return 0x49; } }

		[Constructable] 
		public TreasureLevel1() : base( Utility.RandomList( 0xE3C, 0xE3E, 0x9a9 ) )
		{ 
			RequiredSkill = 52;
			LockLevel = this.RequiredSkill - Utility.Random( 1, 10 );
			MaxLockLevel = this.RequiredSkill;
			TrapType = TrapType.MagicTrap;
			TrapPower = 1 * Utility.Random( 1, 25 );

            DropItem(new Gold(30, 100));
            DropItem(new Bolt(10));
            DropItem(Loot.RandomClothing());

            AddLoot(Loot.RandomWeapon());
            AddLoot(Loot.RandomArmorOrShield());
            AddLoot(Loot.RandomJewelry());

			for (int i = Utility.Random(3) + 1; i > 0; i--) // random 1 to 3
				DropItem( Loot.RandomGem() );

            DropItem(CustomLoot.GetRandomItem()); // Add one item from the list
		}

		public TreasureLevel1( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt();
		} 
	}

// ---------- [Level 1 Hybrid] ----------
// Large, Medium and Small Crate
	[FlipableAttribute( 0xe3e, 0xe3f )] 
	public class TreasureLevel1h : BaseTreasureChestMod 
	{ 
		public override int DefaultGumpID{ get{ return 0x49; } }

		[Constructable] 
		public TreasureLevel1h() : base( Utility.RandomList( 0xE3C, 0xE3E, 0x9a9 ) ) 
		{ 
			RequiredSkill = 56;
			LockLevel = this.RequiredSkill - Utility.Random( 1, 10 );
			MaxLockLevel = this.RequiredSkill;
			TrapType = TrapType.MagicTrap;
			TrapPower = 1 * Utility.Random( 1, 25 );

			DropItem( new Gold( 10, 40 ) );
			DropItem( new Bolt( 5 ) );
			switch ( Utility.Random( 6 )) 
			{ 
				case 0: DropItem( new Candelabra()  ); break; 
				case 1: DropItem( new Candle() ); break; 
				case 2: DropItem( new CandleLarge() ); break; 
				case 3: DropItem( new CandleLong() ); break; 
				case 4: DropItem( new CandleShort() ); break; 
				case 5: DropItem( new CandleSkull() ); break; 
			}
			switch ( Utility.Random( 2 )) 
			{ 
				case 0: DropItem( new Shoes( Utility.Random( 1, 2 ) ) ); break; 
				case 1: DropItem( new Sandals( Utility.Random( 1, 2 ) ) ); break; 
			}

			switch ( Utility.Random( 3 )) 
			{ 
				case 0: DropItem( new BeverageBottle(BeverageType.Ale) ); break;
				case 1: DropItem( new BeverageBottle(BeverageType.Liquor) ); break;
				case 2: DropItem( new Jug(BeverageType.Cider) ); break;
			}

            DropItem(CustomLoot.GetRandomItem()); // Add one item from the list
		} 

		public TreasureLevel1h( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	}

// ---------- [Level 2] ----------
// Large, Medium and Small Crate
// Wooden, Metal and Metal Golden Chest
// Keg and Barrel
	[FlipableAttribute( 0xe43, 0xe42 )] 
	public class TreasureLevel2 : BaseTreasureChestMod 
	{
		[Constructable] 
		public TreasureLevel2() : base( Utility.RandomList( 0xe3c, 0xE3E, 0x9a9, 0xe42, 0x9ab, 0xe40, 0xe7f, 0xe77 ) ) 
		{ 
			RequiredSkill = 72;
			LockLevel = this.RequiredSkill - Utility.Random( 1, 10 );
			MaxLockLevel = this.RequiredSkill;
			TrapType = TrapType.MagicTrap;
			TrapPower = 2 * Utility.Random( 1, 25 );

			DropItem( new Gold( 70, 100 ) );
			DropItem( new Arrow( 10 ) );
			DropItem( Loot.RandomPotion() );
			for( int i = Utility.Random( 1, 2 ); i > 1; i-- )
			{
				Item ReagentLoot = Loot.RandomReagent();
				ReagentLoot.Amount = Utility.Random( 1, 2 );
				DropItem( ReagentLoot );
			}
			if (Utility.RandomBool()) //50% chance
				for (int i = Utility.Random(8) + 1; i > 0; i--)
					DropItem(Loot.RandomScroll(0, 39, SpellbookType.Regular));

			if (Utility.RandomBool()) //50% chance
				for (int i = Utility.Random(6) + 1; i > 0; i--)
					DropItem( Loot.RandomGem() );

            for (int i = 0; i < 2; i++) // Add two items from the list
                DropItem(CustomLoot.GetRandomItem());
		} 

		public TreasureLevel2( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	} 

// ---------- [Level 3] ----------
// Wooden, Metal and Metal Golden Chest
	[FlipableAttribute( 0x9ab, 0xe7c )] 
	public class TreasureLevel3 : BaseTreasureChestMod 
	{ 
		public override int DefaultGumpID{ get{ return 0x4A; } }

		[Constructable] 
		public TreasureLevel3() : base( Utility.RandomList( 0x9ab, 0xe40, 0xe42 ) ) 
		{ 
			RequiredSkill = 84;
			LockLevel = this.RequiredSkill - Utility.Random( 1, 10 );
			MaxLockLevel = this.RequiredSkill;
			TrapType = TrapType.MagicTrap;
			TrapPower = 3 * Utility.Random( 1, 25 );

			DropItem( new Gold( 180, 240 ) );
			DropItem( new Arrow( 10 ) );

			for( int i = Utility.Random( 1, 3 ); i > 1; i-- )
			{
				Item ReagentLoot = Loot.RandomReagent();
				ReagentLoot.Amount = Utility.Random( 1, 9 );
				DropItem( ReagentLoot );
			}

			for ( int i = Utility.Random( 1, 3 ); i > 1; i-- )
				DropItem( Loot.RandomPotion() );

			if ( 0.67 > Utility.RandomDouble() ) //67% chance = 2/3
				for (int i = Utility.Random(12) + 1; i > 0; i--)
					DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

			if ( 0.67 > Utility.RandomDouble() ) //67% chance = 2/3
				for (int i = Utility.Random(9) + 1; i > 0; i--)
					DropItem( Loot.RandomGem() );

			for( int i = Utility.Random( 1, 3 ); i > 1; i-- )
				DropItem( Loot.RandomWand() );

			// Magical ArmorOrWeapon
			for( int i = Utility.Random( 1, 3 ); i > 1; i-- )
			{
				Item item = Loot.RandomArmorOrShieldOrWeapon();

                if (!Core.AOS)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;
                        weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(3);
                        weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(3);
                        weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(3);
                        weapon.Quality = ItemQuality.Normal;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;
                        armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(3);
                        armor.Durability = (ArmorDurabilityLevel)Utility.Random(3);
                        armor.Quality = ItemQuality.Normal;
                    }
                }
                else
                    AddLoot(item);
			}

			for( int i = Utility.Random( 1, 2 ); i > 1; i-- )
				AddLoot( Loot.RandomClothing() );

			for( int i = Utility.Random( 1, 2 ); i > 1; i-- )
                AddLoot(Loot.RandomJewelry());

            for (int i = 0; i < 3; i++) // Add three items from the list
                DropItem(CustomLoot.GetRandomItem());
		} 

		public TreasureLevel3( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	} 

// ---------- [Level 4] ----------
// Wooden, Metal and Metal Golden Chest
	[FlipableAttribute( 0xe41, 0xe40 )] 
	public class TreasureLevel4 : BaseTreasureChestMod 
	{ 
		[Constructable] 
		public TreasureLevel4() : base( Utility.RandomList( 0xe40, 0xe42, 0x9ab ) )
		{ 
			RequiredSkill = 92;
			LockLevel = this.RequiredSkill - Utility.Random( 1, 10 );
			MaxLockLevel = this.RequiredSkill;
			TrapType = TrapType.MagicTrap;
			TrapPower = 4 * Utility.Random( 1, 25 );

			DropItem( new Gold( 200, 400 ) );
			DropItem( new BlankScroll( Utility.Random( 1, 4 ) ) );
			
			for( int i = Utility.Random( 1, 4 ); i > 1; i-- )
			{
				Item ReagentLoot = Loot.RandomReagent();
				ReagentLoot.Amount = Utility.Random( 6, 12 );
				DropItem( ReagentLoot );
			}
	
			for ( int i = Utility.Random( 1, 4 ); i > 1; i-- )
				DropItem( Loot.RandomPotion() );
			
			if ( 0.75 > Utility.RandomDouble() ) //75% chance = 3/4
				for (int i = Utility.RandomMinMax(8,16); i > 0; i--)
					DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

			if ( 0.75 > Utility.RandomDouble() ) //75% chance = 3/4
				for (int i = Utility.RandomMinMax(6,12) + 1; i > 0; i--)
					DropItem( Loot.RandomGem() );

			for( int i = Utility.Random( 1, 4 ); i > 1; i-- )
				DropItem( Loot.RandomWand() );

			// Magical ArmorOrWeapon
			for( int i = Utility.Random( 1, 4 ); i > 1; i-- )
			{
				Item item = Loot.RandomArmorOrShieldOrWeapon();

                if (!Core.AOS)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;
                        weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(4);
                        weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(4);
                        weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(4);
                        weapon.Quality = ItemQuality.Normal;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;
                        armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(4);
                        armor.Durability = (ArmorDurabilityLevel)Utility.Random(4);
                        armor.Quality = ItemQuality.Normal;
                    }
                }
                else
                    AddLoot(item);
			}

			for( int i = Utility.Random( 1, 2 ); i > 1; i-- )
				AddLoot( Loot.RandomClothing() );
			
			for( int i = Utility.Random( 1, 2 ); i > 1; i-- )
				AddLoot( Loot.RandomJewelry() );

            for (int i = 0; i < 4; i++) // Add four items from the list
                DropItem(CustomLoot.GetRandomItem());
			
			//DropItem( new MagicCrystalBall() );

			// Magic clothing (not implemented)
			
			// Magic jewelry (not implemented)
		} 

		public TreasureLevel4( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 

			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 

			int version = reader.ReadInt(); 
		} 
	} 

}
