using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Misc;
using Server.Engines.Craft;
using Server.Spells;
using Server.SkillHandlers;
using Server.Mobiles;
using Server.Engines.PartySystem;

namespace Server.Items
{
    public enum TreasureLevel
    {
        Stash,
        Supply,
        Cache,
        Hoard,
        Trove
    }

    public enum TreasurePackage
    {
        Artisan,
        Assassin,
        Mage,
        Ranger,
        Warrior
    }

    public enum TreasureFacet
    {
        Trammel,
        Felucca,
        Ilshenar,
        Malas,
        Tokuno,
        TerMur,
        Eodon
    }

    public enum ChestQuality
    {
        None,
        Rusty,
        Standard,
        Gold
    }

    public static class TreasureMapInfo
    {
        private static readonly Type[] _NewItemsList = new Type[]
		{
            typeof(Apple),
			typeof(RandomMagicWeapon), // Adjust chance as needed
			typeof(RandomMagicArmor),
			typeof(RandomMagicClothing),
			typeof(RandomMagicClothing),
			typeof(RandomMagicClothing),
			typeof(RandomMagicClothing),
			typeof(RandomMagicClothing),
			typeof(RandomMagicClothing),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomMagicWeapon),
			typeof(RandomSkillJewelryA),
			typeof(RandomSkillJewelryAA),
			typeof(RandomSkillJewelryAB),
			typeof(RandomSkillJewelryAC),
			typeof(RandomSkillJewelryAD),
			typeof(RandomSkillJewelryAE),
			typeof(RandomSkillJewelryAF),
			typeof(RandomSkillJewelryAG),
			typeof(RandomSkillJewelryAH),
			typeof(RandomSkillJewelryAI),
			typeof(RandomSkillJewelryAJ),
			typeof(RandomSkillJewelryAK),
			typeof(RandomSkillJewelryAL),
			typeof(RandomSkillJewelryAM),
			typeof(RandomSkillJewelryAN),
			typeof(RandomSkillJewelryAO),
			typeof(RandomSkillJewelryAP),
			typeof(RandomSkillJewelryB),
			typeof(RandomSkillJewelryC),
			typeof(RandomSkillJewelryD),
			typeof(RandomSkillJewelryE),
			typeof(RandomSkillJewelryF),
			typeof(RandomSkillJewelryG),
			typeof(RandomSkillJewelryH),
			typeof(RandomSkillJewelryI),
			typeof(RandomSkillJewelryJ),
			typeof(RandomSkillJewelryK),
			typeof(RandomSkillJewelryL),
			typeof(RandomSkillJewelryM),
			typeof(RandomSkillJewelryN),
			typeof(RandomSkillJewelryO),
			typeof(RandomSkillJewelryP),
			typeof(RandomSkillJewelryQ),
			typeof(RandomSkillJewelryR),
			typeof(RandomSkillJewelryS),
			typeof(RandomSkillJewelryT),
			typeof(RandomSkillJewelryU),
			typeof(RandomSkillJewelryV),
			typeof(RandomSkillJewelryW),
			typeof(RandomMagicJewelry),
			typeof(RandomSkillJewelryY),
			typeof(RandomSkillJewelryZ),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicJewelry),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(RandomMagicArmor),
			typeof(AlchemyAugmentCrystal),
			typeof(AnatomyAugmentCrystal),
			typeof(AnimalLoreAugmentCrystal),
			typeof(AnimalTamingAugmentCrystal),
			typeof(ArcheryAugmentCrystal),
			typeof(ArmsLoreAugmentCrystal),
			typeof(ArmSlotChangeDeed),
			typeof(BagOfBombs),
			typeof(BagOfHealth),
			typeof(BagOfJuice),
			typeof(BanishingOrb),
			typeof(BanishingRod),
			typeof(BeggingAugmentCrystal),
			typeof(BeltSlotChangeDeed),
			typeof(BlacksmithyAugmentCrystal),
			typeof(BloodSword),
			typeof(BootsOfCommand),
			typeof(BraceletSlotChangeDeed),
			typeof(BushidoAugmentCrystal),
			typeof(CampingAugmentCrystal),
			typeof(CapacityIncreaseDeed),
			typeof(CarpentryAugmentCrystal),
			typeof(CartographyAugmentCrystal),
			typeof(ChestSlotChangeDeed),
			typeof(ChivalryAugmentCrystal),
			typeof(ColdHitAreaCrystal),
			typeof(ColdResistAugmentCrystal),
			typeof(CookingAugmentCrystal),
			typeof(CurseAugmentCrystal),
			typeof(DetectingHiddenAugmentCrystal),
			typeof(DiscordanceAugmentCrystal),
			typeof(DispelAugmentCrystal),
			typeof(EarringSlotChangeDeed),
			typeof(EnergyHitAreaCrystal),
			typeof(EnergyResistAugmentCrystal),
			typeof(FatigueAugmentCrystal),
			typeof(FencingAugmentCrystal),
			typeof(FireballAugmentCrystal),
			typeof(FireHitAreaCrystal),
			typeof(FireResistAugmentCrystal),
			typeof(FishingAugmentCrystal),
			typeof(FletchingAugmentCrystal),
			typeof(FocusAugmentCrystal),
			typeof(FootwearSlotChangeDeed),
			typeof(ForensicEvaluationAugmentCrystal),
			typeof(GlovesOfCommand),
			typeof(HarmAugmentCrystal),
			typeof(HeadSlotChangeDeed),
			typeof(HealingAugmentCrystal),
			typeof(HerdingAugmentCrystal),
			typeof(HidingAugmentCrystal),
			typeof(ImbuingAugmentCrystal),
			typeof(InscriptionAugmentCrystal),
			typeof(ItemIdentificationAugmentCrystal),
			typeof(JesterHatOfCommand),
			typeof(LegsSlotChangeDeed),
			typeof(LifeLeechAugmentCrystal),
			typeof(LightningAugmentCrystal),
			typeof(LockpickingAugmentCrystal),
			typeof(LowerAttackAugmentCrystal),
			typeof(LuckAugmentCrystal),
			typeof(LumberjackingAugmentCrystal),
			typeof(MaceFightingAugmentCrystal),
			typeof(MageryAugmentCrystal),
			typeof(ManaDrainAugmentCrystal),
			typeof(ManaLeechAugmentCrystal),
			typeof(MaxxiaScroll),
			typeof(MeditationAugmentCrystal),
			typeof(MiningAugmentCrystal),
			typeof(MirrorOfKalandra),
			typeof(MusicianshipAugmentCrystal),
			typeof(NeckSlotChangeDeed),
			typeof(NecromancyAugmentCrystal),
			typeof(NinjitsuAugmentCrystal),
			typeof(OneHandedTransformDeed),
			typeof(ParryingAugmentCrystal),
			typeof(PeacemakingAugmentCrystal),
			typeof(PhysicalHitAreaCrystal),
			typeof(PhysicalResistAugmentCrystal),
			typeof(PlateLeggingsOfCommand),
			typeof(PoisonHitAreaCrystal),
			typeof(PoisoningAugmentCrystal),
			typeof(PoisonResistAugmentCrystal),
			typeof(ProvocationAugmentCrystal),
			typeof(RemoveTrapAugmentCrystal),
			typeof(ResistingSpellsAugmentCrystal),
			typeof(RingSlotChangeDeed),
			typeof(RodOfOrcControl),
			typeof(ShirtSlotChangeDeed),
			typeof(SnoopingAugmentCrystal),
			typeof(SpellweavingAugmentCrystal),
			typeof(SpiritSpeakAugmentCrystal),
			typeof(StaminaLeechAugmentCrystal),
			typeof(StealingAugmentCrystal),
			typeof(StealthAugmentCrystal),
			typeof(SwingSpeedAugmentCrystal),
			typeof(SwordsmanshipAugmentCrystal),
			typeof(TacticsAugmentCrystal),
			typeof(TailoringAugmentCrystal),
			typeof(TalismanSlotChangeDeed),
			typeof(TasteIDAugmentCrystal),
			typeof(ThrowingAugmentCrystal),
			typeof(TinkeringAugmentCrystal),
			typeof(TrackingAugmentCrystal),
			typeof(VeterinaryAugmentCrystal),
			typeof(WeaponSpeedAugmentCrystal),
			typeof(WrestlingAugmentCrystal),
			typeof(PetSlotDeed),
			typeof(PetBondDeed),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(StatCapOrb),
			typeof(SkillOrb),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(MaxxiaScroll),
			typeof(AbysmalHorrorSummoningMateria),
			typeof(AcidElementalSummoningMateria),
			typeof(AgapiteElementalSummoningMateria),
			typeof(AirElementalSummoningMateria),
			typeof(AlligatorSummoningMateria),
			typeof(AncientLichSummoningMateria),
			typeof(AncientWyrmSummoningMateria),
			typeof(AntLionSummoningMateria),
			typeof(ArcaneDaemonSummoningMateria),
			typeof(ArcticOgreLordSummoningMateria),
			typeof(AxeBreathMateria),
			typeof(AxeCircleMateria),
			typeof(AxeLineMateria),
			typeof(BakeKitsuneSummoningMateria),
			typeof(BalronSummoningMateria),
			typeof(BarracoonSummoningMateria),
			typeof(BeeBreathMateria),
			typeof(BeeCircleMateria),
			typeof(BeeLineMateria),
			typeof(BeetleSummoningMateria),
			typeof(BlackBearSummoningMateria),
			typeof(BlackDragoonPirateMateria),
			typeof(BlackSolenInfiltratorQueenSummoningMateria),
			typeof(BlackSolenInfiltratorWarriorMateria),
			typeof(BlackSolenQueenSummoningMateria),
			typeof(BlackSolenWarriorSummoningMateria),
			typeof(BlackSolenWorkerSummoningMateria),
			typeof(BladesBreathMateria),
			typeof(BladesCircleMateria),
			typeof(BladesLineMateria),
			typeof(BloodElementalSummoningGem),
			typeof(BloodSwarmGem),
			typeof(BoarSummoningMateria),
			typeof(BogleSummoningMateria),
			typeof(BoglingSummoningMateria),
			typeof(BogThingSummoningMateria),
			typeof(BoneDemonSummoningMateria),
			typeof(BoneKnightSummoningMateria),
			typeof(BoneMagiSummoningMateria),
			typeof(BoulderBreathMateria),
			typeof(BoulderCircleMateria),
			typeof(BoulderLineMateria),
			typeof(BrigandSummoningMateria),
			typeof(BronzeElementalSummoningMateria),
			typeof(BrownBearSummoningMateria),
			typeof(BullFrogSummoningMateria),
			typeof(BullSummoningMateria),
			typeof(CatSummoningMateria),
			typeof(CentaurSummoningMateria),
			typeof(ChaosDaemonSummoningMateria),
			typeof(ChaosDragoonEliteSummoningMateria),
			typeof(ChaosDragoonSummoningMateria),
			typeof(ChickenSummoningMateria),
			typeof(CopperElementalSummoningMateria),
			typeof(CorpserSummoningMateria),
			typeof(CorrosiveSlimeSummoningMateria),
			typeof(CorruptedSoulMateria),
			typeof(CougarSummoningMateria),
			typeof(CowSummoningMateria),
			typeof(CraneSummoningMateria),
			typeof(CrankBreathMateria),
			typeof(CrankCircleMateria),
			typeof(CrankLineMateria),
			typeof(CrimsonDragonSummoningMateria),
			typeof(CrystalElementalSummoningMateria),
			typeof(CurtainBreathMateria),
			typeof(CurtainCircleMateria),
			typeof(CurtainLineMateria),
			typeof(CuSidheSummoningMateria),
			typeof(CyclopsSummoningMateria),
			typeof(DaemonSummoningMateria),
			typeof(DarkWispSummoningMateria),
			typeof(DarkWolfSummoningMateria),
			typeof(DeathWatchBeetleSummoningMateria),
			typeof(DeepSeaSerpentSummoningMateria),
			typeof(DeerBreathMateria),
			typeof(DeerCircleMateria),
			typeof(DeerLineMateria),
			typeof(DemonKnightSummoningMateria),
			typeof(DesertOstardSummoningMateria),
			typeof(DevourerSummoningMateria),
			typeof(DireWolfSummoningMateria),
			typeof(DogSummoningMateria),
			typeof(DolphinSummoningMateria),
			typeof(DopplegangerSummoningMateria),
			typeof(DragonSummoningMateria),
			typeof(DrakeSummoningMateria),
			typeof(DreadSpiderSummoningMateria),
			typeof(DullCopperElementalSummoningMateria),
			typeof(DVortexBreathMateria),
			typeof(DVortexCircleMateria),
			typeof(DVortexLineMateria),
			typeof(EagleSummoningMateria),
			typeof(EarthElementalSummoningMateria),
			typeof(EfreetSummoningMateria),
			typeof(ElderGazerSummoningMateria),
			typeof(EliteNinjaSummoningMateria),
			typeof(EttinSummoningMateria),
			typeof(EvilHealerSummoningMateria),
			typeof(EvilMageSummoningMateria),
			typeof(ExecutionerMateria),
			typeof(ExodusMinionSummoningMateria),
			typeof(ExodusOverseerSummoningMateria),
			typeof(FanDancerSummoningMateria),
			typeof(FeralTreefellowSummoningMateria),
			typeof(FetidEssenceMateria),
			typeof(FireBeetleSummoningMateria),
			typeof(FireElementalSummoningMateria),
			typeof(FireGargoyleSummoningMateria),
			typeof(FireSteedSummoningMateria),
			typeof(FlaskBreathMateria),
			typeof(FlaskCircleMateria),
			typeof(FlaskLineMateria),
			typeof(FleshGolemSummoningMateria),
			typeof(FleshRendererSummoningMateria),
			typeof(ForestOstardSummoningMateria),
			typeof(FrenziedOstardSummoningMateria),
			typeof(FrostOozeSummoningMateria),
			typeof(FrostSpiderSummoningMateria),
			typeof(FrostTrollSummoningMateria),
			typeof(FTreeCircleMateria),
			typeof(FTreeLineMateria),
			typeof(GamanSummoningMateria),
			typeof(GargoyleSummoningMateria),
			typeof(GasBreathMateria),
			typeof(GasCircleMateria),
			typeof(GasLineMateria),
			typeof(GateBreathMateria),
			typeof(GateCircleMateria),
			typeof(GateLineMateria),
			typeof(GazerSummoningMateria),
			typeof(GhoulSummoningMateria),
			typeof(GiantBlackWidowSummoningMateria),
			typeof(GiantRatSummoningMateria),
			typeof(GiantSerpentSummoningMateria),
			typeof(GiantSpiderSummoningMateria),
			typeof(GiantToadSummoningMateria),
			typeof(GibberlingSummoningMateria),
			typeof(GlowBreathMateria),
			typeof(GlowCircleMateria),
			typeof(GlowLineMateria),
			typeof(GoatSummoningMateria),
			typeof(GoldenElementalSummoningMateria),
			typeof(GolemSummoningMateria),
			typeof(GoreFiendSummoningMateria),
			typeof(GorillaSummoningMateria),
			typeof(GreaterDragonSummoningMateria),
			typeof(GreaterMongbatSummoningMateria),
			typeof(GreatHartSummoningMateria),
			typeof(GreyWolfSummoningMateria),
			typeof(GrizzlyBearSummoningMateria),
			typeof(GuillotineBreathMateria),
			typeof(GuillotineCircleMateria),
			typeof(GuillotineLineMateria),
			typeof(HarpySummoningMateria),
			typeof(HeadBreathMateria),
			typeof(HeadCircleMateria),
			typeof(HeadlessOneSummoningMateria),
			typeof(HeadLineMateria),
			typeof(HealerMateria),
			typeof(HeartBreathMateria),
			typeof(HeartCircleMateria),
			typeof(HeartLineMateria),
			typeof(HellCatSummoningMateria),
			typeof(HellHoundSummoningMateria),
			typeof(HellSteedSummoningMateria),
			typeof(HindSummoningMateria),
			typeof(HiryuSummoningMateria),
			typeof(HorseSummoningMateria),
			typeof(IceElementalSummoningMateria),
			typeof(IceFiendSummoningMateria),
			typeof(IceSerpentSummoningMateria),
			typeof(IceSnakeSummoningMateria),
			typeof(ImpSummoningMateria),
			typeof(JackRabbitSummoningMateria),
			typeof(KazeKemonoSummoningMateria),
			typeof(KirinSummoningMateria),
			typeof(LavaLizardSummoningMateria),
			typeof(LavaSerpentSummoningMateria),
			typeof(LavaSnakeSummoningMateria),
			typeof(LesserHiryuSummoningMateria),
			typeof(LichLordSummoningMateria),
			typeof(LichSummoningMateria),
			typeof(LizardmanSummoningMateria),
			typeof(LlamaSummoningMateria),
			typeof(MaidenBreathMateria),
			typeof(MaidenCircleMateria),
			typeof(MaidenLineMateria),
			typeof(MinotaurCaptainSummoningMateria),
			typeof(MountainGoatSummoningMateria),
			typeof(MummySummoningMateria),
			typeof(MushroomBreathMateria),
			typeof(MushroomCircleMateria),
			typeof(MushroomLineMateria),
			typeof(NightmareSummoningMateria),
			typeof(NutcrackerBreathMateria),
			typeof(NutcrackerCircleMateria),
			typeof(NutcrackerLineMateria),
			typeof(OFlaskBreathMateria),
			typeof(OFlaskCircleMateria),
			typeof(OFlaskMateria),
			typeof(OgreLordSummoningMateria),
			typeof(OgreSummoningMateria),
			typeof(OniSummoningMateria),
			typeof(OphidianArchmageSummoningMateria),
			typeof(OphidianKnightSummoningMateria),
			typeof(OrcBomberSummoningMateria),
			typeof(OrcBruteSummoningMateria),
			typeof(OrcCaptainSummoningMateria),
			typeof(OrcishLordSummoningMateria),
			typeof(OrcishMageSummoningMateria),
			typeof(OrcSummoningMateria),
			typeof(PackHorseSummoningMateria),
			typeof(PackLlamaSummoningMateria),
			typeof(PantherSummoningMateria),
			typeof(ParaBreathMateria),
			typeof(ParaCircleMateria),
			typeof(ParaLineMateria),
			typeof(PhoenixSummoningMateria),
			typeof(PigSummoningMateria),
			typeof(PixieSummoningMateria),
			typeof(PlagueBeastSummoningMateria),
			typeof(PoisonElementalSummoningMateria),
			typeof(PolarBearSummoningMateria),
			typeof(RabbitSummoningMateria),
			typeof(RaiJuSummoningMateria),
			typeof(RatmanArcherSummoningMateria),
			typeof(RatmanMageSummoningMateria),
			typeof(RatmanSummoningMateria),
			typeof(RatSummoningMateria),
			typeof(ReaperSummoningMateria),
			typeof(RevenantSummoningMateria),
			typeof(RidgebackSummoningMateria),
			typeof(RikktorSummoningMateria),
			typeof(RoninSummoningMateria),
			typeof(RuneBeetleSummoningMateria),
			typeof(RuneBreathMateria),
			typeof(RuneCircleMateria),
			typeof(RuneLineMateria),
			typeof(SatyrSummoningMateria),
			typeof(SavageShamanSummoningMateria),
			typeof(SavageSummoningMateria),
			typeof(SawBreathMateria),
			typeof(SawCircleMateria),
			typeof(SawLineMateria),
			typeof(ScaledSwampDragonSummoningMateria),
			typeof(ScorpionSummoningMateria),
			typeof(SeaSerpentSummoningMateria),
			typeof(ShadowWispSummoningMateria),
			typeof(ShadowWyrmSummoningMateria),
			typeof(SheepSummoningMateria),
			typeof(SilverSerpentSummoningMateria),
			typeof(SilverSteedSummoningMateria),
			typeof(SkeletalDragonSummoningMateria),
			typeof(SkeletalKnightSummoningMateria),
			typeof(SkeletalMageSummoningMateria),
			typeof(SkeletalMountSummoningMateria),
			typeof(SkeletonBreathMateria),
			typeof(SkeletonCircleMateria),
			typeof(SkeletonLineMateria),
			typeof(SkeletonSummoningMateria),
			typeof(SkullBreathMateria),
			typeof(SkullCircleMateria),
			typeof(SkullLineMateria),
			typeof(SlimeSummoningMateria),
			typeof(SmokeBreathMateria),
			typeof(SmokeCircleMateria),
			typeof(SmokeLineMateria),
			typeof(SnakeSummoningMateria),
			typeof(SnowElementalSummoningMateria),
			typeof(SnowLeopardSummoningMateria),
			typeof(SocketDeed),
			typeof(SocketDeed1),
			typeof(SocketDeed2),
			typeof(SocketDeed3),
			typeof(SocketDeed4),
			typeof(SocketDeed5),
			typeof(SparkleBreathMateria),
			typeof(SparkleCircleMateria),
			typeof(SparkleLineMateria),
			typeof(SpikeBreathMateria),
			typeof(SpikeCircleMateria),
			typeof(SpikeLineMateria),
			typeof(StoneBreathMateria),
			typeof(StoneCircleMateria),
			typeof(StoneLineMateria),
			typeof(SuccubusSummoningMateria),
			typeof(TimeBreathMateria),
			typeof(TimeCircleMateria),
			typeof(TimeLineMateria),
			typeof(TitanSummoningMateria),
			typeof(ToxicElementalSummoningMateria),
			typeof(TrapBreathMateria),
			typeof(TrapCircleMateria),
			typeof(TrapLineMateria),
			typeof(TreeBreathMateria),
			typeof(TroglodyteSummoningMateria),
			typeof(TrollSummoningMateria),
			typeof(UnicornSummoningMateria),
			typeof(ValoriteElementalSummoningMateria),
			typeof(VampireBatSummoningMateria),
			typeof(VeriteElementalSummoningMateria),
			typeof(VortexBreathMateria),
			typeof(VortexCircleMateria),
			typeof(VortexLineMateria),
			typeof(WalrusSummoningMateria),
			typeof(WaterBreathMateria),
			typeof(WaterCircleMateria),
			typeof(WaterElementalSummoningMateria),
			typeof(WaterLineMateria),
			typeof(WhiteWolfSummoningMateria),
			typeof(WhiteWyrmSummoningMateria),
			typeof(WispSummoningMateria),
			typeof(WraithSummoningMateria),
			typeof(WyvernSummoningMateria),
			typeof(ZombieSummoningMateria),
			typeof(MythicAmethyst),
			typeof(LegendaryAmethyst),
			typeof(AncientAmethyst),
			typeof(FenCrystal),
			typeof(RhoCrystal),
			typeof(RysCrystal),
			typeof(WyrCrystal),
			typeof(FreCrystal),
			typeof(TorCrystal),
			typeof(VelCrystal),
			typeof(XenCrystal),
			typeof(PolCrystal),
			typeof(WolCrystal),
			typeof(BalCrystal),
			typeof(TalCrystal),
			typeof(JalCrystal),
			typeof(RalCrystal),
			typeof(KalCrystal),
			typeof(MythicDiamond),
			typeof(LegendaryDiamond),
			typeof(AncientDiamond),
			typeof(MythicEmerald),
			typeof(LegendaryEmerald),
			typeof(AncientEmerald),
			typeof(KeyAugment),
			typeof(RadiantRhoCrystal),
			typeof(RadiantRysCrystal),
			typeof(RadiantWyrCrystal),
			typeof(RadiantFreCrystal),
			typeof(RadiantTorCrystal),
			typeof(RadiantVelCrystal),
			typeof(RadiantXenCrystal),
			typeof(RadiantPolCrystal),
			typeof(RadiantWolCrystal),
			typeof(RadiantBalCrystal),
			typeof(RadiantTalCrystal),
			typeof(RadiantJalCrystal),
			typeof(RadiantRalCrystal),
			typeof(RadiantKalCrystal),
			typeof(MythicRuby),
			typeof(LegendaryRuby),
			typeof(AncientRuby),
			typeof(TyrRune),
			typeof(AhmRune),
			typeof(MorRune),
			typeof(MefRune),
			typeof(YlmRune),
			typeof(KotRune),
			typeof(JorRune),
			typeof(MythicSapphire),
			typeof(LegendarySapphire),
			typeof(AncientSapphire),
			typeof(MythicSkull),
			typeof(AncientSkull),
			typeof(LegendarySkull),
			typeof(GlimmeringGranite),
			typeof(GlimmeringClay),
			typeof(GlimmeringHeartstone),
			typeof(GlimmeringGypsum),
			typeof(GlimmeringIronOre),
			typeof(GlimmeringOnyx),
			typeof(GlimmeringMarble),
			typeof(GlimmeringPetrifiedWood),
			typeof(GlimmeringLimestone),
			typeof(GlimmeringBloodrock),
			typeof(MythicTourmaline),
			typeof(LegendaryTourmaline),
			typeof(AncientTourmaline),
			typeof(MythicWood),
			typeof(LegendaryWood),
			typeof(AncientWood),
			typeof(BootsOfCommand),
			typeof(GlovesOfCommand),
			typeof(GrandmastersRobe),
			typeof(JesterHatOfCommand),
			typeof(PlateLeggingsOfCommand),
			typeof(AshAxe),
			typeof(BraceletOfNaturesBounty),
			typeof(CampersBackpack),
			typeof(ExtraPack),
			typeof(FrostwoodAxe),
			typeof(GoldenCrown),
			typeof(GoldenDragon),
			typeof(GoldenDrakelingScaleShield),
			typeof(HeartwoodAxe),
			typeof(IcicleStaff),
			typeof(LightLordsScepter),
			typeof(MasterBall),
			typeof(MasterWeaponOil),
			typeof(Pokeball),
			typeof(ShadowIronShovel),
			typeof(StolenTile),
			typeof(TrapGloves),
			typeof(TrapGorget),
			typeof(TrapLegs),
			typeof(TrapSleeves),
			typeof(TrapTunic),
			typeof(WeaponOil),
			typeof(WizardKey),
			typeof(YewAxe),
			typeof(AssassinsDagger),
			typeof(BagOfBombs),
			typeof(BagOfHealth),
			typeof(BagOfJuice),
			typeof(BanishingOrb),
			typeof(BanishingRod),
			typeof(BeggarKingsCrown),
			typeof(BloodSword),
			typeof(BloodwoodAxe),
			typeof(GlovesOfTheGrandmasterThief),
			typeof(MagicMasterKey),
			typeof(PlantingGloves),
			typeof(QuickswordEnilno),
			typeof(RodOfOrcControl),
			typeof(ScryingOrb),
			typeof(SiegeHammer),
			typeof(SnoopersMasterScope),
			typeof(ThiefsGlove),
			typeof(TileExcavatorShovel),
			typeof(TomeOfTime),
			typeof(UniversalAbsorbingDyeTub),
			typeof(AegisOfAthena),
			typeof(AegisOfValor),
			typeof(AlchemistsAmbition),
			typeof(AlchemistsConduit),
			typeof(AlchemistsGroundedBoots),
			typeof(AlchemistsHeart),
			typeof(AlchemistsPreciseGloves),
			typeof(AlchemistsResilientLeggings),
			typeof(AlchemistsVisionaryHelm),
			typeof(ApronOfFlames),
			typeof(ArkainesValorArms),
			typeof(ArtisansCraftedGauntlets),
			typeof(ArtisansHelm),
			typeof(AshlandersResilience),
			typeof(AstartesBattlePlate),
			typeof(AstartesGauntletsOfMight),
			typeof(AstartesHelmOfVigilance),
			typeof(AstartesShoulderGuard),
			typeof(AstartesWarBoots),
			typeof(AstartesWarGreaves),
			typeof(AtzirisStep),
			typeof(AVALANCHEDefender),
			typeof(AvatarsVestments),
			typeof(BardsNimbleStep),
			typeof(BeastmastersCrown),
			typeof(BeastmastersGrips),
			typeof(BeastsWhisperersRobe),
			typeof(BerserkersEmbrace),
			typeof(BlackMagesMysticRobe),
			typeof(BlackMagesRuneRobe),
			typeof(BlacksmithsBurden),
			typeof(BlackthornesSpur),
			typeof(BladedancersCloseHelm),
			typeof(BladedancersOrderShield),
			typeof(BladedancersPlateArms),
			typeof(BladeDancersPlateChest),
			typeof(BladeDancersPlateLegs),
			typeof(BlazePlateLegs),
			typeof(BombDisposalPlate),
			typeof(BootsOfBalladry),
			typeof(BootsOfFleetness),
			typeof(BootsOfSwiftness),
			typeof(BootsOfTheNetherTraveller),
			typeof(CarpentersCrown),
			typeof(CelesRunebladeBuckler),
			typeof(CetrasBlessing),
			typeof(ChefsHatOfFocus),
			typeof(CourtesansDaintyBuckler),
			typeof(CourtesansFlowingRobe),
			typeof(CourtesansGracefulHelm),
			typeof(CourtesansWhisperingBoots),
			typeof(CourtesansWhisperingGloves),
			typeof(CourtierDashingBoots),
			typeof(CourtiersEnchantedAmulet),
			typeof(CourtierSilkenRobe),
			typeof(CourtiersRegalCirclet),
			typeof(CovensShadowedHood),
			typeof(CreepersLeatherCap),
			typeof(CrownOfTheAbyss),
			typeof(DaedricWarHelm),
			typeof(DarkFathersCrown),
			typeof(DarkFathersDreadnaughtBoots),
			typeof(DarkFathersHeartplate),
			typeof(DarkFathersSoulGauntlets),
			typeof(DarkFathersVoidLeggings),
			typeof(DarkKnightsCursedChestplate),
			typeof(DarkKnightsDoomShield),
			typeof(DarkKnightsObsidianHelm),
			typeof(DarkKnightsShadowedGauntlets),
			typeof(DarkKnightsVoidLeggings),
			typeof(DemonspikeGuard),
			typeof(DespairsShadow),
			typeof(Doombringer),
			typeof(DragonbornChestplate),
			typeof(DragonsBulwark),
			typeof(DragoonsAegis),
			typeof(DwemerAegis),
			typeof(EbonyChainArms),
			typeof(EdgarsEngineerChainmail),
			typeof(EldarRuneGuard),
			typeof(ElixirProtector),
			typeof(EmberPlateArms),
			typeof(EnderGuardiansChestplate),
			typeof(ExodusBarrier),
			typeof(FalconersCoif),
			typeof(FlamePlateGorget),
			typeof(FortunesGorget),
			typeof(FortunesHelm),
			typeof(FortunesPlateArms),
			typeof(FortunesPlateChest),
			typeof(FortunesPlateLegs),
			typeof(FrostwardensBascinet),
			typeof(FrostwardensPlateChest),
			typeof(FrostwardensPlateGloves),
			typeof(FrostwardensPlateLegs),
			typeof(FrostwardensWoodenShield),
			typeof(GauntletsOfPrecision),
			typeof(GauntletsOfPurity),
			typeof(GauntletsOfTheWild),
			typeof(GloomfangChain),
			typeof(GlovesOfTheSilentAssassin),
			typeof(GlovesOfTransmutation),
			typeof(GoronsGauntlets),
			typeof(GreyWanderersStride),
			typeof(GuardianAngelArms),
			typeof(GuardianOfTheAbyss),
			typeof(GuardiansHeartplate),
			typeof(GuardiansHelm),
			typeof(HammerlordsArmguards),
			typeof(HammerlordsChestplate),
			typeof(HammerlordsHelm),
			typeof(HammerlordsLegplates),
			typeof(HammerlordsShield),
			typeof(HarmonyGauntlets),
			typeof(HarmonysGuard),
			typeof(HarvestersFootsteps),
			typeof(HarvestersGrasp),
			typeof(HarvestersGuard),
			typeof(HarvestersHelm),
			typeof(HarvestersStride),
			typeof(HexweaversMysticalGloves),
			typeof(HlaaluTradersCuffs),
			typeof(HyruleKnightsShield),
			typeof(ImmortalKingsIronCrown),
			typeof(InfernoPlateChest),
			typeof(InquisitorsGuard),
			typeof(IstarisTouch),
			typeof(JestersGleefulGloves),
			typeof(JestersMerryCap),
			typeof(JestersMischievousBuckler),
			typeof(JestersPlayfulTunic),
			typeof(JestersTricksterBoots),
			typeof(KnightsAegis),
			typeof(KnightsValorShield),
			typeof(LeggingsOfTheRighteous),
			typeof(LioneyesRemorse),
			typeof(LionheartPlate),
			typeof(LockesAdventurerLeather),
			typeof(LocksleyLeatherChest),
			typeof(LyricalGreaves),
			typeof(LyricistsInsight),
			typeof(MagitekInfusedPlate),
			typeof(MakoResonance),
			typeof(MaskedAvengersAgility),
			typeof(MaskedAvengersDefense),
			typeof(MaskedAvengersFocus),
			typeof(MaskedAvengersPrecision),
			typeof(MaskedAvengersVoice),
			typeof(MelodicCirclet),
			typeof(MerryMensStuddedGloves),
			typeof(MeteorWard),
			typeof(MinersHelmet),
			typeof(MinstrelsMelody),
			typeof(MisfortunesChains),
			typeof(MondainsSkull),
			typeof(MonksBattleWraps),
			typeof(MonksSoulGloves),
			typeof(MysticSeersPlate),
			typeof(MysticsGuard),
			typeof(NajsArcaneVestment),
			typeof(NaturesEmbraceBelt),
			typeof(NaturesEmbraceHelm),
			typeof(NaturesGuardBoots),
			typeof(NecklaceOfAromaticProtection),
			typeof(NecromancersBoneGrips),
			typeof(NecromancersDarkLeggings),
			typeof(NecromancersHood),
			typeof(NecromancersRobe),
			typeof(NecromancersShadowBoots),
			typeof(NightingaleVeil),
			typeof(NinjaWrappings),
			typeof(NottinghamStalkersLeggings),
			typeof(OrkArdHat),
			typeof(OutlawsForestBuckler),
			typeof(PhilosophersGreaves),
			typeof(PyrePlateHelm),
			typeof(RadiantCrown),
			typeof(RatsNest),
			typeof(ReconnaissanceBoots),
			typeof(RedoranDefendersGreaves),
			typeof(RedstoneArtificersGloves),
			typeof(RiotDefendersShield),
			typeof(RoguesShadowBoots),
			typeof(RoguesStealthShield),
			typeof(RoyalCircletHelm),
			typeof(SabatonsOfDawn),
			typeof(SerenadesEmbrace),
			typeof(SerpentScaleArmor),
			typeof(SerpentsEmbrace),
			typeof(ShadowGripGloves),
			typeof(ShaftstopArmor),
			typeof(ShaminosGreaves),
			typeof(SherwoodArchersCap),
			typeof(ShinobiHood),
			typeof(ShurikenBracers),
			typeof(SilentStepTabi),
			typeof(SilksOfTheVictor),
			typeof(SirensLament),
			typeof(SirensResonance),
			typeof(SkinOfTheVipermagi),
			typeof(SlitheringSeal),
			typeof(SolarisAegis),
			typeof(SolarisLorica),
			typeof(SOLDIERSMight),
			typeof(SorrowsGrasp),
			typeof(StealthOperatorsGear),
			typeof(StormcrowsGaze),
			typeof(StormforgedBoots),
			typeof(StormforgedGauntlets),
			typeof(StormforgedHelm),
			typeof(StormforgedLeggings),
			typeof(StormforgedPlateChest),
			typeof(Stormshield),
			typeof(StringOfEars),
			typeof(SummonersEmbrace),
			typeof(TabulaRasa),
			typeof(TacticalVest),
			typeof(TailorsTouch),
			typeof(TalsRashasRelic),
			typeof(TamersBindings),
			typeof(TechPriestMantle),
			typeof(TelvanniMagistersCap),
			typeof(TerrasMysticRobe),
			typeof(TheThinkingCap),
			typeof(ThiefsNimbleCap),
			typeof(ThievesGuildPants),
			typeof(ThundergodsVigor),
			typeof(TinkersTreads),
			typeof(ToxinWard),
			typeof(TunicOfTheWild),
			typeof(TyraelsVigil),
			typeof(ValkyriesWard),
			typeof(VeilOfSteel),
			typeof(Venomweave),
			typeof(VialWarden),
			typeof(VipersCoif),
			typeof(VirtueGuard),
			typeof(VortexMantle),
			typeof(VyrsGraspingGauntlets),
			typeof(WardensAegis),
			typeof(WhispersHeartguard),
			typeof(WhiteMagesDivineVestment),
			typeof(WhiteRidersGuard),
			typeof(WhiteSageCap),
			typeof(WildwalkersGreaves),
			typeof(WinddancerBoots),
			typeof(WisdomsCirclet),
			typeof(WisdomsEmbrace),
			typeof(WitchesBindingGloves),
			typeof(WitchesCursedRobe),
			typeof(WitchesEnchantedHat),
			typeof(WitchesEnchantedRobe),
			typeof(WitchesHeartAmulet),
			typeof(WitchesWhisperingBoots),
			typeof(WitchfireShield),
			typeof(WitchwoodGreaves),
			typeof(WraithsBane),
			typeof(WrestlersArmsOfPrecision),
			typeof(WrestlersChestOfPower),
			typeof(WrestlersGrippingGloves),
			typeof(WrestlersHelmOfFocus),
			typeof(WrestlersLeggingsOfBalance),
			typeof(ZorasFins),
			typeof(AdventurersBoots),
			typeof(AerobicsInstructorsLegwarmers),
			typeof(AmbassadorsCloak),
			typeof(AnglersSeabreezeCloak),
			typeof(ArchivistsShoes),
			typeof(ArrowsmithsSturdyBoots),
			typeof(ArtisansTimberShoes),
			typeof(AssassinsBandana),
			typeof(AssassinsMaskedCap),
			typeof(BaggyHipHopPants),
			typeof(BakersSoftShoes),
			typeof(BalladeersMuffler),
			typeof(BanditsHiddenCloak),
			typeof(BardOfErinsMuffler),
			typeof(BardsTunicOfStonehenge),
			typeof(BaristasMuffler),
			typeof(BeastmastersTanic),
			typeof(BeastmastersTonic),
			typeof(BeastmastersTunic),
			typeof(BeastmiastersTunic),
			typeof(BeatniksBeret),
			typeof(BeggarsLuckyBandana),
			typeof(BlacksmithsReinforcedGloves),
			typeof(BobbySoxersShoes),
			typeof(BohoChicSundress),
			typeof(BootsOfTheDeepCaverns),
			typeof(BowcraftersProtectiveCloak),
			typeof(BowyersInsightfulBandana),
			typeof(BreakdancersCap),
			typeof(CarpentersStalwartTunic),
			typeof(CartographersExploratoryTunic),
			typeof(CartographersHat),
			typeof(CeltidDruidsRobe),
			typeof(ChampagneToastTunic),
			typeof(ChefsGourmetApron),
			typeof(ClericsSacredSash),
			typeof(CourtesansGracefulKimono),
			typeof(CourtisansRefinedGown),
			typeof(CouturiersSundress),
			typeof(CraftsmansProtectiveGloves),
			typeof(CropTopMystic),
			typeof(CuratorsKilt),
			typeof(CyberpunkNinjaTabi),
			typeof(DancersEnchantedSkirt),
			typeof(DapperFedoraOfInsight),
			typeof(DarkLordsRobe),
			typeof(DataMagesDigitalCloak),
			typeof(DeepSeaTunic),
			typeof(DenimJacketOfReflection),
			typeof(DiplomatsTunic),
			typeof(DiscoDivaBoots),
			typeof(ElementalistsProtectiveCloak),
			typeof(ElvenSnowBoots),
			typeof(EmoSceneHairpin),
			typeof(ExplorersBoots),
			typeof(FilmNoirDetectivesTrenchCoat),
			typeof(FishermansSunHat),
			typeof(FishermansVest),
			typeof(FishmongersKilt),
			typeof(FletchersPrecisionGloves),
			typeof(FlowerChildSundress),
			typeof(ForestersTunic),
			typeof(ForgeMastersBoots),
			typeof(GazeCapturingVeil),
			typeof(GeishasGracefulKasa),
			typeof(GhostlyShroud),
			typeof(GlamRockersJacket),
			typeof(GlovesOfStonemasonry),
			typeof(GoGoBootsOfAgility),
			typeof(GrapplersTunic),
			typeof(GreenwichMagesRobe),
			typeof(GroovyBellBottomPants),
			typeof(GrungeBandana),
			typeof(HackersVRGoggles),
			typeof(HammerlordsCap),
			typeof(HarmonistsSoftShoes),
			typeof(HealersBlessedSandals),
			typeof(HealersFurCape),
			typeof(HelmetOfTheOreWhisperer),
			typeof(HerbalistsProtectiveHat),
			typeof(HerdersMuffler),
			typeof(HippiePeaceBandana),
			typeof(HippiesPeacefulSandals),
			typeof(IntriguersFeatheredHat),
			typeof(JazzMusiciansMuffler),
			typeof(KnightsHelmOfTheRoundTable),
			typeof(LeprechaunsLuckyHat),
			typeof(LorekeepersSash),
			typeof(LuchadorsMask),
			typeof(MapmakersInsightfulMuffler),
			typeof(MarinersLuckyBoots),
			typeof(MelodiousMuffler),
			typeof(MendersDivineRobe),
			typeof(MidnightRevelersBoots),
			typeof(MinersSturdyBoots),
			typeof(MinstrelsTunedTunic),
			typeof(MistletoeMuffler),
			typeof(ModStyleTunic),
			typeof(MoltenCloak),
			typeof(MonksMeditativeRobe),
			typeof(MummysWrappings),
			typeof(MysticsFeatheredHat),
			typeof(NaturalistsCloak),
			typeof(NaturesMuffler),
			typeof(NavigatorsProtectiveCap),
			typeof(NecromancersCape),
			typeof(NeonStreetSash),
			typeof(NewWaveNeonShades),
			typeof(NinjasKasa),
			typeof(NinjasStealthyTabi),
			typeof(OreSeekersBandana),
			typeof(PickpocketsNimbleGloves),
			typeof(PickpocketsSleekTunic),
			typeof(PinUpHalterDress),
			typeof(PlatformSneakers),
			typeof(PoodleSkirtOfCharm),
			typeof(PopStarsFingerlessGloves),
			typeof(PopStarsGlitteringCap),
			typeof(PopStarsSparklingBandana),
			typeof(PreserversCap),
			typeof(PsychedelicTieDyeShirt),
			typeof(PsychedelicWizardsHat),
			typeof(PumpkinKingsCrown),
			typeof(QuivermastersTunic),
			typeof(RangersCap),
			typeof(RangersHat),
			typeof(RangersHatNightSight),
			typeof(ReindeerFurCap),
			typeof(ResolutionKeepersSash),
			typeof(RingmastersSandals),
			typeof(RockabillyRebelJacket),
			typeof(RoguesDeceptiveMask),
			typeof(RoguesShadowCloak),
			typeof(RoyalGuardsBoots),
			typeof(SamuraisHonorableTunic),
			typeof(SantasEnchantedRobe),
			typeof(SawyersMightyApron),
			typeof(ScholarsRobe),
			typeof(ScoutsWideBrimHat),
			typeof(ScribersRobe),
			typeof(ScribesEnlightenedSandals),
			typeof(ScriptoriumMastersRobe),
			typeof(SeductressSilkenShoes),
			typeof(SeersMysticSash),
			typeof(ShadowWalkersTabi),
			typeof(ShanachiesStorytellingShoes),
			typeof(ShepherdsKilt),
			typeof(SherlocksSleuthingCap),
			typeof(ShogunsAuthoritativeSurcoat),
			typeof(SilentNightCloak),
			typeof(SkatersBaggyPants),
			typeof(SmithsProtectiveTunic),
			typeof(SneaksSilentShoes),
			typeof(SnoopersSoftGloves),
			typeof(SommelierBodySash),
			typeof(SorceressMidnightRobe),
			typeof(SpellweaversEnchantedShoes),
			typeof(StarletsFancyDress),
			typeof(StarlightWizardsHat),
			typeof(StarlightWozardsHat),
			typeof(StreetArtistsBaggyPants),
			typeof(StreetPerformersCap),
			typeof(SubmissionsArtistsMuffler),
			typeof(SurgeonsInsightfulMask),
			typeof(SwingsDancersShoes),
			typeof(TailorsFancyApron),
			typeof(TamersKilt),
			typeof(TamersMuffler),
			typeof(TechGurusGlasses),
			typeof(TechnomancersHoodie),
			typeof(ThiefsShadowTunic),
			typeof(ThiefsSilentShoes),
			typeof(TidecallersSandals),
			typeof(TruckersIconicCap),
			typeof(UrbanitesSneakers),
			typeof(VampiresMidnightCloak),
			typeof(VestOfTheVeinSeeker),
			typeof(WarHeronsCap),
			typeof(WarriorOfUlstersTunic),
			typeof(WarriorsBelt),
			typeof(WhisperersBoots),
			typeof(WhisperersSandals),
			typeof(WhisperingSandals),
			typeof(WhisperingSondals),
			typeof(WhisperingWindSash),
			typeof(WitchesBewitchingRobe),
			typeof(WitchesBrewedHat),
			typeof(WoodworkersInsightfulCap),
			typeof(AegisShield),
			typeof(AeonianBow),
			typeof(AlamoDefendersAxe),
			typeof(AlucardsBlade),
			typeof(AnubisWarMace),
			typeof(ApepsCoiledScimitar),
			typeof(ApollosSong),
			typeof(ArchersYewBow),
			typeof(AssassinsKryss),
			typeof(AtmaBlade),
			typeof(AxeOfTheJuggernaut),
			typeof(AxeOfTheRuneweaver),
			typeof(BaneOfTheDead),
			typeof(BanshoFanClub),
			typeof(BarbarossaScimitar),
			typeof(BardsBowOfDiscord),
			typeof(BeowulfsWarAxe),
			typeof(BismarckianWarAxe),
			typeof(Blackrazor),
			typeof(BlacksmithsWarHammer),
			typeof(BlackSwordOfMondain),
			typeof(BlackTailWhip),
			typeof(BladeOfTheStars),
			typeof(Bonehew),
			typeof(BowiesLegacy),
			typeof(BowOfAuriel),
			typeof(BowOfIsrafil),
			typeof(BowspritOfBluenose),
			typeof(BulKathosTribalGuardian),
			typeof(BusterSwordReplica),
			typeof(ButchersCleaver),
			typeof(CaduceusStaff),
			typeof(CelestialLongbow),
			typeof(CelestialScimitar),
			typeof(CetrasStaff),
			typeof(ChakramBlade),
			typeof(CharlemagnesWarAxe),
			typeof(CherubsBlade),
			typeof(ChillrendLongsword),
			typeof(ChuKoNu),
			typeof(CrissaegrimEdge),
			typeof(CthulhusGaze),
			typeof(CursedArmorCleaver),
			typeof(CustersLastStandBow),
			typeof(DaggerOfShadows),
			typeof(DavidsSling),
			typeof(DawnbreakerMace),
			typeof(DeadMansLegacy),
			typeof(DestructoDiscDagger),
			typeof(DianasMoonBow),
			typeof(DoomfletchsPrism),
			typeof(Doomsickle),
			typeof(DragonClaw),
			typeof(DragonsBreath),
			typeof(DragonsBreathWarAxe),
			typeof(DragonsScaleDagger),
			typeof(DragonsWrath),
			typeof(Dreamseeker),
			typeof(EarthshakerMaul),
			typeof(EbonyWarAxeOfVampires),
			typeof(EldritchBowOfShadows),
			typeof(EldritchWhisper),
			typeof(ErdricksBlade),
			typeof(Excalibur),
			typeof(ExcaliburLongsword),
			typeof(ExcalibursLegacy),
			typeof(FangOfStorms),
			typeof(FlamebaneWarAxe),
			typeof(FrostfireCleaver),
			typeof(FrostflameKatana),
			typeof(FuHaosBattleAxe),
			typeof(GenjiBow),
			typeof(GeomancersStaff),
			typeof(GhoulSlayersLongsword),
			typeof(GlassSword),
			typeof(GlassSwordOfValor),
			typeof(GoldbrandScimitar),
			typeof(GreenDragonCrescentBlade),
			typeof(Grimmblade),
			typeof(GrimReapersCleaver),
			typeof(GriswoldsEdge),
			typeof(GrognaksAxe),
			typeof(GuardianOfTheFey),
			typeof(GuillotineBladeDagger),
			typeof(HalberdOfHonesty),
			typeof(HanseaticCrossbow),
			typeof(HarmonyBow),
			typeof(HarpeBlade),
			typeof(HeartbreakerSunder),
			typeof(HelmOfDarkness),
			typeof(IlluminaDagger),
			typeof(InuitUluOfTheNorth),
			typeof(JoansDivineLongsword),
			typeof(JuggernautHammer),
			typeof(KaomsCleaver),
			typeof(KaomsMaul),
			typeof(Keenstrike),
			typeof(KhufusWarSpear),
			typeof(KingsSwordOfHaste),
			typeof(MaatsBalancedBow),
			typeof(MablungsDefender),
			typeof(MaceOfTheVoid),
			typeof(MageMasher),
			typeof(MageMusher),
			typeof(MagesStaff),
			typeof(MagicAxeOfGreatStrength),
			typeof(MagusRod),
			typeof(MakhairaOfAchilles),
			typeof(ManajumasKnife),
			typeof(MarssBattleAxeOfValor),
			typeof(MasamuneBlade),
			typeof(MasamuneKatana),
			typeof(MasamunesEdge),
			typeof(MasamunesGrace),
			typeof(MaulOfSulayman),
			typeof(MehrunesCleaver),
			typeof(MortuarySword),
			typeof(MosesStaff),
			typeof(MuramasasBloodlust),
			typeof(MusketeersRapier),
			typeof(MysticBowOfLight),
			typeof(MysticStaffOfElements),
			typeof(NaginataOfTomoeGozen),
			typeof(NebulaBow),
			typeof(NecromancersDagger),
			typeof(NeptunesTrident),
			typeof(NormanConquerorsBow),
			typeof(PaladinsChrysblade),
			typeof(PlasmaInfusedWarHammer),
			typeof(PlutosAbyssalMace),
			typeof(PotaraEarringClub),
			typeof(PowerPoleHalberd),
			typeof(PowersBeacon),
			typeof(ProhibitionClub),
			typeof(QamarDagger),
			typeof(QuasarAxe),
			typeof(RainbowBlade),
			typeof(RasSearingDagger),
			typeof(ReflectionShield),
			typeof(RevolutionarySabre),
			typeof(RielsRebellionSabre),
			typeof(RuneAss),
			typeof(RuneAxe),
			typeof(SaiyanTailWhip),
			typeof(SamsonsJawbone),
			typeof(SaxonSeax),
			typeof(SearingTouch),
			typeof(SerpentsFang),
			typeof(SerpentsVenomDagger),
			typeof(ShadowstrideBow),
			typeof(ShavronnesRapier),
			typeof(SkyPiercer),
			typeof(SoulTaker),
			typeof(StaffOfAeons),
			typeof(StaffOfApocalypse),
			typeof(StaffOfRainsWrath),
			typeof(StaffOfTheElements),
			typeof(StarfallDagger),
			typeof(Sunblade),
			typeof(SwordOfAlBattal),
			typeof(SwordOfGideon),
			typeof(TabulasDagger),
			typeof(TantoOfThe47Ronin),
			typeof(TempestHammer),
			typeof(TeutonicWarMace),
			typeof(TheFurnace),
			typeof(TheOculus),
			typeof(ThorsHammer),
			typeof(Thunderfury),
			typeof(Thunderstroke),
			typeof(TitansFury),
			typeof(TomahawkOfTecumseh),
			typeof(TouchOfAnguish),
			typeof(TriLithiumBlade),
			typeof(TwoShotCrossbow),
			typeof(UltimaGlaive),
			typeof(UmbraWarAxe),
			typeof(UndeadCrown),
			typeof(ValiantThrower),
			typeof(VampireKiller),
			typeof(VATSEnhancedDagger),
			typeof(VenomsSting),
			typeof(VoidsEmbrace),
			typeof(VolendrungWarHammer),
			typeof(VolendrungWorHammer),
			typeof(VoltaxicRiftLance),
			typeof(VoyageursPaddle),
			typeof(VulcansForgeHammer),
			typeof(WabbajackClub),
			typeof(WandOfWoh),
			typeof(Whelm),
			typeof(WhisperingWindWarMace),
			typeof(WhisperwindBow),
			typeof(WindDancersDagger),
			typeof(WindripperBow),
			typeof(Wizardspike),
			typeof(WondershotCrossbow),
			typeof(Xcalibur),
			typeof(YumiOfEmpressJingu),
			typeof(ZhugeFeathersFan),
			typeof(AbbasidsTreasureChest),
			typeof(AbyssalPlaneChest),
			typeof(AlehouseChest),
			typeof(AlienArtifactChest),
			typeof(AlienArtifaxChest),
			typeof(AlliedForcesTreasureChest),
			typeof(AnarchistsCache),
			typeof(AncientRelicChest),
			typeof(AngelBlessingChest),
			typeof(AnglersBounty),
			typeof(ArcadeKingsTreasure),
			typeof(ArcadeMastersVault),
			typeof(ArcaneTreasureChest),
			typeof(ArcanumChest),
			typeof(ArcheryBonusChest),
			typeof(AshokasTreasureChest),
			typeof(AshokaTreasureChest),
			typeof(AssassinsCoffer),
			typeof(AthenianTreasureChest),
			typeof(BabylonianChest),
			typeof(BakersDelightChest),
			typeof(BakersDolightChest),
			typeof(BavarianFestChest),
			typeof(BismarcksTreasureChest),
			typeof(BolsheviksLoot),
			typeof(BountyHuntersCache),
			typeof(BoyBandBox),
			typeof(BrewmastersChest),
			typeof(BritainsRoyalTreasuryChest),
			typeof(BuccaneersChest),
			typeof(CaesarChest),
			typeof(CandyCarnivalCoffer),
			typeof(CaptainCooksTreasure),
			typeof(CelticLegendsChest),
			typeof(ChamplainTreasureChest),
			typeof(CheeseConnoisseursCache),
			typeof(ChocolatierTreasureChest),
			typeof(CivilRightsStrongbox),
			typeof(CivilWarCache),
			typeof(CivilWarChest),
			typeof(CivilWorChest),
			typeof(ClownsWhimsicalChest),
			typeof(ColonialPioneersCache),
			typeof(ComradesCache),
			typeof(ConfederationCache),
			typeof(ConquistadorsHoard),
			typeof(CovenTreasuresChest),
			typeof(CyberneticCache),
			typeof(CyrusTreasure),
			typeof(DesertPharaohChest),
			typeof(DinerDelightChest),
			typeof(DoctorsBag),
			typeof(DojoLegacyChest),
			typeof(DragonGuardiansHoardChest),
			typeof(DragonHoardChest),
			typeof(DragonHoChest),
			typeof(DragonHodChest),
			typeof(DragonHorChest),
			typeof(DriveInTreasureTrove),
			typeof(DroidWorkshopChest),
			typeof(DynastyRelicsChest),
			typeof(EdisonsTreasureChest),
			typeof(EgyptianChest),
			typeof(EliteFoursVault),
			typeof(ElvenEnchantressChest),
			typeof(ElvenTreasuryChest),
			typeof(EmeraldIsleChest),
			typeof(EmperorJustinianCache),
			typeof(EmperorLegacyChest),
			typeof(EnchantedForestChest),
			typeof(EtherealPlaneChest),
			typeof(EuropeanRelicsChest),
			typeof(FairyDustChest),
			typeof(FirstNationsHeritageChest),
			typeof(FlowerPowerChest),
			typeof(FocusBonusChest),
			typeof(ForbiddenAlchemistsCache),
			typeof(FrontierExplorersStash),
			typeof(FunkyFashionChest),
			typeof(FurTradersChest),
			typeof(GalacticExplorersTrove),
			typeof(GalacticRelicsChest),
			typeof(GamersLootbox),
			typeof(GardenersParadiseChest),
			typeof(GeishasGift),
			typeof(GermanUnificationChest),
			typeof(GoldRushBountyChest),
			typeof(GoldRushRelicChest),
			typeof(GreasersGoldmineChest),
			typeof(GroovyVabesChest),
			typeof(GroovyVibesChest),
			typeof(GrungeRockersCache),
			typeof(HipHopRapVault),
			typeof(HolyRomanEmpireChest),
			typeof(HomewardBoundChest),
			typeof(HussarsChest),
			typeof(InfernalPlaneChest),
			typeof(InnovatorVault),
			typeof(JedisReliquary),
			typeof(JestersGigglingChest),
			typeof(JestersJamboreeChest),
			typeof(JestersJest),
			typeof(JudahsTreasureChest),
			typeof(JukeboxJewels),
			typeof(JustinianTreasureChest),
			typeof(KagesTreasureChest),
			typeof(KingdomsVaultChest),
			typeof(KingKamehamehaTreasure),
			typeof(KingsBest),
			typeof(KoscheisUndyingChest),
			typeof(LawyerBriefcase),
			typeof(LeprechaunsLootChest),
			typeof(LeprechaunsTrove),
			typeof(LouisTreasuryChest),
			typeof(MacingBonusChest),
			typeof(MagesArcaneChest),
			typeof(MagesRelicChest),
			typeof(MaharajaTreasureChest),
			typeof(MarioTreasureBox),
			typeof(MedievalEnglandChest),
			typeof(MerchantChest),
			typeof(MerchantFortuneChest),
			typeof(MermaidTreasureChest),
			typeof(MillenniumTimeCapsule),
			typeof(MimeSilentChest),
			typeof(MirageChest),
			typeof(ModMadnessTrunk),
			typeof(MondainsDarkSecretsChest),
			typeof(MysticalDaoChest),
			typeof(MysticalEnchantersChest),
			typeof(MysticEnigmaChest),
			typeof(MysticGardenCache),
			typeof(MysticMoonChest),
			typeof(NaturesBountyChest),
			typeof(NavyCaptainsChest),
			typeof(NecroAlchemicalChest),
			typeof(NeonNightsChest),
			typeof(NeroChest),
			typeof(NinjaChest),
			typeof(NordicExplorersChest),
			typeof(PatriotCache),
			typeof(PeachRoyalCache),
			typeof(PharaohsReliquary),
			typeof(PharaohsTreasure),
			typeof(PixieDustChest),
			typeof(PokeballTreasureChest),
			typeof(PolishRoyalChest),
			typeof(PopStarsTrove),
			typeof(RadBoomboxTrove),
			typeof(Radical90sRelicsChest),
			typeof(RadRidersStash),
			typeof(RailwayWorkersChest),
			typeof(RebelChest),
			typeof(RenaissanceCollectorsChest),
			typeof(RetroArcadeChest),
			typeof(RevolutionaryChess),
			typeof(RevolutionaryChest),
			typeof(RevolutionaryRelicChest),
			typeof(RevolutionChest),
			typeof(RhineValleyChest),
			typeof(RiverPiratesChest),
			typeof(RiverRaftersChest),
			typeof(RockersVault),
			typeof(RockNBallVault),
			typeof(RockNRallVault),
			typeof(RockNRollVault),
			typeof(RoguesHiddenChest),
			typeof(RomanBritanniaChest),
			typeof(RomanEmperorsVault),
			typeof(SamuraiHonorChest),
			typeof(SamuraiStash),
			typeof(SandstormChest),
			typeof(ScholarEnlightenmentChest),
			typeof(SeaDogsChest),
			typeof(ShinobiSecretsChest),
			typeof(SilkRoadTreasuresChest),
			typeof(SilverScreenChest),
			typeof(SithsVault),
			typeof(SlavicBrosChest),
			typeof(SlavicLegendsChest),
			typeof(SmugglersCache),
			typeof(SocialMediaMavensChest),
			typeof(SorceressSecretsChest),
			typeof(SpaceRaceCache),
			typeof(SpartanTreasureChest),
			typeof(SpecialChivalryChest),
			typeof(SpecialWoodenChestConstantine),
			typeof(SpecialWoodenChestExplorerLegacy),
			typeof(SpecialWoodenChestFrench),
			typeof(SpecialWoodenChestHelios),
			typeof(SpecialWoodenChestIvan),
			typeof(SpecialWoodenChestOisin),
			typeof(SpecialWoodenChestTomoe),
			typeof(SpecialWoodenChestWashington),
			typeof(StarfleetsVault),
			typeof(SugarplumFairyChest),
			typeof(SwingTimeChest),
			typeof(SwordsmanshipBonusChest),
			typeof(TacticsBonusChest),
			typeof(TangDynastyChest),
			typeof(TechnicolorTalesChest),
			typeof(TechnophilesCache),
			typeof(TeutonicRelicChest),
			typeof(TeutonicTreasuresChest),
			typeof(ThiefsHideawayStash),
			typeof(ToxicologistsTrove),
			typeof(TrailblazersTrove),
			typeof(TravelerChest),
			typeof(TreasureChestOfTheQinDynasty),
			typeof(TreasureChestOfTheThreeKingdoms),
			typeof(TsarsLegacyChest),
			typeof(TsarsRoyalChest),
			typeof(TsarsTreasureChest),
			typeof(TudorDynastyChest),
			typeof(UndergroundAnarchistsCache),
			typeof(USSRRelicsChest),
			typeof(VenetianMerchantsStash),
			typeof(VHSAdventureCache),
			typeof(VictorianEraChest),
			typeof(VikingChest),
			typeof(VintnersVault),
			typeof(VinylVault),
			typeof(VirtuesGuardianChest),
			typeof(WarOf1812Vault),
			typeof(WarringStatesChest),
			typeof(WingedHusChest),
			typeof(WingedHussarsChest),
			typeof(WitchsBrewChest),
			typeof(WorkersRevolutionChest),
			typeof(WorldWarIIChest),
			typeof(AbbadonTheAbyssalMap),
			typeof(AbyssalWardenMap),
			typeof(AbyssinianTrackerMap),
			typeof(AcidicAlligatorMap),
			typeof(AlchemicalLabMap),
			typeof(AncientAlligatorMap),
			typeof(AncientDragonsLairMap),
			typeof(AngusBerserkersCampMap),
			typeof(BloodstainedFieldsMap),
			typeof(CorruptedOrchardMap),
			typeof(DogTheBountyHuntersDenMap),
			typeof(DuelistPoetArenaMap),
			typeof(EarthClanNinjaLairMap),
			typeof(EarthClanSamuraiEncampmentMap),
			typeof(EvilAlchemistLaboratoryMap),
			typeof(EvilClownCarnivalMap),
			typeof(FairyQueenGladeMap),
			typeof(FastExplorerExpeditionMap),
			typeof(FireClanNinjaHideoutMap),
			typeof(FireClanSamuraiDojoMap),
			typeof(FlapperElementalistAltarMap),
			typeof(FloridaMansCarnivalMap),
			typeof(ForestRangerOutpostMap),
			typeof(FunkFungiFamiliarGardenMap),
			typeof(GangLeadersHideoutMap),
			typeof(GlamRockMageConcertMap),
			typeof(GothicNovelistCryptMap),
			typeof(GraffitiGargoyleAlleyMap),
			typeof(GreaserGryphonRidersArenaMap),
			typeof(GreenHagsSwampMap),
			typeof(GreenNinjasHiddenLairMap),
			typeof(HippieHoplitesGroveMap),
			typeof(HolyKnightsCathedralMap),
			typeof(HostileDruidsGladeMap),
			typeof(HostilePrincessCourtMap),
			typeof(IceKingsDomainMap),
			typeof(InfernoDragonsLairMap),
			typeof(InsaneRoboticistWorkshopMap),
			typeof(JazzAgeBrawlMap),
			typeof(JestersCourtMap),
			typeof(LawyersTribunalMap),
			typeof(LineDragonsAscentMap),
			typeof(LordBlackthornsDominionMap),
			typeof(LordBritishsSummoningCircleMap),
			typeof(MagmaElementalRiftMap),
			typeof(MedievalMeteorologistsObservatoryMap),
			typeof(MegaDragonsLairMap),
			typeof(MinaxSorceressSanctumMap),
			typeof(MischievousWitchCovenMap),
			typeof(MotownMermaidLagoonMap),
			typeof(MushroomWitchGroveMap),
			typeof(MusketeerHallMap),
			typeof(MysticGroveMap),
			typeof(NeovictorianVampireCourtMap),
			typeof(NinjaLibrarianSanctumMap),
			typeof(NoirDetectiveHideoutMap),
			typeof(OgreMastersDomainMap),
			typeof(PhoenixStyleMastersArenaMap),
			typeof(PigFarmersPenMap),
			typeof(PinupPandemoniumParlorMap),
			typeof(PirateOfTheStarsOutpostMap),
			typeof(PiratesCoveMap),
			typeof(PkMurderersHideoutMap),
			typeof(RaKingsPyramidMap),
			typeof(RanchMastersPrairieMap),
			typeof(RapRangersJungleMap),
			typeof(RaveRoguesUndergroundMap),
			typeof(RebelCathedralMap),
			typeof(RedQueensCourtMap),
			typeof(ReggaeRunesmithWorkshopMap),
			typeof(RenaissanceMechanicFactoryMap),
			typeof(RetroAndroidWorkshopMap),
			typeof(RetroFuturistDomeMap),
			typeof(RetroRobotRomancersLairMap),
			typeof(RingmastersCircusMap),
			typeof(RockabillyRevenantsStageMap),
			typeof(SanctuaryOfTheHolyKnightMap),
			typeof(ScorpomancersDomainMap),
			typeof(SilentMovieStudioMap),
			typeof(SilverSlimeCavernsMap),
			typeof(SithAcademyMap),
			typeof(SkaSkaldConcertHallMap),
			typeof(SkeletonLordCryptMap),
			typeof(SlimeMageSwampMap),
			typeof(SneakyNinjaClanMap),
			typeof(StarCitizenOutpostMap),
			typeof(StarfleetCaptainsCommandMap),
			typeof(StarfleetCommandCenterMap),
			typeof(SteampunkSamuraiForgeMap),
			typeof(StormtrooperAcademyMap),
			typeof(SurferSummonerCoveMap),
			typeof(SwampThingLairMap),
			typeof(SwinginSorceressBallroomMap),
			typeof(TexanRancherPrairieMap),
			typeof(TwistedCultistHideoutMap),
			typeof(VaudevilleValkyrieStageMap),
			typeof(WastelandBikerCompoundMap),
			typeof(WaterClanNinjaHideoutMap),
			typeof(WaterClanSamuraiFortressMap),
			typeof(WildWestWizardCanyonMap),
			typeof(FleshEaterOgreTombMap),
			typeof(FlyingSquirrelHollowMap),
			typeof(ForgottenWardenCryptMap),
			typeof(FoxSquirrelGlenMap),
			typeof(FrenziedSatyrsForestMap),
			typeof(FrostbiteWolfsDomainMap),
			typeof(FrostboundChampionsHallMap),
			typeof(FrostDroidFactoryMap),
			typeof(FrostLichsCryptMap),
			typeof(FrostOgreLairMap),
			typeof(FrostWapitiGroundsMap),
			typeof(FrostWardenWatchMap),
			typeof(FrozenOozeCaveMap),
			typeof(FungalToadSwampMap),
			typeof(GeminiHarpysLairMap),
			typeof(GeminiTwinBearsDenMap),
			typeof(GentleSatyrsGroveMap),
			typeof(GiantForestHogsNestMap),
			typeof(GiantWolfSpidersWebMap),
			typeof(GoliathBirdeatersLairMap),
			typeof(GoralsDomainMap),
			typeof(GrapplerDronesArenaMap),
			typeof(GraveKnightsCryptMap),
			typeof(GummySheepsPastureMap),
			typeof(InfernoStallionArenaMap),
			typeof(InfinitePouncersDenMap),
			typeof(IroncladDefendersFortressMap),
			typeof(IroncladOgresDomainMap),
			typeof(IronGolemsWorkshopMap),
			typeof(IronSteedStablesMap),
			typeof(JavelinaJinxHuntMap),
			typeof(JellybeanJestersCarnivalMap),
			typeof(KasTheBloodyhandedCryptMap),
			typeof(KelthuzadsFrozenCitadelMap),
			typeof(KhufuTheGreatBuildersTombMap),
			typeof(LairOfTheGibbonMysticsGroveMap),
			typeof(LairOfTheGlisteningOozesCavernMap),
			typeof(LairOfTheGloomOgresFortressMap),
			typeof(LairOfTheGloomWolfsDenMap),
			typeof(LairOfTheGoldenOrbWeaversWebMap),
			typeof(LairOfTheHatshepsutTheQueensTombMap),
			typeof(LairOfTheHogWildsSwinePenMap),
			typeof(LairOfTheHowlerMonkeysJungleMap),
			typeof(LairOfTheHuntsmanSpidersLairMap),
			typeof(LairOfTheHydrokineticWardensWaterShrineMap),
			typeof(LairOfTheIbexHighlandMap),
			typeof(LairOfTheIndianPalmSquirrelGroveMap),
			typeof(LairOfTheInfernalLichCitadelMap),
			typeof(LairOfTheInfernalToadSwampMap),
			typeof(LairOfTheInfernoSentinelFortressMap),
			typeof(LairOfTheLibraBalanceBearMap),
			typeof(LairOfTheLibraHarpyMap),
			typeof(LairOfTheLicoriceSheepMap),
			typeof(LairOfTheLollipopLordMap),
			typeof(LairOfTheLuchadorLlamaMap),
			typeof(LairOfTheMalariaratDenMap),
			typeof(LairOfTheMandrillshamanJungleMap),
			typeof(LairOfTheMarkhorPeaksMap),
			typeof(LairOfTheMeatgolemLaboratoryMap),
			typeof(LairOfTheMelodicsatyrGroveMap),
			typeof(LairOfTheMountainGorillaMap),
			typeof(LairOfTheMuckGolemMap),
			typeof(LairOfTheMysticFallowMap),
			typeof(LairOfTheMysticSatyrMap),
			typeof(LairOfTheNagashMap),
			typeof(LairOfTheNefertitisTombMap),
			typeof(LarlochTheShadowKingsCryptMap),
			typeof(LeoTheHarpysLairMap),
			typeof(LeoTheSunBearsDenMap),
			typeof(LeprosyRatNestMap),
			typeof(MentuhotepTheWiseTombMap),
			typeof(MetallicWindsteedPeaksMap),
			typeof(MimicronsLairMap),
			typeof(MireSpawnerMarshMap),
			typeof(MoltenSlimePitMap),
			typeof(NanoSwarmLabMap),
			typeof(NebulaCatsCelestialRealmMap),
			typeof(NecroticGeneralsBattlefieldMap),
			typeof(NecroticLichsTombMap),
			typeof(NecroticOgresDomainMap),
			typeof(NemesisUnitFacilityMap),
			typeof(NightmareLeaperAbyssMap),
			typeof(NightmarePanthersDomainMap),
			typeof(OmegaSentinelsFortressMap),
			typeof(OrangutanSageGroveMap),
			typeof(OverlordMkiiStrongholdMap),
			typeof(PeccaryProtectorForestMap),
			typeof(PeppermintPuffDomainMap),
			typeof(PhantomAutomatonVaultMap),
			typeof(PhantomPanthersDomainMap),
			typeof(PlagueLichsCryptMap),
			typeof(PlasmaJuggernautsForgeMap),
			typeof(PurseSpidersLairMap),
			typeof(QuantumGuardiansRealmMap),
			typeof(RabidRatLairMap),
			typeof(RadiantSlimeCavernMap),
			typeof(RaistlinMajeresTowerMap),
			typeof(RamsesTheImmortalTombMap),
			typeof(RedSquirrelNestMap),
			typeof(RedtailedSquirrelGroveMap),
			typeof(BreezePhantomAbyssMap),
			typeof(BubbleFerretForestMap),
			typeof(CelestialDragonShrineMap),
			typeof(CerebralEttinCavernMap),
			typeof(ChanequeGroveMap),
			typeof(ChimereonSwampMap),
			typeof(CinderWraithRuinsMap),
			typeof(CorruptingCreeperForestMap),
			typeof(CrystalDragonCavernMap),
			typeof(CrystalWardenTempleMap),
			typeof(CursedHarbingerCryptMap),
			typeof(CycloneDemonPlainsMap),
			typeof(DairyWraithFieldMap),
			typeof(DeadlordFortressMap),
			typeof(DreadedCreeperHollowMap),
			typeof(DreamyFerretHollowMap),
			typeof(DrolaticWastesMap),
			typeof(DryadGroveMap),
			typeof(EarthquakeEttinDenMap),
			typeof(ElderTendrilSwampMap),
			typeof(EmberSerpentLairMap),
			typeof(EmberSpiritDomainMap),
			typeof(EtherealCrabNestMap),
			typeof(EtherealDragonsKeepMap),
			typeof(FirebreathAlligatorSwampMap),
			typeof(FireRoosterCavernMap),
			typeof(FlameBearerCaveMap),
			typeof(FlameWardenEttinFortressMap),
			typeof(FlareImpNestMap),
			typeof(FossilElementalCavernMap),
			typeof(FrostBearDomainMap),
			typeof(FrostbiteAlligatorSwampMap),
			typeof(FrostboundBehemothCaveMap),
			typeof(FrostDrakonKeepMap),
			typeof(FrostHensPerchMap),
			typeof(FrostSerpentLairMap),
			typeof(FrostWardenEttinStrongholdMap),
			typeof(FrostyFerretsBurrowMap),
			typeof(GaleWispsDomainMap),
			typeof(GiantTrapdoorSpiderLairMap),
			typeof(GiantWolfSpiderNestMap),
			typeof(GlimmeringFerretBurrowMap),
			typeof(GoldenOrbWeaverCavernMap),
			typeof(GoliathBirdeaterJungleMap),
			typeof(GorgonVipersLairMap),
			typeof(GraniteColossusCavernMap),
			typeof(GrimoriesTomeMap),
			typeof(GrotesqueOfRouensCryptMap),
			typeof(GrymalkinTheWatchersDomainMap),
			typeof(GuernseyGuardianKeepMap),
			typeof(HarmonyFerretGroveMap),
			typeof(HellfireJuggernautForgeMap),
			typeof(HerefordWarlockTowerMap),
			typeof(HighlandBullHerdMap),
			typeof(HuntsmanSpidersLairMap),
			typeof(IceCrabCavernMap),
			typeof(IllusionarySwampMap),
			typeof(IllusionHensParadiseMap),
			typeof(IllusionistEttinsDomainMap),
			typeof(InfernalDukesCitadelMap),
			typeof(InfernalIncineratorForgeMap),
			typeof(InfernoDrakonsRoostMap),
			typeof(InfernoPythonPitMap),
			typeof(InfernoWardensFortressMap),
			typeof(IshKarTheForgottenLairMap),
			typeof(JerseyEnchantressCovenMap),
			typeof(LairOfTheShadowogresDomainMap),
			typeof(LairOfTheShadowprowlersHuntMap),
			typeof(LairOfTheShadowsludgesSwampMap),
			typeof(LairOfTheShadowtoadsBogMap),
			typeof(LairOfTheSifakawarriorsJungleMap),
			typeof(LairOfTheSmallpoxRatLairMap),
			typeof(LairOfTheSombreroDeSolLlamaMap),
			typeof(LairOfTheSombreroLlamaMap),
			typeof(LairOfTheSothTheDeathKnightMap),
			typeof(LairOfTheSoulEaterLichMap),
			typeof(LairOfTheTahrsWildHordeMap),
			typeof(LairOfTheTalonmachinesForgeMap),
			typeof(LairOfTheTaurusearthbearsDominionMap),
			typeof(LairOfTheTaurusharpysSkiesMap),
			typeof(LairOfTheTempestsatyrsStormMap),
			typeof(LairOfTheVietnamesePigMap),
			typeof(LairOfTheVileToadMap),
			typeof(LairOfTheVirgoHarpyMap),
			typeof(LairOfTheVirgoPurityBearMap),
			typeof(LairOfTheVoidCatMap),
			typeof(LairOfTheVoidSlimeMap),
			typeof(LairOfTheVolcaniccHargerMap),
			typeof(LairOfTheVortexConstructMap),
			typeof(LairOfTheVortexWraithMap),
			typeof(LairOfTheWarthogWarriorMap),
			typeof(LairOfTheWraithlichCryptMap),
			typeof(LairOfTheYangstallionPlainsMap),
			typeof(LairOfTheYinsteedForestMap),
			typeof(LavaCrabCavernMap),
			typeof(LavaFiendFortressMap),
			typeof(LeafBearGroveMap),
			typeof(LeprechaunsLairMap),
			typeof(LightBearersSanctuaryMap),
			typeof(MagmaGolemForgeMap),
			typeof(MagneticCrabCavernMap),
			typeof(MaineCoonTitansRoostMap),
			typeof(MilkingDemonStablesMap),
			typeof(RhythmicSatyrsGladeMap),
			typeof(RockSquirrelCavernMap),
			typeof(SableAntelopeSavannaMap),
			typeof(SagittariusArcherBearForestMap),
			typeof(SagittariusHarpysPerchMap),
			typeof(SandGolemsTombMap),
			typeof(ScorpioHarpysLairMap),
			typeof(ScorpionSpidersHollowMap),
			typeof(ScorpioVenomBearsDenMap),
			typeof(SetiTheAvengersCryptMap),
			typeof(ShadowbladeAssassinsLairMap),
			typeof(ShadowGolemsAbyssMap),
			typeof(ShadowLichsNecropolisMap),
			typeof(ShadowMuntjacsDomainMap),
			typeof(SpectralAutomatonForgeMap),
			typeof(SpectralToadSwampMap),
			typeof(SpectralWardenCryptMap),
			typeof(SpiderlingMinionBroodmotherMap),
			typeof(SpiderMonkeyJungleMap),
			typeof(StarbornPredatorNestMap),
			typeof(SteamLeviathanAbyssMap),
			typeof(StoneGolemCavernMap),
			typeof(StoneSteedStablesMap),
			typeof(StormBoneFortressMap),
			typeof(SynthroidPrimeFactoryMap),
			typeof(SzassTamsNecropolisMap),
			typeof(TacoLlamaFestivalMap),
			typeof(TacticalEnforcerOperationsMap),
			typeof(TaffyTitansArenaMap),
			typeof(TequilaLlamaTavernMap),
			typeof(TheForestTempestMap),
			typeof(TheStormOfDeathMap),
			typeof(TheTempestsFuryMap),
			typeof(TheTempestsWrathMap),
			typeof(TheWrathOfTheThunderKingMap),
			typeof(ThutmoseTheConquerorsTombMap),
			typeof(TidalMaresDeepMap),
			typeof(ToxicLichsLairMap),
			typeof(ToxicOgresStrongholdMap),
			typeof(ToxicSludgeSwampMap),
			typeof(TrapdoorSpiderNestMap),
			typeof(TsunamiTitansDeepMap),
			typeof(TutankhamunTheCursedTombMap),
			typeof(TyphusRatInfestationMap),
			typeof(VampiricBladesLairMap),
			typeof(VecnasSanctumMap),
			typeof(VenomousRoesMarshMap),
			typeof(VenomousToadsSwampMap),
			typeof(VenomousWolfsLairMap),
			typeof(WhisperingPookaGroveMap),
			typeof(WickedSatyrsForestMap),
			typeof(WoodGolemsHollowMap),
			typeof(WoodlandChargerDomainMap),
			typeof(WoodlandSpiritHorseMeadowMap),
			typeof(MoltenGolemCavernMap),
			typeof(MordrakesManorMap),
			typeof(MudGolemSwampMap),
			typeof(MysticFerretSanctuaryMap),
			typeof(MysticFowlSanctuaryMap),
			typeof(MysticWispRealmMap),
			typeof(NatureDragonsLairMap),
			typeof(NecroEttinCryptMap),
			typeof(NecroRoosterTombMap),
			typeof(NightshadeBrambleGroveMap),
			typeof(NymphsSanctuaryMap),
			typeof(NyxRithRuinsMap),
			typeof(PersianShadeTombMap),
			typeof(PhantomVinesOvergrowthMap),
			typeof(PoisonousCrabCoveMap),
			typeof(PoisonPulletFarmMap),
			typeof(PucksMischiefMap),
			typeof(PuffyFerretHollowMap),
			typeof(PurseSpiderNestMap),
			typeof(PyroclasticGolemForgeMap),
			typeof(QuakeBringerCavernMap),
			typeof(QuorZaelsDomainMap),
			typeof(RagdollGuardianCitadelMap),
			typeof(RagingAlligatorSwampMap),
			typeof(RathzorTheShatteredsLairMap),
			typeof(RiptideCrabCoveMap),
			typeof(RockBearCavernMap),
			typeof(SahiwalShamansGroveMap),
			typeof(SandstormElementalDesertMap),
			typeof(ScorpionSpiderPitMap),
			typeof(ScottishFoldSentinelDenMap),
			typeof(SelkieCavernMap),
			typeof(ShadowAlligatorSwampMap),
			typeof(ShadowAnacondaJungleMap),
			typeof(ShadowBearsLairMap),
			typeof(ShadowChicksNestMap),
			typeof(ShadowCrabsTidepoolMap),
			typeof(ShadowDragonsRoostMap),
			typeof(ShadowDriftersMistsMap),
			typeof(SiameseIllusionistChamberMap),
			typeof(SiberianFrostclawsDomainMap),
			typeof(SidheFaeRealmMap),
			typeof(SinisterRootHollowMap),
			typeof(SkeletonEttinStrongholdMap),
			typeof(SkySeraphsAerieMap),
			typeof(SolarElementalSummitMap),
			typeof(SparkFerretWildsMap),
			typeof(SphinxCatsRiddleMap),
			typeof(SpiderlingOverlordBroodmotherMap),
			typeof(StarryFerretsCelestialRealmMap),
			typeof(SteelBearDenMap),
			typeof(StoneGuardianFortressMap),
			typeof(StoneRoosterCryptMap),
			typeof(StormAlligatorSwampMap),
			typeof(StormCrabsLairMap),
			typeof(StormDaemonsDomainMap),
			typeof(StormDragonsPeakMap),
			typeof(StormHeraldsSanctuaryMap),
			typeof(StrixsPerchMap),
			typeof(SunbeamFerretHollowMap),
			typeof(TarantulaWarriorLairMap),
			typeof(TarantulaWorriorCavernMap),
			typeof(TempestSpiritDomainMap),
			typeof(TempestWyrmSpireMap),
			typeof(TerraWispGroveMap),
			typeof(ThornedHorrorSwampMap),
			typeof(ThulGorTheForsakenLairMap),
			typeof(ThunderBearHighlandsMap),
			typeof(ThunderbirdMountainMap),
			typeof(ThunderSerpentCavernMap),
			typeof(TidalEttinMarshMap),
			typeof(TitanBoaSwampMap),
			typeof(ToxicAlligatorSwampsMap),
			typeof(ToxicReaverNecropolisMap),
			typeof(TurkishAngoraEnchantersDomainMap),
			typeof(TwinTerrorEttinsFortressMap),
			typeof(UruKothsLairMap),
			typeof(VengefulPitVipersPitMap),
			typeof(VenomBearsDenMap),
			typeof(VenomousAlligatorSwampMap),
			typeof(VenomousDragonLairMap),
			typeof(VenomousEttinCaveMap),
			typeof(VenomousIvyGroveMap),
			typeof(VespaHiveMap),
			typeof(VileBlossomGroveMap),
			typeof(VitrailTheMosaicMap),
			typeof(VoidStalkerAbyssMap),
			typeof(VolcanicTitanCraterMap),
			typeof(VorgathTheDestroyerMap),
			typeof(VortexCrabReefMap),
			typeof(VortexGuardianKeepMap),
			typeof(WhirlwindFiendAbyssMap),
			typeof(WillothewispEnclaveMap),
			typeof(WindBearGroveMap),
			typeof(WindChickenNestMap),
			typeof(XalrathCultMap),
			typeof(ZebuZealotRuinsMap),
			typeof(ZelvrakStrongholdMap),
			typeof(ZephyrWardensDomainMap),
			typeof(EasternMagicMap),
			typeof(FarEasternMagicMap),
			typeof(FeluccaMagicMap),
			typeof(IlshenarMagicMap),
			typeof(NorthernMagicMap),
			typeof(SouthernMagicMap),
			typeof(TerMurMagicMap),
			typeof(TokunoMagicMap),
			typeof(TrammelMagicMap),
			typeof(WesternMagicMap),
			typeof(AirClanNinjaCampMap),
			typeof(AirClanSamuraiDojoMap),
			typeof(AlienWarriorNestMap),
			typeof(AppleGroveElementalMap),
			typeof(AssassinGuildHallMap),
			typeof(AstralTravelerRealmMap),
			typeof(AvatarOfElementsShrineMap),
			typeof(BaroqueBarbarianCampMap),
			typeof(BeetleJuiceSummoningCircleMap),
			typeof(BiomancersGroveMap),
			typeof(BluesSingingGorgonAmphitheaterMap),
			typeof(BMovieBeastmasterArenaMap),
			typeof(BountyHunterOutpostMap),
			typeof(CabaretKrakenStageMap),
			typeof(CannibalTribeCampMap),
			typeof(CavemanScientistExperimentSiteMap),
			typeof(CelestialSamuraiDojoMap),
			typeof(ChrisRobertsGalacticArenaMap),
			typeof(CorporateExecutiveTowerMap),
			typeof(CountryCowgirlCyclopsRanchMap),
			typeof(CyberpunkNexusMap),
			typeof(DarkElfCitadelMap),
			typeof(DinoRiderExpeditionMap),
			typeof(DiscoDruidFestivalMap),
			typeof(HarvestFestivalFrenzyMap),
			typeof(LongbowSniperOutpostMap),
			typeof(LuchadorTrainingGroundsMap),
			typeof(MagiciansArcaneHallMap),
			typeof(MartialMonkDojoMap),
			typeof(MasterFlutistsConcertMap),
			typeof(MechanicsWorkshopMap),
			typeof(MusclePitMap),
			typeof(NecromancersHollowMap),
			typeof(NetCasterReefMap),
			typeof(NinjaShadowHideoutMap),
			typeof(NymphSingerGladeMap),
			typeof(OraclesSanctumMap),
			typeof(PastryChefsBakeryMap),
			typeof(PatchworkMonsterLaboratoryMap),
			typeof(PathologistsLairMap),
			typeof(PhantomAssassinsHideoutMap),
			typeof(PickpocketsDenMap),
			typeof(PocketPickersRefugeMap),
			typeof(ProtestersCampMap),
			typeof(QiGongHealerSanctuaryMap),
			typeof(QuantumPhysicistResearchFacilityMap),
			typeof(RamRiderOutpostMap),
			typeof(RapierDuelistArenaMap),
			typeof(RelativistObservatoryMap),
			typeof(RelicHunterExpeditionMap),
			typeof(RuneCasterSanctumMap),
			typeof(RuneKeeperChamberMap),
			typeof(SaboteurHideoutMap),
			typeof(SabreFighterArenaMap),
			typeof(SafecrackersDenMap),
			typeof(SamuraiMastersDojoMap),
			typeof(SatyrPipersGlenMap),
			typeof(SawmillWorkersDomainMap),
			typeof(ScoutArchersRefugeMap),
			typeof(ScoutLeaderEncampmentMap),
			typeof(ScrollMagesTowerMap),
			typeof(SerpentHandlerPitMap),
			typeof(ShadowLordsDomainMap),
			typeof(ShadowLurkerCavernMap),
			typeof(ShadowPriestLairMap),
			typeof(SheepdogHandlersPenMap),
			typeof(ShieldBearersBastionMap),
			typeof(ShieldMaidensCitadelMap),
			typeof(SlyStorytellersTheatreMap),
			typeof(SousChefsKitchenMap),
			typeof(SpearFishersCoveMap),
			typeof(SpearSentryKeepMap),
			typeof(SpellbreakersTrialMap),
			typeof(SpiritMediumsSeanceMap),
			typeof(SpyHideoutMap),
			typeof(StarReaderObservatoryMap),
			typeof(StormConjurerSummitMap),
			typeof(StrategistsWarTableMap),
			typeof(SumoWrestlerArenaMap),
			typeof(SwordDefenderCitadelMap),
			typeof(TaekwondoDojoMap),
			typeof(TerrainScoutEncampmentMap),
			typeof(ThievesGuildHideoutMap),
			typeof(ToxicLaboratoryMap),
			typeof(ToxicologistsKitchenMap),
			typeof(TrapEngineerWorkshopMap),
			typeof(TrapMakersWorkshopMap),
			typeof(TrapMastersWorkshopMap),
			typeof(TrapSettersHideoutMap),
			typeof(TreeFellersGroveMap),
			typeof(TrickShotArtistsArenaMap),
			typeof(UrbanTrackersOutpostMap),
			typeof(VenomousAssassinsLairMap),
			typeof(ViolinistsOrchestraMap),
			typeof(WardCastersKeepMap),
			typeof(WaterAlchemistsLaboratoryMap),
			typeof(WeaponEnchantersSanctumMap),
			typeof(WildWestOutpostMap),
			typeof(WoolWeaversLoomMap),
			typeof(ZenMonksSanctuaryMap),
			typeof(AnvilHurlerForgeMap),
			typeof(AquaticTamerLagoonMap),
			typeof(ArcaneScribeEnclaveMap),
			typeof(ArcticNaturalistDenMap),
			typeof(ArmorCurerLaboratoryMap),
			typeof(ArrowFletchersRoostMap),
			typeof(AsceticHermitsRefugeMap),
			typeof(AstrologersObservatoryMap),
			typeof(BanneretsBastionMap),
			typeof(BattleDressmakersWorkshopMap),
			typeof(BattlefieldHealersSanctuaryMap),
			typeof(BattleHerbalistGroveMap),
			typeof(BattleStormCallersEyeMap),
			typeof(BattleWeaverLoomMap),
			typeof(BeastmastersDomainMap),
			typeof(BigCatTamerJungleMap),
			typeof(BiologistsLaboratoryMap),
			typeof(BirdTrainersAviaryMap),
			typeof(BoneShielderCryptMap),
			typeof(BoomerangThrowerCampMap),
			typeof(CabinetMakersWorkshopMap),
			typeof(CarversAtelier),
			typeof(ChemistsLaboratory),
			typeof(ChoirSingersHallMap),
			typeof(ClockworkEngineersWorkshopMap),
			typeof(ClueSeekersPuzzleGroundsMap),
			typeof(CombatMedicsSanctuaryMap),
			typeof(CombatNursesRecoveryWardMap),
			typeof(ConArtistsDenMap),
			typeof(ContortionistsCircusMap),
			typeof(CrimeSceneTechInvestigationMap),
			typeof(CrossbowMarksmanOutpostMap),
			typeof(CryingOrphanRefugeMap),
			typeof(CryptologistsChamberMap),
			typeof(DarkSorcererDomainMap),
			typeof(DeathCultistCryptMap),
			typeof(DecoyDeployerOutpostMap),
			typeof(DeepMinerExcavationMap),
			typeof(DemolitionExpertQuarryMap),
			typeof(DesertNaturalistOasisMap),
			typeof(DesertTrackersOasisMap),
			typeof(DiplomatsParleyMap),
			typeof(DisguiseMastersHavenMap),
			typeof(DivinersPeakMap),
			typeof(DrumBoysSpectacleMap),
			typeof(DrummersArenaMap),
			typeof(DualWielderDojoMap),
			typeof(EarthAlchemistsLairMap),
			typeof(ElectriciansWorkshopMap),
			typeof(ElementalWizardsKeepMap),
			typeof(EnchantersLabyrinthMap),
			typeof(EpeeSpecialistArenaMap),
			typeof(EscapeArtistHideoutMap),
			typeof(EvidenceAnalystsBureauMap),
			typeof(EvilMapMakersWorkshopMap),
			typeof(ExplorersExpeditionMap),
			typeof(ExplosiveDemolitionistsFoundryMap),
			typeof(FeastMastersBanquetMap),
			typeof(FencingMastersArenaMap),
			typeof(FieldCommanderOutpostMap),
			typeof(FieldMedicCampMap),
			typeof(FireAlchemistLaboratoryMap),
			typeof(FireMageConclaveMap),
			typeof(FirestarterPyreMap),
			typeof(FlameWelderForgeMap),
			typeof(ForagersHollowMap),
			typeof(ForensicAnalystsLairMap),
			typeof(ForestMinstrelsGlenMap),
			typeof(ForestScoutOutpostMap),
			typeof(ForestTrackerCampMap),
			typeof(GemCutterCavernMap),
			typeof(GhostScoutOutpostMap),
			typeof(GhostWarriorBattlefieldMap),
			typeof(GourmetChefKitchenMap),
			typeof(GraveDiggerCryptMap),
			typeof(GrecoRomanArenaMap),
			typeof(GrillMasterPitMap),
			typeof(HammerGuardArmoryMap),
			typeof(HarpistsGroveMap),
			typeof(HerbalistPoisonerGroveMap),
			typeof(IceSorcererCitadelMap),
			typeof(IllusionistsLabyrinthMap),
			typeof(InfiltratorsHideoutMap),
			typeof(InvisibleSaboteursWorkshopMap),
			typeof(IronSmithForgeMap),
			typeof(JavelinAthleteArenaMap),
			typeof(JoinerWorkshopMap),
			typeof(JungleNaturalistGroveMap),
			typeof(KarateExpertDojoMap),
			typeof(KatanaDuelistDojoMap),
			typeof(KnifeThrowersArenaMap),
			typeof(KnightOfJusticeCitadelMap),
			typeof(KnightOfMercyChapelMap),
			typeof(KnightOfValorFortressMap),
			typeof(KunoichiHideoutMap),
			typeof(LibrarianCustodiansArchiveMap),
			typeof(LightningBearersStormNexusMap),
			typeof(LocksmithsWorkshopMap),
			typeof(LogiciansPuzzleHallMap),
			typeof(MeraktusTheTormentedMap),
			typeof(AbyssalBouncersArenaMap),
			typeof(AbyssalPanthersProwlMap),
			typeof(AbyssalTidesSurgeMap),
			typeof(AcereraksNecropolisMap),
			typeof(AcidicSlimesLairMap),
			typeof(AegisConstructForgeMap),
			typeof(AkhenatensHereticShrineMap),
			typeof(AkhenatensTombMap),
			typeof(AlbertsSquirrelGladeMap),
			typeof(AlphaBaboonTroopMap),
			typeof(AncientWolfDenMap),
			typeof(AnthraxRatNestMap),
			typeof(ArbiterDroneHiveMap),
			typeof(ArcaneSatyrGladeMap),
			typeof(ArcaneSentinelBastionMap),
			typeof(AriesHarpyAerieMap),
			typeof(AriesRamBearPlateauMap),
			typeof(AzalinRexsCryptMap),
			typeof(AzureMiragesRealmMap),
			typeof(AzureMooseGroveMap),
			typeof(BabirusaBeastsBogMap),
			typeof(BansheeCrabsNestMap),
			typeof(BansheesWailMap),
			typeof(BeardedGoatPasturesMap),
			typeof(BeldingsGroundSquirrelBurrowMap),
			typeof(BengalStormsJungleMap),
			typeof(BisonBrutePlateauMap),
			typeof(BlackDeathRatSewersMap),
			typeof(BlackWidowQueenLairMap),
			typeof(BlackWidowsLairMap),
			typeof(BlightDemonFissureMap),
			typeof(BlightedToadSwampMap),
			typeof(BloodDragonRoostMap),
			typeof(BloodLichsCryptMap),
			typeof(BloodSerpentsNestMap),
			typeof(BloodthirstyVinesThicketMap),
			typeof(BonecrusherOgresDomainMap),
			typeof(BoneGolemsWorkshopMap),
			typeof(BorneoPigstyMap),
			typeof(BubblegumBlasterFactoryMap),
			typeof(BushPigEncampmentMap),
			typeof(CactusLlamaGroveMap),
			typeof(CancerHarpyAerieMap),
			typeof(CancerShellBearDenMap),
			typeof(CandyCornCreepsLairMap),
			typeof(CapricornHarpysNestMap),
			typeof(CapricornMountainBearsDomainMap),
			typeof(CapuchinTrickstersPlaygroundMap),
			typeof(CaramelConjurersWorkshopMap),
			typeof(CelestialHorrorRealmMap),
			typeof(CelestialPythonDomainMap),
			typeof(CelestialSatyrGroveMap),
			typeof(CelestialWolfDenMap),
			typeof(ChamoisHillMap),
			typeof(CholeraRatInfestationMap),
			typeof(ChromaticOgreClanMap),
			typeof(CleopatraTheEnigmaticMap),
			typeof(CliffGoatDominionMap),
			typeof(CoralSentinelsReefMap),
			typeof(CorrosiveToadSwampMap),
			typeof(CosmicBouncerArenaMap),
			typeof(CosmicStalkerVoidMap),
			typeof(CrimsonMuleValleyMap),
			typeof(CrystalGolemForgeMap),
			typeof(CrystalOozeCavernMap),
			typeof(CursedToadSwampMap),
			typeof(CursedWhiteTailForestMap),
			typeof(CursedWolfsDenMap),
			typeof(DallSheepHighlandMap),
			typeof(DeathRatCavernMap),
			typeof(DiaDeLosMuertosLlamaMap),
			typeof(DisplacerBeastDomainMap),
			typeof(DomesticSwineRetreatMap),
			typeof(DouglasSquirrelForestMap),
			typeof(DreadnaughtFortressMap),
			typeof(EarthquakeWolfCavernMap),
			typeof(EasternGraySquirrelGroveMap),
			typeof(EclipseReindeerGladeMap),
			typeof(EldritchHarbingerRealmMap),
			typeof(EtherealPanthrasLairMap),
			typeof(FaintingGoatsPastureMap),
			typeof(FeverRatsDenMap),
			typeof(FiestaLlamasCelebrationMap),
			typeof(FlameborneKnightsFortressMap),
			typeof(FlamebringerOgreLairMap),
			typeof(LairOfTheChaosHareMap),
			typeof(LairOfTheCharroLlamaMap),
			typeof(LairOfTheCheeseGolemMap),
			typeof(LairOfTheChimpanzeeBerserkerMap),
			typeof(LairOfTheChocolateTruffleMap),
			typeof(LairOfTheEldritchHaresWarrenMap),
			typeof(LairOfTheEldritchToadsSwampMap),
			typeof(LairOfTheElectricSlimesLabyrinthMap),
			typeof(LairOfTheElectroWraithsRealmMap),
			typeof(LairOfTheElMariachiLlamasFiestaMap),
			typeof(LairOfTheEmberAxisForgeMap),
			typeof(LairOfTheEmberWolfDenMap),
			typeof(LairOfTheEmperorCobraTempleMap),
			typeof(LairOfTheEnigmaticSatyrGroveMap),
			typeof(LairOfTheEnigmaticSkipperReefMap),	
			typeof(AlchemistsWax),
			typeof(ArcaneHourglass),
			typeof(BlacksmithingCatalyst),
			typeof(CartographersPen),
			typeof(CartographersPin),
			typeof(ChaosGlyph),
			typeof(CompassRose),
			typeof(DiamondLootScroll),
			typeof(EasternBrand),
			typeof(ErasureScroll),
			typeof(ExaltedOrb),
			typeof(FarEasternBrand),
			typeof(FeluccanBrand),
			typeof(FeluccaPortalPrism),
			typeof(FirestormGlyph),
			typeof(FletchingCatalyst),
			typeof(GoldenSeal),
			typeof(GlyphOfBounty),
			typeof(IlshenarBrand),
			typeof(InkOfRegression),
			typeof(MalasBrand),
			typeof(MonsterMixMedallion),
			typeof(NorthernBrand),
			typeof(OrbOfAnnulment),
			typeof(PlanarCompass),
			typeof(RadiusRune),
			typeof(ScrollOfIntensification),
			typeof(SingularityRune),
			typeof(SosariaBrand),
			typeof(SouthernBrand),
			typeof(StabilizerRune),
			typeof(SurveyorsCompass),
			typeof(TailoringCatalyst),
			typeof(TemporalSundial),
			typeof(TerMurBrand),
			typeof(TimeTurnToken),
			typeof(TinkeringCatalyst),
			typeof(TokunoBrand),
			typeof(TrammelBrand),
			typeof(WesternBrand),			
			typeof(WWIIValorChest),
			typeof(Zulfiqar)
			// Add more item types as needed
		};

		
		public static bool NewSystem { get { return Core.EJ; } }

        /// <summary>
        /// This is called from BaseCreature. Instead of editing EVERY creature that drops a map, we'll simply convert it here.
        /// </summary>
        /// <param name="level"></param>
        public static int ConvertLevel(int level)
        {
            if (!NewSystem || level == -1)
                return level;

            switch (level)
            {
                default: return (int)TreasureLevel.Stash;
                case 2: 
                case 3: return (int)TreasureLevel.Supply;
                case 4: 
                case 5: return (int)TreasureLevel.Cache;
                case 6: return (int)TreasureLevel.Hoard;
                case 7: return (int)TreasureLevel.Trove;
            }
        }

        public static TreasureFacet GetFacet(IEntity e)
        {
            return GetFacet(e.Location, e.Map);
        }

        public static int PackageLocalization(TreasurePackage package)
        {
            switch (package)
            {
                case TreasurePackage.Artisan: return 1158989;
                case TreasurePackage.Assassin: return 1158987;
                case TreasurePackage.Mage: return 1158986;
                case TreasurePackage.Ranger: return 1158990;
                case TreasurePackage.Warrior: return 1158988;
            }

            return 0;
        }

        public static TreasureFacet GetFacet(IPoint2D p, Map map)
        {
            if (map == Map.TerMur)
            {
                if (SpellHelper.IsEodon(map, new Point3D(p.X, p.Y, 0)))
                {
                    return TreasureFacet.Eodon;
                }

                return TreasureFacet.TerMur;
            }

            if (map == Map.Felucca)
            {
                return TreasureFacet.Felucca;
            }

            if (map == Map.Malas)
            {
                return TreasureFacet.Malas;
            }

            if (map == Map.Ilshenar)
            {
                return TreasureFacet.Ilshenar;
            }

            if (map == Map.Tokuno)
            {
                return TreasureFacet.Tokuno;
            }

            return TreasureFacet.Trammel;
        }

        public static IEnumerable<Type> GetRandomEquipment(TreasureLevel level, TreasurePackage package, TreasureFacet facet, int amount)
        {
            Type[] weapons = GetWeaponList(level, package, facet);
            Type[] armor = GetArmorList(level, package, facet);
            Type[] jewels = GetJewelList(level, package, facet);
            Type[] list;

            for (int i = 0; i < amount; i++)
            {
                switch (Utility.Random(5))
                {
                    default:
                    case 0: list = weapons; break;
                    case 1:
                    case 2: list = armor; break;
                    case 3:
                    case 4: list = jewels; break;
                }

                yield return list[Utility.Random(list.Length)];
            }
        }

        public static Type[] GetWeaponList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            switch (facet)
            {
                case TreasureFacet.Trammel:
                case TreasureFacet.Felucca: list = _WeaponTable[(int)package][0]; break;
                case TreasureFacet.Ilshenar: list = _WeaponTable[(int)package][1]; break;
                case TreasureFacet.Malas: list = _WeaponTable[(int)package][2]; break;
                case TreasureFacet.Tokuno: list = _WeaponTable[(int)package][3]; break;
                case TreasureFacet.TerMur: list = _WeaponTable[(int)package][4]; break;
                case TreasureFacet.Eodon: list = _WeaponTable[(int)package][5]; break;
            }

            // tram/fel lists are always default
            if (list == null || list.Length == 0)
            {
                list = _WeaponTable[(int)package][0];
            }

            return list;
        }

        public static Type[] GetArmorList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            switch (facet)
            {
                case TreasureFacet.Trammel:
                case TreasureFacet.Felucca: list = _ArmorTable[(int)package][0]; break;
                case TreasureFacet.Ilshenar: list = _ArmorTable[(int)package][1]; break;
                case TreasureFacet.Malas: list = _ArmorTable[(int)package][2]; break;
                case TreasureFacet.Tokuno: list = _ArmorTable[(int)package][3]; break;
                case TreasureFacet.TerMur: list = _ArmorTable[(int)package][4]; break;
                case TreasureFacet.Eodon: list = _ArmorTable[(int)package][5]; break;
            }

            // tram/fel lists are always default
            if (list == null || list.Length == 0)
            {
                list = _ArmorTable[(int)package][0];
            }

            return list;
        }

        public static Type[] GetJewelList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (facet == TreasureFacet.TerMur)
            {
                return _JewelTable[1];
            }
            else
            {
                return _JewelTable[0];
            }
        }

        public static SkillName[] GetTranscendenceList(TreasureLevel level, TreasurePackage package)
        {
            if (level == TreasureLevel.Supply || level == TreasureLevel.Cache)
            {
                return null;
            }

            return _TranscendenceTable[(int)package];
        }

        public static SkillName[] GetAlacrityList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (level == TreasureLevel.Stash || (facet == TreasureFacet.Felucca && level == TreasureLevel.Cache))
            {
                return null;
            }

            return _AlacrityTable[(int)package];
        }

        public static SkillName[] GetPowerScrollList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (facet != TreasureFacet.Felucca)
                return null;

            if (level >= TreasureLevel.Cache)
            {
                return _PowerscrollTable[(int)package];
            }

            return null;
        }

        public static Type[] GetCraftingMaterials(TreasureLevel level, TreasurePackage package, ChestQuality quality)
        {
            if (package == TreasurePackage.Artisan && level <= TreasureLevel.Supply && quality != ChestQuality.None)
            {
                return _MaterialTable[(int)quality - 1];
            }

            return null;
        }

        public static Type[] GetSpecialMaterials(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
            {
                return _SpecialMaterialTable[(int)facet];
            }

            return null;
        }

        public static Type[] GetDecorativeList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            if (level >= TreasureLevel.Cache)
            {
                list = _DecorativeTable[(int)package];

                if (facet == TreasureFacet.Malas)
                {
                    list.Concat(new Type[] { typeof(CoffinPiece) });
                }
            }
            else if (level == TreasureLevel.Supply)
            {
                list = _DecorativeMinorArtifacts;
            }

            return list;
        }

        public static Type[] GetReagentList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (level != TreasureLevel.Stash || package != TreasurePackage.Mage)
                return null;

            switch (facet)
            {
                case TreasureFacet.Felucca:
                case TreasureFacet.Trammel: return Loot.RegTypes;
                case TreasureFacet.Malas: return Loot.NecroRegTypes;
                case TreasureFacet.TerMur: return Loot.MysticRegTypes;
            }

            return null;
        }

        public static Recipe[] GetRecipeList(TreasureLevel level, TreasurePackage package)
        {
            if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
            {
                return Recipe.Recipes.Values.ToArray();
            }

            return null;
        }

        public static Type[] GetSpecialLootList(TreasureLevel level, TreasurePackage package)
        {
            if (level == TreasureLevel.Stash)
                return null;

            Type[] list;

            if (level == TreasureLevel.Supply)
            {
                list = _SpecialSupplyLoot[(int)package];
            }
            else
            {
                list = _SpecialCacheHordeAndTrove;
            }

            if (package > TreasurePackage.Artisan)
            {
                list.Concat(_FunctionalMinorArtifacts);
            }

            return list;
        }

        public static int GetGemCount(ChestQuality quality, TreasureLevel level)
        {
            var baseAmount = 0;

            switch(quality)
            {
                case ChestQuality.Rusty: baseAmount = 7; break;
                case ChestQuality.Standard: baseAmount = Utility.RandomBool() ? 7 : 9; break;
                case ChestQuality.Gold: baseAmount = Utility.RandomList(7, 9, 11); break;
            }

            return baseAmount + ((int)level * 5);
        }

        public static int GetGoldCount(TreasureLevel level)
        {
            switch(level)
            {
                default:
                case TreasureLevel.Stash: return Utility.RandomMinMax(10000, 40000);
                case TreasureLevel.Supply: return Utility.RandomMinMax(20000, 50000);
                case TreasureLevel.Cache: return Utility.RandomMinMax(30000, 60000);
                case TreasureLevel.Hoard: return Utility.RandomMinMax(40000, 70000);
                case TreasureLevel.Trove: return Utility.RandomMinMax(50000, 70000);
            }
        }

        public static int GetRefinementRolls(ChestQuality quality)
        {
            switch (quality)
            {
                default:
                case ChestQuality.Rusty: return 2;
                case ChestQuality.Standard: return 4;
                case ChestQuality.Gold: return 6;
            }
        }

        public static int GetResourceAmount(TreasureLevel level)
        {
            switch(level)
            {
                case TreasureLevel.Stash: return 50;
                case TreasureLevel.Supply: return 100;
            }

            return 0;
        }

        public static int GetRegAmount(ChestQuality quality)
        {
            switch(quality)
            {
                default:
                case ChestQuality.Rusty: return 20;
                case ChestQuality.Standard: return 40;
                case ChestQuality.Gold: return 60;
            }
        }

        public static int GetSpecialResourceAmount(ChestQuality quality)
        {
            switch (quality)
            {
                default:
                case ChestQuality.Rusty: return 1;
                case ChestQuality.Standard: return 2;
                case ChestQuality.Gold: return 3;
            }
        }

        public static int GetEquipmentAmount(Mobile from, TreasureLevel level, TreasurePackage package)
        {
            var amount = 0;

            switch (level)
            {
                default:
                case TreasureLevel.Stash: amount = 6; break;
                case TreasureLevel.Supply: amount = 8; break;
                case TreasureLevel.Cache: amount = package == TreasurePackage.Assassin ? 24 : 12; break;
                case TreasureLevel.Hoard: amount = 18; break;
                case TreasureLevel.Trove: amount = 36; break;
            }

            var p = Party.Get(from);

            if (p != null && p.Count > 1)
            {
                for (int i = 0; i < p.Count - 1; i++)
                {
                    if (Utility.RandomBool())
                    {
                        amount++;
                    }
                }
            }

            return amount;
        }

        public static void GetMinMaxBudget(TreasureLevel level, Item item, out int min, out int max)
        {
            var preArtifact = Imbuing.GetMaxWeight(item) + 100;
            min = max = 0;

            switch (level)
            {
                default:
                case TreasureLevel.Stash:
                case TreasureLevel.Supply: min = 250; max = preArtifact; break;
                case TreasureLevel.Cache:
                case TreasureLevel.Hoard:
                case TreasureLevel.Trove: min = 500; max = 1300; break;
            }
        }

        private static Type[][][] _WeaponTable = new Type[][][]
        {
            new Type[][] // Artisan
                {
                    new Type[] { typeof(HammerPick), typeof(SledgeHammerWeapon), typeof(SmithyHammer), typeof(WarAxe), typeof(WarHammer), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet), typeof(LargeBattleAxe), typeof(OrnateAxe), typeof(TwoHandedAxe), typeof(Pickaxe) }, // Trammel, Felucca
                    null, // Ilshenar
                    null, // Malas
                    null, // Tokuno
                    new Type[] { typeof(HammerPick), typeof(SledgeHammerWeapon), typeof(SmithyHammer), typeof(WarAxe), typeof(WarHammer), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet), typeof(LargeBattleAxe), typeof(OrnateAxe), typeof(TwoHandedAxe), typeof(Pickaxe), typeof(DualShortAxes) },  // TerMur
                    new Type[] {  }  // Eodon
                },
            new Type[][] // Assassin
                {
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass), typeof(ElvenMachete) },
                    null,
                    null,
                    null,
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass) },
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass), typeof(BladedWhip), typeof(BarbedWhip), typeof(SpikedWhip) },
                },
            new Type[][] // Mage
                {
                    new Type[] { typeof(BlackStaff), typeof(ShepherdsCrook), typeof(GnarledStaff), typeof(QuarterStaff) },
                    null,
                    null,
                    null,
                    null,
                    null,
                },
            new Type[][] // Ranger
                {
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife) },
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(SoulGlaive) },
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(ElvenCompositeLongbow) },
                    null,
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(GargishButcherKnife), typeof(Cyclone), typeof(SoulGlaive) },
                    null,
                },
            new Type[][] // Warrior
                {
                    new Type[] { typeof(Lance), typeof(Pike), typeof(Pitchfork), typeof(ShortSpear), typeof(WarFork), typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe), typeof(Bardiche), typeof(Broadsword), typeof(CrescentBlade), typeof(Halberd), typeof(Longsword), typeof(Scimitar), typeof(VikingSword) },
                    null,
                    null,
                    new Type[] { typeof(Lance), typeof(Pike), typeof(Pitchfork), typeof(ShortSpear), typeof(WarFork), typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe), typeof(Bardiche), typeof(Broadsword), typeof(CrescentBlade), typeof(Halberd), typeof(Longsword), typeof(Scimitar), typeof(VikingSword), typeof(Bokuto), typeof(Daisho) },
                    null,
                    null,
                },
        };

        private static Type[][][] _ArmorTable = new Type[][][]
        {
            new Type[][] // Artisan
                {
                    new Type[] { typeof(Bonnet), typeof(Cap), typeof(Circlet), typeof(ElvenGlasses), typeof(FeatheredHat), typeof(FlowerGarland), typeof(JesterHat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(WideBrimHat) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    null, // Tokuno
                    null, // TerMur
                    new Type[] { typeof(Bonnet), typeof(Cap), typeof(Circlet), typeof(ElvenGlasses), typeof(FeatheredHat), typeof(FlowerGarland), typeof(JesterHat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(WideBrimHat), typeof(ChefsToque) }, // Eodon
                },
            new Type[][] // Assassin
                {
                    new Type[] { typeof(ChainLegs), typeof(ChainCoif), typeof(ChainChest), typeof(RingmailLegs), typeof(RingmailGloves), typeof(RingmailChest), typeof(RingmailArms), typeof(Bandana) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    new Type[] { typeof(ChainLegs), typeof(ChainCoif), typeof(ChainChest), typeof(RingmailLegs), typeof(RingmailGloves), typeof(RingmailArms), typeof(RingmailArms), typeof(Bandana), typeof(LeatherSuneate), typeof(LeatherMempo), typeof(LeatherJingasa), typeof(LeatherHiroSode), typeof(LeatherHaidate), typeof(LeatherDo) }, // Tokuno
                    null, // TerMur
                    null, // Eodon
                },
            new Type[][] // Mage
                {
                    new Type[] { typeof(LeafGloves), typeof(LeafLegs), typeof(LeafTonlet), typeof(LeafGorget), typeof(LeafArms),typeof(LeafChest), typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // Trammel/Fel
                    null, // Ilshenar
                    new Type[] { typeof(LeafGloves), typeof(LeafLegs), typeof(LeafTonlet), typeof(LeafGorget), typeof(LeafArms),typeof(LeafChest), typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat), typeof(BoneLegs), typeof(BoneHelm), typeof(BoneGloves), typeof(BoneChest), typeof(BoneArms) }, // Malas
                    null, // Tokuno
                    new Type[] { typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // TerMur
                    new Type[] { typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // Eodon
                },
            new Type[][] // Ranger
                {
                    new Type[] { typeof(HidePants), typeof(HidePauldrons), typeof(HideGorget), typeof(HideFemaleChest), typeof(HideChest), typeof(HideGloves), typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(RavenHelm), typeof(VultureHelm), typeof(WingedHelm) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    new Type[] { typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms) }, // Tokuno
                    new Type[] { typeof(HidePants), typeof(HidePauldrons), typeof(HideGorget), typeof(HideFemaleChest), typeof(HideChest), typeof(HideGloves), typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(GargishLeatherKilt), typeof(GargishLeatherLegs), typeof(GargishLeatherArms), typeof(GargishLeatherChest) }, // TerMur
                    new Type[] { typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(TigerPeltSkirt), typeof(TigerPeltShorts), typeof(TigerPeltLegs), typeof(TigerPeltLongSkirt), typeof(TigerPeltHelm), typeof(TigerPeltChest), typeof(TigerPeltCollar), typeof(TigerPeltBustier), typeof(VultureHelm), typeof(TribalMask) }, // Eodon
                },
            new Type[][] // Warrior
                {
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield) }, // Trammel/Fel
                    null, // Ilshenar
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(DragonHelm), typeof(DragonGloves), typeof(DragonChest), typeof(DragonArms), typeof(DragonLegs) }, // Malas
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(PlateSuneate), typeof(PlateMempo), typeof(PlateHiroSode), typeof(PlateHatsuburi), typeof(PlateHaidate), typeof(PlateDo), typeof(PlateBattleKabuto), typeof(DecorativePlateKabuto), typeof(LightPlateJingasa), typeof(SmallPlateJingasa)  }, // Tokuno
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(GargishPlateArms), typeof(GargishPlateChest), typeof(GargishPlateKilt), typeof(GargishPlateLegs), typeof(GargishStoneKilt), typeof(GargishStoneLegs), typeof(GargishStoneArms), typeof(GargishStoneChest) }, // TerMur
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(DragonTurtleHideHelm), typeof(DragonTurtleHideLegs), typeof(DragonTurtleHideChest), typeof(DragonTurtleHideBustier), typeof(DragonTurtleHideArms) }, // Eodon
                }
        };

        public static Type[][] _MaterialTable = new Type[][]
        {
            new Type[] { typeof(SpinedLeather), typeof(OakBoard), typeof(AshBoard), typeof(DullCopperIngot), typeof(ShadowIronIngot), typeof(CopperIngot) },
            new Type[] { typeof(HornedLeather), typeof(YewBoard), typeof(HeartwoodBoard), typeof(BronzeIngot), typeof(GoldIngot), typeof(AgapiteIngot) },
            new Type[] { typeof(BarbedLeather), typeof(BloodwoodBoard), typeof(FrostwoodBoard), typeof(ValoriteIngot), typeof(VeriteIngot) }
        };

        public static Type[][] _JewelTable = new Type[][]
            {
                new Type[] { typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet) }, // standard
                new Type[] { typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet), typeof(GargishBracelet) }, // Ranger/TerMur
            };

        public static Type[][] _DecorativeTable = new Type[][]
            {
                new Type[] { typeof(SkullTiledFloorAddonDeed) },
                new Type[] { typeof(AncientWeapon3) },
                new Type[] { typeof(DecorativeHourglass) },
                new Type[] { typeof(AncientWeapon1), typeof(CreepingVine) },
                new Type[] { typeof(AncientWeapon2) },
            };

        public static Type[][] _SpecialMaterialTable = new Type[][]
            {
                null, // tram
                null, // fel
                null, // ilsh
                new Type[] { typeof(LuminescentFungi), typeof(BarkFragment), typeof(Blight), typeof(Corruption), typeof(Muculent), typeof(Putrefaction), typeof(Scourge), typeof(Taint)  }, // malas
                null, // tokuno
                TreasureMapChest.ImbuingIngreds, // ter
                null, // eodon
            };

        public static Type[][] _SpecialSupplyLoot = new Type[][]
            {
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(ShieldEngravingTool), null },
                new Type[] { typeof(ForgedPardon), typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(SkeletonKey), typeof(MasterSkeletonKey), typeof(SurgeShield) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(TastyTreat) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
            };

        public static Type[] _SpecialCacheHordeAndTrove = new Type[]
            {
                typeof(OctopusNecklace), typeof(SkullGnarledStaff), typeof(SkullLongsword)
            };

        public static Type[] _DecorativeMinorArtifacts = new Type[]
            {
                typeof(CandelabraOfSouls), typeof(GoldBricks), typeof(PhillipsWoodenSteed), typeof(AncientShipModelOfTheHMSCape), typeof(AdmiralsHeartyRum)
            };

        public static Type[] _FunctionalMinorArtifacts = new Type[]
            {
                typeof(ArcticDeathDealer), typeof(BlazeOfDeath), typeof(BurglarsBandana),
                typeof(CavortingClub), typeof(DreadPirateHat),
                typeof(EnchantedTitanLegBone), typeof(GwennosHarp), typeof(IolosLute),
                typeof(LunaLance), typeof(NightsKiss), typeof(NoxRangersHeavyCrossbow),
                typeof(PolarBearMask), typeof(VioletCourage), typeof(HeartOfTheLion),
                typeof(ColdBlood), typeof(AlchemistsBauble), typeof(CaptainQuacklebushsCutlass),
                typeof(ShieldOfInvulnerability),
            };

        public static SkillName[][] _TranscendenceTable = new SkillName[][]
            {
                new SkillName[] { SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking, SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring },
                new SkillName[] { SkillName.Anatomy, SkillName.DetectHidden, SkillName.Fencing, SkillName.Poisoning, SkillName.RemoveTrap, SkillName.Snooping, SkillName.Stealth },
                new SkillName[] { SkillName.Magery, SkillName.Meditation, SkillName.MagicResist, SkillName.Spellweaving },
                new SkillName[] { SkillName.Alchemy, SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery, },
                new SkillName[] { SkillName.Chivalry, SkillName.Focus, SkillName.Parry, SkillName.Swords, SkillName.Tactics, SkillName.Wrestling },
            };

        public static SkillName[][] _AlacrityTable = new SkillName[][]
           {
                new SkillName[] { SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking, SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring, SkillName.Lumberjacking },
                new SkillName[] { SkillName.DetectHidden, SkillName.Fencing, SkillName.Hiding, SkillName.Lockpicking, SkillName.Poisoning, SkillName.RemoveTrap, SkillName.Snooping, SkillName.Stealing, SkillName.Stealth },
                new SkillName[] { SkillName.Alchemy, SkillName.EvalInt, SkillName.Inscribe, SkillName.Magery, SkillName.Meditation, SkillName.Spellweaving, SkillName.SpiritSpeak },
                new SkillName[] { SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery, SkillName.Musicianship, SkillName.Peacemaking, SkillName.Provocation, SkillName.Tinkering, SkillName.Tracking, SkillName.Veterinary },
                new SkillName[] { SkillName.Chivalry, SkillName.Focus, SkillName.Macing, SkillName.Parry, SkillName.Swords, SkillName.Wrestling },
           };

        public static SkillName[][] _PowerscrollTable = new SkillName[][]
            {
                null,
                new SkillName[] { SkillName.Ninjitsu },
                new SkillName[] { SkillName.Magery, SkillName.Meditation, SkillName.Mysticism, SkillName.Spellweaving, SkillName.SpiritSpeak },
                new SkillName[] { SkillName.AnimalTaming, SkillName.Discordance, SkillName.Provocation, SkillName.Veterinary },
                new SkillName[] { SkillName.Bushido, SkillName.Chivalry, SkillName.Focus, SkillName.Healing, SkillName.Parry, SkillName.Swords, SkillName.Tactics },
            };

        public static void Fill(Mobile from, TreasureMapChest chest, TreasureMap tMap)
        {
            var level = tMap.TreasureLevel;
            var package = tMap.Package;
            var facet = tMap.TreasureFacet;
            var quality = chest.ChestQuality;

            chest.Movable = false;
            chest.Locked = true;

            chest.TrapType = TrapType.ExplosionTrap;

            switch ((int)level)
            {
                default:
                case 0:
                    chest.RequiredSkill = 5;
                    chest.TrapPower = 25;
                    chest.TrapLevel = 1;
                    break;
                case 1:
                    chest.RequiredSkill = 45;
                    chest.TrapPower = 75;
                    chest.TrapLevel = 3;
                    break;
                case 2:
                    chest.RequiredSkill = 75;
                    chest.TrapPower = 125;
                    chest.TrapLevel = 5;
                    break;
                case 3:
                    chest.RequiredSkill = 80;
                    chest.TrapPower = 150;
                    chest.TrapLevel = 6;
                    break;
                case 4:
                    chest.RequiredSkill = 80;
                    chest.TrapPower = 170;
                    chest.TrapLevel = 7;
                    break;
            }

            chest.LockLevel = chest.RequiredSkill - 10;
            chest.MaxLockLevel = chest.RequiredSkill + 40;

            #region Refinements
            if (level == TreasureLevel.Stash)
            {
                RefinementComponent.Roll(chest, GetRefinementRolls(quality), 0.9);
            }
            #endregion

            #region TMaps
            var dropMap = false;
            if (level < TreasureLevel.Trove && 0.1 > Utility.RandomDouble())
            {
                chest.DropItem(new TreasureMap(tMap.Level + 1, chest.Map));
                dropMap = true;
            }
            #endregion

            Type[] list = null;
            int amount = 0;
            double dropChance = 0.0;

            #region Regs
            list = GetReagentList(level, package, facet);

            if (list != null)
            {
                amount = GetRegAmount(quality);

                for (int i = 0; i < amount; i++)
                {
                    chest.DropItemStacked(Loot.Construct(list));
                }

                list = null;
            }
            #endregion

            #region Gems
            amount = GetGemCount(quality, level);

            if (amount > 0)
            {
                var bag = new BagOfGems();

                foreach (var gemType in Loot.GemTypes)
                {
                    var gem = Loot.Construct(gemType);
                    gem.Amount = amount;

                    bag.DropItem(gem);

                }

                var goldAmount = GetGoldCount(level);

                while (goldAmount > 0)
                {
                    if (goldAmount <= 20000)
                    {
                        bag.DropItem(new Gold(goldAmount));
                        goldAmount = 0;
                    }
                    else
                    {
                        bag.DropItem(new Gold(20000));
                        goldAmount -= 20000;
                    }
                }

                chest.DropItem(bag);
            }
            #endregion

            #region Crafting Resources
            // TODO: DO each drop, or do only 1 drop?
            list = GetCraftingMaterials(level, package, quality);

            if (list != null)
            {
                amount = GetResourceAmount(level);

                foreach (var type in list)
                {
                    var craft = Loot.Construct(type);
                    craft.Amount = amount;

                    chest.DropItem(craft);
                }

                list = null;
            }
            #endregion

            #region Special Resources
            // TODO: DO each drop, or do only 1 drop?
            list = GetSpecialMaterials(level, package, facet);

            if (list != null)
            {
                amount = GetSpecialResourceAmount(quality);

                foreach (var type in list)
                {
                    var specialCraft = Loot.Construct(type);
                    specialCraft.Amount = amount;

                    chest.DropItem(specialCraft);
                }

                list = null;
            }
            #endregion

            #region Special Scrolls
            amount = (int)level + 1;

            if (dropMap)
            {
                amount--;
            }

            if (amount > 0)
            {
                var transList = GetTranscendenceList(level, package);
                var alacList = GetAlacrityList(level, package, facet);
                var pscrollList = GetPowerScrollList(level, package, facet);

                var scrollList = new List<Tuple<int, SkillName>>();

                if (transList != null)
                {
                    foreach (var sk in transList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(1, sk));
                    }
                }

                if (alacList != null)
                {
                    foreach (var sk in alacList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(2, sk));
                    }
                }

                if (pscrollList != null)
                {
                    foreach (var sk in pscrollList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(3, sk));
                    }
                }

                if (scrollList.Count > 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        var random = scrollList[Utility.Random(scrollList.Count)];

                        switch (random.Item1)
                        {
                            case 1: chest.DropItem(new ScrollOfTranscendence(random.Item2, Utility.RandomMinMax(1.0, chest.Map == Map.Felucca ? 7.0 : 5.0) / 10)); break;
                            case 2: chest.DropItem(new ScrollOfAlacrity(random.Item2)); break;
                            case 3: chest.DropItem(new PowerScroll(random.Item2, 110.0)); break;
                        }
                    }
                }
            }
            #endregion

            #region Decorations
            switch (level)
            {
                case TreasureLevel.Stash: dropChance = 0.0; break;
                case TreasureLevel.Supply: dropChance = 0.2; break;
                case TreasureLevel.Cache: dropChance = 0.4; break;
                case TreasureLevel.Hoard: dropChance = 0.5; break;
                case TreasureLevel.Trove: dropChance = .75; break;
            }

            if (Utility.RandomDouble() < dropChance)
            {
                list = GetDecorativeList(level, package, facet);

                if (list != null)
                {
                    if (list.Length > 0)
                    {
                        var deco = Loot.Construct(list[Utility.Random(list.Length)]);

                        if (_DecorativeMinorArtifacts.Any(t => t == deco.GetType()))
                        {
                            Container pack = new Backpack();
                            pack.Hue = 1278;

                            pack.DropItem(deco);
                            chest.DropItem(pack);
                        }
                        else
                        {
                            chest.DropItem(deco);
                        }
                    }

                    list = null;
                }
            }

            switch (level)
            {
                case TreasureLevel.Stash: dropChance = 0.0; break;
                case TreasureLevel.Supply: dropChance = 0.10; break;
                case TreasureLevel.Cache: dropChance = 0.20; break;
                case TreasureLevel.Hoard: dropChance = 0.50; break;
                case TreasureLevel.Trove: dropChance = .75; break;
            }

            if (Utility.RandomDouble() < dropChance)
            {
                list = GetSpecialLootList(level, package);

                if (list != null)
                {
                    if (list.Length > 0)
                    {
                        var type = MutateType(list[Utility.Random(list.Length)], facet);
                        Item deco;

                        if (type == null)
                        {
                            deco = TreasureMapChest.GetRandomRecipe();
                        }
                        else
                        {
                            deco = Loot.Construct(type);
                        }

                        if (deco is SkullGnarledStaff || deco is SkullLongsword)
                        {
                            if (package == TreasurePackage.Artisan)
                            {
                                ((IQuality)deco).Quality = ItemQuality.Exceptional;
                            }
                            else
                            {
                                int min, max;
                                GetMinMaxBudget(level, deco, out min, out max);
                                RunicReforging.GenerateRandomItem(deco, from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck, min, max, chest.Map);
                            }
                        }

                        if (_FunctionalMinorArtifacts.Any(t => t == type))
                        {
                            Container pack = new Backpack();
                            pack.Hue = 1278;

                            pack.DropItem(deco);
                            chest.DropItem(pack);
                        }
                        else
                        {
                            chest.DropItem(deco);
                        }
                    }

                    list = null;
                }
            }
            #endregion

            #region Magic Equipment
            amount = GetEquipmentAmount(from, level, package);

            foreach(var type in GetRandomEquipment(level, package, facet, amount))
            {
                var item = Loot.Construct(type);
                int min, max;
                GetMinMaxBudget(level, item, out min, out max);

                if (item != null)
                {
                    RunicReforging.GenerateRandomItem(item, from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck, min, max, chest.Map);
                    chest.DropItem(item);
                }
            }

            list = null;
            #endregion
			#region New Items
			int newItemAmount = Utility.RandomMinMax(1, 3);

			for (int i = 0; i < newItemAmount; i++)
			{
				var newItemType = _NewItemsList[Utility.Random(_NewItemsList.Length)];
				var newItem = Loot.Construct(newItemType);

				if (newItem != null)
				{
					chest.DropItem(newItem);
				}
			}
			#endregion
        }

        private static Type MutateType(Type type, TreasureFacet facet)
        {
            if (type == typeof(SkullGnarledStaff))
            {
                type = typeof(GargishSkullGnarledStaff);
            }
            else if (type == typeof(SkullLongsword))
            {
                type = typeof(GargishSkullLongsword);
            }

            return type;
        }
    }
}

