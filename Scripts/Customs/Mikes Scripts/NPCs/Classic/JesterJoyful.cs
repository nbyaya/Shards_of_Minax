using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Jester Joyful")]
public class JesterJoyful : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JesterJoyful() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jester Joyful";
        Body = 0x190; // Human male body

        // Stats
        SetStr(85);
        SetDex(80);
        SetInt(85);
        SetHits(85);

        // Appearance
        AddItem(new JesterHat() { Hue = 35 });
        AddItem(new JesterSuit() { Hue = 45 });
        AddItem(new Sandals() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public JesterJoyful(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Jester Joyful, the keeper of mirth and wisdom. What brings you to my whimsical domain today?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job is to bring laughter to the world, like sunshine breaking through the clouds! Every jest and joke is a step toward joy!");
                jobModule.AddOption("What is your favorite joke?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule jokeModule = new DialogueModule("Why did the chicken cross the road? To get to the other side! Haha! What a classic!");
                        jokeModule.AddOption("That's funny! Tell me another.",
                            p => true,
                            p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                        pl.SendGump(new DialogueGump(pl, jokeModule));
                    });
                jobModule.AddOption("Do you perform anywhere special?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule performModule = new DialogueModule("Oh, indeed! I perform at festivals and gatherings, spreading cheer wherever I go. It's a merry dance of laughter and joy!");
                        performModule.AddOption("Sounds delightful! Do you have a favorite festival?",
                            p => true,
                            p =>
                            {
                                DialogueModule festivalModule = new DialogueModule("The Festival of Fools is my favorite! Everyone dons silly costumes, and the merriment is contagious! Have you ever been?");
                                festivalModule.AddOption("Not yet! I would love to go!",
                                    plq => true,
                                    plq => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                festivalModule.AddOption("I prefer quieter gatherings.",
                                    plw => true,
                                    plw =>
                                    {
                                        DialogueModule quietModule = new DialogueModule("Ah, the tranquility of a peaceful gathering can be just as refreshing! Laughter can still be found in whispers, you know.");
                                        quietModule.AddOption("You make a good point!",
                                            pe => true,
                                            pe => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                        pl.SendGump(new DialogueGump(pl, quietModule));
                                    });
                                player.SendGump(new DialogueGump(player, festivalModule));
                            });
                        pl.SendGump(new DialogueGump(pl, performModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you know about virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The eight virtues guide us, like stars guiding a sailor at sea. They help us navigate our lives and choices.");
                virtuesModule.AddOption("Can you explain the virtues?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule explainModule = new DialogueModule("Certainly! Each virtue complements the others, creating harmony. They are Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
                        explainModule.AddOption("Which one do you think is the most important?",
                            p => true,
                            p =>
                            {
                                DialogueModule importantModule = new DialogueModule("Ah, a challenging question! I believe Compassion is vital; it fosters understanding and connection between us. What do you think?");
                                importantModule.AddOption("I agree! Compassion is essential.",
                                    plr => true,
                                    plr => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                importantModule.AddOption("I think Honor is more important.",
                                    plt => true,
                                    plt =>
                                    {
                                        DialogueModule honorModule = new DialogueModule("Honor is indeed noble! It shapes our character and guides our actions. We must uphold it fiercely!");
                                        honorModule.AddOption("Wise words, Jester!",
                                            py => true,
                                            py => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                        pl.SendGump(new DialogueGump(pl, honorModule));
                                    });
                                player.SendGump(new DialogueGump(player, importantModule));
                            });
                        player.SendGump(new DialogueGump(player, explainModule));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Do you have a reward for laughter?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                DialogueModule rewardModule = new DialogueModule("Ah, laughter! It is the best medicine! Here's a little reward from me.");
                player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                rewardModule.AddOption("Thank you, Jester! What else do you know?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moreKnowledge = new DialogueModule("I know many tales and riddles! Ask me about tales of old, and I'll regale you with stories of heroes and their quests!");
                        moreKnowledge.AddOption("Tell me a tale!",
                            p => true,
                            p =>
                            {
                                DialogueModule taleModule = new DialogueModule("Once, there was a brave knight who faced a terrible dragon to save a kingdom. With courage and cleverness, he turned the beast into a friend!");
                                taleModule.AddOption("What happened next?",
                                    plu => true,
                                    plu =>
                                    {
                                        DialogueModule nextModule = new DialogueModule("Together, they protected the kingdom from invaders. The knight learned that understanding and friendship could conquer fear. A true tale of camaraderie!");
                                        nextModule.AddOption("What a wonderful story!",
                                            pi => true,
                                            pi => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                        pl.SendGump(new DialogueGump(pl, nextModule));
                                    });
                                player.SendGump(new DialogueGump(player, taleModule));
                            });
                        pl.SendGump(new DialogueGump(pl, moreKnowledge));
                    });
                player.SendGump(new DialogueGump(player, rewardModule));
            });

        greeting.AddOption("What is folly?",
            player => true,
            player =>
            {
                DialogueModule follyModule = new DialogueModule("Folly reminds us of our humanity! Through folly, we learn, grow, and laugh at ourselves! It keeps life interesting, don’t you think?");
                follyModule.AddOption("Absolutely! Tell me more.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moreFolly = new DialogueModule("One must embrace folly! Without it, we'd be too serious! Every mistake can turn into a memorable story. What folly have you encountered?");
                        moreFolly.AddOption("I once slipped on a banana peel!",
                            p => true,
                            p =>
                            {
                                DialogueModule slipModule = new DialogueModule("Ah, the classic slip! Such moments bring joy to others! Did you laugh afterward?");
                                slipModule.AddOption("Of course! I couldn't help it.",
                                    plo => true,
                                    plo => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                slipModule.AddOption("I was embarrassed.",
                                    plp => true,
                                    plp =>
                                    {
                                        DialogueModule embarrassmentModule = new DialogueModule("Embarrassment is fleeting! It's the laughter that lasts! Embrace it, my friend!");
                                        embarrassmentModule.AddOption("Wise advice, Jester!",
                                            pa => true,
                                            pa => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                        pl.SendGump(new DialogueGump(pl, embarrassmentModule));
                                    });
                                player.SendGump(new DialogueGump(player, slipModule));
                            });
                        pl.SendGump(new DialogueGump(pl, moreFolly));
                    });
                player.SendGump(new DialogueGump(player, follyModule));
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
