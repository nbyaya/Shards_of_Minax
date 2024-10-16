using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Jester Jibe")]
public class JesterJibe : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JesterJibe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jester Jibe";
        Body = 0x190; // Human male body

        // Stats
        SetStr(90);
        SetDex(75);
        SetInt(90);
        SetHits(90);

        // Appearance
        AddItem(new JesterHat() { Hue = 2128 });
        AddItem(new JesterSuit() { Hue = 2128 });
        AddItem(new ThighBoots() { Hue = 1104 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Jester Jibe, the eternal jester of these lands. What brings you to my merry company today?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
                player.SendGump(new DialogueGump(player, new DialogueModule("I am but a humble jester, always jesting and dancing, never a care in the world."))));

        greeting.AddOption("What is your job?",
            player => true,
            player =>
                player.SendGump(new DialogueGump(player, new DialogueModule("My job, you ask? It's to bring laughter to the world and remind folks of the virtues that bind us. But there's much more to it! Let me tell you...") 
                {
                    Options = {
                        new DialogueOption("What virtues do you speak of?", 
                            p => true, 
                            p => player.SendGump(new DialogueGump(player, new DialogueModule("The virtues are the heart of our community! Each one holds wisdom. Which would you like to learn about?") 
                            {
                                Options = {
                                    new DialogueOption("Compassion", 
                                        pl => true, 
                                        pl => player.SendGump(new DialogueGump(player, new DialogueModule("Compassion is about understanding others' pain. It binds us together, making us stronger!")))),
                                    new DialogueOption("Honor", 
                                        pl => true, 
                                        pl => player.SendGump(new DialogueGump(player, new DialogueModule("Honor is about standing by your word. A person of honor commands respect and trust.")))),
                                    new DialogueOption("Courage", 
                                        pl => true, 
                                        pl => player.SendGump(new DialogueGump(player, new DialogueModule("Courage allows us to face our fears and protect those we love. A truly noble trait!")))),
                                    new DialogueOption("Wisdom", 
                                        pl => true, 
                                        pl => player.SendGump(new DialogueGump(player, new DialogueModule("Wisdom guides our decisions. It comes from experience and listening to others."))))
                                }
                            }))),
                        new DialogueOption("Do you have any favorite jokes?", 
                            p => true, 
                            p => player.SendGump(new DialogueGump(player, new DialogueModule("Ah, humor is my forte! Here’s one: Why did the chicken join a band? Because it had the drumsticks!"))))
                    }
                })));

        greeting.AddOption("What can you tell me about 'Jibe'?",
            player => true,
            player =>
                player.SendGump(new DialogueGump(player, new DialogueModule("Jester Jibe is not my birth name, you know. I adopted it when I became a jester, to better fit the role. Before that, people knew me as a simple minstrel, wandering the lands with my lute."))));

        greeting.AddOption("What about your secret potion?",
            player => true,
            player =>
                player.SendGump(new DialogueGump(player, new DialogueModule("Jest and dance may keep my spirit alive, but I do have a secret potion that keeps me on my toes. It's made from rare herbs found only in the Whispering Woods. Want to hear more about the ingredients?") 
                {
                    Options = {
                        new DialogueOption("Yes, tell me more!", 
                            pl => true, 
                            pl => player.SendGump(new DialogueGump(player, new DialogueModule("The main ingredient is the Laughing Leaf, known for its uplifting properties. It only blooms under the light of a full moon!")))),
                        new DialogueOption("Sounds interesting, but dangerous...", 
                            pl => true, 
                            pl => player.SendGump(new DialogueGump(player, new DialogueModule("Dangerous, indeed! Many seek it, few find it. But the joy it brings is worth the peril, wouldn't you agree?"))))
                    }
                })));

        greeting.AddOption("Do you have a question for me?",
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
                player.SendGump(new DialogueGump(player, new DialogueModule("Ah, it's always a delight to meet someone who ponders the deeper questions of life. For your curiosity, allow me to gift you something. It may come in handy on your journey.")
                {
                    Options = { new DialogueOption("Receive your reward", p => true, p =>
                    {
                        p.AddToBackpack(new LuckAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    })}
                }));
            }
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

    public JesterJibe(Serial serial) : base(serial) { }
}
