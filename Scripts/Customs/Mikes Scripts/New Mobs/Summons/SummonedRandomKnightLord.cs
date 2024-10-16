using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of a unique knight or lord")]
    public class SummonedRandomKnightLord : BaseCreature
    {
        private static readonly int[] knightLordBodies = new int[] { 0x3DF, 0x3E2, 0x3DE, 0x306, 0xF7, 0x2FC, 0x303, 0x305, 0x2FE }; // Knight and lord body values
        private static readonly string[] wordBank1 = new string[]
        {
            "Sir", "Lord", "Lady", "Baron", "Count",
            "Duke", "Earl", "Marquess", "Viscount", "Dame",
            "King", "Queen", "Prince", "Princess", "Knight",
            "Noble", "Valiant", "Brave", "Gallant", "Heroic",
			"Despot", "Tyrant", "Usurper", "Oppressor", "Autocrat",
			"Monarch", "Sovereign", "Emperor", "Empress", "Regent",
			"Paladin", "Cavalier", "Chevalier", "Vassal", "Liege",
			"Baroness", "Countess", "Duchess", "Marchioness", "Squire",
			"Sultan", "Sultana", "Caliph", "Calipha", "Pharaoh", "Pharaohness",
			"Tsar", "Tsarina", "Khan", "Khatun", "Shah", "Shahbanu",
			"Maharaja", "Maharani", "Raja", "Rani", "Emir", "Emira",
			"Sheikh", "Sheikha", "Chieftain", "Chieftess", "Daimyo", "Daimyōjo",
			"Shogun", "Shoguna", "Kaiser", "Kaiserin", "Rey", "Reina",
			"Conde", "Condesa", "Infante", "Infanta", "Grandee", "Dona",
			"Don", "Señor", "Señora", "Czar", "Czarina", "Sardar", "Sardarni",
			"Bey", "Hanım", "Voivode", "Voivodina", "Hetman", "Hetmana",
			"Atabeg", "Atabegah", "Mirza", "Mirzaneh", "Nawab", "Nawab Begum",
			"Wali", "Walida", "Amir", "Amira", "Sardar", "Sardarni",
			"Laird", "Lady", "Thane", "Thaness", "Jarl", "Jarla",
			"Dauphin", "Dauphine", "Doge", "Dogaressa", "Elector", "Electress",
			"Landgrave", "Landgravine", "Margrave", "Margravine", "Burgrave", "Burgravine"

        };

        private static readonly string[] wordBank2 = new string[]
        {
            "of the Round Table", "of the North", "of the South", "of the East", "of the West",
            "of the Realm", "of the Kingdom", "of the Land", "of the Castle", "of the Fortress",
            "of the Keep", "of the Manor", "of the Tower", "of the Citadel", "of the Duchy",
            "of the County", "of the Shire", "of the Fiefdom", "of the Barony", "of the Principality",
			"of the Abyss", "of the Void", "of the Shadow", "of the Darkness", "of the Night",
			"of the Blood", "of the Fire", "of the Ice", "of the Storm", "of Chaos",
			"of Doom", "of Death", "of Decay", "of Despair", "of Desolation",
			"of the Coven", "of the Grimoire", "of Runes", "of Sigils", "of Totems",
			"of the Necropolis", "of Catacombs", "of the Labyrinth", "of the Asylum", "of the Sanctum",
			"of the Abyss", "of the Void", "of the Shadow", "of the Darkness", "of the Underworld",
			"of Inferno", "of Purgatory", "of the Chaos", "of Doom", "of the Apocalypse",
			"of the Necropolis", "of the Coven", "of the Grimoire", "of the Runes", "of the Sigils",
			"of the Pentacle", "of the Ouija", "of the Tarot", "of the Crystal", "of the Amulet",
			"of the Talisman", "of the Relic", "of the Artifact", "of the Scroll", "of the Tome",
			"of the Oasis", "of the Desert", "of the Savannah", "of the Jungle", "of the Forest",
			"of the Mountain", "of the Valley", "of the River", "of the Lake", "of the Sea",
			"of the Island", "of the Peninsula", "of the Archipelago", "of the Plateau", "of the Canyon",
			"of the Volcano", "of the Glacier", "of the Tundra", "of the Taiga", "of the Steppe",
			"of the Delta", "of the Bay", "of the Gulf", "of the Strait", "of the Cape",
			"of the Cliff", "of the Shore", "of the Dune", "of the Cave", "of the Meadow",
			"of the Grove", "of the Orchard", "of the Vineyard", "of the Pasture", "of the Field",
			"of the Mine", "of the Quarry", "of the Mill", "of the Forge", "of the Foundry",
			"of the Farm", "of the Ranch", "of the Estate", "of the Plantation", "of the Colony",
			"of the Village", "of the Town", "of the City", "of the Metropolis", "of the Empire",
			"of the Federation", "of the Confederation", "of the Alliance", "of the Union", "of the Coalition",
			"of the League", "of the Guild", "of the Order", "of the Society", "of the Brotherhood",
			"of the Sisterhood", "of the Fellowship", "of the Cult", "of the Sect", "of the Tribe",
			"of the Clan", "of the House", "of the Dynasty", "of the Lineage", "of the Bloodline"
        };

        private static readonly AIType[] aiTypes = new AIType[]
        {
            AIType.AI_Melee,
            AIType.AI_Archer,
            AIType.AI_Healer,
            AIType.AI_Mage
        };

        private static readonly FightMode[] fightModes = new FightMode[]
        {
            FightMode.Closest,
            FightMode.Aggressor
        };

        private class Ability
        {
            public Type AbilityType { get; set; }
            public string StringArgument { get; set; }
            public int Value1 { get; set; }
            public int Value2 { get; set; }
            public bool TakesArguments { get; set; }

            public Ability(Type abilityType, int value1, int value2, bool takesArguments = true)
            {
                AbilityType = abilityType;
                StringArgument = null;
                Value1 = value1;
                Value2 = value2;
                TakesArguments = takesArguments;
            }

            public Ability(Type abilityType, string stringArgument)
            {
                AbilityType = abilityType;
                StringArgument = stringArgument;
                Value1 = 0;
                Value2 = 0;
                TakesArguments = true;
            }
        }

        private static readonly Ability[] abilities = new Ability[]
        {
            new Ability(typeof(XmlEarthquakeStrike), 20, 0),
            new Ability(typeof(XmlTheftStrike), 1, 0),
            new Ability(typeof(XmlFrostStrike), 25, 25),
            new Ability(typeof(XmlFTreeBreath), 0, 0, false),
            new Ability(typeof(XmlHeartBreath), 0, 0, false),
            new Ability(typeof(XmlNutcrackerBreath), 0, 0, false),
            new Ability(typeof(XmlDeerBreath), 0, 0, false),
            new Ability(typeof(XmlParaBreath), 0, 0, false),
            new Ability(typeof(XmlGateBreath), 0, 0, false),
            new Ability(typeof(XmlTrapBreath), 0, 0, false),
            new Ability(typeof(XmlOFlaskBreath), 0, 0, false),
            new Ability(typeof(XmlSkeletonBreath), 0, 0, false),
            new Ability(typeof(XmlCrankBreath), 0, 0, false),
            new Ability(typeof(XmlCurtainBreath), 0, 0, false),
            new Ability(typeof(XmlMaidenBreath), 0, 0, false),
            new Ability(typeof(XmlGuillotineBreath), 0, 0, false),
            new Ability(typeof(XmlStoneBreath), 0, 0, false),
            new Ability(typeof(XmlSawBreath), 0, 0, false),
            new Ability(typeof(XmlMushroomBreath), 0, 0, false),
            new Ability(typeof(XmlRuneBreath), 0, 0, false),
            new Ability(typeof(XmlBeeBreath), 0, 0, false),
            new Ability(typeof(XmlWaterBreath), 0, 0, false),
            new Ability(typeof(XmlAxeBreath), 0, 0, false),
            new Ability(typeof(XmlHeadBreath), 0, 0, false),
            new Ability(typeof(XmlSpikeBreath), 0, 0, false),
            new Ability(typeof(XmlGasBreath), 0, 0, false),
            new Ability(typeof(XmlTimeBreath), 0, 0, false),
            new Ability(typeof(XmlFlaskBreath), 0, 0, false),
            new Ability(typeof(XmlSkullBreath), 0, 0, false),
            new Ability(typeof(XmlDVortexBreath), 0, 0, false),
            new Ability(typeof(XmlGlowBreath), 0, 0, false),
            new Ability(typeof(XmlBladesBreath), 0, 0, false),
            new Ability(typeof(XmlVortexBreath), 0, 0, false),
            new Ability(typeof(XmlSparkleBreath), 0, 0, false),
            new Ability(typeof(XmlSmokeBreath), 0, 0, false),
            new Ability(typeof(XmlBoulderBreath), 0, 0, false),
            new Ability(typeof(XmlFTreeLine), 0, 0, false),
            new Ability(typeof(XmlHeartLine), 0, 0, false),
            new Ability(typeof(XmlNutcrackerLine), 0, 0, false),
            new Ability(typeof(XmlDeerLine), 0, 0, false),
            new Ability(typeof(XmlParaLine), 0, 0, false),
            new Ability(typeof(XmlGateLine), 0, 0, false),
            new Ability(typeof(XmlTrapLine), 0, 0, false),
            new Ability(typeof(XmlOFlaskLine), 0, 0, false),
            new Ability(typeof(XmlSkeletonLine), 0, 0, false),
            new Ability(typeof(XmlCrankLine), 0, 0, false),
            new Ability(typeof(XmlCurtainLine), 0, 0, false),
            new Ability(typeof(XmlMaidenLine), 0, 0, false),
            new Ability(typeof(XmlGuillotineLine), 0, 0, false),
            new Ability(typeof(XmlStoneLine), 0, 0, false),
            new Ability(typeof(XmlSawLine), 0, 0, false),
            new Ability(typeof(XmlMushroomLine), 0, 0, false),
            new Ability(typeof(XmlRuneLine), 0, 0, false),
            new Ability(typeof(XmlBeeLine), 0, 0, false),
            new Ability(typeof(XmlWaterLine), 0, 0, false),
            new Ability(typeof(XmlAxeLine), 0, 0, false),
            new Ability(typeof(XmlHeadLine), 0, 0, false),
            new Ability(typeof(XmlSpikeLine), 0, 0, false),
            new Ability(typeof(XmlGasLine), 0, 0, false),
            new Ability(typeof(XmlTimeLine), 0, 0, false),
            new Ability(typeof(XmlFlaskLine), 0, 0, false),
            new Ability(typeof(XmlSkullLine), 0, 0, false),
            new Ability(typeof(XmlDVortexLine), 0, 0, false),
            new Ability(typeof(XmlGlowLine), 0, 0, false),
            new Ability(typeof(XmlBladesLine), 0, 0, false),
            new Ability(typeof(XmlVortexLine), 0, 0, false),
            new Ability(typeof(XmlSparkleLine), 0, 0, false),
            new Ability(typeof(XmlSmokeLine), 0, 0, false),
            new Ability(typeof(XmlBoulderLine), 0, 0, false),
            new Ability(typeof(XmlFTreeCircle), 0, 0, false),
            new Ability(typeof(XmlHeartCircle), 0, 0, false),
            new Ability(typeof(XmlNutcrackerCircle), 0, 0, false),
            new Ability(typeof(XmlDeerCircle), 0, 0, false),
            new Ability(typeof(XmlParaCircle), 0, 0, false),
            new Ability(typeof(XmlGateCircle), 0, 0, false),
            new Ability(typeof(XmlTrapCircle), 0, 0, false),
            new Ability(typeof(XmlOFlaskCircle), 0, 0, false),
            new Ability(typeof(XmlSkeletonCircle), 0, 0, false),
            new Ability(typeof(XmlCrankCircle), 0, 0, false),
            new Ability(typeof(XmlCurtainCircle), 0, 0, false),
            new Ability(typeof(XmlMaidenCircle), 0, 0, false),
            new Ability(typeof(XmlGuillotineCircle), 0, 0, false),
            new Ability(typeof(XmlStoneCircle), 0, 0, false),
            new Ability(typeof(XmlSawCircle), 0, 0, false),
            new Ability(typeof(XmlMushroomCircle), 0, 0, false),
            new Ability(typeof(XmlRuneCircle), 0, 0, false),
            new Ability(typeof(XmlBeeCircle), 0, 0, false),
            new Ability(typeof(XmlWaterCircle), 0, 0, false),
            new Ability(typeof(XmlAxeCircle), 0, 0, false),
            new Ability(typeof(XmlHeadCircle), 0, 0, false),
            new Ability(typeof(XmlSpikeCircle), 0, 0, false),
            new Ability(typeof(XmlGasCircle), 0, 0, false),
            new Ability(typeof(XmlTimeCircle), 0, 0, false),
            new Ability(typeof(XmlFlaskCircle), 0, 0, false),
            new Ability(typeof(XmlSkullCircle), 0, 0, false),
            new Ability(typeof(XmlDVortexCircle), 0, 0, false),
            new Ability(typeof(XmlGlowCircle), 0, 0, false),
            new Ability(typeof(XmlBladesCircle), 0, 0, false),
            new Ability(typeof(XmlVortexCircle), 0, 0, false),
            new Ability(typeof(XmlSparkleCircle), 0, 0, false),
            new Ability(typeof(XmlSmokeCircle), 0, 0, false),
            new Ability(typeof(XmlBoulderCircle), 0, 0, false),
            new Ability(typeof(XmlPoisonAppleThrow), 0, 0, false),
            new Ability(typeof(XmlMagmaThrow), 0, 0, false),
            new Ability(typeof(XmlBreathAttack), 0, 0, false),
            new Ability(typeof(XmlCircleFireAttack), 0, 0, false),
            new Ability(typeof(XmlLineAttack), 0, 0, false),
            new Ability(typeof(XmlManaBurn), 0, 0, false),
            new Ability(typeof(XmlArcaneExplosion), 0, 0, false),
            new Ability(typeof(XmlSilenceStrike), 0, 0, false),
            new Ability(typeof(XmlEarthquakeStrike), 0, 0, false),
            new Ability(typeof(XmlPoisonCloud), 0, 0, false),
            new Ability(typeof(XmlFreezeStrike), 0, 0, false),
            new Ability(typeof(XmlBlazeStrike), 0, 0, false),
            new Ability(typeof(XmlWhirlwind), 0, 0, false),
            new Ability(typeof(XmlGrasp), 0, 0, false),
            new Ability(typeof(XmlBackstab), 0, 0, false),
            new Ability(typeof(XmlChillTouch), 0, 0, false),
            new Ability(typeof(XmlDevour), 0, 0, false),
            new Ability(typeof(XmlFlesheater), 0, 0, false),
            new Ability(typeof(XmlSting), 0, 0, false),
            new Ability(typeof(XmlEnrage), 0, 0, false),
            new Ability(typeof(XmlWeaken), 0, 0, false),
            new Ability(typeof(XmlFrenzy), 0, 0, false),
            new Ability(typeof(XmlWebCooldown), 0, 0, false),
            new Ability(typeof(XmlFire), 20, 0),
            new Ability(typeof(XmlLifeDrain), 20, 0),
            new Ability(typeof(XmlLightning), 20, 0),
            new Ability(typeof(XmlManaDrain), 20, 0),
            new Ability(typeof(XmlStamDrain), 20, 0),
            new Ability(typeof(XmlSmokeStrike), 20, 60),
            new Ability(typeof(XmlBlastStrike), 50, 30),
            new Ability(typeof(XmlBlazeStrike), 20, 30),		
            new Ability(typeof(XmlCursedTouch), 5, 5),
            new Ability(typeof(XmlMinionStrike), "AbysmalHorror"),
            new Ability(typeof(XmlMinionStrike), "AcidElemental"),
            new Ability(typeof(XmlMinionStrike), "AgapiteElemental"),
            new Ability(typeof(XmlMinionStrike), "AirElemental"),
            new Ability(typeof(XmlMinionStrike), "Alligator"),
            new Ability(typeof(XmlMinionStrike), "AncientLich"),
            new Ability(typeof(XmlMinionStrike), "AncientWyrm"),
            new Ability(typeof(XmlMinionStrike), "AntLion"),
            new Ability(typeof(XmlMinionStrike), "ArcaneDaemon"),
            new Ability(typeof(XmlMinionStrike), "ArcherGuard"),
            new Ability(typeof(XmlMinionStrike), "ArcticOgreLord"),
            new Ability(typeof(XmlMinionStrike), "BakeKitsune"),
            new Ability(typeof(XmlMinionStrike), "Balron"),
            new Ability(typeof(XmlMinionStrike), "Barracoon"),





            new Ability(typeof(XmlMinionStrike), "BaseShieldGuard"),
            new Ability(typeof(XmlMinionStrike), "BaseWarHorse"),
            new Ability(typeof(XmlMinionStrike), "Beetle"),
            new Ability(typeof(XmlMinionStrike), "Betrayer"),
            new Ability(typeof(XmlMinionStrike), "Bird"),
            new Ability(typeof(XmlMinionStrike), "BlackBear"),
            new Ability(typeof(XmlMinionStrike), "BlackDragoonPirate"),
            new Ability(typeof(XmlMinionStrike), "BlackSolenInfiltratorQueen"),
            new Ability(typeof(XmlMinionStrike), "BlackSolenInfiltratorWarrior"),
            new Ability(typeof(XmlMinionStrike), "BlackSolenQueen"),
            new Ability(typeof(XmlMinionStrike), "BlackSolenWarrior"),
            new Ability(typeof(XmlMinionStrike), "BlackSolenWorker"),
            new Ability(typeof(XmlMinionStrike), "BladeSpirits"),
            new Ability(typeof(XmlMinionStrike), "BloodElemental"),
            new Ability(typeof(XmlMinionStrike), "Boar"),
            new Ability(typeof(XmlMinionStrike), "BogThing"),
            new Ability(typeof(XmlMinionStrike), "Bogle"),
            new Ability(typeof(XmlMinionStrike), "Bogling"),
            new Ability(typeof(XmlMinionStrike), "BoneDemon"),
            new Ability(typeof(XmlMinionStrike), "BoneKnight"),
            new Ability(typeof(XmlMinionStrike), "BoneMagi"),
            new Ability(typeof(XmlMinionStrike), "Brigand"),
            new Ability(typeof(XmlMinionStrike), "BronzeElemental"),
            new Ability(typeof(XmlMinionStrike), "BrownBear"),
            new Ability(typeof(XmlMinionStrike), "Bull"),
            new Ability(typeof(XmlMinionStrike), "BullFrog"),
            new Ability(typeof(XmlMinionStrike), "CapturedHordeMinion"),
            new Ability(typeof(XmlMinionStrike), "Cat"),
            new Ability(typeof(XmlMinionStrike), "Centaur"),
            new Ability(typeof(XmlMinionStrike), "ChaosDaemon"),
            new Ability(typeof(XmlMinionStrike), "ChaosDragoon"),
            new Ability(typeof(XmlMinionStrike), "ChaosDragoonElite"),
            new Ability(typeof(XmlMinionStrike), "ChaosGuard"),
            new Ability(typeof(XmlMinionStrike), "Chicken"),
            new Ability(typeof(XmlMinionStrike), "CoMWarHorse"),
            new Ability(typeof(XmlMinionStrike), "CopperElemental"),
            new Ability(typeof(XmlMinionStrike), "Corpser"),
            new Ability(typeof(XmlMinionStrike), "CorrosiveSlime"),
            new Ability(typeof(XmlMinionStrike), "CorruptedSoul"),
            new Ability(typeof(XmlMinionStrike), "Cougar"),
            new Ability(typeof(XmlMinionStrike), "Cow"),
            new Ability(typeof(XmlMinionStrike), "Crane"),
            new Ability(typeof(XmlMinionStrike), "CrimsonDragon"),
            new Ability(typeof(XmlMinionStrike), "CrystalElemental"),
            new Ability(typeof(XmlMinionStrike), "CuSidhe"),
            new Ability(typeof(XmlMinionStrike), "Cursed"),
            new Ability(typeof(XmlMinionStrike), "Cyclops"),
            new Ability(typeof(XmlMinionStrike), "Daemon"),
            new Ability(typeof(XmlMinionStrike), "DarkWisp"),
            new Ability(typeof(XmlMinionStrike), "DarkWolf"),
            new Ability(typeof(XmlMinionStrike), "DarknightCreeper"),
            new Ability(typeof(XmlMinionStrike), "DeathAdder"),
            new Ability(typeof(XmlMinionStrike), "DeathWatchBeetle"),
            new Ability(typeof(XmlMinionStrike), "DeathWatchBeetleHatchling"),
            new Ability(typeof(XmlMinionStrike), "DeepSeaSerpent"),
            new Ability(typeof(XmlMinionStrike), "DemonKnight"),
            new Ability(typeof(XmlMinionStrike), "DesertOstard"),
            new Ability(typeof(XmlMinionStrike), "Devourer"),
            new Ability(typeof(XmlMinionStrike), "DireWolf"),
            new Ability(typeof(XmlMinionStrike), "Dog"),
            new Ability(typeof(XmlMinionStrike), "Dolphin"),
            new Ability(typeof(XmlMinionStrike), "Doppleganger"),
            new Ability(typeof(XmlMinionStrike), "Dragon"),
            new Ability(typeof(XmlMinionStrike), "Drake"),
            new Ability(typeof(XmlMinionStrike), "DreadSpider"),
            new Ability(typeof(XmlMinionStrike), "DullCopperElemental"),
            new Ability(typeof(XmlMinionStrike), "Dummy"),
            new Ability(typeof(XmlMinionStrike), "DummySpecific"),
            new Ability(typeof(XmlMinionStrike), "Eagle"),
            new Ability(typeof(XmlMinionStrike), "EarthElemental"),
            new Ability(typeof(XmlMinionStrike), "Efreet"),
            new Ability(typeof(XmlMinionStrike), "ElderGazer"),
            new Ability(typeof(XmlMinionStrike), "EliteNinja"),
            new Ability(typeof(XmlMinionStrike), "EnergyVortex"),
            new Ability(typeof(XmlMinionStrike), "EnragedCreatures"),
            new Ability(typeof(XmlMinionStrike), "EnslavedGargoyle"),
            new Ability(typeof(XmlMinionStrike), "EtherealCuSidhe"),
            new Ability(typeof(XmlMinionStrike), "EtherealWarrior"),
            new Ability(typeof(XmlMinionStrike), "Ethereals"),
            new Ability(typeof(XmlMinionStrike), "Ettin"),
            new Ability(typeof(XmlMinionStrike), "EvilHealer"),
            new Ability(typeof(XmlMinionStrike), "EvilMage"),
            new Ability(typeof(XmlMinionStrike), "EvilMageLord"),
            new Ability(typeof(XmlMinionStrike), "EvilWanderingHealer"),
            new Ability(typeof(XmlMinionStrike), "Executioner"),
            new Ability(typeof(XmlMinionStrike), "ExodusMinion"),
            new Ability(typeof(XmlMinionStrike), "ExodusOverseer"),
            new Ability(typeof(XmlMinionStrike), "FanDancer"),
            new Ability(typeof(XmlMinionStrike), "FeralTreefellow"),
            new Ability(typeof(XmlMinionStrike), "FetidEssence"),
            new Ability(typeof(XmlMinionStrike), "FireBeetle"),
            new Ability(typeof(XmlMinionStrike), "FireElemental"),
            new Ability(typeof(XmlMinionStrike), "FireGargoyle"),
            new Ability(typeof(XmlMinionStrike), "FireSteed"),
            new Ability(typeof(XmlMinionStrike), "FleshGolem"),
            new Ability(typeof(XmlMinionStrike), "FleshRenderer"),
            new Ability(typeof(XmlMinionStrike), "ForestOstard"),
            new Ability(typeof(XmlMinionStrike), "FortuneTeller"),
            new Ability(typeof(XmlMinionStrike), "FrenziedOstard"),
            new Ability(typeof(XmlMinionStrike), "FrostOoze"),
            new Ability(typeof(XmlMinionStrike), "FrostSpider"),
            new Ability(typeof(XmlMinionStrike), "FrostTroll"),
            new Ability(typeof(XmlMinionStrike), "Gaman"),
            new Ability(typeof(XmlMinionStrike), "Gargoyle"),
            new Ability(typeof(XmlMinionStrike), "GargoyleDestroyer"),
            new Ability(typeof(XmlMinionStrike), "GargoyleEnforcer"),
            new Ability(typeof(XmlMinionStrike), "Gazer"),
            new Ability(typeof(XmlMinionStrike), "GazerLarva"),
            new Ability(typeof(XmlMinionStrike), "Ghoul"),
            new Ability(typeof(XmlMinionStrike), "GiantBlackWidow"),
            new Ability(typeof(XmlMinionStrike), "GiantRat"),
            new Ability(typeof(XmlMinionStrike), "GiantSerpent"),
            new Ability(typeof(XmlMinionStrike), "GiantSpider"),
            new Ability(typeof(XmlMinionStrike), "GiantToad"),
            new Ability(typeof(XmlMinionStrike), "Gibberling"),
            new Ability(typeof(XmlMinionStrike), "Goat"),
            new Ability(typeof(XmlMinionStrike), "GoldenElemental"),
            new Ability(typeof(XmlMinionStrike), "Golem"),
            new Ability(typeof(XmlMinionStrike), "GolemController"),
            new Ability(typeof(XmlMinionStrike), "GoreFiend"),
            new Ability(typeof(XmlMinionStrike), "Gorilla"),
            new Ability(typeof(XmlMinionStrike), "GreatHart"),
            new Ability(typeof(XmlMinionStrike), "GreaterDragon"),
            new Ability(typeof(XmlMinionStrike), "GreaterMongbat"),
            new Ability(typeof(XmlMinionStrike), "GreyWolf"),
            new Ability(typeof(XmlMinionStrike), "GrizzlyBear"),
            new Ability(typeof(XmlMinionStrike), "Guardian"),
            new Ability(typeof(XmlMinionStrike), "Harpy"),
            new Ability(typeof(XmlMinionStrike), "Harrower"),
            new Ability(typeof(XmlMinionStrike), "HarrowerTentacles"),
            new Ability(typeof(XmlMinionStrike), "HeadlessOne"),
            new Ability(typeof(XmlMinionStrike), "Healer"),
            new Ability(typeof(XmlMinionStrike), "HellCat"),
            new Ability(typeof(XmlMinionStrike), "HellHound"),
            new Ability(typeof(XmlMinionStrike), "HellSteed"),
            new Ability(typeof(XmlMinionStrike), "Hind"),
            new Ability(typeof(XmlMinionStrike), "Hiryu"),
            new Ability(typeof(XmlMinionStrike), "HordeMinion"),
            new Ability(typeof(XmlMinionStrike), "HordeMinion"),
            new Ability(typeof(XmlMinionStrike), "Horse"),
            new Ability(typeof(XmlMinionStrike), "IceElemental"),
            new Ability(typeof(XmlMinionStrike), "IceFiend"),
            new Ability(typeof(XmlMinionStrike), "IceSerpent"),
            new Ability(typeof(XmlMinionStrike), "IceSnake"),
            new Ability(typeof(XmlMinionStrike), "Ilhenir"),
            new Ability(typeof(XmlMinionStrike), "Imp"),
            new Ability(typeof(XmlMinionStrike), "Impaler"),
            new Ability(typeof(XmlMinionStrike), "InterredGrizzle "),
            new Ability(typeof(XmlMinionStrike), "JackRabbit"),
            new Ability(typeof(XmlMinionStrike), "Juggernaut"),
            new Ability(typeof(XmlMinionStrike), "JukaLord"),
            new Ability(typeof(XmlMinionStrike), "JukaMage"),
            new Ability(typeof(XmlMinionStrike), "JukaWarrior"),
            new Ability(typeof(XmlMinionStrike), "Jwilson"),
            new Ability(typeof(XmlMinionStrike), "Kappa"),
            new Ability(typeof(XmlMinionStrike), "KazeKemono"),
            new Ability(typeof(XmlMinionStrike), "KhaldunRevenant"),
            new Ability(typeof(XmlMinionStrike), "KhaldunSummoner"),
            new Ability(typeof(XmlMinionStrike), "KhaldunZealot"),
            new Ability(typeof(XmlMinionStrike), "Kirin"),
            new Ability(typeof(XmlMinionStrike), "Kraken"),
            new Ability(typeof(XmlMinionStrike), "LadyOfTheSnow"),
            new Ability(typeof(XmlMinionStrike), "LavaLizard"),
            new Ability(typeof(XmlMinionStrike), "LavaSerpent"),
            new Ability(typeof(XmlMinionStrike), "LavaSnake"),
            new Ability(typeof(XmlMinionStrike), "LesserHiryu"),
            new Ability(typeof(XmlMinionStrike), "Leviathan"),
            new Ability(typeof(XmlMinionStrike), "Lich"),
            new Ability(typeof(XmlMinionStrike), "LichLord"),
            new Ability(typeof(XmlMinionStrike), "Lizardman"),
            new Ability(typeof(XmlMinionStrike), "Llama"),
            new Ability(typeof(XmlMinionStrike), "LordOaks"),
            new Ability(typeof(XmlMinionStrike), "MLDryad"),
            new Ability(typeof(XmlMinionStrike), "MeerCaptain"),
            new Ability(typeof(XmlMinionStrike), "MeerEternal"),
            new Ability(typeof(XmlMinionStrike), "MeerMage"),
            new Ability(typeof(XmlMinionStrike), "MeerWarrior"),
            new Ability(typeof(XmlMinionStrike), "Mephitis"),
            new Ability(typeof(XmlMinionStrike), "Meraktus"),
            new Ability(typeof(XmlMinionStrike), "MinaxWarHorse"),
            new Ability(typeof(XmlMinionStrike), "Minotaur"),
            new Ability(typeof(XmlMinionStrike), "MinotaurCaptain"),
            new Ability(typeof(XmlMinionStrike), "MinotaurScout"),
            new Ability(typeof(XmlMinionStrike), "Moloch"),
            new Ability(typeof(XmlMinionStrike), "Mongbat"),
            new Ability(typeof(XmlMinionStrike), "MoundOfMaggots"),
            new Ability(typeof(XmlMinionStrike), "MountainGoat"),
            new Ability(typeof(XmlMinionStrike), "Mummy"),
            new Ability(typeof(XmlMinionStrike), "Neira"),
            new Ability(typeof(XmlMinionStrike), "Nightmare"),
            new Ability(typeof(XmlMinionStrike), "Ogre"),
            new Ability(typeof(XmlMinionStrike), "OgreLord"),
            new Ability(typeof(XmlMinionStrike), "OneEyedWilly"),
            new Ability(typeof(XmlMinionStrike), "Oni"),
            new Ability(typeof(XmlMinionStrike), "OphidianArchmage"),
            new Ability(typeof(XmlMinionStrike), "OphidianKnight"),
            new Ability(typeof(XmlMinionStrike), "OphidianMage"),
            new Ability(typeof(XmlMinionStrike), "OphidianMatriarch"),
            new Ability(typeof(XmlMinionStrike), "OphidianWarrior"),
            new Ability(typeof(XmlMinionStrike), "Orc"),
            new Ability(typeof(XmlMinionStrike), "OrcBomber"),
            new Ability(typeof(XmlMinionStrike), "OrcBrute"),
            new Ability(typeof(XmlMinionStrike), "OrcCaptain"),
            new Ability(typeof(XmlMinionStrike), "OrcChopper"),
            new Ability(typeof(XmlMinionStrike), "OrcishLord"),
            new Ability(typeof(XmlMinionStrike), "OrcishMage"),
            new Ability(typeof(XmlMinionStrike), "OrderGuard"),
            new Ability(typeof(XmlMinionStrike), "PackHorse"),
            new Ability(typeof(XmlMinionStrike), "PackLlama"),
            new Ability(typeof(XmlMinionStrike), "Panther"),
            new Ability(typeof(XmlMinionStrike), "Paragon"),
            new Ability(typeof(XmlMinionStrike), "PatchworkSkeleton"),
            new Ability(typeof(XmlMinionStrike), "PestilentBandage"),
            new Ability(typeof(XmlMinionStrike), "Phoenix"),
            new Ability(typeof(XmlMinionStrike), "Pig"),
            new Ability(typeof(XmlMinionStrike), "Pixie"),
            new Ability(typeof(XmlMinionStrike), "PlagueBeast"),
            new Ability(typeof(XmlMinionStrike), "PlagueBeastLord"),
            new Ability(typeof(XmlMinionStrike), "PlagueSpawn"),
            new Ability(typeof(XmlMinionStrike), "PoisonElemental"),
            new Ability(typeof(XmlMinionStrike), "PolarBear"),
            new Ability(typeof(XmlMinionStrike), "PredatorHellCat"),
            new Ability(typeof(XmlMinionStrike), "PricedHealer"),
            new Ability(typeof(XmlMinionStrike), "Quagmire"),
            new Ability(typeof(XmlMinionStrike), "Rabbit"),
            new Ability(typeof(XmlMinionStrike), "RagingGrizzlyBear"),
            new Ability(typeof(XmlMinionStrike), "RaiJu"),
            new Ability(typeof(XmlMinionStrike), "Rat"),
            new Ability(typeof(XmlMinionStrike), "Ratman"),
            new Ability(typeof(XmlMinionStrike), "RatmanArcher"),
            new Ability(typeof(XmlMinionStrike), "RatmanMage"),
            new Ability(typeof(XmlMinionStrike), "Ravager"),
            new Ability(typeof(XmlMinionStrike), "Reaper"),
            new Ability(typeof(XmlMinionStrike), "RedSolenInfiltratorQueen"),
            new Ability(typeof(XmlMinionStrike), "RedSolenInfiltratorWarrior"),
            new Ability(typeof(XmlMinionStrike), "RedSolenQueen"),
            new Ability(typeof(XmlMinionStrike), "RedSolenWarrior"),
            new Ability(typeof(XmlMinionStrike), "RedSolenWorker"),
            new Ability(typeof(XmlMinionStrike), "RestlessSoul"),
            new Ability(typeof(XmlMinionStrike), "Revenant"),
            new Ability(typeof(XmlMinionStrike), "RevenantLion"),
            new Ability(typeof(XmlMinionStrike), "RidableLlama"),
            new Ability(typeof(XmlMinionStrike), "RidablePolarBear"),
            new Ability(typeof(XmlMinionStrike), "Ridgeback"),
            new Ability(typeof(XmlMinionStrike), "Rikktor"),
            new Ability(typeof(XmlMinionStrike), "Ronin"),
            new Ability(typeof(XmlMinionStrike), "RottingCorpse"),
            new Ability(typeof(XmlMinionStrike), "RuneBeetle"),
            new Ability(typeof(XmlMinionStrike), "SLWarHorse"),
            new Ability(typeof(XmlMinionStrike), "SandVortex"),
            new Ability(typeof(XmlMinionStrike), "Satyr"),
            new Ability(typeof(XmlMinionStrike), "Savage"),
            new Ability(typeof(XmlMinionStrike), "SavageRider"),
            new Ability(typeof(XmlMinionStrike), "SavageRidgeback"),
            new Ability(typeof(XmlMinionStrike), "SavageShaman"),
            new Ability(typeof(XmlMinionStrike), "ScaledSwampDragon"),
            new Ability(typeof(XmlMinionStrike), "Scorpion"),
            new Ability(typeof(XmlMinionStrike), "SeaSerpent"),
            new Ability(typeof(XmlMinionStrike), "Semidar"),
            new Ability(typeof(XmlMinionStrike), "Serado"),
            new Ability(typeof(XmlMinionStrike), "SerpentineDragon"),
            new Ability(typeof(XmlMinionStrike), "ServantOfSemidar"),
            new Ability(typeof(XmlMinionStrike), "SewerRat"),
            new Ability(typeof(XmlMinionStrike), "Shade"),
            new Ability(typeof(XmlMinionStrike), "ShadowFiend"),
            new Ability(typeof(XmlMinionStrike), "ShadowIronElemental"),
            new Ability(typeof(XmlMinionStrike), "ShadowKnight"),
            new Ability(typeof(XmlMinionStrike), "ShadowWisp"),
            new Ability(typeof(XmlMinionStrike), "ShadowWisp"),
            new Ability(typeof(XmlMinionStrike), "ShadowWyrm"),
            new Ability(typeof(XmlMinionStrike), "Sheep"),
            new Ability(typeof(XmlMinionStrike), "Silvani"),
            new Ability(typeof(XmlMinionStrike), "SilverSerpent"),
            new Ability(typeof(XmlMinionStrike), "SilverSteed"),
            new Ability(typeof(XmlMinionStrike), "SkeletalDragon"),
            new Ability(typeof(XmlMinionStrike), "SkeletalKnight"),
            new Ability(typeof(XmlMinionStrike), "SkeletalMage"),
            new Ability(typeof(XmlMinionStrike), "SkeletalMount"),
            new Ability(typeof(XmlMinionStrike), "Skeleton"),
            new Ability(typeof(XmlMinionStrike), "SkitteringHopper"),
            new Ability(typeof(XmlMinionStrike), "Slime"),
            new Ability(typeof(XmlMinionStrike), "Snake"),
            new Ability(typeof(XmlMinionStrike), "SnowElemental"),
            new Ability(typeof(XmlMinionStrike), "SnowLeopard"),
            new Ability(typeof(XmlMinionStrike), "SolenHelper"),
            new Ability(typeof(XmlMinionStrike), "SpawnedOrcishLord"),
            new Ability(typeof(XmlMinionStrike), "SpectralArmour"),
            new Ability(typeof(XmlMinionStrike), "Spectre"),
            new Ability(typeof(XmlMinionStrike), "Spellbinder"),
            new Ability(typeof(XmlMinionStrike), "StoneGargoyle"),
            new Ability(typeof(XmlMinionStrike), "StoneHarpy"),
            new Ability(typeof(XmlMinionStrike), "StrongMongbat"),
            new Ability(typeof(XmlMinionStrike), "Succubus"),
            new Ability(typeof(XmlMinionStrike), "SummonedAirElemental"),
            new Ability(typeof(XmlMinionStrike), "SummonedDaemon"),
            new Ability(typeof(XmlMinionStrike), "SummonedEarthElemental"),
            new Ability(typeof(XmlMinionStrike), "SummonedFireElemental"),
            new Ability(typeof(XmlMinionStrike), "SummonedWaterElemental"),
            new Ability(typeof(XmlMinionStrike), "SwampDragon"),
            new Ability(typeof(XmlMinionStrike), "SwampTentacle"),
            new Ability(typeof(XmlMinionStrike), "TBWarHorse"),
            new Ability(typeof(XmlMinionStrike), "TerathanAvenger"),
            new Ability(typeof(XmlMinionStrike), "TerathanDrone"),
            new Ability(typeof(XmlMinionStrike), "TerathanMatriarch"),
            new Ability(typeof(XmlMinionStrike), "TerathanWarrior"),
            new Ability(typeof(XmlMinionStrike), "TheExecutioner"),
            new Ability(typeof(XmlMinionStrike), "TimberWolf"),
            new Ability(typeof(XmlMinionStrike), "Titan"),
            new Ability(typeof(XmlMinionStrike), "ToxicElemental"),
            new Ability(typeof(XmlMinionStrike), "Treefellow"),
            new Ability(typeof(XmlMinionStrike), "Troglodyte"),
            new Ability(typeof(XmlMinionStrike), "Troll"),
            new Ability(typeof(XmlMinionStrike), "TsukiWolf"),
            new Ability(typeof(XmlMinionStrike), "Twaulo"),
            new Ability(typeof(XmlMinionStrike), "Unicorn"),
            new Ability(typeof(XmlMinionStrike), "ValoriteElemental"),
            new Ability(typeof(XmlMinionStrike), "VampireBat"),
            new Ability(typeof(XmlMinionStrike), "VampireBat"),
            new Ability(typeof(XmlMinionStrike), "VeriteElemental"),
            new Ability(typeof(XmlMinionStrike), "VorpalBunny"),
            new Ability(typeof(XmlMinionStrike), "WailingBanshee"),
            new Ability(typeof(XmlMinionStrike), "Walrus"),
            new Ability(typeof(XmlMinionStrike), "Wanderer"),
            new Ability(typeof(XmlMinionStrike), "WandererOfTheVoid"),
            new Ability(typeof(XmlMinionStrike), "WanderingHealer"),
            new Ability(typeof(XmlMinionStrike), "WarriorGuard"),
            new Ability(typeof(XmlMinionStrike), "WaterElemental"),
            new Ability(typeof(XmlMinionStrike), "WhippingVine"),
            new Ability(typeof(XmlMinionStrike), "WhiteWolf"),
            new Ability(typeof(XmlMinionStrike), "WhiteWyrm"),
            new Ability(typeof(XmlMinionStrike), "Wisp"),
            new Ability(typeof(XmlMinionStrike), "Wraith"),
            new Ability(typeof(XmlMinionStrike), "Wyvern"),
            new Ability(typeof(XmlMinionStrike), "Yamandon"),
            new Ability(typeof(XmlMinionStrike), "YomotsuElder"),
            new Ability(typeof(XmlMinionStrike), "YomotsuPriest"),
            new Ability(typeof(XmlMinionStrike), "YomotsuWarrior"),
            new Ability(typeof(XmlMinionStrike), "Zombie"),
            new Ability(typeof(XmlMinionStrike), "Corsair"),
            new Ability(typeof(XmlMinionStrike), "ElvenRanger"),
            new Ability(typeof(XmlMinionStrike), "EttinBrute"),
            new Ability(typeof(XmlMinionStrike), "LizardmanArcher"),
            new Ability(typeof(XmlMinionStrike), "LizardmanMage"),
            new Ability(typeof(XmlMinionStrike), "Napalm"),
            new Ability(typeof(XmlMinionStrike), "PlagueBat"),
            new Ability(typeof(XmlMinionStrike), "ScourgeBat"),
            new Ability(typeof(XmlMinionStrike), "ScourgeWolf"),
            new Ability(typeof(XmlMinionStrike), "Witch"),
            new Ability(typeof(XmlMinionStrike), "AntifreezeElemental"),
            new Ability(typeof(XmlMinionStrike), "JadeElemental"),
            new Ability(typeof(XmlMinionStrike), "MilkElemental"),
            new Ability(typeof(XmlMinionStrike), "MithrilElemental"),
            new Ability(typeof(XmlMinionStrike), "OilElemental"),
            new Ability(typeof(XmlMinionStrike), "ThoriumElemental"),
            new Ability(typeof(XmlMinionStrike), "KraitArchmage"),
            new Ability(typeof(XmlMinionStrike), "KraitDragon"),
            new Ability(typeof(XmlMinionStrike), "KraitMatriarch"),
            new Ability(typeof(XmlMinionStrike), "KraitNecross"),
            new Ability(typeof(XmlMinionStrike), "KraitNeoss"),
            new Ability(typeof(XmlMinionStrike), "KraitWarrior"),
            new Ability(typeof(XmlMinionStrike), "OozeOfDecay"),
            new Ability(typeof(XmlMinionStrike), "SludgeOfDecay"),
            new Ability(typeof(XmlMinionStrike), "EngrudgingCesspool"),
            new Ability(typeof(XmlMinionStrike), "SinfullRegret"),
            new Ability(typeof(XmlMinionStrike), "ArcaneLich"),
            new Ability(typeof(XmlMinionStrike), "Atropal"),
            new Ability(typeof(XmlMinionStrike), "AtropalScion"),
            new Ability(typeof(XmlMinionStrike), "Baelnorn"),
            new Ability(typeof(XmlMinionStrike), "Beguiled"),
            new Ability(typeof(XmlMinionStrike), "DrownedDead"),
            new Ability(typeof(XmlMinionStrike), "FesteringEel"),
            new Ability(typeof(XmlMinionStrike), "FesteringMutant"),
            new Ability(typeof(XmlMinionStrike), "Frankenstein"),
            new Ability(typeof(XmlMinionStrike), "Guilded"),
            new Ability(typeof(XmlMinionStrike), "GuildedDead"),
            new Ability(typeof(XmlMinionStrike), "IllSoul"),
            new Ability(typeof(XmlMinionStrike), "LeftForDead"),
            new Ability(typeof(XmlMinionStrike), "Necrobus"),
            new Ability(typeof(XmlMinionStrike), "OrganOfUndeath"),
            new Ability(typeof(XmlMinionStrike), "PlagueBearer"),
            new Ability(typeof(XmlMinionStrike), "Ravaged"),
            new Ability(typeof(XmlMinionStrike), "Redead"),
            new Ability(typeof(XmlMinionStrike), "ReturnedFotter"),
            new Ability(typeof(XmlMinionStrike), "RevenantCat"),
            new Ability(typeof(XmlMinionStrike), "RevenantWolf"),
            new Ability(typeof(XmlMinionStrike), "ShamblingHorror"),
            new Ability(typeof(XmlMinionStrike), "Slaughtered"),
            new Ability(typeof(XmlMinionStrike), "TumorOfUndeath"),
            new Ability(typeof(XmlMinionStrike), "WalkingCorpse"),
            new Ability(typeof(XmlMinionStrike), "Wronged"),
            new Ability(typeof(XmlMinionStrike), "Apparition"),
            new Ability(typeof(XmlMinionStrike), "DreadFrequency"),
            new Ability(typeof(XmlMinionStrike), "EssenceOfUndeath"),
            new Ability(typeof(XmlMinionStrike), "FadedMemory"),
            new Ability(typeof(XmlMinionStrike), "GraveDustElemental"),
            new Ability(typeof(XmlMinionStrike), "HauntedVision"),
            new Ability(typeof(XmlMinionStrike), "Phantom"),
            new Ability(typeof(XmlMinionStrike), "Poltergeist"),
            new Ability(typeof(XmlMinionStrike), "Quell"),
            new Ability(typeof(XmlMinionStrike), "SoulflameElemental"),
            new Ability(typeof(XmlMinionStrike), "SpiritEnergy"),
            new Ability(typeof(XmlMinionStrike), "AshenArcher"),
            new Ability(typeof(XmlMinionStrike), "AshenCorpse"),
            new Ability(typeof(XmlMinionStrike), "AshenHorror"),
            new Ability(typeof(XmlMinionStrike), "AshenKnight"),
            new Ability(typeof(XmlMinionStrike), "BurningArcher"),
            new Ability(typeof(XmlMinionStrike), "BurningCorpse"),
            new Ability(typeof(XmlMinionStrike), "BurningDead"),
            new Ability(typeof(XmlMinionStrike), "BurningKnight"),
            new Ability(typeof(XmlMinionStrike), "DracoLich"),
            new Ability(typeof(XmlMinionStrike), "Othro"),
            new Ability(typeof(XmlMinionStrike), "EgyptainBustier"),
            new Ability(typeof(XmlMinionStrike), "EgyptainFrenziedOstard"),
            new Ability(typeof(XmlMinionStrike), "EgyptainSpear"),
            new Ability(typeof(XmlMinionStrike), "EgyptianArcher"),
            new Ability(typeof(XmlMinionStrike), "EgyptianArcherRider"),
            new Ability(typeof(XmlMinionStrike), "EgyptianPeasant"),
            new Ability(typeof(XmlMinionStrike), "EgyptianPriest"),
            new Ability(typeof(XmlMinionStrike), "EgyptianPriestess"),
            new Ability(typeof(XmlMinionStrike), "EgyptianSoldier"),
            new Ability(typeof(XmlMinionStrike), "EgyptianSoldierRider"),
            new Ability(typeof(XmlMinionStrike), "Anubis"),
            new Ability(typeof(XmlMinionStrike), "LadyCleopatra"),
            new Ability(typeof(XmlMinionStrike), "LordHorus"),
            new Ability(typeof(XmlMinionStrike), "RAntLion"),
            new Ability(typeof(XmlMinionStrike), "RCentaur"),
            new Ability(typeof(XmlMinionStrike), "RDaemon"),
            new Ability(typeof(XmlMinionStrike), "RDragon"),
            new Ability(typeof(XmlMinionStrike), "RDrake"),
            new Ability(typeof(XmlMinionStrike), "RPig"),
            new Ability(typeof(XmlMinionStrike), "RSandVortex"),
            new Ability(typeof(XmlMinionStrike), "RScorpion"),
            new Ability(typeof(XmlMinionStrike), "RSeaSerpent"),
            new Ability(typeof(XmlMinionStrike), "RSkeletalDragon"),
            new Ability(typeof(XmlMinionStrike), "RSpider"),
            new Ability(typeof(XmlMinionStrike), "RWyvern")
        };

        // Define the variable for the number of abilities
        private const int NumberOfAbilitiesToAttach = 1;

        [Constructable]
        public SummonedRandomKnightLord() : base(AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = knightLordBodies[Utility.Random(knightLordBodies.Length)];
            Hue = Utility.RandomMinMax(1, 1000);
            Name = wordBank1[Utility.Random(wordBank1.Length)] + " " + wordBank2[Utility.Random(wordBank2.Length)];

            SetStr(Utility.RandomMinMax(50, 500));
            SetDex(Utility.RandomMinMax(50, 500));
            SetInt(Utility.RandomMinMax(50, 500));
            SetHits(Utility.RandomMinMax(100, 500));
            
            BaseSoundID = 427;
            
            AI = aiTypes[Utility.Random(aiTypes.Length)];
            FightMode = fightModes[Utility.Random(fightModes.Length)];

            SetResistance(ResistanceType.Physical, Utility.RandomMinMax(20, 50));
            SetResistance(ResistanceType.Fire, Utility.RandomMinMax(20, 50));
            SetResistance(ResistanceType.Cold, Utility.RandomMinMax(20, 50));
            SetResistance(ResistanceType.Poison, Utility.RandomMinMax(20, 50));
            SetResistance(ResistanceType.Energy, Utility.RandomMinMax(20, 50));

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            PackPowerfulLoot();
			PackItem(new RandomFancyBanner());
            PackItem(new Gold(Utility.RandomMinMax(10, 1000)));

            AttachRandomAbilities();
        }

        private void AttachRandomAbilities()
        {
            var random = new Random();
            var selectedAbilities = new HashSet<int>();

            while (selectedAbilities.Count < NumberOfAbilitiesToAttach)
            {
                int index = random.Next(abilities.Length);
                selectedAbilities.Add(index);
            }

            foreach (var index in selectedAbilities)
            {
                var ability = abilities[index];
                if (ability.StringArgument != null)
                {
                    XmlAttach.AttachTo(this, (XmlAttachment)Activator.CreateInstance(ability.AbilityType, ability.StringArgument));
                }
                else if (ability.TakesArguments)
                {
                    if (ability.Value2 == 0)
                    {
                        XmlAttach.AttachTo(this, (XmlAttachment)Activator.CreateInstance(ability.AbilityType, ability.Value1));
                    }
                    else
                    {
                        XmlAttach.AttachTo(this, (XmlAttachment)Activator.CreateInstance(ability.AbilityType, ability.Value1, ability.Value2));
                    }
                }
                else
                {
                    XmlAttach.AttachTo(this, (XmlAttachment)Activator.CreateInstance(ability.AbilityType));
                }
            }
        }

		private void PackPowerfulLoot()
		{
			// Powerful magic weapon
			if (Utility.RandomDouble() < 0.25) // Adjust the chance as needed
			{
				PackPowerfulMagicWeapon();
			}

			// Powerful magic armor
			if (Utility.RandomDouble() < 0.25) // Adjust the chance as needed
			{
				PackPowerfulMagicArmor();
			}

			// Powerful magic jewelry
			if (Utility.RandomDouble() < 0.10) // Adjust the chance as needed
			{
				PackPowerfulMagicJewelry();
			}
		}

		private void PackPowerfulMagicWeapon()
		{
			BaseWeapon weapon = Loot.RandomWeapon();
			// Customizing the weapon with base modifiers
			weapon.Attributes.SpellChanneling = 1;
			weapon.Attributes.BonusHits = Utility.RandomMinMax(5, 25);
			weapon.WeaponAttributes.HitLowerAttack = Utility.RandomMinMax(10, 30);

			// Tiered system for weapon damage and additional modifiers
			double chance = Utility.RandomDouble();
			if (chance < 0.05) // Very rare - highest damage and unique modifiers
			{
				weapon.MinDamage = Utility.RandomMinMax(1, 100);
				weapon.MaxDamage = Utility.RandomMinMax(100, 150);

				// Additional high-tier modifiers
				weapon.Attributes.WeaponSpeed = Utility.RandomMinMax(20, 30);
				weapon.Attributes.WeaponDamage = Utility.RandomMinMax(25, 50);
				weapon.WeaponAttributes.HitLightning = Utility.RandomMinMax(40, 60);
				weapon.WeaponAttributes.HitHarm = Utility.RandomMinMax(20, 30);
			}
			else if (chance < 0.2) // Rare
			{
				weapon.MinDamage = Utility.RandomMinMax(1, 50);
				weapon.MaxDamage = Utility.RandomMinMax(50, 100);

				// Additional rare-tier modifiers
				weapon.Attributes.WeaponSpeed = Utility.RandomMinMax(10, 19);
				weapon.Attributes.WeaponDamage = Utility.RandomMinMax(15, 24);
				weapon.WeaponAttributes.HitMagicArrow = Utility.RandomMinMax(20, 40);
				weapon.WeaponAttributes.HitFireball = Utility.RandomMinMax(10, 20);
			}
			else if (chance < 0.5) // Uncommon
			{
				weapon.MinDamage = Utility.RandomMinMax(1, 30);
				weapon.MaxDamage = Utility.RandomMinMax(30, 70);

				// Additional uncommon-tier modifiers
				weapon.Attributes.WeaponSpeed = Utility.RandomMinMax(5, 9);
				weapon.Attributes.WeaponDamage = Utility.RandomMinMax(10, 14);
				weapon.WeaponAttributes.HitColdArea = Utility.RandomMinMax(10, 20);
			}
			else // Common - lowest damage and basic modifiers
			{
				weapon.MinDamage = Utility.RandomMinMax(1, 20);
				weapon.MaxDamage = Utility.RandomMinMax(20, 50);

				// Additional common-tier modifiers
				weapon.Attributes.WeaponSpeed = Utility.RandomMinMax(0, 4);
				weapon.Attributes.WeaponDamage = Utility.RandomMinMax(5, 9);
			}

			// Base modifiers applicable to all tiers
			weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(5); // Up to Ruin
			weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(5); // Up to Supremely Accurate
			weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(5); // Up to Indestructible

			// Rename the weapon using the word lists
			weapon.Name = wordBank1[Utility.Random(wordBank1.Length)] + " " + wordBank2[Utility.Random(wordBank2.Length)] + " " + weapon.Name;
			// Give the weapon a random hue
			weapon.Hue = Utility.RandomMinMax(1, 1000);
			PackItem(weapon);
		}



		private void PackPowerfulMagicArmor()
		{
			BaseArmor armor = Loot.RandomArmor();
			// Tiered system for attribute bonuses
			double attributeChance = Utility.RandomDouble();
			if (attributeChance < 0.005) // Very rare - highest bonuses
			{
				armor.Attributes.ReflectPhysical = Utility.RandomMinMax(20, 25);
				armor.Attributes.DefendChance = Utility.RandomMinMax(25, 50);
				armor.Attributes.BonusDex = Utility.RandomMinMax(20, 50);
				armor.Attributes.BonusInt = Utility.RandomMinMax(20, 50);
				armor.Attributes.BonusStr = Utility.RandomMinMax(20, 50);
				armor.Attributes.BonusHits = Utility.RandomMinMax(50, 100);
				armor.Attributes.BonusMana = Utility.RandomMinMax(50, 100);
			}
			else if (attributeChance < 0.05) // Rare
			{
				armor.Attributes.ReflectPhysical = Utility.RandomMinMax(15, 19);
				armor.Attributes.DefendChance = Utility.RandomMinMax(15, 24);
				armor.Attributes.BonusDex = Utility.RandomMinMax(10, 19);
				armor.Attributes.BonusInt = Utility.RandomMinMax(10, 19);
				armor.Attributes.BonusStr = Utility.RandomMinMax(10, 19);
				armor.Attributes.BonusHits = Utility.RandomMinMax(25, 49);
				armor.Attributes.BonusMana = Utility.RandomMinMax(25, 49);
			}
			else if (attributeChance < 0.5) // Uncommon
			{
				armor.Attributes.ReflectPhysical = Utility.RandomMinMax(10, 14);
				armor.Attributes.DefendChance = Utility.RandomMinMax(5, 14);
				armor.Attributes.BonusDex = Utility.RandomMinMax(5, 9);
				armor.Attributes.BonusInt = Utility.RandomMinMax(5, 9);
				armor.Attributes.BonusStr = Utility.RandomMinMax(5, 9);
				armor.Attributes.BonusHits = Utility.RandomMinMax(10, 24);
				armor.Attributes.BonusMana = Utility.RandomMinMax(10, 24);
			}
			else // Common - lowest bonuses
			{
				armor.Attributes.ReflectPhysical = Utility.RandomMinMax(5, 9);
				armor.Attributes.DefendChance = Utility.RandomMinMax(1, 4);
				armor.Attributes.BonusDex = Utility.RandomMinMax(1, 4);
				armor.Attributes.BonusInt = Utility.RandomMinMax(1, 4);
				armor.Attributes.BonusStr = Utility.RandomMinMax(1, 4);
				armor.Attributes.BonusHits = Utility.RandomMinMax(5, 9);
				armor.Attributes.BonusMana = Utility.RandomMinMax(5, 9);
			}

			// Tiered system for elemental resistance bonuses
			double resistanceChance = Utility.RandomDouble();
			if (resistanceChance < 0.05) // Very rare - highest resistance
			{
				SetHighResistance(armor);
			}
			else if (resistanceChance < 0.2) // Rare
			{
				SetMediumResistance(armor);
			}
			else if (resistanceChance < 0.5) // Uncommon
			{
				SetLowResistance(armor);
			}
			else // Common - lowest resistance
			{
				SetVeryLowResistance(armor);
			}

			// Rename the armor using the word lists
			armor.Name = wordBank1[Utility.Random(wordBank1.Length)] + " " + wordBank2[Utility.Random(wordBank2.Length)] + " " + armor.Name;
			// Give the armor a random hue
			armor.Hue = Utility.RandomMinMax(1, 1000);
			PackItem(armor);
		}

		// Example methods to set resistance levels
		private void SetHighResistance(BaseArmor armor)
		{
			armor.ColdBonus = Utility.RandomMinMax(20, 30);
			armor.EnergyBonus = Utility.RandomMinMax(20, 30);
			armor.FireBonus = Utility.RandomMinMax(20, 30);
			armor.PhysicalBonus = Utility.RandomMinMax(20, 30);
			armor.PoisonBonus = Utility.RandomMinMax(20, 30);
		}

		private void SetMediumResistance(BaseArmor armor)
		{
			armor.ColdBonus = Utility.RandomMinMax(15, 19);
			armor.EnergyBonus = Utility.RandomMinMax(15, 19);
			armor.FireBonus = Utility.RandomMinMax(15, 19);
			armor.PhysicalBonus = Utility.RandomMinMax(15, 19);
			armor.PoisonBonus = Utility.RandomMinMax(15, 19);
		}

		private void SetLowResistance(BaseArmor armor)
		{
			armor.ColdBonus = Utility.RandomMinMax(10, 14);
			armor.EnergyBonus = Utility.RandomMinMax(10, 14);
			armor.FireBonus = Utility.RandomMinMax(10, 14);
			armor.PhysicalBonus = Utility.RandomMinMax(10, 14);
			armor.PoisonBonus = Utility.RandomMinMax(10, 14);
		}

		private void SetVeryLowResistance(BaseArmor armor)
		{
			armor.ColdBonus = Utility.RandomMinMax(5, 9);
			armor.EnergyBonus = Utility.RandomMinMax(5, 9);
			armor.FireBonus = Utility.RandomMinMax(5, 9);
			armor.PhysicalBonus = Utility.RandomMinMax(5, 9);
			armor.PoisonBonus = Utility.RandomMinMax(5, 9);
		}


		private void PackPowerfulMagicJewelry()
		{
			BaseJewel jewel = Loot.RandomJewelry();
			// Customizing the jewelry to make it powerful
			double chance = Utility.RandomDouble();

			if (chance < 0.05) // Very rare
			{
				// Highest bonuses
				jewel.Attributes.SpellDamage = Utility.RandomMinMax(100, 200);
				jewel.Attributes.CastRecovery = Utility.RandomMinMax(3, 5);
				jewel.Attributes.CastSpeed = Utility.RandomMinMax(3, 5);
				jewel.Attributes.LowerManaCost = Utility.RandomMinMax(30, 55);
				jewel.Attributes.LowerRegCost = Utility.RandomMinMax(30, 50);
				// Very rare bonuses for Dex, Int, Str, Hits, Mana
				jewel.Attributes.BonusDex = Utility.RandomMinMax(20, 50);
				jewel.Attributes.BonusInt = Utility.RandomMinMax(20, 50);
				jewel.Attributes.BonusStr = Utility.RandomMinMax(20, 50);
				jewel.Attributes.BonusHits = Utility.RandomMinMax(50, 100);
				jewel.Attributes.BonusMana = Utility.RandomMinMax(50, 100);
			}
			else if (chance < 0.2) // Rare
			{
				// Medium-high bonuses
				jewel.Attributes.SpellDamage = Utility.RandomMinMax(50, 99);
				jewel.Attributes.CastRecovery = Utility.RandomMinMax(2, 3);
				jewel.Attributes.CastSpeed = Utility.RandomMinMax(2, 3);
				jewel.Attributes.LowerManaCost = Utility.RandomMinMax(20, 29);
				jewel.Attributes.LowerRegCost = Utility.RandomMinMax(20, 29);
				// Rare bonuses for Dex, Int, Str, Hits, Mana
				jewel.Attributes.BonusDex = Utility.RandomMinMax(10, 19);
				jewel.Attributes.BonusInt = Utility.RandomMinMax(10, 19);
				jewel.Attributes.BonusStr = Utility.RandomMinMax(10, 19);
				jewel.Attributes.BonusHits = Utility.RandomMinMax(25, 49);
				jewel.Attributes.BonusMana = Utility.RandomMinMax(25, 49);
			}
			else if (chance < 0.5) // Uncommon
			{
				// Medium bonuses
				jewel.Attributes.SpellDamage = Utility.RandomMinMax(25, 49);
				jewel.Attributes.CastRecovery = Utility.RandomMinMax(1, 2);
				jewel.Attributes.CastSpeed = Utility.RandomMinMax(1, 2);
				jewel.Attributes.LowerManaCost = Utility.RandomMinMax(10, 19);
				jewel.Attributes.LowerRegCost = Utility.RandomMinMax(10, 19);
				// Uncommon bonuses for Dex, Int, Str, Hits, Mana
				jewel.Attributes.BonusDex = Utility.RandomMinMax(5, 9);
				jewel.Attributes.BonusInt = Utility.RandomMinMax(5, 9);
				jewel.Attributes.BonusStr = Utility.RandomMinMax(5, 9);
				jewel.Attributes.BonusHits = Utility.RandomMinMax(10, 24);
				jewel.Attributes.BonusMana = Utility.RandomMinMax(10, 24);
			}
			else // Common
			{
				// Lowest bonuses
				jewel.Attributes.SpellDamage = Utility.RandomMinMax(5, 24);
				jewel.Attributes.CastRecovery = 1;
				jewel.Attributes.CastSpeed = 1;
				jewel.Attributes.LowerManaCost = Utility.RandomMinMax(0, 9);
				jewel.Attributes.LowerRegCost = Utility.RandomMinMax(0, 9);
				// Common bonuses for Dex, Int, Str, Hits, Mana
				jewel.Attributes.BonusDex = Utility.RandomMinMax(1, 4);
				jewel.Attributes.BonusInt = Utility.RandomMinMax(1, 4);
				jewel.Attributes.BonusStr = Utility.RandomMinMax(1, 4);
				jewel.Attributes.BonusHits = Utility.RandomMinMax(5, 9);
				jewel.Attributes.BonusMana = Utility.RandomMinMax(5, 9);
			}

			// Rename the jewelry using the word lists
			jewel.Name = wordBank1[Utility.Random(wordBank1.Length)] + " " + wordBank2[Utility.Random(wordBank2.Length)] + " " + jewel.Name;
			// Give the jewelry a random hue
			jewel.Hue = Utility.RandomMinMax(1, 1000);
			PackItem(jewel);
		}

        public SummonedRandomKnightLord(Serial serial) : base(serial)
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
}
