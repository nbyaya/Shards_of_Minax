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
    public class MagicalLootbox : LockableContainer
    {
        [Constructable]
        public MagicalLootbox() : base(0xE41) // Treasure Chest item ID
        {
            Name = "Magical Lootbox";
            Hue = Utility.RandomMinMax(1, 1600);

            // Add gold
            AddItemWithProbability(new Gold(Utility.RandomMinMax(500, 10000)), 1.0);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicClothing(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomMagicWeapon(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryA(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAA(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAB(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAC(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAD(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAE(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAF(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAG(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAH(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAI(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAJ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAK(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAL(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAM(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAN(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAO(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryAP(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryB(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryC(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryD(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryE(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryF(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryG(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryH(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryI(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryJ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryK(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryL(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryM(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryN(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryO(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryP(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryQ(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryR(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryS(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryT(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryU(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryV(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryW(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryY(), 0.004);
			AddItemWithProbability(new RandomSkillJewelryZ(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicJewelry(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new RandomMagicArmor(), 0.004);
			AddItemWithProbability(new AlchemyAugmentCrystal(), 0.004);
			AddItemWithProbability(new AnatomyAugmentCrystal(), 0.004);
			AddItemWithProbability(new AnimalLoreAugmentCrystal(), 0.004);
			AddItemWithProbability(new AnimalTamingAugmentCrystal(), 0.004);
			AddItemWithProbability(new ArcheryAugmentCrystal(), 0.004);
			AddItemWithProbability(new ArmsLoreAugmentCrystal(), 0.004);
			AddItemWithProbability(new ArmSlotChangeDeed(), 0.004);
			AddItemWithProbability(new BagOfBombs(), 0.004);
			AddItemWithProbability(new BagOfHealth(), 0.004);
			AddItemWithProbability(new BagOfJuice(), 0.004);
			AddItemWithProbability(new BanishingOrb(), 0.004);
			AddItemWithProbability(new BanishingRod(), 0.004);
			AddItemWithProbability(new BeggingAugmentCrystal(), 0.004);
			AddItemWithProbability(new BeltSlotChangeDeed(), 0.004);
			AddItemWithProbability(new BlacksmithyAugmentCrystal(), 0.004);
			AddItemWithProbability(new BloodSword(), 0.004);
			AddItemWithProbability(new BootsOfCommand(), 0.004);
			AddItemWithProbability(new BraceletSlotChangeDeed(), 0.004);
			AddItemWithProbability(new BushidoAugmentCrystal(), 0.004);
			AddItemWithProbability(new CampingAugmentCrystal(), 0.004);
			AddItemWithProbability(new CapacityIncreaseDeed(), 0.004);
			AddItemWithProbability(new CarpentryAugmentCrystal(), 0.004);
			AddItemWithProbability(new CartographyAugmentCrystal(), 0.004);
			AddItemWithProbability(new ChestSlotChangeDeed(), 0.004);
			AddItemWithProbability(new ChivalryAugmentCrystal(), 0.004);
			AddItemWithProbability(new ColdHitAreaCrystal(), 0.004);
			AddItemWithProbability(new ColdResistAugmentCrystal(), 0.004);
			AddItemWithProbability(new CookingAugmentCrystal(), 0.004);
			AddItemWithProbability(new CurseAugmentCrystal(), 0.004);
			AddItemWithProbability(new DetectingHiddenAugmentCrystal(), 0.004);
			AddItemWithProbability(new DiscordanceAugmentCrystal(), 0.004);
			AddItemWithProbability(new DispelAugmentCrystal(), 0.004);
			AddItemWithProbability(new EarringSlotChangeDeed(), 0.004);
			AddItemWithProbability(new EnergyHitAreaCrystal(), 0.004);
			AddItemWithProbability(new EnergyResistAugmentCrystal(), 0.004);
			AddItemWithProbability(new FatigueAugmentCrystal(), 0.004);
			AddItemWithProbability(new FencingAugmentCrystal(), 0.004);
			AddItemWithProbability(new FireballAugmentCrystal(), 0.004);
			AddItemWithProbability(new FireHitAreaCrystal(), 0.004);
			AddItemWithProbability(new FireResistAugmentCrystal(), 0.004);
			AddItemWithProbability(new FishingAugmentCrystal(), 0.004);
			AddItemWithProbability(new FletchingAugmentCrystal(), 0.004);
			AddItemWithProbability(new FocusAugmentCrystal(), 0.004);
			AddItemWithProbability(new FootwearSlotChangeDeed(), 0.004);
			AddItemWithProbability(new ForensicEvaluationAugmentCrystal(), 0.004);
			AddItemWithProbability(new GlovesOfCommand(), 0.004);
			AddItemWithProbability(new HarmAugmentCrystal(), 0.004);
			AddItemWithProbability(new HeadSlotChangeDeed(), 0.004);
			AddItemWithProbability(new HealingAugmentCrystal(), 0.004);
			AddItemWithProbability(new HerdingAugmentCrystal(), 0.004);
			AddItemWithProbability(new HidingAugmentCrystal(), 0.004);
			AddItemWithProbability(new ImbuingAugmentCrystal(), 0.004);
			AddItemWithProbability(new InscriptionAugmentCrystal(), 0.004);
			AddItemWithProbability(new ItemIdentificationAugmentCrystal(), 0.004);
			AddItemWithProbability(new JesterHatOfCommand(), 0.004);
			AddItemWithProbability(new LegsSlotChangeDeed(), 0.004);
			AddItemWithProbability(new LifeLeechAugmentCrystal(), 0.004);
			AddItemWithProbability(new LightningAugmentCrystal(), 0.004);
			AddItemWithProbability(new LockpickingAugmentCrystal(), 0.004);
			AddItemWithProbability(new LowerAttackAugmentCrystal(), 0.004);
			AddItemWithProbability(new LuckAugmentCrystal(), 0.004);
			AddItemWithProbability(new LumberjackingAugmentCrystal(), 0.004);
			AddItemWithProbability(new MaceFightingAugmentCrystal(), 0.004);
			AddItemWithProbability(new MageryAugmentCrystal(), 0.004);
			AddItemWithProbability(new ManaDrainAugmentCrystal(), 0.004);
			AddItemWithProbability(new ManaLeechAugmentCrystal(), 0.004);
			AddItemWithProbability(new MaxxiaScroll(), 0.004);
			AddItemWithProbability(new MeditationAugmentCrystal(), 0.004);
			AddItemWithProbability(new MiningAugmentCrystal(), 0.004);
			AddItemWithProbability(new MirrorOfKalandra(), 0.004);
			AddItemWithProbability(new MusicianshipAugmentCrystal(), 0.004);
			AddItemWithProbability(new NeckSlotChangeDeed(), 0.004);
			AddItemWithProbability(new NecromancyAugmentCrystal(), 0.004);
			AddItemWithProbability(new NinjitsuAugmentCrystal(), 0.004);
			AddItemWithProbability(new OneHandedTransformDeed(), 0.004);
			AddItemWithProbability(new ParryingAugmentCrystal(), 0.004);
			AddItemWithProbability(new PeacemakingAugmentCrystal(), 0.004);
			AddItemWithProbability(new PhysicalHitAreaCrystal(), 0.004);
			AddItemWithProbability(new PhysicalResistAugmentCrystal(), 0.004);
			AddItemWithProbability(new PlateLeggingsOfCommand(), 0.004);
			AddItemWithProbability(new PoisonHitAreaCrystal(), 0.004);
			AddItemWithProbability(new PoisoningAugmentCrystal(), 0.004);
			AddItemWithProbability(new PoisonResistAugmentCrystal(), 0.004);
			AddItemWithProbability(new ProvocationAugmentCrystal(), 0.004);
			AddItemWithProbability(new RemoveTrapAugmentCrystal(), 0.004);
			AddItemWithProbability(new ResistingSpellsAugmentCrystal(), 0.004);
			AddItemWithProbability(new RingSlotChangeDeed(), 0.004);
			AddItemWithProbability(new RodOfOrcControl(), 0.004);
			AddItemWithProbability(new ShirtSlotChangeDeed(), 0.004);
			AddItemWithProbability(new SnoopingAugmentCrystal(), 0.004);
			AddItemWithProbability(new SpellweavingAugmentCrystal(), 0.004);
			AddItemWithProbability(new SpiritSpeakAugmentCrystal(), 0.004);
			AddItemWithProbability(new StaminaLeechAugmentCrystal(), 0.004);
			AddItemWithProbability(new StealingAugmentCrystal(), 0.004);
			AddItemWithProbability(new StealthAugmentCrystal(), 0.004);
			AddItemWithProbability(new SwingSpeedAugmentCrystal(), 0.004);
			AddItemWithProbability(new SwordsmanshipAugmentCrystal(), 0.004);
			AddItemWithProbability(new TacticsAugmentCrystal(), 0.004);
			AddItemWithProbability(new TailoringAugmentCrystal(), 0.004);
			AddItemWithProbability(new TalismanSlotChangeDeed(), 0.004);
			AddItemWithProbability(new TasteIDAugmentCrystal(), 0.004);
			AddItemWithProbability(new ThrowingAugmentCrystal(), 0.004);
			AddItemWithProbability(new TinkeringAugmentCrystal(), 0.004);
			AddItemWithProbability(new TrackingAugmentCrystal(), 0.004);
			AddItemWithProbability(new VeterinaryAugmentCrystal(), 0.004);
			AddItemWithProbability(new WeaponSpeedAugmentCrystal(), 0.004);
			AddItemWithProbability(new WrestlingAugmentCrystal(), 0.004);
			AddItemWithProbability(new PetSlotDeed(), 0.004);
			AddItemWithProbability(new PetBondDeed(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new StatCapOrb(), 0.004);
			AddItemWithProbability(new SkillOrb(), 0.004);
			AddItemWithProbability(new AbysmalHorrorSummoningMateria(), 0.004);
			AddItemWithProbability(new AcidElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new AgapiteElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new AirElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new AlligatorSummoningMateria(), 0.004);
			AddItemWithProbability(new AncientLichSummoningMateria(), 0.004);
			AddItemWithProbability(new AncientWyrmSummoningMateria(), 0.004);
			AddItemWithProbability(new AntLionSummoningMateria(), 0.004);
			AddItemWithProbability(new ArcaneDaemonSummoningMateria(), 0.004);
			AddItemWithProbability(new ArcticOgreLordSummoningMateria(), 0.004);
			AddItemWithProbability(new AxeBreathMateria(), 0.004);
			AddItemWithProbability(new AxeCircleMateria(), 0.004);
			AddItemWithProbability(new AxeLineMateria(), 0.004);
			AddItemWithProbability(new BakeKitsuneSummoningMateria(), 0.004);
			AddItemWithProbability(new BalronSummoningMateria(), 0.004);
			AddItemWithProbability(new BarracoonSummoningMateria(), 0.004);
			AddItemWithProbability(new BeeBreathMateria(), 0.004);
			AddItemWithProbability(new BeeCircleMateria(), 0.004);
			AddItemWithProbability(new BeeLineMateria(), 0.004);
			AddItemWithProbability(new BeetleSummoningMateria(), 0.004);
			AddItemWithProbability(new BlackBearSummoningMateria(), 0.004);
			AddItemWithProbability(new BlackDragoonPirateMateria(), 0.004);
			AddItemWithProbability(new BlackSolenInfiltratorQueenSummoningMateria(), 0.004);
			AddItemWithProbability(new BlackSolenInfiltratorWarriorMateria(), 0.004);
			AddItemWithProbability(new BlackSolenQueenSummoningMateria(), 0.004);
			AddItemWithProbability(new BlackSolenWarriorSummoningMateria(), 0.004);
			AddItemWithProbability(new BlackSolenWorkerSummoningMateria(), 0.004);
			AddItemWithProbability(new BladesBreathMateria(), 0.004);
			AddItemWithProbability(new BladesCircleMateria(), 0.004);
			AddItemWithProbability(new BladesLineMateria(), 0.004);
			AddItemWithProbability(new BloodElementalSummoningGem(), 0.004);
			AddItemWithProbability(new BloodSwarmGem(), 0.004);
			AddItemWithProbability(new BoarSummoningMateria(), 0.004);
			AddItemWithProbability(new BogleSummoningMateria(), 0.004);
			AddItemWithProbability(new BoglingSummoningMateria(), 0.004);
			AddItemWithProbability(new BogThingSummoningMateria(), 0.004);
			AddItemWithProbability(new BoneDemonSummoningMateria(), 0.004);
			AddItemWithProbability(new BoneKnightSummoningMateria(), 0.004);
			AddItemWithProbability(new BoneMagiSummoningMateria(), 0.004);
			AddItemWithProbability(new BoulderBreathMateria(), 0.004);
			AddItemWithProbability(new BoulderCircleMateria(), 0.004);
			AddItemWithProbability(new BoulderLineMateria(), 0.004);
			AddItemWithProbability(new BrigandSummoningMateria(), 0.004);
			AddItemWithProbability(new BronzeElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new BrownBearSummoningMateria(), 0.004);
			AddItemWithProbability(new BullFrogSummoningMateria(), 0.004);
			AddItemWithProbability(new BullSummoningMateria(), 0.004);
			AddItemWithProbability(new CatSummoningMateria(), 0.004);
			AddItemWithProbability(new CentaurSummoningMateria(), 0.004);
			AddItemWithProbability(new ChaosDaemonSummoningMateria(), 0.004);
			AddItemWithProbability(new ChaosDragoonEliteSummoningMateria(), 0.004);
			AddItemWithProbability(new ChaosDragoonSummoningMateria(), 0.004);
			AddItemWithProbability(new ChickenSummoningMateria(), 0.004);
			AddItemWithProbability(new CopperElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new CorpserSummoningMateria(), 0.004);
			AddItemWithProbability(new CorrosiveSlimeSummoningMateria(), 0.004);
			AddItemWithProbability(new CorruptedSoulMateria(), 0.004);
			AddItemWithProbability(new CougarSummoningMateria(), 0.004);
			AddItemWithProbability(new CowSummoningMateria(), 0.004);
			AddItemWithProbability(new CraneSummoningMateria(), 0.004);
			AddItemWithProbability(new CrankBreathMateria(), 0.004);
			AddItemWithProbability(new CrankCircleMateria(), 0.004);
			AddItemWithProbability(new CrankLineMateria(), 0.004);
			AddItemWithProbability(new CrimsonDragonSummoningMateria(), 0.004);
			AddItemWithProbability(new CrystalElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new CurtainBreathMateria(), 0.004);
			AddItemWithProbability(new CurtainCircleMateria(), 0.004);
			AddItemWithProbability(new CurtainLineMateria(), 0.004);
			AddItemWithProbability(new CuSidheSummoningMateria(), 0.004);
			AddItemWithProbability(new CyclopsSummoningMateria(), 0.004);
			AddItemWithProbability(new DaemonSummoningMateria(), 0.004);
			AddItemWithProbability(new DarkWispSummoningMateria(), 0.004);
			AddItemWithProbability(new DarkWolfSummoningMateria(), 0.004);
			AddItemWithProbability(new DeathWatchBeetleSummoningMateria(), 0.004);
			AddItemWithProbability(new DeepSeaSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new DeerBreathMateria(), 0.004);
			AddItemWithProbability(new DeerCircleMateria(), 0.004);
			AddItemWithProbability(new DeerLineMateria(), 0.004);
			AddItemWithProbability(new DemonKnightSummoningMateria(), 0.004);
			AddItemWithProbability(new DesertOstardSummoningMateria(), 0.004);
			AddItemWithProbability(new DevourerSummoningMateria(), 0.004);
			AddItemWithProbability(new DireWolfSummoningMateria(), 0.004);
			AddItemWithProbability(new DogSummoningMateria(), 0.004);
			AddItemWithProbability(new DolphinSummoningMateria(), 0.004);
			AddItemWithProbability(new DopplegangerSummoningMateria(), 0.004);
			AddItemWithProbability(new DragonSummoningMateria(), 0.004);
			AddItemWithProbability(new DrakeSummoningMateria(), 0.004);
			AddItemWithProbability(new DreadSpiderSummoningMateria(), 0.004);
			AddItemWithProbability(new DullCopperElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new DVortexBreathMateria(), 0.004);
			AddItemWithProbability(new DVortexCircleMateria(), 0.004);
			AddItemWithProbability(new DVortexLineMateria(), 0.004);
			AddItemWithProbability(new EagleSummoningMateria(), 0.004);
			AddItemWithProbability(new EarthElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new EfreetSummoningMateria(), 0.004);
			AddItemWithProbability(new ElderGazerSummoningMateria(), 0.004);
			AddItemWithProbability(new EliteNinjaSummoningMateria(), 0.004);
			AddItemWithProbability(new EttinSummoningMateria(), 0.004);
			AddItemWithProbability(new EvilHealerSummoningMateria(), 0.004);
			AddItemWithProbability(new EvilMageSummoningMateria(), 0.004);
			AddItemWithProbability(new ExecutionerMateria(), 0.004);
			AddItemWithProbability(new ExodusMinionSummoningMateria(), 0.004);
			AddItemWithProbability(new ExodusOverseerSummoningMateria(), 0.004);
			AddItemWithProbability(new FanDancerSummoningMateria(), 0.004);
			AddItemWithProbability(new FeralTreefellowSummoningMateria(), 0.004);
			AddItemWithProbability(new FetidEssenceMateria(), 0.004);
			AddItemWithProbability(new FireBeetleSummoningMateria(), 0.004);
			AddItemWithProbability(new FireElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new FireGargoyleSummoningMateria(), 0.004);
			AddItemWithProbability(new FireSteedSummoningMateria(), 0.004);
			AddItemWithProbability(new FlaskBreathMateria(), 0.004);
			AddItemWithProbability(new FlaskCircleMateria(), 0.004);
			AddItemWithProbability(new FlaskLineMateria(), 0.004);
			AddItemWithProbability(new FleshGolemSummoningMateria(), 0.004);
			AddItemWithProbability(new FleshRendererSummoningMateria(), 0.004);
			AddItemWithProbability(new ForestOstardSummoningMateria(), 0.004);
			AddItemWithProbability(new FrenziedOstardSummoningMateria(), 0.004);
			AddItemWithProbability(new FrostOozeSummoningMateria(), 0.004);
			AddItemWithProbability(new FrostSpiderSummoningMateria(), 0.004);
			AddItemWithProbability(new FrostTrollSummoningMateria(), 0.004);
			AddItemWithProbability(new FTreeCircleMateria(), 0.004);
			AddItemWithProbability(new FTreeLineMateria(), 0.004);
			AddItemWithProbability(new GamanSummoningMateria(), 0.004);
			AddItemWithProbability(new GargoyleSummoningMateria(), 0.004);
			AddItemWithProbability(new GasBreathMateria(), 0.004);
			AddItemWithProbability(new GasCircleMateria(), 0.004);
			AddItemWithProbability(new GasLineMateria(), 0.004);
			AddItemWithProbability(new GateBreathMateria(), 0.004);
			AddItemWithProbability(new GateCircleMateria(), 0.004);
			AddItemWithProbability(new GateLineMateria(), 0.004);
			AddItemWithProbability(new GazerSummoningMateria(), 0.004);
			AddItemWithProbability(new GhoulSummoningMateria(), 0.004);
			AddItemWithProbability(new GiantBlackWidowSummoningMateria(), 0.004);
			AddItemWithProbability(new GiantRatSummoningMateria(), 0.004);
			AddItemWithProbability(new GiantSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new GiantSpiderSummoningMateria(), 0.004);
			AddItemWithProbability(new GiantToadSummoningMateria(), 0.004);
			AddItemWithProbability(new GibberlingSummoningMateria(), 0.004);
			AddItemWithProbability(new GlowBreathMateria(), 0.004);
			AddItemWithProbability(new GlowCircleMateria(), 0.004);
			AddItemWithProbability(new GlowLineMateria(), 0.004);
			AddItemWithProbability(new GoatSummoningMateria(), 0.004);
			AddItemWithProbability(new GoldenElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new GolemSummoningMateria(), 0.004);
			AddItemWithProbability(new GoreFiendSummoningMateria(), 0.004);
			AddItemWithProbability(new GorillaSummoningMateria(), 0.004);
			AddItemWithProbability(new GreaterDragonSummoningMateria(), 0.004);
			AddItemWithProbability(new GreaterMongbatSummoningMateria(), 0.004);
			AddItemWithProbability(new GreatHartSummoningMateria(), 0.004);
			AddItemWithProbability(new GreyWolfSummoningMateria(), 0.004);
			AddItemWithProbability(new GrizzlyBearSummoningMateria(), 0.004);
			AddItemWithProbability(new GuillotineBreathMateria(), 0.004);
			AddItemWithProbability(new GuillotineCircleMateria(), 0.004);
			AddItemWithProbability(new GuillotineLineMateria(), 0.004);
			AddItemWithProbability(new HarpySummoningMateria(), 0.004);
			AddItemWithProbability(new HeadBreathMateria(), 0.004);
			AddItemWithProbability(new HeadCircleMateria(), 0.004);
			AddItemWithProbability(new HeadlessOneSummoningMateria(), 0.004);
			AddItemWithProbability(new HeadLineMateria(), 0.004);
			AddItemWithProbability(new HealerMateria(), 0.004);
			AddItemWithProbability(new HeartBreathMateria(), 0.004);
			AddItemWithProbability(new HeartCircleMateria(), 0.004);
			AddItemWithProbability(new HeartLineMateria(), 0.004);
			AddItemWithProbability(new HellCatSummoningMateria(), 0.004);
			AddItemWithProbability(new HellHoundSummoningMateria(), 0.004);
			AddItemWithProbability(new HellSteedSummoningMateria(), 0.004);
			AddItemWithProbability(new HindSummoningMateria(), 0.004);
			AddItemWithProbability(new HiryuSummoningMateria(), 0.004);
			AddItemWithProbability(new HorseSummoningMateria(), 0.004);
			AddItemWithProbability(new IceElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new IceFiendSummoningMateria(), 0.004);
			AddItemWithProbability(new IceSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new IceSnakeSummoningMateria(), 0.004);
			AddItemWithProbability(new ImpSummoningMateria(), 0.004);
			AddItemWithProbability(new JackRabbitSummoningMateria(), 0.004);
			AddItemWithProbability(new KazeKemonoSummoningMateria(), 0.004);
			AddItemWithProbability(new KirinSummoningMateria(), 0.004);
			AddItemWithProbability(new LavaLizardSummoningMateria(), 0.004);
			AddItemWithProbability(new LavaSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new LavaSnakeSummoningMateria(), 0.004);
			AddItemWithProbability(new LesserHiryuSummoningMateria(), 0.004);
			AddItemWithProbability(new LichLordSummoningMateria(), 0.004);
			AddItemWithProbability(new LichSummoningMateria(), 0.004);
			AddItemWithProbability(new LizardmanSummoningMateria(), 0.004);
			AddItemWithProbability(new LlamaSummoningMateria(), 0.004);
			AddItemWithProbability(new MaidenBreathMateria(), 0.004);
			AddItemWithProbability(new MaidenCircleMateria(), 0.004);
			AddItemWithProbability(new MaidenLineMateria(), 0.004);
			AddItemWithProbability(new MinotaurCaptainSummoningMateria(), 0.004);
			AddItemWithProbability(new MountainGoatSummoningMateria(), 0.004);
			AddItemWithProbability(new MummySummoningMateria(), 0.004);
			AddItemWithProbability(new MushroomBreathMateria(), 0.004);
			AddItemWithProbability(new MushroomCircleMateria(), 0.004);
			AddItemWithProbability(new MushroomLineMateria(), 0.004);
			AddItemWithProbability(new NightmareSummoningMateria(), 0.004);
			AddItemWithProbability(new NutcrackerBreathMateria(), 0.004);
			AddItemWithProbability(new NutcrackerCircleMateria(), 0.004);
			AddItemWithProbability(new NutcrackerLineMateria(), 0.004);
			AddItemWithProbability(new OFlaskBreathMateria(), 0.004);
			AddItemWithProbability(new OFlaskCircleMateria(), 0.004);
			AddItemWithProbability(new OFlaskMateria(), 0.004);
			AddItemWithProbability(new OgreLordSummoningMateria(), 0.004);
			AddItemWithProbability(new OgreSummoningMateria(), 0.004);
			AddItemWithProbability(new OniSummoningMateria(), 0.004);
			AddItemWithProbability(new OphidianArchmageSummoningMateria(), 0.004);
			AddItemWithProbability(new OphidianKnightSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcBomberSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcBruteSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcCaptainSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcishLordSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcishMageSummoningMateria(), 0.004);
			AddItemWithProbability(new OrcSummoningMateria(), 0.004);
			AddItemWithProbability(new PackHorseSummoningMateria(), 0.004);
			AddItemWithProbability(new PackLlamaSummoningMateria(), 0.004);
			AddItemWithProbability(new PantherSummoningMateria(), 0.004);
			AddItemWithProbability(new ParaBreathMateria(), 0.004);
			AddItemWithProbability(new ParaCircleMateria(), 0.004);
			AddItemWithProbability(new ParaLineMateria(), 0.004);
			AddItemWithProbability(new PhoenixSummoningMateria(), 0.004);
			AddItemWithProbability(new PigSummoningMateria(), 0.004);
			AddItemWithProbability(new PixieSummoningMateria(), 0.004);
			AddItemWithProbability(new PlagueBeastSummoningMateria(), 0.004);
			AddItemWithProbability(new PoisonElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new PolarBearSummoningMateria(), 0.004);
			AddItemWithProbability(new RabbitSummoningMateria(), 0.004);
			AddItemWithProbability(new RaiJuSummoningMateria(), 0.004);
			AddItemWithProbability(new RatmanArcherSummoningMateria(), 0.004);
			AddItemWithProbability(new RatmanMageSummoningMateria(), 0.004);
			AddItemWithProbability(new RatmanSummoningMateria(), 0.004);
			AddItemWithProbability(new RatSummoningMateria(), 0.004);
			AddItemWithProbability(new ReaperSummoningMateria(), 0.004);
			AddItemWithProbability(new RevenantSummoningMateria(), 0.004);
			AddItemWithProbability(new RidgebackSummoningMateria(), 0.004);
			AddItemWithProbability(new RikktorSummoningMateria(), 0.004);
			AddItemWithProbability(new RoninSummoningMateria(), 0.004);
			AddItemWithProbability(new RuneBeetleSummoningMateria(), 0.004);
			AddItemWithProbability(new RuneBreathMateria(), 0.004);
			AddItemWithProbability(new RuneCircleMateria(), 0.004);
			AddItemWithProbability(new RuneLineMateria(), 0.004);
			AddItemWithProbability(new SatyrSummoningMateria(), 0.004);
			AddItemWithProbability(new SavageShamanSummoningMateria(), 0.004);
			AddItemWithProbability(new SavageSummoningMateria(), 0.004);
			AddItemWithProbability(new SawBreathMateria(), 0.004);
			AddItemWithProbability(new SawCircleMateria(), 0.004);
			AddItemWithProbability(new SawLineMateria(), 0.004);
			AddItemWithProbability(new ScaledSwampDragonSummoningMateria(), 0.004);
			AddItemWithProbability(new ScorpionSummoningMateria(), 0.004);
			AddItemWithProbability(new SeaSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new ShadowWispSummoningMateria(), 0.004);
			AddItemWithProbability(new ShadowWyrmSummoningMateria(), 0.004);
			AddItemWithProbability(new SheepSummoningMateria(), 0.004);
			AddItemWithProbability(new SilverSerpentSummoningMateria(), 0.004);
			AddItemWithProbability(new SilverSteedSummoningMateria(), 0.004);
			AddItemWithProbability(new SkeletalDragonSummoningMateria(), 0.004);
			AddItemWithProbability(new SkeletalKnightSummoningMateria(), 0.004);
			AddItemWithProbability(new SkeletalMageSummoningMateria(), 0.004);
			AddItemWithProbability(new SkeletalMountSummoningMateria(), 0.004);
			AddItemWithProbability(new SkeletonBreathMateria(), 0.004);
			AddItemWithProbability(new SkeletonCircleMateria(), 0.004);
			AddItemWithProbability(new SkeletonLineMateria(), 0.004);
			AddItemWithProbability(new SkeletonSummoningMateria(), 0.004);
			AddItemWithProbability(new SkullBreathMateria(), 0.004);
			AddItemWithProbability(new SkullCircleMateria(), 0.004);
			AddItemWithProbability(new SkullLineMateria(), 0.004);
			AddItemWithProbability(new SlimeSummoningMateria(), 0.004);
			AddItemWithProbability(new SmokeBreathMateria(), 0.004);
			AddItemWithProbability(new SmokeCircleMateria(), 0.004);
			AddItemWithProbability(new SmokeLineMateria(), 0.004);
			AddItemWithProbability(new SnakeSummoningMateria(), 0.004);
			AddItemWithProbability(new SnowElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new SnowLeopardSummoningMateria(), 0.004);
			AddItemWithProbability(new SocketDeed(), 0.004);
			AddItemWithProbability(new SocketDeed1(), 0.004);
			AddItemWithProbability(new SocketDeed2(), 0.004);
			AddItemWithProbability(new SocketDeed3(), 0.004);
			AddItemWithProbability(new SocketDeed4(), 0.004);
			AddItemWithProbability(new SocketDeed5(), 0.004);
			AddItemWithProbability(new SparkleBreathMateria(), 0.004);
			AddItemWithProbability(new SparkleCircleMateria(), 0.004);
			AddItemWithProbability(new SparkleLineMateria(), 0.004);
			AddItemWithProbability(new SpikeBreathMateria(), 0.004);
			AddItemWithProbability(new SpikeCircleMateria(), 0.004);
			AddItemWithProbability(new SpikeLineMateria(), 0.004);
			AddItemWithProbability(new StoneBreathMateria(), 0.004);
			AddItemWithProbability(new StoneCircleMateria(), 0.004);
			AddItemWithProbability(new StoneLineMateria(), 0.004);
			AddItemWithProbability(new SuccubusSummoningMateria(), 0.004);
			AddItemWithProbability(new TimeBreathMateria(), 0.004);
			AddItemWithProbability(new TimeCircleMateria(), 0.004);
			AddItemWithProbability(new TimeLineMateria(), 0.004);
			AddItemWithProbability(new TitanSummoningMateria(), 0.004);
			AddItemWithProbability(new ToxicElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new TrapBreathMateria(), 0.004);
			AddItemWithProbability(new TrapCircleMateria(), 0.004);
			AddItemWithProbability(new TrapLineMateria(), 0.004);
			AddItemWithProbability(new TreeBreathMateria(), 0.004);
			AddItemWithProbability(new TroglodyteSummoningMateria(), 0.004);
			AddItemWithProbability(new TrollSummoningMateria(), 0.004);
			AddItemWithProbability(new UnicornSummoningMateria(), 0.004);
			AddItemWithProbability(new ValoriteElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new VampireBatSummoningMateria(), 0.004);
			AddItemWithProbability(new VeriteElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new VortexBreathMateria(), 0.004);
			AddItemWithProbability(new VortexCircleMateria(), 0.004);
			AddItemWithProbability(new VortexLineMateria(), 0.004);
			AddItemWithProbability(new WalrusSummoningMateria(), 0.004);
			AddItemWithProbability(new WaterBreathMateria(), 0.004);
			AddItemWithProbability(new WaterCircleMateria(), 0.004);
			AddItemWithProbability(new WaterElementalSummoningMateria(), 0.004);
			AddItemWithProbability(new WaterLineMateria(), 0.004);
			AddItemWithProbability(new WhiteWolfSummoningMateria(), 0.004);
			AddItemWithProbability(new WhiteWyrmSummoningMateria(), 0.004);
			AddItemWithProbability(new WispSummoningMateria(), 0.004);
			AddItemWithProbability(new WraithSummoningMateria(), 0.004);
			AddItemWithProbability(new WyvernSummoningMateria(), 0.004);
			AddItemWithProbability(new ZombieSummoningMateria(), 0.004);
			AddItemWithProbability(new MythicAmethyst(), 0.004);
			AddItemWithProbability(new LegendaryAmethyst(), 0.004);
			AddItemWithProbability(new AncientAmethyst(), 0.004);
			AddItemWithProbability(new FenCrystal(), 0.004);
			AddItemWithProbability(new RhoCrystal(), 0.004);
			AddItemWithProbability(new RysCrystal(), 0.004);
			AddItemWithProbability(new WyrCrystal(), 0.004);
			AddItemWithProbability(new FreCrystal(), 0.004);
			AddItemWithProbability(new TorCrystal(), 0.004);
			AddItemWithProbability(new VelCrystal(), 0.004);
			AddItemWithProbability(new XenCrystal(), 0.004);
			AddItemWithProbability(new PolCrystal(), 0.004);
			AddItemWithProbability(new WolCrystal(), 0.004);
			AddItemWithProbability(new BalCrystal(), 0.004);
			AddItemWithProbability(new TalCrystal(), 0.004);
			AddItemWithProbability(new JalCrystal(), 0.004);
			AddItemWithProbability(new RalCrystal(), 0.004);
			AddItemWithProbability(new KalCrystal(), 0.004);
			AddItemWithProbability(new MythicDiamond(), 0.004);
			AddItemWithProbability(new LegendaryDiamond(), 0.004);
			AddItemWithProbability(new AncientDiamond(), 0.004);
			AddItemWithProbability(new MythicEmerald(), 0.004);
			AddItemWithProbability(new LegendaryEmerald(), 0.004);
			AddItemWithProbability(new AncientEmerald(), 0.004);
			AddItemWithProbability(new KeyAugment(), 0.004);
			AddItemWithProbability(new RadiantRhoCrystal(), 0.004);
			AddItemWithProbability(new RadiantRysCrystal(), 0.004);
			AddItemWithProbability(new RadiantWyrCrystal(), 0.004);
			AddItemWithProbability(new RadiantFreCrystal(), 0.004);
			AddItemWithProbability(new RadiantTorCrystal(), 0.004);
			AddItemWithProbability(new RadiantVelCrystal(), 0.004);
			AddItemWithProbability(new RadiantXenCrystal(), 0.004);
			AddItemWithProbability(new RadiantPolCrystal(), 0.004);
			AddItemWithProbability(new RadiantWolCrystal(), 0.004);
			AddItemWithProbability(new RadiantBalCrystal(), 0.004);
			AddItemWithProbability(new RadiantTalCrystal(), 0.004);
			AddItemWithProbability(new RadiantJalCrystal(), 0.004);
			AddItemWithProbability(new RadiantRalCrystal(), 0.004);
			AddItemWithProbability(new RadiantKalCrystal(), 0.004);
			AddItemWithProbability(new MythicRuby(), 0.004);
			AddItemWithProbability(new LegendaryRuby(), 0.004);
			AddItemWithProbability(new AncientRuby(), 0.004);
			AddItemWithProbability(new TyrRune(), 0.004);
			AddItemWithProbability(new AhmRune(), 0.004);
			AddItemWithProbability(new MorRune(), 0.004);
			AddItemWithProbability(new MefRune(), 0.004);
			AddItemWithProbability(new YlmRune(), 0.004);
			AddItemWithProbability(new KotRune(), 0.004);
			AddItemWithProbability(new JorRune(), 0.004);
			AddItemWithProbability(new MythicSapphire(), 0.004);
			AddItemWithProbability(new LegendarySapphire(), 0.004);
			AddItemWithProbability(new AncientSapphire(), 0.004);
			AddItemWithProbability(new MythicSkull(), 0.004);
			AddItemWithProbability(new AncientSkull(), 0.004);
			AddItemWithProbability(new LegendarySkull(), 0.004);
			AddItemWithProbability(new GlimmeringGranite(), 0.004);
			AddItemWithProbability(new GlimmeringClay(), 0.004);
			AddItemWithProbability(new GlimmeringHeartstone(), 0.004);
			AddItemWithProbability(new GlimmeringGypsum(), 0.004);
			AddItemWithProbability(new GlimmeringIronOre(), 0.004);
			AddItemWithProbability(new GlimmeringOnyx(), 0.004);
			AddItemWithProbability(new GlimmeringMarble(), 0.004);
			AddItemWithProbability(new GlimmeringPetrifiedWood(), 0.004);
			AddItemWithProbability(new GlimmeringLimestone(), 0.004);
			AddItemWithProbability(new GlimmeringBloodrock(), 0.004);
			AddItemWithProbability(new MythicTourmaline(), 0.004);
			AddItemWithProbability(new LegendaryTourmaline(), 0.004);
			AddItemWithProbability(new AncientTourmaline(), 0.004);
			AddItemWithProbability(new MythicWood(), 0.004);
			AddItemWithProbability(new LegendaryWood(), 0.004);
			AddItemWithProbability(new BootsOfCommand(), 0.004);
			AddItemWithProbability(new GlovesOfCommand(), 0.004);
			AddItemWithProbability(new GrandmastersRobe(), 0.004);
			AddItemWithProbability(new JesterHatOfCommand(), 0.004);
			AddItemWithProbability(new PlateLeggingsOfCommand(), 0.004);
			AddItemWithProbability(new AshAxe(), 0.004);
			AddItemWithProbability(new BraceletOfNaturesBounty(), 0.004);
			AddItemWithProbability(new CampersBackpack(), 0.004);
			AddItemWithProbability(new ExtraPack(), 0.004);
			AddItemWithProbability(new FrostwoodAxe(), 0.004);
			AddItemWithProbability(new GoldenCrown(), 0.004);
			AddItemWithProbability(new GoldenDrakelingScaleShield(), 0.004);
			AddItemWithProbability(new HeartwoodAxe(), 0.004);
			AddItemWithProbability(new IcicleStaff(), 0.004);
			AddItemWithProbability(new LightLordsScepter(), 0.004);
			AddItemWithProbability(new MasterBall(), 0.004);
			AddItemWithProbability(new MasterWeaponOil(), 0.004);
			AddItemWithProbability(new Pokeball(), 0.004);
			AddItemWithProbability(new ShadowIronShovel(), 0.004);
			AddItemWithProbability(new StolenTile(), 0.004);
			AddItemWithProbability(new TrapGloves(), 0.004);
			AddItemWithProbability(new TrapGorget(), 0.004);
			AddItemWithProbability(new TrapLegs(), 0.004);
			AddItemWithProbability(new TrapSleeves(), 0.004);
			AddItemWithProbability(new TrapTunic(), 0.004);
			AddItemWithProbability(new WeaponOil(), 0.004);
			AddItemWithProbability(new WizardKey(), 0.004);
			AddItemWithProbability(new YewAxe(), 0.004);
			AddItemWithProbability(new AssassinsDagger(), 0.004);
			AddItemWithProbability(new BagOfBombs(), 0.004);
			AddItemWithProbability(new BagOfHealth(), 0.004);
			AddItemWithProbability(new BagOfJuice(), 0.004);
			AddItemWithProbability(new BanishingOrb(), 0.004);
			AddItemWithProbability(new BanishingRod(), 0.004);
			AddItemWithProbability(new BeggarKingsCrown(), 0.004);
			AddItemWithProbability(new BloodSword(), 0.004);
			AddItemWithProbability(new BloodwoodAxe(), 0.004);
			AddItemWithProbability(new GlovesOfTheGrandmasterThief(), 0.004);
			AddItemWithProbability(new MagicMasterKey(), 0.004);
			AddItemWithProbability(new PlantingGloves(), 0.004);
			AddItemWithProbability(new QuickswordEnilno(), 0.004);
			AddItemWithProbability(new RodOfOrcControl(), 0.004);
			AddItemWithProbability(new ScryingOrb(), 0.004);
			AddItemWithProbability(new SiegeHammer(), 0.004);
			AddItemWithProbability(new SnoopersMasterScope(), 0.004);
			AddItemWithProbability(new ThiefsGlove(), 0.004);
			AddItemWithProbability(new TileExcavatorShovel(), 0.004);
			AddItemWithProbability(new TomeOfTime(), 0.004);
			AddItemWithProbability(new UniversalAbsorbingDyeTub(), 0.004);


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

        public MagicalLootbox(Serial serial) : base(serial)
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
