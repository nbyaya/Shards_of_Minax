using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class PandaPatty : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public PandaPatty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Panda Patty";
        Body = 0x191; // Human female body

        // Stats
        SetStr(150);
        SetDex(70);
        SetInt(110);
        SetHits(110);

        // Appearance
        AddItem(new BodySash() { Hue = 1173 });
        AddItem(new Kilt() { Hue = 1173 });
        AddItem(new Cloak() { Hue = 1173 });
        AddItem(new Sandals() { Hue = 1173 });
        AddItem(new QuarterStaff() { Name = "Panda Patty's Staff" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0; // Default speech hue

        lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime
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
        DialogueModule greeting = new DialogueModule("Hello there! I am Panda Patty, the animal tamer! I dream of one day joining the Monster Hunters Guild in Serpent's Hold. Do you want to hear more about my aspirations?");

        greeting.AddOption("Yes, tell me about your dream.",
            player => true,
            player =>
            {
                DialogueModule dreamModule = new DialogueModule("Joining the Monster Hunters Guild would allow me to learn how to protect animals from dangerous creatures and perhaps even tame some of them! But I'm not quite skilled enough yet.");
                dreamModule.AddOption("What skills do you need to join?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule skillsModule = new DialogueModule("I need to improve my combat skills, learn more about monster behavior, and become adept at survival in the wild. It's a lot to take on, but I'm determined!");
                        skillsModule.AddOption("What combat skills do you plan to learn?",
                            p => true,
                            p =>
                            {
                                DialogueModule combatModule = new DialogueModule("I hope to master archery and the use of my staff in close combat. I've always admired those who can handle their weapons with grace and precision.");
                                combatModule.AddOption("Do you practice regularly?",
                                    plq => true,
                                    plq =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreatePracticeModule()));
                                    });
                                combatModule.AddOption("Sounds difficult.",
                                    plw => true,
                                    plw =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, combatModule));
                            });
                        skillsModule.AddOption("What do you know about monster behavior?",
                            p => true,
                            p =>
                            {
                                DialogueModule behaviorModule = new DialogueModule("I've studied many books on monsters, particularly their habits and weaknesses. Understanding them is crucial to both taming and battling.");
                                behaviorModule.AddOption("What kinds of monsters have you studied?",
                                    ple => true,
                                    ple =>
                                    {
                                        DialogueModule monstersModule = new DialogueModule("I've learned about the ferocious wyverns, cunning goblins, and even the elusive unicorns! Each has unique traits that make them fascinating to study.");
                                        monstersModule.AddOption("Tell me more about the wyverns.",
                                            pr => true,
                                            pr =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateWyvernModule()));
                                            });
                                        monstersModule.AddOption("Do you think you could tame a unicorn?",
                                            plt => true,
                                            plt =>
                                            {
                                                pl.SendGump(new DialogueGump(pl, CreateUnicornModule()));
                                            });
                                        p.SendGump(new DialogueGump(p, monstersModule));
                                    });
                                p.SendGump(new DialogueGump(p, behaviorModule));
                            });
                        player.SendGump(new DialogueGump(player, skillsModule));
                    });
                dreamModule.AddOption("What does the guild do?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule guildModule = new DialogueModule("The Monster Hunters Guild is dedicated to protecting the land from monstrous threats, training hunters, and conducting research on these creatures. I want to be part of that!");
                        guildModule.AddOption("How can I help you prepare?",
                            p => true,
                            p =>
                            {
                                DialogueModule helpModule = new DialogueModule("I appreciate your offer! If you encounter any monster-related challenges during your adventures, please share your experiences with me. I can learn so much from them!");
                                helpModule.AddOption("I'll keep an eye out for monsters.",
                                    ply => true,
                                    ply =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                    });
                                helpModule.AddOption("Maybe I can help you train?",
                                    plu => true,
                                    plu =>
                                    {
                                        pl.SendGump(new DialogueGump(pl, CreateTrainingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, helpModule));
                            });
                        player.SendGump(new DialogueGump(player, guildModule));
                    });
                player.SendGump(new DialogueGump(player, dreamModule));
            });

        greeting.AddOption("What animals do you train?",
            player => true,
            player =>
            {
                DialogueModule animalsModule = new DialogueModule("I specialize in many creatures, including griffins and pandas! Have you ever seen a griffin?");
                animalsModule.AddOption("Yes, they are magnificent!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                animalsModule.AddOption("No, but I've heard stories.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, animalsModule));
            });

        greeting.AddOption("Why is compassion important to you?",
            player => true,
            player =>
            {
                DialogueModule compassionModule = new DialogueModule("Compassion is the cornerstone of my work. It helps me connect with my animals and understand their needs. Would you agree?");
                compassionModule.AddOption("Absolutely!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                compassionModule.AddOption("Not really.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, compassionModule));
            });

        return greeting;
    }

    private DialogueModule CreatePracticeModule()
    {
        DialogueModule practiceModule = new DialogueModule("I practice daily! Sometimes I train at the archery range, while other times, I spar with my friends. It's all about honing my skills!");
        practiceModule.AddOption("Can I join you for practice?",
            pl => true,
            pl =>
            {
                pl.SendMessage("That would be wonderful! Let's practice together and share techniques.");
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        practiceModule.AddOption("Good luck with your training!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return practiceModule;
    }

    private DialogueModule CreateWyvernModule()
    {
        DialogueModule wyvernModule = new DialogueModule("Wyverns are fearsome creatures with deadly claws and venomous breath. They are often found in the mountains, guarding their nests fiercely. I hope to one day be brave enough to approach one!");
        wyvernModule.AddOption("That sounds dangerous!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return wyvernModule;
    }

    private DialogueModule CreateUnicornModule()
    {
        DialogueModule unicornModule = new DialogueModule("Unicorns are known for their grace and beauty, often seen in enchanted forests. Taming one would require patience and understanding. I dream of befriending such a majestic creature!");
        unicornModule.AddOption("I wish you luck with that!",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return unicornModule;
    }

    private DialogueModule CreateTrainingModule()
    {
        DialogueModule trainingModule = new DialogueModule("Training is essential! I’d love to spar with you and learn new techniques. Together we can improve our combat skills!");
        trainingModule.AddOption("Let's find a training spot.",
            pl => true,
            pl =>
            {
                pl.SendMessage("Fantastic! Follow me to the training grounds.");
                // Implement training session logic here if needed
            });
        trainingModule.AddOption("Maybe another time.",
            pl => true,
            pl =>
            {
                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
            });
        return trainingModule;
    }

    public PandaPatty(Serial serial) : base(serial) { }

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
