using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorMarcus : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorMarcus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Marcus";
        Body = 0x190; // Human male body

        // Stats
        SetStr(135);
        SetDex(75);
        SetInt(80);

        SetHits(95);
        Fame = 0;
        Karma = 0;
        VirtualArmor = 25;

        // Appearance
        AddItem(new ChainLegs() { Hue = 1428 });
        AddItem(new ChainChest() { Hue = 1428 });
        AddItem(new ChainCoif() { Hue = 1428 });
        AddItem(new PlateGloves() { Hue = 1428 });
        AddItem(new Longsword() { Name = "Emperor Marcus' Sword" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public EmperorMarcus(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Emperor Marcus glares at you with disdain. 'What business do you have with the ruler of this wretched land?'");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job? I rule this wretched land, if you must know. True power lies in manipulation, not valor. Let me tell you about my experiences as a ruler.");
                jobModule.AddOption("How do you view power?",
                    p => true,
                    p =>
                    {
                        DialogueModule powerModule = new DialogueModule("Power, you see, is not simply about brute strength or military might. It is about controlling the hearts and minds of people. During my reign, I have learned that true power lies in influence and the ability to sway others to your side. Even the Roman Senate, with all its pomp and tradition, could be manipulated when approached with the right strategies.");
                        powerModule.AddOption("Did the Senate challenge you often?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule senateModule = new DialogueModule("Ah, the Senate. They were often an obstacle, full of pride and stubbornness. Many Senators believed themselves equal to the Emperor. But I used patience and philosophy to win them over. I would remind them of the Stoic ideals—the virtue of the common good, and the fleeting nature of material wealth. Eventually, they would align with my vision, if only to save face before the public.");
                                senateModule.AddOption("Tell me more about your philosophy.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule philosophyModule = new DialogueModule("Philosophy has been my guiding light. As a Stoic, I believe in accepting the world as it is, in focusing on what is within my control, and letting go of what is not. During the campaigns against the Germanic tribes, I often found solace in meditation, reflecting on the nature of duty, fate, and the transient nature of power. It kept me grounded, even as chaos raged around me.");
                                        philosophyModule.AddOption("How did Stoicism help during battles?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule battleModule = new DialogueModule("In battle, Stoicism taught me to remain calm, to make decisions based on reason rather than fear or anger. The Germanic tribes were fierce, and many of my men were terrified. I had to be their rock, their anchor. By embodying Stoic principles, I inspired my soldiers to fight with courage, knowing that fear is merely an emotion, and our duty lay in action, regardless of the outcome.");
                                                battleModule.AddOption("Your soldiers must have respected you greatly.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule respectModule = new DialogueModule("Respect is earned, not demanded. I fought alongside my men, shared their hardships, and never asked of them anything I would not do myself. They saw my commitment to Rome and to them. That is why they followed me, even in the harshest conditions. My leadership was not just about orders—it was about shared purpose and mutual trust.");
                                                        respectModule.AddOption("Did you face betrayal despite this?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule betrayalModule = new DialogueModule("Betrayal is an inevitable part of power. Even among those who seem loyal, ambition and fear can sow the seeds of treachery. There were those within my ranks who sought to undermine me, to seize power for themselves. But I learned to forgive, to understand that their actions were driven by weakness, not malice. Mercy, when appropriate, often won back their loyalty, more effectively than any punishment.");
                                                                betrayalModule.AddOption("How do you balance mercy and justice?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule justiceModule = new DialogueModule("Balancing mercy and justice is perhaps the hardest task of any ruler. Too much mercy, and you invite chaos. Too much justice, and you become a tyrant. I have always sought to act for the greater good of Rome. Each decision weighed not for its immediate consequence, but for its long-term impact on stability and prosperity. The Stoics teach that virtue is the highest good, and in my rulings, I have always aimed to uphold virtue above all.");
                                                                        justiceModule.AddOption("You must have made difficult decisions.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                DialogueModule decisionsModule = new DialogueModule("Indeed. There were times when I had to make decisions that haunted me. Sending men to their deaths, quelling rebellions, even executing those who had betrayed Rome. Each choice weighed heavily on my soul, but as Emperor, I could not afford hesitation. Every action had to be for the greater good, even if it cost me personally. That is the burden of leadership.");
                                                                                decisionsModule.AddOption("Thank you for sharing your wisdom, Emperor.",
                                                                                    plg => true,
                                                                                    plg =>
                                                                                    {
                                                                                        plg.SendGump(new DialogueGump(plg, CreateGreetingModule()));
                                                                                    });
                                                                                plf.SendGump(new DialogueGump(plf, decisionsModule));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, justiceModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, betrayalModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, respectModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, battleModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, philosophyModule));
                                    });
                                senateModule.AddOption("I see. Politics must have been challenging.",
                                    plh => true,
                                    plh =>
                                    {
                                        plh.SendGump(new DialogueGump(plh, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, senateModule));
                            });
                        powerModule.AddOption("Thank you for the insight, Emperor.",
                            pli => true,
                            pli =>
                            {
                                pli.SendGump(new DialogueGump(pli, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, powerModule));
                    });
                jobModule.AddOption("I wish you luck ruling this wretched land.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Do you have any remedies?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward for you right now. Return later.");
                    noRewardModule.AddOption("I understand.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    DialogueModule rewardModule = new DialogueModule("My remedies are crafted by the best alchemists in the land. Take this example of their work, and consider yourself fortunate.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new ColdHitAreaCrystal()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Why do you scoff so much?",
            player => true,
            player =>
            {
                DialogueModule scoffModule = new DialogueModule("Though I scoff, the weight of the crown has taken a toll on my well-being. Yet, I have remedies and secret potions to keep me strong.");
                scoffModule.AddOption("Sounds like a heavy burden.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, scoffModule));
            });

        greeting.AddOption("What do you think of your empire?",
            player => true,
            player =>
            {
                DialogueModule empireModule = new DialogueModule("My empire is vast and full of secrets. There are many hidden treasures within its confines. Help me with a task, and I might just share one with you.");
                empireModule.AddOption("Tell me about this task.",
                    p => true,
                    p =>
                    {
                        DialogueModule taskModule = new DialogueModule("There's a rogue sorcerer undermining my rule. Find and stop him, and you will earn my favor and an unspecified reward.");
                        taskModule.AddOption("I'll stop the sorcerer.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You accept the mission to stop the rogue sorcerer.");
                            });
                        taskModule.AddOption("That's too much for me.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, taskModule));
                    });
                empireModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, empireModule));
            });

        greeting.AddOption("Goodbye, Emperor.",
            player => true,
            player =>
            {
                player.SendMessage("Emperor Marcus watches you leave with an indifferent expression.");
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