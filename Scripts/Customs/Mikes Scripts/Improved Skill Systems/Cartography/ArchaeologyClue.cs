using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;
using Server.Network;

namespace Server.Custom
{
    public class ArchaeologyClue : Item
    {
        private Point3D m_TargetLocation;
        private Map m_TargetFacet;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D TargetLocation { get { return m_TargetLocation; } set { m_TargetLocation = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetFacet { get { return m_TargetFacet; } set { m_TargetFacet = value; } }

        [Constructable]
        public ArchaeologyClue() : base(0x14ED)
        {
            Name = "Archaeology Clue";
            LootType = LootType.Blessed;
        }

        public ArchaeologyClue(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage(String.Format("You examine the archaeological evidence. It points to: {0} on {1}", m_TargetLocation, m_TargetFacet));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_TargetLocation);
            writer.Write(m_TargetFacet);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_TargetLocation = reader.ReadPoint3D();
            m_TargetFacet = reader.ReadMap();
        }
    }

    public class ArchaeologyMissionSystem
    {
        private static List<Type> ChestTypes = new List<Type>();

        public static void Initialize()
        {
            // Add default chest types
			ChestTypes.Add(typeof(AbbasidsTreasureChest));
			ChestTypes.Add(typeof(AbyssalPlaneChest));
			ChestTypes.Add(typeof(AlehouseChest));
			ChestTypes.Add(typeof(AlienArtifactChest));
			ChestTypes.Add(typeof(AlienArtifaxChest));
			ChestTypes.Add(typeof(AlliedForcesTreasureChest));
			ChestTypes.Add(typeof(AnarchistsCache));
			ChestTypes.Add(typeof(AncientRelicChest));
			ChestTypes.Add(typeof(AngelBlessingChest));
			ChestTypes.Add(typeof(AnglersBounty));
			ChestTypes.Add(typeof(ArcadeKingsTreasure));
			ChestTypes.Add(typeof(ArcadeMastersVault));
			ChestTypes.Add(typeof(ArcaneTreasureChest));
			ChestTypes.Add(typeof(ArcanumChest));
			ChestTypes.Add(typeof(ArcheryBonusChest));
			ChestTypes.Add(typeof(AshokasTreasureChest));
			ChestTypes.Add(typeof(AshokaTreasureChest));
			ChestTypes.Add(typeof(AssassinsCoffer));
			ChestTypes.Add(typeof(AthenianTreasureChest));
			ChestTypes.Add(typeof(BabylonianChest));
			ChestTypes.Add(typeof(BakersDelightChest));
			ChestTypes.Add(typeof(BakersDolightChest));
			ChestTypes.Add(typeof(BavarianFestChest));
			ChestTypes.Add(typeof(BismarcksTreasureChest));
			ChestTypes.Add(typeof(BolsheviksLoot));
			ChestTypes.Add(typeof(BountyHuntersCache));
			ChestTypes.Add(typeof(BoyBandBox));
			ChestTypes.Add(typeof(BrewmastersChest));
			ChestTypes.Add(typeof(BritainsRoyalTreasuryChest));
			ChestTypes.Add(typeof(BuccaneersChest));
			ChestTypes.Add(typeof(CaesarChest));
			ChestTypes.Add(typeof(CandyCarnivalCoffer));
			ChestTypes.Add(typeof(CaptainCooksTreasure));
			ChestTypes.Add(typeof(CelticLegendsChest));
			ChestTypes.Add(typeof(ChamplainTreasureChest));
			ChestTypes.Add(typeof(CheeseConnoisseursCache));
			ChestTypes.Add(typeof(ChocolatierTreasureChest));
			ChestTypes.Add(typeof(CivilRightsStrongbox));
			ChestTypes.Add(typeof(CivilWarCache));
			ChestTypes.Add(typeof(CivilWarChest));
			ChestTypes.Add(typeof(CivilWorChest));
			ChestTypes.Add(typeof(ClownsWhimsicalChest));
			ChestTypes.Add(typeof(ColonialPioneersCache));
			ChestTypes.Add(typeof(ComradesCache));
			ChestTypes.Add(typeof(ConfederationCache));
			ChestTypes.Add(typeof(ConquistadorsHoard));
			ChestTypes.Add(typeof(CovenTreasuresChest));
			ChestTypes.Add(typeof(CyberneticCache));
			ChestTypes.Add(typeof(CyrusTreasure));
			ChestTypes.Add(typeof(DesertPharaohChest));
			ChestTypes.Add(typeof(DinerDelightChest));
			ChestTypes.Add(typeof(DoctorsBag));
			ChestTypes.Add(typeof(DojoLegacyChest));
			ChestTypes.Add(typeof(DragonGuardiansHoardChest));
			ChestTypes.Add(typeof(DragonHoardChest));
			ChestTypes.Add(typeof(DragonHoChest));
			ChestTypes.Add(typeof(DragonHodChest));
			ChestTypes.Add(typeof(DragonHorChest));
			ChestTypes.Add(typeof(DriveInTreasureTrove));
			ChestTypes.Add(typeof(DroidWorkshopChest));
			ChestTypes.Add(typeof(DynastyRelicsChest));
			ChestTypes.Add(typeof(EdisonsTreasureChest));
			ChestTypes.Add(typeof(EgyptianChest));
			ChestTypes.Add(typeof(EliteFoursVault));
			ChestTypes.Add(typeof(ElvenEnchantressChest));
			ChestTypes.Add(typeof(ElvenTreasuryChest));
			ChestTypes.Add(typeof(EmeraldIsleChest));
			ChestTypes.Add(typeof(EmperorJustinianCache));
			ChestTypes.Add(typeof(EmperorLegacyChest));
			ChestTypes.Add(typeof(EnchantedForestChest));
			ChestTypes.Add(typeof(EtherealPlaneChest));
			ChestTypes.Add(typeof(EuropeanRelicsChest));
			ChestTypes.Add(typeof(FairyDustChest));
			ChestTypes.Add(typeof(FirstNationsHeritageChest));
			ChestTypes.Add(typeof(FlowerPowerChest));
			ChestTypes.Add(typeof(FocusBonusChest));
			ChestTypes.Add(typeof(ForbiddenAlchemistsCache));
			ChestTypes.Add(typeof(FrontierExplorersStash));
			ChestTypes.Add(typeof(FunkyFashionChest));
			ChestTypes.Add(typeof(FurTradersChest));
			ChestTypes.Add(typeof(GalacticExplorersTrove));
			ChestTypes.Add(typeof(GalacticRelicsChest));
			ChestTypes.Add(typeof(GamersLootbox));
			ChestTypes.Add(typeof(GardenersParadiseChest));
			ChestTypes.Add(typeof(GeishasGift));
			ChestTypes.Add(typeof(GermanUnificationChest));
			ChestTypes.Add(typeof(GoldRushBountyChest));
			ChestTypes.Add(typeof(GoldRushRelicChest));
			ChestTypes.Add(typeof(GreasersGoldmineChest));
			ChestTypes.Add(typeof(GroovyVabesChest));
			ChestTypes.Add(typeof(GroovyVibesChest));
			ChestTypes.Add(typeof(GrungeRockersCache));
			ChestTypes.Add(typeof(HipHopRapVault));
			ChestTypes.Add(typeof(HolyRomanEmpireChest));
			ChestTypes.Add(typeof(HomewardBoundChest));
			ChestTypes.Add(typeof(HussarsChest));
			ChestTypes.Add(typeof(InfernalPlaneChest));
			ChestTypes.Add(typeof(InnovatorVault));
			ChestTypes.Add(typeof(JedisReliquary));
			ChestTypes.Add(typeof(JestersGigglingChest));
			ChestTypes.Add(typeof(JestersJamboreeChest));
			ChestTypes.Add(typeof(JestersJest));
			ChestTypes.Add(typeof(JudahsTreasureChest));
			ChestTypes.Add(typeof(JukeboxJewels));
			ChestTypes.Add(typeof(JustinianTreasureChest));
			ChestTypes.Add(typeof(KagesTreasureChest));
			ChestTypes.Add(typeof(KingdomsVaultChest));
			ChestTypes.Add(typeof(KingKamehamehaTreasure));
			ChestTypes.Add(typeof(KingsBest));
			ChestTypes.Add(typeof(KoscheisUndyingChest));
			ChestTypes.Add(typeof(LawyerBriefcase));
			ChestTypes.Add(typeof(LeprechaunsLootChest));
			ChestTypes.Add(typeof(LeprechaunsTrove));
			ChestTypes.Add(typeof(LouisTreasuryChest));
			ChestTypes.Add(typeof(MacingBonusChest));
			ChestTypes.Add(typeof(MagesArcaneChest));
			ChestTypes.Add(typeof(MagesRelicChest));
			ChestTypes.Add(typeof(MaharajaTreasureChest));
			ChestTypes.Add(typeof(MarioTreasureBox));
			ChestTypes.Add(typeof(MedievalEnglandChest));
			ChestTypes.Add(typeof(MerchantChest));
			ChestTypes.Add(typeof(MerchantFortuneChest));
			ChestTypes.Add(typeof(MermaidTreasureChest));
			ChestTypes.Add(typeof(MillenniumTimeCapsule));
			ChestTypes.Add(typeof(MimeSilentChest));
			ChestTypes.Add(typeof(MirageChest));
			ChestTypes.Add(typeof(ModMadnessTrunk));
			ChestTypes.Add(typeof(MondainsDarkSecretsChest));
			ChestTypes.Add(typeof(MysticalDaoChest));
			ChestTypes.Add(typeof(MysticalEnchantersChest));
			ChestTypes.Add(typeof(MysticEnigmaChest));
			ChestTypes.Add(typeof(MysticGardenCache));
			ChestTypes.Add(typeof(MysticMoonChest));
			ChestTypes.Add(typeof(NaturesBountyChest));
			ChestTypes.Add(typeof(NavyCaptainsChest));
			ChestTypes.Add(typeof(NecroAlchemicalChest));
			ChestTypes.Add(typeof(NeonNightsChest));
			ChestTypes.Add(typeof(NeroChest));
			ChestTypes.Add(typeof(NinjaChest));
			ChestTypes.Add(typeof(NordicExplorersChest));
			ChestTypes.Add(typeof(PatriotCache));
			ChestTypes.Add(typeof(PeachRoyalCache));
			ChestTypes.Add(typeof(PharaohsReliquary));
			ChestTypes.Add(typeof(PharaohsTreasure));
			ChestTypes.Add(typeof(PixieDustChest));
			ChestTypes.Add(typeof(PokeballTreasureChest));
			ChestTypes.Add(typeof(PolishRoyalChest));
			ChestTypes.Add(typeof(PopStarsTrove));
			ChestTypes.Add(typeof(RadBoomboxTrove));
			ChestTypes.Add(typeof(Radical90sRelicsChest));
			ChestTypes.Add(typeof(RadRidersStash));
			ChestTypes.Add(typeof(RailwayWorkersChest));
			ChestTypes.Add(typeof(RebelChest));
			ChestTypes.Add(typeof(RenaissanceCollectorsChest));
			ChestTypes.Add(typeof(RetroArcadeChest));
			ChestTypes.Add(typeof(RevolutionaryChess));
			ChestTypes.Add(typeof(RevolutionaryChest));
			ChestTypes.Add(typeof(RevolutionaryRelicChest));
			ChestTypes.Add(typeof(RevolutionChest));
			ChestTypes.Add(typeof(RhineValleyChest));
			ChestTypes.Add(typeof(RiverPiratesChest));
			ChestTypes.Add(typeof(RiverRaftersChest));
			ChestTypes.Add(typeof(RockersVault));
			ChestTypes.Add(typeof(RockNBallVault));
			ChestTypes.Add(typeof(RockNRallVault));
			ChestTypes.Add(typeof(RockNRollVault));
			ChestTypes.Add(typeof(RoguesHiddenChest));
			ChestTypes.Add(typeof(RomanBritanniaChest));
			ChestTypes.Add(typeof(RomanEmperorsVault));
			ChestTypes.Add(typeof(SamuraiHonorChest));
			ChestTypes.Add(typeof(SamuraiStash));
			ChestTypes.Add(typeof(SandstormChest));
			ChestTypes.Add(typeof(ScholarEnlightenmentChest));
			ChestTypes.Add(typeof(SeaDogsChest));
			ChestTypes.Add(typeof(ShinobiSecretsChest));
			ChestTypes.Add(typeof(SilkRoadTreasuresChest));
			ChestTypes.Add(typeof(SilverScreenChest));
			ChestTypes.Add(typeof(SithsVault));
			ChestTypes.Add(typeof(SlavicBrosChest));
			ChestTypes.Add(typeof(SlavicLegendsChest));
			ChestTypes.Add(typeof(SmugglersCache));
			ChestTypes.Add(typeof(SocialMediaMavensChest));
			ChestTypes.Add(typeof(SorceressSecretsChest));
			ChestTypes.Add(typeof(SpaceRaceCache));
			ChestTypes.Add(typeof(SpartanTreasureChest));
			ChestTypes.Add(typeof(SpecialChivalryChest));
			ChestTypes.Add(typeof(SpecialWoodenChestConstantine));
			ChestTypes.Add(typeof(SpecialWoodenChestExplorerLegacy));
			ChestTypes.Add(typeof(SpecialWoodenChestFrench));
			ChestTypes.Add(typeof(SpecialWoodenChestHelios));
			ChestTypes.Add(typeof(SpecialWoodenChestIvan));
			ChestTypes.Add(typeof(SpecialWoodenChestOisin));
			ChestTypes.Add(typeof(SpecialWoodenChestTomoe));
			ChestTypes.Add(typeof(SpecialWoodenChestWashington));
			ChestTypes.Add(typeof(StarfleetsVault));
			ChestTypes.Add(typeof(SugarplumFairyChest));
			ChestTypes.Add(typeof(SwingTimeChest));
			ChestTypes.Add(typeof(SwordsmanshipBonusChest));
			ChestTypes.Add(typeof(TacticsBonusChest));
			ChestTypes.Add(typeof(TangDynastyChest));
			ChestTypes.Add(typeof(TechnicolorTalesChest));
			ChestTypes.Add(typeof(TechnophilesCache));
			ChestTypes.Add(typeof(TeutonicRelicChest));
			ChestTypes.Add(typeof(TeutonicTreasuresChest));
			ChestTypes.Add(typeof(ThiefsHideawayStash));
			ChestTypes.Add(typeof(ToxicologistsTrove));
			ChestTypes.Add(typeof(TrailblazersTrove));
			ChestTypes.Add(typeof(TravelerChest));
			ChestTypes.Add(typeof(TreasureChestOfTheQinDynasty));
			ChestTypes.Add(typeof(TreasureChestOfTheThreeKingdoms));
			ChestTypes.Add(typeof(TsarsLegacyChest));
			ChestTypes.Add(typeof(TsarsRoyalChest));
			ChestTypes.Add(typeof(TsarsTreasureChest));
			ChestTypes.Add(typeof(TudorDynastyChest));
			ChestTypes.Add(typeof(UndergroundAnarchistsCache));
			ChestTypes.Add(typeof(USSRRelicsChest));
			ChestTypes.Add(typeof(VenetianMerchantsStash));
			ChestTypes.Add(typeof(VHSAdventureCache));
			ChestTypes.Add(typeof(VictorianEraChest));
			ChestTypes.Add(typeof(VikingChest));
			ChestTypes.Add(typeof(VintnersVault));
			ChestTypes.Add(typeof(VinylVault));
			ChestTypes.Add(typeof(VirtuesGuardianChest));
			ChestTypes.Add(typeof(WarOf1812Vault));
			ChestTypes.Add(typeof(WarringStatesChest));
			ChestTypes.Add(typeof(WingedHusChest));
			ChestTypes.Add(typeof(WingedHussarsChest));
			ChestTypes.Add(typeof(WitchsBrewChest));
			ChestTypes.Add(typeof(WorkersRevolutionChest));
			ChestTypes.Add(typeof(WorldWarIIChest));
			ChestTypes.Add(typeof(WWIIValorChest));
			
        }

