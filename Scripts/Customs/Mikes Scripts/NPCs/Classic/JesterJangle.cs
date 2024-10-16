using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Jester Jangle")]
public class JesterJangle : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JesterJangle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jester Jangle";
        Body = 0x191; // Human male body

        // Stats
        Str = 130;
        Hits = 75;

        // Appearance
        AddItem(new JesterHat() { Hue = 2150 });
        AddItem(new JesterSuit() { Hue = 1287 });
        AddItem(new Boots() { Hue = 2105 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0;

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public JesterJangle(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Welcome, traveler! I am Jester Jangle, the mirthful one. What brings you to my humble presence?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I spread joy and ponder the eight virtues. But my path is filled with riddles and reflections.");
                jobModule.AddOption("What are the eight virtues?",
                    pl => true,
                    pl => { pl.SendGump(new DialogueGump(pl, CreateVirtuesModule())); });
                jobModule.AddOption("What kind of joy do you spread?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule joyDetailModule = new DialogueModule("The joy of laughter, the joy of kindness! I seek to brighten the hearts of those who cross my path.");
                        joyDetailModule.AddOption("How do you spread this joy?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule spreadJoyModule = new DialogueModule("Through jokes, tales, and perhaps a dance or two! Would you like to hear a joke?");
                                spreadJoyModule.AddOption("Sure, tell me a joke.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, CreateJokeModule()));
                                    });
                                spreadJoyModule.AddOption("Maybe later.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, greeting));
                                    });
                                player.SendGump(new DialogueGump(player, spreadJoyModule));
                            });
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think about balance?",
            player => true,
            player =>
            {
                DialogueModule balanceModule = new DialogueModule("Balance is key in all things. The virtues dance together, creating harmony. Have you ever thought of how they interact?");
                balanceModule.AddOption("Can you explain their interactions?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule interactionModule = new DialogueModule("Absolutely! For example, Compassion and Honor often walk hand in hand. When you act kindly, you uphold your honor.");
                        interactionModule.AddOption("What about Justice?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule justiceModule = new DialogueModule("Justice is a sturdy pillar. Without it, the other virtues may falter. But remember, true justice comes from understanding.");
                                justiceModule.AddOption("How can I understand better?",
                                    plb => true,
                                    plb =>
                                    {
                                        DialogueModule understandingModule = new DialogueModule("Seek knowledge! Speak with those around you and learn their stories. Each person holds a unique perspective.");
                                        player.SendGump(new DialogueGump(player, understandingModule));
                                    });
                                player.SendGump(new DialogueGump(player, justiceModule));
                            });
                        player.SendGump(new DialogueGump(player, interactionModule));
                    });
                player.SendGump(new DialogueGump(player, balanceModule));
            });

        greeting.AddOption("Do you have any riddles?",
            player => true,
            player =>
            {
                DialogueModule riddleModule = new DialogueModule("Ah, riddles! They are the essence of mystery. Here’s one for you: In a world without justice, can there be true honor?");
                riddleModule.AddOption("What do you mean by that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule meaningModule = new DialogueModule("Without justice, honor is merely a shadow—a fleeting concept. What is your answer to the riddle?");
                        meaningModule.AddOption("Honor can exist through personal integrity.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule integrityModule = new DialogueModule("An excellent perspective! Honor can indeed exist independently, but the world needs justice to reflect it.");
                                player.SendGump(new DialogueGump(player, integrityModule));
                            });
                        meaningModule.AddOption("I don’t know the answer.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule noAnswerModule = new DialogueModule("That’s okay! It’s a tough one. Reflect on it and perhaps the answer will reveal itself to you in time.");
                                player.SendGump(new DialogueGump(player, noAnswerModule));
                            });
                        player.SendGump(new DialogueGump(player, meaningModule));
                    });
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        greeting.AddOption("What can you tell me about joy?",
            player => true,
            player =>
            {
                DialogueModule joyModule = new DialogueModule("Joy is not just an emotion; it’s a way of life. When joy is shared, it multiplies! How do you find joy in your life?");
                joyModule.AddOption("I find joy in helping others.",
                    pla => true,
                    pla =>
                    {
                        DialogueModule helpingJoyModule = new DialogueModule("A noble pursuit! Helping others can illuminate your own path and bring happiness to both you and those you aid.");
                        player.SendGump(new DialogueGump(player, helpingJoyModule));
                    });
                joyModule.AddOption("I seek joy through adventure.",
                    pla => true,
                    pla =>
                    {
                        DialogueModule adventureJoyModule = new DialogueModule("Ah, the thrill of adventure! Each new experience brings a fresh spark of joy. Do you have a favorite tale of adventure?");
                        adventureJoyModule.AddOption("Yes! Let me share it with you.",
                            plb => true,
                            plb =>
                            {
                                DialogueModule shareTaleModule = new DialogueModule("I would love to hear it! Sharing tales is a joy in itself. Please, go ahead!");
                                player.SendGump(new DialogueGump(player, shareTaleModule));
                            });
                        adventureJoyModule.AddOption("Not at the moment.",
                            plb => true,
                            plb =>
                            {
                                player.SendGump(new DialogueGump(player, joyModule));
                            });
                        player.SendGump(new DialogueGump(player, adventureJoyModule));
                    });
                player.SendGump(new DialogueGump(player, joyModule));
            });

        greeting.AddOption("Can you reward me for seeking the balance?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To seek balance is to accept the challenge of the virtues. Here, take this token of appreciation.");
                    player.AddToBackpack(new FatigueAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule())); // Return to greeting
            });

        return greeting;
    }

    private DialogueModule CreateVirtuesModule()
    {
        DialogueModule virtuesModule = new DialogueModule("The virtues are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility. Each plays a vital role in our lives.");
        virtuesModule.AddOption("Tell me more about Honesty.",
            player => true,
            player =>
            {
                DialogueModule honestyModule = new DialogueModule("Honesty is the foundation upon which trust is built. Without it, relationships crumble.");
                honestyModule.AddOption("What about Compassion?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule compassionModule = new DialogueModule("Compassion allows us to empathize with others. It is the bridge between isolation and connection.");
                        player.SendGump(new DialogueGump(player, compassionModule));
                    });
                player.SendGump(new DialogueGump(player, honestyModule));
            });
        virtuesModule.AddOption("What about Valor?",
            player => true,
            player =>
            {
                DialogueModule valorModule = new DialogueModule("Valor is the courage to act rightly in the face of adversity. It inspires others to stand strong.");
                valorModule.AddOption("And Justice?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule justiceModule = new DialogueModule("Justice ensures fairness and equality. It is the safeguard of peace and order in our society.");
                        player.SendGump(new DialogueGump(player, justiceModule));
                    });
                player.SendGump(new DialogueGump(player, valorModule));
            });
        return virtuesModule;
    }

    private DialogueModule CreateJokeModule()
    {
        DialogueModule jokeModule = new DialogueModule("Why don’t scientists trust atoms? Because they make up everything!");
        jokeModule.AddOption("Ha! That's funny.",
            player => true,
            player =>
            {
                DialogueModule laughterModule = new DialogueModule("I’m glad you enjoyed it! Laughter is the best medicine, after all.");
                player.SendGump(new DialogueGump(player, laughterModule));
            });
        jokeModule.AddOption("Do you have another one?",
            player => true,
            player =>
            {
                DialogueModule anotherJokeModule = new DialogueModule("Sure! What did the ocean say to the beach? Nothing, it just waved!");
                player.SendGump(new DialogueGump(player, anotherJokeModule));
            });
        return jokeModule;
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
