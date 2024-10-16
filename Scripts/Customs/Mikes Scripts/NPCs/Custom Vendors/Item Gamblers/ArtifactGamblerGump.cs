using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.SpecialVendor
{
    public class ArtifactGamblerGump : Gump
    {
        private Mobile m_From;
        private List<Item> itemsList = new List<Item>
        {
            // Your items list...
			new AlmanacOfAethericArtifacts(),
			new RareArtifact(),
			new ArtifactDeed(),
			new AcademicBooksArtifact(),
			new ArtifactLargeVase(),
			new ArtifactVase(),
			new BackpackArtifact(),
			new BloodyWaterArtifact(),
			new BooksWestArtifact(),
			new BooksNorthArtifact(),
			new BooksFaceDownArtifact(),
			new BottleArtifact(),
			new BrazierArtifact(),
			new CocoonArtifact(),
			new DamagedBooksArtifact(),
			new EggCaseArtifact(),
			new GruesomeStandardArtifact(),
			new LampPostArtifact(),
			new LeatherTunicArtifact(),
			new RockArtifact(),
			new RuinedPaintingArtifact(),
			new SaddleArtifact(),
			new SkinnedDeerArtifact(),
			new SkinnedGoatArtifact(),
			new SkullCandleArtifact(),
			new StretchedHideArtifact(),
			new StuddedLeggingsArtifact(),
			new StuddedTunicArtifact(),
			new TarotCardsArtifact(),
			new ArtifactBookshelf(),
			new GargishBentasVaseArtifact(),
			new GargishPortraitArtifact(),
			new DyingPlantArtifact(),
			new LargeDyingPlantArtifact(),
			new GargishLuckTotemArtifact(),
			new GargishKnowledgeTotemArtifact(),
			new BookOfTruthArtifact(),
			new GargishTraditionalVaseArtifact(),
			new GargishProtectiveTotemArtifact(),
			new PushmePullyuArtifact(),
			new LargePewterBowlArtifact(),
			new GargishMemorialStatueArtifact(),
			new StolenBottlesOfLiquor1Artifact(),
			new StolenBottlesOfLiquor2Artifact(),
			new StolenBottlesOfLiquor3Artifact(),
			new StolenBottlesOfLiquor4Artifact(),
			new BottlesOfSpoiledWine1Artifact(),
			new BottlesOfSpoiledWine2Artifact(),
			new BottlesOfSpoiledWine3Artifact(),
			new NaverysWeb1Artifact(),
			new NaverysWeb2Artifact(),
			new NaverysWeb3Artifact(),
			new NaverysWeb4Artifact(),
			new NaverysWeb5Artifact(),
			new NaverysWeb6Artifact(),
			new RottedOarsArtifact(),
			new BloodySpoonArtifact(),
			new MysteriousSupperArtifact(),
			new DriedUpInkWellArtifact(),
			new TyballsFlaskStandArtifact(),
			new BlockAndTackleArtifact(),
			new RemnantsOfMeatLoafArtifact(),
			new HalfEatenSupperArtifact(),
			new PricelessTreasureArtifact(),
			new FakeCopperIngotsArtifact(),
			new JugsOfGoblinRotgutArtifact(),
			new BatteredPanArtifact(),
			new RustedPanArtifact(),
			new Basket1Artifact(),
			new Basket2Artifact(),
			new Basket3WestArtifact(),
			new Basket3NorthArtifact(),
			new Basket4Artifact(),
			new Basket5WestArtifact(),
			new Basket5NorthArtifact(),
			new Basket6Artifact(),
			new BowlArtifact(),
			new BowlsVerticalArtifact(),
			new BowlsHorizontalArtifact(),
			new CupsArtifact(),
			new FanWestArtifact(),
			new FanNorthArtifact(),
			new TripleFanWestArtifact(),
			new TripleFanNorthArtifact(),
			new FlowersArtifact(),
			new Painting1WestArtifact(),
			new Painting1NorthArtifact(),
			new Painting2WestArtifact(),
			new Painting2NorthArtifact(),
			new Painting3Artifact(),
			new Painting4WestArtifact(),
			new Painting4NorthArtifact(),
			new Painting5WestArtifact(),
			new Painting5NorthArtifact(),
			new Painting6WestArtifact(),
			new Painting6NorthArtifact(),
			new SakeArtifact(),
			new Sculpture1Artifact(),
			new Sculpture2Artifact(),
			new DolphinLeftArtifact(),
			new DolphinRightArtifact(),
			new ManStatuetteSouthArtifact(),
			new ManStatuetteEastArtifact(),
			new SwordDisplay1WestArtifact(),
			new SwordDisplay1NorthArtifact(),
			new SwordDisplay2WestArtifact(),
			new SwordDisplay2NorthArtifact(),
			new SwordDisplay3SouthArtifact(),
			new SwordDisplay3EastArtifact(),
			new SwordDisplay4WestArtifact(),
			new SwordDisplay4NorthArtifact(),
			new SwordDisplay5WestArtifact(),
			new SwordDisplay5NorthArtifact(),
			new TeapotWestArtifact(),
			new TeapotNorthArtifact(),
			new TowerLanternArtifact(),
			new Urn1Artifact(),
			new Urn2Artifact(),
			new ZenRock1Artifact(),
			new ZenRock2Artifact(),
			new ZenRock3Artifact(),
			new MinotaurArtifact(),
			new ForgedMetalOfArtifacts(),
        };

        private List<Item> currentRandomItems = new List<Item>();
        private int[] currentRandomPrices = new int[9];

        public ArtifactGamblerGump(Mobile from) : base(0, 0)
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
                    from.SendGump(new ArtifactGamblerGump(from));
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
                        from.SendGump(new ArtifactGamblerGump(from));
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
