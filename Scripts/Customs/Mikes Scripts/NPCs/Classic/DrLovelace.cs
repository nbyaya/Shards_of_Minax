using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DrLovelace : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DrLovelace() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dr. Lovelace";
        Body = 0x191; // Human female body

        // Stats
        SetStr(75);
        SetDex(70);
        SetInt(125);

        SetHits(58);
        SetMana(125);
        SetStam(70);

        Fame = 0;
        Karma = 0;
        VirtualArmor = 10;

        // Appearance
        AddItem(new Kilt() { Hue = 1902 });
        AddItem(new FemaleLeatherChest() { Hue = 1155 });
        AddItem(new Shoes() { Hue = 0 });
        AddItem(new Bonnet() { Hue = 1900 });
        AddItem(new Spellbook() { Name = "Dr. Lovelace's Algorithms" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(true); // Female
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

	public DrLovelace(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Dr. Lovelace, a humble scientist and mathematician. How may I assist you today?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Ada Lovelace, known for my work on Charles Babbage's Difference Engine. My life's work has revolved around mathematics and the foundations of computing. It was my desire to prove that numbers can represent more than mere quantities—they could represent abstract concepts, such as patterns and logic.");
                aboutModule.AddOption("What is the Difference Engine?",
                    p => true,
                    p =>
                    {
                        DialogueModule differenceModule = new DialogueModule("The Difference Engine is an early mechanical computer designed by Charles Babbage. It is capable of performing complex calculations automatically. My contributions involved creating the first algorithm intended to be processed by a machine, which many consider to be the first computer program.");
                        differenceModule.AddOption("How did you create the algorithm?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule algorithmModule = new DialogueModule("Creating the algorithm was a challenging task. I had to envision how the machine would interpret sequences of instructions. It was much like composing music—each instruction needed to flow into the next, creating harmony within the mechanical components. It involved breaking down mathematical processes into smaller, manageable steps.");
                                algorithmModule.AddOption("That sounds fascinating. Tell me more.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule detailsModule = new DialogueModule("Indeed, it was! My approach was based on applying loops and conditions—concepts that are fundamental in modern computing. I anticipated that the machine could be used for more than mere calculations. It could analyze any data fed into it, including music or even language, given the right instructions.");
                                        detailsModule.AddOption("What challenges did you face?",
                                            plaa => true,
                                            plaa =>
                                            {
                                                DialogueModule challengesModule = new DialogueModule("There were many challenges, both practical and societal. Mechanically, the technology was ahead of its time, and we faced numerous difficulties in manufacturing the precise gears and components needed. On a societal level, there were doubts regarding whether such a machine could ever be useful or even created. My position as a woman in science was also constantly under scrutiny.");
                                                challengesModule.AddOption("How did you overcome these challenges?",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        DialogueModule overcomeModule = new DialogueModule("I had unwavering support from Charles Babbage, and I was fortunate enough to have a strong belief in the power of knowledge. Persistence was key—I studied every possible mathematical and mechanical facet of the project, spending countless hours refining the algorithm and demonstrating its potential applications. Additionally, I drew upon my love for both mathematics and poetry, combining logic with creativity.");
                                                        overcomeModule.AddOption("Your story is truly inspiring.",
                                                            plaaaa => true,
                                                            plaaaa =>
                                                            {
                                                                plaaaa.SendMessage("Thank you, traveler. Remember, knowledge and imagination can take us beyond what we ever thought possible.");
                                                                plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule()));
                                                            });
                                                        plaaa.SendGump(new DialogueGump(plaaa, overcomeModule));
                                                    });
                                                challengesModule.AddOption("That must have been difficult.",
                                                    plaaa => true,
                                                    plaaa =>
                                                    {
                                                        plaaa.SendGump(new DialogueGump(plaaa, CreateGreetingModule()));
                                                    });
                                                plaa.SendGump(new DialogueGump(plaa, challengesModule));
                                            });
                                        detailsModule.AddOption("Thank you for explaining.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, detailsModule));
                                    });
                                algorithmModule.AddOption("Maybe another time.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, algorithmModule));
                            });
                        differenceModule.AddOption("That sounds like a monumental project.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, differenceModule));
                    });
                aboutModule.AddOption("Tell me about your collaboration with Charles Babbage.",
                    p => true,
                    p =>
                    {
                        DialogueModule collaborationModule = new DialogueModule("Charles Babbage was a brilliant thinker, often called the 'father of the computer.' He designed the Difference Engine and later the Analytical Engine. I worked closely with him, translating and expanding on his notes. My contributions included adding my own thoughts on how the machine could be programmed to carry out a variety of tasks.");
                        collaborationModule.AddOption("What was the Analytical Engine?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule engineModule = new DialogueModule("The Analytical Engine was the more advanced version of the Difference Engine. It had the potential to be a true general-purpose computer, capable of handling conditional statements and loops. My notes contained ideas on how it could be used for purposes beyond calculations, such as creating music or interpreting data.");
                                engineModule.AddOption("Did you believe the Analytical Engine would be built?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule beliefModule = new DialogueModule("I was always hopeful, though I knew the challenges were immense. The mechanical limitations of our time meant that constructing such an advanced machine was difficult. However, I envisioned a future where these machines could revolutionize society, changing how we work, communicate, and even think.");
                                        beliefModule.AddOption("You were truly ahead of your time.",
                                            plaa => true,
                                            plaa =>
                                            {
                                                plaa.SendMessage("Thank you, traveler. Vision is what drives progress. One must dare to dream big.");
                                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, beliefModule));
                                    });
                                engineModule.AddOption("Thank you for sharing.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, engineModule));
                            });
                        collaborationModule.AddOption("What was it like working with him?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule workingModule = new DialogueModule("Working with Babbage was both exhilarating and challenging. His mind was constantly full of new ideas, and I had to match his intensity. We would often spend hours debating mathematical theories and how they could be translated into mechanical action. Our collaboration was a true meeting of minds.");
                                workingModule.AddOption("It must have been exciting.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, workingModule));
                            });
                        p.SendGump(new DialogueGump(p, collaborationModule));
                    });
                aboutModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you teach me about alchemy?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                }
                else
                {
                    DialogueModule alchemyModule = new DialogueModule("Alchemy is the fusion of science and magic. I've created many potions and elixirs. In fact, for someone as inquisitive as you, here's a little gift. May it serve you well.");
                    alchemyModule.AddOption("Thank you!",
                        p => true,
                        p =>
                        {
                            p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, alchemyModule));
                }
            });

        greeting.AddOption("What do you think about discovery?",
            player => true,
            player =>
            {
                DialogueModule discoveryModule = new DialogueModule("The world is filled with mysteries, waiting for eager minds to uncover. Just recently, I stumbled upon an ancient artifact that has baffled many. Discovery is what drives us forward.");
                discoveryModule.AddOption("Tell me more about the artifact.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, discoveryModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Dr. Lovelace nods to you, her eyes filled with curiosity.");
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