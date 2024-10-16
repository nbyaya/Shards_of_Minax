using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CharmingCharles : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CharmingCharles() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Charming Charles";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(70);
        SetInt(50);

        SetHits(75);
        SetMana(50);
        SetStam(70);

        // Appearance
        AddItem(new Tunic() { Hue = 1157 });
        AddItem(new Kilt() { Hue = 1150 });
        AddItem(new Sandals() { Hue = 38 });
        AddItem(new GoldRing() { Name = "Charles's Ring" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CharmingCharles(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Charming Charles, a courtesan. How may I charm you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am a courtesan by trade, dedicated to the arts of seduction and companionship. Some might call it a simple profession, but there is beauty in understanding the human heart.");
                aboutModule.AddOption("What do you mean by seduction?",
                    p => true,
                    p =>
                    {
                        DialogueModule seductionModule = new DialogueModule("Seduction isn't just physical allure; it's about understanding, listening, and truly connecting with someone's soul. Have you ever felt a deep connection with someone?");
                        seductionModule.AddOption("Yes, I have.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule deepConnectionModule = new DialogueModule("A deep connection is indeed a rare and beautiful experience. I once had two customers at the same time, and let me tell you, it was quite the balancing act.");
                                deepConnectionModule.AddOption("Two customers at the same time? Tell me more.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule storyModule = new DialogueModule("Ah, yes. It was a peculiar evening. One was a noblewoman, the other a merchant. Both sought my company, unaware of the other's presence at first. I had to use every bit of my charm and wit to keep them both entertained without raising suspicion.");
                                        storyModule.AddOption("How did you manage that?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule manageModule = new DialogueModule("I had to create different atmospheres for each of them. For the noblewoman, I spoke of poetry, art, and the subtleties of courtly love. For the merchant, I spoke of commerce, adventure, and the excitement of trading exotic goods. Switching topics seamlessly was key.");
                                                manageModule.AddOption("Did they ever find out?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule foundOutModule = new DialogueModule("Oh, they did, eventually. There was a moment when the noblewoman heard me mention spices from the East to the merchant. She grew suspicious, and soon they both realized they weren't alone.");
                                                        foundOutModule.AddOption("What happened then?",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                DialogueModule aftermathModule = new DialogueModule("Thankfully, I managed to turn the situation into a jest. I told them that I was merely trying to create a unique experience, where two individuals could meet someone completely different in the same room. They both laughed, though I could tell they weren't entirely pleased.");
                                                                aftermathModule.AddOption("That sounds tense.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendMessage("It was indeed. But in the end, they both appreciated the honesty—or at least, the humor I attempted to bring to the situation.");
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                aftermathModule.AddOption("You must be very skilled to pull that off.",
                                                                    ple => true,
                                                                    ple =>
                                                                    {
                                                                        ple.SendMessage("Thank you, dear traveler. It was certainly one of the more challenging evenings of my career.");
                                                                        ple.SendGump(new DialogueGump(ple, CreateGreetingModule()));
                                                                    });
                                                                pld.SendGump(new DialogueGump(pld, aftermathModule));
                                                            });
                                                        foundOutModule.AddOption("I would have been furious.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendMessage("Ah, and rightly so. But charm often lies in defusing tense situations, even when one's back is against the wall.");
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, foundOutModule));
                                                    });
                                                manageModule.AddOption("That sounds exhausting.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("It was, indeed. But such is the life of a courtesan—constantly balancing emotions and expectations.");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, manageModule));
                                            });
                                        storyModule.AddOption("That sounds risky.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendMessage("It was quite the gamble, but sometimes risk is part of the charm.");
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, storyModule));
                                    });
                                deepConnectionModule.AddOption("That must have been difficult.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("It was, but I learned a lot about managing expectations and reading people's emotions that night.");
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, deepConnectionModule));
                            });
                        seductionModule.AddOption("No, I haven't.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Ah, then you have much to discover. Perhaps someday you will.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, seductionModule));
                    });
                aboutModule.AddOption("Why do they call you charming?",
                    p => true,
                    p =>
                    {
                        DialogueModule charmingModule = new DialogueModule("Ah, my reputation precedes me! The people of the town named me so because of my ability to win hearts with just a conversation.");
                        charmingModule.AddOption("Quite an impressive skill.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Thank you, my dear traveler.");
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, charmingModule));
                    });
                aboutModule.AddOption("That is all I need to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have any advice on virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("True charm lies not in beauty alone, but in kindness and compassion. Art thou compassionate?");
                virtuesModule.AddOption("Yes, I believe I am.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Your words resonate with wisdom. Remember, kindness begets kindness.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                virtuesModule.AddOption("I'm not sure.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Doubt is only natural. Just remember that true compassion requires effort and sincerity.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Do you have anything for me?",
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
                    DialogueModule rewardModule = new DialogueModule("A deep connection with someone can be the most rewarding experience in life. As a token of appreciation for our heartfelt chat, here's a small gift for you.");
                    rewardModule.AddOption("Thank you for the gift.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendMessage("Charming Charles hands you a small scroll.");
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("Goodbye, Charles.",
            player => true,
            player =>
            {
                player.SendMessage("Charming Charles smiles warmly at you.");
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