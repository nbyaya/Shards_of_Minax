using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class MaelrikTheMysticTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public MaelrikTheMysticTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Maelrik the Mystic Trader";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(75);
        SetDex(70);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(80);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Robe with a deep blue hue
        AddItem(new Boots(1109)); // Dark brown boots
        AddItem(new WizardsHat(1150)); // A mystical hat
        AddItem(new QuarterStaff()); // Staff to complete the mystic look

        VirtualArmor = 15;
    }

    public MaelrikTheMysticTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. You have the aura of one who delves into strange mysteries. I am Maelrik, trader of rare curiosities. Tell me, do you seek something beyond the ordinary?");

        greeting.AddOption("What kind of curiosities do you have?",
            p => true,
            p =>
            {
                DialogueModule curiositiesModule = new DialogueModule("I deal in items not often seen in the hands of the common folk. Mystic herbs, rare tomes, enchanted relics... but I could also use some help acquiring a certain object. Do you have a FleshLight item with you?");

                curiositiesModule.AddOption("Yes, I have a FleshLight.",
                    pl => HasFleshLight(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                
                curiositiesModule.AddOption("No, I don't have one right now.",
                    pl => !HasFleshLight(pl),
                    pl =>
                    {
                        pl.SendMessage("Ah, a pity. Should you come across a FleshLight, bring it to me. I have something special to offer in exchange.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                curiositiesModule.AddOption("I have traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("Patience is a virtue, traveler. You may only trade once every ten minutes. Return to me later.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                
                curiositiesModule.AddOption("Why are you interested in the FleshLight?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reasonModule = new DialogueModule("You ask too many questions, stranger. Let's just say it has... certain properties. Or maybe I'm just testing you. Either way, it doesn't concern you.");
                        reasonModule.AddOption("I see. Fair enough.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        pl.SendGump(new DialogueGump(pl, reasonModule));
                    });

                curiositiesModule.AddOption("Tell me about yourself, Maelrik.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("Ah, curious, are you? Well, I suppose there’s no harm in sharing a bit. I wasn’t always a mystic trader. No, once I was what you’d call a 'prepper'. Yes, a man ready for the end of days. I had supplies, plans, fortifications. But life has a funny way of turning your plans into ash.");

                        backstoryModule.AddOption("What happened to your plans?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule lossModule = new DialogueModule("Everything was lost. Betrayal, disaster... it doesn't matter now. I trusted no one, and yet, somehow, I was still blindsided. I live off the land now, relying on no one but myself. The world is a cruel place, traveler, and only fools believe otherwise.");

                                lossModule.AddOption("That sounds difficult. How do you manage?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule manageModule = new DialogueModule("Difficult? Hah! Difficult is trusting others and watching them take everything from you. I manage because I have to. I forage, I trade, I fight when needed. I’m always prepared, always watching. You should do the same, if you want to survive.");
                                        manageModule.AddOption("I see your point. Trust is dangerous.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Maelrik nods approvingly.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        manageModule.AddOption("That sounds lonely.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule lonelyModule = new DialogueModule("Lonely? Hah, I prefer it this way. People are a liability. They betray, they disappoint. Out here, it’s just me, the wild, and my own wits. That’s all I need.");
                                                lonelyModule.AddOption("Fair enough. I respect your independence.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, lonelyModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, manageModule));
                                    });

                                lossModule.AddOption("Sounds like you’ve been through a lot. What keeps you going?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule goingModule = new DialogueModule("What keeps me going? Hah, spite, perhaps. A stubborn refusal to let this world beat me. Or maybe I just want to see how much worse it can get. Every day I survive is another day I prove I’m stronger than whatever fate throws at me.");
                                        goingModule.AddOption("That’s a strong attitude.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        goingModule.AddOption("That sounds exhausting.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Maelrik chuckles darkly.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, goingModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, lossModule));
                            });

                        backstoryModule.AddOption("Why are you so suspicious of newcomers?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule suspicionModule = new DialogueModule("Suspicious? Hah! Stranger, I’ve learned the hard way that people are the greatest threat you’ll face. The desperate, the greedy, the jealous—they’ll smile at you, then stab you in the back the moment you let your guard down. Trust is a weakness, and I have no use for weakness.");
                                suspicionModule.AddOption("You must have met some good people, surely?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule goodPeopleModule = new DialogueModule("Good people? Perhaps, once or twice. But they never last, do they? Either they’re taken advantage of, or they become just like the rest. I prefer to keep my distance. Better to stay alive than to get attached and lose everything again.");
                                        goodPeopleModule.AddOption("I understand. Attachment can be dangerous.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        goodPeopleModule.AddOption("That’s a sad way to live.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Maelrik scoffs, clearly unimpressed.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, goodPeopleModule));
                                    });
                                suspicionModule.AddOption("I see. Caution has kept you alive.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Maelrik nods.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, suspicionModule));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                p.SendGump(new DialogueGump(p, curiositiesModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Maelrik nods knowingly and turns away, his robes swirling around him.");
            });

        return greeting;
    }

    private bool HasFleshLight(PlayerMobile player)
    {
        // Check the player's inventory for FleshLight item
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(FleshLight)) != null;
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
        // Remove the FleshLight item and give the LovelyLilies and MaxxiaScroll, then set the cooldown timer
        Item fleshLight = player.Backpack.FindItemByType(typeof(FleshLight));
        if (fleshLight != null)
        {
            fleshLight.Delete();
            player.AddToBackpack(new LovelyLilies());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the FleshLight and receive LovelyLilies and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a FleshLight.");
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