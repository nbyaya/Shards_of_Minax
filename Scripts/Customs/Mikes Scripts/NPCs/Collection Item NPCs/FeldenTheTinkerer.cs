using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class FeldenTheTinkerer : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public FeldenTheTinkerer() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Felden the Tinkerer";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(80);
        SetInt(120);

        SetHits(100);
        SetMana(180);
        SetStam(70);

        Fame = 500;
        Karma = 200;

        // Outfit
        AddItem(new FancyShirt(1153)); // A blue fancy shirt
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new Sandals(1359)); // Brown sandals
        AddItem(new FloppyHat(1165)); // Green floppy hat
        AddItem(new ToolBox()); // Carrying a toolbox for his tinkering

        VirtualArmor = 15;
    }

    public FeldenTheTinkerer(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Hello there, traveler! I'm Felden, a tinkerer of unique curiosities. You wouldn't happen to be interested in a little trade, would you?");

        // Dialogue options
        greeting.AddOption("What kind of trade are you talking about?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Ah, well, you see, I've been searching for something quite special - a RareSausage. If you happen to have one, I can offer you a choice of rewards: either a WatermelonFruit or a ToolBox, plus a little something extra for your trouble.");

                tradeIntroductionModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a RareSausage for me?");

                        tradeModule.AddOption("Yes, I have a RareSausage.",
                            plaa => HasRareSausage(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });

                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasRareSausage(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a RareSausage. I'll be here, tinkering away.");
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

                // Add more layers of dialogue to emphasize Felden's gambler traits
                tradeIntroductionModule.AddOption("You seem a bit... desperate?",
                    pla => true,
                    pla =>
                    {
                        DialogueModule gamblerModule = new DialogueModule("Desperate? Ha! I suppose you could say that. You see, I've made a few... risky wagers lately, and now I've got myself into a bit of trouble. There's a darkness that's been following me ever since I won a cursed artifact in a high-stakes game.");

                        gamblerModule.AddOption("A cursed artifact? That sounds dangerous.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule artifactModule = new DialogueModule("Dangerous? Oh, absolutely! But you see, that's where the thrill comes from! The artifact is said to bring luck, but lately, it has brought me nothing but strange shadows and ill omens. I can't let it go though... not yet. It's like a part of me now.");

                                artifactModule.AddOption("Why not get rid of it?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule ridModule = new DialogueModule("Get rid of it? I wish I could, friend. But it feels like it's tied to me. Every time I try to sell it or even hide it, something strange happens. Once, I buried it deep in the woods, and when I returned home, it was right there on my workbench, as if it had never left.");

                                        ridModule.AddOption("That sounds terrifying. Have you tried anything else?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule solutionModule = new DialogueModule("Oh, I've tried everything I can think of. Priests, mages, even a few charlatans who claimed they could cleanse curses. Nothing worked. But you know what? Maybe it's just my destiny to bear it. After all, it did help me win a fortune once... before things went wrong.");

                                                solutionModule.AddOption("You must be quite the gambler to take on such risks.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule gamblerStoryModule = new DialogueModule("Gambler? Ha! You bet I am. I live for the thrill, the high stakes, the feeling that everything could change with just one roll of the dice. It's not just about money, you see. It's about life itself, and how close you can get to the edge without falling off.");

                                                        gamblerStoryModule.AddOption("That sounds exhilarating, but also dangerous.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                DialogueModule dangerousLifeModule = new DialogueModule("Danger is part of the charm, my friend. The closer you get to losing it all, the sweeter the victory when you win. But... lately, I've been feeling like I'm running out of luck. The artifact, the dark forces... it's all catching up to me. I need something to change my fortune again.");

                                                                dangerousLifeModule.AddOption("Maybe the RareSausage will help you somehow?",
                                                                    plaaaaaaa => true,
                                                                    plaaaaaaa =>
                                                                    {
                                                                        DialogueModule hopeModule = new DialogueModule("Maybe. Maybe the RareSausage is just the lucky charm I need to turn things around. Or maybe it's another piece of my downfall. Who knows? But I have to try. That's what gambling is all about—taking the risk, even when the odds are against you.");

                                                                        hopeModule.AddOption("Alright, let's make the trade.",
                                                                            plaaaaaaaa => CanTradeWithPlayer(plaaaaaaaa),
                                                                            plaaaaaaaa =>
                                                                            {
                                                                                CompleteTrade(plaaaaaaaa);
                                                                            });

                                                                        hopeModule.AddOption("I'm not sure I want to get involved in this...",
                                                                            plaaaaaaaa => true,
                                                                            plaaaaaaaa =>
                                                                            {
                                                                                plaaaaaaaa.SendMessage("I understand. Not everyone is cut out for the thrill of risk. Come back if you change your mind.");
                                                                                plaaaaaaaa.SendGump(new DialogueGump(plaaaaaaaa, CreateGreetingModule(plaaaaaaaa)));
                                                                            });

                                                                        plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, hopeModule));
                                                                    });

                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, dangerousLifeModule));
                                                            });

                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, gamblerStoryModule));
                                                    });

                                                plaaaa.SendGump(new DialogueGump(plaaaa, solutionModule));
                                            });

                                        plaaa.SendGump(new DialogueGump(plaaa, ridModule));
                                    });

                                plaa.SendGump(new DialogueGump(plaa, artifactModule));
                            });

                        pla.SendGump(new DialogueGump(pla, gamblerModule));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Felden nods and goes back to tinkering with his toolbox.");
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

            // Add options for WatermelonFruit and ToolBox
            rewardChoiceModule.AddOption("WatermelonFruit", pl => true, pl =>
            {
                pl.AddToBackpack(new WatermelonFruit());
                pl.SendMessage("You receive a WatermelonFruit! Delicious!");
            });

            rewardChoiceModule.AddOption("ToolBox", pl => true, pl =>
            {
                pl.AddToBackpack(new ToolBox());
                pl.SendMessage("You receive a ToolBox! Perfect for tinkering.");
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