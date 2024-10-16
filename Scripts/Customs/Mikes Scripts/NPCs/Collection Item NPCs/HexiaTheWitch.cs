using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class HexiaTheWitch : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public HexiaTheWitch() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Hexia the Enigmatic Witch";
        Body = 0x191; // Human female body
        Hue = 0; // Default hue

        SetStr(75);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 500;
        Karma = -500;

        // Outfit
        AddItem(new Robe(2357)); // Dark purple robe
        AddItem(new Boots(1109)); // Black boots
        AddItem(new WizardsHat(1153)); // Dark wizard hat
        AddItem(new GoldBracelet()); // A mystical bracelet
        AddItem(new BlackStaff()); // A staff with dark energy

        VirtualArmor = 15;
    }

    public HexiaTheWitch(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Well, well, what do we have here? A curious soul drawn to the mysteries of the arcane... I am Hexia, keeper of dark secrets and forgotten lore. What is it you seek, stranger?");

        // Start with some introductory dialogue options
        greeting.AddOption("Who are you, exactly?", 
            p => true, 
            p =>
            {
                DialogueModule whoModule = new DialogueModule("I am Hexia, a witch who delves into the arcane arts. I study the balance between light and dark, and the delicate threads that weave magic into our world. Are you interested in the deeper mysteries, traveler?");
                whoModule.AddOption("Tell me about these mysteries.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule mysteriesModule = new DialogueModule("There are relics and artifacts scattered across the lands, each with its own power. Some of these relics hold dark, unsettling powers that most dare not touch. But those brave enough might find them... useful.");
                        mysteriesModule.AddOption("Interesting...", 
                            plaa => true, 
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateTormentedSoulModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, mysteriesModule));
                    });
                whoModule.AddOption("Perhaps another time.", 
                    pl => true, 
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, whoModule));
            });

        // Introduce the trade option
        greeting.AddOption("Do you need any help with your studies?", 
            p => true, 
            p =>
            {
                DialogueModule tradeIntroductionModule = new DialogueModule("Ah, you might be of some assistance indeed. I am in need of an item called the 'SatanicTable' for my experiments. If you bring it to me, I shall reward you handsomely with a FunMushroom and always a MaxxiaScroll as a token of my gratitude.");
                tradeIntroductionModule.AddOption("I have the SatanicTable.", 
                    pla => HasSatanicTable(pla) && CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        CompleteTrade(pla);
                    });
                tradeIntroductionModule.AddOption("I don't have it right now.", 
                    pla => !HasSatanicTable(pla), 
                    pla =>
                    {
                        pla.SendMessage("Come back when you have the SatanicTable.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                tradeIntroductionModule.AddOption("I traded recently; I'll come back later.", 
                    pla => !CanTradeWithPlayer(pla), 
                    pla =>
                    {
                        pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, tradeIntroductionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.", 
            p => true, 
            p =>
            {
                p.SendMessage("Hexia nods, her eyes glinting with dark curiosity.");
            });

        return greeting;
    }

    private DialogueModule CreateTormentedSoulModule(PlayerMobile player)
    {
        DialogueModule tormentedSoul = new DialogueModule("Ah, you remind me of someone... perhaps, a tormented soul seeking something unattainable. My own past is riddled with darkness, the kind that makes even the bravest quiver.");
        tormentedSoul.AddOption("Why do you say I remind you of someone?",
            p => true,
            p =>
            {
                DialogueModule reminderModule = new DialogueModule("You have that haunted look in your eyes... as if you too have suffered and endured pain that no one else could fathom. I have seen such a look before, in the mirror.");
                reminderModule.AddOption("What happened to you?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule pastModule = new DialogueModule("I was once like you, seeking power to right a wrong. But the price of magic is never small. I lost someone... someone dear, and I used my powers in desperation to bring them back. The result... was not what I intended.");
                        pastModule.AddOption("Did you succeed?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule consequenceModule = new DialogueModule("In a way, yes. They returned, but not as they were. They were a shell, a twisted echo of their former self. And now, I bear the weight of that mistake, haunted by their cries. Redemption, they say... perhaps I will never find it.");
                                consequenceModule.AddOption("I understand your pain.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        DialogueModule understandingModule = new DialogueModule("Do you? Truly? It is a heavy burden, and one that consumes me even now. Every step I take in this world is filled with regret, yet I press on, hoping... hoping that maybe one day, I will be free.");
                                        understandingModule.AddOption("I hope you find peace.",
                                            plaaaa => true,
                                            plaaaa =>
                                            {
                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                            });
                                        plaaa.SendGump(new DialogueGump(plaaa, understandingModule));
                                    });
                                consequenceModule.AddOption("That's a terrible fate.",
                                    plaaa => true,
                                    plaaa =>
                                    {
                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                    });
                                plaa.SendGump(new DialogueGump(plaa, consequenceModule));
                            });
                        pastModule.AddOption("I don't wish to hear more.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, pastModule));
                    });
                reminderModule.AddOption("Perhaps I am just tired.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, reminderModule));
            });

        tormentedSoul.AddOption("I carry my own burdens, but they are mine alone.",
            p => true,
            p =>
            {
                DialogueModule burdenModule = new DialogueModule("Ah, a strong one, I see. But do not underestimate the weight of grief. It has a way of creeping into your soul when you least expect it. May you be careful on your journey, stranger.");
                burdenModule.AddOption("I will. Thank you.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, burdenModule));
            });

        return tormentedSoul;
    }

    private bool HasSatanicTable(PlayerMobile player)
    {
        // Check the player's inventory for SatanicTable
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(SatanicTable)) != null;
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
        // Remove the SatanicTable and give the FunMushroom and MaxxiaScroll, then set the cooldown timer
        Item satanicTable = player.Backpack.FindItemByType(typeof(SatanicTable));
        if (satanicTable != null)
        {
            satanicTable.Delete();
            player.AddToBackpack(new FunMushroom());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the SatanicTable and receive a FunMushroom and MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have the SatanicTable.");
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