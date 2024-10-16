using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lily the Lost")]
public class LilyTheLost : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LilyTheLost() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lily the Lost";
        Body = 0x191; // Human female body

        // Stats
        SetStr(35);
        SetDex(35);
        SetInt(35);
        SetHits(35);

        // Appearance
        AddItem(new PlainDress() { Hue = 1109 });
        AddItem(new Boots() { Hue = 1109 });
        AddItem(new Bonnet() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public LilyTheLost(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("I am Lily the Lost, a beggar. Life's a constant struggle, but I manage. How may I help you today?");

        greeting.AddOption("Tell me about your past.",
            player => true,
            player =>
            {
                DialogueModule pastModule = new DialogueModule("I was once a florist in the city, filled with roses, lilies, and tulips. The scent was heavenly. Now, I can only dream of those days.");
                pastModule.AddOption("What happened to your shop?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule shopModule = new DialogueModule("I lost everything during the Great Fire. The flames consumed not only my shop but also my hopes. I wandered the streets, unable to find my way back.");
                        shopModule.AddOption("That sounds terrible. Did anyone help you?",
                            p => true,
                            p =>
                            {
                                DialogueModule helpModule = new DialogueModule("Some showed compassion, but many turned away. Kindness seems rare in times of hardship. I learned that trust must be earned.");
                                helpModule.AddOption("How do you earn trust?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule trustModule = new DialogueModule("It takes time and honesty. I often share my story, hoping it resonates with others. In vulnerability, we find connections.");
                                        trustModule.AddOption("That's a wise perspective.",
                                            pw => true,
                                            pw => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                        helpModule.AddOption("I'm sorry to hear that.",
                                            ple => true,
                                            ple => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        player.SendGump(new DialogueGump(player, trustModule));
                                    });
                                player.SendGump(new DialogueGump(player, helpModule));
                            });
                        player.SendGump(new DialogueGump(player, shopModule));
                    });
                pastModule.AddOption("What did you love about being a florist?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule loveModule = new DialogueModule("Every flower tells a story. I loved creating beautiful arrangements for people to share joy. Each petal held a memory.");
                        loveModule.AddOption("What was your favorite flower?",
                            p => true,
                            p =>
                            {
                                DialogueModule favoriteModule = new DialogueModule("Roses, of course! Their beauty is matched only by their thorns, much like life itself. Each color carries a different meaning.");
                                favoriteModule.AddOption("What does a black rose symbolize?",
                                    plr => true,
                                    plr =>
                                    {
                                        DialogueModule blackRoseModule = new DialogueModule("A black rose often symbolizes death or farewell. It reminds us that all things must come to an end, making room for new beginnings.");
                                        blackRoseModule.AddOption("Such a deep thought.",
                                            pt => true,
                                            pt => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                        favoriteModule.AddOption("Interesting. Tell me more about other flowers.",
                                            ply => true,
                                            ply => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        player.SendGump(new DialogueGump(player, blackRoseModule));
                                    });
                                loveModule.AddOption("I never thought of flowers that way.",
                                    plu => true,
                                    plu => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                player.SendGump(new DialogueGump(player, favoriteModule));
                            });
                        pastModule.AddOption("What do you miss the most?",
                            pli => true,
                            pli =>
                            {
                                DialogueModule missModule = new DialogueModule("The laughter of customers, the hustle of the market. Now, I only hear whispers of passing souls.");
                                missModule.AddOption("Have you found joy in anything now?",
                                    p => true,
                                    p =>
                                    {
                                        DialogueModule joyModule = new DialogueModule("Small moments, like the sun warming my face or a shared smile with a passerby. These fleeting joys remind me I'm alive.");
                                        joyModule.AddOption("That's beautiful, Lily.",
                                            plo => true,
                                            plo => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        player.SendGump(new DialogueGump(player, joyModule));
                                    });
                                player.SendGump(new DialogueGump(player, missModule));
                            });
                        player.SendGump(new DialogueGump(player, loveModule));
                    });
                player.SendGump(new DialogueGump(player, pastModule));
            });

        greeting.AddOption("What do you do for a living?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("Begging for coin and food is my job, kind stranger. Most days, I manage to gather enough for a small meal.");
                jobModule.AddOption("How can I help?",
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
                            pl.SendMessage("Thank you for your kindness. Here, take this small token as a reminder of hope.");
                            pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                jobModule.AddOption("What do you usually find?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule findModule = new DialogueModule("Sometimes coins, sometimes scraps of food. Occasionally, a kind soul gives me a warm blanket or a piece of bread. Each gift is a treasure.");
                        findModule.AddOption("What’s the most valuable thing you’ve found?",
                            p => true,
                            p =>
                            {
                                DialogueModule valuableModule = new DialogueModule("Once, I found a locket with a picture inside. It seemed to hold a story of love, now lost. I wonder who it belonged to.");
                                valuableModule.AddOption("What will you do with it?",
                                    plp => true,
                                    plp =>
                                    {
                                        DialogueModule decisionModule = new DialogueModule("I keep it close, a reminder that every person has a tale. One day, I hope to return it to its rightful owner, should I meet them.");
                                        decisionModule.AddOption("That's a lovely thought.",
                                            pa => true,
                                            pa => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                        valuableModule.AddOption("I hope you find the owner.",
                                            pls => true,
                                            pls => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        player.SendGump(new DialogueGump(player, decisionModule));
                                    });
                                findModule.AddOption("Such a touching story.",
                                    pld => true,
                                    pld => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                player.SendGump(new DialogueGump(player, valuableModule));
                            });
                        player.SendGump(new DialogueGump(player, findModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What are your thoughts on kindness?",
            player => true,
            player =>
            {
                DialogueModule kindnessModule = new DialogueModule("Kindness is vital. It's what keeps me going, even on the coldest nights. A simple hello can make a world of difference.");
                kindnessModule.AddOption("Can you share a moment of kindness you experienced?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule momentModule = new DialogueModule("One winter night, a traveler stopped to share his fire and food. He told stories of distant lands, warming my heart more than the flames.");
                        momentModule.AddOption("That sounds magical.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, momentModule));
                    });
                kindnessModule.AddOption("How can I spread kindness?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule spreadModule = new DialogueModule("Small acts can create ripples. A smile, a helping hand, or simply listening to someone's story can brighten a dark day.");
                        spreadModule.AddOption("I'll remember that.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, spreadModule));
                    });
                player.SendGump(new DialogueGump(player, kindnessModule));
            });

        greeting.AddOption("What do you struggle with?",
            player => true,
            player =>
            {
                DialogueModule struggleModule = new DialogueModule("It's not just hunger, but also the cold nights and occasional cruelty of passersby. Yet, some still show kindness.");
                struggleModule.AddOption("What keeps you going?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule hopeModule = new DialogueModule("Hope is a fragile thing. Sometimes, I see a child laugh or hear music in the distance, and it reminds me of life's beauty.");
                        hopeModule.AddOption("Life can be beautiful, indeed.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, hopeModule));
                    });
                struggleModule.AddOption("What about loneliness?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule lonelyModule = new DialogueModule("Loneliness is a silent battle. Sometimes a simple hello or a shared meal can make a world of difference.");
                        lonelyModule.AddOption("You deserve kindness, too.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        player.SendGump(new DialogueGump(player, lonelyModule));
                    });
                player.SendGump(new DialogueGump(player, struggleModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Lily nods her head as you leave. 'Take care, kind stranger. May your path be filled with light.'");
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
