using System;
using Server.Mobiles;

namespace Server.Engines.CannedEvil
{
    public enum ChampionSpawnType
    {
        Abyss,
        Arachnid,
        ColdBlood,
        ForestLord,
        VerminHorde,
        UnholyTerror,
        SleepingDragon,
        Glade,
        Corrupt,
        #region Stygian Abyss
        Terror,
        Infuse,
        #endregion
        #region TOL
        DragonTurtle,
        #endregion
        Khaldun,
        UltimateMasterAlchemist,
        UltimateMasterAnatomy,
        UltimateMasterAnimalLore,
        UltimateMasterAnimalTamer,
        UltimateMasterArcher,
        UltimateMasterArmsLore,
        UltimateMasterBeggar,
        UltimateMasterBlacksmith,
        UltimateMasterBushido,
        UltimateMasterCamper,
        UltimateMasterCarpenter,
        UltimateMasterCartographer,
        UltimateMasterChivalry,
        UltimateMasterChef,
        UltimateMasterDetectingHidden,
        UltimateMasterDiscordance,
        UltimateMasterIntelligence,
        UltimateMasterFencer,
        UltimateMasterFishing,
        UltimateMasterBowcraft,
        UltimateMasterFocus,
        UltimateMasterForensicEvaluator,
        UltimateMasterHealer,
        UltimateMasterHerder,
        UltimateMasterHider,
        UltimateMasterImbuing,
        UltimateMasterInscription,
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
        UltimateMasterThief,
        UltimateMasterStealth,
        UltimateMasterSwordsman,
        UltimateMasterTactician,
        UltimateMasterTailor,
        UltimateMasterTasteIdentification,
        UltimateMasterThrowing,
        UltimateMasterTinkering,
        UltimateMasterTracker,
        UltimateMasterVeterinary,
        UltimateMasterWrestling    }

    public class ChampionSpawnInfo
    {
        private readonly string m_Name;
        private readonly Type m_Champion;
        private readonly Type[][] m_SpawnTypes;
        private readonly string[] m_LevelNames;

        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public Type Champion
        {
            get
            {
                return m_Champion;
            }
        }
        public Type[][] SpawnTypes
        {
            get
            {
                return m_SpawnTypes;
            }
        }
        public string[] LevelNames
        {
            get
            {
                return m_LevelNames;
            }
        }

        public ChampionSpawnInfo(string name, Type champion, string[] levelNames, Type[][] spawnTypes)
        {
            m_Name = name;
            m_Champion = champion;
            m_LevelNames = levelNames;
            m_SpawnTypes = spawnTypes;
        }

        public static ChampionSpawnInfo[] Table
        {
            get
            {
                return m_Table;
            }
        }

