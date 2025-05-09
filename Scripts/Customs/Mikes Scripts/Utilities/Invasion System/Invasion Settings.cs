﻿namespace Server.Customs.Invasion_System
{
	public enum TownMonsterType
	{
		Abyss,
		Arachnid,
		DragonKind,
		Elementals,
		Humanoid,
		OrcsandRatmen,
		OreElementals,
		Ophidian,
		Snakes,
		Undead,
        CombatOrientedNPCs,
        MartialArtsSpecialists,
        SpecializedCombatants,
        MagicalSupernaturalNPCs,
        ClericsHealersNPCs,
        ShamansMysticsNPCs,
        NatureThemedNPCs,
        StealthEspionageNPCs,
        MysticalMythologicalNPCs,
        CraftsmenArtisansNPCs,
        EntertainersCulturalNPCs,
        SciFiFuturisticNPCs,
        HistoricalThematicNPCs,
        MiscellaneousNPCs,
        DemonicEntities,
        ExtendedArachnids,
        Mammals,
        Birds,
        ReptilesAmphibians,
        Goblins,
        HumanoidsExtended,
        MagicalBeasts,
        SpiritsWisps,
        AquaticCreatures,
        InsectsCrawlers,
        GiantsTrolls,
        MiscellaneousMonsters,		
        InvasionAirElementals,
        InvasionAlligators,
        InvasionBears,
        InvasionBulls,
        InvasionCats,
        InvasionChickens,
        InvasionCorpser,
        InvasionCrabs,
        InvasionDaemons,
        InvasionDragons,
        InvasionEarthElementals,
        InvasionEttin,
        InvasionFerrets,
        InvasionFey,
        InvasionFireElementals,
        InvasionGargoyles,
        InvasionGazers,
        InvasionGiantSerpant,
        InvasionGiantSpider,
        InvasionGoats,
        InvasionGolems,
        InvasionGorilla,
        InvasionGreatHart,
        InvasionGrizzlyBear,
        InvasionHarpy,
        InvasionHorses,
        InvasionAncientLiches,
        InvasionLichs,
        InvasionLlama,
        InvasionMummy,
        InvasionOgres,
        InvasionPanther,
        InvasionPigs,
        InvasionRabbit,
        InvasionRats,
        InvasionRobot,
        InvasionRobotOverlord,
        InvasionSatyr,
        InvasionSheep,
        InvasionSkeletonKnight,
        InvasionSlimes,
        InvasionSquirrel,
        InvasionToads,
        InvasionWaterElementals,
        InvasionWolf
	}

	public enum TownChampionType
	{
		Barracoon,
		Harrower,
		LordOaks,
		Mephitis,
		Neira,
		Rikktor,
		Semidar,
		Serado,
		ChaosLord,
		ChaosWyrm,
		ChronosTheTimeLord,
		DraconicusTheDestroyer,
		Dreadlord,
		FrostWraith,
		JesterOfChaos,
		MinaxTheTimeSorceress,
		PrimevalDragon,
		PrismaticDragon,
		SaiyanWarrior,
		SearingExarch,
		SlimePrincessSuiblex,
		SuperDragon,
		UltimateAlchemyMaster,
		UltimateMasterAnatomy,
		UltimateMasterAnimalLore,
		UltimateMasterAnimalTamer,
		UltimateMasterArcher,
		UltimateMasterArmsLore,
		UltimateMasterBeggar,
		UltimateMasterBlacksmith,
		UltimateMasterBowcraft,
		UltimateMasterBushido,
		UltimateMasterCamper,
		UltimateMasterCarpenter,
		UltimateMasterCartographer,
		UltimateMasterChef,
		UltimateMasterChivalry,
		UltimateMasterDetectingHidden,
		UltimateMasterDiscordance,
		UltimateMasterFencer,
		UltimateMasterFishing,
		UltimateMasterFocus,
		UltimateMasterForensicEvaluator,
		UltimateMasterHealer,
		UltimateMasterHerder,
		UltimateMasterHider,
		UltimateMasterImbuing,
		UltimateMasterInscription,
		UltimateMasterIntelligence,
		UltimateMasterItemIdentification,
		UltimateMasterLockpicker,
		UltimateMasterLumberjack,
		UltimateMasterMaceFighting,
		UltimateMasterMage,
		UltimateMasterMagicResistance,
		UltimateMasterMeditation,
		UltimateMasterMiner,
		UltimateMasterMusician,
		UltimateMasterMysticism,
		UltimateMasterNecromancer,
		UltimateMasterNinjitsu,
		UltimateMasterParrying,
		UltimateMasterPeacemaker,
		UltimateMasterPoisoner,
		UltimateMasterProvocation,
		UltimateMasterRemoveTrap,
		UltimateMasterSnooping,
		UltimateMasterSpellweaver,
		UltimateMasterSpiritSpeak,
		UltimateMasterStealth,
		UltimateMasterSwordsman,
		UltimateMasterTactician,
		UltimateMasterTailor,
		UltimateMasterTasteIdentification,
		UltimateMasterThief,
		UltimateMasterThrowing,
		UltimateMasterTinkering,
		UltimateMasterTracker,
		UltimateMasterVeterinary,
		UltimateMasterWrestling		
	}

    public enum InvasionTowns
    {
        BuccaneersDen,
        Cove,
        Delucia,
        Jhelom,
        Minoc,
        Moonglow,
        Nujel,
        Ocllo,
        Papua,
        SkaraBrae,
        Vesper,
        Yew,
		// Newly Added Regions
		HedgeMaze,
		DungeonDestard,
		BritainCemetery,
		DungeonShame_Level1,
		DungeonShame_Level2,
		DungeonShame_Level3,
		DungeonShame_Level5,
		DungeonShame_MageTowers1,
		DungeonShame_MageTowers2,
		IceDungeon,
		IceDemonLair,
		RatmanFort,
		DungeonHythloth_Level1,
		DungeonHythloth_Level2,
		DungeonHythloth_Level3,
		DungeonHythloth_Level4,
		DungeonDeceit_Level1,
		DungeonDeceit_Level2,
		DungeonDeceit_Level3,
		DungeonDeceit_Level4,
		DungeonDespise_Level1,
		DungeonDespise_Level2,
		DungeonDespise_Level3,
		DungeonWrong_Level1,
		DungeonWrong_Level2,
		DungeonWrong_Level3,
		DungeonKhaldun,
		TrinsicPassage,
		FireDungeon_Level1,
		FireDungeon_Level2,
		Sewers,
		TerathanKeep,
		SolenHive,
		DungeonCovetous_Level1,
		DungeonCovetous_Level2,
		DungeonCovetous_Level3,
		DungeonCovetous_JailCells,
		DungeonCovetous_LakeCave,
		DungeonDoom,
		Bedlam,
		SorcerersDungeon_Level1,
		SorcerersDungeon_Level2,
		SorcerersDungeon_Level3,
		SorcerersDungeon_JailCells,
		AncientCave,
		KirinPassage,
		DungeonAnkh,
		SerpentinePassage,
		WispDungeon_Level3,
		WispDungeon_Level5,
		WispDungeon_Level7,
		WispDungeon_Level8,
		RatmanMines_Level1,
		RatmanMines_Level2,
		SpiderCave,
		SpectreDungeon,
		DungeonBlood,
		RockDungeon,
		DungeonExodus		
    }
}