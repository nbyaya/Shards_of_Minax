using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DrElara : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DrElara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dr. Elara";
        Body = 0x191; // Human female body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(100);
        SetHits(60);

        // Appearance
        AddItem(new Skirt() { Hue = 1150 });
        AddItem(new FancyShirt() { Hue = 1133 });
        AddItem(new HalfApron() { Hue = 0 });
        AddItem(new ThighBoots() { Hue = 1103 });
        AddItem(new HalfApron() { Hue = 1910 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(true); // true for female
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DrElara(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Dr. Elara, a scientist with a deep passion for the study of weather science. How may I assist you today?");

        greeting.AddOption("What is your name?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("I am Dr. Elara, a scientist devoted to the pursuit of knowledge and understanding of the world's mysteries, especially the fascinating phenomena of weather.");
                nameModule.AddOption("Thank you.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I'm in perfect health, thank you for asking. Staying active with my experiments and studies of weather keeps me well.");
                healthModule.AddOption("Glad to hear it.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What do you do?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I am a scientist, fascinated by the mysteries of the world, particularly the wonders of weather. The intricate patterns of storms, the beauty of the clouds, and the delicate balance of atmospheric conditions all captivate my curiosity. I devote my time to studying and researching these wonders, hoping to uncover hidden truths.");
                jobModule.AddOption("That sounds fascinating.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule fascinationModule = new DialogueModule("Indeed, it truly is. Have you ever watched the clouds roll across the sky and wondered about the forces at play? The wind, the temperature, the humidity—all coming together to create something as simple yet as magnificent as a rain shower.");
                        fascinationModule.AddOption("I have. It's quite a spectacle.",
                            p => true,
                            p =>
                            {
                                DialogueModule spectacleModule = new DialogueModule("Exactly! Each drop of rain, each gust of wind, tells a story of nature's grand design. My passion is to understand and share these stories.");
                                spectacleModule.AddOption("How do you study the weather?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule studyModule = new DialogueModule("I use a variety of tools—barometers to measure pressure, anemometers for wind speed, and hygrometers for humidity. I even have a few magical instruments that allow me to glimpse weather patterns beyond the normal perception. Would you like to know more about any of these instruments?");
                                        studyModule.AddOption("Tell me about the barometer.",
                                            pla => true,
                                            pla =>
                                            {
                                                DialogueModule barometerModule = new DialogueModule("The barometer measures atmospheric pressure. When pressure drops, it often means a storm is approaching. It's incredible how such a simple device can help predict something so powerful.");
                                                barometerModule.AddOption("That's amazing.",
                                                    plb => true,
                                                    plb =>
                                                    {
                                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                    });
                                                pla.SendGump(new DialogueGump(pla, barometerModule));
                                            });
                                        studyModule.AddOption("Tell me about the anemometer.",
                                            pla => true,
                                            pla =>
                                            {
                                                DialogueModule anemometerModule = new DialogueModule("The anemometer measures wind speed. Wind is a key factor in determining weather changes. The faster the wind, the more likely a dramatic shift in weather is coming. It's exhilarating to see the readings rise just before a storm hits.");
                                                anemometerModule.AddOption("It must be thrilling.",
                                                    plb => true,
                                                    plb =>
                                                    {
                                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                    });
                                                pla.SendGump(new DialogueGump(pla, anemometerModule));
                                            });
                                        studyModule.AddOption("Tell me about the hygrometer.",
                                            pla => true,
                                            pla =>
                                            {
                                                DialogueModule hygrometerModule = new DialogueModule("The hygrometer measures humidity—the amount of moisture in the air. Humidity is crucial for predicting rain and fog. High humidity often means that precipitation is on the way.");
                                                hygrometerModule.AddOption("Interesting. Thank you.",
                                                    plb => true,
                                                    plb =>
                                                    {
                                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                                    });
                                                pla.SendGump(new DialogueGump(pla, hygrometerModule));
                                            });
                                        studyModule.AddOption("Maybe another time.",
                                            pla => true,
                                            pla =>
                                            {
                                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                            });
                                        pl.SendGump(new DialogueGump(pl, studyModule));
                                    });
                                spectacleModule.AddOption("Perhaps you could teach me more sometime.",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, spectacleModule));
                            });
                        fascinationModule.AddOption("I haven't, but now I'm curious.",
                            p => true,
                            p =>
                            {
                                DialogueModule curiosityModule = new DialogueModule("Curiosity is the first step toward knowledge. I encourage you to watch the sky, feel the wind, and let nature speak to you. The more you observe, the more you'll understand the incredible balance at play.");
                                curiosityModule.AddOption("I will do that.",
                                    ple => true,
                                    ple =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, curiosityModule));
                            });
                        player.SendGump(new DialogueGump(player, fascinationModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you value knowledge?",
            player => true,
            player =>
            {
                DialogueModule knowledgeModule = new DialogueModule("The pursuit of knowledge is a noble endeavor. It aligns with the virtue of spirituality. Do you value knowledge, traveler?");
                knowledgeModule.AddOption("Yes, I do.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Knowledge is indeed valuable. It is the key to unlocking the mysteries of our world.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                knowledgeModule.AddOption("Not really.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("A pity. Knowledge can open many doors.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, knowledgeModule));
            });

        greeting.AddOption("Can you tell me about your family?",
            player => true,
            player =>
            {
                DialogueModule familyModule = new DialogueModule("I come from a long line of scholars. My grandfather was a famed historian. Our family has always pursued knowledge above all else. In particular, my interest in weather science was inspired by my mother, who was fascinated by the winds and the stars.");
                familyModule.AddOption("Your mother sounds interesting.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule motherModule = new DialogueModule("She truly was. She would take me to the hills at night to watch the stars and feel the wind's gentle caress. She believed that the weather and the stars were deeply connected, and that understanding one would help in understanding the other.");
                        motherModule.AddOption("Did she teach you about the weather?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule teachingModule = new DialogueModule("Yes, she taught me to observe and appreciate the subtleties of weather. She taught me to read the clouds, feel the wind, and understand the patterns. It was her influence that led me to become a scientist dedicated to studying the weather.");
                                teachingModule.AddOption("She must have been proud of you.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                    });
                                pla.SendGump(new DialogueGump(pla, teachingModule));
                            });
                        familyModule.AddOption("Thank you for sharing.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, motherModule));
                    });
                player.SendGump(new DialogueGump(player, familyModule));
            });

        greeting.AddOption("What mysteries are you researching?",
            player => true,
            player =>
            {
                DialogueModule mysteryModule = new DialogueModule("I've been researching a rare herb with unique properties. If you happen to come across it, I may have a reward for you. Are you familiar with the herb?");
                mysteryModule.AddOption("Tell me more about this herb.",
                    pl => true,
                    pl =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            pl.SendMessage("The herb has a unique blue hue and a sweet scent. Bring it to me, and you shall have my gratitude and a reward.");
                            pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                mysteryModule.AddOption("I'm not familiar with it.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, mysteryModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Dr. Elara nods at you.");
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