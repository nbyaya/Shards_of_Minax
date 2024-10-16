using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ValeriaTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ValeriaTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Valeria the Collector";
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
        AddItem(new FancyDress(1153)); // A bright blue fancy dress
        AddItem(new ThighBoots(1109)); // Dark leather boots
        AddItem(new FeatheredHat(1175)); // A stylish feathered hat with a purple hue
        AddItem(new GoldBracelet()); // A shiny golden bracelet

        VirtualArmor = 15;
    }

    public ValeriaTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there, traveler! I am Valeria, a collector of rare and fascinating items. Have you come across anything peculiar in your adventures?");

        // General dialogue options
        greeting.AddOption("Tell me about the items you collect.",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I am particularly interested in unique artifacts with a story behind them. Some items are valuable not because of their material worth, but because of the history they carry. For example, have you ever heard of the ZebulinVase?");

                collectionModule.AddOption("What is the ZebulinVase?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule vaseModule = new DialogueModule("The ZebulinVase is said to be crafted by the ancient artisans of Zebulin, a lost civilization known for their mysterious pottery techniques. The vase is rumored to bring good fortune to those who possess it, but I have yet to confirm this myself.");
                        vaseModule.AddOption("I think I might have one.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateTradeModule(pla)));
                            });
                        vaseModule.AddOption("That sounds interesting, but I don't have one.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        vaseModule.AddOption("Tell me more about Zebulin.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule zebulinHistoryModule = new DialogueModule("Zebulin was a civilization of inventors and dreamers. They built intricate machinery and had a fascination with robotics, which was quite ahead of their time. They were inventive, quirky, and mostly kept to themselves—much like myself! They say that their artisans used to create wonders, not just pottery, but also mechanical constructs.");
                                zebulinHistoryModule.AddOption("Mechanical constructs?", 
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule constructsModule = new DialogueModule("Indeed! Some say they even managed to create autonomous robots that guarded their treasures and served the people. My dream is to someday recreate those machines, using the remnants I've salvaged from the wasteland. My workshop is filled with bits and pieces—oddities that others might call junk, but to me, they're treasure.");
                                        constructsModule.AddOption("You seem passionate about this. Why robots?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule robotsModule = new DialogueModule("Ah, well, that's a story of hope. You see, the wasteland is vast and dangerous. I believe that with a well-built army of robots, we could reclaim these lands, restore order, and perhaps even rebuild what was lost. It's a dream, I know—a wild one—but it's what keeps me going, day after day.");
                                                robotsModule.AddOption("That's an ambitious dream.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                robotsModule.AddOption("How do you plan to build these robots?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule buildRobotsModule = new DialogueModule("Well, it's not easy. I need all sorts of components—gears, servos, power sources, and the rarest of all: knowledge. I've been piecing together old manuals and experimenting with parts I find. The ZebulinVase, for example, is said to contain inscriptions that may provide clues about power distribution in their ancient constructs.");
                                                        buildRobotsModule.AddOption("I see, that's why it's so valuable to you.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        buildRobotsModule.AddOption("Is there anything I could help with?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule helpModule = new DialogueModule("If you come across any old mechanical parts, or even something as simple as a forgotten cog, please bring it to me. Every little piece could be the key to making a difference. And, of course, the ZebulinVase—if you find it, I'll make it worth your while.");
                                                                helpModule.AddOption("I'll keep an eye out.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, helpModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, buildRobotsModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, robotsModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, constructsModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, zebulinHistoryModule));
                            });
                        pl.SendGump(new DialogueGump(pl, vaseModule));
                    });

                collectionModule.AddOption("I haven't heard of it.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Trade Option
        greeting.AddOption("Do you need anything in particular?",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, CreateTradeModule(p)));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Valeria smiles and nods politely.");
            });

        return greeting;
    }

    private DialogueModule CreateTradeModule(PlayerMobile player)
    {
        DialogueModule tradeModule = new DialogueModule("I am currently looking for a ZebulinVase. If you bring me one, I can offer you a StuffedDoll in exchange, along with a MaxxiaScroll. However, I can only make this trade once every ten minutes per person.");

        tradeModule.AddOption("I have a ZebulinVase for you.",
            pl => HasZebulinVase(pl) && CanTradeWithPlayer(pl),
            pl =>
            {
                CompleteTrade(pl);
            });

        tradeModule.AddOption("I don't have one right now.",
            pl => !HasZebulinVase(pl),
            pl =>
            {
                pl.SendMessage("Come back when you have a ZebulinVase. I am eager to see it!");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });

        tradeModule.AddOption("I traded recently; I'll come back later.",
            pl => !CanTradeWithPlayer(pl),
            pl =>
            {
                pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
            });

        return tradeModule;
    }

    private bool HasZebulinVase(PlayerMobile player)
    {
        // Check the player's inventory for ZebulinVase
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ZebulinVase)) != null;
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
        // Remove the ZebulinVase and give the StuffedDoll and MaxxiaScroll, then set the cooldown timer
        Item zebulinVase = player.Backpack.FindItemByType(typeof(ZebulinVase));
        if (zebulinVase != null)
        {
            zebulinVase.Delete();
            player.AddToBackpack(new StuffedDoll());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ZebulinVase and receive a StuffedDoll and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a ZebulinVase.");
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