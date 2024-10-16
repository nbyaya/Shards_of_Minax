using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ThaldriaTheGourmetTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ThaldriaTheGourmetTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Thaldria the Gourmet Trader";
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
        AddItem(new FancyDress(2129)); // Vibrant red dress
        AddItem(new Sandals(1175)); // Golden sandals
        AddItem(new FeatheredHat(1359)); // A hat adorned with a colorful feather
        AddItem(new GoldEarrings());
        AddItem(new GoldBracelet());

        VirtualArmor = 10;
    }

    public ThaldriaTheGourmetTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a fellow connoisseur of fine delicacies! I am Thaldria, seeker of rare and exotic flavors. Tell me, have you come across anything truly extraordinary on your travels?");
        
        // Dialogue options
        greeting.AddOption("What kind of delicacies are you looking for?", 
            p => true, 
            p =>
            {
                DialogueModule delicaciesModule = new DialogueModule("I am always on the lookout for unique and rare ingredients. Right now, I am particularly interested in a RareSausage. If you have one, I could offer you something special in return.");
                
                // Add additional nested dialogue for more context
                delicaciesModule.AddOption("Why are you interested in RareSausage?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Ah, the RareSausage is unlike any other. It contains flavors lost to time, crafted by the tribal Warchiefs of old to honor their ancestors. It is both a delicacy and a symbol of resilience. I use it to create dishes that bring people together, reminding them of our past while offering them hope for the future.");
                        reasonModule.AddOption("Tell me more about the Warchiefs.", 
                            p2 => true, 
                            p2 =>
                            {
                                DialogueModule warchiefModule = new DialogueModule("The Warchiefs were the leaders of small, proud tribes. They were wise, gruff, and stoic, just like the land they lived upon. They balanced tradition with the need for survival in the harsh wasteland. To their people, they were both respected and feared, and their wisdom was passed down through generations, often shared during great feasts. The RareSausage was central to these gatherings, symbolizing unity amidst struggle.");
                                
                                warchiefModule.AddOption("It sounds like you respect them a lot.", 
                                    p3 => true, 
                                    p3 =>
                                    {
                                        DialogueModule respectModule = new DialogueModule("Indeed, I do. The Warchiefs held a strength that is rare in these times. They were not only warriors but leaders who knew the value of community, the value of sacrifice for the greater good. My people, too, understand that surviving the wasteland is not just about strength of arms, but also about the strength of the spirit.");
                                        respectModule.AddOption("Do you lead a group as well?", 
                                            p4 => true, 
                                            p4 =>
                                            {
                                                DialogueModule leaderModule = new DialogueModule("I do, in my own way. I lead a group of traders and wanderers, all of whom share a passion for the exotic and the unique. We may not wield weapons like the Warchiefs, but we bring people together with the power of our offerings. To trade, to barter, to share a story over a meal—this is how we survive, and this is how we grow stronger.");
                                                leaderModule.AddOption("I admire your dedication.", 
                                                    p5 => true, 
                                                    p5 =>
                                                    {
                                                        p5.SendMessage("Thaldria nods, her eyes glinting with appreciation. 'Thank you, traveler. Dedication is what keeps us all moving forward, even in the toughest of times.'");
                                                        p5.SendGump(new DialogueGump(p5, CreateGreetingModule(p5)));
                                                    });
                                                p4.SendGump(new DialogueGump(p4, leaderModule));
                                            });
                                        p3.SendGump(new DialogueGump(p3, respectModule));
                                    });
                                p2.SendGump(new DialogueGump(p2, warchiefModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });

                // Trade option after story
                delicaciesModule.AddOption("I have a RareSausage. What can you offer?", 
                    pl => HasRareSausage(pl) && CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                delicaciesModule.AddOption("I don't have anything like that.", 
                    pl => !HasRareSausage(pl), 
                    pl =>
                    {
                        pl.SendMessage("Come back if you find a RareSausage. I promise it will be worth your while!");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                delicaciesModule.AddOption("I traded recently; I'll come back later.", 
                    pl => !CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                p.SendGump(new DialogueGump(p, delicaciesModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Thaldria waves goodbye, her eyes still searching for the next rare treat.");
            });

        return greeting;
    }

    private bool HasRareSausage(PlayerMobile player)
    {
        // Check the player's inventory for RareSausage
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(RareSausage)) != null;
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
        // Remove the RareSausage and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item rareSausage = player.Backpack.FindItemByType(typeof(RareSausage));
        if (rareSausage != null)
        {
            rareSausage.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for SheepCarcass and GrandmasSpecialRolls
            rewardChoiceModule.AddOption("SheepCarcass", pl => true, pl => 
            {
                pl.AddToBackpack(new SheepCarcass());
                pl.SendMessage("You receive a SheepCarcass!");
            });
            
            rewardChoiceModule.AddOption("GrandmasSpecialRolls", pl => true, pl =>
            {
                pl.AddToBackpack(new GrandmasSpecialRolls());
                pl.SendMessage("You receive Grandma's Special Rolls!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a RareSausage.");
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