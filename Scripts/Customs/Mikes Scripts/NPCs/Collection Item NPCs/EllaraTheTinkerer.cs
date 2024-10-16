using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EllaraTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EllaraTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ellara the Tinkerer";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(180);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Deep blue robe
        AddItem(new Sandals(1175)); // Light brown sandals
        AddItem(new Bandana(1161)); // Dark green bandana
        AddItem(new SmithHammer()); // Holds a smith hammer, symbolizing her craft

        VirtualArmor = 15;
    }

    public EllaraTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Ellara, master tinkerer and lover of all things mechanical. Do you have a fascination for inventions as I do?");

        // Dialogue options
        greeting.AddOption("What kind of inventions?",
            p => true,
            p =>
            {
                DialogueModule inventionsModule = new DialogueModule("I specialize in salvaging and repurposing items into something new and useful. But I always need special parts for my projects.");

                // Trade option after story
                inventionsModule.AddOption("Do you need any parts?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed! If you have a SalvageMachine, I would gladly trade you something unique. You may choose between a MahJongTile or a LampPostA.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a SalvageMachine for me?");
                                tradeModule.AddOption("Yes, I have a SalvageMachine.",
                                    plaa => HasSalvageMachine(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasSalvageMachine(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a SalvageMachine.");
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

                inventionsModule.AddOption("Why are you interested in salvaging?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("I wasn’t always a tinkerer. Once, I was a soldier in the pre-war army. Life was harsh, and we had to make do with whatever we could find. I learned to create tools from scraps, to salvage anything of value. It's a habit that never left me.");

                        // Nested backstory options
                        backstoryModule.AddOption("What was life like as a soldier?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule soldierModule = new DialogueModule("It was brutal. We were taught to be stoic, to endure. There were no luxuries, no comforts, just orders and survival. I saw many comrades fall, and it wasn't until the war ended that I realized the weight of what we had done.");

                                soldierModule.AddOption("Do you have regrets?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regrets? More than I can count. Every life I took, every friend I lost haunts me. I thought I was fighting for a cause, but in the end, I was just following orders. Now, I try to create instead of destroy, to atone in some small way for my past.");

                                        regretModule.AddOption("That sounds difficult.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule difficultModule = new DialogueModule("It is. Some nights, the memories come back, and I can still hear the cries, feel the weight of the sword in my hand. But I keep going. I keep tinkering, keep building, because it's the only way I know how to keep the past from consuming me.");
                                                plaaa.SendGump(new DialogueGump(plaaa, difficultModule));
                                            });

                                        regretModule.AddOption("Is that why you became a tinkerer?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule tinkererReasonModule = new DialogueModule("Yes. After the war, I had nothing left. My hands were skilled in destruction, but I wanted to learn to build, to create something meaningful. Tinkering became my salvation, a way to use my skills for something better.");
                                                plaaa.SendGump(new DialogueGump(plaaa, tinkererReasonModule));
                                            });

                                        plaa.SendGump(new DialogueGump(plaa, regretModule));
                                    });

                                soldierModule.AddOption("What kept you going during the war?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule survivalModule = new DialogueModule("Survival, and the hope that one day it would all end. I had no illusions of glory or honor, just the simple desire to see another dawn. We were resourceful, using whatever we could find to stay alive. That resourcefulness is what led me to tinkering, I suppose.");
                                        plaa.SendGump(new DialogueGump(plaa, survivalModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, soldierModule));
                            });

                        backstoryModule.AddOption("Do you still do mercenary work?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mercenaryModule = new DialogueModule("Not anymore. I did for a while after the war ended, selling my skills to the highest bidder. But every contract just added to the weight I carried. It was a ruthless life, and I found myself losing whatever humanity I had left. So I walked away from it.");

                                mercenaryModule.AddOption("Do you miss it?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule missModule = new DialogueModule("Sometimes. It was simple, in a way. Follow orders, get paid, no questions asked. But it was also empty. There was no purpose beyond survival and gold. I don’t miss the emptiness.");
                                        plaa.SendGump(new DialogueGump(plaa, missModule));
                                    });

                                mercenaryModule.AddOption("Did you make any friends during that time?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule friendsModule = new DialogueModule("A few, here and there. Most were like me, lost souls with nowhere else to go. Some didn’t make it, and those that did... well, we all went our separate ways eventually. Mercenaries don’t often get happy endings.");
                                        plaa.SendGump(new DialogueGump(plaa, friendsModule));
                                    });

                                pla.SendGump(new DialogueGump(pla, mercenaryModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, inventionsModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Ellara waves you off with a smile.");
            });

        return greeting;
    }

    private bool HasSalvageMachine(PlayerMobile player)
    {
        // Check the player's inventory for SalvageMachine
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SalvageMachine)) != null;
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
        // Remove the SalvageMachine and give the chosen item, along with the MaxxiaScroll, then set the cooldown timer
        Item salvageMachine = player.Backpack.FindItemByType(typeof(SalvageMachine));
        if (salvageMachine != null)
        {
            salvageMachine.Delete();

            player.AddToBackpack(new MaxxiaScroll()); // Always give a MaxxiaScroll

            // Create a module for the reward choice
            DialogueModule rewardChoiceModule = new DialogueModule("Which reward do you choose?");

            // Add options for MahJongTile and LampPostA
            rewardChoiceModule.AddOption("MahJongTile", pl => true, pl =>
            {
                pl.AddToBackpack(new MahJongTile());
                pl.SendMessage("You receive a MahJongTile!");
            });

            rewardChoiceModule.AddOption("LampPostA", pl => true, pl =>
            {
                pl.AddToBackpack(new LampPostA());
                pl.SendMessage("You receive a LampPostA!");
            });

            // Send the gump with the reward choice
            player.SendGump(new DialogueGump(player, rewardChoiceModule));

            LastTradeTime[player] = DateTime.UtcNow;
        }
        else
        {
            player.SendMessage("It seems you no longer have a SalvageMachine.");
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