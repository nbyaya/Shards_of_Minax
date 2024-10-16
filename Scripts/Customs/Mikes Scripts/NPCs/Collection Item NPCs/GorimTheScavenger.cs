using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class GorimTheScavenger : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public GorimTheScavenger() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Gorim the Scavenger";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(80);

        SetHits(90);
        SetMana(100);
        SetStam(60);

        Fame = 0;
        Karma = -1000;

        // Outfit
        AddItem(new FancyShirt(1109)); // Dark gray fancy shirt
        AddItem(new LongPants(1175)); // Dark green pants
        AddItem(new Boots(1109)); // Black boots
        AddItem(new Cloak(1157)); // Deep red cloak
        AddItem(new Bandana(1153)); // Dark blue bandana
        AddItem(new Dagger()); // Worn on his belt, symbolizing his scavenger nature

        VirtualArmor = 15;
    }

    public GorimTheScavenger(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Oi, traveler! You look like someone who appreciates the rare and unusual. The name's Gorim. I deal in things others throw away or can't find. Interested in a trade?");

        // Start with dialogue about his work
        greeting.AddOption("What kind of trades do you offer?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroModule = new DialogueModule("I specialize in rare items, things that people don't even know they need until they see 'em. Right now, I'm looking for something very specific: MeatHooks. Got one on ya?");

                // Introduce the trade option
                tradeIntroModule.AddOption("I have a MeatHooks. What can you offer?",
                    pl => HasMeatHooks(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                tradeIntroModule.AddOption("I don't have a MeatHooks.",
                    pl => !HasMeatHooks(pl),
                    pl =>
                    {
                        pl.SendMessage("Well, come back when you find a set of MeatHooks. I'll be here.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                tradeIntroModule.AddOption("I traded with you recently.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("You can only trade once every 10 minutes. Come back later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });

                tradeIntroModule.AddOption("Maybe another time.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Suit yourself, but don't take too long. I might find another buyer.");
                    });

                tradeIntroModule.AddOption("Tell me more about yourself.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, curious, are ya? I wasn't always like this, rummaging through the detritus of others. Once, I was Gorim the Brilliant, a scholar, an academic of renown. But that world is gone now, buried under greed and folly.");

                        backstoryModule.AddOption("What happened to you?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule downfallModule = new DialogueModule("What happened? Hah! The world happened. I sought knowledge too eagerly, delved too deeply into ancient texts, secrets that others wanted kept buried. I became obsessed—an obsession that cost me everything. My colleagues turned their backs, called me mad. I found myself alone, exiled, hiding in a bunker filled with scrolls and relics of a bygone age.");

                                downfallModule.AddOption("Why ancient technology?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule techModule = new DialogueModule("Ancient technology is the key, don't you see? We could restore what was lost—bring back the light, the warmth, the civilization! The fools in power never understood. They preferred ignorance, called me dangerous. But I know the truth: without the old knowledge, we're doomed to scrabble in the dirt forever.");

                                        techModule.AddOption("Do you still search for it?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule searchModule = new DialogueModule("Of course I still search! Every trinket, every shard of knowledge I find is a step closer to understanding the ancients. The bunker I live in is filled with relics—some broken, some almost functional. I'm getting close, but I need more, always more. That's why I need things like the MeatHooks—tiny pieces of a larger puzzle.");

                                                searchModule.AddOption("Can I help you in your quest?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule helpModule = new DialogueModule("Help? Hah! Not many offer that. If you bring me ancient components, things others consider junk, perhaps I could make use of them. Just know this: I trust no one easily. You bring me things of value, and maybe, just maybe, I'll let you in on a secret or two.");
                                                        helpModule.AddOption("I'll see what I can find.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Gorim gives you a curt nod, his eyes gleaming with a hint of hope.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        helpModule.AddOption("Sounds too risky for me.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Suit yourself. The path of knowledge is not for the faint of heart.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, helpModule));
                                                    });

                                                searchModule.AddOption("Why are you so antisocial?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule antisocialModule = new DialogueModule("Antisocial? Heh, that's one way of putting it. People are... distractions. They ask too many questions, pry into things they shouldn't. Most don't understand what I'm doing, and those who do usually want to steal it. I've learned to rely only on myself—trust is a luxury I can't afford.");
                                                        antisocialModule.AddOption("I can understand that.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Gorim gives you an approving nod, a rare expression of respect.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        antisocialModule.AddOption("That must be lonely.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendMessage("Lonely? Maybe. But loneliness is the price of brilliance, the price of knowledge. It's a price I'm willing to pay.");
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, antisocialModule));
                                                    });

                                                plaaaa.SendGump(new DialogueGump(plaaaa, searchModule));
                                            });

                                        techModule.AddOption("Why did they call you dangerous?",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule dangerousModule = new DialogueModule("Dangerous? Ha! Only because I was willing to go further than any of them. Knowledge is dangerous, especially when it threatens those in power. They feared what I could uncover, what I could build. The ancients had powers beyond our imagination, and the leaders of today fear losing their control if those powers return.");
                                                dangerousModule.AddOption("I see your point.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Gorim grins, clearly pleased with your understanding.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                dangerousModule.AddOption("Maybe they had a reason to fear.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendMessage("Perhaps. But true progress comes with risk. Without it, we're nothing.");
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, dangerousModule));
                                            });

                                        plaaa.SendGump(new DialogueGump(plaaa, techModule));
                                    });

                                downfallModule.AddOption("Did you ever regret it?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regret? Every day. But not because I was wrong—no, because I wasn't strong enough to see it through. I let their voices get to me, let their judgments drive me away. Now, I have nothing but my quest. But that's enough for me.");
                                        regretModule.AddOption("I understand your resolve.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Gorim looks at you, a glimmer of appreciation in his usually cold eyes.");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        regretModule.AddOption("That sounds painful.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Painful, yes. But pain is part of the process. Only through struggle do we grow.");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, regretModule));
                                    });

                                plaa.SendGump(new DialogueGump(plaa, downfallModule));
                            });

                        backstoryModule.AddOption("Why did you become obsessed?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule obsessionModule = new DialogueModule("Because I saw the potential! Imagine a world where the ancient technology works again, where we could build cities that shine at night, heal the gravest of wounds, and travel distances in the blink of an eye. How could anyone see that and not become obsessed? It became my mission, my purpose.");
                                obsessionModule.AddOption("I can see why you'd be driven.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Gorim's eyes gleam with a manic intensity. 'Exactly! You understand, at least a little.'");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                obsessionModule.AddOption("That sounds dangerous.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Dangerous? Ha! Only to those who fear progress.");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, obsessionModule));
                            });

                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, tradeIntroModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Gorim nods and returns to rummaging through his pack.");
            });

        return greeting;
    }

    private bool HasMeatHooks(PlayerMobile player)
    {
        // Check the player's inventory for MeatHooks
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(MeatHooks)) != null;
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
        // Remove the MeatHooks and give the RareMinerals and MaxxiaScroll, then set the cooldown timer
        Item meatHooks = player.Backpack.FindItemByType(typeof(MeatHooks));
        if (meatHooks != null)
        {
            meatHooks.Delete();
            player.AddToBackpack(new RareMinerals());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the MeatHooks and receive RareMinerals and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the MeatHooks.");
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