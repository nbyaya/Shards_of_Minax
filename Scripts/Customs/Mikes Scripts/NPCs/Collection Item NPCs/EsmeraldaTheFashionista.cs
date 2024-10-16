using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EsmeraldaTheFashionista : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EsmeraldaTheFashionista() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Esmeralda the Fashionista";
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
        AddItem(new FeatheredHat(2753)); // Bright blue feathered hat
        AddItem(new FancyDress(2727)); // Stylish red dress
        AddItem(new Sandals(136)); // Elegant sandals
        AddItem(new GoldEarrings()); // Gold earrings
        AddItem(new GoldBracelet()); // Gold bracelet

        VirtualArmor = 10;
    }

    public EsmeraldaTheFashionista(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello, darling! I'm Esmeralda, the fashionista of this realm. You look like someone with a keen eye for style. Might I interest you in a little trade?");
        
        // Dialogue options
        greeting.AddOption("What kind of trade are you talking about?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroModule = new DialogueModule("I am in search of a particular item—a DressForm! If you happen to have one, I can offer you a choice between a LargeWeatheredBook or a WeddingCandelabra. Plus, I always add a little something extra for my fashionable friends.");
                
                // Nested dialogues
                tradeIntroModule.AddOption("Tell me more about why you need a DressForm.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule dressFormExplanation = new DialogueModule("Ah, the DressForm! It is a key part of my creative process, darling. You see, each DressForm holds the essence of the garments that were once adorned on them—energy that I can harness for my own fashionable creations! There's something magical about fashion, wouldn't you agree?");
                        dressFormExplanation.AddOption("That's fascinating! I do have a DressForm.",
                            pla => HasDressForm(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        dressFormExplanation.AddOption("I don't have one right now.",
                            pla => !HasDressForm(pla),
                            pla =>
                            {
                                pla.SendMessage("Oh, how disappointing! Come back when you have a DressForm, darling.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        dressFormExplanation.AddOption("Why is fashion so important to you?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule fashionImportance = new DialogueModule("Fashion, my dear, is an expression of one's soul. I grew up in a world devoid of color, and clothing was my way of painting the world around me. Now, I help others express themselves in a way they never thought possible.");
                                fashionImportance.AddOption("That's truly inspiring. Let's talk about the trade.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, tradeIntroModule));
                                    });
                                fashionImportance.AddOption("I need some time to think about this.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendMessage("Take all the time you need, darling. Inspiration waits for no one, but I do!");
                                    });
                                pla.SendGump(new DialogueGump(pla, fashionImportance));
                            });
                        pl.SendGump(new DialogueGump(pl, dressFormExplanation));
                    });
                
                // Trade option after story
                tradeIntroModule.AddOption("I have a DressForm. Let's make a deal!", 
                    pl => CanTradeWithPlayer(pl), 
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Wonderful! Do you have the DressForm with you?");
                        tradeModule.AddOption("Yes, here it is.", 
                            plaa => HasDressForm(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.", 
                            plaa => !HasDressForm(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Oh, how disappointing! Come back when you have a DressForm, darling.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.", 
                            plaa => !CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Patience is key to great style. You can only trade once every 10 minutes. Come back later, dear!");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });
                
                tradeIntroModule.AddOption("Maybe another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                
                p.SendGump(new DialogueGump(p, tradeIntroModule));
            });

        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Esmeralda waves at you with a graceful smile.");
            });

        return greeting;
    }

    private bool HasDressForm(PlayerMobile player)
    {
        // Check the player's inventory for DressForm
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DressForm)) != null;
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
        // Remove the DressForm and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item dressForm = player.Backpack.FindItemByType(typeof(DressForm));
        if (dressForm != null)
        {
            dressForm.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward would you like, darling?");

            // Add options for LargeWeatheredBook and WeddingCandelabra
            rewardChoiceModule.AddOption("LargeWeatheredBook", pl => true, pl => 
            {
                pl.AddToBackpack(new LargeWeatheredBook());
                pl.SendMessage("You receive a LargeWeatheredBook! How charming!");
            });

            rewardChoiceModule.AddOption("WeddingCandelabra", pl => true, pl =>
            {
                pl.AddToBackpack(new WeddingCandelabra());
                pl.SendMessage("You receive a WeddingCandelabra! Simply exquisite!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a DressForm. Oh dear!");
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