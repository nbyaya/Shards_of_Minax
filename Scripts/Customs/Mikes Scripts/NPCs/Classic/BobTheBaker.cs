using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class BobTheBaker : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BobTheBaker() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Bob the Baker";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(40);
        SetInt(60);

        SetHits(50);

        // Appearance
        AddItem(new Skirt(1153));
        AddItem(new FancyShirt(1153));
        AddItem(new Boots(1153));
        AddItem(new Cap { Name = "Bob's Chef Hat" });
        AddItem(new Cleaver { Name = "Bob's Baking Knife" });

        Hue = Utility.RandomSkinHue();
        HairItemID = Utility.RandomList(8251, 8252, 8253, 8254);
        HairHue = Utility.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize reward time
    }

    public BobTheBaker(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Bob the Baker. How can I assist thee today?");

        greeting.AddOption("Tell me about your work.",
            player => true,
            player =>
            {
                DialogueModule workModule = new DialogueModule("I am a humble baker, crafting delicious bread and pastries for the town. It's my life's work! My special apple tart is loved by many.");
                workModule.AddOption("What is the secret ingredient in your tart?",
                    p => true,
                    p =>
                    {
                        DialogueModule ingredientModule = new DialogueModule("Ah, a curious soul! My secret ingredient is a rare apple variety from the Whispering Orchard. They're not easy to find, but their flavor is unmatched. Bring me some, and I'll reward you.");
                        ingredientModule.AddOption("Where is the Whispering Orchard?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule orchardModule = new DialogueModule("The Whispering Orchard lies to the east of this town. The trees there are said to whisper secrets, and some even say they're guarded by spirits. Tread cautiously if you go.");
                                orchardModule.AddOption("I will go and find the apples.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You set off in search of the Whispering Orchard.");
                                    });
                                orchardModule.AddOption("That sounds too dangerous.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, orchardModule));
                            });
                        ingredientModule.AddOption("I'll return with the apples.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Bob nods, a hopeful look in his eyes.");
                            });
                        p.SendGump(new DialogueGump(p, ingredientModule));
                    });
                workModule.AddOption("Perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, workModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I'm in fine health, thank thee for asking! Baking keeps me fit, you know.");
                healthModule.AddOption("Glad to hear it!",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Do you believe in the spirits of the Whispering Orchard?",
            player => true,
            player =>
            {
                DialogueModule spiritsModule = new DialogueModule("Legend has it that the spirits of ancient guardians protect the orchard, ensuring only the worthy pick its fruits. Whether real or imagined, I advise caution if you go. Do you believe in such tales?");
                spiritsModule.AddOption("Yes, I believe in the spirits.",
                    p => true,
                    p =>
                    {
                        DialogueModule believeModule = new DialogueModule("Ah, a believer! It's said that those who respect the spirits and listen to the trees' whispers may be blessed. Remember to show reverence if you venture there.");
                        believeModule.AddOption("I will keep that in mind.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, believeModule));
                    });
                spiritsModule.AddOption("No, I am skeptical.",
                    p => true,
                    p =>
                    {
                        DialogueModule skepticModule = new DialogueModule("Skepticism is healthy. But whether real or not, the orchard is still a treacherous place. Be cautious.");
                        skepticModule.AddOption("I will be careful.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, skepticModule));
                    });
                player.SendGump(new DialogueGump(player, spiritsModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
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
                    DialogueModule rewardModule = new DialogueModule("Ah, eager for rewards, I see! If you bring me apples from the Whispering Orchard, I shall bake you a special tart and give you a token of my appreciation. Do we have a deal?");
                    rewardModule.AddOption("Deal! I will bring the apples.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Bob hands you a scroll as a token of appreciation.");
                        });
                    rewardModule.AddOption("Maybe later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Can we discuss the importance of spaying or neutering pets?",
            player => true,
            player =>
            {
                DialogueModule spayNeuterModule = new DialogueModule("Ah, an important topic indeed! Controlling the pet population is crucial. Too many animals are left without homes. Have you ever considered the impact of not spaying or neutering pets?");
                spayNeuterModule.AddOption("Why is it important to control the pet population?",
                    p => true,
                    p =>
                    {
                        DialogueModule controlModule = new DialogueModule("The number of stray animals continues to grow, leading to overcrowded shelters and many unfortunate pets being euthanized. By spaying or neutering pets, we can prevent overpopulation and give each animal a better chance at finding a loving home.");
                        controlModule.AddOption("That makes sense. What else can be done?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule actionsModule = new DialogueModule("Besides spaying and neutering, raising awareness and supporting local shelters are key. Educating people about responsible pet ownership can make a big difference. Have you ever thought about volunteering at a shelter?");
                                actionsModule.AddOption("Yes, I would love to volunteer!",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("That's wonderful! Shelters always need help, whether it's walking dogs, cleaning, or just giving animals some love.");
                                    });
                                actionsModule.AddOption("I don't have time to volunteer.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule noTimeModule = new DialogueModule("That's understandable. Even small actions, like donating supplies or spreading the word, can make a big difference for shelters and pets in need.");
                                        noTimeModule.AddOption("I will see what I can do.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, noTimeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, actionsModule));
                            });
                        controlModule.AddOption("Is spaying/neutering safe for pets?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule safetyModule = new DialogueModule("Absolutely! The procedure is very common and generally safe. It not only helps control the pet population but also provides health benefits, such as reducing the risk of certain cancers and behavioral issues.");
                                safetyModule.AddOption("That's reassuring to know.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, safetyModule));
                            });
                        p.SendGump(new DialogueGump(p, controlModule));
                    });
                spayNeuterModule.AddOption("What are the benefits of spaying or neutering?",
                    p => true,
                    p =>
                    {
                        DialogueModule benefitsModule = new DialogueModule("Spaying or neutering can greatly benefit pets. It helps reduce aggressive behaviors, prevents unwanted litters, and even improves overall health by reducing the risk of certain diseases.");
                        benefitsModule.AddOption("How does it affect their behavior?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule behaviorModule = new DialogueModule("Neutered pets are generally calmer and less likely to roam or mark territory. It helps them focus their energy on companionship rather than mating instincts.");
                                behaviorModule.AddOption("That's interesting.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, behaviorModule));
                            });
                        benefitsModule.AddOption("Are there any risks involved?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule riskModule = new DialogueModule("Like any surgery, there are minimal risks, but they are rare. Most pets recover quickly and without complications. It's always best to consult with a veterinarian to understand what's right for your pet.");
                                riskModule.AddOption("Thank you for the information.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, riskModule));
                            });
                        p.SendGump(new DialogueGump(p, benefitsModule));
                    });
                spayNeuterModule.AddOption("I hadn't thought much about it.",
                    p => true,
                    p =>
                    {
                        DialogueModule awarenessModule = new DialogueModule("That's alright! It's never too late to learn more. The more we understand, the better choices we can make for our pets and community.");
                        awarenessModule.AddOption("I'll keep it in mind.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, awarenessModule));
                    });
                player.SendGump(new DialogueGump(player, spayNeuterModule));
            });

        greeting.AddOption("Goodbye, Bob.",
            player => true,
            player =>
            {
                player.SendMessage("Bob waves at you warmly.");
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