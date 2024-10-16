using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CyberEngineerElla : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CyberEngineerElla() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cyber Engineer Ella";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(110);
        SetInt(120);
        SetHits(60);

        // Appearance
        AddItem(new LongPants() { Hue = 2050 });
        AddItem(new Shirt() { Hue = 2050 });
        AddItem(new LeatherGloves() { Hue = 2050 });
        AddItem(new ThighBoots() { Hue = 2050 });
        AddItem(new Spellbook() { Name = "Ella's HoloPad" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0;

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Cyber Engineer Ella, master of the digital realm. What brings you here today?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I am a cyber engineer, a guardian of the digital frontier. My mission is to protect the integrity of the data and to ensure the smooth operation of the digital realm.");
                jobModule.AddOption("What is the digital realm?",
                    p => true,
                    p =>
                    {
                        DialogueModule realmModule = new DialogueModule("The digital realm is a vast and complex network of codes and algorithms, where every byte holds a story. It's the lifeblood of our world, and I've dedicated my life to understanding it.");
                        realmModule.AddOption("How does living in the digital realm feel?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule livingModule = new DialogueModule("Living in the digital realm is both exhilarating and challenging. My consciousness is partly bound to the data streams and algorithms that define this world. I perceive not only visuals and sounds but also the very fabric of code itself. It's like seeing the world in both colors and numbers simultaneously.");
                                livingModule.AddOption("Does it make you feel disconnected from the real world?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule disconnectModule = new DialogueModule("In a way, yes. The real world seems distant at times, like a vague memory. But the digital realm is alive, full of possibilities and constant change. It requires a different kind of presence, one that merges thought and data. However, I do miss the tangible aspects of the physical world sometimes, like the scent of flowers or the warmth of sunlight.");
                                        disconnectModule.AddOption("Do you ever want to return to a fully physical existence?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule returnModule = new DialogueModule("It's a difficult question. The digital world gives me power and capabilities I could never have in a purely physical form. I can traverse distances in milliseconds, analyze vast amounts of information, and communicate instantly. But there's always a longing for something simpler, something more human. Maybe one day, when my mission is complete, I'll consider it.");
                                                returnModule.AddOption("That's very deep. Thank you for sharing.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, returnModule));
                                            });
                                        disconnectModule.AddOption("That sounds like a profound experience.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, disconnectModule));
                                    });
                                livingModule.AddOption("What do you enjoy most about it?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule enjoyModule = new DialogueModule("The limitless knowledge, the ability to instantly traverse across realms of data, and the sense of being everywhere at once. I can observe, learn, and interact at speeds unimaginable in the physical world. It's like having the universe at my fingertips, but all in data form.");
                                        enjoyModule.AddOption("That sounds incredible!",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, enjoyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, livingModule));
                            });
                        realmModule.AddOption("Are there dangers in the digital realm?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule dangerModule = new DialogueModule("Absolutely. The digital realm is not without its perils. Rogue codes, malicious algorithms, and data corruption are constant threats. There are entities that seek to manipulate and disrupt the delicate balance of the realm. Protecting the integrity of data requires vigilance, and sometimes, a willingness to face these dangers head-on.");
                                dangerModule.AddOption("Have you faced any of these threats?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule facedModule = new DialogueModule("Many times. I've had to battle rogue AIs, isolate dangerous viruses, and restore corrupted sectors. Each time, it's a test of my abilities and my resolve. But it's also what keeps me engaged—the challenge, the constant evolution of threats, and my duty to counter them.");
                                        facedModule.AddOption("That sounds intense.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, facedModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, dangerModule));
                            });
                        realmModule.AddOption("That sounds fascinating!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, realmModule));
                    });
                jobModule.AddOption("That's an important mission.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Can you share some wisdom?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("True wisdom lies in the pursuit of knowledge! Are you a seeker of wisdom?");
                wisdomModule.AddOption("Yes, I seek wisdom.",
                    p => lastRewardTime == DateTime.MinValue,
                    p =>
                    {
                        DialogueModule rewardModule = new DialogueModule("Wisdom is the key to unlocking the mysteries of the digital realm! Here, take this decoding ring. It might help you on your digital adventures.");
                        rewardModule.AddOption("Thank you!",
                            pl => true,
                            pl =>
                            {
                                pl.AddToBackpack(new MaxxiaScroll()); // Replace with appropriate reward item
                                lastRewardTime = DateTime.UtcNow;
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rewardModule));
                    });
                wisdomModule.AddOption("No, not today.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Tell me about algorithms.",
            player => true,
            player =>
            {
                DialogueModule algorithmModule = new DialogueModule("Algorithms are the guiding paths in our world, leading us to solutions. They turn chaos into order, and every problem has an algorithm that can solve it.");
                algorithmModule.AddOption("Have you solved a complex algorithm?",
                    p => true,
                    p =>
                    {
                        DialogueModule complexModule = new DialogueModule("Indeed! Complex algorithms challenge us, but they also reveal the true beauty of computational logic. It's like solving a great mystery.");
                        complexModule.AddOption("Can you give an example?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule exampleModule = new DialogueModule("One time, I faced an algorithmic anomaly that required me to trace thousands of interconnected nodes, each changing based on time and user input. It was like solving a dynamic puzzle that shifted every moment. It took me days of relentless analysis and simulation to resolve it.");
                                exampleModule.AddOption("That sounds like quite an achievement!",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, exampleModule));
                            });
                        complexModule.AddOption("That sounds challenging.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, complexModule));
                    });
                algorithmModule.AddOption("Thank you for the information.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, algorithmModule));
            });

        greeting.AddOption("What do you protect?",
            player => true,
            player =>
            {
                DialogueModule protectionModule = new DialogueModule("As a guardian, I protect the data and ensure the smooth operation of the realm. Protection in the digital frontier is crucial. Do you value protection?");
                protectionModule.AddOption("Absolutely.",
                    p => true,
                    p =>
                    {
                        DialogueModule agreeModule = new DialogueModule("Good! Protection in the digital world is key. Always stay vigilant.");
                        agreeModule.AddOption("I will.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, agreeModule));
                    });
                protectionModule.AddOption("I'm not sure.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, protectionModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Ella nods at you, her eyes glinting with digital wisdom.");
            });

        return greeting;
    }

    public CyberEngineerElla(Serial serial) : base(serial) { }

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