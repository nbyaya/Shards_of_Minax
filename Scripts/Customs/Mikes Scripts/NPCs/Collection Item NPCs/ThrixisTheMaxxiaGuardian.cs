using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThrixisTheMaxxiaGuardian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThrixisTheMaxxiaGuardian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thrixis, Guardian of Maxxia";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 1500;
        Karma = 1000;

        // Outfit
        AddItem(new HoodedShroudOfShadows(2301)); // Dark gray mystical shroud
        AddItem(new Sandals(2413)); // Dark red sandals
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new Doublet(2413)); // Dark red doublet
        AddItem(new StaffOfPower()); // A custom staff in his inventory (could be stolen)

        VirtualArmor = 15;
    }

    public ThrixisTheMaxxiaGuardian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Thrixis, Guardian of the ancient power of Maxxia. What brings you here?");
        
        // Dialogue options
        greeting.AddOption("What is the ancient power of Maxxia?", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("Maxxia's power is a long-lost magic, capable of wonders beyond imagination. However, it is not without its price. I guard the knowledge and relics of this power, sharing only with those who prove themselves worthy.");
                
                // Trade option after story
                storyModule.AddOption("Do you need anything for your cause?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am seeking MaxxiaDust, an elusive substance of great potency. Should you have some, I will offer you a choice between two relics: the TrexSkull or the RootThrone.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have the MaxxiaDust for me?");
                                tradeModule.AddOption("Yes, I have MaxxiaDust.", 
                                    plaa => HasMaxxiaDust(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have any right now.", 
                                    plaa => !HasMaxxiaDust(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have acquired some MaxxiaDust.");
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

                // Nested dialogue related to the Mute Child
                storyModule.AddOption("I heard of a strange child in these parts.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule childModule = new DialogueModule("Ah, you must be speaking of the mute child, a mysterious presence that roams these lands. She lost her voice after witnessing an ancient ritual, and since then, she has not uttered a word.");

                        childModule.AddOption("What happened during the ritual?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule ritualModule = new DialogueModule("No one knows the full story. Some say she saw things that a child should never witness—an ancient incantation gone wrong, or perhaps a summoning. Whatever she saw, it stripped her of her voice and left her with an eerie, silent gaze.");
                                
                                ritualModule.AddOption("Where can I find her?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule locationModule = new DialogueModule("She is often found drawing strange symbols in the dirt, symbols that few can understand. Her presence is unsettling, yet she seems to be searching for something—perhaps solace, perhaps something lost.");
                                        
                                        locationModule.AddOption("What do the symbols mean?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule symbolsModule = new DialogueModule("The symbols are old, older than even the legends of Maxxia. Some believe they are a map, others a warning. If you find the child, perhaps you could try to understand her drawings. But be warned—looking into her eyes might leave you with more questions than answers.");
                                                
                                                symbolsModule.AddOption("Can I help her?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Helping her is not simple. She does not speak, and she shies away from strangers. But she is observant—perhaps if you offer her something meaningful, she might respond. A relic of Maxxia, or something that belonged to her past, might draw her interest.");
                                                        
                                                        helpModule.AddOption("I will try to find something for her.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Thrixis nods solemnly. 'Be careful, traveler. The child's silence hides much.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });

                                                        plaaaa.SendGump(new DialogueGump(plaaaa, helpModule));
                                                    });

                                                plaaa.SendGump(new DialogueGump(plaaa, symbolsModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, locationModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, ritualModule));
                            });

                        pl.SendGump(new DialogueGump(pl, childModule));
                    });

                p.SendGump(new DialogueGump(p, storyModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thrixis nods in understanding, his eyes glinting with ancient knowledge.");
            });

        return greeting;
    }

    private bool HasMaxxiaDust(PlayerMobile player)
    {
        // Check the player's inventory for MaxxiaDust
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MaxxiaDust)) != null;
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
        // Remove the MaxxiaDust and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item maxxiaDust = player.Backpack.FindItemByType(typeof(MaxxiaDust));
        if (maxxiaDust != null)
        {
            maxxiaDust.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which relic do you choose?");
            
            // Add options for TrexSkull and RootThrone
            rewardChoiceModule.AddOption("TrexSkull", pl => true, pl => 
            {
                pl.AddToBackpack(new TrexSkull());
                pl.SendMessage("You receive the TrexSkull!");
            });
            
            rewardChoiceModule.AddOption("RootThrone", pl => true, pl =>
            {
                pl.AddToBackpack(new RootThrone());
                pl.SendMessage("You receive the RootThrone!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have MaxxiaDust.");
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