        public static void AddChestType(Type chestType)
        {
            if (!ChestTypes.Contains(chestType))
            {
                ChestTypes.Add(chestType);
            }
        }

        public static void StartMission(Mobile from)
        {
            Point3D location;
            Map facet;
            GetRandomLocation(out location, out facet);

            ArchaeologyClue missionScroll = new ArchaeologyClue();
            missionScroll.TargetLocation = location;
            missionScroll.TargetFacet = facet;

            if (from.AddToBackpack(missionScroll))
            {
                from.SendMessage("You've found your first archaeological evidence. Examine it to start your excavation.");

                // Create and place the special chest
                Item chest = CreateRandomChest();
                chest.MoveToWorld(location, facet);
            }
            else
            {
                missionScroll.Delete();
                from.SendMessage("You receive a mission, but have no room in your backpack for the scroll.");
            }
        }

        private static void GetRandomLocation(out Point3D location, out Map facet)
        {
            List<Point3D> possibleLocations = new List<Point3D>();
            List<Map> possibleFacets = new List<Map>();

            // Add your predefined locations here
			possibleLocations.Add(new Point3D(5135, 155, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5263, 53, 13)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5389, 89, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5417, 19, 12)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5426, 60, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5399, 110, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5422, 109, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5414, 141, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5398, 178, 7)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5444, 212, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5411, 237, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5466, 243, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5503, 221, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5547, 209, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5491, 201, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5482, 159, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5532, 156, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5595, 141, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5611, 191, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5614, 236, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5619, 88, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5597, 20, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5551, 21, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5519, 15, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5659, 16, -10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5653, 81, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5680, 104, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5702, 82, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5717, 61, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5737, 107, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5793, 115, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5837, 58, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5798, 37, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5719, 31, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5876, 38, -10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5867, 110, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5922, 28, 44)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5906, 57, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5912, 102, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5947, 27, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5984, 26, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5996, 64, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5995, 105, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6049, 50, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6059, 67, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6061, 96, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6107, 67, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6106, 91, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6102, 44, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6081, 67, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6187, 107, -8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6192, 155, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6196, 173, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6258, 48, -10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6334, 54, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6322, 126, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6362, 116, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6379, 131, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6535, 76, -10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6562, 96, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6559, 123, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6536, 159, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6511, 867, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6285, 461, -50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6586, 882, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6520, 868, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6263, 883, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6295, 873, -11)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6290, 609, -51)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6349, 603, -52)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6428, 560, -50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6519, 514, -50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6419, 392, -40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6385, 352, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6366, 403, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6249, 388, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6096, 180, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6122, 215, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6047, 222, 44)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6055, 202, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5955, 224, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5918, 221, 44)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5855, 203, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5807, 229, -4)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5771, 192, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(560, 146, -4)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5825, 147, -4)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5607, 206, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5674, 408, -1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5755, 337, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5723, 442, 18)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5832, 447, -1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5839, 361, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5801, 549, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5830, 529, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5856, 552, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5862, 594, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5791, 578, 10)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5724, 562, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5682, 530, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5658, 558, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5505, 526, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5453, 551, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5394, 554, 60)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5387, 524, 65)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5394, 627, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5462, 615, 45)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5495, 627, 25)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5459, 681, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5440, 747, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5471, 739, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5393, 699, 15)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5301, 673, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5258, 674, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5220, 723, -20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5179, 699, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5154, 728, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5138, 666, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5140, 603, -50)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5171, 567, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5176, 599, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5228, 539, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5226, 537, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5346, 587, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5290, 617, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5247, 798, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5195, 783, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5308, 833, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5298, 924, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5324, 952, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5189, 1007, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5157, 906, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5139, 846, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5399, 999, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5425, 907, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5469, 859, 45)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5521, 854, 45)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5588, 884, 30)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5219, 905, -40)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5324, 1291, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5347, 1322, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5305, 1369, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5545, 1323, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5506, 1371, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5481, 1410, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5449, 1476, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5401, 1468, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5435, 1345, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5396, 1295, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5650, 1345, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5734, 1299, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5743, 1379, 6)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5828, 1332, -3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5842, 1403, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5864, 1468, -1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5796, 1474, 22)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5957, 1368, 52)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(6008, 1327, 20)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5869, 1802, 1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5919, 1831, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5886, 1917, -8)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5917, 1935, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5861, 2018, 1)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5836, 1988, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5782, 2019, -2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5765, 1961, 4)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5687, 1974, 5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5657, 1925, 3)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5705, 1842, -5)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5678, 1823, 2)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5611, 1840, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5586, 1864, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5601, 1896, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5556, 1890, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5488, 1898, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5471, 1878, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5411, 1935, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5405, 1876, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5457, 1814, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5305, 1970, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5144, 1978, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5345, 2016, 0)); possibleFacets.Add(Map.Felucca);
			possibleLocations.Add(new Point3D(5135, 155, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5263, 53, 13)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5389, 89, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5417, 19, 12)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5426, 60, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5399, 110, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5422, 109, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5414, 141, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5398, 178, 7)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5444, 212, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5411, 237, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5466, 243, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5503, 221, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5547, 209, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5491, 201, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5482, 159, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5532, 156, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5595, 141, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5611, 191, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5614, 236, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5619, 88, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5597, 20, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5551, 21, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5519, 15, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5659, 16, -10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5653, 81, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5680, 104, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5702, 82, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5717, 61, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5737, 107, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5793, 115, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5837, 58, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5798, 37, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5719, 31, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5876, 38, -10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5867, 110, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5922, 28, 44)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5906, 57, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5912, 102, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5947, 27, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5984, 26, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5996, 64, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5995, 105, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6049, 50, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6059, 67, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6061, 96, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6107, 67, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6106, 91, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6102, 44, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6081, 67, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6187, 107, -8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6192, 155, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6196, 173, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6258, 48, -10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6334, 54, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6322, 126, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6362, 116, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6379, 131, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6535, 76, -10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6562, 96, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6559, 123, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6536, 159, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6511, 867, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6285, 461, -50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6586, 882, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6520, 868, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6263, 883, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6295, 873, -11)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6290, 609, -51)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6349, 603, -52)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6428, 560, -50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6519, 514, -50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6419, 392, -40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6385, 352, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6366, 403, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6249, 388, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6096, 180, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6122, 215, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6047, 222, 44)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6055, 202, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5955, 224, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5918, 221, 44)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5855, 203, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5807, 229, -4)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5771, 192, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(560, 146, -4)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5825, 147, -4)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5607, 206, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5674, 408, -1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5755, 337, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5723, 442, 18)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5832, 447, -1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5839, 361, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5801, 549, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5830, 529, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5856, 552, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5862, 594, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5791, 578, 10)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5724, 562, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5682, 530, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5658, 558, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5505, 526, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5453, 551, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5394, 554, 60)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5387, 524, 65)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5394, 627, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5462, 615, 45)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5495, 627, 25)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5459, 681, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5440, 747, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5471, 739, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5393, 699, 15)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5301, 673, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5258, 674, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5220, 723, -20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5179, 699, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5154, 728, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5138, 666, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5140, 603, -50)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5171, 567, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5176, 599, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5228, 539, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5226, 537, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5346, 587, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5290, 617, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5247, 798, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5195, 783, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5308, 833, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5298, 924, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5324, 952, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5189, 1007, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5157, 906, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5139, 846, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5399, 999, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5425, 907, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5469, 859, 45)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5521, 854, 45)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5588, 884, 30)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5219, 905, -40)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5324, 1291, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5347, 1322, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5305, 1369, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5545, 1323, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5506, 1371, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5481, 1410, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5449, 1476, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5401, 1468, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5435, 1345, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5396, 1295, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5650, 1345, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5734, 1299, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5743, 1379, 6)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5828, 1332, -3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5842, 1403, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5864, 1468, -1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5796, 1474, 22)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5957, 1368, 52)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(6008, 1327, 20)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5869, 1802, 1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5919, 1831, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5886, 1917, -8)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5917, 1935, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5861, 2018, 1)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5836, 1988, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5782, 2019, -2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5765, 1961, 4)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5687, 1974, 5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5657, 1925, 3)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5705, 1842, -5)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5678, 1823, 2)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5611, 1840, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5586, 1864, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5601, 1896, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5556, 1890, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5488, 1898, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5471, 1878, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5411, 1935, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5405, 1876, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5457, 1814, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5305, 1970, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5144, 1978, 0)); possibleFacets.Add(Map.Trammel);
			possibleLocations.Add(new Point3D(5345, 2016, 0)); possibleFacets.Add(Map.Trammel);
            // Add more locations as needed

            if (possibleLocations.Count > 0)
            {
                int index = Utility.Random(possibleLocations.Count);
                location = possibleLocations[index];
                facet = possibleFacets[index];
            }
            else
            {
                // If we run out of predefined locations, generate a random one
                location = new Point3D(Utility.Random(2000), Utility.Random(2000), 0);
                facet = Map.Trammel;
            }
        }

        private static Item CreateRandomChest()
        {
            if (ChestTypes.Count == 0)
                return new WoodenChest(); // Default to WoodenChest if no types are defined

            Type chestType = ChestTypes[Utility.Random(ChestTypes.Count)];
            return Activator.CreateInstance(chestType) as Item;
        }
    }

    public class ArchaeologyGump : Gump
    {
        private readonly Mobile m_From;

        public ArchaeologyGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            AddPage(0);
            AddBackground(0, 0, 200, 200, 5054);
            AddLabel(20, 20, 0, "Archaeology Excavation");
            AddButton(20, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddLabel(55, 50, 0, "Start Excavation");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                ArchaeologyMissionSystem.StartMission(m_From);
            }
        }
    }
}