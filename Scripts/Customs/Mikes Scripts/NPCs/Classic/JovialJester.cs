using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of a jovial jester")]
public class JovialJester : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JovialJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jovial Jester";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        
        // Stats
        SetStr(150);
        SetDex(100);
        SetInt(100);
        SetHits(100);
        
        // Appearance
        AddItem(new FancyShirt() { Hue = 2118 });
        AddItem(new LongPants() { Hue = 2118 });
        AddItem(new JesterHat() { Hue = 2118 });
        AddItem(new LeatherGloves() { Hue = 2118 });
        AddItem(new Shoes() { Hue = 2118 });

        lastRewardTime = DateTime.MinValue; // Initialize to past time
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am the Jovial Jester. How may I entertain you?");

        greeting.AddOption("Tell me a joke.",
            player => true,
            player =>
            {
                DialogueModule jokeModule = new DialogueModule("Why did the chicken cross the road? To get to the other side, of course!");
                jokeModule.AddOption("Ha! Good one!",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                jokeModule.AddOption("Tell me another.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule anotherJoke = new DialogueModule("What did one wall say to the other wall? I'll meet you at the corner!");
                        anotherJoke.AddOption("LOL, I love it!",
                            pla => true,
                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                        anotherJoke.AddOption("That was silly!",
                            pla => true,
                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, anotherJoke));
                    });
                player.SendGump(new DialogueGump(player, jokeModule));
            });

        greeting.AddOption("Do you know any riddles?",
            player => true,
            player =>
            {
                DialogueModule riddleModule = new DialogueModule("Do you enjoy riddles, my friend?");
                riddleModule.AddOption("Yes, tell me a riddle.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule riddleAnswer = new DialogueModule("What has keys but can't open locks? A piano!");
                        riddleAnswer.AddOption("That’s a clever one!",
                            pla => true,
                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                        riddleAnswer.AddOption("Do you have more?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule moreRiddles = new DialogueModule("Here's another: I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?");
                                moreRiddles.AddOption("Hmm, I need to think.",
                                    plaa => true,
                                    plaa => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule())));
                                moreRiddles.AddOption("An echo!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendMessage("Correct! Well done, traveler!");
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, moreRiddles));
                            });
                        pl.SendGump(new DialogueGump(pl, riddleAnswer));
                    });
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        greeting.AddOption("What's your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job is to make people laugh, to forget their worries even if just for a moment. But there’s more to my craft than just jokes.");
                jobModule.AddOption("What else do you do?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moreAboutJob = new DialogueModule("I also share wisdom hidden within jests. Each laugh carries a lesson, if you know where to look. Want to hear one?");
                        moreAboutJob.AddOption("Yes, please!",
                            pla => true,
                            pla =>
                            {
                                DialogueModule wisdomModule = new DialogueModule("Laughter is the best medicine, but too much can lead to neglect. Balance is key, my friend.");
                                wisdomModule.AddOption("That’s profound.",
                                    plaa => true,
                                    plaa => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule())));
                                wisdomModule.AddOption("More wisdom, please!",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule deeperWisdom = new DialogueModule("Remember, every jest has its audience. Not every joke fits every crowd. Tailor your words with care.");
                                        deeperWisdom.AddOption("Wise words indeed!",
                                            plaaa => true,
                                            plaaa => plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule())));
                                        plaa.SendGump(new DialogueGump(plaa, deeperWisdom));
                                    });
                                pla.SendGump(new DialogueGump(pla, wisdomModule));
                            });
                        pl.SendGump(new DialogueGump(pl, moreAboutJob));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Share a secret.",
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
                    player.SendMessage("Ah, a seeker of truths! Very well, one secret I shall share: the third syllable of the mantra of Humility is MUH.");
                    player.AddToBackpack(new Gold(1000)); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("What do you know about mantras?",
            player => true,
            player =>
            {
                DialogueModule mantraModule = new DialogueModule("Mantras are powerful words of magic, each carrying deep meaning. The mantra of Humility is especially potent.");
                mantraModule.AddOption("What does it mean?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule meaningModule = new DialogueModule("Humility teaches us to be grounded, to recognize our limits, and to embrace simplicity. It’s a path to wisdom.");
                        meaningModule.AddOption("How can I practice humility?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule practiceModule = new DialogueModule("Start by listening more than you speak. Appreciate the strengths of others and find joy in their success.");
                                practiceModule.AddOption("That sounds enlightening.",
                                    plaa => true,
                                    plaa => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule())));
                                pla.SendGump(new DialogueGump(pla, practiceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, meaningModule));
                    });
                player.SendGump(new DialogueGump(player, mantraModule));
            });

        greeting.AddOption("What about your dances?",
            player => true,
            player =>
            {
                DialogueModule danceModule = new DialogueModule("Ah, dancing! It's a jesters' way of expressing happiness. If you ever want to learn some moves, let me know!");
                danceModule.AddOption("Can you teach me?",
                    pla => true,
                    pla =>
                    {
                        DialogueModule teachDance = new DialogueModule("Of course! A little jig here, a little twirl there. Follow my lead!");
                        teachDance.AddOption("I'm ready!",
                            plaa => true,
                            plaa => plaa.SendMessage("You attempt to mimic the jester's dance, bringing smiles to those around you."));
                        teachDance.AddOption("Maybe later.",
                            plaa => true,
                            plaa => plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule())));
                        pla.SendGump(new DialogueGump(pla, teachDance));
                    });
                player.SendGump(new DialogueGump(player, danceModule));
            });

        return greeting;
    }

    public JovialJester(Serial serial) : base(serial) { }

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
