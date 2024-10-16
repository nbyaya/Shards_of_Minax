using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class MorosTheBonecaller : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MorosTheBonecaller() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Moros the Bonecaller";
        Body = 0x190; // Human male body

        // Stats
        SetStr(138);
        SetDex(50);
        SetInt(135);
        SetHits(115);

        // Appearance
        AddItem(new Robe() { Hue = 2126 });
        AddItem(new Sandals() { Hue = 1175 });
        AddItem(new SkullCap() { Hue = 1 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("I am Moros the Bonecaller, master of the dark arts! What brings you to my domain?");

        greeting.AddOption("Tell me about your powers.",
            player => true,
            player => 
            {
                DialogueModule powerModule = new DialogueModule("My health is sustained by the dark forces that bind me. Power is not to be taken lightly. Do you seek knowledge of the dark arts, or perhaps you desire a demonstration?");
                powerModule.AddOption("I seek knowledge of the dark arts.",
                    p => true,
                    p => 
                    {
                        if (SpeechCooldownElapsed())
                        {
                            p.SendGump(new DialogueGump(p, CreateKnowledgeModule()));
                        }
                        else
                        {
                            p.SendMessage("I have no reward for you at this moment. Please return later.");
                        }
                    });
                powerModule.AddOption("Show me a demonstration.",
                    p => true,
                    p => 
                    {
                        p.SendMessage("Moros conjures a dark energy, creating a fleeting apparition of a ghoul, only to watch it dissolve into shadows.");
                        p.SendGump(new DialogueGump(p, greeting));
                    });
                powerModule.AddOption("No, just curious.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, greeting)));
                player.SendGump(new DialogueGump(player, powerModule));
            });

        greeting.AddOption("What is a necromancer?",
            player => true,
            player => 
            {
                DialogueModule necromancerModule = new DialogueModule("I harness the energy of the deceased, bending it to my will. This power comes with great responsibility. Are you here to learn or to challenge my authority?");
                necromancerModule.AddOption("I want to learn.",
                    p => true,
                    p => 
                    {
                        DialogueModule learnModule = new DialogueModule("To learn from me, you must first prove your worth. The path of darkness is not for the faint of heart. What do you wish to know?");
                        learnModule.AddOption("What must I do to prove myself?",
                            pl => true,
                            pl => 
                            {
                                DialogueModule taskModule = new DialogueModule("Bring me the skull of a ghoul as proof of your dedication. Only then can I teach you the secrets of necromancy.");
                                taskModule.AddOption("I will find it!",
                                    pq => true,
                                    pq => 
                                    {
                                        p.SendMessage("You set off to find the ghoul's skull.");
                                        // Add any additional task-related logic here
                                    });
                                taskModule.AddOption("I'm not ready for such a task.",
                                    plw => true,
                                    plw => pl.SendGump(new DialogueGump(pl, greeting)));
                                pl.SendGump(new DialogueGump(pl, taskModule));
                            });
                        learnModule.AddOption("Tell me more about necromancy.",
                            pl => true,
                            pl => 
                            {
                                DialogueModule necromancyDetails = new DialogueModule("Necromancy is the art of communicating with the dead and controlling their spirits. It requires not only skill but also an understanding of the cost involved. Do you still wish to pursue this path?");
                                necromancyDetails.AddOption("Yes, I wish to learn.",
                                    ple => true,
                                    ple => pl.SendGump(new DialogueGump(pl, learnModule)));
                                necromancyDetails.AddOption("Maybe another time.",
                                    plr => true,
                                    plr => pl.SendGump(new DialogueGump(pl, greeting)));
                                pl.SendGump(new DialogueGump(pl, necromancyDetails));
                            });
                        player.SendGump(new DialogueGump(player, learnModule));
                    });
                necromancerModule.AddOption("I am just curious.",
                    p => true,
                    p => p.SendGump(new DialogueGump(p, greeting)));
                player.SendGump(new DialogueGump(player, necromancerModule));
            });

        greeting.AddOption("What is the cost of your powers?",
            player => true,
            player => 
            {
                DialogueModule costModule = new DialogueModule("The cost is the very essence of one's being. To harness the dark forces, one must be willing to sacrifice. Are you prepared to pay that price?");
                costModule.AddOption("I will do what it takes.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, greeting)));
                costModule.AddOption("I don't think I can.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule hesitationModule = new DialogueModule("The path of darkness is fraught with peril. It is wise to reconsider your choices. Do you wish to explore another subject?");
                        hesitationModule.AddOption("What else can you teach me?",
                            hpl => true,
                            hpl => hpl.SendGump(new DialogueGump(hpl, greeting)));
                        hesitationModule.AddOption("No, I want to know more about the cost.",
                            hpl => true,
                            hpl => hpl.SendGump(new DialogueGump(hpl, costModule)));
                        pl.SendGump(new DialogueGump(pl, hesitationModule));
                    });
                player.SendGump(new DialogueGump(player, costModule));
            });

        greeting.AddOption("Do you have any tasks for me?",
            player => true,
            player =>
            {
                DialogueModule taskModule = new DialogueModule("If you wish to embrace the darkness, bring me the skull of a ghoul as proof of your dedication. In return, I will reward you with knowledge and power. Are you ready for this challenge?");
                taskModule.AddOption("I will find it!",
                    pl => true,
                    pl => 
                    {
                        pl.SendMessage("You set off to find the ghoul's skull.");
                        // Additional task logic can be added here
                    });
                taskModule.AddOption("I'm not ready for such a task.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, greeting)));
                player.SendGump(new DialogueGump(player, taskModule));
            });

        greeting.AddOption("What do you think of other magic schools?",
            player => true,
            player => 
            {
                DialogueModule schoolsModule = new DialogueModule("Each school of magic has its merits and dangers. Sorcery is chaotic, while healing can be a double-edged sword. But necromancy, it is a path few dare to tread. Do you seek to understand them?");
                schoolsModule.AddOption("Yes, tell me more.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule sorceryModule = new DialogueModule("Sorcery harnesses the chaotic forces of the universe. It can grant immense power but often at great personal cost. Those who wield it must be cautious, lest they become consumed by it.");
                        sorceryModule.AddOption("What about healing?",
                            hpl => true,
                            hpl => 
                            {
                                DialogueModule healingModule = new DialogueModule("Healing magic is noble but can be burdensome. It demands a great deal of empathy and selflessness. Healers often bear the emotional weight of those they save.");
                                healingModule.AddOption("What do you prefer?",
                                    hplq => true,
                                    hplq => 
                                    {
                                        DialogueModule preferenceModule = new DialogueModule("I prefer the shadows. The thrill of bending life and death to my will is intoxicating. But remember, with great power comes greater consequences. Are you willing to face them?");
                                        preferenceModule.AddOption("I will embrace the shadows!",
                                            ph => true,
                                            ph => ph.SendGump(new DialogueGump(ph, greeting)));
                                        preferenceModule.AddOption("I will tread carefully.",
                                            ph => true,
                                            ph => ph.SendGump(new DialogueGump(ph, greeting)));
                                        hpl.SendGump(new DialogueGump(hpl, preferenceModule));
                                    });
                                hpl.SendGump(new DialogueGump(hpl, healingModule));
                            });
                        pl.SendGump(new DialogueGump(pl, sorceryModule));
                    });
                schoolsModule.AddOption("No, I am not interested.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, greeting)));
                player.SendGump(new DialogueGump(player, schoolsModule));
            });

        return greeting;
    }

    private DialogueModule CreateKnowledgeModule()
    {
        DialogueModule knowledgeModule = new DialogueModule("Knowledge is power, but in necromancy, it can also be a curse. What specific secrets do you wish to uncover?");
        knowledgeModule.AddOption("Tell me about the dark arts.",
            pl => true,
            pl => 
            {
                DialogueModule darkArtsModule = new DialogueModule("The dark arts involve summoning the dead, manipulating shadows, and understanding the afterlife. It is both a science and an art. Are you prepared to delve into such forbidden knowledge?");
                darkArtsModule.AddOption("Yes, I want to learn.",
                    pla => true,
                    pla => pl.SendGump(new DialogueGump(pl, knowledgeModule))); // Redirect to knowledge module
                darkArtsModule.AddOption("No, it's too dangerous.",
                    pls => true,
                    pls => pl.SendGump(new DialogueGump(pl, CreateGreetingModule()))); // Redirect to greeting
                pl.SendGump(new DialogueGump(pl, darkArtsModule));
            });
        knowledgeModule.AddOption("What are the risks of necromancy?",
            pl => true,
            pl => 
            {
                DialogueModule riskModule = new DialogueModule("The risks are profound. One may lose their sanity, be haunted by vengeful spirits, or become a target for other dark practitioners. Do you still wish to proceed?");
                riskModule.AddOption("Yes, I am ready.",
                    pld => true,
                    pld => pl.SendGump(new DialogueGump(pl, knowledgeModule))); // Redirect to knowledge module
                riskModule.AddOption("No, I've reconsidered.",
                    plf => true,
                    plf => pl.SendGump(new DialogueGump(pl, CreateGreetingModule()))); // Redirect to greeting
                pl.SendGump(new DialogueGump(pl, riskModule));
            });

        return knowledgeModule;
    }

    private bool SpeechCooldownElapsed()
    {
        return (DateTime.UtcNow - lastRewardTime).TotalMinutes >= 10;
    }

    public MorosTheBonecaller(Serial serial) : base(serial) { }

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
