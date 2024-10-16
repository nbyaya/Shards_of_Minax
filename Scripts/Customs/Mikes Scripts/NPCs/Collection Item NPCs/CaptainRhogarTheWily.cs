using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CaptainRhogarTheWily : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaptainRhogarTheWily() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Captain Rhogar the Wily";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(75);
        SetInt(90);

        SetHits(100);
        SetMana(100);
        SetStam(80);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new TricorneHat(1150)); // Black tricorn hat
        AddItem(new FancyShirt(1109)); // Dark red shirt
        AddItem(new LongPants(1107)); // Brown pants
        AddItem(new Boots(1109)); // Black boots
        AddItem(new Cutlass()); // A weapon for show, not use

        VirtualArmor = 20;
    }

    public CaptainRhogarTheWily(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy there, traveler! I be Captain Rhogar, a man of opportunities. Fancy makin' a deal with ol' Rhogar?");
        
        // Dialogue options
        greeting.AddOption("What kind of deal?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("If ye happen to have a SmugglersCrate, I can offer ye a choice of treasures. Either a HolidayPillow or an UncrackedGeode. Plus, I'll always throw in a MaxxiaScroll for good measure.");
                
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do ye have a SmugglersCrate for ol' Rhogar?");
                        tradeModule.AddOption("Yes, I have a SmugglersCrate.", 
                            plaa => HasSmugglersCrate(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasSmugglersCrate(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when ye've got a SmugglersCrate, matey!");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Ye can only trade once every 10 minutes. Be patient, matey!");
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
                
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Who are you really, Captain Rhogar?", 
            p => true, 
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("Ah, ye be curious 'bout me past, eh? Well, I wasn't always the roguish sailor ye see before ye. Once, I lived as a respectable town doctor—'Doctor Rhogar,' they called me. Cured the sick, healed the wounded, and mended broken souls. But I had darker ambitions.");
                
                backstoryModule.AddOption("Darker ambitions? What do you mean?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule ambitionModule = new DialogueModule("Aye, I wanted to understand the mind—what makes it tick, what breaks it, and how to piece it back together. I began to experiment... harmless at first. But soon, I delved deeper, testing limits that perhaps no man should. I did things I ain't proud of, and those patients... they still visit me, in my dreams.");
                        
                        ambitionModule.AddOption("You experimented on your patients?", 
                            pla => true, 
                            pla =>
                            {
                                DialogueModule experimentModule = new DialogueModule("Aye, lad. I pushed boundaries. Thought I could find cures for ailments of the mind, or even unlock hidden potential. But instead, I found madness. Some I healed, others... well, let's just say they ain't restin' easy. Their eyes haunt me still, accusin' and hollow.");
                                
                                experimentModule.AddOption("Do you regret what you've done?", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Every day, I regret it. I traded my honor as a healer for the secrets of the mind, and in the end, I lost both. The sea became my refuge—out here, the waves drown the whispers of the past. But sometimes, late at night, I still see 'em—faces twisted, eyes empty.");
                                        
                                        regretModule.AddOption("Why stay on the sea then?", 
                                            plaaa => true, 
                                            plaaa =>
                                            {
                                                DialogueModule seaModule = new DialogueModule("The sea... it offers freedom. The salt air, the endless horizon—no walls, no prying eyes. Out here, I'm just Captain Rhogar, not Doctor Rhogar the Betrayer. Besides, there are still people who need help, in their own way. A smuggler's life lets me do a bit of good, even if it's wrapped in shades of gray.");
                                                
                                                seaModule.AddOption("Do you help others, even now?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Aye, in my own way. When I trade, it's not always just for gold or gems. Sometimes, it's for medicines or supplies a town desperately needs—without askin' too many questions. I've learned there ain't no black or white in this world—just survival, and tryin' to leave it a little better than ye found it.");
                                                        
                                                        helpModule.AddOption("That's a noble way to look at it.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Captain Rhogar nods, a glimmer of warmth returning to his eyes. 'Aye, noble or foolish, I can't say. But we all have our ghosts to face.'");
                                                            });

                                                        helpModule.AddOption("Sounds like you're still haunted.", 
                                                            plaaaaa => true, 
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Captain Rhogar sighs deeply. 'Haunted, aye. Some ghosts never leave, no matter how far ye sail. But maybe that's a price I have to pay.'");
                                                            });
                                                        
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });
                                                
                                                seaModule.AddOption("Enough about the sea. What else do you trade?", 
                                                    plaaaa => true, 
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                
                                                plaaa.SendGump(new DialogueGump(plaaa, seaModule));
                                            });
                                        
                                        plaa.SendGump(new DialogueGump(plaa, regretModule));
                                    });
                                
                                experimentModule.AddOption("I think I'd rather not hear any more.", 
                                    plaa => true, 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Captain Rhogar gives a grim nod. 'Aye, some things are better left unsaid.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                
                                pla.SendGump(new DialogueGump(pla, experimentModule));
                            });
                        
                        ambitionModule.AddOption("That sounds terrifying.", 
                            pla => true, 
                            pla =>
                            {
                                pla.SendMessage("Captain Rhogar smiles sadly. 'Aye, terrifying indeed. And some terrors never let go.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, ambitionModule));
                    });
                
                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Captain Rhogar gives you a knowing nod.");
            });

        return greeting;
    }

    private bool HasSmugglersCrate(PlayerMobile player)
    {
        // Check the player's inventory for SmugglersCrate
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SmugglersCrate)) != null;
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
        // Remove the SmugglersCrate and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item smugglersCrate = player.Backpack.FindItemByType(typeof(SmugglersCrate));
        if (smugglersCrate != null)
        {
            smugglersCrate.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do ye choose?");
            
            // Add options for HolidayPillow and UncrackedGeode
            rewardChoiceModule.AddOption("HolidayPillow", pl => true, pl => 
            {
                pl.AddToBackpack(new HolidayPillow());
                pl.SendMessage("Ye receive a HolidayPillow!");
            });
            
            rewardChoiceModule.AddOption("UncrackedGeode", pl => true, pl =>
            {
                pl.AddToBackpack(new UncrackedGeode());
                pl.SendMessage("Ye receive an UncrackedGeode!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems ye no longer have a SmugglersCrate, matey.");
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