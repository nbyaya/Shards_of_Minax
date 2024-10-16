using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Karl Marx")]
public class KarlMarx : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public KarlMarx() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Karl Marx";
        Body = 0x190; // Human male body

        // Stats
        SetStr(80);
        SetDex(60);
        SetInt(100);
        SetHits(70);

        // Appearance
        AddItem(new LongPants() { Hue = 1154 });
        AddItem(new FancyShirt() { Hue = 1109 });
        AddItem(new Boots() { Hue = 1109 });
        AddItem(new Spellbook() { Name = "Das Kapital" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public KarlMarx(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("I am Karl Marx, a philosopher and economist. What would you like to discuss?");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player => 
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My health is not relevant; what matters is the health of society.")));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("My mission is to promote the ideals of communism. What aspect would you like to explore?")));
                // Nested options
                DialogueModule jobModule = new DialogueModule("What aspect would you like to explore?");
                jobModule.AddOption("The role of philosophy.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Philosophy serves as the foundation for critical thought and societal critique. It allows us to question the status quo.")));
                    });
                jobModule.AddOption("How do you promote these ideals?",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I engage in dialogue, write extensively, and hope to inspire a movement towards equality.")));
                    });
                jobModule.AddOption("Tell me about your writings.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("My most famous works, like 'The Communist Manifesto,' advocate for class struggle and the abolition of private property.")));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think about communism?",
            player => true,
            player =>
            {
                DialogueModule communismModule = new DialogueModule("Communism values the common good above all else. Do you believe in equality?");
                communismModule.AddOption("Yes.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Your answer reveals much about your character. Strive for the greater good, my friend.")));
                    });
                communismModule.AddOption("No.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Then you may find my ideas controversial. Would you like to discuss this further?")));
                        DialogueModule noResponseModule = new DialogueModule("Would you like to discuss this further?");
                        noResponseModule.AddOption("Yes, I want to understand your views.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("I believe in the necessity of a classless society and the communal ownership of production. Such ideas challenge the existing norms.")));
                            });
                        noResponseModule.AddOption("No, I prefer to stay out of politics.",
                            p => true,
                            p => 
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Politics affects us all, but I understand your hesitation. What else would you like to discuss?")));
                            });
                        pl.SendGump(new DialogueGump(pl, noResponseModule));
                    });
                player.SendGump(new DialogueGump(player, communismModule));
            });

        greeting.AddOption("What is philosophy?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Philosophy is the pursuit of wisdom, a means of understanding the world around us. Would you like to learn about a specific branch?")));
                // Nested options
                DialogueModule philosophyModule = new DialogueModule("Would you like to learn about a specific branch?");
                philosophyModule.AddOption("Ethics",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Ethics studies what is morally right and wrong. It challenges us to consider our actions and their consequences.")));
                    });
                philosophyModule.AddOption("Political Philosophy",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Political philosophy examines the role of government, justice, and individual rights. It's essential for a functioning society.")));
                    });
                philosophyModule.AddOption("Epistemology",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Epistemology is the study of knowledge. It questions what we can know and how we come to know it.")));
                    });
                player.SendGump(new DialogueGump(player, philosophyModule));
            });

        greeting.AddOption("What about society?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Society is a complex web. The true health of a society is measured by the well-being of its most vulnerable members.")));
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
                    player.SendGump(new DialogueGump(player, new DialogueModule("Character is the manifestation of one's values and beliefs. Here, take this token as a testament to our conversation.")));
                    player.AddToBackpack(new CarpentryAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            });

        greeting.AddOption("What is the greater good?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Working for the greater good means sometimes setting aside personal desires for the benefit of all. Would you like to discuss how to achieve this?")));
                // Nested options
                DialogueModule greaterGoodModule = new DialogueModule("Would you like to discuss how to achieve this?");
                greaterGoodModule.AddOption("Yes, please elaborate.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("One must prioritize collective interests, educate others, and strive for social justice. It's a noble endeavor.")));
                    });
                greaterGoodModule.AddOption("No, I’m not interested.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("That’s understandable. We all have different paths. What else would you like to talk about?")));
                    });
                player.SendGump(new DialogueGump(player, greaterGoodModule));
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
