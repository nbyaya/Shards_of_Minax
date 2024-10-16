using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class KallistaTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KallistaTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kallista the Alchemist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(50);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Dark blue robe
        AddItem(new Boots(2109)); // Dark leather boots
        AddItem(new PunishmentStocks()); // Unique eyewear item
        AddItem(new LeatherGloves());
        AddItem(new Backpack()); // Contains her research items

        VirtualArmor = 15;
    }

    public KallistaTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Kallista, an alchemist seeking rare components for my experiments. Are you perhaps interested in a trade?");

        // Dialogue about her alchemy work
        greeting.AddOption("What kind of experiments are you working on?",
            p => true,
            p =>
            {
                DialogueModule experimentsModule = new DialogueModule("I experiment with the essence of creatures and plants alike. From Reactive Hormones to the distillate of the Moonvine, each holds untold potential. I am particularly interested in acquiring rare components like the ReactiveHormones.");
                experimentsModule.AddOption("Do you need ReactiveHormones?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Ah, indeed! If you have ReactiveHormones, I can offer you PunishmentStocks and a MaxxiaScroll as a reward. Shall we proceed?");
                        tradeModule.AddOption("Yes, I have ReactiveHormones.",
                            plaa => HasReactiveHormones(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have it right now.",
                            plaa => !HasReactiveHormones(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have ReactiveHormones, and I shall make it worth your while.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        tradeModule.AddOption("I traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, tradeModule));
                    });

                experimentsModule.AddOption("What drives you to do these experiments?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule motivationModule = new DialogueModule("My drive? It is not merely curiosity, traveler. It is devotion. I have seen visions—dark prophecies that foretold a coming age of shadows. These experiments are my attempt to understand and prevent it. The heretical elements in this world must be purged if we are to see the dawn of a new, pure age.");
                        motivationModule.AddOption("What kind of visions have you seen?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule visionsModule = new DialogueModule("The visions are unsettling—cities swallowed by darkness, people consumed by their own greed and sins, the world torn asunder by heretical magic. The heavens cry out for justice, and I know it is my duty to root out the corruption. Only then can we pave the way for a new age of light and order.");
                                visionsModule.AddOption("That sounds terrifying. How do you plan to prevent it?",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule preventModule = new DialogueModule("Terrifying, yes, but also galvanizing. I plan to prevent it by arming myself and others with knowledge. The alchemical process is a tool—a weapon, if you will. The right concoctions can reveal the truth, cleanse impurities, and purge the corrupt. I will stop at nothing until every heretic is either redeemed or removed.");
                                        preventModule.AddOption("That sounds ruthless.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                DialogueModule ruthlessModule = new DialogueModule("Ruthless? Perhaps. But it is the ruthlessness of a healer cutting away the rot to save the patient. The world is ill, and I must be its cure. Mercy to the heretic is cruelty to the innocent. Only through this cleansing can we hope to create a future worth living.");
                                                ruthlessModule.AddOption("I understand. You are truly devoted.",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        plaabcd.SendMessage("Kallista nods solemnly, her eyes burning with conviction.");
                                                        plaabcd.SendGump(new DialogueGump(plaabcd, CreateGreetingModule(plaabcd)));
                                                    });
                                                ruthlessModule.AddOption("This is too much for me. Farewell.",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        plaabcd.SendMessage("Kallista regards you coldly as you leave. 'The weak often fear what they do not understand,' she murmurs.");
                                                    });
                                                plaabc.SendGump(new DialogueGump(plaabc, ruthlessModule));
                                            });
                                        preventModule.AddOption("I don't think I can support this.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                plaabc.SendMessage("Kallista narrows her eyes. 'Then you are either part of the solution or part of the problem. Choose wisely, traveler.'");
                                                plaabc.SendGump(new DialogueGump(plaabc, CreateGreetingModule(plaabc)));
                                            });
                                        plaab.SendGump(new DialogueGump(plaab, preventModule));
                                    });
                                visionsModule.AddOption("I must leave now.",
                                    plaab => true,
                                    plaab =>
                                    {
                                        plaab.SendMessage("Kallista nods, her expression darkened by the weight of her visions.");
                                    });
                                plaa.SendGump(new DialogueGump(plaa, visionsModule));
                            });
                        motivationModule.AddOption("I can see your devotion. Let's continue.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, motivationModule));
                    });
                p.SendGump(new DialogueGump(p, experimentsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Kallista nods, returning to her alchemical notes.");
            });

        return greeting;
    }

    private bool HasReactiveHormones(PlayerMobile player)
    {
        // Check the player's inventory for ReactiveHormones
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ReactiveHormones)) != null;
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
        // Remove the ReactiveHormones and give the PunishmentStocks, then set the cooldown timer
        Item reactiveHormones = player.Backpack.FindItemByType(typeof(ReactiveHormones));
        if (reactiveHormones != null)
        {
            reactiveHormones.Delete();
            player.AddToBackpack(new PunishmentStocks());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ReactiveHormones and receive PunishmentStocks and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have ReactiveHormones.");
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