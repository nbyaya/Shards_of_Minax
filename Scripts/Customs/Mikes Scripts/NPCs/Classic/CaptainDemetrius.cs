using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class CaptainDemetrius : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public CaptainDemetrius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Captain Demetrius";
        Body = 0x190; // Human male body

        // Stats
        SetStr(110);
        SetDex(100);
        SetInt(70);
        SetHits(80);

        // Appearance
        AddItem(new LongPants() { Hue = 1152 });
        AddItem(new FancyShirt() { Hue = 1152 });
        AddItem(new TricorneHat() { Hue = 1152 });
        AddItem(new Boots() { Hue = 1109 });
        AddItem(new Cutlass() { Name = "Demetrius' Cutlass" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
        lastRewardTime = DateTime.MinValue;
    }

    public CaptainDemetrius(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("What do you want, stranger? I'm Captain Demetrius, but I'm not one for idle chit-chat. State your business.");

        greeting.AddOption("Who are you?",
            player => true,
            player =>
            {
                DialogueModule identityModule = new DialogueModule("Captain Demetrius is the name. Many have heard of me, but few know my true story.");
                identityModule.AddOption("Tell me your story.",
                    p => true,
                    p =>
                    {
                        DialogueModule storyModule = new DialogueModule("I once was a mere soldier myself, climbing up the ranks. Then, a fateful event at Frostpeak changed everything.");
                        storyModule.AddOption("What happened at Frostpeak?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule frostpeakModule = new DialogueModule("Frostpeak... it was a place where bravery was tested and lives were lost. It was the dead of winter, the winds howling like wolves, and the snow blinding our every step.");
                                frostpeakModule.AddOption("Tell me more about the battle itself.",
                                    pl2 => true,
                                    pl2 =>
                                    {
                                        DialogueModule battleModule = new DialogueModule("The battle began at dawn. We were outnumbered, facing an enemy who knew the terrain better than we did. The enemy was relentless, striking from the shadows, using the blizzard to their advantage. We had to fight inch by inch, not only against them but also against the biting cold.");
                                        battleModule.AddOption("What was the enemy like?",
                                            pl3 => true,
                                            pl3 =>
                                            {
                                                DialogueModule enemyModule = new DialogueModule("They were not ordinary foes. They were the Frostpeak Marauders, a group of merciless raiders who thrived in the harshest of climates. They had mastered the art of guerrilla warfare, striking quickly and disappearing into the snow. We heard tales of their leader, a man named Skorn, who was said to have no mercy.");
                                                enemyModule.AddOption("Who was Skorn?",
                                                    pl4 => true,
                                                    pl4 =>
                                                    {
                                                        DialogueModule skornModule = new DialogueModule("Skorn was more than just a leader; he was a symbol of fear. They said he was born in the heart of a blizzard, his heart as cold as the ice that surrounded him. He commanded his men with an iron fist, and his presence on the battlefield was like that of a specter. He was ruthless, and many of my comrades fell to his blade.");
                                                        skornModule.AddOption("Did you face Skorn yourself?",
                                                            pl5 => true,
                                                            pl5 =>
                                                            {
                                                                DialogueModule faceSkornModule = new DialogueModule("Yes, I faced Skorn. It was during the final moments of the battle. We had pushed them back to the edge of a cliff. The wind was fierce, and the snow made it hard to see, but I remember the cold steel of his blade as we clashed. It was as if time had slowed. Every strike, every parry, felt like an eternity. Skorn was powerful, but he underestimated my resolve.");
                                                                faceSkornModule.AddOption("How did you defeat him?",
                                                                    pl6 => true,
                                                                    pl6 =>
                                                                    {
                                                                        DialogueModule defeatSkornModule = new DialogueModule("Defeating Skorn wasn't about strength; it was about endurance and finding the right moment. He lunged at me, and I used his momentum against him. I sidestepped, and with the last of my strength, I pushed him. He stumbled, losing his footing on the icy ground, and fell into the abyss below. It wasn't a glorious victory, but it was enough to end the battle.");
                                                                        defeatSkornModule.AddOption("What happened after that?",
                                                                            pl7 => true,
                                                                            pl7 =>
                                                                            {
                                                                                DialogueModule aftermathModule = new DialogueModule("After Skorn fell, the Marauders lost their will to fight. We managed to secure Frostpeak, but at a great cost. Many of my comrades lay in the snow, their bodies frozen where they fell. I was given a medal for my bravery, but to this day, I wonder if it was worth it. The price of victory was too high.");
                                                                                aftermathModule.AddOption("How did this change you?",
                                                                                    pl8 => true,
                                                                                    pl8 =>
                                                                                    {
                                                                                        DialogueModule changeModule = new DialogueModule("Frostpeak changed me in ways I couldn't have imagined. The horrors I witnessed, the friends I lost... it made me realize the true cost of leadership. It's not about glory or medals; it's about the people who stand by you, and the burden of their lives on your shoulders. Since that day, I've vowed to protect those under my command, no matter the cost.");
                                                                                        changeModule.AddOption("Thank you for sharing your story.",
                                                                                            pl9 => true,
                                                                                            pl9 =>
                                                                                            {
                                                                                                pl9.SendGump(new DialogueGump(pl9, CreateGreetingModule()));
                                                                                            });
                                                                                        pl8.SendGump(new DialogueGump(pl8, changeModule));
                                                                                    });
                                                                                aftermathModule.AddOption("I can't imagine the pain you went through.",
                                                                                    pl8 => true,
                                                                                    pl8 =>
                                                                                    {
                                                                                        pl8.SendGump(new DialogueGump(pl8, CreateGreetingModule()));
                                                                                    });
                                                                                pl7.SendGump(new DialogueGump(pl7, aftermathModule));
                                                                            });
                                                                        defeatSkornModule.AddOption("It sounds like a tough fight.",
                                                                            pl7 => true,
                                                                            pl7 =>
                                                                            {
                                                                                pl7.SendGump(new DialogueGump(pl7, CreateGreetingModule()));
                                                                            });
                                                                        pl6.SendGump(new DialogueGump(pl6, defeatSkornModule));
                                                                    });
                                                                faceSkornModule.AddOption("You were very brave.",
                                                                    pl6 => true,
                                                                    pl6 =>
                                                                    {
                                                                        pl6.SendGump(new DialogueGump(pl6, CreateGreetingModule()));
                                                                    });
                                                                pl5.SendGump(new DialogueGump(pl5, faceSkornModule));
                                                            });
                                                        skornModule.AddOption("He sounds terrifying.",
                                                            pl5 => true,
                                                            pl5 =>
                                                            {
                                                                pl5.SendGump(new DialogueGump(pl5, CreateGreetingModule()));
                                                            });
                                                        pl4.SendGump(new DialogueGump(pl4, skornModule));
                                                    });
                                                enemyModule.AddOption("How did you survive such odds?",
                                                    pl4 => true,
                                                    pl4 =>
                                                    {
                                                        DialogueModule survivalModule = new DialogueModule("Survival wasn't guaranteed. It took every ounce of strength, every bit of cunning, and a great deal of luck. We had to make the terrain work for us, using every rock and snowbank as cover. There were moments when I thought we wouldn't make it, but we pressed on.");
                                                        survivalModule.AddOption("You must have been very determined.",
                                                            pl5 => true,
                                                            pl5 =>
                                                            {
                                                                pl5.SendGump(new DialogueGump(pl5, CreateGreetingModule()));
                                                            });
                                                        pl4.SendGump(new DialogueGump(pl4, survivalModule));
                                                    });
                                                pl3.SendGump(new DialogueGump(pl3, enemyModule));
                                            });
                                        battleModule.AddOption("What kept you going?",
                                            pl3 => true,
                                            pl3 =>
                                            {
                                                DialogueModule motivationModule = new DialogueModule("It was the thought of my comrades, the men and women who were counting on me. I couldn't let them down. I also thought of my family, and the promise I made to come back alive. In those moments, you find strength you never knew you had.");
                                                motivationModule.AddOption("Your resolve is inspiring.",
                                                    pl4 => true,
                                                    pl4 =>
                                                    {
                                                        pl4.SendGump(new DialogueGump(pl4, CreateGreetingModule()));
                                                    });
                                                pl3.SendGump(new DialogueGump(pl3, motivationModule));
                                            });
                                        pl2.SendGump(new DialogueGump(pl2, battleModule));
                                    });
                                frostpeakModule.AddOption("That must have been horrifying.",
                                    pl2 => true,
                                    pl2 =>
                                    {
                                        pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, frostpeakModule));
                            });
                        storyModule.AddOption("That's enough for now.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, storyModule));
                    });
                identityModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, identityModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My job? Hah! I'm a glorified babysitter for a bunch of incompetent soldiers. It's not just about handling soldiers. It's about protecting this city, its people, and its secrets.");
                jobModule.AddOption("What secrets?",
                    p => true,
                    p =>
                    {
                        DialogueModule secretsModule = new DialogueModule("The city has many tales, but there's one artifact, the Emerald Compass, that very few speak of.");
                        secretsModule.AddOption("Tell me about the Emerald Compass.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule compassModule = new DialogueModule("Legend says that this compass points to a treasure beyond imagination. But many have sought it and failed.");
                                compassModule.AddOption("Sounds intriguing.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, compassModule));
                            });
                        secretsModule.AddOption("Maybe later.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, secretsModule));
                    });
                jobModule.AddOption("I see.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Why are you so bitter?",
            player => true,
            player =>
            {
                DialogueModule bitternessModule = new DialogueModule("While you may think I'm just grumbling, the life of a Captain isn't as glorious as it seems. The scars run deeper than the skin.");
                bitternessModule.AddOption("Tell me about your scars.",
                    p => true,
                    p =>
                    {
                        DialogueModule scarsModule = new DialogueModule("These scars are reminders of the battles I've fought, both external and internal. Some wounds, you see, never truly heal.");
                        scarsModule.AddOption("I'm sorry to hear that.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, scarsModule));
                    });
                bitternessModule.AddOption("I didn't mean to pry.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, bitternessModule));
            });

        greeting.AddOption("Goodbye, Captain.",
            player => true,
            player =>
            {
                player.SendMessage("Captain Demetrius nods at you dismissively.");
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