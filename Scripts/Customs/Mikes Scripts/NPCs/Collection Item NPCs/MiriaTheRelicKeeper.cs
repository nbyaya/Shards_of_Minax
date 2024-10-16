using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MiriaTheRelicKeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MiriaTheRelicKeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Miria the Relic Keeper";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = 500;

        // Outfit
        AddItem(new FancyDress(1153)); // Dress with a blue hue
        AddItem(new Sandals(1157)); // Sandals with a dark blue hue
        AddItem(new GoldBracelet()); // Adds a touch of elegance
        AddItem(new Lantern()); // She carries a lantern

        VirtualArmor = 15;
    }

    public MiriaTheRelicKeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Miria, keeper of forgotten relics. I see you have an interest in history and artifacts. Do you seek a relic of your own?");

        // Introduction dialogue with options for deeper conversation
        greeting.AddOption("Tell me about your relics.",
            p => true,
            p =>
            {
                DialogueModule relicModule = new DialogueModule("I guard items of old, collected over years of journeying across lands unknown. Each piece has a story to tell. Perhaps you have something of interest to me as well?");

                // Nested dialogue options about the relics
                relicModule.AddOption("What kind of relics do you collect?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule typesModule = new DialogueModule("I collect relics that whisper of forgotten stories - tools from old civilizations, magical trinkets, and artifacts of historical significance. Each relic has a tale that needs to be protected.");

                        // Further nested dialogue
                        typesModule.AddOption("Can you tell me more about these old civilizations?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule civilizationsModule = new DialogueModule("Ah, the civilizations of old. They were cautious, like myself, and preferred solitude. They built in harmony with nature, understanding that there is a balance. I relate to them deeply; as a hermit, I too find comfort in isolation and self-reliance.");

                                civilizationsModule.AddOption("Why do you prefer solitude?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule solitudeModule = new DialogueModule("In solitude, there is peace. I have seen too much conflict in my travels. People can be unpredictable, driven by greed or fear. In my valley, surrounded by my crops, I can live simply. Away from prying eyes, away from danger.");

                                        solitudeModule.AddOption("What crops do you grow?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule cropsModule = new DialogueModule("I grow what I need to survive: herbs for healing, roots for nourishment, and a few rare flowers that thrive in the valley's isolated climate. The valley is my sanctuary, and the earth provides all I need.");

                                                cropsModule.AddOption("Do you ever share your harvest?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule shareModule = new DialogueModule("Rarely. I avoid contact when I can. But if a traveler is truly in need, I may share. I am cautious, you see. Trust must be earned, and only those who respect the land and its balance can hope to gain my trust.");
                                                        shareModule.AddOption("How can I earn your trust?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule trustModule = new DialogueModule("Patience, respect, and caution. These are traits I value. If you can show me that you tread lightly on this earth, that you do not seek conflict, then perhaps one day I will share more of what I know.");
                                                                trustModule.AddOption("I understand. Thank you for your wisdom.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, trustModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, shareModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, cropsModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, solitudeModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, civilizationsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, typesModule));
                    });

                // Trade dialogue options
                relicModule.AddOption("What are you looking for?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("I am searching for a WeddingCandelabra. Bring me one, and I shall reward you with a BrassBell, along with a MaxxiaScroll for your knowledge and efforts. However, my resources are limited, and I can only offer this trade once every ten minutes.");

                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a WeddingCandelabra for me?");

                                tradeModule.AddOption("Yes, I have a WeddingCandelabra.",
                                    plaa => HasWeddingCandelabra(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });

                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasWeddingCandelabra(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a WeddingCandelabra.");
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

                p.SendGump(new DialogueGump(p, relicModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Miria nods politely, her eyes glinting with mystery.");
            });

        return greeting;
    }

    private bool HasWeddingCandelabra(PlayerMobile player)
    {
        // Check the player's inventory for WeddingCandelabra
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WeddingCandelabra)) != null;
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
        // Remove the WeddingCandelabra and give the BrassBell, then set the cooldown timer
        Item weddingCandelabra = player.Backpack.FindItemByType(typeof(WeddingCandelabra));
        if (weddingCandelabra != null)
        {
            weddingCandelabra.Delete();
            player.AddToBackpack(new BrassBell());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the WeddingCandelabra and receive a BrassBell and a MaxxiaScroll in return. Thank you for your help, traveler.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a WeddingCandelabra.");
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