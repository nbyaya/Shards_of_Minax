using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AstronomerStella : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AstronomerStella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Astronomer Stella";
        Body = 0x191; // Human female body

        // Stats
        SetStr(75);
        SetDex(65);
        SetInt(110);
        SetHits(60);

        // Appearance
        AddItem(new PlainDress(1125));
        AddItem(new Shoes(1135));
        AddItem(new WideBrimHat(1106));

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AstronomerStella(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Astronomer Stella, a seeker of celestial truths. What brings you to gaze at the stars tonight?");

        greeting.AddOption("Tell me about your work.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My purpose is to study the cosmos and uncover its mysteries. The stars, planets, and other celestial bodies hold secrets that can guide our lives.");
                jobModule.AddOption("What kind of mysteries?",
                    p => true,
                    p =>
                    {
                        DialogueModule mysteriesModule = new DialogueModule("The mysteries often lead me to ancient prophecies. Just recently, I stumbled upon a prophecy that speaks of a hero who will bring balance to our world.");
                        mysteriesModule.AddOption("Tell me more about the prophecy.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule prophecyModule = new DialogueModule("The prophecy speaks of dark times, but also of hope. It foretells the rise of a beacon of light amidst the darkness, a symbol of hope for all.");
                                prophecyModule.AddOption("That sounds inspiring.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, prophecyModule));
                            });
                        mysteriesModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, mysteriesModule));
                    });
                jobModule.AddOption("I wish you success in your research.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Can you teach me about the stars?",
            player => true,
            player =>
            {
                DialogueModule starsModule = new DialogueModule("The stars have been particularly bright recently, suggesting a significant event is on the horizon. Have you noticed any anomalies in the sky?");
                starsModule.AddOption("What kind of anomalies?",
                    p => true,
                    p =>
                    {
                        DialogueModule anomaliesModule = new DialogueModule("I've observed several anomalies recently, like shooting stars taking unpredictable paths or constellations slightly shifting. It's both intriguing and concerning.");
                        anomaliesModule.AddOption("What does it mean?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule meaningModule = new DialogueModule("It could mean many things—a change in the world's fate, or perhaps the rise of a new hero. Only time will tell. But there is something else, something much more worrying.");
                                meaningModule.AddOption("What is it that's worrying you?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule worryModule = new DialogueModule("I have seen something through my telescope, something I can scarcely believe. A great eye, a creature the size of a planet, drifting slowly towards our world. Its gaze is fixed upon us, and I fear its intentions are not benevolent.");
                                        worryModule.AddOption("A great eye? What could it be?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule eyeModule = new DialogueModule("The eye is unlike anything I've ever seen before. It's as if the cosmos itself has come alive, and this entity is its herald. It is vast, cold, and filled with an intelligence that feels ancient and uncaring.");
                                                eyeModule.AddOption("Can it be stopped?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule stopModule = new DialogueModule("I do not know if it can be stopped, but I believe we must try. There are ancient texts that speak of cosmic beings, and perhaps they contain some insight. I need someone brave enough to seek these texts, hidden in ruins and guarded by creatures of great power.");
                                                        stopModule.AddOption("I will help you find these texts.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule helpModule = new DialogueModule("Thank you, brave soul. The first text is said to lie within the Temple of the Forgotten Stars, deep within the Whispering Woods. Beware, for the guardians of the temple do not take kindly to intruders.");
                                                                helpModule.AddOption("I'll set out at once.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendMessage("You set off towards the Whispering Woods, determined to find the Temple of the Forgotten Stars.");
                                                                    });
                                                                helpModule.AddOption("That sounds too dangerous for me.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, helpModule));
                                                            });
                                                        stopModule.AddOption("I need time to think about this.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, stopModule));
                                                    });
                                                eyeModule.AddOption("Why is it coming here?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule reasonModule = new DialogueModule("I do not know why it is coming, but I fear it is drawn to something here—something powerful, or perhaps something vulnerable. The stars themselves seem to tremble in its presence.");
                                                        reasonModule.AddOption("How can we prepare?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule prepareModule = new DialogueModule("We must gather knowledge and allies. Speak to the elders in the nearby village, and seek out the druids who are attuned to the cosmic energies. They may have insights that could help us.");
                                                                prepareModule.AddOption("I will speak to them.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendMessage("You decide to seek the counsel of the elders and druids.");
                                                                    });
                                                                prepareModule.AddOption("I need more time to decide.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, prepareModule));
                                                            });
                                                        reasonModule.AddOption("This is too much for me.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, reasonModule));
                                                    });
                                                eyeModule.AddOption("I must leave now.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, eyeModule));
                                            });
                                        worryModule.AddOption("That sounds terrifying. I hope it's just a vision.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, worryModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, meaningModule));
                            });
                        anomaliesModule.AddOption("That's enough for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, anomaliesModule));
                    });
                starsModule.AddOption("I haven't noticed anything unusual.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, starsModule));
            });

        greeting.AddOption("I heard you give rewards to fellow stargazers.",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime > TimeSpan.FromMinutes(10))
                {
                    DialogueModule rewardModule = new DialogueModule("It's heartening to meet another stargazer. For your shared interest, take this telescope. It may help you see the stars more clearly.");
                    rewardModule.AddOption("Thank you!",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
                else
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    noRewardModule.AddOption("Alright, I'll check back later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Astronomer Stella smiles warmly at you.");
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