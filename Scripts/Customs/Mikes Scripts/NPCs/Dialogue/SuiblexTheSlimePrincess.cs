using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

public class SuiblexTheSlimePrincess : BaseCreature
{
    [Constructable]
    public SuiblexTheSlimePrincess() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Suiblex the Slime Princess";
        Body = 207; // Slime body
        Hue = 1152; // Light green hue

        SetStr(60);
        SetDex(50);
        SetInt(80);

        SetHits(100);
        SetMana(100);
        SetStam(70);

        Fame = 0;
        Karma = 0;

        VirtualArmor = 15;

        Frozen = true; // Prevent NPC from moving
        CantWalk = true;
    }

    public SuiblexTheSlimePrincess(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Oh, hello there! I’m Suiblex, the Slime Princess! I’ve fled from my realm to escape my father’s suffocating expectations. It’s so nice to meet someone from outside my sticky world. How can I help you today?");

        greeting.AddOption("What made you run away?",
            player => true,
            player =>
            {
                DialogueModule escapeModule = new DialogueModule("You see, my father, King Glopp, wants me to rule the Slime Kingdom. But all I desire is to explore the world, to taste its wonders beyond the gelatinous borders of my realm. I felt trapped, like a bubble waiting to burst!");
                escapeModule.AddOption("That sounds tough. What will you do now?",
                    p => true,
                    p =>
                    {
                        DialogueModule futureModule = new DialogueModule("I want to learn about the world! There are so many things I’ve never seen. Perhaps I could become an explorer or a storyteller! But I need guidance. Do you have any advice for me?");
                        futureModule.AddOption("Follow your heart; it's your life to live.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        futureModule.AddOption("You should consider going back.",
                            pl => true,
                            pl =>
                            {
                                DialogueModule backModule = new DialogueModule("Return? But then I’d be stuck! I wish to experience freedom, not to feel like a puppet on a string! Maybe I’ll find a way to balance both.");
                                backModule.AddOption("I understand. It's a tough decision.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, backModule));
                            });
                        p.SendGump(new DialogueGump(p, futureModule));
                    });
                player.SendGump(new DialogueGump(player, escapeModule));
            });

        greeting.AddOption("What is it like in the Slime Realm?",
            player => true,
            player =>
            {
                DialogueModule realmModule = new DialogueModule("Oh, it’s a gooey place filled with bouncing slimes of all sizes! We live in gelatinous palaces, and the landscape is a shimmering sea of colors. The air smells sweet like syrup, but the atmosphere can feel stifling. It can be fun, but it's also very... repetitive. I wanted to see more of the world!");
                realmModule.AddOption("Sounds like an interesting place.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                realmModule.AddOption("Did you ever like living there?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule likeModule = new DialogueModule("At times, yes! The festivals are wonderful—filled with games and laughter! But the pressure to conform is overwhelming. I wanted to explore art and adventure, not just be a princess in a castle of slime.");
                        likeModule.AddOption("Art and adventure sound amazing!",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, likeModule));
                    });
                player.SendGump(new DialogueGump(player, realmModule));
            });

        greeting.AddOption("Do you need help with anything?",
            player => true,
            player =>
            {
                DialogueModule helpModule = new DialogueModule("That’s so kind of you! I’m searching for a special gemstone called the Prism Crystal. It’s said to be hidden in the Caverns of Reflection. If I can find it, I might be able to harness its power to bring happiness to my people.");
                helpModule.AddOption("I can help you find it.",
                    p => true,
                    p =>
                    {
                        DialogueModule questModule = new DialogueModule("Oh, would you? That would mean so much! The Caverns can be perilous, filled with illusions and tricky creatures. Are you ready for such an adventure?");
                        questModule.AddOption("I am ready! Let's go!",
                            pla => true,
                            pla =>
                            {
                                pla.SendMessage("You prepare to embark on the quest for the Prism Crystal.");
                                // Here you can add code to initiate the quest
                            });
                        questModule.AddOption("Maybe another time; it sounds risky.",
                            pla => true,
                            pla =>
                            {
                                pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, questModule));
                    });
                helpModule.AddOption("What is the Prism Crystal?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule crystalModule = new DialogueModule("The Prism Crystal is a magnificent gem that can amplify joy and creativity. It can even help unify different realms! I hope to bring it back to my home and inspire my people.");
                        crystalModule.AddOption("That sounds magical.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        crystalModule.AddOption("What if you can’t find it?",
                            p => true,
                            p =>
                            {
                                DialogueModule worryModule = new DialogueModule("If I can’t find it... then I’ll have to think of another way to help my people. But I refuse to give up hope! I’ve come too far for that.");
                                worryModule.AddOption("You have the heart of a true adventurer.",
                                    pla => true,
                                    pla =>
                                    {
                                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                    });
                                p.SendGump(new DialogueGump(p, worryModule));
                            });
                        player.SendGump(new DialogueGump(player, crystalModule));
                    });
                player.SendGump(new DialogueGump(player, helpModule));
            });

        greeting.AddOption("What do you miss about your home?",
            player => true,
            player =>
            {
                DialogueModule missModule = new DialogueModule("I miss my friends! They were always so bubbly and joyful. We would play games and bounce around the gelatin pools. But I also wanted to see if there’s more to life than just that.");
                missModule.AddOption("What games did you play?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule gamesModule = new DialogueModule("We had this fantastic game called 'Slimeball,' where we’d roll around and try to bounce a gelatin ball into a goal! It was so exhilarating! But I often lost, since I was too busy dreaming of adventure.");
                        gamesModule.AddOption("Sounds like fun! You should play again sometime.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, gamesModule));
                    });
                missModule.AddOption("You seem brave for exploring the unknown.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, missModule));
            });

        greeting.AddOption("Have you learned anything from your journey?",
            player => true,
            player =>
            {
                DialogueModule lessonModule = new DialogueModule("Oh, yes! I've learned that the world is vast and beautiful, but also unpredictable. I've met so many interesting beings—each with their own stories and dreams. It’s taught me the value of friendship and courage.");
                lessonModule.AddOption("Friendship is important; it can guide you.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                    });
                lessonModule.AddOption("What’s the most memorable encounter?",
                    p => true,
                    p =>
                    {
                        DialogueModule encounterModule = new DialogueModule("There was this wise old tree that could talk! It shared tales of the past and whispered secrets about the forest. I felt so connected to nature in that moment. It made me realize how small my worries really are.");
                        encounterModule.AddOption("That sounds enchanting! I'd love to meet it.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        p.SendGump(new DialogueGump(p, encounterModule));
                    });
                player.SendGump(new DialogueGump(player, lessonModule));
            });

        greeting.AddOption("Good luck on your journey.",
            player => true,
            player =>
            {
                player.SendMessage("Suiblex beams with gratitude, her gooey form shimmering with hope. 'Thank you! Your kindness gives me strength to keep going!'");
            });

        return greeting;
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
