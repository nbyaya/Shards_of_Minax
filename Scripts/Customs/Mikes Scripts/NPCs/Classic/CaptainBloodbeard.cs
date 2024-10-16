using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CaptainBloodbeard : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CaptainBloodbeard() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Captain Bloodbeard";
        Body = 0x190; // Human male body

        // Stats
        SetStr(140);
        SetDex(70);
        SetInt(30);

        SetHits(100);

        // Appearance
        AddItem(new ShortPants() { Hue = 38 });
        AddItem(new FancyShirt() { Hue = 2111 });
        AddItem(new Boots() { Hue = 1157 });
        AddItem(new ThinLongsword() { Name = "Bloodbeard's Saber" });
        AddItem(new TricorneHat() { Hue = 2112 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
        SpeechHue = 0;
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
        DialogueModule greeting = new DialogueModule("Ahoy, traveler! Ye be standin' before Captain Bloodbeard, the most feared pirate on these seas! What business have ye with me?");

        greeting.AddOption("What's your name?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("Ye be standin' before Captain Bloodbeard, the most feared pirate on these seas!");
                nameModule.AddOption("How did you get the name Bloodbeard?",
                    p => true,
                    p =>
                    {
                        DialogueModule storyModule = new DialogueModule("Ah, that be a tale of treachery, revenge, and the sea's unyielding fury! I wasn't always known as Bloodbeard. Once, I was just Captain Redrick, sailin' the seas with me loyal crew. Until one day, a mutineer decided he wanted me ship and my life.");
                        storyModule.AddOption("What happened with the mutineer?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule mutineerModule = new DialogueModule("The scallywag, a man I once called me first mate, tried to slit me throat in the dead of night. But he underestimated ol' Redrick. I fought back, and in the struggle, I ended up spillin' his blood all over me beard. The crimson never washed out, and the crew took to callin' me Bloodbeard from that day on.");
                                mutineerModule.AddOption("How did the crew react?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule crewModule = new DialogueModule("The crew, they were torn. Some were loyal to me, others feared what I had become. But when I stood on deck, covered in the mutineer's blood, and roared that no man would ever challenge me again, they fell in line. Fear can be a powerful ally, lad. From that day, I led with an iron fist, and none dared cross me.");
                                        crewModule.AddOption("Did you ever regret it?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule regretModule = new DialogueModule("Regret? Aye, perhaps a bit. The sea be a harsh mistress, and the choices we make often weigh heavy on our souls. But a captain must be strong, or he loses everything. I regret the need for bloodshed, but I do not regret survivin'. It be the way of the sea.");
                                                regretModule.AddOption("It must have been difficult.",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        DialogueModule difficultModule = new DialogueModule("Aye, lad, it were difficult. But the sea teaches ye that strength be the only way to survive. Ye can mourn, ye can cry, but in the end, ye must be the storm that others fear. And that be what I became: a storm with a beard stained red, a warning to all who would betray me.");
                                                        difficultModule.AddOption("What happened after that?",
                                                            plaac => true,
                                                            plaac =>
                                                            {
                                                                DialogueModule aftermathModule = new DialogueModule("After that night, me reputation grew. Tales spread across the seas of Captain Bloodbeard, the man who survived mutiny and painted his beard with the blood of his enemies. Some feared me, some admired me, and others wanted to challenge me. But none succeeded. The name became a banner, a warning, and a symbol of what happens to those who cross me.");
                                                                aftermathModule.AddOption("That's quite the story.",
                                                                    plaad => true,
                                                                    plaad =>
                                                                    {
                                                                        plaad.SendGump(new DialogueGump(plaad, CreateGreetingModule()));
                                                                    });
                                                                aftermathModule.AddOption("Back.",
                                                                    plaad => true,
                                                                    plaad =>
                                                                    {
                                                                        plaad.SendGump(new DialogueGump(plaad, CreateGreetingModule()));
                                                                    });
                                                                plaac.SendGump(new DialogueGump(plaac, aftermathModule));
                                                            });
                                                        plaab.SendGump(new DialogueGump(plaab, difficultModule));
                                                    });
                                                regretModule.AddOption("Back.",
                                                    plaab => true,
                                                    plaab =>
                                                    {
                                                        plaab.SendGump(new DialogueGump(plaab, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, regretModule));
                                            });
                                        crewModule.AddOption("Back.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, crewModule));
                                    });
                                mutineerModule.AddOption("Back.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, mutineerModule));
                            });
                        storyModule.AddOption("Back.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, storyModule));
                    });
                nameModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I be as healthy as a shark in a frenzy!");
                healthModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What's your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job be plunderin' and seekin' treasure, of course!");
                jobModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about your battles.",
            player => true,
            player =>
            {
                DialogueModule battleModule = new DialogueModule("Life be a series of battles, lad. Be ye ready to face the storms?");
                battleModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, battleModule));
            });

        greeting.AddOption("What do you think of scallywags who follow the law?",
            player => true,
            player =>
            {
                DialogueModule lawModule = new DialogueModule("Arr, ye have a spine, I see. Tell me, what be yer thoughts on the scallywags who follow the law blindly?");
                lawModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, lawModule));
            });

        greeting.AddOption("What do you think about sharks?",
            player => true,
            player =>
            {
                DialogueModule sharkModule = new DialogueModule("Ah, the shark, a creature I respect greatly. They be the true rulers of the deep, always on the hunt, never restin'. Just like meself!");
                sharkModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, sharkModule));
            });

        greeting.AddOption("Tell me about treasure.",
            player => true,
            player =>
            {
                DialogueModule treasureModule = new DialogueModule("Treasure be not just about gold and jewels, lad. The greatest treasures be the tales we collect and the mates we share 'em with.");
                treasureModule.AddOption("Back.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, treasureModule));
            });

        greeting.AddOption("What about the storms?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    DialogueModule stormModule = new DialogueModule("Storms be nature's way of testin' us. But after each storm, there be calm and a rainbow. If ye can withstand the storm, ye might find yer own treasure at the end of that rainbow. Here, for yer bravery, take this as a token.");
                    stormModule.AddOption("Back.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow;
                    player.SendGump(new DialogueGump(player, stormModule));
                }
            });

        greeting.AddOption("Farewell, Captain.",
            player => true,
            player =>
            {
                player.SendMessage("Captain Bloodbeard nods at ye.");
            });

        return greeting;
    }

    public CaptainBloodbeard(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        writer.Write(lastRewardTime);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        lastRewardTime = reader.ReadDateTime();
    }
}