using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TheodricTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TheodricTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Theodric the Tinkerer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1150)); // Shirt with a blue hue
        AddItem(new LongPants(1109)); // Pants with a dark grey hue
        AddItem(new Boots(1175)); // Boots with a light brown hue
        AddItem(new LeatherGloves()); // Unique goggles for a tinkerer look
        AddItem(new GlassFurnace()); // A decorative toolbox on his belt

        VirtualArmor = 15;
    }

    public TheodricTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Theodric, a tinkerer by trade. Are you interested in a bit of gadgetry or perhaps a fair exchange?");

        // Start with dialogue about his work
        greeting.AddOption("What kind of gadgets do you work on?",
            p => true,
            p =>
            {
                DialogueModule gadgetsModule = new DialogueModule("I tinker with all sorts of things: clockwork creations, wind-up contraptions, and delicate wiring. My latest fascination is with Fine Copper Wire, an essential part of many ingenious devices. Would you like to help me in my pursuit?");

                gadgetsModule.AddOption("What do you need the Fine Copper Wire for?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule copperWireModule = new DialogueModule("Ah, Fine Copper Wire is crucial for my latest invention—a self-heating teapot! But enough about that... if you bring me some, I have something in exchange for you.");
                        copperWireModule.AddOption("What would you give me in return?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("I can offer you a Glass Furnace for your efforts, as well as a special Maxxia Scroll. However, I can only do this trade once every 10 minutes, as my supplies are limited.");
                                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                                    plaa => CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        DialogueModule tradeModule = new DialogueModule("Do you have a Fine Copper Wire for me?");
                                        tradeModule.AddOption("Yes, I have a Fine Copper Wire.",
                                            plaaa => HasFineCopperWire(plaaa) && CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                CompleteTrade(plaaa);
                                            });
                                        tradeModule.AddOption("No, I don't have one right now.",
                                            plaaa => !HasFineCopperWire(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Come back when you have a Fine Copper Wire.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        tradeModule.AddOption("I traded recently; I'll come back later.",
                                            plaaa => !CanTradeWithPlayer(plaaa),
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, tradeModule));
                                    });
                                tradeIntroductionModule.AddOption("Perhaps another time.",
                                    plaq => true,
                                    plaq =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, copperWireModule));
                    });
                
                gadgetsModule.AddOption("Have you always been a tinkerer?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("No, not always. I used to be a wanderer, trying to survive in a harsh world. Life hasn't been easy for me. I've fought battles, lost friends, and endured countless hardships. I learned to tinker as a way to bring some light into the darkness, to find hope among the broken pieces. Would you like to hear more about my past?");
                        backstoryModule.AddOption("Tell me about the hardships you've faced.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule hardshipsModule = new DialogueModule("Ah, the scars I carry are not just on my skin but on my soul. I have faced monsters in the night, watched my friends fall to betrayal, and had to make choices that haunt me. I have always fought for my friends, but the world has a way of testing one's loyalty. Still, I have never given up on those I care for.");
                                hardshipsModule.AddOption("That sounds incredibly tough. How did you endure it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule enduranceModule = new DialogueModule("Endurance comes from hope, and hope is something I have clung to despite everything. I believe in a better tomorrow. I believe that with enough tinkering, enough creativity, we can make the world a little less cruel. Every gadget I make, every invention, is a step toward that future. And I will fight for it until my last breath.");
                                        enduranceModule.AddOption("You're an inspiration, Theodric.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Theodric smiles, a glimmer of warmth in his eyes. 'Thank you, traveler. We all need to find our way to keep going.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        enduranceModule.AddOption("I can see why you're wary of others.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Theodric nods solemnly. 'Indeed. Trust is earned, not given lightly. I've learned that the hard way.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, enduranceModule));
                                    });
                                hardshipsModule.AddOption("I'm sorry to hear that. It must be difficult.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Theodric sighs, his eyes distant. 'It is. But we endure, don't we? Because we must.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, hardshipsModule));
                            });
                        backstoryModule.AddOption("I'd rather not dwell on sad stories.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Theodric nods. 'I understand. Not everyone likes to hear about the darkness.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });
                
                gadgetsModule.AddOption("What motivates you to keep going?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule motivationModule = new DialogueModule("Hope, traveler. Hope for a better world. I have lost many friends, and I have seen enough suffering to last several lifetimes. But I keep going because I know there's still something worth fighting for. There are still people who need someone to fix what is broken, to build what has been lost.");
                        motivationModule.AddOption("That's truly admirable.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Theodric smiles faintly. 'Admirable or foolish, perhaps both. But it is who I am.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        motivationModule.AddOption("Do you think we'll ever see a better world?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule betterWorldModule = new DialogueModule("I believe we will, someday. Not today, and perhaps not even in my lifetime, but the seeds we plant today will grow into something beautiful. That's why I tinker, that's why I build. Because every small creation is a spark that lights the way.");
                                betterWorldModule.AddOption("Thank you for sharing your hope with me.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Theodric nods deeply. 'Hope is meant to be shared, traveler. Take it with you on your journey.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, betterWorldModule));
                            });
                        pl.SendGump(new DialogueGump(pl, motivationModule));
                    });
                
                p.SendGump(new DialogueGump(p, gadgetsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Theodric nods, adjusting his goggles. 'Safe travels, friend. And remember, even the smallest gear can turn the largest machine.'");
            });

        return greeting;
    }

    private bool HasFineCopperWire(PlayerMobile player)
    {
        // Check the player's inventory for FineCopperWire
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FineCopperWire)) != null;
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
        // Remove the FineCopperWire and give the GlassFurnace and MaxxiaScroll, then set the cooldown timer
        Item fineCopperWire = player.Backpack.FindItemByType(typeof(FineCopperWire));
        if (fineCopperWire != null)
        {
            fineCopperWire.Delete();
            player.AddToBackpack(new GlassFurnace());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Fine Copper Wire and receive a Glass Furnace and Maxxia Scroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Fine Copper Wire.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
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