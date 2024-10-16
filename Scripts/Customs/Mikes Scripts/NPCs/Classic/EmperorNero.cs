using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorNero : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorNero() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Nero";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(65);
        SetInt(85);
        SetHits(85);

        // Appearance
        AddItem(new Robe() { Hue = 1157 });
        AddItem(new GoldRing() { Name = "Emperor Nero's Ring" });
        AddItem(new Boots() { Hue = 1175 });
        AddItem(new Dagger() { Name = "Emperor's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public EmperorNero(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Emperor Nero, ruler of these lands. How may I assist you?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Emperor Nero, ruler of these lands. It is my duty to govern and lead my people with wisdom and justice.");
                aboutModule.AddOption("Tell me about your duties.",
                    p => true,
                    p =>
                    {
                        DialogueModule dutiesModule = new DialogueModule("Governing requires listening to my people, making decisions that benefit the majority, and ensuring that law and order are maintained. It is a heavy but noble task.");
                        dutiesModule.AddOption("How do you ensure fairness in governance?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule fairnessModule = new DialogueModule("Fairness is ensured by listening to the voices of all, even the common folk. I strive to understand the needs and challenges of my subjects, balancing their well-being with the stability of the empire.");
                                fairnessModule.AddOption("Have you faced challenges in doing so?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule challengeModule = new DialogueModule("Indeed, there are many challenges. Not all agree with my methods, and sometimes difficult decisions must be made. Balancing the welfare of the people with the demands of the nobility is a constant struggle.");
                                        challengeModule.AddOption("Tell me more about these challenges.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule moreChallengesModule = new DialogueModule("One significant challenge was dealing with the Great Fire of Rome. Many accused me of starting it, but the truth is far more complex. I used it as an opportunity to rebuild Rome with grandeur, but the accusations were painful.");
                                                moreChallengesModule.AddOption("Did you rebuild Rome after the fire?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule rebuildModule = new DialogueModule("Yes, I spearheaded the reconstruction of Rome. I envisioned a city filled with wide streets, majestic buildings, and beautiful gardens. It was an opportunity to transform Rome into the jewel of the ancient world, although it wasn't without controversy.");
                                                        rebuildModule.AddOption("What was controversial about it?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule controversyModule = new DialogueModule("The controversy stemmed from the perception that I cared more for art and architecture than for the lives lost. Some believed I sought personal glory. However, I saw it as a duty to restore Rome's greatness.");
                                                                controversyModule.AddOption("How do you respond to such criticism?",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        DialogueModule responseModule = new DialogueModule("Criticism is part of leadership. I cannot please everyone, but my intentions were for the glory and stability of Rome. History will judge me, but I acted in what I believed were the best interests of the empire.");
                                                                        responseModule.AddOption("I understand. Leadership is never easy.",
                                                                            plf => true,
                                                                            plf =>
                                                                            {
                                                                                plf.SendGump(new DialogueGump(plf, CreateGreetingModule()));
                                                                            });
                                                                        ple.SendGump(new DialogueGump(ple, responseModule));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, controversyModule));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, rebuildModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, moreChallengesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, challengeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, fairnessModule));
                            });
                        dutiesModule.AddOption("That sounds challenging.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, dutiesModule));
                    });
                aboutModule.AddOption("Tell me about your family.",
                    p => true,
                    p =>
                    {
                        DialogueModule familyModule = new DialogueModule("My family lineage is steeped in power and prestige. My mother, Agrippina, was a formidable woman who played a significant role in securing my place as emperor.");
                        familyModule.AddOption("Tell me more about your mother.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule motherModule = new DialogueModule("Agrippina was ambitious and intelligent, and she wielded influence like a weapon. Some say she was overbearing, but I believe she acted in what she thought was my best interest. Our relationship, however, was complex.");
                                motherModule.AddOption("Why was it complex?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule complexModule = new DialogueModule("My mother's ambitions often clashed with my own. She wanted control over my reign, and as I grew older, I sought independence. Eventually, our relationship deteriorated to a point of no return.");
                                        complexModule.AddOption("Did this affect your rule?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule affectRuleModule = new DialogueModule("It did. The tensions between us created political instability. I had to assert my authority, which led to difficult decisions, including distancing myself from her influence. The consequences were both personal and political.");
                                                affectRuleModule.AddOption("That must have been difficult.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, affectRuleModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, complexModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, motherModule));
                            });
                        familyModule.AddOption("What about your ancestors?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule ancestorsModule = new DialogueModule("My ancestors were illustrious rulers and statesmen. They established traditions and principles that I strive to uphold today. Their legacy is a source of great pride, but also a heavy burden.");
                                ancestorsModule.AddOption("Tell me about their legacy.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule legacyModule = new DialogueModule("The legacy of my ancestors includes expanding Rome's borders, establishing law, and creating the foundation of the empire. They were visionaries, and I feel both privileged and pressured to continue their work.");
                                        legacyModule.AddOption("It must be difficult to live up to such expectations.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, legacyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, ancestorsModule));
                            });
                        familyModule.AddOption("Goodbye.",
                            pq => true,
                            pq =>
                            {
                                p.SendMessage("Emperor Nero nods solemnly.");
                            });
                        p.SendGump(new DialogueGump(p, familyModule));
                    });
                aboutModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Emperor Nero nods solemnly.");
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in peak health, thanks to my royal physicians and the rare herbs they procure for me.");
                healthModule.AddOption("Tell me more about the herbs.",
                    p => true,
                    p =>
                    {
                        DialogueModule herbsModule = new DialogueModule("The herbs not only help in maintaining my health but also have mysterious properties that even our alchemists are trying to uncover.");
                        herbsModule.AddOption("Where do these herbs come from?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule originModule = new DialogueModule("The herbs are gathered from distant lands, often at great risk. Some are found deep in the forests, while others grow in treacherous mountain regions. They are highly valued not only for their healing properties but also for their rarity.");
                                originModule.AddOption("Are they used for anything else?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule useModule = new DialogueModule("Yes, some herbs are used in rituals and ceremonies, while others are ingredients in potent elixirs. The alchemists of my kingdom are constantly experimenting to unlock their full potential.");
                                        useModule.AddOption("That sounds intriguing.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, useModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, originModule));
                            });
                        herbsModule.AddOption("Fascinating.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, herbsModule));
                    });
                healthModule.AddOption("I see.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Tell me about justice in your reign.",
            player => true,
            player =>
            {
                DialogueModule justiceModule = new DialogueModule("The virtue of justice guides my reign. Law is essential in maintaining peace and order. Fair trials ensure that everyone, regardless of standing, gets a fair chance.");
                justiceModule.AddOption("Do you believe in the importance of law?",
                    p => true,
                    p =>
                    {
                        DialogueModule lawModule = new DialogueModule("Indeed, law is the cornerstone of our society. Uphold it, and you shall find favor in my realm.");
                        lawModule.AddOption("How do you handle those who break the law?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule punishmentModule = new DialogueModule("Punishments vary depending on the severity of the crime. Minor offenses are met with fines or public service, while serious crimes can lead to imprisonment or exile. The goal is to maintain order while giving people a chance for redemption.");
                                punishmentModule.AddOption("What about crimes against the state?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule stateCrimesModule = new DialogueModule("Crimes against the state, such as treason, are dealt with harshly. The stability of the empire is paramount, and those who threaten it face severe consequences. However, I always ensure a fair trial.");
                                        stateCrimesModule.AddOption("Do you believe in mercy?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule mercyModule = new DialogueModule("Mercy has its place, especially when the accused shows genuine remorse. I believe in giving people a second chance when possible, but the safety of Rome must always come first.");
                                                mercyModule.AddOption("That is a balanced approach.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, mercyModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, stateCrimesModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, punishmentModule));
                            });
                        lawModule.AddOption("I will uphold the law.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Emperor Nero nods approvingly.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        lawModule.AddOption("I have other matters to attend to.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, lawModule));
                    });
                justiceModule.AddOption("How do you ensure fairness?",
                    p => true,
                    p =>
                    {
                        DialogueModule fairnessModule = new DialogueModule("Fairness is ensured through a council of judges who review each case independently. They are trained to be impartial, and their role is crucial in maintaining the people's trust in our legal system.");
                        fairnessModule.AddOption("Do you trust your judges?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule trustModule = new DialogueModule("I have handpicked the judges for their integrity and wisdom. Trust is essential, but I also keep a watchful eye to ensure they remain fair and just.");
                                trustModule.AddOption("That is wise.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, trustModule));
                            });
                        p.SendGump(new DialogueGump(p, fairnessModule));
                    });
                justiceModule.AddOption("I see.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, justiceModule));
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
                    DialogueModule rewardModule = new DialogueModule("For those who prove their loyalty and dedication to the empire, I ensure they are rewarded generously. You seem to be on a noble quest. Aid me in a matter, and I might bestow upon you a gift from the royal treasury.");
                    rewardModule.AddOption("What kind of aid do you require?",
                        p => true,
                        p =>
                        {
                            DialogueModule aidModule = new DialogueModule("There are rumors of unrest in the northern provinces. I need someone to gather information and ensure that the local leaders remain loyal. This mission is delicate and requires subtlety.");
                            aidModule.AddOption("I will help gather information.",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new HarmAugmentCrystal()); // Give the reward
                                    lastRewardTime = DateTime.UtcNow;
                                    pl.SendMessage("Emperor Nero hands you a Harm Augment Crystal as a token of gratitude.");
                                });
                            aidModule.AddOption("Maybe later.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, aidModule));
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

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Emperor Nero nods solemnly at you.");
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