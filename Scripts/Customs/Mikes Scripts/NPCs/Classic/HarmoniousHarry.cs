using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

public class HarmoniousHarry : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public HarmoniousHarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Harmonious Harry";
        Body = 0x190; // Human male body
        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(this);
        HairHue = Race.RandomHairHue();
        SpeechHue = 0;

        // Stats
        SetStr(115);
        SetDex(70);
        SetInt(85);
        SetHits(90);

        // Appearance
        AddItem(new FancyShirt() { Hue = 1107 });
        AddItem(new LongPants() { Hue = 1912 });
        AddItem(new Boots() { Hue = 38 });
        AddItem(new LeatherGloves() { Name = "Harry's Harmony Gloves" });

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
        DialogueModule greeting = new DialogueModule("I am Harmonious Harry, the bard with a tune as sour as life itself!");

        greeting.AddOption("Tell me about your health.",
            player => true,
            player =>
            {
                DialogueModule healthModule = new DialogueModule("My health? As miserable as my melodies, I assure you! Though, perhaps a song might lift my spirits. Would you care to hear one?");
                healthModule.AddOption("Yes, please sing for me.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule songModule = new DialogueModule("Ah, gather 'round! Here’s a tune of lost hopes and dreams, a ballad that echoes through the ages...");
                        songModule.AddOption("What’s the story behind this song?",
                            p => true,
                            p =>
                            {
                                DialogueModule storyModule = new DialogueModule("This song speaks of a lost city, where laughter once filled the air, now only shadows remain. Its name is forgotten, buried beneath time's cruel embrace.");
                                p.SendGump(new DialogueGump(p, storyModule));
                            });
                        pl.SendGump(new DialogueGump(pl, songModule));
                    });
                player.SendGump(new DialogueGump(player, healthModule));
            });

        greeting.AddOption("What is your job?",
            player => true,
            player =>
            {
                DialogueModule jobModule = new DialogueModule("My 'job' is to serenade this wretched world with songs of despair. Yet, in my heart, I dream of brighter days.");
                jobModule.AddOption("What do you dream about?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule dreamModule = new DialogueModule("I dream of a world where joy reigns supreme, where laughter dances on the wind, and where the notes of my lute soar high into the sky.");
                        dreamModule.AddOption("What would you do in such a world?",
                            p => true,
                            p =>
                            {
                                DialogueModule futureModule = new DialogueModule("In such a world, I would compose symphonies of joy, spreading harmony and light to every corner. But alas, it remains but a dream.");
                                p.SendGump(new DialogueGump(p, futureModule));
                            });
                        pl.SendGump(new DialogueGump(pl, dreamModule));
                    });
                player.SendGump(new DialogueGump(player, jobModule));
            });

        greeting.AddOption("What do you think of battles?",
            player => true,
            player =>
            {
                DialogueModule battlesModule = new DialogueModule("Valiant, you say? In this world of endless sorrow? I have my doubts. Prove your valor, if you dare. But tell me, what do you seek in battle?");
                battlesModule.AddOption("I seek glory.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule gloryModule = new DialogueModule("Glory, the sweetest poison! It can uplift the soul or destroy the heart. What will you do with such glory when it is yours?");
                        gloryModule.AddOption("I will use it to inspire others.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("A noble cause! To inspire is to be a true bard. The echoes of your deeds will resonate for generations.")));
                            });
                        pl.SendGump(new DialogueGump(pl, gloryModule));
                    });
                battlesModule.AddOption("I seek to protect the innocent.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule protectModule = new DialogueModule("A valiant pursuit! Protecting the innocent is the mark of a true hero. You bear a heavy burden, yet it is a path of honor.");
                        protectModule.AddOption("It is a burden I gladly bear.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Then walk your path with pride. May your heart remain unyielding amidst the chaos.")));
                            });
                        pl.SendGump(new DialogueGump(pl, protectModule));
                    });
                player.SendGump(new DialogueGump(player, battlesModule));
            });

        greeting.AddOption("Tell me about your songs.",
            player => true,
            player =>
            {
                DialogueModule songsModule = new DialogueModule("Each of my songs carries a tale, stories of lost hopes and broken dreams. Would you like to hear a specific tale or just a tune?");
                songsModule.AddOption("I want to hear a tale.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule taleModule = new DialogueModule("Ah, there’s one about a lost city that’s my favorite. A city swallowed by shadows, never to be seen again. Would you care for its details?");
                        taleModule.AddOption("Yes, tell me more!",
                            p => true,
                            p =>
                            {
                                DialogueModule detailModule = new DialogueModule("The city was once vibrant, filled with laughter and music. But darkness crept in, and it vanished as if it never existed. Many seek its treasures, but none return.");
                                p.SendGump(new DialogueGump(p, detailModule));
                            });
                        pl.SendGump(new DialogueGump(pl, taleModule));
                    });
                songsModule.AddOption("Just a tune, please.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule tuneModule = new DialogueModule("Very well! *strums his lute*... Here’s a melody that captures the essence of sorrow, the bittersweet dance of life.");
                        pl.SendGump(new DialogueGump(pl, tuneModule));
                    });
                player.SendGump(new DialogueGump(player, songsModule));
            });

        greeting.AddOption("What do you think of art?",
            player => true,
            player =>
            {
                DialogueModule artModule = new DialogueModule("Art, in its truest form, is born from emotion, be it joy or sorrow. I channel my emotions, no matter how bleak, into my melodies. What is your view on art?");
                artModule.AddOption("Art is a reflection of life.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule reflectionModule = new DialogueModule("Indeed! Each brushstroke, each note tells a story. What story would you tell through your art?");
                        reflectionModule.AddOption("A story of triumph.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Triumph is a powerful tale! It inspires hope and courage in others.")));
                            });
                        reflectionModule.AddOption("A story of despair.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Despair, too, has its place. It reveals the fragility of life, making triumph even sweeter.")));
                            });
                        pl.SendGump(new DialogueGump(pl, reflectionModule));
                    });
                artModule.AddOption("Art is a waste of time.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule wasteModule = new DialogueModule("A bold stance! Perhaps you’ve yet to see its power to change hearts and minds. Care to share your thoughts?");
                        wasteModule.AddOption("I just don't see its value.",
                            p => true,
                            p =>
                            {
                                p.SendGump(new DialogueGump(p, new DialogueModule("Understanding comes with time. Perhaps one day, you’ll see the world through an artist’s eyes.")));
                            });
                        pl.SendGump(new DialogueGump(pl, wasteModule));
                    });
                player.SendGump(new DialogueGump(player, artModule));
            });

        greeting.AddOption("Can you give me something?",
            player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
            player =>
            {
                lastRewardTime = DateTime.UtcNow;
                player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                player.SendMessage("Gloom, you see, is a potent ingredient for art. Here, take this token as a reminder of the duality of existence.");
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player => player.SendMessage("Farewell, traveler. May your path be less sorrowful than mine."));

        return greeting;
    }

    public HarmoniousHarry(Serial serial) : base(serial) { }

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
