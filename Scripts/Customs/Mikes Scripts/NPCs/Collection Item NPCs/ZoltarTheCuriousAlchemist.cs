using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZoltarTheCuriousAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZoltarTheCuriousAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zoltar the Curious Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1260)); // A dark purple robe
        AddItem(new Sandals(1175)); // Emerald green sandals
        AddItem(new WizardsHat(1260)); // A matching purple wizard's hat
        AddItem(new GoldBracelet()); // A golden bracelet
        AddItem(new SalvageMachine()); // A decorative alchemical item in his inventory

        VirtualArmor = 12;
    }

    public ZoltarTheCuriousAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Zoltar, a seeker of forgotten alchemical secrets. Tell me, have you dabbled in the mysterious arts of alchemy?");
        
        // Dialogue options
        greeting.AddOption("Tell me about your alchemical pursuits.", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Alchemy is the key to unraveling the secrets of nature itself! I have been experimenting with rare ingredients and old recipes, but I am in need of certain rare items to continue my research.");
                
                // Nested dialogue options
                storyModule.AddOption("Why are these secrets so important to you?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule importanceModule = new DialogueModule("Ah, you see, there are whispers of power beyond imagination, hidden in the ancient arts. There is also... a personal reason. A spirit, bound to this realm, speaks to me. She is tormented, relentless, and she demands justice for her untimely demise. She was once betrayed, and only by discovering these alchemical secrets can I bring peace to her tortured soul.");
                        
                        importanceModule.AddOption("A spirit? Who is she?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule spiritModule = new DialogueModule("Her name is Aeliana, a vengeful spirit bound to the place of her tragic end. She was betrayed by those she trusted, and now she seeks retribution. I have become her unwilling vessel, and she drives me to seek revenge against her murderers through alchemical means. It is not just knowledge I seek, but a way to help her find justice and perhaps, redemption.");
                                
                                spiritModule.AddOption("How can I help?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule helpModule = new DialogueModule("If you truly wish to help, I need items of great rarity to create the potion that will free Aeliana from her torment. One such item is the AlchemistBookcase, filled with the knowledge of those who betrayed her. If you bring it to me, I can take a step closer to giving her peace.");
                                        
                                        helpModule.AddOption("What happens if you fail?", 
                                            plaai => true, 
                                            plaai =>
                                            {
                                                DialogueModule failureModule = new DialogueModule("If I fail, she will never rest. Her wrath grows stronger with each passing day. She torments my dreams, and soon, she may seek another to fulfill her quest. The innocent will suffer, for her rage knows no bounds. I must succeed, or others will pay the price for my shortcomings.");
                                                
                                                failureModule.AddOption("This sounds dangerous. Are you sure you should be doing this?", 
                                                    plaaa => true, 
                                                    plaaa =>
                                                    {
                                                        DialogueModule dangerousModule = new DialogueModule("I have no choice. Aeliana's presence is bound to me, and her whispers are unending. She promises power, but I desire only freedom—from her voice, from her pain. If you bring me what I need, perhaps together we can break her chains.");
                                                        
                                                        dangerousModule.AddOption("I will see what I can do.", 
                                                            plaaaa => true, 
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendMessage("Zoltar nods solemnly, his eyes filled with both hope and fear. 'Thank you, traveler. Remember, I need the AlchemistBookcase.'");
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                            });
                                                        
                                                        plaaa.SendGump(new DialogueGump(plaaa, dangerousModule));
                                                    });
                                                
                                                plaai.SendGump(new DialogueGump(plaai, failureModule));
                                            });
                                        
                                        helpModule.AddOption("I'd like to help, but I need time to prepare.", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Zoltar nods understandingly. 'Take your time, but do not delay too long. Aeliana grows restless.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, helpModule));
                                    });
                                
                                spiritModule.AddOption("This is too dangerous for me.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zoltar nods, though his eyes are darkened by despair. 'I understand. Few have the courage to face such restless spirits.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, spiritModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, importanceModule));
                    });
                
                // Trade option after story
                storyModule.AddOption("What items do you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I am searching for an AlchemistBookcase. If you have one, I can offer you a choice of either an ElementRegular or a Satchel in return. Additionally, I always have a MaxxiaScroll to give to those who help me.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an AlchemistBookcase for me?");
                                tradeModule.AddOption("Yes, I have an AlchemistBookcase.", 
                                    plaa => HasAlchemistBookcase(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasAlchemistBookcase(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an AlchemistBookcase.");
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

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Zoltar nods thoughtfully, his eyes still glimmering with curiosity.");
            });

        return greeting;
    }

    private bool HasAlchemistBookcase(PlayerMobile player)
    {
        // Check the player's inventory for AlchemistBookcase
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(AlchemistBookcase)) != null;
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
        // Remove the AlchemistBookcase and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item alchemistBookcase = player.Backpack.FindItemByType(typeof(AlchemistBookcase));
        if (alchemistBookcase != null)
        {
            alchemistBookcase.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for ElementRegular and Satchel
            rewardChoiceModule.AddOption("ElementRegular", pl => true, pl => 
            {
                pl.AddToBackpack(new ElementRegular());
                pl.SendMessage("You receive an ElementRegular!");
            });
            
            rewardChoiceModule.AddOption("Satchel", pl => true, pl =>
            {
                pl.AddToBackpack(new Satchel());
                pl.SendMessage("You receive a Satchel!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an AlchemistBookcase.");
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