using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lorenzo the Lorekeeper")]
    public class LorenzoTheLorekeeper : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LorenzoTheLorekeeper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lorenzo the Lorekeeper";
            Body = 0x190; // Human male body
            SetStr(80);
            SetDex(60);
            SetInt(100);
            SetHits(60);

            AddItem(new LongPants() { Hue = 1109 });
            AddItem(new FancyShirt() { Hue = 1109 });
            AddItem(new Cloak() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Lute() { Name = "Lorenzo's Lute" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue; // Initialize last reward time
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lorenzo the Lorekeeper, a bard and collector of tales. How may I assist you today?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player => {
                    DialogueModule jobModule = new DialogueModule("As a lorekeeper, I gather stories from across the land, sharing wisdom and knowledge. Stories have the power to inspire and teach. Would you like to hear a tale?");
                    jobModule.AddOption("Yes, tell me a tale.",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, CreateStoryModule()));
                        });
                    jobModule.AddOption("No, what else do you do?",
                        p => true,
                        p => {
                            DialogueModule moreInfoModule = new DialogueModule("I also collect rare artifacts and seek knowledge of ancient magic. Each item tells a story of its own. Have you encountered any magical artifacts?");
                            moreInfoModule.AddOption("I found something mysterious.",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah! Every artifact has a history. Share what you found, and I may offer insights or rewards.")));
                                });
                            moreInfoModule.AddOption("Not yet.",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, moreInfoModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you think about battles?",
                player => true,
                player => {
                    DialogueModule battleModule = new DialogueModule("True valor lies not in the strength of one's arm, but in the strength of one's character. Do you possess valor?");
                    battleModule.AddOption("Yes.",
                        p => true,
                        p => {
                            DialogueModule valorModule = new DialogueModule("Then you understand that courage is not the absence of fear, but the ability to act despite it. Have you ever been in a battle?");
                            valorModule.AddOption("Yes, I fought bravely.",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Bravery is commendable! Every warrior's tale adds to the tapestry of history. Would you share your story with me?")));
                                });
                            valorModule.AddOption("No, I prefer peace.",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Peace is a treasure of its own. Sometimes, wisdom comes from avoiding conflict. Would you like to learn more about peaceful resolutions?")));
                                });
                            p.SendGump(new DialogueGump(p, valorModule));
                        });
                    battleModule.AddOption("No, I'm still learning.",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, new DialogueModule("That's understandable. Every journey begins with a single step. Perhaps you’d like to learn the ways of wisdom instead?")));
                        });
                    player.SendGump(new DialogueGump(player, battleModule));
                });

            greeting.AddOption("Tell me about the Lost City.",
                player => true,
                player => {
                    DialogueModule cityModule = new DialogueModule("The Lost City is a legend, said to be hidden deep within the mountains. Many have sought its treasures, but few have returned.");
                    cityModule.AddOption("What treasures does it hold?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("It’s rumored to contain ancient artifacts and knowledge long forgotten. Those who seek it must be prepared for trials. Would you dare to seek it?")));
                        });
                    cityModule.AddOption("How can I prepare for such a journey?",
                        pl => true,
                        pl => {
                            DialogueModule prepareModule = new DialogueModule("Preparation is key. Gather supplies, sharpen your skills, and seek allies. Stories tell of the trials faced by those who ventured there. Would you like to hear about them?");
                            prepareModule.AddOption("Yes, tell me about the trials.",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("There are tests of courage, wisdom, and strength. Each trial reveals the true nature of the seeker. Are you ready to face your own challenges?")));
                                });
                            prepareModule.AddOption("No, I’ll seek knowledge elsewhere.",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, prepareModule));
                        });
                    player.SendGump(new DialogueGump(player, cityModule));
                });

            greeting.AddOption("What can you tell me about the Enchanted Lyre?",
                player => true,
                player => {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        DialogueModule lyreModule = new DialogueModule("The Enchanted Lyre is said to have the power to soothe even the fiercest of beasts. I've been searching for it to add its story to my collection.");
                        lyreModule.AddOption("What would you give me for it?",
                            pl => true,
                            pl => {
                                DialogueModule rewardModule = new DialogueModule("If you bring it to me, I will reward you with knowledge that few possess. The stories it can tell are worth more than gold. Do you accept this quest?");
                                rewardModule.AddOption("Yes, I will find the Lyre.",
                                    p => {
                                        lastRewardTime = DateTime.UtcNow; // Update reward time
                                        p.SendMessage("Lorenzo nods appreciatively.");
                                        return true;
                                    },
                                    p => {
                                        p.SendGump(new DialogueGump(p, new DialogueModule("Thank you! I eagerly await your return.")));
                                    });
                                rewardModule.AddOption("No, I will seek other quests.",
                                    p => true,
                                    p => {
                                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                    });
                                pl.SendGump(new DialogueGump(pl, rewardModule));
                            });
                        player.SendGump(new DialogueGump(player, lyreModule));
                    }
                });

            greeting.AddOption("What do you know about the Whispering Forest?",
                player => true,
                player => {
                    DialogueModule forestModule = new DialogueModule("The Whispering Forest is a place of magic and mystery. The trees themselves are said to whisper ancient tales. Have you visited it?");
                    forestModule.AddOption("Yes, I have.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("What secrets did you uncover? The forest is known for its many hidden wonders.")));
                        });
                    forestModule.AddOption("No, but I wish to go.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I recommend you go prepared. The whispers can guide you, but they can also lead you astray. Would you like advice on how to navigate the forest?")));
                        });
                    player.SendGump(new DialogueGump(player, forestModule));
                });

            return greeting;
        }

        private DialogueModule CreateStoryModule()
        {
            DialogueModule storyModule = new DialogueModule("Stories have the power to inspire and teach. One of my cherished tales is about the Whispering Forest. Are you interested?");
            storyModule.AddOption("Yes, tell me more.",
                player => true,
                player => {
                    DialogueModule whisperStoryModule = new DialogueModule("Long ago, a brave adventurer entered the Whispering Forest in search of lost knowledge. The whispers guided him, revealing hidden paths and ancient wisdom. But, he soon realized that the forest also tested his resolve. Would you like to know how he overcame the trials?");
                    whisperStoryModule.AddOption("Yes, how did he do it?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("He listened closely to the whispers, deciphering their meanings. With each challenge, he learned more about himself and the world around him. Eventually, he emerged with not only stories but a deeper understanding of life. What do you seek in your own journey?")));
                        });
                    whisperStoryModule.AddOption("No, I want to hear a different story.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well. The world is full of tales. One of my favorites is about a legendary dragon that guarded a treasure of knowledge. Would you like to hear it?")));
                        });
                    player.SendGump(new DialogueGump(player, whisperStoryModule));
                });
            storyModule.AddOption("No, maybe another time.",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });
            return storyModule;
        }

        public LorenzoTheLorekeeper(Serial serial) : base(serial) { }

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
