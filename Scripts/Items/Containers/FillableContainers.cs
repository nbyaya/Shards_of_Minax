using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Commands;
using System.Linq;

namespace Server.Items
{
    public enum FillableContentType
    {
        None = -1,
        Weaponsmith,
        Provisioner,
        Mage,
        Alchemist,
        Armorer,
        ArtisanGuild,
        Baker,
        Bard,
        Blacksmith,
        Bowyer,
        Butcher,
        Carpenter,
        Clothier,
        Cobbler,
        Docks,
        Farm,
        FighterGuild,
        Guard,
        Healer,
        Herbalist,
        Inn,
        Jeweler,
        Library,
        Merchant,
        Mill,
        Mine,
        Observatory,
        Painter,
        Ranger,
        Stables,
        Tanner,
        Tavern,
        ThiefGuild,
        Tinker,
        Veterinarian
    }

    public abstract class FillableContainer : LockableContainer
    {
        public static void Initialize()
        {
            CommandSystem.Register("CheckFillables", AccessLevel.Administrator, CheckFillables_OnCommand);
        }

        public static void CheckFillables_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            int count = 0;
            int fail = 0;

            List<FillableContainer> toCheck = new List<FillableContainer>(World.Items.Values.OfType<FillableContainer>().Where(i => i is FillableContainer && ((FillableContainer)i).ContentType == FillableContentType.None));

            foreach (FillableContainer cont in toCheck)
            {
                cont.AcquireContent();

                if (cont.ContentType == FillableContentType.None)
                    fail++;

                count++;
            }

            toCheck.Clear();
            toCheck.TrimExcess();
            m.SendMessage("Fixed {0} fillable containers, while {1} failed.", count, fail);
        }

        protected FillableContent m_Content;
        protected DateTime m_NextRespawnTime;
        protected Timer m_RespawnTimer;

        public FillableContainer(int itemID)
            : base(itemID)
        {
            Movable = false;

            MaxSpawnCount = Utility.RandomMinMax(3, 5);
        }

        public FillableContainer(Serial serial)
            : base(serial)
        {
        }

        public virtual int MinRespawnMinutes { get { return 5; } }
        public virtual int MaxRespawnMinutes { get { return 30; } }
        public virtual bool IsLockable { get { return true; } }
        public virtual bool IsTrapable { get { return IsLockable; } }
        public virtual int SpawnThreshold { get { return MaxSpawnCount - 1; } }

