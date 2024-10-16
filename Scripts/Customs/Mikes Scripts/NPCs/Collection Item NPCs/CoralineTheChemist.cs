using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CoralineTheChemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CoralineTheChemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Coraline the Chemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(60);
        SetInt(100);

        SetHits(80);
        SetMana(150);
        SetStam(60);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(2453)); // Robe with a bluish hue
        AddItem(new Boots(1109)); // Boots with a dark hue
        AddItem(new WizardsHat(1153)); // Matching wizard hat
        AddItem(new HalfApron(1)); // A white apron, chemist style
        AddItem(new Bottle()); // Decorative item, she holds a bottle

        VirtualArmor = 10;
    }

    public CoralineTheChemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Coraline, master of concoctions and curious brews. Do you have an interest in the art of fermentation?");

        // Dialogue about her work
        greeting.AddOption("Tell me about your experiments.", 
            p => true, 
            p =>
            {
                DialogueModule experimentsModule = new DialogueModule("I specialize in transforming simple substances into magnificent elixirs! Recently, I've been perfecting my Festive Champagne, a brew for celebrations. But I need a special ingredient: Wood Alcohol.");
                
                experimentsModule.AddOption("What do you need Wood Alcohol for?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule woodAlcoholModule = new DialogueModule("Ah, Wood Alcohol, it's essential for a unique distillation process. When treated properly, it adds a special fizz to my Festive Champagne! Though, of course, it's dangerous if not handled by a professional like myself.");
                        woodAlcoholModule.AddOption("Sounds intriguing, how can I help?", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateTradeModule(pla)));
                            });
                        woodAlcoholModule.AddOption("I think I'll pass for now.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        // Adding new nested dialogue regarding Coraline's background
                        woodAlcoholModule.AddOption("Why do you seem so desperate for this ingredient?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule griefModule = new DialogueModule("Desperate? Perhaps... You see, I once had a son. He fell ill, a mysterious illness that no healer could cure. In my despair, I turned to the dark arts, believing that perhaps some forbidden elixir could bring him back.");
                                griefModule.AddOption("That's heartbreaking... Did it work?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule failedAttemptModule = new DialogueModule("No... It failed. My son is gone, but I can't let go of the hope that I can still find a way. I am willing to use any means necessary, no matter how dark, to bring him back.");
                                        failedAttemptModule.AddOption("Are you sure this is the right path?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule reflectionModule = new DialogueModule("I... I don't know. My heart is torn. Every day I feel the pain of his absence, and every day I fight against my own conscience. Perhaps you're right, but what else do I have left?");
                                                reflectionModule.AddOption("Maybe there are other ways to honor his memory.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Coraline looks down, her eyes misty with tears. 'Perhaps you're right... Perhaps I should focus on the good he brought into my life, rather than the void his absence has left.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                reflectionModule.AddOption("I hope you find peace, Coraline.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Coraline gives a sad smile, her eyes still filled with sorrow. 'Thank you, traveler. May your journey be less burdened than mine.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, reflectionModule));
                                            });
                                        failedAttemptModule.AddOption("I'll help you, whatever it takes.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Coraline's eyes gleam with a mixture of hope and desperation. 'Thank you, traveler. Perhaps with your help, I will succeed where I once failed.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, failedAttemptModule));
                                    });
                                griefModule.AddOption("That's too dark for me. I wish you luck.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Coraline nods solemnly, her eyes filled with both sadness and determination. 'I understand. Not everyone can walk this path.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, griefModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, woodAlcoholModule));
                    });
                
                experimentsModule.AddOption("Is there anything else you need?", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateTradeModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, experimentsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Coraline gives you a friendly nod and goes back to her brewing.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeModule(PlayerMobile player)
    {
        DialogueModule tradeModule = new DialogueModule("Do you happen to have any Wood Alcohol? I would gladly exchange it for a bottle of my finest Festive Champagne, and perhaps something extra.");

        tradeModule.AddOption("Yes, I have Wood Alcohol.", 
            p => HasWoodAlcohol(p) && CanTradeWithPlayer(p), 
            p =>
            {
                CompleteTrade(p);
            });

        tradeModule.AddOption("No, I don't have any right now.", 
            p => !HasWoodAlcohol(p), 
            p =>
            {
                p.SendMessage("Come back when you have some Wood Alcohol for me.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        tradeModule.AddOption("I traded recently; I'll come back later.", 
            p => !CanTradeWithPlayer(p), 
            p =>
            {
                p.SendMessage("You can only trade once every 10 minutes. Please return later.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        return tradeModule;
    }

    private bool HasWoodAlcohol(PlayerMobile player)
    {
        // Check the player's inventory for Wood Alcohol
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WoodAlchohol)) != null;
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
        // Remove the Wood Alcohol and give the Festive Champagne and Maxxia Scroll, then set the cooldown timer
        Item woodAlcohol = player.Backpack.FindItemByType(typeof(WoodAlchohol));
        if (woodAlcohol != null)
        {
            woodAlcohol.Delete();
            player.AddToBackpack(new FestiveChampagne());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Wood Alcohol and receive a bottle of Festive Champagne and a Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have Wood Alcohol.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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