using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorAlexios : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorAlexios() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Alexios";
        Body = 0x190; // Human male body

        // Stats
        SetStr(130);
        SetDex(70);
        SetInt(80);
        SetHits(100);

        // Appearance
        AddItem(new PlateChest() { Hue = 2213 });
        AddItem(new PlateLegs() { Hue = 2213 });
        AddItem(new PlateHelm() { Hue = 2213 });
        AddItem(new PlateGloves() { Hue = 2213 });
        AddItem(new Boots() { Hue = 2213 });
        AddItem(new WarMace() { Name = "Emperor's Scepter" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public EmperorAlexios(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Emperor Alexios, ruler of Byzantium. What brings you to my court?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("My health is robust, as befits a ruler. A strong body and mind are essential for leading an empire.");
                healthModule.AddOption("That's good to hear, Emperor.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job as Emperor?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My duty is to uphold the Byzantine Empire, a task of great importance. I strive to protect and lead my people with wisdom and justice. The burdens are heavy, but the rewards are immeasurable.");
                jobModule.AddOption("The burden of leadership must be great.",
                    p => true,
                    p =>
                    {
                        DialogueModule burdenModule = new DialogueModule("Indeed, it is. There are constant threats from enemies both within and outside our borders. One must have strength, resolve, and the wisdom to make difficult decisions. The welfare of thousands depends on my every action.");
                        burdenModule.AddOption("How do you manage such a responsibility?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule manageModule = new DialogueModule("I draw strength from the history of my people and the virtues that guide us. The lessons of past emperors, both their triumphs and their failures, are invaluable. I also rely on trusted advisors who help me navigate the complexities of rule.");
                                manageModule.AddOption("Tell me more about past emperors.",
                                    p2 => true,
                                    p2 =>
                                    {
                                        DialogueModule historyModule = new DialogueModule("The history of Byzantium is vast. Emperors like Constantine the Great laid the foundation of our empire, creating a city that would stand as a beacon of civilization. Justinian expanded our territory and created the legal code that still guides us today. But there were also darker times, like the betrayal of Emperor Maurice or the madness of Phocas. Each ruler has left a mark, and it is my duty to learn from them all.");
                                        historyModule.AddOption("What about Constantine the Great?",
                                            p3 => true,
                                            p3 =>
                                            {
                                                DialogueModule constantineModule = new DialogueModule("Constantine was the first to embrace Christianity as the guiding faith of the empire. He moved the capital to Byzantium, renaming it Constantinople. It was a bold decision, one that shifted the power center of the Roman world. His vision was to create a city that would be the new Rome, and his legacy endures in every stone of the city.");
                                                constantineModule.AddOption("A visionary indeed.",
                                                    p4 => true,
                                                    p4 =>
                                                    {
                                                        p4.SendGump(new DialogueGump(p4, CreateGreetingModule()));
                                                    });
                                                p3.SendGump(new DialogueGump(p3, constantineModule));
                                            });
                                        historyModule.AddOption("What about Justinian?",
                                            p3 => true,
                                            p3 =>
                                            {
                                                DialogueModule justinianModule = new DialogueModule("Justinian was one of our greatest emperors. He sought to reunite the lost western provinces of the Roman Empire and succeeded in expanding our territory significantly. His wife, Empress Theodora, was equally influential, guiding him through internal revolts and helping shape policy. The construction of Hagia Sophia, a marvel of architectural beauty, stands as a testament to his ambition.");
                                                justinianModule.AddOption("What was Empress Theodora like?",
                                                    p4 => true,
                                                    p4 =>
                                                    {
                                                        DialogueModule theodoraModule = new DialogueModule("Theodora was a remarkable woman. She rose from humble origins to become one of the most powerful figures in the empire. Her wisdom and resolve saved the empire during the Nika riots, when others advised Justinian to flee. She was a champion for women's rights, passing laws that protected women from exploitation.");
                                                        theodoraModule.AddOption("She sounds like an incredible leader.",
                                                            p5 => true,
                                                            p5 =>
                                                            {
                                                                p5.SendGump(new DialogueGump(p5, CreateGreetingModule()));
                                                            });
                                                        p4.SendGump(new DialogueGump(p4, theodoraModule));
                                                    });
                                                justinianModule.AddOption("A great emperor indeed.",
                                                    p4 => true,
                                                    p4 =>
                                                    {
                                                        p4.SendGump(new DialogueGump(p4, CreateGreetingModule()));
                                                    });
                                                p3.SendGump(new DialogueGump(p3, justinianModule));
                                            });
                                        historyModule.AddOption("The darker times?",
                                            p3 => true,
                                            p3 =>
                                            {
                                                DialogueModule darkTimesModule = new DialogueModule("There were emperors who brought despair instead of glory. Emperor Maurice was betrayed and murdered by his own troops, leading to the rise of Phocas, whose reign was marked by cruelty and instability. Such times serve as a reminder of the fragility of power and the consequences of failing to uphold justice and virtue.");
                                                darkTimesModule.AddOption("A tragic part of history.",
                                                    p4 => true,
                                                    p4 =>
                                                    {
                                                        p4.SendGump(new DialogueGump(p4, CreateGreetingModule()));
                                                    });
                                                p3.SendGump(new DialogueGump(p3, darkTimesModule));
                                            });
                                        p2.SendGump(new DialogueGump(p2, historyModule));
                                    });
                                manageModule.AddOption("That must take great wisdom.",
                                    p2 => true,
                                    p2 =>
                                    {
                                        p2.SendGump(new DialogueGump(p2, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, manageModule));
                            });
                        burdenModule.AddOption("You must be proud of your achievements.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, burdenModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What can you tell me about justice?",
            player => true,
            player =>
            {
                DialogueModule justiceModule = new DialogueModule("The virtue of justice guides my rule. Tell me, do you believe in the just rule of law?");
                justiceModule.AddOption("Yes, I do.",
                    p => true,
                    p =>
                    {
                        DialogueModule responseModule = new DialogueModule("Your answer speaks volumes. In the realm of virtues, justice is the cornerstone of all. Strive to be just in all your actions, and you will find the path to righteousness.");
                        responseModule.AddOption("Thank you for your wisdom, Emperor.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, responseModule));
                    });
                justiceModule.AddOption("I am not sure.",
                    p => true,
                    p =>
                    {
                        DialogueModule doubtModule = new DialogueModule("Doubt is natural, but one must strive to understand the virtues. Justice is not just about punishment; it is about fairness and compassion. True justice requires empathy and the ability to understand all sides of a conflict.");
                        doubtModule.AddOption("I will think on this.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        doubtModule.AddOption("Can you tell me more about justice?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule moreJusticeModule = new DialogueModule("Justice is the foundation upon which empires stand. In Byzantium, we seek to maintain balance and fairness in all things. The laws we create must be just, and those who enforce them must do so without prejudice. In my rule, I strive to correct the mistakes of the past and ensure that every citizen, no matter their status, is treated with respect and dignity.");
                                moreJusticeModule.AddOption("A noble goal indeed.",
                                    p2 => true,
                                    p2 =>
                                    {
                                        p2.SendGump(new DialogueGump(p2, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, moreJusticeModule));
                            });
                        p.SendGump(new DialogueGump(p, doubtModule));
                    });
                player.SendGump(new DialogueGump(player, justiceModule));
            });

        greeting.AddOption("Tell me about Byzantium.",
            player => true,
            player =>
            {
                DialogueModule byzantiumModule = new DialogueModule("Byzantium is a city rich in history and culture. It is my honor and duty to protect and lead its people. Our legacy is one of resilience and glory. The walls of Constantinople have withstood many sieges, and our culture is a blend of Roman, Greek, and Christian influences.");
                byzantiumModule.AddOption("Tell me more about Constantinople.",
                    p => true,
                    p =>
                    {
                        DialogueModule constantinopleModule = new DialogueModule("Constantinople is the heart of our empire, strategically located between Europe and Asia. Its formidable walls have protected us from countless invaders, and its bustling markets are filled with goods from across the known world. The Hagia Sophia, with its magnificent dome, symbolizes the divine inspiration behind our rule.");
                        constantinopleModule.AddOption("What makes the walls so formidable?",
                            p2 => true,
                            p2 =>
                            {
                                DialogueModule wallsModule = new DialogueModule("The Theodosian Walls are among the greatest defensive structures ever built. With multiple layers, including a moat, an outer wall, and an inner wall, they have withstood attacks from Goths, Persians, and even the Huns. They are a testament to the ingenuity and determination of our ancestors.");
                                wallsModule.AddOption("An impressive feat of engineering.",
                                    p3 => true,
                                    p3 =>
                                    {
                                        p3.SendGump(new DialogueGump(p3, CreateGreetingModule()));
                                    });
                                p2.SendGump(new DialogueGump(p2, wallsModule));
                            });
                        constantinopleModule.AddOption("What about the Hagia Sophia?",
                            p2 => true,
                            p2 =>
                            {
                                DialogueModule hagiaSophiaModule = new DialogueModule("The Hagia Sophia is not just a church; it is a symbol of divine power and the unity of the empire. Built under Emperor Justinian's orders, its dome seems to float as if suspended by heaven itself. It has witnessed coronations, prayers, and moments of both triumph and despair. It is the soul of Constantinople.");
                                hagiaSophiaModule.AddOption("A truly wondrous building.",
                                    p3 => true,
                                    p3 =>
                                    {
                                        p3.SendGump(new DialogueGump(p3, CreateGreetingModule()));
                                    });
                                p2.SendGump(new DialogueGump(p2, hagiaSophiaModule));
                            });
                        p.SendGump(new DialogueGump(p, constantinopleModule));
                    });
                byzantiumModule.AddOption("What cultural influences shape Byzantium?",
                    p => true,
                    p =>
                    {
                        DialogueModule cultureModule = new DialogueModule("Byzantium is unique, blending Roman governance, Greek philosophy, and Christian spirituality. Our art, literature, and architecture reflect this blend, creating a culture that is both classical and forward-looking. Our mosaics tell stories from both scripture and history, and our scholars preserve the knowledge of ancient times while adding new discoveries.");
                        cultureModule.AddOption("A fascinating blend of traditions.",
                            p2 => true,
                            p2 =>
                            {
                                p2.SendGump(new DialogueGump(p2, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, cultureModule));
                    });
                player.SendGump(new DialogueGump(player, byzantiumModule));
            });

        greeting.AddOption("What can you tell me about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtueModule = new DialogueModule("Each virtue is a guiding light in the darkness. Speaking of virtues, have you delved into the study of mantras associated with them? For instance, the second syllable of the mantra of Spirituality is JIX.");
                virtueModule.AddOption("What does JIX mean?",
                    p => true,
                    p =>
                    {
                        DialogueModule mantraModule = new DialogueModule("Ah, you show interest in the sacred syllables. The mantras are a means to connect with the virtues on a deeper level. Meditating on them can grant insight and strength. Spirituality, for example, connects us to a higher purpose, reminding us that we are part of something greater.");
                        mantraModule.AddOption("How do the virtues guide you as Emperor?",
                            p2 => true,
                            p2 =>
                            {
                                DialogueModule guideModule = new DialogueModule("As Emperor, I must embody the virtues to set an example for my people. Justice, courage, humility, and compassion are not just ideals; they are essential qualities for leadership. Without them, power becomes tyranny, and the empire would fall into chaos.");
                                guideModule.AddOption("That is a heavy responsibility.",
                                    p3 => true,
                                    p3 =>
                                    {
                                        p3.SendGump(new DialogueGump(p3, CreateGreetingModule()));
                                    });
                                p2.SendGump(new DialogueGump(p2, guideModule));
                            });
                        p.SendGump(new DialogueGump(p, mantraModule));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("Farewell, Emperor.",
            player => true,
            player =>
            {
                player.SendMessage("Emperor Alexios nods at you in acknowledgment.");
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