using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Zhuge Liang")]
public class ZhugeLiang : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ZhugeLiang() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Zhuge Liang";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(130);
        SetHits(75);

        // Appearance
        AddItem(new Robe() { Hue = 1117 });
        AddItem(new Sandals() { Hue = 1117 });
        AddItem(new Mace() { Name = "Zhuge's Fan" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Zhuge Liang. How may I assist you today?");

        greeting.AddOption("What is your name?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I am known as Zhuge Liang, a strategist of great renown.")));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My well-being is of little consequence, for my mind is my greatest asset.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I serve as a strategist and advisor, wielding the power of knowledge and planning.")));
            });

        greeting.AddOption("Tell me about wisdom.",
            player => true,
            player =>
            {
                DialogueModule wisdomModule = new DialogueModule("True power lies not in might, but in the cunning of the mind. Are you wise?");
                wisdomModule.AddOption("Yes.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Wisdom is a treasure, indeed. May you find it on your journey.")));
                    });
                wisdomModule.AddOption("No.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule encouragementModule = new DialogueModule("Seek knowledge, and wisdom will follow. Learning is a lifelong journey.");
                        encouragementModule.AddOption("How can I learn more?",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Read books, seek mentors, and experience the world. Every moment is a lesson.")));
                            });
                        pl.SendGump(new DialogueGump(pl, encouragementModule));
                    });
                player.SendGump(new DialogueGump(player, wisdomModule));
            });

        greeting.AddOption("What do you know about strategy?",
            player => true,
            player =>
            {
                DialogueModule strategyModule = new DialogueModule("Indeed, strategy is an art. It’s understanding people and predicting outcomes. Would you like to learn some strategies?");
                strategyModule.AddOption("Yes, please teach me.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule strategyDetails = new DialogueModule("One essential strategy is to know your opponent's strengths and weaknesses. What area interests you?");
                        strategyDetails.AddOption("Military tactics.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("In warfare, positioning and deception can turn the tide. Consider the terrain and morale of your troops.")));
                            });
                        strategyDetails.AddOption("Diplomatic strategies.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("In diplomacy, the right words can forge alliances or create enemies. Always listen more than you speak.")));
                            });
                        strategyDetails.AddOption("Economic strategies.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Understanding trade routes and resource management is crucial. Wealth can provide power.")));
                            });
                        pl.SendGump(new DialogueGump(pl, strategyDetails));
                    });
                strategyModule.AddOption("No, I already know enough.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well. A wise person knows their limits.")));
                    });
                player.SendGump(new DialogueGump(player, strategyModule));
            });

        greeting.AddOption("Can you teach me?",
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
                    lastRewardTime = DateTime.UtcNow;
                    player.AddToBackpack(new MaxxiaScroll()); // Replace with actual item name
                    player.SendGump(new DialogueGump(player, new DialogueModule("Ah, a keen mind! Here is a scroll containing some of my strategies.")));
                }
            });

        greeting.AddOption("What is your purpose?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("A man without purpose is like a ship without a compass. Seek your own purpose, and you might find greatness.")));
            });

        greeting.AddOption("Do you have any advice for me?",
            player => true,
            player =>
            {
                DialogueModule adviceModule = new DialogueModule("My advice is simple: always be prepared, and never underestimate your opponents. Each encounter can teach you something.");
                adviceModule.AddOption("Can you elaborate?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Preparation involves gathering knowledge and resources. Know your environment and your allies.")));
                    });
                adviceModule.AddOption("What if I fail?",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Failure is a part of the journey. Learn from it and rise stronger.")));
                    });
                player.SendGump(new DialogueGump(player, adviceModule));
            });

        return greeting;
    }

    public ZhugeLiang(Serial serial) : base(serial) { }

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
