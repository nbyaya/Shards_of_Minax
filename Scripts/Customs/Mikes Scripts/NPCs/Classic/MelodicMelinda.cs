using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Melodic Melinda")]
public class MelodicMelinda : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public MelodicMelinda() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Melodic Melinda";
        Body = 0x191; // Human female body

        // Stats
        SetStr(120);
        SetDex(65);
        SetInt(80);
        SetHits(85);

        // Appearance
        AddItem(new FancyDress() { Hue = 72 });
        AddItem(new Sandals() { Hue = 1157 });
        AddItem(new LeatherGloves() { Name = "Melinda's Melody Gloves" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public MelodicMelinda(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Melodic Melinda, the bard. How can I assist you today?");
        
        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I weave tales and songs, traveling from town to town. Music and art are my lifeblood. I often find inspiration in the beauty of nature.");
                aboutModule.AddOption("What inspires your music?",
                    p => true,
                    p =>
                    {
                        DialogueModule inspirationModule = new DialogueModule("The rustling of leaves, the flowing rivers, and the whispers of the wind all spark my creativity. Each sound tells a story.");
                        inspirationModule.AddOption("Can you share a story?",
                            pl => true,
                            pl => 
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Once, I composed a ballad about a brave knight who saved a village from a fierce dragon. The villagers honored him with a grand feast!")));
                            });
                        p.SendGump(new DialogueGump(p, inspirationModule));
                    });
                greeting.AddOption("What is your favorite story?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("My favorite story is about the ancient druid circle, where the druids would commune with nature and channel the earth's energies.")));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("I'm in good health, thank you for asking! Music always lifts my spirits.")));
            });

        greeting.AddOption("What do you think of honesty?",
            player => true,
            player =>
            {
                DialogueModule honestyModule = new DialogueModule("True honesty is a gift. Do you value honesty?");
                honestyModule.AddOption("Yes, I do.",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("I have no reward right now. Please return later.")));
                        }
                        else
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward item
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, new DialogueModule("As a token of appreciation, here's a small reward for you.")));
                        }
                    });
                honestyModule.AddOption("What do you mean?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The eight virtues guide our lives. Among them, honesty is the foundation.")));
                    });
                player.SendGump(new DialogueGump(player, honestyModule));
            });

        greeting.AddOption("What do you know about spirituality?",
            player => true,
            player =>
            {
                DialogueModule spiritualityModule = new DialogueModule("Art and music are expressions of spirituality. Have you pondered its nature?");
                spiritualityModule.AddOption("Tell me about the Moonstone.",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The Moonstone is said to hold immense power, especially when the two moons align. Legends say it's hidden in a secret shrine.")));
                    });
                spiritualityModule.AddOption("Do you have any spiritual practices?",
                    p => true,
                    p =>
                    {
                        DialogueModule practicesModule = new DialogueModule("I often meditate under the moonlight, letting the music of the night guide my thoughts. It brings clarity and peace.");
                        practicesModule.AddOption("Can you teach me to meditate?",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Of course! Find a quiet spot, close your eyes, and listen to the sounds around you. Breathe deeply and let your mind wander.")));
                            });
                        p.SendGump(new DialogueGump(p, practicesModule));
                    });
                player.SendGump(new DialogueGump(player, spiritualityModule));
            });

        greeting.AddOption("What is your favorite place in the forest?",
            player => true,
            player =>
            {
                DialogueModule forestModule = new DialogueModule("The forest is filled with magic! My favorite spot is an ancient grove, where the trees sing with the wind.");
                forestModule.AddOption("What happens there?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("The grove is alive with energy, and I feel inspired to compose my best melodies there. It's a sanctuary of peace.")));
                    });
                forestModule.AddOption("Can I visit it with you?",
                    p => true,
                    p => 
                    {
                        p.SendGump(new DialogueGump(p, new DialogueModule("Absolutely! We can gather herbs and enjoy the sounds of nature together. Let's set off!")));
                    });
                player.SendGump(new DialogueGump(player, forestModule));
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
