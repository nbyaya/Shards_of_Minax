using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ElizaTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ElizaTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Eliza the Collector";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(110);

        SetHits(90);
        SetMana(160);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1161)); // Bright blue fancy shirt
        AddItem(new Skirt(2072)); // Forest green skirt
        AddItem(new Sandals(1150)); // Golden sandals
        AddItem(new Bandana(1153)); // Golden bandana
        AddItem(new GoldBracelet()); // A gold bracelet for flair

        VirtualArmor = 12;
    }

    public ElizaTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello, traveler! I'm Eliza, a collector of rare and unusual items. Have you stumbled across anything slimy in your travels?");
        
        // Dialogue options
        greeting.AddOption("What kind of items do you collect?", 
            p => true, 
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I collect all sorts of oddities, but lately, I've been fascinated by a substance called 'CupOfSlime.' It's said to be quite rare. If you happen to have one, I'd be willing to offer you something special in exchange.");
                
                collectionModule.AddOption("Why are you interested in CupOfSlime?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule slimeInterestModule = new DialogueModule("Ah, that's a good question! You see, CupOfSlime is not just rare, it's also valuable for its many... uses. Some say it has mystical properties, others just want it for its viscosity. Me? I have my own reasons, but let's just say it brings me opportunities to deal with those in high places and low ones too.");
                        
                        slimeInterestModule.AddOption("Who are these people you deal with?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule dealingsModule = new DialogueModule("Well, I don't usually reveal my clients, but since you're curious... let's say they aren't the kind of people you'd invite over for tea. Some are nobles with a penchant for dark arts, others are... well, less reputable folks who frequent the shadows, if you catch my drift.");
                                
                                dealingsModule.AddOption("Sounds dangerous. How do you stay safe?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule safetyModule = new DialogueModule("Oh, I'm always careful. I never show all my cards, and I make sure to keep my relationships... transactional. Loyalty is a luxury I cannot afford. Besides, I have a few tricks up my sleeve, and I know more secrets about those people than they'd like anyone to know.");
                                        
                                        safetyModule.AddOption("Do you have any secrets to share?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule secretsModule = new DialogueModule("Oh, you're quite the curious one, aren't you? Well, let's just say there's a certain guard captain in Britain who's not as loyal to the crown as he appears. But, information like that isn't free, my friend. If you help me out, maybe I'll let you in on more secrets.");
                                                
                                                secretsModule.AddOption("What kind of help do you need?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Bring me a CupOfSlime, and I'll tell you something that could change your fortunes. You see, I play both sides, and knowing who is betraying who is the key to survival in this world. Are you willing to take the risk?");
                                                        
                                                        helpModule.AddOption("I have a CupOfSlime. What can you offer?", 
                                                            plq => true, 
                                                            plq =>
                                                            {
                                                                DialogueModule tradeIntroductionModule = new DialogueModule("Wonderful! If you give me a CupOfSlime, you may choose between a GingerbreadCookie or a CarvingMachine, plus I'll throw in a MaxxiaScroll as a token of my gratitude.");
                                                                
                                                                tradeIntroductionModule.AddOption("Let's make the trade.", 
                                                                    plaw => CanTradeWithPlayer(pla), 
                                                                    plaw =>
                                                                    {
                                                                        DialogueModule tradeModule = new DialogueModule("Do you have a CupOfSlime for me?");
                                                                        
                                                                        tradeModule.AddOption("Yes, I have a CupOfSlime.", 
                                                                            plaae => HasCupOfSlime(plaa) && CanTradeWithPlayer(plaa), 
                                                                            plaae =>
                                                                            {
                                                                                CompleteTrade(plaa);
                                                                            });
                                                                        
                                                                        tradeModule.AddOption("No, I don't have one right now.", 
                                                                            plaar => !HasCupOfSlime(plaa), 
                                                                            plaar =>
                                                                            {
                                                                                plaa.SendMessage("Come back when you have a CupOfSlime.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        
                                                                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                            plaat => !CanTradeWithPlayer(plaat), 
                                                                            plaat =>
                                                                            {
                                                                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                                                            });
                                                                        
                                                                        pla.SendGump(new DialogueGump(pla, tradeModule));
                                                                    });
                                                                
                                                                tradeIntroductionModule.AddOption("Maybe another time.", 
                                                                    play => true, 
                                                                    play =>
                                                                    {
                                                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                                                    });
                                                                
                                                                pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                                                            });
                                                        
                                                        helpModule.AddOption("Why should I take such a risk for you?", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule riskModule = new DialogueModule("Why, you should take the risk because knowledge is power, and power is everything in this world. Besides, if you know the right things about the right people, you can make a lot of gold... or ensure your own survival. Think about it.");
                                                                
                                                                riskModule.AddOption("I'll think about it.", 
                                                                    plaaaaaa => true, 
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Eliza nods knowingly, as if she already knows your answer.");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, riskModule));
                                                            });
                                                        
                                                        helpModule.AddOption("I'm not interested.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Eliza smirks, as if amused by your hesitation.");
                                                            });
                                                        
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });
                                                
                                                secretsModule.AddOption("Tell me more about these betrayals.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule betrayalsModule = new DialogueModule("Ah, well, let's just say that the walls of Britain have ears, and the nobles who live behind them often have secrets they don't want getting out. The question is, do you want to be the one who knows, or the one who is left in the dark?");
                                                        
                                                        betrayalsModule.AddOption("I want to know more.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Eliza grins slyly. 'Then bring me what I need, and the knowledge will be yours.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        
                                                        betrayalsModule.AddOption("No thanks, I'd rather stay out of it.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Eliza shrugs. 'Suit yourself. But remember, knowledge is the best armor in this dangerous world.'");
                                                            });
                                                        
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, betrayalsModule));
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, secretsModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, safetyModule));
                                    });
                                
                                dealingsModule.AddOption("Can you tell me more about your less reputable clients?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule clientsModule = new DialogueModule("Ah, the smugglers and thieves? They can be quite the interesting bunch. Always looking for ways to make a quick profit, and always willing to sell out their own mother if it meant an extra coin in their pocket. Opportunistic, sly, and utterly untrustworthy... but they do have their uses.");
                                        
                                        clientsModule.AddOption("Sounds like you're just like them.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule likeThemModule = new DialogueModule("Oh, perhaps I am. But unlike them, I know how to keep myself alive, and I know when to cut my losses. If you think you can navigate these waters without getting your feet wet, you're mistaken.");
                                                
                                                likeThemModule.AddOption("I suppose you're right.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Eliza nods. 'Smart answer. Now, let's talk business.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                
                                                likeThemModule.AddOption("I'd rather stay on dry land, thanks.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Eliza chuckles. 'Good luck with that, traveler. The world isn't as simple as you'd like it to be.'");
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, likeThemModule));
                                            });
                                        
                                        clientsModule.AddOption("Why do you deal with them if they're so untrustworthy?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule whyDealModule = new DialogueModule("Because, my dear traveler, everyone has a price. Even the most untrustworthy scoundrel can be useful if you know how to leverage their greed. It's all about knowing what motivates people, and then using it to your advantage.");
                                                
                                                whyDealModule.AddOption("I see. You really are opportunistic.", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Eliza smiles. 'Opportunistic, sly, untrustworthy... call me what you will. In the end, I'm still here, and that's what matters.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, whyDealModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, clientsModule));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, dealingsModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, slimeInterestModule));
                    });
                
                collectionModule.AddOption("I have a CupOfSlime. What can you offer?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Wonderful! If you give me a CupOfSlime, you may choose between a GingerbreadCookie or a CarvingMachine, plus I'll throw in a MaxxiaScroll as a token of my gratitude.");
                        tradeIntroductionModule.AddOption("Let's make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a CupOfSlime for me?");
                                tradeModule.AddOption("Yes, I have a CupOfSlime.", 
                                    plaa => HasCupOfSlime(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasCupOfSlime(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a CupOfSlime.");
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

                p.SendGump(new DialogueGump(p, collectionModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Eliza nods and smiles at you.");
            });

        return greeting;
    }

    private bool HasCupOfSlime(PlayerMobile player)
    {
        // Check the player's inventory for CupOfSlime
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CupOfSlime)) != null;
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
        // Remove the CupOfSlime and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item cupOfSlime = player.Backpack.FindItemByType(typeof(CupOfSlime));
        if (cupOfSlime != null)
        {
            cupOfSlime.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for GingerbreadCookie and CarvingMachine
            rewardChoiceModule.AddOption("GingerbreadCookie", pl => true, pl => 
            {
                pl.AddToBackpack(new GingerbreadCookie());
                pl.SendMessage("You receive a GingerbreadCookie!");
            });
            
            rewardChoiceModule.AddOption("CarvingMachine", pl => true, pl =>
            {
                pl.AddToBackpack(new CarvingMachine());
                pl.SendMessage("You receive a CarvingMachine!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a CupOfSlime.");
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