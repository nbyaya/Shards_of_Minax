using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class EldricTheWardenOfFlames : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EldricTheWardenOfFlames() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Eldric the Warden of Flames";
        Body = 0x190; // Human male body
        Hue = 33770; // Fiery red hue

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1359)); // Fiery red robe
        AddItem(new Sandals(1161)); // Dark sandals
        AddItem(new LeatherGloves()); // Special gloves for a unique look
        AddItem(new SkullCap(1358)); // Fiery red skullcap

        VirtualArmor = 15;
    }

    public EldricTheWardenOfFlames(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, a traveler braving the flames! I am Eldric, guardian of forgotten embers. Do you carry the spirit of adventure within you?");
        
        // Dialogue options
        greeting.AddOption("Who are you, exactly?", 
            p => true, 
            p =>
            {
                DialogueModule storyModule = new DialogueModule("I am the Warden of Flames, the keeper of the sacred fires that protect the old cities. My task is to find worthy adventurers willing to keep these flames alive. But the truth is... I wasn't always like this.");
                
                // Deeper backstory option
                storyModule.AddOption("Tell me more about your past.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("Once, I was known as Eldric the Archivist. I hoarded knowledge, books, scrolls—anything that could preserve the history and wisdom of the old world. But when the flames consumed our civilization, I found myself with only embers of the past. I became obsessed with preserving what little was left.");
                        
                        pastModule.AddOption("Why did you hoard knowledge?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("Knowledge was power, and I feared losing it. I locked myself away in my grand library, pouring over every text, every scrap of parchment I could find. I thought that if I could just collect enough, I could prevent the world from forgetting. But it wasn't enough. The world fell apart despite my efforts.");
                                
                                obsessionModule.AddOption("Did you ever regret it?",
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? Every day. I wonder if I should have done more, if I should have shared my knowledge instead of hiding it. But then again, would anyone have listened? People were too busy with their own lives, too ignorant of the fragile balance. I was reclusive, but now I try to help in my own way, finding those like you who might understand.");
                                        
                                        regretModule.AddOption("I understand. What can I do to help?",
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule tradeIntroductionModule = new DialogueModule("I need a BrandingIron, a rare item that contains the essence of flame. If you have one, I can offer you a CityBanner or a set of Shears, whichever you prefer, along with a special scroll.");
                                                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                                    plaaaa => CanTradeWithPlayer(plaaaa),
                                                    plaaaa =>
                                                    {
                                                        DialogueModule tradeModule = new DialogueModule("Do you have a BrandingIron for me?");
                                                        tradeModule.AddOption("Yes, I have a BrandingIron.",
                                                            plaaaaa => HasBrandingIron(plaaaaa) && CanTradeWithPlayer(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                CompleteTrade(plaaaaa);
                                                            });
                                                        tradeModule.AddOption("No, I don't have one right now.",
                                                            plaaaaa => !HasBrandingIron(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Return when you have a BrandingIron.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                                            plaaaaa => !CanTradeWithPlayer(plaaaaa),
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, tradeModule));
                                                    });
                                                tradeIntroductionModule.AddOption("Maybe another time.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                pllll.SendGump(new DialogueGump(pllll, tradeIntroductionModule));
                                            });

                                        regretModule.AddOption("Goodbye.",
                                            pllll => true,
                                            pllll =>
                                            {
                                                pllll.SendMessage("Eldric nods, his eyes filled with both sadness and determination.");
                                            });
                                        plll.SendGump(new DialogueGump(plll, regretModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, obsessionModule));
                            });
                        
                        pastModule.AddOption("What happened to your library?",
                            pll => true,
                            pll =>
                            {
                                DialogueModule libraryModule = new DialogueModule("My library... it crumbled as the old world fell. The tomes were scattered, and the scrolls turned to ash. I tried to save what I could, but the flames were relentless. Now, I live among ruins, searching for those who still value knowledge. I keep these flames alive so that, perhaps, the world can be rebuilt one day.");
                                
                                libraryModule.AddOption("I wish I could have seen it.",
                                    plll => true,
                                    plll =>
                                    {
                                        plll.SendMessage("Eldric's eyes gleam with a mixture of pride and sorrow. 'It was magnificent. Perhaps one day, we will build another.'");
                                    });
                                pll.SendGump(new DialogueGump(pll, libraryModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });

                // Trade option after story
                storyModule.AddOption("What help do you need?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I need a BrandingIron, a rare item that contains the essence of flame. If you have one, I can offer you a CityBanner or a set of Shears, whichever you prefer, along with a special scroll.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a BrandingIron for me?");
                                tradeModule.AddOption("Yes, I have a BrandingIron.", 
                                    plaa => HasBrandingIron(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasBrandingIron(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Return when you have a BrandingIron.");
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
                p.SendMessage("Eldric nods knowingly, the flames in his eyes dimming slightly.");
            });

        return greeting;
    }

    private bool HasBrandingIron(PlayerMobile player)
    {
        // Check the player's inventory for BrandingIron
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BrandingIron)) != null;
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
        // Remove the BrandingIron and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item brandingIron = player.Backpack.FindItemByType(typeof(BrandingIron));
        if (brandingIron != null)
        {
            brandingIron.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for CityBanner and Shears
            rewardChoiceModule.AddOption("CityBanner", pl => true, pl => 
            {
                pl.AddToBackpack(new CityBanner());
                pl.SendMessage("You receive a CityBanner!");
            });
            
            rewardChoiceModule.AddOption("Shears", pl => true, pl =>
            {
                pl.AddToBackpack(new Shears());
                pl.SendMessage("You receive a set of Shears!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a BrandingIron.");
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