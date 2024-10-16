using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Merry the Mischievous")]
    public class MerryTheMischievous : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MerryTheMischievous() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Merry the Mischievous";
            Body = 0x190; // Human male body

            // Stats
            Str = 128;
            Dex = 100;
            Int = 100;
            Hits = 86;

            // Appearance
            AddItem(new JesterHat() { Hue = 54 });
            AddItem(new JesterSuit() { Hue = 1930 });
            AddItem(new Shoes() { Hue = 1915 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public MerryTheMischievous(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Merry the Mischievous, a jester by trade. What brings you to my realm of mirth and merriment?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player => 
                {
                    DialogueModule jobModule = new DialogueModule("Ah, my job! I spread laughter and joy wherever I go. But it's not just about the jokes—there's an art to it! Care to learn more?");
                    jobModule.AddOption("What do you mean by art?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule artModule = new DialogueModule("The art of jesting involves timing, delivery, and knowing your audience. Each joke is like a dance—perfectly choreographed to bring forth the greatest joy!");
                            artModule.AddOption("Can you give me an example?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule exampleModule = new DialogueModule("Of course! For instance, I might say: 'Why did the chicken cross the road? To prove to the possum that it could be done!' It’s simple, yet effective!");
                                    exampleModule.AddOption("That's clever! Do you have more?",
                                        plaa => true,
                                        plaa => 
                                        {
                                            plaa.SendGump(new DialogueGump(plaa, exampleModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, exampleModule));
                                });
                            artModule.AddOption("Sounds complicated.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, greeting)));
                            pl.SendGump(new DialogueGump(pl, artModule));
                        });
                    jobModule.AddOption("What about battles?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule battlesModule = new DialogueModule("Ah, but humor and merriment are the greatest battles of all! I face foes of gloom and despair daily. Do you appreciate a good jest?");
                            battlesModule.AddOption("Yes, I do!",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule jestModule = new DialogueModule("Ah, a connoisseur of humor! Tell me, what's your favorite jest?");
                                    pla.SendGump(new DialogueGump(pla, jestModule));
                                });
                            battlesModule.AddOption("Not really.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, greeting)));
                            pl.SendGump(new DialogueGump(pl, battlesModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Do you have any pranks?",
                player => true,
                player =>
                {
                    DialogueModule pranksModule = new DialogueModule("Oh, I have plenty! One of my favorites was when I replaced the blacksmith's iron with rubber. The look on his face was priceless!");
                    pranksModule.AddOption("What did he do?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule reactionModule = new DialogueModule("He was furious at first, but after a moment of confusion, he burst into laughter! That's the magic of a good prank!");
                            reactionModule.AddOption("That's hilarious! Any others?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule morePranksModule = new DialogueModule("Well, there was the time I filled the mayor's office with balloons. He couldn't see a thing and had to navigate through them to get to his desk!");
                                    morePranksModule.AddOption("What did he say?",
                                        plaa => true,
                                        plaa =>
                                        {
                                            plaa.SendGump(new DialogueGump(plaa, morePranksModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, morePranksModule));
                                });
                            pl.SendGump(new DialogueGump(pl, reactionModule));
                        });
                    player.SendGump(new DialogueGump(player, pranksModule));
                });

            greeting.AddOption("What can you tell me about laughter?",
                player => true,
                player =>
                {
                    DialogueModule laughterModule = new DialogueModule("Laughter is the best medicine, they say! It's the sound of joy that can brighten even the darkest days. If you bring me a joke that truly tickles me, I might reward you.");
                    laughterModule.AddOption("I have a joke!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule jokeModule = new DialogueModule("Oh, you have a joke for me? Let's hear it! If it truly amuses me, I'll give you something special.");
                            jokeModule.AddOption("Tell you the joke.",
                                pla => true,
                                pla =>
                                {
                                    // Placeholder for actual joke input
                                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                                    {
                                        pla.SendMessage("I have no reward right now. Please return later.");
                                    }
                                    else
                                    {
                                        pla.SendMessage("You've truly made my day! For that, I shall reward you. Check your pockets; there might be something extra there now.");
                                        pla.AddToBackpack(new CampingAugmentCrystal()); // Replace with actual reward item
                                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                    }
                                });
                            pl.SendGump(new DialogueGump(pl, jokeModule));
                        });
                    player.SendGump(new DialogueGump(player, laughterModule));
                });

            greeting.AddOption("What adventures have you had?",
                player => true,
                player =>
                {
                    DialogueModule adventureModule = new DialogueModule("Ah, my adventures! One time, I ended up in a faraway land, just because I was chasing a butterfly. It led me to a hidden treasure, guarded by a rather grumpy dragon!");
                    adventureModule.AddOption("What happened with the dragon?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dragonModule = new DialogueModule("Oh, I used my wit! I told the dragon that I was sent by the Royal Court to assess its treasure. It was so flattered that it let me take a small piece in exchange for a promise to return and tell tales of its grandeur!");
                            dragonModule.AddOption("That sounds dangerous!",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, dragonModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dragonModule));
                        });
                    adventureModule.AddOption("Tell me more about your travels.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule travelModule = new DialogueModule("I've traveled to many distant lands! I once visited a kingdom where the rivers flowed with chocolate. It was a delightful place, but a tad sticky!");
                            travelModule.AddOption("What did you do there?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule activitiesModule = new DialogueModule("I organized a chocolate-eating contest! The winner received a golden spoon. Everyone was covered in chocolate by the end, but their laughter echoed through the kingdom!");
                                    pla.SendGump(new DialogueGump(pla, activitiesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, travelModule));
                        });
                    player.SendGump(new DialogueGump(player, adventureModule));
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
}
