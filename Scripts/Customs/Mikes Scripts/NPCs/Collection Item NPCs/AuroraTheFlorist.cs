using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class AuroraTheFlorist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public AuroraTheFlorist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aurora the Florist";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(40);
        SetDex(50);
        SetInt(120);

        SetHits(70);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(153)); // Dress with a lavender hue
        AddItem(new Sandals(117)); // Sandals with a pink hue
        AddItem(new Bonnet(173)); // A colorful bonnet
        AddItem(new FlowerGarland()); // A floral headpiece

        VirtualArmor = 10;
    }

    public AuroraTheFlorist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I am Aurora, a florist with a love for celebrations and beautiful blooms. Have you come to brighten your day with flowers, or perhaps you have something special for me?");

        // Start with dialogue about her work
        greeting.AddOption("Tell me about your flowers.",
            p => true,
            p =>
            {
                DialogueModule flowersModule = new DialogueModule("Flowers have the power to convey emotions beyond words. Each bloom tells a story of love, hope, or remembrance. My favorite is the Celebration Lily, which blossoms only during joyful gatherings.");
                flowersModule.AddOption("Tell me more about the Celebration Lily.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule lilyModule = new DialogueModule("The Celebration Lily is a rare flower that blooms during weddings or festivals. Its petals are said to bring good fortune to those who cherish them, and I use them to craft special charms for those who bring me something unique.");
                        lilyModule.AddOption("How fascinating! Thank you.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        lilyModule.AddOption("What kind of charms do you make?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule charmModule = new DialogueModule("Ah, the charms I make are not merely decorative. They hold subtle enchantments that can enhance one's fortune or even protect against ill fate. Of course, these charms are reserved for those who can offer something valuable in return.");
                                charmModule.AddOption("What do you mean by 'something valuable'?",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule valuableModule = new DialogueModule("Well, you see, not everything has a price in gold. I seek rare items, trinkets of sentimental value, or objects with stories attached to them. You wouldn't believe how much people are willing to part with for a simple flower when they think it brings luck.");
                                        valuableModule.AddOption("That sounds rather... cunning.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                DialogueModule cunningModule = new DialogueModule("Cunning? Perhaps. But isn't it all just part of the game, my dear? Those who want something badly enough will pay the price, and I merely provide the opportunity. After all, who am I to deny someone a little happiness?");
                                                cunningModule.AddOption("I suppose you're right.",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                    });
                                                cunningModule.AddOption("You're deceiving people, aren't you?",
                                                    plaaaaa => true,
                                                    plaaaaa =>
                                                    {
                                                        DialogueModule deceiveModule = new DialogueModule("Deceiving? Such a harsh word. I prefer to think of it as giving people what they want—at a cost they are willing to pay. Everyone leaves happy, and no one gets hurt... usually.");
                                                        deceiveModule.AddOption("I see. Well, good luck with your business.",
                                                            plaaaaaa => true,
                                                            plaaaaaa =>
                                                            {
                                                                plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                            });
                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, deceiveModule));
                                                    });
                                                plaaaa.SendGump(new DialogueGump(plaaaa, cunningModule));
                                            });
                                        valuableModule.AddOption("Sounds intriguing. Perhaps I have something for you.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendMessage("Aurora smiles slyly, her eyes glinting with interest.");
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, valuableModule));
                                    });
                                charmModule.AddOption("Perhaps another time.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, charmModule));
                            });
                        pl.SendGump(new DialogueGump(pl, lilyModule));
                    });
                flowersModule.AddOption("Thank you, but I have other questions.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, flowersModule));
            });

        // Trade dialogue
        greeting.AddOption("I have a WeddingDayCake.",
            p => HasWeddingDayCake(p) && CanTradeWithPlayer(p),
            p =>
            {
                CompleteTrade(p);
            });
        greeting.AddOption("I have a WeddingDayCake, but I traded recently.",
            p => HasWeddingDayCake(p) && !CanTradeWithPlayer(p),
            p =>
            {
                p.SendMessage("You can only trade once every 10 minutes. Please return later.");
                p.SendGump(new DialogueGump(p, CreateGreetingModule(p)));
            });

        // More cunning dialogue
        greeting.AddOption("You seem quite charming, but there's something... more about you, isn't there?",
            p => true,
            p =>
            {
                DialogueModule cunningDialogue = new DialogueModule("Ah, you have a keen eye, traveler. I may look like a simple florist, but appearances can be deceiving. You see, I have a knack for finding opportunities wherever they bloom. Some call it cunning, but I call it survival.");
                cunningDialogue.AddOption("What kind of opportunities?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule opportunityModule = new DialogueModule("Opportunities to make a profit, to gain influence, or to simply stay one step ahead of those who might wish me ill. I have posed as many things—a noble, a merchant, even a healer—to achieve my goals. All in pursuit of the ultimate prize.");
                        opportunityModule.AddOption("What ultimate prize?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule prizeModule = new DialogueModule("Ah, now that is the real question, isn't it? Power, wealth, freedom... Perhaps all three. Or perhaps something more elusive—a chance to rewrite one's destiny. And what about you, traveler? What is it that you seek?");
                                prizeModule.AddOption("I seek adventure.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendMessage("Aurora smiles knowingly, as if she has heard this answer many times before.");
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                prizeModule.AddOption("I seek wealth.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule wealthModule = new DialogueModule("Wealth is a noble pursuit, and one I can certainly respect. It can buy comfort, safety, and even loyalty. But be careful—sometimes, the price of wealth is more than just gold.");
                                        wealthModule.AddOption("I'll take my chances.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        wealthModule.AddOption("Perhaps you're right. I'll think on it.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, wealthModule));
                                    });
                                prizeModule.AddOption("I seek freedom.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule freedomModule = new DialogueModule("Freedom... now that is a worthy goal. To live unchained by the expectations of others, to go wherever the wind takes you. But freedom often comes at the cost of severing ties, of leaving people behind.");
                                        freedomModule.AddOption("That's a price I'm willing to pay.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        freedomModule.AddOption("Perhaps I value my ties too much.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, freedomModule));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, prizeModule));
                            });
                        pl.SendGump(new DialogueGump(pl, opportunityModule));
                    });
                cunningDialogue.AddOption("I think I'll keep my distance.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, cunningDialogue));
            });

        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Aurora smiles warmly and nods.");
            });

        return greeting;
    }

    private bool HasWeddingDayCake(PlayerMobile player)
    {
        // Check the player's inventory for WeddingDayCake
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(WeddingDayCake)) != null;
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
        // Remove the WeddingDayCake and give the TailoringTalisman, always give MaxxiaScroll
        Item weddingDayCake = player.Backpack.FindItemByType(typeof(WeddingDayCake));
        if (weddingDayCake != null)
        {
            weddingDayCake.Delete();
            player.AddToBackpack(new TailoringTalisman());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the WeddingDayCake and receive a TailoringTalisman and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a WeddingDayCake.");
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