using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

[CorpseName("the corpse of Jolly Jana")]
public class JollyJana : BaseCreature
{
    private DateTime lastRewardTime;

    [Constructable]
    public JollyJana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Jolly Jana";
        Body = 0x191; // Human female body

        // Stats
        SetStr(70);
        SetDex(70);
        SetInt(120);
        SetHits(70);

        // Appearance
        AddItem(new FancyDress() { Hue = 2214 });
        AddItem(new Boots() { Hue = 2214 });
        AddItem(new GoldNecklace() { Name = "Jana's Jolly Necklace" });
        AddItem(new ShortSpear() { Name = "Jana's Jolly Spear" });

        Hue = Race.RandomSkinHue();
        HairItemID = Race.RandomHair(Female);
        HairHue = Race.RandomHairHue();

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
        DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Jolly Jana, the keeper of laughter! How can I brighten your day?");

        greeting.AddOption("Tell me about yourself.",
            player => true,
            player =>
            {
                DialogueModule aboutMe = new DialogueModule("I spread joy and merriment wherever I go! My laughter is infectious, and I believe it can heal even the heaviest hearts. Would you like to know how I became the keeper of laughter?");
                aboutMe.AddOption("Yes, how did you become the keeper of laughter?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstory = new DialogueModule("Ah! It all started when I was a young girl. I discovered that laughter could bring people together, and I decided to dedicate my life to spreading joy. My travels have taken me to many lands, each filled with unique stories. Would you like to hear about one of my adventures?");
                        backstory.AddOption("Please, tell me about an adventure.",
                            pll => true,
                            pll =>
                            {
                                DialogueModule adventure = new DialogueModule("Once, I found myself in a village plagued by sorrow. I organized a grand festival, filled with games and laughter, which brought the villagers together. By the end of the night, their worries had faded away! They thanked me with joyous smiles. It was truly a heartwarming experience!");
                                pll.SendGump(new DialogueGump(pll, adventure));
                            });
                        player.SendGump(new DialogueGump(player, backstory));
                    });
                player.SendGump(new DialogueGump(player, aboutMe));
            });

        greeting.AddOption("Do you know any jokes?",
            player => true,
            player =>
            {
                DialogueModule jokeModule = new DialogueModule("Ah! I have a good one for you: Why did the scarecrow win an award? Because he was outstanding in his field! Haha!");
                jokeModule.AddOption("That was funny!",
                    pl => true,
                    pl =>
                    {
                        DialogueModule followUp = new DialogueModule("I'm glad you enjoyed it! Laughter is like a flower that blooms in the heart. Would you like to hear another?");
                        followUp.AddOption("Yes, tell me another!",
                            pll => true,
                            pll => 
                            {
                                pll.SendGump(new DialogueGump(pll, new DialogueModule("Why don’t scientists trust atoms? Because they make up everything! Haha!")));
                            });
                        followUp.AddOption("Maybe later.",
                            pll => true,
                            pll => 
                            {
                                pll.SendGump(new DialogueGump(pll, CreateGreetingModule()));
                            });
                        player.SendGump(new DialogueGump(player, followUp));
                    });
                player.SendGump(new DialogueGump(player, jokeModule));
            });

        greeting.AddOption("Can you tell me about 'The Laughing Order'?",
            player => true,
            player =>
            {
                DialogueModule guildInfo = new DialogueModule("Our guild believes in the healing power of humor. We gather jokes and tales to share with the world. Each member contributes their favorite stories. Would you be interested in joining our guild of laughter?");
                guildInfo.AddOption("Yes, I’d love to join!",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Welcome! Laughter is a bond that unites us. Together, we can spread joy far and wide!")));
                    });
                guildInfo.AddOption("Not right now, but tell me more about the guild.",
                    pl => true,
                    pl =>
                    {
                        DialogueModule moreInfo = new DialogueModule("The Laughing Order organizes events where we perform and share stories. We also host laughter workshops to help others find their humor! What kind of humor do you enjoy?");
                        moreInfo.AddOption("I enjoy puns.",
                            pll => true,
                            pll => 
                            {
                                pll.SendGump(new DialogueGump(pll, new DialogueModule("Puns are delightful! They tickle the mind! Would you like to hear some punny jokes?")));
                            });
                        moreInfo.AddOption("I prefer stories.",
                            pll => true,
                            pll => 
                            {
                                pll.SendGump(new DialogueGump(pll, new DialogueModule("Stories bring laughter to life! I can share some of the funniest tales from my travels!")));
                            });
                        pl.SendGump(new DialogueGump(pl, moreInfo));
                    });
                player.SendGump(new DialogueGump(player, guildInfo));
            });

        greeting.AddOption("What do you think about laughter?",
            player => true,
            player =>
            {
                DialogueModule laughterThoughts = new DialogueModule("Laughter is a virtue! It's the light in the darkest of times. It brings us together and helps us heal. Can you find humor in life's trials?");
                laughterThoughts.AddOption("Absolutely! Laughter is essential.",
                    pl => true,
                    pl => 
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("Then you possess the heart of a true jester! Tell me, what's the funniest thing that's ever happened to you?")));
                    });
                laughterThoughts.AddOption("Sometimes it's hard to laugh.",
                    pl => true,
                    pl =>
                    {
                        pl.SendGump(new DialogueGump(pl, new DialogueModule("I understand. Life can be challenging. But remember, even in tough times, a good laugh can make everything feel a little lighter!")));
                    });
                player.SendGump(new DialogueGump(player, laughterThoughts));
            });

        greeting.AddOption("May I have a token of gratitude for listening?",
            player => true,
            player =>
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                }
                else
                {
                    player.AddToBackpack(new SnoopingAugmentCrystal());
                    lastRewardTime = DateTime.UtcNow;
                    player.SendGump(new DialogueGump(player, new DialogueModule("Here's a little token of gratitude for appreciating my jokes! Keep spreading joy!")));
                }
            });

        greeting.AddOption("Goodbye.",
            player => true,
            player =>
            {
                player.SendGump(new DialogueGump(player, new DialogueModule("Oh, what a splendid tale! Laughter truly is the best medicine. Farewell, and may your days be filled with mirth!")));
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

    public JollyJana(Serial serial) : base(serial) { }
}
