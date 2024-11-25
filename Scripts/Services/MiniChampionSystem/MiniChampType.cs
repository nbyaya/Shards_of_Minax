using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.MiniChamps
{
    public enum MiniChampType
    {
        CrimsonVeins,
        FairyDragonLair,
        AbyssalLair,
        DiscardedCavernClanRibbon,
        DiscardedCavernClanScratch,
        DiscardedCavernClanChitter,
        PassageofTears,
        LandsoftheLich,
        SecretGarden,
        FireTemple,
        EnslavedGoblins,
        SkeletalDragon,
        LavaCaldera,
		Encounter1,
		Encounter2,
		Encounter3,
		Encounter4,
		Encounter5,
		Encounter6,
		Encounter7,
		Encounter8,
		Encounter9,
		Encounter10,
		Encounter11,
		Encounter12,
		Encounter13,
		Encounter14,
		Encounter15,
		Encounter16,
		Encounter17,
		Encounter18,
		Encounter19,
		Encounter20,
		Encounter21,
		Encounter22,
		Encounter23,
		Encounter24,
		Encounter25,
		Encounter26,
		Encounter27,
		Encounter28,
		Encounter29,
		Encounter30,
		Encounter31,
		Encounter32,
		Encounter33,
		Encounter34,
		Encounter35,
		Encounter36,
		Encounter37,
		Encounter38,
		Encounter39,
		Encounter40,
		Encounter41,
		Encounter42,
		Encounter43,
		Encounter44,
		Encounter45,
		Encounter46,
		Encounter47,
		Encounter48,
		Encounter49,
		Encounter50,
		Encounter51,
		Encounter52,
		Encounter53,
		Encounter54,
		Encounter55,
		Encounter56,
		Encounter57,
		Encounter58,
		Encounter59,
		Encounter60,
		Encounter61,
		Encounter62,
		Encounter63,
		Encounter64,
		Encounter65,
		Encounter66,
		Encounter67,
		Encounter68,
		Encounter69,
		Encounter70,
		Encounter71,
		Encounter72,
		Encounter73,
		Encounter74,
		Encounter75,
		Encounter76,
		Encounter77,
		Encounter78,
		Encounter79,
		Encounter80,
		Encounter81,
		Encounter82,
		Encounter83,
		Encounter84,
		Encounter85,
		Encounter86,
		Encounter87,
		Encounter88,
		Encounter89,
		Encounter90,
		Encounter91,
		Encounter92,
		Encounter93,
		Encounter94,
		Encounter95,
		Encounter96,
		Encounter97,
		Encounter98,
		Encounter99,
		Encounter100,
		Encounter101,
		Encounter102,
		Encounter103,
		Encounter104,
		Encounter105,
		Encounter106,
		Encounter107,
		Encounter108,
		Encounter109,
		Encounter110,
		Encounter111,
		Encounter112,
		Encounter113,
		Encounter114,
		Encounter115,
		Encounter116,
		Encounter117,
		Encounter118,
		Encounter119,
		Encounter120,
		Encounter121,
		Encounter122,
		Encounter123,
		Encounter124,
		Encounter125,
		Encounter126,
		Encounter127,
		Encounter128,
		Encounter129,
		Encounter130,
		Encounter131,
		Encounter132,
		Encounter133,
		Encounter134,
		Encounter135,
		Encounter136,
		Encounter137,
		Encounter138,
		Encounter139,
		Encounter140,
		Encounter141,
		Encounter142,
		Encounter143,
		Encounter144,
		Encounter145,
		Encounter146,
		Encounter147,
		Encounter148,
		Encounter149,
		Encounter150,
		Encounter151,
		Encounter152,
		Encounter153,
		Encounter154,
		Encounter155,
		Encounter156,
		Encounter157,
		Encounter158,
		Encounter159,
		Encounter160,
		Encounter161,
		Encounter162,
		Encounter163,
		Encounter164,
		Encounter165,
		Encounter166,
		Encounter167,
		Encounter168,
		Encounter169,
		Encounter170,
		Encounter171,
		Encounter172,
		Encounter173,
		Encounter174,
		Encounter175,
		Encounter176,
		Encounter177,
		Encounter178,
		Encounter179,
		Encounter180,
		Encounter181,
		Encounter182,
		Encounter183,
		Encounter184,
		Encounter185,
		Encounter186,
		Encounter187,
		Encounter188,
		Encounter189,
		Encounter190,
		Encounter191,
		Encounter192,
		Encounter193,
		Encounter194,
		Encounter195,
		Encounter196,
		Encounter197,
		Encounter198,
		Encounter199,
		Encounter200,
		Encounter201,
		Encounter202,
		Encounter203,
		Encounter204,
		Encounter205,
		Encounter206,
		Encounter207,
		Encounter208,
		Encounter209,
		Encounter210,
		Encounter211,
		Encounter212,
		Encounter213,
		Encounter214,
		Encounter215,
		Encounter216,
		Encounter217,
		Encounter218,
		Encounter219,
		Encounter220,
		Encounter221,
		Encounter222,
		Encounter223,
		Encounter224,
		Encounter225,
		Encounter226,
		Encounter227,
		Encounter228,
		Encounter229,
		Encounter230,
		Encounter231,
		Encounter232,
		Encounter233,
		Encounter234,
		Encounter235,
		Encounter236,
		Encounter237,
		Encounter238,
		Encounter239,
		Encounter240,
		Encounter241,
		Encounter242,
		Encounter243,
		Encounter244,
		Encounter245,
		Encounter246,
		Encounter247,
		Encounter248,
		Encounter249,
		Encounter250,
		Encounter251,
		Encounter252,
		Encounter253,
		Encounter254,
		Encounter255,
		Encounter256,
		Encounter257,
		Encounter258,
		Encounter259,
		Encounter260,
		Encounter261,
		Encounter262,
		Encounter263,
		Encounter264,
		Encounter265,
		Encounter266,
		Encounter267,
		Encounter268,
		Encounter269,
		Encounter270,
		Encounter271,
		Encounter272,
		Encounter273,
		Encounter274,
		Encounter275,
		Encounter276,
		Encounter277,
		Encounter278,
		Encounter279,
		Encounter280,
		Encounter281,
		Encounter282,
		Encounter283,
		Encounter284,
		Encounter285,
		Encounter286,
		Encounter287,
		Encounter288,
		Encounter289,
		Encounter290,
		Encounter291,
		Encounter292,
		Encounter293,
		Encounter294,
		Encounter295,
		Encounter296,
		Encounter297,
		Encounter298,
		Encounter299,
		Encounter300,
		Encounter301,
		Encounter302,
		Encounter303,
		Encounter304,
		Encounter305,
		Encounter306,
		Encounter307,
		Encounter308,
		Encounter309,
		Encounter310,
		Encounter311,
		Encounter312,
		Encounter313,
		Encounter314,
		Encounter315,
		Encounter316,
		Encounter317,
		Encounter318,
		Encounter319,
		Encounter320,
		Encounter321,
		Encounter322,
		Encounter323,
		Encounter324,
		Encounter325,
		Encounter326,
		Encounter327,
		Encounter328,
		Encounter329,
		Encounter330,
		Encounter331,
		Encounter332,
		Encounter333,
		Encounter334,
		Encounter335,
		Encounter336,
		Encounter337,
		Encounter338,
		Encounter339,
		Encounter340,
		Encounter341,
		Encounter342,
		Encounter343,
		Encounter344,
		Encounter345,
		Encounter346,
		Encounter347,
		Encounter348,
		Encounter349,
		Encounter350,
		Encounter351,
		Encounter352,
		Encounter353,
		Encounter354,
		Encounter355,
		Encounter356,
		Encounter357,
		Encounter358,
		Encounter359,
		Encounter360,
		Encounter361,
		Encounter362,
		Encounter363,
		Encounter364,
		Encounter365,
		Encounter366,
		Encounter367,
		Encounter368,
		Encounter369,
		Encounter370,
		Encounter371,
		Encounter372,
		Encounter373,
		Encounter374,
		Encounter375,
		Encounter376,
		Encounter377,
		Encounter378,
		Encounter379,
		Encounter380,
		Encounter381,
		Encounter382,
		Encounter383,
		Encounter384,
		Encounter385,
		Encounter386,
		Encounter387,
		Encounter388,
		Encounter389,
		Encounter390,
		Encounter391,
		Encounter392,
		Encounter393,
		Encounter394,
		Encounter395,
		Encounter396,
		Encounter397,
		Encounter398,
		Encounter399,
		Encounter400,
		Encounter401,
		Encounter402,
		Encounter403,
		Encounter404,
		Encounter405,
		Encounter406,
		Encounter407,
		Encounter408,
		Encounter409,
		Encounter410,
		Encounter411,
		Encounter412,
		Encounter413,
		Encounter414,
		Encounter415,
		Encounter416,
		Encounter417,
		Encounter418,
		Encounter419,
		Encounter420,
		Encounter421,
		Encounter422,
		Encounter423,
		Encounter424,
		Encounter425,
		Encounter426,
		Encounter427,
		Encounter428,
		Encounter429,
		Encounter430,
		Encounter431,
		Encounter432,
		Encounter433,
		Encounter434,
		Encounter435,
		Encounter436,
		Encounter437,
		Encounter438,
		Encounter439,
		Encounter440,
		Encounter441,
		Encounter442,
		Encounter443,
		Encounter444,
		Encounter445,
		Encounter446,
		Encounter447,
		Encounter448,
		Encounter449,
		Encounter450,
		Encounter451,
		Encounter452,
		Encounter453,
		Encounter454,
		Encounter455,
		Encounter456,
		Encounter457,
		Encounter458,
		Encounter459,
		Encounter460,
		Encounter461,
		Encounter462,
		Encounter463,
		Encounter464,
		Encounter465,
		Encounter466,
		Encounter467,
		Encounter468,
		Encounter469,
		Encounter470,
		Encounter471,
		Encounter472,
		Encounter473,
		Encounter474,
		Encounter475,
		Encounter476,
		Encounter477,
		Encounter478,
		Encounter479,
		Encounter480,
		Encounter481,
		Encounter482,
		Encounter483,
		Encounter484,
		Encounter485,
		Encounter486,
		Encounter487,
		Encounter488,
		Encounter489,
		Encounter490,
		Encounter491,
		Encounter492,
		Encounter493,
		Encounter494,
		Encounter495,
		Encounter496,
		Encounter497,
		Encounter498,
		Encounter499,
		Encounter500,
		Encounter501,
		Encounter502,
		Encounter503,
		Encounter504,
		Encounter505,
		Encounter506,
		Encounter507,
		Encounter508,
		Encounter509,
		Encounter510,
		Encounter511,
		Encounter512,
		Encounter513,
		Encounter514,
		Encounter515,
		Encounter516,
		Encounter517,
		Encounter518,
		Encounter519,
		Encounter520,
		Encounter521,
		Encounter522,
		Encounter523,
		Encounter524,
		Encounter525,
		Encounter526,
		Encounter527,
		Encounter528,
		Encounter529,
		Encounter530,
		Encounter531,
		Encounter532,
		Encounter533,
		Encounter534,
		Encounter535,
		Encounter536,
		Encounter537,
		Encounter538,
		Encounter539,
		Encounter540,
		Encounter541,
		Encounter542,
		Encounter543,
		Encounter544,
		Encounter545,
		Encounter546,
		Encounter547,
		Encounter548,
		Encounter549,
		Encounter550,
		Encounter551,
		Encounter552,
		Encounter553,
		Encounter554,
		Encounter555,
		Encounter556,
		Encounter557,
		Encounter558,
		Encounter559,
		Encounter560,
		Encounter561,
		Encounter562,
		Encounter563,
		Encounter564,
		Encounter565,
		Encounter566,
		Encounter567,
		Encounter568,
		Encounter569,
		Encounter570,
		Encounter571,
		Encounter572,
		Encounter573,
		Encounter574,
		Encounter575,
		Encounter576,
		Encounter577,
		Encounter578,
		Encounter579,
		Encounter580,
		Encounter581,
		Encounter582,
		Encounter583,
		Encounter584,
		Encounter585,
		Encounter586,
		Encounter587,
		Encounter588,
		Encounter589,
		Encounter590,
		Encounter591,
		Encounter592,
		Encounter593,
		Encounter594,
		Encounter595,
		Encounter596,
		Encounter597,
		Encounter598,
		Encounter599,
		Encounter600,
		Encounter601,
		Encounter602,
		Encounter603,
		Encounter604,
		Encounter605,
		Encounter606,
		Encounter607,
		Encounter608,
		Encounter609,
		Encounter610,
		Encounter611,
		Encounter612,
		Encounter613,
		Encounter614,
		Encounter615,
		Encounter616,
		Encounter617,
		Encounter618,
		Encounter619,
		Encounter620,
		Encounter621,
		Encounter622,
		Encounter623,
		Encounter624,
		Encounter625,
		Encounter626,
		Encounter627,
		Encounter628,
		Encounter629,
		Encounter630,
		Encounter631,
		Encounter632,
		Encounter633,
		Encounter634,
		Encounter635,
		Encounter636,
		Encounter637,
		Encounter638,
		Encounter639,
		Encounter640,
		Encounter641,
		Encounter642,
		Encounter643,
		Encounter644,
		Encounter645,
		Encounter646,
		Encounter647,
		Encounter648,
		Encounter649,
		Encounter650,
		Encounter651,
		Encounter652,
		Encounter653,
		Encounter654,
		Encounter655,
		Encounter656,
		Encounter657,
		Encounter658,
		Encounter659,
		Encounter660,
		Encounter661,
		Encounter662,
		Encounter663,
		Encounter664,
		Encounter665,
		Encounter666,
		Encounter667,
		Encounter668,
		Encounter669,
		Encounter670,
		Encounter671,
		Encounter672,
		Encounter673,
		Encounter674,
		Encounter675,
		Encounter676,
		Encounter677,
		Encounter678,
		Encounter679,
		Encounter680,
		Encounter681,
		Encounter682,
		Encounter683,
		Encounter684,
		Encounter685,
		Encounter686,
		Encounter687,
		Encounter688,
		Encounter689,
		Encounter690,
		Encounter691,
		Encounter692,
		Encounter693,
		Encounter694,
		Encounter695,
		Encounter696,
		Encounter697,
		Encounter698,
		Encounter699,
		Encounter700,
		Encounter701,
		Encounter702,
		Encounter703,
		Encounter704,
		Encounter705,
		Encounter706,
		Encounter707,
		Encounter708,
		Encounter709,
		Encounter710,
		Encounter711,
		Encounter712,
		Encounter713,
		Encounter714,
		Encounter715,
		Encounter716,
		Encounter717,
		Encounter718,
		Encounter719,
		Encounter720,
		Encounter721,
		Encounter722,
		Encounter723,
		Encounter724,
		Encounter725,
		Encounter726,
		Encounter727,
		Encounter728,
		Encounter729,
		Encounter730,
		Encounter731,
		Encounter732,
		Encounter733,
		Encounter734,
		Encounter735,
		Encounter736,
		Encounter737,
		Encounter738,
		Encounter739,
		Encounter740,
		Encounter741,
		Encounter742,
		Encounter743,
		Encounter744,
		Encounter745,
		Encounter746,
		Encounter747,
		Encounter748,
		Encounter749,
		Encounter750,
		Encounter751,
		Encounter752,
		Encounter753,
		Encounter754,
		Encounter755,
        MeraktusTheTormented
    }

    public class MiniChampTypeInfo
    {
        public int Required { get; set; }
        public Type SpawnType { get; set; }

        public MiniChampTypeInfo(int required, Type spawnType)
        {
            Required = required;
            SpawnType = spawnType;
        }
    }

    public class MiniChampLevelInfo
    {
        public MiniChampTypeInfo[] Types { get; set; }

        public MiniChampLevelInfo(params MiniChampTypeInfo[] types)
        {
            Types = types;
        }
    }

    public class MiniChampInfo
    {
        public MiniChampLevelInfo[] Levels { get; set; }
        public Type EssenceType { get; set; }

        public int MaxLevel { get { return Levels == null ? 0 : Levels.Length; } }

        public MiniChampInfo(Type essenceType, params MiniChampLevelInfo[] levels)
        {
            Levels = levels;
            EssenceType = essenceType;
        }

        public MiniChampLevelInfo GetLevelInfo(int level)
        {
            level--;

            if (level >= 0 && level < Levels.Length)
                return Levels[level];

            return null;
        }

        public static MiniChampInfo[] Table { get { return m_Table; } }

        private static readonly MiniChampInfo[] m_Table = new MiniChampInfo[]
        {
            new MiniChampInfo // Crimson Veins
            (
                typeof(EssencePrecision),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(20, typeof(FireAnt)),
                    new MiniChampTypeInfo(10, typeof(LavaSnake)),
                    new MiniChampTypeInfo(10, typeof(LavaLizard))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(5, typeof(Efreet)),
                    new MiniChampTypeInfo(5, typeof(FireGargoyle))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(10, typeof(LavaElemental)),
                    new MiniChampTypeInfo(5, typeof(FireDaemon))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(FireElementalRenowned))
                )
            ),
            new MiniChampInfo // Fairy Dragon Lair
            (
                typeof(EssenceDiligence),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(25, typeof(FairyDragon))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(Wyvern))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(10, typeof(ForgottenServant))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(WyvernRenowned))
                )
            ),
            new MiniChampInfo // Abyssal Lair
            (
                typeof(EssenceAchievement),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(20, typeof(GreaterMongbat)),
                    new MiniChampTypeInfo(20, typeof(Imp))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(Daemon))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(PitFiend))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(DevourerRenowned))
                )
            ),
            new MiniChampInfo // Discarded Cavern Clan Ribbon
            (
                typeof(EssenceBalance),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(ClanRibbonPlagueRat)),
                    new MiniChampTypeInfo(10, typeof(ClanRS))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(ClanRibbonPlagueRat)),
                    new MiniChampTypeInfo(10, typeof(ClanRC))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(VitaviRenowned))
                )
            ),
            new MiniChampInfo // Discarded Cavern Clan Scratch
            (
                typeof(EssenceBalance),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(ClanSSW)),
                    new MiniChampTypeInfo(10, typeof(ClanSS))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(ClanSSW)),
                    new MiniChampTypeInfo(10, typeof(ClanSH))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(TikitaviRenowned))
                )
            ),
            new MiniChampInfo // Discarded Cavern Clan Chitter
            (
                typeof(EssenceBalance),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
                    new MiniChampTypeInfo(10, typeof(ClanCA))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
                    new MiniChampTypeInfo(10, typeof(ClanCT))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(RakktaviRenowned))
                )
            ),
            new MiniChampInfo // Passage of Tears
            (
                typeof(EssenceSingularity),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(AcidSlug)),
                    new MiniChampTypeInfo(20, typeof(CorrosiveSlime))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(AcidElemental))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(3, typeof(InterredGrizzle))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(AcidElementalRenowned))
                )
            ),
            new MiniChampInfo // Lands of the Lich
            (
                typeof(EssenceDirection),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(5, typeof(Wraith)),
                    new MiniChampTypeInfo(10, typeof(Spectre)),
                    new MiniChampTypeInfo(5, typeof(Shade)),
                    new MiniChampTypeInfo(30, typeof(Skeleton)),
                    new MiniChampTypeInfo(20, typeof(Zombie))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(5, typeof(BoneMagi)),
                    new MiniChampTypeInfo(10, typeof(SkeletalMage)),
                    new MiniChampTypeInfo(10, typeof(BoneKnight)),
                    new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
                    new MiniChampTypeInfo(10, typeof(WailingBanshee))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(SkeletalLich)),
                    new MiniChampTypeInfo(20, typeof(RottingCorpse))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(AncientLichRenowned))
                )
            ),
            new MiniChampInfo // Secret Garden
            (
                typeof(EssenceFeeling),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(20, typeof(Pixie))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(15, typeof(Wisp))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(10, typeof(DarkWisp))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(PixieRenowned))
                )
            ),
            new MiniChampInfo // Fire Temple Ruins
            (
                typeof(EssenceOrder),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(20, typeof(LavaSnake)),
                    new MiniChampTypeInfo(10, typeof(LavaLizard)),
                    new MiniChampTypeInfo(10, typeof(FireAnt))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(LavaSerpent)),
                    new MiniChampTypeInfo(10, typeof(HellCat)),
                    new MiniChampTypeInfo(10, typeof(HellHound))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(FireDaemon)),
                    new MiniChampTypeInfo(10, typeof(LavaElemental))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(FireDaemonRenowned))
                )
            ),
            new MiniChampInfo // Enslaved Goblins
            (
                typeof(EssenceControl),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(EnslavedGrayGoblin)),
                    new MiniChampTypeInfo(15, typeof(EnslavedGreenGoblin))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(EnslavedGoblinScout)),
                    new MiniChampTypeInfo(10, typeof(EnslavedGoblinKeeper))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(EnslavedGoblinMage)),
                    new MiniChampTypeInfo(5, typeof(EnslavedGreenGoblinAlchemist))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(GrayGoblinMageRenowned)),
                    new MiniChampTypeInfo(1, typeof(GreenGoblinAlchemistRenowned))
                )
            ),
            new MiniChampInfo // Skeletal Dragon
            (
                typeof(EssencePersistence),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(5, typeof(PatchworkSkeleton)),
                    new MiniChampTypeInfo(15, typeof(Skeleton))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(5, typeof(BoneKnight)),
                    new MiniChampTypeInfo(5, typeof(SkeletalKnight))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(BoneMagi)),
                    new MiniChampTypeInfo(2, typeof(SkeletalMage))
                ),
                new MiniChampLevelInfo // Level 4
                (
                    new MiniChampTypeInfo(2, typeof(SkeletalLich))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(SkeletalDragonRenowned))
                )
            ),
            new MiniChampInfo // Lava Caldera
            (
                typeof(EssencePassion),
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(10, typeof(LavaSnake)),
                    new MiniChampTypeInfo(10, typeof(LavaLizard)),
                    new MiniChampTypeInfo(20, typeof(FireAnt))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(10, typeof(LavaSerpent)),
                    new MiniChampTypeInfo(10, typeof(HellCat)),
                    new MiniChampTypeInfo(10, typeof(HellHound))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(5, typeof(FireDaemon)),
                    new MiniChampTypeInfo(10, typeof(LavaElemental))
                ),
                new MiniChampLevelInfo // Renowned
                (
                    new MiniChampTypeInfo(1, typeof(FireDaemonRenowned))
                )
            ),
            new MiniChampInfo // Meraktus the Tormented
            (
                null,
                new MiniChampLevelInfo // Level 1
                (
                    new MiniChampTypeInfo(20, typeof(Minotaur))
                ),
                new MiniChampLevelInfo // Level 2
                (
                    new MiniChampTypeInfo(20, typeof(MinotaurScout))
                ),
                new MiniChampLevelInfo // Level 3
                (
                    new MiniChampTypeInfo(15, typeof(MinotaurCaptain))
                ),
                new MiniChampLevelInfo // Level 4
                (
                    new MiniChampTypeInfo(15, typeof(MinotaurCaptain))
                ),
                new MiniChampLevelInfo // Champion
                (
                    new MiniChampTypeInfo(1, typeof(Meraktus))
                )
            ),
		new MiniChampInfo // Anvil Hurler Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(IronSmith)),
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BronzeElemental)),
				new MiniChampTypeInfo(5, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AnvilHurlerBoss))
			)
		),
		new MiniChampInfo // Aquatic Tamer Lagoon
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(15, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Kraken)),
				new MiniChampTypeInfo(10, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AquaticTamerBoss))
			)
		),
		new MiniChampInfo // Arcane Scribe Enclave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(ElementalWizard))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ArcaneScribeBoss))
			)
		),
		new MiniChampInfo // Arctic Naturalist Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IceSnake)),
				new MiniChampTypeInfo(10, typeof(IceElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ArcticNaturalistBoss))
			)
		),
		new MiniChampInfo // Armor Curer Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(BattleDressmaker)),
				new MiniChampTypeInfo(15, typeof(ArrowFletcher)),
				new MiniChampTypeInfo(15, typeof(GemCutter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SlimeMage)),
				new MiniChampTypeInfo(10, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ArmorCurerBoss))
			)
		),
		new MiniChampInfo // Arrow Fletcher's Roost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(CrossbowMarksman)),
				new MiniChampTypeInfo(15, typeof(LongbowSniper)),
				new MiniChampTypeInfo(10, typeof(JavelinAthlete))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShieldBearer)),
				new MiniChampTypeInfo(5, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ArrowFletcherBoss))
			)
		),
		new MiniChampInfo // Ascetic Hermit's Refuge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SpiritMedium)),
				new MiniChampTypeInfo(15, typeof(ZenMonk)),
				new MiniChampTypeInfo(10, typeof(QiGongHealer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AsceticHermitBoss))
			)
		),
		new MiniChampInfo // Astrologer's Observatory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScrollMage)),
				new MiniChampTypeInfo(15, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(FireMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AstrologerBoss))
			)
		),
		new MiniChampInfo // Banneret's Bastion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwordDefender)),
				new MiniChampTypeInfo(15, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(10, typeof(KnightOfJustice))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RapierDuelist)),
				new MiniChampTypeInfo(10, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(5, typeof(KnightOfValor))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BanneretBoss))
			)
		),
		new MiniChampInfo // Battle Dressmaker's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GemCutter)),
				new MiniChampTypeInfo(15, typeof(BattleWeaver)),
				new MiniChampTypeInfo(10, typeof(ToxicologistChef))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WoolWeaver)),
				new MiniChampTypeInfo(10, typeof(AnvilHurler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BattlefieldHealer)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BattleDressmakerBoss))
			)
		),
		new MiniChampInfo // Battlefield Healer's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CombatMedic)),
				new MiniChampTypeInfo(15, typeof(CombatNurse)),
				new MiniChampTypeInfo(10, typeof(FieldCommander))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(QiGongHealer)),
				new MiniChampTypeInfo(5, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BattlefieldHealerBoss))
			)
		),
		new MiniChampInfo // Battle Herbalist Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkWisp)),
				new MiniChampTypeInfo(5, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BattleHerbalistBoss))
			)
		),
		new MiniChampInfo // Battle Storm Caller's Eye
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(StormConjurer)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(ElementalWizard))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AirElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BattleHerbalistBoss))
			)
		),
		new MiniChampInfo // Battle Weaver Loom
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BattleWeaver)),
				new MiniChampTypeInfo(15, typeof(WoolWeaver)),
				new MiniChampTypeInfo(10, typeof(ArrowFletcher))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GemCutter)),
				new MiniChampTypeInfo(10, typeof(ToxicologistChef))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BattleWeaverBoss))
			)
		),
		new MiniChampInfo // Beastmaster's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Lion)),
				new MiniChampTypeInfo(15, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Cougar)),
				new MiniChampTypeInfo(10, typeof(BigCatTamer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(Beastmaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BeastmasterBoss))
			)
		),
		new MiniChampInfo // Big Cat Tamer Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Cougar)),
				new MiniChampTypeInfo(10, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DireWolf)),
				new MiniChampTypeInfo(10, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WhiteWolf)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BigCatTamerBoss))
			)
		),
		new MiniChampInfo // Biologist’s Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PestilentBandage)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BiologistBoss))
			)
		),
		new MiniChampInfo // Bird Trainer’s Aviary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Bird)),
				new MiniChampTypeInfo(10, typeof(Chicken)),
				new MiniChampTypeInfo(10, typeof(Macaw))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Phoenix)),
				new MiniChampTypeInfo(5, typeof(Crane))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BirdTrainerBoss))
			)
		),
		new MiniChampInfo // Bone Shielder Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneKnight)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BoneShielderBoss))
			)
		),
		new MiniChampInfo // Boomerang Thrower Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CrossbowMarksman)),
				new MiniChampTypeInfo(15, typeof(JavelinAthlete))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BoomerangThrowerBoss))
			)
		),
		new MiniChampInfo // Cabinet Maker's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(BattleWeaver)),
				new MiniChampTypeInfo(10, typeof(WoolWeaver))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AnvilHurler)),
				new MiniChampTypeInfo(10, typeof(GemCutter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IronSmith)),
				new MiniChampTypeInfo(5, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CabinetMakerBoss))
			)
		),
		new MiniChampInfo // Carver's Atelier
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ArrowFletcher)),
				new MiniChampTypeInfo(15, typeof(BattleDressmaker)),
				new MiniChampTypeInfo(10, typeof(GemCutter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(AnvilHurler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(5, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CarverBoss))
			)
		),
		new MiniChampInfo // Chemist's Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(SlimeMage)),
				new MiniChampTypeInfo(10, typeof(ToxicologistChef))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(BloodElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ChemistBoss))
			)
		),
		new MiniChampInfo // Choir Singer's Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Flutist)),
				new MiniChampTypeInfo(10, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ChoirSingerBoss))
			)
		),
		new MiniChampInfo // Clockwork Engineer's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IronSmith)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ClockworkEngineerBoss))
			)
		),
		new MiniChampInfo // Clue Seeker's Puzzle Grounds
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SafeCracker)),
				new MiniChampTypeInfo(5, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ClueSeekerBoss))
			)
		),
		new MiniChampInfo // Combat Medic's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FieldCommander)),
				new MiniChampTypeInfo(10, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(10, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer)),
				new MiniChampTypeInfo(10, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(QiGongHealer)),
				new MiniChampTypeInfo(5, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CombatMedicBoss))
			)
		),
		new MiniChampInfo // Combat Nurse's Recovery Ward
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CombatNurse)),
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HolyKnight)),
				new MiniChampTypeInfo(5, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CombatNurseBoss))
			)
		),
		new MiniChampInfo // Con Artist's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapSetter)),
				new MiniChampTypeInfo(5, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ConArtistBoss))
			)
		),
		new MiniChampInfo // Contortionist's Circus
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(5, typeof(MushroomWitch))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ContortionistBoss))
			)
		),
		new MiniChampInfo // Crime Scene Tech Investigation
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(15, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NoirDetective)),
				new MiniChampTypeInfo(5, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CrimeSceneTechBoss))
			)
		),
		new MiniChampInfo // Crossbow Marksman Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LongbowSniper)),
				new MiniChampTypeInfo(15, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(BoomerangThrower))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CrossbowMarksmanBoss))
			)
		),
		new MiniChampInfo // Crying Orphan Refuge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(15, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CryingOrphanBoss))
			)
		),
		new MiniChampInfo // Cryptologist's Chamber
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkElf)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CryptologistBoss))
			)
		),
		new MiniChampInfo // Dark Sorcerer Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DarkSorcererBoss))
			)
		),
		new MiniChampInfo // Death Cultist Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(15, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(GhostScout))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WailingBanshee)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Revenant)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DeathCultistBoss))
			)
		),
		new MiniChampInfo // Decoy Deployer Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Infiltrator)),
				new MiniChampTypeInfo(5, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DecoyDeployerBoss))
			)
		),
		new MiniChampInfo // Deep Miner Excavation
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Golem)),
				new MiniChampTypeInfo(15, typeof(IronSmith)),
				new MiniChampTypeInfo(10, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BronzeElemental)),
				new MiniChampTypeInfo(10, typeof(DullCopperElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(ValoriteElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DeepMinerBoss))
			)
		),
		new MiniChampInfo // Demolition Expert Quarry
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapEngineer)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DemolitionExpertBoss))
			)
		),
		new MiniChampInfo // Desert Naturalist Oasis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(Lizardman)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Lizardman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Yamandon)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DesertNaturalistBoss))
			)
		),
		new MiniChampInfo // Desert Tracker's Oasis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(EarthElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EarthElemental)),
				new MiniChampTypeInfo(10, typeof(CopperElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Yamandon)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DesertTrackerBoss))
			)
		),
		new MiniChampInfo // Diplomat's Parley
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DiplomatBoss))
			)
		),
		new MiniChampInfo // Disguise Master's Haven
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapSetter)),
				new MiniChampTypeInfo(5, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DisguiseMasterBoss))
			)
		),
		new MiniChampInfo // Diviner's Peak
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ZenMonk)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PsychedelicShaman)),
				new MiniChampTypeInfo(10, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StormConjurer)),
				new MiniChampTypeInfo(5, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DivinerBoss))
			)
		),
		new MiniChampInfo // Drum Boy's Spectacle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Flutist)),
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DrumBoyBoss))
			)
		),
		new MiniChampInfo // Drummer's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Flutist)),
				new MiniChampTypeInfo(15, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RaveRogue)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DrummerBoss))
			)
		),
		new MiniChampInfo // Dual Wielder Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(15, typeof(SabreFighter)),
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DualWielderBoss))
			)
		),
		new MiniChampInfo // Earth Alchemist's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SlimeMage)),
				new MiniChampTypeInfo(15, typeof(ToxicologistChef)),
				new MiniChampTypeInfo(10, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(DullCopperElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EarthAlchemistBoss))
			)
		),
		new MiniChampInfo // Electrician's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LightningBearer)),
				new MiniChampTypeInfo(15, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ElectricianBoss))
			)
		),
		new MiniChampInfo // Elemental Wizard's Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(FireElemental)),
				new MiniChampTypeInfo(10, typeof(IceElemental)),
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AirElemental)),
				new MiniChampTypeInfo(10, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ElementalWizardBoss))
			)
		),
		new MiniChampInfo // Enchanter's Labyrinth
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Magician)),
				new MiniChampTypeInfo(15, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EnchanterBoss))
			)
		),
		new MiniChampInfo // Epee Specialist Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FencingMaster)),
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SabreFighter)),
				new MiniChampTypeInfo(5, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EpeeSpecialistBoss))
			)
		),
		new MiniChampInfo // Escape Artist Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(Spy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EscapeArtistBoss))
			)
		),
		new MiniChampInfo // Evidence Analyst's Bureau
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(15, typeof(NoirDetective)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EvidenceAnalystBoss))
			)
		),
		new MiniChampInfo // Evil Map Maker's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(15, typeof(ArrowFletcher)),
				new MiniChampTypeInfo(10, typeof(GemCutter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EvilMapMakerBoss))
			)
		),
		new MiniChampInfo // Explorer's Expedition
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(UrbanTracker)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwampThing)),
				new MiniChampTypeInfo(5, typeof(Treefellow))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ExplorerBoss))
			)
		),
		new MiniChampInfo // Explosive Demolitionist's Foundry
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(OrcBomber)),
				new MiniChampTypeInfo(15, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ExplosiveDemolitionistBoss))
			)
		),
		new MiniChampInfo // Feast Master's Banquet
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(Lion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantToad)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FeastMasterBoss))
			)
		),
		new MiniChampInfo // Fencing Master's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FencingMaster)),
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(5, typeof(LongbowSniper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FencingMasterBoss))
			)
		),
		new MiniChampInfo // Field Commander Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfValor)),
				new MiniChampTypeInfo(15, typeof(Banneret)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FieldCommander)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SabreFighter)),
				new MiniChampTypeInfo(5, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FieldCommanderBoss))
			)
		),
		new MiniChampInfo // Field Medic Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CombatMedic)),
				new MiniChampTypeInfo(15, typeof(CombatNurse)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WardCaster)),
				new MiniChampTypeInfo(10, typeof(QiGongHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SpiritMedium)),
				new MiniChampTypeInfo(5, typeof(ZenMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FieldMedicBoss))
			)
		),
		new MiniChampInfo // Fire Alchemist Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(15, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaElemental)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FireAlchemistBoss))
			)
		),
		new MiniChampInfo // Fire Mage Conclave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireMage)),
				new MiniChampTypeInfo(10, typeof(Magician)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(Efreet))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Balron)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FireMageBoss))
			)
		),
		new MiniChampInfo // Firestarter Pyre
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireAnt)),
				new MiniChampTypeInfo(15, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireGargoyle)),
				new MiniChampTypeInfo(10, typeof(FireElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Efreet)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FirestarterBoss))
			)
		),
		new MiniChampInfo // Flame Welder Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireGargoyle)),
				new MiniChampTypeInfo(10, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(Efreet))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FlameWelderBoss))
			)
		),
		new MiniChampInfo // Forager's Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(AppleElemental)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestRanger)),
				new MiniChampTypeInfo(5, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForagerBoss))
			)
		),
		new MiniChampInfo // Forensic Analyst's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(5, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForensicAnalystBoss))
			)
		),
		new MiniChampInfo // Forest Minstrel's Glen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SatyrPiper)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForestMinstrelBoss))
			)
		),
		new MiniChampInfo // Forest Scout Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(UrbanTracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForestScoutBoss))
			)
		),
		new MiniChampInfo // Forest Tracker Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(15, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(UrbanTracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SatyrPiper)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForestTrackerBoss))
			)
		),
		new MiniChampInfo // Gem Cutter Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GemCutter)),
				new MiniChampTypeInfo(10, typeof(CrystalElemental)),
				new MiniChampTypeInfo(10, typeof(BronzeElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(CopperElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GemCutterBoss))
			)
		),
		new MiniChampInfo // Ghost Scout Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GhostScout)),
				new MiniChampTypeInfo(10, typeof(RestlessSoul)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GhostScoutBoss))
			)
		),
		new MiniChampInfo // Ghost Warrior Battlefield
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GhostWarrior)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GhostWarriorBoss))
			)
		),
		new MiniChampInfo // Gourmet Chef Kitchen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ToxicologistChef)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MushroomWitch)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlimeMage)),
				new MiniChampTypeInfo(5, typeof(BogThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GourmetChefBoss))
			)
		),
		new MiniChampInfo // Grave Digger Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Zombie)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wight)),
				new MiniChampTypeInfo(10, typeof(Ghoul))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GraveDiggerBoss))
			)
		),
		new MiniChampInfo // Greco-Roman Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(15, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KnightOfValor)),
				new MiniChampTypeInfo(5, typeof(KnightOfJustice))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GrecoRomanWrestlerBoss))
			)
		),
		new MiniChampInfo // Grill Master Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaSnake)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HellHound)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaElemental)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GrillMasterBoss))
			)
		),
		new MiniChampInfo // Hammer Guard Armory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RapierDuelist)),
				new MiniChampTypeInfo(15, typeof(SabreFighter)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AnvilHurler)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IronSmith)),
				new MiniChampTypeInfo(5, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HammerGuardBoss))
			)
		),
		new MiniChampInfo // Harpist's Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ForestRanger)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(Pixie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DarkWisp)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyQueen)),
				new MiniChampTypeInfo(5, typeof(Wisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HarpistBoss))
			)
		),
		new MiniChampInfo // Herbalist Poisoner Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonElemental)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BogThing)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HerbalistPoisonerBoss))
			)
		),
		new MiniChampInfo // Ice Sorcerer Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(IceSnake)),
				new MiniChampTypeInfo(15, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IceElemental)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IceSorcererBoss))
			)
		),
		new MiniChampInfo // Illusionist's Labyrinth
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkElf)),
				new MiniChampTypeInfo(5, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IllusionistBoss))
			)
		),
		new MiniChampInfo // Infiltrator's Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(Spy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(5, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfiltratorBoss))
			)
		),
		new MiniChampInfo // Invisible Saboteur's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Saboteur)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InvisibleSaboteurBoss))
			)
		),
		new MiniChampInfo // Iron Smith Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DullCopperElemental)),
				new MiniChampTypeInfo(15, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(10, typeof(BronzeElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CrystalElemental)),
				new MiniChampTypeInfo(10, typeof(GoldenElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ValoriteElemental)),
				new MiniChampTypeInfo(5, typeof(VeriteElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IronSmithBoss))
			)
		),
		new MiniChampInfo // Javelin Athlete Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScoutNinja)),
				new MiniChampTypeInfo(15, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(CrossbowMarksman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoomerangThrower)),
				new MiniChampTypeInfo(10, typeof(LongbowSniper))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SabreFighter)),
				new MiniChampTypeInfo(5, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JavelinAthleteBoss))
			)
		),
		new MiniChampInfo // Joiner Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(15, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(AnvilHurler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GemCutter)),
				new MiniChampTypeInfo(10, typeof(BattleWeaver))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(5, typeof(ArrowFletcher))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JoinerBoss))
			)
		),
		new MiniChampInfo // Jungle Naturalist Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SerpentHandler)),
				new MiniChampTypeInfo(15, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenHag)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JungleNaturalistBoss))
			)
		),
		new MiniChampInfo // Karate Expert Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(10, typeof(Samurai)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenNinja)),
				new MiniChampTypeInfo(5, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KarateExpertBoss))
			)
		),
		new MiniChampInfo // Katana Duelist Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Samurai)),
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kunoichi)),
				new MiniChampTypeInfo(5, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KatanaDuelistBoss))
			)
		),
		new MiniChampInfo // Knife Thrower's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BoomerangThrower)),
				new MiniChampTypeInfo(15, typeof(JavelinAthlete)),
				new MiniChampTypeInfo(10, typeof(CrossbowMarksman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(5, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KnifeThrowerBoss))
			)
		),
		new MiniChampInfo // Knight of Justice Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(15, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KnightOfJusticeBoss))
			)
		),
		new MiniChampInfo // Knight of Mercy Chapel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(15, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer)),
				new MiniChampTypeInfo(10, typeof(QiGongHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ZenMonk)),
				new MiniChampTypeInfo(5, typeof(EtherealWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KnightOfMercyBoss))
			)
		),
		new MiniChampInfo // Knight of Valor Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfValor)),
				new MiniChampTypeInfo(15, typeof(Banneret))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RamRider)),
				new MiniChampTypeInfo(10, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(FencingMaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KnightOfValorBoss))
			)
		),
		new MiniChampInfo // Kunoichi Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScoutNinja)),
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(Samurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(KunoichiBoss))
			)
		),
		new MiniChampInfo // Librarian Custodian's Archive
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RuneCaster)),
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(ScrollMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkWisp)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LibrarianCustodianBoss))
			)
		),
		new MiniChampInfo // Lightning Bearer's Storm Nexus
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(ElementalWizard))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AirElemental)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LightningBearerBoss))
			)
		),
		new MiniChampInfo // Locksmith's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Spy)),
				new MiniChampTypeInfo(5, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LocksmithBoss))
			)
		),
		new MiniChampInfo // Logician's Puzzle Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Magician)),
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LogicianBoss))
			)
		),
		new MiniChampInfo // Longbow Sniper Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CrossbowMarksman)),
				new MiniChampTypeInfo(15, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GrecoRomanWrestler)),
				new MiniChampTypeInfo(5, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LongbowSniperBoss))
			)
		),
		new MiniChampInfo // Luchador Training Grounds
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(15, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SwordDefender)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Samurai)),
				new MiniChampTypeInfo(5, typeof(BoomerangThrower))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LuchadorBoss))
			)
		),
		new MiniChampInfo // Magician's Arcane Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScrollMage)),
				new MiniChampTypeInfo(15, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MagicianBoss))
			)
		),
		new MiniChampInfo // Martial Monk Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(15, typeof(ZenMonk)),
				new MiniChampTypeInfo(10, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MartialMonkBoss))
			)
		),
		new MiniChampInfo // Master Flutist's Concert
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Flutist)),
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GlamRockMage)),
				new MiniChampTypeInfo(5, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MasterFlutist))
			)
		),
		new MiniChampInfo // Thieves' Guild Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(5, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MasterPickpocketBoss))
			)
		),
		new MiniChampInfo // Mechanic's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(15, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(Golem))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SlimeMage)),
				new MiniChampTypeInfo(10, typeof(RenaissanceMechanic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CrystalElemental)),
				new MiniChampTypeInfo(5, typeof(BronzeElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MechanicBoss))
			)
		),
		new MiniChampInfo // Muscle Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(Minotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MuscleBruteBoss))
			)
		),
		new MiniChampInfo // Necromancer's Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lich)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneMagi)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NecroSummonerBoss))
			)
		),
		new MiniChampInfo // Toxic Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(PestilentBandage)),
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NerveAgentBoss))
			)
		),
		new MiniChampInfo // Net Caster Reef
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(AntLion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NetCasterBoss))
			)
		),
		new MiniChampInfo // Nymph Singer Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(Pixie)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(Wisp))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkWisp)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NymphSingerBoss))
			)
		),
		new MiniChampInfo // Oracle's Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SpiritMedium)),
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(ZenMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(OracleBoss))
			)
		),
		new MiniChampInfo // Pastry Chef's Bakery
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ToxicologistChef)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(WoolWeaver))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(BogThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PastryChefBoss))
			)
		),
		new MiniChampInfo // Patchwork Monster Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(15, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(InterredGrizzle)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PatchworkMonsterBoss))
			)
		),
		new MiniChampInfo // Pathologist's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PathologistBoss))
			)
		),
		new MiniChampInfo // Phantom Assassin's Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SneakyNinja)),
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Kunoichi)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PhantomAssassinBoss))
			)
		),
		new MiniChampInfo // Pickpocket's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ConArtist)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PickpocketBoss))
			)
		),
		new MiniChampInfo // Pocket Picker's Refuge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PocketPickerBoss))
			)
		),
		new MiniChampInfo // Protester's Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FloridaMan)),
				new MiniChampTypeInfo(15, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Protester)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ProtesterBoss))
			)
		),
		new MiniChampInfo // Qi Gong Healer Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BattlefieldHealer)),
				new MiniChampTypeInfo(15, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ZenMonk)),
				new MiniChampTypeInfo(10, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(QiGongHealerBoss))
			)
		),
		new MiniChampInfo // Quantum Physicist Research Facility
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(15, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(QuantumPhysicistBoss))
			)
		),
		new MiniChampInfo // Ram Rider Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(Savage)),
				new MiniChampTypeInfo(15, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SavageShaman)),
				new MiniChampTypeInfo(5, typeof(HellHound))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RamRiderBoss))
			)
		),
		new MiniChampInfo // Rapier Duelist Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SabreFighter)),
				new MiniChampTypeInfo(10, typeof(LongbowSniper))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FencingMaster)),
				new MiniChampTypeInfo(5, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RapierDuelistBoss))
			)
		),
		new MiniChampInfo // Relativist Observatory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(15, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RelativistBoss))
			)
		),
		new MiniChampInfo // Relic Hunter Expedition
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Infiltrator)),
				new MiniChampTypeInfo(15, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RelicHunterBoss))
			)
		),
		new MiniChampInfo // Rune Caster Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ElementalWizard)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RuneCasterBoss))
			)
		),
		new MiniChampInfo // Rune Keeper Chamber
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkElf)),
				new MiniChampTypeInfo(5, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RuneKeeperBoss))
			)
		),
		new MiniChampInfo // Saboteur Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(15, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(5, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SaboteurBoss))
			)
		),
		new MiniChampInfo // Sabre Fighter Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SabreFighter)),
				new MiniChampTypeInfo(15, typeof(SwordDefender)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RapierDuelist)),
				new MiniChampTypeInfo(5, typeof(LongbowSniper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SabreFighterBoss))
			)
		),
		new MiniChampInfo // Safecracker's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Infiltrator)),
				new MiniChampTypeInfo(5, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SafeCrackerBoss))
			)
		),
		new MiniChampInfo // Samurai Master's Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kunoichi)),
				new MiniChampTypeInfo(5, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SamuraiMasterBoss))
			)
		),
		new MiniChampInfo // Satyr Piper's Glen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pixie)),
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HostileDruid)),
				new MiniChampTypeInfo(5, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SatyrPiperBoss))
			)
		),
		new MiniChampInfo // Sawmill Worker's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(BattleWeaver)),
				new MiniChampTypeInfo(15, typeof(ArrowFletcher))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SawmillWorkerBoss))
			)
		),
		new MiniChampInfo // Scout Archer's Refuge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(LongbowSniper)),
				new MiniChampTypeInfo(10, typeof(CrossbowMarksman)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(BoomerangThrower))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScoutArcherBoss))
			)
		),
		new MiniChampInfo // Scout Leader Encampment
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ForestScout)),
				new MiniChampTypeInfo(15, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(UrbanTracker)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenNinja)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScoutLeaderBoss))
			)
		),
		new MiniChampInfo // Ninja Shadow Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapSetter)),
				new MiniChampTypeInfo(5, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScoutNinjaBoss))
			)
		),
		new MiniChampInfo // Scroll Mage’s Tower
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScrollMage)),
				new MiniChampTypeInfo(15, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Magician)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ElementalWizard)),
				new MiniChampTypeInfo(5, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScrollMageBoss))
			)
		),
		new MiniChampInfo // Serpent Handler Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(GiantSerpent)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lizardman)),
				new MiniChampTypeInfo(10, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilverSerpent)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SerpentHandlerBoss))
			)
		),
		new MiniChampInfo // Shadow Lurker Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Shade)),
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ghoul)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WailingBanshee)),
				new MiniChampTypeInfo(5, typeof(Revenant))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowLurkerBoss))
			)
		),
		new MiniChampInfo // Shadow Priest Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowPriestBoss))
			)
		),
		new MiniChampInfo // Sheepdog Handler's Pen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Dog)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Sheep))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreyWolf)),
				new MiniChampTypeInfo(5, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SheepdogHandlerBoss))
			)
		),
		new MiniChampInfo // Shield Bearer's Bastion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SwordDefender)),
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(KnightOfJustice))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HolyKnight)),
				new MiniChampTypeInfo(10, typeof(CombatMedic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(5, typeof(FieldCommander))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShieldBearerBoss))
			)
		),
		new MiniChampInfo // Shield Maiden's Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(KnightOfMercy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwordDefender)),
				new MiniChampTypeInfo(5, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShieldMaidenBoss))
			)
		),
		new MiniChampInfo // Sly Storyteller's Theatre
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Spy)),
				new MiniChampTypeInfo(15, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SlyStorytellerBoss))
			)
		),
		new MiniChampInfo // Sous Chef’s Kitchen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ToxicologistChef)),
				new MiniChampTypeInfo(10, typeof(MushroomWitch)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SousChefBoss))
			)
		),
		new MiniChampInfo // Spear Fisher’s Cove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(CoralSnake)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpearFisherBoss))
			)
		),
		new MiniChampInfo // Spear Sentry Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SabreFighter)),
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SumoWrestler)),
				new MiniChampTypeInfo(5, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpearSentryBoss))
			)
		),
		new MiniChampInfo // Spellbreaker’s Trial
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Magician)),
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpellbreakerBoss))
			)
		),
		new MiniChampInfo // Spirit Medium’s Seance
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SpiritMedium)),
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WailingBanshee)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpiritMediumBoss))
			)
		),
		new MiniChampInfo // Spy Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Infiltrator)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpyBoss))
			)
		),
		new MiniChampInfo // Star Reader Observatory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StarReaderBoss))
			)
		),
		new MiniChampInfo // Storm Conjurer Summit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(AirElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormConjurerBoss))
			)
		),
		new MiniChampInfo // Strategist’s War Table
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist)),
				new MiniChampTypeInfo(10, typeof(FieldCommander))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SwordDefender)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Samurai)),
				new MiniChampTypeInfo(5, typeof(TaekwondoMaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StrategistBoss))
			)
		),
		new MiniChampInfo // Sumo Wrestler Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(10, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(JavelinAthlete))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantToad)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SumoWrestlerBoss))
			)
		),
		new MiniChampInfo // Sword Defender Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RapierDuelist)),
				new MiniChampTypeInfo(5, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SwordDefenderBoss))
			)
		),
		new MiniChampInfo // Taekwondo Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenNinja)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TaekwondoMasterBoss))
			)
		),
		new MiniChampInfo // Terrain Scout Encampment
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(UrbanTracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TerrainScoutBoss))
			)
		),
		new MiniChampInfo // Toxicologist's Kitchen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(SlimeMage)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ToxicologistChefBoss))
			)
		),
		new MiniChampInfo // Trap Engineer Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapEngineer)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TrapEngineerBoss))
			)
		),
		new MiniChampInfo // Trap Maker's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapEngineer)),
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TrapMakerBoss))
			)
		),
		new MiniChampInfo // Trap Setter's Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(Spy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Infiltrator)),
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TrapSetterBoss))
			)
		),
		new MiniChampInfo // Tree Feller's Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Treefellow)),
				new MiniChampTypeInfo(10, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TreeFellerBoss))
			)
		),
		new MiniChampInfo // Trick Shot Artist's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LongbowSniper)),
				new MiniChampTypeInfo(10, typeof(CrossbowMarksman)),
				new MiniChampTypeInfo(10, typeof(JavelinAthlete))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SabreFighter)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(5, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TrickShotArtistBoss))
			)
		),
		new MiniChampInfo // Urban Tracker's Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(UrbanTrackerBoss))
			)
		),
		new MiniChampInfo // Trap Maker's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(Mimic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TrapMakerBoss))
			)
		),
		new MiniChampInfo // Venomous Assassin's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonElemental)),
				new MiniChampTypeInfo(10, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SpeckledScorpion)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomousAssassinBoss))
			)
		),
		new MiniChampInfo // Violinist's Orchestra
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(5, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ViolinistBoss))
			)
		),
		new MiniChampInfo // Ward Caster's Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer)),
				new MiniChampTypeInfo(10, typeof(WardCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(QiGongHealer)),
				new MiniChampTypeInfo(10, typeof(ZenMonk))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WardCasterBoss))
			)
		),
		new MiniChampInfo // Water Alchemist's Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SlimeMage)),
				new MiniChampTypeInfo(15, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(CorrosiveSlime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidElemental)),
				new MiniChampTypeInfo(10, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(5, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WaterAlchemistBoss))
			)
		),
		new MiniChampInfo // Weapon Enchanter's Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ElementalWizard)),
				new MiniChampTypeInfo(10, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WeaponEnchanterBoss))
			)
		),
		new MiniChampInfo // Wool Weaver's Loom
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(20, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(JackRabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WoolWeaver)),
				new MiniChampTypeInfo(10, typeof(BattleWeaver))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(5, typeof(Forager))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WoolWeaverBoss))
			)
		),
		new MiniChampInfo // Zen Monk's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(QiGongHealer)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ZenMonk)),
				new MiniChampTypeInfo(10, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(DarkWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ZenMonkBoss))
			)
		),
		new MiniChampInfo // Air Clan Ninja Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AirClanNinjaBoss))
			)
		),
		new MiniChampInfo // Air Clan Samurai Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Samurai)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(LongbowSniper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwordDefender)),
				new MiniChampTypeInfo(5, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AirClanSamuraiBoss))
			)
		),
		new MiniChampInfo // Alien Warrior Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AlienWarriorBoss))
			)
		),
		new MiniChampInfo // Apple Grove Elemental
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AppleElementalBoss))
			)
		),
		new MiniChampInfo // Assassin Guild Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(15, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ConArtist)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AssassinBoss))
			)
		),
		new MiniChampInfo // Astral Traveler Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(GhostScout))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AstralTravelerBoss))
			)
		),
		new MiniChampInfo // Avatar of Elements Shrine
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(FireElemental)),
				new MiniChampTypeInfo(10, typeof(WaterElemental)),
				new MiniChampTypeInfo(10, typeof(AirElemental)),
				new MiniChampTypeInfo(10, typeof(EarthElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidElemental)),
				new MiniChampTypeInfo(10, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CrystalElemental)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AvatarOfElementsBoss))
			)
		),
		new MiniChampInfo // Baroque Barbarian Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BaroqueBarbarian)),
				new MiniChampTypeInfo(15, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HolyKnight)),
				new MiniChampTypeInfo(5, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BaroqueBarbarianBoss))
			)
		),
		new MiniChampInfo // Beetle Juice Summoning Circle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(AntLion)),
				new MiniChampTypeInfo(10, typeof(BlackSolenWarrior)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BeetleJuiceSummonerBoss))
			)
		),
		new MiniChampInfo // Biomancer’s Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BogThing)),
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BiomancerBoss))
			)
		),
		new MiniChampInfo // Blues Singing Gorgon Amphitheater
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BluesSingingGorgonBoss))
			)
		),
		new MiniChampInfo // B-Movie Beastmaster Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(VorpalBunny))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CabaretKrakenGirl)),
				new MiniChampTypeInfo(10, typeof(AntLion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantIceWorm)),
				new MiniChampTypeInfo(5, typeof(PatchworkMonster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BmovieBeastmasterBoss))
			)
		),
		new MiniChampInfo // Bounty Hunter Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Brigand)),
				new MiniChampTypeInfo(10, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Infiltrator)),
				new MiniChampTypeInfo(5, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BountyHunterBoss))
			)
		),
		new MiniChampInfo // Cabaret Kraken Stage
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CabaretKrakenGirl)),
				new MiniChampTypeInfo(10, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RaveRogue)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CabaretKrakenBoss))
			)
		),
		new MiniChampInfo // Cannibal Tribe Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Savage)),
				new MiniChampTypeInfo(10, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(PatchworkMonster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CannibalBoss))
			)
		),
		new MiniChampInfo // Caveman Scientist Experiment Site
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Ogre)),
				new MiniChampTypeInfo(15, typeof(Cyclops)),
				new MiniChampTypeInfo(15, typeof(Ettin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(PatchworkMonster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CavemanScientistBoss))
			)
		),
		new MiniChampInfo // Celestial Samurai Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Samurai)),
				new MiniChampTypeInfo(15, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CelestialSamuraiBoss))
			)
		),
		new MiniChampInfo // Chris Roberts Galactic Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ChrisRobertsBoss))
			)
		),
		new MiniChampInfo // Corporate Executive Tower
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Spy)),
				new MiniChampTypeInfo(5, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CorporateExecutiveBoss))
			)
		),
		new MiniChampInfo // Country Cowgirl Cyclops Ranch
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Cougar)),
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(Minotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CountryCowgirlCyclopsBoss))
			)
		),
		new MiniChampInfo // Wild West Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CowboyBoss))
			)
		),
		new MiniChampInfo // Cyberpunk Nexus
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(5, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CyberpunkSorcererBoss))
			)
		),
		new MiniChampInfo // Harvest Festival Frenzy
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DancingFarmerBoss))
			)
		),
		new MiniChampInfo // Dark Elf Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TwistedCultist)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyQueen)),
				new MiniChampTypeInfo(5, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DarkElfBoss))
			)
		),
		new MiniChampInfo // Shadow Lord's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DarkLordBoss))
			)
		),
		new MiniChampInfo // Dino Rider Expedition
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GreatHart)),
				new MiniChampTypeInfo(10, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CuSidhe)),
				new MiniChampTypeInfo(10, typeof(Hiryu))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SerpentineDragon)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DinoRiderBoss))
			)
		),
		new MiniChampInfo // Disco Druid Festival
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pixie)),
				new MiniChampTypeInfo(15, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DiscoDruidBoss))
			)
		),
		new MiniChampInfo // Dog the Bounty Hunter's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Brigand)),
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Spy)),
				new MiniChampTypeInfo(5, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DogtheBountyHunterBoss))
			)
		),
		new MiniChampInfo // Duelist Poet Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RapierDuelist)),
				new MiniChampTypeInfo(15, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SwordDefender)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GlamRockMage)),
				new MiniChampTypeInfo(5, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DuelistPoetBoss))
			)
		),
		new MiniChampInfo // Earth Clan Ninja Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(15, typeof(Kunoichi)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(TrapSetter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EarthClanNinjaBoss))
			)
		),
		new MiniChampInfo // Earth Clan Samurai Encampment
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(15, typeof(Samurai)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(JukaWarrior)),
				new MiniChampTypeInfo(5, typeof(JukaLord))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EarthClanSamuraiBoss))
			)
		),
		new MiniChampInfo // Evil Alchemist Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SlimeMage)),
				new MiniChampTypeInfo(15, typeof(PoisonElemental)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EvilAlchemistBoss))
			)
		),
		new MiniChampInfo // Evil Clown Carnival
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GlamRockMage)),
				new MiniChampTypeInfo(15, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EvilClownBoss))
			)
		),
		new MiniChampInfo // Fairy Queen Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pixie)),
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DarkWisp)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenHag)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FairyQueenBoss))
			)
		),
		new MiniChampInfo // Fast Explorer Expedition
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScoutNinja)),
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FastExplorerBoss))
			)
		),
		new MiniChampInfo // Fire Clan Ninja Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(10, typeof(FireMage)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FireClanNinjaBoss))
			)
		),
		new MiniChampInfo // Fire Clan Samurai Dojo
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Samurai)),
				new MiniChampTypeInfo(10, typeof(SwordDefender)),
				new MiniChampTypeInfo(10, typeof(BoomerangThrower))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(SumoWrestler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FireClanSamuraiBoss))
			)
		),
		new MiniChampInfo // Flapper Elementalist Altar
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(AirElemental)),
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WindElemental)),
				new MiniChampTypeInfo(5, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FlapperElementalistBoss))
			)
		),
		new MiniChampInfo // Florida Man’s Carnival
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Protester)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(10, typeof(GhostScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FloridaManBoss))
			)
		),
		new MiniChampInfo // Forest Ranger Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BigCatTamer)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CuSidhe)),
				new MiniChampTypeInfo(5, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ForestRangerBoss))
			)
		),
		new MiniChampInfo // Funk Fungi Familiar Garden
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(15, typeof(MushroomWitch))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BogThing)),
				new MiniChampTypeInfo(5, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FunkFungiFamiliarBoss))
			)
		),
		new MiniChampInfo // Gang Leader's Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pickpocket)),
				new MiniChampTypeInfo(15, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Saboteur)),
				new MiniChampTypeInfo(5, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GangLeaderBoss))
			)
		),
		new MiniChampInfo // Glam Rock Mage Concert
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(15, typeof(Flutist)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GlamRockMageBoss))
			)
		),
		new MiniChampInfo // Gothic Novelist Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RestlessSoul)),
				new MiniChampTypeInfo(15, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GhostScout)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lich)),
				new MiniChampTypeInfo(5, typeof(GhostWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GothicNovelistBoss))
			)
		),
		new MiniChampInfo // Graffiti Gargoyle Alley
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GraffitiGargoyleBoss))
			)
		),
		new MiniChampInfo // Greaser Gryphon Rider’s Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GlamRockMage)),
				new MiniChampTypeInfo(15, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(MegaDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GreaserGryphonRiderBoss))
			)
		),
		new MiniChampInfo // Green Hag’s Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonElemental)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenHag)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GreenHagBoss))
			)
		),
		new MiniChampInfo // Green Ninja’s Hidden Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GreenNinjaBoss))
			)
		),
		new MiniChampInfo // Hippie Hoplite’s Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(Pixie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestRanger)),
				new MiniChampTypeInfo(5, typeof(HippieHoplite))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HippieHopliteBoss))
			)
		),
		new MiniChampInfo // Holy Knight’s Cathedral
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(10, typeof(KnightOfValor))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HolyKnight)),
				new MiniChampTypeInfo(10, typeof(Paladin))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SpiritMedium)),
				new MiniChampTypeInfo(5, typeof(QiGongHealer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HolyKnight2Boss))
			)
		),
		new MiniChampInfo // Sanctuary of the Holy Knight
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(SwordDefender)),
				new MiniChampTypeInfo(15, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(15, typeof(KnightOfMercy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FieldCommander)),
				new MiniChampTypeInfo(5, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HolyKnightBoss))
			)
		),
		new MiniChampInfo // Hostile Druid’s Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HostileDruidBoss))
			)
		),
		new MiniChampInfo // Hostile Princess’ Court
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DisguiseMaster)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HostilePrincessBoss))
			)
		),
		new MiniChampInfo // Ice King’s Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(IceSnake)),
				new MiniChampTypeInfo(15, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IceElemental)),
				new MiniChampTypeInfo(10, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDragon)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IceKingBoss))
			)
		),
		new MiniChampInfo // Inferno Dragon’s Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernoDragonBoss))
			)
		),
		new MiniChampInfo // Insane Roboticist Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InsaneRoboticistBoss))
			)
		),
		new MiniChampInfo // Jazz Age Brawl
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NoirDetective)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JazzAgeJuggernautBoss))
			)
		),
		new MiniChampInfo // Jester’s Court
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(Flutist)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GothicNovelist)),
				new MiniChampTypeInfo(5, typeof(MushroomWitch))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JesterBoss))
			)
		),
		new MiniChampInfo // Lawyer’s Tribunal
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ConArtist)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SafeCracker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(10, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LawyerBoss))
			)
		),
		new MiniChampInfo // Line Dragon’s Ascent
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(15, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Dragon)),
				new MiniChampTypeInfo(10, typeof(Drake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PlatinumDrake)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LineDragonBoss))
			)
		),
		new MiniChampInfo // Lord Blackthorn's Dominion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(DarkElf)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Infiltrator)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Assassin)),
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LordBlackthornBoss))
			)
		),
		new MiniChampInfo // Lord British's Summoning Circle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HolyKnight)),
				new MiniChampTypeInfo(15, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LordBritishSummonerBoss))
			)
		),
		new MiniChampInfo // Magma Elemental Rift
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaLizard)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(FireGargoyle))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MagmaElementalBoss))
			)
		),
		new MiniChampInfo // Medieval Meteorologist's Observatory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(LightningBearer)),
				new MiniChampTypeInfo(10, typeof(AirElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SnowElemental)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MedievalMeteorologistBoss))
			)
		),
		new MiniChampInfo // Mega Dragon's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Drake)),
				new MiniChampTypeInfo(15, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyDragon)),
				new MiniChampTypeInfo(10, typeof(SerpentineDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MegaDragonBoss))
			)
		),
		new MiniChampInfo // Minax Sorceress Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(DarkElf)),
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(ScrollMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MinaxSorceressBoss))
			)
		),
		new MiniChampInfo // Mischievous Witch Coven
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MischievousWitchBoss))
			)
		),
		new MiniChampInfo // Motown Mermaid Lagoon
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Kraken)),
				new MiniChampTypeInfo(10, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MotownMermaidBoss))
			)
		),
		new MiniChampInfo // Mushroom Witch Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bogling)),
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MushroomWitchBoss))
			)
		),
		new MiniChampInfo // Musketeer Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RapierDuelist)),
				new MiniChampTypeInfo(15, typeof(SabreFighter)),
				new MiniChampTypeInfo(10, typeof(EpeeSpecialist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DualWielder)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShieldBearer)),
				new MiniChampTypeInfo(5, typeof(CombatMedic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MusketeerBoss))
			)
		),
		new MiniChampInfo // NeoVictorian Vampire Court
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(DarkElf)),
				new MiniChampTypeInfo(10, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VampireBat)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodElemental)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NeoVictorianVampireBoss))
			)
		),
		new MiniChampInfo // Ninja Librarian Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScoutNinja)),
				new MiniChampTypeInfo(15, typeof(Kunoichi)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Saboteur)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NinjaLibrarianBoss))
			)
		),
		new MiniChampInfo // Noir Detective Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(NoirDetective)),
				new MiniChampTypeInfo(15, typeof(SafeCracker)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Spy)),
				new MiniChampTypeInfo(10, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapSetter)),
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NoirDetectiveBoss))
			)
		),
		new MiniChampInfo // Ogre Master's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ogre)),
				new MiniChampTypeInfo(15, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(Ettin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(OgreMasterBoss))
			)
		),
		new MiniChampInfo // Phoenix Style Master's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SumoWrestler)),
				new MiniChampTypeInfo(15, typeof(TaekwondoMaster)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(10, typeof(BoomerangThrower))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PhoenixStyleMasterBoss))
			)
		),
		new MiniChampInfo // Pig Farmer's Pen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(JackRabbit)),
				new MiniChampTypeInfo(10, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PigFarmerBoss))
			)
		),
		new MiniChampInfo // Pinup Pandemonium Parlor
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GlamRockMage)),
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Flutist)),
				new MiniChampTypeInfo(5, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PinupPandemoniumBoss))
			)
		),
		new MiniChampInfo // Pirate's Cove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Brigand)),
				new MiniChampTypeInfo(15, typeof(GhostWarrior)),
				new MiniChampTypeInfo(10, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Kraken)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CoralSnake)),
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PirateBoss))
			)
		),
		new MiniChampInfo // Pirate of the Stars Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(15, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(RetroAndroid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PirateOfTheStarsBoss))
			)
		),
		new MiniChampInfo // PK Murderer's Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Assassin)),
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(DecoyDeployer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MasterPickpocket)),
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ScoutNinja)),
				new MiniChampTypeInfo(5, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PKMurdererBoss))
			)
		),
		new MiniChampInfo // Bloodstained Fields
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Assassin)),
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DualWielder)),
				new MiniChampTypeInfo(5, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PKMurdererLordBoss))
			)
		),
		new MiniChampInfo // Corrupted Orchard
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(Slime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PestilentBandage)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PoisonAppleTreeBoss))
			)
		),
		new MiniChampInfo // Mystic Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PsychedelicShaman)),
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(Wisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DarkWisp)),
				new MiniChampTypeInfo(10, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PsychedelicShamanBoss))
			)
		),
		new MiniChampInfo // Alchemical Lab
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(15, typeof(SlimeMage)),
				new MiniChampTypeInfo(10, typeof(GemCutter))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicologistChef)),
				new MiniChampTypeInfo(10, typeof(BattlefieldHealer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(BloodElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PulpyPotionPurveyorBoss))
			)
		),
		new MiniChampInfo // Rebel Cathedral
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GlamRockMage)),
				new MiniChampTypeInfo(15, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HolyKnight)),
				new MiniChampTypeInfo(5, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PunkRockPaladinBoss))
			)
		),
		new MiniChampInfo // Ra King's Pyramid
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lich)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RaKingBoss))
			)
		),
		new MiniChampInfo // Ranch Master's Prairie
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(Cow)),
				new MiniChampTypeInfo(10, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bull)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreatHart)),
				new MiniChampTypeInfo(5, typeof(Lion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RanchMasterBoss))
			)
		),
		new MiniChampInfo // Rap Ranger's Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ForestScout)),
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(UrbanTracker))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kirin)),
				new MiniChampTypeInfo(5, typeof(Unicorn))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RapRangerBoss))
			)
		),
		new MiniChampInfo // Rave Rogue's Underground
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(15, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(Pickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(5, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RaveRogueBoss))
			)
		),
		new MiniChampInfo // Red Queen's Court
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HolyKnight)),
				new MiniChampTypeInfo(10, typeof(KnightOfMercy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RapierDuelist)),
				new MiniChampTypeInfo(5, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RedQueenBoss))
			)
		),
		new MiniChampInfo // Reggae Runesmith Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(ArcaneScribe))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireMage)),
				new MiniChampTypeInfo(5, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ReggaeRunesmithBoss))
			)
		),
		new MiniChampInfo // Renaissance Mechanic Factory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapEngineer)),
				new MiniChampTypeInfo(15, typeof(Carpenter)),
				new MiniChampTypeInfo(10, typeof(IronSmith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RenaissanceMechanicBoss))
			)
		),
		new MiniChampInfo // Retro Android Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(StarCitizen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RetroAndroidBoss))
			)
		),
		new MiniChampInfo // Retro Futurist Dome
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(15, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RetroFuturistBoss))
			)
		),
		new MiniChampInfo // Retro Robot Romancer's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(15, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RetroRobotRomancerBoss))
			)
		),
		new MiniChampInfo // Ringmaster's Circus
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Flutist)),
				new MiniChampTypeInfo(10, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CabaretKrakenGirl)),
				new MiniChampTypeInfo(5, typeof(SlyStoryteller))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RingmasterBoss))
			)
		),
		new MiniChampInfo // Rockabilly Revenant's Stage
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(GhostScout)),
				new MiniChampTypeInfo(15, typeof(GothicNovelist)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Revenant)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(GhostWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RockabillyRevenantBoss))
			)
		),
		new MiniChampInfo // Scorpomancer's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SpeckledScorpion)),
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScorpomancerBoss))
			)
		),
		new MiniChampInfo // Silent Movie Studio
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(15, typeof(NoirDetective)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Spy)),
				new MiniChampTypeInfo(5, typeof(ConArtist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SilentMovieMonkBoss))
			)
		),
		new MiniChampInfo // Silver Slime Caverns
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SithBoss))
			)
		),
		new MiniChampInfo // Sith Academy
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(10, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(10, typeof(Impaler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(Balron))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SithBoss))
			)
		),
		new MiniChampInfo // Ska Skald Concert Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DrumBoy)),
				new MiniChampTypeInfo(15, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SkaSkaldBoss))
			)
		),
		new MiniChampInfo // Skeleton Lord Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SkeletonLordBoss))
			)
		),
		new MiniChampInfo // Slime Mage Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SlimeMageBoss))
			)
		),
		new MiniChampInfo // Sneaky Ninja Clan
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScoutNinja)),
				new MiniChampTypeInfo(15, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SneakyNinjaBoss))
			)
		),
		new MiniChampInfo // Star Citizen Outpost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StarCitizenBoss))
			)
		),
		new MiniChampInfo // Starfleet Captain's Command
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(15, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StarfleetCaptainBoss))
			)
		),
		new MiniChampInfo // Starfleet Command Center
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(15, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StarfleetCommander))
			)
		),
		new MiniChampInfo // Steampunk Samurai Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(RetroAndroid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Saboteur)),
				new MiniChampTypeInfo(10, typeof(DualWielder))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(BloodElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SteampunkSamuraiBoss))
			)
		),
		new MiniChampInfo // Stormtrooper Academy
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Balron))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormtrooperBoss))
			)
		),
		new MiniChampInfo // Surfer Summoner Cove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WaterElemental)),
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AquaticTamer)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(Kraken))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SurferSummonerBoss))
			)
		),
		new MiniChampInfo // Swamp Thing Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SwampThingBoss))
			)
		),
		new MiniChampInfo // Swingin' Sorceress Ballroom
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GlamRockMage)),
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RaveRogue)),
				new MiniChampTypeInfo(5, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SwinginSorceressBoss))
			)
		),
		new MiniChampInfo // Texan Rancher Prairie
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Horse)),
				new MiniChampTypeInfo(15, typeof(Cow)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(RidableLlama))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(GreatHart))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TexanRancherBoss))
			)
		),
		new MiniChampInfo // Twisted Cultist Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TwistedCultist)),
				new MiniChampTypeInfo(10, typeof(DarkElf)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TwistedCultistBoss))
			)
		),
		new MiniChampInfo // Vaudeville Valkyrie Stage
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkaSkald)),
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VaudevilleValkyrieBoss))
			)
		),
		new MiniChampInfo // Wasteland Biker Compound
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(15, typeof(OrcBrute)),
				new MiniChampTypeInfo(10, typeof(Brigand))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SavageShaman)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(OgreLord)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WastelandBikerBoss))
			)
		),
		new MiniChampInfo // Water Clan Ninja Hideout
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(15, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(Kunoichi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(Saboteur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WaterElemental)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WaterClanNinjaBoss))
			)
		),
		new MiniChampInfo // Water Clan Samurai Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Samurai)),
				new MiniChampTypeInfo(15, typeof(KatanaDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AquaticTamer)),
				new MiniChampTypeInfo(5, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WaterClanSamuraiBoss))
			)
		),
		new MiniChampInfo // Wild West Wizard Canyon
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(15, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WildWestWizardBoss))
			)
		),
		new MiniChampInfo // Abbadon the Abyssal
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Imp)),
				new MiniChampTypeInfo(15, typeof(GreaterMongbat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Daemon)),
				new MiniChampTypeInfo(10, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Balron)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AbbadonTheAbyssalBoss))
			)
		),
		new MiniChampInfo // Abyssal Warden
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(15, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(EffetePutridGargoyle))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AbyssalWardenBoss))
			)
		),
		new MiniChampInfo // Abyssinian Tracker
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ForestScout)),
				new MiniChampTypeInfo(15, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(GreenGoblinScout))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AbyssinianTrackerBoss))
			)
		),
		new MiniChampInfo // Acidic Alligator
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(AcidSlug)),
				new MiniChampTypeInfo(15, typeof(CorrosiveSlime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidElemental)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BullFrog)),
				new MiniChampTypeInfo(5, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AcidicAlligatorBoss))
			)
		),
		new MiniChampInfo // Ancient Alligator
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BullFrog)),
				new MiniChampTypeInfo(15, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lizardman)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilverSerpent)),
				new MiniChampTypeInfo(5, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AncientAlligatorBoss))
			)
		),
		new MiniChampInfo // Ancient Dragon's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Drake)),
				new MiniChampTypeInfo(15, typeof(SerpentineDragon)),
				new MiniChampTypeInfo(10, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostDragon)),
				new MiniChampTypeInfo(10, typeof(PlatinumDrake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(GreaterDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AncientDragonBoss))
			)
		),
		new MiniChampInfo // Angus Berserker's Camp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwordDefender)),
				new MiniChampTypeInfo(15, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(10, typeof(RapierDuelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(CombatMedic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DualWielder)),
				new MiniChampTypeInfo(5, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(AngusBerserkerBoss))
			)
		),
		new MiniChampInfo // Banshee's Wail
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Lich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WailingBanshee)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BansheeBoss))
			)
		),
		new MiniChampInfo // Banshee Crab's Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(CoralSnake)),
				new MiniChampTypeInfo(10, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WailingBanshee)),
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BansheeCrabBoss))
			)
		),
		new MiniChampInfo // Bengal Storm's Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Cougar)),
				new MiniChampTypeInfo(15, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Beastmaster)),
				new MiniChampTypeInfo(5, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BengalStormBoss))
			)
		),
		new MiniChampInfo // Bison Brute Plateau
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bull)),
				new MiniChampTypeInfo(15, typeof(GreatHart)),
				new MiniChampTypeInfo(10, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BisonBruteBoss))
			)
		),
		new MiniChampInfo // Black Widow's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(AntLion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BlackWidowQueenBoss))
			)
		),
		new MiniChampInfo // Blight Demon Fissure
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BlightDemonBoss))
			)
		),
		new MiniChampInfo // Blood Dragon Roost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(CrimsonDrake)),
				new MiniChampTypeInfo(15, typeof(Dragon)),
				new MiniChampTypeInfo(10, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Drake)),
				new MiniChampTypeInfo(10, typeof(SerpentineDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(GreaterDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BloodDragonBoss))
			)
		),
		new MiniChampInfo // Bloodthirsty Vines Thicket
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Forager)),
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(BogThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BloodthirstyVinesBoss))
			)
		),
		new MiniChampInfo // Abyssal Bouncer's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(10, typeof(OrcBrute)),
				new MiniChampTypeInfo(10, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Minotaur)),
				new MiniChampTypeInfo(10, typeof(OgreLord))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DemonKnight)),
				new MiniChampTypeInfo(5, typeof(Balron))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAbyssalBouncer))
			)
		),
		new MiniChampInfo // Abyssal Panther's Prowl
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(ScoutNinja)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAbyssalPanther))
			)
		),
		new MiniChampInfo // Abyssal Tide's Surge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAbyssalTide))
			)
		),
		new MiniChampInfo // Acererak's Necropolis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAcererak))
			)
		),
		new MiniChampInfo // Acidic Slime's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(CorrosiveSlime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidElemental)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(InterredGrizzle))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAcidicSlime))
			)
		),
		new MiniChampInfo // Aegis Construct Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Golem)),
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CrystalElemental)),
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BronzeElemental)),
				new MiniChampTypeInfo(5, typeof(AcidElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAegisConstruct))
			)
		),
		new MiniChampInfo // Akhenaten's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(Wight)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lich)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAkhenatenTheHeretic))
			)
		),
		new MiniChampInfo // Akhenaten's Heretic Shrine
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lich)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAkhenatenTheHeretic))
			)
		),
		new MiniChampInfo // Albert's Squirrel Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(JackRabbit)),
				new MiniChampTypeInfo(10, typeof(Bird))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VorpalBunny)),
				new MiniChampTypeInfo(10, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantToad)),
				new MiniChampTypeInfo(5, typeof(Wisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAlbertsSquirrel))
			)
		),
		new MiniChampInfo // Ancient Wolf Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(LeatherWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAncientWolf))
			)
		),
		new MiniChampInfo // Anthrax Rat Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantRat)),
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonElemental)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAnthraxRat))
			)
		),
		new MiniChampInfo // Arbiter Drone Hive
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(AntLion)),
				new MiniChampTypeInfo(15, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WolfSpider)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossArbiterDrone))
			)
		),
		new MiniChampInfo // Arcane Satyr Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(Pixie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DarkElf)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wisp)),
				new MiniChampTypeInfo(5, typeof(DarkWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossArcaneSatyr))
			)
		),
		new MiniChampInfo // Arcane Sentinel Bastion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Enchanter)),
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossArcaneSentinel))
			)
		),
		new MiniChampInfo // Aries Harpy Aerie
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Crane)),
				new MiniChampTypeInfo(15, typeof(Eagle)),
				new MiniChampTypeInfo(15, typeof(Parrot))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Phoenix)),
				new MiniChampTypeInfo(10, typeof(Harpy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StoneHarpy)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAriesHarpy))
			)
		),
		new MiniChampInfo // Aries Ram Bear Plateau
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(15, typeof(MountainGoat)),
				new MiniChampTypeInfo(15, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAriesRamBear))
			)
		),
		new MiniChampInfo // Azalin Rex's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(10, typeof(Lich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RottingCorpse)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAzalinRex))
			)
		),
		new MiniChampInfo // Azure Mirage's Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Wisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DarkWisp)),
				new MiniChampTypeInfo(10, typeof(Pixie))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyDragon)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAzureMirage))
			)
		),
		new MiniChampInfo // Azure Moose Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ForestScout)),
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CuSidhe)),
				new MiniChampTypeInfo(5, typeof(Kirin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossAzureMoose))
			)
		),
		new MiniChampInfo // Babirusa Beast's Bog
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PatchworkMonster)),
				new MiniChampTypeInfo(5, typeof(PlatinumDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBabirusaBeast))
			)
		),
		new MiniChampInfo // Alpha Baboon Troop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Gorilla)),
				new MiniChampTypeInfo(15, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Savage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreatHart)),
				new MiniChampTypeInfo(5, typeof(Yamandon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBaboonsAlpha))
			)
		),
		new MiniChampInfo // Bearded Goat Pastures
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Goat)),
				new MiniChampTypeInfo(15, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBeardedGoat))
			)
		),
		new MiniChampInfo // Beldings Ground Squirrel Burrow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(JackRabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(LeatherWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VorpalBunny)),
				new MiniChampTypeInfo(5, typeof(GreatHart))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBeldingsGroundSquirrel))
			)
		),
		new MiniChampInfo // Black Death Rat Sewers
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRibbonPlagueRat)),
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBlackDeathRat))
			)
		),
		new MiniChampInfo // Black Widow Queen Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(AntLion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBlackWidowQueen))
			)
		),
		new MiniChampInfo // Blighted Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BullFrog)),
				new MiniChampTypeInfo(10, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(Slime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBlightedToad))
			)
		),
		new MiniChampInfo // Blood Lich's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Ghoul)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBloodLich))
			)
		),
		new MiniChampInfo // Blood Serpent's Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Snake)),
				new MiniChampTypeInfo(10, typeof(IceSnake)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(SilverSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantSerpent)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBloodSerpent))
			)
		),
		new MiniChampInfo // Bonecrusher Ogre's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(Ettin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(Minotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBonecrusherOgre))
			)
		),
		new MiniChampInfo // Bone Golem's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneMagi)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBoneGolem))
			)
		),
		new MiniChampInfo // Borneo Pigsty
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pig)),
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(Rabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBorneoPig))
			)
		),
		new MiniChampInfo // Bubblegum Blaster Factory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(Bogling))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Imp)),
				new MiniChampTypeInfo(5, typeof(Mimic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBubblegumBlaster))
			)
		),
		new MiniChampInfo // Bush Pig Encampment
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Pig)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreatHart)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossBushPig))
			)
		),
		new MiniChampInfo // Cactus Llama Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CrystalElemental)),
				new MiniChampTypeInfo(5, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCactusLlama))
			)
		),
		new MiniChampInfo // Cancer Harpy Aerie
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Harpy)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WailingBanshee)),
				new MiniChampTypeInfo(10, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(Lich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCancerHarpy))
			)
		),
		new MiniChampInfo // Cancer Shell Bear Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneKnight)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCancerShellBear))
			)
		),
		new MiniChampInfo // Candy Corn Creep's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PestilentBandage)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonElemental)),
				new MiniChampTypeInfo(5, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCandyCornCreep))
			)
		),
		new MiniChampInfo // Capricorn Harpy's Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Chicken)),
				new MiniChampTypeInfo(15, typeof(Crane)),
				new MiniChampTypeInfo(10, typeof(Eagle))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Parrot)),
				new MiniChampTypeInfo(10, typeof(Macaw))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Phoenix)),
				new MiniChampTypeInfo(5, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCapricornHarpy))
			)
		),
		new MiniChampInfo // Capricorn Mountain Bear's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BrownBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreyWolf)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCapricornMountainBear))
			)
		),
		new MiniChampInfo // Capuchin Trickster's Playground
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SneakyNinja)),
				new MiniChampTypeInfo(15, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapSetter)),
				new MiniChampTypeInfo(10, typeof(DisguiseMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilentMovieMonk)),
				new MiniChampTypeInfo(5, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCapuchinTrickster))
			)
		),
		new MiniChampInfo // Caramel Conjurer's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(15, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCaramelConjurer))
			)
		),
		new MiniChampInfo // Celestial Horror Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(GothicNovelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(WandererOfTheVoid))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCelestialHorror))
			)
		),
		new MiniChampInfo // Celestial Python Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantSerpent)),
				new MiniChampTypeInfo(10, typeof(Snake)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaSerpent)),
				new MiniChampTypeInfo(5, typeof(SerpentineDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCelestialPython))
			)
		),
		new MiniChampInfo // Celestial Satyr Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyQueen)),
				new MiniChampTypeInfo(5, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCelestialSatyr))
			)
		),
		new MiniChampInfo // Celestial Wolf Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(DireWolf)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TimberWolf)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCelestialWolf))
			)
		),
		new MiniChampInfo // Chamois Hill
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Goat)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossChamois))
			)
		),
		new MiniChampInfo // Boss Chaos Hare
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(VorpalBunny)),
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(5, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossChaosHare))
			)
		),
		new MiniChampInfo // Boss Charro Llama
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RidableLlama)),
				new MiniChampTypeInfo(15, typeof(PackHorse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Horse)),
				new MiniChampTypeInfo(10, typeof(GreatHart))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(5, typeof(Beastmaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCharroLlama))
			)
		),
		new MiniChampInfo // Boss Cheese Golem
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Mimic)),
				new MiniChampTypeInfo(10, typeof(Ravager)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Goat)),
				new MiniChampTypeInfo(5, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Slime)),
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCheeseGolem))
			)
		),
		new MiniChampInfo // Boss Chimpanzee Berserker
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Gorilla)),
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bull)),
				new MiniChampTypeInfo(5, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackBear)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossChimpanzeeBerserker))
			)
		),
		new MiniChampInfo // Boss Chocolate Truffle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(DrumBoy)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RaveRogue)),
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossChocolateTruffle))
			)
		),
		new MiniChampInfo // Cholera Rat Infestation
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRS)),
				new MiniChampTypeInfo(15, typeof(ClanRC)),
				new MiniChampTypeInfo(15, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantRat)),
				new MiniChampTypeInfo(5, typeof(Slime))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCholeraRat))
			)
		),
		new MiniChampInfo // Chromatic Ogre Clan
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(OrcBomber)),
				new MiniChampTypeInfo(10, typeof(OrcChopper)),
				new MiniChampTypeInfo(10, typeof(OrcCaptain))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(GrecoRomanWrestler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SavageShaman)),
				new MiniChampTypeInfo(5, typeof(OrcishMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossChromaticOgre))
			)
		),
		new MiniChampInfo // Cleopatra the Enigmatic
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyQueen)),
				new MiniChampTypeInfo(5, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCleopatraTheEnigmatic))
			)
		),
		new MiniChampInfo // Cliff Goat Dominion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MountainGoat)),
				new MiniChampTypeInfo(15, typeof(RamRider)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackBear)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCliffGoat))
			)
		),
		new MiniChampInfo // Coral Sentinel's Reef
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(Dolphin)),
				new MiniChampTypeInfo(10, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(5, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCoralSentinel))
			)
		),
		new MiniChampInfo // Corrosive Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantToad)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PestilentBandage)),
				new MiniChampTypeInfo(5, typeof(CorrosiveSlime))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCorrosiveToad))
			)
		),
		new MiniChampInfo // Cosmic Bouncer Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StarCitizen)),
				new MiniChampTypeInfo(15, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(5, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(3, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCosmicBouncer))
			)
		),
		new MiniChampInfo // Cosmic Stalker Void
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(5, typeof(Balron))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCosmicStalker))
			)
		),
		new MiniChampInfo // Crimson Mule Valley
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Horse)),
				new MiniChampTypeInfo(10, typeof(RidableLlama)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GrayGoblin)),
				new MiniChampTypeInfo(5, typeof(Boar))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCrimsonMule))
			)
		),
		new MiniChampInfo // Crystal Golem Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(IronSmith)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BronzeElemental)),
				new MiniChampTypeInfo(5, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCrystalGolem))
			)
		),
		new MiniChampInfo // Crystal Ooze Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(5, typeof(CrystalElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BogThing)),
				new MiniChampTypeInfo(3, typeof(Bogling))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCrystalOoze))
			)
		),
		new MiniChampInfo // Cursed Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(Slime))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCursedToad))
			)
		),
		new MiniChampInfo // Cursed White Tail Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(PatchworkSkeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SpectralWolf)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCursedWolf))
			)
		),
		new MiniChampInfo // Cursed Wolf's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wolf)),
				new MiniChampTypeInfo(15, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SpectralWolf)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossCursedWolf))
			)
		),
		new MiniChampInfo // Dall Sheep Highland
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(MountainGoat)),
				new MiniChampTypeInfo(15, typeof(DallSheep)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(BrownBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(5, typeof(IceElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDallSheep))
			)
		),
		new MiniChampInfo // Death Rat Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRS)),
				new MiniChampTypeInfo(20, typeof(ClanRC)),
				new MiniChampTypeInfo(15, typeof(ClanSS))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(5, typeof(Zombie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDeathRat))
			)
		),
		new MiniChampInfo // Dia de Los Muertos Llama
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Zombie)),
				new MiniChampTypeInfo(15, typeof(PlagueRat)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDiaDeLosMuertosLlama))
			)
		),
		new MiniChampInfo // Displacer Beast Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(GiantSpider)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(DisplacerBeast))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDisplacerBeast))
			)
		),
		new MiniChampInfo // Domestic Swine Retreat
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(25, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(5, typeof(Troll))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDomesticSwine))
			)
		),
		new MiniChampInfo // Douglas Squirrel Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DouglasSquirrel)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDouglasSquirrel))
			)
		),
		new MiniChampInfo // Dreadnaught Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BaroqueBarbarian)),
				new MiniChampTypeInfo(5, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ravager)),
				new MiniChampTypeInfo(5, typeof(Mimic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossDreadnaught))
			)
		),
		new MiniChampInfo // Earthquake Wolf Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(TimberWolf)),
				new MiniChampTypeInfo(10, typeof(LeatherWolf)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(EarthElemental)),
				new MiniChampTypeInfo(5, typeof(StoneGolem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEarthquakeWolf))
			)
		),
		new MiniChampInfo // Eastern Gray Squirrel Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(JackRabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestScout)),
				new MiniChampTypeInfo(5, typeof(Beastmaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(Forager))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEasternGraySquirrel))
			)
		),
		new MiniChampInfo // Eclipse Reindeer Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Rat)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEclipseReindeer))
			)
		),
		new MiniChampInfo // Eldritch Harbinger Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(Succubus)),
				new MiniChampTypeInfo(5, typeof(DemonKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEldritchHarbinger))
			)
		),
		new MiniChampInfo // Boss Eldritch Hare's Warren
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(VorpalBunny)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(GhostScout))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEldritchHare))
			)
		),
		new MiniChampInfo // Boss Eldritch Toad's Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantToad)),
				new MiniChampTypeInfo(20, typeof(BullFrog)),
				new MiniChampTypeInfo(10, typeof(Slime))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantIceWorm)),
				new MiniChampTypeInfo(5, typeof(Bogling))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(GhostWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEldritchToad))
			)
		),
		new MiniChampInfo // Boss Electric Slime's Labyrinth
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(Rabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(GhostWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossElectricSlime))
			)
		),
		new MiniChampInfo // Boss Electro Wraith's Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossElectroWraith))
			)
		),
		new MiniChampInfo // Boss El Mariachi Llama's Fiesta
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(Horse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Flutist)),
				new MiniChampTypeInfo(10, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkaSkald)),
				new MiniChampTypeInfo(5, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossElMariachiLlama))
			)
		),
		new MiniChampInfo // Boss Ember Axis Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaElemental)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireGargoyle)),
				new MiniChampTypeInfo(5, typeof(Efreet))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEmberAxis))
			)
		),
		new MiniChampInfo // Boss Ember Wolf Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireElemental)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(EmberWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEmberWolf))
			)
		),
		new MiniChampInfo // Boss Emperor Cobra Temple
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(GiantSerpent)),
				new MiniChampTypeInfo(10, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(EmperorCobra))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEmperorCobra))
			)
		),
		new MiniChampInfo // Boss Enigmatic Satyr Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(EnigmaticSatyr))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEnigmaticSatyr))
			)
		),
		new MiniChampInfo // Boss Enigmatic Skipper Reef
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CoralSnake)),
				new MiniChampTypeInfo(10, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kraken)),
				new MiniChampTypeInfo(5, typeof(EnigmaticSkipper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEnigmaticSkipper))
			)
		),
		new MiniChampInfo // Ethereal Panthra's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CuSidhe)),
				new MiniChampTypeInfo(15, typeof(EtherealWarrior)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BakeKitsune)),
				new MiniChampTypeInfo(10, typeof(Reptalon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kirin)),
				new MiniChampTypeInfo(5, typeof(Yamandon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossEtherealPanthra))
			)
		),
		new MiniChampInfo // Fainting Goat's Pasture
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Goat)),
				new MiniChampTypeInfo(15, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFaintingGoat))
			)
		),
		new MiniChampInfo // Fever Rat's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PlagueRat)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(Ratman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(10, typeof(Bogling))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Slime)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFeverRat))
			)
		),
		new MiniChampInfo // Fiesta Llama's Celebration
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RidableLlama)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Llama))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Pig)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodFox)),
				new MiniChampTypeInfo(5, typeof(Goat))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFiestaLlama))
			)
		),
		new MiniChampInfo // Flameborne Knight's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFlameborneKnight))
			)
		),
		new MiniChampInfo // Flamebringer Ogre Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ogre)),
				new MiniChampTypeInfo(15, typeof(OrcChopper)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(OrcChopper)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(FlameElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFlamebringerOgre))
			)
		),
		new MiniChampInfo // Flesh Eater Ogre Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(20, typeof(GrayGoblin)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(GrayGoblin))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(Revenant))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFleshEaterOgre))
			)
		),
		new MiniChampInfo // Flying Squirrel Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(Pig)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FlyingSquirrel)),
				new MiniChampTypeInfo(5, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFlyingSquirrel))
			)
		),
		new MiniChampInfo // Forgotten Warden Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossForgottenWarden))
			)
		),
		new MiniChampInfo // Fox Squirrel Glen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(5, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodFox)),
				new MiniChampTypeInfo(5, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFoxSquirrel))
			)
		),
		new MiniChampInfo // Frenzied Satyr's Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrenziedSatyr))
			)
		),
		new MiniChampInfo // Frostbite Wolf's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(DireWolf)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(TimberWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostbiteWolf))
			)
		),
		new MiniChampInfo // Frostbound Champion's Hall
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(HolyKnight)),
				new MiniChampTypeInfo(10, typeof(KnightOfValor))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(KnightOfValor))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostboundChampion))
			)
		),
		new MiniChampInfo // Frost Droid Factory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(RetroAndroid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(ValoriteElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostDroid))
			)
		),
		new MiniChampInfo // Frost Lich's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostLich))
			)
		),
		new MiniChampInfo // Frost Ogre Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(15, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostOgre)),
				new MiniChampTypeInfo(5, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostOgre))
			)
		),
		new MiniChampInfo // Frost Wapiti Grounds
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(25, typeof(FrostDrake)),
				new MiniChampTypeInfo(20, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(5, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostWapiti))
			)
		),
		new MiniChampInfo // Frost Warden Watch
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(15, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PolarBear)),
				new MiniChampTypeInfo(5, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrostWarden))
			)
		),
		new MiniChampInfo // Frozen Ooze Cave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(PlagueRat)),
				new MiniChampTypeInfo(10, typeof(FrozenOoze)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(FrozenOoze)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SlimeMage)),
				new MiniChampTypeInfo(5, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFrozenOoze))
			)
		),
		new MiniChampInfo // Fungal Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantToad)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FungalToad)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossFungalToad))
			)
		),
		new MiniChampInfo // Gemini Harpy's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(GreenNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGeminiHarpy))
			)
		),
		new MiniChampInfo // Gemini Twin Bear's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BrownBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGeminiTwinBear))
			)
		),
		new MiniChampInfo // Gentle Satyr's Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenNinja)),
				new MiniChampTypeInfo(5, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGentleSatyr))
			)
		),
		new MiniChampInfo // Giant Forest Hog's Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(GiantToad)),
				new MiniChampTypeInfo(15, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGiantForestHog))
			)
		),
		new MiniChampInfo // Giant Wolf Spider's Web
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SentinelSpider)),
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGiantWolfSpider))
			)
		),
		new MiniChampInfo // Boss Gibbon Mystic's Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GreenNinja)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(10, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGibbonMystic))
			)
		),
		new MiniChampInfo // Boss Glistening Ooze's Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGlisteningOoze))
			)
		),
		new MiniChampInfo // Boss Gloom Ogre's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(Cyclops))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGloomOgre))
			)
		),
		new MiniChampInfo // Boss Gloom Wolf's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(TimberWolf)),
				new MiniChampTypeInfo(10, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(LeatherWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGloomWolf))
			)
		),
		new MiniChampInfo // Boss Golden Orb Weaver's Web
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(5, typeof(GiantSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGoldenOrbWeaver))
			)
		),
		new MiniChampInfo // Goliath Birdeater's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantIceWorm)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGoliathBirdeater))
			)
		),
		new MiniChampInfo // Goral's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Savage)),
				new MiniChampTypeInfo(5, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGoral))
			)
		),
		new MiniChampInfo // Grappler Drone's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(10, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(10, typeof(Assassin))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(RamRider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CombatMedic)),
				new MiniChampTypeInfo(5, typeof(CrossbowMarksman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGrapplerDrone))
			)
		),
		new MiniChampInfo // Grave Knight's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGraveKnight))
			)
		),
		new MiniChampInfo // Gummy Sheep's Pasture
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Sheep))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(WoolWeaver))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantToad)),
				new MiniChampTypeInfo(5, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossGummySheep))
			)
		),
		new MiniChampInfo // Boss Hatshepsut the Queen's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GothicNovelist)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(BoneKnight)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossHatshepsutTheQueen))
			)
		),
		new MiniChampInfo // Boss Hog Wild's Swine Pen
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Pig)),
				new MiniChampTypeInfo(5, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Savage)),
				new MiniChampTypeInfo(10, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodFox)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossHogWild))
			)
		),
		new MiniChampInfo // Boss Howler Monkey's Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HowlerMonkey)),
				new MiniChampTypeInfo(15, typeof(Gorilla)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Gorilla)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Gorilla)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossHowlerMonkey))
			)
		),
		new MiniChampInfo // Boss Huntsman Spider's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossHuntsmanSpider))
			)
		),
		new MiniChampInfo // Boss Hydrokinetic Warden's Water Shrine
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WaterElemental)),
				new MiniChampTypeInfo(15, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(SeaHorse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Leviathan)),
				new MiniChampTypeInfo(10, typeof(AquaticTamer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(CrystalElemental)),
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossHydrokineticWarden))
			)
		),
		new MiniChampInfo // Boss Ibex Highland
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GreatHart)),
				new MiniChampTypeInfo(15, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(Boar))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIbex))
			)
		),
		new MiniChampInfo // Boss Indian Palm Squirrel Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(JackRabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Goat)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ferret)),
				new MiniChampTypeInfo(5, typeof(RidableLlama))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIndianPalmSquirrel))
			)
		),
		new MiniChampInfo // Boss Infernal Lich Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(20, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossInfernalLich))
			)
		),
		new MiniChampInfo // Boss Infernal Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(BullFrog)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(3, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossInfernalToad))
			)
		),
		new MiniChampInfo // Boss Inferno Sentinel Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossInfernoSentinel))
			)
		),
		new MiniChampInfo // Inferno Stallion Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireElemental)),
				new MiniChampTypeInfo(15, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HellHound)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossInfernoStallion))
			)
		),
		new MiniChampInfo // Infinite Pouncer's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wolf)),
				new MiniChampTypeInfo(15, typeof(Cougar)),
				new MiniChampTypeInfo(10, typeof(Lion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Lion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WhiteWolf)),
				new MiniChampTypeInfo(5, typeof(Lion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossInfinitePouncer))
			)
		),
		new MiniChampInfo // Ironclad Defender's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(5, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIroncladDefender))
			)
		),
		new MiniChampInfo // Ironclad Ogre's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Cyclops)),
				new MiniChampTypeInfo(10, typeof(Brigand))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantTurkey)),
				new MiniChampTypeInfo(5, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIroncladOgre))
			)
		),
		new MiniChampInfo // Iron Golem's Workshop
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(15, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(AnvilHurler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IronSmith)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(Mimic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIronGolem))
			)
		),
		new MiniChampInfo // Iron Steed Stables
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RamRider)),
				new MiniChampTypeInfo(10, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HolyKnight)),
				new MiniChampTypeInfo(10, typeof(ShieldMaiden))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BaroqueBarbarian)),
				new MiniChampTypeInfo(5, typeof(KnightOfMercy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossIronSteed))
			)
		),
		new MiniChampInfo // Javelina Jinx Hunt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Boar)),
				new MiniChampTypeInfo(15, typeof(Hind)),
				new MiniChampTypeInfo(10, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SavageShaman)),
				new MiniChampTypeInfo(10, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossJavelinaJinx))
			)
		),
		new MiniChampInfo // Jellybean Jester's Carnival
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RaveRogue)),
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossJellybeanJester))
			)
		),
		new MiniChampInfo // Kas the Bloody-Handed Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ghoul)),
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Zombie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossKasTheBloodyHanded))
			)
		),
		new MiniChampInfo // Kel'Thuzad's Frozen Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(IceElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostDragon)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lich)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossKelThuzad))
			)
		),
		new MiniChampInfo // Khufu the Great Builder's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LichLord)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossKhufuTheGreatBuilder))
			)
		),
		new MiniChampInfo // Larloch the Shadow King's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(Lich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLarlochTheShadowKing))
			)
		),
		new MiniChampInfo // Leo the Harpy's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Harpy)),
				new MiniChampTypeInfo(15, typeof(AirElemental)),
				new MiniChampTypeInfo(5, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wyvern))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WyvernRenowned)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLeoHarpy))
			)
		),
		new MiniChampInfo // Leo the Sun Bear's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BrownBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(Savage)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SavageShaman)),
				new MiniChampTypeInfo(5, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLeoSunBear))
			)
		),
		new MiniChampInfo // Leprosy Rat Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Rat)),
				new MiniChampTypeInfo(20, typeof(PestilentBandage)),
				new MiniChampTypeInfo(10, typeof(PlagueRat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(5, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ratman)),
				new MiniChampTypeInfo(10, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLeprosyRat))
			)
		),
		new MiniChampInfo // Boss Libra Balance Bear
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BrownBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLibraBalanceBear))
			)
		),
		new MiniChampInfo // Boss Libra Harpy
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bird)),
				new MiniChampTypeInfo(15, typeof(Chicken)),
				new MiniChampTypeInfo(10, typeof(Crane))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(5, typeof(Phoenix))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLibraHarpy))
			)
		),
		new MiniChampInfo // Boss Licorice Sheep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Sheep)),
				new MiniChampTypeInfo(10, typeof(Goat)),
				new MiniChampTypeInfo(10, typeof(Llama))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WoolWeaver)),
				new MiniChampTypeInfo(5, typeof(BattleWeaver))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLicoriceSheep))
			)
		),
		new MiniChampInfo // Boss Lollipop Lord
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(15, typeof(RaveRogue)),
				new MiniChampTypeInfo(10, typeof(DrumBoy))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(10, typeof(VaudevilleValkyrie))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkaSkald)),
				new MiniChampTypeInfo(5, typeof(GlamRockMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLollipopLord))
			)
		),
		new MiniChampInfo // Boss Luchador Llama
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Horse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SumoWrestler)),
				new MiniChampTypeInfo(10, typeof(TaekwondoMaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KatanaDuelist)),
				new MiniChampTypeInfo(5, typeof(SteampunkSamurai))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossLuchadorLlama))
			)
		),
		new MiniChampInfo // BossMalariaRat Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRS)),
				new MiniChampTypeInfo(15, typeof(ClanRibbonPlagueRat)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PlagueRat)),
				new MiniChampTypeInfo(5, typeof(Zombie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMalariaRat))
			)
		),
		new MiniChampInfo // BossMandrillShaman Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Savage)),
				new MiniChampTypeInfo(15, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(OrcCaptain))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SavageShaman)),
				new MiniChampTypeInfo(5, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMandrillShaman))
			)
		),
		new MiniChampInfo // BossMarkhor Peaks
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Markhor)),
				new MiniChampTypeInfo(15, typeof(SavageShaman)),
				new MiniChampTypeInfo(10, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Lion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lion)),
				new MiniChampTypeInfo(5, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMarkhor))
			)
		),
		new MiniChampInfo // BossMeatGolem Laboratory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(Mimic)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MeatGolem)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneKnight)),
				new MiniChampTypeInfo(5, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMeatGolem))
			)
		),
		new MiniChampInfo // BossMelodicSatyr Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MelodicSatyr)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMelodicSatyr))
			)
		),
		new MiniChampInfo // Mentuhotep the Wise Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Lich)),
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMentuhotepTheWise))
			)
		),
		new MiniChampInfo // Metallic Windsteed Peaks
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(15, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(MetallicWindsteed))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MetallicWindsteed)),
				new MiniChampTypeInfo(10, typeof(AirElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMetallicWindsteed))
			)
		),
		new MiniChampInfo // Mimicron’s Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(Mimic)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ravager)),
				new MiniChampTypeInfo(10, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMimicron))
			)
		),
		new MiniChampInfo // Mire Spawner Marsh
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bogling)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PestilentBandage)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Slime)),
				new MiniChampTypeInfo(5, typeof(Bogling))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMireSpawner))
			)
		),
		new MiniChampInfo // Molten Slime Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaLizard)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMoltenSlime))
			)
		),
		new MiniChampInfo // Boss Mountain Gorilla
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Gorilla)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodFox)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMountainGorilla))
			)
		),
		new MiniChampInfo // Boss Muck Golem
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PestilentBandage)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MuckGolem)),
				new MiniChampTypeInfo(5, typeof(BogThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMuckGolem))
			)
		),
		new MiniChampInfo // Boss Mystic Fallow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(GreatHart)),
				new MiniChampTypeInfo(10, typeof(Hind))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestRanger)),
				new MiniChampTypeInfo(5, typeof(ForestScout))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMysticFallow))
			)
		),
		new MiniChampInfo // Boss Mystic Satyr
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MysticSatyr)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossMysticSatyr))
			)
		),
		new MiniChampInfo // Boss Nagash
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(5, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNagash))
			)
		),
		new MiniChampInfo // Nano Swarm Lab
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(10, typeof(StarCitizen))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(5, typeof(RenaissanceMechanic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNanoSwarm))
			)
		),
		new MiniChampInfo // Nebula Cat's Celestial Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Phoenix)),
				new MiniChampTypeInfo(15, typeof(Macaw)),
				new MiniChampTypeInfo(10, typeof(Crane))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(10, typeof(Parrot))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNebulaCat))
			)
		),
		new MiniChampInfo // Necrotic General's Battlefield
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneDemon)),
				new MiniChampTypeInfo(5, typeof(Revenant))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNecroticGeneral))
			)
		),
		new MiniChampInfo // Necrotic Lich's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Spectre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(InterredGrizzle))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNecroticLich))
			)
		),
		new MiniChampInfo // Necrotic Ogre's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNecroticOgre))
			)
		),
		new MiniChampInfo // Boss Nefertiti's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Ghoul))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNefertiti))
			)
		),
		new MiniChampInfo // Nemesis Unit Facility
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(RetroAndroid))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNemesisUnit))
			)
		),
		new MiniChampInfo // Nightmare Leaper Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Nightmare)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNightmareLeaper))
			)
		),
		new MiniChampInfo // Nightmare Panther's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(SpectralWolf)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(10, typeof(DarkWisp))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Nightmare)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossNightmarePanther))
			)
		),
		new MiniChampInfo // Omega Sentinel's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(StarCitizen))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StarCitizen)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossOmegaSentinel))
			)
		),
		new MiniChampInfo // Orangutan Sage Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Gorilla)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestRanger)),
				new MiniChampTypeInfo(5, typeof(CuSidhe))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossOrangutanSage))
			)
		),
		new MiniChampInfo // Overlord MkII Stronghold
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(15, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(CyberpunkSorcerer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(5, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossOverlordMkII))
			)
		),
		new MiniChampInfo // Peccary Protector Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Horse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Gorilla)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lion)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPeccaryProtector))
			)
		),
		new MiniChampInfo // Peppermint Puff Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SlyStoryteller)),
				new MiniChampTypeInfo(15, typeof(BluesSingingGorgon)),
				new MiniChampTypeInfo(10, typeof(Flutist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GlamRockMage)),
				new MiniChampTypeInfo(5, typeof(SkaSkald))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VaudevilleValkyrie)),
				new MiniChampTypeInfo(5, typeof(RaveRogue))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPeppermintPuff))
			)
		),
		new MiniChampInfo // Phantom Automaton Vault
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Mimic)),
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPhantomAutomaton))
			)
		),
		new MiniChampInfo // Phantom Panther's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Spectre)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPhantomPanther))
			)
		),
		new MiniChampInfo // Plague Lich's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Zombie)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mummy)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPlagueLich))
			)
		),
		new MiniChampInfo // Plasma Juggernaut's Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(15, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(IronSmith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IronSmith)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPlasmaJuggernaut))
			)
		),
		new MiniChampInfo // Purse Spider's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossPurseSpider))
			)
		),
		new MiniChampInfo // Quantum Guardian's Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CyberpunkSorcerer)),
				new MiniChampTypeInfo(15, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(StarCitizen)),
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossQuantumGuardian))
			)
		),
		new MiniChampInfo // Rabid Rat Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRS)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(15, typeof(ClanRibbonPlagueRat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GrayGoblin)),
				new MiniChampTypeInfo(5, typeof(GrayGoblinMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ratman)),
				new MiniChampTypeInfo(5, typeof(Bogling))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRabidRat))
			)
		),
		new MiniChampInfo // Radiant Slime Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(CorrosiveSlime)),
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ValoriteElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRadiantSlime))
			)
		),
		new MiniChampInfo // Raistlin Majere's Tower
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(15, typeof(Magician)),
				new MiniChampTypeInfo(10, typeof(ScrollMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(ElementalWizard)),
				new MiniChampTypeInfo(5, typeof(Enchanter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRaistlinMajere))
			)
		),
		new MiniChampInfo // Ramses the Immortal Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(AncientLich)),
				new MiniChampTypeInfo(15, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneDemon)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRamsesTheImmortal))
			)
		),
		new MiniChampInfo // Red Squirrel Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(RedSquirrel)),
				new MiniChampTypeInfo(10, typeof(Rabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(Rabbit))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRedSquirrel))
			)
		),
		new MiniChampInfo // Red-Tailed Squirrel Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(JackRabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BloodFox)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRedTailedSquirrel))
			)
		),
		new MiniChampInfo // Rhythmic Satyr’s Glade
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(15, typeof(FairyQueen))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Satyr)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Pixie)),
				new MiniChampTypeInfo(10, typeof(Nymph))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRhythmicSatyr))
			)
		),
		new MiniChampInfo // Rock Squirrel Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GrayGoblin)),
				new MiniChampTypeInfo(5, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossRockSquirrel))
			)
		),
		new MiniChampInfo // Sable Antelope Savanna
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SableAntelope)),
				new MiniChampTypeInfo(10, typeof(Goat)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreatHart)),
				new MiniChampTypeInfo(5, typeof(Cougar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSableAntelope))
			)
		),
		new MiniChampInfo // Sagittarius Archer Bear Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(GreatHart)),
				new MiniChampTypeInfo(15, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyDragon)),
				new MiniChampTypeInfo(5, typeof(Harpy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Dryad)),
				new MiniChampTypeInfo(10, typeof(Centaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSagittariusArcherBear))
			)
		),
		new MiniChampInfo // Sagittarius Harpy’s Perch
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FairyDragon)),
				new MiniChampTypeInfo(15, typeof(Harpy)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyDragon)),
				new MiniChampTypeInfo(10, typeof(Dryad)),
				new MiniChampTypeInfo(5, typeof(Centaur))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Centaur)),
				new MiniChampTypeInfo(5, typeof(Dryad))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSagittariusHarpy))
			)
		),
		new MiniChampInfo // Sand Golem’s Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StoneGolem)),
				new MiniChampTypeInfo(15, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(SkeletonWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(SkeletonWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mummy)),
				new MiniChampTypeInfo(5, typeof(StoneGolem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSandGolem))
			)
		),
		new MiniChampInfo // Scorpio Harpy's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Harpy)),
				new MiniChampTypeInfo(10, typeof(Scorpion)),
				new MiniChampTypeInfo(5, typeof(StoneGolem))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StoneGolem)),
				new MiniChampTypeInfo(10, typeof(Mummy)),
				new MiniChampTypeInfo(10, typeof(Harpy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mummy)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioHarpy))
			)
		),
		new MiniChampInfo // Scorpion Spider’s Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Scorpion)),
				new MiniChampTypeInfo(10, typeof(ScorpionSpider)),
				new MiniChampTypeInfo(5, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScorpionSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpionSpider))
			)
		),
		new MiniChampInfo // Scorpio Venom Bear’s Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackBear)),
				new MiniChampTypeInfo(15, typeof(Scorpion)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(Scorpion)),
				new MiniChampTypeInfo(5, typeof(ScorpionSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonElemental)),
				new MiniChampTypeInfo(5, typeof(ScorpioVenomBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // Seti the Avenger's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BoneKnight)),
				new MiniChampTypeInfo(15, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // Shadowblade Assassin's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(10, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SneakyNinja)),
				new MiniChampTypeInfo(5, typeof(ShadowGolem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // Shadow Golem's Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(15, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(PatchworkMonster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // Shadow Lich's Necropolis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wraith)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(LichLord))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // Shadow Muntjac's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DireWolf)),
				new MiniChampTypeInfo(10, typeof(ShadowGolem))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowGolem)),
				new MiniChampTypeInfo(5, typeof(ShadowMuntjac))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossScorpioVenomBear))
			)
		),
		new MiniChampInfo // BossShadowOgre's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(5, typeof(OrcBrute))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossShadowOgre))
			)
		),
		new MiniChampInfo // BossShadowProwler's Hunt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Spectre)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorQueen))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossShadowProwler))
			)
		),
		new MiniChampInfo // BossShadowSludge's Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(10, typeof(Slime))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidSlug)),
				new MiniChampTypeInfo(5, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossShadowSludge))
			)
		),
		new MiniChampInfo // BossShadowToad's Bog
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(BlackSolenWorker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AcidElemental)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossShadowToad))
			)
		),
		new MiniChampInfo // BossSifakaWarrior's Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WildTiger)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSifakaWarrior))
			)
		),
		new MiniChampInfo // Boss Smallpox Rat Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClanRibbonPlagueRat)),
				new MiniChampTypeInfo(15, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PestilentBandage)),
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSmallpoxRat))
			)
		),
		new MiniChampInfo // Boss Sombrero de Sol Llama
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Llama)),
				new MiniChampTypeInfo(15, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(BigCatTamer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSombreroDeSolLlama))
			)
		),
		new MiniChampInfo // Boss Sombrero Llama
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(Goat)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Sheep))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lion)),
				new MiniChampTypeInfo(5, typeof(Horse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSombreroLlama))
			)
		),
		new MiniChampInfo // Boss Soth the Death Knight
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneMagi)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSothTheDeathKnight))
			)
		),
		new MiniChampInfo // Boss Soul Eater Lich
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(LichLord)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(3, typeof(BoneDemon)),
				new MiniChampTypeInfo(2, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSoulEaterLich))
			)
		),
		new MiniChampInfo // Spectral Automaton Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(15, typeof(AnvilHurler)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(10, typeof(Golem))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mimic)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSpectralAutomaton))
			)
		),
		new MiniChampInfo // Spectral Toad Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(Bogling))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(InterredGrizzle))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(SpectralToad))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSpectralToad))
			)
		),
		new MiniChampInfo // Spectral Warden Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSpectralWarden))
			)
		),
		new MiniChampInfo // Spiderling Minion Broodmother
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SentinelSpider)),
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorQueen)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSpiderlingMinionBroodmother))
			)
		),
		new MiniChampInfo // Spider Monkey Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SpiderMonkey)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSpiderMonkey))
			)
		),
		new MiniChampInfo // Starborn Predator Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(10, typeof(SerpentineDragon)),
				new MiniChampTypeInfo(10, typeof(PlatinumDrake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CrimsonDrake)),
				new MiniChampTypeInfo(10, typeof(GreaterDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreaterDragon)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStarbornPredator))
			)
		),
		new MiniChampInfo // Steam Leviathan Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(15, typeof(InsaneRoboticist)),
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RetroAndroid)),
				new MiniChampTypeInfo(10, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StarCitizen)),
				new MiniChampTypeInfo(5, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSteamLeviathan))
			)
		),
		new MiniChampInfo // Stone Golem Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(StoneGolem)),
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(SkeletalDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStoneGolem))
			)
		),
		new MiniChampInfo // Stone Steed Stables
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(StoneGolem)),
				new MiniChampTypeInfo(15, typeof(PatchworkSkeleton)),
				new MiniChampTypeInfo(10, typeof(Horse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(SkeletalDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStoneSteed))
			)
		),
		new MiniChampInfo // Storm Bone Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneDemon)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormBone))
			)
		),
		new MiniChampInfo // BossSkeleton - The Tempest's Wrath
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(StormConjurer)),
				new MiniChampTypeInfo(15, typeof(WindElemental)),
				new MiniChampTypeInfo(10, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WindElemental)),
				new MiniChampTypeInfo(5, typeof(PsychedelicShaman))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormLich))
			)
		),
		new MiniChampInfo // BossStormLich - The Storm of Death
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(LichLord))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LichLord)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormLich))
			)
		),
		new MiniChampInfo // BossStormOgre - The Wrath of the Thunder King
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(15, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Cyclops)),
				new MiniChampTypeInfo(5, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(BaroqueBarbarian))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormOgre))
			)
		),
		new MiniChampInfo // BossStormSika - The Forest Tempest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(15, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CuSidhe)),
				new MiniChampTypeInfo(10, typeof(Unicorn))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kirin)),
				new MiniChampTypeInfo(5, typeof(Reptalon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormSika))
			)
		),
		new MiniChampInfo // BossStormWolf - The Tempest's Fury
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TimberWolf)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SilverSerpent)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossStormWolf))
			)
		),
		new MiniChampInfo // Synthroid Prime Factory
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RetroAndroid)),
				new MiniChampTypeInfo(15, typeof(RetroRobotRomancer)),
				new MiniChampTypeInfo(10, typeof(InsaneRoboticist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StarCitizen)),
				new MiniChampTypeInfo(5, typeof(StarfleetCaptain))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StarfleetCaptain)),
				new MiniChampTypeInfo(5, typeof(RenaissanceMechanic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSynthroidPrime))
			)
		),
		new MiniChampInfo // Szass Tam's Necropolis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Lich)),
				new MiniChampTypeInfo(15, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossSzassTam))
			)
		),
		new MiniChampInfo // Taco Llama Festival
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(25, typeof(Squirrel)),
				new MiniChampTypeInfo(15, typeof(Llama)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DireWolf)),
				new MiniChampTypeInfo(5, typeof(VorpalBunny))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTacoLlama))
			)
		),
		new MiniChampInfo // Tactical Enforcer Operations
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SteampunkSamurai)),
				new MiniChampTypeInfo(15, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(10, typeof(KnightOfJustice))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RapierDuelist)),
				new MiniChampTypeInfo(10, typeof(ShieldBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FieldCommander)),
				new MiniChampTypeInfo(5, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTacticalEnforcer))
			)
		),
		new MiniChampInfo // Taffy Titan's Arena
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Banneret)),
				new MiniChampTypeInfo(15, typeof(CombatMedic)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(5, typeof(SabreFighter))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(KnightOfMercy)),
				new MiniChampTypeInfo(5, typeof(KnightOfValor))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTaffyTitan))
			)
		),
		new MiniChampInfo // BossTahr's Wild Horde
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Savage)),
				new MiniChampTypeInfo(15, typeof(SavageShaman)),
				new MiniChampTypeInfo(10, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenGoblin)),
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Troll)),
				new MiniChampTypeInfo(5, typeof(Ogre))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTahr))
			)
		),
		new MiniChampInfo // BossTalonMachine's Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(AnvilHurler)),
				new MiniChampTypeInfo(15, typeof(IronSmith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(Mimic))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTalonMachine))
			)
		),
		new MiniChampInfo // BossTaurusEarthBear's Dominion
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Boar)),
				new MiniChampTypeInfo(15, typeof(BrownBear)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Gorilla)),
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTaurusEarthBear))
			)
		),
		new MiniChampInfo // BossTaurusHarpy's Skies
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Parrot)),
				new MiniChampTypeInfo(15, typeof(Eagle)),
				new MiniChampTypeInfo(10, typeof(Crane))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wyvern)),
				new MiniChampTypeInfo(10, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Dragon)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTaurusHarpy))
			)
		),
		new MiniChampInfo // BossTempestSatyr's Storm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(15, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTempestSatyr))
			)
		),
		new MiniChampInfo // Tequila Llama Tavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(RidableLlama)),
				new MiniChampTypeInfo(10, typeof(Pig)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Gorilla)),
				new MiniChampTypeInfo(10, typeof(Llama))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(Lion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTequilaLlama))
			)
		),
		new MiniChampInfo // Thutmose the Conqueror's Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(15, typeof(Savage)),
				new MiniChampTypeInfo(10, typeof(OrcCaptain))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(JukaWarrior)),
				new MiniChampTypeInfo(5, typeof(JukaLord)),
				new MiniChampTypeInfo(10, typeof(Brigand))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoonElite)),
				new MiniChampTypeInfo(5, typeof(Minotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossThutmoseTheConqueror))
			)
		),
		new MiniChampInfo // Tidal Mare's Deep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(AquaticTamer)),
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Leviathan)),
				new MiniChampTypeInfo(10, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(5, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTidalMare))
			)
		),
		new MiniChampInfo // Toxic Lich's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(RottingCorpse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossToxicLich))
			)
		),
		new MiniChampInfo // Toxic Ogre's Stronghold
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoonElite)),
				new MiniChampTypeInfo(5, typeof(SavageRider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossToxicOgre))
			)
		),
		new MiniChampInfo // Toxic Sludge Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(15, typeof(Slime)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(VeriteElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossToxicSludge))
			)
		),
		new MiniChampInfo // Trapdoor Spider Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTrapdoorSpider))
			)
		),
		new MiniChampInfo // Tsunami Titan's Deep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Kraken))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(SerpentineDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTsunamiTitan))
			)
		),
		new MiniChampInfo // Tutankhamun The Cursed Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTutankhamunTheCursed))
			)
		),
		new MiniChampInfo // Typhus Rat Infestation
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GrayGoblin)),
				new MiniChampTypeInfo(15, typeof(GrayGoblinMage)),
				new MiniChampTypeInfo(10, typeof(GrayGoblinKeeper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PlagueRat)),
				new MiniChampTypeInfo(5, typeof(OrcBomber))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ratman)),
				new MiniChampTypeInfo(5, typeof(Savage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossTyphusRat))
			)
		),
		new MiniChampInfo // Vampiric Blade's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(GhostScout))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVampiricBlade))
			)
		),
		new MiniChampInfo // Vecna's Sanctum
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(15, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lich)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVecna))
			)
		),
		new MiniChampInfo // Venomous Roe's Marsh
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(VenomousWolf)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(15, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(VenomousSerpent)),
				new MiniChampTypeInfo(10, typeof(SerpentHandler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(VenomousDragon)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVenomousRoe))
			)
		),
		new MiniChampInfo // Venomous Toad's Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(ToxicElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(SerpentHandler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVenomousToad))
			)
		),
		new MiniChampInfo // Venomous Wolf's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(VenomousWolf)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(VenomousSerpent)),
				new MiniChampTypeInfo(10, typeof(SilverSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilverSerpent)),
				new MiniChampTypeInfo(5, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVenomousWolf))
			)
		),
		new MiniChampInfo // BossVietnamesePig
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pig)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(LeatherWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(Savage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SavageRider)),
				new MiniChampTypeInfo(5, typeof(GrayGoblin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVietnamesePig))
			)
		),
		new MiniChampInfo // BossVileToad
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantToad)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(15, typeof(Snake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVileToad))
			)
		),
		new MiniChampInfo // BossVirgoHarpy
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FairyDragon)),
				new MiniChampTypeInfo(15, typeof(Harpy)),
				new MiniChampTypeInfo(10, typeof(Phoenix))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(10, typeof(Crane))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenHag)),
				new MiniChampTypeInfo(5, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVirgoHarpy))
			)
		),
		new MiniChampInfo // BossVirgoPurityBear
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BlackBear)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PolarBear)),
				new MiniChampTypeInfo(5, typeof(Cougar))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVirgoPurityBear))
			)
		),
		new MiniChampInfo // BossVoidCat
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(LeatherWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorQueen))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVoidCat))
			)
		),
		new MiniChampInfo // BossVoidSlime
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Slime)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AcidSlug)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVoidSlime))
			)
		),
		new MiniChampInfo // BossVolcanicCharger
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaSnake)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(HellCat))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVolcanicCharger))
			)
		),
		new MiniChampInfo // BossVortexConstruct
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ClockworkEngineer)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer)),
				new MiniChampTypeInfo(10, typeof(Golem))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVortexConstruct))
			)
		),
		new MiniChampInfo // BossVortexWraith
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossVortexWraith))
			)
		),
		new MiniChampInfo // BossWarthogWarrior
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Pig)),
				new MiniChampTypeInfo(5, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Boar)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWarthogWarrior))
			)
		),
		new MiniChampInfo // Whispering Pooka Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Pixie)),
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(3, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWhisperingPooka))
			)
		),
		new MiniChampInfo // Wicked Satyr's Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(Satyr)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(Succubus))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyQueen)),
				new MiniChampTypeInfo(3, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWickedSatyr))
			)
		),
		new MiniChampInfo // Wood Golem's Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Treefellow)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(5, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Forager)),
				new MiniChampTypeInfo(3, typeof(Treefellow))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWoodGolem))
			)
		),
		new MiniChampInfo // Woodland Charger Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WildTiger)),
				new MiniChampTypeInfo(15, typeof(Lion)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Lion)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WoodlandCharger)),
				new MiniChampTypeInfo(3, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWoodlandCharger))
			)
		),
		new MiniChampInfo // Woodland Spirit Horse Meadow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Horse)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SpiritWolf)),
				new MiniChampTypeInfo(5, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WoodlandSpiritHorse)),
				new MiniChampTypeInfo(3, typeof(FairyDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWoodlandSpiritHorse))
			)
		),
		new MiniChampInfo // BossWraithLich Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(20, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossWraithLich))
			)
		),
		new MiniChampInfo // BossYangStallion Plains
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HolyKnight)),
				new MiniChampTypeInfo(15, typeof(KnightOfValor)),
				new MiniChampTypeInfo(10, typeof(RamRider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(KnightOfJustice)),
				new MiniChampTypeInfo(10, typeof(KnightOfMercy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SabreFighter)),
				new MiniChampTypeInfo(5, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossYangStallion))
			)
		),
		new MiniChampInfo // BossYinSteed Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BossYinSteed))
			)
		),
		new MiniChampInfo // Breeze Phantom Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BreezePhantomBoss))
			)
		),
		new MiniChampInfo // Bubble Ferret Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BubbleFerret)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SerpentHandler)),
				new MiniChampTypeInfo(10, typeof(SheepdogHandler))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Llama)),
				new MiniChampTypeInfo(5, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(BubbleFerretBoss))
			)
		),
		new MiniChampInfo // Celestial Dragon Shrine
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FairyDragon)),
				new MiniChampTypeInfo(10, typeof(Dragon)),
				new MiniChampTypeInfo(10, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CrimsonDrake)),
				new MiniChampTypeInfo(10, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SerpentineDragon)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CelestialDragonBoss))
			)
		),
		new MiniChampInfo // Cerebral Ettin Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(JukaLord)),
				new MiniChampTypeInfo(10, typeof(JukaWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantTurkey)),
				new MiniChampTypeInfo(10, typeof(Ogre))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(OgreLord)),
				new MiniChampTypeInfo(5, typeof(Troll))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CerebralEttinBoss))
			)
		),
		new MiniChampInfo // Chaneque Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Chaneque)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(FairyQueen))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ChanequeBoss))
			)
		),
		new MiniChampInfo // Chimereon Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(10, typeof(MegaDragon)),
				new MiniChampTypeInfo(10, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ChimereonBoss))
			)
		),
		new MiniChampInfo // Cinder Wraith Ruins
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FireDaemon)),
				new MiniChampTypeInfo(15, typeof(HellHound)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(FireElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CinderWraithBoss))
			)
		),
		new MiniChampInfo // Corrupting Creeper Forest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(15, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Bogling)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(Slime))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CorruptingCreeperBoss))
			)
		),
		new MiniChampInfo // Crystal Dragon Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(CrystalElemental)),
				new MiniChampTypeInfo(15, typeof(GoldenElemental)),
				new MiniChampTypeInfo(10, typeof(SilverSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(CrystalDragon)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ValoriteElemental)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CrystalDragonBoss))
			)
		),
		new MiniChampInfo // Crystal Warden Temple
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SkeletalMage)),
				new MiniChampTypeInfo(15, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CrystalWardenBoss))
			)
		),
		new MiniChampInfo // Cursed Harbinger Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GhostScout)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WailingBanshee)),
				new MiniChampTypeInfo(5, typeof(GhostWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(3, typeof(InterredGrizzle))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CursedHarbingerBoss))
			)
		),
		new MiniChampInfo // Cyclone Demon Plains
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(Imp)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(10, typeof(Daemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(TormentedMinotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(CycloneDemonBoss))
			)
		),
		new MiniChampInfo // Dairy Wraith Field
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(Wight)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(InterredGrizzle)),
				new MiniChampTypeInfo(5, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DairyWraithBoss))
			)
		),
		new MiniChampInfo // Deadlord Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneDemon)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DeadlordBoss))
			)
		),
		new MiniChampInfo // Dreaded Creeper Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DreadedCreeperBoss))
			)
		),
		new MiniChampInfo // Dreamy Ferret Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ferret)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(VorpalBunny)),
				new MiniChampTypeInfo(5, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(3, typeof(GrayGoblin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DreamyFerretBoss))
			)
		),
		new MiniChampInfo // Drolatic Wastes
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantIceWorm)),
				new MiniChampTypeInfo(10, typeof(BogThing)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwampThing)),
				new MiniChampTypeInfo(5, typeof(Bogling))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DrolaticBoss))
			)
		),
		new MiniChampInfo // Dryad Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Dryad)),
				new MiniChampTypeInfo(15, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HostileDruid)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(DryadBoss))
			)
		),
		new MiniChampInfo // Earthquake Ettin Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ogre)),
				new MiniChampTypeInfo(20, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ettin)),
				new MiniChampTypeInfo(5, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Cyclops)),
				new MiniChampTypeInfo(3, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EarthquakeEttinBoss))
			)
		),
		new MiniChampInfo // Elder Tendril Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(SwampThing)),
				new MiniChampTypeInfo(15, typeof(Bogling))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ElderTendrilBoss))
			)
		),
		new MiniChampInfo // Ember Serpent Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaSnake)),
				new MiniChampTypeInfo(15, typeof(LavaLizard)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireElemental)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EmberSerpentBoss))
			)
		),
		new MiniChampInfo // Ember Spirit Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireMage)),
				new MiniChampTypeInfo(15, typeof(FlameElemental)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSerpent)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Succubus)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EmberSpiritBoss))
			)
		),
		new MiniChampInfo // Ethereal Crab Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(IceCrab)),
				new MiniChampTypeInfo(15, typeof(EtherealWarrior)),
				new MiniChampTypeInfo(10, typeof(AquaticTamer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EtherealWarrior)),
				new MiniChampTypeInfo(5, typeof(EtherealCrab))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AquaticTamer)),
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EtherealCrabBoss))
			)
		),
		new MiniChampInfo // Ethereal Dragon's Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(EtherealWarrior)),
				new MiniChampTypeInfo(10, typeof(Dragon)),
				new MiniChampTypeInfo(5, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EtherealWarrior)),
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(EtherealDragonBoss))
			)
		),
		new MiniChampInfo // Firebreath Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Alligator)),
				new MiniChampTypeInfo(15, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(GrayGoblin))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilverSerpent)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FirebreathAlligatorBoss))
			)
		),
		new MiniChampInfo // Fire Rooster Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireAnt)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireElemental)),
				new MiniChampTypeInfo(5, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FireRoosterBoss))
			)
		),
		new MiniChampInfo // Flame Bearer Cave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Pig))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FlameBearBoss))
			)
		),
		new MiniChampInfo // Flame Warden Ettin Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(FlameElemental)),
				new MiniChampTypeInfo(10, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(OrcCaptain))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(OrcBrute)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FlameWardenEttinBoss))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FlameWardenEttinBoss))
			)
		),
		new MiniChampInfo // Flare Imp Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Imp)),
				new MiniChampTypeInfo(15, typeof(FlareImp)),
				new MiniChampTypeInfo(10, typeof(PitFiend))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(10, typeof(DemonKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FlareImpBoss))
			)
		),
		new MiniChampInfo // Fossil Elemental Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BoneKnight)),
				new MiniChampTypeInfo(15, typeof(SkeletalMage)),
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneMagi)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FossilElementalBoss))
			)
		),
		new MiniChampInfo // Frost Bear Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PolarBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostBearBoss))
			)
		),
		new MiniChampInfo // Frostbite Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Alligator)),
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IceElemental)),
				new MiniChampTypeInfo(5, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostbiteAlligatorBoss))
			)
		),
		new MiniChampInfo // Frostbound Behemoth Cave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantTurkey)),
				new MiniChampTypeInfo(15, typeof(Cyclops)),
				new MiniChampTypeInfo(10, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OgreLord))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostboundBehemothBoss))
			)
		),
		new MiniChampInfo // Frost Drakon Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FrostDragon)),
				new MiniChampTypeInfo(15, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostbiteAlligatorBoss)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SnowElemental)),
				new MiniChampTypeInfo(5, typeof(IceElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostDrakonBoss))
			)
		),
		new MiniChampInfo // Frost Hen's Perch
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Chicken)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WhiteWolf)),
				new MiniChampTypeInfo(5, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostHenBoss))
			)
		),
		new MiniChampInfo // Frost Serpent Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(IceSnake)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(WhiteWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostSerpentBoss))
			)
		),
		new MiniChampInfo // Frost Warden Ettin Stronghold
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(GiantTurkey)),
				new MiniChampTypeInfo(10, typeof(Ogre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(10, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostWardenEttinBoss))
			)
		),
		new MiniChampInfo // Frosty Ferret's Burrow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ferret)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SnowElemental)),
				new MiniChampTypeInfo(5, typeof(IceElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(FrostyFerretBoss))
			)
		),
		new MiniChampInfo // Gale Wisp's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Wisp)),
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AirElemental)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GaleWispBoss))
			)
		),
		new MiniChampInfo // Giant Trapdoor Spider Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion)),
				new MiniChampTypeInfo(10, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GiantTrapdoorSpiderBoss))
			)
		),
		new MiniChampInfo // Giant Wolf Spider Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GiantIceWorm)),
				new MiniChampTypeInfo(5, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GiantWolfSpiderBoss))
			)
		),
		new MiniChampInfo // Glimmering Ferret Burrow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ferret)),
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Squirrel))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BigCatTamer)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Beastmaster)),
				new MiniChampTypeInfo(5, typeof(AquaticTamer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GlimmeringFerretBoss))
			)
		),
		new MiniChampInfo // Golden Orb Weaver Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(15, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(5, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GoldenOrbWeaverBoss))
			)
		),
		new MiniChampInfo // Goliath Birdeater Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(15, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(5, typeof(GiantIceWorm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GoliathBirdeaterBoss))
			)
		),
		new MiniChampInfo // Gorgon Viper's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(15, typeof(Snake)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonElemental)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GorgonViperBoss))
			)
		),
		new MiniChampInfo // Granite Colossus Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Golem)),
				new MiniChampTypeInfo(15, typeof(StoneElemental)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(10, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BronzeElemental)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GraniteColossusBoss))
			)
		),
		new MiniChampInfo // Grimorie's Tome
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ScrollMage)),
				new MiniChampTypeInfo(15, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ElementalWizard)),
				new MiniChampTypeInfo(10, typeof(EvilAlchemist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(5, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GrimorieBoss))
			)
		),
		new MiniChampInfo // Grotesque of Rouen's Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(Ghoul)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GrotesqueOfRouenBoss))
			)
		),
		new MiniChampInfo // Grymalkin the Watcher's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PatchworkMonster)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GrymalkinTheWatcherBoss))
			)
		),
		new MiniChampInfo // Guernsey Guardian Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Bull)),
				new MiniChampTypeInfo(15, typeof(Goat)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RamRider)),
				new MiniChampTypeInfo(10, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShieldBearer)),
				new MiniChampTypeInfo(5, typeof(KnightOfJustice))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(GuernseyGuardianBoss))
			)
		),
		new MiniChampInfo // Harmony Ferret Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ferret)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(BirdTrainer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestScout)),
				new MiniChampTypeInfo(5, typeof(Beastmaster))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HarmonyFerretBoss))
			)
		),
		new MiniChampInfo // Hellfire Juggernaut Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HellCat)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(HellHound)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HellfireJuggernautBoss))
			)
		),
		new MiniChampInfo // Hereford Warlock Tower
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Enchanter)),
				new MiniChampTypeInfo(15, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Magician)),
				new MiniChampTypeInfo(10, typeof(ScrollMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(FireMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HerefordWarlockBoss))
			)
		),
		new MiniChampInfo // Highland Bull Herd
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HighlandBull)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(RamRider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Savage)),
				new MiniChampTypeInfo(10, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(5, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HighlandBullBoss))
			)
		),
		new MiniChampInfo // Huntsman Spider's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(DreadSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorQueen)),
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(HuntsmanSpiderBoss))
			)
		),
		new MiniChampInfo // Ice Crab Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(15, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist)),
				new MiniChampTypeInfo(10, typeof(SnowElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IceCrab)),
				new MiniChampTypeInfo(5, typeof(WhiteWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IceCrabBoss))
			)
		),
		new MiniChampInfo // Illusionary Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwampThing)),
				new MiniChampTypeInfo(10, typeof(PatchworkMonster)),
				new MiniChampTypeInfo(10, typeof(GothicNovelist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Mimic))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwampThing)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IllusionaryAlligatorBoss))
			)
		),
		new MiniChampInfo // Illusion Hen's Paradise
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Chicken)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Sheep))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Chicken)),
				new MiniChampTypeInfo(10, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkWisp)),
				new MiniChampTypeInfo(5, typeof(Pixie))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IllusionHenBoss))
			)
		),
		new MiniChampInfo // Illusionist Ettin's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(15, typeof(OrcBomber)),
				new MiniChampTypeInfo(10, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(JukaMage)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mimic)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IllusionistEttinBoss))
			)
		),
		new MiniChampInfo // Infernal Duke's Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(Imp)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernalDukeBoss))
			)
		),
		new MiniChampInfo // Infernal Incinerator Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(FireElemental)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(Succubus))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(AbysmalHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernalIncineratorBoss))
			)
		),
		new MiniChampInfo // Inferno Drakon's Roost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(PlatinumDrake)),
				new MiniChampTypeInfo(10, typeof(Drake)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(Dragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernoDrakonBoss))
			)
		),
		new MiniChampInfo // Inferno Python Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantSerpent)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(SilverSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SeaSerpent)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernoPythonBoss))
			)
		),
		new MiniChampInfo // Inferno Warden's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(15, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(MaddeningHorror))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(InfernoWardenBoss))
			)
		),
		new MiniChampInfo // Ish Kar the Forgotten Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DarkElf)),
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalLich)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(IshKarTheForgottenBoss))
			)
		),
		new MiniChampInfo // Jersey Enchantress Coven
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(NymphSinger)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyDragon)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreenHag)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(JerseyEnchantressBoss))
			)
		),
		new MiniChampInfo // Lava Crab Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaLizard)),
				new MiniChampTypeInfo(15, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LavaCrabBoss))
			)
		),
		new MiniChampInfo // Lava Fiend Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaSerpent)),
				new MiniChampTypeInfo(15, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(FireAnt))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LavaFiendBoss))
			)
		),
		new MiniChampInfo // Leaf Bear Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Hind))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Cougar)),
				new MiniChampTypeInfo(5, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LeafBearBoss))
			)
		),
		new MiniChampInfo // Leprechaun's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GreenGoblin)),
				new MiniChampTypeInfo(15, typeof(GrayGoblin)),
				new MiniChampTypeInfo(10, typeof(Pixie))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Pixie)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FairyQueen)),
				new MiniChampTypeInfo(5, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LeprechaunBoss))
			)
		),
		new MiniChampInfo // Light Bearer's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Enchanter)),
				new MiniChampTypeInfo(15, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(10, typeof(FireMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(AvatarOfElements))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(LightBearBoss))
			)
		),
		new MiniChampInfo // Magma Golem Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaSerpent)),
				new MiniChampTypeInfo(15, typeof(FireAnt)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(HellCat))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MagmaGolemBoss))
			)
		),
		new MiniChampInfo // Magnetic Crab Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SentinelSpider)),
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Golem)),
				new MiniChampTypeInfo(5, typeof(TrapEngineer))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MagneticCrabBoss))
			)
		),
		new MiniChampInfo // Maine Coon Titan's Roost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Cougar)),
				new MiniChampTypeInfo(15, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(Lion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Gorilla)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MaineCoonTitanBoss))
			)
		),
		new MiniChampInfo // Milking Demon Stables
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Imp)),
				new MiniChampTypeInfo(15, typeof(DemonKnight)),
				new MiniChampTypeInfo(10, typeof(Succubus))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(10, typeof(EffetePutridGargoyle))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MilkingDemonBoss))
			)
		),
		new MiniChampInfo // Molten Golem Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LavaElemental)),
				new MiniChampTypeInfo(15, typeof(LavaSerpent)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireDaemon)),
				new MiniChampTypeInfo(10, typeof(HellHound))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireElemental)),
				new MiniChampTypeInfo(5, typeof(HellCat))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MoltenGolemBoss))
			)
		),
		new MiniChampInfo // Mordrake's Manor
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GhostWarrior)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wraith))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mummy)),
				new MiniChampTypeInfo(5, typeof(Revenant))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MordrakeBoss))
			)
		),
		new MiniChampInfo // Mud Golem Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(MudGolem)),
				new MiniChampTypeInfo(15, typeof(Bogling)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(10, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(BogThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MudGolemBoss))
			)
		),
		new MiniChampInfo // Mystic Ferret Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(MysticFerret)),
				new MiniChampTypeInfo(15, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(BloodFox))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MysticFerretBoss))
			)
		),
		new MiniChampInfo // Mystic Fowl Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Phoenix)),
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(15, typeof(Chicken))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Macaw)),
				new MiniChampTypeInfo(10, typeof(Parrot))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Parrot)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MysticFowlBoss))
			)
		),
		new MiniChampInfo // Mystic Wisp Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(Wisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(MysticWispBoss))
			)
		),
		new MiniChampInfo // Nature Dragon's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(CuSidhe)),
				new MiniChampTypeInfo(10, typeof(Unicorn)),
				new MiniChampTypeInfo(10, typeof(Hiryu))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Reptalon)),
				new MiniChampTypeInfo(10, typeof(Nightmare))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Kirin)),
				new MiniChampTypeInfo(5, typeof(Dragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NatureDragonBoss))
			)
		),
		new MiniChampInfo // Necro Ettin Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneKnight)),
				new MiniChampTypeInfo(10, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Lich)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NecroEttinBoss))
			)
		),
		new MiniChampInfo // Necro Rooster Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ghoul)),
				new MiniChampTypeInfo(10, typeof(InterredGrizzle))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(Ghoul))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NecroRoosterBoss))
			)
		),
		new MiniChampInfo // Nightshade Bramble Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Forager)),
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorQueen)),
				new MiniChampTypeInfo(10, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NightshadeBrambleBoss))
			)
		),
		new MiniChampInfo // Nymph's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(NymphSinger)),
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(10, typeof(SatyrPiper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DarkElf)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NymphBoss))
			)
		),
		new MiniChampInfo // Nyx Rith Ruins
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(NyxRithBoss))
			)
		),
		new MiniChampInfo // Persian Shade Tomb
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GothicNovelist)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalMage)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PersianShadeBoss))
			)
		),
		new MiniChampInfo // Phantom Vines Overgrowth
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SwampThing)),
				new MiniChampTypeInfo(15, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(BlackSolenQueen))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Treefellow)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mimic)),
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PhantomVinesBoss))
			)
		),
		new MiniChampInfo // Poisonous Crab Cove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Scorpion)),
				new MiniChampTypeInfo(15, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(10, typeof(GiantSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonElemental)),
				new MiniChampTypeInfo(10, typeof(AcidSlug))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(SkitteringHopper))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PoisonousCrabBoss))
			)
		),
		new MiniChampInfo // Poison Pullet Farm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SerpentHandler)),
				new MiniChampTypeInfo(15, typeof(BirdTrainer)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(PoisonElemental)),
				new MiniChampTypeInfo(5, typeof(Wisp))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PoisonPulletBoss))
			)
		),
		new MiniChampInfo // Puck's Mischief
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(FairyQueen)),
				new MiniChampTypeInfo(15, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(TwistedCultist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(NymphSinger)),
				new MiniChampTypeInfo(5, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PuckBoss))
			)
		),
		new MiniChampInfo // Puffy Ferret Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SerpentHandler)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Horse)),
				new MiniChampTypeInfo(10, typeof(GrizzlyBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RidableLlama)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PuffyFerretBoss))
			)
		),
		new MiniChampInfo // Purse Spider Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(10, typeof(SentinelSpider)),
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantSpider)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(5, typeof(Shade))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PurseSpiderBoss))
			)
		),
		new MiniChampInfo // Pyroclastic Golem Forge
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaElemental)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(10, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(LavaLizard))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireElemental)),
				new MiniChampTypeInfo(5, typeof(BronzeElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(PyroclasticGolemBoss))
			)
		),
		new MiniChampInfo // Quake Bringer Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Cyclops)),
				new MiniChampTypeInfo(15, typeof(GiantTurkey)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ogre)),
				new MiniChampTypeInfo(5, typeof(Troll))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(StoneElemental)),
				new MiniChampTypeInfo(5, typeof(Golem))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(QuakeBringerBoss))
			)
		),
		new MiniChampInfo // Quor Zael's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(DemonKnight)),
				new MiniChampTypeInfo(15, typeof(Imp)),
				new MiniChampTypeInfo(10, typeof(AbysmalHorror))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Devourer)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(QuorZaelBoss))
			)
		),
		new MiniChampInfo // Ragdoll Guardian Citadel
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(15, typeof(Mimic)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(5, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GhostWarrior)),
				new MiniChampTypeInfo(5, typeof(RestlessSoul))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RagdollGuardianBoss))
			)
		),
		new MiniChampInfo // Raging Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Alligator)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GiantSerpent)),
				new MiniChampTypeInfo(5, typeof(WildTiger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(WolfSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RagingAlligatorBoss))
			)
		),
		new MiniChampInfo // RathZor the Shattered's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Golem)),
				new MiniChampTypeInfo(10, typeof(Mimic)),
				new MiniChampTypeInfo(10, typeof(PatchworkSkeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Ravager)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RathZorTheShatteredBoss))
			)
		),
		new MiniChampInfo // Riptide Crab Cove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SeaSerpent)),
				new MiniChampTypeInfo(15, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(10, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Kraken)),
				new MiniChampTypeInfo(10, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RiptideCrabBoss))
			)
		),
		new MiniChampInfo // Rock Bear Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(15, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(BrownBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TimberWolf)),
				new MiniChampTypeInfo(5, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(RockBearBoss))
			)
		),
		new MiniChampInfo // Sahiwal Shaman's Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(HostileDruid)),
				new MiniChampTypeInfo(15, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(ArcticNaturalist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DesertNaturalist)),
				new MiniChampTypeInfo(10, typeof(ForestRanger))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ForestRanger)),
				new MiniChampTypeInfo(5, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SahiwalShamanBoss))
			)
		),
		new MiniChampInfo // Sandstorm Elemental Desert
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Shade)),
				new MiniChampTypeInfo(15, typeof(ForestRanger)),
				new MiniChampTypeInfo(10, typeof(Scorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SpiritMedium)),
				new MiniChampTypeInfo(10, typeof(DesertNaturalist))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SandstormElemental)),
				new MiniChampTypeInfo(5, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SandstormElementalBoss))
			)
		),
		new MiniChampInfo // Scorpion Spider Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(SpeckledScorpion))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScorpionSpiderBoss))
			)
		),
		new MiniChampInfo // Scottish Fold Sentinel Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShieldBearer)),
				new MiniChampTypeInfo(10, typeof(CombatMedic)),
				new MiniChampTypeInfo(10, typeof(SwordDefender))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShieldMaiden)),
				new MiniChampTypeInfo(5, typeof(HolyKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(RamRider)),
				new MiniChampTypeInfo(5, typeof(CombatNurse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ScottishFoldSentinelBoss))
			)
		),
		new MiniChampInfo // Selkie Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SerpentHandler)),
				new MiniChampTypeInfo(10, typeof(AquaticTamer)),
				new MiniChampTypeInfo(10, typeof(SeaHorse))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaHorse)),
				new MiniChampTypeInfo(5, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DeepSeaSerpent)),
				new MiniChampTypeInfo(5, typeof(Dolphin))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SelkieBoss))
			)
		),
		new MiniChampInfo // Shadow Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Alligator)),
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(Boar))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(5, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior)),
				new MiniChampTypeInfo(5, typeof(Wraith))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowAlligatorBoss))
			)
		),
		new MiniChampInfo // Shadow Anaconda Jungle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantSerpent)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Lizardman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(5, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SilverSerpent)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowAnacondaBoss))
			)
		),
		new MiniChampInfo // Shadow Bear's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackBear)),
				new MiniChampTypeInfo(15, typeof(Wolf)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowBearBoss))
			)
		),
		new MiniChampInfo // Shadow Chick's Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Chicken)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(BlackSolenWorker))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowChickBoss))
			)
		),
		new MiniChampInfo // Shadow Crab's Tidepool
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(15, typeof(IceCrab)),
				new MiniChampTypeInfo(10, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(Kraken))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowCrabBoss))
			)
		),
		new MiniChampInfo // Shadow Dragon's Roost
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(15, typeof(Wyvern)),
				new MiniChampTypeInfo(20, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalDragon)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowDragon)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowDragonBoss))
			)
		),
		new MiniChampInfo // Shadow Drifter's Mists
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GhostScout)),
				new MiniChampTypeInfo(15, typeof(Spectre)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ShadowDrifterBoss))
			)
		),
		new MiniChampInfo // Siamese Illusionist Chamber
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Enchanter)),
				new MiniChampTypeInfo(15, typeof(ArcaneScribe)),
				new MiniChampTypeInfo(10, typeof(Magician))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ScrollMage)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(EvilAlchemist)),
				new MiniChampTypeInfo(5, typeof(SlimeMage))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SiameseIllusionistBoss))
			)
		),
		new MiniChampInfo // Siberian Frostclaw's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WhiteWolf)),
				new MiniChampTypeInfo(15, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SnowElemental)),
				new MiniChampTypeInfo(10, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(FrostDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SiberianFrostclawBoss))
			)
		),
		new MiniChampInfo // Sidhe Fae Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FairyQueen)),
				new MiniChampTypeInfo(15, typeof(SatyrPiper)),
				new MiniChampTypeInfo(10, typeof(NymphSinger))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(GreenHag)),
				new MiniChampTypeInfo(10, typeof(DarkElf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MegaDragon)),
				new MiniChampTypeInfo(5, typeof(Dragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SidheBoss))
			)
		),
		new MiniChampInfo // Sinister Root Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(AppleElemental)),
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Treefellow)),
				new MiniChampTypeInfo(10, typeof(BogThing))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SinisterRootBoss))
			)
		),
		new MiniChampInfo // Sky Seraph's Aerie
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Eagle)),
				new MiniChampTypeInfo(15, typeof(Parrot)),
				new MiniChampTypeInfo(10, typeof(Macaw))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Crane)),
				new MiniChampTypeInfo(10, typeof(Phoenix))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(StarCitizen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SkySeraphBoss))
			)
		),
		new MiniChampInfo // Solar Elemental Summit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FireElemental)),
				new MiniChampTypeInfo(15, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(ValoriteElemental))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(BronzeElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SolarElementalBoss))
			)
		),
		new MiniChampInfo // Spark Ferret Wilds
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(25, typeof(SparkFerret)),
				new MiniChampTypeInfo(15, typeof(Rabbit)),
				new MiniChampTypeInfo(10, typeof(Goat))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(Hind))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WildTiger)),
				new MiniChampTypeInfo(5, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SparkFerretBoss))
			)
		),
		new MiniChampInfo // Sphinx Cat's Riddle
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(SphinxCat)),
				new MiniChampTypeInfo(20, typeof(Cat)),
				new MiniChampTypeInfo(10, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(BoneKnight))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletalKnight)),
				new MiniChampTypeInfo(5, typeof(BoneMagi))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SphinxCatBoss))
			)
		),
		new MiniChampInfo // Spiderling Overlord Broodmother
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackSolenWorker)),
				new MiniChampTypeInfo(15, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(5, typeof(GiantWolfSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SpiderlingOverlordBroodmother))
			)
		),
		new MiniChampInfo // Starry Ferret's Celestial Realm
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(25, typeof(StarryFerret)),
				new MiniChampTypeInfo(10, typeof(Squirrel)),
				new MiniChampTypeInfo(10, typeof(Hind))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Hind)),
				new MiniChampTypeInfo(10, typeof(Bird))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Crane)),
				new MiniChampTypeInfo(5, typeof(Phoenix))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StarryFerretBoss))
			)
		),
		new MiniChampInfo // Steel Bear Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(BrownBear))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wolf)),
				new MiniChampTypeInfo(5, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SteelBearBoss))
			)
		),
		new MiniChampInfo // Stone Guardian Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BoneKnight)),
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SkeletalLich)),
				new MiniChampTypeInfo(10, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SkeletonLord)),
				new MiniChampTypeInfo(5, typeof(SkeletalDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StoneGuardianBoss))
			)
		),
		new MiniChampInfo // Stone Rooster Crypt
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Skeleton)),
				new MiniChampTypeInfo(10, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Shade)),
				new MiniChampTypeInfo(10, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wraith)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StoneRoosterBoss))
			)
		),
		new MiniChampInfo // Storm Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Alligator)),
				new MiniChampTypeInfo(15, typeof(Snake)),
				new MiniChampTypeInfo(10, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(Snake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaSnake)),
				new MiniChampTypeInfo(5, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormAlligatorBoss))
			)
		),
		new MiniChampInfo // Skeleton Ettin Stronghold
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(15, typeof(Orc)),
				new MiniChampTypeInfo(10, typeof(OrcBrute))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormCrabBoss))
			)
		),
		new MiniChampInfo // Storm Crab's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(15, typeof(SentinelSpider)),
				new MiniChampTypeInfo(10, typeof(SkitteringHopper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(IceCrab)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(IceCrab)),
				new MiniChampTypeInfo(5, typeof(BlackSolenWarrior))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormCrabBoss))
			)
		),
		new MiniChampInfo // Storm Daemon's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(15, typeof(Impaler)),
				new MiniChampTypeInfo(10, typeof(ArcaneDaemon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(10, typeof(Daemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WandererOfTheVoid)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormDaemonBoss))
			)
		),
		new MiniChampInfo // Storm Dragon's Peak
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Dragon)),
				new MiniChampTypeInfo(15, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(PlatinumDrake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Dragon)),
				new MiniChampTypeInfo(10, typeof(GreaterDragon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormDragonBoss))
			)
		),
		new MiniChampInfo // Storm Herald's Sanctuary
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(10, typeof(WindElemental)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(AvatarOfElements)),
				new MiniChampTypeInfo(10, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(SpiritMedium))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StormHeraldBoss))
			)
		),
		new MiniChampInfo // Strix's Perch
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(Strix))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Strix)),
				new MiniChampTypeInfo(5, typeof(GreyWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(GreyWolf)),
				new MiniChampTypeInfo(5, typeof(Ferret))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(StrixBoss))
			)
		),
		new MiniChampInfo // Sunbeam Ferret Hollow
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Ferret)),
				new MiniChampTypeInfo(15, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(Rabbit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SunbeamFerret)),
				new MiniChampTypeInfo(5, typeof(GoldenElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Squirrel)),
				new MiniChampTypeInfo(5, typeof(PolarBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(SunbeamFerretBoss))
			)
		),
		new MiniChampInfo // Tarantula Warrior Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorWarrior)),
				new MiniChampTypeInfo(5, typeof(SentinelSpider))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TarantulaWarriorBoss))
			)
		),
		new MiniChampInfo // Tarantula Worrior Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WolfSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(10, typeof(TrapdoorSpider))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(10, typeof(BlackSolenInfiltratorWarrior))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BlackSolenQueen)),
				new MiniChampTypeInfo(5, typeof(BlackSolenInfiltratorQueen))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TarantulaWorriorBoss))
			)
		),
		new MiniChampInfo // Tempest Spirit Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(TempestSpirit))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(5, typeof(TempestSpirit))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(AirElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TempestSpiritBoss))
			)
		),
		new MiniChampInfo // Tempest Wyrm Spire
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(FrostDrake)),
				new MiniChampTypeInfo(10, typeof(Wyvern)),
				new MiniChampTypeInfo(10, typeof(SerpentineDragon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(TempestWyrm)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientWyrm)),
				new MiniChampTypeInfo(5, typeof(ShadowWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TempestWyrmBoss))
			)
		),
		new MiniChampInfo // Terra Wisp Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Treefellow)),
				new MiniChampTypeInfo(5, typeof(SwampThing))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TerraWispBoss))
			)
		),
		new MiniChampInfo // Thorned Horror Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Bogling)),
				new MiniChampTypeInfo(15, typeof(BogThing)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(DreadSpider)),
				new MiniChampTypeInfo(5, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(SwampThing)),
				new MiniChampTypeInfo(5, typeof(MegaDragon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ThornedHorrorBoss))
			)
		),
		new MiniChampInfo // Thul Gor the Forsaken Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(AncientLich)),
				new MiniChampTypeInfo(10, typeof(WailingBanshee))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WailingBanshee)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(InterredGrizzle)),
				new MiniChampTypeInfo(5, typeof(AncientLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ThulGorTheForsakenBoss))
			)
		),
		new MiniChampInfo // Thunder Bear Highlands
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BlackBear)),
				new MiniChampTypeInfo(15, typeof(GrizzlyBear)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PolarBear)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(Gorilla))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ThunderBearBoss))
			)
		),
		new MiniChampInfo // Thunderbird Mountain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Eagle)),
				new MiniChampTypeInfo(15, typeof(Macaw)),
				new MiniChampTypeInfo(10, typeof(Parrot))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(FairyDragon)),
				new MiniChampTypeInfo(5, typeof(Thunderbird))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Wyvern)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ThunderbirdBoss))
			)
		),
		new MiniChampInfo // Thunder Serpent Cavern
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(LightningBearer)),
				new MiniChampTypeInfo(15, typeof(Wyvern)),
				new MiniChampTypeInfo(10, typeof(Stormtrooper2))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Wyvern)),
				new MiniChampTypeInfo(10, typeof(RuneCaster))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ThunderSerpent)),
				new MiniChampTypeInfo(5, typeof(PlatinumDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ThunderSerpentBoss))
			)
		),
		new MiniChampInfo // Tidal Ettin Marsh
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(AquaticTamer)),
				new MiniChampTypeInfo(15, typeof(SeaSerpent)),
				new MiniChampTypeInfo(10, typeof(CoralSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Leviathan)),
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TidalEttin)),
				new MiniChampTypeInfo(5, typeof(Minotaur))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TidalEttinBoss))
			)
		),
		new MiniChampInfo // Titan Boa Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Snake)),
				new MiniChampTypeInfo(10, typeof(SerpentHandler)),
				new MiniChampTypeInfo(15, typeof(GiantSerpent))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(LavaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TitanBoa)),
				new MiniChampTypeInfo(5, typeof(SeaSerpent))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TitanBoaBoss))
			)
		),
		new MiniChampInfo // Toxic Alligator Swamps
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BloodFox)),
				new MiniChampTypeInfo(10, typeof(GrayGoblin)),
				new MiniChampTypeInfo(10, typeof(GreenGoblinAlchemist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ToxicElemental)),
				new MiniChampTypeInfo(10, typeof(PoisonElemental))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicAlligator)),
				new MiniChampTypeInfo(5, typeof(BlackBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ToxicAlligatorBoss))
			)
		),
		new MiniChampInfo // Toxic Reaver Necropolis
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Zombie)),
				new MiniChampTypeInfo(10, typeof(InterredGrizzle)),
				new MiniChampTypeInfo(10, typeof(SkeletalKnight))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BoneDemon)),
				new MiniChampTypeInfo(10, typeof(Mummy))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicReaver)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ToxicReaverBoss))
			)
		),
		new MiniChampInfo // Turkish Angora Enchanter's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Enchanter)),
				new MiniChampTypeInfo(15, typeof(Magician)),
				new MiniChampTypeInfo(10, typeof(ScrollMage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(RuneCaster)),
				new MiniChampTypeInfo(10, typeof(ArcaneScribe))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ElementalWizard)),
				new MiniChampTypeInfo(5, typeof(Enchanter))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TurkishAngoraEnchanterBoss))
			)
		),
		new MiniChampInfo // Twin Terror Ettin's Fortress
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Ettin)),
				new MiniChampTypeInfo(15, typeof(OrcCaptain)),
				new MiniChampTypeInfo(10, typeof(GiantTurkey))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Ogre)),
				new MiniChampTypeInfo(10, typeof(OrcishLord))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(5, typeof(Troll))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(TwinTerrorEttinBoss))
			)
		),
		new MiniChampInfo // Uru Koth's Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(GreenGoblin)),
				new MiniChampTypeInfo(15, typeof(GrayGoblin)),
				new MiniChampTypeInfo(10, typeof(OrcChopper))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(OrcBomber))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoonElite)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(UruKothBoss))
			)
		),
		new MiniChampInfo // Vengeful Pit Viper's Pit
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(PitFiend)),
				new MiniChampTypeInfo(15, typeof(SerpentHandler)),
				new MiniChampTypeInfo(10, typeof(Snake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(PlatinumDrake)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LavaSerpent)),
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VengefulPitViperBoss))
			)
		),
		new MiniChampInfo // Venom Bear's Den
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(BloodFox)),
				new MiniChampTypeInfo(15, typeof(SheepdogHandler)),
				new MiniChampTypeInfo(10, typeof(Wolf))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(BlackBear)),
				new MiniChampTypeInfo(10, typeof(DireWolf))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomBearBoss))
			)
		),
		new MiniChampInfo // Venomous Alligator Swamp
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Alligator)),
				new MiniChampTypeInfo(10, typeof(Boar)),
				new MiniChampTypeInfo(15, typeof(GiantToad))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SerpentHandler)),
				new MiniChampTypeInfo(5, typeof(Snake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ToxicElemental)),
				new MiniChampTypeInfo(5, typeof(Leviathan))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomousAlligatorBoss))
			)
		),
		new MiniChampInfo // Venomous Dragon Lair
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(Snake)),
				new MiniChampTypeInfo(15, typeof(LavaSnake)),
				new MiniChampTypeInfo(5, typeof(IceSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWyrm)),
				new MiniChampTypeInfo(5, typeof(Drake))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FrostDrake)),
				new MiniChampTypeInfo(5, typeof(CrimsonDrake))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomousDragonBoss))
			)
		),
		new MiniChampInfo // Venomous Ettin Cave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GiantTurkey)),
				new MiniChampTypeInfo(10, typeof(Troll)),
				new MiniChampTypeInfo(10, typeof(Ogre))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(Ettin)),
				new MiniChampTypeInfo(10, typeof(Ogre))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(10, typeof(OgreLord)),
				new MiniChampTypeInfo(5, typeof(BoneDemon))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomousEttinBoss))
			)
		),
		new MiniChampInfo // Venomous Ivy Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(AppleElemental)),
				new MiniChampTypeInfo(15, typeof(PoisonAppleTree)),
				new MiniChampTypeInfo(10, typeof(Forager))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(AppleElemental)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(HostileDruid)),
				new MiniChampTypeInfo(5, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VenomousIvyBoss))
			)
		),
		new MiniChampInfo // Vespa Hive
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(SkitteringHopper)),
				new MiniChampTypeInfo(10, typeof(MoundOfMaggots)),
				new MiniChampTypeInfo(10, typeof(PestilentBandage))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(15, typeof(TrapdoorSpider)),
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(10, typeof(GiantBlackWidow)),
				new MiniChampTypeInfo(5, typeof(AppleElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VespaBoss))
			)
		),
		new MiniChampInfo // Vile Blossom Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(AppleElemental)),
				new MiniChampTypeInfo(15, typeof(Forager)),
				new MiniChampTypeInfo(10, typeof(PoisonAppleTree))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ForestScout)),
				new MiniChampTypeInfo(10, typeof(HostileDruid))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(TwistedCultist)),
				new MiniChampTypeInfo(5, typeof(GreenHag))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VileBlossomBoss))
			)
		),
		new MiniChampInfo // Vitrail the Mosaic
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ShadowWisp)),
				new MiniChampTypeInfo(15, typeof(CrystalElemental)),
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ClockworkScorpion)),
				new MiniChampTypeInfo(10, typeof(Golem))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Mimic)),
				new MiniChampTypeInfo(5, typeof(ClockworkScorpion))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VitrailTheMosaicBoss))
			)
		),
		new MiniChampInfo // Void Stalker Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Skeleton)),
				new MiniChampTypeInfo(15, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(10, typeof(WandererOfTheVoid))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(MaddeningHorror)),
				new MiniChampTypeInfo(5, typeof(ChaosDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ArcaneDaemon)),
				new MiniChampTypeInfo(5, typeof(VoidStalker))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VoidStalkerBoss))
			)
		),
		new MiniChampInfo // Volcanic Titan Crater
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(LavaSerpent)),
				new MiniChampTypeInfo(15, typeof(HellCat)),
				new MiniChampTypeInfo(10, typeof(LavaSnake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(LavaElemental)),
				new MiniChampTypeInfo(10, typeof(FireDaemon))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(FireDaemon)),
				new MiniChampTypeInfo(5, typeof(AncientWyrm))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VolcanicTitanBoss))
			)
		),
		new MiniChampInfo // Vorgath the Destroyer
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(ChaosDragoon)),
				new MiniChampTypeInfo(15, typeof(JukaLord)),
				new MiniChampTypeInfo(10, typeof(JukaWarrior))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(SavageRider)),
				new MiniChampTypeInfo(10, typeof(OrcBomber))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(BoneDemon)),
				new MiniChampTypeInfo(5, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VorgathBoss))
			)
		),
		new MiniChampInfo // Vortex Crab Reef
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WaterElemental)),
				new MiniChampTypeInfo(15, typeof(Leviathan)),
				new MiniChampTypeInfo(10, typeof(SavageShaman))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WaterElemental)),
				new MiniChampTypeInfo(10, typeof(DeepSeaSerpent))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Leviathan)),
				new MiniChampTypeInfo(5, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VortexCrabBoss))
			)
		),
		new MiniChampInfo // Vortex Guardian Keep
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(WindElemental)),
				new MiniChampTypeInfo(15, typeof(AirElemental)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(StormConjurer)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WindElemental)),
				new MiniChampTypeInfo(5, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(VortexGuardianBoss))
			)
		),
		new MiniChampInfo // Whirlwind Fiend Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WindElemental)),
				new MiniChampTypeInfo(10, typeof(AirElemental)),
				new MiniChampTypeInfo(10, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WhirlwindFiend)),
				new MiniChampTypeInfo(10, typeof(StormConjurer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WhirlwindFiend)),
				new MiniChampTypeInfo(5, typeof(WaterElemental))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WhirlwindFiendBoss))
			)
		),
		new MiniChampInfo // Will-O-The-Wisp Enclave
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Wisp)),
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(LightningBearer))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(LightningBearer)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WillOTheWispBoss))
			)
		),
		new MiniChampInfo // Wind Bear Grove
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(WhiteWolf)),
				new MiniChampTypeInfo(10, typeof(GreyWolf)),
				new MiniChampTypeInfo(10, typeof(FrostDrake))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(WindElemental)),
				new MiniChampTypeInfo(10, typeof(WindBear))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(WindElemental)),
				new MiniChampTypeInfo(5, typeof(FrostBear))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WindBearBoss))
			)
		),
		new MiniChampInfo // Wind Chicken Nest
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Chicken)),
				new MiniChampTypeInfo(15, typeof(Bird)),
				new MiniChampTypeInfo(10, typeof(Crane))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(5, typeof(Phoenix))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Stormtrooper2)),
				new MiniChampTypeInfo(5, typeof(Skeleton))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(WindChickenBoss))
			)
		),
		new MiniChampInfo // Xal'Rath Cult
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(TwistedCultist)),
				new MiniChampTypeInfo(10, typeof(Eagle)),
				new MiniChampTypeInfo(10, typeof(ChaosDragoon))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(ChaosDaemon)),
				new MiniChampTypeInfo(5, typeof(AbyssalAbomination))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(3, typeof(AbysmalHorror)),
				new MiniChampTypeInfo(3, typeof(DemonKnight))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(XalRathBoss))
			)
		),
		new MiniChampInfo // Zebu Zealot Ruins
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(Orc)),
				new MiniChampTypeInfo(20, typeof(OrcChopper)),
				new MiniChampTypeInfo(10, typeof(OrcBrute))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(5, typeof(OrcishLord)),
				new MiniChampTypeInfo(5, typeof(OrcCaptain))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ChaosDragoonElite)),
				new MiniChampTypeInfo(5, typeof(Horse))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ZebuZealotBoss))
			)
		),
		new MiniChampInfo // Zel'Vrak Stronghold
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(10, typeof(DecoyDeployer)),
				new MiniChampTypeInfo(10, typeof(SneakyNinja)),
				new MiniChampTypeInfo(15, typeof(MasterPickpocket))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(ShadowWisp)),
				new MiniChampTypeInfo(10, typeof(Infiltrator))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(ShadowIronElemental)),
				new MiniChampTypeInfo(5, typeof(Spy))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ZelVrakBoss))
			)
		),
		new MiniChampInfo // Zephyr Warden's Domain
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(20, typeof(Spectre)),
				new MiniChampTypeInfo(15, typeof(Wraith)),
				new MiniChampTypeInfo(10, typeof(Shade))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Skeleton)),
				new MiniChampTypeInfo(5, typeof(SkeletalMage))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(AncientLich)),
				new MiniChampTypeInfo(5, typeof(SkeletalLich))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ZephyrWardenBoss))
			)
		),
		new MiniChampInfo // Zor'Thul Abyss
		(
			typeof(MaxxiaScroll),
			new MiniChampLevelInfo // Level 1
			(
				new MiniChampTypeInfo(15, typeof(GreenGoblin)),
				new MiniChampTypeInfo(20, typeof(GrayGoblin)),
				new MiniChampTypeInfo(10, typeof(GreenGoblinAlchemist))
			),
			new MiniChampLevelInfo // Level 2
			(
				new MiniChampTypeInfo(10, typeof(Imp)),
				new MiniChampTypeInfo(10, typeof(PitFiend))
			),
			new MiniChampLevelInfo // Level 3
			(
				new MiniChampTypeInfo(5, typeof(Daemon)),
				new MiniChampTypeInfo(5, typeof(AbysmalHorror))
			),
			new MiniChampLevelInfo // Renowned
			(
				new MiniChampTypeInfo(1, typeof(ZorThulBoss))
			)
		),
        };

        public static MiniChampInfo GetInfo(MiniChampType type)
        {
            int v = (int)type;

            if (v < 0 || v >= m_Table.Length)
                v = 0;

            return m_Table[v];
        }
    }
}