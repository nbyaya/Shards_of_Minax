using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Locke Cole")]
public class LockeCole : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LockeCole() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Locke Cole";
        Body = 0x190; // Human male body

        // Stats
        SetStr(100);
        SetDex(130);
        SetInt(70);
        SetHits(75);

        // Appearance
        AddItem(new LeatherLegs() { Hue = 1175 });
        AddItem(new LeatherChest() { Hue = 1175 });
        AddItem(new LeatherCap() { Hue = 1175 });
        AddItem(new Dagger() { Name = "Locke's Dagger" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public LockeCole(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, stranger. I'm Locke Cole. What would you like to know?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => player.SendGump(new DialogueGump(player, new DialogueModule("I've had my fair share of scrapes, but I'm still standing."))));

        greeting.AddOption("What do you do for a living?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I make a living as a treasure hunter, searching for hidden relics. It's an adventure unlike any other!");
                jobModule.AddOption("What treasures have you found?",
                    p => true,
                    p =>
                    {
                        DialogueModule treasuresModule = new DialogueModule("Oh, some fascinating things! I've discovered ancient coins, enchanted artifacts, and even a cursed amulet.");
                        treasuresModule.AddOption("Tell me about the cursed amulet.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("It's said to bring misfortune to its wearer. I found it in a sunken ship, and it took weeks to get rid of the bad luck!")));
                            });
                        treasuresModule.AddOption("What about the enchanted artifacts?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Those are the best! They can enhance your abilities, but you must be careful. Not all are what they seem.")));
                            });
                        p.SendGump(new DialogueGump(p, treasuresModule));
                    });
                jobModule.AddOption("What challenges do you face?",
                    p => true,
                    p =>
                    {
                        DialogueModule challengesModule = new DialogueModule("Every hunt comes with its dangers. From traps to fierce creatures, it requires both skill and luck to survive.");
                        challengesModule.AddOption("What creatures have you encountered?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I've faced wolves, trolls, and even a dragon once. Each encounter teaches you something new.")));
                            });
                        challengesModule.AddOption("How do you prepare for a hunt?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Preparation is key! I gather supplies, study maps, and ensure my gear is in top condition.")));
                            });
                        p.SendGump(new DialogueGump(p, challengesModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What are your thoughts on challenges?",
            player => true,
            player =>
            {
                DialogueModule challenges = new DialogueModule("Life's full of choices, isn't it? What's your approach to challenges?");
                challenges.AddOption("I face them head-on.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("A bold approach! Courage is vital in our world. What drives your bravery?")));
                    });
                challenges.AddOption("I prefer to plan ahead.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Smart thinking! A good plan can save your life. Do you have a favorite strategy?")));
                    });
                player.SendGump(new DialogueGump(player, challenges));
            });

        greeting.AddOption("Do you have any stories about your adventures?",
            player => true,
            player =>
            {
                DialogueModule storiesModule = new DialogueModule("Every scar tells a story, and trust me, I've got plenty. Would you like to hear about a specific adventure?");
                storiesModule.AddOption("Yes, tell me about your encounter with a dragon.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("It was a fateful night. I had stumbled upon a lair deep in the mountains. The dragon was fierce, and escaping its wrath was a true test of my skills.")));
                    });
                storiesModule.AddOption("How about a treasure hunt gone wrong?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, there's a tale! I once thought I'd found a hidden trove, but it turned out to be a trap laid by mercenaries. I barely escaped!")));
                    });
                player.SendGump(new DialogueGump(player, storiesModule));
            });

        greeting.AddOption("What about treasure?",
            player => true,
            player =>
            {
                DialogueModule treasureModule = new DialogueModule("It's not just about the gold and jewels. It's the thrill of the hunt, and the legends behind each relic that drive me.");
                treasureModule.AddOption("What legends have you encountered?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("One legend speaks of an ancient city, hidden beneath the mountains, where time stands still. I aim to find it one day.")));
                    });
                treasureModule.AddOption("What do you think makes a treasure valuable?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Value comes from history and rarity. A simple trinket can be worth more than gold if it has a captivating story.")));
                    });
                player.SendGump(new DialogueGump(player, treasureModule));
            });

        greeting.AddOption("What do you know about rumors?",
            player => true,
            player =>
            {
                DialogueModule rumorModule = new DialogueModule("They say that the most valuable treasures are guarded by the most dangerous creatures. But, for those brave enough to face them, the rewards are worth it.");
                rumorModule.AddOption("What creatures do you speak of?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Dragons, hydras, and ancient golems. Each holds treasures beyond imagination, but they demand respect and caution.")));
                    });
                player.SendGump(new DialogueGump(player, rumorModule));
            });

        greeting.AddOption("Tell me about your scars.",
            player => true,
            player =>
            {
                DialogueModule scarsModule = new DialogueModule("This one here on my arm? Got that from a near encounter with a dragon. Not my finest moment, but I learned a valuable lesson.");
                scarsModule.AddOption("What lesson did you learn?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Respect the creatures of the wild, and always be prepared for the unexpected. A lesson hard-earned!")));
                    });
                player.SendGump(new DialogueGump(player, scarsModule));
            });

        greeting.AddOption("Do you have any rewards for me?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Speaking of rewards, for someone as inquisitive as you, here's a little something. Keep it safe, it might come in handy.")));
                    player.AddToBackpack(new StealingAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("What do you think about dragons?",
            player => true,
            player =>
            {
                DialogueModule dragonModule = new DialogueModule("Dragons are powerful, majestic creatures. Not something you'd want to confront without proper preparation. But their treasures? Unparalleled.");
                dragonModule.AddOption("Have you ever fought a dragon?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Once. It was a clash that tested every skill I had. The roar still echoes in my dreams.")));
                    });
                player.SendGump(new DialogueGump(player, dragonModule));
            });

        greeting.AddOption("Have you heard of Locke Cole?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, you've heard of me before? Rumors tend to spread, but not all of them are true. What have you heard?")));
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
