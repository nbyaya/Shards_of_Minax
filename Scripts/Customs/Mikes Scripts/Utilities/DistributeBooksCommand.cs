using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Commands
{
    public class DistributeBooksCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("DistributeBooks", AccessLevel.GameMaster, new CommandEventHandler(DistributeBooks_OnCommand));
        }

        [Usage("DistributeBooks")]
        [Description("Gives a cursor to select a container to distribute 1-3 random books.")]
        public static void DistributeBooks_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new DistributeBooksTarget();
        }

        private class DistributeBooksTarget : Target
        {
            private static readonly Type[] m_BookTypes = new Type[]
            {
                typeof(ABCsForBarbarians), 
                typeof(AlchemistsCompendium),
                typeof(AnatomyOfCorporealUndead),
				typeof(AnatomyOfKrakens), 
                typeof(AnatomyOfReapers),
                typeof(AnatomyOfSlimes),
				typeof(AnatomyOfSpectralUndead), 
                typeof(AncestorsAndTheOrcs),
                typeof(AnOrcishCookbook),
				typeof(AntecedantsOfElvenLaw), 
                typeof(ConfessionsOfMinax),
                typeof(ElvenCarpentry),
				typeof(EttinPoetry), 
                typeof(HistoryAirElementals),
                typeof(HistoryFireElementals),
				typeof(HistoryOfDragons), 
                typeof(HistoryOfEarthElementals),
                typeof(HistoryOfEttins),
				typeof(HistoryOfImps), 
                typeof(HistoryOfLiches),
                typeof(HistoryOfLizardmen),
				typeof(HistoryOfMongbats), 
                typeof(HistoryOfNecromancy),
                typeof(HistoryOfTerathans),
				typeof(HistoryOfTheCyclopes), 
                typeof(HistoryOfTheDaemons),
                typeof(HistoryOfTheGargoyles),
				typeof(HistoryOfTheGazers), 
                typeof(HistoryOfTheHarpies),
                typeof(HistoryOfTheHeadless),
				typeof(HistoryOfTheOphidians), 
                typeof(HistoryOfTheOrcs),
                typeof(HistoryOfTheRatmen),
				typeof(HistoryOfTheTitans), 
                typeof(HistoryOfTrolls),
                typeof(HistoryOfWaterElementals),
				typeof(MagicalPropertiesOfBlackPearl), 
                typeof(MagicalPropertiesOfBloodMoss),
                typeof(MagicalPropertiesOfGarlic),
				typeof(MagicalPropertiesOfMandrakeRoot), 
                typeof(MagicalPropertiesOfNightshade),
                typeof(MagicalPropertiesOfSpiderSilk),
				typeof(MagicalPropertiesOfSulfurousAsh), 
                typeof(MalidrexHistory),
                typeof(OgreBaking),
				typeof(OgreLordsAndHierarchy), 
                typeof(OgrePoetryBook),
                typeof(OgreTailoringBook),
				typeof(OnJestersBook), 
                typeof(OnTheOriginOfGiantFrogs),
                typeof(OnTheVirtueOfCompassion),
				typeof(OnTheVirtueOfHonesty), 
                typeof(OnTheVirtueOfHonor),
                typeof(OnTheVirtueOfHumility),
				typeof(OnTheVirtueOfJustice), 
                typeof(OnTheVirtueOfSacrifice),
                typeof(OnTheVirtueOfSpirituality),
				typeof(OnTheVirtueOfValor), 
                typeof(OrcishAlchemy),
                typeof(OrcishBaking),
				typeof(OrcishBlacksmithingBook), 
                typeof(OrcishCarpentryBook),
                typeof(OrcishFishingTechniques),
				typeof(OrcishHistory), 
                typeof(OrcishTailoringBook),
                typeof(OrcishTinkeringBook),
				typeof(RedBookOfRiddles), 
                typeof(RulesOfAcquisition),
                typeof(SpeculationsOnGiantAnimals),
				typeof(SpeculationsOnIceAndFrost), 
                typeof(SpeculationsOnOriginOfLordBritish),
                typeof(SpeculationsOnTheOriginOfMinax),
				typeof(SpeculationsOnWisps),
				typeof(TheAffairsOfWizards), 
                typeof(TheAgeOfShadows),
                typeof(TheAlchemistsFormulary),
				typeof(TheArtOfWarMagic),
				typeof(TheJestersCode), 
                typeof(TheMagicalPropertiesOfGinseng),
                typeof(TheNeedForOrderInBritannia),
				typeof(TheRedAndTheGreyDragons),
				typeof(TheShardRealms), 
				typeof( MisadventuresOfABumblingMage ),
				typeof( OrcishStandUpComedy ),
				typeof( EnigmaOfTheWhisperingWoods ),
				typeof( TalesOfTheMysteriousMoonstones ),
				typeof( DiaryOfARogueGargoyle ),
				typeof( HilariousHarpyHijinks ),
				typeof( LizardmensLostLegacy ),
				typeof( EttinElegiesPoetryAnthology ),
				typeof( SecretLivesOfImps ),
				typeof( ACyclopsPerspective ),
				typeof( GazerGossip ),
				typeof( RatmenRiddles ),
				typeof( TheTitansForgottenLegacy ),
				typeof( TrollTalesFromTheBridge ),
				typeof( WhispersOfWaterElementals ),
				typeof( BardsGuideToBizarreCreatures ),
				typeof( ChroniclesOfMalidrex ),
				typeof( OgreCuisineBeyondTheStewpot ),
				typeof( OgreHaikus ),
				typeof( OgreCoutureTailoringTips ),
				typeof( JestersJestBook ),
				typeof( AnAmphibianAnecdote ),
				typeof( CompassionTalesOfEmpathy ),
				typeof( TheCodeOfHonesty ),
				typeof( StoriesOfRighteousness ),
				typeof( HumilitysTriumph ),
				typeof( VirtueLegendaryDeeds ),
				typeof( OrcishAlchemy2 ),
				typeof( BakingWithABarbaricTwist ),
				typeof( RiddleMastersBlueBook ),
				typeof( GoblinsGuideToGreed ),
				typeof( WizardsGuideToCulinaryMagic ),
				typeof( BalladsOfTheBefuddledBard ),
				typeof( EnchantmentsOfTheElvenShoemaker ),
				typeof( GnomishGadgetsAndHowToTinkerThem ),
				typeof( ChroniclesOfTheCrimsonSorcerer ),
				typeof( MisadventuresOfAHalfTrollDiplomat ),
				typeof( AHerpetomancersTale ),
				typeof( CompendiumOfCuriousConstellations ),
				typeof( DiariesOfADwarvenDentist ),
				typeof( ThePhantomPipersOfPaws ),
				typeof( TheSirensCookbook ),
				typeof( UnheardTalesOfSosaria ),
				typeof( MisdeedsOfMischievousMimics ),
				typeof( OdesToTheObsidianOgres ),
				typeof( GuideToNonHumanHorticulture ),
				typeof( LibrariansSecretSpells ),
				typeof( CompendiumOfMythicalBeasts ),
				typeof( WizardsGuideToWandering ),
				typeof( MoansOfMournfulGhosts ),
				typeof( FablesOfTheFey ),
				typeof( TheClumsyNecromancer ),
				typeof( BlacksmithsGuideToBuildingInterstellarSpaceships ),
				typeof( TheSpectralCourt ),
				typeof( LycanthropicLegends ),
				typeof( AdventurersAccoutrements ),
				typeof( TheDrunkenDragon ),
				typeof( ElementalistsEthos ),
				typeof( HowToHaggleWithAHag ),
				typeof( EchoesOfTheEtherealPlane ),
				typeof( MinotaurMazeMasters ),
				typeof( TheMaidensQuest ),
				typeof( PixiePotions ),
				typeof( QuixoticQuestsOfQuibbleQuasit ),
				typeof( RunesAndRuins ),
				typeof( SilentSongsOfTheStoneGargoyles ),
				typeof( VivaciousVenturesOfVexingValkyries ),
				typeof( WordsOfWispyWisdom ),
				typeof( XenodochialXorns ),
				typeof( YarnsOfTheYeti ),
				typeof( ZenAndTheArtOfZombieMaintenance ),
				typeof( KnightsGuideToDragonDiplomacy ),
				typeof( FineDiningInTheUnderworld ),
				typeof( ChildsGuideToBeginnerWitchcraft ),
				typeof( ScrollsOfTheSemiSaneSorcerer ),
				typeof( DarkTalesOfHiddenCults ),
				typeof( TheThriftyThaumaturge ),
				typeof( UnlikelyUnityOfElvesAndEnts ),
				typeof( CompendiumOfCharmedCreatures ),
				typeof( GoblinGastronomyGoneWild ),
				typeof( ALeprechaunsLedger ),
				typeof( VagrantVampires ),
				typeof( MimicMusingsAChestsPerspective ),
				typeof( TheHungoverHydra ),
				typeof( BakingWithBasilisks ),
				typeof( TrollsGuideToBridgeEtiquette ),
				typeof( GnomesGuideToGiantNegotiations ),
				typeof( PixiePranks ),
				typeof( ConfessionsOfADisenchantedDryad ),
				typeof( BardsWorstBallads ),
				typeof( VampiresGuideToBloodTypeDiets ),
				typeof( MermaidMannersForLandwalkers ),
				typeof( WerewolfsGuideToFur ),
				typeof( WhenGolemsGrumble ),
				typeof( SirensSingingLessons ),
				typeof( CodexOfCelestialAlignments ),
				typeof( AnEldersAnthology ),
				typeof( LegendsOfTheLostLyricum ),
				typeof( ProphetsOfThePast ),
				typeof( OdesToTheFallen ),
				typeof( ChroniclesOfTheCrystalCaverns ),
				typeof( BalladsOfTheBattleborne ),
				typeof( TreatisesOnTranscendentalMagics ),
				typeof( GuardiansOfTheGrimoire ),
				typeof( RiseAndFallOfShadowlords ),
				typeof( EnigmasOfTheEldritchElves ),
				typeof( TheWraithWars ),
				typeof( TheSovereignScrolls ),
				typeof( EchoesOfEtherealEmpires ),
				typeof( DeitiesOfBritannia ),
				typeof( WhispersFromTheVoid ),
				typeof( SecretsOfTheSeers ),
				typeof( RiddlesOfTheRunestone ),
				typeof( TheEclipsedMoon ),
				typeof( PhantomsPhylactery ),
				typeof( ScrollsOfTheSubliminal ),
				typeof( PathwaysToPerdition ),
				typeof( ShadowsBehindTheStars ),
				typeof( CodicilsOfTheCrypticConjurers ),
				typeof( AlmanacOfAethericArtifacts ),
				typeof( SecretsOfTheLunarCult ),
				typeof( FragmentsOfFuturity ),
				typeof( TheForgottenFortress ),
				typeof( EnigmaOfElementalEquilibrium ),
                typeof(TheVirtueOfChaos)
                // ... Add more book types as necessary
            };

            public DistributeBooksTarget() : base(10, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Container)
                {
                    Container container = (Container)targeted;

                    int count = Utility.Random(1, 3); // Random number between 1 and 3

                    for (int i = 0; i < count; i++)
                    {
                        Type bookType = m_BookTypes[Utility.Random(m_BookTypes.Length)];
                        Item book = (Item)Activator.CreateInstance(bookType);
                        container.DropItem(book);
                    }

                    from.SendMessage("Added {0} random book(s) to the container.", count);
                }
                else
                {
                    from.SendMessage("That's not a container!");
                }
            }
        }
    }
}