        private static readonly ChampionSpawnInfo[] m_Table = new ChampionSpawnInfo[]
        {
            new ChampionSpawnInfo("Abyss", typeof(Semidar), new string[] { "Foe", "Assassin", "Conqueror" }, new Type[][]	// Abyss
            { // Abyss
                new Type[] { typeof(GreaterMongbat), typeof(Imp) }, // Level 1
                new Type[] { typeof(Gargoyle), typeof(Harpy) }, // Level 2
                new Type[] { typeof(FireGargoyle), typeof(StoneGargoyle) }, // Level 3
                new Type[] { typeof(Daemon), typeof(Succubus) }// Level 4
            }),
            new ChampionSpawnInfo("Arachnid", typeof(Mephitis), new string[] { "Bane", "Killer", "Vanquisher" }, new Type[][]	// Arachnid
            { // Arachnid
                new Type[] { typeof(Scorpion), typeof(GiantSpider) }, // Level 1
                new Type[] { typeof(TerathanDrone), typeof(TerathanWarrior) }, // Level 2
                new Type[] { typeof(DreadSpider), typeof(TerathanMatriarch) }, // Level 3
                new Type[] { typeof(PoisonElemental), typeof(TerathanAvenger) }// Level 4
            }),
            new ChampionSpawnInfo("Cold Blood", typeof(Rikktor), new string[] { "Blight", "Slayer", "Destroyer" }, new Type[][]	// Cold Blood
            { // Cold Blood
                new Type[] { typeof(Lizardman), typeof(Snake) }, // Level 1
                new Type[] { typeof(LavaLizard), typeof(OphidianWarrior) }, // Level 2
                new Type[] { typeof(Drake), typeof(OphidianArchmage) }, // Level 3
                new Type[] { typeof(Dragon), typeof(OphidianKnight) }// Level 4
            }),
            new ChampionSpawnInfo("Forest Lord", typeof(LordOaks), new string[] { "Enemy", "Curse", "Slaughterer" }, new Type[][]	// Forest Lord
            { // Forest Lord
                new Type[] { typeof(Pixie), typeof(ShadowWisp) }, // Level 1
                new Type[] { typeof(Kirin), typeof(Wisp) }, // Level 2
                new Type[] { typeof(Centaur), typeof(Unicorn) }, // Level 3
                new Type[] { typeof(EtherealWarrior), typeof(SerpentineDragon) }// Level 4
            }),
            new ChampionSpawnInfo("Vermin Horde", typeof(Barracoon), new string[] { "Adversary", "Subjugator", "Eradicator" }, new Type[][]	// Vermin Horde
            { // Vermin Horde
                new Type[] { typeof(GiantRat), typeof(Slime) }, // Level 1
                new Type[] { typeof(DireWolf), typeof(Ratman) }, // Level 2
                new Type[] { typeof(HellHound), typeof(RatmanMage) }, // Level 3
                new Type[] { typeof(RatmanArcher), typeof(SilverSerpent) }// Level 4
            }),
            new ChampionSpawnInfo("Unholy Terror", typeof(Neira), new string[] { "Scourge", "Punisher", "Nemesis" }, new Type[][]	// Unholy Terror
            { // Unholy Terror
                (Core.AOS ? new Type[] { typeof(Bogle), typeof(Ghoul), typeof(Shade), typeof(Spectre), typeof(Wraith) }// Level 1 (Pre-AoS)
                 : new Type[] { typeof(Ghoul), typeof(Shade), typeof(Spectre), typeof(Wraith) }), // Level 1

                new Type[] { typeof(BoneMagi), typeof(Mummy), typeof(SkeletalMage) }, // Level 2
                new Type[] { typeof(BoneKnight), typeof(Lich), typeof(SkeletalKnight) }, // Level 3
                new Type[] { typeof(LichLord), typeof(RottingCorpse) }// Level 4
            }),
            new ChampionSpawnInfo("Sleeping Dragon", typeof(Serado), new string[] { "Rival", "Challenger", "Antagonist" }, new Type[][]
            { // Unholy Terror
                new Type[] { typeof(DeathwatchBeetleHatchling), typeof(Lizardman) },
                new Type[] { typeof(DeathwatchBeetle), typeof(Kappa) },
                new Type[] { typeof(LesserHiryu), typeof(RevenantLion) },
                new Type[] { typeof(Hiryu), typeof(Oni) }
            }),
            new ChampionSpawnInfo("Glade", typeof(Twaulo), new string[] { "Banisher", "Enforcer", "Eradicator" }, new Type[][]
            { // Glade
                new Type[] { typeof(Pixie), typeof(ShadowWisp) },
                new Type[] { typeof(Centaur), typeof(MLDryad) },
                new Type[] { typeof(Satyr), typeof(CuSidhe) },
                new Type[] { typeof(FeralTreefellow), typeof(RagingGrizzlyBear) }
            }),
            new ChampionSpawnInfo("Corrupt", typeof(Ilhenir), new string[] { "Cleanser", "Expunger", "Depurator" }, new Type[][]
            { // Corrupt
                new Type[] { typeof(PlagueSpawn), typeof(Bogling) },
                new Type[] { typeof(PlagueBeast), typeof(BogThing) },
                new Type[] { typeof(PlagueBeastLord), typeof(InterredGrizzle) },
                new Type[] { typeof(FetidEssence), typeof(PestilentBandage) }
            }),

            new ChampionSpawnInfo("Terror", typeof(AbyssalInfernal), new string[] { "Banisher", "Enforcer", "Eradicator" }, new Type[][]
            { // Terror
                new Type[] { typeof(HordeMinion), typeof(ChaosDaemon) }, // Level 1
                new Type[] { typeof(StoneHarpy), typeof(ArcaneDaemon) }, // Level 2
                new Type[] { typeof(PitFiend), typeof(Moloch) }, // Level 3
                new Type[] { typeof(ArchDaemon), typeof(AbyssalAbomination) }// Level 4
            }),
            new ChampionSpawnInfo("Infuse", typeof(PrimevalLich), new string[] { "Cleanser", "Expunger", "Depurator" }, new Type[][]
            { // Infused
                new Type[] { typeof(GoreFiend), typeof(VampireBat) }, // Level 1
                new Type[] { typeof(FleshGolem), typeof(DarkWisp) }, // Level 2
                new Type[] { typeof(UndeadGargoyle), typeof(Wight) }, // Level 3
                new Type[] { typeof(SkeletalDrake), typeof(DreamWraith) }// Level 4
            }),

            new ChampionSpawnInfo( "Valley", typeof( DragonTurtle ), new string[]{ "Explorer", "Huntsman", "Msafiri", } , new Type[][]
            {																											// DragonTurtle
				new Type[]{ typeof( MyrmidexDrone ), typeof( MyrmidexLarvae ) },										// Level 1
				new Type[]{ typeof( SilverbackGorilla ), typeof( WildTiger ) },											// Level 2
				new Type[]{ typeof( GreaterPhoenix  ), typeof( Infernus ) },										    // Level 3
				new Type[]{ typeof( Dimetrosaur ), typeof( Allosaurus ) }											    // Level 4
			} ),

            new ChampionSpawnInfo( "Khaldun", typeof( KhalAnkur ), new string[]{ "Banisher", "Enforcer", "Eradicator" } , new Type[][]
            {																					                        // KhalAnkur
				new Type[]{ typeof( SkelementalKnight ), typeof( KhaldunBlood ) },										// Level 1
				new Type[]{ typeof( SkelementalMage ), typeof( Viscera ) },											    // Level 2
				new Type[]{ typeof( CultistAmbusher  ), typeof( ShadowFiend ) },										// Level 3
				new Type[]{ typeof( KhalAnkurWarriors ) }											                    // Level 4
			} ),
			
			// New Ultimate Master Champion Spawns
            new ChampionSpawnInfo("Ultimate Master Alchemist", typeof(UltimateMasterAlchemist), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(EvilAlchemist), typeof(Lich) }, // Level 1
                new Type[] { typeof(WaterAlchemist), typeof(AquaticTamer) }, // Level 2
                new Type[] { typeof(EarthAlchemist), typeof(KarateExpert) }, // Level 3
                new Type[] { typeof(FireAlchemist), typeof(Herbalist) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Anatomy", typeof(UltimateMasterAnatomy), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Samurai), typeof(Assassin) }, // Level 1
                new Type[] { typeof(MuscleBrute), typeof(AsceticHermit) }, // Level 2
                new Type[] { typeof(NerveAgent), typeof(BattleDressmaker) }, // Level 3
                new Type[] { typeof(BoomerangThrower), typeof(BoneShielder) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master AnimalLore", typeof(UltimateMasterAnimalLore), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(CombatNurse), typeof(DancingFarmerB) }, // Level 1
                new Type[] { typeof(Cryptologist), typeof(ArcticNaturalist) }, // Level 2
                new Type[] { typeof(JungleNaturalist), typeof(DemolitionExpert) }, // Level 3
                new Type[] { typeof(DesertNaturalist), typeof(DesertTracker) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master AnimalTaming", typeof(UltimateMasterAnimalTamer), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(DancingFarmerC), typeof(Impaler) }, // Level 1
                new Type[] { typeof(RamRider), typeof(SerpentHandler) }, // Level 2
                new Type[] { typeof(BigCatTamer), typeof(Cat) }, // Level 3
                new Type[] { typeof(BirdTrainer), typeof(SheepdogHandler) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Archery", typeof(UltimateMasterArcher), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ForestRanger), typeof(ShadowWyrm) }, // Level 1
                new Type[] { typeof(ScoutArcher), typeof(TigersClawThief) }, // Level 2
                new Type[] { typeof(CrossbowMarksman), typeof(ForestMinstrel) }, // Level 3
                new Type[] { typeof(DarkElf), typeof(VeriteElemental) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master ArmsLore", typeof(UltimateMasterArmsLore), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(FireClanSamurai), typeof(SandVortex) }, // Level 1
                new Type[] { typeof(KatanaDuelist), typeof(StoneSlith) }, // Level 2
                new Type[] { typeof(SerpentsFangAssassin), typeof(SpearSentry) }, // Level 3
                new Type[] { typeof(ShieldBearer), typeof(SabertoothedTiger) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Begging", typeof(UltimateMasterBeggar), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(CryingOrphan), typeof(TsukiWolf) }, // Level 1
                new Type[] { typeof(PocketPicker), typeof(Dog) }, // Level 2
                new Type[] { typeof(SlyStoryteller), typeof(Brigand) }, // Level 3
                new Type[] { typeof(Pickpocket), typeof(Diplomat) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Blacksmith", typeof(UltimateMasterBlacksmith), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Daemon), typeof(LichLord) }, // Level 1
                new Type[] { typeof(FlameWelder), typeof(RestlessSoul) }, // Level 2
                new Type[] { typeof(AnvilHurler), typeof(Virulent) }, // Level 3
                new Type[] { typeof(FireMage), typeof(ArchDaemon) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Bushido", typeof(UltimateMasterBushido), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(FireClanSamurai), typeof(FieldCommander) }, // Level 1
                new Type[] { typeof(EarthClanSamurai), typeof(GrayGoblinKeeper) }, // Level 2
                new Type[] { typeof(WaterClanSamurai), typeof(SolenHelper) }, // Level 3
                new Type[] { typeof(AirClanSamurai), typeof(EvilMageLord) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Camping", typeof(UltimateMasterCamper), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Forager), typeof(ForgottenServant) }, // Level 1
                new Type[] { typeof(Firestarter), typeof(GargoyleGuardian) }, // Level 2
                new Type[] { typeof(GrayGoblin), typeof(FleshRenderer) }, // Level 3
                new Type[] { typeof(RatmanMage), typeof(Wyvern) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Carpentry", typeof(UltimateMasterCarpenter), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Joiner), typeof(GargoyleDestroyer) }, // Level 1
                new Type[] { typeof(Carver), typeof(DullCopperElemental) }, // Level 2
                new Type[] { typeof(SawmillWorker), typeof(Kraken) }, // Level 3
                new Type[] { typeof(CabinetMaker), typeof(Drake) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Cartography", typeof(UltimateMasterCartographer), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(TerrainScout), typeof(WolfSpider) }, // Level 1
                new Type[] { typeof(StarReader), typeof(ElfBrigand) }, // Level 2
                new Type[] { typeof(EvilMapMaker), typeof(WhiteWyrm) }, // Level 3
                new Type[] { typeof(RelicHunter), typeof(AncientLich) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Chivalry", typeof(UltimateMasterChivalry), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(GiantRat), typeof(ShadowIronElemental) }, // Level 1
                new Type[] { typeof(KnightOfValor), typeof(GrayGoblinMage) }, // Level 2
                new Type[] { typeof(KnightOfMercy), typeof(Imp) }, // Level 3
                new Type[] { typeof(KnightOfJustice), typeof(BloodWorm) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Cooking", typeof(UltimateMasterChef), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(EnslavedGoblinScout), typeof(EnslavedGoblinMage) }, // Level 1
                new Type[] { typeof(GrillMaster), typeof(EnslavedGrayGoblin) }, // Level 2
                new Type[] { typeof(PastryChef), typeof(Rotworm) }, // Level 3
                new Type[] { typeof(SousChef), typeof(ToxicologistChef) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master DetectHidden", typeof(UltimateMasterDetectingHidden), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(EnslavedGoblinKeeper), typeof(EnslavedGreenGoblin) }, // Level 1
                new Type[] { typeof(ClueSeeker), typeof(EnslavedGreenGoblinAlchemist) }, // Level 2
                new Type[] { typeof(DecoyDeployer), typeof(OrcishMage) }, // Level 3
                new Type[] { typeof(ForensicAnalyst), typeof(OrcScout) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Discordance", typeof(UltimateMasterDiscordance), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ChoirSinger), typeof(ColdDrake) }, // Level 1
                new Type[] { typeof(Drummer), typeof(Kappa) }, // Level 2
                new Type[] { typeof(Harpist), typeof(Dragon) }, // Level 3
                new Type[] { typeof(FrostDragon), typeof(TrapdoorSpider) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master EvalInt", typeof(UltimateMasterIntelligence), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(QuantumPhysicist), typeof(PackLlama) }, // Level 1
                new Type[] { typeof(Relativist), typeof(Enchanter) }, // Level 2
                new Type[] { typeof(Logician), typeof(JukaWarrior) }, // Level 3
                new Type[] { typeof(AncientWyrm), typeof(Juggernaut) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Fencing", typeof(UltimateMasterFencer), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(SabreFighter), typeof(Savage) }, // Level 1
                new Type[] { typeof(EpeeSpecialist), typeof(Sheep) }, // Level 2
                new Type[] { typeof(RapierDuelist), typeof(Cow) }, // Level 3
                new Type[] { typeof(VorpalBunny), typeof(Kepetch) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Fishing", typeof(UltimateMasterFishing), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(NetCaster), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(SpearFisher), typeof(Dog) }, // Level 2
                new Type[] { typeof(GiantSpider), typeof(Cat) }, // Level 3
                new Type[] { typeof(GiantSpider), typeof(Slime) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Fletching", typeof(UltimateMasterBowcraft), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(PutridUndeadGargoyle), typeof(RaiJu) }, // Level 1
                new Type[] { typeof(ArrowFletcher), typeof(JukaLord) }, // Level 2
                new Type[] { typeof(TrickShotArtist), typeof(SavageRider) }, // Level 3
                new Type[] { typeof(LongbowSniper), typeof(SerpentineDragon) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Focus", typeof(UltimateMasterFocus), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(YomotsuWarrior), typeof(BogThing) }, // Level 1
                new Type[] { typeof(TaekwondoMaster), typeof(RuneBeetle) }, // Level 2
                new Type[] { typeof(QiGongHealer), typeof(Satyr) }, // Level 3
                new Type[] { typeof(OrcishLord), typeof(InterredGrizzle ) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Forensics", typeof(UltimateMasterForensicEvaluator), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(EvidenceAnalyst), typeof(RuddyBoura) }, // Level 1
                new Type[] { typeof(Pathologist), typeof(KhaldunSummoner) }, // Level 2
                new Type[] { typeof(CrimeSceneTech), typeof(Revenant) }, // Level 3
                new Type[] { typeof(GiantSpider), typeof(GreenGoblin) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Healing", typeof(UltimateMasterHealer), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(FieldMedic), typeof(PlagueSpawn) }, // Level 1
                new Type[] { typeof(Raptor), typeof(Ronin) }, // Level 2
                new Type[] { typeof(GreenGoblinAlchemist), typeof(YomotsuPriest) }, // Level 3
                new Type[] { typeof(LowlandBoura), typeof(OrcCaptain) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Herding", typeof(UltimateMasterHerder), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(WoolWeaver), typeof(HighPlainsBoura) }, // Level 1
                new Type[] { typeof(OrcChopper), typeof(Orc) }, // Level 2
                new Type[] { typeof(GreaterDragon), typeof(ClockworkScorpion) }, // Level 3
                new Type[] { typeof(ValoriteElemental), typeof(TanglingRoots) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Hiding", typeof(UltimateMasterHider), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Illusionist), typeof(KhaldunZealot) }, // Level 1
                new Type[] { typeof(Contortionist), typeof(Nightmare) }, // Level 2
                new Type[] { typeof(Magician), typeof(DarknightCreeper) }, // Level 3
                new Type[] { typeof(InvisibleSaboteur), typeof(CrystalHydra) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Imbuing", typeof(UltimateMasterImbuing), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(WeaponEnchanter), typeof(CrystalLatticeSeeker) }, // Level 1
                new Type[] { typeof(ArmorCurer), typeof(FireBeetle) }, // Level 2
                new Type[] { typeof(RuneCaster), typeof(EliteNinja) }, // Level 3
                new Type[] { typeof(MinionOfScelestus), typeof(Betrayer) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Inscribe", typeof(UltimateMasterInscription), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ArcaneScribe), typeof(OrcBrute) }, // Level 1
                new Type[] { typeof(LibrarianCustodian), typeof(GargishOutcast) }, // Level 2
                new Type[] { typeof(ScrollMage), typeof(Kirin) }, // Level 3
                new Type[] { typeof(OrcBomber), typeof(CrimsonDrake) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master ItemID", typeof(UltimateMasterItemIdentification), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Explorer), typeof(Unicorn) }, // Level 1
                new Type[] { typeof(CuSidhe), typeof(MeerCaptain) }, // Level 2
                new Type[] { typeof(KhaldunRevenant), typeof(RedSolenWarrior) }, // Level 3
                new Type[] { typeof(BlackSolenWarrior), typeof(PlatinumDrake) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Lockpicking", typeof(UltimateMasterLockpicker), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Spy), typeof(EtherealWarrior) }, // Level 1
                new Type[] { typeof(SafeCracker), typeof(Beetle) }, // Level 2
                new Type[] { typeof(Saboteur), typeof(WildTiger) }, // Level 3
                new Type[] { typeof(Yamandon), typeof(KepetchAmbusher) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Lumberjacking", typeof(UltimateMasterLumberjack), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(TreeFeller), typeof(InsaneDryad) }, // Level 1
                new Type[] { typeof(ForestScout), typeof(Dog) }, // Level 2
                new Type[] { typeof(PackHorse), typeof(ExodusOverseer) }, // Level 3
                new Type[] { typeof(Hiryu), typeof(Golem) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Macing", typeof(UltimateMasterMaceFighting), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(LeatherWolf), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(LightningBearer), typeof(JukaMage) }, // Level 2
                new Type[] { typeof(StormConjurer), typeof(LesserHiryu) }, // Level 3
                new Type[] { typeof(GreaterPoisonElemental), typeof(HammerGuard) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Magery", typeof(UltimateMasterMage), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(IceSorcerer), typeof(AntLion) }, // Level 1
                new Type[] { typeof(SavageShaman), typeof(FanDancer) }, // Level 2
                new Type[] { typeof(GargishRouser), typeof(ChaosDragoon) }, // Level 3
                new Type[] { typeof(BakeKitsune), typeof(ExodusMinion) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master MagicResist", typeof(UltimateMasterMagicResistance), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(RuneKeeper), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(Spellbreaker), typeof(ChaosDragoonElite) }, // Level 2
                new Type[] { typeof(StoneMonster), typeof(MeerEternal) }, // Level 3
                new Type[] { typeof(MeerMage), typeof(PlagueBeast) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Meditation", typeof(UltimateMasterMeditation), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(GiantRat), typeof(Changeling) }, // Level 1
                new Type[] { typeof(ZenMonk), typeof(Dog) }, // Level 2
                new Type[] { typeof(MartialMonk), typeof(RedSolenQueen) }, // Level 3
                new Type[] { typeof(WardCaster), typeof(BlackSolenQueen) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Mining", typeof(UltimateMasterMiner), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(GiantRat), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(ExplosiveDemolitionist), typeof(Dog) }, // Level 2
                new Type[] { typeof(GemCutter), typeof(IronBeetle) }, // Level 3
                new Type[] { typeof(DeepMiner), typeof(SwampDragon) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Musicianship", typeof(UltimateMasterMusician), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(DrumBoy), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(Violinist), typeof(PlagueBeastLord) }, // Level 2
                new Type[] { typeof(SatyrPiper), typeof(DemonKnight) }, // Level 3
                new Type[] { typeof(Flutist), typeof(GiantSerpent) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Mysticism", typeof(UltimateMasterMysticism), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Astrologer), typeof(FastExplorer) }, // Level 1
                new Type[] { typeof(MagmaElemental), typeof(HolyKnight) }, // Level 2
                new Type[] { typeof(Oracle), typeof(AppleElemental) }, // Level 3
                new Type[] { typeof(Diviner), typeof(Slime) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Necromancy", typeof(UltimateMasterNecromancer), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(GraveDigger), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(GhostScout), typeof(InfernoDragon) }, // Level 2
                new Type[] { typeof(NecroSummoner), typeof(PoisonAppleTree) }, // Level 3
                new Type[] { typeof(PitFiend), typeof(LineDragon) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Ninjitsu", typeof(UltimateMasterNinjitsu), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Kunoichi), typeof(Rabbit) }, // Level 1
                new Type[] { typeof(ScoutNinja), typeof(Dog) }, // Level 2
                new Type[] { typeof(AirClanNinja), typeof(WaterClanNinja) }, // Level 3
                new Type[] { typeof(FireClanNinja), typeof(EarthClanNinja) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Parry", typeof(UltimateMasterParrying), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Banneret), typeof(MotownMermaid) }, // Level 1
                new Type[] { typeof(SneakyNinja), typeof(MushroomWitch) }, // Level 2
                new Type[] { typeof(MedievalMeteorologist), typeof(PigFarmer) }, // Level 3
                new Type[] { typeof(FairyQueen), typeof(HippieHoplite) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Peacemaking", typeof(UltimateMasterPeacemaker), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Protester), typeof(NinjaLibrarian) }, // Level 1
                new Type[] { typeof(PirateOfTheStars), typeof(GangLeader) }, // Level 2
                new Type[] { typeof(JavelinAthlete), typeof(RetroFuturist) }, // Level 3
                new Type[] { typeof(FunkFungiFamiliar), typeof(IceKing) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Poisoning", typeof(UltimateMasterPoisoner), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Chemist), typeof(FloridaMan) }, // Level 1
                new Type[] { typeof(GreenHag), typeof(HerbalistPoisoner) }, // Level 2
                new Type[] { typeof(PhoenixStyleMaster), typeof(VenomousAssassin) }, // Level 3
                new Type[] { typeof(DancingFarmerC), typeof(ForestRanger) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Provocation", typeof(UltimateMasterProvocation), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(NymphSinger), typeof(FlapperElementalist) }, // Level 1
                new Type[] { typeof(NoirDetective), typeof(RetroAndroid) }, // Level 2
                new Type[] { typeof(SkaSkald), typeof(PulpyPotionPurveyor) }, // Level 3
                new Type[] { typeof(InsaneRoboticist), typeof(OgreMaster) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master RemoveTrap", typeof(UltimateMasterRemoveTrap), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(EscapeArtist), typeof(WaterClanSamurai) }, // Level 1
                new Type[] { typeof(TrapSetter), typeof(GreenNinja) }, // Level 2
                new Type[] { typeof(TrapEngineer), typeof(MegaDragon) }, // Level 3
                new Type[] { typeof(TrapMaker), typeof(RedQueen) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Snooping", typeof(UltimateMasterSnooping), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(SkeletonLord), typeof(MischievousWitch) }, // Level 1
                new Type[] { typeof(Infiltrator), typeof(CelestialSamurai) }, // Level 2
                new Type[] { typeof(DisguiseMaster), typeof(EvilClown) }, // Level 3
                new Type[] { typeof(JazzAgeJuggernaut), typeof(Jester) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Spellweaving", typeof(UltimateMasterSpellweaver), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ElementalWizard), typeof(RenaissanceMechanic) }, // Level 1
                new Type[] { typeof(DarkSorcerer), typeof(EarthClanSamurai) }, // Level 2
                new Type[] { typeof(SilentMovieMonk), typeof(AirClanSamurai) }, // Level 3
                new Type[] { typeof(FairyDragon), typeof(FireClanNinja) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master SpiritSpeak", typeof(UltimateMasterSpiritSpeak), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(SpiritMedium), typeof(RanchMaster) }, // Level 1
                new Type[] { typeof(GhostWarrior), typeof(SlimeMage) }, // Level 2
                new Type[] { typeof(ShadowPriest), typeof(Musketeer) }, // Level 3
                new Type[] { typeof(DeathCultist), typeof(RaKing) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Stealing", typeof(UltimateMasterThief), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Locksmith), typeof(SwampThing) }, // Level 1
                new Type[] { typeof(ShadowLurker), typeof(WaterClanNinja) }, // Level 2
                new Type[] { typeof(ConArtist), typeof(AirClanNinja) }, // Level 3
                new Type[] { typeof(MasterPickpocket), typeof(RetroRobotRomancer) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Stealth", typeof(UltimateMasterStealth), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(PhantomAssassin), typeof(GraffitiGargoyle) }, // Level 1
                new Type[] { typeof(TexanRancher), typeof(Ringmaster) }, // Level 2
                new Type[] { typeof(SurferSummoner), typeof(EarthClanNinja) }, // Level 3
                new Type[] { typeof(StarfleetCaptain), typeof(BluesSingingGorgon) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Swords", typeof(UltimateMasterSwordsman), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(FireClanSamurai), typeof(GothicNovelist) }, // Level 1
                new Type[] { typeof(DualWielder), typeof(DancingFarmerB) }, // Level 2
                new Type[] { typeof(FencingMaster), typeof(ChrisRoberts) }, // Level 3
                new Type[] { typeof(SwordDefender), typeof(DarkLord) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Tactics", typeof(UltimateMasterTactician), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ScoutLeader), typeof(AstralTraveler) }, // Level 1
                new Type[] { typeof(Strategist), typeof(CorporateExecutive) }, // Level 2
                new Type[] { typeof(Stormtrooper2), typeof(NeoVictorianVampire) }, // Level 3
                new Type[] { typeof(DogtheBountyHunter), typeof(RapRanger) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Tailoring", typeof(UltimateMasterTailor), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(CountryCowgirlCyclops), typeof(Sith) }, // Level 1
                new Type[] { typeof(BattleWeaver), typeof(Biomancer) }, // Level 2
                new Type[] { typeof(PatchworkMonster), typeof(DarkElf) }, // Level 3
                new Type[] { typeof(BeetleJuiceSummoner), typeof(PinupPandemonium) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master TasteID", typeof(UltimateMasterTasteIdentification), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(VaudevilleValkyrie), typeof(RockabillyRevenant) }, // Level 1
                new Type[] { typeof(GourmetChef), typeof(RaveRogue) }, // Level 2
                new Type[] { typeof(FeastMaster), typeof(TwistedCultist) }, // Level 3
                new Type[] { typeof(HostilePrincess), typeof(PsychedelicShaman) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Throwing", typeof(UltimateMasterThrowing), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(KnifeThrower), typeof(StarCitizen) }, // Level 1
                new Type[] { typeof(SteampunkSamurai), typeof(SwinginSorceress) }, // Level 2
                new Type[] { typeof(GreaserGryphonRider), typeof(CavemanScientist) }, // Level 3
                new Type[] { typeof(CabaretKrakenGirl), typeof(StarfleetOfficer) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Tinkering", typeof(UltimateMasterTinkering), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(Electrician), typeof(GlamRockMage) }, // Level 1
                new Type[] { typeof(ReggaeRunesmith), typeof(BmovieBeastmaster) }, // Level 2
                new Type[] { typeof(PunkRockPaladin), typeof(CyberpunkSorcerer) }, // Level 3
                new Type[] { typeof(DuelistPoet), typeof(WildWestWizard) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Tracking", typeof(UltimateMasterTracker), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(ForestTracker), typeof(AlienWarrior) }, // Level 1
                new Type[] { typeof(UrbanTracker), typeof(Lawyer) }, // Level 2
                new Type[] { typeof(AvatarOfElements), typeof(BountyHunter) }, // Level 3
                new Type[] { typeof(EvilAlchemist), typeof(DinoRider) } // Level 4
            }),
			new ChampionSpawnInfo("Ultimate Master Veterinary", typeof(UltimateMasterVeterinary), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(CombatMedic), typeof(HostileDruid) }, // Level 1
                new Type[] { typeof(Biologist), typeof(Cannibal) }, // Level 2
                new Type[] { typeof(Beastmaster), typeof(BaroqueBarbarian) }, // Level 3
                new Type[] { typeof(Cowboy), typeof(DiscoDruid) } // Level 4
            }),
            new ChampionSpawnInfo("Ultimate Master Wrestling", typeof(UltimateMasterWrestling), new string[] { "Apprentice", "Journeyman", "Expert", "Master" }, new Type[][]
            {
                new Type[] { typeof(AirElemental), typeof(PKMurderer) }, // Level 1
                new Type[] { typeof(SumoWrestler), typeof(Assassin) }, // Level 2
                new Type[] { typeof(Luchador), typeof(PKMurdererLord) }, // Level 3
                new Type[] { typeof(GrecoRomanWrestler), typeof(WastelandBiker) } // Level 4
            }),
			        };

        public static ChampionSpawnInfo GetInfo(ChampionSpawnType type)
        {
            int v = (int)type;

            if (v < 0 || v >= m_Table.Length)
                v = 0;

            return m_Table[v];
        }
    }
}
