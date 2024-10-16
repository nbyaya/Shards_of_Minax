using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class KalidorTheTrickster : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public KalidorTheTrickster() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Kalidor the Trickster";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(85);
        SetInt(95);

        SetHits(100);
        SetMana(120);
        SetStam(85);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new FancyShirt(1150)); // A flashy dark red shirt
        AddItem(new LongPants(909)); // Stylish dark pants
        AddItem(new Boots(1175)); // Lightly colored boots
        AddItem(new FloppyHat(1102)); // A flamboyant green hat
        AddItem(new Cloak(1130)); // A deep purple cloak for extra flair

        VirtualArmor = 20;
    }

    public KalidorTheTrickster(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! I am Kalidor, the master of all things mysterious and mischievous! Have you come to play a trick... or perhaps make a trade?");

        // Introduce his trickster background and the trade option
        greeting.AddOption("What kind of tricks do you do?",
            p => true,
            p =>
            {
                DialogueModule tricksModule = new DialogueModule("I am a master of sleight of hand and illusions! Cards, smoke, and mirrors are my tools, but sometimes... I dabble in trades. Something intriguing, perhaps?");
                tricksModule.AddOption("What kind of trade?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tradeIntroductionModule = new DialogueModule("Ah, well, if you have a Deck of Magic Cards, I might just have something interesting for you! I'll even throw in a MaxxiaScroll to sweeten the deal.");
                        tradeIntroductionModule.AddOption("I'd like to make the trade.",
                            pla => CanTradeWithPlayer(pla),
                            pla =>
                            {
                                DialogueModule tradeModule = new DialogueModule("Do you have a Deck of Magic Cards for me?");
                                tradeModule.AddOption("Yes, I have a Deck of Magic Cards.",
                                    plaa => HasDeckOfMagicCards(plaa) && CanTradeWithPlayer(plaa),
                                    plaa =>
                                    {
                                        CompleteTrade(plaa);
                                    });
                                tradeModule.AddOption("No, I don't have one right now.",
                                    plaa => !HasDeckOfMagicCards(plaa),
                                    plaa =>
                                    {
                                        plaa.SendMessage("Come back when you have a Deck of Magic Cards.");
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
                tricksModule.AddOption("Why do you do all this?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("Why, you ask? I wasn't always the trickster you see before you. Once, I was Kalidor of the noble House of Vance. Wealth, power, respect... it was all mine. But family, pride, and betrayal shattered my world. Now, I live in the shadows, with only tricks and schemes to keep me company.");
                        pastModule.AddOption("What happened to your family?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule familyModule = new DialogueModule("Ah, my family. They betrayed me. I was once their golden child, their heir. But power makes people jealous, even those closest to you. My own brother, an envious, conniving snake, orchestrated my fall. He turned them all against me, whispered poison in their ears. In the end, they cast me out, disowned me as if I were nothing.");
                                familyModule.AddOption("Do you ever think of going back?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule returnModule = new DialogueModule("Go back? Hah! I have thought about it. Every day, I dream of returning. Not as the prodigal son, though. No, I dream of vengeance. Of showing them what they lost, and what they made of me. Let them tremble at the sight of Kalidor, the Trickster of the Shadows.");
                                        returnModule.AddOption("Vengeance? That sounds dangerous.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule dangerModule = new DialogueModule("Dangerous? Yes, perhaps. But when you have nothing left, what is danger but a friend? I have learned much in the shadows. The noble houses play their games, but they have never faced one like me. One who knows their secrets, who walks unseen, who strikes when least expected.");
                                                dangerModule.AddOption("I hope you find peace.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Kalidor's eyes soften momentarily. 'Peace? Perhaps one day, when all debts are settled.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                dangerModule.AddOption("I hope you get your revenge.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Kalidor's eyes gleam with intensity. 'Revenge is all I have left. I will see it through.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, dangerModule));
                                            });
                                        returnModule.AddOption("Perhaps it's best to leave the past behind.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Kalidor scoffs. 'Leave it behind? The past is all I have. I cannot simply forget.'");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, returnModule));
                                    });
                                familyModule.AddOption("Your brother sounds terrible.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule brotherModule = new DialogueModule("Terrible? He is far worse than that. He has always wanted what I had. Now, he sits at the head of House Vance, wearing my title, spending my fortune. But I bide my time. The higher he climbs, the harder he will fall.");
                                        brotherModule.AddOption("Sounds like you're planning something.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule planModule = new DialogueModule("Oh, I am always planning. I have allies, even if they do not know it. The servants, the guards, the discontented. They talk to me, even when they do not realize it. One day, all the whispers will come together, and my brother will face the reckoning he deserves.");
                                                planModule.AddOption("I wish you luck.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Kalidor nods, a smile playing on his lips. 'Thank you, traveler. Luck favors the bold, and I am nothing if not bold.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                planModule.AddOption("Be careful, Kalidor.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendMessage("Kalidor chuckles. 'Careful? Where's the fun in that? But I appreciate your concern.'");
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, planModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, brotherModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, familyModule));
                            });
                        pastModule.AddOption("That sounds like a lot of pain.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule painModule = new DialogueModule("Pain? Yes, it is. But pain makes us who we are, does it not? It sharpens us, molds us. Without it, I would still be Kalidor the Noble, blind to the truth of the world. Now, I see clearly.");
                                painModule.AddOption("Perhaps there's still good in the world.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Kalidor pauses, his expression unreadable. 'Perhaps. But it is well hidden.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                painModule.AddOption("You seem to have found your own way.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Kalidor smiles slightly. 'Indeed, I have. It may not be the way of nobility, but it is mine.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, painModule));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });
                p.SendGump(new DialogueGump(p, tricksModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Kalidor smirks and gives you a theatrical bow.");
            });

        return greeting;
    }

    private bool HasDeckOfMagicCards(PlayerMobile player)
    {
        // Check the player's inventory for DeckOfMagicCards
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(DeckOfMagicCards)) != null;
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
        // Remove the DeckOfMagicCards and give the GarbageBag, then set the cooldown timer
        Item deckOfMagicCards = player.Backpack.FindItemByType(typeof(DeckOfMagicCards));
        if (deckOfMagicCards != null)
        {
            deckOfMagicCards.Delete();
            player.AddToBackpack(new GarbageBag());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Deck of Magic Cards and receive a GarbageBag and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Deck of Magic Cards.");
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