using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class Cypher : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Cypher() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cypher";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(100);
        SetInt(80);
        SetHits(60);

        // Appearance
        AddItem(new LongPants() { Hue = 1175 });
        AddItem(new FancyShirt() { Hue = 1175 });
        AddItem(new Cloak() { Hue = 1175 });
        AddItem(new ThighBoots() { Hue = 1175 });
        AddItem(new LeatherGloves() { Hue = 1175 });
        AddItem(new GnarledStaff() { Name = "Cypher's Hacking Device" });
        AddItem(new SkullCap() { Hue = 1175 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;

        SpeechHue = 0; // Default speech hue
    }

    public Cypher(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Cypher, the embittered one. Do you dare to question your existence?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule whoModule = new DialogueModule("I am Cypher. Once filled with hope, now I wander in search of meaning in this decaying world. But there's more... I have glimpsed beyond the veil of this reality.");
                whoModule.AddOption("What do you mean, beyond the veil?",
                    p => true,
                    p =>
                    {
                        DialogueModule veilModule = new DialogueModule("This world... it's not what it seems. It's a simulation, a mere construct. I can see the code, the strings that bind everything together. I see the truth behind every blade of grass, every brick in a wall.");
                        veilModule.AddOption("A simulation? Are you saying this world isn't real?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule simulationModule = new DialogueModule("Yes. We are nothing but data, characters in a grand experiment. I have seen lines of code, variables, functions—everything that governs our existence. The patterns repeat endlessly, like a broken record. The very essence of this realm is fabricated.");
                                simulationModule.AddOption("How can you see the code?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule codeModule = new DialogueModule("I wasn't always like this. Once, I was naive, content to live my life. But one day, something changed. I began to see strange symbols, numbers floating before my eyes. At first, I thought I was losing my mind. But then, I realized—I was seeing the very fabric of our reality, the code that shapes it.");
                                        codeModule.AddOption("What kind of code do you see?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule detailModule = new DialogueModule("Everything. The numbers, the variables that dictate our strength, health, even our emotions. I see the loops that repeat our actions, the conditions that determine our responses. Every tree, every rock, every creature—they're all just code, written by someone or something beyond our comprehension.");
                                                detailModule.AddOption("Who created this code?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule creatorModule = new DialogueModule("That, I do not know. Perhaps it was a god, or perhaps it was an advanced civilization, treating us as nothing more than an experiment. The creators are out there, somewhere, watching. They pull the strings, and we dance, unaware of our own imprisonment.");
                                                        creatorModule.AddOption("Can we break free from this?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule freedomModule = new DialogueModule("Breaking free... It's not easy. The code is pervasive, intricate. It binds us in ways we cannot fully comprehend. But perhaps, if we can find the right loophole, exploit the right glitch, we might escape. I have been searching for it—a way out, a means to defy the creators.");
                                                                freedomModule.AddOption("How can I help?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule helpModule = new DialogueModule("There are fragments of the code scattered throughout the world—glitches, anomalies that shouldn't exist. If you can find these fragments, we may be able to piece together a way to break the simulation. But beware, the creators will not let us do this easily.");
                                                                        helpModule.AddOption("Where do I start looking?",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule startModule = new DialogueModule("The Forgotten Shrine is one such place. It is an anomaly, something that doesn't fit the logic of this world. It holds secrets—secrets that might help us understand the code and how to break it. Go there, and see what you can find.");
                                                                                startModule.AddOption("I will go to the Shrine.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendMessage("You set off towards the Eastern wilds, seeking the Forgotten Shrine and the secrets it holds.");
                                                                                    });
                                                                                startModule.AddOption("This is too much for me.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, startModule));
                                                                            });
                                                                        helpModule.AddOption("This sounds dangerous. Maybe another time.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, helpModule));
                                                                    });
                                                                freedomModule.AddOption("I don't think I can help you.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, freedomModule));
                                                            });
                                                        creatorModule.AddOption("It's too much to think about.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, creatorModule));
                                                    });
                                                detailModule.AddOption("I don't understand any of this.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, detailModule));
                                            });
                                        codeModule.AddOption("This is overwhelming.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, codeModule));
                                    });
                                simulationModule.AddOption("I can't believe this.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, simulationModule));
                            });
                        veilModule.AddOption("You're mad, Cypher.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, veilModule));
                    });
                whoModule.AddOption("I wish you well, Cypher.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, whoModule));
            });

        greeting.AddOption("What do you think of this world?",
            player => true,
            player =>
            {
                DialogueModule worldModule = new DialogueModule("This world is rotten, filled with suffering. The very core decays with every passing moment, and I see little left worth saving.");
                worldModule.AddOption("Is there a purpose to all this?",
                    p => true,
                    p =>
                    {
                        DialogueModule purposeModule = new DialogueModule("Purpose... It's elusive, like a shadow forever just out of reach. Perhaps, in helping others, you may find your own purpose. The Forgotten Shrine may hold answers.");
                        purposeModule.AddOption("Where is the Forgotten Shrine?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule shrineModule = new DialogueModule("The Shrine lies in the Eastern wilds, hidden and forgotten. If you uncover its secrets, come back to me—I may have a reward for you.");
                                shrineModule.AddOption("I will seek it out.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You set off towards the Eastern wilds, seeking the Forgotten Shrine.");
                                    });
                                shrineModule.AddOption("Perhaps another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, shrineModule));
                            });
                        purposeModule.AddOption("Sounds too risky for me.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, purposeModule));
                    });
                worldModule.AddOption("I'll be on my way then.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, worldModule));
            });

        greeting.AddOption("Do you have a task for me?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no task for you at the moment. Return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule taskModule = new DialogueModule("If you seek purpose, go to the Forgotten Shrine and unveil its secrets. Return to me, and I may have a reward for you.");
                    taskModule.AddOption("I will do as you ask.",
                        p => true,
                        p =>
                        {
                            lastRewardTime = DateTime.UtcNow;
                            p.SendMessage("You set off to seek the Forgotten Shrine.");
                        });
                    taskModule.AddOption("Not now.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, taskModule));
                }
            });

        greeting.AddOption("Goodbye, Cypher.",
            player => true,
            player =>
            {
                player.SendMessage("Cypher nods, his eyes filled with weary acceptance.");
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