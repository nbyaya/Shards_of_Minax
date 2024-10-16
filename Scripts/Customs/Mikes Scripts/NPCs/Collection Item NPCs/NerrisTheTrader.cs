using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class NerrisTheTrader : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public NerrisTheTrader() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Nerris the Trader";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(70);
        SetInt(110);

        SetHits(100);
        SetMana(200);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyShirt(1157)); // Fancy Shirt with a dark blue hue
        AddItem(new LongPants(1175)); // Pants with a light grey hue
        AddItem(new Sandals(1109)); // Sandals with a brown hue
        AddItem(new Cloak(1153)); // A cloak with a maroon hue
        AddItem(new FeatheredHat(1150)); // A hat to make him stand out

        VirtualArmor = 15;
    }

    public NerrisTheTrader(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I'm Nerris, a collector of unusual and exotic items. Do you happen to have anything interesting for me?");

        // Dialogue about his collection and trade offer
        greeting.AddOption("What kind of items do you collect?",
            p => true,
            p =>
            {
                DialogueModule collectionModule = new DialogueModule("I have a particular interest in rare and quirky items. For example, I'm always on the lookout for... Hamburgers. Strange, yes? But they are a rare delicacy in these parts.");
                collectionModule.AddOption("Why hamburgers?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hamburgerModule = new DialogueModule("Ah, it's not just the taste, my friend! Hamburgers are said to bring good fortune to those who trade them. I can exchange a rare RootThrone for one, but only once every so often.");
                        hamburgerModule.AddOption("Why are you so interested in good fortune?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule fortuneModule = new DialogueModule("Ah, good fortune... It's something I need after all I've been through. You see, I was not always a simple trader. I was once a soldier in a warlord's army. My childhood was lost to battles, my youth spent on blood-soaked fields. I was trained to kill, to follow orders without question. Now, I seek to undo some of that damage, to protect those who cannot protect themselves. Trading is a way to connect with others, to help without drawing a blade.");
                                fortuneModule.AddOption("That sounds tough. How did you escape?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule escapeModule = new DialogueModule("Escape? It wasn't easy. Many of my comrades weren't as lucky. One night, during a raid, I saw an opportunity. The village we were supposed to sack was full of innocent people. They were terrified, hiding in their homes. I couldn't do it anymore. I turned on my own and helped the villagers escape. After that, I ran as far as I could. The warlord put a price on my head, but I was willing to take the risk. I had to atone for what I'd done.");
                                        escapeModule.AddOption("Did anyone help you after that?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("Yes, there were a few kind souls along the way. I owe my life to a healer named Elara. She found me half-dead in the woods, took me in, and nursed me back to health. She taught me the value of compassion. It was through her that I learned to use my skills for good. It's why I'm loyal to the innocent now—people like her deserve to live without fear.");
                                                helpModule.AddOption("It must be hard to leave the past behind.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule pastModule = new DialogueModule("The past is a heavy burden, one that never truly leaves. There are nights when I still hear the screams, see the faces of those I wronged. I know I can't change what I've done, but I can decide what I do now. That's why I travel, why I trade—every small act of kindness helps me heal, even if just a little.");
                                                        pastModule.AddOption("You're doing your best now. That's what matters.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Nerris smiles, a glimmer of hope in his eyes. 'Thank you, friend. It means more than you know.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        pastModule.AddOption("I hope you find peace someday.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Nerris nods solemnly. 'Perhaps. Until then, I will keep trying.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, pastModule));
                                                    });
                                                helpModule.AddOption("You're loyal to the innocent now?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule loyaltyModule = new DialogueModule("Yes. After everything I've seen, I understand how important it is to protect those who cannot protect themselves. I was once the instrument of fear; now, I strive to be a shield. It's not always easy, but it's the path I've chosen. Loyalty, to me, means standing by those who need help, even when it's dangerous or difficult.");
                                                        loyaltyModule.AddOption("That's admirable, Nerris.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Nerris gives you a grateful nod. 'Thank you. I just hope my actions can make a difference, even if it's a small one.'");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        loyaltyModule.AddOption("It's a dangerous life, isn't it?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule dangerModule = new DialogueModule("It is, but I've made my peace with that. Danger is something I've known my whole life. At least now, I face it for a worthy cause. I protect, I help, and if that means risking my life, so be it. It's better than living as I once did, causing pain without purpose.");
                                                                dangerModule.AddOption("Stay safe, Nerris.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Nerris smiles faintly. 'I'll do my best. And you, traveler, take care of yourself as well.'");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                dangerModule.AddOption("You're brave. I respect that.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Nerris bows his head. 'Thank you. Courage is something we all need, especially in times like these.'");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, dangerModule));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, loyaltyModule));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, helpModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, escapeModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, fortuneModule));
                            });
                        hamburgerModule.AddOption("Perhaps another time.",
                            plaa => true,
                            plaa =>
                            {
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });
                        pl.SendGump(new DialogueGump(pl, hamburgerModule));
                    });
                collectionModule.AddOption("Interesting. Maybe I'll find one.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });
                p.SendGump(new DialogueGump(p, collectionModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Nerris waves you off with a knowing smile.");
            });

        return greeting;
    }

    private bool HasHamburger(PlayerMobile player)
    {
        // Check the player's inventory for Hamburgers
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(Hamburgers)) != null;
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
        // Remove the Hamburger and give the RootThrone, then set the cooldown timer
        Item hamburger = player.Backpack.FindItemByType(typeof(Hamburgers));
        if (hamburger != null)
        {
            hamburger.Delete();
            player.AddToBackpack(new RootThrone());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the Hamburger and receive a RootThrone and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a Hamburger.");
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