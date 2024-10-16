using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ElaraSwiftwind : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ElaraSwiftwind() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elara Swiftwind";
        Body = 0x191; // Female human body

        // Stats
        SetStr(100);
        SetDex(90);
        SetInt(80);

        SetHits(100);

        // Appearance
        AddItem(new Kilt() { Hue = 1107 });
        AddItem(new BodySash() { Hue = 1123 });
        AddItem(new ElvenBoots() { Hue = 1152 });
        AddItem(new CompositeBow() { Name = "Elara's Bow" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ElaraSwiftwind(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Elara Swiftwind, the archer. What brings you to me today?");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Elara Swiftwind, skilled in the art of archery. The bow is an extension of myself, and I excel in its mastery.");
                aboutModule.AddOption("Tell me more about archery.",
                    p => true,
                    p =>
                    {
                        DialogueModule archeryModule = new DialogueModule("The bow is not just a weapon; it requires patience, understanding, and a connection with oneself to truly master. Each arrow is like a whisper to the wind, carrying a message.");
                        archeryModule.AddOption("Can you tell me about different types of magical wood?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule woodModule = new DialogueModule("Ah, magical woods are fascinating materials, each with its own unique properties that influence the kind of bow crafted. Allow me to elaborate on some of them.");
                                woodModule.AddOption("Tell me about Heartwood.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule heartwoodModule = new DialogueModule("Heartwood is a highly prized wood, known for its resilience and flexibility. Bows crafted from Heartwood are especially favored by experienced archers for their balanced draw weight and durability. It is said that a Heartwood bow can almost sense its master's intent, making it an excellent choice for those with a deep connection to their weapon.");
                                        heartwoodModule.AddOption("What kind of bows are best made with Heartwood?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule heartwoodBowModule = new DialogueModule("Heartwood is perfect for creating longbows that require a fine balance of power and accuracy. These bows are ideal for those who value consistency in their shots, particularly when hunting or in long-range combat. Heartwood's innate resilience also makes it suitable for bows that must endure frequent use without losing their form.");
                                                heartwoodBowModule.AddOption("That sounds impressive.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, heartwoodBowModule));
                                            });
                                        heartwoodModule.AddOption("Tell me about another type of wood.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, woodModule));
                                            });
                                        pll.SendGump(new DialogueGump(pll, heartwoodModule));
                                    });
                                woodModule.AddOption("Tell me about Yew wood.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule yewModule = new DialogueModule("Yew wood is known for its magical affinity, often chosen for bows that require a connection to the mystical energies of the world. Bows made from Yew are favored by rangers who channel magic through their shots, as the wood enhances the flow of energy, allowing arrows to carry enchantments more effectively.");
                                        yewModule.AddOption("What kind of bows are best made with Yew?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule yewBowModule = new DialogueModule("Yew is ideal for recurve bows, which benefit from the wood's natural ability to store energy. These bows are excellent for those who use spells in tandem with archery, as the Yew wood enhances the potency of enchanted arrows, making them more effective in magical combat.");
                                                yewBowModule.AddOption("That sounds fascinating.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, yewBowModule));
                                            });
                                        yewModule.AddOption("Tell me about another type of wood.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, woodModule));
                                            });
                                        pll.SendGump(new DialogueGump(pll, yewModule));
                                    });
                                woodModule.AddOption("Tell me about Ebony wood.",
                                    pll => true,
                                    pll =>
                                    {
                                        DialogueModule ebonyModule = new DialogueModule("Ebony wood is dark and dense, imbued with a mysterious energy that makes it ideal for bows meant to pierce even the toughest of armors. It is often used by archers who need to deliver powerful, penetrating shots, especially against heavily armored foes.");
                                        ebonyModule.AddOption("What kind of bows are best made with Ebony?",
                                            plll => true,
                                            plll =>
                                            {
                                                DialogueModule ebonyBowModule = new DialogueModule("Ebony is perfect for shortbows that focus on delivering swift, powerful shots. The density of the wood allows for high draw strength, making each shot pack a significant punch. These bows are favored by those who need to strike quickly and decisively, often in close-quarters combat.");
                                                ebonyBowModule.AddOption("I see, that sounds powerful.",
                                                    pllll => true,
                                                    pllll =>
                                                    {
                                                        pllll.SendGump(new DialogueGump(pllll, CreateGreetingModule()));
                                                    });
                                                plll.SendGump(new DialogueGump(plll, ebonyBowModule));
                                            });
                                        ebonyModule.AddOption("Tell me about another type of wood.",
                                            plll => true,
                                            plll =>
                                            {
                                                plll.SendGump(new DialogueGump(plll, woodModule));
                                            });
                                        pll.SendGump(new DialogueGump(pll, ebonyModule));
                                    });
                                woodModule.AddOption("Thank you, that's enough for now.",
                                    pll => true,
                                    pll =>
                                    {
                                        pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, woodModule));
                            });
                        archeryModule.AddOption("That's fascinating.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, archeryModule));
                    });
                aboutModule.AddOption("Perhaps another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("How is your health?",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("I am in good health, thank you for asking. An archer must always maintain their physical well-being.");
                healthModule.AddOption("Good to know.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("Tell me about battles.",
            player => true,
            player =>
            {
                DialogueModule battleModule = new DialogueModule("Valor in an archer lies not only in the precision of her shots but also in the wisdom of her choices. Are you wise?");
                battleModule.AddOption("Yes, I believe I am wise.",
                    p => true,
                    p =>
                    {
                        DialogueModule wisdomModule = new DialogueModule("Wisdom is a virtue that guides one's actions. Never underestimate the power of a well-placed arrow, and never let folly rule your choices.");
                        wisdomModule.AddOption("Thank you for the advice.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, wisdomModule));
                    });
                battleModule.AddOption("I am not sure.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, battleModule));
            });

        greeting.AddOption("Tell me about virtues.",
            player => true,
            player =>
            {
                DialogueModule virtueModule = new DialogueModule("Ah, virtues! They guide us, shape us, and remind us of our true path. Compassion is one such virtue, and its mantra is 'MUH'.");
                virtueModule.AddOption("What does Compassion mean to you?",
                    p => true,
                    p =>
                    {
                        DialogueModule compassionModule = new DialogueModule("Compassion is about understanding and empathy. It means putting yourself in another's shoes and feeling their emotions. The world could use more compassion.");
                        compassionModule.AddOption("I agree.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, compassionModule));
                    });
                virtueModule.AddOption("Thank you for sharing.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtueModule));
            });

        greeting.AddOption("I wish to ponder my destiny.",
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
                    DialogueModule rewardModule = new DialogueModule("Deep reflection on virtues is essential for one's personal growth. For your thoughtful inquiry, please accept this reward.");
                    rewardModule.AddOption("Thank you.",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new Gold(1000)); // Reward: 1000 gold
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
                player.SendMessage("Elara nods at you respectfully.");
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