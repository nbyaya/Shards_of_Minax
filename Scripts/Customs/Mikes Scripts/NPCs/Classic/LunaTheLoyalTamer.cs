using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class LunaTheLoyalTamer : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LunaTheLoyalTamer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Luna the Loyal Tamer";
        Body = 0x191; // Human female body

        // Stats
        SetStr(90);
        SetDex(70);
        SetInt(80);
        SetHits(90);

        // Appearance
        AddItem(new Skirt() { Hue = 65 });
        AddItem(new BodySash() { Hue = 34 });
        AddItem(new Sandals() { Hue = 1175 });
        AddItem(new ShepherdsCrook() { Name = "Luna's Crook" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Luna the Loyal Tamer. How may I assist you today?");

        greeting.AddOption("Tell me about your job.",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("I am an animal tamer, dedicated to nurturing and training the creatures of this land. Every day is an adventure filled with challenges and rewards.");
                jobModule.AddOption("What kind of animals do you tame?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule animalsModule = new DialogueModule("I've tamed various creatures, from the humble sheep to majestic dragons. Each animal has its own personality and needs.");
                        animalsModule.AddOption("What do you mean by personality?",
                            p => true,
                            p =>
                            {
                                DialogueModule personalityModule = new DialogueModule("Every creature is unique. For example, some dogs are playful and affectionate, while others are more reserved. Understanding them is key to successful taming.");
                                personalityModule.AddOption("How do you build trust with them?",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, personalityModule));
                                    });
                                p.SendGump(new DialogueGump(p, personalityModule));
                            });
                        animalsModule.AddOption("What challenges do you face?",
                            p => true,
                            p =>
                            {
                                DialogueModule challengesModule = new DialogueModule("Taming can be quite difficult. Some creatures resist training, while others may be frightened or aggressive. Patience and understanding are essential.");
                                challengesModule.AddOption("Do you have any success stories?",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, challengesModule));
                                    });
                                p.SendGump(new DialogueGump(p, challengesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, animalsModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What about compassion?",
            player => true,
            player =>
            {
                DialogueModule compassionModule = new DialogueModule("Compassion is crucial in my work. It means understanding and caring for the feelings of both animals and people.");
                compassionModule.AddOption("Can you give me an example?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule exampleModule = new DialogueModule("Once, I found a wounded wolf. Instead of fearing it, I showed compassion, bandaging its wounds. In time, it learned to trust me and became a loyal companion.");
                        exampleModule.AddOption("What happened to the wolf?",
                            p => true,
                            p =>
                            {
                                DialogueModule wolfStoryModule = new DialogueModule("The wolf, whom I named Shadow, became my protector and friend. Together, we explored many hidden paths in the wilderness.");
                                wolfStoryModule.AddOption("That sounds amazing!",
                                    ple => true,
                                    ple =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, wolfStoryModule));
                            });
                        pl.SendGump(new DialogueGump(pl, exampleModule));
                    });
                compassionModule.AddOption("I see; compassion is vital.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, compassionModule));
            });

        greeting.AddOption("Do you have any rewards for kindness?",
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
                    player.SendMessage("True kindness is rare. I have a small token for you.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            });

        greeting.AddOption("What is loyalty to you?",
            player => true,
            player =>
            {
                DialogueModule loyaltyModule = new DialogueModule("Loyalty is the strongest bond one can have, whether with a person or an animal. It’s built through trust and shared experiences.");
                loyaltyModule.AddOption("Can you share a story about loyalty?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule loyaltyStoryModule = new DialogueModule("Once, I had a loyal falcon named Astra. She would scout ahead during our journeys, always returning to my side, no matter the distance.");
                        loyaltyStoryModule.AddOption("What made Astra special?",
                            p => true,
                            p =>
                            {
                                DialogueModule astraDetailsModule = new DialogueModule("Astra had a keen eye and unmatched speed. I trained her to respond to my call, and she never let me down, even in dangerous situations.");
                                astraDetailsModule.AddOption("Did you face any dangers?",
                                    plr => true,
                                    plr =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, astraDetailsModule));
                                    });
                                p.SendGump(new DialogueGump(p, astraDetailsModule));
                            });
                        pl.SendGump(new DialogueGump(pl, loyaltyStoryModule));
                    });
                loyaltyModule.AddOption("How can I cultivate loyalty?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule cultivationModule = new DialogueModule("Loyalty is cultivated through honesty, reliability, and showing care. Spend time with those you wish to bond with, and always be there in their time of need.");
                        cultivationModule.AddOption("Thank you for the advice.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        pl.SendGump(new DialogueGump(pl, cultivationModule));
                    });
                player.SendGump(new DialogueGump(player, loyaltyModule));
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

    public LunaTheLoyalTamer(Serial serial) : base(serial) { }
}
