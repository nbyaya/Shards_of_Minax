using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zarth the Star Wanderer")]
    public class ZarthTheStarWanderer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZarthTheStarWanderer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zarth the Star Wanderer";
            Body = 0x190; // Human male body
            Str = 80;
            Dex = 60;
            Int = 150;
            Hits = 90;

            AddItem(new Sandals() { Hue = 1173 });
            AddItem(new Cloak() { Hue = 1172 });
            AddItem(new QuarterStaff() { Name = "Zarth's Starseeker" });

            Hue = 1170; // Body hue
            HairItemID = Race.RandomHair(this);
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
            DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! I am Zarth the Star Wanderer, an explorer of cosmic mysteries! How may I assist you?");

            greeting.AddOption("Tell me about yourself.",
                player => true,
                player =>
                {
                    DialogueModule aboutModule = new DialogueModule("I wander the cosmos in search of secrets and harmony. My journey is filled with wonders!");
                    aboutModule.AddOption("What wonders have you seen?",
                        p => true,
                        p =>
                        {
                            DialogueModule wondersModule = new DialogueModule("I've traversed the Nebula of Dreams and the Realm of Lost Time. Each place holds stories waiting to be uncovered.");
                            wondersModule.AddOption("Can you share a story?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("In the Nebula of Dreams, I encountered shimmering entities that spoke in colors rather than words. Their wisdom was profound and beyond comprehension.")));
                                });
                            wondersModule.AddOption("What else do you seek?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("I seek the balance of the cosmos and knowledge to help others find their path.")));
                                });
                            p.SendGump(new DialogueGump(p, wondersModule));
                        });
                    player.SendGump(new DialogueGump(player, aboutModule));
                });

            greeting.AddOption("What do you know about health?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I am in perfect harmony with the cosmos. True health comes from balance and understanding oneself.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("I seek the secrets of the universe, traveling far and wide. Each star tells a story, and every journey reveals a truth.");
                    jobModule.AddOption("What truths have you discovered?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("One truth I cherish is that our destinies are intertwined with the stars. Every choice shapes our path.")));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What can you tell me about spirituality?",
                player => true,
                player =>
                {
                    DialogueModule spiritualityModule = new DialogueModule("Spirituality is key to understanding the cosmos. Is your spirit in harmony?");
                    spiritualityModule.AddOption("How can I find harmony?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule harmonyModule = new DialogueModule("Finding harmony requires introspection and a connection to the universe. Meditation under the stars can be enlightening.");
                            harmonyModule.AddOption("What is meditation?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Meditation is a practice of calming the mind, allowing one to connect with deeper truths and the universe.")));
                                });
                            pl.SendGump(new DialogueGump(pl, harmonyModule));
                        });
                    player.SendGump(new DialogueGump(player, spiritualityModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
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
                        lastRewardTime = DateTime.UtcNow;
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        player.SendGump(new DialogueGump(player, new DialogueModule("The cosmos has gifted you with knowledge and a small trinket!")));
                    }
                });

            greeting.AddOption("Tell me about the galaxies.",
                player => true,
                player =>
                {
                    DialogueModule galaxiesModule = new DialogueModule("There are countless galaxies, each with its own wonders. I once visited a galaxy where stars sang melodies of ancient times.");
                    galaxiesModule.AddOption("What melodies did they sing?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("They sang of the creation of worlds and the dance of time, a symphony of light and sound that echoed through the cosmos.")));
                        });
                    player.SendGump(new DialogueGump(player, galaxiesModule));
                });

            greeting.AddOption("How do you maintain balance?",
                player => true,
                player =>
                {
                    DialogueModule balanceModule = new DialogueModule("Balance is crucial. Too much of one thing can offset the natural order. Do you strive for balance in your life?");
                    balanceModule.AddOption("Yes, but it's difficult.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed, it can be challenging. Remember to take time for yourself and connect with nature.")));
                        });
                    balanceModule.AddOption("No, I struggle with that.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("It's never too late to begin. Consider setting small goals to help you find your center.")));
                        });
                    player.SendGump(new DialogueGump(player, balanceModule));
                });

            return greeting;
        }

        public ZarthTheStarWanderer(Serial serial) : base(serial) { }

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
