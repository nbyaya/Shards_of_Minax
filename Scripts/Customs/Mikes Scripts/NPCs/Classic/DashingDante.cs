using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

public class DashingDante : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public DashingDante() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Dashing Dante";
        Body = 0x190; // Human male body

        // Stats
        SetStr(95);
        SetDex(75);
        SetInt(45);
        SetHits(70);

        // Appearance
        AddItem(new LongPants(38)); // Pants with hue 38
        AddItem(new FancyShirt(1904)); // Shirt with hue 1904
        AddItem(new Boots(64)); // Boots with hue 64
        AddItem(new GoldRing { Name = "Dante's Signet Ring" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();

        // Initialize the lastRewardTime to a past time
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Dashing Dante, a courtesan of these lands. How may I assist you?");
        
        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutModule = new DialogueModule("Ah, a curious soul! I am a companion, a listener, and sometimes an adventurer. They call me Dashing Dante, after all!");
                aboutModule.AddOption("Why do they call you 'Dashing' Dante?",
                    p => true,
                    p =>
                    {
                        DialogueModule nameModule = new DialogueModule("Ah, my moniker is indeed unusual. My parents hoped I'd grow into an adventurous and charming soul, and so, Dashing Dante I became!");
                        nameModule.AddOption("That is quite a name!",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        nameModule.AddOption("Did your parents succeed in shaping your destiny?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule destinyModule = new DialogueModule("They certainly did! My life has been filled with adventure, charm, and a fair bit of danger. I've met kings and beggars, crossed vast deserts, and navigated treacherous waters. Each moment has shaped me into the Dante you see today.");
                                destinyModule.AddOption("Tell me about the most dangerous adventure you've had.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Ah, the most dangerous adventure? That would be my journey through the Desolation Swamps. The air was thick with the stench of decay, and every step I took was fraught with danger. I narrowly escaped a group of bog trolls with my wit and a bit of luck.");
                                        dangerModule.AddOption("You must have been terrified!",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule fearModule = new DialogueModule("Indeed, fear was my constant companion. But fear can also sharpen the mind and focus the spirit. I learned much about myself during that ordeal, and it made me the man I am today.");
                                                fearModule.AddOption("That's inspiring, Dante.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, fearModule));
                                            });
                                        dangerModule.AddOption("How did you manage to escape?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule escapeModule = new DialogueModule("With quick thinking and a little bit of charm! I convinced one of the trolls that I knew where to find a hidden treasure. They let me go in exchange for directions, which of course led them far away from me. It wasn't my proudest moment, but survival often requires creativity.");
                                                escapeModule.AddOption("You are quite the trickster!",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, escapeModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, dangerModule));
                                    });
                                destinyModule.AddOption("What about the happiest moment of your life?",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule happyModule = new DialogueModule("The happiest moment? That would be meeting Lady Seraphina, one of my favorite customers. She had a laugh that could light up the darkest night, and a heart full of kindness. She showed me that even amidst all the intrigue, there are pure souls in this world.");
                                        happyModule.AddOption("Tell me more about Lady Seraphina.",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule seraphinaModule = new DialogueModule("Lady Seraphina was a noblewoman, but she never let her status define her. She often visited me just to escape the rigid expectations of her life. We would share stories, laugh, and sometimes just sit quietly, watching the stars. Her presence was a gift.");
                                                seraphinaModule.AddOption("She sounds wonderful.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                seraphinaModule.AddOption("Did you ever fall in love with her?",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        DialogueModule loveModule = new DialogueModule("Ah, love. It was complicated. I admired her deeply, but our worlds were too different. She was destined for greater things, and I knew my place. We shared a bond, but it was one that transcended romantic love—more of a kindred spirit, a deep understanding.");
                                                        loveModule.AddOption("That's beautiful, Dante.",
                                                            pld => true,
                                                            pld =>
                                                            {
                                                                pld.SendGump(new DialogueGump(pld, CreateGreetingModule()));
                                                            });
                                                        plc.SendGump(new DialogueGump(plc, loveModule));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, seraphinaModule));
                                            });
                                        happyModule.AddOption("Did she ever need your help?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule helpModule = new DialogueModule("Once, she needed to escape the political games of her family. I helped her hide for a few days in the countryside, away from prying eyes. We lived simply, without titles or responsibilities, just two people enjoying each other's company. It was fleeting, but perfect.");
                                                helpModule.AddOption("Sounds like a cherished memory.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, helpModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, happyModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, destinyModule));
                            });
                        p.SendGump(new DialogueGump(p, nameModule));
                    });
                aboutModule.AddOption("What is your profession?",
                    p => true,
                    p =>
                    {
                        DialogueModule professionModule = new DialogueModule("My profession, dear interlocutor, is the art of companionship and intrigue. In this lonely world, companionship is a solace to many. It's an art, really.");
                        professionModule.AddOption("It sounds fascinating.",
                            pl => true,
                            pl =>
                            {
                                pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                            });
                        professionModule.AddOption("Who are your favorite customers?",
                            pl => true,
                            pl =>
                            {
                                DialogueModule favoriteCustomersModule = new DialogueModule("Ah, my favorite customers are those who bring a little bit of themselves into our time together. There is Lady Seraphina, of course, but also young Thom, a farmer's son who dreams of adventure, and old Master Corwin, who loves to reminisce about the past.");
                                favoriteCustomersModule.AddOption("Tell me about young Thom.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule thomModule = new DialogueModule("Thom is a bright young man, full of dreams of far-off lands and heroic deeds. He comes to me to hear stories of adventure, and I do my best to inspire him. I see a bit of my younger self in him, and I hope he finds the courage to chase his dreams.");
                                        thomModule.AddOption("Do you think he will become an adventurer?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule courageModule = new DialogueModule("I believe he will. He has the heart for it, and that is often the most important part. Adventure isn't just about skill with a sword—it's about the courage to face the unknown, to take risks, and to learn from failure.");
                                                courageModule.AddOption("I hope he succeeds.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, courageModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, thomModule));
                                    });
                                favoriteCustomersModule.AddOption("Tell me about old Master Corwin.",
                                    pla => true,
                                    pla =>
                                    {
                                        DialogueModule corwinModule = new DialogueModule("Master Corwin is a storyteller at heart. He has lived a full life, filled with both joy and sorrow. He often comes to me to share tales of his youth—of battles fought, loves won and lost, and lessons learned. I cherish our conversations, for they remind me of the importance of remembering where we come from.");
                                        corwinModule.AddOption("What kind of lessons does he share?",
                                            plb => true,
                                            plb =>
                                            {
                                                DialogueModule lessonsModule = new DialogueModule("Corwin speaks of patience, of forgiveness, and of the value of true friendship. He says that as the years go by, it's not the riches or victories that matter, but the connections we make and the people we love. His wisdom is a treasure.");
                                                lessonsModule.AddOption("That's a beautiful sentiment.",
                                                    plc => true,
                                                    plc =>
                                                    {
                                                        plc.SendGump(new DialogueGump(plc, CreateGreetingModule()));
                                                    });
                                                plb.SendGump(new DialogueGump(plb, lessonsModule));
                                            });
                                        pla.SendGump(new DialogueGump(pla, corwinModule));
                                    });
                                pl.SendGump(new DialogueGump(pl, favoriteCustomersModule));
                            });
                        player.SendGump(new DialogueGump(player, professionModule));
                    });
                player.SendGump(new DialogueGump(player, aboutModule));
            });

        greeting.AddOption("Can you tell me about the virtues?",
            player => true,
            player =>
            {
                DialogueModule virtuesModule = new DialogueModule("True virtue lies not only in deeds but in the warmth of one's heart. Honesty, Compassion, Valor, Justice... which virtues guide your path?");
                virtuesModule.AddOption("Tell me more about Compassion.",
                    p => true,
                    p =>
                    {
                        TimeSpan cooldown = TimeSpan.FromMinutes(10);
                        if (DateTime.UtcNow - lastRewardTime < cooldown)
                        {
                            p.SendMessage("I have no reward right now. Please return later.");
                        }
                        else
                        {
                            DialogueModule compassionModule = new DialogueModule("Compassion is a rare gem. Once, I met a weary traveler on the road who was starving. I shared my food, and in gratitude, he gifted me a mysterious token. Here, I think you should have it.");
                            compassionModule.AddOption("Thank you for the token.",
                                pl => true,
                                pl =>
                                {
                                    pl.AddToBackpack(new MaxxiaScroll());
                                    lastRewardTime = DateTime.UtcNow;
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, compassionModule));
                        }
                    });
                virtuesModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, virtuesModule));
            });

        greeting.AddOption("Can you sing me a song?",
            player => true,
            player =>
            {
                DialogueModule songModule = new DialogueModule("Music has a way of connecting souls. If you have some time, I could sing you a ballad of old, a tale of love and loss that has touched many hearts.");
                songModule.AddOption("Please sing it for me.",
                    p => true,
                    p =>
                    {
                        p.SendMessage("Dashing Dante sings a haunting melody, his voice full of emotion. The tale of love and loss resonates deeply within you.");
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                songModule.AddOption("Maybe another time.",
                    p => true,
                    p =>
                    {
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                player.SendGump(new DialogueGump(player, songModule));
            });

        greeting.AddOption("Goodbye, Dante.",
            player => true,
            player =>
            {
                player.SendMessage("Dashing Dante nods gracefully and smiles.");
            });

        return greeting;
    }

    public DashingDante(Serial serial) : base(serial) { }

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