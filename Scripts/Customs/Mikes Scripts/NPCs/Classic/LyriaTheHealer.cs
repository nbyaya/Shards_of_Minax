using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Lyria the Healer")]
public class LyriaTheHealer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LyriaTheHealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lyria the Healer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(100);
        SetDex(80);
        SetInt(80);
        SetHits(80);

        // Appearance
        AddItem(new Robe() { Hue = 2501 });
        AddItem(new Sandals() { Hue = 1153 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(true); // true for female
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue;
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
        DialogueModule greeting = new DialogueModule("I am Lyria the Healer! How may I assist you today?");

        greeting.AddOption("Tell me about your healing.",
            player => true,
            player => 
            {
                DialogueModule healingModule = new DialogueModule("Healing is not just mending bones, it's also soothing the soul. Would you like to learn about herbs?");
                healingModule.AddOption("Yes, tell me about herbs.",
                    p => true,
                    p =>
                    {
                        DialogueModule herbModule = new DialogueModule("Each herb has a unique property. For example, Ginseng can help restore one's stamina.");
                        herbModule.AddOption("What about Mandrake root?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateMandrakeModule()));
                            });
                        herbModule.AddOption("What herbs are used for healing?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule healingHerbs = new DialogueModule("Healing herbs like Aloe Vera, Echinacea, and Yarrow are vital for restoring health. Would you like to know about their specific uses?");
                                healingHerbs.AddOption("Yes, tell me more.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule specificUses = new DialogueModule("Aloe Vera is great for burns, Echinacea boosts the immune system, and Yarrow is excellent for stopping bleeding. Do you wish to know how to find them?");
                                        specificUses.AddOption("How can I find these herbs?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule findingHerbs = new DialogueModule("Aloe Vera can often be found near water sources, while Echinacea grows in sunny fields. Yarrow prefers damp, grassy areas. Be cautious; some areas are inhabited by aggressive creatures.");
                                                findingHerbs.AddOption("I will seek these herbs!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("You set off to gather the herbs.");
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                findingHerbs.AddOption("Maybe later.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, findingHerbs));
                                            });
                                        specificUses.AddOption("Not right now.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, specificUses));
                                    });
                                healingHerbs.AddOption("Not interested.",
                                    plb => true,
                                    plb =>
                                    {
                                        plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, healingHerbs));
                            });
                        herbModule.AddOption("Maybe another time.",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, herbModule));
                    });
                healingModule.AddOption("No, thank you.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healingModule));
            });

        greeting.AddOption("What can you tell me about balance?",
            player => true,
            player =>
            {
                DialogueModule balanceModule = new DialogueModule("Balance in life comes from understanding oneself and one's surroundings. Nature has a lot to teach us. Do you seek balance in your life?");
                balanceModule.AddOption("Yes, I do.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule seekingBalance = new DialogueModule("It's a journey that requires introspection and connection to the world around you. Meditation and the right herbs can help. Would you like to learn meditation techniques?");
                        seekingBalance.AddOption("Absolutely!",
                            plc => true,
                            plc =>
                            {
                                DialogueModule meditationModule = new DialogueModule("Meditation helps clear the mind and focus your energy. Find a quiet place, sit comfortably, and breathe deeply. Visualize your troubles floating away. Do you wish to try it now?");
                                meditationModule.AddOption("Yes, let's try.",
                                    pld => true,
                                    pld =>
                                    {
                                        pld.SendMessage("You take a moment to meditate, feeling a sense of peace wash over you.");
                                        pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                    });
                                meditationModule.AddOption("Maybe later.",
                                    pld => true,
                                    pld =>
                                    {
                                        pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                    });
                                plc.SendGump(new DialogueGump(plc, meditationModule));
                            });
                        seekingBalance.AddOption("No, I prefer action.",
                            plc => true,
                            plc =>
                            {
                                plc.SendMessage("Action is also a path to balance. Engage with the world around you!");
                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, seekingBalance));
                    });
                balanceModule.AddOption("No, I'm fine.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, balanceModule));
            });

        greeting.AddOption("Do you have any rewards for knowledge?",
            player => CanRewardKnowledge(player),
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateRewardModule()));
            });

        greeting.AddOption("Tell me about your family.",
            player => true,
            player =>
            {
                DialogueModule familyModule = new DialogueModule("My family has a long lineage of healers. My grandmother taught me the ways of herbs, and my mother shared wisdom about the soul. Would you like to hear a story from my childhood?");
                familyModule.AddOption("Yes, please!",
                    pl => true,
                    pl =>
                    {
                        DialogueModule storyModule = new DialogueModule("When I was young, I ventured into the woods with my grandmother. She taught me how to identify plants and their properties. One day, we encountered a wounded deer. We healed it together, and it changed my life forever.");
                        storyModule.AddOption("What happened to the deer?",
                            plc => true,
                            plc =>
                            {
                                plc.SendMessage("The deer recovered and returned to the wild, but it inspired me to dedicate my life to healing.");
                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                            });
                        storyModule.AddOption("That's a lovely story.",
                            plc => true,
                            plc =>
                            {
                                plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, storyModule));
                    });
                familyModule.AddOption("Not right now.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, familyModule));
            });

        return greeting;
    }

    private DialogueModule CreateMandrakeModule()
    {
        DialogueModule mandrakeModule = new DialogueModule("Mandrake root is powerful and used in many rituals. It can enhance your healing abilities. Do you wish to learn how to use it?");
        mandrakeModule.AddOption("Yes, how do I use it?",
            pl => true,
            pl =>
            {
                DialogueModule usageModule = new DialogueModule("To use Mandrake, you can brew it into a tincture or use it in rituals. It requires careful handling due to its potent properties. Would you like to learn a simple tincture recipe?");
                usageModule.AddOption("Yes, tell me the recipe!",
                    plc => true,
                    plc =>
                    {
                        plc.SendMessage("To make a tincture, chop the Mandrake root finely, then steep it in alcohol for two weeks. Strain and store in a dark bottle.");
                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                    });
                usageModule.AddOption("Maybe another time.",
                    plc => true,
                    plc =>
                    {
                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, usageModule));
            });
        mandrakeModule.AddOption("No, I want to know more about it first.",
            pl => true,
            pl =>
            {
                DialogueModule detailsModule = new DialogueModule("Mandrake has a long history in folklore. It's said to scream when pulled from the ground, and its root often resembles a human figure. Are you interested in its historical uses?");
                detailsModule.AddOption("Yes, tell me about its history.",
                    plc => true,
                    plc =>
                    {
                        plc.SendMessage("Historically, Mandrake has been used for its medicinal properties and in various magical rituals. It was once thought to have protective powers against evil spirits.");
                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                    });
                detailsModule.AddOption("No, I'm fine.",
                    plc => true,
                    plc =>
                    {
                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                    });
                pl.SendGump(new DialogueGump(pl, detailsModule));
            });
        return mandrakeModule;
    }

    private bool CanRewardKnowledge(PlayerMobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        return DateTime.UtcNow - lastRewardTime >= cooldown;
    }

    private DialogueModule CreateRewardModule()
    {
        lastRewardTime = DateTime.UtcNow;
        DialogueModule rewardModule = new DialogueModule("Knowledge is a treasure, but applying it is the key to wisdom. For your thirst for understanding, here's a small reward.");
        rewardModule.AddOption("Thank you!",
            pl => true,
            pl =>
            {
                pl.AddToBackpack(new MaxxiaScroll()); // Example reward
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return rewardModule;
    }

    public LyriaTheHealer(Serial serial) : base(serial) { }

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
