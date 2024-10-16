using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TarekTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TarekTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tarek the Tinkerer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(Utility.RandomDyedHue())); // Colorful tinkerer shirt
        AddItem(new LongPants(Utility.RandomDyedHue())); // Dyed pants
        AddItem(new Sandals(Utility.RandomBrightHue())); // Bright sandals
        AddItem(new LeatherGloves()); // Leather gloves for tinkering
        AddItem(new HalfApron(Utility.RandomNeutralHue())); // Apron to hold tinkering tools

        VirtualArmor = 15;
    }

    public TarekTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Tarek, a tinkerer of oddities and curiosities. Do you have an eye for rare metals and a heart for adventure?");
        
        // Dialogue options
        greeting.AddOption("What kind of oddities do you tinker with?", 
            p => true, 
            p =>
            {
                DialogueModule odditiesModule = new DialogueModule("I craft devices and potions unlike anything you've seen. The rarest of my creations require FineIronWire, a material of incredible versatility. Perhaps you have some?");
                
                // Additional nested options to enhance the dialogue depth
                odditiesModule.AddOption("Tell me more about your creations.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule creationsDetailModule = new DialogueModule("Ah, where do I begin? I have concoctions that can alter one's voice, gears that can twist the flow of time, and blades sharp enough to cut through the very fabric of reality. My obsession, however, is not without its cost. The art of tinkering takes its toll on the mind and body, and I often find myself in need of rare materials.");
                        
                        creationsDetailModule.AddOption("What kind of toll does it take on you?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule tollDetailModule = new DialogueModule("A toll both physical and mental. The constant hammering, the heat of the forge, and the precision required have scarred my hands. The nights grow long, and my dreams have become haunted by gears and endless mechanisms. But the passion keeps me alive, the need to create keeps me sane—if you can call it that.");
                                
                                tollDetailModule.AddOption("That sounds intense. Why do you keep doing it?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule reasonDetailModule = new DialogueModule("Because, traveler, it's not about choice anymore. It's an obsession. The sound of gears clicking, the smell of molten metal—it is my life. I crave the perfection of each creation, and once you hear the whisper of inspiration, it never lets you go. It might seem unsettling, but we all have our purpose, don't we?");
                                        reasonDetailModule.AddOption("I understand. Can I help you somehow?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("Indeed, you can. As I mentioned earlier, FineIronWire is invaluable to me. If you have any, I can make it worth your while.");
                                                helpModule.AddOption("Let's talk about trading FineIronWire.", 
                                                    plllll => true, 
                                                    plllll =>
                                                    {
                                                        DialogueModule tradeIntroductionModule = new DialogueModule("If you bring me FineIronWire, I can offer you either a SeaSerpentSteak or a vial of RareOil, along with a special scroll of knowledge. But be warned—the rewards come only once every 10 minutes.");
                                                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                            pllllll => CanTradeWithPlayer(pllllll), 
                                                            pllllll =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have FineIronWire for me?");
                                                                tradeModule.AddOption("Yes, I have FineIronWire.", 
                                                                    plllllll => HasFineIronWire(plllllll) && CanTradeWithPlayer(plllllll), 
                                                                    plllllll =>
                                                                    {
                                                                        CompleteTrade(plllllll);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have any right now.", 
                                                                    plllllll => !HasFineIronWire(plllllll), 
                                                                    plllllll =>
                                                                    {
                                                                        plllllll.SendMessage("Come back when you have FineIronWire. I will be here, tinkering away.");
                                                                        plllllll.SendGump(new DialogueGump(plllllll, CreateGreetingModule(plllllll)));
                                                                    });
                                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                    plllllll => !CanTradeWithPlayer(plllllll), 
                                                                    plllllll =>
                                                                    {
                                                                        plllllll.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plllllll.SendGump(new DialogueGump(plllllll, CreateGreetingModule(plllllll)));
                                                                    });
                                                                pllllll.SendGump(new DialogueGump(pllllll, tradeModule));
                                                            });
                                                        tradeIntroductionModule.AddOption("Maybe another time.", 
                                                            pllllll => true, 
                                                            pllllll =>
                                                            {
                                                                pllllll.SendGump(new DialogueGump(pllllll, CreateGreetingModule(pllllll)));
                                                            });
                                                        plllll.SendGump(new DialogueGump(plllll, tradeIntroductionModule));
                                                    });
                                                pllll.SendGump(new DialogueGump(pllll, helpModule));
                                            });
                                        plll.SendGump(new DialogueGump(plll, reasonDetailModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, tollDetailModule));
                            });
                        pl.SendGump(new DialogueGump(pl, creationsDetailModule));
                    });
                
                // Trade option after story
                odditiesModule.AddOption("Do you need FineIronWire?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I do! If you bring me FineIronWire, I can offer you either a SeaSerpentSteak or a vial of RareOil, along with a special scroll of knowledge.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have FineIronWire for me?");
                                tradeModule.AddOption("Yes, I have FineIronWire.", 
                                    plaa => HasFineIronWire(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have any right now.", 
                                    plaa => !HasFineIronWire(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have FineIronWire. I will be here, tinkering away.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });
                        tradeIntroductionModule.AddOption("Maybe another time.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });

                p.SendGump(new DialogueGump(p, odditiesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Tarek waves and returns to his tinkering.");
            });

        return greeting;
    }

    private bool HasFineIronWire(PlayerMobile player)
    {
        // Check the player's inventory for FineIronWire
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FineIronWire)) != null;
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
        // Remove the FineIronWire and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item fineIronWire = player.Backpack.FindItemByType(typeof(FineIronWire));
        if (fineIronWire != null)
        {
            fineIronWire.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for SeaSerpentSteak and RareOil
            rewardChoiceModule.AddOption("SeaSerpantSteak", pl => true, pl => 
            {
                pl.AddToBackpack(new SeaSerpantSteak());
                pl.SendMessage("You receive a SeaSerpentSteak!");
            });
            
            rewardChoiceModule.AddOption("RareOil", pl => true, pl =>
            {
                pl.AddToBackpack(new RareOil());
                pl.SendMessage("You receive a vial of RareOil!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have FineIronWire.");
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