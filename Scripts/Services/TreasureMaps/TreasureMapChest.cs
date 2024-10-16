using System;
using System.Collections.Generic;
using System.Linq;

using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class TreasureMapChest : LockableContainer
    {
        public static Type[] Artifacts { get { return m_Artifacts; } }
        private static readonly Type[] m_Artifacts = new Type[]
        {
            typeof(CandelabraOfSouls), typeof(GoldBricks), typeof(PhillipsWoodenSteed),
            typeof(ArcticDeathDealer), typeof(BlazeOfDeath), typeof(BurglarsBandana),
            typeof(CavortingClub), typeof(DreadPirateHat),
            typeof(EnchantedTitanLegBone), typeof(GwennosHarp), typeof(IolosLute),
            typeof(LunaLance), typeof(NightsKiss), typeof(NoxRangersHeavyCrossbow),
            typeof(PolarBearMask), typeof(VioletCourage), typeof(HeartOfTheLion),
            typeof(ColdBlood), typeof(AlchemistsBauble), typeof(CaptainQuacklebushsCutlass),
            typeof(ShieldOfInvulnerability), typeof(AncientShipModelOfTheHMSCape),
            typeof(AdmiralsHeartyRum)
        };

        public static Type[] ArtifactsLevelFiveToSeven { get { return m_LevelFiveToSeven; } }
        private static Type[] m_LevelFiveToSeven = new Type[]
        {
            typeof(ForgedPardon), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(SurgeShield)
        };

        public static Type[] ArtifactsLevelSeven { get { return m_LevelSevenOnly; } }
        private static Type[] m_LevelSevenOnly = new Type[]
        {
            typeof(CoffinPiece), typeof(MasterSkeletonKey)
        };

        public static Type[] SOSArtifacts { get { return m_SOSArtifacts; } }
        private static Type[] m_SOSArtifacts = new Type[]
        {
            typeof(AntiqueWeddingDress),
            typeof(KelpWovenLeggings),
            typeof(RunedDriftwoodBow),
            typeof(ValkyrieArmor)
        };
        public static Type[] SOSDecor { get { return m_SOSDecor; } }
        private static Type[] m_SOSDecor = new Type[]
        {
            typeof(GrapeVine),
            typeof(LargeFishingNet)
        };

        public static Type[] ImbuingIngreds {  get { return m_ImbuingIngreds; } }
        private static Type[] m_ImbuingIngreds =
        {
            typeof(AbyssalCloth),   typeof(EssencePrecision), typeof(EssenceAchievement), typeof(EssenceBalance),
            typeof(EssenceControl), typeof(EssenceDiligence), typeof(EssenceDirection),   typeof(EssenceFeeling),
            typeof(EssenceOrder),   typeof(EssencePassion),   typeof(EssencePersistence), typeof(EssenceSingularity)
		};

        // Define the list of items to spawn in the chest
        private static readonly Type[] m_SpecificItems = new Type[]
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
			typeof(WWIIValorChest),
			typeof(Zulfiqar),
			// Replace with actual items
            // Add more items as needed        
        };

        private List<Item> m_Lifted = new List<Item>();

        private ChestQuality _Quality;

        [Constructable]
        public TreasureMapChest(int level)
            : this(null, level, false)
        {
        }

        public TreasureMapChest(Mobile owner, int level, bool temporary)
            : base(0xE40)
        {
            Owner = owner;
            Level = level;
            DeleteTime = DateTime.UtcNow + TimeSpan.FromHours(3.0);

            Temporary = temporary;
            Guardians = new List<Mobile>();
            AncientGuardians = new List<Mobile>();

            Timer = new DeleteTimer(this, DeleteTime);
            Timer.Start();
        }

        public TreasureMapChest(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 3000541; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Level { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DeleteTime { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime DigTime { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Temporary { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FirstOpenedByOwner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TreasureMap TreasureMap { get; set; }

        public Timer Timer { get; set; }

        public List<Mobile> Guardians { get; set; }

        public List<Mobile> AncientGuardians { get; set; }

        public ChestQuality ChestQuality
        {
            get { return _Quality; }
            set
            {
                if (_Quality != value)
                {
                    _Quality = value;

                    switch (_Quality)
                    {
                        case ChestQuality.Rusty: ItemID = 0xA306; break;
                        case ChestQuality.Standard: ItemID = 0xA304; break;
                        case ChestQuality.Gold: ItemID = 0xA308; break;
                    }
                }
            }
        }

        public bool FailedLockpick { get; set; }

        public override bool IsDecoContainer
        {
            get
            {
                return false;
            }
        }

        public static void Fill(Mobile from, LockableContainer cont, int level, bool isSos)
        {
            var map = from.Map;
            var luck = from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck;

            cont.Movable = false;
            cont.Locked = true;
            int count;

            if (level == 0)
            {
                cont.LockLevel = 0; // Can't be unlocked

                cont.DropItem(new Gold(Utility.RandomMinMax(50, 100)));

                if (Utility.RandomDouble() < 0.75)
                    cont.DropItem(new TreasureMap(0, Map.Trammel));
            }
            else
            {
                cont.TrapType = TrapType.ExplosionTrap;
                cont.TrapPower = level * 25;
                cont.TrapLevel = level;

                switch (level)
                {
                    case 1:
                        cont.RequiredSkill = 5;
                        break;
                    case 2:
                        cont.RequiredSkill = 45;
                        break;
                    case 3:
                        cont.RequiredSkill = 65;
                        break;
                    case 4:
                        cont.RequiredSkill = 75;
                        break;
                    case 5:
                        cont.RequiredSkill = 75;
                        break;
                    case 6:
                        cont.RequiredSkill = 80;
                        break;
					case 7:
                        cont.RequiredSkill = 80;
                        break;
                }

                cont.LockLevel = cont.RequiredSkill - 10;
                cont.MaxLockLevel = cont.RequiredSkill + 40;

                #region Gold
                cont.DropItem(new Gold(isSos ? level * 10000 : level * 5000));
                #endregion

                #region Scrolls
                if (isSos)
                {
                    switch(level)
                    {
                        default: count = 20; break;
                        case 0:
                        case 1: count = Utility.RandomMinMax(2, 5); break;
                        case 2: count = Utility.RandomMinMax(10, 15); break;
                    }
                }
                else
                {
                    count = level * 5;
                }

                for (int i = 0; i < count; ++i)
                    cont.DropItem(Loot.RandomScroll(0, 63, SpellbookType.Regular));
                #endregion

                #region Magical Items
                double propsScale = 1.0;

                if (Core.SE)
                {
                    switch (level)
                    {
                        case 1:
                            count = isSos ? Utility.RandomMinMax(2, 6) : 32;
							propsScale = 0.5625;
                            break;
                        case 2:
                            count = isSos ? Utility.RandomMinMax(10, 15) : 40;
							propsScale = 0.6875;
                            break;
                        case 3:
                            count = isSos ? Utility.RandomMinMax(15, 20) : 48;
							propsScale = 0.875;
                            break;
                        case 4:
                            count = isSos ? Utility.RandomMinMax(15, 20) : 56;
                            break;
                        case 5:
                            count = isSos ? Utility.RandomMinMax(15, 20) : 64;
                            break;
                        case 6:
                            count = isSos ? Utility.RandomMinMax(15, 20) : 72;
                            break;
                        case 7:
                            count = isSos ? Utility.RandomMinMax(15, 20) : 80;
                            break;
                        default:
                            count = 0;
                            break;
                    }
                }
                else
                    count = level * 6;

                for (int i = 0; i < count; ++i)
                {
                    Item item;

                    if (Core.AOS)
                        item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();
                    else
                        item = Loot.RandomArmorOrShieldOrWeapon();

                    if (item != null && Core.HS && RandomItemGenerator.Enabled)
                    {
                        int min, max;
                        GetRandomItemStat(out min, out max, propsScale);

                        RunicReforging.GenerateRandomItem(item, luck, min, max, map);

                        cont.DropItem(item);
                    }
                    else if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;

                        if (Core.AOS)
                        {
                            int attributeCount;
                            int min, max;

                            GetRandomAOSStats(out attributeCount, out min, out max);

                            BaseRunicTool.ApplyAttributesTo(weapon, attributeCount, min, max);
                        }
                        else
                        {
                            weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(6);
                            weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(6);
                            weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(6);
                        }

                        cont.DropItem(item);
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;

                        if (Core.AOS)
                        {
                            int attributeCount;
                            int min, max;

                            GetRandomAOSStats(out attributeCount, out min, out max);

                            BaseRunicTool.ApplyAttributesTo(armor, attributeCount, min, max);
                        }
                        else
                        {
                            armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(6);
                            armor.Durability = (ArmorDurabilityLevel)Utility.Random(6);
                        }

                        cont.DropItem(item);
                    }
                    else if (item is BaseHat)
                    {
                        BaseHat hat = (BaseHat)item;

                        if (Core.AOS)
                        {
                            int attributeCount;
                            int min, max;

                            GetRandomAOSStats(out attributeCount, out min, out max);

                            BaseRunicTool.ApplyAttributesTo(hat, attributeCount, min, max);
                        }

                        cont.DropItem(item);
                    }
                    else if (item is BaseJewel)
                    {
                        int attributeCount;
                        int min, max;

                        GetRandomAOSStats(out attributeCount, out min, out max);

                        BaseRunicTool.ApplyAttributesTo((BaseJewel)item, attributeCount, min, max);

                        cont.DropItem(item);
                    }
                }
            }
            #endregion

            #region Reagents
            if (isSos)
            {
                switch (level)
                {
                    default: count = Utility.RandomMinMax(45, 60); break;
                    case 0:
                    case 1: count = Utility.RandomMinMax(15, 20); break;
                    case 2: count = Utility.RandomMinMax(25, 40); break;
                }
            }
            else
            {
                count = level == 0 ? 12 : Utility.RandomMinMax(40, 60) * (level + 1);
            }

            for (int i = 0; i < count; i++)
            {
                cont.DropItemStacked(Loot.RandomPossibleReagent());
            }
            #endregion

            #region Gems
            if (level == 0)
                count = 2;
            else
                count = (level * 3) + 1;

            for (int i = 0; i < count; i++)
            {
                cont.DropItem(Loot.RandomGem());
            }
            #endregion

            #region Imbuing Ingreds
            if (level > 1)
            {
                Item item = Loot.Construct(m_ImbuingIngreds[Utility.Random(m_ImbuingIngreds.Length)]);

                item.Amount = level;
                cont.DropItem(item);
            }
            #endregion

            #region Specific Items
            // Add 3 specific items from the list to the chest
            for (int i = 0; i < 3; i++)
            {
                Item specificItem = Loot.Construct(m_SpecificItems[Utility.Random(m_SpecificItems.Length)]);
                cont.DropItem(specificItem);
            }
            #endregion            
			Item arty = null;
            Item special = null;

            if (isSos)
            {
                if (0.004 * level > Utility.RandomDouble())
                    arty = Loot.Construct(m_SOSArtifacts);
                if (0.006 * level > Utility.RandomDouble())
                    special = Loot.Construct(m_SOSDecor);
                else if (0.009 * level > Utility.RandomDouble())
                    special = new TreasureMap(Utility.RandomMinMax(level, Math.Min(7, level + 1)), cont.Map);

            }
            else
            {
                if (level >= 7)
                {
                    if (0.025 > Utility.RandomDouble())
                        special = Loot.Construct(m_LevelSevenOnly);
                    else if (0.10 > Utility.RandomDouble())
                        special = Loot.Construct(m_LevelFiveToSeven);
                    else if (0.25 > Utility.RandomDouble())
                        special = GetRandomSpecial(level, cont.Map);

                    arty = Loot.Construct(m_Artifacts);
                }
                else if (level >= 6)
                {
                    if (0.025 > Utility.RandomDouble())
                        special = Loot.Construct(m_LevelFiveToSeven);
                    else if (0.20 > Utility.RandomDouble())
                        special = GetRandomSpecial(level, cont.Map);

                    arty = Loot.Construct(m_Artifacts);
                }
                else if (level >= 5)
                {
                    if (0.005 > Utility.RandomDouble())
                        special = Loot.Construct(m_LevelFiveToSeven);
                    else if (0.15 > Utility.RandomDouble())
                        special = GetRandomSpecial(level, cont.Map);
                }
                else if (.10 > Utility.RandomDouble())
                {
                    special = GetRandomSpecial(level, cont.Map);
                }
            }

            if (arty != null)
            {
                Container pack = new Backpack();
                pack.Hue = 1278;

                pack.DropItem(arty);
                cont.DropItem(pack);
            }

            if (special != null)
                cont.DropItem(special);

            if (Core.SA)
            {
                int rolls = 2;

                if (level >= 5)
                    rolls += level - 2;

                RefinementComponent.Roll(cont, rolls, 0.10);
            }
        }

        private static Item GetRandomSpecial(int level, Map map)
        {
            Item special;

            switch (Utility.Random(8))
            {
                default:
                case 0: special = new CreepingVine(); break;
                case 1: special = new MessageInABottle(); break;
                case 2: special = new ScrollOfAlacrity(PowerScroll.Skills[Utility.Random(PowerScroll.Skills.Count)]); break;
                case 3: special = new Skeletonkey(); break;
                case 4: special = new TastyTreat(5); break;
                case 5: special = new TreasureMap(Utility.RandomMinMax(level, Math.Min(7, level + 1)), map); break;
                case 6: special = GetRandomRecipe(); break;
                case 7: special = ScrollOfTranscendence.CreateRandom(1, 5); break;
            }

            return special;
        }

        public static void GetRandomItemStat(out int min, out int max, double scale = 1.0)
        {
            int rnd = Utility.Random(100);

            if (rnd <= 1)
            {
                min = 500; max = 1300;
            }
            else if (rnd < 5)
            {
                min = 400; max = 1100;
            }
            else if (rnd < 25)
            {
                min = 350; max = 900;
            }
            else if (rnd < 50)
            {
                min = 250; max = 800;
            }
            else
            {
                min = 100; max = 600;
            }

			min = (int)(min * scale);
			max = (int)(max * scale);
        }

        public static Item GetRandomRecipe()
        {
            List<Server.Engines.Craft.Recipe> recipes = new List<Server.Engines.Craft.Recipe>(Server.Engines.Craft.Recipe.Recipes.Values);

            return new RecipeScroll(recipes[Utility.Random(recipes.Count)]);
        }

        public override bool CheckLocked(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                return false;
            }

            if (!TreasureMapInfo.NewSystem && Level == 0)
            {
                if (Guardians.Any(g => g.Alive))
                {
                    from.SendLocalizedMessage(1046448); // You must first kill the guardians before you may open this chest.
                    return true;
                }

                LockPick(from);
                return false;
            }
            else if (CanOpen(from))
            {
                return base.CheckLocked(from);
            }

            return true;
        }

        public virtual bool CanOpen(Mobile from)
        {
            if (TreasureMapInfo.NewSystem)
            {
                if (!Locked && TrapType != TrapType.None)
                {
                    from.SendLocalizedMessage(1159008); // That appears to be trapped, using the remove trap skill would yield better results...
                    return false;
                }
                else if (AncientGuardians.Any(ag => ag.Alive))
                {
                    from.SendLocalizedMessage(1046448); // You must first kill the guardians before you may open this chest.
                    return false;
                }
            }

            return !Locked;
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            return CheckLoot(from, item != this) && base.CheckItemUse(from, item);
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            return CheckLoot(from, true) && base.CheckLift(from, item, ref reject);
        }

        public override void OnItemLifted(Mobile from, Item item)
        {
            bool notYetLifted = !m_Lifted.Contains(item);

            from.RevealingAction();

            if (notYetLifted)
            {
                m_Lifted.Add(item);

                if (0.1 >= Utility.RandomDouble()) // 10% chance to spawn a new monster
                {
                    var spawn = TreasureMap.Spawn(Level, GetWorldLocation(), Map, from, false);

                    spawn.Hue = 2725;
                }
            }

            base.OnItemLifted(from, item);
        }

        public void SpawnAncientGuardian(Mobile from)
        {
            ExecuteTrap(from);

            if (!AncientGuardians.Any(g => g.Alive))
            {
                var spawn = TreasureMap.Spawn(Level, GetWorldLocation(), Map, from, false);
                spawn.NoLootOnDeath = true;

                spawn.Name = "Ancient Chest Guardian";
                spawn.Title = "(Guardian)";
                spawn.Tamable = false;

                if (spawn.HitsMaxSeed >= 0)
                    spawn.HitsMaxSeed = (int)(spawn.HitsMaxSeed * Paragon.HitsBuff);

                spawn.RawStr = (int)(spawn.RawStr * Paragon.StrBuff);
                spawn.RawInt = (int)(spawn.RawInt * Paragon.IntBuff);
                spawn.RawDex = (int)(spawn.RawDex * Paragon.DexBuff);

                spawn.Hits = spawn.HitsMax;
                spawn.Mana = spawn.ManaMax;
                spawn.Stam = spawn.StamMax;

                spawn.Hue = 1960;

                for (int i = 0; i < spawn.Skills.Length; i++)
                {
                    Skill skill = (Skill)spawn.Skills[i];

                    if (skill.Base > 0.0)
                        skill.Base *= Paragon.SkillsBuff;
                }

                AncientGuardians.Add(spawn);
            }
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (m.AccessLevel < AccessLevel.GameMaster)
            {
                m.SendLocalizedMessage(1048122, "", 0x8A5); // The chest refuses to be filled with treasure again.
                return false;
            }

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)4); // version

            writer.Write(FailedLockpick);
            writer.Write((int)_Quality);
            writer.Write(DigTime);
            writer.Write(AncientGuardians, true);

            writer.Write(FirstOpenedByOwner);
            writer.Write(TreasureMap);

            writer.Write(Guardians, true);
            writer.Write((bool)Temporary);

            writer.Write(Owner);

            writer.Write((int)Level);
            writer.WriteDeltaTime(DeleteTime);
            writer.Write(m_Lifted, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 4:
                    FailedLockpick = reader.ReadBool();
                    _Quality = (ChestQuality)reader.ReadInt();
                    DigTime = reader.ReadDateTime();
                    AncientGuardians = reader.ReadStrongMobileList();
                    goto case 3;
                case 3:
                    FirstOpenedByOwner = reader.ReadBool();
                    TreasureMap = reader.ReadItem() as TreasureMap;
                    goto case 2;
                case 2:
                    {
                        Guardians = reader.ReadStrongMobileList();
                        Temporary = reader.ReadBool();

                        goto case 1;
                    }
                case 1:
                    {
                        Owner = reader.ReadMobile();

                        goto case 0;
                    }
                case 0:
                    {
                        Level = reader.ReadInt();
                        DeleteTime = reader.ReadDeltaTime();
                        m_Lifted = reader.ReadStrongItemList();

                        if (version < 2)
                            Guardians = new List<Mobile>();

                        break;
                    }
            }

            if (!Temporary)
            {
                Timer = new DeleteTimer(this, DeleteTime);
                Timer.Start();
            }
            else
            {
                Delete();
            }
        }

        public override void OnAfterDelete()
        {
            if (Timer != null)
                Timer.Stop();

            Timer = null;

            base.OnAfterDelete();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
                list.Add(new RemoveEntry(from, this));
        }

        public override void LockPick(Mobile from)
        {
            base.LockPick(from);

            if (Map != null && ((TreasureMapInfo.NewSystem && FailedLockpick) || (Core.SA && 0.05 >= Utility.RandomDouble())))
            {
                var grubber = new Grubber();
                grubber.MoveToWorld(Map.GetSpawnPosition(Location, 1), Map);

                Item item = null;

                if (Items.Count > 0)
                {
                    do
                    {
                        item = Items[Utility.Random(Items.Count)];
                    }
                    while (item == null || item.LootType == LootType.Blessed);
                }

                grubber.PackItem(item);

                if (TreasureMapInfo.NewSystem)
                {
                    grubber.PrivateOverheadMessage(MessageType.Regular, 33, 1159062, from.NetState); // *A grubber appears and ganks a piece of your loot!*
                }
            }
        }

        public override void DisplayTo(Mobile to)
        {
            base.DisplayTo(to);

            if (!FirstOpenedByOwner && to == Owner)
            {
                if (TreasureMap != null)
                {
                    TreasureMap.OnChestOpened((PlayerMobile)to, this);
                }

                FirstOpenedByOwner = true;
            }
        }

        public override bool ExecuteTrap(Mobile from)
        {
            if (TreasureMapInfo.NewSystem && TrapType != TrapType.None)
            {
                int damage;

                if (TrapLevel > 0)
                    damage = Utility.RandomMinMax(10, 30) * TrapLevel;
                else
                    damage = TrapPower;

                AOS.Damage(from, damage, 0, 100, 0, 0, 0);

                // Your skin blisters from the heat!
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x2A, 503000);

                Effects.SendLocationEffect(from.Location, from.Map, 0x36BD, 15, 10);
                Effects.PlaySound(from.Location, from.Map, 0x307);

                return true;
            }
            else
            {
                return base.ExecuteTrap(from);
            }
        }

        public void BeginRemove(Mobile from)
        {
            if (!from.Alive)
                return;

            from.CloseGump(typeof(RemoveGump));
            from.SendGump(new RemoveGump(from, this));
        }

        public void EndRemove(Mobile from)
        {
            if (Deleted || from != Owner || !from.InRange(GetWorldLocation(), 3))
                return;

            from.SendLocalizedMessage(1048124, "", 0x8A5); // The old, rusted chest crumbles when you hit it.
            Delete();
        }

        private static void GetRandomAOSStats(out int attributeCount, out int min, out int max)
        {
            int rnd = Utility.Random(15);

            if (Core.SE)
            {
                if (rnd < 1)
                {
                    attributeCount = Utility.RandomMinMax(3, 5);
                    min = 50;
                    max = 100;
                }
                else if (rnd < 3)
                {
                    attributeCount = Utility.RandomMinMax(2, 5);
                    min = 40;
                    max = 80;
                }
                else if (rnd < 6)
                {
                    attributeCount = Utility.RandomMinMax(2, 4);
                    min = 30;
                    max = 60;
                }
                else if (rnd < 10)
                {
                    attributeCount = Utility.RandomMinMax(1, 3);
                    min = 20;
                    max = 40;
                }
                else
                {
                    attributeCount = 1;
                    min = 10;
                    max = 20;
                }
            }
            else
            {
                if (rnd < 1)
                {
                    attributeCount = Utility.RandomMinMax(2, 5);
                    min = 20;
                    max = 70;
                }
                else if (rnd < 3)
                {
                    attributeCount = Utility.RandomMinMax(2, 4);
                    min = 20;
                    max = 50;
                }
                else if (rnd < 6)
                {
                    attributeCount = Utility.RandomMinMax(2, 3);
                    min = 20;
                    max = 40;
                }
                else if (rnd < 10)
                {
                    attributeCount = Utility.RandomMinMax(1, 2);
                    min = 10;
                    max = 30;
                }
                else
                {
                    attributeCount = 1;
                    min = 10;
                    max = 20;
                }
            }
        }

        private bool CheckLoot(Mobile m, bool criminalAction)
        {
            if (Temporary)
                return false;

            if (m.AccessLevel >= AccessLevel.GameMaster || Owner == null || m == Owner)
                return true;

            Party p = Party.Get(Owner);

            if (p != null && p.Contains(m))
                return true;

            Map map = Map;

            if (map != null && (map.Rules & MapRules.HarmfulRestrictions) == 0)
            {
                if (criminalAction)
                    m.CriminalAction(true);
                else
                    m.SendLocalizedMessage(1010630); // Taking someone else's treasure is a criminal offense!

                return true;
            }

            m.SendLocalizedMessage(1010631); // You did not discover this chest!
            return false;
        }

        private class RemoveGump : Gump
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveGump(Mobile from, TreasureMapChest chest)
                : base(15, 15)
            {
                m_From = from;
                m_Chest = chest;

                Closable = false;
                Disposable = false;

                AddPage(0);

                AddBackground(30, 0, 240, 240, 2620);

                AddHtmlLocalized(45, 15, 200, 80, 1048125, 0xFFFFFF, false, false); // When this treasure chest is removed, any items still inside of it will be lost.
                AddHtmlLocalized(45, 95, 200, 60, 1048126, 0xFFFFFF, false, false); // Are you certain you're ready to remove this chest?

                AddButton(40, 153, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 155, 180, 40, 1048127, 0xFFFFFF, false, false); // Remove the Treasure Chest

                AddButton(40, 195, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(75, 197, 180, 35, 1006045, 0xFFFFFF, false, false); // Cancel
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 1)
                    m_Chest.EndRemove(m_From);
            }
        }

        private class RemoveEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly TreasureMapChest m_Chest;
            public RemoveEntry(Mobile from, TreasureMapChest chest)
                : base(6149, 3)
            {
                m_From = from;
                m_Chest = chest;

                Enabled = (from == chest.Owner);
            }

            public override void OnClick()
            {
                if (m_Chest.Deleted || m_From != m_Chest.Owner || !m_From.CheckAlive())
                    return;

                m_Chest.BeginRemove(m_From);
            }
        }

        private class DeleteTimer : Timer
        {
            private readonly Item m_Item;
            public DeleteTimer(Item item, DateTime time)
                : base(time - DateTime.UtcNow)
            {
                m_Item = item;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }
    }
}
