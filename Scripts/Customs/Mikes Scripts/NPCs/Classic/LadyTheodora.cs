using System;
using Server;
using Server.Mobiles;
using Server.Items;

[CorpseName("the corpse of Lady Theodora")]
public class LadyTheodora : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public LadyTheodora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Lady Theodora";
        Body = 0x191; // Human female body

        // Stats
        Str = 70;
        Dex = 60;
        Int = 100;
        Hits = 60;

        // Appearance
        AddItem(new FancyDress() { Hue = 1133 });
        AddItem(new Boots() { Hue = 1109 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        SpeechHue = 0; // Default speech hue

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
        DialogueModule greeting = new DialogueModule("Oh, it's you. What do you want, mortal?");

        greeting.AddOption("Who are you?",
            player => true,
            player => 
            {
                DialogueModule nameModule = new DialogueModule("Lady Theodora, a name whispered in the shadows and feared by many. Why do you seek me?");
                nameModule.AddOption("What makes you feared?",
                    pl => true,
                    pl => 
                    {
                        DialogueModule fearedModule = new DialogueModule("I have seen many things, mortal. Secrets that could unravel the fabric of this realm. My past is a tapestry of dark choices and lost love.");
                        fearedModule.AddOption("Tell me about your choices.",
                            p => true,
                            p =>
                            {
                                DialogueModule choicesModule = new DialogueModule("There are choices that haunt me. Each decision carved a path, but some led to sorrow. Love lost, friendships shattered.");
                                choicesModule.AddOption("What happened to your love?",
                                    plq => true,
                                    plq =>
                                    {
                                        DialogueModule loveModule = new DialogueModule("Ah, my heart was entwined with one whose name still stings. A love that was both my salvation and my curse. Do you believe in love, mortal?");
                                        loveModule.AddOption("I do.",
                                            pw => true,
                                            pw =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                                            });
                                        loveModule.AddOption("Love is folly.",
                                            pe => true,
                                            pe =>
                                            {
                                                DialogueModule follyModule = new DialogueModule("Perhaps. Yet, it gives life a certain thrill, does it not? A dance of joy and sorrow.");
                                                follyModule.AddOption("It does indeed.",
                                                    plr => true,
                                                    plr => 
                                                    {
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to greeting
                                                    });
                                                follyModule.AddOption("I'd rather avoid such entanglements.",
                                                    plt => true,
                                                    plt => 
                                                    {
                                                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to greeting
                                                    });
                                                p.SendGump(new DialogueGump(p, follyModule));
                                            });
                                        p.SendGump(new DialogueGump(p, loveModule));
                                    });
                                choicesModule.AddOption("What do you regret?",
                                    py => true,
                                    py =>
                                    {
                                        DialogueModule regretModule = new DialogueModule("Regrets are chains that bind our souls. I regret the paths I did not take, the words I did not say.");
                                        regretModule.AddOption("What paths?",
                                            plu => true,
                                            plu =>
                                            {
                                                DialogueModule pathsModule = new DialogueModule("Paths to redemption, to forgiveness. Choices that could have led to a brighter future.");
                                                pathsModule.AddOption("Is it too late?",
                                                    pi => true,
                                                    pi =>
                                                    {
                                                        DialogueModule lateModule = new DialogueModule("Never too late, yet each passing moment dims the chance. What about you? Do you have regrets?");
                                                        lateModule.AddOption("I have a few.",
                                                            plo => true,
                                                            plo => 
                                                            {
                                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to greeting
                                                            });
                                                        lateModule.AddOption("No, I live without regrets.",
                                                            plp => true,
                                                            plp => 
                                                            {
                                                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to greeting
                                                            });
                                                        p.SendGump(new DialogueGump(p, lateModule));
                                                    });
                                                p.SendGump(new DialogueGump(p, pathsModule));
                                            });
                                        p.SendGump(new DialogueGump(p, regretModule));
                                    });
                                p.SendGump(new DialogueGump(p, choicesModule));
                            });
                        pl.SendGump(new DialogueGump(pl, fearedModule));
                    });
                player.SendGump(new DialogueGump(player, nameModule));
            });

        greeting.AddOption("How's your health?",
            player => true,
            player => 
            {
                DialogueModule healthModule = new DialogueModule("Why do you care about my well-being? It's not like anyone else does.");
                healthModule.AddOption("I care because...",
                    pl => true,
                    pl =>
                    {
                        DialogueModule careModule = new DialogueModule("Such compassion is rare. Perhaps the world isn't as dark as I thought.");
                        careModule.AddOption("We all need someone.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        careModule.AddOption("It's just curiosity.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        pl.SendGump(new DialogueGump(pl, careModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What do you do?",
            player => true,
            player => 
            {
                DialogueModule jobModule = new DialogueModule("My \"job,\" if you must know, is to endure the endless tedium of existence.");
                jobModule.AddOption("Is it really so tedious?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tediousModule = new DialogueModule("Oh, yes. Each day blends into the next, a mere shadow of the last. Yet, I find solace in the small joys.");
                        tediousModule.AddOption("What small joys?",
                            p => true,
                            p =>
                            {
                                DialogueModule joyModule = new DialogueModule("The beauty of a sunset, the laughter of a child, the thrill of an adventure. They remind me of life's fleeting nature.");
                                joyModule.AddOption("Do you seek adventure?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule adventureModule = new DialogueModule("Sometimes. But I have grown wary. Adventure brings danger, and danger brings pain.");
                                        adventureModule.AddOption("Pain can be worth it for a good story.",
                                            ps => true,
                                            ps =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                                            });
                                        adventureModule.AddOption("I understand your caution.",
                                            pd => true,
                                            pd =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                                            });
                                        pl.SendGump(new DialogueGump(pl, adventureModule));
                                    });
                                p.SendGump(new DialogueGump(p, joyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, tediousModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("Tell me about battles.",
            player => true,
            player => 
            {
                DialogueModule battlesModule = new DialogueModule("Valor? Ha! What does valor matter in a world that has abandoned me?");
                battlesModule.AddOption("What do you mean by that?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule meaningModule = new DialogueModule("Valor is meaningless if it isn't recognized. I've fought many battles, yet few remember my name.");
                        meaningModule.AddOption("What battles have you fought?",
                            p => true,
                            p =>
                            {
                                DialogueModule foughtModule = new DialogueModule("Battles of heart and mind. Each conflict leaves scars that linger far longer than the physical wounds.");
                                foughtModule.AddOption("Scars tell stories.",
                                    plf => true,
                                    plf =>
                                    {
                                        DialogueModule scarsModule = new DialogueModule("Indeed. My scars whisper tales of lost loves, betrayal, and the darkness that lurks in the hearts of men.");
                                        scarsModule.AddOption("What darkness do you speak of?",
                                            pg => true,
                                            pg =>
                                            {
                                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                                            });
                                        pl.SendGump(new DialogueGump(pl, scarsModule));
                                    });
                                p.SendGump(new DialogueGump(p, foughtModule));
                            });
                        pl.SendGump(new DialogueGump(pl, meaningModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("Do you have any secrets?",
            player => true,
            player => 
            {
                DialogueModule secretModule = new DialogueModule("Not all secrets should be told, but I sense a genuine curiosity in you. There's an old shrine hidden in the mountains. Seek it if you dare.");
                secretModule.AddOption("What shrine?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule shrineModule = new DialogueModule("It is said to be a place of great power. Many who seek it are never seen again. Are you brave enough to pursue it?");
                        shrineModule.AddOption("I am brave!",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        shrineModule.AddOption("Perhaps I should think twice.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        pl.SendGump(new DialogueGump(pl, shrineModule));
                    });
                player.SendGump(new DialogueGump(player, secretModule));
            });

        greeting.AddOption("Do you grant rewards?",
            player => true,
            player => 
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                    player.SendGump(new DialogueGump(player, noRewardModule));
                }
                else
                {
                    player.AddToBackpack(new MaxxiaScroll());
                    lastRewardTime = DateTime.UtcNow;
                    DialogueModule rewardModule = new DialogueModule("For your bravery in asking, I grant you this reward. Use it wisely.");
                    rewardModule.AddOption("Thank you!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); // Return to greeting
                        });
                    player.SendGump(new DialogueGump(player, rewardModule));
                }
            });

        greeting.AddOption("What haunts you?",
            player => true,
            player => 
            {
                DialogueModule hauntModule = new DialogueModule("Being haunted is not just about ghosts, it's about memories and regrets. Do you have regrets?");
                hauntModule.AddOption("I have a few.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule myRegretsModule = new DialogueModule("Regrets are chains that bind our souls. Tell me, what weighs heavily on your heart?");
                        myRegretsModule.AddOption("I regret lost opportunities.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        myRegretsModule.AddOption("I regret not pursuing my dreams.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        pl.SendGump(new DialogueGump(pl, myRegretsModule));
                    });
                hauntModule.AddOption("I have no regrets.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule noRegretsModule = new DialogueModule("A bold stance! Yet, one must wonder if you've truly lived if you have no shadows to accompany your light.");
                        noRegretsModule.AddOption("I live fully, without fear.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        noRegretsModule.AddOption("Fear is a part of life.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Return to greeting
                            });
                        pl.SendGump(new DialogueGump(pl, noRegretsModule));
                    });
                player.SendGump(new DialogueGump(player, hauntModule));
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

    public LadyTheodora(Serial serial) : base(serial) { }
}
