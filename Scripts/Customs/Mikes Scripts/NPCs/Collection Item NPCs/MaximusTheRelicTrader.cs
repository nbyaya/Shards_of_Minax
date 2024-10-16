using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class MaximusTheRelicTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MaximusTheRelicTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Maximus the Relic Trader";
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
        AddItem(new Robe(1157)); // Long coat with a dark blue hue
        AddItem(new Boots(2309)); // Boots with a black hue
        AddItem(new WideBrimHat(1175)); // A mysterious wide-brimmed hat
        AddItem(new Scepter()); // He carries a decorative scepter

        VirtualArmor = 20;
    }

    public MaximusTheRelicTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! You seem like someone who appreciates the finer mysteries of our world. I am Maximus, collector and trader of rare relics. Do you possess something interesting?");

        // Start with dialogue about relics and stories
        greeting.AddOption("Tell me about the relics you collect.",
            p => true,
            p =>
            {
                DialogueModule relicsModule = new DialogueModule("Relics, my friend, are more than mere objects. They hold the echoes of ages past. I specialize in objects like the GargishTotem, an artifact of the mysterious Gargoyle culture. Their power is subtle, but undeniable.");
                relicsModule.AddOption("What kind of power does the GargishTotem hold?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule powerModule = new DialogueModule("The GargishTotem, they say, can ward off misfortune and attract the favor of the elements. A rare item indeed, and one I am always interested in acquiring. But let me tell you, these relics hold more than power—they hold stories. Dark stories of past bearers and their struggles with fate.");
                        powerModule.AddOption("Tell me more about these stories.",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule storyModule = new DialogueModule("Ah, these tales are not for the faint of heart. One such tale is of Lord Vexian, a noble whose obsession with power led him to acquire the GargishTotem. He sought the favor of the elements but found himself haunted by whispers in the night—dark whispers that promised power but took his sanity in return. His descent into madness was a tragic sight, and his kingdom fell to ruin as he became a shadow of his former self.");
                                storyModule.AddOption("Did he ever find peace?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule peaceModule = new DialogueModule("Peace? Perhaps, in the end. They say he vanished into the mist one night, leaving behind only the totem, which I now seek to trade. Some believe he was claimed by the spirits he had summoned, while others say he found a darker purpose beyond the veil. In truth, no one knows.");
                                        peaceModule.AddOption("That is quite tragic.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, peaceModule));
                                    });
                                storyModule.AddOption("I think I've heard enough of tragic tales.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, storyModule));
                            });
                        powerModule.AddOption("Interesting...", plaa => true, plaa => { plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa))); });
                        pl.SendGump(new DialogueGump(pl, powerModule));
                    });
                relicsModule.AddOption("Fascinating. Do you trade these relics?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Indeed, I am willing to trade. If you happen to possess a GargishTotem, I can offer you something in return: a CowToken and a MaxxiaScroll as a token of my gratitude. However, I can only perform such trades every so often.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a GargishTotem for me?");
                                tradeModule.AddOption("Yes, I have a GargishTotem.",
                                    plaa => HasGargishTotem(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasGargishTotem(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a GargishTotem.");
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
                p.SendGump(new DialogueGump(p, relicsModule));
            });

        // Add a deeper, brooding element about Maximus' past
        greeting.AddOption("You seem troubled. What burdens you, Maximus?",
            p => true,
            p =>
            {
                DialogueModule burdenModule = new DialogueModule("Ah, traveler, you perceive too much. Yes, I was once a celebrated poet, my words sought by nobles and scholars alike. But those days are long gone. The tragedy that befell my family has left me but a husk of my former self. My works grew darker, my thoughts... clouded by despair. Now, I walk this land, trading relics, searching for meaning that I fear may never return.");
                burdenModule.AddOption("What tragedy befell you?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tragedyModule = new DialogueModule("It was my beloved Elara, my muse, my light. She was taken from me—a sickness that no healer could cure, a darkness that consumed her while I watched, powerless. In my grief, I wrote verses that called to forces beyond our understanding. Dark forces answered, and I have been haunted by them ever since.");
                        tragedyModule.AddOption("Dark forces? What do you mean?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule darkForcesModule = new DialogueModule("They whisper to me, traveler. Whispers of power, of vengeance, of a way to bring her back. I have resisted, but their voices grow louder each night. The relics I collect... some of them resonate with these whispers, as if they are keys to unlocking something far greater—or far more terrible.");
                                darkForcesModule.AddOption("Do you truly wish to bring her back?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule bringBackModule = new DialogueModule("I do not know anymore. The thought of her, alive again, fills my heart with hope and dread in equal measure. What if she returns, but not as she was? What if the darkness that touched her has tainted her soul? These are the questions that keep me wandering, never finding peace.");
                                        bringBackModule.AddOption("Perhaps it is best to let her rest.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Maximus lowers his gaze, nodding solemnly. 'You may be right. Perhaps that is the only kindness left to me.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        bringBackModule.AddOption("I hope you find your answers, Maximus.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Maximus offers a sad smile. 'As do I, traveler. As do I.'");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, bringBackModule));
                                    });
                                darkForcesModule.AddOption("You must resist them, Maximus.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Maximus nods, his eyes shadowed. 'I try, traveler. Every day, I try.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, darkForcesModule));
                            });
                        tragedyModule.AddOption("I am truly sorry for your loss.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendMessage("Maximus bows his head. 'Thank you, traveler. Words are all I have left, and yet, they feel so empty now.'");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tragedyModule));
                    });
                burdenModule.AddOption("I hope you find peace one day.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Maximus smiles faintly, though his eyes remain distant. 'Peace... a fleeting dream, perhaps. But thank you for your kindness.'");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, burdenModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Maximus nods knowingly, his eyes twinkling with mystery.");
            });

        return greeting;
    }

    private bool HasGargishTotem(PlayerMobile player)
    {
        // Check the player's inventory for GargishTotem
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(GargishTotem)) != null;
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
        // Remove the GargishTotem and give the CowToken and MaxxiaScroll, then set the cooldown timer
        Item gargishTotem = player.Backpack.FindItemByType(typeof(GargishTotem));
        if (gargishTotem != null)
        {
            gargishTotem.Delete();
            player.AddToBackpack(new CowToken());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the GargishTotem and receive a CowToken and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a GargishTotem.");
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