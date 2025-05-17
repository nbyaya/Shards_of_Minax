using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Custom
{
    public class ZanaTheDimensionalCartographer : BaseDynamicVendor
    {
        [Constructable]
        public ZanaTheDimensionalCartographer()
            : base("The Dimensional Cartographer")
        {

            Name = "Zana";
			Body = 401; // Human female body
            Female = true;
            Hue = 1052; // Fair skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 997; // Bright red

            // Basic stats
            SetStr(75);
            SetDex(75);
            SetInt(150);

            SetHits(100);
            SetMana(200);
            SetStam(75);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 10;

            // Keep her from wandering away
            Frozen = true;
            CantWalk = true;



            // Dress or equip Zana as you see fit
            AddItem(new FancyShirt() { Hue = 0, Name = "Zana's Ruffled Shirt" }); // Dark grey
            AddItem(new FancyKilt() { Hue = 802, Name = "Cartographer's Skirt" }); // Black or dark hue
            AddItem(new ThighBoots() { Hue = 1109, Name = "Explorer's Boots" }); // Black boots
            AddItem(new BodySash() { Hue = 802, Name = "Ribbon of the Atlas" }); // Red sash
            AddItem(new ElvenGlasses() { Hue = 0, Name = "Eyes of Realms" }); // Optional, dark cloak
            AddItem(new MageWand() { Hue = 1171, Name = "Atlas Wand" }); // Golden hue staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Map Satchel";
            AddItem(backpack);
        }

        public override void InitDynamicStock()
        {
            // --- Potential Items to SELL ---
            // Syntax: (Type, BasePrice, Chance, MinStock, MaxStock)
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AlchemistsWax),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArcaneHourglass),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlacksmithingCatalyst),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CartographersPen),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CartographersPin),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChaosGlyph),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CompassRose),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DiamondLootScroll),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EasternBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ErasureScroll),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ExaltedOrb),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FarEasternBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FeluccanBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FeluccaPortalPrism),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FirestormGlyph),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FletchingCatalyst),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GoldenSeal),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GlyphOfBounty),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IlshenarBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InkOfRegression),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MalasBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MonsterMixMedallion),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NorthernBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OrbOfAnnulment),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PlanarCompass),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RadiusRune),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScrollOfIntensification),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SingularityRune),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SosariaBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SouthernBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StabilizerRune),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SurveyorsCompass),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TailoringCatalyst),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TemporalSundial),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TerMurBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TimeTurnToken),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TinkeringCatalyst),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TokunoBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrammelBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WesternBrand),      25000, 0.05, 1, 3));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbbadonTheAbyssalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbyssalWardenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbyssinianTrackerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AcidicAlligatorMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AlchemicalLabMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AncientAlligatorMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AncientDragonsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AngusBerserkersCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BloodstainedFieldsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CorruptedOrchardMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DogTheBountyHuntersDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DuelistPoetArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EarthClanNinjaLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EarthClanSamuraiEncampmentMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EvilAlchemistLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EvilClownCarnivalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FairyQueenGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FastExplorerExpeditionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FireClanNinjaHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FireClanSamuraiDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlapperElementalistAltarMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FloridaMansCarnivalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForestRangerOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FunkFungiFamiliarGardenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GangLeadersHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GlamRockMageConcertMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GothicNovelistCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GraffitiGargoyleAlleyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GreaserGryphonRidersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GreenHagsSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GreenNinjasHiddenLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HippieHoplitesGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HolyKnightsCathedralMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HostileDruidsGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HostilePrincessCourtMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IceKingsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernoDragonsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InsaneRoboticistWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JazzAgeBrawlMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JestersCourtMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LawyersTribunalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LineDragonsAscentMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LordBlackthornsDominionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LordBritishsSummoningCircleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MagmaElementalRiftMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MedievalMeteorologistsObservatoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MegaDragonsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MinaxSorceressSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MischievousWitchCovenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MotownMermaidLagoonMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MushroomWitchGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MusketeerHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MysticGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NeovictorianVampireCourtMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NinjaLibrarianSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NoirDetectiveHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OgreMastersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PhoenixStyleMastersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PigFarmersPenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PinupPandemoniumParlorMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PirateOfTheStarsOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PiratesCoveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PkMurderersHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RaKingsPyramidMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RanchMastersPrairieMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RapRangersJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RaveRoguesUndergroundMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RebelCathedralMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RedQueensCourtMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ReggaeRunesmithWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RenaissanceMechanicFactoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RetroAndroidWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RetroFuturistDomeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RetroRobotRomancersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RingmastersCircusMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RockabillyRevenantsStageMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SanctuaryOfTheHolyKnightMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScorpomancersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SilentMovieStudioMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SilverSlimeCavernsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SithAcademyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SkaSkaldConcertHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SkeletonLordCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SlimeMageSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SneakyNinjaClanMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarCitizenOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarfleetCaptainsCommandMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarfleetCommandCenterMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SteampunkSamuraiForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormtrooperAcademyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SurferSummonerCoveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SwampThingLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SwinginSorceressBallroomMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TexanRancherPrairieMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TwistedCultistHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VaudevilleValkyrieStageMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WastelandBikerCompoundMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WaterClanNinjaHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WaterClanSamuraiFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WildWestWizardCanyonMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FleshEaterOgreTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlyingSquirrelHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForgottenWardenCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FoxSquirrelGlenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrenziedSatyrsForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostbiteWolfsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostboundChampionsHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostDroidFactoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostLichsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostOgreLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostWapitiGroundsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostWardenWatchMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrozenOozeCaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FungalToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GeminiHarpysLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GeminiTwinBearsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GentleSatyrsGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GiantForestHogsNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GiantWolfSpidersWebMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GoliathBirdeatersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GoralsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrapplerDronesArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GraveKnightsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GummySheepsPastureMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernoStallionArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfinitePouncersDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IroncladDefendersFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IroncladOgresDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IronGolemsWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IronSteedStablesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JavelinaJinxHuntMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JellybeanJestersCarnivalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KasTheBloodyhandedCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KelthuzadsFrozenCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KhufuTheGreatBuildersTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheGibbonMysticsGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheGlisteningOozesCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheGloomOgresFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheGloomWolfsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheGoldenOrbWeaversWebMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheHatshepsutTheQueensTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheHogWildsSwinePenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheHowlerMonkeysJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheHuntsmanSpidersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheHydrokineticWardensWaterShrineMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheIbexHighlandMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheIndianPalmSquirrelGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheInfernalLichCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheInfernalToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheInfernoSentinelFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheLibraBalanceBearMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheLibraHarpyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheLicoriceSheepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheLollipopLordMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheLuchadorLlamaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMalariaratDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMandrillshamanJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMarkhorPeaksMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMeatgolemLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMelodicsatyrGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMountainGorillaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMuckGolemMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMysticFallowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheMysticSatyrMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheNagashMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheNefertitisTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LarlochTheShadowKingsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LeoTheHarpysLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LeoTheSunBearsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LeprosyRatNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MentuhotepTheWiseTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MetallicWindsteedPeaksMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MimicronsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MireSpawnerMarshMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MoltenSlimePitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NanoSwarmLabMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NebulaCatsCelestialRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecroticGeneralsBattlefieldMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecroticLichsTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecroticOgresDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NemesisUnitFacilityMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NightmareLeaperAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NightmarePanthersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OmegaSentinelsFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OrangutanSageGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OverlordMkiiStrongholdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PeccaryProtectorForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PeppermintPuffDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PhantomAutomatonVaultMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PhantomPanthersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PlagueLichsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PlasmaJuggernautsForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PurseSpidersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(QuantumGuardiansRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RabidRatLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RadiantSlimeCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RaistlinMajeresTowerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RamsesTheImmortalTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RedSquirrelNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RedtailedSquirrelGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BreezePhantomAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BubbleFerretForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialDragonShrineMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CerebralEttinCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChanequeGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChimereonSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CinderWraithRuinsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CorruptingCreeperForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrystalDragonCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrystalWardenTempleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CursedHarbingerCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CycloneDemonPlainsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DairyWraithFieldMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DeadlordFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DreadedCreeperHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DreamyFerretHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DrolaticWastesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DryadGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EarthquakeEttinDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ElderTendrilSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EmberSerpentLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EmberSpiritDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EtherealCrabNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EtherealDragonsKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FirebreathAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FireRoosterCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlameBearerCaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlameWardenEttinFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlareImpNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FossilElementalCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostBearDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostbiteAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostboundBehemothCaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostDrakonKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostHensPerchMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostSerpentLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostWardenEttinStrongholdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FrostyFerretsBurrowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GaleWispsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GiantTrapdoorSpiderLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GiantWolfSpiderNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GlimmeringFerretBurrowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GoldenOrbWeaverCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GoliathBirdeaterJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GorgonVipersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GraniteColossusCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrimoriesTomeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrotesqueOfRouensCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrymalkinTheWatchersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GuernseyGuardianKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HarmonyFerretGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HellfireJuggernautForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HerefordWarlockTowerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HighlandBullHerdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HuntsmanSpidersLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IceCrabCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IllusionarySwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IllusionHensParadiseMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IllusionistEttinsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernalDukesCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernalIncineratorForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernoDrakonsRoostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernoPythonPitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfernoWardensFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IshKarTheForgottenLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JerseyEnchantressCovenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheShadowogresDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheShadowprowlersHuntMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheShadowsludgesSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheShadowtoadsBogMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSifakawarriorsJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSmallpoxRatLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSombreroDeSolLlamaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSombreroLlamaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSothTheDeathKnightMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheSoulEaterLichMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheTahrsWildHordeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheTalonmachinesForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheTaurusearthbearsDominionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheTaurusharpysSkiesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheTempestsatyrsStormMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVietnamesePigMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVileToadMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVirgoHarpyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVirgoPurityBearMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVoidCatMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVoidSlimeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVolcaniccHargerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVortexConstructMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheVortexWraithMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheWarthogWarriorMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheWraithlichCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheYangstallionPlainsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheYinsteedForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LavaCrabCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LavaFiendFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LeafBearGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LeprechaunsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LightBearersSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MagmaGolemForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MagneticCrabCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MaineCoonTitansRoostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MilkingDemonStablesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RhythmicSatyrsGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RockSquirrelCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SableAntelopeSavannaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SagittariusArcherBearForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SagittariusHarpysPerchMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SandGolemsTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScorpioHarpysLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScorpionSpidersHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScorpioVenomBearsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SetiTheAvengersCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowbladeAssassinsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowGolemsAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowLichsNecropolisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowMuntjacsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpectralAutomatonForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpectralToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpectralWardenCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpiderlingMinionBroodmotherMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpiderMonkeyJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarbornPredatorNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SteamLeviathanAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StoneGolemCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StoneSteedStablesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormBoneFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SynthroidPrimeFactoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SzassTamsNecropolisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TacoLlamaFestivalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TacticalEnforcerOperationsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TaffyTitansArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TequilaLlamaTavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TheForestTempestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TheStormOfDeathMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TheTempestsFuryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TheTempestsWrathMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TheWrathOfTheThunderKingMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThutmoseTheConquerorsTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TidalMaresDeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicLichsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicOgresStrongholdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicSludgeSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrapdoorSpiderNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TsunamiTitansDeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TutankhamunTheCursedTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TyphusRatInfestationMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VampiricBladesLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VecnasSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousRoesMarshMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousToadsSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousWolfsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WhisperingPookaGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WickedSatyrsForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WoodGolemsHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WoodlandChargerDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WoodlandSpiritHorseMeadowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MoltenGolemCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MordrakesManorMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MudGolemSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MysticFerretSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MysticFowlSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MysticWispRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NatureDragonsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecroEttinCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecroRoosterTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NightshadeBrambleGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NymphsSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NyxRithRuinsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PersianShadeTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PhantomVinesOvergrowthMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PoisonousCrabCoveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PoisonPulletFarmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PucksMischiefMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PuffyFerretHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PurseSpiderNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PyroclasticGolemForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(QuakeBringerCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(QuorZaelsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RagdollGuardianCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RagingAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RathzorTheShatteredsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RiptideCrabCoveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RockBearCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SahiwalShamansGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SandstormElementalDesertMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScorpionSpiderPitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScottishFoldSentinelDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SelkieCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowAnacondaJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowBearsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowChicksNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowCrabsTidepoolMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowDragonsRoostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowDriftersMistsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SiameseIllusionistChamberMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SiberianFrostclawsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SidheFaeRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SinisterRootHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SkeletonEttinStrongholdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SkySeraphsAerieMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SolarElementalSummitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SparkFerretWildsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SphinxCatsRiddleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpiderlingOverlordBroodmotherMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarryFerretsCelestialRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SteelBearDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StoneGuardianFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StoneRoosterCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormCrabsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormDaemonsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormDragonsPeakMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormHeraldsSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StrixsPerchMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SunbeamFerretHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TarantulaWarriorLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TarantulaWorriorCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TempestSpiritDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TempestWyrmSpireMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TerraWispGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThornedHorrorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThulGorTheForsakenLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThunderBearHighlandsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThunderbirdMountainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThunderSerpentCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TidalEttinMarshMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TitanBoaSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicAlligatorSwampsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicReaverNecropolisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TurkishAngoraEnchantersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TwinTerrorEttinsFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(UruKothsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VengefulPitVipersPitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomBearsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousAlligatorSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousDragonLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousEttinCaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousIvyGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VespaHiveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VileBlossomGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VitrailTheMosaicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VoidStalkerAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VolcanicTitanCraterMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VorgathTheDestroyerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VortexCrabReefMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VortexGuardianKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WhirlwindFiendAbyssMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WillothewispEnclaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WindBearGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WindChickenNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(XalrathCultMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ZebuZealotRuinsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ZelvrakStrongholdMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ZephyrWardensDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EasternMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FarEasternMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FeluccaMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IlshenarMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NorthernMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SouthernMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TerMurMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TokunoMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrammelMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WesternMagicMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AirClanNinjaCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AirClanSamuraiDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AlienWarriorNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AppleGroveElementalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AssassinGuildHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AstralTravelerRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AvatarOfElementsShrineMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BaroqueBarbarianCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BeetleJuiceSummoningCircleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BiomancersGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BluesSingingGorgonAmphitheaterMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BMovieBeastmasterArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BountyHunterOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CabaretKrakenStageMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CannibalTribeCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CavemanScientistExperimentSiteMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialSamuraiDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChrisRobertsGalacticArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CorporateExecutiveTowerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CountryCowgirlCyclopsRanchMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CyberpunkNexusMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DarkElfCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DinoRiderExpeditionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DiscoDruidFestivalMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HarvestFestivalFrenzyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LongbowSniperOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LuchadorTrainingGroundsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MagiciansArcaneHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MartialMonkDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MasterFlutistsConcertMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MechanicsWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MusclePitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NecromancersHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NetCasterReefMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NinjaShadowHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(NymphSingerGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(OraclesSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PastryChefsBakeryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PatchworkMonsterLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PathologistsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PhantomAssassinsHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PickpocketsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(PocketPickersRefugeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ProtestersCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(QiGongHealerSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(QuantumPhysicistResearchFacilityMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RamRiderOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RapierDuelistArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RelativistObservatoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RelicHunterExpeditionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RuneCasterSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(RuneKeeperChamberMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SaboteurHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SabreFighterArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SafecrackersDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SamuraiMastersDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SatyrPipersGlenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SawmillWorkersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScoutArchersRefugeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScoutLeaderEncampmentMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ScrollMagesTowerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SerpentHandlerPitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowLordsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowLurkerCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShadowPriestLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SheepdogHandlersPenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShieldBearersBastionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ShieldMaidensCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SlyStorytellersTheatreMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SousChefsKitchenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpearFishersCoveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpearSentryKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpellbreakersTrialMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpiritMediumsSeanceMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SpyHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StarReaderObservatoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StormConjurerSummitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(StrategistsWarTableMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SumoWrestlerArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(SwordDefenderCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TaekwondoDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TerrainScoutEncampmentMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ThievesGuildHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ToxicologistsKitchenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrapEngineerWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrapMakersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrapMastersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrapSettersHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TreeFellersGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(TrickShotArtistsArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(UrbanTrackersOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(VenomousAssassinsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ViolinistsOrchestraMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WardCastersKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WaterAlchemistsLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WeaponEnchantersSanctumMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WildWestOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(WoolWeaversLoomMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ZenMonksSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AnvilHurlerForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AquaticTamerLagoonMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArcaneScribeEnclaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArcticNaturalistDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArmorCurerLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArrowFletchersRoostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AsceticHermitsRefugeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AstrologersObservatoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BanneretsBastionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BattleDressmakersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BattlefieldHealersSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BattleHerbalistGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BattleStormCallersEyeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BattleWeaverLoomMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BeastmastersDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BigCatTamerJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BiologistsLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BirdTrainersAviaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BoneShielderCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BoomerangThrowerCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CabinetMakersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CarversAtelier),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChemistsLaboratory),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChoirSingersHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ClockworkEngineersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ClueSeekersPuzzleGroundsMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CombatMedicsSanctuaryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CombatNursesRecoveryWardMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ConArtistsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ContortionistsCircusMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrimeSceneTechInvestigationMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrossbowMarksmanOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CryingOrphanRefugeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CryptologistsChamberMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DarkSorcererDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DeathCultistCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DecoyDeployerOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DeepMinerExcavationMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DemolitionExpertQuarryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DesertNaturalistOasisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DesertTrackersOasisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DiplomatsParleyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DisguiseMastersHavenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DivinersPeakMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DrumBoysSpectacleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DrummersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DualWielderDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EarthAlchemistsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ElectriciansWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ElementalWizardsKeepMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EnchantersLabyrinthMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EpeeSpecialistArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EscapeArtistHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EvidenceAnalystsBureauMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EvilMapMakersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ExplorersExpeditionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ExplosiveDemolitionistsFoundryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FeastMastersBanquetMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FencingMastersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FieldCommanderOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FieldMedicCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FireAlchemistLaboratoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FireMageConclaveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FirestarterPyreMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlameWelderForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForagersHollowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForensicAnalystsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForestMinstrelsGlenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForestScoutOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ForestTrackerCampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GemCutterCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GhostScoutOutpostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GhostWarriorBattlefieldMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GourmetChefKitchenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GraveDiggerCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrecoRomanArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(GrillMasterPitMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HammerGuardArmoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HarpistsGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(HerbalistPoisonerGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IceSorcererCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IllusionistsLabyrinthMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InfiltratorsHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(InvisibleSaboteursWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(IronSmithForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JavelinAthleteArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JoinerWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(JungleNaturalistGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KarateExpertDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KatanaDuelistDojoMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KnifeThrowersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KnightOfJusticeCitadelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KnightOfMercyChapelMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KnightOfValorFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(KunoichiHideoutMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LibrarianCustodiansArchiveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LightningBearersStormNexusMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LocksmithsWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LogiciansPuzzleHallMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(MeraktusTheTormentedMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbyssalBouncersArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbyssalPanthersProwlMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AbyssalTidesSurgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AcereraksNecropolisMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AcidicSlimesLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AegisConstructForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AkhenatensHereticShrineMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AkhenatensTombMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AlbertsSquirrelGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AlphaBaboonTroopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AncientWolfDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AnthraxRatNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArbiterDroneHiveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArcaneSatyrGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ArcaneSentinelBastionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AriesHarpyAerieMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AriesRamBearPlateauMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AzalinRexsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AzureMiragesRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(AzureMooseGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BabirusaBeastsBogMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BansheeCrabsNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BansheesWailMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BeardedGoatPasturesMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BeldingsGroundSquirrelBurrowMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BengalStormsJungleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BisonBrutePlateauMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlackDeathRatSewersMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlackWidowQueenLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlackWidowsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlightDemonFissureMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BlightedToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BloodDragonRoostMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BloodLichsCryptMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BloodSerpentsNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BloodthirstyVinesThicketMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BonecrusherOgresDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BoneGolemsWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BorneoPigstyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BubblegumBlasterFactoryMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(BushPigEncampmentMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CactusLlamaGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CancerHarpyAerieMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CancerShellBearDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CandyCornCreepsLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CapricornHarpysNestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CapricornMountainBearsDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CapuchinTrickstersPlaygroundMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CaramelConjurersWorkshopMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialHorrorRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialPythonDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialSatyrGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CelestialWolfDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChamoisHillMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CholeraRatInfestationMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(ChromaticOgreClanMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CleopatraTheEnigmaticMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CliffGoatDominionMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CoralSentinelsReefMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CorrosiveToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CosmicBouncerArenaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CosmicStalkerVoidMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrimsonMuleValleyMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrystalGolemForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CrystalOozeCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CursedToadSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CursedWhiteTailForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(CursedWolfsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DallSheepHighlandMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DeathRatCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DiaDeLosMuertosLlamaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DisplacerBeastDomainMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DomesticSwineRetreatMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DouglasSquirrelForestMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(DreadnaughtFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EarthquakeWolfCavernMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EasternGraySquirrelGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EclipseReindeerGladeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EldritchHarbingerRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(EtherealPanthrasLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FaintingGoatsPastureMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FeverRatsDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FiestaLlamasCelebrationMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlameborneKnightsFortressMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(FlamebringerOgreLairMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheChaosHareMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheCharroLlamaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheCheeseGolemMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheChimpanzeeBerserkerMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheChocolateTruffleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEldritchHaresWarrenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEldritchToadsSwampMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheElectricSlimesLabyrinthMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheElectroWraithsRealmMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheElMariachiLlamasFiestaMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEmberAxisForgeMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEmberWolfDenMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEmperorCobraTempleMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEnigmaticSatyrGroveMap),      6000, 0.001, 1, 1));
            PotentialSellStock.Add(new DynamicItemEntry(typeof(LairOfTheEnigmaticSkipperReefMap),      6000, 0.001, 1, 1));			

            // --- Potential Items to BUY ---
            // Syntax: (Type, BasePurchasePrice, ChanceToBuy, MinDemand, MaxDemand)
            PotentialBuyStock.Add(new DynamicItemEntry(typeof(Bottle),         3, 0.70, 40, 80)); // Wants 40-80 bottles

        }

        public ZanaTheDimensionalCartographer(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            // Show the main dialogue gump
            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // First module text
            DialogueModule greeting = new DialogueModule(
                "Greetings, traveler. I am Zana, once an exile from Wraeclast, now determined to save all worlds from the threats that spill through interdimensional rifts. " +
                "These lands are besieged by eldritch powers, and I fear even Britannia is not safe. How can I assist you?"
            );

            // Option 1: Ask about her past / exile
            greeting.AddOption(
                "Tell me more about your exile.",
                player => true, 
                player =>
                {
                    DialogueModule exileModule = new DialogueModule(
                        "I was cast out of my homeland during the crisis brought on by The Elder’s corruption. " +
                        "I devoted my life to mapping alternate realms to contain and combat these horrors. But fate brought me here, " +
                        "where Minax’s invasions tear rifts between dimensions. I refuse to sit idle while another realm falls."
                    );
                    exileModule.AddOption("That sounds dire. How can we stop it?",
                        p => true,
                        p =>
                        {
                            DialogueModule direModule = new DialogueModule(
                                "Dimensional threats must be met head-on. I'm researching anomalies, but it’s slow going. " +
                                "If you'd brave these portals, I'd gladly supply you with maps to monster-infested locales. " +
                                "Battling these threats and collecting artifacts might turn the tide."
                            );
                            direModule.AddOption("Tell me more about these maps.",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule mapModule = new DialogueModule(
                                        "Each Magic Map is attuned to a specific region—Felucca, Trammel, Ilshenar, Tokuno, even Ter Mur. " +
                                        "They’ll transport you to places overrun with monsters, but also laden with treasure. If you survive, " +
                                        "you’ll be stronger for the battles ahead."
                                    );
                                    mapModule.AddOption("I'm interested. Show me the maps.",
                                        pla => true,
                                        pla =>
                                        {
                                            // Show the vendor gump for maps
                                            pla.CloseGump(typeof(ZanaMapVendorGump));
                                            pla.SendGump(new ZanaMapVendorGump(pla));
                                        }
                                    );
                                    mapModule.AddOption("I need more time to prepare.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        }
                                    );
                                    pl.SendGump(new DialogueGump(pl, mapModule));
                                }
                            );
                            direModule.AddOption("I’ll fight in my own way. Farewell.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                }
                            );
                            p.SendGump(new DialogueGump(p, direModule));
                        }
                    );

                    exileModule.AddOption("I see. Let’s talk about something else.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        }
                    );

                    player.SendGump(new DialogueGump(player, exileModule));
                }
            );

            // Option 2: Buy maps directly
            greeting.AddOption(
                "Show me your Magic Maps.",
                player => true,
                player =>
                {
                    // Open the custom map vendor gump
                    player.CloseGump(typeof(ZanaMapVendorGump));
                    player.SendGump(new ZanaMapVendorGump(player));
                }
            );

            // Option 3: Goodbye
            greeting.AddOption(
                "Farewell, Zana. I have other matters to attend to.",
                player => true,
                player =>
                {
                    player.SendMessage("You take your leave from Zana.");
                }
            );

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
