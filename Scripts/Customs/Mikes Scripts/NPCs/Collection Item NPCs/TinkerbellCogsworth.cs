using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class TinkerbellCogsworth : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public TinkerbellCogsworth() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tinkerbell Cogsworth";
        Title = "the Clockmaker";
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
        AddItem(new FancyDress(153)); // A fancy dress with a blue hue
        AddItem(new Boots(1109)); // Boots with a dark shade
        AddItem(new LeatherLegs()); // Custom-made clock-themed leggings
        AddItem(new HalfApron(1175)); // A tinker's apron with tools hanging off it
        AddItem(new Lantern()); // Unique lantern she's holding

        VirtualArmor = 15;
    }

    public TinkerbellCogsworth(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Tinkerbell Cogsworth, master of all things mechanical and time-related. Have you perhaps come across a peculiar contraption known as the FunPumpkinCannon?");

        // Options for the player to choose from
        greeting.AddOption("What is a FunPumpkinCannon?",
            p => true,
            p =>
            {
                DialogueModule cannonExplanation = new DialogueModule("Ah, the FunPumpkinCannon! It's a whimsical device designed to launch pumpkins for fun and joy. But, alas, I could use one for a rather special project. If you bring me one, I shall reward you handsomely!");
                cannonExplanation.AddOption("Sounds interesting. What will you give me in return?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeExplanation = new DialogueModule("In exchange for the FunPumpkinCannon, I'll give you a FancySewingMachine, perfect for creating extraordinary garments. And as a bonus, you'll also receive a MaxxiaScroll. However, keep in mind that I can only make this trade once every 10 minutes per person.");
                        tradeExplanation.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a FunPumpkinCannon for me?");
                                tradeModule.AddOption("Yes, I have a FunPumpkinCannon.",
                                    plaa => HasFunPumpkinCannon(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasFunPumpkinCannon(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a FunPumpkinCannon.");
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
                        tradeExplanation.AddOption("Maybe another time.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeExplanation));
                    });
                cannonExplanation.AddOption("Why do you need a FunPumpkinCannon?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("Why, you ask? I am conducting a series of experiments involving kinetic energy transfer, and the FunPumpkinCannon is a perfect tool for my research! You see, I am no ordinary tinkerer—I am a rogue scientist, unbound by the confines of formal institutions. I work tirelessly in isolation, pushing the boundaries of what is known.");
                        reasonModule.AddOption("That sounds intriguing. Tell me more about your research.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule researchModule = new DialogueModule("My research is focused on understanding the intersection of magic and machinery. Imagine a world where mechanical constructs could operate with the autonomy of living beings, powered by arcane energies! I believe that by studying the properties of enchanted items—like the FunPumpkinCannon—I can unlock new ways to blend magic and technology. It is difficult, often frustrating work, but I am determined to make groundbreaking discoveries.");
                                researchModule.AddOption("You must be very dedicated.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dedicationModule = new DialogueModule("Dedicated? Absolutely. Every ticking second is precious, and I refuse to let convention or bureaucracy slow me down. I work alone, independent of guilds or sponsors, because only in solitude can I truly focus on what needs to be done. My passion is to uncover secrets no one else dares to explore.");
                                        dedicationModule.AddOption("Don't you ever get lonely?",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule lonelinessModule = new DialogueModule("Lonely? Perhaps at times. But I find solace in my work. Every invention, every discovery—it becomes a companion of sorts. And then there are travelers like you, who bring a moment of connection, a reminder that perhaps one day my work will benefit others as well. The joy of discovery far outweighs the solitude.");
                                                lonelinessModule.AddOption("I admire your resolve.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendMessage("Tinkerbell smiles warmly, her eyes glinting with determination.");
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, lonelinessModule));
                                            });
                                        dedicationModule.AddOption("I can see why you work alone.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Tinkerbell nods, her expression resolute.");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dedicationModule));
                                    });
                                researchModule.AddOption("That sounds like dangerous work.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Indeed, it is. Experimenting with volatile magic and machinery is not for the faint of heart. But danger is part of progress. I have had my fair share of explosions and mishaps, but each failure is a step closer to success. I cannot afford to be deterred.");
                                        dangerModule.AddOption("I respect your courage.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Tinkerbell gives a determined nod, her eyes filled with fierce focus.");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, researchModule));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });
                p.SendGump(new DialogueGump(p, cannonExplanation));
            });

        greeting.AddOption("Who are you exactly?",
            p => true,
            p =>
            {
                DialogueModule backstoryModule = new DialogueModule("I am Tinkerbell Cogsworth, a rogue scientist and clockmaker. Unlike most tinkers who focus on mundane repairs, I am fascinated by the idea of merging life and machinery. I have dedicated my life to perfecting constructs that can think, react, and even feel. My independence allows me to explore these ideas without restrictions or limits.");
                backstoryModule.AddOption("Why work alone?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule independenceModule = new DialogueModule("Working alone gives me the freedom to pursue whatever direction my research takes me, without having to justify myself to anyone. Bureaucracy stifles creativity, and collaboration often means compromise. I am driven to make discoveries that others would deem impossible, and I cannot let anything—or anyone—hold me back.");
                        independenceModule.AddOption("You sound very focused.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule focusModule = new DialogueModule("Focus is key to everything I do. There are moments when the rest of the world fades away, and I can see the gears of possibility turning in my mind. It is in those moments that true breakthroughs happen. Distraction is the enemy of innovation.");
                                focusModule.AddOption("I wish you success in your endeavors.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Tinkerbell smiles, her eyes betraying a hint of warmth amidst her determined demeanor.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, focusModule));
                            });
                        independenceModule.AddOption("That sounds exhausting.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule exhaustionModule = new DialogueModule("It is, but I would not have it any other way. The drive to discover keeps me going. I will rest when I have made my mark on this world, when my work is complete—and that, my friend, will take a lifetime.");
                                exhaustionModule.AddOption("I hope your efforts pay off.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Tinkerbell nods, her expression resolute.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, exhaustionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, independenceModule));
                    });
                p.SendGump(new DialogueGump(p, backstoryModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Tinkerbell nods and continues tinkering with her gadgets.");
            });

        return greeting;
    }

    private bool HasFunPumpkinCannon(PlayerMobile player)
    {
        // Check the player's inventory for FunPumpkinCannon
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FunPumpkinCannon)) != null;
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
        // Remove the FunPumpkinCannon and give the FancySewingMachine and MaxxiaScroll, then set the cooldown timer
        Item funPumpkinCannon = player.Backpack.FindItemByType(typeof(FunPumpkinCannon));
        if (funPumpkinCannon != null)
        {
            funPumpkinCannon.Delete();
            player.AddToBackpack(new FancySewingMachine());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FunPumpkinCannon and receive a FancySewingMachine and a MaxxiaScroll in return. How delightful!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FunPumpkinCannon.");
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