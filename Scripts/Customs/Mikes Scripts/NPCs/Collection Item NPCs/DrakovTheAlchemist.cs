using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class DrakovTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public DrakovTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Drakov the Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new Robe(1157)); // Dark purple robe
        AddItem(new Sandals(1175)); // Dark sandals
        AddItem(new WizardsHat(1157)); // Matching hat
        AddItem(new GoldBracelet()); // Accessory for a mystical look
        AddItem(new AtomicRegulator()); // His reward item is also in his inventory, so it can be stolen

        VirtualArmor = 15;
    }

    public DrakovTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, traveler! You look like someone who appreciates the mysteries of alchemy. I am Drakov, master of transmutations and obscure formulas. What brings you here?");

        // Start with general dialogue about his work
        greeting.AddOption("Tell me about your alchemy.", 
            p => true, 
            p =>
            {
                DialogueModule alchemyModule = new DialogueModule("Alchemy is the art of transformation. Metals into gold, water into elixirs, even time itself can be bent with the right formula. But, it is not without danger.");
                alchemyModule.AddOption("What kind of dangers?", 
                    pl => true, 
                    pl =>
                    {
                        DialogueModule dangerModule = new DialogueModule("The unprepared may find themselves turned to stone, or worse. Alchemy is not just about ingredients; it requires intent, skill, and a little luck.");
                        dangerModule.AddOption("Fascinating! Thank you.", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        dangerModule.AddOption("Is that how you survived all this time?", 
                            plaa => true, 
                            plaa =>
                            {
                                DialogueModule survivalModule = new DialogueModule("Ah, you noticed, did you? I wasn't always an alchemist. I was a child, once, in a place that was nothing but ruins. Raiders came, took everything from me, and I was left with nothing. But I survived. Alchemy gave me power, a way to fight back, a way to survive. My resilience kept me alive, my cunning allowed me to thrive, and my resourcefulness transformed me into who I am today.");
                                survivalModule.AddOption("How did you learn alchemy?", 
                                    plaab => true, 
                                    plaab =>
                                    {
                                        DialogueModule learnAlchemyModule = new DialogueModule("I learned alchemy from scraps and whispers. I stole books from traders, listened to the mutterings of wise men in taverns, and practiced in the shadows. It wasn't easy. The first time I tried to transmute lead into gold, I nearly lost my hand. But I was desperate. Desperation is a powerful teacher.");
                                        learnAlchemyModule.AddOption("You must be very resourceful.", 
                                            plaabc => true, 
                                            plaabc =>
                                            {
                                                DialogueModule resourcefulModule = new DialogueModule("Resourcefulness is the only way to survive when you have nothing. I learned to use everything I could find - broken glass, weeds growing between stones, even the ash left after a fire. Every failure taught me something new, and every success brought me one step closer to escaping the life I was trapped in.");
                                                resourcefulModule.AddOption("Do you regret your past?", 
                                                    plaabcd => true, 
                                                    plaabcd =>
                                                    {
                                                        DialogueModule regretModule = new DialogueModule("Regret? Perhaps. But those regrets are what fuel me. They remind me why I chose this path. I don't want to live in fear, to be powerless again. And now, with my knowledge, I can change things - for myself, and maybe for others who are willing to learn.");
                                                        regretModule.AddOption("Can you teach me?", 
                                                            plaabcde => true, 
                                                            plaabcde =>
                                                            {
                                                                DialogueModule teachModule = new DialogueModule("Teach you? Perhaps. But alchemy demands sacrifice, and it demands your spirit to be as strong as your will. If you truly wish to learn, bring me the SubOil. Let that be your first test - to prove your dedication.");
                                                                teachModule.AddOption("I will bring you the SubOil.", 
                                                                    plaabcdef => true, 
                                                                    plaabcdef =>
                                                                    {
                                                                        plaabcdef.SendMessage("Drakov nods slowly, his eyes glinting with a mix of approval and caution.");
                                                                        plaabcdef.SendGump(new DialogueGump(plaabcdef, CreateGreetingModule(plaabcdef)));
                                                                    });
                                                                teachModule.AddOption("Perhaps I'm not ready.", 
                                                                    plaabcdef => true, 
                                                                    plaabcdef =>
                                                                    {
                                                                        plaabcdef.SendMessage("Drakov shrugs, his expression unreadable. 'Perhaps one day, then.'");
                                                                        plaabcdef.SendGump(new DialogueGump(plaabcdef, CreateGreetingModule(plaabcdef)));
                                                                    });
                                                                plaabcde.SendGump(new DialogueGump(plaabcde, teachModule));
                                                            });
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, regretModule));
                                                    });
                                                plaabc.SendGump(new DialogueGump(plaabc, resourcefulModule));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, learnAlchemyModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, survivalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, dangerModule));
                    });
                alchemyModule.AddOption("That sounds too risky for me.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, alchemyModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need any rare ingredients?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am in need of a SubOil for one of my experiments. In return, I can offer you an AtomicRegulator and always a MaxxiaScroll as a bonus. But remember, such a trade can only occur once every 10 minutes.");
                tradeIntroductionModule.AddOption("I'd like to make the trade.", 
                    pla => CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have the SubOil I need?");
                        tradeModule.AddOption("Yes, I have SubOil.", 
                            plaa => HasSubOil(plaa) && CanTradeWithPlayer(plaa), 
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have it right now.", 
                            plaa => !HasSubOil(plaa), 
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have SubOil.");
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
                tradeIntroductionModule.AddOption("Perhaps another time.", 
                    pla => true, 
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Drakov nods thoughtfully, his eyes still studying you.");
            });

        return greeting;
    }

    private bool HasSubOil(PlayerMobile player)
    {
        // Check the player's inventory for SubOil
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SubOil)) != null;
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
        // Remove the SubOil and give the AtomicRegulator and MaxxiaScroll, then set the cooldown timer
        Item subOil = player.Backpack.FindItemByType(typeof(SubOil));
        if (subOil != null)
        {
            subOil.Delete();
            player.AddToBackpack(new AtomicRegulator());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SubOil and receive an AtomicRegulator and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the SubOil.");
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