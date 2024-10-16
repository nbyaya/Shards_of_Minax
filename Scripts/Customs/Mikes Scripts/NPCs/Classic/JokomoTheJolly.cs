using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jokomo the Jolly")]
    public class JokomoTheJolly : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JokomoTheJolly() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jokomo the Jolly";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(60);
            SetInt(90);
            SetHits(90);

            // Appearance
            AddItem(new JesterHat() { Hue = 38 });
            AddItem(new JesterSuit() { Hue = 1324 });
            AddItem(new Boots() { Hue = 1102 });

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
            DialogueModule greeting = new DialogueModule("Ho ho! I am Jokomo the Jolly, the jester of riddles and merriment! How may I entertain you?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Ah, my health? The laughter keeps me hearty and hale!")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My job, you ask? To spread joy and wisdom through riddles and jests!");
                    jobModule.AddOption("What kind of jests?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, the jests! They are the sparkles of life! I tell jokes, stories, and riddles to lighten the heart! Would you like to hear one?")));
                        });
                    jobModule.AddOption("Do you perform anywhere?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! I perform at fairs and festivals, bringing laughter to all. If you ever hear of a gathering, come and join the fun!")));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you think of the virtues?",
                player => true,
                player =>
                {
                    DialogueModule virtuesModule = new DialogueModule("But dost thou know of the virtues, those eight guiding lights of Britannia?");
                    virtuesModule.AddOption("What are these virtues?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility! Each plays a part in our lives and shapes our destinies!")));
                        });
                    virtuesModule.AddOption("Why is humility important?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, humility! It teaches us to listen, to learn, and to grow. Without it, one can easily stray from the path of wisdom!")));
                        });
                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("Do you have a riddle for me?",
                player => true,
                player =>
                {
                    DialogueModule riddleModule = new DialogueModule("Very well! Here's a riddle for you: \"I speak without a mouth and hear without ears. I have no body, but I come alive with the wind.\" What am I?");
                    riddleModule.AddOption("I want to guess!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("What is your guess?")));
                            // Here you could implement guess checking logic.
                        });
                    riddleModule.AddOption("Can you give me a hint?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Here's a hint: I am often heard in the mountains and valleys, but I have no physical form. What could I be?")));
                        });
                    player.SendGump(new DialogueGump(player, riddleModule));
                });

            greeting.AddOption("Do you tell jokes?",
                player => true,
                player =>
                {
                    DialogueModule jokeModule = new DialogueModule("Jests and jokes are the sparks that light up our souls! Here's one for you: Why did the scarecrow win an award? Because he was outstanding in his field!");
                    jokeModule.AddOption("Tell me another joke!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Alright! How do you organize a space party? You planet! Ha ha!")));
                        });
                    jokeModule.AddOption("What’s your favorite kind of humor?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Oh, I enjoy puns and clever wordplay! They tickle the mind and the heart! What about you?")));
                        });
                    player.SendGump(new DialogueGump(player, jokeModule));
                });

            greeting.AddOption("What about rewards?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("Ah, you seek the reward! Solve my riddle correctly, and something valuable will be yours.")));
                    }
                    else
                    {
                        player.AddToBackpack(new MaxxiaScroll());
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.SendGump(new DialogueGump(player, new DialogueModule("Deep reflection on virtues is essential for one's personal growth. For your thoughtful inquiry, please accept this reward.")));
                    }
                });

            greeting.AddOption("Tell me about your past.",
                player => true,
                player =>
                {
                    DialogueModule pastModule = new DialogueModule("Ah, my past is filled with merry moments! I traveled across Britannia, spreading joy wherever I went. Would you like to hear a tale?");
                    pastModule.AddOption("Yes, please share a tale!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Once, I saved a village from a gloomy spirit with a simple joke! The laughter that erupted brought light back to their hearts!")));
                        });
                    pastModule.AddOption("No, I’m more interested in your travels.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I have traveled through dense forests, across vast mountains, and along bustling towns, always seeking the next opportunity to bring joy!")));
                        });
                    player.SendGump(new DialogueGump(player, pastModule));
                });

            greeting.AddOption("What do you think about the future?",
                player => true,
                player =>
                {
                    DialogueModule futureModule = new DialogueModule("The future, ah! It is like a grand stage waiting for the next performance. I believe laughter will always hold a place in it!");
                    futureModule.AddOption("What if the future is bleak?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Even in darkness, a spark of joy can ignite hope! It’s up to us to keep the laughter alive!")));
                        });
                    futureModule.AddOption("Will you always be a jester?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("As long as there are ears to listen and hearts to uplift, I shall always be a jester!")));
                        });
                    player.SendGump(new DialogueGump(player, futureModule));
                });

            return greeting;
        }

        public JokomoTheJolly(Serial serial) : base(serial) { }

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
}
