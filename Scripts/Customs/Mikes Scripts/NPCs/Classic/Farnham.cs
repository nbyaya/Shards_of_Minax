using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class Farnham : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public Farnham() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Farnham";
        Body = 0x190; // Human male body

        // Stats
        SetStr(70);
        SetDex(30);
        SetInt(30);

        SetHits(40);

        // Appearance
        AddItem(new LongPants() { Hue = 1105 });
        AddItem(new FancyShirt() { Hue = 1102 });
        AddItem(new Boots() { Hue = 0 });
        AddItem(new Dagger() { Name = "Farnham's Drink" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public Farnham(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Ah, greetings traveler! I'm Farnham, the drinker. What brings you to this old fool?");

        greeting.AddOption("What's your name?",
            player => true,
            player =>
            {
                DialogueModule nameModule = new DialogueModule("I'm Farnham, the drinker! A pleasure, I'm sure.");
                nameModule.AddOption("Tell me more about yourself.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateJobModule()));
                    });
                nameModule.AddOption("Goodbye, Farnham.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You take your leave from Farnham.");
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player =>
            {
                player.SendMessage("Farnham mutters, \"Oh, my head...\"");
            });

        greeting.AddOption("What do you do here?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateJobModule()));
            });

        greeting.AddOption("Do you like ale?",
            player => true,
            player =>
            {
                player.SendMessage("Ah, good choice! Ale makes everything better, don't you think?");
            });

        greeting.AddOption("Have you faced dragons?",
            player => true,
            player =>
            {
                player.SendMessage("Have you faced a dragon? They're mighty beasts. Their roar alone could shake the ground. Lost a good friend to one. But at the tavern, tales of dragons are best shared over a drink!");
            });

        greeting.AddOption("Tell me about treasure.",
            player => true,
            player =>
            {
                player.SendMessage("I once found a gleaming chalice in a dungeon. Thought it'd make me rich, but turned out it was just a really fancy cup for my ale!");
            });

        greeting.AddOption("Why do you drink every day?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateTristramStoryModule()));
            });

        greeting.AddOption("Do you have headaches?",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, CreateHeadacheModule()));
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendMessage("Farnham nods slowly, lost in thought.");
            });

        return greeting;
    }

    private DialogueModule CreateJobModule()
    {
        DialogueModule jobModule = new DialogueModule("I used to be an adventurer like you... until I discovered the ale!");
        jobModule.AddOption("Tell me about your adventures.",
            player => true,
            player =>
            {
                DialogueModule adventureModule = new DialogueModule("Ah, those were the days! Battling dragons, finding treasures, and meeting other brave souls. But ale... it's a different kind of adventure.");
                adventureModule.AddOption("Why did you stop adventuring?",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateTristramStoryModule()));
                    });
                adventureModule.AddOption("Goodbye, Farnham.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You take your leave from Farnham.");
                    });
                player.SendGump(new DialogueGump(player, adventureModule));
            });
        jobModule.AddOption("Goodbye, Farnham.",
            player => true,
            player =>
            {
                player.SendMessage("You take your leave from Farnham.");
            });

        return jobModule;
    }

    private DialogueModule CreateHeadacheModule()
    {
        DialogueModule headacheModule = new DialogueModule("You wouldn't happen to know a good remedy for these constant headaches, would you?");
        headacheModule.AddOption("Have you tried herbs?",
            player => true,
            player =>
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    player.SendMessage("I have no reward right now. Please return later.");
                }
                else
                {
                    player.SendMessage("Ah, herbs! I've tried some. Mixed them with ale once. Didn't help the headache, but it sure made the ale interesting!");
                    player.AddToBackpack(new Gold(100));
                    lastRewardTime = DateTime.UtcNow;
                }
            });
        headacheModule.AddOption("I don't know any remedies.",
            player => true,
            player =>
            {
                player.SendMessage("Farnham sighs heavily, his headaches persist.");
            });
        return headacheModule;
    }

    private DialogueModule CreateTristramStoryModule()
    {
        DialogueModule tristramStoryModule = new DialogueModule("Ah... Tristram. It used to be a peaceful town, you know. But everything changed when the darkness came. I lost everything—my friends, my family. The demons took it all. And now... all I have left is the ale to drown the memories.");
        tristramStoryModule.AddOption("What happened to your family?",
            player => true,
            player =>
            {
                DialogueModule familyModule = new DialogueModule("My wife, Eliza... she was the kindest soul you'd ever meet. And my children... they were just innocent. When the demons came, I tried to fight, but I was powerless. They were taken from me. I still see their faces in my dreams, and every day I drink to forget that nightmare.");
                familyModule.AddOption("I'm so sorry for your loss.",
                    p => true,
                    p =>
                    {
                        DialogueModule sympathyModule = new DialogueModule("Thank you, traveler. It's kind of you to say. But no words can ever fill the void. Only the ale can keep the memories at bay, even if just for a short while.");
                        sympathyModule.AddOption("Is there anything I can do to help?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule helpModule = new DialogueModule("Unless you can turn back time and stop the darkness from taking my family, there's little that can be done. But I appreciate the sentiment. Just sharing a drink with an old fool like me is more help than you know.");
                                helpModule.AddOption("I'll have a drink with you, Farnham.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You sit down and share a drink with Farnham. For a moment, the burdens of the past seem to lift.");
                                    });
                                helpModule.AddOption("I wish you peace, Farnham.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Farnham nods, his eyes misty with gratitude.");
                                    });
                                pl.SendGump(new DialogueGump(pl, helpModule));
                            });
                        sympathyModule.AddOption("Goodbye, Farnham.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("You take your leave, feeling the weight of Farnham's sorrow.");
                            });
                        p.SendGump(new DialogueGump(p, sympathyModule));
                    });
                familyModule.AddOption("That must be why you drink every day.",
                    p => true,
                    p =>
                    {
                        DialogueModule drinkReasonModule = new DialogueModule("Aye, that it is. The ale numbs the pain. Every bottle takes away a piece of the darkness, even if just for a short while. It's not much, but it's all I have left.");
                        drinkReasonModule.AddOption("What kind of ales do you like?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule aleModule = new DialogueModule("Ah, you ask the right questions! My favorite? There's a rich red wine from the eastern hills—smooth, with a hint of cherry. Then there's the golden ale from the village of Wortham, crisp and refreshing. But if I had to choose... it would be the deep, dark stout from the Black Mountain Brewery. It's as heavy as my heart, and just as dark.");
                                aleModule.AddOption("I would love to try those someday.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("Farnham smiles, his eyes lighting up for a brief moment. \"If you ever do, have a drink for me, will you?\"");
                                    });
                                aleModule.AddOption("Goodbye, Farnham.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendMessage("You take your leave, feeling a deeper understanding of Farnham's sorrow.");
                                    });
                                pl.SendGump(new DialogueGump(pl, aleModule));
                            });
                        drinkReasonModule.AddOption("I hope you find peace.",
                            pl => true,
                            pl =>
                            {
                                pl.SendMessage("Farnham sighs, staring into the distance. \"Peace... maybe one day.\"");
                            });
                        p.SendGump(new DialogueGump(p, drinkReasonModule));
                    });
                player.SendGump(new DialogueGump(player, familyModule));
            });
        tristramStoryModule.AddOption("What do you remember most about Tristram?",
            player => true,
            player =>
            {
                DialogueModule memoryModule = new DialogueModule("The laughter. The market square filled with people. The children playing near the fountain. It was a beautiful place, full of life. I remember the smell of fresh bread from Sara's bakery, the sound of the blacksmith's hammer. Those memories... they're what I hold onto when the nightmares come.");
                memoryModule.AddOption("Those sound like wonderful memories.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Farnham smiles faintly. \"Aye, they were. Before everything fell apart. Before the darkness took it all away.\"");
                    });
                memoryModule.AddOption("Goodbye, Farnham.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("You take your leave, the echoes of a lost Tristram lingering in your mind.");
                    });
                player.SendGump(new DialogueGump(player, memoryModule));
            });

        tristramStoryModule.AddOption("Goodbye, Farnham.",
            player => true,
            player =>
            {
                player.SendMessage("Farnham nods slowly, lost in thought.");
            });

        return tristramStoryModule;
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