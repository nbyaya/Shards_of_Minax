using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CaptainSeabreeze : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CaptainSeabreeze() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Captain Seabreeze";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(70);
        SetInt(100);

        SetHits(120);
        SetMana(150);
        SetStam(70);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new TricorneHat(1150)); // Blue Tricorne Hat
        AddItem(new FancyShirt(136)); // Deep Blue Shirt
        AddItem(new LongPants(1109)); // Dark Pants
        AddItem(new ThighBoots(1)); // Black Boots
        AddItem(new Cutlass()); // Weapon for aesthetic

        VirtualArmor = 15;
    }

    public CaptainSeabreeze(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ahoy, traveler! I be Captain Seabreeze, master of the high seas. Do ye have an anchor to spare for an old sailor?");
        
        // Dialogue options
        greeting.AddOption("What do you need the anchor for?", 
            p => true, 
            p =>
            {
                DialogueModule anchorModule = new DialogueModule("An anchor, ye ask? It be for an important experiment to find the lost routes to the mythical Star Isles. If ye have a HeavyAnchor, I'd be willing to trade for it!");
                
                // Nested options about Star Isles
                anchorModule.AddOption("Tell me more about the Star Isles.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule starIslesModule = new DialogueModule("Ah, the Star Isles... A place shrouded in mystery, seen only in fleeting glimpses by sailors who dare venture far beyond the horizon. Some say they hold untold treasures, while others speak of dark secrets best left undisturbed.");
                        starIslesModule.AddOption("Untold treasures?", 
                            pll => true,
                            pll =>
                            {
                                DialogueModule treasuresModule = new DialogueModule("Aye, matey! The Star Isles are said to be filled with riches—gems that glow with moonlight, golden relics from ancient times, and secrets that could change the fate of any who find them. But beware, for not all treasures be of gold and jewels...");
                                treasuresModule.AddOption("What other secrets are there?", 
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule secretsModule = new DialogueModule("The kind of secrets that weigh heavy on a sailor's soul. Some say the Isles whisper to ye, showing visions of yer deepest desires and darkest fears. I meself have seen things I wish I could forget.");
                                        secretsModule.AddOption("Sounds dangerous. Why do you pursue it?", 
                                            pllll => true,
                                            pllll =>
                                            {
                                                DialogueModule pursuitModule = new DialogueModule("Dangerous, aye, but the call of the unknown is stronger. I be haunted by visions, whispers of a past I cannot escape. Perhaps, somewhere in those Isles, lies the key to freeing me from these chains.");
                                                pursuitModule.AddOption("I see. I'll help you if I can.",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        plllll.SendMessage("Captain Seabreeze looks at you with a glimmer of hope in his eyes.");
                                                        plllll.SendGump(new DialogueGump(plllll, anchorModule));
                                                    });
                                                pursuitModule.AddOption("This is too much for me.",
                                                    plllll => true,
                                                    plllll =>
                                                    {
                                                        plllll.SendMessage("Captain Seabreeze nods solemnly. 'Aye, not everyone is meant to sail these waters.'");
                                                    });
                                                pllll.SendGump(new DialogueGump(pllll, pursuitModule));
                                            });
                                        plll.SendGump(new DialogueGump(plll, secretsModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, treasuresModule));
                            });
                        
                        starIslesModule.AddOption("Dark secrets?", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule darkSecretsModule = new DialogueModule("Aye, dark secrets indeed. I heard tales of sailors who made it to the Isles, only to return as mere shells of their former selves. They speak in riddles, as if they saw something their minds could not fathom. Some say it be a curse, others say it be the truth of our very souls reflected back at us.");
                                darkSecretsModule.AddOption("That sounds terrifying.", 
                                    plll => true,
                                    plll =>
                                    {
                                        DialogueModule terrifyingModule = new DialogueModule("Terrifying, aye, but truth often is. The sea holds many mysteries, and not all of them are meant to be understood. I be driven to face that fear, to find answers—or perhaps, to find peace.");
                                        plll.SendGump(new DialogueGump(plll, terrifyingModule));
                                    });
                                pll.SendGump(new DialogueGump(pll, darkSecretsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, starIslesModule));
                    });

                // Trade option after story
                anchorModule.AddOption("What can you offer in exchange?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("If ye have a HeavyAnchor, ye can choose between a set of PunishmentStocks or a rare StarMap. And, I'll always throw in a MaxxiaScroll as a token of gratitude.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                            pla => CanTradeWithPlayer(pla), 
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do ye have the HeavyAnchor for me?");
                                tradeModule.AddOption("Yes, I have a HeavyAnchor.", 
                                    plaa => HasHeavyAnchor(plaa) && CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.", 
                                    plaa => !HasHeavyAnchor(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when ye have a HeavyAnchor, matey!");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.", 
                                    plaa => !CanTradeWithPlayer(plaa), 
                                    plaa =>
                                    {
                                        plaa.SendMessage("Ye can only trade once every 10 minutes. Come back when the tide changes, aye?");
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

                p.SendGump(new DialogueGump(p, anchorModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Captain Seabreeze nods and tips his hat.");
            });

        return greeting;
    }

    private bool HasHeavyAnchor(PlayerMobile player)
    {
        // Check the player's inventory for HeavyAnchor
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(HeavyAnchor)) != null;
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
        // Remove the HeavyAnchor and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item heavyAnchor = player.Backpack.FindItemByType(typeof(HeavyAnchor));
        if (heavyAnchor != null)
        {
            heavyAnchor.Delete();
            
            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do ye choose, matey?");
            
            // Add options for PunishmentStocks and StarMap
            rewardChoiceModule.AddOption("PunishmentStocks", pl => true, pl => 
            {
                pl.AddToBackpack(new PunishmentStocks());
                pl.SendMessage("Ye receive a set of PunishmentStocks!");
            });
            
            rewardChoiceModule.AddOption("StarMap", pl => true, pl =>
            {
                pl.AddToBackpack(new StarMap());
                pl.SendMessage("Ye receive a StarMap, a guide to the mythical Star Isles!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems ye no longer have a HeavyAnchor, matey.");
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