using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class FinnianTheTreasureSeeker : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FinnianTheTreasureSeeker() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Finnian the Treasure Seeker";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(80);
        SetInt(90);

        SetHits(100);
        SetMana(100);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1150)); // Bright blue shirt
        AddItem(new LongPants(1109)); // Dark green pants
        AddItem(new Bandana(1153)); // A yellow bandana
        AddItem(new Boots(1107)); // Brown boots
        AddItem(new Backpack());

        VirtualArmor = 15;
    }

    public FinnianTheTreasureSeeker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow adventurer! I am Finnian, a seeker of hidden treasures. Tell me, do you fancy yourself a treasure hunter as well?");
        
        // Dialogue options
        greeting.AddOption("What kind of treasures do you seek?", 
            p => true, 
            p =>
            {
                DialogueModule treasuresModule = new DialogueModule("I seek all manner of rare artifacts and curious items, but recently I've been on the lookout for something very specific. Say, do you have a HildebrandtShield?");
                
                // Nested dialogue about treasures
                treasuresModule.AddOption("Why the HildebrandtShield? What's so special about it?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule obsessionModule = new DialogueModule("Ah, the HildebrandtShield... it's not just any shield, you see. It's said to carry the echoes of a battle long forgotten, one where the warriors' spirits still linger. I must have it for my collection! You wouldn't understand the beauty of such a rarity, the thrill of possessing something others dare not touch.");
                        
                        obsessionModule.AddOption("You seem quite obsessed. Why do you need these rare items?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule obsessionDetailModule = new DialogueModule("Obsessed? Perhaps... But isn't everyone obsessed with something? I collect these artifacts because they speak to me. They tell stories, and I am their keeper. Each item has a voice, and together they form a symphony of history. The HildebrandtShield... oh, it will complete my masterpiece, a collection unmatched by any other!");
                                
                                obsessionDetailModule.AddOption("What if someone else wants the shield?", 
                                    plll => true, 
                                    plll =>
                                    {
                                        DialogueModule paranoiaModule = new DialogueModule("Someone else? No, no, no! No one else must have it! I know there are others who covet what I seek. They are always watching, waiting to snatch it from my grasp. That's why I must act swiftly and decisively. You see, you must trust no one when it comes to treasures like these!");
                                        
                                        paranoiaModule.AddOption("Sounds dangerous. Have you had trouble with others before?", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                DialogueModule troubleModule = new DialogueModule("Oh, you have no idea. There have been... incidents. People who tried to deceive me, to take what is rightfully mine. I had to... take care of them. No one crosses Finnian and gets away with it. The treasures demand loyalty, and I am their guardian. But enough of that, do you have the HildebrandtShield?");
                                                
                                                troubleModule.AddOption("I think I'll keep my distance...", 
                                                    plllll => true, 
                                                    plllll =>
                                                    {
                                                        plllll.SendMessage("Finnian narrows his eyes, muttering something about 'cowards and thieves' as you step away.");
                                                    });

                                                troubleModule.AddOption("I have the HildebrandtShield. What can you offer in return?", 
                                                    plllll => HasHildebrandtShield(plllll) && CanTradeWithPlayer(plllll), 
                                                    plllll =>
                                                    {
                                                        CompleteTrade(plllll);
                                                    });
                                                
                                                pllll.SendGump(new DialogueGump(pllll, troubleModule));
                                            });
                                        
                                        paranoiaModule.AddOption("You're right, you can't be too careful.", 
                                            pllll => true, 
                                            pllll =>
                                            {
                                                pllll.SendMessage("Finnian nods vigorously, clearly pleased by your understanding.");
                                                pllll.SendGump(new DialogueGump(pllll, treasuresModule));
                                            });
                                        
                                        plll.SendGump(new DialogueGump(plll, paranoiaModule));
                                    });
                                
                                pll.SendGump(new DialogueGump(pll, obsessionDetailModule));
                            });
                        
                        pl.SendGump(new DialogueGump(pl, obsessionModule));
                    });
                
                // Trade option after story
                treasuresModule.AddOption("I have a HildebrandtShield. What can you offer in return?", 
                    pl => HasHildebrandtShield(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                treasuresModule.AddOption("No, I don't have one right now.", 
                    pl => !HasHildebrandtShield(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back when you have a HildebrandtShield. There's a special reward waiting!");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                treasuresModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, treasuresModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Finnian gives you a nod and returns to examining his map, muttering something about 'missed opportunities.'");
            });

        return greeting;
    }

    private bool HasHildebrandtShield(PlayerMobile player)
    {
        // Check the player's inventory for HildebrandtShield
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HildebrandtShield)) != null;
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
        // Remove the HildebrandtShield and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item hildebrandtShield = player.Backpack.FindItemByType(typeof(HildebrandtShield));
        if (hildebrandtShield != null)
        {
            hildebrandtShield.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for FeedingTrough and UncrackedGeode
            rewardChoiceModule.AddOption("FeedingTrough", pl => true, pl => 
            {
                pl.AddToBackpack(new FeedingTrough());
                pl.SendMessage("You receive a FeedingTrough!");
            });
            
            rewardChoiceModule.AddOption("UncrackedGeode", pl => true, pl =>
            {
                pl.AddToBackpack(new UncrackedGeode());
                pl.SendMessage("You receive an UncrackedGeode!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a HildebrandtShield.");
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