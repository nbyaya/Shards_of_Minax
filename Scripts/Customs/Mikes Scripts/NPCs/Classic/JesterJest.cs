using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Jest")]
    public class JesterJest : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJest() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Jest";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 90;
            Int = 85;
            Hits = 70;

            // Appearance
            AddItem(new JesterHat() { Hue = 88 });
            AddItem(new JesterSuit() { Hue = 88 });
            AddItem(new Sandals() { Hue = 1190 });

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Jester Jest, the merry jester of these lands! What brings you to my whimsical world?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("Fear not, my health is as vibrant as a rainbow! But I jest, for I am but a humble jester.");
                    healthModule.AddOption("That's good to hear! Any ailments I can assist with?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My job, you ask? Why, it is to spread laughter and merriment throughout the realm! But tell me, do you find joy in humor?");
                    jobModule.AddOption("I do! Laughter brightens the day.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule joyModule = new DialogueModule("Ah, a fellow lover of laughter! What type of humor tickles your fancy? Slapstick or witty wordplay?");
                            joyModule.AddOption("I prefer slapstick humor!",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("You laugh as you recall the antics of clumsy fools.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            joyModule.AddOption("Witty wordplay is my favorite.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Ah, the clever twists of language! They always bring a smile.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, joyModule));
                        });
                    jobModule.AddOption("Not really, but I admire your spirit.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What can you tell me about virtues?",
                player => true,
                player =>
                {
                    DialogueModule virtuesModule = new DialogueModule("Ah, the virtues! They are like the colors of a rainbow, each one shining in its own unique way. Can you name one of them?");
                    virtuesModule.AddOption("How about honesty?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule honestyModule = new DialogueModule("Very good! \"Honesty\" is one of the virtues. In this realm, as in life, honesty is a key to harmony. Do you seek more wisdom about the virtues?");
                            honestyModule.AddOption("Yes, tell me about compassion.",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Compassion is the heart's warmth towards others! It binds us together.");
                                    p.SendGump(new DialogueGump(p, honestyModule));
                                });
                            honestyModule.AddOption("No, I prefer to jest.",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Ah, humor is a virtue of its own! Let us share a laugh.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, honestyModule));
                        });
                    virtuesModule.AddOption("I can name a few more.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule moreVirtuesModule = new DialogueModule("Excellent! Which virtues do you hold dear?");

                            moreVirtuesModule.AddOption("Courage!",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Courage is the bravery to face challenges head-on! A true virtue.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            moreVirtuesModule.AddOption("Justice!",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Justice is the pursuit of fairness and balance! Essential for peace.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, moreVirtuesModule));
                        });
                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("Do you have any jokes?",
                player => true,
                player =>
                {
                    DialogueModule jokeModule = new DialogueModule("Ah, one of my favorites goes: \"Why did the scarecrow win an award? Because he was outstanding in his field!\" Do you have any jokes to share?");
                    jokeModule.AddOption("Yes! Here’s one: Why did the chicken cross the road?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule responseModule = new DialogueModule("To get to the other side! Classic! Do you have any more?");
                            responseModule.AddOption("One more! What do you call cheese that isn't yours?",
                                p => true,
                                p => 
                                {
                                    DialogueModule cheeseModule = new DialogueModule("Nacho cheese! Haha! You're quite the comedian!");
                                    cheeseModule.AddOption("Thank you! I'm just trying to brighten the day.",
                                        pla => true,
                                        pla => 
                                        {
                                            pla.SendMessage("Your jokes certainly bring joy!");
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, cheeseModule));
                                });
                            pl.SendGump(new DialogueGump(pl, responseModule));
                        });
                    jokeModule.AddOption("Not at the moment, but thanks!",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, jokeModule));
                });

            greeting.AddOption("Can you tell me a tale about a rainbow?",
                player => true,
                player =>
                {
                    DialogueModule taleModule = new DialogueModule("Once, I saw a rainbow so close, I could touch it! It led me to a mystical grove where fairies danced. Their leader, a sprite named Lila, gave me a charm that ensures my jokes never fall flat! Would you like to hear more about the fairies?");
                    taleModule.AddOption("Absolutely! What were they like?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule fairyModule = new DialogueModule("Oh, they were splendid! Their laughter rang like bells, and their wings shimmered like starlight. Each fairy had a unique talent, from singing to weaving magic. One even painted the skies with vibrant colors!");
                            fairyModule.AddOption("That sounds enchanting! Did you join them?",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Indeed! I danced among them, feeling light as air, and shared jokes that made even the stoic ones giggle!");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            fairyModule.AddOption("Did they teach you any magic?",
                                p => true,
                                p => 
                                {
                                    p.SendMessage("Ah, yes! They showed me how to make flowers bloom with laughter. But alas, my magic is limited to jest and cheer!");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, fairyModule));
                        });
                    player.SendGump(new DialogueGump(player, taleModule));
                });

            greeting.AddOption("What about laughter?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.SendMessage("Laughter is the best medicine, they say. If you share a joke with me and make me laugh, I might have a special reward for you!");
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule())); // Return to greeting
                });

            greeting.AddOption("What is your reward?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.SendMessage("A promise is a promise! Here, take this. It's a trinket that has brought me luck in my travels. May it serve you well!");
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule())); // Return to greeting
                });

            return greeting;
        }

        public JesterJest(Serial serial) : base(serial) { }

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
}
