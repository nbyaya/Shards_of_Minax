using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class JaxonTheStoneKeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public JaxonTheStoneKeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jaxon the Stone Keeper";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(80);
        SetDex(50);
        SetInt(120);

        SetHits(120);
        SetMana(200);
        SetStam(80);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyShirt(1153)); // Blue Fancy Shirt
        AddItem(new LongPants(2406)); // Dark grey pants
        AddItem(new Boots(1109)); // Brown boots
        AddItem(new Cloak(1175)); // Light green cloak
        AddItem(new MasterCello()); // Smith hammer as part of his outfit

        VirtualArmor = 20;
    }

    public JaxonTheStoneKeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Jaxon, Keeper of Stones and perhaps a keeper of more... esoteric knowledge. My, you seem like someone who is no stranger to secrets. Are you here to learn, to trade, or perhaps for something else entirely?");

        // Dialogue about his work and secrets
        greeting.AddOption("Tell me about stone crafting.",
            p => true,
            p =>
            {
                DialogueModule craftModule = new DialogueModule("Stone crafting is not just about shaping the physical. Each stone, each mineral has its own resonance, its own story. But... I suspect you want more than just tales of stone, don't you?");

                // Further delve into secrets
                craftModule.AddOption("You seem to know a lot. What else can you tell me?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule secretsModule = new DialogueModule("Ah, I see you are perceptive. Yes, I do know a lot. Information, you see, is more valuable than gold to the right person. Let me ask you: how much are you willing to pay to know what your enemies are planning? Or perhaps... what your allies are hiding from you?");

                        secretsModule.AddOption("What do you know about my allies?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule alliesModule = new DialogueModule("Your allies, hmm? Let's just say not all of them are as loyal as they claim to be. Some have debts, some have... desires that might conflict with yours. I could tell you more, but knowledge comes with a price, dear friend.");

                                alliesModule.AddOption("What kind of price?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule priceModule = new DialogueModule("Ah, it could be gold, it could be a favor. But for you, I think... a secret of equal value. Tell me something only you know, and I will tell you about the one who plots behind your back.");

                                        priceModule.AddOption("I will tell you a secret.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                DialogueModule exchangeModule = new DialogueModule("Very well. Whisper it to me...");
                                                exchangeModule.AddOption("[Whisper a secret about a rival]",
                                                    plaabb => true,
                                                    plaabb =>
                                                    {
                                                        plaabb.SendMessage("Jaxon smiles slyly. 'Ah yes, that is indeed useful. In return, I will tell you this: One of your supposed allies has been meeting with a stranger in the dead of night. Beware of misplaced trust.'");
                                                        plaabb.SendGump(new DialogueGump(plaabb, CreateGreetingModule(plaabb)));
                                                    });
                                                exchangeModule.AddOption("I changed my mind.",
                                                    plaabb => true,
                                                    plaabb =>
                                                    {
                                                        plaabb.SendMessage("Ah, a shame. Perhaps another time, then.");
                                                        plaabb.SendGump(new DialogueGump(plaabb, CreateGreetingModule(plaabb)));
                                                    });
                                                plaab.SendGump(new DialogueGump(plaab, exchangeModule));
                                            });

                                        priceModule.AddOption("I cannot share a secret.",
                                            plaab => true,
                                            plaab =>
                                            {
                                                plaab.SendMessage("Knowledge is power, friend. Without an exchange, I cannot help you.");
                                                plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule(plaab)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, priceModule));
                                    });

                                alliesModule.AddOption("Never mind, I don't want to know.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("As you wish. Some stones are better left unturned.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, alliesModule));
                            });

                        secretsModule.AddOption("I don't need to know about my allies.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Very well. Just know that secrets have a way of catching up with us all.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, secretsModule));
                    });

                // Trade option for GraniteHammer
                craftModule.AddOption("I have a GraniteHammer for you.",
                    pl => HasGraniteHammer(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });

                craftModule.AddOption("No, I do not have a GraniteHammer.",
                    pl => !HasGraniteHammer(pl),
                    pl =>
                    {
                        pl.SendMessage("Ah, a pity. Should you find one, I would be most interested.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                craftModule.AddOption("I traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                p.SendGump(new DialogueGump(p, craftModule));
            });

        // Offer to provide intel for a price
        greeting.AddOption("Do you have any useful information?",
            p => true,
            p =>
            {
                DialogueModule infoModule = new DialogueModule("Ah, information is my specialty. But nothing is free, my friend. What are you looking for? Secrets about the nobles? The hidden dealings of merchants? Or perhaps... the weaknesses of your enemies?");

                infoModule.AddOption("Tell me about the nobles.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule noblesModule = new DialogueModule("The nobles are a curious bunch. They smile and wave, but behind closed doors, they plot against each other. Lord Albrecht, for instance, has been siphoning funds meant for the town guard. If you were to make this information public, it could create... opportunities.");

                        noblesModule.AddOption("How do you know this?",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Ah, I have my sources. Let us say that even the most careful of nobles has someone who can be... persuaded.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        noblesModule.AddOption("Interesting. I'll keep that in mind.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Knowledge is power, my friend. Use it wisely.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, noblesModule));
                    });

                infoModule.AddOption("Tell me about the merchants.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule merchantsModule = new DialogueModule("The merchants are greedy, as they always are. But did you know that Silas the Importer has been bringing in more than just spices and silks? Rumor has it that he is smuggling rare artifacts. Such things could be very valuable... to the right people.");

                        merchantsModule.AddOption("Where does he keep these artifacts?",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("He keeps them hidden in a warehouse by the docks. But be warned, it is well guarded.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        merchantsModule.AddOption("That's good to know. Thank you.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("Of course. Just remember, I provided this information. If you profit from it, I may expect a share.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, merchantsModule));
                    });

                infoModule.AddOption("Tell me about my enemies.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule enemiesModule = new DialogueModule("Your enemies are closer than you think. I know of at least one who has been plotting against you for some time. They are clever, but not clever enough to escape my notice. Do you wish to know more?");

                        enemiesModule.AddOption("Yes, tell me more.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreEnemiesModule = new DialogueModule("They have been meeting with a mysterious figure in the forest. It seems they are planning something... disruptive. If you act quickly, you may be able to catch them in the act.");
                                moreEnemiesModule.AddOption("Where exactly?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Near the old stone circle, just beyond the northern treeline. But be careful, they will not be alone.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                moreEnemiesModule.AddOption("I'll handle it myself.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Very well. Just remember, information is my trade. If you need more, you know where to find me.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, moreEnemiesModule));
                            });

                        enemiesModule.AddOption("No, I don't need to know right now.",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("As you wish. But be wary. Ignorance can be more dangerous than any weapon.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });

                        pl.SendGump(new DialogueGump(pl, enemiesModule));
                    });

                p.SendGump(new DialogueGump(p, infoModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Jaxon gives a knowing smile, his eyes twinkling with secrets untold.");
            });

        return greeting;
    }

    private bool HasGraniteHammer(PlayerMobile player)
    {
        // Check the player's inventory for GraniteHammer
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GraniteHammer)) != null;
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
        // Remove the GraniteHammer and give the MasterCello and MaxxiaScroll, then set the cooldown timer
        Item graniteHammer = player.Backpack.FindItemByType(typeof(GraniteHammer));
        if (graniteHammer != null)
        {
            graniteHammer.Delete();
            player.AddToBackpack(new MasterCello());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the GraniteHammer and receive a MasterCello and MaxxiaScroll in return. Jaxon smiles at you with gratitude, though his eyes suggest he gained more from this exchange than you realize.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a GraniteHammer.");
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