using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ChartingCharlene : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ChartingCharlene() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Charting Charlene";
        Body = 0x191; // Human female body

        // Stats
        SetStr(105);
        SetDex(52);
        SetInt(118);
        SetHits(72);

        // Appearance
        AddItem(new FancyDress() { Hue = 1124 }); // Clothing item with hue 1124
        AddItem(new Sandals() { Hue = 1165 }); // Sandals with hue 1165

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ChartingCharlene(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Charting Charlene, the cartographer. How can I assist you today?");

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job is to map the lands and chart the unexplored territories. Cartography is a meticulous task that requires patience and precision. I also have an unnatural love for charts and geometry, which drives my passion.");
                jobModule.AddOption("Tell me more about your love for charts and geometry.",
                    p => true,
                    p =>
                    {
                        DialogueModule chartLoveModule = new DialogueModule("Ah, charts! The beauty of a well-drawn chart is beyond words. The intricate lines, the precision of each point, the way every curve fits into a perfect whole. Geometry is the language of the universe, you know! I could spend hours staring at a beautiful map, admiring the perfect alignment of latitudes and longitudes.");
                        chartLoveModule.AddOption("That sounds fascinating. Why do you think geometry is so important?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule geometryImportanceModule = new DialogueModule("Geometry is everywhere! From the shape of the stars to the flow of rivers, everything follows geometric principles. The harmony and balance found in geometric shapes make it possible to navigate the world, to understand distances, directions, and even the secrets hidden in the cosmos. Geometry is the key to unlocking the mysteries of the natural world.");
                                geometryImportanceModule.AddOption("Can you give me an example of geometry in nature?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule natureGeometryModule = new DialogueModule("Absolutely! Take the spirals of a seashell or the hexagonal patterns of a honeycomb. Both are perfect examples of nature using geometry to create beauty and efficiency. The seashell grows in a logarithmic spiral, allowing it to expand without changing its shape, and the honeycomb is nature's way of maximizing storage space using minimal material. Isn't that just amazing?");
                                        natureGeometryModule.AddOption("It's incredible! Do you use such principles in your maps?",
                                            plla => true,
                                            plla =>
                                            {
                                                DialogueModule mapPrinciplesModule = new DialogueModule("Indeed, I do! When I create maps, I always consider the natural geometry of the land. The curves of rivers, the ridges of mountains, even the paths that animals take—all of these follow geometric principles. Understanding these patterns helps me create more accurate and aesthetically pleasing maps. It's almost like I'm revealing the hidden order of the world.");
                                                mapPrinciplesModule.AddOption("You must be very dedicated to your work.",
                                                    pllab => true,
                                                    pllab =>
                                                    {
                                                        DialogueModule dedicationModule = new DialogueModule("Dedicated is an understatement! I am obsessed. There's something almost magical about the process of charting unknown lands. It's like I'm in a dance with the world, uncovering its secrets one step at a time. Every chart I draw feels like a tribute to the beauty and complexity of the world.");
                                                        dedicationModule.AddOption("I admire your passion.",
                                                            pllabc => true,
                                                            pllabc =>
                                                            {
                                                                pllabc.SendMessage("Charlene smiles brightly, clearly pleased by your words.");
                                                                pllabc.SendGump(new DialogueGump(pllabc, CreateGreetingModule()));
                                                            });
                                                        dedicationModule.AddOption("I have more questions.",
                                                            pllabc => true,
                                                            pllabc =>
                                                            {
                                                                pllabc.SendGump(new DialogueGump(pllabc, CreateGreetingModule()));
                                                            });
                                                        pllab.SendGump(new DialogueGump(pllab, dedicationModule));
                                                    });
                                                mapPrinciplesModule.AddOption("I have more questions.",
                                                    pllaq => true,
                                                    pllaq =>
                                                    {
                                                        plla.SendGump(new DialogueGump(plla, CreateGreetingModule()));
                                                    });
                                                pll.SendGump(new DialogueGump(pll, mapPrinciplesModule));
                                            });
                                        natureGeometryModule.AddOption("I have more questions.",
                                            pllw => true,
                                            pllw =>
                                            {
                                                pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, natureGeometryModule));
                                    });
                                geometryImportanceModule.AddOption("I have other questions.",
                                    ple => true,
                                    ple =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, geometryImportanceModule));
                            });
                        chartLoveModule.AddOption("I have other questions.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, chartLoveModule));
                    });
                jobModule.AddOption("I have other questions.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you seek adventure?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("True valor is not just about courage in battle, but also about the courage to explore the unknown. Adventures await those who dare to step into the unknown. May your journeys be filled with discovery and valor!");
                valorModule.AddOption("I am ready for adventure.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Charlene nods approvingly, wishing you luck on your travels.");
                    });
                valorModule.AddOption("Perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });

        greeting.AddOption("Do you have any maps or artifacts?",
            player => true,
            player =>
            {
                DialogueModule artifactsModule = new DialogueModule("The unexplored territories are vast and full of mysteries. Over the years, I've collected various artifacts and maps that I might be willing to part with for the right price or proof of valor.");
                artifactsModule.AddOption("How can I prove my valor?",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            DialogueModule rewardModule = new DialogueModule("Ah, you're interested in a reward! Very well, for someone as adventurous as you, I have just the thing. Here, take this. It might come in handy on your travels.");
                            rewardModule.AddOption("Thank you!",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                    pl.SendMessage("Charlene hands you an ancient scroll.");
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, rewardModule));
                        }
                    });
                artifactsModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, artifactsModule));
            });

        greeting.AddOption("Tell me about exploring.",
            player => true,
            player =>
            {
                DialogueModule exploringModule = new DialogueModule("Exploring is not just a profession for me, it's a passion. The thrill of setting foot on untouched lands, the mysteries waiting to be uncovered, it's an experience like no other. Have you encountered any mysteries on your journeys?");
                exploringModule.AddOption("I have, and it's been thrilling!",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Charlene smiles, pleased to hear about your adventures.");
                    });
                exploringModule.AddOption("Not yet, but I hope to.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, exploringModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Charlene waves you off with a smile.");
            });

        return greeting;
    }

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