        public virtual int AmountPerSpawn { get { return 1; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxSpawnCount { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TotalTraps { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextRespawnTime 
        {
            get 
            {
                return m_NextRespawnTime; 
            }
            set
            {
                m_NextRespawnTime = value;

                if (m_NextRespawnTime > DateTime.UtcNow)
                {
                    TimeSpan delay = m_NextRespawnTime - DateTime.UtcNow;
                    m_RespawnTimer = Timer.DelayCall(delay, new TimerCallback(Respawn));
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public FillableContentType ContentType
        {
            get
            {
                return FillableContent.Lookup(m_Content);
            }
            set
            {
                Content = FillableContent.Lookup(value);
            }
        }

        public FillableContent Content
        {
            get
            {
                return m_Content;
            }
            set
            {
                if (m_Content == value)
                    return;

                m_Content = value;

                for (int i = Items.Count - 1; i >= 0; --i)
                {
                    if (i < Items.Count)
                        Items[i].Delete();
                }

                Respawn(true);
            }
        }

        public override void OnMapChange()
        {
            base.OnMapChange();
            AcquireContent();
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);
            AcquireContent();
        }

        public virtual void AcquireContent()
        {
            if (m_Content != null)
                return;

            m_Content = FillableContent.Acquire(GetWorldLocation(), Map);

            if (m_Content != null)
                Respawn();
        }

        public override void OnItemRemoved(Item item)
        {
            CheckRespawn();
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_RespawnTimer != null)
            {
                m_RespawnTimer.Stop();
                m_RespawnTimer = null;
            }
        }

        public int GetItemsCount()
        {
            int count = 0;

            foreach (Item item in Items)
            {
                count += item.Amount;
            }

            return count;
        }

        public void CheckRespawn()
        {
            bool canSpawn = (m_Content != null && !Deleted && GetItemsCount() <= SpawnThreshold && !Movable && Parent == null && !IsLockedDown && !IsSecure);

            if (canSpawn)
            {
                if (m_RespawnTimer == null)
                {
                    int mins = Utility.RandomMinMax(MinRespawnMinutes, MaxRespawnMinutes);
                    TimeSpan delay = TimeSpan.FromMinutes(mins);

                    m_NextRespawnTime = DateTime.UtcNow + delay;
                    m_RespawnTimer = Timer.DelayCall(delay, new TimerCallback(Respawn));
                }
            }
            else if (m_RespawnTimer != null)
            {
                m_RespawnTimer.Stop();
                m_RespawnTimer = null;
            }
        }

        public void Respawn()
        {
            Respawn(false);
        }

        public void Respawn(bool all)
        {
            if (m_RespawnTimer != null)
            {
                m_RespawnTimer.Stop();
                m_RespawnTimer = null;
            }

            if (m_Content == null || Deleted)
                return;

            GenerateContent(all);

            if (IsLockable && !Locked)
            {
                Locked = true;

                int difficulty = (m_Content.Level - 1) * 30;

                LockLevel = difficulty - 10;
                MaxLockLevel = difficulty + 30;
                RequiredSkill = difficulty;
            }

            if (IsTrapable && (m_Content.Level > 1 || 4 > Utility.Random(5)))
            {
                ResetTrap();

                TotalTraps = 1;

                if (0.25 > Utility.RandomDouble())
                {
                    TotalTraps++;

                    if (0.25 > Utility.RandomDouble())
                    {
                        TotalTraps++;
                    }
                }
            }
            else
            {
                TrapType = TrapType.None;
                TrapPower = 0;
                TrapLevel = 0;
            }

            CheckRespawn();
        }

        public virtual bool CanSpawnRefinement()
        {
            return Map == Map.Felucca && (ContentType == FillableContentType.Clothier || ContentType == FillableContentType.Blacksmith || ContentType == FillableContentType.Carpenter);
        }

		// Special Global Item List (Items that can spawn in any container)
		private static readonly FillableEntry[] SpecialItems = new FillableEntry[]
		{
			new FillableEntry(50, typeof(MaxxiaScroll)),      // Example item: Gold Coins
			new FillableEntry(50, typeof(MaxxiaScroll)),          // Example item: Random Gem
			new FillableEntry(50, typeof(MaxxiaScroll)),   // Example item: Special Scroll
			new FillableEntry(50, typeof(MaxxiaScroll)), // Example item: Some rare artifact
			new FillableEntry(1, typeof(RandomMagicWeapon)), // Adjust chance as needed
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomSkillJewelryA)),
			new FillableEntry(1, typeof(RandomSkillJewelryAA)),
			new FillableEntry(1, typeof(RandomSkillJewelryAB)),
			new FillableEntry(1, typeof(RandomSkillJewelryAC)),
			new FillableEntry(1, typeof(RandomSkillJewelryAD)),
			new FillableEntry(1, typeof(RandomSkillJewelryAE)),
			new FillableEntry(1, typeof(RandomSkillJewelryAF)),
			new FillableEntry(1, typeof(RandomSkillJewelryAG)),
			new FillableEntry(1, typeof(RandomSkillJewelryAH)),
			new FillableEntry(1, typeof(RandomSkillJewelryAI)),
			new FillableEntry(1, typeof(RandomSkillJewelryAJ)),
			new FillableEntry(1, typeof(RandomSkillJewelryAK)),
			new FillableEntry(1, typeof(RandomSkillJewelryAL)),
			new FillableEntry(1, typeof(RandomSkillJewelryAM)),
			new FillableEntry(1, typeof(RandomSkillJewelryAN)),
			new FillableEntry(1, typeof(RandomSkillJewelryAO)),
			new FillableEntry(1, typeof(RandomSkillJewelryAP)),
			new FillableEntry(1, typeof(RandomSkillJewelryB)),
			new FillableEntry(1, typeof(RandomSkillJewelryC)),
			new FillableEntry(1, typeof(RandomSkillJewelryD)),
			new FillableEntry(1, typeof(RandomSkillJewelryE)),
			new FillableEntry(1, typeof(RandomSkillJewelryF)),
			new FillableEntry(1, typeof(RandomSkillJewelryG)),
			new FillableEntry(1, typeof(RandomSkillJewelryH)),
			new FillableEntry(1, typeof(RandomSkillJewelryI)),
			new FillableEntry(1, typeof(RandomSkillJewelryJ)),
			new FillableEntry(1, typeof(RandomSkillJewelryK)),
			new FillableEntry(1, typeof(RandomSkillJewelryL)),
			new FillableEntry(1, typeof(RandomSkillJewelryM)),
			new FillableEntry(1, typeof(RandomSkillJewelryN)),
			new FillableEntry(1, typeof(RandomSkillJewelryO)),
			new FillableEntry(1, typeof(RandomSkillJewelryP)),
			new FillableEntry(1, typeof(RandomSkillJewelryQ)),
			new FillableEntry(1, typeof(RandomSkillJewelryR)),
			new FillableEntry(1, typeof(RandomSkillJewelryS)),
			new FillableEntry(1, typeof(RandomSkillJewelryT)),
			new FillableEntry(1, typeof(RandomSkillJewelryU)),
			new FillableEntry(1, typeof(RandomSkillJewelryV)),
			new FillableEntry(1, typeof(RandomSkillJewelryW)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomSkillJewelryY)),
			new FillableEntry(1, typeof(RandomSkillJewelryZ)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)), // Adjust chance as needed
			new FillableEntry(1, typeof(AlchemyAugmentCrystal)),
			new FillableEntry(1, typeof(AnatomyAugmentCrystal)),
			new FillableEntry(1, typeof(AnimalLoreAugmentCrystal)),
			new FillableEntry(1, typeof(AnimalTamingAugmentCrystal)),
			new FillableEntry(1, typeof(ArcheryAugmentCrystal)),
			new FillableEntry(1, typeof(ArmsLoreAugmentCrystal)),
			new FillableEntry(1, typeof(ArmSlotChangeDeed)),
			new FillableEntry(1, typeof(BagOfBombs)),
			new FillableEntry(1, typeof(BagOfHealth)),
			new FillableEntry(1, typeof(BagOfJuice)),
			new FillableEntry(1, typeof(BanishingOrb)),
			new FillableEntry(1, typeof(BanishingRod)),
			new FillableEntry(1, typeof(BeggingAugmentCrystal)),
			new FillableEntry(1, typeof(BeltSlotChangeDeed)),
			new FillableEntry(1, typeof(BlacksmithyAugmentCrystal)),
			new FillableEntry(1, typeof(BloodSword)),
			new FillableEntry(1, typeof(BootsOfCommand)),
			new FillableEntry(1, typeof(BraceletSlotChangeDeed)),
			new FillableEntry(1, typeof(BushidoAugmentCrystal)),
			new FillableEntry(1, typeof(CampingAugmentCrystal)),
			new FillableEntry(1, typeof(CapacityIncreaseDeed)),
			new FillableEntry(1, typeof(CarpentryAugmentCrystal)),
			new FillableEntry(1, typeof(CartographyAugmentCrystal)),
			new FillableEntry(1, typeof(ChestSlotChangeDeed)),
			new FillableEntry(1, typeof(ChivalryAugmentCrystal)),
			new FillableEntry(1, typeof(ColdHitAreaCrystal)),
			new FillableEntry(1, typeof(ColdResistAugmentCrystal)),
			new FillableEntry(1, typeof(CookingAugmentCrystal)),
			new FillableEntry(1, typeof(CurseAugmentCrystal)),
			new FillableEntry(1, typeof(DetectingHiddenAugmentCrystal)),
			new FillableEntry(1, typeof(DiscordanceAugmentCrystal)),
			new FillableEntry(1, typeof(DispelAugmentCrystal)),
			new FillableEntry(1, typeof(EarringSlotChangeDeed)),
			new FillableEntry(1, typeof(EnergyHitAreaCrystal)),
			new FillableEntry(1, typeof(EnergyResistAugmentCrystal)),
			new FillableEntry(1, typeof(FatigueAugmentCrystal)),
			new FillableEntry(1, typeof(FencingAugmentCrystal)),
			new FillableEntry(1, typeof(FireballAugmentCrystal)),
			new FillableEntry(1, typeof(FireHitAreaCrystal)),
			new FillableEntry(1, typeof(FireResistAugmentCrystal)),
			new FillableEntry(1, typeof(FishingAugmentCrystal)),
			new FillableEntry(1, typeof(FletchingAugmentCrystal)),
			new FillableEntry(1, typeof(FocusAugmentCrystal)),
			new FillableEntry(1, typeof(FootwearSlotChangeDeed)),
			new FillableEntry(1, typeof(ForensicEvaluationAugmentCrystal)),
			new FillableEntry(1, typeof(GlovesOfCommand)),
			new FillableEntry(1, typeof(HarmAugmentCrystal)),
			new FillableEntry(1, typeof(HeadSlotChangeDeed)),
			new FillableEntry(1, typeof(HealingAugmentCrystal)),
			new FillableEntry(1, typeof(HerdingAugmentCrystal)),
			new FillableEntry(1, typeof(HidingAugmentCrystal)),
			new FillableEntry(1, typeof(ImbuingAugmentCrystal)),
			new FillableEntry(1, typeof(InscriptionAugmentCrystal)),
			new FillableEntry(1, typeof(ItemIdentificationAugmentCrystal)),
			new FillableEntry(1, typeof(JesterHatOfCommand)),
			new FillableEntry(1, typeof(LegsSlotChangeDeed)),
			new FillableEntry(1, typeof(LifeLeechAugmentCrystal)),
			new FillableEntry(1, typeof(LightningAugmentCrystal)),
			new FillableEntry(1, typeof(LockpickingAugmentCrystal)),
			new FillableEntry(1, typeof(LowerAttackAugmentCrystal)),
			new FillableEntry(1, typeof(LuckAugmentCrystal)),
			new FillableEntry(1, typeof(LumberjackingAugmentCrystal)),
			new FillableEntry(1, typeof(MaceFightingAugmentCrystal)),
			new FillableEntry(1, typeof(MageryAugmentCrystal)),
			new FillableEntry(1, typeof(ManaDrainAugmentCrystal)),
			new FillableEntry(1, typeof(ManaLeechAugmentCrystal)),
			new FillableEntry(1, typeof(MaxxiaScroll)),
			new FillableEntry(1, typeof(MeditationAugmentCrystal)),
			new FillableEntry(1, typeof(MiningAugmentCrystal)),
			new FillableEntry(1, typeof(MirrorOfKalandra)),
			new FillableEntry(1, typeof(MusicianshipAugmentCrystal)),
			new FillableEntry(1, typeof(NeckSlotChangeDeed)),
			new FillableEntry(1, typeof(NecromancyAugmentCrystal)),
			new FillableEntry(1, typeof(NinjitsuAugmentCrystal)),
			new FillableEntry(1, typeof(OneHandedTransformDeed)),
			new FillableEntry(1, typeof(ParryingAugmentCrystal)),
			new FillableEntry(1, typeof(PeacemakingAugmentCrystal)),
			new FillableEntry(1, typeof(PhysicalHitAreaCrystal)),
			new FillableEntry(1, typeof(PhysicalResistAugmentCrystal)),
			new FillableEntry(1, typeof(PlateLeggingsOfCommand)),
			new FillableEntry(1, typeof(PoisonHitAreaCrystal)),
			new FillableEntry(1, typeof(PoisoningAugmentCrystal)),
			new FillableEntry(1, typeof(PoisonResistAugmentCrystal)),
			new FillableEntry(1, typeof(ProvocationAugmentCrystal)),
			new FillableEntry(1, typeof(RemoveTrapAugmentCrystal)),
			new FillableEntry(1, typeof(ResistingSpellsAugmentCrystal)),
			new FillableEntry(1, typeof(RingSlotChangeDeed)),
			new FillableEntry(1, typeof(RodOfOrcControl)),
			new FillableEntry(1, typeof(ShirtSlotChangeDeed)),
			new FillableEntry(1, typeof(SnoopingAugmentCrystal)),
			new FillableEntry(1, typeof(SpellweavingAugmentCrystal)),
			new FillableEntry(1, typeof(SpiritSpeakAugmentCrystal)),
			new FillableEntry(1, typeof(StaminaLeechAugmentCrystal)),
			new FillableEntry(1, typeof(StealingAugmentCrystal)),
			new FillableEntry(1, typeof(StealthAugmentCrystal)),
			new FillableEntry(1, typeof(SwingSpeedAugmentCrystal)),
			new FillableEntry(1, typeof(SwordsmanshipAugmentCrystal)),
			new FillableEntry(1, typeof(TacticsAugmentCrystal)),
			new FillableEntry(1, typeof(TailoringAugmentCrystal)),
			new FillableEntry(1, typeof(TalismanSlotChangeDeed)),
			new FillableEntry(1, typeof(TasteIDAugmentCrystal)),
			new FillableEntry(1, typeof(ThrowingAugmentCrystal)),
			new FillableEntry(1, typeof(TinkeringAugmentCrystal)),
			new FillableEntry(1, typeof(TrackingAugmentCrystal)),
			new FillableEntry(1, typeof(VeterinaryAugmentCrystal)),
			new FillableEntry(1, typeof(WeaponSpeedAugmentCrystal)),
			new FillableEntry(1, typeof(WrestlingAugmentCrystal)),
			new FillableEntry(1, typeof(PetSlotDeed)),
			new FillableEntry(1, typeof(PetBondDeed)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(RandomMagicWeapon)), // Adjust chance as needed
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicClothing)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomMagicWeapon)),
			new FillableEntry(1, typeof(RandomSkillJewelryA)),
			new FillableEntry(1, typeof(RandomSkillJewelryAA)),
			new FillableEntry(1, typeof(RandomSkillJewelryAB)),
			new FillableEntry(1, typeof(RandomSkillJewelryAC)),
			new FillableEntry(1, typeof(RandomSkillJewelryAD)),
			new FillableEntry(1, typeof(RandomSkillJewelryAE)),
			new FillableEntry(1, typeof(RandomSkillJewelryAF)),
			new FillableEntry(1, typeof(RandomSkillJewelryAG)),
			new FillableEntry(1, typeof(RandomSkillJewelryAH)),
			new FillableEntry(1, typeof(RandomSkillJewelryAI)),
			new FillableEntry(1, typeof(RandomSkillJewelryAJ)),
			new FillableEntry(1, typeof(RandomSkillJewelryAK)),
			new FillableEntry(1, typeof(RandomSkillJewelryAL)),
			new FillableEntry(1, typeof(RandomSkillJewelryAM)),
			new FillableEntry(1, typeof(RandomSkillJewelryAN)),
			new FillableEntry(1, typeof(RandomSkillJewelryAO)),
			new FillableEntry(1, typeof(RandomSkillJewelryAP)),
			new FillableEntry(1, typeof(RandomSkillJewelryB)),
			new FillableEntry(1, typeof(RandomSkillJewelryC)),
			new FillableEntry(1, typeof(RandomSkillJewelryD)),
			new FillableEntry(1, typeof(RandomSkillJewelryE)),
			new FillableEntry(1, typeof(RandomSkillJewelryF)),
			new FillableEntry(1, typeof(RandomSkillJewelryG)),
			new FillableEntry(1, typeof(RandomSkillJewelryH)),
			new FillableEntry(1, typeof(RandomSkillJewelryI)),
			new FillableEntry(1, typeof(RandomSkillJewelryJ)),
			new FillableEntry(1, typeof(RandomSkillJewelryK)),
			new FillableEntry(1, typeof(RandomSkillJewelryL)),
			new FillableEntry(1, typeof(RandomSkillJewelryM)),
			new FillableEntry(1, typeof(RandomSkillJewelryN)),
			new FillableEntry(1, typeof(RandomSkillJewelryO)),
			new FillableEntry(1, typeof(RandomSkillJewelryP)),
			new FillableEntry(1, typeof(RandomSkillJewelryQ)),
			new FillableEntry(1, typeof(RandomSkillJewelryR)),
			new FillableEntry(1, typeof(RandomSkillJewelryS)),
			new FillableEntry(1, typeof(RandomSkillJewelryT)),
			new FillableEntry(1, typeof(RandomSkillJewelryU)),
			new FillableEntry(1, typeof(RandomSkillJewelryV)),
			new FillableEntry(1, typeof(RandomSkillJewelryW)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomSkillJewelryY)),
			new FillableEntry(1, typeof(RandomSkillJewelryZ)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicJewelry)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)),
			new FillableEntry(1, typeof(RandomMagicArmor)), // Adjust chance as needed
			new FillableEntry(1, typeof(AlchemyAugmentCrystal)),
			new FillableEntry(1, typeof(AnatomyAugmentCrystal)),
			new FillableEntry(1, typeof(AnimalLoreAugmentCrystal)),
			new FillableEntry(1, typeof(AnimalTamingAugmentCrystal)),
			new FillableEntry(1, typeof(ArcheryAugmentCrystal)),
			new FillableEntry(1, typeof(ArmsLoreAugmentCrystal)),
			new FillableEntry(1, typeof(ArmSlotChangeDeed)),
			new FillableEntry(1, typeof(BagOfBombs)),
			new FillableEntry(1, typeof(BagOfHealth)),
			new FillableEntry(1, typeof(BagOfJuice)),
			new FillableEntry(1, typeof(BanishingOrb)),
			new FillableEntry(1, typeof(BanishingRod)),
			new FillableEntry(1, typeof(BeggingAugmentCrystal)),
			new FillableEntry(1, typeof(BeltSlotChangeDeed)),
			new FillableEntry(1, typeof(BlacksmithyAugmentCrystal)),
			new FillableEntry(1, typeof(BloodSword)),
			new FillableEntry(1, typeof(BootsOfCommand)),
			new FillableEntry(1, typeof(BraceletSlotChangeDeed)),
			new FillableEntry(1, typeof(BushidoAugmentCrystal)),
			new FillableEntry(1, typeof(CampingAugmentCrystal)),
			new FillableEntry(1, typeof(CapacityIncreaseDeed)),
			new FillableEntry(1, typeof(CarpentryAugmentCrystal)),
			new FillableEntry(1, typeof(CartographyAugmentCrystal)),
			new FillableEntry(1, typeof(ChestSlotChangeDeed)),
			new FillableEntry(1, typeof(ChivalryAugmentCrystal)),
			new FillableEntry(1, typeof(ColdHitAreaCrystal)),
			new FillableEntry(1, typeof(ColdResistAugmentCrystal)),
			new FillableEntry(1, typeof(CookingAugmentCrystal)),
			new FillableEntry(1, typeof(CurseAugmentCrystal)),
			new FillableEntry(1, typeof(DetectingHiddenAugmentCrystal)),
			new FillableEntry(1, typeof(DiscordanceAugmentCrystal)),
			new FillableEntry(1, typeof(DispelAugmentCrystal)),
			new FillableEntry(1, typeof(EarringSlotChangeDeed)),
			new FillableEntry(1, typeof(EnergyHitAreaCrystal)),
			new FillableEntry(1, typeof(EnergyResistAugmentCrystal)),
			new FillableEntry(1, typeof(FatigueAugmentCrystal)),
			new FillableEntry(1, typeof(FencingAugmentCrystal)),
			new FillableEntry(1, typeof(FireballAugmentCrystal)),
			new FillableEntry(1, typeof(FireHitAreaCrystal)),
			new FillableEntry(1, typeof(FireResistAugmentCrystal)),
			new FillableEntry(1, typeof(FishingAugmentCrystal)),
			new FillableEntry(1, typeof(FletchingAugmentCrystal)),
			new FillableEntry(1, typeof(FocusAugmentCrystal)),
			new FillableEntry(1, typeof(FootwearSlotChangeDeed)),
			new FillableEntry(1, typeof(ForensicEvaluationAugmentCrystal)),
			new FillableEntry(1, typeof(GlovesOfCommand)),
			new FillableEntry(1, typeof(HarmAugmentCrystal)),
			new FillableEntry(1, typeof(HeadSlotChangeDeed)),
			new FillableEntry(1, typeof(HealingAugmentCrystal)),
			new FillableEntry(1, typeof(HerdingAugmentCrystal)),
			new FillableEntry(1, typeof(HidingAugmentCrystal)),
			new FillableEntry(1, typeof(ImbuingAugmentCrystal)),
			new FillableEntry(1, typeof(InscriptionAugmentCrystal)),
			new FillableEntry(1, typeof(ItemIdentificationAugmentCrystal)),
			new FillableEntry(1, typeof(JesterHatOfCommand)),
			new FillableEntry(1, typeof(LegsSlotChangeDeed)),
			new FillableEntry(1, typeof(LifeLeechAugmentCrystal)),
			new FillableEntry(1, typeof(LightningAugmentCrystal)),
			new FillableEntry(1, typeof(LockpickingAugmentCrystal)),
			new FillableEntry(1, typeof(LowerAttackAugmentCrystal)),
			new FillableEntry(1, typeof(LuckAugmentCrystal)),
			new FillableEntry(1, typeof(LumberjackingAugmentCrystal)),
			new FillableEntry(1, typeof(MaceFightingAugmentCrystal)),
			new FillableEntry(1, typeof(MageryAugmentCrystal)),
			new FillableEntry(1, typeof(ManaDrainAugmentCrystal)),
			new FillableEntry(1, typeof(ManaLeechAugmentCrystal)),
			new FillableEntry(1, typeof(MaxxiaScroll)),
			new FillableEntry(1, typeof(MeditationAugmentCrystal)),
			new FillableEntry(1, typeof(MiningAugmentCrystal)),
			new FillableEntry(1, typeof(MirrorOfKalandra)),
			new FillableEntry(1, typeof(MusicianshipAugmentCrystal)),
			new FillableEntry(1, typeof(NeckSlotChangeDeed)),
			new FillableEntry(1, typeof(NecromancyAugmentCrystal)),
			new FillableEntry(1, typeof(NinjitsuAugmentCrystal)),
			new FillableEntry(1, typeof(OneHandedTransformDeed)),
			new FillableEntry(1, typeof(ParryingAugmentCrystal)),
			new FillableEntry(1, typeof(PeacemakingAugmentCrystal)),
			new FillableEntry(1, typeof(PhysicalHitAreaCrystal)),
			new FillableEntry(1, typeof(PhysicalResistAugmentCrystal)),
			new FillableEntry(1, typeof(PlateLeggingsOfCommand)),
			new FillableEntry(1, typeof(PoisonHitAreaCrystal)),
			new FillableEntry(1, typeof(PoisoningAugmentCrystal)),
			new FillableEntry(1, typeof(PoisonResistAugmentCrystal)),
			new FillableEntry(1, typeof(ProvocationAugmentCrystal)),
			new FillableEntry(1, typeof(RemoveTrapAugmentCrystal)),
			new FillableEntry(1, typeof(ResistingSpellsAugmentCrystal)),
			new FillableEntry(1, typeof(RingSlotChangeDeed)),
			new FillableEntry(1, typeof(RodOfOrcControl)),
			new FillableEntry(1, typeof(ShirtSlotChangeDeed)),
			new FillableEntry(1, typeof(SnoopingAugmentCrystal)),
			new FillableEntry(1, typeof(SpellweavingAugmentCrystal)),
			new FillableEntry(1, typeof(SpiritSpeakAugmentCrystal)),
			new FillableEntry(1, typeof(StaminaLeechAugmentCrystal)),
			new FillableEntry(1, typeof(StealingAugmentCrystal)),
			new FillableEntry(1, typeof(StealthAugmentCrystal)),
			new FillableEntry(1, typeof(SwingSpeedAugmentCrystal)),
			new FillableEntry(1, typeof(SwordsmanshipAugmentCrystal)),
			new FillableEntry(1, typeof(TacticsAugmentCrystal)),
			new FillableEntry(1, typeof(TailoringAugmentCrystal)),
			new FillableEntry(1, typeof(TalismanSlotChangeDeed)),
			new FillableEntry(1, typeof(TasteIDAugmentCrystal)),
			new FillableEntry(1, typeof(ThrowingAugmentCrystal)),
			new FillableEntry(1, typeof(TinkeringAugmentCrystal)),
			new FillableEntry(1, typeof(TrackingAugmentCrystal)),
			new FillableEntry(1, typeof(VeterinaryAugmentCrystal)),
			new FillableEntry(1, typeof(WeaponSpeedAugmentCrystal)),
			new FillableEntry(1, typeof(WrestlingAugmentCrystal)),
			new FillableEntry(1, typeof(PetSlotDeed)),
			new FillableEntry(1, typeof(PetBondDeed)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),
			new FillableEntry(1, typeof(StatCapOrb)),
			new FillableEntry(1, typeof(SkillOrb)),			
			new FillableEntry(1, typeof(AbysmalHorrorSummoningMateria)),
			new FillableEntry(1, typeof(AcidElementalSummoningMateria)),
			new FillableEntry(1, typeof(AgapiteElementalSummoningMateria)),
			new FillableEntry(1, typeof(AirElementalSummoningMateria)),
			new FillableEntry(1, typeof(AlligatorSummoningMateria)),
			new FillableEntry(1, typeof(AncientLichSummoningMateria)),
			new FillableEntry(1, typeof(AncientWyrmSummoningMateria)),
			new FillableEntry(1, typeof(AntLionSummoningMateria)),
			new FillableEntry(1, typeof(ArcaneDaemonSummoningMateria)),
			new FillableEntry(1, typeof(ArcticOgreLordSummoningMateria)),
			new FillableEntry(1, typeof(AxeBreathMateria)),
			new FillableEntry(1, typeof(AxeCircleMateria)),
			new FillableEntry(1, typeof(AxeLineMateria)),
			new FillableEntry(1, typeof(BakeKitsuneSummoningMateria)),
			new FillableEntry(1, typeof(BalronSummoningMateria)),
			new FillableEntry(1, typeof(BarracoonSummoningMateria)),
			new FillableEntry(1, typeof(BeeBreathMateria)),
			new FillableEntry(1, typeof(BeeCircleMateria)),
			new FillableEntry(1, typeof(BeeLineMateria)),
			new FillableEntry(1, typeof(BeetleSummoningMateria)),
			new FillableEntry(1, typeof(BlackBearSummoningMateria)),
			new FillableEntry(1, typeof(BlackDragoonPirateMateria)),
			new FillableEntry(1, typeof(BlackSolenInfiltratorQueenSummoningMateria)),
			new FillableEntry(1, typeof(BlackSolenInfiltratorWarriorMateria)),
			new FillableEntry(1, typeof(BlackSolenQueenSummoningMateria)),
			new FillableEntry(1, typeof(BlackSolenWarriorSummoningMateria)),
			new FillableEntry(1, typeof(BlackSolenWorkerSummoningMateria)),
			new FillableEntry(1, typeof(BladesBreathMateria)),
			new FillableEntry(1, typeof(BladesCircleMateria)),
			new FillableEntry(1, typeof(BladesLineMateria)),
			new FillableEntry(1, typeof(BloodElementalSummoningGem)),
			new FillableEntry(1, typeof(BloodSwarmGem)),
			new FillableEntry(1, typeof(BoarSummoningMateria)),
			new FillableEntry(1, typeof(BogleSummoningMateria)),
			new FillableEntry(1, typeof(BoglingSummoningMateria)),
			new FillableEntry(1, typeof(BogThingSummoningMateria)),
			new FillableEntry(1, typeof(BoneDemonSummoningMateria)),
			new FillableEntry(1, typeof(BoneKnightSummoningMateria)),
			new FillableEntry(1, typeof(BoneMagiSummoningMateria)),
			new FillableEntry(1, typeof(BoulderBreathMateria)),
			new FillableEntry(1, typeof(BoulderCircleMateria)),
			new FillableEntry(1, typeof(BoulderLineMateria)),
			new FillableEntry(1, typeof(BrigandSummoningMateria)),
			new FillableEntry(1, typeof(BronzeElementalSummoningMateria)),
			new FillableEntry(1, typeof(BrownBearSummoningMateria)),
			new FillableEntry(1, typeof(BullFrogSummoningMateria)),
			new FillableEntry(1, typeof(BullSummoningMateria)),
			new FillableEntry(1, typeof(CatSummoningMateria)),
			new FillableEntry(1, typeof(CentaurSummoningMateria)),
			new FillableEntry(1, typeof(ChaosDaemonSummoningMateria)),
			new FillableEntry(1, typeof(ChaosDragoonEliteSummoningMateria)),
			new FillableEntry(1, typeof(ChaosDragoonSummoningMateria)),
			new FillableEntry(1, typeof(ChickenSummoningMateria)),
			new FillableEntry(1, typeof(CopperElementalSummoningMateria)),
			new FillableEntry(1, typeof(CorpserSummoningMateria)),
			new FillableEntry(1, typeof(CorrosiveSlimeSummoningMateria)),
			new FillableEntry(1, typeof(CorruptedSoulMateria)),
			new FillableEntry(1, typeof(CougarSummoningMateria)),
			new FillableEntry(1, typeof(CowSummoningMateria)),
			new FillableEntry(1, typeof(CraneSummoningMateria)),
			new FillableEntry(1, typeof(CrankBreathMateria)),
			new FillableEntry(1, typeof(CrankCircleMateria)),
			new FillableEntry(1, typeof(CrankLineMateria)),
			new FillableEntry(1, typeof(CrimsonDragonSummoningMateria)),
			new FillableEntry(1, typeof(CrystalElementalSummoningMateria)),
			new FillableEntry(1, typeof(CurtainBreathMateria)),
			new FillableEntry(1, typeof(CurtainCircleMateria)),
			new FillableEntry(1, typeof(CurtainLineMateria)),
			new FillableEntry(1, typeof(CuSidheSummoningMateria)),
			new FillableEntry(1, typeof(CyclopsSummoningMateria)),
			new FillableEntry(1, typeof(DaemonSummoningMateria)),
			new FillableEntry(1, typeof(DarkWispSummoningMateria)),
			new FillableEntry(1, typeof(DarkWolfSummoningMateria)),
			new FillableEntry(1, typeof(DeathWatchBeetleSummoningMateria)),
			new FillableEntry(1, typeof(DeepSeaSerpentSummoningMateria)),
			new FillableEntry(1, typeof(DeerBreathMateria)),
			new FillableEntry(1, typeof(DeerCircleMateria)),
			new FillableEntry(1, typeof(DeerLineMateria)),
			new FillableEntry(1, typeof(DemonKnightSummoningMateria)),
			new FillableEntry(1, typeof(DesertOstardSummoningMateria)),
			new FillableEntry(1, typeof(DevourerSummoningMateria)),
			new FillableEntry(1, typeof(DireWolfSummoningMateria)),
			new FillableEntry(1, typeof(DogSummoningMateria)),
			new FillableEntry(1, typeof(DolphinSummoningMateria)),
			new FillableEntry(1, typeof(DopplegangerSummoningMateria)),
			new FillableEntry(1, typeof(DragonSummoningMateria)),
			new FillableEntry(1, typeof(DrakeSummoningMateria)),
			new FillableEntry(1, typeof(DreadSpiderSummoningMateria)),
			new FillableEntry(1, typeof(DullCopperElementalSummoningMateria)),
			new FillableEntry(1, typeof(DVortexBreathMateria)),
			new FillableEntry(1, typeof(DVortexCircleMateria)),
			new FillableEntry(1, typeof(DVortexLineMateria)),
			new FillableEntry(1, typeof(EagleSummoningMateria)),
			new FillableEntry(1, typeof(EarthElementalSummoningMateria)),
			new FillableEntry(1, typeof(EfreetSummoningMateria)),
			new FillableEntry(1, typeof(ElderGazerSummoningMateria)),
			new FillableEntry(1, typeof(EliteNinjaSummoningMateria)),
			new FillableEntry(1, typeof(EttinSummoningMateria)),
			new FillableEntry(1, typeof(EvilHealerSummoningMateria)),
			new FillableEntry(1, typeof(EvilMageSummoningMateria)),
			new FillableEntry(1, typeof(ExecutionerMateria)),
			new FillableEntry(1, typeof(ExodusMinionSummoningMateria)),
			new FillableEntry(1, typeof(ExodusOverseerSummoningMateria)),
			new FillableEntry(1, typeof(FanDancerSummoningMateria)),
			new FillableEntry(1, typeof(FeralTreefellowSummoningMateria)),
			new FillableEntry(1, typeof(FetidEssenceMateria)),
			new FillableEntry(1, typeof(FireBeetleSummoningMateria)),
			new FillableEntry(1, typeof(FireElementalSummoningMateria)),
			new FillableEntry(1, typeof(FireGargoyleSummoningMateria)),
			new FillableEntry(1, typeof(FireSteedSummoningMateria)),
			new FillableEntry(1, typeof(FlaskBreathMateria)),
			new FillableEntry(1, typeof(FlaskCircleMateria)),
			new FillableEntry(1, typeof(FlaskLineMateria)),
			new FillableEntry(1, typeof(FleshGolemSummoningMateria)),
			new FillableEntry(1, typeof(FleshRendererSummoningMateria)),
			new FillableEntry(1, typeof(ForestOstardSummoningMateria)),
			new FillableEntry(1, typeof(FrenziedOstardSummoningMateria)),
			new FillableEntry(1, typeof(FrostOozeSummoningMateria)),
			new FillableEntry(1, typeof(FrostSpiderSummoningMateria)),
			new FillableEntry(1, typeof(FrostTrollSummoningMateria)),
			new FillableEntry(1, typeof(FTreeCircleMateria)),
			new FillableEntry(1, typeof(FTreeLineMateria)),
			new FillableEntry(1, typeof(GamanSummoningMateria)),
			new FillableEntry(1, typeof(GargoyleSummoningMateria)),
			new FillableEntry(1, typeof(GasBreathMateria)),
			new FillableEntry(1, typeof(GasCircleMateria)),
			new FillableEntry(1, typeof(GasLineMateria)),
			new FillableEntry(1, typeof(GateBreathMateria)),
			new FillableEntry(1, typeof(GateCircleMateria)),
			new FillableEntry(1, typeof(GateLineMateria)),
			new FillableEntry(1, typeof(GazerSummoningMateria)),
			new FillableEntry(1, typeof(GhoulSummoningMateria)),
			new FillableEntry(1, typeof(GiantBlackWidowSummoningMateria)),
			new FillableEntry(1, typeof(GiantRatSummoningMateria)),
			new FillableEntry(1, typeof(GiantSerpentSummoningMateria)),
			new FillableEntry(1, typeof(GiantSpiderSummoningMateria)),
			new FillableEntry(1, typeof(GiantToadSummoningMateria)),
			new FillableEntry(1, typeof(GibberlingSummoningMateria)),
			new FillableEntry(1, typeof(GlowBreathMateria)),
			new FillableEntry(1, typeof(GlowCircleMateria)),
			new FillableEntry(1, typeof(GlowLineMateria)),
			new FillableEntry(1, typeof(GoatSummoningMateria)),
			new FillableEntry(1, typeof(GoldenElementalSummoningMateria)),
			new FillableEntry(1, typeof(GolemSummoningMateria)),
			new FillableEntry(1, typeof(GoreFiendSummoningMateria)),
			new FillableEntry(1, typeof(GorillaSummoningMateria)),
			new FillableEntry(1, typeof(GreaterDragonSummoningMateria)),
			new FillableEntry(1, typeof(GreaterMongbatSummoningMateria)),
			new FillableEntry(1, typeof(GreatHartSummoningMateria)),
			new FillableEntry(1, typeof(GreyWolfSummoningMateria)),
			new FillableEntry(1, typeof(GrizzlyBearSummoningMateria)),
			new FillableEntry(1, typeof(GuillotineBreathMateria)),
			new FillableEntry(1, typeof(GuillotineCircleMateria)),
			new FillableEntry(1, typeof(GuillotineLineMateria)),
			new FillableEntry(1, typeof(HarpySummoningMateria)),
			new FillableEntry(1, typeof(HeadBreathMateria)),
			new FillableEntry(1, typeof(HeadCircleMateria)),
			new FillableEntry(1, typeof(HeadlessOneSummoningMateria)),
			new FillableEntry(1, typeof(HeadLineMateria)),
			new FillableEntry(1, typeof(HealerMateria)),
			new FillableEntry(1, typeof(HeartBreathMateria)),
			new FillableEntry(1, typeof(HeartCircleMateria)),
			new FillableEntry(1, typeof(HeartLineMateria)),
			new FillableEntry(1, typeof(HellCatSummoningMateria)),
			new FillableEntry(1, typeof(HellHoundSummoningMateria)),
			new FillableEntry(1, typeof(HellSteedSummoningMateria)),
			new FillableEntry(1, typeof(HindSummoningMateria)),
			new FillableEntry(1, typeof(HiryuSummoningMateria)),
			new FillableEntry(1, typeof(HorseSummoningMateria)),
			new FillableEntry(1, typeof(IceElementalSummoningMateria)),
			new FillableEntry(1, typeof(IceFiendSummoningMateria)),
			new FillableEntry(1, typeof(IceSerpentSummoningMateria)),
			new FillableEntry(1, typeof(IceSnakeSummoningMateria)),
			new FillableEntry(1, typeof(ImpSummoningMateria)),
			new FillableEntry(1, typeof(JackRabbitSummoningMateria)),
			new FillableEntry(1, typeof(KazeKemonoSummoningMateria)),
			new FillableEntry(1, typeof(KirinSummoningMateria)),
			new FillableEntry(1, typeof(LavaLizardSummoningMateria)),
			new FillableEntry(1, typeof(LavaSerpentSummoningMateria)),
			new FillableEntry(1, typeof(LavaSnakeSummoningMateria)),
			new FillableEntry(1, typeof(LesserHiryuSummoningMateria)),
			new FillableEntry(1, typeof(LichLordSummoningMateria)),
			new FillableEntry(1, typeof(LichSummoningMateria)),
			new FillableEntry(1, typeof(LizardmanSummoningMateria)),
			new FillableEntry(1, typeof(LlamaSummoningMateria)),
			new FillableEntry(1, typeof(MaidenBreathMateria)),
			new FillableEntry(1, typeof(MaidenCircleMateria)),
			new FillableEntry(1, typeof(MaidenLineMateria)),
			new FillableEntry(1, typeof(MinotaurCaptainSummoningMateria)),
			new FillableEntry(1, typeof(MountainGoatSummoningMateria)),
			new FillableEntry(1, typeof(MummySummoningMateria)),
			new FillableEntry(1, typeof(MushroomBreathMateria)),
			new FillableEntry(1, typeof(MushroomCircleMateria)),
			new FillableEntry(1, typeof(MushroomLineMateria)),
			new FillableEntry(1, typeof(NightmareSummoningMateria)),
			new FillableEntry(1, typeof(NutcrackerBreathMateria)),
			new FillableEntry(1, typeof(NutcrackerCircleMateria)),
			new FillableEntry(1, typeof(NutcrackerLineMateria)),
			new FillableEntry(1, typeof(OFlaskBreathMateria)),
			new FillableEntry(1, typeof(OFlaskCircleMateria)),
			new FillableEntry(1, typeof(OFlaskMateria)),
			new FillableEntry(1, typeof(OgreLordSummoningMateria)),
			new FillableEntry(1, typeof(OgreSummoningMateria)),
			new FillableEntry(1, typeof(OniSummoningMateria)),
			new FillableEntry(1, typeof(OphidianArchmageSummoningMateria)),
			new FillableEntry(1, typeof(OphidianKnightSummoningMateria)),
			new FillableEntry(1, typeof(OrcBomberSummoningMateria)),
			new FillableEntry(1, typeof(OrcBruteSummoningMateria)),
			new FillableEntry(1, typeof(OrcCaptainSummoningMateria)),
			new FillableEntry(1, typeof(OrcishLordSummoningMateria)),
			new FillableEntry(1, typeof(OrcishMageSummoningMateria)),
			new FillableEntry(1, typeof(OrcSummoningMateria)),
			new FillableEntry(1, typeof(PackHorseSummoningMateria)),
			new FillableEntry(1, typeof(PackLlamaSummoningMateria)),
			new FillableEntry(1, typeof(PantherSummoningMateria)),
			new FillableEntry(1, typeof(ParaBreathMateria)),
			new FillableEntry(1, typeof(ParaCircleMateria)),
			new FillableEntry(1, typeof(ParaLineMateria)),
			new FillableEntry(1, typeof(PhoenixSummoningMateria)),
			new FillableEntry(1, typeof(PigSummoningMateria)),
			new FillableEntry(1, typeof(PixieSummoningMateria)),
			new FillableEntry(1, typeof(PlagueBeastSummoningMateria)),
			new FillableEntry(1, typeof(PoisonElementalSummoningMateria)),
			new FillableEntry(1, typeof(PolarBearSummoningMateria)),
			new FillableEntry(1, typeof(RabbitSummoningMateria)),
			new FillableEntry(1, typeof(RaiJuSummoningMateria)),
			new FillableEntry(1, typeof(RatmanArcherSummoningMateria)),
			new FillableEntry(1, typeof(RatmanMageSummoningMateria)),
			new FillableEntry(1, typeof(RatmanSummoningMateria)),
			new FillableEntry(1, typeof(RatSummoningMateria)),
			new FillableEntry(1, typeof(ReaperSummoningMateria)),
			new FillableEntry(1, typeof(RevenantSummoningMateria)),
			new FillableEntry(1, typeof(RidgebackSummoningMateria)),
			new FillableEntry(1, typeof(RikktorSummoningMateria)),
			new FillableEntry(1, typeof(RoninSummoningMateria)),
			new FillableEntry(1, typeof(RuneBeetleSummoningMateria)),
			new FillableEntry(1, typeof(RuneBreathMateria)),
			new FillableEntry(1, typeof(RuneCircleMateria)),
			new FillableEntry(1, typeof(RuneLineMateria)),
			new FillableEntry(1, typeof(SatyrSummoningMateria)),
			new FillableEntry(1, typeof(SavageShamanSummoningMateria)),
			new FillableEntry(1, typeof(SavageSummoningMateria)),
			new FillableEntry(1, typeof(SawBreathMateria)),
			new FillableEntry(1, typeof(SawCircleMateria)),
			new FillableEntry(1, typeof(SawLineMateria)),
			new FillableEntry(1, typeof(ScaledSwampDragonSummoningMateria)),
			new FillableEntry(1, typeof(ScorpionSummoningMateria)),
			new FillableEntry(1, typeof(SeaSerpentSummoningMateria)),
			new FillableEntry(1, typeof(ShadowWispSummoningMateria)),
			new FillableEntry(1, typeof(ShadowWyrmSummoningMateria)),
			new FillableEntry(1, typeof(SheepSummoningMateria)),
			new FillableEntry(1, typeof(SilverSerpentSummoningMateria)),
			new FillableEntry(1, typeof(SilverSteedSummoningMateria)),
			new FillableEntry(1, typeof(SkeletalDragonSummoningMateria)),
			new FillableEntry(1, typeof(SkeletalKnightSummoningMateria)),
			new FillableEntry(1, typeof(SkeletalMageSummoningMateria)),
			new FillableEntry(1, typeof(SkeletalMountSummoningMateria)),
			new FillableEntry(1, typeof(SkeletonBreathMateria)),
			new FillableEntry(1, typeof(SkeletonCircleMateria)),
			new FillableEntry(1, typeof(SkeletonLineMateria)),
			new FillableEntry(1, typeof(SkeletonSummoningMateria)),
			new FillableEntry(1, typeof(SkullBreathMateria)),
			new FillableEntry(1, typeof(SkullCircleMateria)),
			new FillableEntry(1, typeof(SkullLineMateria)),
			new FillableEntry(1, typeof(SlimeSummoningMateria)),
			new FillableEntry(1, typeof(SmokeBreathMateria)),
			new FillableEntry(1, typeof(SmokeCircleMateria)),
			new FillableEntry(1, typeof(SmokeLineMateria)),
			new FillableEntry(1, typeof(SnakeSummoningMateria)),
			new FillableEntry(1, typeof(SnowElementalSummoningMateria)),
			new FillableEntry(1, typeof(SnowLeopardSummoningMateria)),
			new FillableEntry(1, typeof(SocketDeed)),
			new FillableEntry(1, typeof(SocketDeed1)),
			new FillableEntry(1, typeof(SocketDeed2)),
			new FillableEntry(1, typeof(SocketDeed3)),
			new FillableEntry(1, typeof(SocketDeed4)),
			new FillableEntry(1, typeof(SocketDeed5)),
			new FillableEntry(1, typeof(SparkleBreathMateria)),
			new FillableEntry(1, typeof(SparkleCircleMateria)),
			new FillableEntry(1, typeof(SparkleLineMateria)),
			new FillableEntry(1, typeof(SpikeBreathMateria)),
			new FillableEntry(1, typeof(SpikeCircleMateria)),
			new FillableEntry(1, typeof(SpikeLineMateria)),
			new FillableEntry(1, typeof(StoneBreathMateria)),
			new FillableEntry(1, typeof(StoneCircleMateria)),
			new FillableEntry(1, typeof(StoneLineMateria)),
			new FillableEntry(1, typeof(SuccubusSummoningMateria)),
			new FillableEntry(1, typeof(TimeBreathMateria)),
			new FillableEntry(1, typeof(TimeCircleMateria)),
			new FillableEntry(1, typeof(TimeLineMateria)),
			new FillableEntry(1, typeof(TitanSummoningMateria)),
			new FillableEntry(1, typeof(ToxicElementalSummoningMateria)),
			new FillableEntry(1, typeof(TrapBreathMateria)),
			new FillableEntry(1, typeof(TrapCircleMateria)),
			new FillableEntry(1, typeof(TrapLineMateria)),
			new FillableEntry(1, typeof(TreeBreathMateria)),
			new FillableEntry(1, typeof(TroglodyteSummoningMateria)),
			new FillableEntry(1, typeof(TrollSummoningMateria)),
			new FillableEntry(1, typeof(UnicornSummoningMateria)),
			new FillableEntry(1, typeof(ValoriteElementalSummoningMateria)),
			new FillableEntry(1, typeof(VampireBatSummoningMateria)),
			new FillableEntry(1, typeof(VeriteElementalSummoningMateria)),
			new FillableEntry(1, typeof(VortexBreathMateria)),
			new FillableEntry(1, typeof(VortexCircleMateria)),
			new FillableEntry(1, typeof(VortexLineMateria)),
			new FillableEntry(1, typeof(WalrusSummoningMateria)),
			new FillableEntry(1, typeof(WaterBreathMateria)),
			new FillableEntry(1, typeof(WaterCircleMateria)),
			new FillableEntry(1, typeof(WaterElementalSummoningMateria)),
			new FillableEntry(1, typeof(WaterLineMateria)),
			new FillableEntry(1, typeof(WhiteWolfSummoningMateria)),
			new FillableEntry(1, typeof(WhiteWyrmSummoningMateria)),
			new FillableEntry(1, typeof(WispSummoningMateria)),
			new FillableEntry(1, typeof(WraithSummoningMateria)),
			new FillableEntry(1, typeof(WyvernSummoningMateria)),
			new FillableEntry(1, typeof(ZombieSummoningMateria)),
			new FillableEntry(1, typeof(MythicAmethyst)),
			new FillableEntry(1, typeof(LegendaryAmethyst)),
			new FillableEntry(1, typeof(AncientAmethyst)),
			new FillableEntry(1, typeof(FenCrystal)),
			new FillableEntry(1, typeof(RhoCrystal)),
			new FillableEntry(1, typeof(RysCrystal)),
			new FillableEntry(1, typeof(WyrCrystal)),
			new FillableEntry(1, typeof(FreCrystal)),
			new FillableEntry(1, typeof(TorCrystal)),
			new FillableEntry(1, typeof(VelCrystal)),
			new FillableEntry(1, typeof(XenCrystal)),
			new FillableEntry(1, typeof(PolCrystal)),
			new FillableEntry(1, typeof(WolCrystal)),
			new FillableEntry(1, typeof(BalCrystal)),
			new FillableEntry(1, typeof(TalCrystal)),
			new FillableEntry(1, typeof(JalCrystal)),
			new FillableEntry(1, typeof(RalCrystal)),
			new FillableEntry(1, typeof(KalCrystal)),
			new FillableEntry(1, typeof(MythicDiamond)),
			new FillableEntry(1, typeof(LegendaryDiamond)),
			new FillableEntry(1, typeof(AncientDiamond)),
			new FillableEntry(1, typeof(MythicEmerald)),
			new FillableEntry(1, typeof(LegendaryEmerald)),
			new FillableEntry(1, typeof(AncientEmerald)),
			new FillableEntry(1, typeof(KeyAugment)),
			new FillableEntry(1, typeof(RadiantRhoCrystal)),
			new FillableEntry(1, typeof(RadiantRysCrystal)),
			new FillableEntry(1, typeof(RadiantWyrCrystal)),
			new FillableEntry(1, typeof(RadiantFreCrystal)),
			new FillableEntry(1, typeof(RadiantTorCrystal)),
			new FillableEntry(1, typeof(RadiantVelCrystal)),
			new FillableEntry(1, typeof(RadiantXenCrystal)),
			new FillableEntry(1, typeof(RadiantPolCrystal)),
			new FillableEntry(1, typeof(RadiantWolCrystal)),
			new FillableEntry(1, typeof(RadiantBalCrystal)),
			new FillableEntry(1, typeof(RadiantTalCrystal)),
			new FillableEntry(1, typeof(RadiantJalCrystal)),
			new FillableEntry(1, typeof(RadiantRalCrystal)),
			new FillableEntry(1, typeof(RadiantKalCrystal)),
			new FillableEntry(1, typeof(MythicRuby)),
			new FillableEntry(1, typeof(LegendaryRuby)),
			new FillableEntry(1, typeof(AncientRuby)),
			new FillableEntry(1, typeof(TyrRune)),
			new FillableEntry(1, typeof(AhmRune)),
			new FillableEntry(1, typeof(MorRune)),
			new FillableEntry(1, typeof(MefRune)),
			new FillableEntry(1, typeof(YlmRune)),
			new FillableEntry(1, typeof(KotRune)),
			new FillableEntry(1, typeof(JorRune)),
			new FillableEntry(1, typeof(MythicSapphire)),
			new FillableEntry(1, typeof(LegendarySapphire)),
			new FillableEntry(1, typeof(AncientSapphire)),
			new FillableEntry(1, typeof(MythicSkull)),
			new FillableEntry(1, typeof(AncientSkull)),
			new FillableEntry(1, typeof(LegendarySkull)),
			new FillableEntry(1, typeof(GlimmeringGranite)),
			new FillableEntry(1, typeof(GlimmeringClay)),
			new FillableEntry(1, typeof(GlimmeringHeartstone)),
			new FillableEntry(1, typeof(GlimmeringGypsum)),
			new FillableEntry(1, typeof(GlimmeringIronOre)),
			new FillableEntry(1, typeof(GlimmeringOnyx)),
			new FillableEntry(1, typeof(GlimmeringMarble)),
			new FillableEntry(1, typeof(GlimmeringPetrifiedWood)),
			new FillableEntry(1, typeof(GlimmeringLimestone)),
			new FillableEntry(1, typeof(GlimmeringBloodrock)),
			new FillableEntry(1, typeof(MythicTourmaline)),
			new FillableEntry(1, typeof(LegendaryTourmaline)),
			new FillableEntry(1, typeof(AncientTourmaline)),
			new FillableEntry(1, typeof(MythicWood)),
			new FillableEntry(1, typeof(LegendaryWood)),
			new FillableEntry(1, typeof(BootsOfCommand)),
			new FillableEntry(1, typeof(GlovesOfCommand)),
			new FillableEntry(1, typeof(GrandmastersRobe)),
			new FillableEntry(1, typeof(JesterHatOfCommand)),
			new FillableEntry(1, typeof(PlateLeggingsOfCommand)),
			new FillableEntry(1, typeof(AshAxe)),
			new FillableEntry(1, typeof(BraceletOfNaturesBounty)),
			new FillableEntry(1, typeof(CampersBackpack)),
			new FillableEntry(1, typeof(ExtraPack)),
			new FillableEntry(1, typeof(FrostwoodAxe)),
			new FillableEntry(1, typeof(GoldenCrown)),
			new FillableEntry(1, typeof(GoldenDragon)),
			new FillableEntry(1, typeof(GoldenDrakelingScaleShield)),
			new FillableEntry(1, typeof(HeartwoodAxe)),
			new FillableEntry(1, typeof(IcicleStaff)),
			new FillableEntry(1, typeof(LightLordsScepter)),
			new FillableEntry(1, typeof(MasterBall)),
			new FillableEntry(1, typeof(MasterWeaponOil)),
			new FillableEntry(1, typeof(Pokeball)),
			new FillableEntry(1, typeof(ShadowIronShovel)),
			new FillableEntry(1, typeof(StolenTile)),
			new FillableEntry(1, typeof(TrapGloves)),
			new FillableEntry(1, typeof(TrapGorget)),
			new FillableEntry(1, typeof(TrapLegs)),
			new FillableEntry(1, typeof(TrapSleeves)),
			new FillableEntry(1, typeof(TrapTunic)),
			new FillableEntry(1, typeof(WeaponOil)),
			new FillableEntry(1, typeof(WizardKey)),
			new FillableEntry(1, typeof(YewAxe)),
			new FillableEntry(1, typeof(AssassinsDagger)),
			new FillableEntry(1, typeof(BagOfBombs)),
			new FillableEntry(1, typeof(BagOfHealth)),
			new FillableEntry(1, typeof(BagOfJuice)),
			new FillableEntry(1, typeof(BanishingOrb)),
			new FillableEntry(1, typeof(BanishingRod)),
			new FillableEntry(1, typeof(BeggarKingsCrown)),
			new FillableEntry(1, typeof(BloodSword)),
			new FillableEntry(1, typeof(BloodwoodAxe)),
			new FillableEntry(1, typeof(GlovesOfTheGrandmasterThief)),
			new FillableEntry(1, typeof(MagicMasterKey)),
			new FillableEntry(1, typeof(PlantingGloves)),
			new FillableEntry(1, typeof(QuickswordEnilno)),
			new FillableEntry(1, typeof(RodOfOrcControl)),
			new FillableEntry(1, typeof(ScryingOrb)),
			new FillableEntry(1, typeof(SiegeHammer)),
			new FillableEntry(1, typeof(SnoopersMasterScope)),
			new FillableEntry(1, typeof(ThiefsGlove)),
			new FillableEntry(1, typeof(TileExcavatorShovel)),
			new FillableEntry(1, typeof(TomeOfTime)),
			new FillableEntry(1, typeof(UniversalAbsorbingDyeTub)),
			new FillableEntry(1, typeof(AegisOfAthena)),
			new FillableEntry(1, typeof(AegisOfValor)),
			new FillableEntry(1, typeof(AlchemistsAmbition)),
			new FillableEntry(1, typeof(AlchemistsConduit)),
			new FillableEntry(1, typeof(AlchemistsGroundedBoots)),
			new FillableEntry(1, typeof(AlchemistsHeart)),
			new FillableEntry(1, typeof(AlchemistsPreciseGloves)),
			new FillableEntry(1, typeof(AlchemistsResilientLeggings)),
			new FillableEntry(1, typeof(AlchemistsVisionaryHelm)),
			new FillableEntry(1, typeof(ApronOfFlames)),
			new FillableEntry(1, typeof(ArkainesValorArms)),
			new FillableEntry(1, typeof(ArtisansCraftedGauntlets)),
			new FillableEntry(1, typeof(ArtisansHelm)),
			new FillableEntry(1, typeof(AshlandersResilience)),
			new FillableEntry(1, typeof(AstartesBattlePlate)),
			new FillableEntry(1, typeof(AstartesGauntletsOfMight)),
			new FillableEntry(1, typeof(AstartesHelmOfVigilance)),
			new FillableEntry(1, typeof(AstartesShoulderGuard)),
			new FillableEntry(1, typeof(AstartesWarBoots)),
			new FillableEntry(1, typeof(AstartesWarGreaves)),
			new FillableEntry(1, typeof(AtzirisStep)),
			new FillableEntry(1, typeof(AVALANCHEDefender)),
			new FillableEntry(1, typeof(AvatarsVestments)),
			new FillableEntry(1, typeof(BardsNimbleStep)),
			new FillableEntry(1, typeof(BeastmastersCrown)),
			new FillableEntry(1, typeof(BeastmastersGrips)),
			new FillableEntry(1, typeof(BeastsWhisperersRobe)),
			new FillableEntry(1, typeof(BerserkersEmbrace)),
			new FillableEntry(1, typeof(BlackMagesMysticRobe)),
			new FillableEntry(1, typeof(BlackMagesRuneRobe)),
			new FillableEntry(1, typeof(BlacksmithsBurden)),
			new FillableEntry(1, typeof(BlackthornesSpur)),
			new FillableEntry(1, typeof(BladedancersCloseHelm)),
			new FillableEntry(1, typeof(BladedancersOrderShield)),
			new FillableEntry(1, typeof(BladedancersPlateArms)),
			new FillableEntry(1, typeof(BladeDancersPlateChest)),
			new FillableEntry(1, typeof(BladeDancersPlateLegs)),
			new FillableEntry(1, typeof(BlazePlateLegs)),
			new FillableEntry(1, typeof(BombDisposalPlate)),
			new FillableEntry(1, typeof(BootsOfBalladry)),
			new FillableEntry(1, typeof(BootsOfFleetness)),
			new FillableEntry(1, typeof(BootsOfSwiftness)),
			new FillableEntry(1, typeof(BootsOfTheNetherTraveller)),
			new FillableEntry(1, typeof(CarpentersCrown)),
			new FillableEntry(1, typeof(CelesRunebladeBuckler)),
			new FillableEntry(1, typeof(CetrasBlessing)),
			new FillableEntry(1, typeof(ChefsHatOfFocus)),
			new FillableEntry(1, typeof(CourtesansDaintyBuckler)),
			new FillableEntry(1, typeof(CourtesansFlowingRobe)),
			new FillableEntry(1, typeof(CourtesansGracefulHelm)),
			new FillableEntry(1, typeof(CourtesansWhisperingBoots)),
			new FillableEntry(1, typeof(CourtesansWhisperingGloves)),
			new FillableEntry(1, typeof(CourtierDashingBoots)),
			new FillableEntry(1, typeof(CourtiersEnchantedAmulet)),
			new FillableEntry(1, typeof(CourtierSilkenRobe)),
			new FillableEntry(1, typeof(CourtiersRegalCirclet)),
			new FillableEntry(1, typeof(CovensShadowedHood)),
			new FillableEntry(1, typeof(CreepersLeatherCap)),
			new FillableEntry(1, typeof(CrownOfTheAbyss)),
			new FillableEntry(1, typeof(DaedricWarHelm)),
			new FillableEntry(1, typeof(DarkFathersCrown)),
			new FillableEntry(1, typeof(DarkFathersDreadnaughtBoots)),
			new FillableEntry(1, typeof(DarkFathersHeartplate)),
			new FillableEntry(1, typeof(DarkFathersSoulGauntlets)),
			new FillableEntry(1, typeof(DarkFathersVoidLeggings)),
			new FillableEntry(1, typeof(DarkKnightsCursedChestplate)),
			new FillableEntry(1, typeof(DarkKnightsDoomShield)),
			new FillableEntry(1, typeof(DarkKnightsObsidianHelm)),
			new FillableEntry(1, typeof(DarkKnightsShadowedGauntlets)),
			new FillableEntry(1, typeof(DarkKnightsVoidLeggings)),
			new FillableEntry(1, typeof(DemonspikeGuard)),
			new FillableEntry(1, typeof(DespairsShadow)),
			new FillableEntry(1, typeof(Doombringer)),
			new FillableEntry(1, typeof(DragonbornChestplate)),
			new FillableEntry(1, typeof(DragonsBulwark)),
			new FillableEntry(1, typeof(DragoonsAegis)),
			new FillableEntry(1, typeof(DwemerAegis)),
			new FillableEntry(1, typeof(EbonyChainArms)),
			new FillableEntry(1, typeof(EdgarsEngineerChainmail)),
			new FillableEntry(1, typeof(EldarRuneGuard)),
			new FillableEntry(1, typeof(ElixirProtector)),
			new FillableEntry(1, typeof(EmberPlateArms)),
			new FillableEntry(1, typeof(EnderGuardiansChestplate)),
			new FillableEntry(1, typeof(ExodusBarrier)),
			new FillableEntry(1, typeof(FalconersCoif)),
			new FillableEntry(1, typeof(FlamePlateGorget)),
			new FillableEntry(1, typeof(FortunesGorget)),
			new FillableEntry(1, typeof(FortunesHelm)),
			new FillableEntry(1, typeof(FortunesPlateArms)),
			new FillableEntry(1, typeof(FortunesPlateChest)),
			new FillableEntry(1, typeof(FortunesPlateLegs)),
			new FillableEntry(1, typeof(FrostwardensBascinet)),
			new FillableEntry(1, typeof(FrostwardensPlateChest)),
			new FillableEntry(1, typeof(FrostwardensPlateGloves)),
			new FillableEntry(1, typeof(FrostwardensPlateLegs)),
			new FillableEntry(1, typeof(FrostwardensWoodenShield)),
			new FillableEntry(1, typeof(GauntletsOfPrecision)),
			new FillableEntry(1, typeof(GauntletsOfPurity)),
			new FillableEntry(1, typeof(GauntletsOfTheWild)),
			new FillableEntry(1, typeof(GloomfangChain)),
			new FillableEntry(1, typeof(GlovesOfTheSilentAssassin)),
			new FillableEntry(1, typeof(GlovesOfTransmutation)),
			new FillableEntry(1, typeof(GoronsGauntlets)),
			new FillableEntry(1, typeof(GreyWanderersStride)),
			new FillableEntry(1, typeof(GuardianAngelArms)),
			new FillableEntry(1, typeof(GuardianOfTheAbyss)),
			new FillableEntry(1, typeof(GuardiansHeartplate)),
			new FillableEntry(1, typeof(GuardiansHelm)),
			new FillableEntry(1, typeof(HammerlordsArmguards)),
			new FillableEntry(1, typeof(HammerlordsChestplate)),
			new FillableEntry(1, typeof(HammerlordsHelm)),
			new FillableEntry(1, typeof(HammerlordsLegplates)),
			new FillableEntry(1, typeof(HammerlordsShield)),
			new FillableEntry(1, typeof(HarmonyGauntlets)),
			new FillableEntry(1, typeof(HarmonysGuard)),
			new FillableEntry(1, typeof(HarvestersFootsteps)),
			new FillableEntry(1, typeof(HarvestersGrasp)),
			new FillableEntry(1, typeof(HarvestersGuard)),
			new FillableEntry(1, typeof(HarvestersHelm)),
			new FillableEntry(1, typeof(HarvestersStride)),
			new FillableEntry(1, typeof(HexweaversMysticalGloves)),
			new FillableEntry(1, typeof(HlaaluTradersCuffs)),
			new FillableEntry(1, typeof(HyruleKnightsShield)),
			new FillableEntry(1, typeof(ImmortalKingsIronCrown)),
			new FillableEntry(1, typeof(InfernoPlateChest)),
			new FillableEntry(1, typeof(InquisitorsGuard)),
			new FillableEntry(1, typeof(IstarisTouch)),
			new FillableEntry(1, typeof(JestersGleefulGloves)),
			new FillableEntry(1, typeof(JestersMerryCap)),
			new FillableEntry(1, typeof(JestersMischievousBuckler)),
			new FillableEntry(1, typeof(JestersPlayfulTunic)),
			new FillableEntry(1, typeof(JestersTricksterBoots)),
			new FillableEntry(1, typeof(KnightsAegis)),
			new FillableEntry(1, typeof(KnightsValorShield)),
			new FillableEntry(1, typeof(LeggingsOfTheRighteous)),
			new FillableEntry(1, typeof(LioneyesRemorse)),
			new FillableEntry(1, typeof(LionheartPlate)),
			new FillableEntry(1, typeof(LockesAdventurerLeather)),
			new FillableEntry(1, typeof(LocksleyLeatherChest)),
			new FillableEntry(1, typeof(LyricalGreaves)),
			new FillableEntry(1, typeof(LyricistsInsight)),
			new FillableEntry(1, typeof(MagitekInfusedPlate)),
			new FillableEntry(1, typeof(MakoResonance)),
			new FillableEntry(1, typeof(MaskedAvengersAgility)),
			new FillableEntry(1, typeof(MaskedAvengersDefense)),
			new FillableEntry(1, typeof(MaskedAvengersFocus)),
			new FillableEntry(1, typeof(MaskedAvengersPrecision)),
			new FillableEntry(1, typeof(MaskedAvengersVoice)),
			new FillableEntry(1, typeof(MelodicCirclet)),
			new FillableEntry(1, typeof(MerryMensStuddedGloves)),
			new FillableEntry(1, typeof(MeteorWard)),
			new FillableEntry(1, typeof(MinersHelmet)),
			new FillableEntry(1, typeof(MinstrelsMelody)),
			new FillableEntry(1, typeof(MisfortunesChains)),
			new FillableEntry(1, typeof(MondainsSkull)),
			new FillableEntry(1, typeof(MonksBattleWraps)),
			new FillableEntry(1, typeof(MonksSoulGloves)),
			new FillableEntry(1, typeof(MysticSeersPlate)),
			new FillableEntry(1, typeof(MysticsGuard)),
			new FillableEntry(1, typeof(NajsArcaneVestment)),
			new FillableEntry(1, typeof(NaturesEmbraceBelt)),
			new FillableEntry(1, typeof(NaturesEmbraceHelm)),
			new FillableEntry(1, typeof(NaturesGuardBoots)),
			new FillableEntry(1, typeof(NecklaceOfAromaticProtection)),
			new FillableEntry(1, typeof(NecromancersBoneGrips)),
			new FillableEntry(1, typeof(NecromancersDarkLeggings)),
			new FillableEntry(1, typeof(NecromancersHood)),
			new FillableEntry(1, typeof(NecromancersRobe)),
			new FillableEntry(1, typeof(NecromancersShadowBoots)),
			new FillableEntry(1, typeof(NightingaleVeil)),
			new FillableEntry(1, typeof(NinjaWrappings)),
			new FillableEntry(1, typeof(NottinghamStalkersLeggings)),
			new FillableEntry(1, typeof(OrkArdHat)),
			new FillableEntry(1, typeof(OutlawsForestBuckler)),
			new FillableEntry(1, typeof(PhilosophersGreaves)),
			new FillableEntry(1, typeof(PyrePlateHelm)),
			new FillableEntry(1, typeof(RadiantCrown)),
			new FillableEntry(1, typeof(RatsNest)),
			new FillableEntry(1, typeof(ReconnaissanceBoots)),
			new FillableEntry(1, typeof(RedoranDefendersGreaves)),
			new FillableEntry(1, typeof(RedstoneArtificersGloves)),
			new FillableEntry(1, typeof(RiotDefendersShield)),
			new FillableEntry(1, typeof(RoguesShadowBoots)),
			new FillableEntry(1, typeof(RoguesStealthShield)),
			new FillableEntry(1, typeof(RoyalCircletHelm)),
			new FillableEntry(1, typeof(SabatonsOfDawn)),
			new FillableEntry(1, typeof(SerenadesEmbrace)),
			new FillableEntry(1, typeof(SerpentScaleArmor)),
			new FillableEntry(1, typeof(SerpentsEmbrace)),
			new FillableEntry(1, typeof(ShadowGripGloves)),
			new FillableEntry(1, typeof(ShaftstopArmor)),
			new FillableEntry(1, typeof(ShaminosGreaves)),
			new FillableEntry(1, typeof(SherwoodArchersCap)),
			new FillableEntry(1, typeof(ShinobiHood)),
			new FillableEntry(1, typeof(ShurikenBracers)),
			new FillableEntry(1, typeof(SilentStepTabi)),
			new FillableEntry(1, typeof(SilksOfTheVictor)),
			new FillableEntry(1, typeof(SirensLament)),
			new FillableEntry(1, typeof(SirensResonance)),
			new FillableEntry(1, typeof(SkinOfTheVipermagi)),
			new FillableEntry(1, typeof(SlitheringSeal)),
			new FillableEntry(1, typeof(SolarisAegis)),
			new FillableEntry(1, typeof(SolarisLorica)),
			new FillableEntry(1, typeof(SOLDIERSMight)),
			new FillableEntry(1, typeof(SorrowsGrasp)),
			new FillableEntry(1, typeof(StealthOperatorsGear)),
			new FillableEntry(1, typeof(StormcrowsGaze)),
			new FillableEntry(1, typeof(StormforgedBoots)),
			new FillableEntry(1, typeof(StormforgedGauntlets)),
			new FillableEntry(1, typeof(StormforgedHelm)),
			new FillableEntry(1, typeof(StormforgedLeggings)),
			new FillableEntry(1, typeof(StormforgedPlateChest)),
			new FillableEntry(1, typeof(Stormshield)),
			new FillableEntry(1, typeof(StringOfEars)),
			new FillableEntry(1, typeof(SummonersEmbrace)),
			new FillableEntry(1, typeof(TabulaRasa)),
			new FillableEntry(1, typeof(TacticalVest)),
			new FillableEntry(1, typeof(TailorsTouch)),
			new FillableEntry(1, typeof(TalsRashasRelic)),
			new FillableEntry(1, typeof(TamersBindings)),
			new FillableEntry(1, typeof(TechPriestMantle)),
			new FillableEntry(1, typeof(TelvanniMagistersCap)),
			new FillableEntry(1, typeof(TerrasMysticRobe)),
			new FillableEntry(1, typeof(TheThinkingCap)),
			new FillableEntry(1, typeof(ThiefsNimbleCap)),
			new FillableEntry(1, typeof(ThievesGuildPants)),
			new FillableEntry(1, typeof(ThundergodsVigor)),
			new FillableEntry(1, typeof(TinkersTreads)),
			new FillableEntry(1, typeof(ToxinWard)),
			new FillableEntry(1, typeof(TunicOfTheWild)),
			new FillableEntry(1, typeof(TyraelsVigil)),
			new FillableEntry(1, typeof(ValkyriesWard)),
			new FillableEntry(1, typeof(VeilOfSteel)),
			new FillableEntry(1, typeof(Venomweave)),
			new FillableEntry(1, typeof(VialWarden)),
			new FillableEntry(1, typeof(VipersCoif)),
			new FillableEntry(1, typeof(VirtueGuard)),
			new FillableEntry(1, typeof(VortexMantle)),
			new FillableEntry(1, typeof(VyrsGraspingGauntlets)),
			new FillableEntry(1, typeof(WardensAegis)),
			new FillableEntry(1, typeof(WhispersHeartguard)),
			new FillableEntry(1, typeof(WhiteMagesDivineVestment)),
			new FillableEntry(1, typeof(WhiteRidersGuard)),
			new FillableEntry(1, typeof(WhiteSageCap)),
			new FillableEntry(1, typeof(WildwalkersGreaves)),
			new FillableEntry(1, typeof(WinddancerBoots)),
			new FillableEntry(1, typeof(WisdomsCirclet)),
			new FillableEntry(1, typeof(WisdomsEmbrace)),
			new FillableEntry(1, typeof(WitchesBindingGloves)),
			new FillableEntry(1, typeof(WitchesCursedRobe)),
			new FillableEntry(1, typeof(WitchesEnchantedHat)),
			new FillableEntry(1, typeof(WitchesEnchantedRobe)),
			new FillableEntry(1, typeof(WitchesHeartAmulet)),
			new FillableEntry(1, typeof(WitchesWhisperingBoots)),
			new FillableEntry(1, typeof(WitchfireShield)),
			new FillableEntry(1, typeof(WitchwoodGreaves)),
			new FillableEntry(1, typeof(WraithsBane)),
			new FillableEntry(1, typeof(WrestlersArmsOfPrecision)),
			new FillableEntry(1, typeof(WrestlersChestOfPower)),
			new FillableEntry(1, typeof(WrestlersGrippingGloves)),
			new FillableEntry(1, typeof(WrestlersHelmOfFocus)),
			new FillableEntry(1, typeof(WrestlersLeggingsOfBalance)),
			new FillableEntry(1, typeof(ZorasFins)),
			new FillableEntry(1, typeof(AdventurersBoots)),
			new FillableEntry(1, typeof(AerobicsInstructorsLegwarmers)),
			new FillableEntry(1, typeof(AmbassadorsCloak)),
			new FillableEntry(1, typeof(AnglersSeabreezeCloak)),
			new FillableEntry(1, typeof(ArchivistsShoes)),
			new FillableEntry(1, typeof(ArrowsmithsSturdyBoots)),
			new FillableEntry(1, typeof(ArtisansTimberShoes)),
			new FillableEntry(1, typeof(AssassinsBandana)),
			new FillableEntry(1, typeof(AssassinsMaskedCap)),
			new FillableEntry(1, typeof(BaggyHipHopPants)),
			new FillableEntry(1, typeof(BakersSoftShoes)),
			new FillableEntry(1, typeof(BalladeersMuffler)),
			new FillableEntry(1, typeof(BanditsHiddenCloak)),
			new FillableEntry(1, typeof(BardOfErinsMuffler)),
			new FillableEntry(1, typeof(BardsTunicOfStonehenge)),
			new FillableEntry(1, typeof(BaristasMuffler)),
			new FillableEntry(1, typeof(BeastmastersTanic)),
			new FillableEntry(1, typeof(BeastmastersTonic)),
			new FillableEntry(1, typeof(BeastmastersTunic)),
			new FillableEntry(1, typeof(BeastmiastersTunic)),
			new FillableEntry(1, typeof(BeatniksBeret)),
			new FillableEntry(1, typeof(BeggarsLuckyBandana)),
			new FillableEntry(1, typeof(BlacksmithsReinforcedGloves)),
			new FillableEntry(1, typeof(BobbySoxersShoes)),
			new FillableEntry(1, typeof(BohoChicSundress)),
			new FillableEntry(1, typeof(BootsOfTheDeepCaverns)),
			new FillableEntry(1, typeof(BowcraftersProtectiveCloak)),
			new FillableEntry(1, typeof(BowyersInsightfulBandana)),
			new FillableEntry(1, typeof(BreakdancersCap)),
			new FillableEntry(1, typeof(CarpentersStalwartTunic)),
			new FillableEntry(1, typeof(CartographersExploratoryTunic)),
			new FillableEntry(1, typeof(CartographersHat)),
			new FillableEntry(1, typeof(CeltidDruidsRobe)),
			new FillableEntry(1, typeof(ChampagneToastTunic)),
			new FillableEntry(1, typeof(ChefsGourmetApron)),
			new FillableEntry(1, typeof(ClericsSacredSash)),
			new FillableEntry(1, typeof(CourtesansGracefulKimono)),
			new FillableEntry(1, typeof(CourtisansRefinedGown)),
			new FillableEntry(1, typeof(CouturiersSundress)),
			new FillableEntry(1, typeof(CraftsmansProtectiveGloves)),
			new FillableEntry(1, typeof(CropTopMystic)),
			new FillableEntry(1, typeof(CuratorsKilt)),
			new FillableEntry(1, typeof(CyberpunkNinjaTabi)),
			new FillableEntry(1, typeof(DancersEnchantedSkirt)),
			new FillableEntry(1, typeof(DapperFedoraOfInsight)),
			new FillableEntry(1, typeof(DarkLordsRobe)),
			new FillableEntry(1, typeof(DataMagesDigitalCloak)),
			new FillableEntry(1, typeof(DeepSeaTunic)),
			new FillableEntry(1, typeof(DenimJacketOfReflection)),
			new FillableEntry(1, typeof(DiplomatsTunic)),
			new FillableEntry(1, typeof(DiscoDivaBoots)),
			new FillableEntry(1, typeof(ElementalistsProtectiveCloak)),
			new FillableEntry(1, typeof(ElvenSnowBoots)),
			new FillableEntry(1, typeof(EmoSceneHairpin)),
			new FillableEntry(1, typeof(ExplorersBoots)),
			new FillableEntry(1, typeof(FilmNoirDetectivesTrenchCoat)),
			new FillableEntry(1, typeof(FishermansSunHat)),
			new FillableEntry(1, typeof(FishermansVest)),
			new FillableEntry(1, typeof(FishmongersKilt)),
			new FillableEntry(1, typeof(FletchersPrecisionGloves)),
			new FillableEntry(1, typeof(FlowerChildSundress)),
			new FillableEntry(1, typeof(ForestersTunic)),
			new FillableEntry(1, typeof(ForgeMastersBoots)),
			new FillableEntry(1, typeof(GazeCapturingVeil)),
			new FillableEntry(1, typeof(GeishasGracefulKasa)),
			new FillableEntry(1, typeof(GhostlyShroud)),
			new FillableEntry(1, typeof(GlamRockersJacket)),
			new FillableEntry(1, typeof(GlovesOfStonemasonry)),
			new FillableEntry(1, typeof(GoGoBootsOfAgility)),
			new FillableEntry(1, typeof(GrapplersTunic)),
			new FillableEntry(1, typeof(GreenwichMagesRobe)),
			new FillableEntry(1, typeof(GroovyBellBottomPants)),
			new FillableEntry(1, typeof(GrungeBandana)),
			new FillableEntry(1, typeof(HackersVRGoggles)),
			new FillableEntry(1, typeof(HammerlordsCap)),
			new FillableEntry(1, typeof(HarmonistsSoftShoes)),
			new FillableEntry(1, typeof(HealersBlessedSandals)),
			new FillableEntry(1, typeof(HealersFurCape)),
			new FillableEntry(1, typeof(HelmetOfTheOreWhisperer)),
			new FillableEntry(1, typeof(HerbalistsProtectiveHat)),
			new FillableEntry(1, typeof(HerdersMuffler)),
			new FillableEntry(1, typeof(HippiePeaceBandana)),
			new FillableEntry(1, typeof(HippiesPeacefulSandals)),
			new FillableEntry(1, typeof(IntriguersFeatheredHat)),
			new FillableEntry(1, typeof(JazzMusiciansMuffler)),
			new FillableEntry(1, typeof(KnightsHelmOfTheRoundTable)),
			new FillableEntry(1, typeof(LeprechaunsLuckyHat)),
			new FillableEntry(1, typeof(LorekeepersSash)),
			new FillableEntry(1, typeof(LuchadorsMask)),
			new FillableEntry(1, typeof(MapmakersInsightfulMuffler)),
			new FillableEntry(1, typeof(MarinersLuckyBoots)),
			new FillableEntry(1, typeof(MelodiousMuffler)),
			new FillableEntry(1, typeof(MendersDivineRobe)),
			new FillableEntry(1, typeof(MidnightRevelersBoots)),
			new FillableEntry(1, typeof(MinersSturdyBoots)),
			new FillableEntry(1, typeof(MinstrelsTunedTunic)),
			new FillableEntry(1, typeof(MistletoeMuffler)),
			new FillableEntry(1, typeof(ModStyleTunic)),
			new FillableEntry(1, typeof(MoltenCloak)),
			new FillableEntry(1, typeof(MonksMeditativeRobe)),
			new FillableEntry(1, typeof(MummysWrappings)),
			new FillableEntry(1, typeof(MysticsFeatheredHat)),
			new FillableEntry(1, typeof(NaturalistsCloak)),
			new FillableEntry(1, typeof(NaturesMuffler)),
			new FillableEntry(1, typeof(NavigatorsProtectiveCap)),
			new FillableEntry(1, typeof(NecromancersCape)),
			new FillableEntry(1, typeof(NeonStreetSash)),
			new FillableEntry(1, typeof(NewWaveNeonShades)),
			new FillableEntry(1, typeof(NinjasKasa)),
			new FillableEntry(1, typeof(NinjasStealthyTabi)),
			new FillableEntry(1, typeof(OreSeekersBandana)),
			new FillableEntry(1, typeof(PickpocketsNimbleGloves)),
			new FillableEntry(1, typeof(PickpocketsSleekTunic)),
			new FillableEntry(1, typeof(PinUpHalterDress)),
			new FillableEntry(1, typeof(PlatformSneakers)),
			new FillableEntry(1, typeof(PoodleSkirtOfCharm)),
			new FillableEntry(1, typeof(PopStarsFingerlessGloves)),
			new FillableEntry(1, typeof(PopStarsGlitteringCap)),
			new FillableEntry(1, typeof(PopStarsSparklingBandana)),
			new FillableEntry(1, typeof(PreserversCap)),
			new FillableEntry(1, typeof(PsychedelicTieDyeShirt)),
			new FillableEntry(1, typeof(PsychedelicWizardsHat)),
			new FillableEntry(1, typeof(PumpkinKingsCrown)),
			new FillableEntry(1, typeof(QuivermastersTunic)),
			new FillableEntry(1, typeof(RangersCap)),
			new FillableEntry(1, typeof(RangersHat)),
			new FillableEntry(1, typeof(RangersHatNightSight)),
			new FillableEntry(1, typeof(ReindeerFurCap)),
			new FillableEntry(1, typeof(ResolutionKeepersSash)),
			new FillableEntry(1, typeof(RingmastersSandals)),
			new FillableEntry(1, typeof(RockabillyRebelJacket)),
			new FillableEntry(1, typeof(RoguesDeceptiveMask)),
			new FillableEntry(1, typeof(RoguesShadowCloak)),
			new FillableEntry(1, typeof(RoyalGuardsBoots)),
			new FillableEntry(1, typeof(SamuraisHonorableTunic)),
			new FillableEntry(1, typeof(SantasEnchantedRobe)),
			new FillableEntry(1, typeof(SawyersMightyApron)),
			new FillableEntry(1, typeof(ScholarsRobe)),
			new FillableEntry(1, typeof(ScoutsWideBrimHat)),
			new FillableEntry(1, typeof(ScribersRobe)),
			new FillableEntry(1, typeof(ScribesEnlightenedSandals)),
			new FillableEntry(1, typeof(ScriptoriumMastersRobe)),
			new FillableEntry(1, typeof(SeductressSilkenShoes)),
			new FillableEntry(1, typeof(SeersMysticSash)),
			new FillableEntry(1, typeof(ShadowWalkersTabi)),
			new FillableEntry(1, typeof(ShanachiesStorytellingShoes)),
			new FillableEntry(1, typeof(ShepherdsKilt)),
			new FillableEntry(1, typeof(SherlocksSleuthingCap)),
			new FillableEntry(1, typeof(ShogunsAuthoritativeSurcoat)),
			new FillableEntry(1, typeof(SilentNightCloak)),
			new FillableEntry(1, typeof(SkatersBaggyPants)),
			new FillableEntry(1, typeof(SmithsProtectiveTunic)),
			new FillableEntry(1, typeof(SneaksSilentShoes)),
			new FillableEntry(1, typeof(SnoopersSoftGloves)),
			new FillableEntry(1, typeof(SommelierBodySash)),
			new FillableEntry(1, typeof(SorceressMidnightRobe)),
			new FillableEntry(1, typeof(SpellweaversEnchantedShoes)),
			new FillableEntry(1, typeof(StarletsFancyDress)),
			new FillableEntry(1, typeof(StarlightWizardsHat)),
			new FillableEntry(1, typeof(StarlightWozardsHat)),
			new FillableEntry(1, typeof(StreetArtistsBaggyPants)),
			new FillableEntry(1, typeof(StreetPerformersCap)),
			new FillableEntry(1, typeof(SubmissionsArtistsMuffler)),
			new FillableEntry(1, typeof(SurgeonsInsightfulMask)),
			new FillableEntry(1, typeof(SwingsDancersShoes)),
			new FillableEntry(1, typeof(TailorsFancyApron)),
			new FillableEntry(1, typeof(TamersKilt)),
			new FillableEntry(1, typeof(TamersMuffler)),
			new FillableEntry(1, typeof(TechGurusGlasses)),
			new FillableEntry(1, typeof(TechnomancersHoodie)),
			new FillableEntry(1, typeof(ThiefsShadowTunic)),
			new FillableEntry(1, typeof(ThiefsSilentShoes)),
			new FillableEntry(1, typeof(TidecallersSandals)),
			new FillableEntry(1, typeof(TruckersIconicCap)),
			new FillableEntry(1, typeof(UrbanitesSneakers)),
			new FillableEntry(1, typeof(VampiresMidnightCloak)),
			new FillableEntry(1, typeof(VestOfTheVeinSeeker)),
			new FillableEntry(1, typeof(WarHeronsCap)),
			new FillableEntry(1, typeof(WarriorOfUlstersTunic)),
			new FillableEntry(1, typeof(WarriorsBelt)),
			new FillableEntry(1, typeof(WhisperersBoots)),
			new FillableEntry(1, typeof(WhisperersSandals)),
			new FillableEntry(1, typeof(WhisperingSandals)),
			new FillableEntry(1, typeof(WhisperingSondals)),
			new FillableEntry(1, typeof(WhisperingWindSash)),
			new FillableEntry(1, typeof(WitchesBewitchingRobe)),
			new FillableEntry(1, typeof(WitchesBrewedHat)),
			new FillableEntry(1, typeof(WoodworkersInsightfulCap)),
			new FillableEntry(1, typeof(AegisShield)),
			new FillableEntry(1, typeof(AeonianBow)),
			new FillableEntry(1, typeof(AlamoDefendersAxe)),
			new FillableEntry(1, typeof(AlucardsBlade)),
			new FillableEntry(1, typeof(AnubisWarMace)),
			new FillableEntry(1, typeof(ApepsCoiledScimitar)),
			new FillableEntry(1, typeof(ApollosSong)),
			new FillableEntry(1, typeof(ArchersYewBow)),
			new FillableEntry(1, typeof(AssassinsKryss)),
			new FillableEntry(1, typeof(AtmaBlade)),
			new FillableEntry(1, typeof(AxeOfTheJuggernaut)),
			new FillableEntry(1, typeof(AxeOfTheRuneweaver)),
			new FillableEntry(1, typeof(BaneOfTheDead)),
			new FillableEntry(1, typeof(BanshoFanClub)),
			new FillableEntry(1, typeof(BarbarossaScimitar)),
			new FillableEntry(1, typeof(BardsBowOfDiscord)),
			new FillableEntry(1, typeof(BeowulfsWarAxe)),
			new FillableEntry(1, typeof(BismarckianWarAxe)),
			new FillableEntry(1, typeof(Blackrazor)),
			new FillableEntry(1, typeof(BlacksmithsWarHammer)),
			new FillableEntry(1, typeof(BlackSwordOfMondain)),
			new FillableEntry(1, typeof(BlackTailWhip)),
			new FillableEntry(1, typeof(BladeOfTheStars)),
			new FillableEntry(1, typeof(Bonehew)),
			new FillableEntry(1, typeof(BowiesLegacy)),
			new FillableEntry(1, typeof(BowOfAuriel)),
			new FillableEntry(1, typeof(BowOfIsrafil)),
			new FillableEntry(1, typeof(BowspritOfBluenose)),
			new FillableEntry(1, typeof(BulKathosTribalGuardian)),
			new FillableEntry(1, typeof(BusterSwordReplica)),
			new FillableEntry(1, typeof(ButchersCleaver)),
			new FillableEntry(1, typeof(CaduceusStaff)),
			new FillableEntry(1, typeof(CelestialLongbow)),
			new FillableEntry(1, typeof(CelestialScimitar)),
			new FillableEntry(1, typeof(CetrasStaff)),
			new FillableEntry(1, typeof(ChakramBlade)),
			new FillableEntry(1, typeof(CharlemagnesWarAxe)),
			new FillableEntry(1, typeof(CherubsBlade)),
			new FillableEntry(1, typeof(ChillrendLongsword)),
			new FillableEntry(1, typeof(ChuKoNu)),
			new FillableEntry(1, typeof(CrissaegrimEdge)),
			new FillableEntry(1, typeof(CthulhusGaze)),
			new FillableEntry(1, typeof(CursedArmorCleaver)),
			new FillableEntry(1, typeof(CustersLastStandBow)),
			new FillableEntry(1, typeof(DaggerOfShadows)),
			new FillableEntry(1, typeof(DavidsSling)),
			new FillableEntry(1, typeof(DawnbreakerMace)),
			new FillableEntry(1, typeof(DeadMansLegacy)),
			new FillableEntry(1, typeof(DestructoDiscDagger)),
			new FillableEntry(1, typeof(DianasMoonBow)),
			new FillableEntry(1, typeof(DoomfletchsPrism)),
			new FillableEntry(1, typeof(Doomsickle)),
			new FillableEntry(1, typeof(DragonClaw)),
			new FillableEntry(1, typeof(DragonsBreath)),
			new FillableEntry(1, typeof(DragonsBreathWarAxe)),
			new FillableEntry(1, typeof(DragonsScaleDagger)),
			new FillableEntry(1, typeof(DragonsWrath)),
			new FillableEntry(1, typeof(Dreamseeker)),
			new FillableEntry(1, typeof(EarthshakerMaul)),
			new FillableEntry(1, typeof(EbonyWarAxeOfVampires)),
			new FillableEntry(1, typeof(EldritchBowOfShadows)),
			new FillableEntry(1, typeof(EldritchWhisper)),
			new FillableEntry(1, typeof(ErdricksBlade)),
			new FillableEntry(1, typeof(Excalibur)),
			new FillableEntry(1, typeof(ExcaliburLongsword)),
			new FillableEntry(1, typeof(ExcalibursLegacy)),
			new FillableEntry(1, typeof(FangOfStorms)),
			new FillableEntry(1, typeof(FlamebaneWarAxe)),
			new FillableEntry(1, typeof(FrostfireCleaver)),
			new FillableEntry(1, typeof(FrostflameKatana)),
			new FillableEntry(1, typeof(FuHaosBattleAxe)),
			new FillableEntry(1, typeof(GenjiBow)),
			new FillableEntry(1, typeof(GeomancersStaff)),
			new FillableEntry(1, typeof(GhoulSlayersLongsword)),
			new FillableEntry(1, typeof(GlassSword)),
			new FillableEntry(1, typeof(GlassSwordOfValor)),
			new FillableEntry(1, typeof(GoldbrandScimitar)),
			new FillableEntry(1, typeof(GreenDragonCrescentBlade)),
			new FillableEntry(1, typeof(Grimmblade)),
			new FillableEntry(1, typeof(GrimReapersCleaver)),
			new FillableEntry(1, typeof(GriswoldsEdge)),
			new FillableEntry(1, typeof(GrognaksAxe)),
			new FillableEntry(1, typeof(GuardianOfTheFey)),
			new FillableEntry(1, typeof(GuillotineBladeDagger)),
			new FillableEntry(1, typeof(HalberdOfHonesty)),
			new FillableEntry(1, typeof(HanseaticCrossbow)),
			new FillableEntry(1, typeof(HarmonyBow)),
			new FillableEntry(1, typeof(HarpeBlade)),
			new FillableEntry(1, typeof(HeartbreakerSunder)),
			new FillableEntry(1, typeof(HelmOfDarkness)),
			new FillableEntry(1, typeof(IlluminaDagger)),
			new FillableEntry(1, typeof(InuitUluOfTheNorth)),
			new FillableEntry(1, typeof(JoansDivineLongsword)),
			new FillableEntry(1, typeof(JuggernautHammer)),
			new FillableEntry(1, typeof(KaomsCleaver)),
			new FillableEntry(1, typeof(KaomsMaul)),
			new FillableEntry(1, typeof(Keenstrike)),
			new FillableEntry(1, typeof(KhufusWarSpear)),
			new FillableEntry(1, typeof(KingsSwordOfHaste)),
			new FillableEntry(1, typeof(MaatsBalancedBow)),
			new FillableEntry(1, typeof(MablungsDefender)),
			new FillableEntry(1, typeof(MaceOfTheVoid)),
			new FillableEntry(1, typeof(MageMasher)),
			new FillableEntry(1, typeof(MageMusher)),
			new FillableEntry(1, typeof(MagesStaff)),
			new FillableEntry(1, typeof(MagicAxeOfGreatStrength)),
			new FillableEntry(1, typeof(MagusRod)),
			new FillableEntry(1, typeof(MakhairaOfAchilles)),
			new FillableEntry(1, typeof(ManajumasKnife)),
			new FillableEntry(1, typeof(MarssBattleAxeOfValor)),
			new FillableEntry(1, typeof(MasamuneBlade)),
			new FillableEntry(1, typeof(MasamuneKatana)),
			new FillableEntry(1, typeof(MasamunesEdge)),
			new FillableEntry(1, typeof(MasamunesGrace)),
			new FillableEntry(1, typeof(MaulOfSulayman)),
			new FillableEntry(1, typeof(MehrunesCleaver)),
			new FillableEntry(1, typeof(MortuarySword)),
			new FillableEntry(1, typeof(MosesStaff)),
			new FillableEntry(1, typeof(MuramasasBloodlust)),
			new FillableEntry(1, typeof(MusketeersRapier)),
			new FillableEntry(1, typeof(MysticBowOfLight)),
			new FillableEntry(1, typeof(MysticStaffOfElements)),
			new FillableEntry(1, typeof(NaginataOfTomoeGozen)),
			new FillableEntry(1, typeof(NebulaBow)),
			new FillableEntry(1, typeof(NecromancersDagger)),
			new FillableEntry(1, typeof(NeptunesTrident)),
			new FillableEntry(1, typeof(NormanConquerorsBow)),
			new FillableEntry(1, typeof(PaladinsChrysblade)),
			new FillableEntry(1, typeof(PlasmaInfusedWarHammer)),
			new FillableEntry(1, typeof(PlutosAbyssalMace)),
			new FillableEntry(1, typeof(PotaraEarringClub)),
			new FillableEntry(1, typeof(PowerPoleHalberd)),
			new FillableEntry(1, typeof(PowersBeacon)),
			new FillableEntry(1, typeof(ProhibitionClub)),
			new FillableEntry(1, typeof(QamarDagger)),
			new FillableEntry(1, typeof(QuasarAxe)),
			new FillableEntry(1, typeof(RainbowBlade)),
			new FillableEntry(1, typeof(RasSearingDagger)),
			new FillableEntry(1, typeof(ReflectionShield)),
			new FillableEntry(1, typeof(RevolutionarySabre)),
			new FillableEntry(1, typeof(RielsRebellionSabre)),
			new FillableEntry(1, typeof(RuneAss)),
			new FillableEntry(1, typeof(RuneAxe)),
			new FillableEntry(1, typeof(SaiyanTailWhip)),
			new FillableEntry(1, typeof(SamsonsJawbone)),
			new FillableEntry(1, typeof(SaxonSeax)),
			new FillableEntry(1, typeof(SearingTouch)),
			new FillableEntry(1, typeof(SerpentsFang)),
			new FillableEntry(1, typeof(SerpentsVenomDagger)),
			new FillableEntry(1, typeof(ShadowstrideBow)),
			new FillableEntry(1, typeof(ShavronnesRapier)),
			new FillableEntry(1, typeof(SkyPiercer)),
			new FillableEntry(1, typeof(SoulTaker)),
			new FillableEntry(1, typeof(StaffOfAeons)),
			new FillableEntry(1, typeof(StaffOfApocalypse)),
			new FillableEntry(1, typeof(StaffOfRainsWrath)),
			new FillableEntry(1, typeof(StaffOfTheElements)),
			new FillableEntry(1, typeof(StarfallDagger)),
			new FillableEntry(1, typeof(Sunblade)),
			new FillableEntry(1, typeof(SwordOfAlBattal)),
			new FillableEntry(1, typeof(SwordOfGideon)),
			new FillableEntry(1, typeof(TabulasDagger)),
			new FillableEntry(1, typeof(TantoOfThe47Ronin)),
			new FillableEntry(1, typeof(TempestHammer)),
			new FillableEntry(1, typeof(TeutonicWarMace)),
			new FillableEntry(1, typeof(TheFurnace)),
			new FillableEntry(1, typeof(TheOculus)),
			new FillableEntry(1, typeof(ThorsHammer)),
			new FillableEntry(1, typeof(Thunderfury)),
			new FillableEntry(1, typeof(Thunderstroke)),
			new FillableEntry(1, typeof(TitansFury)),
			new FillableEntry(1, typeof(TomahawkOfTecumseh)),
			new FillableEntry(1, typeof(TouchOfAnguish)),
			new FillableEntry(1, typeof(TriLithiumBlade)),
			new FillableEntry(1, typeof(TwoShotCrossbow)),
			new FillableEntry(1, typeof(UltimaGlaive)),
			new FillableEntry(1, typeof(UmbraWarAxe)),
			new FillableEntry(1, typeof(UndeadCrown)),
			new FillableEntry(1, typeof(ValiantThrower)),
			new FillableEntry(1, typeof(VampireKiller)),
			new FillableEntry(1, typeof(VATSEnhancedDagger)),
			new FillableEntry(1, typeof(VenomsSting)),
			new FillableEntry(1, typeof(VoidsEmbrace)),
			new FillableEntry(1, typeof(VolendrungWarHammer)),
			new FillableEntry(1, typeof(VolendrungWorHammer)),
			new FillableEntry(1, typeof(VoltaxicRiftLance)),
			new FillableEntry(1, typeof(VoyageursPaddle)),
			new FillableEntry(1, typeof(VulcansForgeHammer)),
			new FillableEntry(1, typeof(WabbajackClub)),
			new FillableEntry(1, typeof(WandOfWoh)),
			new FillableEntry(1, typeof(Whelm)),
			new FillableEntry(1, typeof(WhisperingWindWarMace)),
			new FillableEntry(1, typeof(WhisperwindBow)),
			new FillableEntry(1, typeof(WindDancersDagger)),
			new FillableEntry(1, typeof(WindripperBow)),
			new FillableEntry(1, typeof(Wizardspike)),
			new FillableEntry(1, typeof(WondershotCrossbow)),
			new FillableEntry(1, typeof(Xcalibur)),
			new FillableEntry(1, typeof(YumiOfEmpressJingu)),
			new FillableEntry(1, typeof(ZhugeFeathersFan)),
			new FillableEntry(1, typeof(Zulfiqar)),
			new FillableEntry(1, typeof(AbbasidsTreasureChest)),
			new FillableEntry(1, typeof(AbyssalPlaneChest)),
			new FillableEntry(1, typeof(AlehouseChest)),
			new FillableEntry(1, typeof(AlienArtifactChest)),
			new FillableEntry(1, typeof(AlienArtifaxChest)),
			new FillableEntry(1, typeof(AlliedForcesTreasureChest)),
			new FillableEntry(1, typeof(AnarchistsCache)),
			new FillableEntry(1, typeof(AncientRelicChest)),
			new FillableEntry(1, typeof(AngelBlessingChest)),
			new FillableEntry(1, typeof(AnglersBounty)),
			new FillableEntry(1, typeof(ArcadeKingsTreasure)),
			new FillableEntry(1, typeof(ArcadeMastersVault)),
			new FillableEntry(1, typeof(ArcaneTreasureChest)),
			new FillableEntry(1, typeof(ArcanumChest)),
			new FillableEntry(1, typeof(ArcheryBonusChest)),
			new FillableEntry(1, typeof(AshokasTreasureChest)),
			new FillableEntry(1, typeof(AshokaTreasureChest)),
			new FillableEntry(1, typeof(AssassinsCoffer)),
			new FillableEntry(1, typeof(AthenianTreasureChest)),
			new FillableEntry(1, typeof(BabylonianChest)),
			new FillableEntry(1, typeof(BakersDelightChest)),
			new FillableEntry(1, typeof(BakersDolightChest)),
			new FillableEntry(1, typeof(BavarianFestChest)),
			new FillableEntry(1, typeof(BismarcksTreasureChest)),
			new FillableEntry(1, typeof(BolsheviksLoot)),
			new FillableEntry(1, typeof(BountyHuntersCache)),
			new FillableEntry(1, typeof(BoyBandBox)),
			new FillableEntry(1, typeof(BrewmastersChest)),
			new FillableEntry(1, typeof(BritainsRoyalTreasuryChest)),
			new FillableEntry(1, typeof(BuccaneersChest)),
			new FillableEntry(1, typeof(CaesarChest)),
			new FillableEntry(1, typeof(CandyCarnivalCoffer)),
			new FillableEntry(1, typeof(CaptainCooksTreasure)),
			new FillableEntry(1, typeof(CelticLegendsChest)),
			new FillableEntry(1, typeof(ChamplainTreasureChest)),
			new FillableEntry(1, typeof(CheeseConnoisseursCache)),
			new FillableEntry(1, typeof(ChocolatierTreasureChest)),
			new FillableEntry(1, typeof(CivilRightsStrongbox)),
			new FillableEntry(1, typeof(CivilWarCache)),
			new FillableEntry(1, typeof(CivilWarChest)),
			new FillableEntry(1, typeof(CivilWorChest)),
			new FillableEntry(1, typeof(ClownsWhimsicalChest)),
			new FillableEntry(1, typeof(ColonialPioneersCache)),
			new FillableEntry(1, typeof(ComradesCache)),
			new FillableEntry(1, typeof(ConfederationCache)),
			new FillableEntry(1, typeof(ConquistadorsHoard)),
			new FillableEntry(1, typeof(CovenTreasuresChest)),
			new FillableEntry(1, typeof(CyberneticCache)),
			new FillableEntry(1, typeof(CyrusTreasure)),
			new FillableEntry(1, typeof(DesertPharaohChest)),
			new FillableEntry(1, typeof(DinerDelightChest)),
			new FillableEntry(1, typeof(DoctorsBag)),
			new FillableEntry(1, typeof(DojoLegacyChest)),
			new FillableEntry(1, typeof(DragonGuardiansHoardChest)),
			new FillableEntry(1, typeof(DragonHoardChest)),
			new FillableEntry(1, typeof(DragonHoChest)),
			new FillableEntry(1, typeof(DragonHodChest)),
			new FillableEntry(1, typeof(DragonHorChest)),
			new FillableEntry(1, typeof(DriveInTreasureTrove)),
			new FillableEntry(1, typeof(DroidWorkshopChest)),
			new FillableEntry(1, typeof(DynastyRelicsChest)),
			new FillableEntry(1, typeof(EdisonsTreasureChest)),
			new FillableEntry(1, typeof(EgyptianChest)),
			new FillableEntry(1, typeof(EliteFoursVault)),
			new FillableEntry(1, typeof(ElvenEnchantressChest)),
			new FillableEntry(1, typeof(ElvenTreasuryChest)),
			new FillableEntry(1, typeof(EmeraldIsleChest)),
			new FillableEntry(1, typeof(EmperorJustinianCache)),
			new FillableEntry(1, typeof(EmperorLegacyChest)),
			new FillableEntry(1, typeof(EnchantedForestChest)),
			new FillableEntry(1, typeof(EtherealPlaneChest)),
			new FillableEntry(1, typeof(EuropeanRelicsChest)),
			new FillableEntry(1, typeof(FairyDustChest)),
			new FillableEntry(1, typeof(FirstNationsHeritageChest)),
			new FillableEntry(1, typeof(FlowerPowerChest)),
			new FillableEntry(1, typeof(FocusBonusChest)),
			new FillableEntry(1, typeof(ForbiddenAlchemistsCache)),
			new FillableEntry(1, typeof(FrontierExplorersStash)),
			new FillableEntry(1, typeof(FunkyFashionChest)),
			new FillableEntry(1, typeof(FurTradersChest)),
			new FillableEntry(1, typeof(GalacticExplorersTrove)),
			new FillableEntry(1, typeof(GalacticRelicsChest)),
			new FillableEntry(1, typeof(GamersLootbox)),
			new FillableEntry(1, typeof(GardenersParadiseChest)),
			new FillableEntry(1, typeof(GeishasGift)),
			new FillableEntry(1, typeof(GermanUnificationChest)),
			new FillableEntry(1, typeof(GoldRushBountyChest)),
			new FillableEntry(1, typeof(GoldRushRelicChest)),
			new FillableEntry(1, typeof(GreasersGoldmineChest)),
			new FillableEntry(1, typeof(GroovyVabesChest)),
			new FillableEntry(1, typeof(GroovyVibesChest)),
			new FillableEntry(1, typeof(GrungeRockersCache)),
			new FillableEntry(1, typeof(HipHopRapVault)),
			new FillableEntry(1, typeof(HolyRomanEmpireChest)),
			new FillableEntry(1, typeof(HomewardBoundChest)),
			new FillableEntry(1, typeof(HussarsChest)),
			new FillableEntry(1, typeof(InfernalPlaneChest)),
			new FillableEntry(1, typeof(InnovatorVault)),
			new FillableEntry(1, typeof(JedisReliquary)),
			new FillableEntry(1, typeof(JestersGigglingChest)),
			new FillableEntry(1, typeof(JestersJamboreeChest)),
			new FillableEntry(1, typeof(JestersJest)),
			new FillableEntry(1, typeof(JudahsTreasureChest)),
			new FillableEntry(1, typeof(JukeboxJewels)),
			new FillableEntry(1, typeof(JustinianTreasureChest)),
			new FillableEntry(1, typeof(KagesTreasureChest)),
			new FillableEntry(1, typeof(KingdomsVaultChest)),
			new FillableEntry(1, typeof(KingKamehamehaTreasure)),
			new FillableEntry(1, typeof(KingsBest)),
			new FillableEntry(1, typeof(KoscheisUndyingChest)),
			new FillableEntry(1, typeof(LawyerBriefcase)),
			new FillableEntry(1, typeof(LeprechaunsLootChest)),
			new FillableEntry(1, typeof(LeprechaunsTrove)),
			new FillableEntry(1, typeof(LouisTreasuryChest)),
			new FillableEntry(1, typeof(MacingBonusChest)),
			new FillableEntry(1, typeof(MagesArcaneChest)),
			new FillableEntry(1, typeof(MagesRelicChest)),
			new FillableEntry(1, typeof(MaharajaTreasureChest)),
			new FillableEntry(1, typeof(MarioTreasureBox)),
			new FillableEntry(1, typeof(MedievalEnglandChest)),
			new FillableEntry(1, typeof(MerchantChest)),
			new FillableEntry(1, typeof(MerchantFortuneChest)),
			new FillableEntry(1, typeof(MermaidTreasureChest)),
			new FillableEntry(1, typeof(MillenniumTimeCapsule)),
			new FillableEntry(1, typeof(MimeSilentChest)),
			new FillableEntry(1, typeof(MirageChest)),
			new FillableEntry(1, typeof(ModMadnessTrunk)),
			new FillableEntry(1, typeof(MondainsDarkSecretsChest)),
			new FillableEntry(1, typeof(MysticalDaoChest)),
			new FillableEntry(1, typeof(MysticalEnchantersChest)),
			new FillableEntry(1, typeof(MysticEnigmaChest)),
			new FillableEntry(1, typeof(MysticGardenCache)),
			new FillableEntry(1, typeof(MysticMoonChest)),
			new FillableEntry(1, typeof(NaturesBountyChest)),
			new FillableEntry(1, typeof(NavyCaptainsChest)),
			new FillableEntry(1, typeof(NecroAlchemicalChest)),
			new FillableEntry(1, typeof(NeonNightsChest)),
			new FillableEntry(1, typeof(NeroChest)),
			new FillableEntry(1, typeof(NinjaChest)),
			new FillableEntry(1, typeof(NordicExplorersChest)),
			new FillableEntry(1, typeof(PatriotCache)),
			new FillableEntry(1, typeof(PeachRoyalCache)),
			new FillableEntry(1, typeof(PharaohsReliquary)),
			new FillableEntry(1, typeof(PharaohsTreasure)),
			new FillableEntry(1, typeof(PixieDustChest)),
			new FillableEntry(1, typeof(PokeballTreasureChest)),
			new FillableEntry(1, typeof(PolishRoyalChest)),
			new FillableEntry(1, typeof(PopStarsTrove)),
			new FillableEntry(1, typeof(RadBoomboxTrove)),
			new FillableEntry(1, typeof(Radical90sRelicsChest)),
			new FillableEntry(1, typeof(RadRidersStash)),
			new FillableEntry(1, typeof(RailwayWorkersChest)),
			new FillableEntry(1, typeof(RebelChest)),
			new FillableEntry(1, typeof(RenaissanceCollectorsChest)),
			new FillableEntry(1, typeof(RetroArcadeChest)),
			new FillableEntry(1, typeof(RevolutionaryChess)),
			new FillableEntry(1, typeof(RevolutionaryChest)),
			new FillableEntry(1, typeof(RevolutionaryRelicChest)),
			new FillableEntry(1, typeof(RevolutionChest)),
			new FillableEntry(1, typeof(RhineValleyChest)),
			new FillableEntry(1, typeof(RiverPiratesChest)),
			new FillableEntry(1, typeof(RiverRaftersChest)),
			new FillableEntry(1, typeof(RockersVault)),
			new FillableEntry(1, typeof(RockNBallVault)),
			new FillableEntry(1, typeof(RockNRallVault)),
			new FillableEntry(1, typeof(RockNRollVault)),
			new FillableEntry(1, typeof(RoguesHiddenChest)),
			new FillableEntry(1, typeof(RomanBritanniaChest)),
			new FillableEntry(1, typeof(RomanEmperorsVault)),
			new FillableEntry(1, typeof(SamuraiHonorChest)),
			new FillableEntry(1, typeof(SamuraiStash)),
			new FillableEntry(1, typeof(SandstormChest)),
			new FillableEntry(1, typeof(ScholarEnlightenmentChest)),
			new FillableEntry(1, typeof(SeaDogsChest)),
			new FillableEntry(1, typeof(ShinobiSecretsChest)),
			new FillableEntry(1, typeof(SilkRoadTreasuresChest)),
			new FillableEntry(1, typeof(SilverScreenChest)),
			new FillableEntry(1, typeof(SithsVault)),
			new FillableEntry(1, typeof(SlavicBrosChest)),
			new FillableEntry(1, typeof(SlavicLegendsChest)),
			new FillableEntry(1, typeof(SmugglersCache)),
			new FillableEntry(1, typeof(SocialMediaMavensChest)),
			new FillableEntry(1, typeof(SorceressSecretsChest)),
			new FillableEntry(1, typeof(SpaceRaceCache)),
			new FillableEntry(1, typeof(SpartanTreasureChest)),
			new FillableEntry(1, typeof(SpecialChivalryChest)),
			new FillableEntry(1, typeof(SpecialWoodenChestConstantine)),
			new FillableEntry(1, typeof(SpecialWoodenChestExplorerLegacy)),
			new FillableEntry(1, typeof(SpecialWoodenChestFrench)),
			new FillableEntry(1, typeof(SpecialWoodenChestHelios)),
			new FillableEntry(1, typeof(SpecialWoodenChestIvan)),
			new FillableEntry(1, typeof(SpecialWoodenChestOisin)),
			new FillableEntry(1, typeof(SpecialWoodenChestTomoe)),
			new FillableEntry(1, typeof(SpecialWoodenChestWashington)),
			new FillableEntry(1, typeof(StarfleetsVault)),
			new FillableEntry(1, typeof(SugarplumFairyChest)),
			new FillableEntry(1, typeof(SwingTimeChest)),
			new FillableEntry(1, typeof(SwordsmanshipBonusChest)),
			new FillableEntry(1, typeof(TacticsBonusChest)),
			new FillableEntry(1, typeof(TangDynastyChest)),
			new FillableEntry(1, typeof(TechnicolorTalesChest)),
			new FillableEntry(1, typeof(TechnophilesCache)),
			new FillableEntry(1, typeof(TeutonicRelicChest)),
			new FillableEntry(1, typeof(TeutonicTreasuresChest)),
			new FillableEntry(1, typeof(ThiefsHideawayStash)),
			new FillableEntry(1, typeof(ToxicologistsTrove)),
			new FillableEntry(1, typeof(TrailblazersTrove)),
			new FillableEntry(1, typeof(TravelerChest)),
			new FillableEntry(1, typeof(TreasureChestOfTheQinDynasty)),
			new FillableEntry(1, typeof(TreasureChestOfTheThreeKingdoms)),
			new FillableEntry(1, typeof(TsarsLegacyChest)),
			new FillableEntry(1, typeof(TsarsRoyalChest)),
			new FillableEntry(1, typeof(TsarsTreasureChest)),
			new FillableEntry(1, typeof(TudorDynastyChest)),
			new FillableEntry(1, typeof(UndergroundAnarchistsCache)),
			new FillableEntry(1, typeof(USSRRelicsChest)),
			new FillableEntry(1, typeof(VenetianMerchantsStash)),
			new FillableEntry(1, typeof(VHSAdventureCache)),
			new FillableEntry(1, typeof(VictorianEraChest)),
			new FillableEntry(1, typeof(VikingChest)),
			new FillableEntry(1, typeof(VintnersVault)),
			new FillableEntry(1, typeof(VinylVault)),
			new FillableEntry(1, typeof(VirtuesGuardianChest)),
			new FillableEntry(1, typeof(WarOf1812Vault)),
			new FillableEntry(1, typeof(WarringStatesChest)),
			new FillableEntry(1, typeof(WingedHusChest)),
			new FillableEntry(1, typeof(WingedHussarsChest)),
			new FillableEntry(1, typeof(WitchsBrewChest)),
			new FillableEntry(1, typeof(WorkersRevolutionChest)),
			new FillableEntry(1, typeof(WorldWarIIChest)),
			new FillableEntry(1, typeof(WWIIValorChest)),
			new FillableEntry(1, typeof(MahJongTile)),
			new FillableEntry(1, typeof(AlchemyTalisman)),
			new FillableEntry(1, typeof(CartographyTable)),
			new FillableEntry(1, typeof(StrappedBooks)),
			new FillableEntry(1, typeof(CarpentryTalisman)),
			new FillableEntry(1, typeof(CharcuterieBoard)),
			new FillableEntry(1, typeof(BottledPlague)),
			new FillableEntry(1, typeof(NixieStatue)),
			new FillableEntry(1, typeof(GrandmasSpecialRolls)),
			new FillableEntry(1, typeof(ExoticFish)),
			new FillableEntry(1, typeof(HildebrandtShield)),
			new FillableEntry(1, typeof(GarbageBag)),
			new FillableEntry(1, typeof(GlassOfBubbly)),
			new FillableEntry(1, typeof(FishBasket)),
			new FillableEntry(1, typeof(FineSilverWire)),
			new FillableEntry(1, typeof(PlatinumChip)),
			new FillableEntry(1, typeof(TribalHelm)),
			new FillableEntry(1, typeof(GlassTable)),
			new FillableEntry(1, typeof(WaterWell)),
			new FillableEntry(1, typeof(RibbonAward)),
			new FillableEntry(1, typeof(RookStone)),
			new FillableEntry(1, typeof(AlchemistBookcase)),
			new FillableEntry(1, typeof(ElementRegular)),
			new FillableEntry(1, typeof(Satchel)),
			new FillableEntry(1, typeof(LayingChicken)),
			new FillableEntry(1, typeof(EssenceOfToad)),
			new FillableEntry(1, typeof(SalvageMachine)),
			new FillableEntry(1, typeof(TwentyfiveShield)),
			new FillableEntry(1, typeof(DaggerSign)),
			new FillableEntry(1, typeof(MultiPump)),
			new FillableEntry(1, typeof(HeartPillow)),
			new FillableEntry(1, typeof(HydroxFluid)),
			new FillableEntry(1, typeof(MasterCello)),
			new FillableEntry(1, typeof(DressForm)),
			new FillableEntry(1, typeof(LargeWeatheredBook)),
			new FillableEntry(1, typeof(WeddingCandelabra)),
			new FillableEntry(1, typeof(DeckOfMagicCards)),
			new FillableEntry(1, typeof(BrokenBottle)),
			new FillableEntry(1, typeof(FancyHornOfPlenty)),
			new FillableEntry(1, typeof(FineGoldWire)),
			new FillableEntry(1, typeof(WaterRelic)),
			new FillableEntry(1, typeof(EnchantedAnnealer)),
			new FillableEntry(1, typeof(LampPostC)),
			new FillableEntry(1, typeof(BlueSand)),
			new FillableEntry(1, typeof(FunMushroom)),
			new FillableEntry(1, typeof(ReactiveHormones)),
			new FillableEntry(1, typeof(CandleStick)),
			new FillableEntry(1, typeof(LuckyDice)),
			new FillableEntry(1, typeof(JudasCradle)),
			new FillableEntry(1, typeof(GlassFurnace)),
			new FillableEntry(1, typeof(LovelyLilies)),
			new FillableEntry(1, typeof(CupOfSlime)),
			new FillableEntry(1, typeof(GingerbreadCookie)),
			new FillableEntry(1, typeof(CarvingMachine)),
			new FillableEntry(1, typeof(GargoyleLamp)),
			new FillableEntry(1, typeof(AnimalTopiary)),
			new FillableEntry(1, typeof(TinCowbell)),
			new FillableEntry(1, typeof(WoodAlchohol)),
			new FillableEntry(1, typeof(ChocolateFountain)),
			new FillableEntry(1, typeof(PowerGem)),
			new FillableEntry(1, typeof(AtomicRegulator)),
			new FillableEntry(1, typeof(JesterSkull)),
			new FillableEntry(1, typeof(GamerGirlFeet)),
			new FillableEntry(1, typeof(HildebrandtTapestry)),
			new FillableEntry(1, typeof(AnimalBox)),
			new FillableEntry(1, typeof(BakingBoard)),
			new FillableEntry(1, typeof(SatanicTable)),
			new FillableEntry(1, typeof(WaterFountain)),
			new FillableEntry(1, typeof(FountainWall)),
			new FillableEntry(1, typeof(HildebrandtFlag)),
			new FillableEntry(1, typeof(MysteryOrb)),
			new FillableEntry(1, typeof(BlueberryPie)),
			new FillableEntry(1, typeof(BioSamples)),
			new FillableEntry(1, typeof(OldEmbroideryTool)),
			new FillableEntry(1, typeof(DistillationFlask)),
			new FillableEntry(1, typeof(SexWhip)),
			new FillableEntry(1, typeof(FrostToken)),
			new FillableEntry(1, typeof(SoftTowel)),
			new FillableEntry(1, typeof(WeddingDayCake)),
			new FillableEntry(1, typeof(LargeTome)),
			new FillableEntry(1, typeof(GargishTotem)),
			new FillableEntry(1, typeof(InscriptionTalisman)),
			new FillableEntry(1, typeof(HeavyAnchor)),
			new FillableEntry(1, typeof(PunishmentStocks)),
			new FillableEntry(1, typeof(StarMap)),
			new FillableEntry(1, typeof(SkullRing)),
			new FillableEntry(1, typeof(BrandingIron)),
			new FillableEntry(1, typeof(OldBones)),
			new FillableEntry(1, typeof(MillStones)),
			new FillableEntry(1, typeof(Steroids)),
			new FillableEntry(1, typeof(HildebrandtBunting)),
			new FillableEntry(1, typeof(LexxVase)),
			new FillableEntry(1, typeof(OrnateHarp)),
			new FillableEntry(1, typeof(FletchingTalisman)),
			new FillableEntry(1, typeof(TinTub)),
			new FillableEntry(1, typeof(FishingBear)),
			new FillableEntry(1, typeof(WorldShard)),
			new FillableEntry(1, typeof(SheepCarcass)),
			new FillableEntry(1, typeof(TailoringTalisman)),
			new FillableEntry(1, typeof(DecorativeOrchid)),
			new FillableEntry(1, typeof(SubOil)),
			new FillableEntry(1, typeof(FancyPainting)),
			new FillableEntry(1, typeof(MedusaHead)),
			new FillableEntry(1, typeof(PetRock)),
			new FillableEntry(1, typeof(MeatHooks)),
			new FillableEntry(1, typeof(GamerJelly)),
			new FillableEntry(1, typeof(ShardCrest)),
			new FillableEntry(1, typeof(EvilCandle)),
			new FillableEntry(1, typeof(HotFlamingScarecrow)),
			new FillableEntry(1, typeof(AmatureTelescope)),
			new FillableEntry(1, typeof(AnniversaryPainting)),
			new FillableEntry(1, typeof(MagicBookStand)),
			new FillableEntry(1, typeof(SmugglersCrate)),
			new FillableEntry(1, typeof(HolidayPillow)),
			new FillableEntry(1, typeof(UncrackedGeode)),
			new FillableEntry(1, typeof(DrapedBlanket)),
			new FillableEntry(1, typeof(BlacksmithTalisman)),
			new FillableEntry(1, typeof(CityBanner)),
			new FillableEntry(1, typeof(BookTwentyfive)),
			new FillableEntry(1, typeof(WelcomeMat)),
			new FillableEntry(1, typeof(HolidayCandleArran)),
			new FillableEntry(1, typeof(ValentineTeddybear)),
			new FillableEntry(1, typeof(WitchesCauldron)),
			new FillableEntry(1, typeof(FireRelic)),
			new FillableEntry(1, typeof(CartographyDesk)),
			new FillableEntry(1, typeof(BirdStatue)),
			new FillableEntry(1, typeof(Hamburgers)),
			new FillableEntry(1, typeof(SerpantCrest)),
			new FillableEntry(1, typeof(AstroLabe)),
			new FillableEntry(1, typeof(OrganicHeart)),
			new FillableEntry(1, typeof(Birdhouse)),
			new FillableEntry(1, typeof(SabertoothSkull)),
			new FillableEntry(1, typeof(ZebulinVase)),
			new FillableEntry(1, typeof(PineResin)),
			new FillableEntry(1, typeof(SpiderTree)),
			new FillableEntry(1, typeof(RopeSpindle)),
			new FillableEntry(1, typeof(DailyNewspaper)),
			new FillableEntry(1, typeof(FunPumpkinCannon)),
			new FillableEntry(1, typeof(VirtueRune)),
			new FillableEntry(1, typeof(BonFire)),
			new FillableEntry(1, typeof(MiniCherryTree)),
			new FillableEntry(1, typeof(SkullShield)),
			new FillableEntry(1, typeof(UnluckyDice)),
			new FillableEntry(1, typeof(ImportantBooks)),
			new FillableEntry(1, typeof(RareMinerals)),
			new FillableEntry(1, typeof(QuestWineRack)),
			new FillableEntry(1, typeof(FancyCrystalSkull)),
			new FillableEntry(1, typeof(StoneHead)),
			new FillableEntry(1, typeof(FluxCompound)),
			new FillableEntry(1, typeof(ArtisanHolidayTree)),
			new FillableEntry(1, typeof(TinyWizard)),
			new FillableEntry(1, typeof(MiniKeg)),
			new FillableEntry(1, typeof(DemonPlatter)),
			new FillableEntry(1, typeof(Hotdogs)),
			new FillableEntry(1, typeof(PersonalMortor)),
			new FillableEntry(1, typeof(RareWire)),
			new FillableEntry(1, typeof(ForbiddenTome)),
			new FillableEntry(1, typeof(CompoundF)),
			new FillableEntry(1, typeof(MonsterBones)),
			new FillableEntry(1, typeof(MagicOrb)),
			new FillableEntry(1, typeof(RibEye)),
			new FillableEntry(1, typeof(ButterChurn)),
			new FillableEntry(1, typeof(StainedWindow)),
			new FillableEntry(1, typeof(MutantStarfish)),
			new FillableEntry(1, typeof(WatermelonSliced)),
			new FillableEntry(1, typeof(ImperiumCrest)),
			new FillableEntry(1, typeof(ExoticWoods)),
			new FillableEntry(1, typeof(ZombieHand)),
			new FillableEntry(1, typeof(EasterDayEgg)),
			new FillableEntry(1, typeof(FancyXmasTree)),
			new FillableEntry(1, typeof(CrabBushel)),
			new FillableEntry(1, typeof(PersonalCannon)),
			new FillableEntry(1, typeof(GalvanizedTub)),
			new FillableEntry(1, typeof(FixedScales)),
			new FillableEntry(1, typeof(NexusShard)),
			new FillableEntry(1, typeof(RareSausage)),
			new FillableEntry(1, typeof(WatermelonFruit)),
			new FillableEntry(1, typeof(ToolBox)),
			new FillableEntry(1, typeof(HorrodrickCube)),
			new FillableEntry(1, typeof(TrophieAward)),
			new FillableEntry(1, typeof(ClutteredDesk)),
			new FillableEntry(1, typeof(TinkeringTalisman)),
			new FillableEntry(1, typeof(DeathBlowItem)),
			new FillableEntry(1, typeof(DeadBody)),
			new FillableEntry(1, typeof(FairyOil)),
			new FillableEntry(1, typeof(FleshLight)),
			new FillableEntry(1, typeof(EssentialBooks)),
			new FillableEntry(1, typeof(PileOfChains)),
			new FillableEntry(1, typeof(FancyCopperSunflower)),
			new FillableEntry(1, typeof(FineHoochJug)),
			new FillableEntry(1, typeof(SpookyGhost)),
			new FillableEntry(1, typeof(ScentedCandle)),
			new FillableEntry(1, typeof(SnowSculpture)),
			new FillableEntry(1, typeof(MasterShrubbery)),
			new FillableEntry(1, typeof(MilkingPail)),
			new FillableEntry(1, typeof(MonsterTrophy)),
			new FillableEntry(1, typeof(DistilledEssence)),
			new FillableEntry(1, typeof(WigStand)),
			new FillableEntry(1, typeof(BrassFountain)),
			new FillableEntry(1, typeof(HandOfFate)),
			new FillableEntry(1, typeof(NameTapestry)),
			new FillableEntry(1, typeof(OpalBranch)),
			new FillableEntry(1, typeof(SilverMirror)),
			new FillableEntry(1, typeof(MarbleHourglass)),
			new FillableEntry(1, typeof(KnightStone)),
			new FillableEntry(1, typeof(MaxxiaDust)),
			new FillableEntry(1, typeof(TrexSkull)),
			new FillableEntry(1, typeof(RootThrone)),
			new FillableEntry(1, typeof(BabyLavos)),
			new FillableEntry(1, typeof(CowToken)),
			new FillableEntry(1, typeof(WallFlowers)),
			new FillableEntry(1, typeof(ColoredLamppost)),
			new FillableEntry(1, typeof(EarthRelic)),
			new FillableEntry(1, typeof(ExoticWhistle)),
			new FillableEntry(1, typeof(GrandmasKnitting)),
			new FillableEntry(1, typeof(FancyShipWheel)),
			new FillableEntry(1, typeof(ExoticBoots)),
			new FillableEntry(1, typeof(FineIronWire)),
			new FillableEntry(1, typeof(SeaSerpantSteak)),
			new FillableEntry(1, typeof(RareOil)),
			new FillableEntry(1, typeof(SpikedChair)),
			new FillableEntry(1, typeof(KrakenTrophy)),
			new FillableEntry(1, typeof(ZttyCrystal)),
			new FillableEntry(1, typeof(FineCopperWire)),
			new FillableEntry(1, typeof(GraniteHammer)),
			new FillableEntry(1, typeof(StuffedDoll)),
			new FillableEntry(1, typeof(FancyMirror)),
			new FillableEntry(1, typeof(ExoticPlum)),
			new FillableEntry(1, typeof(Shears)),
			new FillableEntry(1, typeof(FillerPowder)),
			new FillableEntry(1, typeof(HorrorPumpkin)),
			new FillableEntry(1, typeof(HumanCarvingKit)),
			new FillableEntry(1, typeof(HangingMask)),
			new FillableEntry(1, typeof(SkullIncense)),
			new FillableEntry(1, typeof(AncientRunes)),
			new FillableEntry(1, typeof(WeddingChest)),
			new FillableEntry(1, typeof(ExoticShipInABottle)),
			new FillableEntry(1, typeof(FancySewingMachine)),
			new FillableEntry(1, typeof(CowPoo)),
			new FillableEntry(1, typeof(FeedingTrough)),
			new FillableEntry(1, typeof(SkullBottle)),
			new FillableEntry(1, typeof(AncientDrum)),
			new FillableEntry(1, typeof(BrassBell)),
			new FillableEntry(1, typeof(PlagueBanner)),
			new FillableEntry(1, typeof(BeeHive)),
			new FillableEntry(1, typeof(FestiveChampagne)),
			new FillableEntry(1, typeof(DecorativeFishTank)),
			new FillableEntry(1, typeof(SkullsStack)),
			new FillableEntry(1, typeof(VenusFlytrap)),
			new FillableEntry(1, typeof(HorribleMask)),
			new FillableEntry(1, typeof(Meteorite)),
			new FillableEntry(1, typeof(TotumPole)),
			new FillableEntry(1, typeof(ExcitingTome)),
			new FillableEntry(1, typeof(WindRelic)),
			new FillableEntry(1, typeof(FancyCopperWings)),
			new FillableEntry(1, typeof(RareGrease)),
			new FillableEntry(1, typeof(EnchantedRose)),
			new FillableEntry(1, typeof(HeartChair)),
			new FillableEntry(1, typeof(PicnicDayBasket)),
			new FillableEntry(1, typeof(HorseToken)),
			new FillableEntry(1, typeof(AutoPounder)),
			new FillableEntry(1, typeof(WatermelonTray)),
			new FillableEntry(1, typeof(MasterTrumpet)),
			new FillableEntry(1, typeof(WeaponBottle)),
			new FillableEntry(1, typeof(LeatherStrapBelt)),
			new FillableEntry(1, typeof(JackPumpkin)),
			new FillableEntry(1, typeof(MiniYew)),
			new FillableEntry(1, typeof(ImportantFlag)),
			new FillableEntry(1, typeof(HutFlower)),
			new FillableEntry(1, typeof(CookingTalisman)),
			new FillableEntry(1, typeof(LampPostA)),
			new FillableEntry(1, typeof(LampPostB)),
			new FillableEntry(1, typeof(MasterGyro)),
			new FillableEntry(1, typeof(MemorialStone)),
			new FillableEntry(1, typeof(MemorialCopper)),
			new FillableEntry(1, typeof(LargeVat)),
			new FillableEntry(1, typeof(YardCupid)),
			new FillableEntry(1, typeof(FieldPlow)),
			new FillableEntry(1, typeof(ChaliceOfPilfering)),
			new FillableEntry(1, typeof(Jet)),
			new FillableEntry(1, typeof(InfinitySymbol)),
			new FillableEntry(1, typeof(AbyssalCrownOfCommand)),
			new FillableEntry(1, typeof(AcidlordsCrown)),
			new FillableEntry(1, typeof(AcidSlugRing)),
			new FillableEntry(1, typeof(AlchemistsExperimentalBracers)),
			new FillableEntry(1, typeof(AmuletOfCorrosion)),
			new FillableEntry(1, typeof(AmuletOfCorruption)),
			new FillableEntry(1, typeof(AmuletOfTheCrystalDepths)),
			new FillableEntry(1, typeof(AmuletOfTheMeerCaptain)),
			new FillableEntry(1, typeof(AntLionsEmbraceLeggings)),
			new FillableEntry(1, typeof(ArachnidQueensVeil)),
			new FillableEntry(1, typeof(ArchmageMantle)),
			new FillableEntry(1, typeof(BeastmastersMantle)),
			new FillableEntry(1, typeof(BeastmastersSaddle)),
			new FillableEntry(1, typeof(BeetlemastersCarapace)),
			new FillableEntry(1, typeof(BetrayersShroud)),
			new FillableEntry(1, typeof(BirdmastersPlume)),
			new FillableEntry(1, typeof(BlackBearSummoningAmulet)),
			new FillableEntry(1, typeof(BloodFoxCloak)),
			new FillableEntry(1, typeof(BloodmastersRobes)),
			new FillableEntry(1, typeof(BloodwormBreastplate)),
			new FillableEntry(1, typeof(BoglingsEmbrace)),
			new FillableEntry(1, typeof(BogThingsEmbrace)),
			new FillableEntry(1, typeof(BoneAmulet)),
			new FillableEntry(1, typeof(BoneKnightArmor)),
			new FillableEntry(1, typeof(BootsofHaste)),
			new FillableEntry(1, typeof(BrigandLordCloak)),
			new FillableEntry(1, typeof(BrigandsBand)),
			new FillableEntry(1, typeof(BrigandsCloak)),
			new FillableEntry(1, typeof(BronzeGauntlets)),
			new FillableEntry(1, typeof(BullmastersHelm)),
			new FillableEntry(1, typeof(CapybaraWhispererSash)),
			new FillableEntry(1, typeof(CelestialMantle)),
			new FillableEntry(1, typeof(CentaurianCloak)),
			new FillableEntry(1, typeof(ChangelingsEmbrace)),
			new FillableEntry(1, typeof(ChaosbringerShroud)),
			new FillableEntry(1, typeof(ChaosDragoonArmor)),
			new FillableEntry(1, typeof(ChaosDragoonChestplate)),
			new FillableEntry(1, typeof(ChaoslordsMantle)),
			new FillableEntry(1, typeof(ChestOfTheHiveLord)),
			new FillableEntry(1, typeof(ChickenWranglerSash)),
			new FillableEntry(1, typeof(ChitinousCloak)),
			new FillableEntry(1, typeof(ChitinousHelm)),
			new FillableEntry(1, typeof(CloakOfEffervescentMantra)),
			new FillableEntry(1, typeof(CloakOfEtherealWings)),
			new FillableEntry(1, typeof(CloakOfMoltenSerpents)),
			new FillableEntry(1, typeof(CloakOfTheGargoylesVengeance)),
			new FillableEntry(1, typeof(CloakOfTheHellCat)),
			new FillableEntry(1, typeof(CloakOfTheHerd)),
			new FillableEntry(1, typeof(CloakOfTheNightborn)),
			new FillableEntry(1, typeof(CloakOfTheWailingBanshee)),
			new FillableEntry(1, typeof(CloakOfTwistingVines)),
			new FillableEntry(1, typeof(CloakOfViscera)),
			new FillableEntry(1, typeof(CloakOfWinds)),
			new FillableEntry(1, typeof(CluckmasterCloak)),
			new FillableEntry(1, typeof(CoconutCrabCharm)),
			new FillableEntry(1, typeof(CopperCrown)),
			new FillableEntry(1, typeof(CraneMastersRobe)),
			new FillableEntry(1, typeof(CrimsonDragonEmbrace)),
			new FillableEntry(1, typeof(CrownOfMaddeningWhispers)),
			new FillableEntry(1, typeof(CrownOfTheInfernalKing)),
			new FillableEntry(1, typeof(CrownOfTheLichKing)),
			new FillableEntry(1, typeof(CrystalDaemonAmulet)),
			new FillableEntry(1, typeof(CrystalElementalistRobes)),
			new FillableEntry(1, typeof(CrystalSummonersCirclet)),
			new FillableEntry(1, typeof(CyclopeanWarriorHelm)),
			new FillableEntry(1, typeof(DancerRobes)),
			new FillableEntry(1, typeof(DarkArchmagesShroud)),
			new FillableEntry(1, typeof(DarkGuardianAmulet)),
			new FillableEntry(1, typeof(DeathWatchCarapace)),
			new FillableEntry(1, typeof(DecayedBoneHelm)),
			new FillableEntry(1, typeof(DeepSeaSurgeCrown)),
			new FillableEntry(1, typeof(DemonicOniMask)),
			new FillableEntry(1, typeof(DesertWanderersSandals)),
			new FillableEntry(1, typeof(DestroyersAegis)),
			new FillableEntry(1, typeof(DevourerCloak)),
			new FillableEntry(1, typeof(DolphinsGuardianAmulet)),
			new FillableEntry(1, typeof(DoomcallersMantle)),
			new FillableEntry(1, typeof(DragonheartSamuraiChest)),
			new FillableEntry(1, typeof(DragonlordsHelm)),
			new FillableEntry(1, typeof(DragonmasterAmulet)),
			new FillableEntry(1, typeof(DragonmastersRobe)),
			new FillableEntry(1, typeof(DragonsFlameGrandMageStaff)),
			new FillableEntry(1, typeof(DragonsHeartBreastplate)),
			new FillableEntry(1, typeof(DragonWolfAmulet)),
			new FillableEntry(1, typeof(DrakeCommandersBreastplate)),
			new FillableEntry(1, typeof(DrakescaleCloak)),
			new FillableEntry(1, typeof(DreamWraithCloak)),
			new FillableEntry(1, typeof(DullCopperAegis)),
			new FillableEntry(1, typeof(EarthWardensPlateChest)),
			new FillableEntry(1, typeof(ElderGazerAmulet)),
			new FillableEntry(1, typeof(EldersGauntlets)),
			new FillableEntry(1, typeof(ElementalCommandCrown)),
			new FillableEntry(1, typeof(ElementalistsCrown)),
			new FillableEntry(1, typeof(EternalWisdomRobe)),
			new FillableEntry(1, typeof(EttinSummonerHelm)),
			new FillableEntry(1, typeof(ExecutionersCloak)),
			new FillableEntry(1, typeof(ExodusVanguardPlate)),
			new FillableEntry(1, typeof(FeatheredFarmersApron)),
			new FillableEntry(1, typeof(FerretSummonersCloak)),
			new FillableEntry(1, typeof(FireAntAmulet)),
			new FillableEntry(1, typeof(FlamecallersCrown)),
			new FillableEntry(1, typeof(FlamesoulShroud)),
			new FillableEntry(1, typeof(FlamingVeilOfTheFireRabbit)),
			new FillableEntry(1, typeof(FleshRenderersCloak)),
			new FillableEntry(1, typeof(ForgottenMantle)),
			new FillableEntry(1, typeof(FrostbindAmulet)),
			new FillableEntry(1, typeof(FrostbindersCloak)),
			new FillableEntry(1, typeof(FrostboundWarhelm)),
			new FillableEntry(1, typeof(FrostcallersCape)),
			new FillableEntry(1, typeof(FrostcallersCloak)),
			new FillableEntry(1, typeof(FrostcallersShard)),
			new FillableEntry(1, typeof(FrostDrakeCloak)),
			new FillableEntry(1, typeof(FrostlaceCloak)),
			new FillableEntry(1, typeof(FrostLordsCape)),
			new FillableEntry(1, typeof(FrostMantleOfTheGlacier)),
			new FillableEntry(1, typeof(FrostTrollCrown)),
			new FillableEntry(1, typeof(FrostwardCape)),
			new FillableEntry(1, typeof(FrostweaverCloak)),
			new FillableEntry(1, typeof(FrostwraithCloak)),
			new FillableEntry(1, typeof(FrostwyrmDominionStaff)),
			new FillableEntry(1, typeof(GargishStoneChestOfTheHiveLord)),
			new FillableEntry(1, typeof(GargoyleMastersRobes)),
			new FillableEntry(1, typeof(GargoylesEmbrace)),
			new FillableEntry(1, typeof(GargoyleShadesShroud)),
			new FillableEntry(1, typeof(GargoyleSummonerHelm)),
			new FillableEntry(1, typeof(GargoyleSummonerSash)),
			new FillableEntry(1, typeof(GargoylesVeil)),
			new FillableEntry(1, typeof(GazerLordAmulet)),
			new FillableEntry(1, typeof(GazersEmbrace)),
			new FillableEntry(1, typeof(GibberlingSummonersHood)),
			new FillableEntry(1, typeof(GlacialCommandersPlate)),
			new FillableEntry(1, typeof(GoblinsCloakOfMischief)),
			new FillableEntry(1, typeof(GoblinShamansMantle)),
			new FillableEntry(1, typeof(GoblinWardAmulet)),
			new FillableEntry(1, typeof(GoldenAegis)),
			new FillableEntry(1, typeof(GolemControllerBracers)),
			new FillableEntry(1, typeof(GolemforgeChestplate)),
			new FillableEntry(1, typeof(GravekeepersMantle)),
			new FillableEntry(1, typeof(GrayGoblinShamanMask)),
			new FillableEntry(1, typeof(GregoriosMantle)),
			new FillableEntry(1, typeof(GremlinSummonerCap)),
			new FillableEntry(1, typeof(GreyWolfSummonerCloak)),
			new FillableEntry(1, typeof(GrizzledRingOfDread)),
			new FillableEntry(1, typeof(GrizzlyGuardiansHelm)),
			new FillableEntry(1, typeof(HarpyQueensCrown)),
			new FillableEntry(1, typeof(HeadlessHorrorHelm)),
			new FillableEntry(1, typeof(HeartOfTheSlith)),
			new FillableEntry(1, typeof(HellfireCrown)),
			new FillableEntry(1, typeof(HellfireGreaves)),
			new FillableEntry(1, typeof(HelmOfThePlagueLord)),
			new FillableEntry(1, typeof(HelmOfTheUndeadMaster)),
			new FillableEntry(1, typeof(HordeMastersMask)),
			new FillableEntry(1, typeof(HornedCrownOfTheLabyrinth)),
			new FillableEntry(1, typeof(HornOfTheLabyrinth)),
			new FillableEntry(1, typeof(HydraSummonerHelm)),
			new FillableEntry(1, typeof(InfernalCloakOfHounds)),
			new FillableEntry(1, typeof(InfernalGrasp)),
			new FillableEntry(1, typeof(InfernalPactBracelet)),
			new FillableEntry(1, typeof(InfernalPactCloak)),
			new FillableEntry(1, typeof(InfernalPactRobe)),
			new FillableEntry(1, typeof(InfernalRiderCloak)),
			new FillableEntry(1, typeof(InfernalSummonerRobe)),
			new FillableEntry(1, typeof(InfernalSummonersBand)),
			new FillableEntry(1, typeof(InfernosEmbraceCloak)),
			new FillableEntry(1, typeof(InfiltratorsCloak)),
			new FillableEntry(1, typeof(InsectlordsGauntlets)),
			new FillableEntry(1, typeof(IronSentinelPlate)),
			new FillableEntry(1, typeof(JackRabbitCharm)),
			new FillableEntry(1, typeof(JuggernautsMightyGirdle)),
			new FillableEntry(1, typeof(JukaWarlordsCloak)),
			new FillableEntry(1, typeof(KepetchShroudOfShadows)),
			new FillableEntry(1, typeof(KepetchSummonerHelm)),
			new FillableEntry(1, typeof(KhaldunAcolyteRobe)),
			new FillableEntry(1, typeof(KhaldunNecromancersShroud)),
			new FillableEntry(1, typeof(KitsuneSummonerKimono)),
			new FillableEntry(1, typeof(KrakenlordsShroud)),
			new FillableEntry(1, typeof(LabyrinthMastersHelm)),
			new FillableEntry(1, typeof(LatticeAmulet)),
			new FillableEntry(1, typeof(LavaforgedGauntlets)),
			new FillableEntry(1, typeof(LavaLordsEmbrace)),
			new FillableEntry(1, typeof(LeafWovenCirclet)),
			new FillableEntry(1, typeof(LeatherWolfsCowl)),
			new FillableEntry(1, typeof(LeviathansEmbraceShield)),
			new FillableEntry(1, typeof(LichKingsCloak)),
			new FillableEntry(1, typeof(LionheartMantle)),
			new FillableEntry(1, typeof(LizardlordMantle)),
			new FillableEntry(1, typeof(LlamamastersRobes)),
			new FillableEntry(1, typeof(LlamaPackMasterBelt)),
			new FillableEntry(1, typeof(LuminaCrystalRobe)),
			new FillableEntry(1, typeof(MaggotmastersCloak)),
			new FillableEntry(1, typeof(MarshwalkersBoots)),
			new FillableEntry(1, typeof(MechanistsGloves)),
			new FillableEntry(1, typeof(MimicsChestplate)),
			new FillableEntry(1, typeof(MimicsShroud)),
			new FillableEntry(1, typeof(MinotaurLordHornedHelm)),
			new FillableEntry(1, typeof(MistshaperCloak)),
			new FillableEntry(1, typeof(MoltenCoreChestplate)),
			new FillableEntry(1, typeof(MongbatCloak)),
			new FillableEntry(1, typeof(MongbatSummoningCloak)),
			new FillableEntry(1, typeof(NatureGuardiansTalisman)),
			new FillableEntry(1, typeof(NaturesSentinelChestplate)),
			new FillableEntry(1, typeof(NaturesWrathPlateChest)),
			new FillableEntry(1, typeof(NecklaceOfTheEquineLord)),
			new FillableEntry(1, typeof(NecromancerApron)),
			new FillableEntry(1, typeof(NecromancerKiteShield)),
			new FillableEntry(1, typeof(NecromancersBoneChest)),
			new FillableEntry(1, typeof(NecromancersMantle)),
			new FillableEntry(1, typeof(NecroticSash)),
			new FillableEntry(1, typeof(NightmaresMantle)),
			new FillableEntry(1, typeof(NiporailemsShroud)),
			new FillableEntry(1, typeof(OceanmastersRobes)),
			new FillableEntry(1, typeof(OgresGauntlets)),
			new FillableEntry(1, typeof(OphidianKnightRelic)),
			new FillableEntry(1, typeof(OphidianMatriarchSash)),
			new FillableEntry(1, typeof(OphidiansArcaneSash)),
			new FillableEntry(1, typeof(OphidianSerpentineHelm)),
			new FillableEntry(1, typeof(OrcBombersGauntlets)),
			new FillableEntry(1, typeof(OrcishMageRobe)),
			new FillableEntry(1, typeof(OrcishWarchiefsCloak)),
			new FillableEntry(1, typeof(OrcWarchiefsBattleHelm)),
			new FillableEntry(1, typeof(OrcWarlordsPlateChest)),
			new FillableEntry(1, typeof(OrtanordsCrownOfFrost)),
			new FillableEntry(1, typeof(OsseinSummonersHorn)),
			new FillableEntry(1, typeof(OverseerControlCirclet)),
			new FillableEntry(1, typeof(PackmastersCloak)),
			new FillableEntry(1, typeof(ParoxysmusBreastplate)),
			new FillableEntry(1, typeof(PharaohsGoldenCrown)),
			new FillableEntry(1, typeof(PigWhisperersTalisman)),
			new FillableEntry(1, typeof(PixieWhisperRobes)),
			new FillableEntry(1, typeof(PlaguebearersCloak)),
			new FillableEntry(1, typeof(PlaguebearersShroud)),
			new FillableEntry(1, typeof(PlagueBringersHood)),
			new FillableEntry(1, typeof(PlaguebringersTalisman)),
			new FillableEntry(1, typeof(PlaguelordShroud)),
			new FillableEntry(1, typeof(PlagueweaversShroud)),
			new FillableEntry(1, typeof(PredatorClawMantle)),
			new FillableEntry(1, typeof(PredatorsAmulet)),
			new FillableEntry(1, typeof(PrimalStalkerBoots)),
			new FillableEntry(1, typeof(PrimalWardensHelm)),
			new FillableEntry(1, typeof(ProtectorsGuardianShield)),
			new FillableEntry(1, typeof(PutridGuardianRobe)),
			new FillableEntry(1, typeof(PyromancerCuirass)),
			new FillableEntry(1, typeof(QuagmiresMantle)),
			new FillableEntry(1, typeof(QueensGuardRobes)),
			new FillableEntry(1, typeof(QueensSummonerRobe)),
			new FillableEntry(1, typeof(RabbitmastersRobes)),
			new FillableEntry(1, typeof(RaijusEmbrace)),
			new FillableEntry(1, typeof(RancherHat)),
			new FillableEntry(1, typeof(RaptorLordCloak)),
			new FillableEntry(1, typeof(RatcatchersSash)),
			new FillableEntry(1, typeof(RatmanArchersRobes)),
			new FillableEntry(1, typeof(RatmanArchmageRobe)),
			new FillableEntry(1, typeof(RatmastersRobes)),
			new FillableEntry(1, typeof(RatmastersSash)),
			new FillableEntry(1, typeof(RatsMastersRobes)),
			new FillableEntry(1, typeof(RavagersRobes)),
			new FillableEntry(1, typeof(ReaperlordsMantle)),
			new FillableEntry(1, typeof(RedSolenWarriorRobes)),
			new FillableEntry(1, typeof(RedSolenWorkerRobes)),
			new FillableEntry(1, typeof(RenegadeMastersCloak)),
			new FillableEntry(1, typeof(ReptalonMastersRobes)),
			new FillableEntry(1, typeof(RevenantLionCrown)),
			new FillableEntry(1, typeof(RevenantmastersRobes)),
			new FillableEntry(1, typeof(RidersGoldenCloak)),
			new FillableEntry(1, typeof(RidgebackMasterSash)),
			new FillableEntry(1, typeof(RingOfTheJukanCommander)),
			new FillableEntry(1, typeof(RobeOfTheEvilArchmage)),
			new FillableEntry(1, typeof(RobeOfTheMeerCommander)),
			new FillableEntry(1, typeof(RobesOfThePhoenix)),
			new FillableEntry(1, typeof(RoninBlade)),
			new FillableEntry(1, typeof(RoninMastersRobe)),
			new FillableEntry(1, typeof(RotweaversCloak)),
			new FillableEntry(1, typeof(RotwormMastersRobes)),
			new FillableEntry(1, typeof(RousingWingArmor)),
			new FillableEntry(1, typeof(RuddyBouraMastersRobes)),
			new FillableEntry(1, typeof(RunemastersRobes)),
			new FillableEntry(1, typeof(SabertoothMantle)),
			new FillableEntry(1, typeof(SandmastersRobes)),
			new FillableEntry(1, typeof(SashOfTheCatacombs)),
			new FillableEntry(1, typeof(SashOfTheHiryu)),
			new FillableEntry(1, typeof(SashOfTheMilitiaCommander)),
			new FillableEntry(1, typeof(SatyrsEmbrace)),
			new FillableEntry(1, typeof(SavageLordRobes)),
			new FillableEntry(1, typeof(SavageMastersRobe)),
			new FillableEntry(1, typeof(SavageShamansRegalia)),
			new FillableEntry(1, typeof(SavageWardenBoots)),
			new FillableEntry(1, typeof(SavageWarlordsCloak)),
			new FillableEntry(1, typeof(ScelestusDarkMantle)),
			new FillableEntry(1, typeof(ScepterOfTheEternalWail)),
			new FillableEntry(1, typeof(ScepterOfTheUndyingHorde)),
			new FillableEntry(1, typeof(ScepterOfTheVoidWanderer)),
			new FillableEntry(1, typeof(ScepterOfTheWraithLord)),
			new FillableEntry(1, typeof(ScorpionLordsSash)),
			new FillableEntry(1, typeof(ScoutmastersCunningApron)),
			new FillableEntry(1, typeof(SeductressApron)),
			new FillableEntry(1, typeof(SentinelsAegis)),
			new FillableEntry(1, typeof(SentinelsAegisRobes)),
			new FillableEntry(1, typeof(SerpentcallerAmulet)),
			new FillableEntry(1, typeof(SerpentineWarlordsRobe)),
			new FillableEntry(1, typeof(SerpentLordSash)),
			new FillableEntry(1, typeof(SerpentmastersGauntlets)),
			new FillableEntry(1, typeof(SerpentmastersRing)),
			new FillableEntry(1, typeof(SerpentsFangShroud)),
			new FillableEntry(1, typeof(SerpentsStride)),
			new FillableEntry(1, typeof(ShaadowmastersRobes)),
			new FillableEntry(1, typeof(ShadecloakOfTheAbyss)),
			new FillableEntry(1, typeof(ShadowcloakOfTheImpaler)),
			new FillableEntry(1, typeof(ShadowlordsCloak)),
			new FillableEntry(1, typeof(ShadowlordsPlateChest)),
			new FillableEntry(1, typeof(ShadowmastersRobes)),
			new FillableEntry(1, typeof(ShadowNinjasCloak)),
			new FillableEntry(1, typeof(ShadowPantherCloak)),
			new FillableEntry(1, typeof(ShadowweaversRobes)),
			new FillableEntry(1, typeof(ShadowWispRobes)),
			new FillableEntry(1, typeof(ShardmastersDiadem)),
			new FillableEntry(1, typeof(ShepherdsCloak)),
			new FillableEntry(1, typeof(ShepherdsHartCloak)),
			new FillableEntry(1, typeof(ShepherdsHat)),
			new FillableEntry(1, typeof(ShepherdsMantle)),
			new FillableEntry(1, typeof(ShepherdsRobe)),
			new FillableEntry(1, typeof(ShieldOfTheWightLord)),
			new FillableEntry(1, typeof(ShroudOfTheLifestealer)),
			new FillableEntry(1, typeof(ShroudOfTheYomotsuPriest)),
			new FillableEntry(1, typeof(SilverSerpentsEmbrace)),
			new FillableEntry(1, typeof(SkeletalDrakeBreastplate)),
			new FillableEntry(1, typeof(SkeletalReapersBardiche)),
			new FillableEntry(1, typeof(SkitteringSash)),
			new FillableEntry(1, typeof(SkreeSummonersHat)),
			new FillableEntry(1, typeof(SkullLordsHelm)),
			new FillableEntry(1, typeof(SkywatcherCloak)),
			new FillableEntry(1, typeof(SlimeboundSash)),
			new FillableEntry(1, typeof(SlithlordsStoneChest)),
			new FillableEntry(1, typeof(SnowmastersBearMask)),
			new FillableEntry(1, typeof(SnowStalkersCape)),
			new FillableEntry(1, typeof(SolenHiveMasterSash)),
			new FillableEntry(1, typeof(SolenInfiltratorTunic)),
			new FillableEntry(1, typeof(SolenQueenHiveBand)),
			new FillableEntry(1, typeof(SoulbindersRobe)),
			new FillableEntry(1, typeof(SpectralGuardPlate)),
			new FillableEntry(1, typeof(SpectralShroud)),
			new FillableEntry(1, typeof(SpellbindersOrb)),
			new FillableEntry(1, typeof(SpiderlordsWebcloak)),
			new FillableEntry(1, typeof(SpiderWeaverApron)),
			new FillableEntry(1, typeof(SquirrelWhisperersSash)),
			new FillableEntry(1, typeof(StaffOfBoneSummoning)),
			new FillableEntry(1, typeof(StaffOfJukaMagi)),
			new FillableEntry(1, typeof(StaffOfOphidianDomination)),
			new FillableEntry(1, typeof(StaffOfTheAncientGrove)),
			new FillableEntry(1, typeof(StaffOfTheArcaneDaemon)),
			new FillableEntry(1, typeof(StaffOfTheArcaneMeer)),
			new FillableEntry(1, typeof(StaffOfTheHive)),
			new FillableEntry(1, typeof(StaffOfTheLichKing)),
			new FillableEntry(1, typeof(SteedmastersApron)),
			new FillableEntry(1, typeof(StoneguardsAegis)),
			new FillableEntry(1, typeof(StoneHarpyWingArmor)),
			new FillableEntry(1, typeof(StoneTitansHelm)),
			new FillableEntry(1, typeof(StormlordsCloak)),
			new FillableEntry(1, typeof(StygianDrakeSash)),
			new FillableEntry(1, typeof(SummonersMacawBandana)),
			new FillableEntry(1, typeof(SummonersRingOfTheGoreFiend)),
			new FillableEntry(1, typeof(SwamplardsSash)),
			new FillableEntry(1, typeof(SwampLordsCloak)),
			new FillableEntry(1, typeof(SwamplordsCrown)),
			new FillableEntry(1, typeof(SwamplordsSash)),
			new FillableEntry(1, typeof(SwampwardenGreaves)),
			new FillableEntry(1, typeof(SylvanWardenBoots)),
			new FillableEntry(1, typeof(TerathanMatriarchsScepter)),
			new FillableEntry(1, typeof(TidecallersPendant)),
			new FillableEntry(1, typeof(TidecallersRobes)),
			new FillableEntry(1, typeof(TigersClawHarness)),
			new FillableEntry(1, typeof(TigersClawSash)),
			new FillableEntry(1, typeof(TitanforgedStoneChest)),
			new FillableEntry(1, typeof(ToxicDefenderShield)),
			new FillableEntry(1, typeof(ToxicMantleOfCorrosion)),
			new FillableEntry(1, typeof(ToxicWardensSash)),
			new FillableEntry(1, typeof(TrapdoorWeaversWings)),
			new FillableEntry(1, typeof(TriceratopsBoneHelm)),
			new FillableEntry(1, typeof(TritonsTempestTrident)),
			new FillableEntry(1, typeof(TroglodyteSummonerKryss)),
			new FillableEntry(1, typeof(TrollKingsWarHammer)),
			new FillableEntry(1, typeof(TropicsCallRobe)),
			new FillableEntry(1, typeof(TsukiWolfsGaze)),
			new FillableEntry(1, typeof(TurkeySummonersSash)),
			new FillableEntry(1, typeof(TurkeyTamerSash)),
			new FillableEntry(1, typeof(UnicornsGraceSash)),
			new FillableEntry(1, typeof(VenomlordsShroud)),
			new FillableEntry(1, typeof(VenomousMantle)),
			new FillableEntry(1, typeof(VerdantGuardianPlate)),
			new FillableEntry(1, typeof(VineweaversSash)),
			new FillableEntry(1, typeof(VirulentWarlordsSash)),
			new FillableEntry(1, typeof(VoidcallersStoneChest)),
			new FillableEntry(1, typeof(VolcanicKasa)),
			new FillableEntry(1, typeof(VorpalBunnyHeaddress)),
			new FillableEntry(1, typeof(WalrusSummonerBoots)),
			new FillableEntry(1, typeof(WarbossHelm)),
			new FillableEntry(1, typeof(WarbringersGirdle)),
			new FillableEntry(1, typeof(WarchiefHelm)),
			new FillableEntry(1, typeof(WarChiefsBattleCrown)),
			new FillableEntry(1, typeof(WardensGargoyleAmulet)),
			new FillableEntry(1, typeof(WardensRobeOfTheForest)),
			new FillableEntry(1, typeof(WatercallersKimono)),
			new FillableEntry(1, typeof(WhiskersCloak)),
			new FillableEntry(1, typeof(WhisperingOrb)),
			new FillableEntry(1, typeof(WhiteWolfsHowl)),
			new FillableEntry(1, typeof(WidowsVeil)),
			new FillableEntry(1, typeof(WildBoarGirdle)),
			new FillableEntry(1, typeof(WildkeeperSash)),
			new FillableEntry(1, typeof(WildSpiritsAmulet)),
			new FillableEntry(1, typeof(WildTigerSash)),
			new FillableEntry(1, typeof(WildwoodMantle)),
			new FillableEntry(1, typeof(WindrunnersBulwark)),
			new FillableEntry(1, typeof(WintersEmbraceCloak)),
			new FillableEntry(1, typeof(WispcallerScepter)),
			new FillableEntry(1, typeof(WispsOfShadowCloak)),
			new FillableEntry(1, typeof(WolfmastersPelt)),
			new FillableEntry(1, typeof(WolfsbaneCloak)),
			new FillableEntry(1, typeof(WraithboundMantle)),
			new FillableEntry(1, typeof(WraithlordsAmulet)),
			new FillableEntry(1, typeof(WyrmcallersCloak)),
			new FillableEntry(1, typeof(WyvernBoneHelm)),
			new FillableEntry(1, typeof(YamandonLordsStoneChest)),
			new FillableEntry(1, typeof(YomotsuWarlordsYumi)),
			new FillableEntry(1, typeof(YoungNinjasShadowblade)),
			new FillableEntry(1, typeof(TeleportToIlshenarItem)),
			new FillableEntry(1, typeof(TeleportToMalasItem)),		
			new FillableEntry(1, typeof(TeleportToTokuno)),				
			new FillableEntry(1, typeof(MaxxiaScroll))     // Example item: Magical crafting material
		};

		// Modify the GenerateContent Method
		public virtual void GenerateContent(bool all)
		{
			if (m_Content == null || Deleted)
				return;

			int toSpawn = GetSpawnCount(all);
			bool canSpawnRefinement = GetAmount(typeof(RefinementComponent)) == 0 && CanSpawnRefinement();

			// Generate Special Items with a 50% chance
			if (Utility.RandomDouble() < 0.20) // 50% chance for at least one special item
			{
				int specialItemCount = 1;

				// Additional items spawn with decreasing probability
				while (specialItemCount < 5 && Utility.RandomDouble() < 0.10 / (specialItemCount + 1))
				{
					specialItemCount++;
				}

				for (int i = 0; i < specialItemCount; i++)
				{
					FillableEntry specialEntry = SpecialItems[Utility.Random(SpecialItems.Length)];
					Item specialItem = specialEntry.Construct();

					if (specialItem != null)
						DropItem(specialItem);
				}
			}

			// Spawn regular content
			for (int i = 0; i < toSpawn; ++i)
			{
				if (canSpawnRefinement && RefinementComponent.Roll(this, 1, 0.08))
				{
					canSpawnRefinement = false;
					continue;
				}

				Item item = m_Content.Construct();

				if (item != null)
				{
					List<Item> list = Items;

					for (int j = 0; j < list.Count; ++j)
					{
						Item subItem = list[j];

						if (!(subItem is Container) && subItem.StackWith(null, item, false))
							break;
					}

					if (item != null && !item.Deleted)
						DropItem(item);
				}
			}
		}


        public override bool ExecuteTrap(Mobile from)
        {
            bool execute = base.ExecuteTrap(from);

            if (execute && --TotalTraps > 0)
            {
                ResetTrap();
            }

            return execute;
        }

        public void ResetTrap()
        {
            if (m_Content.Level > Utility.Random(5))
                TrapType = TrapType.PoisonTrap;
            else
                TrapType = TrapType.ExplosionTrap;

            TrapPower = m_Content.Level * Utility.RandomMinMax(10, 30);
            TrapLevel = m_Content.Level;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(2); // version

            writer.Write(TotalTraps);
            writer.Write(MaxSpawnCount);

            writer.Write((int)ContentType);

            if (m_RespawnTimer != null)
            {
                writer.Write(true);
                writer.WriteDeltaTime((DateTime)m_NextRespawnTime);
            }
            else
            {
                writer.Write(false);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if (version == 1)
            {
                MaxSpawnCount = Utility.RandomMinMax(3, 5);
                TotalTraps = 1;
            }

            switch (version)
            {
                case 2:
                    {
                        TotalTraps = reader.ReadInt();
                        MaxSpawnCount = reader.ReadInt();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Content = FillableContent.Lookup((FillableContentType)reader.ReadInt());
                        goto case 0;
                    }
                case 0:
                    {
                        if (reader.ReadBool())
                        {
                            m_NextRespawnTime = reader.ReadDeltaTime();

                            TimeSpan delay = m_NextRespawnTime - DateTime.UtcNow;
                            m_RespawnTimer = Timer.DelayCall(delay > TimeSpan.Zero ? delay : TimeSpan.Zero, new TimerCallback(Respawn));
                        }
                        else
                        {
                            CheckRespawn();
                        }

                        break;
                    }
            }
        }

        protected virtual int GetSpawnCount(bool all)
        {
            int itemsCount = GetItemsCount();

            if (itemsCount >= MaxSpawnCount)
                return 0;

            return all ? (MaxSpawnCount - itemsCount) : AmountPerSpawn;
        }
    }

    [Flipable(0xA97, 0xA99, 0xA98, 0xA9A, 0xA9B, 0xA9C)]
    public class LibraryBookcase : FillableContainer
    {
        [Constructable]
        public LibraryBookcase()
            : base(0xA97)
        {
            Weight = 1.0;

            MaxSpawnCount = 5;
        }

        public LibraryBookcase(Serial serial)
            : base(serial)
        {
        }

        public override bool IsLockable
        {
            get
            {
                return false;
            }
        }

        public override void AcquireContent()
        {
            if (m_Content != null)
                return;

            m_Content = FillableContent.Library;

            if (m_Content != null)
                Respawn();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if (version == 0 && m_Content == null)
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(AcquireContent));

            if(version == 1)
                MaxSpawnCount = 5;
        }

        protected override int GetSpawnCount(bool all)
        {
            return (MaxSpawnCount - GetItemsCount());
        }
    }

    [Flipable(0xE3D, 0xE3C)]
    public class FillableLargeCrate : FillableContainer
    {
        [Constructable]
        public FillableLargeCrate()
            : base(0xE3D)
        {
            Weight = 1.0;
        }

        public FillableLargeCrate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    [Flipable(0x9A9, 0xE7E)]
    public class FillableSmallCrate : FillableContainer
    {
        [Constructable]
        public FillableSmallCrate()
            : base(0x9A9)
        {
            Weight = 1.0;
        }

        public FillableSmallCrate(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    [Flipable(0x9AA, 0xE7D)]
    public class FillableWoodenBox : FillableContainer
    {
        [Constructable]
        public FillableWoodenBox()
            : base(0x9AA)
        {
            Weight = 4.0;
        }

        public FillableWoodenBox(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x9A8, 0xE80)]
    public class FillableMetalBox : FillableContainer
    {
        [Constructable]
        public FillableMetalBox()
            : base(0x9A8)
        {
        }

        public FillableMetalBox(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if (version == 0 && Weight == 3)
                Weight = -1;
        }
    }

    public class FillableBarrel : FillableContainer
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D WorldLocation { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map WorldMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextReturn { get; set; }

        [Constructable]
        public FillableBarrel()
            : base(0xE77)
        {
        }

        public FillableBarrel(Serial serial)
            : base(serial)
        {
        }

        public override bool IsLockable
        {
            get
            {
                return false;
            }
        }

        public void Pour(Mobile from, BaseBeverage beverage)
        {
            if (beverage.Content == BeverageType.Water)
            {
                if (Items.Count > 0)
                {
                    from.SendLocalizedMessage(500848); // Couldn't pour it there.  It was already full.
                    beverage.PrivateOverheadMessage(Server.Network.MessageType.Regular, 0, 500841, from.NetState); // that has somethign in it.
                }
                else
                {
                    var barrel = new WaterBarrel();
                    barrel.Movable = false;
                    barrel.MoveToWorld(Location, Map);

                    WorldLocation = Location;
                    WorldMap = Map;
                    NextReturn = DateTime.UtcNow + TimeSpan.FromHours(1);

                    beverage.Pour_OnTarget(from, barrel);

                    Internalize();
                }
            }
        }

        public void TryReturn()
        {
            if (WorldMap != null)
            {
                IPooledEnumerable eable = WorldMap.GetItemsInRange(WorldLocation, 0);

                foreach (Item item in eable)
                {
                    if (item is WaterBarrel && item.Z == WorldLocation.Z)
                    {
                        eable.Free();
                        return;
                    }
                }

                eable.Free();
                NextReturn = DateTime.MinValue;
                MoveToWorld(WorldLocation, WorldMap);
                Respawn();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)2); // version

            writer.Write(WorldLocation);
            writer.Write(WorldMap);

            if (NextReturn != DateTime.MinValue && NextReturn < DateTime.UtcNow)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(20), TryReturn);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            switch (version)
            {
                case 2:
                    WorldLocation = reader.ReadPoint3D();
                    WorldMap = reader.ReadMap();
                    break;
            }

            if (Map == Map.Internal)
            {
                if (WorldMap != null)
                {
                    NextReturn = DateTime.UtcNow;
                    Timer.DelayCall(TimeSpan.FromSeconds(20), TryReturn);
                }
                else
                {
                    Delete();
                }
            }
        }
    }

    [Flipable(0x9AB, 0xE7C)]
    public class FillableMetalChest : FillableContainer
    {
        [Constructable]
        public FillableMetalChest()
            : base(0x9AB)
        {
        }

        public FillableMetalChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 25)
                Weight = -1;
        }
    }

    [Flipable(0xE41, 0xE40)]
    public class FillableMetalGoldenChest : FillableContainer
    {
        [Constructable]
        public FillableMetalGoldenChest()
            : base(0xE41)
        {
        }

        public FillableMetalGoldenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 25)
                Weight = -1;
        }
    }

    [Flipable(0xE43, 0xE42)]
    public class FillableWoodenChest : FillableContainer
    {
        [Constructable]
        public FillableWoodenChest()
            : base(0xE43)
        {
        }

        public FillableWoodenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 2)
                Weight = -1;
        }
    }

    public class FillableEntry
    {
        protected Type[] m_Types;
        protected int m_Weight;
        public FillableEntry(Type type)
            : this(1, new Type[] { type })
        {
        }

        public FillableEntry(int weight, Type type)
            : this(weight, new Type[] { type })
        {
        }

        public FillableEntry(Type[] types)
            : this(1, types)
        {
        }

        public FillableEntry(int weight, Type[] types)
        {
            m_Weight = weight;
            m_Types = types;
        }

        public FillableEntry(int weight, Type[] types, int offset, int count)
        {
            m_Weight = weight;
            m_Types = new Type[count];

            for (int i = 0; i < m_Types.Length; ++i)
                m_Types[i] = types[offset + i];
        }

        public Type[] Types
        {
            get
            {
                return m_Types;
            }
        }
        public int Weight
        {
            get
            {
                return m_Weight;
            }
        }
        public virtual Item Construct()
        {
            Item item = Loot.Construct(m_Types);

            if (item is Key)
                ((Key)item).ItemID = Utility.RandomList((int)KeyType.Copper, (int)KeyType.Gold, (int)KeyType.Iron, (int)KeyType.Rusty);
            else if (item is Arrow || item is Bolt)
                item.Amount = Utility.RandomMinMax(2, 6);
            else if (item is Bandage || item is Lockpick)
                item.Amount = Utility.RandomMinMax(1, 3);

            return item;
        }
    }

    public class FillableBvrge : FillableEntry
    {
        private readonly BeverageType m_Content;
        public FillableBvrge(Type type, BeverageType content)
            : this(1, type, content)
        {
        }

        public FillableBvrge(int weight, Type type, BeverageType content)
            : base(weight, type)
        {
            m_Content = content;
        }

        public BeverageType Content
        {
            get
            {
                return m_Content;
            }
        }
        public override Item Construct()
        {
            Item item;

            int index = Utility.Random(m_Types.Length);

            if (m_Types[index] == typeof(BeverageBottle))
            {
                item = new BeverageBottle(m_Content);
            }
            else if (m_Types[index] == typeof(Jug))
            {
                item = new Jug(m_Content);
            }
            else
            {
                item = base.Construct();

                if (item is BaseBeverage)
                {
                    BaseBeverage bev = (BaseBeverage)item;

                    bev.Content = m_Content;
                    bev.Quantity = bev.MaxQuantity;
                }
            }

            return item;
        }
    }

    public class FillableContent
    {
        public static FillableContent Alchemist = new FillableContent(
            1,
            new Type[]
            {
                typeof(Mobiles.Alchemist)
            },
            new FillableEntry[]
            {
                new FillableEntry(typeof(NightSightPotion)),
                new FillableEntry(typeof(LesserCurePotion)),
                new FillableEntry(typeof(AgilityPotion)),
                new FillableEntry(typeof(StrengthPotion)),
                new FillableEntry(typeof(LesserPoisonPotion)),
                new FillableEntry(typeof(RefreshPotion)),
                new FillableEntry(typeof(LesserHealPotion)),
                new FillableEntry(typeof(LesserExplosionPotion)),
                new FillableEntry(typeof(MortarPestle))
            });
        public static FillableContent Armorer = new FillableContent(
            2,
            new Type[]
            {
                typeof(Armorer)
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(ChainCoif)),
                new FillableEntry(1, typeof(PlateGorget)),
                new FillableEntry(1, typeof(BronzeShield)),
                new FillableEntry(1, typeof(Buckler)),
                new FillableEntry(2, typeof(MetalKiteShield)),
                new FillableEntry(2, typeof(HeaterShield)),
                new FillableEntry(1, typeof(WoodenShield)),
                new FillableEntry(1, typeof(MetalShield))
            });
        public static FillableContent ArtisanGuild = new FillableContent(
            1,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(PaintsAndBrush)),
                new FillableEntry(1, typeof(SledgeHammer)),
                new FillableEntry(2, typeof(SmithHammer)),
                new FillableEntry(2, typeof(Tongs)),
                new FillableEntry(4, typeof(Lockpick)),
                new FillableEntry(4, typeof(TinkerTools)),
                new FillableEntry(1, typeof(MalletAndChisel)),
                new FillableEntry(1, typeof(StatueEast2)),
                new FillableEntry(1, typeof(StatueSouth)),
                new FillableEntry(1, typeof(StatueSouthEast)),
                new FillableEntry(1, typeof(StatueWest)),
                new FillableEntry(1, typeof(StatueNorth)),
                new FillableEntry(1, typeof(StatueEast)),
                new FillableEntry(1, typeof(BustEast)),
                new FillableEntry(1, typeof(BustSouth)),
                new FillableEntry(1, typeof(BearMask)),
                new FillableEntry(1, typeof(DeerMask)),
                new FillableEntry(4, typeof(OrcHelm)),
                new FillableEntry(1, typeof(TribalMask)),
                new FillableEntry(1, typeof(HornedTribalMask))
            });
        public static FillableContent Baker = new FillableContent(
            1,
            new Type[]
            {
                typeof(Baker),
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(RollingPin)),
                new FillableEntry(2, typeof(SackFlour)),
                new FillableEntry(2, typeof(BreadLoaf)),
                new FillableEntry(1, typeof(FrenchBread))
            });
        public static FillableContent Bard = new FillableContent(
            1,
            new Type[]
            {
                typeof(Bard),
                typeof(BardGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(LapHarp)),
                new FillableEntry(2, typeof(Lute)),
                new FillableEntry(1, typeof(Drums)),
                new FillableEntry(1, typeof(Tambourine)),
                new FillableEntry(1, typeof(TambourineTassel))
            });
        public static FillableContent Blacksmith = new FillableContent(
            2,
            new Type[]
            {
                typeof(Blacksmith),
                typeof(BlacksmithGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(8, typeof(SmithHammer)),
                new FillableEntry(8, typeof(Tongs)),
                new FillableEntry(8, typeof(SledgeHammer)),
                //new FillableEntry( 8, typeof( IronOre ) ), TODO: Smaller ore
                new FillableEntry(8, typeof(IronIngot)),
                new FillableEntry(1, typeof(IronWire)),
                new FillableEntry(1, typeof(SilverWire)),
                new FillableEntry(1, typeof(GoldWire)),
                new FillableEntry(1, typeof(CopperWire)),
                new FillableEntry(1, typeof(HorseShoes)),
                new FillableEntry(1, typeof(ForgedMetal))
            });
        public static FillableContent Bowyer = new FillableContent(
            2,
            new Type[]
            {
                typeof(Bowyer)
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(Bow)),
                new FillableEntry(2, typeof(Crossbow)),
                new FillableEntry(1, typeof(Arrow))
            });
        public static FillableContent Butcher = new FillableContent(
            1,
            new Type[]
            {
                typeof(Butcher),
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(Cleaver)),
                new FillableEntry(2, typeof(SlabOfBacon)),
                new FillableEntry(2, typeof(Bacon)),
                new FillableEntry(1, typeof(RawFishSteak)),
                new FillableEntry(1, typeof(FishSteak)),
                new FillableEntry(2, typeof(CookedBird)),
                new FillableEntry(2, typeof(RawBird)),
                new FillableEntry(2, typeof(Ham)),
                new FillableEntry(1, typeof(RawLambLeg)),
                new FillableEntry(1, typeof(LambLeg)),
                new FillableEntry(1, typeof(Ribs)),
                new FillableEntry(1, typeof(RawRibs)),
                new FillableEntry(2, typeof(Sausage)),
                new FillableEntry(1, typeof(RawChickenLeg)),
                new FillableEntry(1, typeof(ChickenLeg))
            });
        public static FillableContent Carpenter = new FillableContent(
            1,
            new Type[]
            {
                typeof(Carpenter),
                typeof(Architect),
                typeof(RealEstateBroker)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(ChiselsNorth)),
                new FillableEntry(1, typeof(ChiselsWest)),
                new FillableEntry(2, typeof(DovetailSaw)),
                new FillableEntry(2, typeof(Hammer)),
                new FillableEntry(2, typeof(MouldingPlane)),
                new FillableEntry(2, typeof(Nails)),
                new FillableEntry(2, typeof(JointingPlane)),
                new FillableEntry(2, typeof(SmoothingPlane)),
                new FillableEntry(2, typeof(Saw)),
                new FillableEntry(2, typeof(DrawKnife)),
                new FillableEntry(1, typeof(Log)),
                new FillableEntry(1, typeof(Froe)),
                new FillableEntry(1, typeof(Inshave)),
                new FillableEntry(1, typeof(Scorp))
            });
        public static FillableContent Clothier = new FillableContent(
            1,
            new Type[]
            {
                typeof(Tailor),
                typeof(Weaver),
                typeof(TailorGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Cotton)),
                new FillableEntry(1, typeof(Wool)),
                new FillableEntry(1, typeof(DarkYarn)),
                new FillableEntry(1, typeof(LightYarn)),
                new FillableEntry(1, typeof(LightYarnUnraveled)),
                new FillableEntry(1, typeof(SpoolOfThread)),
                // Four different types
                //new FillableEntry( 1, typeof( FoldedCloth ) ),
                //new FillableEntry( 1, typeof( FoldedCloth ) ),
                //new FillableEntry( 1, typeof( FoldedCloth ) ),
                //new FillableEntry( 1, typeof( FoldedCloth ) ),
                new FillableEntry(1, typeof(Dyes)),
                new FillableEntry(2, typeof(Leather))
            });
        public static FillableContent Cobbler = new FillableContent(
            1,
            new Type[]
            {
                typeof(Cobbler)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Boots)),
                new FillableEntry(2, typeof(Shoes)),
                new FillableEntry(2, typeof(Sandals)),
                new FillableEntry(1, typeof(ThighBoots))
            });
        public static FillableContent Docks = new FillableContent(
            1,
            new Type[]
            {
                typeof(Fisherman),
                typeof(FisherGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(FishingPole)),
                // Two different types
                new FillableEntry( 1, typeof( SmallFish ) ),
                new FillableEntry( 1, typeof( SmallFish ) ),
                new FillableEntry(4, typeof(Fish))
            });
        public static FillableContent Farm = new FillableContent(
            1,
            new Type[]
            {
                typeof(Farmer),
                typeof(Rancher)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Shirt)),
                new FillableEntry(1, typeof(ShortPants)),
                new FillableEntry(1, typeof(Skirt)),
                new FillableEntry(1, typeof(PlainDress)),
                new FillableEntry(1, typeof(Cap)),
                new FillableEntry(2, typeof(Sandals)),
                new FillableEntry(2, typeof(GnarledStaff)),
                new FillableEntry(2, typeof(Pitchfork)),
                new FillableEntry(1, typeof(Bag)),
                new FillableEntry(1, typeof(Kindling)),
                new FillableEntry(1, typeof(Lettuce)),
                new FillableEntry(1, typeof(Onion)),
                new FillableEntry(1, typeof(Turnip)),
                new FillableEntry(1, typeof(Ham)),
                new FillableEntry(1, typeof(Bacon)),
                new FillableEntry(1, typeof(RawLambLeg)),
                new FillableEntry(1, typeof(SheafOfHay)),
                new FillableBvrge(1, typeof(Pitcher), BeverageType.Milk)
            });
        public static FillableContent FighterGuild = new FillableContent(
            3,
            new Type[]
            {
                typeof(WarriorGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(12, Loot.ArmorTypes),
                new FillableEntry(8, Loot.WeaponTypes),
                new FillableEntry(3, Loot.ShieldTypes),
                new FillableEntry(1, typeof(Arrow))
            });
        public static FillableContent Guard = new FillableContent(
            3,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(12, Loot.ArmorTypes),
                new FillableEntry(8, Loot.WeaponTypes),
                new FillableEntry(3, Loot.ShieldTypes),
                new FillableEntry(1, typeof(Arrow))
            });
        public static FillableContent Healer = new FillableContent(
            1,
            new Type[]
            {
                typeof(Healer),
                typeof(HealerGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Bandage)),
                new FillableEntry(1, typeof(MortarPestle)),
                new FillableEntry(1, typeof(LesserHealPotion))
            });
        public static FillableContent Herbalist = new FillableContent(
            1,
            new Type[]
            {
                typeof(Herbalist)
            },
            new FillableEntry[]
            {
                new FillableEntry(10, typeof(Garlic)),
                new FillableEntry(10, typeof(Ginseng)),
                new FillableEntry(10, typeof(MandrakeRoot)),
                new FillableEntry(1, typeof(DeadWood)),
                new FillableEntry(1, typeof(WhiteDriedFlowers)),
                new FillableEntry(1, typeof(GreenDriedFlowers)),
                new FillableEntry(1, typeof(DriedOnions)),
                new FillableEntry(1, typeof(DriedHerbs))
            });
        public static FillableContent Inn = new FillableContent(
            1,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Candle)),
                new FillableEntry(1, typeof(Torch)),
                new FillableEntry(1, typeof(Lantern))
            });
        public static FillableContent Jeweler = new FillableContent(
            2,
            new Type[]
            {
                typeof(Jeweler)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(GoldRing)),
                new FillableEntry(1, typeof(GoldBracelet)),
                new FillableEntry(1, typeof(GoldEarrings)),
                new FillableEntry(1, typeof(GoldNecklace)),
                new FillableEntry(1, typeof(GoldBeadNecklace)),
                new FillableEntry(1, typeof(Necklace)),
                new FillableEntry(1, typeof(Beads)),
                new FillableEntry(9, Loot.GemTypes)
            });
        public static FillableContent Library = new FillableContent(
            1,
            new Type[]
            {
                typeof(Scribe)
            },
            new FillableEntry[]
            {
                new FillableEntry(8, Loot.LibraryBookTypes),
                new FillableEntry(1, typeof(RedBook)),
                new FillableEntry(1, typeof(BlueBook))
            });
        public static FillableContent Mage = new FillableContent(
            2,
            new Type[]
            {
                typeof(Mage),
                typeof(HolyMage),
                typeof(MageGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(16, typeof(BlankScroll)),
                new FillableEntry(14, typeof(Spellbook)),
                new FillableEntry(12, Loot.RegularScrollTypes, 0, 8),
                new FillableEntry(11, Loot.RegularScrollTypes, 8, 8),
                new FillableEntry(10, Loot.RegularScrollTypes, 16, 8),
                new FillableEntry(9, Loot.RegularScrollTypes, 24, 8),
                new FillableEntry(8, Loot.RegularScrollTypes, 32, 8),
                new FillableEntry(7, Loot.RegularScrollTypes, 40, 8),
                new FillableEntry(6, Loot.RegularScrollTypes, 48, 8),
                new FillableEntry(5, Loot.RegularScrollTypes, 56, 8)
            });
        public static FillableContent Merchant = new FillableContent(
            1,
            new Type[]
            {
                typeof(MerchantGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(CheeseWheel)),
                new FillableEntry(1, typeof(CheeseWedge)),
                new FillableEntry(1, typeof(CheeseSlice)),
                new FillableEntry(1, typeof(Eggs)),
                new FillableEntry(4, typeof(Fish)),
                new FillableEntry(2, typeof(RawFishSteak)),
                new FillableEntry(2, typeof(FishSteak)),
                new FillableEntry(1, typeof(Apple)),
                new FillableEntry(2, typeof(Banana)),
                new FillableEntry(2, typeof(Bananas)),
                new FillableEntry(2, typeof(OpenCoconut)),
                new FillableEntry(1, typeof(SplitCoconut)),
                new FillableEntry(1, typeof(Coconut)),
                new FillableEntry(1, typeof(Dates)),
                new FillableEntry(1, typeof(Grapes)),
                new FillableEntry(1, typeof(Lemon)),
                new FillableEntry(1, typeof(Lemons)),
                new FillableEntry(1, typeof(Lime)),
                new FillableEntry(1, typeof(Limes)),
                new FillableEntry(1, typeof(Peach)),
                new FillableEntry(1, typeof(Pear)),
                new FillableEntry(2, typeof(SlabOfBacon)),
                new FillableEntry(2, typeof(Bacon)),
                new FillableEntry(2, typeof(CookedBird)),
                new FillableEntry(2, typeof(RawBird)),
                new FillableEntry(2, typeof(Ham)),
                new FillableEntry(1, typeof(RawLambLeg)),
                new FillableEntry(1, typeof(LambLeg)),
                new FillableEntry(1, typeof(Ribs)),
                new FillableEntry(1, typeof(RawRibs)),
                new FillableEntry(2, typeof(Sausage)),
                new FillableEntry(1, typeof(RawChickenLeg)),
                new FillableEntry(1, typeof(ChickenLeg)),
                new FillableEntry(1, typeof(Watermelon)),
                new FillableEntry(1, typeof(SmallWatermelon)),
                new FillableEntry(3, typeof(Turnip)),
                new FillableEntry(2, typeof(YellowGourd)),
                new FillableEntry(2, typeof(GreenGourd)),
                new FillableEntry(2, typeof(Pumpkin)),
                new FillableEntry(1, typeof(SmallPumpkin)),
                new FillableEntry(2, typeof(Onion)),
                new FillableEntry(2, typeof(Lettuce)),
                new FillableEntry(2, typeof(Squash)),
                new FillableEntry(2, typeof(HoneydewMelon)),
                new FillableEntry(1, typeof(Carrot)),
                new FillableEntry(2, typeof(Cantaloupe)),
                new FillableEntry(2, typeof(Cabbage)),
                new FillableEntry(4, typeof(EarOfCorn))
            });
        public static FillableContent Mill = new FillableContent(
            1,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(SackFlour))
            });
        public static FillableContent Mine = new FillableContent(
            1,
            new Type[]
            {
                typeof(Miner)
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(Pickaxe)),
                new FillableEntry(2, typeof(Shovel)),
                new FillableEntry(2, typeof(IronIngot)),
                //new FillableEntry( 2, typeof( IronOre ) ),	TODO: Smaller Ore
                new FillableEntry(1, typeof(ForgedMetal))
            });
        public static FillableContent Observatory = new FillableContent(
            1,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(Sextant)),
                new FillableEntry(2, typeof(Clock)),
                new FillableEntry(1, typeof(Spyglass))
            });
        public static FillableContent Painter = new FillableContent(
            1,
            new Type[]
            {
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(PaintsAndBrush)),
                new FillableEntry(2, typeof(PenAndInk))
            });
        public static FillableContent Provisioner = new FillableContent(
            1,
            new Type[]
            {
                typeof(Provisioner)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(CheeseWheel)),
                new FillableEntry(1, typeof(CheeseWedge)),
                new FillableEntry(1, typeof(CheeseSlice)),
                new FillableEntry(1, typeof(Eggs)),
                new FillableEntry(4, typeof(Fish)),
                new FillableEntry(1, typeof(DirtyFrypan)),
                new FillableEntry(1, typeof(DirtyPan)),
                new FillableEntry(1, typeof(DirtyKettle)),
                new FillableEntry(1, typeof(DirtySmallRoundPot)),
                new FillableEntry(1, typeof(DirtyRoundPot)),
                new FillableEntry(1, typeof(DirtySmallPot)),
                new FillableEntry(1, typeof(DirtyPot)),
                new FillableEntry(1, typeof(Apple)),
                new FillableEntry(2, typeof(Banana)),
                new FillableEntry(2, typeof(Bananas)),
                new FillableEntry(2, typeof(OpenCoconut)),
                new FillableEntry(1, typeof(SplitCoconut)),
                new FillableEntry(1, typeof(Coconut)),
                new FillableEntry(1, typeof(Dates)),
                new FillableEntry(1, typeof(Grapes)),
                new FillableEntry(1, typeof(Lemon)),
                new FillableEntry(1, typeof(Lemons)),
                new FillableEntry(1, typeof(Lime)),
                new FillableEntry(1, typeof(Limes)),
                new FillableEntry(1, typeof(Peach)),
                new FillableEntry(1, typeof(Pear)),
                new FillableEntry(2, typeof(SlabOfBacon)),
                new FillableEntry(2, typeof(Bacon)),
                new FillableEntry(1, typeof(RawFishSteak)),
                new FillableEntry(1, typeof(FishSteak)),
                new FillableEntry(2, typeof(CookedBird)),
                new FillableEntry(2, typeof(RawBird)),
                new FillableEntry(2, typeof(Ham)),
                new FillableEntry(1, typeof(RawLambLeg)),
                new FillableEntry(1, typeof(LambLeg)),
                new FillableEntry(1, typeof(Ribs)),
                new FillableEntry(1, typeof(RawRibs)),
                new FillableEntry(2, typeof(Sausage)),
                new FillableEntry(1, typeof(RawChickenLeg)),
                new FillableEntry(1, typeof(ChickenLeg)),
                new FillableEntry(1, typeof(Watermelon)),
                new FillableEntry(1, typeof(SmallWatermelon)),
                new FillableEntry(3, typeof(Turnip)),
                new FillableEntry(2, typeof(YellowGourd)),
                new FillableEntry(2, typeof(GreenGourd)),
                new FillableEntry(2, typeof(Pumpkin)),
                new FillableEntry(1, typeof(SmallPumpkin)),
                new FillableEntry(2, typeof(Onion)),
                new FillableEntry(2, typeof(Lettuce)),
                new FillableEntry(2, typeof(Squash)),
                new FillableEntry(2, typeof(HoneydewMelon)),
                new FillableEntry(1, typeof(Carrot)),
                new FillableEntry(2, typeof(Cantaloupe)),
                new FillableEntry(2, typeof(Cabbage)),
                new FillableEntry(4, typeof(EarOfCorn))
            });
        public static FillableContent Ranger = new FillableContent(
            2,
            new Type[]
            {
                typeof(Ranger),
                typeof(RangerGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(2, typeof(StuddedChest)),
                new FillableEntry(2, typeof(StuddedLegs)),
                new FillableEntry(2, typeof(StuddedArms)),
                new FillableEntry(2, typeof(StuddedGloves)),
                new FillableEntry(1, typeof(StuddedGorget)),
                new FillableEntry(2, typeof(LeatherChest)),
                new FillableEntry(2, typeof(LeatherLegs)),
                new FillableEntry(2, typeof(LeatherArms)),
                new FillableEntry(2, typeof(LeatherGloves)),
                new FillableEntry(1, typeof(LeatherGorget)),
                new FillableEntry(2, typeof(FeatheredHat)),
                new FillableEntry(1, typeof(CloseHelm)),
                new FillableEntry(1, typeof(TallStrawHat)),
                new FillableEntry(1, typeof(Bandana)),
                new FillableEntry(1, typeof(Cloak)),
                new FillableEntry(2, typeof(Boots)),
                new FillableEntry(2, typeof(ThighBoots)),
                new FillableEntry(2, typeof(GnarledStaff)),
                new FillableEntry(1, typeof(Whip)),
                new FillableEntry(2, typeof(Bow)),
                new FillableEntry(2, typeof(Crossbow)),
                new FillableEntry(2, typeof(HeavyCrossbow)),
                new FillableEntry(4, typeof(Arrow))
            });
        public static FillableContent Stables = new FillableContent(
            1,
            new Type[]
            {
                typeof(AnimalTrainer),
                typeof(GypsyAnimalTrainer)
            },
            new FillableEntry[]
            {
                //new FillableEntry( 1, typeof( Wheat ) ),
                new FillableEntry(1, typeof(Carrot))
            });
        public static FillableContent Tanner = new FillableContent(
            2,
            new Type[]
            {
                typeof(Tanner),
                typeof(LeatherWorker),
                typeof(Furtrader)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(FeatheredHat)),
                new FillableEntry(1, typeof(LeatherArms)),
                new FillableEntry(2, typeof(LeatherLegs)),
                new FillableEntry(2, typeof(LeatherChest)),
                new FillableEntry(2, typeof(LeatherGloves)),
                new FillableEntry(1, typeof(LeatherGorget)),
                new FillableEntry(2, typeof(Leather))
            });
        public static FillableContent Tavern = new FillableContent(
            1,
            new Type[]
            {
                typeof(TavernKeeper),
                typeof(Barkeeper),
                typeof(Waiter),
                typeof(Cook)
            },
            new FillableEntry[]
            {
                new FillableBvrge(1, typeof(BeverageBottle), BeverageType.Ale),
                new FillableBvrge(1, typeof(BeverageBottle), BeverageType.Wine),
                new FillableBvrge(1, typeof(BeverageBottle), BeverageType.Liquor),
                new FillableBvrge(1, typeof(Jug), BeverageType.Cider)
            });
        public static FillableContent ThiefGuild = new FillableContent(
            1,
            new Type[]
            {
                typeof(Thief),
                typeof(ThiefGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Lockpick)),
                new FillableEntry(1, typeof(BearMask)),
                new FillableEntry(1, typeof(DeerMask)),
                new FillableEntry(1, typeof(TribalMask)),
                new FillableEntry(1, typeof(HornedTribalMask)),
                new FillableEntry(4, typeof(OrcHelm))
            });
        public static FillableContent Tinker = new FillableContent(
            1,
            new Type[]
            {
                typeof(Tinker),
                typeof(TinkerGuildmaster)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Lockpick)),
                //new FillableEntry( 1, typeof( KeyRing ) ),
                new FillableEntry(2, typeof(Clock)),
                new FillableEntry(2, typeof(ClockParts)),
                new FillableEntry(2, typeof(AxleGears)),
                new FillableEntry(2, typeof(Gears)),
                new FillableEntry(2, typeof(Hinge)),
                //new FillableEntry( 1, typeof( ArrowShafts ) ),
                new FillableEntry(2, typeof(Sextant)),
                new FillableEntry(2, typeof(SextantParts)),
                new FillableEntry(2, typeof(Axle)),
                new FillableEntry(2, typeof(Springs)),
                new FillableEntry(5, typeof(TinkerTools)),
                new FillableEntry(4, typeof(Key)),
                new FillableEntry(1, typeof(DecoArrowShafts)),
                new FillableEntry(1, typeof(Lockpicks)),
                new FillableEntry(1, typeof(ToolKit))
            });
        public static FillableContent Veterinarian = new FillableContent(
            1,
            new Type[]
            {
                typeof(Veterinarian)
            },
            new FillableEntry[]
            {
                new FillableEntry(1, typeof(Bandage)),
                new FillableEntry(1, typeof(MortarPestle)),
                new FillableEntry(1, typeof(LesserHealPotion)),
                //new FillableEntry( 1, typeof( Wheat ) ),
                new FillableEntry(1, typeof(Carrot))
            });
        public static FillableContent Weaponsmith = new FillableContent(
            2,
            new Type[]
            {
                typeof(Weaponsmith)
            },
            new FillableEntry[]
            {
                new FillableEntry(8, Loot.WeaponTypes),
                new FillableEntry(1, typeof(Arrow))
            });
        private static readonly FillableContent[] m_ContentTypes = new FillableContent[]
        {
            Weaponsmith, Provisioner, Mage,
            Alchemist, Armorer, ArtisanGuild,
            Baker, Bard, Blacksmith,
            Bowyer, Butcher, Carpenter,
            Clothier, Cobbler, Docks,
            Farm, FighterGuild, Guard,
            Healer, Herbalist, Inn,
            Jeweler, Library, Merchant,
            Mill, Mine, Observatory,
            Painter, Ranger, Stables,
            Tanner, Tavern, ThiefGuild,
            Tinker, Veterinarian
        };
        private static Hashtable m_AcquireTable;
        private readonly int m_Level;
        private readonly Type[] m_Vendors;
        private readonly FillableEntry[] m_Entries;
        private readonly int m_Weight;
        public FillableContent(int level, Type[] vendors, FillableEntry[] entries)
        {
            m_Level = level;
            m_Vendors = vendors;
            m_Entries = entries;

            for (int i = 0; i < entries.Length; ++i)
                m_Weight += entries[i].Weight;
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }
        public Type[] Vendors
        {
            get
            {
                return m_Vendors;
            }
        }
        public FillableContentType TypeID
        {
            get
            {
                return Lookup(this);
            }
        }
        public static FillableContent Lookup(FillableContentType type)
        {
            int v = (int)type;

            if (v >= 0 && v < m_ContentTypes.Length)
                return m_ContentTypes[v];

            return null;
        }

        public static FillableContentType Lookup(FillableContent content)
        {
            if (content == null)
                return FillableContentType.None;

            return (FillableContentType)Array.IndexOf(m_ContentTypes, content);
        }

        public static FillableContent Acquire(Point3D loc, Map map)
        {
            if (map == null || map == Map.Internal)
                return null;

            if (m_AcquireTable == null)
            {
                m_AcquireTable = new Hashtable();

                for (int i = 0; i < m_ContentTypes.Length; ++i)
                {
                    FillableContent fill = m_ContentTypes[i];

                    for (int j = 0; j < fill.m_Vendors.Length; ++j)
                        m_AcquireTable[fill.m_Vendors[j]] = fill;
                }
            }

            Mobile nearest = null;
            FillableContent content = null;

            IPooledEnumerable eable = map.GetMobilesInRange(loc, 60);

            foreach (Mobile mob in eable)
            {
                if (nearest != null && mob.GetDistanceToSqrt(loc) > nearest.GetDistanceToSqrt(loc) && !(nearest is Cobbler && mob is Provisioner))
                    continue;

                FillableContent check = m_AcquireTable[mob.GetType()] as FillableContent;

                if (check != null)
                {
                    nearest = mob;
                    content = check;
                }
            }

            eable.Free();

            return content;
        }

        public virtual Item Construct()
        {
            int index = Utility.Random(m_Weight);

            for (int i = 0; i < m_Entries.Length; ++i)
            {
                FillableEntry entry = m_Entries[i];

                if (index < entry.Weight)
                    return entry.Construct();

                index -= entry.Weight;
            }

            return null;
        }
    }
}
