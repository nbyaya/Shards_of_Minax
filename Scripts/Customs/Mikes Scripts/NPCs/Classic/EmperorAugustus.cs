using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class EmperorAugustus : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public EmperorAugustus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Emperor Augustus";
        Body = 0x190; // Human male body

        // Stats
        SetStr(130);
        SetDex(70);
        SetInt(90);

        SetHits(90);

        // Appearance
        AddItem(new Robe(1303)); // Robe with hue 1303
        AddItem(new GoldRing()); // Custom item, you need to create or add it
        AddItem(new Boots(1175)); // Boots with hue 1175

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public EmperorAugustus(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Emperor Augustus, ruler of this land. How may I assist you?");

        greeting.AddOption("Tell me about your role as Emperor.",
            player => true,
            player =>
            {
                DialogueModule roleModule = new DialogueModule("My job as Emperor is to ensure the prosperity and safety of my people. The crown I wear is but a symbol; true power lies in wisdom and in serving the people.");
                roleModule.AddOption("What makes a good ruler?",
                    p => true,
                    p =>
                    {
                        DialogueModule rulerModule = new DialogueModule("A good ruler is one who uses power for the betterment of the people, not for personal gain. Wisdom, justice, and compassion are key qualities. Would you agree?");
                        rulerModule.AddOption("I agree, wisdom is vital.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Emperor Augustus nods approvingly.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        rulerModule.AddOption("It's hard to balance power and compassion.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Emperor Augustus sighs. 'Indeed, that is the eternal struggle of leadership.'");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        rulerModule.AddOption("Can you tell me about your long years of rule?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule longRuleModule = new DialogueModule("Ah, the long years of being an emperor... It has been both a blessing and a burden. Uniting Rome's lands was not an easy feat, but it was necessary to bring peace and stability. For centuries, our lands have known prosperity because of the sacrifices made. Would you like to hear more about the challenges I faced?");
                                longRuleModule.AddOption("Yes, tell me about the challenges.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule challengesModule = new DialogueModule("The challenges were numerous. Rebellions, betrayals, and threats from neighboring lands. The hardest part was not the battles fought with sword and shield, but the battles fought in the hearts of men. Convincing the people that unity was worth the struggle was perhaps the greatest challenge. Would you like to hear about a specific rebellion?");
                                        challengesModule.AddOption("Tell me about a rebellion you faced.",
                                            plr => true,
                                            plr =>
                                            {
                                                DialogueModule rebellionModule = new DialogueModule("There was a time when the eastern provinces rose against the empire. They felt neglected, their voices unheard. I traveled personally to meet with their leaders, not with an army, but with an open heart. It was there that I realized true leadership is not about forcing loyalty, but earning it. Through dialogue and compromise, we avoided bloodshed. It was one of my proudest moments.");
                                                rebellionModule.AddOption("That sounds inspiring.",
                                                    plrr => true,
                                                    plrr =>
                                                    {
                                                        plrr.SendMessage("Emperor Augustus smiles. 'It was a lesson that stayed with me always. Words can achieve what swords cannot.'");
                                                        plrr.SendGump(new DialogueGump(plrr, CreateGreetingModule()));
                                                    });
                                                rebellionModule.AddOption("What happened after the rebellion was resolved?",
                                                    plrr => true,
                                                    plrr =>
                                                    {
                                                        DialogueModule aftermathModule = new DialogueModule("After the rebellion was resolved, the eastern provinces flourished. Trust was rebuilt, and the people prospered. It was a testament to the power of understanding and the value of every citizen. But it also taught me that vigilance is always required, for peace is fragile and must be nurtured.");
                                                        aftermathModule.AddOption("How did you maintain peace afterward?",
                                                            plrrr => true,
                                                            plrrr =>
                                                            {
                                                                DialogueModule peaceModule = new DialogueModule("Maintaining peace required constant effort. I established councils to ensure every province had a voice. Representatives from every corner of the empire met regularly to discuss their concerns. It wasn't perfect, but it was a step towards unity. I also traveled extensively, showing the people that their emperor was not distant, but among them. Would you like to know more about these travels?");
                                                                peaceModule.AddOption("Tell me about your travels.",
                                                                    plrrrr => true,
                                                                    plrrrr =>
                                                                    {
                                                                        DialogueModule travelsModule = new DialogueModule("My travels took me to every part of the empire. From the bustling markets of Alexandria to the serene hills of Gaul. Each region had its own beauty, its own challenges. Meeting the people, hearing their stories, understanding their struggles—it made me a better ruler. It showed me that the empire was not just land and gold, but the people who called it home.");
                                                                        travelsModule.AddOption("You must have seen a lot of change.",
                                                                            plrrrrr => true,
                                                                            plrrrrr =>
                                                                            {
                                                                                plrrrrr.SendMessage("Emperor Augustus nods. 'Indeed, I have seen much change. Empires rise and fall, but it is the spirit of the people that endures.'");
                                                                                plrrrrr.SendGump(new DialogueGump(plrrrrr, CreateGreetingModule()));
                                                                            });
                                                                        travelsModule.AddOption("What was your favorite place to visit?",
                                                                            plrrrrr => true,
                                                                            plrrrrr =>
                                                                            {
                                                                                DialogueModule favoritePlaceModule = new DialogueModule("Ah, that is a difficult question. But if I must choose, it would be the gardens of Hispania. The tranquility there, the scent of blossoms in the air, it was a place where I could reflect and find peace amidst the chaos of rule. It reminded me of what I was fighting for—a future where all could find such peace.");
                                                                                favoritePlaceModule.AddOption("That sounds beautiful.",
                                                                                    plrrrrrr => true,
                                                                                    plrrrrrr =>
                                                                                    {
                                                                                        plrrrrrr.SendMessage("Emperor Augustus smiles wistfully. 'It was. And it still is, thanks to the peace we fought to achieve.'");
                                                                                        plrrrrrr.SendGump(new DialogueGump(plrrrrrr, CreateGreetingModule()));
                                                                                    });
                                                                                favoritePlaceModule.AddOption("Thank you for sharing your memories.",
                                                                                    plrrrrrr => true,
                                                                                    plrrrrrr =>
                                                                                    {
                                                                                        plrrrrrr.SendGump(new DialogueGump(plrrrrrr, CreateGreetingModule()));
                                                                                    });
                                                                                plrrrrr.SendGump(new DialogueGump(plrrrrr, favoritePlaceModule));
                                                                            });
                                                                        plrrrr.SendGump(new DialogueGump(plrrrr, travelsModule));
                                                                    });
                                                                peaceModule.AddOption("Thank you for sharing your wisdom.",
                                                                    plrrrq => true,
                                                                    plrrrq =>
                                                                    {
                                                                        plrrr.SendGump(new DialogueGump(plrrr, CreateGreetingModule()));
                                                                    });
                                                                plrrr.SendGump(new DialogueGump(plrrr, peaceModule));
                                                            });
                                                        aftermathModule.AddOption("Thank you for the story.",
                                                            plrrr => true,
                                                            plrrr =>
                                                            {
                                                                plrrr.SendGump(new DialogueGump(plrrr, CreateGreetingModule()));
                                                            });
                                                        plrr.SendGump(new DialogueGump(plrr, aftermathModule));
                                                    });
                                                plr.SendGump(new DialogueGump(plr, rebellionModule));
                                            });
                                        challengesModule.AddOption("Thank you for sharing your experiences.",
                                            plr => true,
                                            plr =>
                                            {
                                                plr.SendGump(new DialogueGump(plr, CreateGreetingModule()));
                                            });
                                        pll.SendGump(new DialogueGump(pll, challengesModule));
                                    });
                                longRuleModule.AddOption("It must have been a heavy burden.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendMessage("Emperor Augustus nods solemnly. 'Indeed, the weight of an empire is not easily borne. But I carried it for the people, and that made it worthwhile.'");
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, longRuleModule));
                            });
                        p.SendGump(new DialogueGump(p, rulerModule));
                    });
                roleModule.AddOption("Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, roleModule));
            });

        greeting.AddOption("How is the prosperity of the land?",
            player => true,
            player =>
            {
                DialogueModule prosperityModule = new DialogueModule("The prosperity of this land is my utmost priority. Gold flows, markets bustle, but true prosperity lies in the happiness of its people. Would you agree?");
                prosperityModule.AddOption("Yes, the people's well-being matters most.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Emperor Augustus smiles warmly at your response.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                prosperityModule.AddOption("Wealth is also important.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Emperor Augustus nods thoughtfully. 'Wealth can aid happiness, but it is not the only measure.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, prosperityModule));
            });

        greeting.AddOption("Do you have any wisdom to share?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Power is not measured by the crown one wears, but by the wisdom one possesses. Use your gifts to better the world.");
                wisdomModule.AddOption("I strive to be wise.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Emperor Augustus smiles. 'That is good to hear.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                wisdomModule.AddOption("I have much to learn.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Emperor Augustus nods. 'We all do, my friend. We all do.'");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Can I receive a reward?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    player.SendMessage("Ah, you seek rewards. As you've shown interest in the matters of this realm, I shall grant you something.");
                    player.AddToBackpack(new MirrorOfKalandra()); // Reward item, you need to create or add it
                    lastRewardTime = DateTime.UtcNow;
                }
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