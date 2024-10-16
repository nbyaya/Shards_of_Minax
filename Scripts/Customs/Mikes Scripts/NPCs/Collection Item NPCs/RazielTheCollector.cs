using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class RazielTheCollector : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RazielTheCollector() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Raziel the Collector";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(60);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1157)); // Dark blue fancy shirt
        AddItem(new LongPants(1109)); // Dark brown pants
        AddItem(new ThighBoots(1175)); // Light tan boots
        AddItem(new Cloak(1153)); // A deep purple cloak
        AddItem(new TricorneHat(1150)); // Stylish tricorne hat
        AddItem(new Beads()); // Beads to give a unique flair

        VirtualArmor = 15;
    }

    public RazielTheCollector(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Raziel, a collector of peculiar and exotic items. You wouldn't happen to have anything interesting, would you?");

        // Start with dialogue about his interest in rare items
        greeting.AddOption("What kind of items do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I have an eye for the curious and unusual. Artifacts, rare trinkets, anything that tells a story. Lately, I've been in search of a peculiar item called a WigStand. Perhaps you could help me out?");

                collectionModule.AddOption("Tell me more about the WigStand.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule wigStandModule = new DialogueModule("The WigStand is an item of elegance from another time, used to display the finest wigs. I have reason to believe they have certain magical properties when used correctly. If you have one, I'd be willing to trade something interesting in return.");
                        wigStandModule.AddOption("What would you offer in return?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule tradeIntroductionModule = new DialogueModule("I can offer you a JudasCradle in exchange for the WigStand, along with a MaxxiaScroll as a token of my gratitude. However, I can only do this once every 10 minutes per traveler.");
                                tradeIntroductionModule.AddOption("I have a WigStand for you.",
                                    plaa => HasWigStand(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeIntroductionModule.AddOption("I traded recently; I'll come back later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeIntroductionModule.AddOption("I don't have one right now.",
                                    plaa => !HasWigStand(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a WigStand.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeIntroductionModule));
                            });
                        wigStandModule.AddOption("Sounds intriguing, but not for me.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        wigStandModule.AddOption("You seem quite passionate about these items.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule passionModule = new DialogueModule("Oh, you have no idea! Back in the day, I used to be a radio DJ, broadcasting out of a dusty old bunker. We shared news, music, and a bit of laughter to help people forget the wasteland for a while. I learned then that these little trinkets, these stories—they’re what keep people going.");
                                passionModule.AddOption("A radio DJ? Tell me more!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule djModule = new DialogueModule("Ah, yes! I was known as 'Raziel the Resonator'! Broadcasting across the wasteland, playing tunes, telling jokes, and sometimes even sharing poetry. People needed something to smile about, and I was there to provide it. Laughter is the best medicine, I always say—well, laughter and maybe a good cup of tea.");
                                        djModule.AddOption("Do you miss it?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule missModule = new DialogueModule("Oh, every day, my friend. There's nothing quite like the feeling of knowing you're reaching someone out there, making their day a little bit better. But these days, I find joy in the stories these items tell, and the people I meet—like you!");
                                                missModule.AddOption("That's quite inspiring, Raziel.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, missModule));
                                            });
                                        djModule.AddOption("It sounds like you made a real difference.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Raziel smiles warmly, his eyes twinkling with nostalgia.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, djModule));
                                    });
                                passionModule.AddOption("That must have been quite the experience.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Raziel chuckles, 'Oh, it certainly was! Never a dull moment in the wasteland, especially when you're the one trying to keep people entertained.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, passionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, wigStandModule));
                    });

                collectionModule.AddOption("Tell me a joke, Raziel.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule jokeModule = new DialogueModule("Ah, a connoisseur of humor, I see! Alright, here goes: Why did the adventurer bring a ladder to the tavern? Because they heard the drinks were on the house!");
                        jokeModule.AddOption("Haha, that's a good one! Got any more?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreJokesModule = new DialogueModule("Of course! How about this: Why don't skeletons fight each other? They just don't have the guts!");
                                moreJokesModule.AddOption("You're a natural comedian, Raziel.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Raziel bows theatrically, 'Thank you, thank you! I'll be here all week... or until the next raider attack.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                moreJokesModule.AddOption("I needed that laugh. Thanks, Raziel.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Raziel smiles, 'Anytime, friend. The wasteland's tough, but a good laugh makes it bearable.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, moreJokesModule));
                            });
                        jokeModule.AddOption("Not bad, but I've heard better.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Raziel laughs, 'Tough crowd, tough crowd! I'll work on my material.'");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, jokeModule));
                    });

                collectionModule.AddOption("I'll keep an eye out for something interesting.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Raziel nods, his eyes glinting with curiosity.");
            });

        return greeting;
    }

    private bool HasWigStand(PlayerMobile player)
    {
        // Check the player's inventory for WigStand
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WigStand)) != null;
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
        // Remove the WigStand and give the JudasCradle and MaxxiaScroll, then set the cooldown timer
        Item wigStand = player.Backpack.FindItemByType(typeof(WigStand));
        if (wigStand != null)
        {
            wigStand.Delete();
            player.AddToBackpack(new JudasCradle());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the WigStand and receive a JudasCradle and a MaxxiaScroll in return. Thank you for your help, traveler!");
        }
        else
        {
            player.SendMessage("It seems you no longer have a WigStand.");
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