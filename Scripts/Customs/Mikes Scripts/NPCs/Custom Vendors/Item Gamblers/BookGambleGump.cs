using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.SpecialVendor
{
    public class BookGambleGump : Gump
    {
        private Mobile m_From;
        private List<Item> itemsList = new List<Item>
        {
            // Your items list...
			new ABCsForBarbarians(),
			new ACyclopsPerspective(),
			new AdventurersAccoutrements(),
			new AHerpetomancersTale(),
			new AlchemistsCompendium(),
			new ALeprechaunsLedger(),
			new AlmanacOfAethericArtifacts(),
			new AnAmphibianAnecdote(),
			new AnatomyOfCorporealUndead(),
			new AnatomyOfKrakens(),
			new AnatomyOfReapers(),
			new AnatomyOfSlimes(),
			new AnatomyOfSpectralUndead(),
			new AncestorsAndTheOrcs(),
			new AnEldersAnthology(),
			new AnOrcishCookbook(),
			new AntecedantsOfElvenLaw(),
			new BakingWithABarbaricTwist(),
			new BakingWithBasilisks(),
			new BalladsOfTheBattleborne(),
			new BalladsOfTheBefuddledBard(),
			new BardsGuideToBizarreCreatures(),
			new BardsWorstBallads(),
			new BlacksmithsGuideToBuildingInterstellarSpaceships(),
			new ChildsGuideToBeginnerWitchcraft(),
			new ChroniclesOfMalidrex(),
			new ChroniclesOfTheCrimsonSorcerer(),
			new ChroniclesOfTheCrystalCaverns(),
			new CodexOfCelestialAlignments(),
			new CodicilsOfTheCrypticConjurers(),
			new CompassionTalesOfEmpathy(),
			new CompendiumOfCharmedCreatures(),
			new CompendiumOfCuriousConstellations(),
			new CompendiumOfMythicalBeasts(),
			new ConfessionsOfADisenchantedDryad(),
			new ConfessionsOfMinax(),
			new DarkTalesOfHiddenCults(),
			new DeitiesOfBritannia(),
			new DiariesOfADwarvenDentist(),
			new DiaryOfARogueGargoyle(),
			new EchoesOfEtherealEmpires(),
			new EchoesOfTheEtherealPlane(),
			new ElementalistsEthos(),
			new ElvenCarpentry(),
			new EnchantmentsOfTheElvenShoemaker(),
			new EnigmaOfElementalEquilibrium(),
			new EnigmaOfTheWhisperingWoods(),
			new EnigmasOfTheEldritchElves(),
			new EttinElegiesPoetryAnthology(),
			new EttinPoetry(),
			new FablesOfTheFey(),
			new FineDiningInTheUnderworld(),
			new FragmentsOfFuturity(),
			new GazerGossip(),
			new GnomesGuideToGiantNegotiations(),
			new GnomishGadgetsAndHowToTinkerThem(),
			new GoblinEtiquette(),
			new GoblinGastronomyGoneWild(),
			new GoblinsGuideToGreed(),
			new GuardiansOfTheGrimoire(),
			new GuideToNonHumanHorticulture(),
			new HilariousHarpyHijinks(),
			new HistoryAirElementals(),
			new HistoryFireElementals(),
			new HistoryOfDragons(),
			new HistoryOfEarthElementals(),
			new HistoryOfEttins(),
			new HistoryOfImps(),
			new HistoryOfLiches(),
			new HistoryOfLizardmen(),
			new HistoryOfMongbats(),
			new HistoryOfNecromancy(),
			new HistoryOfTerathans(),
			new HistoryOfTheCyclopes(),
			new HistoryOfTheDaemons(),
			new HistoryOfTheGargoyles(),
			new HistoryOfTheGazers(),
			new HistoryOfTheHarpies(),
			new HistoryOfTheHeadless(),
			new HistoryOfTheOphidians(),
			new HistoryOfTheOrcs(),
			new HistoryOfTheRatmen(),
			new HistoryOfTheTitans(),
			new HistoryOfTrolls(),
			new HistoryOfWaterElementals(),
			new HowToHaggleWithAHag(),
			new HumilitysTriumph(),
			new JestersJestBook(),
			new KnightsGuideToDragonDiplomacy(),
			new LegendsOfTheLostLyricum(),
			new LibrariansSecretSpells(),
			new LizardmensLostLegacy(),
			new LycanthropicLegends(),
			new MagicalPropertiesOfBlackPearl(),
			new MagicalPropertiesOfBloodMoss(),
			new MagicalPropertiesOfGarlic(),
			new MagicalPropertiesOfMandrakeRoot(),
			new MagicalPropertiesOfNightshade(),
			new MagicalPropertiesOfSpiderSilk(),
			new MagicalPropertiesOfSulfurousAsh(),
			new MalidrexHistory(),
			new MermaidMannersForLandwalkers(),
			new MimicMusingsAChestsPerspective(),
			new MinotaurMazeMasters(),
			new MisadventuresOfABumblingMage(),
			new MisadventuresOfAHalfTrollDiplomat(),
			new MisdeedsOfMischievousMimics(),
			new MoansOfMournfulGhosts(),
			new OdesToTheFallen(),
			new OdesToTheObsidianOgres(),
			new OgreBaking(),
			new OgreCoutureTailoringTips(),
			new OgreCuisineBeyondTheStewpot(),
			new OgreHaikus(),
			new OgreLordsAndHierarchy(),
			new OgrePoetryBook(),
			new OgreTailoringBook(),
			new OnJestersBook(),
			new OnTheOriginOfGiantFrogs(),
			new OnTheVirtueOfCompassion(),
			new OnTheVirtueOfHonesty(),
			new OnTheVirtueOfHonor(),
			new OnTheVirtueOfHumility(),
			new OnTheVirtueOfJustice(),
			new OnTheVirtueOfSacrifice(),
			new OnTheVirtueOfSpirituality(),
			new OnTheVirtueOfValor(),
			new OrcishAlchemy(),
			new OrcishAlchemy2(),
			new OrcishBaking(),
			new OrcishBlacksmithingBook(),
			new OrcishCarpentryBook(),
			new OrcishFishingTechniques(),
			new OrcishHistory(),
			new OrcishStandUpComedy(),
			new OrcishTailoringBook(),
			new OrcishTinkeringBook(),
			new PathwaysToPerdition(),
			new PhantomsPhylactery(),
			new PixiePotions(),
			new PixiePranks(),
			new ProphetsOfThePast(),
			new QuixoticQuestsOfQuibbleQuasit(),
			new RatmenRiddles(),
			new RedBookOfRiddles(),
			new RiddleMastersBlueBook(),
			new RiddlesOfTheRunestone(),
			new RiseAndFallOfShadowlords(),
			new RulesOfAcquisition(),
			new RunesAndRuins(),
			new ScrollsOfTheSemiSaneSorcerer(),
			new ScrollsOfTheSubliminal(),
			new SecretLivesOfImps(),
			new SecretsOfTheLunarCult(),
			new SecretsOfTheSeers(),
			new ShadowsBehindTheStars(),
			new SilentSongsOfTheStoneGargoyles(),
			new SirensSingingLessons(),
			new SpeculationsOnGiantAnimals(),
			new SpeculationsOnIceAndFrost(),
			new SpeculationsOnOriginOfLordBritish(),
			new SpeculationsOnTheOriginOfMinax(),
			new SpeculationsOnWisps(),
			new StoriesOfRighteousness(),
			new TalesOfTheMysteriousMoonstones(),
			new TheAffairsOfWizards(),
			new TheAgeOfShadows(),
			new TheAlchemistsFormulary(),
			new TheArtOfWarMagic(),
			new TheClumsyNecromancer(),
			new TheCodeOfHonesty(),
			new TheDrunkenDragon(),
			new TheEclipsedMoon(),
			new TheForgottenFortress(),
			new TheHungoverHydra(),
			new TheJestersCode(),
			new TheMagicalPropertiesOfGinseng(),
			new TheMaidensQuest(),
			new TheNeedForOrderInBritannia(),
			new ThePhantomPipersOfPaws(),
			new TheRedAndTheGreyDragons(),
			new TheShardRealms(),
			new TheSirensCookbook(),
			new TheSovereignScrolls(),
			new TheSpectralCourt(),
			new TheThriftyThaumaturge(),
			new TheTitansForgottenLegacy(),
			new TheVirtueOfChaos(),
			new TheWraithWars(),
			new TreatisesOnTranscendentalMagics(),
			new TrollsGuideToBridgeEtiquette(),
			new TrollTalesFromTheBridge(),
			new UnheardTalesOfSosaria(),
			new UnlikelyUnityOfElvesAndEnts(),
			new VagrantVampires(),
			new VampiresGuideToBloodTypeDiets(),
			new VirtueLegendaryDeeds(),
			new VivaciousVenturesOfVexingValkyries(),
			new WerewolfsGuideToFur(),
			new WhenGolemsGrumble(),
			new WhispersFromTheVoid(),
			new WhispersOfWaterElementals(),
			new WizardsGuideToCulinaryMagic(),
			new WizardsGuideToWandering(),
			new WordsOfWispyWisdom(),
			new XenodochialXorns(),
			new YarnsOfTheYeti(),
			new ZenAndTheArtOfZombieMaintenance(),
			
        };

        private List<Item> currentRandomItems = new List<Item>();
        private int[] currentRandomPrices = new int[9];

        public BookGambleGump(Mobile from) : base(0, 0)
        {
            m_From = from;

            RandomizeItemsAndPrices();

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            // Adjust the background size to accommodate increased spacing between items

            AddImageTiled(10, 10, 400, 200, 2624);
            AddImageTiled(10, 40, 400, 200, 2624);
            AddImageTiled(10, 50, 400, 200, 2624);
            AddImageTiled(10, 60, 400, 200, 2624);
            AddImageTiled(10, 70, 400, 200, 2624);
            AddImageTiled(10, 80, 400, 200, 2624);
            AddImageTiled(10, 100, 400, 200, 2624);
            AddImageTiled(10, 130, 400, 200, 2624);
            AddImageTiled(10, 150, 400, 200, 2624);
            AddImageTiled(10, 250, 400, 200, 2624);
            AddImageTiled(10, 450, 400, 200, 2624);

            AddImageTiled(40, 10, 400, 200, 2624);
            AddImageTiled(40, 40, 400, 200, 2624);
            AddImageTiled(40, 50, 400, 200, 2624);
            AddImageTiled(40, 60, 400, 200, 2624);
            AddImageTiled(40, 70, 400, 200, 2624);
            AddImageTiled(40, 80, 400, 200, 2624);
            AddImageTiled(40, 100, 400, 200, 2624);
            AddImageTiled(40, 130, 400, 200, 2624);
            AddImageTiled(40, 150, 400, 200, 2624);
            AddImageTiled(40, 250, 400, 200, 2624);
            AddImageTiled(40, 450, 400, 200, 2624);

            AddImageTiled(80, 10, 400, 200, 2624);
            AddImageTiled(80, 40, 400, 200, 2624);
            AddImageTiled(80, 50, 400, 200, 2624);
            AddImageTiled(80, 60, 400, 200, 2624);
            AddImageTiled(80, 70, 400, 200, 2624);
            AddImageTiled(80, 80, 400, 200, 2624);
            AddImageTiled(80, 100, 400, 200, 2624);
            AddImageTiled(80, 130, 400, 200, 2624);
            AddImageTiled(80, 150, 400, 200, 2624);
            AddImageTiled(80, 250, 400, 200, 2624);
            AddImageTiled(80, 450, 400, 200, 2624);

            AddImageTiled(200, 10, 400, 200, 2624);
            AddImageTiled(200, 40, 400, 200, 2624);
            AddImageTiled(200, 50, 400, 200, 2624);
            AddImageTiled(200, 60, 400, 200, 2624);
            AddImageTiled(200, 70, 400, 200, 2624);
            AddImageTiled(200, 80, 400, 200, 2624);
            AddImageTiled(200, 100, 400, 200, 2624);
            AddImageTiled(200, 130, 400, 200, 2624);
            AddImageTiled(200, 150, 400, 200, 2624);
            AddImageTiled(200, 250, 400, 200, 2624);
            AddImageTiled(200, 450, 400, 200, 2624);

            AddImageTiled(250, 10, 400, 200, 2624);
            AddImageTiled(250, 40, 400, 200, 2624);
            AddImageTiled(250, 50, 400, 200, 2624);
            AddImageTiled(250, 60, 400, 200, 2624);
            AddImageTiled(250, 70, 400, 200, 2624);
            AddImageTiled(250, 80, 400, 200, 2624);
            AddImageTiled(250, 100, 400, 200, 2624);
            AddImageTiled(250, 130, 400, 200, 2624);
            AddImageTiled(250, 150, 400, 200, 2624);
            AddImageTiled(250, 250, 400, 200, 2624);
            AddImageTiled(250, 450, 400, 200, 2624);

            AddImageTiled(290, 10, 400, 200, 2624);
            AddImageTiled(290, 40, 400, 200, 2624);
            AddImageTiled(290, 50, 400, 200, 2624);
            AddImageTiled(290, 60, 400, 200, 2624);
            AddImageTiled(290, 70, 400, 200, 2624);
            AddImageTiled(290, 80, 400, 200, 2624);
            AddImageTiled(290, 100, 400, 200, 2624);
            AddImageTiled(290, 130, 400, 200, 2624);
            AddImageTiled(290, 150, 400, 200, 2624);
            AddImageTiled(290, 250, 400, 200, 2624);
            AddImageTiled(290, 450, 400, 200, 2624);

            AddLabel(75, 25, 1152, "Special Vendor Offers");

            // Adjusted loop for increased spacing
            for (int i = 0; i < currentRandomItems.Count; i++)
            {
                // Adjust the positions for better spacing
                int x = 75 + (i % 3) * 200; // Increased spacing on X-axis
                int y = 75 + (i / 3) * 150; // Increased spacing on Y-axis

                AddLabel(x, y, 1153, currentRandomItems[i].Name);
                AddLabel(x, y + 30, 1153, "Price: " + currentRandomPrices[i].ToString() + "gp");
                // Adjust the button and item positions based on new spacing
                AddButton(x + 130, y + 20, 4023, 4023, i + 1, GumpButtonType.Reply, 0); // Buy button for each item
                AddItem(x + 60, y + 60, currentRandomItems[i].ItemID); // Adjust for visual clarity
            }

            // Adjust the reroll button position based on the new layout
            AddButton(550, 550, 4020, 4020, 10, GumpButtonType.Reply, 0); // Reroll button
        }

        private void RandomizeItemsAndPrices()
        {
            Random rand = new Random();
            currentRandomItems.Clear();

            while (currentRandomItems.Count < 9)
            {
                Item potentialItem = itemsList[rand.Next(itemsList.Count)];
                if (!currentRandomItems.Contains(potentialItem))
                {
                    currentRandomItems.Add(potentialItem);
                    currentRandomPrices[currentRandomItems.Count - 1] = rand.Next(500, 40000); // Random price for each item
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID >= 1 && info.ButtonID <= 9)
            {
                int itemIndex = info.ButtonID - 1;
                int price = currentRandomPrices[itemIndex];

                if (from.Backpack.ConsumeTotal(typeof(Gold), price))
                {
                    Item item = (Item)Activator.CreateInstance(currentRandomItems[itemIndex].GetType());
                    from.Backpack.DropItem(item);
                    from.SendMessage("You have bought a " + currentRandomItems[itemIndex].Name + ".");
                }
                else
                {
                    int totalGold = from.Backpack.GetAmount(typeof(Gold)) + Banker.GetBalance(from);

                    if (totalGold >= price)
                    {
                        int backpackGold = from.Backpack.GetAmount(typeof(Gold));
                        int bankGold = price - backpackGold;

                        if (backpackGold > 0)
                        {
                            from.Backpack.ConsumeTotal(typeof(Gold), backpackGold);
                        }

                        if (bankGold > 0)
                        {
                            Banker.Withdraw(from, bankGold);
                        }

                        Item item = (Item)Activator.CreateInstance(currentRandomItems[itemIndex].GetType());
                        from.Backpack.DropItem(item);
                        from.SendMessage("You have bought a " + currentRandomItems[itemIndex].Name + ".");
                    }
                    else
                    {
                        from.SendMessage("You do not have enough gold.");
                    }
                }
            }
            else if (info.ButtonID == 10) // Reroll button
            {
                int rerollCost = 5000;

                if (from.Backpack.ConsumeTotal(typeof(Gold), rerollCost))
                {
                    RandomizeItemsAndPrices();
                    from.SendGump(new BookGambleGump(from));
                    from.SendMessage("The items have been rerolled.");
                }
                else
                {
                    int totalGold = from.Backpack.GetAmount(typeof(Gold)) + Banker.GetBalance(from);

                    if (totalGold >= rerollCost)
                    {
                        int backpackGold = from.Backpack.GetAmount(typeof(Gold));
                        int bankGold = rerollCost - backpackGold;

                        if (backpackGold > 0)
                        {
                            from.Backpack.ConsumeTotal(typeof(Gold), backpackGold);
                        }

                        if (bankGold > 0)
                        {
                            Banker.Withdraw(from, bankGold);
                        }

                        RandomizeItemsAndPrices();
                        from.SendGump(new BookGambleGump(from));
                        from.SendMessage("The items have been rerolled.");
                    }
                    else
                    {
                        from.SendMessage("You do not have 5000 gold to reroll.");
                    }
                }
            }
        }
    }
}
