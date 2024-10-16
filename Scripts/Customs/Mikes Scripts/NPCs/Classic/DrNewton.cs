using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DrNewton : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DrNewton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dr. Newton";
        Body = 0x190; // Human male body

        // Stats
        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(50);

        // Appearance
        AddItem(new LongPants() { Hue = 1104 });
        AddItem(new FancyShirt() { Hue = 1152 });
        AddItem(new Shoes() { Hue = 0 });
        AddItem(new Cap() { Hue = 1104 });
        AddItem(new Spellbook() { Name = "Dr. Newton's Notes" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public DrNewton(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, I am Dr. Newton, the scientist. How may I assist you today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("I am Dr. Newton, a scientist fascinated by the mysteries of the world. I delve into all sorts of research, from ancient artifacts to the wonders of alchemy and mathematics.");
                identityModule.AddOption("Tell me more about your research.",
                    p => true,
                    p =>
                    {
                        DialogueModule researchModule = new DialogueModule("My research is quite diverse, but lately I've been focusing on the intersection between alchemy and mathematics. You see, alchemy is not just about mixing potions—it's also deeply tied to the principles of balance and proportion, which are fundamentally mathematical.");
                        researchModule.AddOption("Alchemy and mathematics? Tell me more!",
                            pl => true,
                            pl =>
                            {
                                DialogueModule alchemyMathModule = new DialogueModule("Ah, yes! In alchemy, ratios and precise measurements are essential to success. One incorrect calculation can lead to disastrous results. I've been developing a mathematical model to predict alchemical reactions, and it's quite fascinating. For example, the Golden Ratio has a peculiar role in stabilizing certain potions.");
                                alchemyMathModule.AddOption("The Golden Ratio? How does that work?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule goldenRatioModule = new DialogueModule("The Golden Ratio, or Phi, appears in many natural phenomena, and I hypothesize that it helps maintain balance in alchemical mixtures. By applying it to the ratios of ingredients, I've managed to create potions that are far more potent and stable. It's like nature itself prefers this harmony.");
                                        goldenRatioModule.AddOption("That's incredible. Can I learn this technique?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule learnModule = new DialogueModule("Learning this technique requires patience and practice. I can guide you, but first, you'll need to gather some rare ingredients—those with a natural affinity for the Golden Ratio. Are you up for the challenge?");
                                                learnModule.AddOption("What ingredients do I need?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule ingredientsModule = new DialogueModule("You'll need Starfire Blossoms, which bloom only under a crescent moon, and Crystal Dew, which must be collected at dawn from the highest peak. These ingredients, when combined in the proper proportions, can demonstrate the power of the Golden Ratio.");
                                                        ingredientsModule.AddOption("I'll gather them.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("You set off on a quest to gather the rare ingredients.");
                                                            });
                                                        ingredientsModule.AddOption("That sounds too difficult for me.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, ingredientsModule));
                                                    });
                                                learnModule.AddOption("I need to think about it.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, learnModule));
                                            });
                                        goldenRatioModule.AddOption("That's too complex for me.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, goldenRatioModule));
                                    });
                                alchemyMathModule.AddOption("What other mathematical principles do you use?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule otherMathModule = new DialogueModule("Another fascinating principle is the Fibonacci sequence. I've found that certain alchemical reactions align perfectly with Fibonacci numbers, especially in the growth and reproduction of ingredients. For instance, Dragon's Breath herb grows new leaves in patterns that follow this sequence, making it ideal for regenerative potions.");
                                        otherMathModule.AddOption("How do you use the Fibonacci sequence in potions?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule fibonacciModule = new DialogueModule("By carefully observing how ingredients grow, I can determine the ideal quantities to use. For regenerative potions, using Fibonacci-based amounts of Dragon's Breath leads to faster healing effects. It's almost as if the universe itself gives us a clue through these numbers.");
                                                fibonacciModule.AddOption("Amazing! I'd love to learn more.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                fibonacciModule.AddOption("Maybe another time.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, fibonacciModule));
                                            });
                                        otherMathModule.AddOption("That's quite interesting, thank you.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, otherMathModule));
                                    });
                                alchemyMathModule.AddOption("Thank you for explaining.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, alchemyMathModule));
                            });
                        researchModule.AddOption("Thank you for sharing.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, researchModule));
                    });
                identityModule.AddOption("Goodbye.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Dr. Newton nods at you.");
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in perfect health, thank you for asking. Though, there was a time I was gravely ill due to an experiment gone awry. But thanks to a rare herb, I made a miraculous recovery.");
                healthModule.AddOption("What happened with the experiment?",
                    p => true,
                    p =>
                    {
                        DialogueModule experimentModule = new DialogueModule("Experiments can be risky, but they pave the way for new discoveries. While some of my tests have been failures, they've taught me invaluable lessons.");
                        experimentModule.AddOption("What kind of lessons?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule lessonModule = new DialogueModule("One of the most important lessons I've learned is to never give up, even when faced with seemingly insurmountable challenges. Persistence is key in science.");
                                lessonModule.AddOption("That's inspiring.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, lessonModule));
                            });
                        experimentModule.AddOption("Thank you for the insight.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, experimentModule));
                    });
                healthModule.AddOption("Glad to hear you're well.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
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
                    DialogueModule rewardModule = new DialogueModule("Your efforts in aiding my research won't go unnoticed. Here, take this as a token of my appreciation. I hope it serves you well.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll());
                            lastRewardTime = DateTime.UtcNow;
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Dr. Newton nods at you.");
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