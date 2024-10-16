using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class InnelTheLostScholar : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public InnelTheLostScholar() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Innel, The Lost Scholar";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(60);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Deep blue robe
        AddItem(new Sandals(135)); // Light brown sandals
        AddItem(new WizardsHat(1153)); // Matching blue wizard's hat
        AddItem(new QuarterStaff()); // A scholar's staff
        AddItem(new Pouch()); // Carrying a pouch for his findings

        VirtualArmor = 15;
    }

    public InnelTheLostScholar(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Innel, once a scholar of the Grand Academy. Now, I wander, seeking knowledge that was lost to time. Tell me, do you carry something extraordinary, like a GlassOfBubbly?");
        
        // Dialogue options
        greeting.AddOption("Who are you and what happened to the Grand Academy?", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Ah, the Grand Academy... Once, it was the beacon of all knowledge, but an ill-conceived experiment shattered its foundations. I barely escaped with my life. Now, I search for relics of what was lost.");
                
                storyModule.AddOption("Why did the experiment go wrong?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule experimentModule = new DialogueModule("A fair question, friend. The experiment involved opening a portal to realms unknown, hoping to tap into uncharted sources of magical energy. The Academy's headmasters were blinded by ambition and ignored the warnings. The portal became unstable, releasing untold horrors that decimated our ranks.");
                        
                        experimentModule.AddOption("You must be incredibly brave to have survived.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule survivalModule = new DialogueModule("Brave? Perhaps. Or maybe just cunning. I found myself an exit route while others tried to contain the chaos. Some might call it cowardice, but I call it survival. And survival requires opportunity, and perhaps... a little charm.");
                                
                                survivalModule.AddOption("You seem to have a way with words.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule charmModule = new DialogueModule("Ah, flattery will get you everywhere, my friend. I’ve learned that people often give away more than they intend when they think they are charmed. You see, knowledge isn't just found in books; it's extracted from people, bit by bit, just like coins from a fool's purse.");
                                        
                                        charmModule.AddOption("Are you saying you manipulate people?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule manipulationModule = new DialogueModule("Manipulate is such a harsh word. I prefer to think of it as providing a service. People need hope, stories, someone to trust. I simply... guide them to see the world my way. In exchange, I acquire what I need—knowledge, items, and sometimes a bit of gold.");
                                                
                                                manipulationModule.AddOption("What is it you're truly after?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule trueMotivesModule = new DialogueModule("Ah, the ultimate question. I seek the grandest prize of all: the forgotten secrets of the ancients. There are relics hidden in this world, relics that could grant power beyond imagination. And while others waste their time on petty squabbles, I plan, I search, and I use every resource at my disposal—including kind souls like yourself.");
                                                        
                                                        trueMotivesModule.AddOption("Why reveal your plans to me?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule revealModule = new DialogueModule("Because you might be useful, dear traveler. I never know when I might need a willing accomplice, someone to do what I cannot. Besides, a secret shared is sometimes more dangerous than one kept, especially when the other person realizes how deep they are entangled.");
                                                                
                                                                revealModule.AddOption("I'm not sure if I should trust you.", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule trustModule = new DialogueModule("Trust? Now, that's a luxury few can afford. But consider this—working with me, you might just learn something incredible, something that could change your fate. Or, you can walk away now, and always wonder what might have been.");
                                                                        
                                                                        trustModule.AddOption("I'll consider it. What do you need from me?", 
                                                                            plaaaaaaa => true, 
                                                                            plaaaaaaa =>
                                                                            {
                                                                                DialogueModule taskModule = new DialogueModule("Ah, now we're talking. For now, I need information. There are rumors of an ancient scroll hidden in the nearby ruins. If you find anything out of the ordinary, bring it to me. And of course, if you have a GlassOfBubbly, we could conduct a little... trade.");
                                                                                
                                                                                taskModule.AddOption("Tell me more about the trade.", 
                                                                                    plaaaaaaaa => true, 
                                                                                    plaaaaaaaa =>
                                                                                    {
                                                                                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a GlassOfBubbly, I can offer you a choice of rewards. Knowledge should be rewarded, don’t you agree? You may choose between a FishBasket or a FineSilverWire.");
                                                                                         
                                                                                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                                                                                            plaaaaaaaaa => CanTradeWithPlayer(plaaaaaaaaa), 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                DialogueModule tradeModule = new DialogueModule("Do you have a GlassOfBubbly for me?");
                                                                                                 
                                                                                                tradeModule.AddOption("Yes, I have a GlassOfBubbly.", 
                                                                                                    plaaaaaaaaaa => HasGlassOfBubbly(plaaaaaaaaaa) && CanTradeWithPlayer(plaaaaaaaaaa), 
                                                                                                    plaaaaaaaaaa =>
                                                                                                    {
                                                                                                        CompleteTrade(plaaaaaaaaaa);
                                                                                                    });
                                                                                                 
                                                                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                                                                    plaaaaaaaaaa => !HasGlassOfBubbly(plaaaaaaaaaa), 
                                                                                                    plaaaaaaaaaa =>
                                                                                                    {
                                                                                                        plaaaaaaaaaa.SendMessage("Come back when you have a GlassOfBubbly.");
                                                                                                        plaaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaaa, CreateGreetingModule(plaaaaaaaaaa)));
                                                                                                    });
                                                                                                 
                                                                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                                                    plaaaaaaaaaa => !CanTradeWithPlayer(plaaaaaaaaaa), 
                                                                                                    plaaaaaaaaaa =>
                                                                                                    {
                                                                                                        plaaaaaaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                                        plaaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaaa, CreateGreetingModule(plaaaaaaaaaa)));
                                                                                                    });
                                                                                                 
                                                                                                plaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaa, tradeModule));
                                                                                            });
                                                                                         
                                                                                        tradeIntroductionModule.AddOption("Maybe another time.", 
                                                                                            plaaaaaaaaa => true, 
                                                                                            plaaaaaaaaa =>
                                                                                            {
                                                                                                plaaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaaa, CreateGreetingModule(plaaaaaaaaa)));
                                                                                            });
                                                                                         
                                                                                        plaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaa, tradeIntroductionModule));
                                                                                    });
                                                                                 
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, taskModule));
                                                                            });
                                                                    });
                                                            });
                                                    });
                                            });
                                    });
                            });
                    });

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Innel nods, lost once again in his thoughts.");
            });

        return greeting;
    }

    private bool HasGlassOfBubbly(PlayerMobile player)
    {
        // Check the player's inventory for GlassOfBubbly
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GlassOfBubbly)) != null;
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
        // Remove the GlassOfBubbly and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item glassOfBubbly = player.Backpack.FindItemByType(typeof(GlassOfBubbly));
        if (glassOfBubbly != null)
        {
            glassOfBubbly.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FishBasket and FineSilverWire
            rewardChoiceModule.AddOption("FishBasket", pl => true, pl => 
            {
                pl.AddToBackpack(new FishBasket());
                pl.SendMessage("You receive a FishBasket!");
            });
            
            rewardChoiceModule.AddOption("FineSilverWire", pl => true, pl =>
            {
                pl.AddToBackpack(new FineSilverWire());
                pl.SendMessage("You receive a FineSilverWire!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a GlassOfBubbly.");
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