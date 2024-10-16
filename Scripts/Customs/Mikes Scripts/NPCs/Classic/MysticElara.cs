using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class MysticElara : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MysticElara() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Mystic Elara";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();
        
        // Stats
        SetStr(100);
        SetDex(75);
        SetInt(150);
        SetHits(100);
        
        // Appearance
        AddItem(new Robe() { Hue = 1109 });
        AddItem(new Sandals() { Hue = 1905 });
        AddItem(new WizardsHat() { Hue = 1109 });
        AddItem(new LeatherGloves() { Hue = 1109 });
        
        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public MysticElara(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Mystic Elara. How may I assist you today?");
        
        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule selfModule = new DialogueModule("I am a mystic, devoted to the study of ancient knowledge and the balance of energies. The world is a tapestry of hidden truths, waiting to be unraveled.");
                selfModule.AddOption("What drives your studies?",
                    p => true,
                    p =>
                    {
                        DialogueModule driveModule = new DialogueModule("The mysteries of life and the forces that govern it fuel my passion. I seek to understand the connection between all things—each thought, action, and element plays a role in this grand design.");
                        driveModule.AddOption("How can I learn more about this?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, driveModule));
                    });
                player.SendGump(new DialogueGump(player, selfModule));
            });

        greeting.AddOption("Do you have any secrets to share?",
            player => true,
            player =>
            {
                DialogueModule secretsModule = new DialogueModule("Secrets are but truths waiting to be uncovered. If you seek, you shall find. However, some secrets come with a price. Do you have the courage to pay it?");
                secretsModule.AddOption("What price?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule priceModule = new DialogueModule("Artifacts imbued with the power of virtues are invaluable. If you bring me one, I may reward you generously.");
                        priceModule.AddOption("Tell me about these artifacts.",
                            p => true,
                            p =>
                            {
                                DialogueModule artifactModule = new DialogueModule("Artifacts come in many forms, often holding immense power or knowledge. Some are relics of ancient civilizations, while others are crafted by skilled hands. Each has a story to tell.");
                                artifactModule.AddOption("How do I find these artifacts?",
                                    plr => true,
                                    plr =>
                                    {
                                        pl.SendMessage("Seek ancient ruins or consult the wise; many hidden treasures lie in places forgotten by time.");
                                    });
                                p.SendGump(new DialogueGump(p, artifactModule));
                            });
                        pl.SendGump(new DialogueGump(pl, priceModule));
                    });
                player.SendGump(new DialogueGump(player, secretsModule));
            });

        greeting.AddOption("Do you meditate?",
            player => true,
            player =>
            {
                DialogueModule meditateModule = new DialogueModule("Meditation allows us to connect with the energies around us, to find peace and clarity. I have a special mantra that I can share with those who prove worthy. Do you seek this mantra?");
                meditateModule.AddOption("Yes, I seek the mantra.",
                    pl => true,
                    pl =>
                    {
                        pl.SendMessage("I will share my mantra with you when the time is right, for it requires a pure heart and clear mind to understand its true meaning.");
                    });
                player.SendGump(new DialogueGump(player, meditateModule));
            });

        greeting.AddOption("What are the virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("The virtues are guiding principles that lead us to harmony with ourselves and the world around us. They are Compassion, Honor, Sacrifice, and others. Each plays a vital role in our lives.");
                virtuesModule.AddOption("Which virtue do you value most?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule valueModule = new DialogueModule("I hold Compassion dear; it fosters understanding and unity among all beings. Without it, our world would be devoid of empathy.");
                        valueModule.AddOption("How can I cultivate compassion?",
                            p => true,
                            p =>
                            {
                                p.SendMessage("Practice kindness in your daily life, seek to understand others' struggles, and extend a hand to those in need.");
                            });
                        pl.SendGump(new DialogueGump(pl, valueModule));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Tell me about balance.",
            player => true,
            player =>
            {
                DialogueModule balanceModule = new DialogueModule("Balance is the key to understanding existence. Like light and dark, good and evil, every aspect of life has its counterpart. One cannot exist without the other.");
                balanceModule.AddOption("How can I find balance?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule findBalanceModule = new DialogueModule("Finding balance involves introspection and honesty with oneself. Acknowledge your strengths and weaknesses, and strive to nurture both.");
                        findBalanceModule.AddOption("Thank you for the advice.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, findBalanceModule));
                    });
                player.SendGump(new DialogueGump(player, balanceModule));
            });

        greeting.AddOption("Tell me about opposing forces.",
            player => true,
            player =>
            {
                DialogueModule opposingModule = new DialogueModule("Opposing forces shape our reality. It's the balance between them that defines our path. Have you ever felt torn between two choices?");
                opposingModule.AddOption("Yes, often.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tornModule = new DialogueModule("Understanding these feelings is crucial. Reflect on what each choice represents for you. What does your heart tell you?");
                        tornModule.AddOption("I'll think about it.",
                            p => true,
                            p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                        pl.SendGump(new DialogueGump(pl, tornModule));
                    });
                player.SendGump(new DialogueGump(player, opposingModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("May balance guide your path. Return whenever you seek knowledge.");
            });

        return greeting;
    }

    private void CheckReward(Mobile player)
    {
        TimeSpan cooldown = TimeSpan.FromMinutes(10);
        if (DateTime.UtcNow - lastRewardTime < cooldown)
        {
            player.SendMessage("I have no reward right now. Please return later.");
        }
        else
        {
            player.SendMessage("You've shown great interest in the virtues. As a token of my appreciation, take this. May it guide you on your journey.");
            player.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }
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
