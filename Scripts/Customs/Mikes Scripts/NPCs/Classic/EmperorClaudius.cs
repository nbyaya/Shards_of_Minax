using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorClaudius : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorClaudius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Claudius";
        Body = 0x190; // Human male body

        // Stats
        SetStr(120);
        SetDex(60);
        SetInt(100);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 1308 }); // Robe with specific hue
        AddItem(new PlateHelm() { Name = "Emperor Claudius' Crown" });
        AddItem(new Mace() { Name = "Emperor Claudius' Scepter" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this); // Assuming this method for hair is available
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public EmperorClaudius(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Emperor Claudius, ruler of this land. What brings you to my presence?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Emperor Claudius, the protector and guide of this realm. My duty is to ensure peace, prosperity, and unity among my people.");
                aboutModule.AddOption("What does prosperity mean to you?",
                    p => true,
                    p =>
                    {
                        DialogueModule prosperityModule = new DialogueModule("Prosperity is not just wealth or power; it is the happiness, security, and well-being of the people. A prosperous realm is one where every citizen thrives.");
                        prosperityModule.AddOption("Tell me more about the people of Rome.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule peopleModule = new DialogueModule("The people of Rome are diverse, ranging from noble patricians to the common plebeians. Each plays a crucial role in the fabric of our society. The strength of Rome lies in its citizens, united in their pursuit of greatness.");
                                peopleModule.AddOption("How do you unite such a diverse population?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule unityModule = new DialogueModule("Unity is forged through shared goals, values, and a sense of purpose. We celebrate our victories together, and face adversities as one. Public works, entertainment, and a fair legal system all contribute to maintaining unity.");
                                        unityModule.AddOption("Tell me about Roman public works.",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule publicWorksModule = new DialogueModule("Our aqueducts bring fresh water to the people, our roads connect the far reaches of the empire, and our temples provide a place for worship. These projects are symbols of Rome's strength and ingenuity, built for the prosperity of all.");
                                                publicWorksModule.AddOption("What is the significance of the aqueducts?",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule aqueductsModule = new DialogueModule("The aqueducts are a marvel of engineering, bringing fresh water from distant sources into the heart of our cities. They are essential for public health, agriculture, and maintaining the quality of life for all citizens. Without them, our cities could not thrive.");
                                                        aqueductsModule.AddOption("Fascinating. Please, continue.",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, aqueductsModule));
                                                    });
                                                publicWorksModule.AddOption("Tell me about the roads.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule roadsModule = new DialogueModule("Roman roads are the lifeline of the empire. They enable swift movement of our legions, facilitate trade, and ensure that communication across vast distances is possible. They are a testament to Roman efficiency and our commitment to connectivity.");
                                                        roadsModule.AddOption("How were they constructed?",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                DialogueModule constructionModule = new DialogueModule("The construction of Roman roads involves several layers: a foundation of stone, a layer of gravel, and finally large paving stones fitted meticulously. This method ensures durability and allows our infrastructure to endure for centuries.");
                                                                constructionModule.AddOption("Remarkable craftsmanship.",
                                                                    pllllll => true,
                                                                    pllllll =>
                                                                    {
                                                                        pllllll.SendGump(new DialogueGump(pllllll, CreateGreetingModule()));
                                                                    });
                                                                plllll.SendGump(new DialogueGump(plllll, constructionModule));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, roadsModule));
                                                    });
                                                publicWorksModule.AddOption("Thank you for the information.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, publicWorksModule));
                                            });
                                        unityModule.AddOption("What role does entertainment play in unity?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule entertainmentModule = new DialogueModule("Entertainment, such as the games at the Colosseum, serves to distract, inspire, and unify the populace. It provides a common experience, a break from the hardships of life, and showcases the might of Rome. It reminds the people of their strength and the glory of the empire.");
                                                entertainmentModule.AddOption("Tell me about the Colosseum games.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule gamesModule = new DialogueModule("The games are a spectacle of bravery and skill. Gladiators face each other and wild beasts, demonstrating courage and endurance. It is both a form of entertainment and a reminder of the values Rome holds dear—strength, honor, and resilience.");
                                                        gamesModule.AddOption("Are the games not cruel?",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                DialogueModule crueltyModule = new DialogueModule("There are those who question the morality of the games, and it is true they are brutal. Yet, they are also a tradition—a way to honor the gods, celebrate victories, and instill a sense of pride and unity among the people. The arena is a place where legends are born.");
                                                                crueltyModule.AddOption("I see. Please, continue.",
                                                                    pllllll => true,
                                                                    pllllll =>
                                                                    {
                                                                        pllllll.SendGump(new DialogueGump(pllllll, CreateGreetingModule()));
                                                                    });
                                                                plllll.SendGump(new DialogueGump(plllll, crueltyModule));
                                                            });
                                                        gamesModule.AddOption("Thank you for sharing.",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, gamesModule));
                                                    });
                                                entertainmentModule.AddOption("Interesting perspective.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, entertainmentModule));
                                            });
                                        unityModule.AddOption("Thank you for your insight.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, unityModule));
                                    });
                                peopleModule.AddOption("Thank you for explaining.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, peopleModule));
                            });
                        prosperityModule.AddOption("Thank you for the explanation.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, prosperityModule));
                    });
                aboutModule.AddOption("Tell me about Rome's history.",
                    p => true,
                    p =>
                    {
                        DialogueModule historyModule = new DialogueModule("Rome's history is vast and glorious. From its founding by Romulus and Remus to the rise of the Republic and the eventual establishment of the Empire, Rome has grown through both triumphs and trials.");
                        historyModule.AddOption("What about the founding of Rome?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule foundingModule = new DialogueModule("Legend has it that Rome was founded by Romulus and Remus, twin brothers raised by a she-wolf. Romulus, after a conflict with his brother, became the first king of Rome. This legend symbolizes the resilience and determination of the Roman people.");
                                foundingModule.AddOption("What happened after Romulus?",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule kingsModule = new DialogueModule("After Romulus, a series of kings ruled Rome, each contributing to its growth. However, tyranny under some of these rulers led to the rise of the Republic—a system where power was shared among elected officials.");
                                        kingsModule.AddOption("How did the Republic function?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule republicModule = new DialogueModule("The Republic was founded on principles of shared governance. Senators, consuls, and tribunes represented the interests of various classes, preventing any one individual from seizing absolute power. It was an era of political maneuvering, alliances, and sometimes conflict.");
                                                republicModule.AddOption("What led to the end of the Republic?",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        DialogueModule endOfRepublicModule = new DialogueModule("The Republic ended due to internal strife, power struggles, and the ambition of influential generals like Julius Caesar. Caesar's crossing of the Rubicon marked the beginning of the end. Eventually, Augustus established the Empire, bringing stability after years of civil war.");
                                                        endOfRepublicModule.AddOption("Tell me about Augustus.",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                DialogueModule augustusModule = new DialogueModule("Augustus, the first emperor, was a master of political strategy. He presented himself as a restorer of the Republic, while in reality, he held supreme power. His reign brought about the Pax Romana—a long period of relative peace and prosperity.");
                                                                augustusModule.AddOption("What was the Pax Romana?",
                                                                    pllllll => true,
                                                                    pllllll =>
                                                                    {
                                                                        DialogueModule paxRomanaModule = new DialogueModule("The Pax Romana, or 'Roman Peace', was a period of stability and minimal expansion, lasting over two centuries. It was marked by economic prosperity, cultural flourishing, and the strengthening of Roman infrastructure and law.");
                                                                        paxRomanaModule.AddOption("A remarkable achievement.",
                                                                            plllllll => true,
                                                                            plllllll =>
                                                                            {
                                                                                plllllll.SendGump(new DialogueGump(plllllll, CreateGreetingModule()));
                                                                            });
                                                                        pllllll.SendGump(new DialogueGump(pllllll, paxRomanaModule));
                                                                    });
                                                                augustusModule.AddOption("Thank you for explaining.",
                                                                    pllllll => true,
                                                                    pllllll =>
                                                                    {
                                                                        pllllll.SendGump(new DialogueGump(pllllll, CreateGreetingModule()));
                                                                    });
                                                                plllll.SendGump(new DialogueGump(plllll, augustusModule));
                                                            });
                                                        endOfRepublicModule.AddOption("Thank you for the history lesson.",
                                                            plllll => true,
                                                            plllll =>
                                                            {
                                                                plllll.SendGump(new DialogueGump(plllll, CreateGreetingModule()));
                                                            });
                                                        pllll.SendGump(new DialogueGump(pllll, endOfRepublicModule));
                                                    });
                                                republicModule.AddOption("Thank you for the insight.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, republicModule));
                                            });
                                        kingsModule.AddOption("Thank you for explaining.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, kingsModule));
                                    });
                                foundingModule.AddOption("Thank you for sharing the legend.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, foundingModule));
                            });
                        historyModule.AddOption("Thank you for the history.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, historyModule));
                    });
                aboutModule.AddOption("Thank you for your wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is your duty as emperor?",
            player => true,
            player =>
            {
                DialogueModule dutyModule = new DialogueModule("My duty is to maintain the balance of power, protect the people, and ensure that justice prevails in this realm.");
                dutyModule.AddOption("How do you maintain balance?",
                    p => true,
                    p =>
                    {
                        DialogueModule balanceModule = new DialogueModule("True power lies not in wealth or might, but in the hearts of the people. A wise ruler listens and learns from those they serve.");
                        balanceModule.AddOption("Your words are inspiring.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, balanceModule));
                    });
                dutyModule.AddOption("Thank you for your dedication.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, dutyModule));
            });

        greeting.AddOption("Do you have a task for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no tasks for you at this time. Please return later, and perhaps there will be more to discuss.");
                    cooldownModule.AddOption("Understood. I will return later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule taskModule = new DialogueModule("Indeed, there is something you could assist me with. The responsibility of leadership weighs heavy, and those who show dedication to this realm deserve to be rewarded.");
                    taskModule.AddOption("I am ready to help.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Emperor Claudius grants you a token of his appreciation.");
                        });
                    taskModule.AddOption("Maybe another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, taskModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Emperor Claudius nods solemnly as you take your leave.");
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