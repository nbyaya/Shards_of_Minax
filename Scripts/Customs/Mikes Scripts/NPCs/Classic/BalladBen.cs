using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BalladBen : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BalladBen() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Ballad Ben";
        Body = 0x190; // Human male body

        // Stats
        SetStr(117);
        SetDex(69);
        SetInt(83);

        SetHits(88);

        // Appearance
        AddItem(new Doublet(45)); // Doublet with hue 45
        AddItem(new Kilt(88)); // Kilt with hue 88
        AddItem(new Sandals(1176)); // Sandals with hue 1176
        AddItem(new LeatherGloves() { Name = "Ben's Ballad Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;

        // Speech Hue
        SpeechHue = 0; // Default speech hue
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
        DialogueModule greeting = new DialogueModule("I am Ballad Ben, the bard of bitter tunes. What brings you to my realm of sorrow?");

        greeting.AddOption("Tell me about your ballads.",
            player => true,
            player =>
            {
                DialogueModule balladModule = new DialogueModule("Ah, my parents hoped I'd sing songs of joy, but fate had different plans. Now I sing ballads that echo the shadows of the heart. Each verse, each chord, is a lament for a life lost to fate's cruel hand.");
                balladModule.AddOption("Why such sorrowful songs?",
                    p => true,
                    p =>
                    {
                        DialogueModule sorrowModule = new DialogueModule("Sorrow is the ink with which I write my songs. Have you ever felt its cold embrace? It is as though every breath is heavy with the weight of a thousand regrets, and every sunrise only serves to remind you of all you have lost.");
                        sorrowModule.AddOption("Yes, I know sorrow well.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule embraceModule = new DialogueModule("Then share with me a bitter tale of woe, and we shall revel in our shared misery! Do you have a story of lost love, of a dream that slipped through your fingers, or perhaps a friendship that turned to ash?");
                                embraceModule.AddOption("I once loved someone who left me.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule lostLoveModule = new DialogueModule("Ah, love... the sweetest of all joys, yet the most painful when it departs. I too once loved, but she was taken by the cruel winds of fate. Every note I play is a tribute to her memory, a plea to the heavens that perhaps, somewhere, she still hears me.");
                                        lostLoveModule.AddOption("How do you bear it?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule bearItModule = new DialogueModule("You do not bear it, not truly. You simply learn to live with the pain, to let it become part of who you are. In time, the sorrow becomes your muse, and you find beauty in the melancholy. It is a heavy burden, but one that shapes you.");
                                                bearItModule.AddOption("That sounds unbearable.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                bearItModule.AddOption("Perhaps there is strength in that.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, bearItModule));
                                            });
                                        lostLoveModule.AddOption("I prefer not to dwell on it.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, lostLoveModule));
                                    });
                                embraceModule.AddOption("I lost a dear friend in battle.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule lostFriendModule = new DialogueModule("A friend lost to the chaos of battle... There is no greater agony than knowing you could do nothing to save them. I have seen comrades fall, their laughter silenced, their hopes extinguished. My songs are filled with their echoes, their stories preserved in melody.");
                                        lostFriendModule.AddOption("It still haunts me.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule hauntModule = new DialogueModule("As it should. The memories of those we have lost are the ghosts that walk beside us. They remind us of our duty, our failures, and the fleeting nature of life. Perhaps one day, these memories will find peace, but until then, we sing for them.");
                                                hauntModule.AddOption("Thank you for understanding.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                hauntModule.AddOption("I wish I could forget.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, hauntModule));
                                            });
                                        lostFriendModule.AddOption("They would want me to move on.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, lostFriendModule));
                                    });
                                embraceModule.AddOption("Perhaps another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, embraceModule));
                            });
                        sorrowModule.AddOption("No, I try to avoid it.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule avoidanceModule = new DialogueModule("Ah, to avoid sorrow is to avoid life itself. Pain and joy are two sides of the same coin. To feel one is to risk the other, but without that risk, we are nothing more than empty vessels.");
                                avoidanceModule.AddOption("I see your point.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                avoidanceModule.AddOption("I still choose to avoid it.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, avoidanceModule));
                            });
                        p.SendGump(new DialogueGump(p, sorrowModule));
                    });
                balladModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, balladModule));
            });

        greeting.AddOption("Do you believe in fate?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no reward right now. Please return later.");
                    cooldownModule.AddOption("Very well.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule fateModule = new DialogueModule("Ah, fortune. It can be as fleeting as a sunset or as enduring as the mountains. But its true value is in how we use it. Do you believe in fate's hand guiding your path?");
                    fateModule.AddOption("Yes, I do.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new CapacityIncreaseDeed()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Ballad Ben hands you a small token, a reminder of fate's fleeting touch.");
                        });
                    fateModule.AddOption("No, I make my own destiny.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, fateModule));
                }
            });

        greeting.AddOption("Why do you speak of darkness?",
            player => true,
            player =>
            {
                DialogueModule darknessModule = new DialogueModule("Darkness isn't just the absence of light. It's the weight one feels when hope seems lost. But sometimes, in the deepest darkness, a glimmer of hope can shine the brightest.");
                darknessModule.AddOption("That's a hopeful thought.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                darknessModule.AddOption("I prefer to avoid such thoughts.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, darknessModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Ballad Ben nods solemnly as you leave.");
            });

        return greeting;
    }

    public BalladBen(Serial serial) : base(serial) { }

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