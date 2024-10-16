using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

[CorpseName("the corpse of King Leopold")]
public class KingLeopold : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KingLeopold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "King Leopold";
        Body = 0x190; // Human male body

        // Stats
        Str = 140;
        Dex = 60;
        Int = 90;
        Hits = 95;

        // Appearance
        AddItem(new RingmailLegs() { Hue = 2118 });
        AddItem(new RingmailChest() { Hue = 2118 });
        AddItem(new Helmet() { Hue = 2118 });
        AddItem(new RingmailGloves() { Hue = 2118 });
        AddItem(new Boots() { Hue = 2118 });
        AddItem(new WarMace() { Name = "Congo Pacifier" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public KingLeopold(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am King Leopold, ruler of these lands. How may I assist you?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I am in perfect health, as a king should be. My days are filled with the duties of rulership.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My job is to maintain order and justice in these lands. It is both a privilege and a heavy burden.")));
            });

        greeting.AddOption("What qualities do you believe make a true leader?",
            player => true,
            player =>
            {
                DialogueModule qualitiesModule = new DialogueModule("But tell me, adventurer, what qualities do you believe make a true leader?");
                qualitiesModule.AddOption("Courage, compassion, and wisdom.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Your response intrigues me. Leadership indeed requires such qualities. Without them, a leader cannot inspire trust.")));
                    });
                qualitiesModule.AddOption("Honesty and humility.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, honesty is the bedrock of trust, and humility allows us to learn from those we lead.")));
                    });
                qualitiesModule.AddOption("Strategic thinking.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule strategyModule = new DialogueModule("Indeed! A leader must be a strategist, always thinking several steps ahead.");
                        strategyModule.AddOption("What strategies do you use?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("I often rely on counsel from my advisors and the wisdom of history to guide my decisions.")));
                            });
                        strategyModule.AddOption("Do you ever make mistakes?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Certainly! Mistakes are inevitable. It is how we learn from them that truly matters.")));
                            });
                        pl.SendGump(new DialogueGump(pl, strategyModule));
                    });
                player.SendGump(new DialogueGump(player, qualitiesModule));
            });

        greeting.AddOption("What can you tell me about these lands?",
            player => true,
            player =>
            {
                DialogueModule landsModule = new DialogueModule("These lands have a rich history, full of valor, honor, and sometimes, treachery. Have you ever heard about the Mantra of Honor?");
                landsModule.AddOption("No, what is it?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("The Mantra of Honor is a chant of great significance. It embodies the virtues we uphold.")));
                    });
                landsModule.AddOption("Yes, tell me more.",
                    pl => true,
                    pl =>
                    {
                        landsModule.AddOption("What is the second syllable?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("The second syllable is KIR. Seek the rest, and you shall be closer to mastering Honor.")));
                            });
                        pl.SendGump(new DialogueGump(pl, landsModule));
                    });
                player.SendGump(new DialogueGump(player, landsModule));
            });

        greeting.AddOption("Can you share the Mantra of Honor?",
            player => true,
            player =>
            {
                DialogueModule mantraModule = new DialogueModule("The Mantra of Honor is a chant of great significance. Those who understand it, uphold the virtue of Honor in its true essence.");
                mantraModule.AddOption("What else can you tell me about virtues?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Virtues are the pillars upon which our society stands. Each virtue has a mantra, a chant that embodies its essence.")));
                    });
                mantraModule.AddOption("How do I master it?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("To master the Mantra, one must seek knowledge and understanding of all virtues. Travel the lands and learn from others.")));
                    });
                player.SendGump(new DialogueGump(player, mantraModule));
            });

        greeting.AddOption("I seek wisdom.",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("Wisdom is not just the accumulation of knowledge, but the application of it. How do you seek wisdom on your travels?");
                wisdomModule.AddOption("I read ancient texts.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ancient texts hold great knowledge! They can guide us, but remember, interpretation is key.")));
                    });
                wisdomModule.AddOption("I seek counsel from elders.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Wise elders often hold the keys to our past. Their stories shape our understanding of the present.")));
                    });
                wisdomModule.AddOption("I learn from my experiences.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Experience is the best teacher. Each encounter adds to your wisdom.")));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("What do you think of courage?",
            player => true,
            player =>
            {
                DialogueModule courageModule = new DialogueModule("Courage is not the absence of fear, but the strength to face it. Many battles have been won not by the mightiest, but by the bravest. Do you consider yourself brave?");
                courageModule.AddOption("Yes, I am brave.",
                    pl => true,
                    pl =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("For your thoughtful inquiry, please accept this reward.")));
                            pl.AddToBackpack(new Gold(1000)); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        }
                    });
                courageModule.AddOption("No, I am not brave.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Bravery is something we can all strive for. Remember, every act of courage, no matter how small, matters.")));
                    });
                player.SendGump(new DialogueGump(player, courageModule));
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
