using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.SpecialVendor
{
    public class SpecialVendorGump : Gump
    {
        private Mobile m_From;
        private List<Item> itemsList = new List<Item>
        {
            // Your items list...
			new RandomMagicWeapon(),
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
        };

        private List<Item> currentRandomItems = new List<Item>();
        private int[] currentRandomPrices = new int[9];

        public SpecialVendorGump(Mobile from) : base(0, 0)
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
                    from.SendGump(new SpecialVendorGump(from));
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
                        from.SendGump(new SpecialVendorGump(from));
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
