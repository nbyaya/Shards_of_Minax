using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class FinneganTheJester : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public FinneganTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Finnegan the Jester";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(80);
        SetInt(80);
        SetHits(80);

        // Appearance
        AddItem(new JesterHat() { Hue = 1150 });
        AddItem(new JesterSuit() { Hue = 1150 });
        AddItem(new ThighBoots() { Hue = 1150 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;

        // Speech Hue
        SpeechHue = 0; // Default speech hue
    }

    public FinneganTheJester(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Finnegan, the court's jester. How may I entertain you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Finnegan, the court's jester! I bring laughter, riddles, and sometimes a little bit of magic to the realm. What else would you like to know?");
                identityModule.AddOption("Tell me about your job.",
                    p => true,
                    p =>
                    {
                        DialogueModule jobModule = new DialogueModule("My job is to entertain! I juggle, tell riddles, and make everyone laugh—from kings to peasants. But there was one jest that I am particularly proud of... Want to hear about it?");
                        jobModule.AddOption("Yes, tell me about your best jest.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule bestJestModule = new DialogueModule("Ah, the time I stole Lord British's pants! Now that was a jest for the ages. Picture it—Lord British, the most dignified ruler in the land, and there I was with a plan to make even the stoniest knight crack a smile.");
                                bestJestModule.AddOption("How did you manage to do that?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule planningModule = new DialogueModule("It all started with a simple observation: Lord British always left his chambers unlocked during his morning meditation. I knew that if I timed it perfectly, I could sneak in and snatch his royal trousers while he was deep in thought. Of course, it wasn't just about the sneaking—it was about the execution, the flair, the showmanship!");
                                        planningModule.AddOption("Weren't you afraid of getting caught?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule caughtModule = new DialogueModule("Afraid? Ha! Fear is just part of the fun! Besides, I had a plan for that too. I disguised myself as one of the royal laundry servants. If anyone had caught me, I would have simply claimed that I was taking his pants for a wash. The trick is to always have a backup story!");
                                                caughtModule.AddOption("What happened next?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule nextModule = new DialogueModule("Once I had the pants, I had to make sure everyone saw it. I dashed through the courtyard, waving the trousers in the air, shouting, 'Behold, the Emperor's new clothes!' The guards were too stunned to react at first, and the entire court burst into laughter!");
                                                        nextModule.AddOption("How did Lord British react?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule reactionModule = new DialogueModule("Ah, Lord British... At first, his face turned the shade of a ripe tomato. But then, much to everyone's surprise, he started laughing too. He knew a good jest when he saw one, and he appreciated the humor. He even declared a feast that evening to celebrate 'the lightening of royal attire'!");
                                                                reactionModule.AddOption("That sounds incredible!",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendMessage("Indeed it was! That day, I truly earned my title as the court's jester. Even Lord British himself couldn't help but laugh!");
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                reactionModule.AddOption("Did you get into trouble afterward?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule troubleModule = new DialogueModule("Well, there was a bit of a consequence. I had to spend a week scrubbing the castle floors as punishment—but it was worth every moment! Besides, it gave me plenty of time to think up my next big jest.");
                                                                        troubleModule.AddOption("You certainly are brave, Finnegan!",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendMessage("Brave or foolish, it's all the same when you're a jester! As long as there's laughter, I'm content.");
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, troubleModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, reactionModule));
                                                            });
                                                        nextModule.AddOption("That's hilarious!",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Laughter is the greatest reward, my friend. And that day, the entire kingdom laughed!");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, nextModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, caughtModule));
                                            });
                                        planningModule.AddOption("You must be quite sneaky!",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Sneaky? Perhaps. But more importantly, one must be bold! Fortune favors the foolish, after all!");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, planningModule));
                                    });
                                bestJestModule.AddOption("Why Lord British's pants, of all things?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule whyPantsModule = new DialogueModule("Ah, you see, pants are a symbol of dignity! To take the pants of the most powerful person in the land—that's the essence of jesting, to humble the mighty and uplift the spirits of everyone else. It was all in good fun, and Lord British knew that too.");
                                        whyPantsModule.AddOption("That makes sense, in a jester's way.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Exactly! A good jest should make people laugh, but also think. It should remind everyone that no matter how powerful they are, they can still be part of the joke.");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, whyPantsModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, bestJestModule));
                            });
                        jobModule.AddOption("Maybe later.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, jobModule));
                    });
                identityModule.AddOption("Tell me about your family.",
                    p => true,
                    p =>
                    {
                        DialogueModule familyModule = new DialogueModule("My family has served the court for generations. My grandfather was a jester too, and he taught me all his tricks. Would you like to hear a story about him?");
                        familyModule.AddOption("Yes, tell me a story about your grandfather.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule storyModule = new DialogueModule("Ah, my dear grandfather! He had a special jingle he'd perform with bells. It was said to be magical. I still remember it. Want to hear the jingle?");
                                storyModule.AddOption("Yes, please!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("*Finnegan performs a joyful jingle with an imaginary bell*");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                storyModule.AddOption("Maybe another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, storyModule));
                            });
                        familyModule.AddOption("Maybe later.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, familyModule));
                    });
                identityModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Farewell, traveler! May laughter always find you!");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("Can you reward me for making you laugh?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward for you right now. Please come back later when I've replenished my bag of tricks!");
                    noRewardModule.AddOption("Alright, I'll come back later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("Ah, you've truly entertained me! Here, take this as a token of my appreciation.");
                    rewardModule.AddOption("Thank you, Finnegan!",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye, jester.",
            player => true,
            player =>
            {
                player.SendMessage("Farewell, traveler! May laughter brighten your path!");
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