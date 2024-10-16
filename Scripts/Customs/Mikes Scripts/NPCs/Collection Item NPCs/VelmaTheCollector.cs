using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class VelmaTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public VelmaTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Velma the Collector";
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
        AddItem(new FancyDress(1175)); // A vibrant purple dress
        AddItem(new Sandals(1109)); // Dark blue sandals
        AddItem(new FeatheredHat(1150)); // A colorful feathered hat
        AddItem(new GoldBracelet()); // A shiny gold bracelet

        VirtualArmor = 10;
    }

    public VelmaTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Velma, a collector of rare and unusual artifacts. Do you happen to have anything... unique that might interest me?");
        
        // Dialogue options
        greeting.AddOption("What kind of artifacts are you interested in?", 
            p => true, 
            p =>
            {
                DialogueModule interestModule = new DialogueModule("I'm particularly interested in rare curiosities. Say... do you have a 'GamerGirlFeet' item? If you do, I'd be willing to offer you a choice of a FancyPainting or a SoftTowel in exchange.");
                
                interestModule.AddOption("I'd like to make a trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the 'GamerGirlFeet' item for me?");
                        tradeModule.AddOption("Yes, I have it right here.", 
                            plaa => HasGamerGirlFeet(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasGamerGirlFeet(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have the 'GamerGirlFeet' item.");
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

                interestModule.AddOption("Tell me more about your collection.", 
                    pla => true, 
                    pla =>
                    {
                        DialogueModule collectionModule = new DialogueModule("My collection is not for the faint of heart. Each piece has a story, and each story has a soul. You see, I was once a silversmith, crafting beautiful trinkets out of pure silver, but I discovered something... extraordinary.");
                        
                        collectionModule.AddOption("What did you discover?", 
                            plb => true, 
                            plb =>
                            {
                                DialogueModule discoveryModule = new DialogueModule("I discovered that silver has a power, a connection to the spirit world. With the right incantations, I could trap the essence of a person within the silver. Imagine a pendant that whispers secrets from beyond, or a bracelet that pulses with the fear of its previous owner.");
                                
                                discoveryModule.AddOption("That sounds dangerous.", 
                                    plc => true, 
                                    plc =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Dangerous? Perhaps. But power always comes with danger, doesn't it? The desperate and the curious are drawn to my creations. They come seeking answers, and I provide them... for a price.");
                                        
                                        dangerModule.AddOption("What kind of price?", 
                                            pld => true, 
                                            pld =>
                                            {
                                                DialogueModule priceModule = new DialogueModule("The price varies. Sometimes it's a simple trade, an item of equal curiosity. Other times, it's something more... personal. A memory, a promise, a favor yet to be named. The value of a soul is not always measured in gold, you see.");
                                                
                                                priceModule.AddOption("I see... Do you have any items for trade now?", 
                                                    ple => true, 
                                                    ple =>
                                                    {
                                                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I do. If you have something that piques my interest, I can offer you a FancyPainting or a SoftTowel. And, of course, a little something extra—a MaxxiaScroll, infused with whispers of knowledge.");
                                                        
                                                        tradeIntroductionModule.AddOption("Let's proceed with the trade.", 
                                                            plf => CanTradeWithPlayer(plf), 
                                                            plf =>
                                                            {
                                                                DialogueModule tradeModule = new DialogueModule("Do you have the 'GamerGirlFeet' item for me?");
                                                                tradeModule.AddOption("Yes, I have it right here.", 
                                                                    plg => HasGamerGirlFeet(plg) && CanTradeWithPlayer(plg), 
                                                                    plg =>
                                                                    {
                                                                        CompleteTrade(plg);
                                                                    });
                                                                tradeModule.AddOption("No, I don't have one right now.", 
                                                                    plg => !HasGamerGirlFeet(plg), 
                                                                    plg =>
                                                                    {
                                                                        plg.SendMessage("Come back when you have the 'GamerGirlFeet' item.");
                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule(plg)));
                                                                    });
                                                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                                                    plg => !CanTradeWithPlayer(plg), 
                                                                    plg =>
                                                                    {
                                                                        plg.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule(plg)));
                                                                    });
                                                                plf.SendGump(new DialogueGump(plf, tradeModule));
                                                            });
                                                        
                                                        tradeIntroductionModule.AddOption("Maybe another time.", 
                                                            plf => true, 
                                                            plf =>
                                                            {
                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule(plf)));
                                                            });
                                                        ple.SendGump(new DialogueGump(ple, tradeIntroductionModule));
                                                    });

                                                priceModule.AddOption("I think I'll pass for now.", 
                                                    ple => true, 
                                                    ple =>
                                                    {
                                                        ple.SendMessage("Velma smiles faintly, her eyes glinting with secrets untold.");
                                                    });
                                                pld.SendGump(new DialogueGump(pld, priceModule));
                                            });
                                        plc.SendGump(new DialogueGump(plc, dangerModule));
                                    });
                                
                                discoveryModule.AddOption("That sounds fascinating. Can you show me an example?", 
                                    plc => true, 
                                    plc =>
                                    {
                                        DialogueModule exampleModule = new DialogueModule("Ah, I see you're intrigued. Very well, behold this pendant. It contains the essence of a man who was once a great scholar. Listen closely, and you may hear his voice, still lamenting the knowledge he sought but never found.");
                                        
                                        exampleModule.AddOption("What happened to him?", 
                                            pld => true, 
                                            pld =>
                                            {
                                                DialogueModule fateModule = new DialogueModule("He delved too deeply into secrets best left unknown. The darkness consumed him, and now his soul is bound within this silver. A warning, perhaps, for those who seek too much.");
                                                
                                                fateModule.AddOption("A warning I will heed.", 
                                                    ple => true, 
                                                    ple =>
                                                    {
                                                        ple.SendMessage("Velma nods solemnly, her expression unreadable.");
                                                    });
                                                
                                                fateModule.AddOption("I must know more. Tell me everything.", 
                                                    ple => true, 
                                                    ple =>
                                                    {
                                                        DialogueModule moreModule = new DialogueModule("Very well, but know that knowledge is a burden. The more you learn, the closer you come to the same fate. Are you prepared for that?");
                                                        
                                                        moreModule.AddOption("I am prepared.", 
                                                            plf => true, 
                                                            plf =>
                                                            {
                                                                plf.SendMessage("Velma smiles, a glimmer of respect in her eyes. 'Then let us continue.'");
                                                            });
                                                        
                                                        moreModule.AddOption("Perhaps another time.", 
                                                            plf => true, 
                                                            plf =>
                                                            {
                                                                plf.SendMessage("Velma gives a knowing nod, her eyes never leaving yours.");
                                                            });
                                                        ple.SendGump(new DialogueGump(ple, moreModule));
                                                    });
                                                pld.SendGump(new DialogueGump(pld, fateModule));
                                            });
                                        plc.SendGump(new DialogueGump(plc, exampleModule));
                                    });
                                plb.SendGump(new DialogueGump(plb, discoveryModule));
                            });
                        
                        collectionModule.AddOption("This all sounds too strange for me.", 
                            plb => true, 
                            plb =>
                            {
                                plb.SendMessage("Velma chuckles softly, 'Not everyone is ready for the whispers of the silver. Perhaps one day, you'll be back.'");
                            });
                        pla.SendGump(new DialogueGump(pla, collectionModule));
                    });

                interestModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, interestModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Velma nods knowingly, her eyes still scanning for interesting items.");
            });

        return greeting;
    }

    private bool HasGamerGirlFeet(PlayerMobile player)
    {
        // Check the player's inventory for GamerGirlFeet
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GamerGirlFeet)) != null;
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
        // Remove the GamerGirlFeet and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item gamerGirlFeet = player.Backpack.FindItemByType(typeof(GamerGirlFeet));
        if (gamerGirlFeet != null)
        {
            gamerGirlFeet.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FancyPainting and SoftTowel
            rewardChoiceModule.AddOption("FancyPainting", pl => true, pl => 
            {
                pl.AddToBackpack(new FancyPainting());
                pl.SendMessage("You receive a FancyPainting!");
            });
            
            rewardChoiceModule.AddOption("SoftTowel", pl => true, pl =>
            {
                pl.AddToBackpack(new SoftTowel());
                pl.SendMessage("You receive a SoftTowel!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have the 'GamerGirlFeet' item.");
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