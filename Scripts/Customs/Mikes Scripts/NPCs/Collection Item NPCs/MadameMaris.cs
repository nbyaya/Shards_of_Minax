using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MadameMaris : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MadameMaris() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Madame Maris, the Magical Dealer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(2658)); // A mystical blue dress
        AddItem(new Sandals(1150)); // Silver sandals
        AddItem(new WideBrimHat(1282)); // A wide-brimmed hat with a mystical aura
        AddItem(new GoldBracelet()); // Elegant golden bracelet
        AddItem(new Lantern()); // Always holding a magical lantern

        VirtualArmor = 20;
    }

    public MadameMaris(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Madame Maris, a collector of magical artifacts and rare wonders. Are you interested in a little magical exchange?");
        
        // Dialogue options
        greeting.AddOption("What sort of exchange are you offering?", 
            p => true, 
            p =>
            {
                DialogueModule exchangeModule = new DialogueModule("I am particularly interested in a DeckOfMagicCards. If you have one, I can offer you a choice between a BrokenBottle or a FancyHornOfPlenty, along with a special scroll of mine.");
                
                // Trade option
                exchangeModule.AddOption("I'd like to make the trade.", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a DeckOfMagicCards for me?");
                        tradeModule.AddOption("Yes, I have a DeckOfMagicCards.", 
                            plaa => HasDeckOfMagicCards(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasDeckOfMagicCards(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a DeckOfMagicCards.");
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
                
                // Additional nested dialogue for more depth
                exchangeModule.AddOption("Who are you really, Madame Maris?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule identityModule = new DialogueModule("Ah, you've touched upon a question that few dare ask. I wasn't always a mere dealer of magical trinkets. Once, I was known as Maris the Vile, the leader of the 'Shadow Serpents' - the underworld's most ruthless gang.");
                        identityModule.AddOption("Tell me more about the 'Shadow Serpents'.", 
                            pll => true, 
                            pll =>
                            {
                                DialogueModule shadowSerpentsModule = new DialogueModule("The 'Shadow Serpents' held a tight grip on the black market, enforcing our will through fear and cunning. My influence stretched from the darkest alleys to the highest nobility who sought the forbidden. We were ruthless, yes, but also charismatic - people followed us out of both fear and admiration.");
                                shadowSerpentsModule.AddOption("How did you go from gang leader to magical dealer?", 
                                    plla => true, 
                                    plla =>
                                    {
                                        DialogueModule transitionModule = new DialogueModule("Power attracts enemies, and the rivals grew too numerous. I was betrayed by one of my most trusted lieutenants, who sought to take my place. Rather than succumb to their trap, I vanished into the ether, leaving behind my old life and my name. Now, I deal in magic - a more subtle power.");
                                        transitionModule.AddOption("Do you still have connections to the underworld?", 
                                            pllb => true, 
                                            pllb =>
                                            {
                                                DialogueModule underworldConnectionModule = new DialogueModule("The underworld never truly lets go of those who know its secrets. I still have contacts - whispers in the dark, hands that reach where the law dares not tread. Sometimes, these hands bring me valuable artifacts. If you cross me, you'll find that the shadows still answer to me.");
                                                underworldConnectionModule.AddOption("I wouldn't dare cross you.", 
                                                    pllc => true, 
                                                    pllc =>
                                                    {
                                                        pllc.SendMessage("Wise choice, traveler. Not everyone understands the value of caution.");
                                                        pllc.SendGump(new DialogueGump(pllc, CreateGreetingModule(pllc)));
                                                    });
                                                underworldConnectionModule.AddOption("I'm not afraid of you.", 
                                                    pllc => true, 
                                                    pllc =>
                                                    {
                                                        pllc.SendMessage("Bold words, but the shadows have a way of swallowing those who speak too loudly. Watch yourself, traveler.");
                                                        pllc.SendGump(new DialogueGump(pllc, CreateGreetingModule(pllc)));
                                                    });
                                                pllb.SendGump(new DialogueGump(pllb, underworldConnectionModule));
                                            });
                                        
                                        transitionModule.AddOption("It sounds like you've led an interesting life.", 
                                            pllb => true, 
                                            pllb =>
                                            {
                                                pllb.SendMessage("Interesting, dangerous, and full of regrets. But here I am, still breathing. That's more than some can say.");
                                                pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule(pllb)));
                                            });
                                        plla.SendGump(new DialogueGump(plla, transitionModule));
                                    });
                                
                                shadowSerpentsModule.AddOption("What happened to your gang?", 
                                    plla => true, 
                                    plla =>
                                    {
                                        DialogueModule gangFateModule = new DialogueModule("The 'Shadow Serpents' fractured without my leadership. My former lieutenant tried to lead, but he lacked the cunning. Soon, our rivals closed in, and what was once an empire became scattered factions fighting over scraps. Such is the fate of those who lack vision.");
                                        gangFateModule.AddOption("That's unfortunate.", 
                                            pllb => true, 
                                            pllb =>
                                            {
                                                pllb.SendMessage("Unfortunate for them, but necessary for my survival. Empires fall so that others may rise.");
                                                pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule(pllb)));
                                            });
                                        gangFateModule.AddOption("Serves them right for betraying you.", 
                                            pllb => true, 
                                            pllb =>
                                            {
                                                pllb.SendMessage("Indeed. Betrayal comes at a high cost, and they paid it in full.");
                                                pllb.SendGump(new DialogueGump(pllb, CreateGreetingModule(pllb)));
                                            });
                                        plla.SendGump(new DialogueGump(plla, gangFateModule));
                                    });
                                
                                pll.SendGump(new DialogueGump(pll, shadowSerpentsModule));
                            });
                        identityModule.AddOption("It must have been difficult to leave all that power behind.", 
                            pll => true, 
                            pll =>
                            {
                                pll.SendMessage("Power is a dangerous thing, traveler. It consumes those who wield it and those who seek it. I traded one form of power for another - one that keeps me in the shadows, but alive.");
                                pll.SendGump(new DialogueGump(pll, CreateGreetingModule(pll)));
                            });
                        pl.SendGump(new DialogueGump(pl, identityModule));
                    });
                
                exchangeModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, exchangeModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Madame Maris gives you a mysterious smile as you turn away.");
            });

        return greeting;
    }

    private bool HasDeckOfMagicCards(PlayerMobile player)
    {
        // Check the player's inventory for DeckOfMagicCards
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DeckOfMagicCards)) != null;
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
        // Remove the DeckOfMagicCards and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item deckOfMagicCards = player.Backpack.FindItemByType(typeof(DeckOfMagicCards));
        if (deckOfMagicCards != null)
        {
            deckOfMagicCards.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");
            
            // Add options for BrokenBottle and FancyHornOfPlenty
            rewardChoiceModule.AddOption("BrokenBottle", pl => true, pl => 
            {
                pl.AddToBackpack(new BrokenBottle());
                pl.SendMessage("You receive a BrokenBottle!");
            });
            
            rewardChoiceModule.AddOption("FancyHornOfPlenty", pl => true, pl =>
            {
                pl.AddToBackpack(new FancyHornOfPlenty());
                pl.SendMessage("You receive a FancyHornOfPlenty!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a DeckOfMagicCards.");
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