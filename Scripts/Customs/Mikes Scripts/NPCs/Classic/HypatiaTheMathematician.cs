using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Hypatia the Mathematician")]
public class HypatiaTheMathematician : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public HypatiaTheMathematician() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Hypatia the Mathematician";
        Body = 0x191; // Human female body

        // Stats
        Str = 72;
        Dex = 48;
        Int = 125;
        Hits = 61;

        // Appearance
        AddItem(new FancyDress() { Hue = 2220 });
        AddItem(new Sandals() { Hue = 1178 });
        AddItem(new Spellbook() { Name = "Hypatia's Theorems" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(true);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Hypatia the Mathematician. What knowledge do you seek?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("My health is as robust as the principles of mathematics."))));

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My vocation is to explore the intricate web of numbers and patterns, a quest for truth akin to the virtues themselves.");
                jobModule.AddOption("What do you find most fascinating about mathematics?",
                    p => true,
                    p => 
                    {
                        DialogueModule fascinationModule = new DialogueModule("The elegance of mathematical proofs and the beauty of geometric forms never cease to amaze me. They reveal truths that are hidden in plain sight.");
                        fascinationModule.AddOption("Can you share a specific example?",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Consider the Fibonacci sequence: a simple series where each number is the sum of the two preceding ones. It appears in nature, art, and architecture, connecting our world through beauty and logic.")));
                            });
                        p.SendGump(new DialogueGump(p, fascinationModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think about humility?",
            player => true,
            player => 
            {
                DialogueModule humilityModule = new DialogueModule("Consider the virtue of Humility, for in the vast realm of numbers, we find that even the most learned are but humble students.");
                humilityModule.AddOption("How can humility be applied in mathematics?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("In mathematics, humility allows one to accept that not every problem has an immediate solution. It encourages collaboration and learning from others' insights.")));
                    });
                player.SendGump(new DialogueGump(player, humilityModule));
            });

        greeting.AddOption("Tell me about the relationship between virtue and mathematics.",
            player => true,
            player => 
            {
                DialogueModule virtueModule = new DialogueModule("Mathematics teaches us discipline and logic, virtues that extend beyond numbers into our daily lives. It encourages integrity in our reasoning and consistency in our actions.");
                virtueModule.AddOption("Can you explain further?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule deeperModule = new DialogueModule("Absolutely! For instance, the discipline of solving complex equations reflects the need for patience in life. Each step must be carefully considered, much like our choices.");
                        deeperModule.AddOption("That's insightful. How do I apply this in my life?",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("You can start by breaking down larger problems into manageable steps, allowing yourself to navigate life's challenges with clarity and focus.")));
                            });
                        pl.SendGump(new DialogueGump(pl, deeperModule));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("What is the importance of truth in your work?",
            player => true,
            player => 
            {
                DialogueModule truthModule = new DialogueModule("Truth is an elusive quarry, yet the hunt for it is what gives meaning to my research. Sometimes, when a traveler shows keen interest, I offer a reward for solving one of my riddles. Are you interested?");
                truthModule.AddOption("Yes, I would like a riddle.",
                    pl => CanReward(pl),
                    pl => 
                    {
                        if (DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10))
                        {
                            pl.SendMessage("Solve this riddle: What has keys but can't open locks?");
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                        else
                        {
                            pl.SendMessage("I have no reward right now. Please return later.");
                        }
                    });
                truthModule.AddOption("No, thank you.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                player.SendGump(new DialogueGump(player, truthModule));
            });

        greeting.AddOption("What do you think about the stability that mathematics provides?",
            player => true,
            player => 
            {
                DialogueModule stabilityModule = new DialogueModule("With stability comes clarity. In a world full of uncertainties, the constants of mathematics provide an anchor.");
                stabilityModule.AddOption("Can you give an example of stability in mathematics?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Consider Pi, the ratio of a circle's circumference to its diameter. It's a constant that holds true regardless of the circle's size, providing a sense of order in geometric chaos.")));
                    });
                player.SendGump(new DialogueGump(player, stabilityModule));
            });

        greeting.AddOption("Tell me about your inspirations.",
            player => true,
            player => 
            {
                DialogueModule inspirationModule = new DialogueModule("I draw inspiration from many great minds of the past, such as Euclid and Archimedes. Their works laid the foundation for modern mathematics.");
                inspirationModule.AddOption("Which of their ideas resonates with you the most?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Euclid's 'Elements' is particularly inspiring for its systematic approach to geometry. It illustrates how logical reasoning can lead to profound discoveries.")));
                    });
                player.SendGump(new DialogueGump(player, inspirationModule));
            });

        greeting.AddOption("Do you have any riddles for me?",
            player => true,
            player => 
            {
                DialogueModule riddleModule = new DialogueModule("I love riddles! They challenge the mind and sharpen our reasoning skills. Here’s one: I speak without a mouth and hear without ears. I have no body, but I come alive with the wind. What am I?");
                riddleModule.AddOption("I give up.",
                    pl => true,
                    pl => pl.SendGump(new DialogueGump(pl, new DialogueModule("The answer is 'an echo'. It’s a fun reminder of how sound travels and reflects in our world!"))));
                riddleModule.AddOption("I think I know! An echo!",
                    pl => true,
                    pl => 
                    {
                        pl.SendMessage("Correct! Well done, traveler.");
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    });
                player.SendGump(new DialogueGump(player, riddleModule));
            });

        return greeting;
    }

    private bool CanReward(PlayerMobile player)
    {
        return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
    }

    public HypatiaTheMathematician(Serial serial) : base(serial) { }

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
