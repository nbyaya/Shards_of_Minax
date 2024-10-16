using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class SwashbucklingSally : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public SwashbucklingSally() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Swashbuckling Sally";
        Body = 0x191; // Human female body

        // Stats
        Str = 130;
        Dex = 65;
        Int = 25;
        Hits = 90;

        // Appearance
        AddItem(new TricorneHat() { Hue = 1175 });
        AddItem(new FancyShirt() { Hue = 1910 });
        AddItem(new ShortPants() { Hue = 2120 });
        AddItem(new Boots() { Hue = 1172 });
        AddItem(new Longsword() { Name = "Sally's Rapier" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("Ahoy, matey! I be Swashbuckling Sally, the fiercest pirate on these seas! What brings ye to my port?");
        
        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutMeModule = new DialogueModule("Arr matey! I be plundering the high seas in search of buried treasures and adventure!");
                aboutMeModule.AddOption("What kind of treasures?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateTreasureModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutMeModule));
            });

        greeting.AddOption("What's your job?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My job is to seek adventure and riches on the high seas! Arr!")));
            });

        greeting.AddOption("Tell me about your greatest heist.",
            player => true,
            player =>
            {
                DialogueModule heistModule = new DialogueModule("Ah, ye want to hear about the time I stole Lord British's personal crown? It be a tale of cunning and daring!");
                heistModule.AddOption("Yes, tell me the story!",
                    p => true,
                    p =>
                    {
                        DialogueModule storyModule = new DialogueModule("It all began on a stormy night... I had heard rumors of a grand ball held by Lord British. The crown would be on display, shimmering like the stars!");
                        storyModule.AddOption("What happened next?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule planModule = new DialogueModule("I devised a plan. Disguised as a noble, I snuck into the ball. The guards were none the wiser, distracted by the festivities!");
                                planModule.AddOption("Did you steal it then?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule theftModule = new DialogueModule("Aye! As the music played and the guests danced, I slipped into the vault where the crown was kept. It sparkled like no treasure I'd ever seen!");
                                        theftModule.AddOption("And then what?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule escapeModule = new DialogueModule("Just as I grabbed it, alarms blared! I had to think fast. I dashed through the crowd, using the chaos to my advantage!");
                                                escapeModule.AddOption("Did they catch you?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule closeCallModule = new DialogueModule("Not this time! I leapt into the night, but not before leaving a few guards scratching their heads. With the crown in hand, I made my escape!");
                                                        closeCallModule.AddOption("What did you do with it?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule burialModule = new DialogueModule("I couldn’t keep it, of course. So I buried it on a deserted island, far from prying eyes! A true pirate’s treasure, waiting for the right adventurer!");
                                                                burialModule.AddOption("Where is this island?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule islandModule = new DialogueModule("Ah, I keep that secret close to me heart! But I might give ye a hint if ye prove yourself worthy!");
                                                                        islandModule.AddOption("How can I prove myself?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule challengeModule = new DialogueModule("Bring me a legendary item, perhaps a piece of treasure from the depths of the sea, and I’ll reveal the location!");
                                                                                challengeModule.AddOption("I accept your challenge!",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendMessage("You accept Sally's challenge to bring her a legendary item!");
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                islandModule.AddOption("I need time to think.",
                                                                                    plh => true,
                                                                                    plh =>
                                                                                    {
                                                                                        plh.SendGump(new DialogueGump(plh, CreateGreetingModule()));
                                                                                    });
                                                                                ple.SendGump(new DialogueGump(ple, challengeModule));
                                                                            });
                                                                        pld.SendGump(new DialogueGump(pld, islandModule));
                                                                    });
                                                                plc.SendGump(new DialogueGump(plc, burialModule));
                                                            });
                                                        plb.SendGump(new DialogueGump(plb, closeCallModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, escapeModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, theftModule));
                                    });
                                p.SendGump(new DialogueGump(p, planModule));
                            });
                        player.SendGump(new DialogueGump(player, storyModule));
                    });
                player.SendGump(new DialogueGump(player, heistModule));
            });

        greeting.AddOption("Can I have a reward?",
            player => CanReceiveReward(player),
            player =>
            {
                if (GiveReward(player))
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("For proving your mettle, here's something special for ye!")));
                }
                else
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
            });

        return greeting;
    }

    private DialogueModule CreateTreasureModule()
    {
        DialogueModule treasureModule = new DialogueModule("I've heard tales of treasures lost to the seas. Do ye believe in the Lost Sapphire of the Seas?");
        treasureModule.AddOption("Aye, I do!",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, new DialogueModule("Aye, it's said to be hidden deep in the ocean, guarded by the kraken!")));
            });
        treasureModule.AddOption("Nay, just a myth!",
            p => true,
            p =>
            {
                p.SendGump(new DialogueGump(p, new DialogueModule("Maybe so, but every pirate needs a dream, don't ye think?")));
            });
        return treasureModule;
    }

    private bool CanReceiveReward(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private bool GiveReward(PlayerMobile player)
    {
        if (CanReceiveReward(player))
        {
            player.AddToBackpack(new MaxxiaScroll()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
            return true;
        }
        return false;
    }

    public SwashbucklingSally(Serial serial) : base(serial) { }

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
