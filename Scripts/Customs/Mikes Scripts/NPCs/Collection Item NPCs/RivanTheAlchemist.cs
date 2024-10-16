using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items; // Important for equipment
using System.Collections.Generic;

public class RivanTheAlchemist : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public RivanTheAlchemist() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rivan the Alchemist";
        Body = 0x190; // Human male body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new Robe(1153)); // Robe with a dark blue hue
        AddItem(new Sandals(1175)); // Light green sandals
        AddItem(new WizardsHat(2550)); // Wizard's hat with a purplish hue
        AddItem(new GoldNecklace()); // Golden necklace for a touch of eccentricity

        VirtualArmor = 15;
    }

    public RivanTheAlchemist(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! You seem like someone who appreciates the mysteries of alchemy. I'm Rivan, an alchemist in search of rare and mystical ingredients. Perhaps you can help me?");

        // Start with dialogue about his work
        greeting.AddOption("What kind of research do you conduct?",
            p => true,
            p =>
            {
                DialogueModule researchModule = new DialogueModule("I dabble in the refinement of magical artifacts and the transformation of ordinary objects into something extraordinary. My latest obsession involves a curious creature known as BabyLavos.");
                researchModule.AddOption("Tell me more about BabyLavos.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule babylavosModule = new DialogueModule("BabyLavos is an elusive being, said to contain untapped alchemical potential. Rumor has it that those who possess its essence can unlock profound secrets of nature. Would you happen to have one?");
                        babylavosModule.AddOption("I have a BabyLavos. What can you offer in exchange?",
                            pla => HasBabyLavos(pla) && CanTradeWithPlayer(pla),
                            pla =>
                            {
                                CompleteTrade(pla);
                            });
                        babylavosModule.AddOption("I don't have one right now.",
                            pla => !HasBabyLavos(pla),
                            pla =>
                            {
                                pla.SendMessage("Return when you have obtained a BabyLavos. I shall be waiting.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        babylavosModule.AddOption("I traded recently; I'll come back later.",
                            pla => !CanTradeWithPlayer(pla),
                            pla =>
                            {
                                pla.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                            });
                        
                        babylavosModule.AddOption("You seem a bit... different. Is something wrong?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule personalModule = new DialogueModule("Different, you say? Ha! Maybe you're right, traveler. You see, I've always been a man who thrives on risks. I once bet my entire workshop in a high-stakes gamble, and came out with something far more... interesting.");
                                personalModule.AddOption("What did you win?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule artifactModule = new DialogueModule("Ah, what a story! I won a strange artifact, carved in black stone and adorned with mysterious runes. They said it was cursed, but I took it anyway. After all, fortune favors the bold, does it not?");
                                        artifactModule.AddOption("That sounds dangerous. Did it affect you?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule curseModule = new DialogueModule("Dangerous? Hah! I thought it was nothing but superstitious drivel. But, strange things began to happen. Shadows grew longer in my laboratory, whispers followed me at night, and my luck, oh my luck... it grew both wonderful and terrible.");
                                                curseModule.AddOption("Wonderful and terrible? How so?",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        DialogueModule luckModule = new DialogueModule("Some days, I'd find gold coins in the most unexpected places. Other days, my potions would explode without warning. I once won a fortune in a dice game, only to lose it all to a sudden fire. It's as if the artifact takes pleasure in my uncertainty, in keeping me on the edge.");
                                                        luckModule.AddOption("Why keep it, then?",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                DialogueModule keepArtifactModule = new DialogueModule("Ah, that's the question, isn't it? Why keep something that brings ruin as often as it brings riches? The truth is, I can't let it go. I'm desperate, yes, desperate to unlock its secrets. I've become addicted to the thrill, to the chaos, to the risk. The artifact is a part of me now.");
                                                                keepArtifactModule.AddOption("That sounds like a curse indeed.",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        plaaaaaa.SendMessage("Rivan nods, a flicker of something—fear, perhaps—passing over his face.");
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, CreateGreetingModule(plaaaaaa)));
                                                                    });
                                                                keepArtifactModule.AddOption("What do you think will happen in the end?",
                                                                    plaaaaaa => true,
                                                                    plaaaaaa =>
                                                                    {
                                                                        DialogueModule endModule = new DialogueModule("The end? Ah, who can say? Maybe I'll unlock the greatest alchemical secret the world has ever known. Or maybe, one day, the shadows will swallow me whole. Either way, I plan to see it through. The thrill of not knowing... that's what keeps me alive.");
                                                                        endModule.AddOption("You are a true gambler, Rivan.",
                                                                            plaaaaaaa => true,
                                                                            plaaaaaaa =>
                                                                            {
                                                                                plaaaaaaa.SendMessage("Rivan gives you a wry smile, his eyes glinting with both charm and desperation.");
                                                                                plaaaaaaa.SendGump(new DialogueGump(plaaaaaaa, CreateGreetingModule(plaaaaaaa)));
                                                                            });
                                                                        plaaaaaa.SendGump(new DialogueGump(plaaaaaa, endModule));
                                                                    });
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, keepArtifactModule));
                                                            });
                                                        luckModule.AddOption("You should be careful, Rivan.",
                                                            plaaaaa => true,
                                                            plaaaaa =>
                                                            {
                                                                plaaaaa.SendMessage("Rivan laughs, though there's a hint of unease behind it.");
                                                                plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule(plaaaaa)));
                                                            });
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, luckModule));
                                                    });
                                                curseModule.AddOption("That sounds terrifying. Maybe you should get rid of it.",
                                                    plaaaq => true,
                                                    plaaaq =>
                                                    {
                                                        plaaa.SendMessage("Rivan shakes his head, his expression unreadable.");
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, curseModule));
                                            });
                                        artifactModule.AddOption("Sounds like a risky prize.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                plaaa.SendMessage("Rivan smirks, a mix of pride and something darker in his eyes.");
                                                plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule(plaaa)));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, artifactModule));
                                    });
                                personalModule.AddOption("That sounds like a lot to handle.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Rivan sighs, the bravado momentarily fading.");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, personalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, babylavosModule));
                    });
                researchModule.AddOption("That sounds interesting, but maybe later.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, researchModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Rivan nods thoughtfully, lost in his alchemical musings.");
            });

        return greeting;
    }

    private bool HasBabyLavos(PlayerMobile player)
    {
        // Check the player's inventory for BabyLavos
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(BabyLavos)) != null;
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
        // Remove the BabyLavos and give the OpalBranch, then set the cooldown timer
        Item babyLavos = player.Backpack.FindItemByType(typeof(BabyLavos));
        if (babyLavos != null)
        {
            babyLavos.Delete();
            player.AddToBackpack(new OpalBranch());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the BabyLavos and receive an OpalBranch and a MaxxiaScroll in return. Use them wisely.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a BabyLavos.");
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