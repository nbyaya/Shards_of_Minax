using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BrawnyBart : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BrawnyBart() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Brawny Bart";
        Body = 0x190; // Human male body

        // Stats
        SetStr(140);
        SetDex(40);
        SetInt(20);
        SetHits(90);

        // Appearance
        AddItem(new ShortPants(2126));
        AddItem(new Doublet(2126));
        AddItem(new Sandals(2126));
        AddItem(new Bandana(2126));
        AddItem(new Cutlass() { Name = "Bart's Cutlass" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        FacialHairItemID = Race.RandomFacialHair(this);

        SpeechHue = 0;
        lastRewardTime = DateTime.MinValue;
    }

    public BrawnyBart(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Ahoy there, matey! I be Brawny Bart, the fiercest pirate on these waters! What brings ye to my presence?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("Arr, I be Brawny Bart, feared by many and bested by none! I sail these seas in search of hidden treasures and the thrill of adventure.");
                identityModule.AddOption("Tell me about these treasures.",
                    p => true,
                    p =>
                    {
                        DialogueModule treasureModule = new DialogueModule("Ah, many have asked, but few have lived to find 'em. But if ye prove yerself to me, maybe I'll share a clue or two.");
                        treasureModule.AddOption("How can I prove myself?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule proveModule = new DialogueModule("Fetch me the golden compass from the isle of shadows. It's been a dream of mine, but I've never dared to venture there.");
                                proveModule.AddOption("Where is the isle of shadows?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule shadowModule = new DialogueModule("To the east, where the sun meets the sea. But be warned, it's cursed! Only the bravest souls dare to tread there. If ye bring back the compass, I'll give ye a reward fit for a king!");
                                        shadowModule.AddOption("I'll bring you the compass.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendMessage("You set sail for the mysterious isle of shadows.");
                                            });
                                        shadowModule.AddOption("That sounds too dangerous.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, shadowModule));
                                    });
                                proveModule.AddOption("I have other matters to attend to.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, proveModule));
                            });
                        treasureModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, treasureModule));
                    });
                identityModule.AddOption("Tell me about your true love.",
                    p => true,
                    p =>
                    {
                        DialogueModule loveModule = new DialogueModule("Arr, you've caught me off guard, matey. I do have a lass... Her name be Eliza, a weaver girl from Vesper. She doesn't know I be a pirate, and I'd do anything to keep it that way.");
                        loveModule.AddOption("Why hide that you are a pirate?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule hideModule = new DialogueModule("Eliza be pure, like the morning sun. She thinks I be a simple sailor. If she knew the truth, she'd want nothing to do with me. I need to earn enough gold to settle down, so she never has to find out.");
                                hideModule.AddOption("How do you plan to earn the gold?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule earnModule = new DialogueModule("Plunderin' treasures, takin' risks—aye, it's dangerous, but it's the only way I know. I want to give her a good life, one without the fear of the gallows.");
                                        earnModule.AddOption("That sounds risky. What if you fail?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule failModule = new DialogueModule("Aye, I've thought about that many a night. If I fail, at least I'll know I tried. But the thought of her marryin' some landlubber who could give her the life she deserves—I'd rather face a kraken than see that.");
                                                failModule.AddOption("Why not tell her the truth?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule truthModule = new DialogueModule("The truth? The truth would break her heart, and mine along with it. She deserves a man she can be proud of, not a scoundrel of the seas. I'd rather lie and see her happy than tell the truth and see her in tears.");
                                                        truthModule.AddOption("Perhaps she'd love you regardless.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                DialogueModule regardlessModule = new DialogueModule("Ye may be right... but it be a gamble I can't take. If I lose her, there'd be no treasure in the world worth the price. I have to do this my way, risky as it may be.");
                                                                regardlessModule.AddOption("I understand. Love makes us do foolish things.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule()));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, regardlessModule));
                                                            });
                                                        truthModule.AddOption("Perhaps it's better to let her go.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                DialogueModule letGoModule = new DialogueModule("Let her go? Aye, I've thought about that too. But every time I try, my heart won't let me. She be the only light in me dark world. I can't give her up, not without tryin' to make it work.");
                                                                letGoModule.AddOption("Follow your heart, Bart.",
                                                                    plaaaaa => true,
                                                                    plaaaaa =>
                                                                    {
                                                                        plaaaaa.SendGump(new DialogueGump(plaaaaa, CreateGreetingModule()));
                                                                    });
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, letGoModule));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, truthModule));
                                                    });
                                                failModule.AddOption("I admire your determination.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, failModule));
                                            });
                                        earnModule.AddOption("That sounds noble, in a pirate's way.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, earnModule));
                                    });
                                hideModule.AddOption("Maybe one day you won't have to hide.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, hideModule));
                            });
                        loveModule.AddOption("What is she like?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule describeModule = new DialogueModule("Eliza? She's got hair like spun gold and eyes as blue as the clearest sky. She works at her loom all day, weaving tapestries more beautiful than any treasure I've ever seen. She's kind, gentle... and everything I ain't.");
                                describeModule.AddOption("She sounds wonderful.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, describeModule));
                            });
                        loveModule.AddOption("I wish you luck, Bart.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, loveModule));
                    });
                identityModule.AddOption("What is your job?",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("A pirate's life be me only job, plunderin' and seekin' hidden treasures!");
                        jobModule.AddOption("Sounds like an exciting life.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("Farewell.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Brawny Bart nods as you leave.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("What do you know about the kraken?",
            player => true,
            player =>
            {
                DialogueModule krakenModule = new DialogueModule("Aye, once or twice, them beasts be no match for Brawny Bart! Though they left scars that'll never heal, both on me ship and in me heart.");
                krakenModule.AddOption("Tell me more.",
                    p => true,
                    p =>
                    {
                        DialogueModule moreModule = new DialogueModule("Them krakens be monstrous and ruthless. Ye need more than just strength; ye need wits and luck to best 'em.");
                        moreModule.AddOption("I see. Thank you.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, moreModule));
                    });
                krakenModule.AddOption("I'll ask you about something else.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, krakenModule));
            });

        greeting.AddOption("Do you have a reward for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    cooldownModule.AddOption("Alright, I'll wait.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, now that's a surprise! Here, take this as a token of my gratitude.");
                    rewardModule.AddOption("Thank you!",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Brawny Bart nods as you walk away.");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}