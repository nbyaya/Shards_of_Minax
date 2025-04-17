using System;
using Server.Mobiles;

namespace Server.Customs.Invasion_System
{
    public class MonsterTownSpawnEntry
    {
        #region Existing SpawnEntries

        public static MonsterTownSpawnEntry[] Undead = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Zombie),             165),
            new MonsterTownSpawnEntry(typeof(Skeleton),           65),
            new MonsterTownSpawnEntry(typeof(SkeletalMage),       40),
            new MonsterTownSpawnEntry(typeof(BoneKnight),         45),
            new MonsterTownSpawnEntry(typeof(SkeletalKnight),     45),
            new MonsterTownSpawnEntry(typeof(Lich),               45),
            new MonsterTownSpawnEntry(typeof(Ghoul),              40),
            new MonsterTownSpawnEntry(typeof(BoneMagi),           40),
            new MonsterTownSpawnEntry(typeof(Wraith),             35),
            new MonsterTownSpawnEntry(typeof(RottingCorpse),      35),
            new MonsterTownSpawnEntry(typeof(LichLord),           55),
            new MonsterTownSpawnEntry(typeof(Spectre),            30),
            new MonsterTownSpawnEntry(typeof(Shade),              30),
            new MonsterTownSpawnEntry(typeof(AncientLich),        50)
        };

        public static MonsterTownSpawnEntry[] Humanoid = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Brigand),            60),
            new MonsterTownSpawnEntry(typeof(Executioner),        30),
            new MonsterTownSpawnEntry(typeof(EvilMage),           70),
            new MonsterTownSpawnEntry(typeof(EvilMageLord),       40),
            new MonsterTownSpawnEntry(typeof(Ettin),              45),
            new MonsterTownSpawnEntry(typeof(Ogre),               45),
            new MonsterTownSpawnEntry(typeof(OgreLord),           40),
            new MonsterTownSpawnEntry(typeof(ArcticOgreLord),     40),
            new MonsterTownSpawnEntry(typeof(Troll),              55),
            new MonsterTownSpawnEntry(typeof(Cyclops),            55),
            new MonsterTownSpawnEntry(typeof(Titan),              40)
        };

        public static MonsterTownSpawnEntry[] OrcsandRatmen = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Orc),                80),
            new MonsterTownSpawnEntry(typeof(OrcishMage),         45),
            new MonsterTownSpawnEntry(typeof(OrcishLord),         55),
            new MonsterTownSpawnEntry(typeof(OrcCaptain),         50),
            new MonsterTownSpawnEntry(typeof(OrcBomber),          55),
            new MonsterTownSpawnEntry(typeof(OrcBrute),           40),
            new MonsterTownSpawnEntry(typeof(Ratman),             80),
            new MonsterTownSpawnEntry(typeof(RatmanArcher),       50),
            new MonsterTownSpawnEntry(typeof(RatmanMage),         45)
        };

        public static MonsterTownSpawnEntry[] Elementals = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(EarthElemental),     95),
            new MonsterTownSpawnEntry(typeof(AirElemental),       70),
            new MonsterTownSpawnEntry(typeof(FireElemental),      60),
            new MonsterTownSpawnEntry(typeof(WaterElemental),     60),
            new MonsterTownSpawnEntry(typeof(SnowElemental),      40),
            new MonsterTownSpawnEntry(typeof(IceElemental),       40),
            new MonsterTownSpawnEntry(typeof(Efreet),           45),
            new MonsterTownSpawnEntry(typeof(PoisonElemental),    35),
            new MonsterTownSpawnEntry(typeof(BloodElemental),     35)
        };

        public static MonsterTownSpawnEntry[] OreElementals = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(DullCopperElemental),90),
            new MonsterTownSpawnEntry(typeof(CopperElemental),    80),
            new MonsterTownSpawnEntry(typeof(BronzeElemental),    50),
            new MonsterTownSpawnEntry(typeof(ShadowIronElemental),60),
            new MonsterTownSpawnEntry(typeof(GoldenElemental),    55),
            new MonsterTownSpawnEntry(typeof(AgapiteElemental),   45),
            new MonsterTownSpawnEntry(typeof(VeriteElemental),     40),
            new MonsterTownSpawnEntry(typeof(ValoriteElemental),   40)
        };

        public static MonsterTownSpawnEntry[] Ophidian = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(OphidianWarrior),    100),
            new MonsterTownSpawnEntry(typeof(OphidianMage),       70),
            new MonsterTownSpawnEntry(typeof(OphidianArchmage),   30),
            new MonsterTownSpawnEntry(typeof(OphidianKnight),     35),
            new MonsterTownSpawnEntry(typeof(OphidianMatriarch),  35)
        };

        public static MonsterTownSpawnEntry[] Arachnid = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Scorpion),           75),
            new MonsterTownSpawnEntry(typeof(GiantSpider),        75),
            new MonsterTownSpawnEntry(typeof(TerathanDrone),      45),
            new MonsterTownSpawnEntry(typeof(TerathanWarrior),    30),
            new MonsterTownSpawnEntry(typeof(TerathanMatriarch),  45),
            new MonsterTownSpawnEntry(typeof(TerathanAvenger),    45),
            new MonsterTownSpawnEntry(typeof(DreadSpider),        40),
            new MonsterTownSpawnEntry(typeof(FrostSpider),        35)
        };

        public static MonsterTownSpawnEntry[] Snakes = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Snake),              95),
            new MonsterTownSpawnEntry(typeof(GiantSerpent),       95),
            new MonsterTownSpawnEntry(typeof(LavaSnake),          50),
            new MonsterTownSpawnEntry(typeof(LavaSerpent),        55),
            new MonsterTownSpawnEntry(typeof(IceSnake),           50),
            new MonsterTownSpawnEntry(typeof(IceSerpent),         55),
            new MonsterTownSpawnEntry(typeof(SilverSerpent),      40)
        };

        public static MonsterTownSpawnEntry[] Abyss = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Gargoyle),           100),
            new MonsterTownSpawnEntry(typeof(StoneGargoyle),      60),
            new MonsterTownSpawnEntry(typeof(FireGargoyle),       60),
            new MonsterTownSpawnEntry(typeof(Daemon),             60),
            new MonsterTownSpawnEntry(typeof(IceFiend),           50),
            new MonsterTownSpawnEntry(typeof(Balron),             30)
        };

        public static MonsterTownSpawnEntry[] DragonKind = new MonsterTownSpawnEntry[]
        {
            // Monster                          // Amount
            new MonsterTownSpawnEntry(typeof(Wyvern),             100),
            new MonsterTownSpawnEntry(typeof(Drake),              60),
            new MonsterTownSpawnEntry(typeof(Dragon),             60),
            new MonsterTownSpawnEntry(typeof(WhiteWyrm),          60),
            new MonsterTownSpawnEntry(typeof(ShadowWyrm),         10),
            new MonsterTownSpawnEntry(typeof(AncientWyrm),        30)
        };

        #endregion

        #region New Invasion Types Covering All Game Categories

        // 1. Combat-Oriented NPCs
        public static MonsterTownSpawnEntry[] CombatOrientedNPCs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Assassin),            50),
            new MonsterTownSpawnEntry(typeof(Banneret),            50),
            new MonsterTownSpawnEntry(typeof(CombatMedic),         40),
            new MonsterTownSpawnEntry(typeof(CombatNurse),         40),
            new MonsterTownSpawnEntry(typeof(FieldCommander),      35),
            new MonsterTownSpawnEntry(typeof(HolyKnight),          45),
            new MonsterTownSpawnEntry(typeof(KnightOfJustice),     45),
            new MonsterTownSpawnEntry(typeof(KnightOfMercy),       45),
            new MonsterTownSpawnEntry(typeof(KnightOfValor),       45),
            new MonsterTownSpawnEntry(typeof(RamRider),            30),
            new MonsterTownSpawnEntry(typeof(RapierDuelist),       30),
            new MonsterTownSpawnEntry(typeof(SabreFighter),        40),
            new MonsterTownSpawnEntry(typeof(Samurai),             50),
            new MonsterTownSpawnEntry(typeof(ShieldBearer),        40),
            new MonsterTownSpawnEntry(typeof(ShieldMaiden),        40),
            new MonsterTownSpawnEntry(typeof(SwordDefender),       45)
        };

        // 2. Martial Arts and Weapon Specialists
        public static MonsterTownSpawnEntry[] MartialArtsSpecialists = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BoomerangThrower),    40),
            new MonsterTownSpawnEntry(typeof(CrossbowMarksman),    40),
            new MonsterTownSpawnEntry(typeof(DualWielder),         35),
            new MonsterTownSpawnEntry(typeof(EpeeSpecialist),       30),
            new MonsterTownSpawnEntry(typeof(FencingMaster),       35),
            new MonsterTownSpawnEntry(typeof(JavelinAthlete),      30),
            new MonsterTownSpawnEntry(typeof(KatanaDuelist),       40),
            new MonsterTownSpawnEntry(typeof(LongbowSniper),       30),
            new MonsterTownSpawnEntry(typeof(SumoWrestler),        25),
            new MonsterTownSpawnEntry(typeof(TaekwondoMaster),     35)
        };

        // 3. Specialized Combatants
        public static MonsterTownSpawnEntry[] SpecializedCombatants = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GreenNinja),          45),
            new MonsterTownSpawnEntry(typeof(Kunoichi),            45),
            new MonsterTownSpawnEntry(typeof(ScoutNinja),          40),
            new MonsterTownSpawnEntry(typeof(SneakyNinja),         40),
            new MonsterTownSpawnEntry(typeof(SteampunkSamurai),    35),
            new MonsterTownSpawnEntry(typeof(Stormtrooper2),       30)
        };

        // 4. Magical and Supernatural NPCs
        public static MonsterTownSpawnEntry[] MagicalSupernaturalNPCs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(ArcaneScribe),        40),
            new MonsterTownSpawnEntry(typeof(ElementalWizard),     45),
            new MonsterTownSpawnEntry(typeof(Enchanter),           40),
            new MonsterTownSpawnEntry(typeof(EvilAlchemist),       35),
            new MonsterTownSpawnEntry(typeof(FireMage),            45),
            new MonsterTownSpawnEntry(typeof(IceSorcerer),         45),
            new MonsterTownSpawnEntry(typeof(LightningBearer),     40),
            new MonsterTownSpawnEntry(typeof(Magician),            40),
            new MonsterTownSpawnEntry(typeof(RuneCaster),          35),
            new MonsterTownSpawnEntry(typeof(ScrollMage),          30),
            new MonsterTownSpawnEntry(typeof(SlimeMage),           30)
        };

        // 5. Clerics and Healers
        public static MonsterTownSpawnEntry[] ClericsHealersNPCs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BattlefieldHealer),   35),
            new MonsterTownSpawnEntry(typeof(QiGongHealer),        30),
            new MonsterTownSpawnEntry(typeof(SpiritMedium),        30),
            new MonsterTownSpawnEntry(typeof(WardCaster),          30)
        };

        // 6. Shamans and Mystics
        public static MonsterTownSpawnEntry[] ShamansMysticsNPCs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(AvatarOfElements),    30),
            new MonsterTownSpawnEntry(typeof(PsychedelicShaman),   25),
            new MonsterTownSpawnEntry(typeof(StormConjurer),         30),
            new MonsterTownSpawnEntry(typeof(StormConjurer),       25),
            new MonsterTownSpawnEntry(typeof(ZenMonk),             30)
        };

        // 7. Nature-Themed NPCs
        public static MonsterTownSpawnEntry[] NatureThemedNPCs = new MonsterTownSpawnEntry[]
        {
            // Tamer and Animal Handlers
            new MonsterTownSpawnEntry(typeof(AquaticTamer),        30),
            new MonsterTownSpawnEntry(typeof(Beastmaster),         35),
            new MonsterTownSpawnEntry(typeof(BigCatTamer),         30),
            new MonsterTownSpawnEntry(typeof(BirdTrainer),         30),
            new MonsterTownSpawnEntry(typeof(SerpentHandler),      30),
            new MonsterTownSpawnEntry(typeof(SheepdogHandler),     25),
            // Druids and Rangers
            new MonsterTownSpawnEntry(typeof(ArcticNaturalist),    30),
            new MonsterTownSpawnEntry(typeof(DesertNaturalist),    30),
            new MonsterTownSpawnEntry(typeof(ForestRanger),        35),
            new MonsterTownSpawnEntry(typeof(ForestScout),         30),
            new MonsterTownSpawnEntry(typeof(HostileDruid),        25),
            new MonsterTownSpawnEntry(typeof(UrbanTracker),        25),
            // Plant and Nature Specialists
            new MonsterTownSpawnEntry(typeof(AppleElemental),      20),
            new MonsterTownSpawnEntry(typeof(Forager),             25),
            new MonsterTownSpawnEntry(typeof(PoisonAppleTree),     15)
        };

        // 8. Stealth and Espionage NPCs
        public static MonsterTownSpawnEntry[] StealthEspionageNPCs = new MonsterTownSpawnEntry[]
        {
            // Spies and Saboteurs
            new MonsterTownSpawnEntry(typeof(ConArtist),           35),
            new MonsterTownSpawnEntry(typeof(DisguiseMaster),      30),
            new MonsterTownSpawnEntry(typeof(Infiltrator),         30),
            new MonsterTownSpawnEntry(typeof(Saboteur),            25),
            new MonsterTownSpawnEntry(typeof(SilentMovieMonk),     20),
            new MonsterTownSpawnEntry(typeof(Spy),                 30),
            // Thieves and Rogues
            new MonsterTownSpawnEntry(typeof(DecoyDeployer),       25),
            new MonsterTownSpawnEntry(typeof(MasterPickpocket),    25),
            new MonsterTownSpawnEntry(typeof(Pickpocket),          30),
            new MonsterTownSpawnEntry(typeof(SafeCracker),         20),
            new MonsterTownSpawnEntry(typeof(TrapSetter),          25)
        };

        // 9. Mystical and Mythological NPCs
        public static MonsterTownSpawnEntry[] MysticalMythologicalNPCs = new MonsterTownSpawnEntry[]
        {
            // Fantasy Characters
            new MonsterTownSpawnEntry(typeof(DarkElf),             35),
            new MonsterTownSpawnEntry(typeof(FairyQueen),          30),
            new MonsterTownSpawnEntry(typeof(GreenHag),            30),
            new MonsterTownSpawnEntry(typeof(NymphSinger),         25),
            new MonsterTownSpawnEntry(typeof(SatyrPiper),          25),
            new MonsterTownSpawnEntry(typeof(TwistedCultist),      30),
            // Monsters and Beasts
            new MonsterTownSpawnEntry(typeof(BeetleJuiceSummoner), 25),
            new MonsterTownSpawnEntry(typeof(CabaretKrakenGirl),   20),
            new MonsterTownSpawnEntry(typeof(InfernoDragon),       20),
            new MonsterTownSpawnEntry(typeof(MegaDragon),          15),
            new MonsterTownSpawnEntry(typeof(PatchworkMonster),    25),
            new MonsterTownSpawnEntry(typeof(SwampThing),          25)
        };

        // 10. Craftsmen and Artisans
        public static MonsterTownSpawnEntry[] CraftsmenArtisansNPCs = new MonsterTownSpawnEntry[]
        {
            // Builders and Engineers
            new MonsterTownSpawnEntry(typeof(AnvilHurler),         25),
            new MonsterTownSpawnEntry(typeof(Carpenter),           30),
            new MonsterTownSpawnEntry(typeof(ClockworkEngineer),   25),
            new MonsterTownSpawnEntry(typeof(IronSmith),           30),
            new MonsterTownSpawnEntry(typeof(TrapEngineer),        20),
            // Crafters and Artists
            new MonsterTownSpawnEntry(typeof(ArrowFletcher),       25),
            new MonsterTownSpawnEntry(typeof(BattleDressmaker),    20),
            new MonsterTownSpawnEntry(typeof(BattleWeaver),        20),
            new MonsterTownSpawnEntry(typeof(GemCutter),           25),
            new MonsterTownSpawnEntry(typeof(ToxicologistChef),    20),
            new MonsterTownSpawnEntry(typeof(WoolWeaver),          20)
        };

        // 11. Entertainers and Cultural Figures
        public static MonsterTownSpawnEntry[] EntertainersCulturalNPCs = new MonsterTownSpawnEntry[]
        {
            // Performers
            new MonsterTownSpawnEntry(typeof(BluesSingingGorgon),  20),
            new MonsterTownSpawnEntry(typeof(DrumBoy),             25),
            new MonsterTownSpawnEntry(typeof(Flutist),             20),
            new MonsterTownSpawnEntry(typeof(GlamRockMage),        20),
            new MonsterTownSpawnEntry(typeof(RaveRogue),           25),
            new MonsterTownSpawnEntry(typeof(SkaSkald),            20),
            // Artists and Storytellers
            new MonsterTownSpawnEntry(typeof(SlyStoryteller),      25),
            new MonsterTownSpawnEntry(typeof(VaudevilleValkyrie),  20)
        };

        // 12. Sci-Fi and Futuristic NPCs
        public static MonsterTownSpawnEntry[] SciFiFuturisticNPCs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(CyberpunkSorcerer),   25),
            new MonsterTownSpawnEntry(typeof(InsaneRoboticist),    20),
            new MonsterTownSpawnEntry(typeof(RenaissanceMechanic), 25),
            new MonsterTownSpawnEntry(typeof(RetroAndroid),        25),
            new MonsterTownSpawnEntry(typeof(RetroRobotRomancer),  20),
            new MonsterTownSpawnEntry(typeof(StarCitizen),         20),
            new MonsterTownSpawnEntry(typeof(StarfleetCaptain),    20)
        };

        // 13. Historical and Thematic NPCs
        public static MonsterTownSpawnEntry[] HistoricalThematicNPCs = new MonsterTownSpawnEntry[]
        {
            // Themed Warriors
            new MonsterTownSpawnEntry(typeof(BaroqueBarbarian),    25),
            new MonsterTownSpawnEntry(typeof(GrecoRomanWrestler),  25),
            new MonsterTownSpawnEntry(typeof(HippieHoplite),       25),
            // Period Figures
            new MonsterTownSpawnEntry(typeof(JazzAgeJuggernaut),   20),
            new MonsterTownSpawnEntry(typeof(NoirDetective),       20),
            new MonsterTownSpawnEntry(typeof(SilentMovieMonk),     20)
        };

        // 14. Miscellaneous NPCs
        public static MonsterTownSpawnEntry[] MiscellaneousNPCs = new MonsterTownSpawnEntry[]
        {
            // Quirky Characters
            new MonsterTownSpawnEntry(typeof(ChrisRoberts),        15),
            new MonsterTownSpawnEntry(typeof(FloridaMan),          15),
            new MonsterTownSpawnEntry(typeof(FloridaMan),        15),
            new MonsterTownSpawnEntry(typeof(MushroomWitch),       15),
            new MonsterTownSpawnEntry(typeof(Protester),           15),
            // Undead and Spooky
            new MonsterTownSpawnEntry(typeof(GhostScout),          20),
            new MonsterTownSpawnEntry(typeof(GhostWarrior),        20),
            new MonsterTownSpawnEntry(typeof(GothicNovelist),      20),
            new MonsterTownSpawnEntry(typeof(SkeletonLord),        20)
        };

        // 15. Demons and Daemonic Entities
        public static MonsterTownSpawnEntry[] DemonicEntities = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(AbysmalHorror),       20),
            new MonsterTownSpawnEntry(typeof(AbyssalAbomination),  20),
            new MonsterTownSpawnEntry(typeof(ArcaneDaemon),        25),
            new MonsterTownSpawnEntry(typeof(Balron),              15),
            new MonsterTownSpawnEntry(typeof(ChaosDaemon),         20),
            new MonsterTownSpawnEntry(typeof(Daemon),              20),
            new MonsterTownSpawnEntry(typeof(DemonKnight),         15),
            new MonsterTownSpawnEntry(typeof(EffetePutridGargoyle),15),
            new MonsterTownSpawnEntry(typeof(FireDaemon),          20),
            new MonsterTownSpawnEntry(typeof(Imp),                 25),
            new MonsterTownSpawnEntry(typeof(Impaler),           20),
            new MonsterTownSpawnEntry(typeof(MaddeningHorror),     15),
            new MonsterTownSpawnEntry(typeof(Succubus),            15),
            new MonsterTownSpawnEntry(typeof(TormentedMinotaur),   15),
            new MonsterTownSpawnEntry(typeof(Succubus),  15),
            new MonsterTownSpawnEntry(typeof(WandererOfTheVoid),   15)
        };

        // 16. Extended Arachnids
        public static MonsterTownSpawnEntry[] ExtendedArachnids = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(AntLion),                 30),
            new MonsterTownSpawnEntry(typeof(BlackSolenInfiltratorQueen),20),
            new MonsterTownSpawnEntry(typeof(BlackSolenInfiltratorWarrior),25),
            new MonsterTownSpawnEntry(typeof(BlackSolenQueen),         20),
            new MonsterTownSpawnEntry(typeof(BlackSolenWarrior),       25),
            new MonsterTownSpawnEntry(typeof(BlackSolenWorker),        30),
            new MonsterTownSpawnEntry(typeof(DreadSpider),             30),
            new MonsterTownSpawnEntry(typeof(GiantBlackWidow),         25),
            new MonsterTownSpawnEntry(typeof(SentinelSpider),          20),
            new MonsterTownSpawnEntry(typeof(SpeckledScorpion),        25),
            new MonsterTownSpawnEntry(typeof(TrapdoorSpider),          25),
            new MonsterTownSpawnEntry(typeof(WolfSpider),              30)
        };

        // 17. Mammals
        public static MonsterTownSpawnEntry[] Mammals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Alligator),             30),
            new MonsterTownSpawnEntry(typeof(BlackBear),             25),
            new MonsterTownSpawnEntry(typeof(BrownBear),             25),
            new MonsterTownSpawnEntry(typeof(GrizzlyBear),           20),
            new MonsterTownSpawnEntry(typeof(PolarBear),             20),
            new MonsterTownSpawnEntry(typeof(Boar),                  30),
            new MonsterTownSpawnEntry(typeof(Bull),                  25),
            new MonsterTownSpawnEntry(typeof(Cougar),                20),
            new MonsterTownSpawnEntry(typeof(GreatHart),             20),
            new MonsterTownSpawnEntry(typeof(Hind),                  20),
            new MonsterTownSpawnEntry(typeof(Dog),                   30),
            new MonsterTownSpawnEntry(typeof(Ferret),                25),
            new MonsterTownSpawnEntry(typeof(BloodFox),              25),
            new MonsterTownSpawnEntry(typeof(Goat),                  25),
            new MonsterTownSpawnEntry(typeof(Gorilla),               20),
            new MonsterTownSpawnEntry(typeof(Horse),                 30),
            new MonsterTownSpawnEntry(typeof(PackHorse),             25),
            new MonsterTownSpawnEntry(typeof(PackHorse),         25),
            new MonsterTownSpawnEntry(typeof(RidableLlama),          20),
            new MonsterTownSpawnEntry(typeof(Lion),                  20),
            new MonsterTownSpawnEntry(typeof(Llama),                 25),
            new MonsterTownSpawnEntry(typeof(Pig),                   30),
            new MonsterTownSpawnEntry(typeof(Rabbit),                35),
            new MonsterTownSpawnEntry(typeof(JackRabbit),            35),
            new MonsterTownSpawnEntry(typeof(VorpalBunny),           20),
            new MonsterTownSpawnEntry(typeof(Squirrel),              30),
            new MonsterTownSpawnEntry(typeof(SabertoothedTiger),     15),
            new MonsterTownSpawnEntry(typeof(WildTiger),             15),
            new MonsterTownSpawnEntry(typeof(Walrus),                20),
            new MonsterTownSpawnEntry(typeof(Wolf),                  30),
            new MonsterTownSpawnEntry(typeof(DireWolf),              25),
            new MonsterTownSpawnEntry(typeof(GreyWolf),              25),
            new MonsterTownSpawnEntry(typeof(GreyWolf),           20),
            new MonsterTownSpawnEntry(typeof(LeatherWolf),           20),
            new MonsterTownSpawnEntry(typeof(TimberWolf),            20),
            new MonsterTownSpawnEntry(typeof(WhiteWolf),             20)
        };

        // 18. Birds
        public static MonsterTownSpawnEntry[] Birds = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Bird),                  40),
            new MonsterTownSpawnEntry(typeof(Chicken),               35),
            new MonsterTownSpawnEntry(typeof(Crane),                 30),
            new MonsterTownSpawnEntry(typeof(Eagle),                 30),
            new MonsterTownSpawnEntry(typeof(Eagle),                 25),
            new MonsterTownSpawnEntry(typeof(Parrot),                25),
            new MonsterTownSpawnEntry(typeof(Phoenix),               20)
        };

        // 19. Reptiles and Amphibians
        public static MonsterTownSpawnEntry[] ReptilesAmphibians = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BullFrog),              35),
            new MonsterTownSpawnEntry(typeof(GiantToad),             30),
            new MonsterTownSpawnEntry(typeof(Lizardman),          25),
            new MonsterTownSpawnEntry(typeof(Lizardman),             25),
            new MonsterTownSpawnEntry(typeof(Snake),                 40),
            new MonsterTownSpawnEntry(typeof(IceSnake),              35),
            new MonsterTownSpawnEntry(typeof(LavaSnake),             35),
            new MonsterTownSpawnEntry(typeof(GiantSerpent),          30),
            new MonsterTownSpawnEntry(typeof(SeaSerpent),            25),
            new MonsterTownSpawnEntry(typeof(SilverSerpent),         30)
        };

        // 20. Goblins
        public static MonsterTownSpawnEntry[] Goblins = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GrayGoblin),            40),
            new MonsterTownSpawnEntry(typeof(GrayGoblinKeeper),      30),
            new MonsterTownSpawnEntry(typeof(GrayGoblinMage),        25),
            new MonsterTownSpawnEntry(typeof(GreenGoblin),           35),
            new MonsterTownSpawnEntry(typeof(GreenGoblinAlchemist),  30),
            new MonsterTownSpawnEntry(typeof(GreenGoblinScout),      30)
        };

        // 21. Extended Humanoids
        public static MonsterTownSpawnEntry[] HumanoidsExtended = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Brigand),             40),
            new MonsterTownSpawnEntry(typeof(ChaosDragoon),        30),
            new MonsterTownSpawnEntry(typeof(ChaosDragoonElite),   30),
            new MonsterTownSpawnEntry(typeof(Ettin),               35),
            new MonsterTownSpawnEntry(typeof(JukaLord),            25),
            new MonsterTownSpawnEntry(typeof(JukaMage),            25),
            new MonsterTownSpawnEntry(typeof(JukaWarrior),         25),
            new MonsterTownSpawnEntry(typeof(Minotaur),            20),
            new MonsterTownSpawnEntry(typeof(Orc),                 40),
            new MonsterTownSpawnEntry(typeof(OrcBomber),           30),
            new MonsterTownSpawnEntry(typeof(OrcBrute),            30),
            new MonsterTownSpawnEntry(typeof(OrcCaptain),          35),
            new MonsterTownSpawnEntry(typeof(OrcChopper),          25),
            new MonsterTownSpawnEntry(typeof(OrcishLord),          30),
            new MonsterTownSpawnEntry(typeof(OrcishMage),          30),
            new MonsterTownSpawnEntry(typeof(Savage),              25),
            new MonsterTownSpawnEntry(typeof(SavageRider),         25),
            new MonsterTownSpawnEntry(typeof(SavageShaman),        20)
        };

        // 22. Magical Beasts
        public static MonsterTownSpawnEntry[] MagicalBeasts = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BakeKitsune),         25),
            new MonsterTownSpawnEntry(typeof(CuSidhe),             25),
            new MonsterTownSpawnEntry(typeof(EtherealWarrior),     20),
            new MonsterTownSpawnEntry(typeof(Hiryu),               20),
            new MonsterTownSpawnEntry(typeof(Kirin),               20),
            new MonsterTownSpawnEntry(typeof(Nightmare),           25),
            new MonsterTownSpawnEntry(typeof(Reptalon),            25),
            new MonsterTownSpawnEntry(typeof(Unicorn),             20),
            new MonsterTownSpawnEntry(typeof(Yamandon),            20)
        };

        // 23. Spirits and Wisps
        public static MonsterTownSpawnEntry[] SpiritsWisps = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(MaddeningHorror),          30),
            new MonsterTownSpawnEntry(typeof(MaddeningHorror),     25),
            new MonsterTownSpawnEntry(typeof(ShadowWisp),          30),
            new MonsterTownSpawnEntry(typeof(Wisp),                30)
        };

        // 24. Aquatic Creatures
        public static MonsterTownSpawnEntry[] AquaticCreatures = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(CoralSnake),          25),
            new MonsterTownSpawnEntry(typeof(DeepSeaSerpent),      20),
            new MonsterTownSpawnEntry(typeof(Dolphin),             30),
            new MonsterTownSpawnEntry(typeof(Kraken),              15),
            new MonsterTownSpawnEntry(typeof(Leviathan),           15),
            new MonsterTownSpawnEntry(typeof(SeaHorse),            30)
        };

        // 25. Insects and Crawlers
        public static MonsterTownSpawnEntry[] InsectsCrawlers = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(AcidSlug),            30),
            new MonsterTownSpawnEntry(typeof(Bogling),             25),
            new MonsterTownSpawnEntry(typeof(BogThing),            25),
            new MonsterTownSpawnEntry(typeof(GiantIceWorm),        20),
            new MonsterTownSpawnEntry(typeof(MoundOfMaggots),      20),
            new MonsterTownSpawnEntry(typeof(PestilentBandage),    20),
            new MonsterTownSpawnEntry(typeof(Slime),               30),
            new MonsterTownSpawnEntry(typeof(SkitteringHopper),    25),
            new MonsterTownSpawnEntry(typeof(SkitteringHopper),             30)
        };

        // 26. Giants and Trolls
        public static MonsterTownSpawnEntry[] GiantsTrolls = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Cyclops),             30),
            new MonsterTownSpawnEntry(typeof(GiantTurkey),         25),
            new MonsterTownSpawnEntry(typeof(Troll),               30),
            new MonsterTownSpawnEntry(typeof(Ogre),                30),
            new MonsterTownSpawnEntry(typeof(OgreLord),            25)
        };

        // 27. Miscellaneous Monsters
        public static MonsterTownSpawnEntry[] MiscellaneousMonsters = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(ClockworkScorpion),   30),
            new MonsterTownSpawnEntry(typeof(Golem),               25),
            new MonsterTownSpawnEntry(typeof(Mimic),               25),
            new MonsterTownSpawnEntry(typeof(Ravager),             25),
            new MonsterTownSpawnEntry(typeof(Shade),               25),
            new MonsterTownSpawnEntry(typeof(Treefellow),          20),
            new MonsterTownSpawnEntry(typeof(YomotsuPriest),       20),
            new MonsterTownSpawnEntry(typeof(YomotsuPriest),      20)
        };

        // Air Elementals encounter:
        public static MonsterTownSpawnEntry[] InvasionAirElementals = new MonsterTownSpawnEntry[]
        {
            // Basic monster type (lots of spawns)
            new MonsterTownSpawnEntry(typeof(AirElemental), 150),
            // Unique air elemental monsters
            new MonsterTownSpawnEntry(typeof(BreezePhantom), 20),
            new MonsterTownSpawnEntry(typeof(CycloneDemon), 20),
            new MonsterTownSpawnEntry(typeof(GaleWisp), 20),
            new MonsterTownSpawnEntry(typeof(MysticWisp), 20),
            new MonsterTownSpawnEntry(typeof(ShadowDrifter), 20),
            new MonsterTownSpawnEntry(typeof(SkySeraph), 20),
            new MonsterTownSpawnEntry(typeof(StormHerald), 20),
            new MonsterTownSpawnEntry(typeof(TempestSpirit), 20),
            new MonsterTownSpawnEntry(typeof(TempestWyrm), 20),
            new MonsterTownSpawnEntry(typeof(VortexGuardian), 20),
            new MonsterTownSpawnEntry(typeof(WhirlwindFiend), 20),
            new MonsterTownSpawnEntry(typeof(ZephyrWarden), 20)
        };

        // Alligators encounter:
        public static MonsterTownSpawnEntry[] InvasionAlligators = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Alligator), 150),
            new MonsterTownSpawnEntry(typeof(AcidicAlligator), 20),
            new MonsterTownSpawnEntry(typeof(AncientAlligator), 20),
            new MonsterTownSpawnEntry(typeof(FirebreathAlligator), 20),
            new MonsterTownSpawnEntry(typeof(FrostbiteAlligator), 20),
            new MonsterTownSpawnEntry(typeof(IllusionaryAlligator), 20),
            new MonsterTownSpawnEntry(typeof(RagingAlligator), 20),
            new MonsterTownSpawnEntry(typeof(ShadowAlligator), 20),
            new MonsterTownSpawnEntry(typeof(StormAlligator), 20),
            new MonsterTownSpawnEntry(typeof(ToxicAlligator), 20),
            new MonsterTownSpawnEntry(typeof(VenomousAlligator), 20)
        };

        // Bears encounter:
        public static MonsterTownSpawnEntry[] InvasionBears = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BrownBear), 150),
            new MonsterTownSpawnEntry(typeof(FlameBear), 20),
            new MonsterTownSpawnEntry(typeof(FrostBear), 20),
            new MonsterTownSpawnEntry(typeof(LeafBear), 20),
            new MonsterTownSpawnEntry(typeof(LightBear), 20),
            new MonsterTownSpawnEntry(typeof(RockBear), 20),
            new MonsterTownSpawnEntry(typeof(ShadowBear), 20),
            new MonsterTownSpawnEntry(typeof(SteelBear), 20),
            new MonsterTownSpawnEntry(typeof(ThunderBear), 20),
            new MonsterTownSpawnEntry(typeof(VenomBear), 20),
            new MonsterTownSpawnEntry(typeof(WindBear), 20)
        };

        // Bulls encounter:
        public static MonsterTownSpawnEntry[] InvasionBulls = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Bull), 150),
            new MonsterTownSpawnEntry(typeof(AngusBerserker), 20),
            new MonsterTownSpawnEntry(typeof(BisonBrute), 20),
            new MonsterTownSpawnEntry(typeof(DairyWraith), 20),
            new MonsterTownSpawnEntry(typeof(GuernseyGuardian), 20),
            new MonsterTownSpawnEntry(typeof(HerefordWarlock), 20),
            new MonsterTownSpawnEntry(typeof(HighlandBull), 20),
            new MonsterTownSpawnEntry(typeof(JerseyEnchantress), 20),
            new MonsterTownSpawnEntry(typeof(MilkingDemon), 20),
            new MonsterTownSpawnEntry(typeof(SahiwalShaman), 20),
            new MonsterTownSpawnEntry(typeof(ZebuZealot), 20)
        };

        // Cats encounter:
        public static MonsterTownSpawnEntry[] InvasionCats = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Cat), 150),
            new MonsterTownSpawnEntry(typeof(AbyssinianTracker), 20),
            new MonsterTownSpawnEntry(typeof(BengalStorm), 20),
            new MonsterTownSpawnEntry(typeof(MaineCoonTitan), 20),
            new MonsterTownSpawnEntry(typeof(PersianShade), 20),
            new MonsterTownSpawnEntry(typeof(RagdollGuardian), 20),
            new MonsterTownSpawnEntry(typeof(ScottishFoldSentinel), 20),
            new MonsterTownSpawnEntry(typeof(SiameseIllusionist), 20),
            new MonsterTownSpawnEntry(typeof(SiberianFrostclaw), 20),
            new MonsterTownSpawnEntry(typeof(SphinxCat), 20),
            new MonsterTownSpawnEntry(typeof(TurkishAngoraEnchanter), 20)
        };

        // Chickens encounter:
        public static MonsterTownSpawnEntry[] InvasionChickens = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Chicken), 150),
            new MonsterTownSpawnEntry(typeof(FireRooster), 20),
            new MonsterTownSpawnEntry(typeof(FrostHen), 20),
            new MonsterTownSpawnEntry(typeof(IllusionHen), 20),
            new MonsterTownSpawnEntry(typeof(MysticFowl), 20),
            new MonsterTownSpawnEntry(typeof(NecroRooster), 20),
            new MonsterTownSpawnEntry(typeof(PoisonPullet), 20),
            new MonsterTownSpawnEntry(typeof(ShadowChick), 20),
            new MonsterTownSpawnEntry(typeof(StoneRooster), 20),
            new MonsterTownSpawnEntry(typeof(Thunderbird), 20),
            new MonsterTownSpawnEntry(typeof(WindChicken), 20)
        };

        // Corpser encounter:
        public static MonsterTownSpawnEntry[] InvasionCorpser = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Corpser), 150),
            new MonsterTownSpawnEntry(typeof(BloodthirstyVines), 20),
            new MonsterTownSpawnEntry(typeof(CorruptingCreeper), 20),
            new MonsterTownSpawnEntry(typeof(DreadedCreeper), 20),
            new MonsterTownSpawnEntry(typeof(ElderTendril), 20),
            new MonsterTownSpawnEntry(typeof(NightshadeBramble), 20),
            new MonsterTownSpawnEntry(typeof(PhantomVines), 20),
            new MonsterTownSpawnEntry(typeof(SinisterRoot), 20),
            new MonsterTownSpawnEntry(typeof(ThornedHorror), 20),
            new MonsterTownSpawnEntry(typeof(VenomousIvy), 20),
            new MonsterTownSpawnEntry(typeof(VileBlossom), 20)
        };

        // Crabs encounter:
        public static MonsterTownSpawnEntry[] InvasionCrabs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(CoconutCrab), 150),
            new MonsterTownSpawnEntry(typeof(BansheeCrab), 20),
            new MonsterTownSpawnEntry(typeof(EtherealCrab), 20),
            new MonsterTownSpawnEntry(typeof(IceCrab), 20),
            new MonsterTownSpawnEntry(typeof(LavaCrab), 20),
            new MonsterTownSpawnEntry(typeof(MagneticCrab), 20),
            new MonsterTownSpawnEntry(typeof(PoisonousCrab), 20),
            new MonsterTownSpawnEntry(typeof(RiptideCrab), 20),
            new MonsterTownSpawnEntry(typeof(ShadowCrab), 20),
            new MonsterTownSpawnEntry(typeof(StormCrab), 20),
            new MonsterTownSpawnEntry(typeof(VortexCrab), 20)
        };

        // Daemons encounter:
        public static MonsterTownSpawnEntry[] InvasionDaemons = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Daemon), 150),
            new MonsterTownSpawnEntry(typeof(AbyssalWarden), 20),
            new MonsterTownSpawnEntry(typeof(BlightDemon), 20),
            new MonsterTownSpawnEntry(typeof(CursedHarbinger), 20),
            new MonsterTownSpawnEntry(typeof(Deadlord), 20),
            new MonsterTownSpawnEntry(typeof(FrostboundBehemoth), 20),
            new MonsterTownSpawnEntry(typeof(HellfireJuggernaut), 20),
            new MonsterTownSpawnEntry(typeof(InfernalIncinerator), 20),
            new MonsterTownSpawnEntry(typeof(ToxicReaver), 20),
            new MonsterTownSpawnEntry(typeof(ToxicReaver), 20),
            new MonsterTownSpawnEntry(typeof(VoidStalker), 20)
        };

        // Dragons encounter:
        public static MonsterTownSpawnEntry[] InvasionDragons = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Dragon), 150),
            new MonsterTownSpawnEntry(typeof(AncientDragon), 20),
            new MonsterTownSpawnEntry(typeof(BloodDragon), 20),
            new MonsterTownSpawnEntry(typeof(CelestialDragon), 20),
            new MonsterTownSpawnEntry(typeof(CrystalDragon), 20),
            new MonsterTownSpawnEntry(typeof(EtherealDragon), 20),
            new MonsterTownSpawnEntry(typeof(FrostDrakon), 20),
            new MonsterTownSpawnEntry(typeof(InfernoDrakon), 20),
            new MonsterTownSpawnEntry(typeof(NatureDragon), 20),
            new MonsterTownSpawnEntry(typeof(ShadowDragon), 20),
            new MonsterTownSpawnEntry(typeof(StormDragon), 20),
            new MonsterTownSpawnEntry(typeof(VenomousDragon), 20)
        };

        // Earth Elementals encounter:
        public static MonsterTownSpawnEntry[] InvasionEarthElementals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(EarthElemental), 150),
            new MonsterTownSpawnEntry(typeof(CrystalWarden), 20),
            new MonsterTownSpawnEntry(typeof(FossilElemental), 20),
            new MonsterTownSpawnEntry(typeof(GraniteColossus), 20),
            new MonsterTownSpawnEntry(typeof(LavaFiend), 20),
            new MonsterTownSpawnEntry(typeof(MagmaGolem), 20),
            new MonsterTownSpawnEntry(typeof(MudGolem), 20),
            new MonsterTownSpawnEntry(typeof(QuakeBringer), 20),
            new MonsterTownSpawnEntry(typeof(SandstormElemental), 20),
            new MonsterTownSpawnEntry(typeof(StoneGuardian), 20),
            new MonsterTownSpawnEntry(typeof(TerraWisp), 20)
        };

        // Ettin encounter:
        public static MonsterTownSpawnEntry[] InvasionEttin = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Ettin), 150),
            new MonsterTownSpawnEntry(typeof(CerebralEttin), 20),
            new MonsterTownSpawnEntry(typeof(EarthquakeEttin), 20),
            new MonsterTownSpawnEntry(typeof(FlameWardenEttin), 20),
            new MonsterTownSpawnEntry(typeof(FrostWardenEttin), 20),
            new MonsterTownSpawnEntry(typeof(IllusionistEttin), 20),
            new MonsterTownSpawnEntry(typeof(NecroEttin), 20),
            new MonsterTownSpawnEntry(typeof(StormcallerEttin), 20),
            new MonsterTownSpawnEntry(typeof(TidalEttin), 20),
            new MonsterTownSpawnEntry(typeof(TwinTerrorEttin), 20),
            new MonsterTownSpawnEntry(typeof(VenomousEttin), 20)
        };

        // Ferrets encounter:
        public static MonsterTownSpawnEntry[] InvasionFerrets = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Ferret), 150),
            new MonsterTownSpawnEntry(typeof(BubbleFerret), 20),
            new MonsterTownSpawnEntry(typeof(DreamyFerret), 20),
            new MonsterTownSpawnEntry(typeof(FrostyFerret), 20),
            new MonsterTownSpawnEntry(typeof(GlimmeringFerret), 20),
            new MonsterTownSpawnEntry(typeof(HarmonyFerret), 20),
            new MonsterTownSpawnEntry(typeof(MysticFerret), 20),
            new MonsterTownSpawnEntry(typeof(PuffyFerret), 20),
            new MonsterTownSpawnEntry(typeof(SparkFerret), 20),
            new MonsterTownSpawnEntry(typeof(StarryFerret), 20),
            new MonsterTownSpawnEntry(typeof(SunbeamFerret), 20)
        };

        // Fey encounter:
        public static MonsterTownSpawnEntry[] InvasionFey = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Satyr), 150),
            new MonsterTownSpawnEntry(typeof(Banshee), 20),
            new MonsterTownSpawnEntry(typeof(Chaneque), 20),
            new MonsterTownSpawnEntry(typeof(Dryad), 20),
            new MonsterTownSpawnEntry(typeof(Fairy), 20),
            new MonsterTownSpawnEntry(typeof(Leprechaun), 20),
            new MonsterTownSpawnEntry(typeof(Nymph), 20),
            new MonsterTownSpawnEntry(typeof(Puck), 20),
            new MonsterTownSpawnEntry(typeof(Selkie), 20),
            new MonsterTownSpawnEntry(typeof(Sidhe), 20),
            new MonsterTownSpawnEntry(typeof(WillOTheWisp), 20)
        };

        // Fire Elementals encounter:
        public static MonsterTownSpawnEntry[] InvasionFireElementals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(FireElemental), 150),
            new MonsterTownSpawnEntry(typeof(CinderWraith), 20),
            new MonsterTownSpawnEntry(typeof(EmberSerpent), 20),
            new MonsterTownSpawnEntry(typeof(EmberSpirit), 20),
            new MonsterTownSpawnEntry(typeof(FlareImp), 20),
            new MonsterTownSpawnEntry(typeof(InfernalDuke), 20),
            new MonsterTownSpawnEntry(typeof(InfernoWarden), 20),
            new MonsterTownSpawnEntry(typeof(MoltenGolem), 20),
            new MonsterTownSpawnEntry(typeof(PyroclasticGolem), 20),
            new MonsterTownSpawnEntry(typeof(SolarElemental), 20),
            new MonsterTownSpawnEntry(typeof(VolcanicTitan), 20)
        };

        // Gargoyles encounter:
        public static MonsterTownSpawnEntry[] InvasionGargoyles = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Gargoyle), 150),
            new MonsterTownSpawnEntry(typeof(AbbadonTheAbyssal), 20),
            new MonsterTownSpawnEntry(typeof(Chimereon), 20),
            new MonsterTownSpawnEntry(typeof(Drolatic), 20),
            new MonsterTownSpawnEntry(typeof(Grimorie), 20),
            new MonsterTownSpawnEntry(typeof(GrotesqueOfRouen), 20),
            new MonsterTownSpawnEntry(typeof(GrymalkinTheWatcher), 20),
            new MonsterTownSpawnEntry(typeof(Mordrake), 20),
            new MonsterTownSpawnEntry(typeof(Strix), 20),
            new MonsterTownSpawnEntry(typeof(Vespa), 20),
            new MonsterTownSpawnEntry(typeof(VitrailTheMosaic), 20)
        };

        // Gazers encounter:
        public static MonsterTownSpawnEntry[] InvasionGazers = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Gazer), 150),
            new MonsterTownSpawnEntry(typeof(IshKarTheForgotten), 20),
            new MonsterTownSpawnEntry(typeof(NyxRith), 20),
            new MonsterTownSpawnEntry(typeof(QuorZael), 20),
            new MonsterTownSpawnEntry(typeof(RathZorTheShattered), 20),
            new MonsterTownSpawnEntry(typeof(ThulGorTheForsaken), 20),
            new MonsterTownSpawnEntry(typeof(UruKoth), 20),
            new MonsterTownSpawnEntry(typeof(Vorgath), 20),
            new MonsterTownSpawnEntry(typeof(XalRath), 20),
            new MonsterTownSpawnEntry(typeof(ZelVrak), 20),
            new MonsterTownSpawnEntry(typeof(ZorThul), 20)
        };

        // Giant Serpant encounter:
        public static MonsterTownSpawnEntry[] InvasionGiantSerpant = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GiantSerpent), 150),
            new MonsterTownSpawnEntry(typeof(BloodSerpent), 20),
            new MonsterTownSpawnEntry(typeof(CelestialPython), 20),
            new MonsterTownSpawnEntry(typeof(EmperorCobra), 20),
            new MonsterTownSpawnEntry(typeof(FrostSerpent), 20),
            new MonsterTownSpawnEntry(typeof(GorgonViper), 20),
            new MonsterTownSpawnEntry(typeof(InfernoPython), 20),
            new MonsterTownSpawnEntry(typeof(ShadowAnaconda), 20),
            new MonsterTownSpawnEntry(typeof(ThunderSerpent), 20),
            new MonsterTownSpawnEntry(typeof(TitanBoa), 20),
            new MonsterTownSpawnEntry(typeof(VengefulPitViper), 20)
        };

        // Giant Spider encounter:
        public static MonsterTownSpawnEntry[] InvasionGiantSpider = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GiantSpider), 150),
            new MonsterTownSpawnEntry(typeof(BlackWidowQueen), 20),
            new MonsterTownSpawnEntry(typeof(GiantTrapdoorSpider), 20),
            new MonsterTownSpawnEntry(typeof(GiantWolfSpider), 20),
            new MonsterTownSpawnEntry(typeof(GoldenOrbWeaver), 20),
            new MonsterTownSpawnEntry(typeof(GoliathBirdeater), 20),
            new MonsterTownSpawnEntry(typeof(HuntsmanSpider), 20),
            new MonsterTownSpawnEntry(typeof(PurseSpider), 20),
            new MonsterTownSpawnEntry(typeof(ScorpionSpider), 20),
            new MonsterTownSpawnEntry(typeof(ScorpionSpider), 20),
            new MonsterTownSpawnEntry(typeof(TarantulaWarrior), 20)
        };

        // Goats encounter:
        public static MonsterTownSpawnEntry[] InvasionGoats = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Goat), 150),
            new MonsterTownSpawnEntry(typeof(BeardedGoat), 20),
            new MonsterTownSpawnEntry(typeof(Chamois), 20),
            new MonsterTownSpawnEntry(typeof(CliffGoat), 20),
            new MonsterTownSpawnEntry(typeof(DallSheep), 20),
            new MonsterTownSpawnEntry(typeof(FaintingGoat), 20),
            new MonsterTownSpawnEntry(typeof(Goral), 20),
            new MonsterTownSpawnEntry(typeof(Ibex), 20),
            new MonsterTownSpawnEntry(typeof(Markhor), 20),
            new MonsterTownSpawnEntry(typeof(SableAntelope), 20),
            new MonsterTownSpawnEntry(typeof(Tahr), 20)
        };

        // Golems encounter:
        public static MonsterTownSpawnEntry[] InvasionGolems = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Golem), 150),
            new MonsterTownSpawnEntry(typeof(BoneGolem), 20),
            new MonsterTownSpawnEntry(typeof(CheeseGolem), 20),
            new MonsterTownSpawnEntry(typeof(CrystalGolem), 20),
            new MonsterTownSpawnEntry(typeof(IronGolem), 20),
            new MonsterTownSpawnEntry(typeof(MeatGolem), 20),
            new MonsterTownSpawnEntry(typeof(MuckGolem), 20),
            new MonsterTownSpawnEntry(typeof(SandGolem), 20),
            new MonsterTownSpawnEntry(typeof(ShadowGolem), 20),
            new MonsterTownSpawnEntry(typeof(StoneGolem), 20),
            new MonsterTownSpawnEntry(typeof(WoodGolem), 20)
        };

        // Gorilla encounter:
        public static MonsterTownSpawnEntry[] InvasionGorilla = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Gorilla), 150),
            new MonsterTownSpawnEntry(typeof(BaboonsAlpha), 20),
            new MonsterTownSpawnEntry(typeof(CapuchinTrickster), 20),
            new MonsterTownSpawnEntry(typeof(ChimpanzeeBerserker), 20),
            new MonsterTownSpawnEntry(typeof(GibbonMystic), 20),
            new MonsterTownSpawnEntry(typeof(HowlerMonkey), 20),
            new MonsterTownSpawnEntry(typeof(MandrillShaman), 20),
            new MonsterTownSpawnEntry(typeof(MountainGorilla), 20),
            new MonsterTownSpawnEntry(typeof(OrangutanSage), 20),
            new MonsterTownSpawnEntry(typeof(SifakaWarrior), 20),
            new MonsterTownSpawnEntry(typeof(SpiderMonkey), 20)
        };

        // GreatHart encounter:
        public static MonsterTownSpawnEntry[] InvasionGreatHart = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GreatHart), 150),
            new MonsterTownSpawnEntry(typeof(AzureMoose), 20),
            new MonsterTownSpawnEntry(typeof(CrimsonMule), 20),
            new MonsterTownSpawnEntry(typeof(CursedWhiteTail), 20),
            new MonsterTownSpawnEntry(typeof(EclipseReindeer), 20),
            new MonsterTownSpawnEntry(typeof(EmberAxis), 20),
            new MonsterTownSpawnEntry(typeof(FrostWapiti), 20),
            new MonsterTownSpawnEntry(typeof(MysticFallow), 20),
            new MonsterTownSpawnEntry(typeof(ShadowMuntjac), 20),
            new MonsterTownSpawnEntry(typeof(StormSika), 20),
            new MonsterTownSpawnEntry(typeof(VenomousRoe), 20)
        };

        // Grizzly Bear encounter:
        public static MonsterTownSpawnEntry[] InvasionGrizzlyBear = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(BlackBear), 150),
            new MonsterTownSpawnEntry(typeof(AriesRamBear), 20),
            new MonsterTownSpawnEntry(typeof(CancerShellBear), 20),
            new MonsterTownSpawnEntry(typeof(CapricornMountainBear), 20),
            new MonsterTownSpawnEntry(typeof(GeminiTwinBear), 20),
            new MonsterTownSpawnEntry(typeof(LeoSunBear), 20),
            new MonsterTownSpawnEntry(typeof(LibraBalanceBear), 20),
            new MonsterTownSpawnEntry(typeof(SagittariusArcherBear), 20),
            new MonsterTownSpawnEntry(typeof(ScorpioVenomBear), 20),
            new MonsterTownSpawnEntry(typeof(TaurusEarthBear), 20),
            new MonsterTownSpawnEntry(typeof(VirgoPurityBear), 20)
        };

        // Harpy encounter:
        public static MonsterTownSpawnEntry[] InvasionHarpy = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Harpy), 150),
            new MonsterTownSpawnEntry(typeof(AriesHarpy), 20),
            new MonsterTownSpawnEntry(typeof(CancerHarpy), 20),
            new MonsterTownSpawnEntry(typeof(CapricornHarpy), 20),
            new MonsterTownSpawnEntry(typeof(GeminiHarpy), 20),
            new MonsterTownSpawnEntry(typeof(LeoHarpy), 20),
            new MonsterTownSpawnEntry(typeof(LibraHarpy), 20),
            new MonsterTownSpawnEntry(typeof(SagittariusHarpy), 20),
            new MonsterTownSpawnEntry(typeof(ScorpioHarpy), 20),
            new MonsterTownSpawnEntry(typeof(TaurusHarpy), 20),
            new MonsterTownSpawnEntry(typeof(VirgoHarpy), 20)
        };

        // Horses encounter:
        public static MonsterTownSpawnEntry[] InvasionHorses = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Horse), 150),
            new MonsterTownSpawnEntry(typeof(InfernoStallion), 20),
            new MonsterTownSpawnEntry(typeof(IronSteed), 20),
            new MonsterTownSpawnEntry(typeof(MetallicWindsteed), 20),
            new MonsterTownSpawnEntry(typeof(StoneSteed), 20),
            new MonsterTownSpawnEntry(typeof(TidalMare), 20),
            new MonsterTownSpawnEntry(typeof(VolcanicCharger), 20),
            new MonsterTownSpawnEntry(typeof(WoodlandCharger), 20),
            new MonsterTownSpawnEntry(typeof(WoodlandSpiritHorse), 20),
            new MonsterTownSpawnEntry(typeof(YangStallion), 20),
            new MonsterTownSpawnEntry(typeof(YinSteed), 20)
        };

        // Ancient Liches encounter:
        public static MonsterTownSpawnEntry[] InvasionAncientLiches = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(AncientLich), 150),
            new MonsterTownSpawnEntry(typeof(Acererak), 20),
            new MonsterTownSpawnEntry(typeof(AzalinRex), 20),
            new MonsterTownSpawnEntry(typeof(KasTheBloodyHanded), 20),
            new MonsterTownSpawnEntry(typeof(KelThuzad), 20),
            new MonsterTownSpawnEntry(typeof(LarlochTheShadowKing), 20),
            new MonsterTownSpawnEntry(typeof(Nagash), 20),
            new MonsterTownSpawnEntry(typeof(RaistlinMajere), 20),
            new MonsterTownSpawnEntry(typeof(SothTheDeathKnight), 20),
            new MonsterTownSpawnEntry(typeof(SzassTam), 20),
            new MonsterTownSpawnEntry(typeof(Vecna), 20)
        };

        // Lichs encounter:
        public static MonsterTownSpawnEntry[] InvasionLichs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Lich), 150),
            new MonsterTownSpawnEntry(typeof(BloodLich), 20),
            new MonsterTownSpawnEntry(typeof(FrostLich), 20),
            new MonsterTownSpawnEntry(typeof(InfernalLich), 20),
            new MonsterTownSpawnEntry(typeof(NecroticLich), 20),
            new MonsterTownSpawnEntry(typeof(PlagueLich), 20),
            new MonsterTownSpawnEntry(typeof(ShadowLich), 20),
            new MonsterTownSpawnEntry(typeof(SoulEaterLich), 20),
            new MonsterTownSpawnEntry(typeof(StormLich), 20),
            new MonsterTownSpawnEntry(typeof(ToxicLich), 20),
            new MonsterTownSpawnEntry(typeof(WraithLich), 20)
        };

        // Llama encounter:
        public static MonsterTownSpawnEntry[] InvasionLlama = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Llama), 150),
            new MonsterTownSpawnEntry(typeof(CactusLlama), 20),
            new MonsterTownSpawnEntry(typeof(CharroLlama), 20),
            new MonsterTownSpawnEntry(typeof(DiaDeLosMuertosLlama), 20),
            new MonsterTownSpawnEntry(typeof(ElMariachiLlama), 20),
            new MonsterTownSpawnEntry(typeof(FiestaLlama), 20),
            new MonsterTownSpawnEntry(typeof(LuchadorLlama), 20),
            new MonsterTownSpawnEntry(typeof(SombreroDeSolLlama), 20),
            new MonsterTownSpawnEntry(typeof(SombreroLlama), 20),
            new MonsterTownSpawnEntry(typeof(TacoLlama), 20),
            new MonsterTownSpawnEntry(typeof(TequilaLlama), 20)
        };

        // Mummy encounter:
        public static MonsterTownSpawnEntry[] InvasionMummy = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Mummy), 150),
            new MonsterTownSpawnEntry(typeof(AkhenatenTheHeretic), 20),
            new MonsterTownSpawnEntry(typeof(CleopatraTheEnigmatic), 20),
            new MonsterTownSpawnEntry(typeof(HatshepsutTheQueen), 20),
            new MonsterTownSpawnEntry(typeof(KhufuTheGreatBuilder), 20),
            new MonsterTownSpawnEntry(typeof(MentuhotepTheWise), 20),
            new MonsterTownSpawnEntry(typeof(Nefertiti), 20),
            new MonsterTownSpawnEntry(typeof(RamsesTheImmortal), 20),
            new MonsterTownSpawnEntry(typeof(SetiTheAvenger), 20),
            new MonsterTownSpawnEntry(typeof(ThutmoseTheConqueror), 20),
            new MonsterTownSpawnEntry(typeof(TutankhamunTheCursed), 20)
        };

        // Ogres encounter:
        public static MonsterTownSpawnEntry[] InvasionOgres = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Ogre), 150),
            new MonsterTownSpawnEntry(typeof(BonecrusherOgre), 20),
            new MonsterTownSpawnEntry(typeof(ChromaticOgre), 20),
            new MonsterTownSpawnEntry(typeof(FlamebringerOgre), 20),
            new MonsterTownSpawnEntry(typeof(FleshEaterOgre), 20),
            new MonsterTownSpawnEntry(typeof(FrostOgre), 20),
            new MonsterTownSpawnEntry(typeof(GloomOgre), 20),
            new MonsterTownSpawnEntry(typeof(IroncladOgre), 20),
            new MonsterTownSpawnEntry(typeof(NecroticOgre), 20),
            new MonsterTownSpawnEntry(typeof(ShadowOgre), 20),
            new MonsterTownSpawnEntry(typeof(StormOgre), 20),
            new MonsterTownSpawnEntry(typeof(ToxicOgre), 20)
        };

        // Panther encounter:
        public static MonsterTownSpawnEntry[] InvasionPanther = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Panther), 150),
            new MonsterTownSpawnEntry(typeof(AbyssalPanther), 20),
            new MonsterTownSpawnEntry(typeof(CelestialHorror), 20),
            new MonsterTownSpawnEntry(typeof(CosmicStalker), 20),
            new MonsterTownSpawnEntry(typeof(EtherealPanthra), 20),
            new MonsterTownSpawnEntry(typeof(NebulaCat), 20),
            new MonsterTownSpawnEntry(typeof(NightmarePanther), 20),
            new MonsterTownSpawnEntry(typeof(PhantomPanther), 20),
            new MonsterTownSpawnEntry(typeof(StarbornPredator), 20),
            new MonsterTownSpawnEntry(typeof(VoidCat), 20)
        };

        // Pigs encounter:
        public static MonsterTownSpawnEntry[] InvasionPigs = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Pig), 150),
            new MonsterTownSpawnEntry(typeof(BabirusaBeast), 20),
            new MonsterTownSpawnEntry(typeof(BorneoPig), 20),
            new MonsterTownSpawnEntry(typeof(BushPig), 20),
            new MonsterTownSpawnEntry(typeof(DomesticSwine), 20),
            new MonsterTownSpawnEntry(typeof(GiantForestHog), 20),
            new MonsterTownSpawnEntry(typeof(HogWild), 20),
            new MonsterTownSpawnEntry(typeof(JavelinaJinx), 20),
            new MonsterTownSpawnEntry(typeof(PeccaryProtector), 20),
            new MonsterTownSpawnEntry(typeof(VietnamesePig), 20),
            new MonsterTownSpawnEntry(typeof(WarthogWarrior), 20)
        };

        // Rabbit encounter:
        public static MonsterTownSpawnEntry[] InvasionRabbit = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Rabbit), 150),
            new MonsterTownSpawnEntry(typeof(AbyssalBouncer), 20),
            new MonsterTownSpawnEntry(typeof(ChaosHare), 20),
            new MonsterTownSpawnEntry(typeof(CosmicBouncer), 20),
            new MonsterTownSpawnEntry(typeof(EldritchHarbinger), 20),
            new MonsterTownSpawnEntry(typeof(EldritchHare), 20),
            new MonsterTownSpawnEntry(typeof(EnigmaticSkipper), 20),
            new MonsterTownSpawnEntry(typeof(ForgottenWarden), 20),
            new MonsterTownSpawnEntry(typeof(InfinitePouncer), 20),
            new MonsterTownSpawnEntry(typeof(NightmareLeaper), 20),
            new MonsterTownSpawnEntry(typeof(WhisperingPooka), 20)
        };

        // Rats encounter:
        public static MonsterTownSpawnEntry[] InvasionRats = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Rat), 150),
            new MonsterTownSpawnEntry(typeof(AnthraxRat), 20),
            new MonsterTownSpawnEntry(typeof(BlackDeathRat), 20),
            new MonsterTownSpawnEntry(typeof(CholeraRat), 20),
            new MonsterTownSpawnEntry(typeof(DeathRat), 20),
            new MonsterTownSpawnEntry(typeof(FeverRat), 20),
            new MonsterTownSpawnEntry(typeof(LeprosyRat), 20),
            new MonsterTownSpawnEntry(typeof(MalariaRat), 20),
            new MonsterTownSpawnEntry(typeof(RabidRat), 20),
            new MonsterTownSpawnEntry(typeof(SmallpoxRat), 20),
            new MonsterTownSpawnEntry(typeof(TyphusRat), 20)
        };

        // Robot encounter:
        public static MonsterTownSpawnEntry[] InvasionRobot = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(ExodusMinion), 150),
            new MonsterTownSpawnEntry(typeof(AegisConstruct), 20),
            new MonsterTownSpawnEntry(typeof(ArbiterDrone), 20),
            new MonsterTownSpawnEntry(typeof(Mimicron), 20),
            new MonsterTownSpawnEntry(typeof(NemesisUnit), 20),
            new MonsterTownSpawnEntry(typeof(OmegaSentinel), 20),
            new MonsterTownSpawnEntry(typeof(OverlordMkII), 20),
            new MonsterTownSpawnEntry(typeof(PhantomAutomaton), 20),
            new MonsterTownSpawnEntry(typeof(QuantumGuardian), 20),
            new MonsterTownSpawnEntry(typeof(SynthroidPrime), 20),
            new MonsterTownSpawnEntry(typeof(TalonMachine), 20)
        };

        // Robot Overlord encounter:
        public static MonsterTownSpawnEntry[] InvasionRobotOverlord = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(ExodusOverseer), 150),
            new MonsterTownSpawnEntry(typeof(Dreadnaught), 20),
            new MonsterTownSpawnEntry(typeof(ElectroWraith), 20),
            new MonsterTownSpawnEntry(typeof(FrostDroid), 20),
            new MonsterTownSpawnEntry(typeof(GrapplerDrone), 20),
            new MonsterTownSpawnEntry(typeof(InfernoSentinel), 20),
            new MonsterTownSpawnEntry(typeof(NanoSwarm), 20),
            new MonsterTownSpawnEntry(typeof(PlasmaJuggernaut), 20),
            new MonsterTownSpawnEntry(typeof(SpectralAutomaton), 20),
            new MonsterTownSpawnEntry(typeof(TacticalEnforcer), 20),
            new MonsterTownSpawnEntry(typeof(VortexConstruct), 20)
        };

        // Satyr encounter:
        public static MonsterTownSpawnEntry[] InvasionSatyr = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Satyr), 150),
            new MonsterTownSpawnEntry(typeof(ArcaneSatyr), 20),
            new MonsterTownSpawnEntry(typeof(CelestialSatyr), 20),
            new MonsterTownSpawnEntry(typeof(EnigmaticSatyr), 20),
            new MonsterTownSpawnEntry(typeof(FrenziedSatyr), 20),
            new MonsterTownSpawnEntry(typeof(GentleSatyr), 20),
            new MonsterTownSpawnEntry(typeof(MelodicSatyr), 20),
            new MonsterTownSpawnEntry(typeof(MysticSatyr), 20),
            new MonsterTownSpawnEntry(typeof(RhythmicSatyr), 20),
            new MonsterTownSpawnEntry(typeof(TempestSatyr), 20),
            new MonsterTownSpawnEntry(typeof(WickedSatyr), 20)
        };

        // Sheep encounter:
        public static MonsterTownSpawnEntry[] InvasionSheep = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Sheep), 150),
            new MonsterTownSpawnEntry(typeof(BubblegumBlaster), 20),
            new MonsterTownSpawnEntry(typeof(CandyCornCreep), 20),
            new MonsterTownSpawnEntry(typeof(CaramelConjurer), 20),
            new MonsterTownSpawnEntry(typeof(ChocolateTruffle), 20),
            new MonsterTownSpawnEntry(typeof(GummySheep), 20),
            new MonsterTownSpawnEntry(typeof(JellybeanJester), 20),
            new MonsterTownSpawnEntry(typeof(LicoriceSheep), 20),
            new MonsterTownSpawnEntry(typeof(LollipopLord), 20),
            new MonsterTownSpawnEntry(typeof(PeppermintPuff), 20),
            new MonsterTownSpawnEntry(typeof(TaffyTitan), 20)
        };

        // Skeleton Knight encounter:
        public static MonsterTownSpawnEntry[] InvasionSkeletonKnight = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(SkeletalKnight), 150),
            new MonsterTownSpawnEntry(typeof(ArcaneSentinel), 20),
            new MonsterTownSpawnEntry(typeof(FlameborneKnight), 20),
            new MonsterTownSpawnEntry(typeof(FrostboundChampion), 20),
            new MonsterTownSpawnEntry(typeof(GraveKnight), 20),
            new MonsterTownSpawnEntry(typeof(IroncladDefender), 20),
            new MonsterTownSpawnEntry(typeof(NecroticGeneral), 20),
            new MonsterTownSpawnEntry(typeof(ShadowbladeAssassin), 20),
            new MonsterTownSpawnEntry(typeof(SpectralWarden), 20),
            new MonsterTownSpawnEntry(typeof(StormBone), 20),
            new MonsterTownSpawnEntry(typeof(VampiricBlade), 20)
        };

        // Slimes encounter:
        public static MonsterTownSpawnEntry[] InvasionSlimes = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Slime), 150),
            new MonsterTownSpawnEntry(typeof(AcidicSlime), 20),
            new MonsterTownSpawnEntry(typeof(CrystalOoze), 20),
            new MonsterTownSpawnEntry(typeof(ElectricSlime), 20),
            new MonsterTownSpawnEntry(typeof(FrozenOoze), 20),
            new MonsterTownSpawnEntry(typeof(GlisteningOoze), 20),
            new MonsterTownSpawnEntry(typeof(MoltenSlime), 20),
            new MonsterTownSpawnEntry(typeof(RadiantSlime), 20),
            new MonsterTownSpawnEntry(typeof(ShadowSludge), 20),
            new MonsterTownSpawnEntry(typeof(ToxicSludge), 20),
            new MonsterTownSpawnEntry(typeof(VoidSlime), 20)
        };

        // Squirrel encounter:
        public static MonsterTownSpawnEntry[] InvasionSquirrel = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(Squirrel), 150),
            new MonsterTownSpawnEntry(typeof(AlbertsSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(BeldingsGroundSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(DouglasSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(EasternGraySquirrel), 20),
            new MonsterTownSpawnEntry(typeof(FlyingSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(FoxSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(IndianPalmSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(RedSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(RedTailedSquirrel), 20),
            new MonsterTownSpawnEntry(typeof(RockSquirrel), 20)
        };

        // Toads encounter:
        public static MonsterTownSpawnEntry[] InvasionToads = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(GiantToad), 150),
            new MonsterTownSpawnEntry(typeof(BlightedToad), 20),
            new MonsterTownSpawnEntry(typeof(CorrosiveToad), 20),
            new MonsterTownSpawnEntry(typeof(CursedToad), 20),
            new MonsterTownSpawnEntry(typeof(EldritchToad), 20),
            new MonsterTownSpawnEntry(typeof(FungalToad), 20),
            new MonsterTownSpawnEntry(typeof(InfernalToad), 20),
            new MonsterTownSpawnEntry(typeof(ShadowToad), 20),
            new MonsterTownSpawnEntry(typeof(SpectralToad), 20),
            new MonsterTownSpawnEntry(typeof(VenomousToad), 20),
            new MonsterTownSpawnEntry(typeof(VileToad), 20)
        };

        // Water Elementals encounter:
        public static MonsterTownSpawnEntry[] InvasionWaterElementals = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(WaterElemental), 150),
            new MonsterTownSpawnEntry(typeof(AbyssalTide), 20),
            new MonsterTownSpawnEntry(typeof(AzureMirage), 20),
            new MonsterTownSpawnEntry(typeof(CoralSentinel), 20),
            new MonsterTownSpawnEntry(typeof(FrostWarden), 20),
            new MonsterTownSpawnEntry(typeof(HydrokineticWarden), 20),
            new MonsterTownSpawnEntry(typeof(MireSpawner), 20),
            new MonsterTownSpawnEntry(typeof(SteamLeviathan), 20),
            new MonsterTownSpawnEntry(typeof(Stormcaller), 20),
            new MonsterTownSpawnEntry(typeof(TsunamiTitan), 20),
            new MonsterTownSpawnEntry(typeof(VortexWraith), 20)
        };

        // Wolf encounter:
        public static MonsterTownSpawnEntry[] InvasionWolf = new MonsterTownSpawnEntry[]
        {
            new MonsterTownSpawnEntry(typeof(DireWolf), 150),
            new MonsterTownSpawnEntry(typeof(AncientWolf), 20),
            new MonsterTownSpawnEntry(typeof(CelestialWolf), 20),
            new MonsterTownSpawnEntry(typeof(CursedWolf), 20),
            new MonsterTownSpawnEntry(typeof(EarthquakeWolf), 20),
            new MonsterTownSpawnEntry(typeof(EmberWolf), 20),
            new MonsterTownSpawnEntry(typeof(FrostbiteWolf), 20),
            new MonsterTownSpawnEntry(typeof(GloomWolf), 20),
            new MonsterTownSpawnEntry(typeof(ShadowProwler), 20),
            new MonsterTownSpawnEntry(typeof(StormWolf), 20),
            new MonsterTownSpawnEntry(typeof(VenomousWolf), 20)
        };

        #endregion

        #region Fields, Properties and Constructor

        private Type m_Monster;
        private int m_Amount;

        public Type Monster { get { return m_Monster; } set { m_Monster = value; } }
        public int Amount { get { return m_Amount; } set { m_Amount = value; } }

        public MonsterTownSpawnEntry(Type monster, int amount)
        {
            m_Monster = monster;
            m_Amount = amount;
        }

        #endregion
    }
}
