using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class Confucius : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Confucius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Confucius";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(80);
        SetInt(100);
        SetHits(60);

        // Appearance
        AddItem(new Robe() { Hue = 1154 });
        AddItem(new Sandals() { Hue = 1154 });
        AddItem(new GnarledStaff() { Name = "The Analects" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public Confucius(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Confucius. How may I assist you on your journey of wisdom?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule introModule = new DialogueModule("I am but a humble philosopher, seeking the path of wisdom. Do you seek wisdom, young one?");
                introModule.AddOption("Yes, I seek wisdom.",
                    p => true,
                    p =>
                    {
                        DialogueModule wisdomModule = new DialogueModule("Confucius is but a name, for in the end, aren't we all just stardust in the vast expanse of the universe? Reflect on this, and may it guide you.");
                        wisdomModule.AddOption("What is the path to wisdom?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule pathModule = new DialogueModule("The path to wisdom is one of humility, learning, and understanding. It is a journey without end, as each answer reveals more questions. The wise person knows they know nothing, and thus they seek endlessly.");
                                pathModule.AddOption("How can I be humble?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule humilityModule = new DialogueModule("Humility comes from recognizing that you are but one among many. The universe is vast, and each of us is a mere speck. By helping others and placing their needs above your own, you cultivate humility. It is in service that we learn our true place.");
                                        humilityModule.AddOption("Thank you for your words of wisdom.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, humilityModule));
                                    });
                                pathModule.AddOption("What questions should I ask?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule questionsModule = new DialogueModule("Ask questions that lead you to understand yourself and the world around you. Ask about virtue, about kindness, and about the nature of life. The right questions will challenge you, make you grow, and bring you closer to truth.");
                                        questionsModule.AddOption("How do I know if I have found the truth?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule truthModule = new DialogueModule("Truth is not always a destination but a journey. When you feel peace and clarity in your heart, you are close to the truth. Truth is often simple, but understanding it can take a lifetime. Embrace the process, not just the result.");
                                                truthModule.AddOption("Thank you for guiding me.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, truthModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, questionsModule));
                                    });
                                pathModule.AddOption("Thank you for the guidance.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, pathModule));
                            });
                        wisdomModule.AddOption("What do you mean by stardust?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule stardustModule = new DialogueModule("We are all made of the same matter that forms the stars. In a cosmic sense, we are deeply connected to the universe. This connection reminds us that our lives are part of a greater whole, and with that perspective, we find meaning and purpose.");
                                stardustModule.AddOption("How does this help us find purpose?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule purposeModule = new DialogueModule("Understanding our connection to the universe helps us see beyond our individual desires. Our purpose becomes clearer when we align ourselves with something greater—helping others, improving the world, and seeking knowledge.");
                                        purposeModule.AddOption("Thank you for the enlightenment.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, purposeModule));
                                    });
                                stardustModule.AddOption("Thank you for your insights.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, stardustModule));
                            });
                        wisdomModule.AddOption("Thank you for the wisdom.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, wisdomModule));
                    });
                introModule.AddOption("No, I do not seek wisdom.",
                    p => true,
                    p =>
                    {
                        DialogueModule noWisdomModule = new DialogueModule("Then, young one, ponder the riddle of existence and return when you seek my guidance.");
                        noWisdomModule.AddOption("I will return when I am ready.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, noWisdomModule));
                    });
                player.SendGump(new DialogueGump(player, introModule));
            });

        greeting.AddOption("Can you provide guidance?",
            player => true,
            player =>
            {
                DialogueModule guidanceModule = new DialogueModule("True guidance often comes from within, but sometimes, one requires a nudge in the right direction. Would you like a clue to the riddles of existence?");
                guidanceModule.AddOption("Yes, please give me a clue.",
                    p => true,
                    p =>
                    {
                        DialogueModule clueModule = new DialogueModule("Very well. 'The greatest treasures are those invisible to the eye but felt by the heart.' Reflect upon this, and should you grasp its meaning, I may have a reward for you.");
                        clueModule.AddOption("What does it mean to feel treasures in the heart?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule treasuresModule = new DialogueModule("The treasures of the heart are virtues like love, kindness, and compassion. These are the things that bring true fulfillment, far beyond material wealth. To nurture these treasures is to live a life of meaning.");
                                treasuresModule.AddOption("How can I nurture these treasures?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule nurtureModule = new DialogueModule("Nurturing these treasures begins with your actions. Be kind, be just, and be patient. Help those in need, forgive those who wrong you, and strive to understand others. Each act of goodness nurtures the treasures within you.");
                                        nurtureModule.AddOption("Thank you for showing me the way.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, nurtureModule));
                                    });
                                treasuresModule.AddOption("Thank you for your wisdom.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, treasuresModule));
                            });
                        clueModule.AddOption("Thank you for the clue.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, clueModule));
                    });
                guidanceModule.AddOption("No, I will seek my own path.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, guidanceModule));
            });

        greeting.AddOption("I have pondered the treasures you spoke of.",
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
                    DialogueModule rewardModule = new DialogueModule("Ah, you have pondered deeply! As promised, here is a reward for your insightful journey. May it serve you well.");
                    rewardModule.AddOption("Thank you for the reward.",
                        pl => true,
                        pl =>
                        {
                            pl.AddToBackpack(new DispelAugmentCrystal()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Confucius nods sagely as you depart.");
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