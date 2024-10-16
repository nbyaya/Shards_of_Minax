using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Fizzlo the Funny")]
public class FizzloTheFunny : BaseCreature
{
    [Constructable]
    public FizzloTheFunny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Fizzlo the Funny";
        Body = 0x190; // Human male body

        // Stats
        SetStr(92);
        SetDex(60);
        SetInt(90);
        SetHits(92);

        // Appearance
        AddItem(new JesterHat() { Hue = 1924 });
        AddItem(new JesterSuit() { Hue = 1254 });
        AddItem(new Boots() { Hue = 1166 });

        // Initialize the NPC
        SpeechHue = 0; // Default speech hue
    }

    public FizzloTheFunny(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ho, ho, ho! I am Fizzlo the Funny, the jester of cryptic mirth! What brings you to my stage today?");

        greeting.AddOption("Tell me about your time at Jester University.",
            player => true,
            player =>
            {
                DialogueModule universityModule = new DialogueModule("Ah, Jester University! A place where laughter is both an art and a science. I learned not only the art of jesting but also the philosophy behind it. Would you like to hear more?");
                universityModule.AddOption("Yes, please tell me more about the philosophy of jesting.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule philosophyModule = new DialogueModule("The philosophy of jesting revolves around the idea that humor is a bridge between hearts. It's about understanding the human condition and using laughter to heal. It teaches us to look at life through a lens of joy! Do you think humor can heal?");
                        philosophyModule.AddOption("Absolutely! Laughter is the best medicine.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        philosophyModule.AddOption("Not really; it's just a distraction.",
                            p => true,
                            p => 
                            {
                                DialogueModule disagreementModule = new DialogueModule("Ah, but distraction can sometimes lead to clarity! When we laugh, we often find the truths we hide from ourselves. Care to explore this idea further?");
                                disagreementModule.AddOption("Okay, let's discuss it more.",
                                    plq => true,
                                    plq => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                disagreementModule.AddOption("No, I'm fine with my perspective.",
                                    plw => true,
                                    plw => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                p.SendGump(new DialogueGump(p, disagreementModule));
                            });
                        player.SendGump(new DialogueGump(player, philosophyModule));
                    });

                universityModule.AddOption("What was your favorite class?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule classModule = new DialogueModule("Ah, my favorite class was 'The Art of the Punchline'! It taught us that timing is everything. A well-placed jest can turn a frown upside down! Have you ever experienced the joy of a perfectly timed joke?");
                        classModule.AddOption("Yes, I love a good joke!",
                            p => true,
                            p => 
                            {
                                DialogueModule jokeModule = new DialogueModule("Then you'll appreciate this: Why did the scarecrow win an award? Because he was outstanding in his field! What's your favorite kind of humor?");
                                jokeModule.AddOption("I love puns!",
                                    ple => true,
                                    ple => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                jokeModule.AddOption("I prefer dark humor.",
                                    plr => true,
                                    plr => 
                                    {
                                        DialogueModule darkHumorModule = new DialogueModule("Ah, dark humor! It can be a delicate balance, but it often highlights the absurdities of life. Do you have a favorite dark joke?");
                                        darkHumorModule.AddOption("I do, but I won't share it.",
                                            pt => true,
                                            pt => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                        darkHumorModule.AddOption("Sure, here's one...",
                                            py => true,
                                            py => 
                                            {
                                                DialogueModule sharedJokeModule = new DialogueModule("That's a good one! Humor can sometimes reveal our innermost thoughts, can't it? Let's not forget to laugh at ourselves too!");
                                                sharedJokeModule.AddOption("True, laughter at oneself is important.",
                                                    plu => true,
                                                    plu => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                sharedJokeModule.AddOption("I prefer to keep my dignity.",
                                                    pli => true,
                                                    pli => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                p.SendGump(new DialogueGump(p, sharedJokeModule));
                                            });
                                        p.SendGump(new DialogueGump(p, darkHumorModule));
                                    });
                                player.SendGump(new DialogueGump(player, jokeModule));
                            });

                        classModule.AddOption("What about the professors?",
                            plo => true,
                            plo =>
                            {
                                DialogueModule professorsModule = new DialogueModule("Oh, the professors were a colorful bunch! Each had their own style. There was Professor Chuckles, who believed that laughter should always come first, and Professor Wisecrack, who favored clever wordplay. Who do you think is more important in jesting: humor or cleverness?");
                                professorsModule.AddOption("Humor is key!",
                                    p => true,
                                    p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                professorsModule.AddOption("Cleverness is crucial.",
                                    p => true,
                                    p => 
                                    {
                                        DialogueModule clevernessModule = new DialogueModule("Cleverness can make a joke resonate deeper, but without humor, it can fall flat! Life is a dance of both. Have you ever had a joke go over your head?");
                                        clevernessModule.AddOption("Yes, many times!",
                                            plp => true,
                                            plp => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        clevernessModule.AddOption("Not really, I catch most of them.",
                                            pla => true,
                                            pla => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        p.SendGump(new DialogueGump(p, clevernessModule));
                                    });
                                player.SendGump(new DialogueGump(player, professorsModule));
                            });
                        
                        player.SendGump(new DialogueGump(player, classModule));
                    });

                player.SendGump(new DialogueGump(player, universityModule));
            });

        greeting.AddOption("What can you tell me about humor?",
            player => true,
            player =>
            {
                DialogueModule humorModule = new DialogueModule("Humor is a magical thing! It can lighten the heaviest of hearts and bring people together. At Jester University, we learned that the best humor often comes from observing the world around us. What kind of humor do you enjoy the most?");
                humorModule.AddOption("I love observational humor.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule observationalModule = new DialogueModule("Observational humor is the essence of connecting with your audience! It turns everyday moments into laughter. Do you have a favorite observational comic?");
                        observationalModule.AddOption("I love stand-up comedians!",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        observationalModule.AddOption("I prefer humorous writers.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, observationalModule));
                    });

                humorModule.AddOption("I enjoy clever wordplay.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule wordplayModule = new DialogueModule("Clever wordplay is a delightful dance of language! It's a favorite among jesters, as it showcases wit. Care to try your hand at a pun?");
                        wordplayModule.AddOption("Sure, hit me with your best pun!",
                            p => true,
                            p => 
                            {
                                DialogueModule punModule = new DialogueModule("Okay, here it goes: I used to be a baker, but I couldn't make enough dough. What do you think?");
                                punModule.AddOption("Good one! I love it.",
                                    pls => true,
                                    pls => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                punModule.AddOption("Eh, it could be better.",
                                    pld => true,
                                    pld => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                p.SendGump(new DialogueGump(p, punModule));
                            });
                        player.SendGump(new DialogueGump(player, wordplayModule));
                    });

                player.SendGump(new DialogueGump(player, humorModule));
            });

        greeting.AddOption("Do you have any riddles?",
            player => true,
            player =>
            {
                DialogueModule riddleModule = new DialogueModule("Ah, a seeker of wisdom! Here's one for you: 'I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.' What am I?");
                riddleModule.AddOption("An echo?",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("Correct! Here is your prize!");
                        // Add the reward logic here
                        pl.AddToBackpack(new MaxxiaScroll()); // Example reward item
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                riddleModule.AddOption("I don't know.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Fizzlo waves goodbye with a flourish!");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
