using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using System.Collections.Generic;
using Server.Items;

public class BeastmasterBella : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public BeastmasterBella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Beastmaster Bella";
        Body = 0x191; // Human female body

        // Stats
        SetStr(93);
        SetDex(70);
        SetInt(85);
        SetHits(93);

        // Appearance
        AddItem(new ShortPants(1143));
        AddItem(new FancyShirt(38));
        AddItem(new Boots(1153));
        AddItem(new ShepherdsCrook { Name = "Bella's Beast Stick" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public BeastmasterBella(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Beastmaster Bella, tamer of creatures and guardian of the wild. How can I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am an animal tamer. My job is to care for and train creatures of all kinds. My greatest passion, however, lies in taming bears, particularly the Zodiac Bears. These magnificent creatures embody the mysteries of the cosmos.");
                aboutModule.AddOption("Tell me more about Zodiac Bears.",
                    p => true,
                    p =>
                    {
                        DialogueModule zodiacModule = new DialogueModule("Ah, the Zodiac Bears! They are bears infused with the power of the stars. Each Zodiac Bear represents one of the constellations, and each has unique traits and abilities that correspond to their astrological sign.");
                        zodiacModule.AddOption("What are the different Zodiac Bears?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule typesModule = new DialogueModule("There are twelve Zodiac Bears in total, each named after a zodiac constellation: Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, and Pisces. Each bear has its own temperament and powers derived from their sign.");
                                typesModule.AddOption("Tell me about Aries the Bear.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule ariesModule = new DialogueModule("Aries the Bear is fierce and courageous. It has a fiery personality and is known for its strength and determination. Aries is often seen protecting its territory with unmatched bravery.");
                                        ariesModule.AddOption("How do you tame Aries?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule tameAriesModule = new DialogueModule("To tame Aries, one must show both courage and respect. Aries values strength, but it also respects those who are calm under pressure. Approach it slowly, and never show fear. Offering it a rare Crimson Berry can also help earn its trust.");
                                                tameAriesModule.AddOption("Where can I find Crimson Berries?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule berriesModule = new DialogueModule("Crimson Berries grow deep in the Whispering Woods, usually guarded by fierce creatures. They only bloom under the light of a full moon, making them quite rare.");
                                                        berriesModule.AddOption("I will search for the Crimson Berries.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendMessage("You set off on a quest to find the rare Crimson Berries.");
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        berriesModule.AddOption("That sounds too dangerous for me.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, berriesModule));
                                                    });
                                                tameAriesModule.AddOption("Thank you for the advice.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, tameAriesModule));
                                            });
                                        ariesModule.AddOption("Tell me about another Zodiac Bear.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, typesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, ariesModule));
                                    });
                                typesModule.AddOption("Tell me about Leo the Bear.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule leoModule = new DialogueModule("Leo the Bear is proud and regal. It possesses a golden mane and has a commanding presence. Leo is fiercely protective of its pride and often requires a show of loyalty from those who wish to tame it.");
                                        leoModule.AddOption("How do you tame Leo?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule tameLeoModule = new DialogueModule("To tame Leo, one must demonstrate loyalty and bravery. Leo respects those who put others before themselves. A gift of a Golden Apple, which can be found near the Sunlit Groves, is often a good way to start.");
                                                tameLeoModule.AddOption("Where can I find Golden Apples?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule applesModule = new DialogueModule("Golden Apples grow in the Sunlit Groves, but they are often protected by enchanted creatures. They shine with a bright golden hue, especially at dawn.");
                                                        applesModule.AddOption("I will search for the Golden Apples.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendMessage("You set off on a quest to find the Golden Apples.");
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        applesModule.AddOption("That sounds too risky for me.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, applesModule));
                                                    });
                                                tameLeoModule.AddOption("Thank you for the advice.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, tameLeoModule));
                                            });
                                        leoModule.AddOption("Tell me about another Zodiac Bear.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, typesModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, leoModule));
                                    });
                                typesModule.AddOption("Thank you for sharing about the Zodiac Bears.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, typesModule));
                            });
                        zodiacModule.AddOption("How do you tame a Zodiac Bear?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule tameModule = new DialogueModule("Taming a Zodiac Bear requires patience, respect, and a deep understanding of the stars. Each Zodiac Bear has its own unique preferences and trials that a tamer must pass to earn its trust.");
                                tameModule.AddOption("What kind of trials?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule trialsModule = new DialogueModule("Each Zodiac Bear has a trial related to its nature. For instance, Scorpio's trial might involve overcoming fear, while Virgo's trial might require a demonstration of purity and honesty. The trials are challenging, but they forge a powerful bond.");
                                        trialsModule.AddOption("I see. Thank you for the information.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, trialsModule));
                                    });
                                tameModule.AddOption("Thank you for the insight.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, tameModule));
                            });
                        zodiacModule.AddOption("Thank you for telling me about the Zodiac Bears.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, zodiacModule));
                    });
                aboutModule.AddOption("What creatures have you trained?",
                    p => true,
                    p =>
                    {
                        DialogueModule creatureModule = new DialogueModule("I've trained creatures from the fiercest of dragons to the gentlest of rabbits. Each has a lesson to teach, but none are as fascinating as the Zodiac Bears.");
                        creatureModule.AddOption("Tell me more about the Zodiac Bears.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, creatureModule));
                    });
                aboutModule.AddOption("Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Do you have any wisdom to share?",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("True valor is found in the bond between a tamer and their loyal beasts. Remember, strength alone does not make one valorous. It's the choices we make that define us.");
                wisdomModule.AddOption("What choices have you made?",
                    p => true,
                    p =>
                    {
                        DialogueModule choicesModule = new DialogueModule("One of the most challenging choices I made was to release a rare beast back into the wild after taming it. It taught me much about freedom. Have you faced such dilemmas?");
                        choicesModule.AddOption("Yes, I have faced tough choices.",
                            pl => true,
                            pl =>
                            {
                                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                                if (DateTime.UtcNow - lastRewardTime < cooldown)
                                {
                                    pl.SendMessage("I have no reward right now. Please return later.");
                                }
                                else
                                {
                                    pl.SendMessage("Dilemmas test our character. For making an effort to understand, here's a small reward for you. May it aid you on your journey!");
                                    pl.AddToBackpack(new MaxxiaScroll());
                                    lastRewardTime = DateTime.UtcNow;
                                }
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        choicesModule.AddOption("Not really.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, choicesModule));
                    });
                wisdomModule.AddOption("Thank you for the wisdom.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("Do you have any herbs for sale?",
            player => true,
            player =>
            {
                DialogueModule herbModule = new DialogueModule("Herbs are wondrous plants. They can heal, harm, and even charm animals. I often use lavender to calm agitated creatures. Would you like some?");
                herbModule.AddOption("Yes, please.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Bella gives you some lavender.");
                        p.AddToBackpack(new MaxxiaScroll()); // Assuming Lavender is a defined item
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                herbModule.AddOption("Maybe later.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, herbModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Beastmaster Bella nods at you.");
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