using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

[CorpseName("the corpse of Tempting Tina")]
public class TemptingTina : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public TemptingTina() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Tempting Tina";
        Body = 0x191; // Human female body

        // Stats
        SetStr(88);
        SetDex(72);
        SetInt(53);
        SetHits(63);

        // Appearance
        AddItem(new FancyDress() { Hue = 2962 });
        AddItem(new Boots() { Hue = 2963 });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

        lastRewardTime = DateTime.MinValue;
    }

    public TemptingTina(Serial serial) : base(serial) { }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule();
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule()
    {
        DialogueModule greeting = new DialogueModule("Greetings, darling. I am Tempting Tina, your heart's desire.");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutMe = new DialogueModule("Oh, my dear, I grew up as an orphan on the streets of a bustling city. My childhood was filled with shadows and whispers. Survival was my only art form.");
                aboutMe.AddOption("What was your childhood like?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule childhoodModule = new DialogueModule("It was a life of hardship. I scavenged for food and found solace in the company of other street children. We shared stories and dreams, but dreams often fade in the harsh light of reality.");
                        childhoodModule.AddOption("Did you have any friends?",
                            p => true,
                            p =>
                            {
                                DialogueModule friendsModule = new DialogueModule("Yes, there was a girl named Lila. She was my closest friend. We promised to look after each other. But one day, she disappeared, and I was left alone once more.");
                                friendsModule.AddOption("That sounds heartbreaking.",
                                    plq => true,
                                    plq => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                friendsModule.AddOption("What happened to her?",
                                    plw => true,
                                    plw =>
                                    {
                                        DialogueModule fateModule = new DialogueModule("I heard rumors that she was taken in by a nobleman. Some say she became a lady in waiting, living a life I could only dream of. I often wonder if she remembers me.");
                                        fateModule.AddOption("I hope you find her one day.",
                                            pla => true,
                                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                        fateModule.AddOption("It's better to move on.",
                                            pla => true,
                                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                        p.SendGump(new DialogueGump(p, fateModule));
                                    });
                                p.SendGump(new DialogueGump(p, friendsModule));
                            });
                        childhoodModule.AddOption("What did you learn from that time?",
                            ple => true,
                            ple =>
                            {
                                DialogueModule lessonsModule = new DialogueModule("I learned that love is fragile and that survival often comes at a cost. I vowed to find my own way in the world, no matter the obstacles.");
                                lessonsModule.AddOption("How did you do that?",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                ple.SendGump(new DialogueGump(ple, lessonsModule));
                            });
                        player.SendGump(new DialogueGump(player, childhoodModule));
                    });
                player.SendGump(new DialogueGump(player, aboutMe));
            });

        greeting.AddOption("How did you become a courtesan?",
            player => true,
            player =>
            {
                DialogueModule courtesanModule = new DialogueModule("Ah, the path to becoming a courtesan was paved with both pain and ambition. I learned the ways of charm and allure, using my beauty as a shield against a harsh world.");
                courtesanModule.AddOption("Who taught you these ways?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule mentorModule = new DialogueModule("There was a kind woman named Madame Esmeralda. She took me under her wing when I was just a girl. She saw potential in me that I never knew I had.");
                        mentorModule.AddOption("What did she teach you?",
                            p => true,
                            p =>
                            {
                                DialogueModule lessonsModule = new DialogueModule("She taught me the art of conversation, how to read a man's desires, and how to captivate an audience. I learned to weave fantasies that would make even the coldest heart warm.");
                                lessonsModule.AddOption("That sounds magical.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                lessonsModule.AddOption("Is that all there is to it?",
                                    pla => true,
                                    pla => 
                                    {
                                        DialogueModule realityModule = new DialogueModule("Unfortunately, no. The world can be cruel. Many men see courtesans as mere objects, forgetting the dreams and stories behind our smiles. It’s a constant battle to maintain one's dignity.");
                                        realityModule.AddOption("I admire your strength.",
                                            plt => true,
                                            plt => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        realityModule.AddOption("That sounds disheartening.",
                                            ply => true,
                                            ply => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                        p.SendGump(new DialogueGump(p, realityModule));
                                    });
                                p.SendGump(new DialogueGump(p, lessonsModule));
                            });
                        player.SendGump(new DialogueGump(player, mentorModule));
                    });
                courtesanModule.AddOption("What has been your greatest challenge?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule challengeModule = new DialogueModule("The greatest challenge? Balancing my heart. There are moments of genuine connection, yet I must always remind myself of my purpose. Love can be a dangerous game.");
                        challengeModule.AddOption("Have you ever fallen in love?",
                            p => true,
                            p =>
                            {
                                DialogueModule loveModule = new DialogueModule("Once, but it was fleeting. He was a charming bard who sang of freedom and adventure. We shared a night filled with laughter, but in the morning, he was gone, leaving only a song in my heart.");
                                loveModule.AddOption("That sounds beautiful.",
                                    plu => true,
                                    plu => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                loveModule.AddOption("A painful memory, then?",
                                    pli => true,
                                    pli =>
                                    {
                                        DialogueModule painModule = new DialogueModule("Indeed. But pain can teach us. I now cherish the moments of joy and see love as both a gift and a lesson.");
                                        painModule.AddOption("Wise words, Tina.",
                                            pla => true,
                                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                        p.SendGump(new DialogueGump(p, painModule));
                                    });
                                p.SendGump(new DialogueGump(p, loveModule));
                            });
                        challengeModule.AddOption("What do you wish for most?",
                            plo => true,
                            plo =>
                            {
                                DialogueModule wishModule = new DialogueModule("I wish for freedom—freedom to choose whom I love, whom I dance with, and where I go. The streets were harsh, but they were mine.");
                                wishModule.AddOption("You deserve that freedom.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                plo.SendGump(new DialogueGump(plo, wishModule));
                            });
                        player.SendGump(new DialogueGump(player, challengeModule));
                    });
                player.SendGump(new DialogueGump(player, courtesanModule));
            });

        greeting.AddOption("Tell me more about love.",
            player => true,
            player =>
            {
                DialogueModule loveModule = new DialogueModule("Ah, love—the most intoxicating of feelings. It can lift you to the heavens or plunge you into the depths of despair. Do you believe in love at first sight?");
                loveModule.AddOption("Yes, I do.",
                    pl => true,
                    pl => 
                    {
                        DialogueModule firstSightModule = new DialogueModule("Ah, then you understand! It’s a spark, a flame that ignites the heart. But it can also be fleeting, like a candle in the wind. Do you have a story of your own?");
                        firstSightModule.AddOption("I met someone once.",
                            p => true,
                            p => 
                            {
                                DialogueModule playerStoryModule = new DialogueModule("Tell me about it! I love to hear stories of romance. They remind me that magic still exists in this world.");
                                playerStoryModule.AddOption("It was enchanting and brief.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                playerStoryModule.AddOption("It ended in heartbreak.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                p.SendGump(new DialogueGump(p, playerStoryModule));
                            });
                        loveModule.AddOption("No, I think it’s a myth.",
                            pls => true,
                            pls => 
                            {
                                DialogueModule mythModule = new DialogueModule("A myth? Perhaps. But even myths carry a truth. They speak of our hopes and desires, don’t you think?");
                                mythModule.AddOption("You make a compelling point.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                mythModule.AddOption("I suppose there’s beauty in hope.",
                                    pla => true,
                                    pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                pls.SendGump(new DialogueGump(pls, mythModule));
                            });
                        player.SendGump(new DialogueGump(player, firstSightModule));
                    });
                loveModule.AddOption("What about passion?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule passionModule = new DialogueModule("Passion fuels desire and gives life its colors. It can be exhilarating and terrifying, like dancing on the edge of a blade. Have you ever felt that fire?");
                        passionModule.AddOption("Yes, it’s overwhelming.",
                            pla => true,
                            pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                        passionModule.AddOption("I try to keep it in check.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule controlModule = new DialogueModule("Wise choice. Passion can consume us if we let it. Balance is key. Find joy in the small things, and don’t lose yourself in the flames.");
                                controlModule.AddOption("Thank you for your wisdom.",
                                    plf => true,
                                    plf => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                pla.SendGump(new DialogueGump(pla, controlModule));
                            });
                        player.SendGump(new DialogueGump(player, passionModule));
                    });
                player.SendGump(new DialogueGump(player, loveModule));
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
}
