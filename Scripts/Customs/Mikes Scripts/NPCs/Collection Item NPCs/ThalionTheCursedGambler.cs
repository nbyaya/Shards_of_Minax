using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThalionTheCursedGambler : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThalionTheCursedGambler() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thalion, The Cursed Gambler";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(90);

        SetHits(100);
        SetMana(120);
        SetStam(70);

        Fame = 100;
        Karma = -100;

        // Outfit
        AddItem(new FancyShirt(1153)); // A deep purple fancy shirt
        AddItem(new LongPants(1109)); // Dark brown pants
        AddItem(new Bandana(1157)); // Black bandana
        AddItem(new Boots(1175)); // Midnight black boots
        AddItem(new GoldRing()); // Gold ring, to signify his love for gambling wealth

        VirtualArmor = 12;
    }

    public ThalionTheCursedGambler(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Thalion, once a man of luck, now bound by misfortune. Do you carry with you a relic of bad luck, perhaps an UnluckyDice? I could make it worth your while.");
        
        // Dialogue options
        greeting.AddOption("Tell me about your curse.", 
            p => true, 
            p =>
            {
                DialogueModule curseStoryModule = new DialogueModule("Once, I was the luckiest gambler alive, or so I thought. I wagered my soul against a demon's fortune, and lost. Since then, I've roamed the lands, trying to undo my fate, seeking items cursed with bad luck.");
                
                // Adding additional depth and nested options
                curseStoryModule.AddOption("How did you meet the demon?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule demonStoryModule = new DialogueModule("I met the demon at a hidden tavern, deep in the woods. It was a place only those who sought fortune would dare enter. He offered me endless wealth, and I, foolishly drunk on my own luck, took the bet without hesitation.");
                        
                        demonStoryModule.AddOption("What was the bet?", 
                            pl2 => true, 
                            pl2 =>
                            {
                                DialogueModule betStoryModule = new DialogueModule("The bet was simple: a game of dice. Roll higher than him, and I would receive his riches. Roll lower, and my soul would be his. I rolled... and lost. The demon laughed, and I have been cursed ever since.");
                                
                                betStoryModule.AddOption("Do you regret it?", 
                                    pl3 => true, 
                                    pl3 =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? Every moment. But regrets are of no use to a man bound by fate. I have learned to live with the consequences, and now I seek to lessen this burden by gathering items that share my misfortune.");
                                        
                                        pl3.SendGump(new DialogueGump(pl3, regretModule));
                                    });
                                
                                betStoryModule.AddOption("Did you ever try to find the demon again?", 
                                    pl3 => true, 
                                    pl3 =>
                                    {
                                        DialogueModule seekDemonModule = new DialogueModule("I have searched for him, many times. He hides well, though. Perhaps he fears I may be desperate enough to try and end him, or perhaps he simply enjoys watching me suffer. Either way, he is beyond my reach for now.");
                                        
                                        pl3.SendGump(new DialogueGump(pl3, seekDemonModule));
                                    });

                                pl2.SendGump(new DialogueGump(pl2, betStoryModule));
                            });

                        pl.SendGump(new DialogueGump(pl, demonStoryModule));
                    });

                // Trade option after story
                curseStoryModule.AddOption("What do you want with UnluckyDice?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("The UnluckyDice holds residual magic of misfortune, and I can use it to weaken the curse upon me. In exchange, I offer you a choice of either ImportantBooks or RareMinerals, and always a MaxxiaScroll as gratitude.");
                        
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have an UnluckyDice for me?");
                                
                                tradeModule.AddOption("Yes, I have an UnluckyDice.", 
                                    plaa => HasUnluckyDice(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasUnluckyDice(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have an UnluckyDice.");
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

                // Additional philosophical dialogue
                curseStoryModule.AddOption("Do you ever find peace?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule peaceModule = new DialogueModule("Peace... I find glimpses of it, sometimes, in the silence of the graveyards. The dead have no burdens, no curses. They lie still, and I envy them that rest. I speak to them often, and sometimes, they whisper back.");
                        
                        peaceModule.AddOption("What do they say?", 
                            pl2 => true, 
                            pl2 =>
                            {
                                DialogueModule whisperModule = new DialogueModule("They speak of the past, of secrets long forgotten, and of an ancient evil lurking beneath the soil. I fear that whatever lies beneath may soon rise, and I will be powerless to stop it.");
                                
                                whisperModule.AddOption("What ancient evil?", 
                                    pl3 => true, 
                                    pl3 =>
                                    {
                                        DialogueModule ancientEvilModule = new DialogueModule("A force older than any kingdom, older than memory itself. It is said that the earth remembers all, and beneath the graves, something stirs. I hear it in the quiet, in the stillness. It watches, it waits.");
                                        
                                        pl3.SendGump(new DialogueGump(pl3, ancientEvilModule));
                                    });
                                
                                pl2.SendGump(new DialogueGump(pl2, whisperModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, peaceModule));
                    });

                p.SendGump(new DialogueGump(p, curseStoryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thalion nods solemnly, the weight of his curse evident in his eyes.");
            });

        return greeting;
    }

    private bool HasUnluckyDice(PlayerMobile player)
    {
        // Check the player's inventory for UnluckyDice
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(UnluckyDice)) != null;
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
        // Remove the UnluckyDice and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item unluckyDice = player.Backpack.FindItemByType(typeof(UnluckyDice));
        if (unluckyDice != null)
        {
            unluckyDice.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for ImportantBooks and RareMinerals
            rewardChoiceModule.AddOption("ImportantBooks", pl => true, pl => 
            {
                pl.AddToBackpack(new ImportantBooks());
                pl.SendMessage("You receive ImportantBooks!");
            });
            
            rewardChoiceModule.AddOption("RareMinerals", pl => true, pl =>
            {
                pl.AddToBackpack(new RareMinerals());
                pl.SendMessage("You receive RareMinerals!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have an UnluckyDice.");
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