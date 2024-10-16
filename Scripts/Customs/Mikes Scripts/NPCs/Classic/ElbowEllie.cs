using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class ElbowEllie : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public ElbowEllie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Elbow Ellie";
        Body = 0x191; // Human female body

        // Stats
        SetStr(95);
        SetDex(60);
        SetInt(70);
        SetHits(95);

        // Appearance
        AddItem(new Kilt() { Hue = 64 });
        AddItem(new Doublet() { Hue = 38 });
        AddItem(new Boots() { Hue = 38 });
        AddItem(new PlateGloves() { Name = "Ellie's Elbow Pads" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        // Speech Hue
        SpeechHue = 0; // Default speech hue

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public ElbowEllie(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Well, well, if it isn't a curious traveler! I'm Elbow Ellie, master of the wrestling ring. What brings you here?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("I am Elbow Ellie, the master of the wrestling ring! I've dedicated my life to wrestling, where honor is not just in winning, but in how we face our battles. Do you fight with honor?");
                aboutModule.AddOption("Yes, I fight with honor.",
                    p => true,
                    p =>
                    {
                        DialogueModule honorModule = new DialogueModule("True honor lies not in the outcome but in the intent. Remember this, traveler.");
                        honorModule.AddOption("Thank you for the wisdom.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, honorModule));
                    });
                aboutModule.AddOption("No, I fight for victory alone.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Elbow Ellie frowns at your response.");
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you tell me about the Britannia Wrestling Championship?",
            player => true,
            player =>
            {
                DialogueModule championshipModule = new DialogueModule("The Britannia Wrestling Championship, you say? Ah, now that's where legends are forged! Every year, the greatest wrestlers gather in the grand arena of Britain to prove their might. It's not just about strength; it's about heart, strategy, and the will to never give up. Are you interested in learning more about it?");
                championshipModule.AddOption("What are the rules of the championship?",
                    p => true,
                    p =>
                    {
                        DialogueModule rulesModule = new DialogueModule("The rules are simple yet demanding. Matches are fought one-on-one, and each wrestler must use their skill and wit to either pin their opponent or force them to submit. There are no weapons allowed—only your body and your mind. The crowd watches closely, and honor is paramount. Breaking the rules results in disqualification. Do you think you have what it takes to compete?");
                        rulesModule.AddOption("Yes, I want to compete!",
                            pl => true,
                            pl =>
                            {
                                DialogueModule competeModule = new DialogueModule("That's the spirit! To enter, you'll need to train hard and prove yourself in the local circuits. Only the best are invited to the Championship. I can help you with training if you're serious about it.");
                                competeModule.AddOption("What kind of training do I need?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule trainingModule = new DialogueModule("Training for the Championship is grueling. You'll need to build your strength, improve your agility, and sharpen your mind. Wrestling isn't just about brute force—it's about anticipating your opponent's moves and reacting swiftly. I recommend starting with endurance exercises and practicing grappling techniques against various opponents.");
                                        trainingModule.AddOption("Can you train me, Ellie?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule trainWithEllieModule = new DialogueModule("Of course, I can train you! We'll start with some basic drills. First, you need to build your stamina. Run around the arena, practice your holds, and make sure you can keep going even when you're exhausted. Only those who can endure will survive in the Championship.");
                                                trainWithEllieModule.AddOption("Thank you, Ellie. I'll start right away!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendMessage("Elbow Ellie nods approvingly as you begin your training.");
                                                    });
                                                plb.SendGump(new DialogueGump(plb, trainWithEllieModule));
                                            });
                                        trainingModule.AddOption("I need to prepare on my own.",
                                            plb => true,
                                            plb =>
                                            {
                                                plb.SendGump(new DialogueGump(plb, CreateGreetingModule()));
                                            });
                                        pla.SendGump(new DialogueGump(pla, trainingModule));
                                    });
                                competeModule.AddOption("Maybe I'm not ready yet.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, competeModule));
                            });
                        rulesModule.AddOption("Not sure if I'm ready for that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rulesModule));
                    });
                championshipModule.AddOption("Who are the previous champions?",
                    p => true,
                    p =>
                    {
                        DialogueModule championsModule = new DialogueModule("Ah, the champions of the Britannia Wrestling Championship! Each one is a legend in their own right. There was Iron Fist Gregor, known for his unbreakable holds, and Swiftfoot Mara, whose speed was unmatched. My greatest rival, Thunderstrike Thomas, also claimed the title once. Each champion had their own unique style and strength. Do you wish to hear more about any of them?");
                        championsModule.AddOption("Tell me about Iron Fist Gregor.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule gregorModule = new DialogueModule("Iron Fist Gregor was a force of nature. His holds were like steel traps—once he had you, there was no escape. He wasn't the fastest, but he didn't need to be. His strength was his greatest asset, and he knew how to use it to wear down his opponents until they had no choice but to submit.");
                                gregorModule.AddOption("Sounds like a true powerhouse.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, gregorModule));
                            });
                        championsModule.AddOption("Tell me about Swiftfoot Mara.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule maraModule = new DialogueModule("Swiftfoot Mara was as quick as a shadow. She danced around her opponents, avoiding their attacks and striking when they least expected it. Her agility was legendary, and many wrestlers found themselves exhausted just trying to keep up with her. She taught me that speed and patience can be just as powerful as brute strength.");
                                maraModule.AddOption("She must have been incredible to watch.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, maraModule));
                            });
                        championsModule.AddOption("Tell me about Thunderstrike Thomas.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule thomasModule = new DialogueModule("Thunderstrike Thomas was my greatest rival. We pushed each other to our limits every time we faced off in the ring. He was known for his explosive power and unpredictable moves. Facing him was like trying to weather a storm—you never knew where the next strike would come from. Our matches were some of the fiercest battles of my career.");
                                thomasModule.AddOption("It must have been an honor to face him.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, thomasModule));
                            });
                        championsModule.AddOption("Maybe another time.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, championsModule));
                    });
                championshipModule.AddOption("What is the prize for winning?",
                    p => true,
                    p =>
                    {
                        DialogueModule prizeModule = new DialogueModule("The prize for winning the Britannia Wrestling Championship is not just gold, though there is a hefty sum involved. The true prize is the Championship Belt, a symbol of honor and prestige. The belt is said to be enchanted, granting the wearer strength and resilience. But more than that, it's the respect of every wrestler in Britannia and the knowledge that you've proven yourself among the best.");
                        prizeModule.AddOption("That sounds incredible.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        prizeModule.AddOption("Maybe one day, I'll claim it.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Elbow Ellie smiles at your determination.");
                            });
                        p.SendGump(new DialogueGump(p, prizeModule));
                    });
                championshipModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, championshipModule));
            });

        greeting.AddOption("Do you have any stories to share?",
            player => true,
            player =>
            {
                DialogueModule storyModule = new DialogueModule("Ah, stories! I have many. From my first match, to my fiercest rivalry. Wrestling has given me memories to cherish. Rivalries are what make the sport exciting. Pushing each other to the limits, challenging, and growing together.");
                storyModule.AddOption("Tell me about your rivalries.",
                    p => true,
                    p =>
                    {
                        DialogueModule rivalryModule = new DialogueModule("Rivalries are what make the sport exciting. Pushing each other to the limits, challenging, and growing together. My greatest rivalry taught me more about myself than any victory ever could.");
                        rivalryModule.AddOption("That sounds inspiring.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, rivalryModule));
                    });
                storyModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, storyModule));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Elbow Ellie nods respectfully as you take your leave.");
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