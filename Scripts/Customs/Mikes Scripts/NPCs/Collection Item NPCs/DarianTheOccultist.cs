using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class DarianTheOccultist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public DarianTheOccultist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Darian the Occultist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new Robe(1109)); // Dark robe
        AddItem(new Boots(1175)); // Black boots
        AddItem(new WizardsHat(1109)); // Dark wizard's hat
        AddItem(new GoldRing()); // A ring to add a bit of flair

        VirtualArmor = 15;
    }

    public DarianTheOccultist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Darian, a seeker of the forbidden arts. Have you come to learn of the arcane, or perhaps to barter with the unknown?");

        // Initial conversation options
        greeting.AddOption("Tell me about your studies.",
            p => true,
            p =>
            {
                DialogueModule studiesModule = new DialogueModule("The occult is not for the faint of heart. I delve into secrets that others fear, communing with spirits, harnessing demonic energy, and exploring the limits of the human mind. But such knowledge comes at a cost...");
                studiesModule.AddOption("What kind of cost?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule costModule = new DialogueModule("The cost is both physical and spiritual. Those who venture too far risk their very soul. But with great risk comes great reward.");
                        costModule.AddOption("Fascinating, yet terrifying.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule rewardModule = new DialogueModule("Ah, but you see, fear and fascination are merely two sides of the same coin. Those willing to push past the fear often find power beyond imagination. Of course, there are those who lose themselves entirely, their spirits twisted into something... unrecognizable.");
                                rewardModule.AddOption("What kind of power are we talking about?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule powerModule = new DialogueModule("Power to bend reality, to command lesser beings, to walk the threshold between life and death. I have learned to summon spirits, control minds, and even glimpse into the future. But these things require sacrifice and, of course, a keen mind willing to take risks others would not.");
                                        powerModule.AddOption("Can you teach me some of this power?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule teachModule = new DialogueModule("Teach you? Hmm... I do not teach for free, nor do I waste my time with the weak-willed. If you wish to learn, you must prove your worth to me. Bring me something rare, something valuable, something that speaks of your commitment.");
                                                teachModule.AddOption("What kind of item would suffice?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule itemModule = new DialogueModule("Ah, a true seeker! I desire items of rarity and power. Artifacts, cursed trinkets, or even the heart of a powerful beast. You must show me that you are willing to make sacrifices. I care not how you obtain it, only that you do.");
                                                        itemModule.AddOption("I'll see what I can find.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, itemModule));
                                                    });
                                                teachModule.AddOption("This sounds too dangerous for me.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, teachModule));
                                            });
                                        powerModule.AddOption("Sounds like too much risk for me.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, powerModule));
                                    });
                                rewardModule.AddOption("I think I'll pass on that.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, rewardModule));
                            });
                        pl.SendGump(new DialogueGump(pl, costModule));
                    });
                studiesModule.AddOption("I think I'll stick to simpler magic.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, studiesModule));
            });

        // Option to initiate the trade
        greeting.AddOption("Do you need any items?",
            p => true,
            p =>
            {
                DialogueModule tradeIntroModule = new DialogueModule("Ah, indeed. I am searching for something rather specific: a CompoundF. If you bring it to me, I can offer you a DemonPlatter in exchange, as well as a MaxxiaScroll. But such a trade can only happen once every ten minutes.");
                tradeIntroModule.AddOption("I'd like to make the trade.",
                    pla => CanTradeWithPlayer(pla),
                    pla =>
                    {
                        DialogueModule tradeModule = new DialogueModule("Do you have a CompoundF for me?");
                        tradeModule.AddOption("Yes, I have a CompoundF.",
                            plaa => HasCompoundF(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });
                        tradeModule.AddOption("No, I don't have one right now.",
                            plaa => !HasCompoundF(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a CompoundF.");
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
                tradeIntroModule.AddOption("Perhaps another time.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroModule));
            });

        // Nested dialogue to show Darian's greedy and shrewd personality
        greeting.AddOption("Why are you interested in CompoundF?",
            p => true,
            p =>
            {
                DialogueModule interestModule = new DialogueModule("Ah, you wish to know my secrets? CompoundF is not just a simple reagent. It holds the potential to amplify certain dark rituals, making them far more potent. Of course, I could tell you more... for the right price.");
                interestModule.AddOption("What price are we talking about?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule priceModule = new DialogueModule("Gold, rare gems, or items of significant magical power. I do not deal in petty things. The knowledge I possess comes at a steep cost, and I only share it with those who understand the value of what I offer.");
                        priceModule.AddOption("I have some gold. How much do you need?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule goldModule = new DialogueModule("Ah, gold. The lifeblood of any true deal. For a mere 10,000 gold coins, I could share a minor secret of the occult with you. For something more significant, well, we'll have to discuss a larger sum.");
                                goldModule.AddOption("10,000 gold for a minor secret? That's outrageous!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Darian smirks. 'Outrageous? Perhaps. But knowledge is worth whatever price one is willing to pay.'");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                goldModule.AddOption("Alright, I'll pay 10,000 gold.",
                                    plaa => HasEnoughGold(plaa, 10000),
                                    plaa =>
                                    {
                                        DeductGold(plaa, 10000);
                                        plaa.SendMessage("Darian takes the gold and whispers an arcane secret into your ear. You feel a chill run down your spine.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, goldModule));
                            });
                        priceModule.AddOption("I don't have anything like that.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, priceModule));
                    });
                interestModule.AddOption("I'm not interested in paying for secrets.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, interestModule));
            });

        // Goodbye option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Darian nods knowingly, his eyes glinting with dark secrets.");
            });

        return greeting;
    }

    private bool HasCompoundF(PlayerMobile player)
    {
        // Check the player's inventory for CompoundF
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(CompoundF)) != null;
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
        // Remove the CompoundF and give the DemonPlatter and MaxxiaScroll, then set the cooldown timer
        Item compoundF = player.Backpack.FindItemByType(typeof(CompoundF));
        if (compoundF != null)
        {
            compoundF.Delete();
            player.AddToBackpack(new DemonPlatter());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the CompoundF and receive a DemonPlatter and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a CompoundF.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
    }

    private bool HasEnoughGold(PlayerMobile player, int amount)
    {
        return player.Backpack != null && player.Backpack.GetAmount(typeof(Gold)) >= amount;
    }

    private void DeductGold(PlayerMobile player, int amount)
    {
        if (player.Backpack != null)
        {
            player.Backpack.ConsumeTotal(typeof(Gold), amount);
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