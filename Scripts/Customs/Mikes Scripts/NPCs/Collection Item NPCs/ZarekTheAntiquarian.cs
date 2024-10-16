using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class ZarekTheAntiquarian : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public ZarekTheAntiquarian() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zarek the Antiquarian";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // A dark blue fancy shirt
        AddItem(new LongPants(1109)); // Black pants
        AddItem(new Boots(1175)); // Light gray boots
        AddItem(new Cloak(1153)); // Deep purple cloak
        AddItem(new Cap(1150)); // Unique tricorn hat
        AddItem(new GoldNecklace()); // A gold necklace, adding to his mysterious aura

        VirtualArmor = 15;
    }

    public ZarekTheAntiquarian(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Zarek, a collector of the unusual and the forgotten. Do you have something of interest, perhaps?");

        // Dialogue about his collection
        greeting.AddOption("What kind of items do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I collect relics, curios, and artifacts from bygone eras. Each has a story to tell, and I ensure these stories are not lost to time. Perhaps you have something worth trading?");
                collectionModule.AddOption("Tell me more about the types of artifacts you collect.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("Ah, I specialize in pre-apocalypse artifacts—those remnants of the world before everything changed. Tools, books, trinkets... each item has its own legacy, and I strive to understand them all. Knowledge is our bridge to a better future, wouldn't you agree?");
                        artifactsModule.AddOption("What do you mean by 'pre-apocalypse' artifacts?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule preApocalypseModule = new DialogueModule("The pre-apocalypse era was a time of grand civilization. Technology, culture, and knowledge flourished, but hubris led to its downfall. I believe that if we understand what went wrong, we can ensure it never happens again. We must learn from the relics they left behind.");
                                preApocalypseModule.AddOption("What kind of technology did they have?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule technologyModule = new DialogueModule("The technology of that era was astounding! Devices that could communicate over great distances, machines that could fly through the sky, and tools that made life more convenient. Sadly, much of this was lost. I've gathered fragments—some machines that click and whir, others that glow faintly—and I study them to unlock their secrets.");
                                        technologyModule.AddOption("Have you managed to make any of them work?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule workingArtifactsModule = new DialogueModule("Only a few. Some machines are beyond repair, but there are a few small successes. I have a box that emits strange noises—music, I believe. And a glowing orb that, when held, seems to respond to the holder's thoughts. There is so much to learn, but each breakthrough brings us closer to the truth.");
                                                workingArtifactsModule.AddOption("That's fascinating! Thank you for sharing.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, workingArtifactsModule));
                                            });
                                        technologyModule.AddOption("I see. It must be difficult to study these relics.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule difficultyModule = new DialogueModule("Indeed, it is not easy. Many of these artifacts were damaged or lost during the fall. The knowledge needed to use them has faded, but I am relentless. Understanding the past is my purpose, and I will not stop until every artifact yields its secrets.");
                                                difficultyModule.AddOption("Your dedication is admirable.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Zarek smiles warmly, his eyes showing both weariness and determination.");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, difficultyModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, technologyModule));
                                    });
                                preApocalypseModule.AddOption("Do you think we can rebuild what was lost?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule rebuildModule = new DialogueModule("I hope so. The key lies in understanding. If we comprehend the mistakes of the past, perhaps we can avoid them in the future. But rebuilding requires more than technology—it requires wisdom, unity, and the will to move forward without repeating the errors of our predecessors.");
                                        rebuildModule.AddOption("That's a noble goal, Zarek.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Zarek nods solemnly, his eyes reflecting his deep conviction.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        rebuildModule.AddOption("It seems like an impossible task.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule impossibleModule = new DialogueModule("Nothing is impossible, my friend. It is true that the path is long and fraught with challenges, but each step we take, each bit of knowledge we reclaim, brings us closer. I am but one man, but with the help of others, I believe we can succeed.");
                                                impossibleModule.AddOption("Perhaps you are right.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, impossibleModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, rebuildModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, preApocalypseModule));
                            });
                        artifactsModule.AddOption("Why are you so interested in these artifacts?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("My interest borders on obsession, I admit. I am an old man, and I have seen the best and worst of humanity. I believe that understanding our history is the only way forward. These artifacts are all that remain of a world that was. I cannot let their stories be forgotten.");
                                obsessionModule.AddOption("It must be a lonely pursuit.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule lonelinessModule = new DialogueModule("Lonely, yes. But also fulfilling. Each discovery is like a spark in the darkness. It reminds me that I am not alone—that those who came before me left something behind for us to find. And now, I share that spark with you, and perhaps you will share it with others.");
                                        lonelinessModule.AddOption("Thank you for sharing your knowledge.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Zarek smiles kindly, his eyes filled with warmth.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, lonelinessModule));
                                    });
                                obsessionModule.AddOption("Your dedication is inspiring.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Zarek bows his head humbly.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, obsessionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, artifactsModule));
                    });
                collectionModule.AddOption("I might have something you're interested in.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I am particularly interested in an artifact called the TotumPole. If you have one, I can offer you a TwentyfiveShield in exchange, along with a MaxxiaScroll. But remember, I can only make this exchange once every ten minutes.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have the TotumPole with you?");
                                tradeModule.AddOption("Yes, I have a TotumPole.",
                                    plaa => HasTotumPole(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have it right now.",
                                    plaa => !HasTotumPole(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a TotumPole.");
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
                        pl.SendGump(new DialogueGump(pl, tradeIntroductionModule));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Zarek nods, his eyes glinting with curiosity.");
            });

        return greeting;
    }

    private bool HasTotumPole(PlayerMobile player)
    {
        // Check the player's inventory for TotumPole
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(TotumPole)) != null;
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
        // Remove the TotumPole and give the TwentyfiveShield and MaxxiaScroll, then set the cooldown timer
        Item totumPole = player.Backpack.FindItemByType(typeof(TotumPole));
        if (totumPole != null)
        {
            totumPole.Delete();
            player.AddToBackpack(new TwentyfiveShield());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the TotumPole and receive a TwentyfiveShield and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a TotumPole.");
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