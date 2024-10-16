using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CheGuevara : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CheGuevara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Che Guevara";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(100);
        SetInt(85);

        SetHits(78);

        // Appearance
        AddItem(new LongPants() { Hue = 1911 });
        AddItem(new Bandana() { Hue = 1157 });
        AddItem(new Tunic() { Hue = 1157 });
        AddItem(new Boots() { Hue = 1103 });
        AddItem(new Cutlass() { Name = "Liberator's Sabre" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CheGuevara(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, comrade. I am Che Guevara, a revolutionary in pursuit of justice and equality. What brings you to me today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I have fought for the rights of the people, striving for unity and equality. I was part of the Cuban Revolution, a movement that aimed to bring justice to the oppressed. The struggle continues.");
                aboutModule.AddOption("What inspired you to fight for the people?",
                    p => true,
                    p =>
                    {
                        DialogueModule inspirationModule = new DialogueModule("The suffering of the people and the tyranny they endured left me no choice. I believe in the power of unity and the will of the people to bring about change. Do you share this belief?");
                        inspirationModule.AddOption("Yes, I believe in equality for all.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Che smiles and nods approvingly. 'Good, the path to virtue lies in compassion and solidarity.'");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        inspirationModule.AddOption("No, I'm not sure it's possible.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Che frowns slightly. 'Change is never easy, but without hope, we have nothing.'");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, inspirationModule));
                    });
                aboutModule.AddOption("I've heard you've spoken to the poor farmers. What did they tell you?",
                    p => true,
                    p =>
                    {
                        DialogueModule farmersModule = new DialogueModule("The farmers of Britannia are suffering under the oppressive rule of Lord British. They live as feudal serfs, bound to the land with no hope of improving their lives. They toil endlessly, but the fruits of their labor are taken from them, leaving them with barely enough to survive.");
                        farmersModule.AddOption("That's terrible. What can be done to help them?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule helpFarmersModule = new DialogueModule("The only way to help them is to organize a collective uprising. The farmers need to understand their power lies in their unity. If they rise together, they can overthrow their oppressors and reclaim the land they work so hard on.");
                                helpFarmersModule.AddOption("How can we start such an uprising?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule uprisingModule = new DialogueModule("First, we need to spread the message of revolution. We must visit each farm, each village, and speak to the people about their rights. They need to know that they deserve more than scraps. Once we have their support, we can plan coordinated actions to disrupt the flow of resources to Lord British's forces.");
                                        uprisingModule.AddOption("What kind of coordinated actions do you mean?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule actionsModule = new DialogueModule("We can begin by organizing strikes, refusing to hand over the harvest. We can sabotage supply lines and prevent the guards from collecting taxes. Every small act of resistance will weaken Lord British's hold, and eventually, we will be strong enough to face them directly.");
                                                actionsModule.AddOption("That sounds dangerous. Will the people be ready for this?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule readinessModule = new DialogueModule("Danger is inevitable in the fight for freedom, but the people are ready. They are tired of living in misery. They want a better future for their children, one where they are not slaves to a distant lord. With the right leadership and a shared vision, they will be ready to take risks.");
                                                        readinessModule.AddOption("I want to help spread the message.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Che nods, his eyes filled with determination. 'Good. Take these pamphlets. Distribute them among the farmers. Speak to them about their rights and their power.'");
                                                                pld.AddToBackpack(new Item(0x14F0) { Name = "Revolutionary Pamphlets" });
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        readinessModule.AddOption("I'm not sure I'm ready for this.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Che gives you a solemn look. 'No one is ever truly ready for revolution, but without action, nothing will change. I hope you find the courage within you.'");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, readinessModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, actionsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, uprisingModule));
                                    });
                                helpFarmersModule.AddOption("I fear Lord British's retaliation.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule fearModule = new DialogueModule("Fear is natural, comrade. Lord British's forces are strong, but they are not invincible. If we stand united, their power will falter. The risk is great, but the reward is a free and just society for all.");
                                        fearModule.AddOption("Perhaps with more people, we can succeed.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Che nods. 'Indeed. The more people who join our cause, the stronger we become. We must convince them that the fight is worth it.'");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        fearModule.AddOption("I need time to think about this.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("Che places a hand on your shoulder. 'Take your time, but remember, every day we wait is another day the people suffer.'");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, fearModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, helpFarmersModule));
                            });
                        farmersModule.AddOption("I have seen their struggles myself.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Che nods grimly. 'Then you understand why this fight is so important. The people need champions who will stand up for them.'");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, farmersModule));
                    });
                aboutModule.AddOption("I wish you luck in your journey.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("What is your goal?",
            player => true,
            player =>
            {
                DialogueModule goalModule = new DialogueModule("My goal is to unite the people against oppression and to promote equality for all. True justice can only be achieved through solidarity. Do you understand the importance of unity?");
                goalModule.AddOption("Yes, unity is crucial.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Che nods. 'In unity, we find strength. Together, we are unstoppable.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                goalModule.AddOption("No, I prefer to stand alone.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Che looks at you thoughtfully. 'Even the strongest individuals need others. Alone, we can do so little.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, goalModule));
            });

        greeting.AddOption("Can I help in your cause?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule cooldownModule = new DialogueModule("I have no task for you right now. Please return later, comrade.");
                    cooldownModule.AddOption("Understood.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, cooldownModule));
                }
                else
                {
                    DialogueModule helpModule = new DialogueModule("Those who believe in the cause are the ones who will carry its spirit forward. For your dedication, I offer you this token of our gratitude.");
                    helpModule.AddOption("Thank you, I will continue to fight for equality.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            p.SendMessage("Che hands you a scroll as a token of appreciation.");
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    helpModule.AddOption("Maybe another time.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, helpModule));
                }
            });

        greeting.AddOption("Goodbye, Che.",
            player => true,
            player =>
            {
                player.SendMessage("Che nods at you solemnly. 'Farewell, comrade. The struggle continues.'");
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