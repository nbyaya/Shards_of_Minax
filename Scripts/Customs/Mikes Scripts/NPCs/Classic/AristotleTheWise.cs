using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class AristotleTheWise : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public AristotleTheWise() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Aristotle the Wise";
        Body = 0x190; // Human male body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(60);
        
        // Appearance
        AddItem(new Robe(2211));
        AddItem(new Sandals(1175));
        AddItem(new QuarterStaff() { Name = "Aristotle's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public AristotleTheWise(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Aristotle the Wise, a seeker of knowledge. How may I assist you today?");

        greeting.AddOption("Tell me about your pursuit of wisdom.",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("My vocation, if you can call it that, is the pursuit of wisdom and the contemplation of life's mysteries. True wisdom comes from understanding oneself and the world around.");
                wisdomModule.AddOption("Do you need help with anything?",
                    p => true,
                    p =>
                    {
                        DialogueModule helpModule = new DialogueModule("There is a special tome I've been searching for, called 'The Chronicles of the Ancients'. If you bring it to me, I will reward you for your efforts. Would you be willing to help?");
                        helpModule.AddOption("I will help you find the tome.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You have agreed to help Aristotle find 'The Chronicles of the Ancients'.");
                            });
                        helpModule.AddOption("I'm not interested right now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, helpModule));
                    });
                wisdomModule.AddOption("Why do you pursue wisdom?",
                    p => true,
                    p =>
                    {
                        DialogueModule whyWisdomModule = new DialogueModule("Wisdom, traveler, is the foundation of a fulfilling life. It is the compass that guides us through the complexities of existence. Without wisdom, one is like a ship lost at sea, tossed about by the waves of fate.");
                        whyWisdomModule.AddOption("Is wisdom attainable for everyone?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule attainWisdomModule = new DialogueModule("Wisdom is not something that can be handed to you. It is earned through experience, contemplation, and the courage to face one's own ignorance. Every person has the potential, but the path is different for each of us.");
                                attainWisdomModule.AddOption("How does one begin the journey to wisdom?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule journeyWisdomModule = new DialogueModule("The journey begins with curiosity. Question everything, especially your own assumptions. Seek out the unfamiliar, and be willing to listen deeply to those whose views differ from your own. True wisdom comes when you can look beyond yourself.");
                                        journeyWisdomModule.AddOption("I see. Thank you for sharing.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, journeyWisdomModule));
                                    });
                                attainWisdomModule.AddOption("I have much to learn, it seems.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, attainWisdomModule));
                            });
                        whyWisdomModule.AddOption("Why is wisdom important in daily life?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule importanceWisdomModule = new DialogueModule("Wisdom is the bridge between knowledge and action. It allows us to make decisions that are not only beneficial to ourselves but to those around us. It gives us perspective and helps us navigate moral dilemmas with grace.");
                                importanceWisdomModule.AddOption("That makes a lot of sense.",
                                    plc => true,
                                    plc =>
                                    {
                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, importanceWisdomModule));
                            });
                        player.SendGump(new DialogueGump(player, whyWisdomModule));
                    });
                wisdomModule.AddOption("That sounds interesting.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Can you tell me about the virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("Do you ponder the eight virtues, traveler? Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility? They shape the path of every true adventurer.");
                virtuesModule.AddOption("Tell me more about Compassion.",
                    p => true,
                    p =>
                    {
                        DialogueModule compassionModule = new DialogueModule("Compassion is what binds us together. In showing kindness to even the smallest creature, we elevate our own spirit. Do you show compassion in your daily life?");
                        compassionModule.AddOption("I strive to be compassionate.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule striveCompassionModule = new DialogueModule("That is admirable, traveler. Remember that compassion is not always easy. It requires strength to forgive and to care, even when it is inconvenient.");
                                striveCompassionModule.AddOption("Why is compassion so difficult sometimes?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule difficultyCompassionModule = new DialogueModule("Compassion is difficult because it requires us to let go of our ego. We must see the world from another's perspective and prioritize their well-being over our own. This goes against our survival instincts, but it is what makes us truly human.");
                                        difficultyCompassionModule.AddOption("I understand. I will try to be better.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, difficultyCompassionModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, striveCompassionModule));
                            });
                        compassionModule.AddOption("I could do better.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule doBetterCompassionModule = new DialogueModule("Acknowledging your shortcomings is the first step towards growth. Do not be too harsh on yourself, but strive to make each day an improvement on the last.");
                                doBetterCompassionModule.AddOption("Thank you for your encouragement.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, doBetterCompassionModule));
                            });
                        p.SendGump(new DialogueGump(p, compassionModule));
                    });
                virtuesModule.AddOption("What about Sacrifice?",
                    p => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                    p =>
                    {
                        DialogueModule sacrificeModule = new DialogueModule("Sacrifice involves giving up something for the greater good. I've known many who have made such sacrifices. For those who truly understand its meaning, I have a token of appreciation.");
                        sacrificeModule.AddOption("I understand the meaning of sacrifice.",
                            pl => true,
                            pl =>
                            {
                                pl.AddToBackpack(new MageryAugmentCrystal()); // Give the reward
                                lastRewardTime = DateTime.UtcNow;
                                pl.SendMessage("Aristotle rewards you for your understanding of sacrifice.");
                            });
                        sacrificeModule.AddOption("Tell me more about sacrifice.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule deeperSacrificeModule = new DialogueModule("Sacrifice is not simply about loss, it is about intention. When we sacrifice, we give up something of value to achieve something greater. It is a conscious choice, often driven by love or duty.");
                                deeperSacrificeModule.AddOption("Can you give an example?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule exampleSacrificeModule = new DialogueModule("A parent sacrificing their comfort to provide for their child, or a soldier sacrificing their life to protect their comrades—these are powerful examples of true sacrifice. It is this willingness to give for the greater good that defines our character.");
                                        exampleSacrificeModule.AddOption("That is truly inspiring.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, exampleSacrificeModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, deeperSacrificeModule));
                            });
                        sacrificeModule.AddOption("Perhaps another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, sacrificeModule));
                    });
                virtuesModule.AddOption("Another time, perhaps.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Do you have any advice on meditation?",
            player => true,
            player =>
            {
                DialogueModule meditationModule = new DialogueModule("The mind, when nourished and cared for, can be a beacon of light in the darkest of times. Meditation helps strengthen the mind and bring clarity. Would you like some guidance on how to start?");
                meditationModule.AddOption("Yes, please.",
                    p => true,
                    p =>
                    {
                        DialogueModule guidanceModule = new DialogueModule("Find a quiet place, sit comfortably, and focus on your breathing. Let go of distractions and be present in the moment. With practice, you will find great peace.");
                        guidanceModule.AddOption("How do I deal with distractions?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule distractionsModule = new DialogueModule("Distractions are a natural part of meditation. When they arise, simply acknowledge them without judgment, and gently bring your focus back to your breathing. The key is persistence and patience.");
                                distractionsModule.AddOption("I will keep that in mind. Thank you.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, distractionsModule));
                            });
                        guidanceModule.AddOption("Thank you for the advice.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, guidanceModule));
                    });
                meditationModule.AddOption("What are the benefits of meditation?",
                    p => true,
                    p =>
                    {
                        DialogueModule benefitsModule = new DialogueModule("Meditation brings clarity, reduces stress, and helps us understand ourselves better. It strengthens the mind and allows us to face challenges with equanimity. Over time, it leads to a deep sense of inner peace.");
                        benefitsModule.AddOption("That sounds wonderful.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, benefitsModule));
                    });
                meditationModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, meditationModule));
            });

        greeting.AddOption("Goodbye, Aristotle.",
            player => true,
            player =>
            {
                player.SendMessage("Aristotle nods thoughtfully as you take your leave.");
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