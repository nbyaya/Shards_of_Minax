using Server.Commands;
using Server.Prompts;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Mobiles
{
    public enum TalentID
    {
        AncientKnowledge = 1,

        StealingSpells = 100, // Talents must follow this pattern 'Name = X' where X is a unique number in the enum
        StealingNodes = 101,
        StealingDistance = 102,
        StealingWeight = 103,
        StealingEquipped = 104,
        StealingLoS = 105,
        StealingImmovable = 106,
		
        // New Lumberjacking Talent IDs
        LumberjackingSpells = 200,
        LumberjackingNodes = 201,
        LumberjackingRange = 202,
        LumberjackingEfficiency = 203,
        LumberjackingYield = 204,
		
        MiningSpells = 300,   // Bitmask for unlocked mining spells
        MiningNodes = 301,    // Bitmask for nodes activated in the mining tree
        MiningYield = 302,    // Extra ore yield bonus
        MiningEfficiency = 303,   // Faster mining / lower tool wear or extra chance for gems
        MiningRange = 304,     // Increases effective mining range or bonus to mining “power”	

        BeggingSpells = 400,   // Bitflags for unlocking begging spells in the spellbook.
        BeggingNodes = 401,    // Bitflags for unlocking the begging skill tree nodes.
        BeggingPersuasion = 402,   // Passive: increases begging success chance.
        BeggingCharm = 403,        // Passive: improves rewards (more gold/items).
        BeggingKarma = 404,        // Passive: reduces Karma loss on successful beg.
        BeggingLuck = 405,          // Passive: chance for bonus rewards.		
		
        // NEW: Fishing-related talents – use unique numbers
        FishingSpells = 500,      // Bitfield for unlocked fishing spells in the spellbook
        FishingNodes = 501,       // Bitfield for unlocked fishing skill tree nodes
        FishingYield = 502,       // Increases extra fish (bonus yield)
        FishingRange = 503,       // Increases fishing range (bonus tiles)
        FishingEfficiency = 504,  // Improves fishing speed/efficiency
        FishingLuck = 505,         // Increases chance to mutate rare items
		
        // New Martial Manual talents
        MartialManualSpells = 600,
        MartialManualNodes = 601,
        MartialManualDamageBonus = 602,
        MartialManualDefenseBonus = 603,
        MartialManualAccuracyBonus = 604,
        MartialManualSpeedBonus = 605,
		
        DetectHiddenNodes = 700,          // Bitmask for activated Detect Hidden nodes
        DetectHiddenSpells = 701,         // Bitmask for unlocking detect hidden spells
        DetectHiddenRange = 702,          // Bonus to active detection range
        DetectHiddenChance = 703,         // Bonus to chance to reveal hidden targets
        DetectHiddenStealthReduction = 704,  // Reduces opponents’ stealth effectiveness		
		
		// New Discordance talents
		DiscordanceSpells = 800,     // Used by the spellbook to set unlocked spells
		DiscordanceNodes = 801,        // Bit flag storage for node unlocks
		DiscordanceRange = 802,        // Passive bonus: increased range
		DiscordanceEffect = 803,       // Passive bonus: improved effect potency
		DiscordanceCastSpeed = 804,    // Passive bonus: faster casting
		DiscordancePassive = 805,       // Passive bonus: miscellaneous bonuses
		
		// New EvalInt talents:
        EvalIntSpells = 907,
        EvalIntNodes = 908,
		
		//Forensics
        ForensicSpells = 1000,
        ForensicNodes = 1001,
        ForensicInsight = 1002,
        ForensicEfficiency = 1003,
        ForensicRevelation = 1004,
		
        // NEW: Hiding talents
        HidingSpells = 1100,
        HidingNodes = 1101,
        HidingEfficiency = 1102,

        // NEW: Meditation talents
        MeditationSpells = 1200,    // Used for bitflags to unlock meditation spells
        MeditationNodes = 1201,     // Tracks which meditation nodes have been activated
        MeditationFocus = 1202,     // Bonus to meditation chance
        MeditationRecovery = 1203,   // Bonus to mana regeneration	
		TemporalStillness = 1204,   // Bonus to mana regeneration	
		
		//Provocation
		ProvocationSpells = 1300,
		ProvocationNodes = 1301,
		ProvocationBonus = 1302,	
		ProvocationRange = 1303,
		ProvocationCooldownReduction = 1304,
		
		//Remove Trap
        RemoveTrapSpells = 1400,         // Used by the spellbook (bit flags for unlocked spells)
        RemoveTrapNodes = 1401,          // Bit flags for unlocked nodes in the tree
        RemoveTrapDetection = 1402,      // Bonus to trap detection (passive)
        RemoveTrapSpeed = 1403,          // Bonus to disarming speed (passive)
        RemoveTrapSuccessChance = 1404,  // Bonus to disarm success chance (passive)
        RemoveTrapKitEfficiency = 1405,   // Bonus to kit efficiency (passive)	

		//Remove Trap
        StealthSpells = 1500,         // Bitwise flags to unlock stealth spells in the spellbook
        StealthNodes = 1501,          // Bitwise flags for the stealth skill tree (if needed)
        StealthStepsBonus = 1502,     // Each point adds extra allowed stealth steps
        StealthDetectionBonus = 1503, // Each point reduces the chance to be detected
        StealthSpeedBonus = 1504,     // Increases movement speed while stealthed
        StealthRecoveryBonus = 1505,  // Reduces recovery time after being detected
        StealthDodgeBonus = 1506,     // Improves chance to dodge when stealthed
        StealthDefenseBonus = 1507,    // Provides a defensive bonus while stealthed		
		
		//TasteID
        TasteIDSpells = 1600,      // Stores bitflags unlocking TasteID spells
        TasteIDNodes = 1601,       // Bitflags for unlocked nodes
        TasteIDSensitivity = 1602, // Passive bonus – e.g., improved palate
        TasteIDAnalysis = 1603,    // Passive bonus – e.g., ingredient discrimination
        TasteIDRefinement = 1604,   // Passive bonus – e.g., subtle flavor enhancement		
		
        // NEW: Tracking Talents (IDs chosen arbitrarily – ensure they’re unique)
        TrackingSpells = 1700,
        TrackingNodes = 1701,
        TrackingRange = 1702,
        TrackingStealth = 1703,
        TrackingDetection = 1704,
		
        // New Archery talents (use unique numbers not conflicting with others)
        ArcherySpells = 1800,
        ArcheryNodes = 1801,
        ArcheryAccuracy = 1802,
        ArcheryDamage = 1803,
        ArcheryDrawSpeed = 1804,
        ArcheryCritical = 1805,
        ArcheryArrowRecovery = 1806,		
		
        // New Camping talent entries
        CampingSpells = 1900,         // Bit flags for unlocking camping spells in the spellbook
        CampingNodes = 1901,          // Tracks which Camping nodes have been activated
        CampingTentDuration = 1902,   // Bonus: reduces tent setup time (or increases tent duration)
        CampingCampfireEfficiency = 1903, // Bonus: improves campfire effects (yield, efficiency, etc.)
        CampingRestoration = 1904,    // Bonus: improves healing/restoration while camping
        CampingTravelBonus = 1905,     // Bonus: improves travel-related effects while camping		

        // New Magery Talent Entries
        MagerySpells = 2000,
        MageryNodes = 2001,
        MageryManaRegen = 2002,
        MageryCastSpeed = 2003,
        MagerySpellPower = 2004,
        MageryManaPool = 2005,
        MageryMagicResist = 2006,
        MageryXPBonus = 2007,
		
		// Animal Taming talents (using unique values):
        AnimalTamingSpells = 2100,
        AnimalTamingNodes = 2101,
        AnimalTamingControl = 2102,    // Increases pet control/obedience
        AnimalTamingSpeed = 2103,      // Boosts pet movement/speed
        AnimalTamingStamina = 2104,    // Increases pet stamina
        AnimalTamingBonding = 2105,    // Improves pet-owner bonding
        AnimalTamingInstinct = 2106,   // Enhances taming chance/instinct
        AnimalTamingResilience = 2107,  // Passively boosts pet combat/resilience

        AnimalLoreSpells = 2200,
        AnimalLoreNodes = 2201,
        AnimalLoreRange = 2202,      // Increases detection range
        AnimalLoreAgility = 2203,    // Enhances movement speed and attack speed
        AnimalLoreTaming = 2204,     // Improves animal taming success
        AnimalLoreGroup = 2205,      // Enhances group coordination
        AnimalLoreEmpathy = 2206,    // Increases animal friendliness
        AnimalLoreHealth = 2207,     // Boosts health or regeneration when hunting
        AnimalLoreFocus = 2208,      // Improves concentration/stealth
        AnimalLoreStealth = 2209,    // Enhances stealth detection bonus
        AnimalLoreDamage = 2210,     // Boosts damage with animal companions
        AnimalLoreSummon = 2211,      // Improves summoned creature effectiveness	

        AlchemySpells = 2300,
        AlchemyNodes = 2301,
        AlchemyEfficiency = 2302,
        AlchemyYield = 2303,
        AlchemyPotency = 2304,

        CarpentryNodes = 2400,
        CarpentrySpells = 2401,
        CarpentryEfficiency = 2402,
        CarpentryRange = 2403,
        CarpentryYield = 2404,

		CartographySpells = 2500,
		CartographyNodes = 2501,
		CartographyAccuracy = 2502,
		CartographyEfficiency = 2503,
		CartographyMapping = 2504,

        // New Cooking talents
        CookingSpells = 2600,      // Bit flags for unlocking cooking spells in the tome.
        CookingNodes = 2601,       // Bit flags for unlocked cooking tree nodes.
        CookingEfficiency = 2602,  // Passive bonus: reduces resource usage or improves ingredient quality.
        CookingSpeed = 2603,       // Passive bonus: faster cooking actions.
        CookingFlavor = 2604,       // Passive bonus: improves final food quality.		
		
        // New Fencing talents
        FencingSpells = 2700,
        FencingNodes = 2701,
        FencingAccuracy = 2702,
        FencingEvasion = 2703,
        FencingSpeed = 2704,
        FencingDamage = 2705,

        // New Fletching talents
        FletchingSpells = 2800,
        FletchingNodes = 2801,
        FletchingAccuracy = 2802,
        FletchingSpeed = 2803,
        FletchingYield = 2804,
        FletchingRange = 2805,		
		
        // New Wrestling talents
        WrestlingSpells = 2900,
        WrestlingNodes = 2901,
        WrestlingPower = 2902,
        WrestlingAgility = 2903,
        WrestlingStamina = 2904,
        WrestlingTechnique = 2905,		
		
		// New Parry talent entries
		ParrySpells = 3000,
		ParryNodes = 3001,
		ParryBlock = 3002,      // Passive: improves blocking/defense
		ParryCounter = 3003,    // Passive: improves counterattack bonus
		ParryAgility = 3004,    // Passive: increases parry speed/reactivity
		ParryStamina = 3005,     // Passive: boosts stamina for parrying		
		
        HealingSpells = 3110,
        HealingNodes = 3111,
        HealingPower = 3112,
        HealingCastSpeed = 3113,
        HealingEfficiency = 3114,

        // New Lockpicking talent entries
        LockpickingSpells = 3200,
        LockpickingNodes = 3201,
        LockpickingChance = 3202,
        LockpickingSpeed = 3203,
        LockpickingDurability = 3204,
        LockpickingStealth = 3205,	

		// New Macing Talent IDs
		MacingSpells = 3300,
		MacingNodes = 3301,
		MacingGripBonus = 3302,
		MacingSpeedBonus = 3303,
		MacingDamageBonus = 3304,
		MacingCriticalBonus = 3305,
		MacingArmorPenetrationBonus = 3306,
		MacingDefenseBonus = 3307,
		MacingStunChance = 3308,
		MacingSecondaryAttack = 3309,
		MacingDamageReduction = 3310,
		MacingParryBonus = 3311,
		MacingRangeBonus = 3312,
		MacingSpellPowerBonus = 3313,	

        // New Chivalry talent entries
        ChivalrySpells = 3450,
        ChivalryNodes = 3451,
        ChivalryDefense = 3452,
        ChivalryDamage = 3453,
        ChivalryHealing = 3454,		
		
		// New Pastoralicon/Herding talents:
		PastoraliconNodes = 3500,
		PastoraliconSpells = 3501,
		PastoraliconGuidance = 3502,     // e.g., increases flock awareness (range bonus)
		PastoraliconEfficiency = 3503,   // e.g., improves handling/command speed
		PastoraliconYield = 3504,         // e.g., increases yield (bonus wool/livestock products)	

        // New Inscription talents
        InscribeSpells = 3600,
        InscribeNodes = 3601,
        InscribeAccuracy = 3602,
        InscribeEfficiency = 3603,
        InscribeYield = 3604,	

        // New Ninjitsu talent entries
        NinjitsuSpells = 3700,
        NinjitsuNodes = 3701,
        NinjitsuStealth = 3702,
        NinjitsuSpeed = 3703,
        NinjitsuPrecision = 3704,
        NinjitsuPower = 3705,
        NinjitsuEvasion = 3706,
        NinjitsuAmbush = 3707,

        BlacksmithSpells = 3800,
        BlacksmithNodes = 3801,
        BlacksmithEfficiency = 3802,   // e.g., forging speed
        BlacksmithStrength = 3803,     // e.g., weapon strength bonus
        BlacksmithQuality = 3804,       // e.g., improved item quality	

        // New Tactics talent entries:
        TacticsSpells = 3900,  // Bitwise flags for unlocking tactics spells in the spellbook.
        TacticsNodes = 3901,   // Bitwise flags to track which tactics nodes have been unlocked.
        TacticsPassive = 3902,  // A counter for passive bonuses granted by tactics nodes.	

        // NEW: Swordsmanship talents
        SwordsSpellbookSpells = 4000,   // Bitmask for unlocked spells
        SwordsNodes = 4001,             // Bitflags for sword skill tree nodes
        SwordsAttack = 4002,            // Passive bonus: increased damage
        SwordsDefense = 4003,           // Passive bonus: improved defense/dodge
        SwordsSpeed = 4004,              // Passive bonus: increased attack speed		
		
		// New Tailoring talents:
		TailoringSpells = 4100,
		TailoringNodes = 4101,
		TailoringEfficiency = 4102,    // e.g., faster crafting/cutting speed
		TailoringQuality = 4103,       // e.g., improved cloth yield or quality
		TailoringCreativity = 4104,     // e.g., unlocks special design patterns		
		
        // New Necromancy talents:
        NecromancySpells = 4200,
        NecromancyNodes = 4201,
        NecromancyEfficiency = 4202,
        NecromancyYield = 4203,
        NecromancyRange = 4204,
        NecromancySummon = 4205,		
		
        // Veterinary talents
        VeterinarySpells = 4300,    // Used by the spellbook to display unlocked spells
        VeterinaryNodes = 4301,     // Bitfield tracking which veterinary nodes have been activated
        VeterinaryHealing = 4302,   // Passive bonus: improves pet healing
        VeterinaryBonding = 4303,   // Passive bonus: strengthens pet bonding
        VeterinaryStamina = 4304,   // Passive bonus: boosts pet stamina
        VeterinarySpeed = 4305,     // Passive bonus: increases pet speed or reaction
        VeterinaryEmpathy = 4306,   // Passive bonus: improves taming/chance for animal empathy
        VeterinaryWisdom = 4307,     // Passive bonus: improves overall pet-related skill effects		
		
		// New Musicianship talents (IDs chosen to be unique)
		MusicianshipSpells = 4400,      // Bitflags will be used to unlock spells in the MusicianshipSpellbook.
		MusicianshipNodes = 4401,       // Used to track activated nodes (if needed).
		MusicianshipPerformance = 4402, // Bonus: affects performance range (or similar).
		MusicianshipTechnique = 4403,   // Bonus: increases playing speed or efficiency.
		MusicianshipResonance = 4404,    // Bonus: increases bonus yield effects.	
		
		LogCollectionRegular    = 4500,
		LogCollectionHeartwood  = 4501,
		LogCollectionBloodwood  = 4502,
		LogCollectionFrostwood  = 4503,
		LogCollectionOak        = 4504,
		LogCollectionAsh        = 4505,
		LogCollectionYew        = 4506,
		
		// New Talent for Iron Ingot Collection quest
		IronIngotCollection     = 4507,
		DullCopperIngotCollection = 4508,
		ShadowIronIngotCollection = 4509,
		CopperIngotQuest        = 4510,
		BronzeIngotCollection   = 4511,
		GoldIngotCollection     = 4512,
		AgapiteIngotCollection  = 4513,
		VeriteCollection = 4514,
		ValoriteCollection = 4515,
		HideCollection = 4516,
		SpinedHideCollection    = 4517,
		HornedHideCollection    = 4518,
		StarSapphireCollection  = 4519,
		EmeraldCollection       = 4520,
		SapphireCollection = 4521,
		RubyCollection          = 4522,
		CitrineCollection = 4523,
		AmethystCollection      = 4524,
		TourmalineCollection    = 4525,
		AmberCollection         = 4526,
		DiamondCollection       = 4527,
		
		OrcSlayerQuest = 4528,
		AbysmalHorrorQuest = 4529,
		AcidElementalQuest = 4530,
		AirElementalQuest = 4531,
		AlligatorSlayerQuest = 4532,
		BakeKitsuneSlayerQuest = 4533,
		BalronSlayerQuest = 4534,
		BloodElementalQuest = 4535,
		BrownBearSlayerQuest = 4536,
		CorpserSlayerQuest = 4537,
		CuSidheHunterQuest = 4538,
		CyclopsSlayerQuest = 4539,
		KillDaemonQuest = 4540,
		DesertOstardQuest = 4541,
		DireWolfSlayerQuest = 4542,
		DolphinSlayerQuest = 4543,
		DragonSlayerQuest = 4544,
		DrakeSlayerQuest = 4545,
		DreadSpiderQuest = 4546,
		EarthElementalSlayerQuest = 4547,
		EttinSlayerQuest = 4548,
		FireElementalSlayerQuest = 4549,
		ForestOstardHunter = 4550,
		FrenziedOstardSlayerQuest = 4551,
		GargoyleSlayerQuest = 4552,
		GazerSlayerQuest = 4553,
		GhoulSlayerQuest = 4554,
		GiantSerpentSlayerQuest = 4555,
		GiantSpiderSlayerQuest = 4556,
		ToadSlayerQuest = 4557,
		GoatSlayerQuest = 4558,
		GorillaSlayerQuest = 4559,
		GreatHartSlayerQuest = 4560,
		GrizzlyBearSlayerQuest = 4561,
		HarpySlayerQuest = 4562,
		HeadlessSlayerQuest = 4563,
		HindSlayerQuest = 4564,
		HiryuSlayerQuest = 4565,
		HorseSlayerQuest = 4566,
		JukaLordSlayerQuest = 4567,
		JukaMageSlayerQuest = 4568,
		JukaSlayerQuest = 4569,
		KirinSlayerQuest = 4570,
		LesserHiryuSlayerQuest = 4571,
		LichSlayerQuest = 4572,
		LichLordHunterQuest = 4573,
		LionSlayerQuest = 4574,
		LizardmanSlayerQuest = 4575,
		LlamaSlayerQuest = 4576,
		MinotaurSlayerQuest = 4577,
		MongbatSlayerQuest = 4578,
		MummySlayerQuest = 4579,
		NightmareSlayerQuest = 4580,
		OgreSlayerQuest = 4581,
		OgreLordSlayerQuest = 4582,
		OphidianWarriorSlayer = 4583,
		OrcCaptainSlayerQuest = 4584,
		PantherSlayerQuest = 4585,


		
		MinionDamageBonus = 4600,
		
		SpiritSpeakSpells = 4601,
		SpiritSpeakNodes = 4602,
		
		
		
        AtlasNodes = 5000      // Bitflags for unlocked Atlas nodes/regions
	
		
		
    }

    public class Talent
    {
        public TalentID ID { get; private set; }
        public int Points { get; set; }

        public Talent(TalentID id)
        {
            ID = id;
        }

        public Talent(GenericReader reader)
        {
            Deserialize(reader);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0); // version
            writer.Write((int)ID);
            writer.Write(Points);
        }

        public void Deserialize(GenericReader reader)
        {
            reader.ReadInt(); // version
            ID = (TalentID)reader.ReadInt();
            Points = reader.ReadInt();
        }
    }

    public class TalentProfile
    {
        public PlayerMobile Owner { get; private set; }
        public Dictionary<TalentID, Talent> Talents { get; private set; } = new Dictionary<TalentID, Talent>();

        // NEW: XP and Level properties
        public int XP { get; set; }
        public int Level { get; set; }

        public TalentProfile(PlayerMobile owner)
        {
            Owner = owner;
            XP = 0;
            Level = 1;
        }

        public TalentProfile(GenericReader reader)
        {
            Deserialize(reader);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(2); // version 2
            writer.Write(Owner?.Serial ?? Serial.MinusOne); // Save the owner's serial
            writer.Write(Talents.Count);
            foreach (var talent in Talents.Values)
            {
                talent.Serialize(writer);
            }
            writer.Write(XP);
            writer.Write(Level);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            Serial ownerSerial = reader.ReadInt(); // Read the stored owner's serial

            int talentCount = reader.ReadInt();
            while (--talentCount >= 0)
            {
                var talent = new Talent(reader);
                if (Enum.IsDefined(typeof(TalentID), talent.ID))
                {
                    Talents[talent.ID] = talent;
                }
            }

            if (version >= 1)
            {
                XP = reader.ReadInt();
                Level = reader.ReadInt();
            }

            if (version >= 2)
            {
                Owner = World.FindMobile(ownerSerial) as PlayerMobile; // Restore Owner
            }
        }


    }

    public static class Talents
    {
        public static Dictionary<PlayerMobile, TalentProfile> Profiles { get; private set; } = new Dictionary<PlayerMobile, TalentProfile>();

        public static void Configure()
        {
            EventSink.WorldSave += e => Persistence.Serialize(@"Saves\\Talents\\Profiles.bin", Save);
            EventSink.WorldLoad += () => Persistence.Deserialize(@"Saves\\Talents\\Profiles.bin", Load);
        }

        public static void Save(GenericWriter writer)
        {
            writer.Write(0); // version
            writer.Write(Profiles.Count);
            foreach (var profile in Profiles.Values)
            {
                profile.Serialize(writer);
            }
        }

        public static void Load(GenericReader reader)
        {
            reader.ReadInt(); // version
            var profileCount = reader.ReadInt();
            while (--profileCount >= 0)
            {
                var profile = new TalentProfile(reader);
                if (profile.Owner != null)
                {
                    Profiles[profile.Owner] = profile;
                }
            }
        }

		private static readonly TalentProfile EmptyProfile = new TalentProfile((PlayerMobile)null);

		public static TalentProfile AcquireTalents(this PlayerMobile player)
		{
			if (player == null)
				return EmptyProfile; // ✅ Always returns the same safe empty profile

			if (!Profiles.TryGetValue(player, out var profile))
			{
				profile = new TalentProfile(player);
				Profiles[player] = profile;
			}

			// Ensure AncientKnowledge exists
			if (!profile.Talents.ContainsKey(TalentID.AncientKnowledge))
			{
				profile.Talents[TalentID.AncientKnowledge] = new Talent(TalentID.AncientKnowledge) { Points = 0 };
			}
			// Existing talents for Stealing…
			if (!profile.Talents.ContainsKey(TalentID.StealingSpells))
			{
				profile.Talents[TalentID.StealingSpells] = new Talent(TalentID.StealingSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingNodes))
			{
				profile.Talents[TalentID.StealingNodes] = new Talent(TalentID.StealingNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingDistance))
			{
				profile.Talents[TalentID.StealingDistance] = new Talent(TalentID.StealingDistance) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingWeight))
			{
				profile.Talents[TalentID.StealingWeight] = new Talent(TalentID.StealingWeight) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingEquipped))
			{
				profile.Talents[TalentID.StealingEquipped] = new Talent(TalentID.StealingEquipped) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingLoS))
			{
				profile.Talents[TalentID.StealingLoS] = new Talent(TalentID.StealingLoS) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.StealingImmovable))
			{
				profile.Talents[TalentID.StealingImmovable] = new Talent(TalentID.StealingImmovable) { Points = 0 };
			}

			// --- New: Ensure Lumberjacking talents exist ---
			if (!profile.Talents.ContainsKey(TalentID.LumberjackingSpells))
			{
				profile.Talents[TalentID.LumberjackingSpells] = new Talent(TalentID.LumberjackingSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.LumberjackingNodes))
			{
				profile.Talents[TalentID.LumberjackingNodes] = new Talent(TalentID.LumberjackingNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.LumberjackingRange))
			{
				profile.Talents[TalentID.LumberjackingRange] = new Talent(TalentID.LumberjackingRange) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.LumberjackingEfficiency))
			{
				profile.Talents[TalentID.LumberjackingEfficiency] = new Talent(TalentID.LumberjackingEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.LumberjackingYield))
			{
				profile.Talents[TalentID.LumberjackingYield] = new Talent(TalentID.LumberjackingYield) { Points = 0 };
			}
			
            // Ensure Mining talents exist.
            if (!profile.Talents.ContainsKey(TalentID.MiningSpells))
            {
                profile.Talents[TalentID.MiningSpells] = new Talent(TalentID.MiningSpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MiningNodes))
            {
                profile.Talents[TalentID.MiningNodes] = new Talent(TalentID.MiningNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MiningYield))
            {
                profile.Talents[TalentID.MiningYield] = new Talent(TalentID.MiningYield) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MiningEfficiency))
            {
                profile.Talents[TalentID.MiningEfficiency] = new Talent(TalentID.MiningEfficiency) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MiningRange))
            {
                profile.Talents[TalentID.MiningRange] = new Talent(TalentID.MiningRange) { Points = 0 };
            }

            // NEW: Ensure Begging talents exist
            // Ensure BeggingSpells exists
            if (!profile.Talents.ContainsKey(TalentID.BeggingSpells))
            {
                profile.Talents[TalentID.BeggingSpells] = new Talent(TalentID.BeggingSpells) { Points = 0 };
            }

            // Ensure BeggingNodes exists
            if (!profile.Talents.ContainsKey(TalentID.BeggingNodes))
            {
                profile.Talents[TalentID.BeggingNodes] = new Talent(TalentID.BeggingNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.BeggingPersuasion))
            {
                profile.Talents[TalentID.BeggingPersuasion] = new Talent(TalentID.BeggingPersuasion) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.BeggingCharm))
            {
                profile.Talents[TalentID.BeggingCharm] = new Talent(TalentID.BeggingCharm) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.BeggingKarma))
            {
                profile.Talents[TalentID.BeggingKarma] = new Talent(TalentID.BeggingKarma) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.BeggingLuck))
            {
                profile.Talents[TalentID.BeggingLuck] = new Talent(TalentID.BeggingLuck) { Points = 0 };
            }
			
            // NEW: Ensure Fishing talents exist
            if (!profile.Talents.ContainsKey(TalentID.FishingSpells))
                profile.Talents[TalentID.FishingSpells] = new Talent(TalentID.FishingSpells) { Points = 0 };

            if (!profile.Talents.ContainsKey(TalentID.FishingNodes))
                profile.Talents[TalentID.FishingNodes] = new Talent(TalentID.FishingNodes) { Points = 0 };

            if (!profile.Talents.ContainsKey(TalentID.FishingYield))
                profile.Talents[TalentID.FishingYield] = new Talent(TalentID.FishingYield) { Points = 0 };

            if (!profile.Talents.ContainsKey(TalentID.FishingRange))
                profile.Talents[TalentID.FishingRange] = new Talent(TalentID.FishingRange) { Points = 0 };

            if (!profile.Talents.ContainsKey(TalentID.FishingEfficiency))
                profile.Talents[TalentID.FishingEfficiency] = new Talent(TalentID.FishingEfficiency) { Points = 0 };

            if (!profile.Talents.ContainsKey(TalentID.FishingLuck))
                profile.Talents[TalentID.FishingLuck] = new Talent(TalentID.FishingLuck) { Points = 0 };

            // Ensure Martial Manual talents
            if (!profile.Talents.ContainsKey(TalentID.MartialManualSpells))
            {
                profile.Talents[TalentID.MartialManualSpells] = new Talent(TalentID.MartialManualSpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MartialManualNodes))
            {
                profile.Talents[TalentID.MartialManualNodes] = new Talent(TalentID.MartialManualNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MartialManualDamageBonus))
            {
                profile.Talents[TalentID.MartialManualDamageBonus] = new Talent(TalentID.MartialManualDamageBonus) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MartialManualDefenseBonus))
            {
                profile.Talents[TalentID.MartialManualDefenseBonus] = new Talent(TalentID.MartialManualDefenseBonus) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MartialManualAccuracyBonus))
            {
                profile.Talents[TalentID.MartialManualAccuracyBonus] = new Talent(TalentID.MartialManualAccuracyBonus) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.MartialManualSpeedBonus))
            {
                profile.Talents[TalentID.MartialManualSpeedBonus] = new Talent(TalentID.MartialManualSpeedBonus) { Points = 0 };
            }			
			
            // Ensure Detect Hidden talents exist
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenNodes))
                profile.Talents[TalentID.DetectHiddenNodes] = new Talent(TalentID.DetectHiddenNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenSpells))
                profile.Talents[TalentID.DetectHiddenSpells] = new Talent(TalentID.DetectHiddenSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenRange))
                profile.Talents[TalentID.DetectHiddenRange] = new Talent(TalentID.DetectHiddenRange) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenChance))
                profile.Talents[TalentID.DetectHiddenChance] = new Talent(TalentID.DetectHiddenChance) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DetectHiddenStealthReduction))
                profile.Talents[TalentID.DetectHiddenStealthReduction] = new Talent(TalentID.DetectHiddenStealthReduction) { Points = 0 };
			
            // Ensure Discordance talents exist
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceNodes))
                profile.Talents[TalentID.DiscordanceNodes] = new Talent(TalentID.DiscordanceNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceSpells))
                profile.Talents[TalentID.DiscordanceSpells] = new Talent(TalentID.DiscordanceSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceRange))
                profile.Talents[TalentID.DiscordanceRange] = new Talent(TalentID.DiscordanceRange) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceEffect))
                profile.Talents[TalentID.DiscordanceEffect] = new Talent(TalentID.DiscordanceEffect) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DiscordanceCastSpeed))
                profile.Talents[TalentID.DiscordanceCastSpeed] = new Talent(TalentID.DiscordanceCastSpeed) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.DiscordancePassive))
                profile.Talents[TalentID.DiscordancePassive] = new Talent(TalentID.DiscordancePassive) { Points = 0 };		

			// Ensure EvalInt talents exist
			if (!profile.Talents.ContainsKey(TalentID.EvalIntSpells))
			{
				profile.Talents[TalentID.EvalIntSpells] = new Talent(TalentID.EvalIntSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.EvalIntNodes))
			{
				profile.Talents[TalentID.EvalIntNodes] = new Talent(TalentID.EvalIntNodes) { Points = 0 };
			}	

			// Ensure ForensicSpells exists
			if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicSpells))
			{
				profile.Talents[(TalentID)TalentID.ForensicSpells] = new Talent((TalentID)TalentID.ForensicSpells) { Points = 0 };
			}
			// Ensure ForensicNodes exists
			if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicNodes))
			{
				profile.Talents[(TalentID)TalentID.ForensicNodes] = new Talent((TalentID)TalentID.ForensicNodes) { Points = 0 };
			}
			// Ensure ForensicInsight exists
			if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicInsight))
			{
				profile.Talents[(TalentID)TalentID.ForensicInsight] = new Talent((TalentID)TalentID.ForensicInsight) { Points = 0 };
			}
			// Ensure ForensicEfficiency exists
			if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicEfficiency))
			{
				profile.Talents[(TalentID)TalentID.ForensicEfficiency] = new Talent((TalentID)TalentID.ForensicEfficiency) { Points = 0 };
			}
			// Ensure ForensicRevelation exists
			if (!profile.Talents.ContainsKey((TalentID)TalentID.ForensicRevelation))
			{
				profile.Talents[(TalentID)TalentID.ForensicRevelation] = new Talent((TalentID)TalentID.ForensicRevelation) { Points = 0 };
			}
			
            // NEW: Ensure Hiding talents exist
            if (!profile.Talents.ContainsKey(TalentID.HidingSpells))
            {
                profile.Talents[TalentID.HidingSpells] = new Talent(TalentID.HidingSpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.HidingNodes))
            {
                profile.Talents[TalentID.HidingNodes] = new Talent(TalentID.HidingNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.HidingEfficiency))
            {
                profile.Talents[TalentID.HidingEfficiency] = new Talent(TalentID.HidingEfficiency) { Points = 0 };
            }
			
            // NEW: Ensure Meditation talents exist
            if (!profile.Talents.ContainsKey(TalentID.MeditationSpells))
                profile.Talents[TalentID.MeditationSpells] = new Talent(TalentID.MeditationSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.MeditationNodes))
                profile.Talents[TalentID.MeditationNodes] = new Talent(TalentID.MeditationNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.MeditationFocus))
                profile.Talents[TalentID.MeditationFocus] = new Talent(TalentID.MeditationFocus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.MeditationRecovery))
                profile.Talents[TalentID.MeditationRecovery] = new Talent(TalentID.MeditationRecovery) { Points = 0 };		
            if (!profile.Talents.ContainsKey(TalentID.TemporalStillness))
                profile.Talents[TalentID.TemporalStillness] = new Talent(TalentID.TemporalStillness) { Points = 0 };

			// Ensure Provocation talents exist
			if (!profile.Talents.ContainsKey(TalentID.ProvocationSpells))
				profile.Talents[TalentID.ProvocationSpells] = new Talent(TalentID.ProvocationSpells) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.ProvocationNodes))
				profile.Talents[TalentID.ProvocationNodes] = new Talent(TalentID.ProvocationNodes) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.ProvocationBonus))
				profile.Talents[TalentID.ProvocationBonus] = new Talent(TalentID.ProvocationBonus) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.ProvocationRange))
				profile.Talents[TalentID.ProvocationRange] = new Talent(TalentID.ProvocationRange) { Points = 0 };			
			if (!profile.Talents.ContainsKey(TalentID.ProvocationCooldownReduction))
				profile.Talents[TalentID.ProvocationCooldownReduction] = new Talent(TalentID.ProvocationCooldownReduction) { Points = 0 };	
			
			// Ensure Remove Trap talents exist
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapSpells))
				profile.Talents[TalentID.RemoveTrapSpells] = new Talent(TalentID.RemoveTrapSpells) { Points = 0 };	
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapNodes))
				profile.Talents[TalentID.RemoveTrapNodes] = new Talent(TalentID.RemoveTrapNodes) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapDetection))
				profile.Talents[TalentID.RemoveTrapDetection] = new Talent(TalentID.RemoveTrapDetection) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapSpeed))
				profile.Talents[TalentID.RemoveTrapSpeed] = new Talent(TalentID.RemoveTrapSpeed) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapSuccessChance))
				profile.Talents[TalentID.RemoveTrapSuccessChance] = new Talent(TalentID.RemoveTrapSuccessChance) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.RemoveTrapKitEfficiency))
				profile.Talents[TalentID.RemoveTrapKitEfficiency] = new Talent(TalentID.RemoveTrapKitEfficiency) { Points = 0 };

            // NEW: Ensure Stealth talents exist
            if (!profile.Talents.ContainsKey(TalentID.StealthSpells))
                profile.Talents[TalentID.StealthSpells] = new Talent(TalentID.StealthSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthNodes))
                profile.Talents[TalentID.StealthNodes] = new Talent(TalentID.StealthNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthStepsBonus))
                profile.Talents[TalentID.StealthStepsBonus] = new Talent(TalentID.StealthStepsBonus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthDetectionBonus))
                profile.Talents[TalentID.StealthDetectionBonus] = new Talent(TalentID.StealthDetectionBonus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthSpeedBonus))
                profile.Talents[TalentID.StealthSpeedBonus] = new Talent(TalentID.StealthSpeedBonus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthRecoveryBonus))
                profile.Talents[TalentID.StealthRecoveryBonus] = new Talent(TalentID.StealthRecoveryBonus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthDodgeBonus))
                profile.Talents[TalentID.StealthDodgeBonus] = new Talent(TalentID.StealthDodgeBonus) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.StealthDefenseBonus))
                profile.Talents[TalentID.StealthDefenseBonus] = new Talent(TalentID.StealthDefenseBonus) { Points = 0 };			
			// Ensure TasteIDSpells exists
			if (!profile.Talents.ContainsKey(TalentID.TasteIDSpells))
			{
				profile.Talents[TalentID.TasteIDSpells] = new Talent(TalentID.TasteIDSpells) { Points = 0 };
			}

			// Ensure TasteIDNodes exists
			if (!profile.Talents.ContainsKey(TalentID.TasteIDNodes))
			{
				profile.Talents[TalentID.TasteIDNodes] = new Talent(TalentID.TasteIDNodes) { Points = 0 };
			}

			// Ensure TasteIDSensitivity exists
			if (!profile.Talents.ContainsKey(TalentID.TasteIDSensitivity))
			{
				profile.Talents[TalentID.TasteIDSensitivity] = new Talent(TalentID.TasteIDSensitivity) { Points = 0 };
			}

			// Ensure TasteIDAnalysis exists
			if (!profile.Talents.ContainsKey(TalentID.TasteIDAnalysis))
			{
				profile.Talents[TalentID.TasteIDAnalysis] = new Talent(TalentID.TasteIDAnalysis) { Points = 0 };
			}

			// Ensure TasteIDRefinement exists
			if (!profile.Talents.ContainsKey(TalentID.TasteIDRefinement))
			{
				profile.Talents[TalentID.TasteIDRefinement] = new Talent(TalentID.TasteIDRefinement) { Points = 0 };
			}			

            // NEW: Ensure Tracking talents exist
            if (!profile.Talents.ContainsKey(TalentID.TrackingSpells))
                profile.Talents[TalentID.TrackingSpells] = new Talent(TalentID.TrackingSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.TrackingNodes))
                profile.Talents[TalentID.TrackingNodes] = new Talent(TalentID.TrackingNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.TrackingRange))
                profile.Talents[TalentID.TrackingRange] = new Talent(TalentID.TrackingRange) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.TrackingStealth))
                profile.Talents[TalentID.TrackingStealth] = new Talent(TalentID.TrackingStealth) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.TrackingDetection))
                profile.Talents[TalentID.TrackingDetection] = new Talent(TalentID.TrackingDetection) { Points = 0 };
	
            // Ensure new Archery talents exist.
            if (!profile.Talents.ContainsKey(TalentID.ArcherySpells))
                profile.Talents[TalentID.ArcherySpells] = new Talent(TalentID.ArcherySpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryNodes))
                profile.Talents[TalentID.ArcheryNodes] = new Talent(TalentID.ArcheryNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryAccuracy))
                profile.Talents[TalentID.ArcheryAccuracy] = new Talent(TalentID.ArcheryAccuracy) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryDamage))
                profile.Talents[TalentID.ArcheryDamage] = new Talent(TalentID.ArcheryDamage) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryDrawSpeed))
                profile.Talents[TalentID.ArcheryDrawSpeed] = new Talent(TalentID.ArcheryDrawSpeed) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryCritical))
                profile.Talents[TalentID.ArcheryCritical] = new Talent(TalentID.ArcheryCritical) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.ArcheryArrowRecovery))
                profile.Talents[TalentID.ArcheryArrowRecovery] = new Talent(TalentID.ArcheryArrowRecovery) { Points = 0 };

            // NEW: Ensure Camping talents exist
            if (!profile.Talents.ContainsKey(TalentID.CampingSpells))
            {
                profile.Talents[TalentID.CampingSpells] = new Talent(TalentID.CampingSpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.CampingNodes))
            {
                profile.Talents[TalentID.CampingNodes] = new Talent(TalentID.CampingNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.CampingTentDuration))
            {
                profile.Talents[TalentID.CampingTentDuration] = new Talent(TalentID.CampingTentDuration) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.CampingCampfireEfficiency))
            {
                profile.Talents[TalentID.CampingCampfireEfficiency] = new Talent(TalentID.CampingCampfireEfficiency) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.CampingRestoration))
            {
                profile.Talents[TalentID.CampingRestoration] = new Talent(TalentID.CampingRestoration) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.CampingTravelBonus))
            {
                profile.Talents[TalentID.CampingTravelBonus] = new Talent(TalentID.CampingTravelBonus) { Points = 0 };
            }

			// Ensure new Magery talents exist:
			if (!profile.Talents.ContainsKey(TalentID.MagerySpells))
			{
				profile.Talents[TalentID.MagerySpells] = new Talent(TalentID.MagerySpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryNodes))
			{
				profile.Talents[TalentID.MageryNodes] = new Talent(TalentID.MageryNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryManaRegen))
			{
				profile.Talents[TalentID.MageryManaRegen] = new Talent(TalentID.MageryManaRegen) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryCastSpeed))
			{
				profile.Talents[TalentID.MageryCastSpeed] = new Talent(TalentID.MageryCastSpeed) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MagerySpellPower))
			{
				profile.Talents[TalentID.MagerySpellPower] = new Talent(TalentID.MagerySpellPower) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryManaPool))
			{
				profile.Talents[TalentID.MageryManaPool] = new Talent(TalentID.MageryManaPool) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryMagicResist))
			{
				profile.Talents[TalentID.MageryMagicResist] = new Talent(TalentID.MageryMagicResist) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MageryXPBonus))
			{
				profile.Talents[TalentID.MageryXPBonus] = new Talent(TalentID.MageryXPBonus) { Points = 0 };
			}
			
			// Ensure new AnimalTaming talents exist:
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingSpells))
			{
				profile.Talents[TalentID.AnimalTamingSpells] = new Talent(TalentID.AnimalTamingSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingNodes))
			{
				profile.Talents[TalentID.AnimalTamingNodes] = new Talent(TalentID.AnimalTamingNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingControl))
			{
				profile.Talents[TalentID.AnimalTamingControl] = new Talent(TalentID.AnimalTamingControl) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingSpeed))
			{
				profile.Talents[TalentID.AnimalTamingSpeed] = new Talent(TalentID.AnimalTamingSpeed) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingStamina))
			{
				profile.Talents[TalentID.AnimalTamingStamina] = new Talent(TalentID.AnimalTamingStamina) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingBonding))
			{
				profile.Talents[TalentID.AnimalTamingBonding] = new Talent(TalentID.AnimalTamingBonding) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingInstinct))
			{
				profile.Talents[TalentID.AnimalTamingInstinct] = new Talent(TalentID.AnimalTamingInstinct) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AnimalTamingResilience))
			{
				profile.Talents[TalentID.AnimalTamingResilience] = new Talent(TalentID.AnimalTamingResilience) { Points = 0 };
			}

			// Ensure AnimalLore talents exist
			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreSpells))
			{
				profile.Talents[TalentID.AnimalLoreSpells] = new Talent(TalentID.AnimalLoreSpells) { Points = 0 }; // Default spells to 0
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreNodes))
			{
				profile.Talents[TalentID.AnimalLoreNodes] = new Talent(TalentID.AnimalLoreNodes) { Points = 0 }; // Default skill tree nodes to 0
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreRange))
			{
				profile.Talents[TalentID.AnimalLoreRange] = new Talent(TalentID.AnimalLoreRange) { Points = 0 }; // Increases detection range
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreAgility))
			{
				profile.Talents[TalentID.AnimalLoreAgility] = new Talent(TalentID.AnimalLoreAgility) { Points = 0 }; // Enhances movement speed & attack speed
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreTaming))
			{
				profile.Talents[TalentID.AnimalLoreTaming] = new Talent(TalentID.AnimalLoreTaming) { Points = 0 }; // Improves animal taming success
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreGroup))
			{
				profile.Talents[TalentID.AnimalLoreGroup] = new Talent(TalentID.AnimalLoreGroup) { Points = 0 }; // Enhances group coordination
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreEmpathy))
			{
				profile.Talents[TalentID.AnimalLoreEmpathy] = new Talent(TalentID.AnimalLoreEmpathy) { Points = 0 }; // Increases animal friendliness
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreHealth))
			{
				profile.Talents[TalentID.AnimalLoreHealth] = new Talent(TalentID.AnimalLoreHealth) { Points = 0 }; // Boosts health or regeneration in the wild
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreFocus))
			{
				profile.Talents[TalentID.AnimalLoreFocus] = new Talent(TalentID.AnimalLoreFocus) { Points = 0 }; // Improves spell casting focus
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreStealth))
			{
				profile.Talents[TalentID.AnimalLoreStealth] = new Talent(TalentID.AnimalLoreStealth) { Points = 0 }; // Enhances stealth detection bonus
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreDamage))
			{
				profile.Talents[TalentID.AnimalLoreDamage] = new Talent(TalentID.AnimalLoreDamage) { Points = 0 }; // Boosts damage with animal companions
			}

			if (!profile.Talents.ContainsKey(TalentID.AnimalLoreSummon))
			{
				profile.Talents[TalentID.AnimalLoreSummon] = new Talent(TalentID.AnimalLoreSummon) { Points = 0 }; // Improves summoned creature effectiveness
			}

			// Inside Talents.AcquireTalents extension method:
			if (!profile.Talents.ContainsKey(TalentID.AlchemySpells))
			{
				profile.Talents[TalentID.AlchemySpells] = new Talent(TalentID.AlchemySpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AlchemyNodes))
			{
				profile.Talents[TalentID.AlchemyNodes] = new Talent(TalentID.AlchemyNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AlchemyEfficiency))
			{
				profile.Talents[TalentID.AlchemyEfficiency] = new Talent(TalentID.AlchemyEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AlchemyYield))
			{
				profile.Talents[TalentID.AlchemyYield] = new Talent(TalentID.AlchemyYield) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AlchemyPotency))
			{
				profile.Talents[TalentID.AlchemyPotency] = new Talent(TalentID.AlchemyPotency) { Points = 0 };
			}

			// Ensure Carpentry talents exist:
			if (!profile.Talents.ContainsKey(TalentID.CarpentryNodes))
			{
				profile.Talents[TalentID.CarpentryNodes] = new Talent(TalentID.CarpentryNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CarpentrySpells))
			{
				profile.Talents[TalentID.CarpentrySpells] = new Talent(TalentID.CarpentrySpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CarpentryEfficiency))
			{
				profile.Talents[TalentID.CarpentryEfficiency] = new Talent(TalentID.CarpentryEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CarpentryRange))
			{
				profile.Talents[TalentID.CarpentryRange] = new Talent(TalentID.CarpentryRange) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CarpentryYield))
			{
				profile.Talents[TalentID.CarpentryYield] = new Talent(TalentID.CarpentryYield) { Points = 0 };
			}

			//Cartography
			if (!profile.Talents.ContainsKey(TalentID.CartographySpells))
			{
				profile.Talents[TalentID.CartographySpells] = new Talent(TalentID.CartographySpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CartographyNodes))
			{
				profile.Talents[TalentID.CartographyNodes] = new Talent(TalentID.CartographyNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CartographyAccuracy))
			{
				profile.Talents[TalentID.CartographyAccuracy] = new Talent(TalentID.CartographyAccuracy) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CartographyEfficiency))
			{
				profile.Talents[TalentID.CartographyEfficiency] = new Talent(TalentID.CartographyEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.CartographyMapping))
			{
				profile.Talents[TalentID.CartographyMapping] = new Talent(TalentID.CartographyMapping) { Points = 0 };
			}

            // Ensure Cooking talents exist
            if (!profile.Talents.ContainsKey(TalentID.CookingSpells))
                profile.Talents[TalentID.CookingSpells] = new Talent(TalentID.CookingSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.CookingNodes))
                profile.Talents[TalentID.CookingNodes] = new Talent(TalentID.CookingNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.CookingEfficiency))
                profile.Talents[TalentID.CookingEfficiency] = new Talent(TalentID.CookingEfficiency) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.CookingSpeed))
                profile.Talents[TalentID.CookingSpeed] = new Talent(TalentID.CookingSpeed) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.CookingFlavor))
                profile.Talents[TalentID.CookingFlavor] = new Talent(TalentID.CookingFlavor) { Points = 0 };

			// Ensure new Fencing talents exist.
			if (!profile.Talents.ContainsKey(TalentID.FencingSpells))
				profile.Talents[TalentID.FencingSpells] = new Talent(TalentID.FencingSpells) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.FencingNodes))
				profile.Talents[TalentID.FencingNodes] = new Talent(TalentID.FencingNodes) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.FencingAccuracy))
				profile.Talents[TalentID.FencingAccuracy] = new Talent(TalentID.FencingAccuracy) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.FencingEvasion))
				profile.Talents[TalentID.FencingEvasion] = new Talent(TalentID.FencingEvasion) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.FencingSpeed))
				profile.Talents[TalentID.FencingSpeed] = new Talent(TalentID.FencingSpeed) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.FencingDamage))
				profile.Talents[TalentID.FencingDamage] = new Talent(TalentID.FencingDamage) { Points = 0 };

			// Ensure FletchingSpells exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingSpells))
			{
				profile.Talents[TalentID.FletchingSpells] = new Talent(TalentID.FletchingSpells) { Points = 0 };
			}

			// Ensure FletchingNodes exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingNodes))
			{
				profile.Talents[TalentID.FletchingNodes] = new Talent(TalentID.FletchingNodes) { Points = 0 };
			}

			// Ensure FletchingAccuracy exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingAccuracy))
			{
				profile.Talents[TalentID.FletchingAccuracy] = new Talent(TalentID.FletchingAccuracy) { Points = 0 };
			}

			// Ensure FletchingSpeed exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingSpeed))
			{
				profile.Talents[TalentID.FletchingSpeed] = new Talent(TalentID.FletchingSpeed) { Points = 0 };
			}

			// Ensure FletchingYield exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingYield))
			{
				profile.Talents[TalentID.FletchingYield] = new Talent(TalentID.FletchingYield) { Points = 0 };
			}

			// Ensure FletchingRange exists
			if (!profile.Talents.ContainsKey(TalentID.FletchingRange))
			{
				profile.Talents[TalentID.FletchingRange] = new Talent(TalentID.FletchingRange) { Points = 0 };
			}

            // Ensure new Wrestling talents exist
            if (!profile.Talents.ContainsKey(TalentID.WrestlingSpells))
                profile.Talents[TalentID.WrestlingSpells] = new Talent(TalentID.WrestlingSpells) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.WrestlingNodes))
                profile.Talents[TalentID.WrestlingNodes] = new Talent(TalentID.WrestlingNodes) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.WrestlingPower))
                profile.Talents[TalentID.WrestlingPower] = new Talent(TalentID.WrestlingPower) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.WrestlingAgility))
                profile.Talents[TalentID.WrestlingAgility] = new Talent(TalentID.WrestlingAgility) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.WrestlingStamina))
                profile.Talents[TalentID.WrestlingStamina] = new Talent(TalentID.WrestlingStamina) { Points = 0 };
            if (!profile.Talents.ContainsKey(TalentID.WrestlingTechnique))
                profile.Talents[TalentID.WrestlingTechnique] = new Talent(TalentID.WrestlingTechnique) { Points = 0 };

			// Ensure Parry talents exist
			if (!profile.Talents.ContainsKey(TalentID.ParrySpells))
			{
				profile.Talents[TalentID.ParrySpells] = new Talent(TalentID.ParrySpells) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.ParryNodes))
			{
				profile.Talents[TalentID.ParryNodes] = new Talent(TalentID.ParryNodes) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.ParryBlock))
			{
				profile.Talents[TalentID.ParryBlock] = new Talent(TalentID.ParryBlock) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.ParryCounter))
			{
				profile.Talents[TalentID.ParryCounter] = new Talent(TalentID.ParryCounter) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.ParryAgility))
			{
				profile.Talents[TalentID.ParryAgility] = new Talent(TalentID.ParryAgility) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.ParryStamina))
			{
				profile.Talents[TalentID.ParryStamina] = new Talent(TalentID.ParryStamina) { Points = 0 };
			}

			// New Healing talents
			if (!profile.Talents.ContainsKey(TalentID.HealingSpells))
			{
				profile.Talents[TalentID.HealingSpells] = new Talent(TalentID.HealingSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.HealingNodes))
			{
				profile.Talents[TalentID.HealingNodes] = new Talent(TalentID.HealingNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.HealingPower))
			{
				profile.Talents[TalentID.HealingPower] = new Talent(TalentID.HealingPower) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.HealingCastSpeed))
			{
				profile.Talents[TalentID.HealingCastSpeed] = new Talent(TalentID.HealingCastSpeed) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.HealingEfficiency))
			{
				profile.Talents[TalentID.HealingEfficiency] = new Talent(TalentID.HealingEfficiency) { Points = 0 };
			}

			// Ensure LockpickingSpells exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingSpells))
			{
				profile.Talents[TalentID.LockpickingSpells] = new Talent(TalentID.LockpickingSpells) { Points = 0 };
			}

			// Ensure LockpickingNodes exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingNodes))
			{
				profile.Talents[TalentID.LockpickingNodes] = new Talent(TalentID.LockpickingNodes) { Points = 0 };
			}

			// Ensure LockpickingChance exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingChance))
			{
				profile.Talents[TalentID.LockpickingChance] = new Talent(TalentID.LockpickingChance) { Points = 0 };
			}

			// Ensure LockpickingSpeed exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingSpeed))
			{
				profile.Talents[TalentID.LockpickingSpeed] = new Talent(TalentID.LockpickingSpeed) { Points = 0 };
			}

			// Ensure LockpickingDurability exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingDurability))
			{
				profile.Talents[TalentID.LockpickingDurability] = new Talent(TalentID.LockpickingDurability) { Points = 0 };
			}

			// Ensure LockpickingStealth exists
			if (!profile.Talents.ContainsKey(TalentID.LockpickingStealth))
			{
				profile.Talents[TalentID.LockpickingStealth] = new Talent(TalentID.LockpickingStealth) { Points = 0 };
			}

			// Ensure MacingSpells exists
			if (!profile.Talents.ContainsKey(TalentID.MacingSpells))
			{
				profile.Talents[TalentID.MacingSpells] = new Talent(TalentID.MacingSpells) { Points = 0 };
			}

			// Ensure MacingNodes exists
			if (!profile.Talents.ContainsKey(TalentID.MacingNodes))
			{
				profile.Talents[TalentID.MacingNodes] = new Talent(TalentID.MacingNodes) { Points = 0 };
			}

			// Ensure each passive bonus talent exists:
			if (!profile.Talents.ContainsKey(TalentID.MacingGripBonus))
			{
				profile.Talents[TalentID.MacingGripBonus] = new Talent(TalentID.MacingGripBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingSpeedBonus))
			{
				profile.Talents[TalentID.MacingSpeedBonus] = new Talent(TalentID.MacingSpeedBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingDamageBonus))
			{
				profile.Talents[TalentID.MacingDamageBonus] = new Talent(TalentID.MacingDamageBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingCriticalBonus))
			{
				profile.Talents[TalentID.MacingCriticalBonus] = new Talent(TalentID.MacingCriticalBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingArmorPenetrationBonus))
			{
				profile.Talents[TalentID.MacingArmorPenetrationBonus] = new Talent(TalentID.MacingArmorPenetrationBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingDefenseBonus))
			{
				profile.Talents[TalentID.MacingDefenseBonus] = new Talent(TalentID.MacingDefenseBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingStunChance))
			{
				profile.Talents[TalentID.MacingStunChance] = new Talent(TalentID.MacingStunChance) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingSecondaryAttack))
			{
				profile.Talents[TalentID.MacingSecondaryAttack] = new Talent(TalentID.MacingSecondaryAttack) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingDamageReduction))
			{
				profile.Talents[TalentID.MacingDamageReduction] = new Talent(TalentID.MacingDamageReduction) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingParryBonus))
			{
				profile.Talents[TalentID.MacingParryBonus] = new Talent(TalentID.MacingParryBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingRangeBonus))
			{
				profile.Talents[TalentID.MacingRangeBonus] = new Talent(TalentID.MacingRangeBonus) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MacingSpellPowerBonus))
			{
				profile.Talents[TalentID.MacingSpellPowerBonus] = new Talent(TalentID.MacingSpellPowerBonus) { Points = 0 };
			}

			//Chiv
			if (!profile.Talents.ContainsKey(TalentID.ChivalrySpells))
			{
				profile.Talents[TalentID.ChivalrySpells] = new Talent(TalentID.ChivalrySpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.ChivalryNodes))
			{
				profile.Talents[TalentID.ChivalryNodes] = new Talent(TalentID.ChivalryNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.ChivalryDefense))
			{
				profile.Talents[TalentID.ChivalryDefense] = new Talent(TalentID.ChivalryDefense) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.ChivalryDamage))
			{
				profile.Talents[TalentID.ChivalryDamage] = new Talent(TalentID.ChivalryDamage) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.ChivalryHealing))
			{
				profile.Talents[TalentID.ChivalryHealing] = new Talent(TalentID.ChivalryHealing) { Points = 0 };
			}

			// Ensure Pastoralicon talents exist
			if (!profile.Talents.ContainsKey(TalentID.PastoraliconNodes))
			{
				profile.Talents[TalentID.PastoraliconNodes] = new Talent(TalentID.PastoraliconNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.PastoraliconSpells))
			{
				profile.Talents[TalentID.PastoraliconSpells] = new Talent(TalentID.PastoraliconSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.PastoraliconGuidance))
			{
				profile.Talents[TalentID.PastoraliconGuidance] = new Talent(TalentID.PastoraliconGuidance) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.PastoraliconEfficiency))
			{
				profile.Talents[TalentID.PastoraliconEfficiency] = new Talent(TalentID.PastoraliconEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.PastoraliconYield))
			{
				profile.Talents[TalentID.PastoraliconYield] = new Talent(TalentID.PastoraliconYield) { Points = 0 };
			}

			// Ensure Inscription talents exist
			if (!profile.Talents.ContainsKey(TalentID.InscribeSpells))
				profile.Talents[TalentID.InscribeSpells] = new Talent(TalentID.InscribeSpells) { Points = 0 };

			if (!profile.Talents.ContainsKey(TalentID.InscribeNodes))
				profile.Talents[TalentID.InscribeNodes] = new Talent(TalentID.InscribeNodes) { Points = 0 };

			if (!profile.Talents.ContainsKey(TalentID.InscribeAccuracy))
				profile.Talents[TalentID.InscribeAccuracy] = new Talent(TalentID.InscribeAccuracy) { Points = 0 };

			if (!profile.Talents.ContainsKey(TalentID.InscribeEfficiency))
				profile.Talents[TalentID.InscribeEfficiency] = new Talent(TalentID.InscribeEfficiency) { Points = 0 };

			if (!profile.Talents.ContainsKey(TalentID.InscribeYield))
				profile.Talents[TalentID.InscribeYield] = new Talent(TalentID.InscribeYield) { Points = 0 };

            // Ensure Ninjitsu talents exist.
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuSpells))
            {
                profile.Talents[TalentID.NinjitsuSpells] = new Talent(TalentID.NinjitsuSpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuNodes))
            {
                profile.Talents[TalentID.NinjitsuNodes] = new Talent(TalentID.NinjitsuNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuStealth))
            {
                profile.Talents[TalentID.NinjitsuStealth] = new Talent(TalentID.NinjitsuStealth) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuSpeed))
            {
                profile.Talents[TalentID.NinjitsuSpeed] = new Talent(TalentID.NinjitsuSpeed) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuPrecision))
            {
                profile.Talents[TalentID.NinjitsuPrecision] = new Talent(TalentID.NinjitsuPrecision) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuPower))
            {
                profile.Talents[TalentID.NinjitsuPower] = new Talent(TalentID.NinjitsuPower) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuEvasion))
            {
                profile.Talents[TalentID.NinjitsuEvasion] = new Talent(TalentID.NinjitsuEvasion) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NinjitsuAmbush))
            {
                profile.Talents[TalentID.NinjitsuAmbush] = new Talent(TalentID.NinjitsuAmbush) { Points = 0 };
            }

			// Ensure BlacksmithSpells exists
			if (!profile.Talents.ContainsKey(TalentID.BlacksmithSpells))
			{
				profile.Talents[TalentID.BlacksmithSpells] = new Talent(TalentID.BlacksmithSpells) { Points = 0 };
			}

			// Ensure BlacksmithNodes exists
			if (!profile.Talents.ContainsKey(TalentID.BlacksmithNodes))
			{
				profile.Talents[TalentID.BlacksmithNodes] = new Talent(TalentID.BlacksmithNodes) { Points = 0 };
			}

			// Ensure BlacksmithEfficiency exists
			if (!profile.Talents.ContainsKey(TalentID.BlacksmithEfficiency))
			{
				profile.Talents[TalentID.BlacksmithEfficiency] = new Talent(TalentID.BlacksmithEfficiency) { Points = 0 };
			}

			// Ensure BlacksmithStrength exists
			if (!profile.Talents.ContainsKey(TalentID.BlacksmithStrength))
			{
				profile.Talents[TalentID.BlacksmithStrength] = new Talent(TalentID.BlacksmithStrength) { Points = 0 };
			}

			// Ensure BlacksmithQuality exists
			if (!profile.Talents.ContainsKey(TalentID.BlacksmithQuality))
			{
				profile.Talents[TalentID.BlacksmithQuality] = new Talent(TalentID.BlacksmithQuality) { Points = 0 };
			}

			// Ensure TacticsSpells exists
			if (!profile.Talents.ContainsKey(TalentID.TacticsSpells))
			{
				profile.Talents[TalentID.TacticsSpells] = new Talent(TalentID.TacticsSpells) { Points = 0 };
			}

			// Ensure TacticsNodes exists
			if (!profile.Talents.ContainsKey(TalentID.TacticsNodes))
			{
				profile.Talents[TalentID.TacticsNodes] = new Talent(TalentID.TacticsNodes) { Points = 0 };
			}

			// Ensure TacticsPassive exists
			if (!profile.Talents.ContainsKey(TalentID.TacticsPassive))
			{
				profile.Talents[TalentID.TacticsPassive] = new Talent(TalentID.TacticsPassive) { Points = 0 };
			}

			// NEW: Ensure Sword talents exist
			if (!profile.Talents.ContainsKey(TalentID.SwordsSpellbookSpells))
				profile.Talents[TalentID.SwordsSpellbookSpells] = new Talent(TalentID.SwordsSpellbookSpells) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.SwordsNodes))
				profile.Talents[TalentID.SwordsNodes] = new Talent(TalentID.SwordsNodes) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.SwordsAttack))
				profile.Talents[TalentID.SwordsAttack] = new Talent(TalentID.SwordsAttack) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.SwordsDefense))
				profile.Talents[TalentID.SwordsDefense] = new Talent(TalentID.SwordsDefense) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.SwordsSpeed))
				profile.Talents[TalentID.SwordsSpeed] = new Talent(TalentID.SwordsSpeed) { Points = 0 };

			// New Tailoring talent initializations:
			if (!profile.Talents.ContainsKey(TalentID.TailoringSpells))
			{
				profile.Talents[TalentID.TailoringSpells] = new Talent(TalentID.TailoringSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.TailoringNodes))
			{
				profile.Talents[TalentID.TailoringNodes] = new Talent(TalentID.TailoringNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.TailoringEfficiency))
			{
				profile.Talents[TalentID.TailoringEfficiency] = new Talent(TalentID.TailoringEfficiency) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.TailoringQuality))
			{
				profile.Talents[TalentID.TailoringQuality] = new Talent(TalentID.TailoringQuality) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.TailoringCreativity))
			{
				profile.Talents[TalentID.TailoringCreativity] = new Talent(TalentID.TailoringCreativity) { Points = 0 };
			}

            // Ensure Necromancy talents exist.
            if (!profile.Talents.ContainsKey(TalentID.NecromancySpells))
            {
                profile.Talents[TalentID.NecromancySpells] = new Talent(TalentID.NecromancySpells) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NecromancyNodes))
            {
                profile.Talents[TalentID.NecromancyNodes] = new Talent(TalentID.NecromancyNodes) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NecromancyEfficiency))
            {
                profile.Talents[TalentID.NecromancyEfficiency] = new Talent(TalentID.NecromancyEfficiency) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NecromancyYield))
            {
                profile.Talents[TalentID.NecromancyYield] = new Talent(TalentID.NecromancyYield) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NecromancyRange))
            {
                profile.Talents[TalentID.NecromancyRange] = new Talent(TalentID.NecromancyRange) { Points = 0 };
            }
            if (!profile.Talents.ContainsKey(TalentID.NecromancySummon))
            {
                profile.Talents[TalentID.NecromancySummon] = new Talent(TalentID.NecromancySummon) { Points = 0 };
            }

			// Ensure VeterinarySpells exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinarySpells))
			{
				profile.Talents[TalentID.VeterinarySpells] = new Talent(TalentID.VeterinarySpells) { Points = 0 };
			}

			// Ensure VeterinaryNodes exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryNodes))
			{
				profile.Talents[TalentID.VeterinaryNodes] = new Talent(TalentID.VeterinaryNodes) { Points = 0 };
			}

			// Ensure VeterinaryHealing exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryHealing))
			{
				profile.Talents[TalentID.VeterinaryHealing] = new Talent(TalentID.VeterinaryHealing) { Points = 0 };
			}

			// Ensure VeterinaryBonding exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryBonding))
			{
				profile.Talents[TalentID.VeterinaryBonding] = new Talent(TalentID.VeterinaryBonding) { Points = 0 };
			}

			// Ensure VeterinaryStamina exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryStamina))
			{
				profile.Talents[TalentID.VeterinaryStamina] = new Talent(TalentID.VeterinaryStamina) { Points = 0 };
			}

			// Ensure VeterinarySpeed exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinarySpeed))
			{
				profile.Talents[TalentID.VeterinarySpeed] = new Talent(TalentID.VeterinarySpeed) { Points = 0 };
			}

			// Ensure VeterinaryEmpathy exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryEmpathy))
			{
				profile.Talents[TalentID.VeterinaryEmpathy] = new Talent(TalentID.VeterinaryEmpathy) { Points = 0 };
			}

			// Ensure VeterinaryWisdom exists
			if (!profile.Talents.ContainsKey(TalentID.VeterinaryWisdom))
			{
				profile.Talents[TalentID.VeterinaryWisdom] = new Talent(TalentID.VeterinaryWisdom) { Points = 0 };
			}

			//Musicianship
			if (!profile.Talents.ContainsKey(TalentID.MusicianshipSpells))
			{
				profile.Talents[TalentID.MusicianshipSpells] = new Talent(TalentID.MusicianshipSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MusicianshipNodes))
			{
				profile.Talents[TalentID.MusicianshipNodes] = new Talent(TalentID.MusicianshipNodes) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MusicianshipPerformance))
			{
				profile.Talents[TalentID.MusicianshipPerformance] = new Talent(TalentID.MusicianshipPerformance) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MusicianshipTechnique))
			{
				profile.Talents[TalentID.MusicianshipTechnique] = new Talent(TalentID.MusicianshipTechnique) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.MusicianshipResonance))
			{
				profile.Talents[TalentID.MusicianshipResonance] = new Talent(TalentID.MusicianshipResonance) { Points = 0 };
			}

			if (!profile.Talents.ContainsKey(TalentID.LogCollectionRegular))
				profile.Talents[TalentID.LogCollectionRegular] = new Talent(TalentID.LogCollectionRegular);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionHeartwood))
				profile.Talents[TalentID.LogCollectionHeartwood] = new Talent(TalentID.LogCollectionHeartwood);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionBloodwood))
				profile.Talents[TalentID.LogCollectionBloodwood] = new Talent(TalentID.LogCollectionBloodwood);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionFrostwood))
				profile.Talents[TalentID.LogCollectionFrostwood] = new Talent(TalentID.LogCollectionFrostwood);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionOak))
				profile.Talents[TalentID.LogCollectionOak] = new Talent(TalentID.LogCollectionOak);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionAsh))
				profile.Talents[TalentID.LogCollectionAsh] = new Talent(TalentID.LogCollectionAsh);
			if (!profile.Talents.ContainsKey(TalentID.LogCollectionYew))
				profile.Talents[TalentID.LogCollectionYew] = new Talent(TalentID.LogCollectionYew);

			if (!profile.Talents.ContainsKey(TalentID.IronIngotCollection))
				profile.Talents[TalentID.IronIngotCollection] = new Talent(TalentID.IronIngotCollection);
			// Ensure the DullCopperIngotCollection talent exists.
			if (!profile.Talents.ContainsKey(TalentID.DullCopperIngotCollection))
			{
				profile.Talents[TalentID.DullCopperIngotCollection] = new Talent(TalentID.DullCopperIngotCollection) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.ShadowIronIngotCollection))
				profile.Talents[TalentID.ShadowIronIngotCollection] = new Talent(TalentID.ShadowIronIngotCollection);
			if (!profile.Talents.ContainsKey(TalentID.CopperIngotQuest))
				profile.Talents[TalentID.CopperIngotQuest] = new Talent(TalentID.CopperIngotQuest);			
			if (!profile.Talents.ContainsKey(TalentID.BronzeIngotCollection))
				profile.Talents[TalentID.BronzeIngotCollection] = new Talent(TalentID.BronzeIngotCollection);
			if (!profile.Talents.ContainsKey(TalentID.GoldIngotCollection))
				profile.Talents[TalentID.GoldIngotCollection] = new Talent(TalentID.GoldIngotCollection);
			if (!profile.Talents.ContainsKey(TalentID.AgapiteIngotCollection))
				profile.Talents[TalentID.AgapiteIngotCollection] = new Talent(TalentID.AgapiteIngotCollection);
			if (!profile.Talents.ContainsKey(TalentID.VeriteCollection))
				profile.Talents[TalentID.VeriteCollection] = new Talent(TalentID.VeriteCollection) { Points = 0 };			
			if (!profile.Talents.ContainsKey(TalentID.ValoriteCollection))
				profile.Talents[TalentID.ValoriteCollection] = new Talent(TalentID.ValoriteCollection) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.HideCollection))
				profile.Talents[TalentID.HideCollection] = new Talent(TalentID.HideCollection);
			if (!profile.Talents.ContainsKey(TalentID.SpinedHideCollection))
				profile.Talents[TalentID.SpinedHideCollection] = new Talent(TalentID.SpinedHideCollection);
			if (!profile.Talents.ContainsKey(TalentID.HornedHideCollection))
				profile.Talents[TalentID.HornedHideCollection] = new Talent(TalentID.HornedHideCollection);
			if (!profile.Talents.ContainsKey(TalentID.StarSapphireCollection))
				profile.Talents[TalentID.StarSapphireCollection] = new Talent(TalentID.StarSapphireCollection);
			if (!profile.Talents.ContainsKey(TalentID.EmeraldCollection))
				profile.Talents[TalentID.EmeraldCollection] = new Talent(TalentID.EmeraldCollection);
			if (!profile.Talents.ContainsKey(TalentID.SapphireCollection))
				profile.Talents[TalentID.SapphireCollection] = new Talent(TalentID.SapphireCollection);
			if (!profile.Talents.ContainsKey(TalentID.RubyCollection))
				profile.Talents[TalentID.RubyCollection] = new Talent(TalentID.RubyCollection);
			if (!profile.Talents.ContainsKey(TalentID.CitrineCollection))
				profile.Talents[TalentID.CitrineCollection] = new Talent(TalentID.CitrineCollection) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.AmethystCollection))
				profile.Talents[TalentID.AmethystCollection] = new Talent(TalentID.AmethystCollection);
			if (!profile.Talents.ContainsKey(TalentID.TourmalineCollection))
			{
				profile.Talents[TalentID.TourmalineCollection] = new Talent(TalentID.TourmalineCollection) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.AmberCollection))
				profile.Talents[TalentID.AmberCollection] = new Talent(TalentID.AmberCollection);
			if (!profile.Talents.ContainsKey(TalentID.DiamondCollection))
				profile.Talents[TalentID.DiamondCollection] = new Talent(TalentID.DiamondCollection);

			if (!profile.Talents.ContainsKey(TalentID.MinionDamageBonus))
			{
				profile.Talents[TalentID.MinionDamageBonus] = new Talent(TalentID.MinionDamageBonus) { Points = 1 };
			}
			
			if (!profile.Talents.ContainsKey(TalentID.SpiritSpeakSpells))
			{
				profile.Talents[TalentID.SpiritSpeakSpells] = new Talent(TalentID.SpiritSpeakSpells) { Points = 0 };
			}
			if (!profile.Talents.ContainsKey(TalentID.SpiritSpeakNodes))
			{
				profile.Talents[TalentID.SpiritSpeakNodes] = new Talent(TalentID.SpiritSpeakNodes) { Points = 0 };
			}			

			// In the Talents.AcquireTalents method:
			if (!profile.Talents.ContainsKey(TalentID.OrcSlayerQuest))
				profile.Talents[TalentID.OrcSlayerQuest] = new Talent(TalentID.OrcSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.AbysmalHorrorQuest))
				profile.Talents[TalentID.AbysmalHorrorQuest] = new Talent(TalentID.AbysmalHorrorQuest);
			if (!profile.Talents.ContainsKey(TalentID.AcidElementalQuest))
				profile.Talents[TalentID.AcidElementalQuest] = new Talent(TalentID.AcidElementalQuest);
			if (!profile.Talents.ContainsKey(TalentID.AirElementalQuest))
				profile.Talents[TalentID.AirElementalQuest] = new Talent(TalentID.AirElementalQuest);
			if (!profile.Talents.ContainsKey(TalentID.AlligatorSlayerQuest))
				profile.Talents[TalentID.AlligatorSlayerQuest] = new Talent(TalentID.AlligatorSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.BakeKitsuneSlayerQuest))
				profile.Talents[TalentID.BakeKitsuneSlayerQuest] = new Talent(TalentID.BakeKitsuneSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.BalronSlayerQuest))
				profile.Talents[TalentID.BalronSlayerQuest] = new Talent(TalentID.BalronSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.BloodElementalQuest))
				profile.Talents[TalentID.BloodElementalQuest] = new Talent(TalentID.BloodElementalQuest);
			if (!profile.Talents.ContainsKey(TalentID.BrownBearSlayerQuest))
				profile.Talents[TalentID.BrownBearSlayerQuest] = new Talent(TalentID.BrownBearSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.CorpserSlayerQuest))
				profile.Talents[TalentID.CorpserSlayerQuest] = new Talent(TalentID.CorpserSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.CuSidheHunterQuest))
				profile.Talents[TalentID.CuSidheHunterQuest] = new Talent(TalentID.CuSidheHunterQuest);
			if (!profile.Talents.ContainsKey(TalentID.CyclopsSlayerQuest))
				profile.Talents[TalentID.CyclopsSlayerQuest] = new Talent(TalentID.CyclopsSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.KillDaemonQuest))
				profile.Talents[TalentID.KillDaemonQuest] = new Talent(TalentID.KillDaemonQuest);
			if (!profile.Talents.ContainsKey(TalentID.DesertOstardQuest))
				profile.Talents[TalentID.DesertOstardQuest] = new Talent(TalentID.DesertOstardQuest);
			if (!profile.Talents.ContainsKey(TalentID.DireWolfSlayerQuest))
				profile.Talents[TalentID.DireWolfSlayerQuest] = new Talent(TalentID.DireWolfSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.DolphinSlayerQuest))
				profile.Talents[TalentID.DolphinSlayerQuest] = new Talent(TalentID.DolphinSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.DragonSlayerQuest))
				profile.Talents[TalentID.DragonSlayerQuest] = new Talent(TalentID.DragonSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.DrakeSlayerQuest))
				profile.Talents[TalentID.DrakeSlayerQuest] = new Talent(TalentID.DrakeSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.DreadSpiderQuest))
				profile.Talents[TalentID.DreadSpiderQuest] = new Talent(TalentID.DreadSpiderQuest);
			if (!profile.Talents.ContainsKey(TalentID.EarthElementalSlayerQuest))
				profile.Talents[TalentID.EarthElementalSlayerQuest] = new Talent(TalentID.EarthElementalSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.EttinSlayerQuest))
				profile.Talents[TalentID.EttinSlayerQuest] = new Talent(TalentID.EttinSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.FireElementalSlayerQuest))
				profile.Talents[TalentID.FireElementalSlayerQuest] = new Talent(TalentID.FireElementalSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.ForestOstardHunter))
				profile.Talents[TalentID.ForestOstardHunter] = new Talent(TalentID.ForestOstardHunter);
			if (!profile.Talents.ContainsKey(TalentID.FrenziedOstardSlayerQuest))
				profile.Talents[TalentID.FrenziedOstardSlayerQuest] = new Talent(TalentID.FrenziedOstardSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GargoyleSlayerQuest))
				profile.Talents[TalentID.GargoyleSlayerQuest] = new Talent(TalentID.GargoyleSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GazerSlayerQuest))
				profile.Talents[TalentID.GazerSlayerQuest] = new Talent(TalentID.GazerSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GhoulSlayerQuest))
				profile.Talents[TalentID.GhoulSlayerQuest] = new Talent(TalentID.GhoulSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GiantSerpentSlayerQuest))
				profile.Talents[TalentID.GiantSerpentSlayerQuest] = new Talent(TalentID.GiantSerpentSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GiantSpiderSlayerQuest))
				profile.Talents[TalentID.GiantSpiderSlayerQuest] = new Talent(TalentID.GiantSpiderSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.ToadSlayerQuest))
				profile.Talents[TalentID.ToadSlayerQuest] = new Talent(TalentID.ToadSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GoatSlayerQuest))
				profile.Talents[TalentID.GoatSlayerQuest] = new Talent(TalentID.GoatSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GorillaSlayerQuest))
				profile.Talents[TalentID.GorillaSlayerQuest] = new Talent(TalentID.GorillaSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GreatHartSlayerQuest))
				profile.Talents[TalentID.GreatHartSlayerQuest] = new Talent(TalentID.GreatHartSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.GrizzlyBearSlayerQuest))
				profile.Talents[TalentID.GrizzlyBearSlayerQuest] = new Talent(TalentID.GrizzlyBearSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.HarpySlayerQuest))
				profile.Talents[TalentID.HarpySlayerQuest] = new Talent(TalentID.HarpySlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.HeadlessSlayerQuest))
				profile.Talents[TalentID.HeadlessSlayerQuest] = new Talent(TalentID.HeadlessSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.HindSlayerQuest))
				profile.Talents[TalentID.HindSlayerQuest] = new Talent(TalentID.HindSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.HiryuSlayerQuest))
				profile.Talents[TalentID.HiryuSlayerQuest] = new Talent(TalentID.HiryuSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.HorseSlayerQuest))
				profile.Talents[TalentID.HorseSlayerQuest] = new Talent(TalentID.HorseSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.JukaLordSlayerQuest))
				profile.Talents[TalentID.JukaLordSlayerQuest] = new Talent(TalentID.JukaLordSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.JukaMageSlayerQuest))
				profile.Talents[TalentID.JukaMageSlayerQuest] = new Talent(TalentID.JukaMageSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.JukaSlayerQuest))
				profile.Talents[TalentID.JukaSlayerQuest] = new Talent(TalentID.JukaSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.KirinSlayerQuest))
				profile.Talents[TalentID.KirinSlayerQuest] = new Talent(TalentID.KirinSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.LesserHiryuSlayerQuest))
				profile.Talents[TalentID.LesserHiryuSlayerQuest] = new Talent(TalentID.LesserHiryuSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.LichSlayerQuest))
				profile.Talents[TalentID.LichSlayerQuest] = new Talent(TalentID.LichSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.LichLordHunterQuest))
				profile.Talents[TalentID.LichLordHunterQuest] = new Talent(TalentID.LichLordHunterQuest);
			if (!profile.Talents.ContainsKey(TalentID.LionSlayerQuest))
				profile.Talents[TalentID.LionSlayerQuest] = new Talent(TalentID.LionSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.LizardmanSlayerQuest))
				profile.Talents[TalentID.LizardmanSlayerQuest] = new Talent(TalentID.LizardmanSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.LlamaSlayerQuest))
				profile.Talents[TalentID.LlamaSlayerQuest] = new Talent(TalentID.LlamaSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.MinotaurSlayerQuest))
				profile.Talents[TalentID.MinotaurSlayerQuest] = new Talent(TalentID.MinotaurSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.MongbatSlayerQuest))
				profile.Talents[TalentID.MongbatSlayerQuest] = new Talent(TalentID.MongbatSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.MummySlayerQuest))
				profile.Talents[TalentID.MummySlayerQuest] = new Talent(TalentID.MummySlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.NightmareSlayerQuest))
				profile.Talents[TalentID.NightmareSlayerQuest] = new Talent(TalentID.NightmareSlayerQuest);
			if (!profile.Talents.ContainsKey(TalentID.OgreSlayerQuest))
				profile.Talents[TalentID.OgreSlayerQuest] = new Talent(TalentID.OgreSlayerQuest) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.OgreLordSlayerQuest))
				profile.Talents[TalentID.OgreLordSlayerQuest] = new Talent(TalentID.OgreLordSlayerQuest) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.OphidianWarriorSlayer))
				profile.Talents[TalentID.OphidianWarriorSlayer] = new Talent(TalentID.OphidianWarriorSlayer) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.OrcCaptainSlayerQuest))
				profile.Talents[TalentID.OrcCaptainSlayerQuest] = new Talent(TalentID.OrcCaptainSlayerQuest) { Points = 0 };
			if (!profile.Talents.ContainsKey(TalentID.PantherSlayerQuest))
				profile.Talents[TalentID.PantherSlayerQuest] = new Talent(TalentID.PantherSlayerQuest) { Points = 0 };



			if (!profile.Talents.ContainsKey(TalentID.AtlasNodes))
				profile.Talents[TalentID.AtlasNodes] = new Talent(TalentID.AtlasNodes) { Points = 0 };


























			

			return profile;
		}

        // NEW: Helper to calculate cumulative XP threshold for a given level.
        // In this example, leveling from level 1 to 2 requires 1000 XP,
        // from level 2 to 3 requires an additional 1500 (total 2500), etc.
		public static int GetXPThresholdForLevel(int level)
		{
			const int baseXP = 50; // XP needed for level 2
			if (level <= 1)
				return 0;

			double totalXP = 0;

			if (level <= 85)
			{
				for (int i = 2; i <= level; i++)
				{
					double growthFactor = 1.05 + (i - 2) * 0.002;
					totalXP += baseXP + (i * 5) + Math.Pow(i, 1.2);
				}
			}
			else
			{
				// Calculate total XP up to level 85
				for (int i = 2; i <= 85; i++)
				{
					double growthFactor = 1.05 + (i - 2) * 0.002;
					totalXP += baseXP + (i * 5) + Math.Pow(i, 1.2);
				}

				double xpAt85 = totalXP;
				double remainingXP = 10_000_000 - xpAt85;

				// Levels 86 to 100 = 15 levels
				int extraLevels = 100 - 85;
				double a = remainingXP / (Math.Pow(extraLevels, 2)); // Quadratic growth
				// You can tweak this to a different curve, like exponential

				for (int i = 86; i <= level; i++)
				{
					int index = i - 85;
					totalXP += a * Math.Pow(index, 2); // Or Math.Pow(2, index) * b for true exponential
				}
			}

			return (int)totalXP;
		}




    }

    // (TalentDeed remains unchanged below...)
    public class TalentDeed : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public TalentID Talent { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Points { get; set; }

        [Constructable]
        public TalentDeed() : this(TalentID.AncientKnowledge)
        {
        }

        [Constructable]
        public TalentDeed(TalentID talent) : this(talent, 1)
        {
        }

        [Constructable]
        public TalentDeed(TalentID talent, int points) : base(0x14F0)
        {
            Talent = talent;
            Points = points;
        }

        public TalentDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (!profile.Talents.TryGetValue(Talent, out var talent))
                {
                    profile.Talents[Talent] = talent = new Talent(Talent);
                }
                talent.Points += Points;

                int totalTalentPoints = profile.Talents.Values.Sum(t => t.Points);
                player.SendMessage($"You gained {Points:N0} in {Talent}!");
                player.SendMessage($"Your total talent points: {totalTalentPoints:N0}");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write((int)Talent);
            writer.Write(Points);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
            Talent = (TalentID)reader.ReadInt();
            Points = reader.ReadInt();
        }
    }

    public class CheckTalentsCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CheckTalents", AccessLevel.Player, new CommandEventHandler(CheckTalents_OnCommand));
            CommandSystem.Register("SetTalent", AccessLevel.GameMaster, new CommandEventHandler(SetTalent_OnCommand));
        }

        private static void CheckTalents_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target a player to check their talents.");
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, CheckTalents_OnTarget);
        }

        private static void CheckTalents_OnTarget(Mobile from, object targeted)
        {
            if (targeted is PlayerMobile player)
            {
                var profile = player.AcquireTalents();

                if (profile == null || profile.Talents.Count == 0)
                {
                    from.SendMessage("This player has no recorded talents.");
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"{player.Name}'s Talents:");

                foreach (var talent in profile.Talents)
                {
                    sb.AppendLine($"{talent.Key}: {talent.Value.Points:N0} Points");
                }

                from.SendMessage(sb.ToString());
            }
            else
            {
                from.SendMessage("You must target a player.");
            }
        }

        private static void SetTalent_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target a player to set their talent value.");
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, SetTalent_OnTarget);
        }

        private static void SetTalent_OnTarget(Mobile from, object targeted)
        {
            if (!(targeted is PlayerMobile player))
            {
                from.SendMessage("You must target a player.");
                return;
            }

            from.SendMessage("Enter the talent ID and value in the format: <TalentID> <Value>");
            from.Prompt = new SetTalentPrompt(player);
        }
    }

    public class SetTalentPrompt : Prompt
    {
        private readonly PlayerMobile _player;

        public SetTalentPrompt(PlayerMobile player)
        {
            _player = player;
        }

        public override void OnResponse(Mobile from, string text)
        {
            var args = text.Split(' ');
            if (args.Length < 2 || !Enum.TryParse(args[0], out TalentID talentID) || !int.TryParse(args[1], out int value))
            {
                from.SendMessage("Invalid input. Use format: <TalentID> <Value>");
                return;
            }

            var profile = _player.AcquireTalents();
            if (!profile.Talents.ContainsKey(talentID))
            {
                profile.Talents[talentID] = new Talent(talentID);
            }

            profile.Talents[talentID].Points = value;

            from.SendMessage($"You set {_player.Name}'s {talentID} to {value} points.");
            _player.SendMessage($"Your {talentID} has been set to {value} points.");
        }
    }
}
