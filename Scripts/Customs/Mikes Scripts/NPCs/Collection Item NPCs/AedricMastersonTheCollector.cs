using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class AedricMastersonTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AedricMastersonTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aedric Masterson, the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1753)); // Deep blue fancy shirt
        AddItem(new LongPants(1175)); // Black pants
        AddItem(new ThighBoots(1109)); // Dark leather boots
        AddItem(new WideBrimHat(1266)); // Wine-colored wide brim hat
        AddItem(new Cloak(2211)); // Emerald green cloak
        AddItem(new SmithHammer()); // Hammer to represent his collector’s work

        VirtualArmor = 15;
    }

    public AedricMastersonTheCollector(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule(player);
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule(PlayerMobile player)
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aedric Masterson, a collector of rare and fascinating artifacts. Tell me, do you possess any treasures worth trading?");

        // Dialogue options
        greeting.AddOption("What kind of artifacts are you looking for?",
            p => true,
            p =>
            {
                DialogueModule artifactsModule = new DialogueModule("I'm currently seeking a Hildebrandt Tapestry. If you have one, I am willing to trade you a unique item from my collection. You may choose between an AnimalBox or a BakingBoard, and I will also give you a MaxxiaScroll as a token of appreciation.");

                // Nested dialogue about Aedric's past
                artifactsModule.AddOption("Why the Hildebrandt Tapestry?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tapestryStoryModule = new DialogueModule("Ah, the Hildebrandt Tapestry... it holds a special place in my heart. You see, during my... less reputable years, I once had the opportunity to 'borrow' it. I say 'borrow' because, back then, I was a pickpocket. Not proud of it, of course. But I've since tried to turn my life around. The Tapestry is my way of giving back. I want to return it to those who deserve it, as a gesture of atonement.");

                        // Additional remorseful reflection
                        tapestryStoryModule.AddOption("You were a pickpocket?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule pickpocketStoryModule = new DialogueModule("Indeed, a reformed one at that. I was quick with my hands, charming enough to deceive, and foolish enough to think it was the only way to survive. The streets can be cruel, you know? I have wrestled with the guilt of my past actions every day since. I now try to help those less fortunate—those who may be headed down the same path I once walked.");

                                pickpocketStoryModule.AddOption("That sounds like a tough journey. How do you help now?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule helpStoryModule = new DialogueModule("I collect rare artifacts, like this tapestry, and use my profits to support the less fortunate. I donate what I can, help the homeless find shelter, and sometimes even teach them skills to make an honest living. It's not enough to erase my past, but it's a start. I believe everyone deserves a second chance, even someone like me.");

                                        helpStoryModule.AddOption("You're doing good work. I'd like to help if I can.",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Aedric smiles warmly, his eyes showing both gratitude and a hint of lingering regret.");
                                            });
                                        helpStoryModule.AddOption("That's admirable. Not everyone would choose to change.",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Aedric bows his head slightly. 'Thank you, traveler. I only wish to make the world a bit better than I left it before.'");
                                            });

                                        plll.SendGump(new DialogueGump(plll, helpStoryModule));
                                    });

                                pickpocketStoryModule.AddOption("It's good to hear you've changed.",
                                    plll => true,
                                    plll =>
                                    {
                                        plll.SendMessage("Aedric nods slowly, a pensive expression on his face. 'I appreciate your words. It's been a long journey, but I wouldn't change my path now.'");
                                    });

                                pll.SendGump(new DialogueGump(pll, pickpocketStoryModule));
                            });

                        tapestryStoryModule.AddOption("I see. Perhaps I could help in some way.",
                            pll => true,
                            pll =>
                            {
                                pll.SendMessage("Aedric smiles, clearly touched by your willingness to assist. 'Any help is appreciated, friend. The world needs more people like you.'");
                            });

                        pl.SendGump(new DialogueGump(pl, tapestryStoryModule));
                    });

                // Trade option after story
                artifactsModule.AddOption("I have a Hildebrandt Tapestry. Let's make a trade.",
                    pl => CanTradeWithPlayer(pl),
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a Hildebrandt Tapestry for me?");
                        tradeModule.AddOption("Yes, here it is.",
                            plaa => HasHildebrandtTapestry(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasHildebrandtTapestry(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a Hildebrandt Tapestry.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });

                artifactsModule.AddOption("Maybe another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, artifactsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Aedric nods respectfully.");
            });

        return greeting;
    }

    private bool HasHildebrandtTapestry(PlayerMobile player)
    {
        // Check the player's inventory for HildebrandtTapestry
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HildebrandtTapestry)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check if the player can trade, based on the 10-minute cooldown
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the HildebrandtTapestry and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item tapestry = player.Backpack.FindItemByType(typeof(HildebrandtTapestry));
        if (tapestry != null)
        {
            tapestry.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for AnimalBox and BakingBoard
            rewardChoiceModule.AddOption("AnimalBox", pl => true, pl =>
            {
                pl.AddToBackpack(new AnimalBox());
                pl.SendMessage("You receive an AnimalBox!");
            });

            rewardChoiceModule.AddOption("BakingBoard", pl => true, pl =>
            {
                pl.AddToBackpack(new BakingBoard());
                pl.SendMessage("You receive a BakingBoard!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a Hildebrandt Tapestry.");
        }
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