using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marina the Mapper")]
    public class MarinaTheMapper : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarinaTheMapper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marina the Mapper";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 50;
            Int = 120;
            Hits = 70;

            // Appearance
            AddItem(new Kilt() { Hue = 45 });
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new Shoes() { Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Marina the Mapper, keeper of maps and knowledge.");

            greeting.AddOption("How are you?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateHealthModule()))
            );

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My job is to chart the lands and discover their hidden treasures. I'm a cartographer by trade. Would you like to learn more about the art of cartography?");
                    jobModule.AddOption("Yes, tell me more about cartography.",
                        p => true,
                        p =>
                        {
                            DialogueModule cartographyModule = new DialogueModule("Cartography involves not only the drawing of maps but also understanding the geography, history, and culture of the lands. It's an art that requires keen observation.");
                            cartographyModule.AddOption("What skills do you need?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule skillsModule = new DialogueModule("A good cartographer needs observational skills, knowledge of navigation, and an understanding of the environment. Maps must reflect the reality of the land accurately.");
                                    skillsModule.AddOption("Sounds interesting! What tools do you use?",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule toolsModule = new DialogueModule("I use various tools like compasses, sextants, and modern writing instruments. In addition, I often rely on local lore to gain insight into less explored regions.");
                                            p1.SendGump(new DialogueGump(p1, toolsModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, skillsModule));
                                });
                            cartographyModule.AddOption("What have you discovered recently?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule discoveryModule = new DialogueModule("Recently, I've uncovered a hidden valley filled with ancient ruins. The stories these places could tell are truly fascinating! Would you like to hear more?");
                                    discoveryModule.AddOption("Yes, please share!",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule valleyStory = new DialogueModule("Legend has it that this valley was once home to a powerful civilization known for its wisdom and prosperity. Many artifacts remain, waiting for someone brave enough to seek them.");
                                            p1.SendGump(new DialogueGump(p1, valleyStory));
                                        });
                                    pl.SendGump(new DialogueGump(pl, discoveryModule));
                                });
                            p.SendGump(new DialogueGump(p, cartographyModule));
                        });

                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you think of justice?",
                player => true,
                player =>
                {
                    DialogueModule justiceModule = new DialogueModule("Justice is one of the eight virtues that shape our world. Each virtue is a beacon, guiding us through life's challenges. Why do you ask about justice?");
                    justiceModule.AddOption("I believe in justice too. It's important.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule importanceModule = new DialogueModule("Indeed! Justice ensures fairness and helps maintain harmony in society. It reminds us that our actions carry weight and impact others.");
                            importanceModule.AddOption("How do you uphold justice in your work?",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule upholdModule = new DialogueModule("As a cartographer, I strive to represent lands and people honestly. Each map is not just a tool, but a historical record that can influence future generations.");
                                    p1.SendGump(new DialogueGump(p1, upholdModule));
                                });
                            pl.SendGump(new DialogueGump(pl, importanceModule));
                        });

                    justiceModule.AddOption("What do you think about the other virtues?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule virtuesModule = new DialogueModule("The eight virtues—Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility—are essential for a balanced life. Each one plays a vital role.");
                            virtuesModule.AddOption("Which one do you value the most?",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule valueModule = new DialogueModule("I hold Honor in high regard. Without honor, one's word loses its weight. It's essential to remain true to one's principles, especially in challenging times.");
                                    p1.SendGump(new DialogueGump(p1, valueModule));
                                });
                            pl.SendGump(new DialogueGump(pl, virtuesModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, justiceModule));
                });

            greeting.AddOption("Tell me about your ancestors.",
                player => true,
                player =>
                {
                    DialogueModule ancestorsModule = new DialogueModule("My ancestors were explorers, always venturing into the unknown. Their legacy is the vast collection of maps I now safeguard. They faced many dangers in pursuit of knowledge.");
                    ancestorsModule.AddOption("What kind of dangers?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangerModule = new DialogueModule("They encountered wild beasts, treacherous terrains, and the wrath of nature itself. Yet, their determination to map the uncharted never waned. Do you enjoy adventure?");
                            dangerModule.AddOption("Absolutely! I love adventures.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule adventureModule = new DialogueModule("Ah, then you must seek out the unknown! There are countless mysteries waiting to be uncovered. Perhaps you would like to hear about one of my recent adventures?");
                                    adventureModule.AddOption("Yes, please!",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule storyModule = new DialogueModule("I once trekked through the Whispering Forest, a place shrouded in legends. They say spirits roam there, guarding ancient secrets. I managed to chart some of its trails despite the eerie atmosphere.");
                                            p2.SendGump(new DialogueGump(p2, storyModule));
                                        });
                                    p1.SendGump(new DialogueGump(p1, adventureModule));
                                });
                            pl.SendGump(new DialogueGump(pl, dangerModule));
                        });
                    player.SendGump(new DialogueGump(player, ancestorsModule));
                });

            greeting.AddOption("Do you have any secrets?",
                player => true,
                player =>
                {
                    if (CanReward())
                    {
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.SendGump(new DialogueGump(player, new DialogueModule("There are places in this world untouched by man, where treasures and mysteries await. Here is a secret location I can share with you.")));
                    }
                    else
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                });

            greeting.AddOption("What can you tell me about the world?",
                player => true,
                player =>
                {
                    DialogueModule worldModule = new DialogueModule("The world is vast and full of secrets. By maintaining my health, I hope to uncover as many of these secrets as I can in my lifetime. What interests you the most about the world?");
                    worldModule.AddOption("The cultures and people.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cultureModule = new DialogueModule("Every culture has its own stories, traditions, and wisdom. It's fascinating to see how they shape the land and its maps. I often collaborate with locals to ensure my maps reflect their heritage.");
                            cultureModule.AddOption("Have you learned any local stories?",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule storiesModule = new DialogueModule("Many! One that stands out is the tale of a lost city rumored to contain ancient knowledge. Locals speak of a guardian spirit that protects it. I intend to find it someday!");
                                    p1.SendGump(new DialogueGump(p1, storiesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, cultureModule));
                        });
                    worldModule.AddOption("The geography and landscapes.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule geographyModule = new DialogueModule("The varied landscapes—from mountains to valleys—offer countless opportunities for exploration. Each region has its own unique features that can be challenging to map.");
                            geographyModule.AddOption("What is the most challenging place you've mapped?",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule challengeModule = new DialogueModule("The Frozen Wastes are treacherous! With shifting ice and sudden storms, it requires not just skill but also a bit of luck. I barely made it back with my notes intact!");
                                    p1.SendGump(new DialogueGump(p1, challengeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, geographyModule));
                        });
                    player.SendGump(new DialogueGump(player, worldModule));
                });

            return greeting;
        }

        private DialogueModule CreateHealthModule()
        {
            return new DialogueModule("I am in good health, as always. The world is my map, and I must stay fit to explore it.");
        }

        private bool CanReward()
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            return DateTime.UtcNow - lastRewardTime >= cooldown;
        }

        public MarinaTheMapper(Serial serial) : base(serial) { }

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
