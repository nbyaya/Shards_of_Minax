using System;
using Server;
using Server.Mobiles;
using Server.Items;

public class RancherLuke : BaseCreature
{
    [Constructable]
    public RancherLuke() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Rancher Luke";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Stats
        SetStr(95);
        SetDex(85);
        SetInt(50);
        SetHits(85);

        // Appearance
        AddItem(new LongPants() { Hue = 1180 });
        AddItem(new FancyShirt() { Hue = 1180 });
        AddItem(new StrawHat() { Hue = 1170 });
        AddItem(new Boots() { Hue = 1157 });
    }

    public RancherLuke(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Howdy, partner! I'm Rancher Luke, keeper of these here cattle. What brings you to my ranch?");

        greeting.AddOption("Tell me about your past.",
            player => true,
            player =>
            {
                DialogueModule pastModule = new DialogueModule("Ah, my past is quite a tale! I was once a Jedi Knight, fighting against the tyranny of the Galactic Empire. It was a time of great peril and adventure.");
                
                pastModule.AddOption("What was it like being a Jedi?",
                    p => true,
                    p =>
                    {
                        DialogueModule jediLifeModule = new DialogueModule("Being a Jedi was a way of life. We were guardians of peace and justice, using the Force to protect those in need. Training was rigorous, but rewarding.");
                        
                        jediLifeModule.AddOption("What about the training?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule trainingModule = new DialogueModule("The training was tough. We learned to harness the Force, wield a lightsaber, and understand the balance between light and dark. It took years to master.");
                                trainingModule.AddOption("What did you learn about the Force?",
                                    pq => true,
                                    pq =>
                                    {
                                        DialogueModule forceModule = new DialogueModule("The Force surrounds us, binds us. It's a source of strength, but it demands respect. Those who misuse it fall to darkness.");
                                        forceModule.AddOption("Did you ever face temptation?",
                                            plw => true,
                                            plw =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateTemptationModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, forceModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, trainingModule));
                            });
                        pastModule.AddOption("What about your battles against the Empire?",
                            pe => true,
                            pe =>
                            {
                                DialogueModule battlesModule = new DialogueModule("Oh, the battles were fierce! The Empire sought to crush any who opposed them. I fought alongside fellow Jedi and brave rebels to restore freedom.");
                                battlesModule.AddOption("What was your greatest battle?",
                                    pl => true,
                                    pl =>
                                    {
                                        DialogueModule greatestBattleModule = new DialogueModule("One of my greatest battles was on the planet Endor. We faced the full might of the Empire's forces in a bid to destroy the second Death Star.");
                                        greatestBattleModule.AddOption("How did you defeat them?",
                                            pr => true,
                                            pr =>
                                            {
                                                DialogueModule defeatModule = new DialogueModule("Through teamwork, strategy, and the power of the Force! We coordinated strikes, taking out key targets. It was a defining moment for the Rebellion.");
                                                defeatModule.AddOption("What happened after the battle?",
                                                    plt => true,
                                                    plt =>
                                                    {
                                                        pl.SendGump(new DialogueGump(pl, CreateAftermathModule()));
                                                    });
                                                p.SendGump(new DialogueGump(p, defeatModule));
                                            });
                                        pl.SendGump(new DialogueGump(pl, greatestBattleModule));
                                    });
                                p.SendGump(new DialogueGump(p, battlesModule));
                            });
                        player.SendGump(new DialogueGump(player, jediLifeModule));
                    });

                pastModule.AddOption("How did you become a rancher?",
                    p => true,
                    p =>
                    {
                        DialogueModule rancherModule = new DialogueModule("After the Empire's defeat, I sought peace and quiet away from the battles. I returned to my family's ranch to start anew, raising cattle and finding solace in nature.");
                        rancherModule.AddOption("Do you miss being a Jedi?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule missJediModule = new DialogueModule("Sometimes I do. The thrill of adventure and the bond with my fellow Jedi is something I cherish. But I have found new joy here among the cattle.");
                                missJediModule.AddOption("Would you ever take up the lightsaber again?",
                                    py => true,
                                    py =>
                                    {
                                        DialogueModule lightsaberModule = new DialogueModule("Perhaps if the galaxy ever needed me again. The Force is still a part of me, and I would respond if called.");
                                        lightsaberModule.AddOption("That’s inspiring, Rancher Luke.",
                                            plu => true,
                                            plu =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, lightsaberModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, missJediModule));
                            });
                        player.SendGump(new DialogueGump(player, rancherModule));
                    });

                player.SendGump(new DialogueGump(player, pastModule));
            });

        greeting.AddOption("Do your cattle have special abilities?",
            player => true,
            player =>
            {
                DialogueModule cattleAbilitiesModule = new DialogueModule("You could say that! I’ve trained my cattle to respond to commands and even sense emotions. It’s like they have a bond with the Force too.");
                cattleAbilitiesModule.AddOption("Can you teach me about this bond?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule bondModule = new DialogueModule("Of course! The connection we share with animals is profound. They can sense our emotions, fears, and hopes. It's essential to treat them with respect and kindness.");
                        bondModule.AddOption("How do you communicate with them?",
                            p => true,
                            p =>
                            {
                                DialogueModule communicationModule = new DialogueModule("I often use gentle gestures and calming words. It's about establishing trust. If they feel safe, they will open up to you.");
                                p.SendGump(new DialogueGump(p, communicationModule));
                            });
                        pl.SendGump(new DialogueGump(pl, bondModule));
                    });
                player.SendGump(new DialogueGump(player, cattleAbilitiesModule));
            });

        greeting.AddOption("What about the Galactic Empire?",
            player => true,
            player =>
            {
                DialogueModule empireModule = new DialogueModule("The Empire was a force of oppression, spreading fear across the galaxy. We fought valiantly to restore freedom and hope to countless worlds.");
                empireModule.AddOption("How did you get involved in the Rebellion?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule rebellionModule = new DialogueModule("I couldn't stand by and watch the tyranny unfold. I joined the Rebellion to fight for the freedom of all beings. Together, we made a difference.");
                        rebellionModule.AddOption("What’s your advice for fighting oppression?",
                            p => true,
                            p =>
                            {
                                DialogueModule adviceModule = new DialogueModule("Stand firm in your beliefs, rally like-minded individuals, and never lose hope. Even the smallest action can lead to great change.");
                                p.SendGump(new DialogueGump(p, adviceModule));
                            });
                        pl.SendGump(new DialogueGump(pl, rebellionModule));
                    });
                player.SendGump(new DialogueGump(player, empireModule));
            });

        return greeting;
    }

    private DialogueModule CreateTemptationModule()
    {
        DialogueModule temptationModule = new DialogueModule("Temptation is a constant battle. There were moments when the power of the dark side whispered sweet promises. I had to remind myself of the light and my true purpose.");
        temptationModule.AddOption("What kept you grounded?",
            p => true,
            p =>
            {
                DialogueModule groundedModule = new DialogueModule("My friends, the teachings of the Jedi, and my love for the galaxy's inhabitants. The light of hope is a powerful guide.");
                p.SendGump(new DialogueGump(p, groundedModule));
            });
        return temptationModule;
    }

    private DialogueModule CreateAftermathModule()
    {
        DialogueModule aftermathModule = new DialogueModule("After the Empire's defeat, the galaxy slowly began to heal. We established new governments, rebuilt communities, and fostered peace. It was a time of hope.");
        aftermathModule.AddOption("Did you face challenges during this time?",
            pl => true,
            pl =>
            {
                DialogueModule challengesModule = new DialogueModule("Yes, many remnants of the Empire lingered. There were skirmishes and unrest. But we stood together, determined to build a better future.");
                challengesModule.AddOption("How did you contribute?",
                    p => true,
                    p =>
                    {
                        DialogueModule contributionModule = new DialogueModule("I used my skills to train others in the ways of the Force and to lead peacekeeping missions. Every effort counted in those uncertain times.");
                        p.SendGump(new DialogueGump(p, contributionModule));
                    });
                pl.SendGump(new DialogueGump(pl, challengesModule));
            });
        return aftermathModule;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
