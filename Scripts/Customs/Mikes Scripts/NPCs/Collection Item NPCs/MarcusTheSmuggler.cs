using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MarcusTheSmuggler : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MarcusTheSmuggler() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Marcus the Smuggler";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(90);
        SetInt(60);

        SetHits(100);
        SetMana(50);
        SetStam(90);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new FancyShirt(2253)); // Dark blue fancy shirt
        AddItem(new LongPants(1109)); // Dark grey pants
        AddItem(new Boots(1175)); // Black boots
        AddItem(new TricorneHat(1175)); // Black tricorne hat
        AddItem(new SmugglersCrate()); // A unique ring to symbolize his smuggler status

        VirtualArmor = 15;
    }

    public MarcusTheSmuggler(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Psst, over here! The name's Marcus. I deal in rare and... let’s say 'questionably acquired' goods. You look like someone who appreciates a good trade. Interested?");

        // Start with dialogue about his work
        greeting.AddOption("What kind of goods do you deal with?",
            p => true,
            p =>
            {
                DialogueModule goodsModule = new DialogueModule("Oh, I've got my hands on all sorts of treasures. But today, I need something specific. You wouldn't happen to have a Large Tome, would you?");
                
                goodsModule.AddOption("What do you need the Large Tome for?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule needTomeModule = new DialogueModule("A client of mine is paying good gold for some... 'old knowledge'. If you bring me a Large Tome, I'll reward you handsomely. In fact, I've got a Smuggler's Crate just waiting for a trade.");
                        needTomeModule.AddOption("Alright, let's make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have the Large Tome for me?");
                                tradeModule.AddOption("Yes, I have a Large Tome.",
                                    plaa => HasLargeTome(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have it right now.",
                                    plaa => !HasLargeTome(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have the Large Tome, and we'll do business.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                tradeModule.AddOption("I traded recently; I'll come back later.",
                                    plaa => !CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Easy there, friend. I can't be doing trades every other minute. Give it some time - ten minutes, at least.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, tradeModule));
                            });

                        needTomeModule.AddOption("Why do you want it so badly?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule manipulationModule = new DialogueModule("Ah, you're sharp. Let me tell you this - the world runs on secrets. My client? Well, let's just say they're looking for answers about someone they lost. Something that might help them sleep better at night. And you, friend, could be part of that 'comfort'.");
                                manipulationModule.AddOption("You're helping the grieving?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule griefModule = new DialogueModule("You could say that. But grief, it's a powerful thing, you know? People want to believe, they want to hope, and when they do, well... it opens all sorts of doors. I'm merely helping them... see what they want to see.");
                                        griefModule.AddOption("That sounds deceitful...",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule deceitModule = new DialogueModule("Deceit? No, no. Think of it as storytelling. I give them a story that helps them live another day. And in return, they pay me for my... 'services'. Besides, everyone needs a little illusion now and then, don't they?");
                                                deceitModule.AddOption("I suppose... everyone likes a good story.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Marcus grins, his eyes glinting. 'Exactly. You get it, friend. Now, about that Tome...'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                deceitModule.AddOption("No, I can't be a part of this.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Marcus sighs dramatically. 'Ah well, not everyone has the stomach for the work I do. Maybe another time, friend.'");
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, deceitModule));
                                            });
                                        griefModule.AddOption("I suppose it's harmless...",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marcus nods, smiling. 'Exactly. Harmless. Just a little comfort for those who need it.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, griefModule));
                                    });
                                manipulationModule.AddOption("That seems shady.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Marcus shrugs nonchalantly. 'Shady? Perhaps. But aren't all the best deals done in the shadows?'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, manipulationModule));
                            });

                        needTomeModule.AddOption("Maybe later.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, needTomeModule));
                    });

                goodsModule.AddOption("What else do you do besides trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Oh, I do a little bit of this and a little bit of that. Truth is, I've got a knack for speaking to the dearly departed... or at least, making people think I do.");
                        backstoryModule.AddOption("You communicate with the dead?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule mediumModule = new DialogueModule("Ha! 'Communicate' is a strong word. Let's say I know how to put on a show. People believe what they want to believe, and I provide the performance. Grieving hearts are vulnerable, and they cling to hope like a drowning man clings to driftwood.");
                                mediumModule.AddOption("That's cruel, Marcus.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule cruelModule = new DialogueModule("Cruel? Maybe. But people pay me for that hope. They need it. And honestly, sometimes it's the only thing keeping them from giving in to despair. I give them what they need, even if it's not real.");
                                        cruelModule.AddOption("I guess everyone needs something to hold on to.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marcus gives a dramatic sigh. 'Exactly, my friend. Exactly. Now, are you ready to talk business, or shall we keep philosophizing?'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        cruelModule.AddOption("I can't condone this.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marcus shrugs. 'Suit yourself. Not everyone has the stomach for my line of work.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, cruelModule));
                                    });
                                mediumModule.AddOption("Do they really believe you?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Marcus grins. 'Oh, you'd be surprised. People see what they want to see, and I give them just enough to make them believe. It's all about the drama, the spectacle.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, mediumModule));
                            });

                        backstoryModule.AddOption("Why do you do it?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule reasonModule = new DialogueModule("Why? Now there's a question. Let's just say I've got my own ghosts to deal with. And maybe, just maybe, pretending to help others lets me forget about my own troubles for a while.");
                                reasonModule.AddOption("What happened to you?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule pastModule = new DialogueModule("Ah, now you're digging deep. Let's just say I lost someone too. Someone I couldn't help. And since then, I've been trying to fill that hole, one way or another.");
                                        pastModule.AddOption("I'm sorry to hear that.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marcus looks away for a moment, his expression darkening. 'Yeah, well... life goes on, doesn't it?'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        pastModule.AddOption("That's no excuse for what you do.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Marcus narrows his eyes. 'Maybe not. But it's the only way I know how to keep going.'");
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, pastModule));
                                    });
                                reasonModule.AddOption("Maybe you're not as heartless as you seem.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Marcus gives a half-smile. 'Don't go spreading that around. I've got a reputation to maintain.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, reasonModule));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, goodsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Marcus smirks and nods. 'Stay out of trouble... or don't.'");
            });

        return greeting;
    }

    private bool HasLargeTome(PlayerMobile player)
    {
        // Check the player's inventory for Large Tome
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(LargeTome)) != null;
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
        // Remove the Large Tome and give the Smuggler's Crate and MaxxiaScroll, then set the cooldown timer
        Item largeTome = player.Backpack.FindItemByType(typeof(LargeTome));
        if (largeTome != null)
        {
            largeTome.Delete();
            player.AddToBackpack(new SmugglersCrate());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Large Tome and receive a Smuggler's Crate and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the Large Tome.");
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